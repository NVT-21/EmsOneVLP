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
    public class ZTikProductUpdateConnectionResponse
    {
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ProductVariantSKU { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTikProductUpdateConnectionRequest
    {
        [DataMember]
        public string TypeUpdate { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public List<ZTikProductUpdateConnectionListItemRequest> ListItem { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZTikProductUpdateConnectionListItemRequest
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int ProductVariantID { get; set; }
        [DataMember]
        public string ProductVariantSKU { get; set; }
        [DataMember]
        public string TiktokProductID { get; set; }
        [DataMember]
        public string TiktokSKUID { get; set; }
    }
}
