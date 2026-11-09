using EMS.TransportWorkerService.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EMS.TransportWorkerService.Repositories;

public class PNSVnpostRepository
{
    private readonly APILibraryRepository _apiLibraryRepository;
    private readonly PushPNSVNPOSTMongoRepository _pushLogRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PNSVnpostRepository> _logger;

    public PNSVnpostRepository(
        APILibraryRepository apiLibraryRepository,
        PushPNSVNPOSTMongoRepository pushLogRepository,
        IConfiguration configuration,
        ILogger<PNSVnpostRepository> logger)
    {
        _apiLibraryRepository = apiLibraryRepository;
        _pushLogRepository = pushLogRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public bool IsEnabled()
    {
        if (!_configuration.GetValue("PNSVnpost:Enabled", true))
            return false;

        return !string.IsNullOrWhiteSpace(_configuration["PNSVnpost:ApiUrl"])
            && !string.IsNullOrWhiteSpace(_configuration["PNSVnpost:ApiKey"]);
    }

    public async Task<ZResponse> CreateOrderAsync(OrderPush order, CancellationToken cancellationToken = default)
    {
        var res = new ZResponse();

        try
        {
            if (order == null)
            {
                res.Code = "99";
                res.Message = "Dữ liệu gửi sang PNS Vnpost không hợp lệ.";
                return res;
            }

            var apiUrl = _configuration["PNSVnpost:ApiUrl"];
            var apiKey = _configuration["PNSVnpost:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiUrl) || string.IsNullOrWhiteSpace(apiKey))
            {
                res.Code = "99";
                res.Message = "Chưa cấu hình PNSVnpost:ApiUrl hoặc PNSVnpost:ApiKey.";
                return res;
            }

            var requestBody = order.ToApiBody();
            var requestJson = JsonConvert.SerializeObject(requestBody);

            var baseUrl = apiUrl.TrimEnd('/') + "/";
            _logger.LogInformation(
                "PNS Vnpost CreateOrder bắt đầu. Url={Url}, ItemCode={ItemCode}",
                baseUrl + "Partner/EMS/CreateOrder",
                order.ItemCode);

            var callAPI = await _apiLibraryRepository.InvokeEMSAsync(new ZEMSRequest
            {
                Method = "POST",
                RequestURL = baseUrl,
                RequestURI = "Partner/EMS/CreateOrder",
                APIKeyName = "API_KEY",
                APIKeyValue = apiKey,
                Value = requestBody
            }, cancellationToken).ConfigureAwait(false);

            if (callAPI.Code != "00")
            {
                _logger.LogWarning(
                    "PNS Vnpost HTTP lỗi. ItemCode={ItemCode}, Code={Code}, Message={Message}, Data={Data}",
                    order.ItemCode,
                    callAPI.Code,
                    callAPI.Message,
                    callAPI.Data);

                _pushLogRepository.InsertLog(
                    requestJson,
                    callAPI.Data ?? callAPI.Message,
                    order.ItemCode,
                    callAPI.Code,
                    success: false);

                return callAPI;
            }

            var parsed = ParseCreateOrderResponse(callAPI.Data, order);
            var ok = parsed.Code == "00" || parsed.Code == "01";



            // Ghi Mongo mọi lần gọi (thành công / thất bại) để dễ kiểm tra.
            _pushLogRepository.InsertLog(
                requestJson,
                callAPI.Data ?? "",
                order.ItemCode,
                parsed.Code,
                ok);

            return parsed;
        }
        catch (Exception ex)
        {
            res.Code = "99";
            res.Message = "Lỗi kết nối với API PNS Vnpost, vui lòng liên hệ quản trị.";
            _logger.LogError(
                ex,
                "PNSVnpostRepository.CreateOrderAsync failed. ItemCode={ItemCode}",
                order?.ItemCode);
            return res;
        }
    }

    private ZResponse ParseCreateOrderResponse(string responseContent, OrderPush order)
    {
        var res = new ZResponse();

        if (string.IsNullOrWhiteSpace(responseContent))
        {
            res.Code = "00";
            res.Message = "PNS Vnpost CreateOrder thành công (empty body).";
            return res;
        }

        try
        {
            var emsStyle = JsonConvert.DeserializeObject<ZEMSResponse>(responseContent);
            if (emsStyle != null && !string.IsNullOrWhiteSpace(emsStyle.Code))
            {
                res.Code = emsStyle.Code;
                res.Message = emsStyle.Message;
                res.Data = emsStyle.Value ?? responseContent;

                if (res.Code != "00" && res.Code != "01")
                {
                    _logger.LogWarning(
                        "PNSVnpost CreateOrder non-success. ItemCode={ItemCode}||Response={Response}",
                        order.ItemCode,
                        responseContent);
                }

                return res;
            }

            var token = JToken.Parse(responseContent);
            var code = token.Value<string>("Code")
                ?? token.Value<string>("code")
                ?? token.Value<string>("Status")
                ?? token.Value<string>("status")
                ?? token.SelectToken("data.Code")?.Value<string>()
                ?? token.SelectToken("Data.Code")?.Value<string>();

            var message = token.Value<string>("Message")
                ?? token.Value<string>("message")
                ?? token.Value<string>("Description")
                ?? token.SelectToken("data.Message")?.Value<string>()
                ?? token.SelectToken("Data.Message")?.Value<string>();

            if (!string.IsNullOrWhiteSpace(code))
            {
                res.Code = code;
                res.Message = message ?? responseContent;
                res.Data = responseContent;

                if (res.Code != "00" && res.Code != "01" &&
                    !string.Equals(res.Code, "success", StringComparison.OrdinalIgnoreCase) &&
                    res.Code != "200")
                {
                    _logger.LogWarning(
                        "PNSVnpost CreateOrder non-success. ItemCode={ItemCode}||Response={Response}",
                        order.ItemCode,
                        responseContent);
                }
                else if (string.Equals(res.Code, "success", StringComparison.OrdinalIgnoreCase) || res.Code == "200")
                {
                    res.Code = "00";
                }

                return res;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "PNSVnpost CreateOrder: không parse được body, coi HTTP OK là thành công. ItemCode={ItemCode}||Body={Body}",
                order.ItemCode,
                responseContent);
        }

        res.Code = "00";
        res.Message = "PNS Vnpost CreateOrder thành công.";
        res.Data = responseContent;
        return res;
    }
}
