using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.Gateway.Repositories
{
    public class TransportPostageRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageSearchListRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/SearchList", req);
        }
        public static ZResponse MainServiceBasicAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageMainServiceBasicAddRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/MainServiceBasicAdd", req);
        }
        public static ZResponse MainServiceBasicEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageMainServiceBasicEditRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/MainServiceBasicEdit", req);
        }
        public static ZResponse MainServiceFeeEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageMainServiceFeeEditRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/MainServiceFeeEdit", req);
        }
        public static ZResponse MainServiceSurchargeEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageSurchargeEditRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/MainServiceSurchargeEdit", req);
        }
        public static ZResponse ExtraServiceBasicAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageExtraServiceBasicAddRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/ExtraServiceBasicAdd", req);
        }
        public static ZResponse ExtraServiceBasicEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageExtraServiceEditRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/ExtraServiceBasicEdit", req);
        }

        public static ZResponse GetByTransportPostageID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("TransportPostage/GetByTransportPostageID?TransportPostageID={0}", req.TransportPostageID), null);
        }
        public static ZResponse ChangeIsLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageChangeIsLockRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/ChangeIsLock", req);
        }
        public static ZResponse Confirm(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageConfirmRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/Confirm", req);
        }
        public static ZResponse UnConfirm(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageUnConfirmRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/UnConfirm", req);
        }
        public static ZResponse SearchHistory(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageSearchHistoryRequest>(data);
            return XHelper.Invoke("POST", "TransportPostage/SearchHistory", req);
        }
    }
}