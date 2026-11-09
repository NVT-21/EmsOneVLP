using System.Net;
using System.Net.Http.Headers;
using System.Text;
using EMS.TransportWorkerService.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EMS.TransportWorkerService;

public class APILibraryRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<APILibraryRepository> _logger;

    public APILibraryRepository(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<APILibraryRepository> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ZResponse> InvokeEMSAsync(ZEMSRequest req, CancellationToken cancellationToken = default)
    {
        var res = new ZResponse();

        try
        {
            using var client = _httpClientFactory.CreateClient("EMSClient");

            client.BaseAddress = new Uri(req.RequestURL);

            if (!string.IsNullOrWhiteSpace(req.APIKeyValue))
            {
                client.DefaultRequestHeaders.Remove(req.APIKeyName);
                client.DefaultRequestHeaders.Add(req.APIKeyName, req.APIKeyValue);
            }

            if (!string.IsNullOrWhiteSpace(req.AuthorizationValue))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(req.AuthorizationName, req.AuthorizationValue);
            }

            HttpResponseMessage response;

            switch (req.Method?.ToUpperInvariant())
            {
                case "GET":
                    response = await client.GetAsync(req.RequestURI, cancellationToken).ConfigureAwait(false);
                    break;

                case "POST":
                {
                    // EMS API kỳ vọng tên field giống Postman (PICKUP_CODE_API, LIST_ITEM, …).
                    // PostAsJsonAsync dùng System.Text.Json + camelCase → pICKUP_CODE_API, API trả -99.
                    var requestValue = req.ListValue ?? req.Value;
                    var json = JsonConvert.SerializeObject(requestValue);
                    using var content = new StringContent(json, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(req.RequestURI, content, cancellationToken).ConfigureAwait(false);
                    break;
                }

                default:
                    res.Code = "99";
                    res.Message = "Phương thức gọi API không hợp lệ.";
                    return res;
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                res.Code = "00";
                res.Data = responseContent;
                res.Message = "Thành công";
                return res;
            }
            var contentLog = string.Format(
                "APILibraryRepository.InvokeEMSAsync: url={0}||request={1}||statusCode={2}||response={3}",
                req.RequestURL + req.RequestURI,
                JsonConvert.SerializeObject(req.ListValue ?? req.Value),
                response.StatusCode,
                responseContent
            );

            _logger.LogWarning("{LogMessage}", contentLog);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    res.Code = "98";
                    res.Message = "Token hết hạn.";
                    break;

                default:
                    res.Code = "99";
                    res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                    break;
            }
        }
        catch (OperationCanceledException ex)
        {
            res.Code = "99";
            res.Message = "Yêu cầu HTTP bị hủy hoặc hết thời gian chờ (timeout / shutdown).";

            _logger.LogWarning(
                ex,
                "APILibraryRepository.InvokeEMSAsync canceled. Url={Url}",
                req.RequestURL + req.RequestURI);
        }
        catch (Exception ex)
        {
            res.Code = "99";
            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";

            _logger.LogError(
                ex,
                "APILibraryRepository.InvokeEMSAsync failed. Url={Url}||Request={Request}",
                req.RequestURL + req.RequestURI,
                JsonConvert.SerializeObject(req.ListValue ?? req.Value));
        }

        return res;
    }
}
