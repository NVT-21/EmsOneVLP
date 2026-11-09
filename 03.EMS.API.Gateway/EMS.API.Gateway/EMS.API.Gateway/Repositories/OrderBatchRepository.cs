using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.Gateway.Repositories
{
    public class OrderBatchRepository
    {
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderBatchAddRequest>(data);
            return XHelper.Invoke("POST", "OrderBatch/Add", req);
        }
        public static ZResponse EditInfo(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderBatchEditInfoRequest>(data);
            return XHelper.Invoke("POST", "OrderBatch/EditInfo", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderBatchEditRequest>(data);
            return XHelper.Invoke("POST", "OrderBatch/Edit", req);
        }
        public static ZResponse CoppyInfo(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderBatchCoppyInfoRequest>(data);
            return XHelper.Invoke("POST", "OrderBatch/CoppyInfo", req);
        }
        public static ZResponse DetailByBatchCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderBatchDetailByBatchCodeRequest>(data);
            return XHelper.Invoke("POST", "OrderBatch/DetailByBatchCode", req);
        }
        
    }
}