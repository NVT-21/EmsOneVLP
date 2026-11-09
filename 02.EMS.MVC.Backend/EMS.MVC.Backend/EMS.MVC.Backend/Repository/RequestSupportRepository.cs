using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Backend.Repository
{
    public class RequestSupportRepository
    {
        public static ZResponse SearchListRequestSupport(ZRequestSupportBESearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_BE_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.SearchListRequestSupport: " + ex.Message);
            }
            return res;
        }

        public static ZResponse DetailRequestSupport(ZRequestSupportDetailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_DETAIL",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.DetailRequestSupport: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SendRequestSupport(ZRequestSupportSendRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_SEND",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.SendRequestSupport: " + ex.Message);
            }
            return res;
        }

        //public static ZResponse AcceptedStatus(ZRequestSupportSendRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "REQUEST_SUPPORT_SEND",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.AcceptedStatus: " + ex.Message);
        //    }
        //    return res;
        //}

        public static ZResponse UnAcceptedStatus(ZRequestSupportUnAcceptedStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_UNACCEPTED_STATUS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.UnAcceptedStatus: " + ex.Message);
            }
            return res;
        }

        public static ZResponse ProcessStatus(ZRequestSupportProcessStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_PROCESS_STATUS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.ProcessStatus: " + ex.Message);
            }
            return res;
        }

        public static ZResponse CloseStatus(ZRequestSupportCloseStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_CLOSE_STATUS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.CloseStatus: " + ex.Message);
            }
            return res;
        }

        public static ZResponse ReOpenStatus(ZRequestSupportReOpenStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_RE_OPEN_STATUS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.ReOpenStatus: " + ex.Message);
            }
            return res;
        }

        public static ZResponse UpdateClickNotify(ZRequestSupportUpdateClickNotifyRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_UPDATE_CLICK_NOTIFY",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.UpdateClickNotify: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddResultRequestSupport(ZRequestSupportAddProcessResultRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_SUPPORT_ADD_PROCESS_RESULT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.AddResultRequestSupport: " + ex.Message);
            }
            return res;
        }
    }
}