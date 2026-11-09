using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Controllers;
using EMS.MVC.Backend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend
{
    public static class XHelper
    {
        public const string SessionUser = "UserB";




        static string _Email = string.Empty;
        static string _EmailPassword = string.Empty;



        static string _Nhan_vien_CSKH__c = string.Empty;
        public const string PasswordDefault = "123456";


        public static SessionController Session = new SessionController();




        public static string Nhan_vien_CSKH__c
        {
            get
            {
                if (string.IsNullOrEmpty(_Nhan_vien_CSKH__c))
                    _Nhan_vien_CSKH__c = ConfigurationManager.AppSettings["Nhan_vien_CSKH__c"];
                return _Nhan_vien_CSKH__c;
            }

            set
            {
                _Nhan_vien_CSKH__c = value;
            }
        }

        #region Kết nối Gateway (API EMSOne)
        static string _gatewayURL = string.Empty;

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
                    _clientGateway.DefaultRequestHeaders.Add("DeviceName", "WEBBE");
                    _clientGateway.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    _clientGateway.Timeout = TimeSpan.FromMinutes(2);
                    //ServicePointManager.FindServicePoint(BaseAddress).ConnectionLeaseTimeout = 5 * 60 * 1000;
                }

                var req = new ZRequest()
                {
                    Code = requestBody.Code,
                    Data = requestBody.Data,
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
        #endregion

        #region Kết nối SyncGateWay (API đối tác)
        static string _syncGatewayURL = string.Empty;

        public static string SyncGatewayUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_syncGatewayURL))
                    _syncGatewayURL = ConfigurationManager.AppSettings["SYNC_GATEWAY_URL"];
                return _syncGatewayURL;
            }

            set
            {
                _syncGatewayURL = value;
            }
        }

        static HttpClient _clientSyncGateway = null;

        public static ZResponse InvokeSyncGateway(ZRequest requestBody)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (_clientSyncGateway == null)
                {
                    _clientSyncGateway = new HttpClient();
                    _clientSyncGateway.BaseAddress = new Uri(SyncGatewayUrl);
                    _clientSyncGateway.DefaultRequestHeaders.Accept.Clear();
                    _clientSyncGateway.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                }

                var req = new ZRequest()
                {
                    Code = requestBody.Code,
                    Data = requestBody.Data,
                    Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                };

                HttpResponseMessage response = _clientSyncGateway.PostAsJsonAsync("Execute", req).Result;

                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsAsync<ZResponse>().Result;
                }
                else
                {
                    LogToFile(LogFileType.TRACE, string.Format("XHelper.InvokeSyncGateway: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
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
                LogToFile(LogFileType.EXCEPTION, string.Format("Helpers.InvokeSyncGateway: request={0}||exception={1}", JsonConvert.SerializeObject(requestBody), ex.Message));
            }
            return res;
        }
        #endregion

        #region Ghi log
        static string _logPath = string.Empty;
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

        #region Kết nối Image
        static string _urlImage = string.Empty;
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

        #endregion

        #region Đường dẫn
        static string _pathImage = string.Empty;

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

        static string _pathRequestSupport = string.Empty;
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

        static string _Ems247Url = string.Empty;
        public static string Ems247Url
        {
            get
            {
                if (string.IsNullOrEmpty(_Ems247Url))
                    _Ems247Url = ConfigurationManager.AppSettings["EMS247_URL"];
                return _Ems247Url;
            }

            set
            {
                _Ems247Url = value;
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
        public static string Email
        {
            get
            {
                if (string.IsNullOrEmpty(_Email))
                    _Email = ConfigurationManager.AppSettings["EMAIL"];
                return _Email;
            }

            set
            {
                _Email = value;
            }
        }
        public static string EmailPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_EmailPassword))
                    _EmailPassword = ConfigurationManager.AppSettings["EMAIL_PASSWORD"];
                return _EmailPassword;
            }

            set
            {
                _EmailPassword = value;
            }
        }


        //public static List<SelectListItem> GetsEmployeeLevel()
        //{
        //    List<SelectListItem> listItem = new List<SelectListItem>();

        //    var res = XHelper.Invoke(new ZRequest()
        //    {
        //        Code = "EMPLOYEE_LEVEL_GETS",
        //        Data = ""
        //    });
        //    if (res.Data != null)
        //    {
        //        List<EmployeeLevelViewModel> list = JsonConvert.DeserializeObject<List<EmployeeLevelViewModel>>(res.Data);

        //        var lstData = list.Where(n => n.IsLock == "N").ToList();

        //        if (lstData != null)
        //        {
        //            listItem.Add(new SelectListItem()
        //            {
        //                Value = "0",
        //                Text = "Chọn vai trò"
        //            });
        //            foreach (var d in lstData)
        //            {
        //                listItem.Add(new SelectListItem()
        //                {
        //                    Value = d.EmployeeLevelCode,
        //                    Text = d.Name
        //                });
        //            }
        //        }
        //    }
        //    return listItem;
        //}
        //public static string GetNameEmployeeLevel(string ID)
        //{
        //    foreach (var item in GetsEmployeeLevel())
        //    {
        //        if (item.Value == ID)
        //        {
        //            return item.Text;
        //        }
        //    }
        //    return "";
        //}

    }
}