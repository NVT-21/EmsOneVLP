using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopOrderDetailRequest_v202309
    {
        public string ids { get; set; }
    }

    public class ZTiktokShopOrderDetailResponse_v202309
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopOrderDetailDataResponse_v202309 Data { get; set; }
    }
    public class ZTiktokShopOrderDetailDataResponse_v202309
    {
        [JsonProperty("orders")]
        public List<ZTiktokShopOrderListDataOrdersResponse_v202309> Orders { get; set; }
    }
    
}
