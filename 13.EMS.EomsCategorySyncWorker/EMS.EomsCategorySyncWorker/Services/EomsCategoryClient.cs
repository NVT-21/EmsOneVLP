using System.Net.Http.Headers;
using System.Text;
using EMS.EomsCategorySyncWorker.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EMS.EomsCategorySyncWorker.Services;

public class EomsCategoryClient
{
    private const string UnifiedPath = "api/unified";
    private const string AreaAppApiCode = "vnp.base.admin.areas.sub";
    private const string ProductAppApiCode = "vnp.base.product.page.sub";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<EomsCategoryOptions> _optionsMonitor;
    private readonly ILogger<EomsCategoryClient> _logger;

    public EomsCategoryClient(
        IHttpClientFactory httpClientFactory,
        IOptionsMonitor<EomsCategoryOptions> optionsMonitor,
        ILogger<EomsCategoryClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    public async Task<List<EomsAreaItem>> GetAreasAsync(string level, string parentCode, CancellationToken ct)
    {
        var innerBody = JsonConvert.SerializeObject(new
        {
            level,
            parentCode,
            nameStr = string.Empty
        });

        var resp = await PostUnifiedAsync<JArray>(AreaAppApiCode, innerBody, ct);
        var items = new List<EomsAreaItem>();
        if (resp.Data == null)
            return items;

        foreach (var token in resp.Data)
        {
            if (token is not JObject obj)
                continue;

            var item = obj.ToObject<EomsAreaItem>();
            if (item == null || string.IsNullOrWhiteSpace(item.Code))
                continue;

            item.RawJson = obj.ToString(Formatting.None);
            items.Add(item);
        }

        return items;
    }

    public async Task<(EomsPagingInfo Paging, List<EomsProductItem> Items)> GetProductPageAsync(
        int currentPage,
        int pageSize,
        string brand,
        CancellationToken ct)
    {
        var innerBody = JsonConvert.SerializeObject(new
        {
            currentPage,
            pageSize,
            brand
        });

        var resp = await PostUnifiedAsync<EomsProductPageData>(ProductAppApiCode, innerBody, ct);
        var paging = resp.Data?.Paging ?? new EomsPagingInfo
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = 0
        };

        var items = new List<EomsProductItem>();
        if (resp.Data?.TableData == null)
            return (paging, items);

        foreach (var obj in resp.Data.TableData)
        {
            if (obj == null)
                continue;

            var item = obj.ToObject<EomsProductItem>();
            if (item == null || string.IsNullOrWhiteSpace(item.Code))
                continue;

            item.RawJson = obj.ToString(Formatting.None);
            items.Add(item);
        }

        return (paging, items);
    }

    private async Task<EomsUnifiedResponse<TData>> PostUnifiedAsync<TData>(
        string appApiCode,
        string innerBodyJson,
        CancellationToken ct)
    {
        var opts = _optionsMonitor.CurrentValue;
        if (string.IsNullOrWhiteSpace(opts.BaseUrl))
            throw new InvalidOperationException("EomsCategory:BaseUrl chưa được cấu hình.");

        var endpointUrl = $"{opts.BaseUrl.TrimEnd('/')}/{UnifiedPath}";
        var unifiedRequest = new EomsUnifiedRequest
        {
            AppCode = string.IsNullOrWhiteSpace(opts.AppCode) ? "VLP" : opts.AppCode.Trim(),
            AppApiCode = appApiCode,
            SchemeCode = string.IsNullOrWhiteSpace(opts.SchemeCode) ? "VLP-RC" : opts.SchemeCode.Trim(),
            Body = innerBodyJson
        };

        var requestJson = JsonConvert.SerializeObject(unifiedRequest);
        var sign = opts.UseSign ? EomsSignatureHelper.ComputeSign(innerBodyJson, opts.SecretKey.Trim()) : null;
        var maxRetry = Math.Max(1, opts.MaxRetry);

        Exception? lastEx = null;
        for (var attempt = 1; attempt <= maxRetry; attempt++)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                var client = _httpClientFactory.CreateClient("EomsCategory");
                using var req = new HttpRequestMessage(HttpMethod.Post, endpointUrl);
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrEmpty(sign))
                    req.Headers.TryAddWithoutValidation("sign", sign);

                req.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                using var res = await client.SendAsync(req, ct);
                var rawResponse = await res.Content.ReadAsStringAsync(ct);

                if ((int)res.StatusCode >= 500 && attempt < maxRetry)
                {
                    _logger.LogWarning(
                        "EOMS API {AppApiCode} trả HTTP {StatusCode} (lần {Attempt}/{MaxRetry}), thử lại...",
                        appApiCode, (int)res.StatusCode, attempt, maxRetry);
                    await DelayBackoffAsync(attempt, ct);
                    continue;
                }

                if (!res.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        $"EOMS API {appApiCode} lỗi HTTP {(int)res.StatusCode}: {Truncate(rawResponse, 500)}");
                }

                var parsed = JsonConvert.DeserializeObject<EomsUnifiedResponse<TData>>(rawResponse);
                if (parsed == null)
                    throw new InvalidOperationException($"EOMS API {appApiCode} trả về JSON rỗng/không hợp lệ.");

                if (!parsed.Success)
                {
                    throw new InvalidOperationException(
                        $"EOMS API {appApiCode} thất bại: [{parsed.ErrorCode}] {parsed.ResolvedErrorMessage} (traceId={parsed.TraceId})");
                }

                return parsed;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                lastEx = ex;
                if (attempt >= maxRetry)
                    break;

                _logger.LogWarning(ex,
                    "Lỗi gọi EOMS API {AppApiCode} (lần {Attempt}/{MaxRetry}), thử lại...",
                    appApiCode, attempt, maxRetry);
                await DelayBackoffAsync(attempt, ct);
            }
        }

        throw lastEx ?? new HttpRequestException($"Gọi EOMS API {appApiCode} thất bại sau {maxRetry} lần.");
    }

    private static Task DelayBackoffAsync(int attempt, CancellationToken ct)
    {
        var delaySeconds = Math.Pow(2, attempt); // 2s, 4s, 8s
        return Task.Delay(TimeSpan.FromSeconds(delaySeconds), ct);
    }

    private static string Truncate(string? value, int maxLen)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLen)
            return value ?? string.Empty;
        return value[..maxLen] + "...";
    }
}
