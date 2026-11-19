using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EMS.EomsCategorySyncWorker.Models;

public class EomsCategoryOptions
{
    public bool Enabled { get; set; } = true;
    public string BaseUrl { get; set; } = "https://uat-vlp-edi.vnpost.vn";
    public string SecretKey { get; set; } = string.Empty;
    public string AppCode { get; set; } = "VLP";
    public string SchemeCode { get; set; } = "VLP-RC";
    public bool UseSign { get; set; } = true;
    public string RunAtLocalTime { get; set; } = "00:00";
    public string TimeZoneId { get; set; } = "Asia/Ho_Chi_Minh";
    public bool RunOnStartup { get; set; } = false;
    public int ProductPageSize { get; set; } = 100;
    public string ProductBrand { get; set; } = "EMS";
    public int MaxRetry { get; set; } = 3;
    public string? MongoConnectionString { get; set; }
    public string? MongoDatabaseName { get; set; }
    public string MongoCollectionName { get; set; } = "EomsCategorySyncLog";
}

/// <summary>
/// Log đồng bộ danh mục EOMS (Tỉnh/Xã/Sản phẩm) lưu MongoDB.
/// </summary>
[BsonIgnoreExtraElements]
public class EomsCategorySyncLog
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string SyncId { get; set; } = string.Empty;
    public string Trigger { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // EOMS_PROVINCES | EOMS_COMMUNES | EOMS_PRODUCT | SUMMARY
    public string Action { get; set; } = string.Empty;   // BATCH_SYNC | DB_UPSERT | API_CALL | RUN_SUMMARY
    public string? ItemCode { get; set; }
    public bool Success { get; set; }
    public int ApiFetchedCount { get; set; }
    public int InsertedCount { get; set; }
    public int UpdatedCount { get; set; }
    public int UnchangedCount { get; set; }
    public int UpsertSuccessCount { get; set; }
    public int UpsertFailCount { get; set; }
    public long DurationMs { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorDetail { get; set; }
    public string? Request { get; set; }
    public string? Response { get; set; }
    public DateTime Time { get; set; }
    public string TimeText { get; set; } = string.Empty;
}

public class EomsUnifiedRequest
{
    [JsonProperty("appCode")]
    public string AppCode { get; set; } = "VLP";

    [JsonProperty("appApiCode")]
    public string AppApiCode { get; set; } = string.Empty;

    [JsonProperty("schemeCode")]
    public string SchemeCode { get; set; } = "VLP-RC";

    [JsonProperty("body")]
    public string Body { get; set; } = string.Empty;
}

public class EomsUnifiedResponse<TData>
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("traceId")]
    public string? TraceId { get; set; }

    [JsonProperty("errorCode")]
    public string? ErrorCode { get; set; }

    [JsonProperty("errorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonProperty("errorMsg")]
    public string? ErrorMsg { get; set; }

    [JsonIgnore]
    public string? ResolvedErrorMessage => !string.IsNullOrWhiteSpace(ErrorMessage) ? ErrorMessage : ErrorMsg;

    [JsonProperty("data")]
    public TData? Data { get; set; }
}

public class EomsAreaItem
{
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("level")]
    public int? Level { get; set; }

    [JsonProperty("parentCode")]
    public string? ParentCode { get; set; }

    [JsonProperty("tag")]
    public string? Tag { get; set; }

    [JsonProperty("postCode")]
    public string? PostCode { get; set; }

    [JsonProperty("phoneCountryCode")]
    public string? PhoneCountryCode { get; set; }

    [JsonIgnore]
    public string RawJson { get; set; } = string.Empty;
}

public class EomsProductPageData
{
    [JsonProperty("paging")]
    public EomsPagingInfo? Paging { get; set; }

    [JsonProperty("tableData")]
    public List<JObject>? TableData { get; set; }
}

public class EomsPagingInfo
{
    [JsonProperty("currentPage")]
    public int CurrentPage { get; set; }

    [JsonProperty("pageSize")]
    public int PageSize { get; set; }

    [JsonProperty("totalCount")]
    public int TotalCount { get; set; }
}

public class EomsProductItem
{
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("businessType")]
    public string? BusinessType { get; set; }

    [JsonProperty("businessTypeName")]
    public string? BusinessTypeName { get; set; }

    [JsonProperty("timeLevel")]
    public string? TimeLevel { get; set; }

    [JsonProperty("timeLevelName")]
    public string? TimeLevelName { get; set; }

    [JsonProperty("operationMethod")]
    public string? OperationMethod { get; set; }

    [JsonProperty("operationMethodName")]
    public string? OperationMethodName { get; set; }

    [JsonProperty("suppliers")]
    public string? Suppliers { get; set; }

    [JsonProperty("suppliersName")]
    public string? SuppliersName { get; set; }

    [JsonProperty("longTermFlag")]
    public string? LongTermFlag { get; set; }

    [JsonProperty("validTimeStart")]
    public string? ValidTimeStart { get; set; }

    [JsonProperty("validTimeEnd")]
    public string? ValidTimeEnd { get; set; }

    [JsonProperty("split")]
    public string? SplitFlag { get; set; }

    [JsonProperty("maxSizeStr")]
    public string? MaxSizeStr { get; set; }

    [JsonProperty("maxWeight")]
    public string? MaxWeight { get; set; }

    [JsonProperty("maxVolume")]
    public string? MaxVolume { get; set; }

    [JsonProperty("volumeWeightRatio")]
    public string? VolumeWeightRatio { get; set; }

    [JsonProperty("deliveryMethod")]
    public string? DeliveryMethod { get; set; }

    [JsonProperty("coverage")]
    public string? Coverage { get; set; }

    [JsonProperty("saleChannel")]
    public string? SaleChannel { get; set; }

    [JsonProperty("limitGoodsAttr")]
    public string? LimitGoodsAttr { get; set; }

    [JsonProperty("transportMode")]
    public string? TransportMode { get; set; }

    [JsonProperty("basicService")]
    public JToken? BasicService { get; set; }

    [JsonProperty("valueAddedService")]
    public JToken? ValueAddedService { get; set; }

    [JsonProperty("brand")]
    public string? Brand { get; set; }

    [JsonIgnore]
    public string RawJson { get; set; } = string.Empty;
}

/// <summary>
/// Port từ ZEomsSignatureHelper trong EMS.Entity để ký header "sign" cho API /api/unified.
/// </summary>
public static class EomsSignatureHelper
{
    private static readonly Regex LongNumberRegex = new(
        @"(:|,|\[)\s*(-?\d{15,})(\s*|,|\]|\})",
        RegexOptions.Compiled);

    public static string BuildSignContent(string innerBodyJson, string secret)
    {
        if (string.IsNullOrWhiteSpace(innerBodyJson))
            throw new ArgumentException("Body JSON không được để trống khi ký EOMS.", nameof(innerBodyJson));

        var safeJsonStr = LongNumberRegex.Replace(innerBodyJson, "$1\"$2\"$3");

        using var sr = new StringReader(safeJsonStr);
        using var jr = new JsonTextReader(sr)
        {
            DateParseHandling = DateParseHandling.None,
            FloatParseHandling = FloatParseHandling.Decimal
        };

        var parsed = JToken.Load(jr);
        var flattened = parsed.Type == JTokenType.Array
            ? FlattenArray(string.Empty, (JArray)parsed)
            : FlattenObject((JObject)parsed);

        return flattened + "&" + (secret ?? string.Empty);
    }

    public static string ComputeSign(string innerBodyJson, string secret)
    {
        var content = BuildSignContent(innerBodyJson, secret);
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = MD5.HashData(bytes);
        return Convert.ToHexStringLower(hash);
    }

    private static string FlattenArray(string key, JArray arr)
    {
        var prefix = !string.IsNullOrEmpty(key) ? (key + "=") : string.Empty;
        var parts = new List<string>(arr.Count);

        foreach (var item in arr)
        {
            if (item == null || item.Type == JTokenType.Null)
                parts.Add("null");
            else if (item.Type == JTokenType.Array)
                parts.Add(FlattenArray(key, (JArray)item));
            else if (item.Type == JTokenType.Object)
                parts.Add(FlattenObject((JObject)item));
            else
                parts.Add(FormatPrimitive((JValue)item));
        }

        return prefix + string.Join(",", parts);
    }

    private static string FlattenObject(JObject obj)
    {
        var sortedProps = obj.Properties()
            .OrderBy(p => p.Name, StringComparer.Ordinal)
            .ToList();

        var result = new List<string>(sortedProps.Count);
        foreach (var prop in sortedProps)
        {
            var key = prop.Name;
            var value = prop.Value;

            if (value == null || value.Type == JTokenType.Null)
                result.Add(key + "=null");
            else if (value.Type == JTokenType.Array)
                result.Add(FlattenArray(key, (JArray)value));
            else if (value.Type == JTokenType.Object)
                result.Add(FlattenObject((JObject)value));
            else
                result.Add(key + "=" + FormatPrimitive((JValue)value));
        }

        return string.Join("&", result);
    }

    private static string FormatPrimitive(JValue val)
    {
        if (val == null || val.Type == JTokenType.Null || val.Value == null)
            return "null";

        if (val.Type == JTokenType.Boolean)
            return (bool)val.Value ? "true" : "false";

        if (val.Value is IFormattable formattable)
            return formattable.ToString(null, CultureInfo.InvariantCulture);

        return val.Value.ToString() ?? string.Empty;
    }
}
