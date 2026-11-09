using System;
using EMS.API.GateWay;
using EMS.API.GateWay.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
namespace EMS.API.SyncGateWay.Repositories
{
    public class CMSRepository
    {
        public static ZResponse CMSAddRequestSupport(string data)
        {
            var res = APILibraryRepository.CMSAddRequestSupport("POST", "", JsonConvert.DeserializeObject<ZCMSAddRequestSupportRequest>(data));
            var MessageLog = string.Format("CMSRepository.CMSAddRequestSupport: req={0}||res={1}", data, JsonConvert.SerializeObject(res));
            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZCMSAddRequestSupportResponse>(res.Data);
                if (ResponseData.rows != null)
                {
                    if (ResponseData.rows.Count == 0 || ResponseData.rows[0].Id == "")
                    {
                        res.Code = "00";
                        res.Message = "Thành công";
                        XHelper.LogToFile(LogFileType.MESSAGE, MessageLog);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Gửi yêu cầu hỗ trợ thất bại.";
                        XHelper.LogToFile(LogFileType.MESSAGE, MessageLog);
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Gửi yêu cầu hỗ trợ thất bại.";
                    XHelper.LogToFile(LogFileType.MESSAGE, MessageLog);
                }
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, MessageLog);
            }
            return res;
        }

        public static ZResponse CMSResultRequestSupport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCMSResultRequestSupportRequest>(data);
            return APILibraryRepository.CMSResultRequestSupport("GET", "?ttkCrmId=" + req.RequestSupportCode, null);
        }

        //public static ZResponse CMSAddRequestSupport(string data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var req = JsonConvert.DeserializeObject<ZCMSAddRequestSupportRequest>(data);
        //        var callAPI = APILibraryRepository.InvokeCMS(new ZCMSRequest()
        //        {
        //            Method = "POST",
        //            RequestURL = APILibraryRepository.CMSAddUrl,
        //            RequestURI = "",
        //            AuthorizationName = "Basic",
        //            AuthorizationValue = Security.EncodeBase64(APILibraryRepository.CMSAuthorization),
        //            Value = req,
        //        });
        //        if (callAPI.Code == "00")
        //        {
        //            var resCMS = JsonConvert.DeserializeObject<ZCMSAddRequestSupportResponse>(callAPI.Data);
        //            if (resCMS.rows != null)
        //            {
        //                if (resCMS.rows.Count == 0 || resCMS.rows[0].Id == "")
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công";
        //                    res.Data = callAPI.Data;
        //                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("CMSRepository.CMSAddRequestSupport: request={0}||response={1}", data, JsonConvert.SerializeObject(callAPI)));
        //                }
        //                else
        //                {
        //                    res.Code = "01";
        //                    res.Message = "Gửi yêu cầu hỗ trợ thất bại.";
        //                    res.Data = callAPI.Data;
        //                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("CMSRepository.CMSAddRequestSupport: request={0}||response={1}", data, JsonConvert.SerializeObject(callAPI)));
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "01";
        //                res.Message = "Gửi yêu cầu hỗ trợ thất bại.";
        //                res.Data = callAPI.Data;
        //                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("CMSRepository.CMSAddRequestSupport: request={0}||response={1}", data, JsonConvert.SerializeObject(callAPI)));
        //            }
        //        }
        //        else
        //        {
        //            res = callAPI;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("CMSRepository.CMSAddRequestSupport: request={0}||exception={1}", data, ex.Message));
        //    }
        //    return res;
        //}

        //public static ZResponse CMSResultRequestSupport(string data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var req = JsonConvert.DeserializeObject<ZCMSResultRequestSupportRequest>(data);
        //        res = APILibraryRepository.InvokeCMS(new ZCMSRequest()
        //        {
        //            Method = "GET",
        //            RequestURL = APILibraryRepository.CMSResultUrl,
        //            RequestURI = string.Format("?ttkCrmId={0}", req.RequestSupportCode),
        //            AuthorizationName = "Basic",
        //            AuthorizationValue = Security.EncodeBase64(APILibraryRepository.CMSAuthorization),
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("CMSRepository.CMSResultRequestSupport: request={0}||exception={1}", data, ex.Message));
        //    }
        //    return res;
        //}
    }
}