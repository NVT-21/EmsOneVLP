using System.Net.Http.Json;
using EMS.TransportWorkerService.Models;
using EMS.TransportWorkerService.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EMS.TransportWorkerService;

public class EMSRepository
{
    private readonly APILibraryRepository _apiLibraryRepository;
    private readonly PushPNSEMSMongoRepository _pushLogRepository;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<EMSRepository> _logger;

    public EMSRepository(
        APILibraryRepository apiLibraryRepository,
        PushPNSEMSMongoRepository pushLogRepository,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<EMSRepository> logger)
    {
        _apiLibraryRepository = apiLibraryRepository;
        _pushLogRepository = pushLogRepository;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<ZResponse> EMSSendRequestTransportAsync(string data, CancellationToken cancellationToken = default)
    {
        var res = new ZResponse();
        var itemCode = "";

        try
        {
            var requestValue =
                JsonConvert.DeserializeObject<ZTransportInfoShippingApiRequest>(data);

            if (requestValue == null)
            {
                res.Code = "99";
                res.Message = "Dữ liệu gửi sang EMS không hợp lệ.";
                _pushLogRepository.InsertLog(data, res.Message, itemCode, res.Code, false);
                return res;
            }

            itemCode = requestValue.LIST_ITEM?.FirstOrDefault()?.TRACKING_CODE ?? "";

            var emsApiUrl = _configuration["EMS:ApiUrl"];
            var emsApiKey = _configuration["EMS:ApiKey"];
            var emsAuthorization = _configuration["EMS:Authorization"];

            _logger.LogInformation(
                "PNS EMS CreatePickupv1 bắt đầu. ItemCode={ItemCode}",
                itemCode);

            var callAPI = await _apiLibraryRepository.InvokeEMSAsync(new ZEMSRequest
            {
                Method = "POST",
                RequestURL = emsApiUrl,
                RequestURI = "api/Pickup/CreatePickupv1",
                APIKeyName = "APIKey",
                APIKeyValue = emsApiKey,
                AuthorizationName = "Basic",
                AuthorizationValue = emsAuthorization,
                Value = requestValue
            }, cancellationToken).ConfigureAwait(false);

            if (callAPI.Code != "00")
            {
                _logger.LogWarning(
                    "PNS EMS HTTP lỗi. ItemCode={ItemCode}, Code={Code}, Message={Message}",
                    itemCode,
                    callAPI.Code,
                    callAPI.Message);

                _pushLogRepository.InsertLog(
                    data,
                    callAPI.Data ?? callAPI.Message,
                    itemCode,
                    callAPI.Code,
                    success: false);

                return callAPI;
            }

            var resEMS = JsonConvert.DeserializeObject<ZEMSResponse>(callAPI.Data?.ToString());

            if (resEMS == null)
            {
                res.Code = "99";
                res.Message = "Không đọc được phản hồi từ EMS.";

                _logger.LogWarning(
                    "EMSRepository.EMSSendRequestTransportAsync: could not parse EMS response. Request={Request}||Response={Response}",
                    data,
                    callAPI.Data);

                _pushLogRepository.InsertLog(data, callAPI.Data?.ToString() ?? "", itemCode, res.Code, false);
                return res;
            }

            if (resEMS.Code == "00" || resEMS.Code == "01")
            {
                res.Code = resEMS.Code;
                res.Message = resEMS.Message;
                res.Data = resEMS.Value;
            }
            else
            {
                res.Code = resEMS.Code;
                res.Message = resEMS.Message;

                _logger.LogWarning(
                    "EMSRepository.EMSSendRequestTransportAsync: EMS returned non-success. Request={Request}||Response={Response}",
                    data,
                    JsonConvert.SerializeObject(resEMS));
            }

            var ok = res.Code == "00" || res.Code == "01";
            _pushLogRepository.InsertLog(
                data,
                callAPI.Data?.ToString() ?? JsonConvert.SerializeObject(resEMS),
                itemCode,
                res.Code,
                ok);
        }
        catch (Exception ex)
        {
            res.Code = "99";
            res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";

            _logger.LogError(
                ex,
                "EMSRepository.EMSSendRequestTransportAsync failed. Request={Request}",
                data);

            _pushLogRepository.InsertLog(data, ex.Message, itemCode, res.Code, false);
        }

        return res;
    }

    /// <summary>
    /// Gọi Sync Gateway (tương đương EMS.API.Core EMSRepository.SendReceiverInfor).
    /// Nếu không cấu hình SyncGateway:BaseUrl thì bỏ qua và trả Code 00.
    /// </summary>
    public async Task<ZResponse> SendReceiverInforAsync(
        ZSendReceiverInforRequest req,
        CancellationToken cancellationToken = default)
    {
        var res = new ZResponse();
        var gatewayUrl = _configuration["SyncGateway:BaseUrl"];
        if (string.IsNullOrWhiteSpace(gatewayUrl))
        {
            _logger.LogDebug("SendReceiverInfor skipped: SyncGateway:BaseUrl is not set.");
            res.Code = "00";
            res.Message = "Skipped";
            return res;
        }

        try
        {
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(gatewayUrl.TrimEnd('/') + "/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var body = new ZRequest
            {
                Code = "EMS_SEND_RECEIVER_INFOR",
                Data = JsonConvert.SerializeObject(req),
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };

            var response = await client.PostAsJsonAsync("Execute", body, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var parsed = JsonConvert.DeserializeObject<ZResponse>(responseContent);
                if (parsed != null)
                    return parsed;
                res.Code = "00";
                res.Data = responseContent;
                res.Message = "Thành công";
                return res;
            }

            _logger.LogWarning(
                "SendReceiverInfor HTTP error. Status={Status}||Body={Body}",
                response.StatusCode,
                responseContent);
            res.Code = "99";
            res.Message = "Lỗi kết nối Sync Gateway.";
        }
        catch (Exception ex)
        {
            res.Code = "99";
            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            _logger.LogError(ex, "EMSRepository.SendReceiverInforAsync failed");
        }

        return res;
    }
}
