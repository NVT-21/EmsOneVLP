using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class TransportRepository
    {
        #region Postage
        //public static ZResponse PostageSurchargeByID(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZTransportPostageSurchargeByIDRequest>(data);
        //    return XHelper.Invoke("POST", "Transport/PostageSurchargeByID", req);
        //}
        
        public static ZResponse PostageExtraServiceByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageExtraServiceByIDRequest>(data);
            return XHelper.Invoke("POST", "Transport/PostageExtraServiceByID", req);
        }
        
        //public static ZResponse PostageMainFeeByID(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZTransportPostageMainFeeByIDRequest>(data);
        //    return XHelper.Invoke("POST", "Transport/PostageMainFeeByID", req);
        //}
        public static ZResponse PostageRangeAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageRangeAddRequest>(data);
            return XHelper.Invoke("POST", "Transport/PostageRangeAdd", req);
        }
        public static ZResponse PostageRangeSearch(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageRangeSearchRequest>(data);
            return XHelper.Invoke("POST", "Transport/PostageRangeSearch", req);
        }
        public static ZResponse PostageWeightAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<List<ZTransportPostageWeightAddRequest>>(data);
            return XHelper.Invoke("POST", "Transport/PostageWeightAdd", req);
        }

        public static ZResponse PostageWeightSearch(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageWeightSearchRequest>(data);
            return XHelper.Invoke("POST", "Transport/PostageWeightSearch", req);
        }

        public static ZResponse Charged(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageChargedRequest>(data);
            return XHelper.Invoke("POST", "Transport/Charged", req);
        }
        #endregion
    }
}
