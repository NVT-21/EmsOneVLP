using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTikProductProcessListRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZTikProductProcessListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ProductAvatar { get; set; }
        [DataMember]
        public string SKU { get; set; }
        [DataMember]
        public string Options { get; set; }
        [DataMember]
        public string TiktokProductID { get; set; }
        [DataMember]
        public string TiktokProductName { get; set; }
        [DataMember]
        public string TiktokProductAvatar { get; set; }
        [DataMember]
        public string TiktokSKUID { get; set; }
        [DataMember]
        public string TiktokSellerSKU { get; set; }
        [DataMember]
        public string TiktokOptions { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusName { get; set; }
    }
}
