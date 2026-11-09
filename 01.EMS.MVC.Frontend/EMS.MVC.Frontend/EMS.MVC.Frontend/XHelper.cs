using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using EMS.MVC.Frontend.Controllers;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
using System.Linq;
using System.Web;
using System.Net.Http.Headers;

namespace EMS.MVC.Frontend
{
    public class XHelper
    {
        #region Kết nối Gateway (API EMSOne)
        public static string _gatewayURL = string.Empty;

        public static string _pathRequestSupport = string.Empty;

        public static string GatewayUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_gatewayURL))
                    _gatewayURL = ConfigurationManager.AppSettings["GATEWAY_URL"];
                return _gatewayURL;
            }

            set
            {
                _gatewayURL = value;
            }
        }

        static HttpClient _clientGateway = null;

        public static ZResponse Invoke(ZRequest requestBody)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (_clientGateway == null)
                {
                    _clientGateway = new HttpClient();
                    _clientGateway.BaseAddress = new Uri(GatewayUrl);
                    _clientGateway.DefaultRequestHeaders.Accept.Clear();
                    _clientGateway.DefaultRequestHeaders.Add("DeviceName", "WEBFE");
                    _clientGateway.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    _clientGateway.Timeout = TimeSpan.FromMinutes(2);
                    //ServicePointManager.FindServicePoint(new Uri(GatewayUrl)).ConnectionLeaseTimeout = 5 * 60 * 1000;
                }

                var req = new ZRequest()
                {
                    Code = requestBody.Code,
                    Data = requestBody.Data,
                    //Signature = Security.RSAEncrypt(requestBody.Data, Security.PrivateKey),
                    Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                };

                HttpResponseMessage response = _clientGateway.PostAsJsonAsync("Execute", req).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<ZResponse>().Result;
                }
                else
                {
                    LogToFile(LogFileType.TRACE, string.Format("XHelper.Invoke: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                    switch (response.StatusCode)
                    {
                        default:
                            res.Code = "99";
                            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
                LogToFile(LogFileType.EXCEPTION, string.Format("Helpers.Invoke: request={0}||exception={1}", JsonConvert.SerializeObject(requestBody), ex.Message));
            }
            return res;
        }

        public static ZResponse InvokeMultipart(HttpPostedFileBase file, string code, string data)
        {
            try
            {
                if(file == null || file.ContentLength <= 0)
                {
                    return new ZResponse
                    {
                        Code = "99",
                        Message = "File không hợp lệ"
                    };
                }

                using(var client = new HttpClient())
                using(var form = new MultipartFormDataContent())
                {
                    client.BaseAddress = new Uri(GatewayUrl);

                    var streamContent = new StreamContent(file.InputStream);

                    if(!string.IsNullOrWhiteSpace(file.ContentType))
                    {
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    }

                    form.Add(streamContent, "file", file.FileName);
                    form.Add(new StringContent(code ?? "", Encoding.UTF8), "Code");
                    form.Add(new StringContent(data ?? "", Encoding.UTF8), "Data");

                    HttpResponseMessage response;

                    response = client.PostAsync("ExecuteFile", form).Result;

                    var responseText = response.Content.ReadAsStringAsync().Result;

                    if(string.IsNullOrWhiteSpace(responseText))
                    {
                        return new ZResponse
                        {
                            Code = "99",
                            Message = "Core không trả dữ liệu"
                        };
                    }

                    var result = JsonConvert.DeserializeObject<ZResponse>(responseText);
                    if(result == null)
                    {
                        return new ZResponse
                        {
                            Code = "99",
                            Message = "Không parse được response từ Gateway"
                        };
                    }

                    return result;
                }
            }
            catch(Exception ex)
            {
                return new ZResponse
                {
                    Code = "99",
                    Message = ex.ToString()
                };
            }
        }
        #endregion

        #region Mã hóa AES
        private static readonly string keyString = "12345678901234567890123456789012";

        // ✅ IV: 16 ký tự tương ứng 128-bit
        private static readonly string ivString = "1234567890123456";

        public static string Encrypt(string plainText)
        {
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            byte[] iv = Encoding.UTF8.GetBytes(ivString);

            using(Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using(MemoryStream ms = new MemoryStream())
                {
                    using(ICryptoTransform encryptor = aes.CreateEncryptor())
                    using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using(StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    return BitConverter.ToString(ms.ToArray()).Replace("-", "").ToLower();
                }
            }
        }

        public static string Decrypt(string hexCipherText)
        {
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            byte[] iv = Encoding.UTF8.GetBytes(ivString);

            // Convert hex -> byte[]
            byte[] cipherBytes = Enumerable.Range(0, hexCipherText.Length / 2)
                .Select(x => Convert.ToByte(hexCipherText.Substring(x * 2, 2), 16))
                .ToArray();

            using(Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using(MemoryStream ms = new MemoryStream(cipherBytes))
                using(ICryptoTransform decryptor = aes.CreateDecryptor())
                using(CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using(StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        #endregion;

        #region Kết nối SyncGateWay (API đối tác)
        //static string _syncGatewayURL = string.Empty;

        //static HttpClient _clientSyncGateway = null;

        //public static string SyncGatewayUrl
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_syncGatewayURL))
        //            _syncGatewayURL = ConfigurationManager.AppSettings["SYNC_GATEWAY_URL"];
        //        return _syncGatewayURL;
        //    }

        //    set
        //    {
        //        _syncGatewayURL = value;
        //    }
        //}

        //public static ZResponse InvokeSyncGateway(ZRequest requestBody)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        if (_clientSyncGateway == null)
        //        {
        //            _clientSyncGateway = new HttpClient();
        //            _clientSyncGateway.BaseAddress = new Uri(SyncGatewayUrl);
        //            _clientSyncGateway.DefaultRequestHeaders.Accept.Clear();
        //            _clientSyncGateway.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //            _clientSyncGateway.Timeout = TimeSpan.FromMinutes(2);
        //        }

        //        var req = new ZRequest()
        //        {
        //            Code = requestBody.Code,
        //            Data = requestBody.Data,
        //            Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
        //        };

        //        HttpResponseMessage response = _clientSyncGateway.PostAsJsonAsync("Execute", req).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            res = response.Content.ReadAsAsync<ZResponse>().Result;
        //        }
        //        else
        //        {
        //            LogToFile(LogFileType.TRACE, string.Format("XHelper.InvokeSyncGateway: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
        //            switch (response.StatusCode)
        //            {
        //                default:
        //                    res.Code = "99";
        //                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
        //        LogToFile(LogFileType.EXCEPTION, string.Format("Helpers.InvokeSyncGateway: request={0}||exception={1}", JsonConvert.SerializeObject(requestBody), ex.Message + _clientSyncGateway.BaseAddress.ToString()));
        //    }
        //    return res;
        //}
        private static readonly object _syncGatewayLock = new object();
        private static string _syncGatewayURL = null;
        private static HttpClient _clientSyncGateway = null;

        public static string SyncGatewayUrl
        {
            get
            {
                // Luôn trim + chỉ cache khi có giá trị hợp lệ
                if(string.IsNullOrWhiteSpace(_syncGatewayURL))
                {
                    _syncGatewayURL = (ConfigurationManager.AppSettings["SYNC_GATEWAY_URL"] ?? "").Trim();
                }
                return _syncGatewayURL;
            }
            set { _syncGatewayURL = value; }
        }

        private static Uri GetSyncGatewayBaseUri()
        {
            var raw = (SyncGatewayUrl ?? "").Trim();
            if(string.IsNullOrWhiteSpace(raw))
                throw new InvalidOperationException("Missing/empty appSettings key: SYNC_GATEWAY_URL");

            // Bắt buộc absolute URI (khuyến nghị để tránh lỗi trên IIS)
            if(!raw.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !raw.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"SYNC_GATEWAY_URL must start with http:// or https://. Current=[{raw}]");

            // Chuẩn hóa dấu /
            return new Uri(raw.TrimEnd('/') + "/");
        }

        private static void EnsureSyncGatewayClient()
        {
            if(_clientSyncGateway != null) return;

            lock(_syncGatewayLock)
            {
                if(_clientSyncGateway != null) return;

                var baseUri = GetSyncGatewayBaseUri();

                // Tạo ở biến local, cấu hình xong rồi mới assign -> tránh race condition IIS
                var client = new HttpClient
                {
                    BaseAddress = baseUri,
                    Timeout = TimeSpan.FromMinutes(2)
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                _clientSyncGateway = client;
            }
        }

        public static ZResponse InvokeSyncGateway(ZRequest requestBody)
        {
            var res = new ZResponse();

            try
            {
                EnsureSyncGatewayClient();

                var req = new ZRequest
                {
                    Code = requestBody.Code,
                    Data = requestBody.Data,
                    Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                };

                // Dùng absolute URI để không phụ thuộc BaseAddress trong thời điểm chạy
                var execUri = new Uri(_clientSyncGateway.BaseAddress, "Execute");

                var response = _clientSyncGateway.PostAsJsonAsync(execUri, req).Result;

                if(response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<ZResponse>().Result;
                    return res;
                }

                // Log nội dung lỗi an toàn hơn (không ép ReadAsAsync<object>())
                var err = response.Content.ReadAsStringAsync().Result;

                LogToFile(
                    LogFileType.TRACE,
                    $"XHelper.InvokeSyncGateway: request={JsonConvert.SerializeObject(req)}||" +
                    $"status={(int)response.StatusCode} {response.ReasonPhrase}||content={err}"
                );

                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                return res;
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";

                // Tránh NullReference nếu client/base chưa kịp init
                var baseAddr = _clientSyncGateway?.BaseAddress?.ToString() ?? "(null)";
                LogToFile(
                    LogFileType.EXCEPTION,
                    $"Helpers.InvokeSyncGateway: request={JsonConvert.SerializeObject(requestBody)}||exception={ex}||baseAddress={baseAddr}||SYNC_GATEWAY_URL=[{SyncGatewayUrl}]"
                );

                return res;
            }
        }
        #endregion

        #region Ghi log
        public static string _logPath = string.Empty;

        public static string LogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logPath))
                    _logPath = ConfigurationManager.AppSettings["LOG_PATH"];
                return _logPath;
            }

            set
            {
                _logPath = value;
            }
        }

        public static string pathRequestSupport
        {
            get
            {
                if (string.IsNullOrEmpty(_pathRequestSupport))
                    _pathRequestSupport = ConfigurationManager.AppSettings["IMAGE_URL"] + "Assets/RequestSupport/";
                return _pathRequestSupport;
            }

            set
            {
                _pathRequestSupport = value;
            }
        }

        public static void LogToFile(LogFileType logFileType, string message)
        {
            try
            {
                LoggerGeneral logger = new LoggerGeneral();
                message = DateTime.Now.ToString("HH:mm:ss") + " " + message;
                FormatterGeneral formatter = new FormatterGeneral(message);
                string logPath = string.Empty;
                logPath = LogPath + @"\" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HH");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                switch (logFileType)
                {
                    case LogFileType.TRACE:
                        logPath = logPath + @"\TRACE.log";
                        break;
                    case LogFileType.MESSAGE:
                        logPath = logPath + @"\MESSAGE.log";
                        break;
                    default:
                        logPath = logPath + @"\EXCEPTION.log";
                        break;
                }
                logger.Write(formatter, logPath);
            }
            catch
            {

            }
        }
        #endregion

        #region Session
        public static SessionController Session = new SessionController();

        public const string SessionEmployee = "Employee";
        public const string SessionZalo = "Zalo";
        public const string SessionFacebook = "Facebook";
        public const string SessionTiktok = "Tiktok";
        public const string PasswordDefault = "123456";
        #endregion

        #region URL Image
        static string _urlImage = string.Empty;
        static string _pathImage = string.Empty;
        public static string URLImage
        {
            get
            {
                if (string.IsNullOrEmpty(_urlImage))
                    _urlImage = ConfigurationManager.AppSettings["IMAGE_URL"];
                return _urlImage;
            }

            set
            {
                _urlImage = value;
            }
        }
        public static string pathImages
        {
            get
            {
                if (string.IsNullOrEmpty(_pathImage))
                    _pathImage = ConfigurationManager.AppSettings["IMAGE_URL"] + "Assets/Images/";
                return _pathImage;
            }

            set
            {
                _pathImage = value;
            }
        }
        #endregion

        #region License của JSpreadSheet
        public static string _JExcelLicense = string.Empty;

        public static string JExcelLicense
        {
            get
            {
                if (string.IsNullOrEmpty(_JExcelLicense))
                    _JExcelLicense = ConfigurationManager.AppSettings["JEXCEL_LICENSE"];
                return _JExcelLicense;
            }

            set
            {
                _JExcelLicense = value;
            }
        }
        #endregion

        #region Secret ReturnHM
        public static string _SecretReturnHM = string.Empty;

        public static string SecretReturnHM
        {
            get
            {
                if(string.IsNullOrEmpty(_SecretReturnHM))
                    _SecretReturnHM = ConfigurationManager.AppSettings["SECRET_RETURN_HM"];
                return _SecretReturnHM;
            }

            set
            {
                _SecretReturnHM = value;
            }
        }
        #endregion

        #region Kết nối Zalo (API Zalo)
        static string _zaloConnectURL = string.Empty;

        static string _zaloSecretKey = string.Empty;

        static string _zaloAppID = string.Empty;

        public static string ZaloConnectURL
        {
            get
            {
                if (string.IsNullOrEmpty(_zaloConnectURL))
                    _zaloConnectURL = string.Format("{0}?app_id={1}&redirect_uri={2}", ConfigurationManager.AppSettings["ZALO_CONNNECT_URL"], ConfigurationManager.AppSettings["ZALO_APP_ID"], ConfigurationManager.AppSettings["ZALO_REDIRECT_URI"]);
                return _zaloConnectURL;
            }

            set
            {
                _zaloConnectURL = value;
            }
        }

        public static string ZaloSecretKey
        {
            get
            {
                if (string.IsNullOrEmpty(_zaloSecretKey))
                    _zaloSecretKey = ConfigurationManager.AppSettings["ZALO_SECRET_KEY"];
                return _zaloSecretKey;
            }

            set
            {
                _zaloSecretKey = value;
            }
        }

        public static string ZaloAppID
        {
            get
            {
                if (string.IsNullOrEmpty(_zaloAppID))
                    _zaloAppID = ConfigurationManager.AppSettings["ZALO_APP_ID"];
                return _zaloAppID;
            }

            set
            {
                _zaloAppID = value;
            }
        }

        public static ZResponse InvokeZaloV1(ZRequest request, string Method = "POST", string AccessToken = "")
        {
            ZResponse res = new ZResponse();
            HttpResponseMessage response = null;
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    _client.BaseAddress = new Uri(request.Code);
                    _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    _client.DefaultRequestHeaders.Add("secret_key", ZaloSecretKey);
                    _client.DefaultRequestHeaders.Add("access_token", AccessToken);

                    switch (Method)
                    {
                        case "GET":
                            response = _client.GetAsync(request.Code).Result;
                            break;
                        case "POST":
                            response = _client.PostAsJsonAsync(request.Code, JsonConvert.DeserializeObject<object>(request.Data)).Result;
                            break;
                        default:
                            break;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic ZaloResponse = response.Content.ReadAsAsync<object>().Result;
                        if (ZaloResponse.error == 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
                            res.Data = JsonConvert.SerializeObject(ZaloResponse);
                        }
                        else
                        {
                            res.Code = ZaloResponse.error;
                            res.Message = ZaloResponse.message != null ? ZaloResponse.message : ZaloResponse.error_reason;
                            LogToFile(LogFileType.MESSAGE, string.Format("Helpers.InvokeZaloV1: request={0} || response={1}", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ZaloResponse)));
                        }
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối API Zalo, vui lòng liên hệ quản trị.";
                                break;
                        }
                        LogToFile(LogFileType.MESSAGE, string.Format("Helpers.InvokeZaloV1: request={0} || response={1}", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response)));
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
                LogToFile(LogFileType.EXCEPTION, "Helpers.InvokeZaloV1: " + ex.Message);
            }
            return res;
        }

        public static ZResponse InvokeZaloV2(ZRequest request, string AccessToken = "")
        {
            ZResponse res = new ZResponse();
            try
            {
                using (WebClient client = new WebClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.Headers.Clear();
                    client.Headers.Add("secret_key", ZaloSecretKey);
                    if (AccessToken != "")
                    {
                        client.Headers.Add("access_token", AccessToken);
                    }
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string result = client.UploadString(request.Code, request.Data);
                    dynamic ZaloResponse = JsonConvert.DeserializeObject<object>(result);
                    if (ZaloResponse.error == 0 || ZaloResponse.error == null)
                    {
                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = result;
                    }
                    else
                    {
                        res.Code = ZaloResponse.error;
                        res.Message = ZaloResponse.message != null ? ZaloResponse.message : ZaloResponse.error_reason;
                        LogToFile(LogFileType.MESSAGE, string.Format("Helpers.InvokeZaloV2: request={0} || response={1}", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ZaloResponse)));
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
                LogToFile(LogFileType.EXCEPTION, "Helpers.InvokeZaloV2: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Kết nối Facebook (API Facebook)
        static string _facebookAppID = string.Empty;
        static string _facebookAppSecret = string.Empty;
        static string _facebookApiURL = string.Empty;
        static string _facebookRedirectURL = string.Empty;
        static string _facebookConnectURL = string.Empty;
        static string _facebookscope = "public_profile,email,user_link,pages_show_list,pages_read_engagement,pages_manage_metadata,pages_read_user_content,pages_manage_posts,pages_manage_engagement,pages_messaging,pages_manage_instant_articles";
        public static string FacebookConnectURL
        {
            get
            {
                if (string.IsNullOrEmpty(_facebookConnectURL))
                    _facebookConnectURL = string.Format("{0}?client_id={1}&redirect_uri={2}&scope={3}&path={4}&ResponseType={5}", ConfigurationManager.AppSettings["FB_CONNNECT_URL"], ConfigurationManager.AppSettings["FB_APP_ID"], ConfigurationManager.AppSettings["FB_REDIRECT_URL"] + "Channel/AddConnectPage", _facebookscope, "oauth", "code");
                return _facebookConnectURL;
            }

            set
            {
                _facebookConnectURL = value;
            }
        }
        public static string FacebookAppID
        {
            get
            {
                if (string.IsNullOrEmpty(_facebookAppID))
                    _facebookAppID = ConfigurationManager.AppSettings["FB_APP_ID"];
                return _facebookAppID;
            }

            set
            {
                _facebookAppID = value;
            }
        }
        public static string FacebookAppSecret
        {
            get
            {
                if (string.IsNullOrEmpty(_facebookAppSecret))
                    _facebookAppSecret = ConfigurationManager.AppSettings["FB_APP_SECRET"];
                return _facebookAppSecret;
            }

            set
            {
                _facebookAppSecret = value;
            }
        }
        public static string FacebookApiURL
        {
            get
            {
                if (string.IsNullOrEmpty(_facebookApiURL))
                    _facebookApiURL = ConfigurationManager.AppSettings["FB_API_URL"];
                return _facebookApiURL;
            }

            set
            {
                _facebookApiURL = value;
            }
        }
        public static string FacebookRedirectURL
        {
            get
            {
                if (string.IsNullOrEmpty(_facebookRedirectURL))
                    _facebookRedirectURL = ConfigurationManager.AppSettings["FB_REDIRECT_URL"];
                return _facebookRedirectURL;
            }

            set
            {
                _facebookRedirectURL = value;
            }
        }
        public static ZResponse InvokeFacebook(ZRequest request, string Method = "POST")
        {
            ZResponse res = new ZResponse();
            HttpResponseMessage response = null;
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                    var apiURL = FacebookApiURL + request.Code;
                    _client.BaseAddress = new Uri(apiURL);
                    _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    switch (Method)
                    {
                        case "GET":
                            response = _client.GetAsync(apiURL).Result;
                            break;
                        case "POST":
                            response = _client.PostAsJsonAsync(apiURL, JsonConvert.DeserializeObject<object>(request.Data)).Result;
                            break;
                        case "DELETE":
                            response = _client.DeleteAsync(apiURL).Result;
                            break;
                        default:
                            break;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result);
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối API Facebook, vui lòng liên hệ quản trị.";
                                break;
                        }
                        LogToFile(LogFileType.MESSAGE, "Helpers.InvokeFacebook: " + "request= " + JsonConvert.SerializeObject(request) + " response= " + JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
                LogToFile(LogFileType.EXCEPTION, "Helpers.InvokeFacebook: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region URL Path Facebook
        static string _pathFabSticker = string.Empty;
        static string _pathFabFile = string.Empty;
        public static string pathFabSticker
        {
            get
            {
                if (string.IsNullOrEmpty(_pathFabSticker))
                    _pathFabSticker = ConfigurationManager.AppSettings["IMAGE_URL"] + "Assets/Facebook/Sticker/";
                return _pathFabSticker;
            }

            set
            {
                _pathFabSticker = value;
            }
        }
        public static string pathFabFile
        {
            get
            {
                if (string.IsNullOrEmpty(_pathFabFile))
                    _pathFabFile = ConfigurationManager.AppSettings["IMAGE_URL"] + "Assets/Facebook/File/";
                return _pathFabFile;
            }

            set
            {
                _pathFabFile = value;
            }
        }
        #endregion

        #region Kết nối Tiktok (API Tiktok)
        static string _tiktokServiceID = string.Empty;
        static string _tiktokAppSecret = string.Empty;
        static string _tiktokAppKey = string.Empty;
        static string _tiktokApiURL = string.Empty;
        static string _tiktokRedirectURL = string.Empty;
        static string _tiktokServiceURL = string.Empty;
        static string _tiktokConnectURL = string.Empty;
        public static string TiktokServiceURL
        {
            get
            {
                if (string.IsNullOrEmpty(_tiktokServiceURL))
                    _tiktokServiceURL = string.Format("{0}?service_id={1}&state={2}", ConfigurationManager.AppSettings["TIKTOK_SERVICE_URL"], ConfigurationManager.AppSettings["TIKTOK_SERVICE_ID"], ConfigurationManager.AppSettings["TIKTOK_STATE"]);
                return _tiktokServiceURL;
            }

            set
            {
                _tiktokServiceURL = value;
            }
        }
        public static string TiktokServiceID
        {
            get
            {
                if (string.IsNullOrEmpty(_tiktokServiceID))
                    _tiktokServiceID = ConfigurationManager.AppSettings["TIKTOK_SERVICE_ID"];
                return _tiktokServiceID;
            }

            set
            {
                _tiktokServiceID = value;
            }
        }
        public static string TiktokAppKey
        {
            get
            {
                if (string.IsNullOrEmpty(_tiktokAppKey))
                    _tiktokAppKey = ConfigurationManager.AppSettings["TIKTOK_APP_KEY"];
                return _tiktokAppKey;
            }

            set
            {
                _tiktokAppKey = value;
            }
        }
        public static string TiktokAppSecret
        {
            get
            {
                if (string.IsNullOrEmpty(_tiktokAppSecret))
                    _tiktokAppSecret = ConfigurationManager.AppSettings["TIKTOK_SECRET_KEY"];
                return _tiktokAppSecret;
            }

            set
            {
                _tiktokAppSecret = value;
            }
        }
        public static string TiktokConnectURL
        {
            get
            {
                if (string.IsNullOrEmpty(_tiktokConnectURL))
                    _tiktokConnectURL = ConfigurationManager.AppSettings["TIKTOK_CONNNECT_URL"];
                return _tiktokConnectURL;
            }

            set
            {
                _tiktokConnectURL = value;
            }
        }
        public static string TiktokApiURL
        {
            get
            {
                if (string.IsNullOrEmpty(_tiktokApiURL))
                    _tiktokApiURL = ConfigurationManager.AppSettings["TIKTOK_API_URL"];
                return _tiktokApiURL;
            }

            set
            {
                _tiktokApiURL = value;
            }
        }
        public static string TiktokRedirectURL
        {
            get
            {
                if (string.IsNullOrEmpty(_tiktokRedirectURL))
                    _tiktokRedirectURL = ConfigurationManager.AppSettings["TIKTOK_REDIRECT_URL"];
                return _tiktokRedirectURL;
            }

            set
            {
                _tiktokRedirectURL = value;
            }
        }
        public static string TiktokSignature(string path, Dictionary<string, string> queries, string secret)
        {
            var keys = new List<string>(queries.Keys);
            keys.Sort();
            var input = new StringBuilder(path);
            foreach (var key in keys)
            {
                input.Append(key);
                input.Append(queries[key]);
            }
            input.Insert(0, secret);
            input.Append(secret);
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var digest = hmac.ComputeHash(Encoding.UTF8.GetBytes(input.ToString()));
            return BitConverter.ToString(digest).Replace("-", string.Empty).ToLower();
        }
        public static ZResponse InvokeTiktok(ZRequest request, string Method = "POST", string TypeAPI = "", string AccessToken = "")
        {
            ZResponse res = new ZResponse();
            HttpResponseMessage response = null;
            try
            {
                using (HttpClient _client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

                    var apiURL = (TypeAPI == "" ? TiktokApiURL : TiktokConnectURL) + request.Code;
                    _client.BaseAddress = new Uri(apiURL);
                    _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    _client.DefaultRequestHeaders.TryAddWithoutValidation("x-tts-access-token", AccessToken);
                    switch (Method)
                    {
                        case "GET":
                            response = _client.GetAsync(apiURL).Result;
                            break;
                        case "POST":
                            response = _client.PostAsJsonAsync(apiURL, JsonConvert.DeserializeObject<object>(request.Data)).Result;
                            break;
                        case "DELETE":
                            response = _client.DeleteAsync(apiURL).Result;
                            break;
                        default:
                            break;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result);
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối API Tiktok, vui lòng liên hệ quản trị.";
                                break;
                        }
                        LogToFile(LogFileType.MESSAGE, "Helpers.InvokeTiktok: " + "request= " + JsonConvert.SerializeObject(request) + " response= " + JsonConvert.SerializeObject(response));
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
                LogToFile(LogFileType.EXCEPTION, "Helpers.InvokeTiktok: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Version Javascript
        public static string JavaScriptVersion = "4";
        #endregion
    }
}