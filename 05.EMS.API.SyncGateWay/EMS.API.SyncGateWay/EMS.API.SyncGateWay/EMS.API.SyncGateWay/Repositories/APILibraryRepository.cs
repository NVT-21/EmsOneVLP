using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace EMS.API.GateWay.Repositories
{
    public class APILibraryRepository
    {
        #region Library Zoho
        static string _zohoSendMailLink = string.Empty;

        static string _zohoSendMailAuthorization = string.Empty;

        public static string ZohoSendMailLink
        {
            get
            {
                if (string.IsNullOrEmpty(_zohoSendMailLink))
                    _zohoSendMailLink = ConfigurationManager.AppSettings["ZOHO_SEND_MAIL_LINK"];
                return _zohoSendMailLink;
            }

            set
            {
                _zohoSendMailLink = value;
            }
        }

        public static string ZohoSendMailAuthorization
        {
            get
            {
                if (string.IsNullOrEmpty(_zohoSendMailAuthorization))
                    _zohoSendMailAuthorization = ConfigurationManager.AppSettings["ZOHO_SEND_MAIL_AUTHORIZATION"];
                return _zohoSendMailAuthorization;
            }

            set
            {
                _zohoSendMailAuthorization = value;
            }
        }

        public static ZResponse CallApiTokenZohoCRM()
        {
            ZResponse res = new ZResponse();
            string Url = "https://accounts.zoho.com/oauth/v2/token";
            string code = "1000.64902be9e90f3f8d2ab4593a47d9931b.3147529f8dc48c56a29a1676c554f329";
            string client_id = "1000.PFW5AN8QL3BLL9C6TTBC4UV5PY778B";
            string client_secret = "ea57b7532d3de8f277a0ce27e8f0b0a66ccf8e2bbc";
            string redirect_url = "http://example.com/yourcallback";
            string grant_type = "authorization_code";

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(Url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AlwaysMultipartFormData = true;
                request.AddParameter("code", code);
                request.AddParameter("client_id", client_id);
                request.AddParameter("client_secret", client_secret);
                request.AddParameter("redirect_url", redirect_url);
                request.AddParameter("grant_type", grant_type);
                IRestResponse response = client.Execute(request);
                //HttpStatusCode statusCode = response.StatusCode;
                //int numericStatusCode = (int)statusCode;
                var a = response.StatusCode.ToString();
                if (a == "OK")
                {
                    res.Code = "00";
                    res.Data = response.Content;
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "APILibraryRepository.CallApiTokenZohoCRM: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
            }
            return res;
        }

        public static ZResponse CallApiTokenRefreshZohoCRM()
        {
            ZResponse res = new ZResponse();
            string Url = "https://accounts.zoho.com/oauth/v2/token";
            string refresh_token = "1000.e19c04747850b5a86fab6713e3076f17.380ed7a2f7fda08d26818bf8f3233a97";
            string client_id = "1000.PFW5AN8QL3BLL9C6TTBC4UV5PY778B";
            string client_secret = "ea57b7532d3de8f277a0ce27e8f0b0a66ccf8e2bbc";
            string redirect_url = "http://example.com/yourcallback";
            string grant_type = "refresh_token";

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(Url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AlwaysMultipartFormData = true;
                request.AddParameter("refresh_token", refresh_token);
                request.AddParameter("client_id", client_id);
                request.AddParameter("client_secret", client_secret);
                request.AddParameter("redirect_url", redirect_url);
                request.AddParameter("grant_type", grant_type);
                IRestResponse response = client.Execute(request);
                var a = response.StatusCode.ToString();
                if (a == "OK")
                {
                    res.Code = "00";
                    res.Data = response.Content;
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "APILibraryRepository.CallApiTokenRefreshZohoCRM: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
            }
            return res;
        }

        public static ZResponse CallApiGetZohoCRMLeads(string zohoToken, string Phone)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.zohoapis.com/crm/v2/Leads/search?phone=" + Phone);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Zoho-oauthtoken {0}", zohoToken));

                    HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var temp = response.Content.ReadAsAsync<ZApiZohoGetLeadsResponse>().Result;
                        if (temp != null)
                        {
                            res.Code = "00";
                            res.Data = JsonConvert.SerializeObject(temp);
                        }
                        else
                        {
                            res.Code = "01";
                            res.Data = null;
                        }
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.TRACE, "APILibraryRepository.CallApiGetZohoCRMLeads error: " + JsonConvert.SerializeObject(response.Content.ReadAsAsync<ZResponse>().Result));
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "APILibraryRepository.CallApiGetZohoCRMLeads: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
            }
            return res;
        }

        public static ZResponse CallApiGetZohoCRMLeadsEmail(string zohoToken, string Email)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.zohoapis.com/crm/v2/Leads/search?email=" + Email);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Zoho-oauthtoken {0}", zohoToken));

                    HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var temp = response.Content.ReadAsAsync<ZApiZohoGetLeadsResponse>().Result;
                        if (temp != null)
                        {
                            res.Code = "00";
                            res.Data = JsonConvert.SerializeObject(temp);
                        }
                        else
                        {
                            res.Code = "01";
                            res.Data = null;
                        }

                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "APILibraryRepository.CallApiGetZohoCRMLeadsEmail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
            }
            return res;
        }

        public static ZResponse CallApiCreateLeadsZohoCRM(string Data)
        {
            ZResponse res = new ZResponse();
            var data = JsonConvert.DeserializeObject<ZZohoCRMCreateLeadsRequest>(Data);
            var req = JsonConvert.DeserializeObject<ZZohoCRMCreateLeads02Request>(Data);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.zohoapis.com/crm/v2/Leads");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Zoho-oauthtoken {0}", data.zohoToken));

                    HttpResponseMessage response = client.PostAsJsonAsync(client.BaseAddress, req).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        res.Code = "00";
                        res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<ZApiZohoCreatResponse>().Result);
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.TRACE, "APILibraryRepository.CallApiCreateLeadsZohoCRM: " + Data);
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "APILibraryRepository.CallApiCreateLeadsZohoCRM: " + ex.Message);
            }
            return res;
        }

        //public static ZResponse SendMail(string data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //        var http = (HttpWebRequest)WebRequest.Create(new Uri("https://api.transmail.com/v1.1/email"));
        //        http.Accept = "application/json";
        //        http.ContentType = "application/json; charset=utf-8";
        //        http.Method = "POST";
        //        http.PreAuthenticate = true;
        //        http.Headers.Add("Authorization", ZohoSendMailAuthorization);
        //        UTF8Encoding encoding = new UTF8Encoding();
        //        Byte[] bytes = encoding.GetBytes(JsonConvert.DeserializeObject<JObject>(data).ToString());

        //        Stream newStream = http.GetRequestStream();
        //        newStream.Write(bytes, 0, bytes.Length);
        //        newStream.Close();

        //        var response = http.GetResponse();
        //        var stream = response.GetResponseStream();
        //        var sr = new StreamReader(stream);
        //        var content = sr.ReadToEnd();

        //        var dynResponse = JsonConvert.DeserializeObject<dynamic>(content);

        //        if (dynResponse.message == "OK")
        //        {
        //            res.Code = "00";
        //            res.Message = "Gửi email thành công";
        //        }
        //        else
        //        {
        //            res.Code = "01";
        //            res.Message = "Gửi email thất bại.";
        //            XHelper.LogToFile(LogFileType.MESSAGE, "APILibraryRepository.SendMail: " + JsonConvert.SerializeObject(dynResponse));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "APILibraryRepository.SendMail: " + ex.Message);
        //    }
        //    return res;
        //}

        public static ZResponse SendMail(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.BaseAddress = new Uri(ZohoSendMailLink);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Zoho-enczapikey {0}", ZohoSendMailAuthorization));

                    var req = JsonConvert.DeserializeObject<ZZohoSendMailRequest>(data);
                    req.bounce_address = "dev@ems.ems.com.vn";
                    req.from = new ZZohoSendMailFromRequest()
                    {
                        address = "transmail@ems.com.vn",
                        name = "ktcntt"
                    };

                    HttpResponseMessage response = client.PostAsJsonAsync(client.BaseAddress, req).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        res.Code = "00";
                        res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result);
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.TRACE, string.Format("APILibraryRepository.SendMail: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.SendMail: request={0}||exception={1}", data, ex.Message));
            }
            return res;
        }
        #endregion

        #region Library PNS
        static string _emsApiKey = string.Empty;

        static string _emsApiUrl = string.Empty;

        static string _emsApiAuthorization = string.Empty;

        public static string EMSApiKey
        {
            get
            {
                if (string.IsNullOrEmpty(_emsApiKey))
                    _emsApiKey = ConfigurationManager.AppSettings["EMS_API_KEY"];
                return _emsApiKey;
            }

            set
            {
                _emsApiKey = value;
            }
        }

        public static string EMSApiURL
        {
            get
            {
                if (string.IsNullOrEmpty(_emsApiUrl))
                    _emsApiUrl = ConfigurationManager.AppSettings["EMS_API_URL"];
                return _emsApiUrl;
            }

            set
            {
                _emsApiUrl = value;
            }
        }

        public static string EMSApiAuthorization
        {
            get
            {
                if (string.IsNullOrEmpty(_emsApiAuthorization))
                    _emsApiAuthorization = ConfigurationManager.AppSettings["EMS_API_AUTHORIZATION"];
                return _emsApiAuthorization;
            }

            set
            {
                _emsApiAuthorization = value;
            }
        }

        //public static ZResponse CallApiCreatePickup(string Data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(EMSApiURL + "api/Pickup/CreatePickupv1");
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.Add("APIKey", EMSApiKey);
        //            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //            client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", EMSApiAuthorization));

        //            var request = JsonConvert.DeserializeObject<ZTransportInfoShippingApiRequest>(Data);
        //            HttpResponseMessage response = client.PostAsJsonAsync(client.BaseAddress, request).Result;

        //            if (response.IsSuccessStatusCode)
        //            {
        //                res.Code = "00";
        //                res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<ZApiResponse>().Result);
        //            }
        //            else
        //            {
        //                XHelper.LogToFile(LogFileType.TRACE, string.Format("APILibraryRepository.CallApiCreatePickup: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
        //                switch (response.StatusCode)
        //                {
        //                    default:
        //                        res.Code = "99";
        //                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.CallApiCreatePickup: request={0}||exception={1}", Data, ex.Message));
        //    }
        //    return res;
        //}

        //public static ZResponse CallApiShipmentCancel(string Data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(EMSApiURL + "api/Order/ShipmentCancel");
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.Add("APIKey", EMSApiKey);
        //            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //            client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", EMSApiAuthorization));

        //            var request = JsonConvert.DeserializeObject<ZTransportShippingCancelRequest>(Data);
        //            HttpResponseMessage response = client.PostAsJsonAsync(client.BaseAddress, request).Result;

        //            if (response.IsSuccessStatusCode)
        //            {
        //                res.Code = "00";
        //                res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<ZApiResponse>().Result);
        //            }
        //            else
        //            {
        //                XHelper.LogToFile(LogFileType.TRACE, string.Format("APILibraryRepository.CallApiShipmentCancel: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
        //                switch (response.StatusCode)
        //                {
        //                    default:
        //                        res.Code = "99";
        //                        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.CallApiShipmentCancel: request={0}||exception={1}", Data, ex.Message));
        //    }
        //    return res;
        //}
        #endregion

        #region Gọi qua hệ thống CMS
        static string _cmsAddUrl = string.Empty;

        static string _cmsResultUrl = string.Empty;

        static string _cmsAuthorization = string.Empty;

        public static string CMSAddUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_cmsAddUrl))
                    _cmsAddUrl = ConfigurationManager.AppSettings["CMS_ADD_URL"];
                return _cmsAddUrl;
            }

            set
            {
                _cmsAddUrl = value;
            }
        }

        public static string CMSResultUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_cmsResultUrl))
                    _cmsResultUrl = ConfigurationManager.AppSettings["CMS_RESULT_URL"];
                return _cmsResultUrl;
            }

            set
            {
                _cmsResultUrl = value;
            }
        }

        public static string CMSAuthorization
        {
            get
            {
                if (string.IsNullOrEmpty(_cmsAuthorization))
                    _cmsAuthorization = ConfigurationManager.AppSettings["CMS_AUTHORIZATION"];
                return _cmsAuthorization;
            }

            set
            {
                _cmsAuthorization = value;
            }
        }

        public static ZResponse CMSAddRequestSupport(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(CMSAddUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Security.EncodeBase64(CMSAuthorization)));
                    HttpResponseMessage response = null;
                    switch (method)
                    {
                        case "GET":
                            response = client.GetAsync(requestUri).Result;
                            break;
                        case "POST":
                            response = client.PostAsJsonAsync(client.BaseAddress, value).Result;
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
                        XHelper.LogToFile(LogFileType.TRACE, string.Format("APILibraryRepository.CMSAddRequestSupport: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.CMSAddRequestSupport: request={0}||exception={1}", JsonConvert.SerializeObject(value), ex.Message));
            }
            return res;
        }

        public static ZResponse CMSResultRequestSupport(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(CMSResultUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Security.EncodeBase64(CMSAuthorization)));
                    HttpResponseMessage response = null;
                    switch (method)
                    {
                        case "GET":
                            response = client.GetAsync(requestUri).Result;
                            break;
                        case "POST":
                            response = client.PostAsJsonAsync(client.BaseAddress, value).Result;
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
                        XHelper.LogToFile(LogFileType.TRACE, string.Format("APILibraryRepository.CMSResultRequestSupport: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.CMSResultRequestSupport: request={0}||exception={1}", JsonConvert.SerializeObject(value), ex.Message));
            }
            return res;
        }

        public static ZResponse InvokeCMS(ZCMSRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient ClientCMS = new HttpClient())
                {
                    ClientCMS.BaseAddress = new Uri(req.RequestURL);
                    ClientCMS.DefaultRequestHeaders.Accept.Clear();
                    ClientCMS.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    ClientCMS.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    if (!string.IsNullOrEmpty(req.APIKeyValue))
                    {
                        ClientCMS.DefaultRequestHeaders.Add(req.APIKeyName, req.APIKeyValue);
                    }

                    if (!string.IsNullOrEmpty(req.AuthorizationValue))
                    {
                        ClientCMS.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(req.AuthorizationName, req.AuthorizationValue);

                    }

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpResponseMessage response = null;
                    switch (req.Method)
                    {
                        case "GET":
                            response = ClientCMS.GetAsync(req.RequestURI).Result;
                            break;
                        case "POST":
                            if (req.ListValue == null)
                            {
                                response = ClientCMS.PostAsJsonAsync(req.RequestURI, req.Value).Result;
                            }
                            else
                            {
                                response = ClientCMS.PostAsJsonAsync(req.RequestURI, req.ListValue).Result;
                            }
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
                        var ContentLog = string.Format("APILibraryRepository.InvokeCMS: url={0}||request={1}||response={2}||content={3}",
                            req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.Value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result));
                        XHelper.LogToFile(LogFileType.MESSAGE, ContentLog);
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
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.InvokeCMS: url={0}||request={1}||exception={2}", req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.Value), ex.Message));

            }
            return res;
        }
        #endregion

        #region Gọi qua hệ thống VNPost
        static string _vnpMapsUrl = string.Empty;

        static string _vnpMapsKey = string.Empty;

        static string _vnpMapsAutocompleteVersion = string.Empty;

        static string _vnpMapsAutocompleteKey = string.Empty;

        public static string VNPMapsUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_vnpMapsUrl))
                    _vnpMapsUrl = ConfigurationManager.AppSettings["VNP_MAPS_URL"];
                return _vnpMapsUrl;
            }

            set
            {
                _vnpMapsUrl = value;
            }
        }

        public static string VNPMapsKey
        {
            get
            {
                if (string.IsNullOrEmpty(_vnpMapsKey))
                    _vnpMapsKey = ConfigurationManager.AppSettings["VNP_MAPS_KEY"];
                return _vnpMapsKey;
            }

            set
            {
                _vnpMapsKey = value;
            }
        }

        public static string VNPMapsAutocompleteVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_vnpMapsAutocompleteVersion))
                    _vnpMapsAutocompleteVersion = ConfigurationManager.AppSettings["VNP_MAPS_AUTOCOMPLETE_VERSION"];
                return _vnpMapsAutocompleteVersion;
            }

            set
            {
                _vnpMapsAutocompleteVersion = value;
            }
        }

        public static string VNPMapsAutocompleteKey
        {
            get
            {
                if (string.IsNullOrEmpty(_vnpMapsAutocompleteKey))
                    _vnpMapsAutocompleteKey = ConfigurationManager.AppSettings["VNP_MAPS_AUTOCOMPLETE_KEY"];
                return _vnpMapsAutocompleteKey;
            }

            set
            {
                _vnpMapsAutocompleteKey = value;
            }
        }

        //public static ZResponse CallApiVNPMaps(string method, string requestUri, object value)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(VNPMapsUrl);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //            client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", VNPMapsKey));
        //            ServicePointManager.Expect100Continue = true;
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //            HttpResponseMessage response = null;
        //            switch (method)
        //            {
        //                case "GET":
        //                    response = client.GetAsync(requestUri).Result;
        //                    break;
        //                case "POST":
        //                    response = client.PostAsJsonAsync(client.BaseAddress, value).Result;
        //                    break;
        //                default:
        //                    break;
        //            }

        //            if (response.IsSuccessStatusCode)
        //            {
        //                res.Code = "00";
        //                res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<ZVNPMapsAutocompleteResponse>().Result);
        //                res.Message = "Thành công";
        //            }
        //            else
        //            {
        //                XHelper.LogToFile(LogFileType.TRACE, string.Format("APILibraryRepository.CallApiVNPMaps: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
        //                switch (response.StatusCode)
        //                {
        //                    default:
        //                        res.Code = "99";
        //                        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.CallApiVNPMaps: request={0}||exception={1}", JsonConvert.SerializeObject(value), ex.Message));
        //    }
        //    return res;
        //}

        static string _vnpImageUrl = string.Empty;

        static string _vnpImageKey = string.Empty;

        public static string VNPImageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_vnpImageUrl))
                    _vnpImageUrl = ConfigurationManager.AppSettings["VNP_IMAGE_URL"];
                return _vnpImageUrl;
            }

            set
            {
                _vnpImageUrl = value;
            }
        }

        public static string VNPImageKey
        {
            get
            {
                if (string.IsNullOrEmpty(_vnpImageKey))
                    _vnpImageKey = ConfigurationManager.AppSettings["VNP_IMAGE_KEY"];
                return _vnpImageKey;
            }

            set
            {
                _vnpImageKey = value;
            }
        }

        static string _vnpEdataImgUrl = string.Empty;

        static string _vnpEdataImgKey = string.Empty;

        public static string EdataImgUrl
        {
            get
            {
                if(string.IsNullOrEmpty(_vnpEdataImgUrl))
                    _vnpEdataImgUrl = ConfigurationManager.AppSettings["EDATA_IMAGE_URL"];
                return _vnpEdataImgUrl;
            }

            set
            {
                _vnpEdataImgUrl = value;
            }
        }

        public static string EdataImgKey
        {
            get
            {
                if(string.IsNullOrEmpty(_vnpEdataImgKey))
                    _vnpEdataImgKey = ConfigurationManager.AppSettings["EDATA_IMAGE_KEY"];
                return _vnpEdataImgKey;
            }

            set
            {
                _vnpEdataImgKey = value;
            }
        }

        public static ZResponse InvokeVNP(ZVNPRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient ClientVNP = new HttpClient())
                {
                    ClientVNP.BaseAddress = new Uri(req.RequestURL);
                    ClientVNP.DefaultRequestHeaders.Accept.Clear();
                    ClientVNP.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    ClientVNP.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    if (!string.IsNullOrEmpty(req.APIKeyValue))
                    {
                        ClientVNP.DefaultRequestHeaders.Add(req.APIKeyName, req.APIKeyValue);
                    }

                    if (!string.IsNullOrEmpty(req.AuthorizationValue))
                    {
                        ClientVNP.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(req.AuthorizationName, req.AuthorizationValue);

                    }
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpResponseMessage response = null;
                    switch (req.Method)
                    {
                        case "GET":
                            response = ClientVNP.GetAsync(req.RequestURI).Result;
                            break;
                        case "POST":
                            response = ClientVNP.PostAsJsonAsync(req.RequestURI, req.Value).Result;
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
                        var ContentLog = string.Format("APILibraryRepository.InvokeVNP: url={0}||request={1}||response={2}||content={3}",
                            req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.Value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result));
                        XHelper.LogToFile(LogFileType.MESSAGE, ContentLog);
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.InvokeVNP: url={0}||request={1}||exception={2}", req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.Value), ex.Message));
            }
            return res;
        }
        #endregion

        #region Gọi qua hệ thống EMS 

        #region EMS Bưu gửi hoàn cước, giấy báo nợ

        static string _emsTradeDiscountUrl = string.Empty;

        static string _emsTradeDiscountAuthor = string.Empty;

        static string EMSTradeDiscountUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_emsTradeDiscountUrl))
                    _emsTradeDiscountUrl = ConfigurationManager.AppSettings["EMS_TRADEDISCOUNT_URL"];
                return _emsTradeDiscountUrl;
            }

            set
            {
                _emsTradeDiscountUrl = value;
            }
        }

        static string EMSTradeDiscountAuthor
        {
            get
            {
                if (string.IsNullOrEmpty(_emsTradeDiscountAuthor))
                    _emsTradeDiscountAuthor = ConfigurationManager.AppSettings["EMS_TRADEDISCOUNT_AUTHORIZATION"];
                return _emsTradeDiscountAuthor;
            }

            set
            {
                _emsTradeDiscountAuthor = value;
            }
        }

        public static ZResponse EMSPostageRefund(ZRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(EMSTradeDiscountUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(EMSTradeDiscountAuthor))));

                    HttpResponseMessage rs = null;
                    rs = client.PostAsJsonAsync(req.Code, JsonConvert.DeserializeObject<ZReconciliationSearchPostageRefundRequest>(req.Data)).Result;

                    if (rs.IsSuccessStatusCode)
                    {
                        res.Code = "00";
                        res.Data = JsonConvert.SerializeObject(rs.Content.ReadAsAsync<object>().Result);
                        res.Message = "Thành công";
                    }
                    else
                    {
                        switch (rs.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("APILibraryRepository.EMSPostageRefund: req={0}||res={1}", req.Data, JsonConvert.SerializeObject(rs.Content.ReadAsAsync<object>().Result)));
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "APILibraryRepository.EMSPostageRefund: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EMSDebtNotices(ZRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(EMSTradeDiscountUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(EMSTradeDiscountAuthor))));

                    HttpResponseMessage rs = null;

                    rs = client.PostAsJsonAsync(req.Code, JsonConvert.DeserializeObject<ZReconciliationSearchDebtNoticesRequest>(req.Data)).Result;

                    if (rs.IsSuccessStatusCode)
                    {
                        res.Code = "00";
                        res.Data = JsonConvert.SerializeObject(rs.Content.ReadAsAsync<object>().Result);
                        res.Message = "Thành công";
                    }
                    else
                    {
                        switch (rs.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("APILibraryRepository.EMSDebtNotices: req={0}||res={1}", req.Data, JsonConvert.SerializeObject(rs.Content.ReadAsAsync<object>().Result)));
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "APILibraryRepository.EMSDebtNotices: " + ex.Message);
            }
            return res;
        }

        #endregion

        #region EMS Siêu tốc

        static string _grabDeliveryUrl = string.Empty;

        static string _grabDeliveryToken = string.Empty;

        static string GrabDeliveryUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_grabDeliveryUrl))
                    _grabDeliveryUrl = ConfigurationManager.AppSettings["GRAB_DELIVERY_URL"];
                return _grabDeliveryUrl;
            }

            set
            {
                _grabDeliveryUrl = value;
            }
        }

        public static string GrabDeliveryToken
        {
            get
            {
                if (string.IsNullOrEmpty(_grabDeliveryToken))
                    _grabDeliveryToken = ConfigurationManager.AppSettings["GRAB_DELIVERY_TOKEN"];
                return _grabDeliveryToken;
            }

            set
            {
                _grabDeliveryToken = value;
            }
        }

        public static ZResponse InvokeGrab(ZGrabRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient ClientGrab = new HttpClient())
                {
                    ClientGrab.BaseAddress = new Uri(GrabDeliveryUrl);
                    ClientGrab.DefaultRequestHeaders.Accept.Clear();
                    ClientGrab.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    ClientGrab.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");


                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpResponseMessage response = null;
                    switch (req.Method)
                    {
                        case "GET":
                            response = ClientGrab.GetAsync(req.RequestURI).Result;
                            break;
                        case "POST":
                            response = ClientGrab.PostAsJsonAsync(req.RequestURI, req.Value).Result;
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
                        var ContentLog = string.Format("APILibraryRepository.InvokeGrab: request={0}||response={1}||content={2}",
                            JsonConvert.SerializeObject(req.Value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result));
                        XHelper.LogToFile(LogFileType.MESSAGE, ContentLog);
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.InvokeGrab: request={0}||exception={1}", JsonConvert.SerializeObject(req.Value), ex.Message));
            }
            return res;
        }

        public static ZResponse GrabDelivery(ZRequest req, string method)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(GrabDeliveryUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                    HttpResponseMessage rs = null;
                    switch (method)
                    {
                        case "GET":
                            rs = client.GetAsync(req.Code).Result;
                            break;
                        case "POST":
                            rs = client.PostAsJsonAsync(req.Code, JsonConvert.DeserializeObject<object>(req.Data)).Result;
                            break;
                        default:
                            break;
                    }

                    if (rs.IsSuccessStatusCode)
                    {
                        res.Code = "00";
                        res.Data = JsonConvert.SerializeObject(rs.Content.ReadAsAsync<object>().Result);
                        res.Message = "Thành công";
                    }
                    else
                    {
                        switch (rs.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("APILibraryRepository.EMSExpressDelivery: req={0}||res={1}", req.Data, JsonConvert.SerializeObject(rs.Content.ReadAsAsync<object>().Result)));
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "APILibraryRepository.GrabDelivery: " + ex.Message);
            }
            return res;
        }

        #endregion

        #region Phân tích địa chỉ của EMS
        static string _emsMapsURL = string.Empty;

        static string _emsMapsKey = string.Empty;

        public static string EMSMapsURL
        {
            get
            {
                if (string.IsNullOrEmpty(_emsMapsURL))
                    _emsMapsURL = ConfigurationManager.AppSettings["EMS_MAPS_URL"];
                return _emsMapsURL;
            }

            set
            {
                _emsMapsURL = value;
            }
        }

        public static string EMSMapsKey
        {
            get
            {
                if (string.IsNullOrEmpty(_emsMapsKey))
                    _emsMapsKey = ConfigurationManager.AppSettings["EMS_MAPS_KEY"];
                return _emsMapsKey;
            }

            set
            {
                _emsMapsKey = value;
            }
        }

        static string _emsMapsMCSURL = string.Empty;

        static string _emsMapsMCSKey = string.Empty;

        public static string EMSMapsMCSURL
        {
            get
            {
                if(string.IsNullOrEmpty(_emsMapsMCSURL))
                    _emsMapsURL = ConfigurationManager.AppSettings["EMS_MCS_MAP_URL"];
                return _emsMapsURL;
            }

            set
            {
                _emsMapsURL = value;
            }
        }


        static string _vnpostMapsURL = string.Empty;

        static string _vnpostMapsKey = string.Empty;

        public static string VNPOSTMapsURL
        {
            get
            {
                if(string.IsNullOrEmpty(_vnpostMapsURL))
                    _vnpostMapsURL = ConfigurationManager.AppSettings["VNPOST_MAPS_URL"];
                return _vnpostMapsURL;
            }

            set
            {
                _vnpostMapsURL = value;
            }
        }

        public static string VNPOSTMapsKey
        {
            get
            {
                if(string.IsNullOrEmpty(_vnpostMapsKey))
                    _vnpostMapsKey = ConfigurationManager.AppSettings["VNPOST_MAPS_KEY"];
                return _vnpostMapsKey;
            }

            set
            {
                _vnpostMapsKey = value;
            }
        }

        //static HttpClient _emsMapsClient = null;

        //public static ZResponse EMSMapSearch(ZRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var requestData = JsonConvert.DeserializeObject<ZEMSMapSearchRequest>(req.Data);
        //        if (_emsMapsClient == null)
        //        {
        //            _emsMapsClient = new HttpClient();
        //            _emsMapsClient.BaseAddress = new Uri(EMSMapsURL);
        //            _emsMapsClient.DefaultRequestHeaders.Accept.Clear();
        //            _emsMapsClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //            _emsMapsClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //            //_emsMapsClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", EMSMapsKey));
        //            //_emsMapsClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("Bearer {0}", EMSMapsKey));
        //            //_emsMapsClient.DefaultRequestHeaders.Remove("Content-Type");
        //            _emsMapsClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", EMSMapsKey);
        //            //ServicePointManager.FindServicePoint(_emsMapsClient.BaseAddress).ConnectionLeaseTimeout = 60 * 1000; //1 minute     
        //        }

        //        HttpResponseMessage response = _emsMapsClient.GetAsync(req.Code + requestData.AddressFull).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            res.Code = "00";
        //            res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result);
        //            res.Message = "Thành công";
        //        }
        //        else
        //        {
        //            XHelper.LogToFile(LogFileType.TRACE, string.Format("APILibraryRepository.EMSMapSearch: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
        //            switch (response.StatusCode)
        //            {
        //                case HttpStatusCode.Unauthorized:
        //                    res.Code = "98";
        //                    res.Message = "Token hết hạn.";
        //                    break;
        //                default:
        //                    res.Code = "99";
        //                    res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.EMSMapSearch: request={0}||exception={1}", JsonConvert.SerializeObject(req), ex.Message));
        //    }
        //    return res;
        //}
        #endregion

        #region Đẩy thông tin sửa tiền COD sang EMS
        static string _emsPushAffairUrl = string.Empty;

        static string _emsPushAffairKey = string.Empty;

        public static string EMSPushAffairUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_emsPushAffairUrl))
                    _emsPushAffairUrl = ConfigurationManager.AppSettings["EMS_PUSH_AFFAIR_URL"];
                return _emsPushAffairUrl;
            }

            set
            {
                _emsPushAffairUrl = value;
            }
        }

        public static string EMSPushAffairKey
        {
            get
            {
                if (string.IsNullOrEmpty(_emsPushAffairKey))
                    _emsPushAffairKey = ConfigurationManager.AppSettings["EMS_PUSH_AFFAIR_KEY"];
                return _emsPushAffairKey;
            }

            set
            {
                _emsPushAffairKey = value;
            }
        }

        //public static ZResponse EMSPushAffair(ZRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(EMSPushAffairUrl);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //            client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", EMSPushAffairKey));

        //            HttpResponseMessage response = client.PostAsJsonAsync(req.Code, JsonConvert.DeserializeObject<List<ZEMSPushAffairRequest>>(req.Data)).Result;

        //            if (response.IsSuccessStatusCode)
        //            {
        //                res.Code = "00";
        //                res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result);
        //                res.Message = "Thành công";
        //            }
        //            else
        //            {
        //                XHelper.LogToFile(LogFileType.TRACE, string.Format("APILibraryRepository.EMSPushAffair: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
        //                switch (response.StatusCode)
        //                {
        //                    default:
        //                        res.Code = "99";
        //                        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.EMSPushAffair: request={0}||exception={1}", JsonConvert.SerializeObject(req), ex.Message));
        //    }
        //    return res;
        //}
        #endregion

        #region Gọi qua hệ thống EMS 
        public static ZResponse InvokeEMS(ZEMSRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient ClientEMS = new HttpClient())
                {
                    ClientEMS.BaseAddress = new Uri(req.RequestURL);
                    ClientEMS.DefaultRequestHeaders.Accept.Clear();
                    ClientEMS.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    ClientEMS.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    if (!string.IsNullOrEmpty(req.APIKeyValue))
                    {
                        ClientEMS.DefaultRequestHeaders.Add(req.APIKeyName, req.APIKeyValue);
                    }

                    if (!string.IsNullOrEmpty(req.AuthorizationValue))
                    {
                        ClientEMS.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(req.AuthorizationName, req.AuthorizationValue);
                    }

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpResponseMessage response = null;
                    switch (req.Method)
                    {
                        case "GET":
                            response = ClientEMS.GetAsync(req.RequestURI).Result;
                            break;
                        case "POST":
                            if (req.ListValue == null)
                            {
                                response = ClientEMS.PostAsJsonAsync(req.RequestURI, req.Value).Result;
                            }
                            else
                            {
                                response = ClientEMS.PostAsJsonAsync(req.RequestURI, req.ListValue).Result;
                            }
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
                        var ContentLog = string.Format("APILibraryRepository.InvokeEMS: url={0}||request={1}||response={2}||content={3}",
                            req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.ListValue == null ? req.Value : req.ListValue), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result));
                        XHelper.LogToFile(LogFileType.MESSAGE, ContentLog);
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
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.InvokeEMS: url={0}||request={1}||exception={2}", req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.ListValue == null ? req.Value : req.ListValue), ex.Message));
            }
            return res;
        }
        #endregion

        #region Gọi qua hệ thống EMS Gateway
        static string _emsGatewaytUrl = string.Empty;

        static string _emsGatewayAuthor = string.Empty;

        public static string EMSGatewayUrl
        {
            get
            {
                if(string.IsNullOrEmpty(_emsGatewaytUrl))
                    _emsGatewaytUrl = ConfigurationManager.AppSettings["EMS_API_GATEWAY_URL"];
                return _emsGatewaytUrl;
            }

            set
            {
                _emsGatewaytUrl = value;
            }
        }

        public static string EMSGatewayAuthor
        {
            get
            {
                if(string.IsNullOrEmpty(_emsGatewayAuthor))
                    _emsGatewayAuthor = ConfigurationManager.AppSettings["EMS_API_GATEWAY_KEY"];
                return _emsGatewayAuthor;
            }

            set
            {
                _emsGatewayAuthor = value;
            }
        }

        public static ZResponse InvokeGatewayEMS(ZEMSRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                using(HttpClient ClientEMS = new HttpClient())
                {
                    ClientEMS.BaseAddress = new Uri(req.RequestURL);
                    ClientEMS.DefaultRequestHeaders.Accept.Clear();
                    ClientEMS.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    ClientEMS.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    if(!string.IsNullOrEmpty(req.APIKeyValue))
                    {
                        ClientEMS.DefaultRequestHeaders.Add(req.APIKeyName, req.APIKeyValue);
                    }

                    if(!string.IsNullOrEmpty(req.AuthorizationValue))
                    {
                        ClientEMS.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(req.AuthorizationName, req.AuthorizationValue);
                    }

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpResponseMessage response = null;
                    switch(req.Method)
                    {
                        case "GET":
                            response = ClientEMS.GetAsync(req.RequestURI).Result;
                            break;
                        case "POST":
                            if(req.ListValue == null)
                            {
                                response = ClientEMS.PostAsJsonAsync(req.RequestURI, req.Value).Result;
                            }
                            else
                            {
                                response = ClientEMS.PostAsJsonAsync(req.RequestURI, req.ListValue).Result;
                            }
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
                        var ContentLog = string.Format("APILibraryRepository.InvokeEMS: url={0}||request={1}||response={2}||content={3}",
                            req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.ListValue == null ? req.Value : req.ListValue), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result));
                        XHelper.LogToFile(LogFileType.MESSAGE, ContentLog);
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
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.InvokeGatewayEMS: url={0}||request={1}||exception={2}", req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.ListValue == null ? req.Value : req.ListValue), ex.Message));
            }
            return res;
        }
        #endregion

        #endregion

        #region Gọi qua hệ thống VNPOST
        public static ZResponse InvokeVNPOSTMap(ZEMSRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                using(HttpClient ClientEMS = new HttpClient())
                {
                    ClientEMS.BaseAddress = new Uri(req.RequestURL);
                    ClientEMS.DefaultRequestHeaders.Accept.Clear();
                    ClientEMS.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    ClientEMS.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    if(!string.IsNullOrEmpty(req.APIKeyValue))
                    {
                        ClientEMS.DefaultRequestHeaders.Add(req.APIKeyName, req.APIKeyValue);
                    }

                    if(!string.IsNullOrEmpty(req.AuthorizationValue))
                    {
                        ClientEMS.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(req.AuthorizationName, req.AuthorizationValue);
                    }

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpResponseMessage response = null;
                    switch(req.Method)
                    {
                        case "GET":
                            response = ClientEMS.GetAsync(req.RequestURI).Result;
                            break;
                        case "POST":
                            if(req.ListValue == null)
                            {
                                response = ClientEMS.PostAsJsonAsync(req.RequestURI, req.Value).Result;
                            }
                            else
                            {
                                response = ClientEMS.PostAsJsonAsync(req.RequestURI, req.ListValue).Result;
                            }
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
                        var ContentLog = string.Format("APILibraryRepository.InvokeEMS: url={0}||request={1}||response={2}||content={3}",
                            req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.ListValue == null ? req.Value : req.ListValue), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result));
                        XHelper.LogToFile(LogFileType.MESSAGE, ContentLog);
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
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.InvokeEMS: url={0}||request={1}||exception={2}", req.RequestURL + req.RequestURI, JsonConvert.SerializeObject(req.ListValue == null ? req.Value : req.ListValue), ex.Message));
            }
            return res;
        }
        #endregion

        #region Gọi qua hệ thống Paypost
        static string _paypostKey = string.Empty;

        static string PaypostKey
        {
            get
            {
                if (string.IsNullOrEmpty(_paypostKey))
                    _paypostKey = ConfigurationManager.AppSettings["PAYPOST_KEY"];
                return _paypostKey;
            }

            set
            {
                _paypostKey = value;
            }
        }

        static string _paypostUrl = string.Empty;

        static string PaypostUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_paypostUrl))
                    _paypostUrl = ConfigurationManager.AppSettings["PAYPOST_URL"];
                return _paypostUrl;
            }

            set
            {
                _paypostUrl = value;
            }
        }

        public static ZResponse CallApiEditCODMoney(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(PaypostUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("APIKey", PaypostKey);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                    HttpResponseMessage response = null;

                    switch (method)
                    {
                        case "GET":
                            response = client.GetAsync(requestUri).Result;
                            break;
                        case "POST":
                            response = client.PostAsJsonAsync(requestUri, value).Result;
                            break;
                        default:
                            break;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        res.Code = "00";
                        res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<ZPaypostEditCODMoneyResponse>().Result);
                        res.Message = "Thành công";
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("APILibraryRepository.CallApiEditCODMoney: request={0}||response={1}||content={2}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result)));
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.CallApiEditCODMoney: request={0}||exception={1}", JsonConvert.SerializeObject(value), ex.Message));
            }
            return res;
        }
        #endregion

        //static string _baseURLInformationParcelRelease = string.Empty;
        //static string _secretKeyInformationParcelRelease = string.Empty;
        //static string BaseUrlInformationParcelRelease
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_baseURLInformationParcelRelease))
        //            _baseURLInformationParcelRelease = ConfigurationManager.AppSettings["BASE_URL_INFORMATION_PARCEL_RELEASE"];
        //        return _baseURLInformationParcelRelease;
        //    }

        //    set
        //    {
        //        _baseURLInformationParcelRelease = value;
        //    }
        //}
        //static string SecretKeyInformationParcelRelease
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_secretKeyInformationParcelRelease))
        //            _secretKeyInformationParcelRelease = ConfigurationManager.AppSettings["SERECT_KEY_INFORMATION_PARCEL_RELEASE"];
        //        return _secretKeyInformationParcelRelease;
        //    }

        //    set
        //    {
        //        _secretKeyInformationParcelRelease = value;
        //    }
        //}

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
                                res.Data = JsonConvert.SerializeObject(response.Content.ReadAsAsync<object>().Result);
                                break;
                        }
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("APILibraryRepository.InvokeFacebook: request={0} || response={1}", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response)));
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("APILibraryRepository.InvokeFacebook: request={0}||exception={1}", JsonConvert.SerializeObject(request), ex.Message));
            }
            return res;
        }
        #endregion

    }
}