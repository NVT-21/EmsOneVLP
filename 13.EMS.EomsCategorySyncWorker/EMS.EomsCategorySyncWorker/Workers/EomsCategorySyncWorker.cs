using System.Diagnostics;
using System.Globalization;
using EMS.EomsCategorySyncWorker.Models;
using EMS.EomsCategorySyncWorker.Repositories;
using EMS.EomsCategorySyncWorker.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EMS.EomsCategorySyncWorker.Workers;

public class EomsCategorySyncWorker : BackgroundService
{
    private readonly EomsCategoryClient _client;
    private readonly EomsCategoryRepository _repository;
    private readonly EomsCategoryMongoRepository _mongoRepo;
    private readonly IOptionsMonitor<EomsCategoryOptions> _optionsMonitor;
    private readonly ILogger<EomsCategorySyncWorker> _logger;
    private readonly SemaphoreSlim _syncLock = new(1, 1);

    public EomsCategorySyncWorker(
        EomsCategoryClient client,
        EomsCategoryRepository repository,
        EomsCategoryMongoRepository mongoRepo,
        IOptionsMonitor<EomsCategoryOptions> optionsMonitor,
        ILogger<EomsCategorySyncWorker> logger)
    {
        _client = client;
        _repository = repository;
        _mongoRepo = mongoRepo;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var opts = _optionsMonitor.CurrentValue;
        if (!opts.Enabled)
        {
            _logger.LogInformation("EomsCategorySyncWorker đang tắt (EomsCategory:Enabled=false).");
            return;
        }

        _logger.LogInformation(
            "EomsCategorySyncWorker đã khởi động. Lịch chạy={RunAt} ({TimeZone}), RunOnStartup={RunOnStartup}, BaseUrl={BaseUrl}",
            opts.RunAtLocalTime, opts.TimeZoneId, opts.RunOnStartup, opts.BaseUrl);

        if (opts.RunOnStartup)
        {
            await RunSyncSafelyAsync("startup", stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var currentOpts = _optionsMonitor.CurrentValue;
                if (!currentOpts.Enabled)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    continue;
                }

                var delay = CalculateDelayUntilNextRun(currentOpts.RunAtLocalTime, currentOpts.TimeZoneId, out var nextLocal);
                _logger.LogInformation(
                    "EomsCategorySyncWorker chờ đến lần đồng bộ kế tiếp: {NextRun} ({TimeZone}, sau {Delay}).",
                    nextLocal.ToString("yyyy-MM-dd HH:mm:ss"), currentOpts.TimeZoneId, delay);

                await Task.Delay(delay, stoppingToken);

                if (!_optionsMonitor.CurrentValue.Enabled)
                    continue;

                await RunSyncSafelyAsync("scheduled", stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi vòng lặp lập lịch EomsCategorySyncWorker.");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

    public async Task RunSyncSafelyAsync(string trigger, CancellationToken ct)
    {
        if (!await _syncLock.WaitAsync(0, ct))
        {
            _logger.LogWarning("Bỏ qua lần đồng bộ ({Trigger}) vì phiên trước vẫn đang chạy.", trigger);
            return;
        }

        try
        {
            await SyncAllAsync(trigger, ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            _logger.LogInformation("Đồng bộ danh mục EOMS ({Trigger}) đã bị hủy do shutdown.", trigger);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không mong đợi khi đồng bộ danh mục EOMS ({Trigger}).", trigger);
        }
        finally
        {
            _syncLock.Release();
        }
    }

    private async Task SyncAllAsync(string trigger, CancellationToken ct)
    {
        var syncId = Guid.NewGuid().ToString("N");
        var sw = Stopwatch.StartNew();
        _logger.LogInformation("Bắt đầu đồng bộ danh mục EOMS ({Trigger}), SyncId={SyncId}...", trigger, syncId);

        var opts = _optionsMonitor.CurrentValue;
        List<EomsAreaItem> provinces = new();
        int provIns = 0, provUpd = 0, provUnch = 0, provFail = 0;
        Exception? provException = null;

        int commIns = 0, commUpd = 0, commUnch = 0, commFail = 0, commProvFail = 0, commFetchedTotal = 0;
        var commErrors = new List<string>();
        Exception? commException = null;

        int prodIns = 0, prodUpd = 0, prodUnch = 0, prodFail = 0, totalFetched = 0;
        Exception? prodException = null;

        // 1. Đồng bộ Tỉnh/Thành (level=2, parentCode=VN)
        var provSw = Stopwatch.StartNew();
        try
        {
            provinces = await _client.GetAreasAsync("2", "VN", ct);
            var provResult = await _repository.UpsertProvincesAsync(provinces, ct, syncId, trigger);
            provIns = provResult.InsertedCount;
            provUpd = provResult.UpdatedCount;
            provUnch = provResult.UnchangedCount;
            provFail = provResult.FailCount;
            _logger.LogInformation(
                "Đồng bộ EOMS_PROVINCES xong: API={Fetched}, Inserted={Ins}, Updated={Upd}, Unchanged={Unch}, Fail={Fail}.",
                provinces.Count, provIns, provUpd, provUnch, provFail);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            provException = ex;
            _logger.LogError(ex, "Đồng bộ EOMS_PROVINCES thất bại.");
        }
        finally
        {
            provSw.Stop();
        }

        var provSuccess = provException == null && provFail == 0 && provinces.Count > 0;
        var provMsg = provException != null
            ? $"Đồng bộ EOMS_PROVINCES thất bại: {provException.Message}"
            : provinces.Count == 0
                ? "API EOMS_PROVINCES không trả về dữ liệu."
                : $"Đồng bộ EOMS_PROVINCES xong: API={provinces.Count}, Inserted={provIns}, Updated={provUpd}, Unchanged={provUnch}, Fail={provFail}.";
        _mongoRepo.InsertLog(new EomsCategorySyncLog
        {
            SyncId = syncId,
            Trigger = trigger,
            Category = "EOMS_PROVINCES",
            Action = "BATCH_SYNC",
            Success = provSuccess,
            ApiFetchedCount = provinces.Count,
            InsertedCount = provIns,
            UpdatedCount = provUpd,
            UnchangedCount = provUnch,
            UpsertSuccessCount = provIns + provUpd,
            UpsertFailCount = provFail,
            DurationMs = provSw.ElapsedMilliseconds,
            Message = provMsg,
            ErrorDetail = provException?.ToString() ?? (provFail > 0 ? $"Có {provFail} bản ghi EOMS_PROVINCES upsert thất bại." : null)
        });

        // 2. Đồng bộ Phường/Xã theo từng tỉnh (level=3, parentCode=<province.Code>)
        var commSw = Stopwatch.StartNew();
        if (provinces.Count > 0)
        {
            try
            {
                var existingCommunes = await _repository.GetExistingRawJsonMapAsync("EOMS_COMMUNES", ct);

                foreach (var prov in provinces)
                {
                    ct.ThrowIfCancellationRequested();
                    try
                    {
                        var communes = await _client.GetAreasAsync("3", prov.Code, ct);
                        commFetchedTotal += communes.Count;
                        var commResult = await _repository.UpsertCommunesAsync(communes, ct, syncId, trigger, existingCommunes);
                        commIns += commResult.InsertedCount;
                        commUpd += commResult.UpdatedCount;
                        commUnch += commResult.UnchangedCount;
                        commFail += commResult.FailCount;
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        commProvFail++;
                        if (commErrors.Count < 20)
                            commErrors.Add($"Province={prov.Code}: {ex.Message}");
                        _logger.LogError(ex, "Đồng bộ EOMS_COMMUNES thất bại cho tỉnh CODE={ProvinceCode} ({ProvinceName}).", prov.Code, prov.Name);
                        _mongoRepo.InsertLog(new EomsCategorySyncLog
                        {
                            SyncId = syncId,
                            Trigger = trigger,
                            Category = "EOMS_COMMUNES",
                            Action = "API_CALL",
                            ItemCode = prov.Code,
                            Success = false,
                            Message = $"Đồng bộ EOMS_COMMUNES thất bại cho tỉnh CODE={prov.Code} ({prov.Name}): {ex.Message}",
                            ErrorDetail = ex.ToString()
                        });
                    }
                }

                _logger.LogInformation(
                    "Đồng bộ EOMS_COMMUNES xong: API={Fetched}, Tỉnh lỗi={ProvFail}/{TotalProv}, Inserted={Ins}, Updated={Upd}, Unchanged={Unch}, Fail={Fail}.",
                    commFetchedTotal, commProvFail, provinces.Count, commIns, commUpd, commUnch, commFail);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                commException = ex;
                _logger.LogError(ex, "Đồng bộ EOMS_COMMUNES thất bại.");
            }
        }
        else
        {
            _logger.LogWarning("Bỏ qua đồng bộ EOMS_COMMUNES vì không lấy được danh sách tỉnh.");
        }
        commSw.Stop();

        var commSuccess = commException == null && commFail == 0 && commProvFail == 0 && provinces.Count > 0;
        var commMsg = commException != null
            ? $"Đồng bộ EOMS_COMMUNES thất bại: {commException.Message}"
            : provinces.Count == 0
                ? "Bỏ qua đồng bộ EOMS_COMMUNES vì không lấy được danh sách tỉnh."
                : $"Đồng bộ EOMS_COMMUNES xong: API={commFetchedTotal}, Tỉnh lỗi={commProvFail}/{provinces.Count}, Inserted={commIns}, Updated={commUpd}, Unchanged={commUnch}, Fail={commFail}.";
        var commErrDetail = commException?.ToString()
            ?? (commErrors.Count > 0
                ? string.Join("; ", commErrors)
                : (commFail > 0 ? $"Có {commFail} bản ghi EOMS_COMMUNES upsert thất bại." : null));
        _mongoRepo.InsertLog(new EomsCategorySyncLog
        {
            SyncId = syncId,
            Trigger = trigger,
            Category = "EOMS_COMMUNES",
            Action = "BATCH_SYNC",
            Success = commSuccess,
            ApiFetchedCount = commFetchedTotal,
            InsertedCount = commIns,
            UpdatedCount = commUpd,
            UnchangedCount = commUnch,
            UpsertSuccessCount = commIns + commUpd,
            UpsertFailCount = commFail,
            DurationMs = commSw.ElapsedMilliseconds,
            Message = commMsg,
            ErrorDetail = commErrDetail
        });

        // 3. Đồng bộ Sản phẩm (phân trang)
        var prodSw = Stopwatch.StartNew();
        try
        {
            var existingProducts = await _repository.GetExistingRawJsonMapAsync("EOMS_PRODUCT", ct);
            var pageSize = opts.ProductPageSize > 0 ? opts.ProductPageSize : 100;
            var brand = string.IsNullOrWhiteSpace(opts.ProductBrand) ? "EMS" : opts.ProductBrand.Trim();
            var currentPage = 1;

            while (!ct.IsCancellationRequested)
            {
                var (paging, items) = await _client.GetProductPageAsync(currentPage, pageSize, brand, ct);
                if (items.Count == 0)
                    break;

                totalFetched += items.Count;
                var prodResult = await _repository.UpsertProductsAsync(items, ct, syncId, trigger, existingProducts);
                prodIns += prodResult.InsertedCount;
                prodUpd += prodResult.UpdatedCount;
                prodUnch += prodResult.UnchangedCount;
                prodFail += prodResult.FailCount;

                if (paging.TotalCount <= 0 || currentPage * pageSize >= paging.TotalCount)
                    break;

                currentPage++;
            }

            _logger.LogInformation(
                "Đồng bộ EOMS_PRODUCT xong: API={Fetched}, Inserted={Ins}, Updated={Upd}, Unchanged={Unch}, Fail={Fail}.",
                totalFetched, prodIns, prodUpd, prodUnch, prodFail);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            prodException = ex;
            _logger.LogError(ex, "Đồng bộ EOMS_PRODUCT thất bại.");
        }
        finally
        {
            prodSw.Stop();
        }

        var prodSuccess = prodException == null && prodFail == 0;
        var prodMsg = prodException != null
            ? $"Đồng bộ EOMS_PRODUCT thất bại: {prodException.Message}"
            : $"Đồng bộ EOMS_PRODUCT xong: API={totalFetched}, Inserted={prodIns}, Updated={prodUpd}, Unchanged={prodUnch}, Fail={prodFail}.";
        _mongoRepo.InsertLog(new EomsCategorySyncLog
        {
            SyncId = syncId,
            Trigger = trigger,
            Category = "EOMS_PRODUCT",
            Action = "BATCH_SYNC",
            Success = prodSuccess,
            ApiFetchedCount = totalFetched,
            InsertedCount = prodIns,
            UpdatedCount = prodUpd,
            UnchangedCount = prodUnch,
            UpsertSuccessCount = prodIns + prodUpd,
            UpsertFailCount = prodFail,
            DurationMs = prodSw.ElapsedMilliseconds,
            Message = prodMsg,
            ErrorDetail = prodException?.ToString() ?? (prodFail > 0 ? $"Có {prodFail} bản ghi EOMS_PRODUCT upsert thất bại." : null)
        });

        sw.Stop();
        var overallSuccess = provSuccess && commSuccess && prodSuccess;
        var totalInserted = provIns + commIns + prodIns;
        var totalUpdated = provUpd + commUpd + prodUpd;
        var totalUnchanged = provUnch + commUnch + prodUnch;
        var summaryMsg =
            $"Hoàn tất đồng bộ danh mục EOMS ({trigger}) trong {sw.ElapsedMilliseconds}ms. " +
            $"Provinces(Ins={provIns},Upd={provUpd},Unch={provUnch},Fail={provFail}), " +
            $"Communes(Ins={commIns},Upd={commUpd},Unch={commUnch},Fail={commFail},ProvFail={commProvFail}), " +
            $"Products(Ins={prodIns},Upd={prodUpd},Unch={prodUnch},Fail={prodFail}).";
        var summaryErrors = new List<string>();
        if (!provSuccess) summaryErrors.Add(provMsg);
        if (!commSuccess) summaryErrors.Add(commMsg);
        if (!prodSuccess) summaryErrors.Add(prodMsg);

        _mongoRepo.InsertLog(new EomsCategorySyncLog
        {
            SyncId = syncId,
            Trigger = trigger,
            Category = "SUMMARY",
            Action = "RUN_SUMMARY",
            Success = overallSuccess,
            ApiFetchedCount = provinces.Count + commFetchedTotal + totalFetched,
            InsertedCount = totalInserted,
            UpdatedCount = totalUpdated,
            UnchangedCount = totalUnchanged,
            UpsertSuccessCount = totalInserted + totalUpdated,
            UpsertFailCount = provFail + commFail + prodFail,
            DurationMs = sw.ElapsedMilliseconds,
            Message = summaryMsg,
            ErrorDetail = summaryErrors.Count > 0 ? string.Join(" | ", summaryErrors) : null
        });

        _logger.LogInformation(
            "Hoàn tất đồng bộ danh mục EOMS ({Trigger}) trong {ElapsedMs}ms. SyncId={SyncId}, Success={Success}, Provinces(Ins={ProvIns},Upd={ProvUpd},Unch={ProvUnch},Fail={ProvFail}), Communes(Ins={CommIns},Upd={CommUpd},Unch={CommUnch},Fail={CommFail},ProvFail={CommProvFail}), Products(Ins={ProdIns},Upd={ProdUpd},Unch={ProdUnch},Fail={ProdFail}).",
            trigger, sw.ElapsedMilliseconds, syncId, overallSuccess,
            provIns, provUpd, provUnch, provFail,
            commIns, commUpd, commUnch, commFail, commProvFail,
            prodIns, prodUpd, prodUnch, prodFail);
    }

    private static TimeSpan CalculateDelayUntilNextRun(string runAtLocalTime, string timeZoneId, out DateTime nextLocal)
    {
        var tz = ResolveTimeZone(timeZoneId);
        var nowUtc = DateTime.UtcNow;
        var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, tz);

        var targetTime = TimeSpan.Zero; // 00:00
        if (!string.IsNullOrWhiteSpace(runAtLocalTime)
            && TimeSpan.TryParseExact(runAtLocalTime.Trim(), @"hh\:mm", CultureInfo.InvariantCulture, out var parsed))
        {
            targetTime = parsed;
        }

        nextLocal = nowLocal.Date.Add(targetTime);
        if (nextLocal <= nowLocal)
            nextLocal = nextLocal.AddDays(1);

        var nextUtc = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(nextLocal, DateTimeKind.Unspecified), tz);
        var delay = nextUtc - nowUtc;
        return delay > TimeSpan.Zero ? delay : TimeSpan.FromSeconds(1);
    }

    private static TimeZoneInfo ResolveTimeZone(string timeZoneId)
    {
        if (!string.IsNullOrWhiteSpace(timeZoneId))
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId.Trim());
            }
            catch (TimeZoneNotFoundException)
            {
            }
        }

        foreach (var fallbackId in new[] { "SE Asia Standard Time", "Asia/Ho_Chi_Minh" })
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(fallbackId);
            }
            catch (TimeZoneNotFoundException)
            {
            }
        }

        return TimeZoneInfo.CreateCustomTimeZone("UTC+7", TimeSpan.FromHours(7), "Vietnam Standard Time", "Vietnam Standard Time");
    }
}
