using System.Data;
using System.Globalization;
using System.Net.Sockets;
using EMS.EomsCategorySyncWorker.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace EMS.EomsCategorySyncWorker.Repositories;

public record SyncBatchResult(
    int InsertedCount,
    int UpdatedCount,
    int UnchangedCount,
    int FailCount)
{
    public int SuccessCount => InsertedCount + UpdatedCount;
}

public class EomsCategoryRepository
{
    private const int MaxMongoErrorLogsPerBatch = 50;
    private const int MaxOracleRetries = 4;

    private readonly string _connectionString;
    private readonly int _commandTimeoutSeconds;
    private readonly EomsCategoryMongoRepository _mongoRepo;
    private readonly ILogger<EomsCategoryRepository> _logger;

    public EomsCategoryRepository(
        IConfiguration configuration,
        EomsCategoryMongoRepository mongoRepo,
        ILogger<EomsCategoryRepository> logger)
    {
        _mongoRepo = mongoRepo;
        _logger = logger;

        var cs = configuration.GetConnectionString("EMSONE") ?? configuration["ConnectionStrings:EMSONE"];
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Connection string 'EMSONE' is not configured (ConnectionStrings:EMSONE).");

        var connectTimeout = configuration.GetValue("Worker:OracleConnectionTimeoutSeconds", 30);
        if (connectTimeout < 5)
            connectTimeout = 30;

        _connectionString = BuildResilientOracleConnectionString(cs.Trim(), connectTimeout);

        var cmdTimeout = configuration.GetValue("Worker:OracleCommandTimeoutSeconds", 180);
        _commandTimeoutSeconds = cmdTimeout < 5 ? 180 : cmdTimeout;
    }

    public async Task<Dictionary<string, string?>> GetExistingRawJsonMapAsync(
        string tableName,
        CancellationToken ct)
    {
        if (!string.Equals(tableName, "EOMS_PROVINCES", StringComparison.Ordinal)
            && !string.Equals(tableName, "EOMS_COMMUNES", StringComparison.Ordinal)
            && !string.Equals(tableName, "EOMS_PRODUCT", StringComparison.Ordinal))
        {
            throw new ArgumentException($"Bảng danh mục không hợp lệ: {tableName}", nameof(tableName));
        }

        for (var attempt = 1; ; attempt++)
        {
            ct.ThrowIfCancellationRequested();
            OracleConnection? conn = null;
            try
            {
                var result = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

                conn = new OracleConnection(_connectionString);
                await conn.OpenAsync(ct);

                await using var cmd = conn.CreateCommand();
                cmd.CommandTimeout = _commandTimeoutSeconds;
                // Dùng ngưỡng 8KB và FetchSize 64KB để tránh gói tin TNS quá lớn bị drop khi mạng/VPN rớt gói
                cmd.InitialLOBFetchSize = 8192;
                cmd.FetchSize = 65536;
                cmd.CommandText = $"SELECT CODE, RAW_JSON FROM {tableName} WHERE CODE IS NOT NULL";

                await using var reader = await cmd.ExecuteReaderAsync(ct);
                while (await reader.ReadAsync(ct))
                {
                    var code = reader.IsDBNull(0) ? null : reader.GetString(0);
                    if (string.IsNullOrWhiteSpace(code))
                        continue;

                    var rawJson = reader.IsDBNull(1) ? null : reader.GetString(1);
                    result[code] = rawJson;
                }

                return result;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex) when (attempt < MaxOracleRetries && IsTransientOracleError(ex))
            {
                var delay = TimeSpan.FromSeconds(Math.Min(15, attempt * 3));
                _logger.LogWarning(
                    ex,
                    "Lỗi mạng tạm thời khi đọc danh mục hiện có từ bảng {TableName} (lần {Attempt}/{MaxRetries}). Sẽ dọn pool và thử lại sau {DelaySeconds}s...",
                    tableName, attempt, MaxOracleRetries, delay.TotalSeconds);

                await DisposeAndClearPoolSafelyAsync(conn);
                conn = null;

                await Task.Delay(delay, ct);
            }
            finally
            {
                if (conn != null)
                    await conn.DisposeAsync();
            }
        }
    }

    public Task<SyncBatchResult> UpsertProvincesAsync(
        IReadOnlyList<EomsAreaItem> items,
        CancellationToken ct,
        string syncId = "",
        string trigger = "",
        IDictionary<string, string?>? existingMap = null) =>
        UpsertAreasAsync("PKG_EOMS_CATEGORY.UPSERT_PROVINCES", "Provinces", "EOMS_PROVINCES", items, ct, syncId, trigger, existingMap);

    public Task<SyncBatchResult> UpsertCommunesAsync(
        IReadOnlyList<EomsAreaItem> items,
        CancellationToken ct,
        string syncId = "",
        string trigger = "",
        IDictionary<string, string?>? existingMap = null) =>
        UpsertAreasAsync("PKG_EOMS_CATEGORY.UPSERT_COMMUNES", "Communes", "EOMS_COMMUNES", items, ct, syncId, trigger, existingMap);

    private async Task<SyncBatchResult> UpsertAreasAsync(
        string procedureName,
        string label,
        string categoryName,
        IReadOnlyList<EomsAreaItem> items,
        CancellationToken ct,
        string syncId,
        string trigger,
        IDictionary<string, string?>? existingMap)
    {
        if (items.Count == 0)
            return new SyncBatchResult(0, 0, 0, 0);

        existingMap ??= await GetExistingRawJsonMapAsync(categoryName, ct);

        var inserted = 0;
        var updated = 0;
        var unchanged = 0;
        var fail = 0;
        var loggedErrorCount = 0;

        OracleConnection? conn = null;
        try
        {
            foreach (var item in items)
            {
                ct.ThrowIfCancellationRequested();

                string? existingRawJson = null;
                var exists = !string.IsNullOrEmpty(item.Code) && existingMap.TryGetValue(item.Code, out existingRawJson);
                if (exists && IsRawJsonEqual(existingRawJson, item.RawJson))
                {
                    unchanged++;
                    continue;
                }

                for (var attempt = 1; ; attempt++)
                {
                    try
                    {
                        if (conn == null || conn.State != ConnectionState.Open)
                        {
                            if (conn != null)
                                await conn.DisposeAsync();
                            conn = new OracleConnection(_connectionString);
                            await conn.OpenAsync(ct);
                        }

                        await using var cmd = conn.CreateCommand();
                        cmd.CommandTimeout = _commandTimeoutSeconds;
                        cmd.CommandText = procedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName = true;

                        cmd.Parameters.Add("P_CODE", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.Code ?? DBNull.Value;
                        cmd.Parameters.Add("P_NAME", OracleDbType.NVarchar2, ParameterDirection.Input).Value = (object?)item.Name ?? DBNull.Value;
                        cmd.Parameters.Add("P_LEVEL_CODE", OracleDbType.Int32, ParameterDirection.Input).Value = item.Level.HasValue ? item.Level.Value : DBNull.Value;
                        cmd.Parameters.Add("P_PARENT_CODE", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.ParentCode ?? DBNull.Value;
                        cmd.Parameters.Add("P_TAG", OracleDbType.NVarchar2, ParameterDirection.Input).Value = (object?)item.Tag ?? DBNull.Value;
                        cmd.Parameters.Add("P_POST_CODE", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.PostCode ?? DBNull.Value;
                        cmd.Parameters.Add("P_RAW_JSON", OracleDbType.Clob, ParameterDirection.Input).Value = string.IsNullOrEmpty(item.RawJson) ? DBNull.Value : item.RawJson;

                        var pCode = new OracleParameter("P_RESULT_CODE", OracleDbType.Int32, ParameterDirection.Output);
                        var pMsg = new OracleParameter("P_RESULT_MESSAGE", OracleDbType.Varchar2, 4000) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(pCode);
                        cmd.Parameters.Add(pMsg);

                        await cmd.ExecuteNonQueryAsync(ct);

                        var resultCode = GetOracleInt32Output(pCode);
                        if (resultCode == 1 || resultCode == 0)
                        {
                            if (exists)
                                updated++;
                            else
                                inserted++;

                            if (!string.IsNullOrEmpty(item.Code))
                                existingMap[item.Code] = item.RawJson;
                        }
                        else
                        {
                            fail++;
                            var errMsg = $"{label} upsert trả về lỗi cho CODE={item.Code}: ResultCode={resultCode}, Message={pMsg.Value}";
                            _logger.LogWarning(
                                "{Label} upsert trả về lỗi cho CODE={Code}: ResultCode={ResultCode}, Message={Message}",
                                label, item.Code, resultCode, pMsg.Value?.ToString());
                            TryLogDbUpsertError(ref loggedErrorCount, syncId, trigger, categoryName, item.Code, 1, errMsg, null, item.RawJson);
                        }

                        break; // Thành công hoặc lỗi nghiệp vụ từ SP -> thoát vòng lặp retry
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (OracleException oex) when (oex.Number == 6550)
                    {
                        var abortedCount = items.Count - (inserted + updated + unchanged + fail);
                        fail += abortedCount;
                        var dataSource = conn?.DataSource ?? string.Empty;
                        var errMsg = $"{label} upsert dừng sớm do lỗi PL/SQL (ORA-06550) tại CODE={item.Code} trên DataSource={dataSource}.";
                        _logger.LogError(
                            oex,
                            "{Label} upsert dừng sớm do lỗi PL/SQL (ORA-06550) tại CODE={Code} trên DataSource={DataSource}. Kiểm tra lại ConnectionStrings:EMSONE và package PKG_EOMS_CATEGORY.",
                            label, item.Code, dataSource);
                        TryLogDbUpsertError(ref loggedErrorCount, syncId, trigger, categoryName, item.Code, abortedCount, errMsg, oex.ToString(), item.RawJson);
                        return new SyncBatchResult(inserted, updated, unchanged, fail);
                    }
                    catch (Exception ex) when (attempt < MaxOracleRetries && IsTransientOracleError(ex))
                    {
                        var delay = TimeSpan.FromSeconds(Math.Min(12, attempt * 2));
                        _logger.LogWarning(
                            ex,
                            "{Label} upsert gặp lỗi mạng tạm thời cho CODE={Code} (lần {Attempt}/{MaxRetries}). Đang mở lại kết nối sau {DelaySeconds}s...",
                            label, item.Code, attempt, MaxOracleRetries, delay.TotalSeconds);

                        await DisposeAndClearPoolSafelyAsync(conn);
                        conn = null;

                        await Task.Delay(delay, ct);
                    }
                    catch (Exception ex)
                    {
                        if (IsTransientOracleError(ex))
                        {
                            await DisposeAndClearPoolSafelyAsync(conn);
                            conn = null;
                        }

                        fail++;
                        _logger.LogError(ex, "{Label} upsert exception cho CODE={Code}", label, item.Code);
                        TryLogDbUpsertError(ref loggedErrorCount, syncId, trigger, categoryName, item.Code, 1, ex.Message, ex.ToString(), item.RawJson);
                        break;
                    }
                }
            }
        }
        finally
        {
            if (conn != null)
                await conn.DisposeAsync();
        }

        return new SyncBatchResult(inserted, updated, unchanged, fail);
    }

    public async Task<SyncBatchResult> UpsertProductsAsync(
        IReadOnlyList<EomsProductItem> items,
        CancellationToken ct,
        string syncId = "",
        string trigger = "",
        IDictionary<string, string?>? existingMap = null)
    {
        if (items.Count == 0)
            return new SyncBatchResult(0, 0, 0, 0);

        existingMap ??= await GetExistingRawJsonMapAsync("EOMS_PRODUCT", ct);

        var inserted = 0;
        var updated = 0;
        var unchanged = 0;
        var fail = 0;
        var loggedErrorCount = 0;

        OracleConnection? conn = null;
        try
        {
            foreach (var item in items)
            {
                ct.ThrowIfCancellationRequested();

                string? existingRawJson = null;
                var exists = !string.IsNullOrEmpty(item.Code) && existingMap.TryGetValue(item.Code, out existingRawJson);
                if (exists && IsRawJsonEqual(existingRawJson, item.RawJson))
                {
                    unchanged++;
                    continue;
                }

                for (var attempt = 1; ; attempt++)
                {
                    try
                    {
                        if (conn == null || conn.State != ConnectionState.Open)
                        {
                            if (conn != null)
                                await conn.DisposeAsync();
                            conn = new OracleConnection(_connectionString);
                            await conn.OpenAsync(ct);
                        }

                        await using var cmd = conn.CreateCommand();
                        cmd.CommandTimeout = _commandTimeoutSeconds;
                        cmd.CommandText = "PKG_EOMS_CATEGORY.UPSERT_PRODUCT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName = true;

                        cmd.Parameters.Add("P_CODE", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.Code ?? DBNull.Value;
                        cmd.Parameters.Add("P_NAME", OracleDbType.NVarchar2, ParameterDirection.Input).Value = (object?)item.Name ?? DBNull.Value;
                        cmd.Parameters.Add("P_DESCRIPTION", OracleDbType.NVarchar2, ParameterDirection.Input).Value = (object?)item.Description ?? DBNull.Value;
                        cmd.Parameters.Add("P_BUSINESS_TYPE", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.BusinessType ?? DBNull.Value;
                        cmd.Parameters.Add("P_BUSINESS_TYPE_NAME", OracleDbType.NVarchar2, ParameterDirection.Input).Value = (object?)item.BusinessTypeName ?? DBNull.Value;
                        cmd.Parameters.Add("P_TIME_LEVEL", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.TimeLevel ?? DBNull.Value;
                        cmd.Parameters.Add("P_TIME_LEVEL_NAME", OracleDbType.NVarchar2, ParameterDirection.Input).Value = (object?)item.TimeLevelName ?? DBNull.Value;
                        cmd.Parameters.Add("P_OPERATION_METHOD", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.OperationMethod ?? DBNull.Value;
                        cmd.Parameters.Add("P_OPERATION_METHOD_NAME", OracleDbType.NVarchar2, ParameterDirection.Input).Value = (object?)item.OperationMethodName ?? DBNull.Value;
                        cmd.Parameters.Add("P_SUPPLIERS", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.Suppliers ?? DBNull.Value;
                        cmd.Parameters.Add("P_SUPPLIERS_NAME", OracleDbType.NVarchar2, ParameterDirection.Input).Value = (object?)item.SuppliersName ?? DBNull.Value;
                        cmd.Parameters.Add("P_LONG_TERM_FLAG", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.LongTermFlag ?? DBNull.Value;
                        cmd.Parameters.Add("P_VALID_TIME_START", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.ValidTimeStart ?? DBNull.Value;
                        cmd.Parameters.Add("P_VALID_TIME_END", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.ValidTimeEnd ?? DBNull.Value;
                        cmd.Parameters.Add("P_SPLIT_FLAG", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.SplitFlag ?? DBNull.Value;
                        cmd.Parameters.Add("P_MAX_SIZE_STR", OracleDbType.Clob, ParameterDirection.Input).Value = string.IsNullOrEmpty(item.MaxSizeStr) ? DBNull.Value : item.MaxSizeStr;
                        cmd.Parameters.Add("P_MAX_WEIGHT", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.MaxWeight ?? DBNull.Value;
                        cmd.Parameters.Add("P_MAX_VOLUME", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.MaxVolume ?? DBNull.Value;
                        cmd.Parameters.Add("P_VOLUME_WEIGHT_RATIO", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.VolumeWeightRatio ?? DBNull.Value;
                        cmd.Parameters.Add("P_DELIVERY_METHOD", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.DeliveryMethod ?? DBNull.Value;
                        cmd.Parameters.Add("P_COVERAGE", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.Coverage ?? DBNull.Value;
                        cmd.Parameters.Add("P_SALE_CHANNEL", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.SaleChannel ?? DBNull.Value;
                        cmd.Parameters.Add("P_LIMIT_GOODS_ATTR", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.LimitGoodsAttr ?? DBNull.Value;
                        cmd.Parameters.Add("P_TRANSPORT_MODE", OracleDbType.Varchar2, ParameterDirection.Input).Value = (object?)item.TransportMode ?? DBNull.Value;
                        cmd.Parameters.Add("P_BASIC_SERVICE", OracleDbType.Clob, ParameterDirection.Input).Value = SerializeJTokenOrNull(item.BasicService);
                        cmd.Parameters.Add("P_VALUE_ADDED_SERVICE", OracleDbType.Clob, ParameterDirection.Input).Value = SerializeJTokenOrNull(item.ValueAddedService);
                        cmd.Parameters.Add("P_RAW_JSON", OracleDbType.Clob, ParameterDirection.Input).Value = string.IsNullOrEmpty(item.RawJson) ? DBNull.Value : item.RawJson;

                        var pCode = new OracleParameter("P_RESULT_CODE", OracleDbType.Int32, ParameterDirection.Output);
                        var pMsg = new OracleParameter("P_RESULT_MESSAGE", OracleDbType.Varchar2, 4000) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(pCode);
                        cmd.Parameters.Add(pMsg);

                        await cmd.ExecuteNonQueryAsync(ct);

                        var resultCode = GetOracleInt32Output(pCode);
                        if (resultCode == 1 || resultCode == 0)
                        {
                            if (exists)
                                updated++;
                            else
                                inserted++;

                            if (!string.IsNullOrEmpty(item.Code))
                                existingMap[item.Code] = item.RawJson;
                        }
                        else
                        {
                            fail++;
                            var errMsg = $"Product upsert trả về lỗi cho CODE={item.Code}: ResultCode={resultCode}, Message={pMsg.Value}";
                            _logger.LogWarning(
                                "Product upsert trả về lỗi cho CODE={Code}: ResultCode={ResultCode}, Message={Message}",
                                item.Code, resultCode, pMsg.Value?.ToString());
                            TryLogDbUpsertError(ref loggedErrorCount, syncId, trigger, "EOMS_PRODUCT", item.Code, 1, errMsg, null, item.RawJson);
                        }

                        break;
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (OracleException oex) when (oex.Number == 6550)
                    {
                        var abortedCount = items.Count - (inserted + updated + unchanged + fail);
                        fail += abortedCount;
                        var dataSource = conn?.DataSource ?? string.Empty;
                        var errMsg = $"Product upsert dừng sớm do lỗi PL/SQL (ORA-06550) tại CODE={item.Code} trên DataSource={dataSource}.";
                        _logger.LogError(
                            oex,
                            "{Label} upsert dừng sớm do lỗi PL/SQL (ORA-06550) tại CODE={Code} trên DataSource={DataSource}. Kiểm tra lại ConnectionStrings:EMSONE và package PKG_EOMS_CATEGORY.",
                            "Product", item.Code, dataSource);
                        TryLogDbUpsertError(ref loggedErrorCount, syncId, trigger, "EOMS_PRODUCT", item.Code, abortedCount, errMsg, oex.ToString(), item.RawJson);
                        return new SyncBatchResult(inserted, updated, unchanged, fail);
                    }
                    catch (Exception ex) when (attempt < MaxOracleRetries && IsTransientOracleError(ex))
                    {
                        var delay = TimeSpan.FromSeconds(Math.Min(12, attempt * 2));
                        _logger.LogWarning(
                            ex,
                            "Product upsert gặp lỗi mạng tạm thời cho CODE={Code} (lần {Attempt}/{MaxRetries}). Đang mở lại kết nối sau {DelaySeconds}s...",
                            item.Code, attempt, MaxOracleRetries, delay.TotalSeconds);

                        await DisposeAndClearPoolSafelyAsync(conn);
                        conn = null;

                        await Task.Delay(delay, ct);
                    }
                    catch (Exception ex)
                    {
                        if (IsTransientOracleError(ex))
                        {
                            await DisposeAndClearPoolSafelyAsync(conn);
                            conn = null;
                        }

                        fail++;
                        _logger.LogError(ex, "Product upsert exception cho CODE={Code}", item.Code);
                        TryLogDbUpsertError(ref loggedErrorCount, syncId, trigger, "EOMS_PRODUCT", item.Code, 1, ex.Message, ex.ToString(), item.RawJson);
                        break;
                    }
                }
            }
        }
        finally
        {
            if (conn != null)
                await conn.DisposeAsync();
        }

        return new SyncBatchResult(inserted, updated, unchanged, fail);
    }

    private static bool IsTransientOracleError(Exception ex)
    {
        for (var curr = ex; curr != null; curr = curr.InnerException)
        {
            if (curr is SocketException or TimeoutException)
                return true;

            if (curr is OracleException oex)
            {
                // Các mã lỗi mạng / kết nối đứt giữa chừng phổ biến của Oracle:
                // 3113: end-of-file on communication channel
                // 3114: not connected to ORACLE
                // 3135: connection lost contact
                // 12154 / 12170: TNS:Connect timeout occurred
                // 12537: TNS:connection closed
                // 12541 / 12543: TNS:no listener / destination host unreachable
                // 12570: TNS:packet reader failure
                // 12571: TNS:packet writer failure
                // 50000: Connection request timed out
                if (oex.Number is 3113 or 3114 or 3135 or 12154 or 12170 or 12537 or 12541 or 12543 or 12570 or 12571 or 50000)
                    return true;
            }
        }

        return false;
    }

    private static async Task DisposeAndClearPoolSafelyAsync(OracleConnection? conn)
    {
        if (conn == null)
            return;

        try
        {
            OracleConnection.ClearPool(conn);
        }
        catch
        {
            // Bỏ qua lỗi khi clear pool trên kết nối đã đứt
        }

        try
        {
            await conn.DisposeAsync();
        }
        catch
        {
            // Bỏ qua lỗi khi đóng kết nối đã hỏng
        }
    }

    private static bool IsRawJsonEqual(string? existingJson, string? newJson)
    {
        if (string.IsNullOrWhiteSpace(existingJson) || string.IsNullOrWhiteSpace(newJson))
            return false;

        if (string.Equals(existingJson.Trim(), newJson.Trim(), StringComparison.Ordinal))
            return true;

        try
        {
            var existingToken = JToken.Parse(existingJson);
            var newToken = JToken.Parse(newJson);
            return JToken.DeepEquals(existingToken, newToken);
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private void TryLogDbUpsertError(
        ref int loggedErrorCount,
        string syncId,
        string trigger,
        string category,
        string? itemCode,
        int failCount,
        string message,
        string? errorDetail,
        string? rawJson)
    {
        if (loggedErrorCount >= MaxMongoErrorLogsPerBatch)
            return;

        loggedErrorCount++;
        _mongoRepo.InsertLog(new EomsCategorySyncLog
        {
            SyncId = syncId,
            Trigger = trigger,
            Category = category,
            Action = "DB_UPSERT",
            ItemCode = itemCode,
            Success = false,
            UpsertFailCount = failCount,
            Message = message,
            ErrorDetail = errorDetail,
            Request = rawJson
        });
    }

    private static object SerializeJTokenOrNull(JToken? token)
    {
        if (token == null || token.Type == JTokenType.Null)
            return DBNull.Value;

        if (token.Type == JTokenType.String)
        {
            var s = token.Value<string>();
            return string.IsNullOrEmpty(s) ? DBNull.Value : s;
        }

        return token.ToString(Formatting.None);
    }

    private static int GetOracleInt32Output(OracleParameter p)
    {
        var v = p.Value;
        if (v is null or DBNull)
            return 0;
        if (v is OracleDecimal od)
            return od.IsNull ? 0 : od.ToInt32();
        if (v is int i)
            return i;
        if (v is long l)
            return checked((int)l);
        if (v is decimal d)
            return (int)d;
        return Convert.ToInt32(Convert.ToDecimal(v, CultureInfo.InvariantCulture));
    }

    private static string BuildResilientOracleConnectionString(string connectionString, int connectTimeoutSeconds)
    {
        var result = connectionString.Trim();

        if (!result.Contains("Connection Timeout", StringComparison.OrdinalIgnoreCase)
            && !result.Contains("CONNECT_TIMEOUT", StringComparison.OrdinalIgnoreCase)
            && !result.Contains("Connect Timeout", StringComparison.OrdinalIgnoreCase))
        {
            var sep = result.EndsWith(';') ? "" : ";";
            result = $"{result}{sep}Connection Timeout={connectTimeoutSeconds}";
        }

        if (!result.Contains("Validate Connection", StringComparison.OrdinalIgnoreCase))
        {
            var sep = result.EndsWith(';') ? "" : ";";
            result = $"{result}{sep}Validate Connection=true";
        }

        return result;
    }
}
