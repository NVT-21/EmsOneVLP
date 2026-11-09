using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace EMS.API.Gateway
{
    public static class XHelper
    {
        #region URL Image
        public static string _imageURL = string.Empty;
        public static string ImageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_imageURL))
                    _imageURL = ConfigurationManager.AppSettings["IMAGE_URL"];
                return _imageURL;
            }

            set
            {
                _imageURL = value;
            }
        }
        #endregion

        #region Lấy giá trị DeviceName ở Header
        public static string _deviceName = string.Empty;

        public static string DeviceName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    _deviceName = "";
                }
                else
                {
                    _deviceName = HttpContext.Current.Request.Headers["DeviceName"];
                }

                return _deviceName;
            }
            set
            {
                _deviceName = value;
            }
        }
        #endregion

        #region Kết nối Core (EMSOne)
        public static string _coreURL = string.Empty;

        public static string CoreUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_coreURL))
                    _coreURL = ConfigurationManager.AppSettings["CORE_URL"];
                return _coreURL;
            }

            set
            {
                _coreURL = value;
            }
        }

        //static HttpClient _clientCore = null;

        public static ZResponse Invoke(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient ClientCore = new HttpClient())
                {
                    ClientCore.BaseAddress = new Uri(CoreUrl);
                    ClientCore.DefaultRequestHeaders.Accept.Clear();
                    ClientCore.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    ClientCore.DefaultRequestHeaders.Add("DeviceName", DeviceName);

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpResponseMessage response = null;
                    switch (method)
                    {
                        case "GET":
                            response = ClientCore.GetAsync(requestUri).Result;
                            break;
                        case "POST":
                            response = ClientCore.PostAsJsonAsync(requestUri, value).Result;
                            break;
                        default:
                            break;
                    }
                    var test = response.Content.ReadAsStringAsync(); ;
                    if (response.IsSuccessStatusCode)
                    {
                        res = response.Content.ReadAsAsync<ZResponse>().Result;
                        //LogToFile(LogFileType.TRACE, string.Format("XHelper.Invoke: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(res), JsonConvert.SerializeObject(response.Content.ReadAsStringAsync().Result)));
                    }
                    else
                    {
                        LogToFile(LogFileType.EXCEPTION, string.Format("XHelper.Invoke: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi " + response.StatusCode + ", vui lòng liên hệ quản trị.";
                                res.Data = JsonConvert.SerializeObject(response);
                                break;
                        }
                    }
                }

                //if (_clientCore == null)
                //{
                //    Uri BaseAddress = new Uri(CoreUrl);
                //    _clientCore = new HttpClient();
                //    _clientCore.BaseAddress = BaseAddress;
                //    _clientCore.DefaultRequestHeaders.Accept.Clear();
                //    _clientCore.DefaultRequestHeaders.Add("DeviceName", DeviceName);
                //    _clientCore.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //    _clientCore.Timeout = TimeSpan.FromMinutes(2);
                //    //client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Security.EncodeBase64("lms:lms")));
                //    //ServicePointManager.FindServicePoint(BaseAddress).ConnectionLeaseTimeout = 5 * 60 * 1000;
                //}

                //HttpResponseMessage response = null;
                //switch (method)
                //{
                //    case "GET":
                //        response = _clientCore.GetAsync(requestUri).Result;
                //        break;
                //    case "POST":
                //        response = _clientCore.PostAsJsonAsync(requestUri, value).Result;
                //        break;
                //    default:
                //        break;
                //}

                //if (response.IsSuccessStatusCode)
                //{
                //    res = response.Content.ReadAsAsync<ZResponse>().Result;
                //}
                //else
                //{
                //    LogToFile(LogFileType.TRACE, string.Format("XHelper.Invoke: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                //    switch (response.StatusCode)
                //    {
                //        default:
                //            res.Code = "99";
                //            res.Message = "Lỗi " + response.StatusCode + ", vui lòng liên hệ quản trị.";
                //            res.Data = JsonConvert.SerializeObject(response);
                //            break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                LogToFile(LogFileType.EXCEPTION, string.Format("Helpers.Invoke: request={0}||exception={1}", JsonConvert.SerializeObject(value), ex.Message));
            }
            return res;
        }

        public static ZResponse InvokeMultipart(string method, string url, HttpPostedFile file, string code, string data)
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
                    client.BaseAddress = new Uri(CoreUrl);

                    var streamContent = new StreamContent(file.InputStream);

                    if(!string.IsNullOrWhiteSpace(file.ContentType))
                    {
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    }

                    form.Add(streamContent, "file", file.FileName);
                    form.Add(new StringContent(code ?? "", Encoding.UTF8), "Code");
                    form.Add(new StringContent(data ?? "", Encoding.UTF8), "Data");

                    HttpResponseMessage response;

                    if(string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase))
                    {
                        response = client.PostAsync(url, form).Result;
                    }
                    else
                    {
                        throw new NotSupportedException("Hiện chỉ hỗ trợ POST");
                    }

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
                            Message = "Không parse được response từ Core"
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

        #region Ghi log
        public static string _logPath = string.Empty;

        static string LogPath
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

        public static string _allowWriteLog = string.Empty;

        static string AllowWriteLog
        {
            get
            {
                if (string.IsNullOrEmpty(_allowWriteLog))
                    _allowWriteLog = ConfigurationManager.AppSettings["ALLOW_WRITE_LOG"];
                return _allowWriteLog;
            }

            set
            {
                _allowWriteLog = value;
            }
        }

        public static void LogToFileV2(string message)
        {
            try
            {
                if (AllowWriteLog == "0" || AllowWriteLog == null)
                {
                    return;
                }
                LoggerGeneral logger = new LoggerGeneral();
                message = DateTime.Now.ToString("HH:mm:ss") + " " + message;
                FormatterGeneral formatter = new FormatterGeneral(message);
                string logPath = string.Empty;
                logPath = LogPath + @"\" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HH");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                logPath = logPath + @"\ALLOW_WRITE_LOG.log";
                logger.Write(formatter, logPath);
            }
            catch
            {

            }
        }
        #endregion

        public static string GetQueryString(Dictionary<string, string> queryParams)
        {
            NameValueCollection queryCollection = System.Web.HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in queryParams)
            {
                queryCollection.Add(param.Key, param.Value);
            }
            return "?" + queryCollection.ToString();
        }

        #region BlockSqlInjection
        public static bool IsSqlInjection(string input)
        {
            if(string.IsNullOrWhiteSpace(input)) return false;

            // Chuyển về lowercase để kiểm tra từ khoá
            string lower = input.ToLower();

            // Regex hoặc từ khóa đơn giản
            string[] dangerousPatterns = new[]
            {
                "'", "\"", "--", ";", "/*", "*/", "xp_", "exec", "union", "select", "insert", "drop", "update", "delete", "or 1=1", "and 1=1"
            };

            foreach(var pattern in dangerousPatterns)
            {
                if(lower.Contains(pattern))
                    return true;
            }

            return false;
        }
        #endregion
    }
}