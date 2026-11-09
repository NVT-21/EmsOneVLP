using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace EMS.API.PartnerGateway
{
    public static class XHelper
    {
        #region Authorization Key
        public static string _emsoneAuthorizationKey = string.Empty;

        public static string EMSOneAuthorizationKey
        {
            get
            {
                if (string.IsNullOrEmpty(_emsoneAuthorizationKey))
                    _emsoneAuthorizationKey = ConfigurationManager.AppSettings["EMSONE_AUTHORIZATION_KEY"];
                return _emsoneAuthorizationKey;
            }

            set
            {
                _emsoneAuthorizationKey = value;
            }
        }
        #endregion

        #region Secret Key
        public static string _emsoneSecretKey = string.Empty;

        public static string EMSOneSecretKey
        {
            get
            {
                if (string.IsNullOrEmpty(_emsoneSecretKey))
                    _emsoneSecretKey = ConfigurationManager.AppSettings["EMSONE_SECRET_KEY"];
                return _emsoneSecretKey;
            }

            set
            {
                _emsoneSecretKey = value;
            }
        }
        #endregion

        #region Ghi log
        static string _logPath = string.Empty;

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
        #endregion

        #region Kết nối Core
        static HttpClient _clientCore = null;

        static string _coreURL = string.Empty;

        static string CoreUrl
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

        public static ZResponse Invoke(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (_clientCore == null)
                {
                    _clientCore = new HttpClient();
                    _clientCore.BaseAddress = new Uri(CoreUrl);
                    _clientCore.DefaultRequestHeaders.Accept.Clear();
                    _clientCore.DefaultRequestHeaders.Add("DeviceName", "PARTNER");
                    _clientCore.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("APIKey", APIKey);
                    //client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Security.EncodeBase64("lms:lms")));
                    //ServicePointManager.FindServicePoint(BaseAddress).ConnectionLeaseTimeout = 5 * 60 * 1000;
                }

                HttpResponseMessage response = null;
                switch (method)
                {
                    case "GET":
                        response = _clientCore.GetAsync(requestUri).Result;
                        break;
                    case "POST":
                        response = _clientCore.PostAsJsonAsync(requestUri, value).Result;
                        break;
                    default:
                        break;
                }

                if (response.IsSuccessStatusCode)
                {
                    var test = response.Content.ReadAsStringAsync().Result;
                    res = response.Content.ReadAsAsync<ZResponse>().Result;
                }
                else
                {
                    LogToFile(LogFileType.MESSAGE, string.Format("XHelper.Invoke: request={0} || response={1} || content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                    switch (response.StatusCode)
                    {
                        default:
                            res.Code = "99";
                            res.Message = "Lỗi " + response.StatusCode + ", vui lòng liên hệ quản trị.";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                LogToFile(LogFileType.EXCEPTION, string.Format("XHelper.Invoke: request={0} || exception={1} || Link={2}", JsonConvert.SerializeObject(value), ex.Message, CoreUrl+requestUri));
            }
            return res;
        }
        #endregion

        #region Kết nối SynGateWay
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
        //        LogToFile(LogFileType.EXCEPTION, string.Format("Helpers.InvokeSyncGateway: request={0}||exception={1}", JsonConvert.SerializeObject(requestBody), ex.Message));
        //    }
        //    return res;
        //}
        #endregion

        #region Kết nối EMS

        #region URL
        static string _emsMapURL = string.Empty;

        public static string EMSMapUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_emsMapURL))
                    _emsMapURL = ConfigurationManager.AppSettings["EMS_MAP_URL"];
                return _emsMapURL;
            }

            set
            {
                _emsMapURL = value;
            }
        }

        static string _emsMapURLGateway = string.Empty;

        public static string EMSMapUrlGateway
        {
            get
            {
                if(string.IsNullOrEmpty(_emsMapURLGateway))
                    _emsMapURLGateway = ConfigurationManager.AppSettings["EMS_API_GATEWAY_URL"];
                return _emsMapURLGateway;
            }

            set
            {
                _emsMapURLGateway = value;
            }
        }

        static string _emsLabelURL = string.Empty;

        public static string EMSLabelURL
        {
            get
            {
                if(string.IsNullOrEmpty(_emsLabelURL))
                    _emsLabelURL = ConfigurationManager.AppSettings["EMS_LABEL_URL"];
                return _emsLabelURL;
            }

            set
            {
                _emsLabelURL = value;
            }
        }
        #endregion

        #region Key
        static string _emsMapKey = string.Empty;
        static string EMSMapKey
        {
            get
            {
                if (string.IsNullOrEmpty(_emsMapKey))
                    _emsMapKey = ConfigurationManager.AppSettings["EMS_MAP_KEY"];
                return _emsMapKey;
            }

            set
            {
                _emsMapKey = value;
            }
        }

        static string _emsMapKeyGateway = string.Empty;
        static string EMSMapKeyGateway
        {
            get
            {
                if(string.IsNullOrEmpty(_emsMapKeyGateway))
                    _emsMapKey = ConfigurationManager.AppSettings["EMS_API_GATEWAY_KEY"];
                return _emsMapKey;
            }

            set
            {
                _emsMapKeyGateway = value;
            }
        }
        #endregion

        static HttpClient _clientEMSMapGateway = null;

        public static ZResponse EMSMapSearch(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                //var requestData = JsonConvert.DeserializeObject<ZEMSMapSearchRequest>(req.Data);
                if (_clientEMSMapGateway == null)
                {
                    
                    _clientEMSMapGateway = new HttpClient();
                    _clientEMSMapGateway.BaseAddress = new Uri(EMSMapUrl);
                    _clientEMSMapGateway.DefaultRequestHeaders.Accept.Clear();
                    _clientEMSMapGateway.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    _clientEMSMapGateway.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    _clientEMSMapGateway.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", EMSMapKey));
                }

                //HttpResponseMessage response = _clientEMSMapGateway.GetAsync(req.Code + requestData.AddressFull).Result;
                HttpResponseMessage response = null;
                switch (method)
                {
                    case "GET":
                        response = _clientEMSMapGateway.GetAsync(requestUri).Result;
                        break;
                    case "POST":
                        response = _clientEMSMapGateway.PostAsJsonAsync(requestUri, value).Result;
                        break;
                    default:
                        break;
                }

                if (response.IsSuccessStatusCode)
                {
                    res.Code = "00";
                    res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result);
                    res.Message = "Thành công";
                }
                else
                {
                    LogToFile(LogFileType.MESSAGE, string.Format("XHelper.EMSMapSearch: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
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
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                LogToFile(LogFileType.EXCEPTION, string.Format("XHelper.EMSMapSearch: request={0}||exception={1}", JsonConvert.SerializeObject(value), ex.Message));
            }
            return res;
        }
        static HttpClient _clientEMSMapGatewayMCS = null;
        public static ZResponse EMSMapSearch_MCS(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                var test = EMSMapUrlGateway;
                //var requestData = JsonConvert.DeserializeObject<ZEMSMapSearchRequest>(req.Data);
                if(_clientEMSMapGatewayMCS == null)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                           | SecurityProtocolType.Tls11
                           | SecurityProtocolType.Tls12
                           | SecurityProtocolType.Ssl3;
                    _clientEMSMapGatewayMCS = new HttpClient();
                    _clientEMSMapGatewayMCS.BaseAddress = new Uri(EMSMapUrlGateway);
                    _clientEMSMapGatewayMCS.DefaultRequestHeaders.Accept.Clear();
                    _clientEMSMapGatewayMCS.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    _clientEMSMapGatewayMCS.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    _clientEMSMapGatewayMCS.DefaultRequestHeaders.Add("API_KEY", EMSMapKeyGateway);
                    //requestUri += "?search";
                }
                //HttpResponseMessage response = _clientEMSMapGateway.GetAsync(req.Code + requestData.AddressFull).Result;
                HttpResponseMessage response = null;
                switch(method)
                {
                    case "GET":
                        response = _clientEMSMapGatewayMCS.GetAsync(requestUri).Result;
                        break;
                    case "POST":
                        response = _clientEMSMapGatewayMCS.PostAsJsonAsync(requestUri, value).Result;
                        break;
                    default:
                        break;
                }

                if(response.IsSuccessStatusCode)
                {
                    res.Code = "00";
                    res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result);
                    res.Message = "Thành công";
                }
                else
                {
                    LogToFile(LogFileType.MESSAGE, string.Format("XHelper.EMSMapSearch: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                    switch(response.StatusCode)
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
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                LogToFile(LogFileType.EXCEPTION, string.Format("XHelper.EMSMapSearch: request={0}||exception={1}", JsonConvert.SerializeObject(value), ex.Message));
            }
            return res;
        }


        static HttpClient _clientEMSLabel = null;
        public static ZResponse EMSLabel(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                //var requestData = JsonConvert.DeserializeObject<ZEMSMapSearchRequest>(req.Data);
                if(_clientEMSLabel == null)
                {
                    _clientEMSLabel = new HttpClient();
                    _clientEMSLabel.BaseAddress = new Uri(EMSLabelURL);
                    _clientEMSLabel.DefaultRequestHeaders.Accept.Clear();
                    _clientEMSLabel.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    _clientEMSLabel.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                    //requestUri += "?search";
                }
                //HttpResponseMessage response = _clientEMSMapGateway.GetAsync(req.Code + requestData.AddressFull).Result;
                HttpResponseMessage response = null;
                switch(method)
                {
                    case "GET":
                        response = _clientEMSLabel.GetAsync(requestUri).Result;
                        break;
                    case "POST":
                        response = _clientEMSLabel.PostAsJsonAsync(requestUri, value).Result;
                        break;
                    default:
                        break;
                }

                if(response.IsSuccessStatusCode)
                {
                    res.Code = "00";
                    res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result);
                    res.Message = "Thành công";
                }
                else
                {
                    LogToFile(LogFileType.MESSAGE, string.Format("XHelper.EMSMapSearch: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                    switch(response.StatusCode)
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
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                LogToFile(LogFileType.EXCEPTION, string.Format("XHelper.EMSMapSearch: request={0}||exception={1}", JsonConvert.SerializeObject(value), ex.Message));
            }
            return res;
        }
        #endregion
    }
}