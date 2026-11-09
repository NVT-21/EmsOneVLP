using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class RequestSupportRepository
    {
        public static ZResponse FESearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportFESearchListRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/FESearchList", req);
        }
        public static ZResponse BESearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportBESearchListRequest>(data);
            if(XHelper.IsSqlInjection(req.RequestSupportCode) || XHelper.IsSqlInjection(req.EShippingCode) || XHelper.IsSqlInjection(req.CrmOrPaypostCode) || XHelper.IsSqlInjection(req.RequestSupportGroup) || XHelper.IsSqlInjection(req.RequestSupportType) || XHelper.IsSqlInjection(req.RequestSupportStatus))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "RequestSupport/BESearchList", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportAddRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/Add", req);
        }

        //public static ZResponse SearchInfoSendRequestSupport(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZRequestSupportSearchInfoSendRequestSupportRequest>(data);
        //    return XHelper.Invoke("POST", "RequestSupport/SearchInfoSendRequestSupport", req);
        //}

        public static ZResponse UpdateStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportUpdateStatusRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/UpdateStatus", req);
        }

        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportGetByIDRequest>(data);
            return XHelper.Invoke("GET", "RequestSupport/GetByID?RequestSupportID=" + req.RequestSupportID + "&ShopID=" + req.ShopID, null);
        }

        public static ZResponse Detail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportDetailRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/Detail", req);
        }

        //public static ZResponse SearchHistory(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZRequestSupportSearchHistoryRequest>(data);
        //    return XHelper.Invoke("POST", "RequestSupport/SearchHistory", req);
        //}

        public static ZResponse AcceptedStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportAcceptedStatusRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/AcceptedStatus", req);
        }

        public static ZResponse UnAcceptedStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportUnAcceptedStatusRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/UnAcceptedStatus", req);
        }

        public static ZResponse ProcessStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportProcessStatusRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/ProcessStatus", req);
        }

        public static ZResponse CloseStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportCloseStatusRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/CloseStatus", req);
        }

        public static ZResponse ReOpenStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportReOpenStatusRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/ReOpenStatus", req);
        }

        public static ZResponse ExistStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportExistStatusRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/ExistStatus", req);
        }

        public static ZResponse NotifyList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportNotifyListRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/NotifyList", req);
        }

        public static ZResponse UpdateClickNotify(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportUpdateClickNotifyRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/UpdateClickNotify", req);
        }

        //public static ZResponse LockAcceptedStatus(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZRequestSupportLockAcceptedStatusRequest>(data);
        //    return XHelper.Invoke("POST", "RequestSupport/LockAcceptedStatus", req);
        //}

        //public static ZResponse UnLockAcceptedStatus(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZRequestSupportUnLockAcceptedStatusRequest>(data);
        //    return XHelper.Invoke("POST", "RequestSupport/UnLockAcceptedStatus", req);
        //}

        public static ZResponse SendRequestSupport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportSendRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/SendRequestSupport", req);
        }

        public static ZResponse AddProcessResult(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportAddProcessResultRequest>(data);
            return XHelper.Invoke("POST", "RequestSupport/AddProcessResult", req);
        }
        
    }
}