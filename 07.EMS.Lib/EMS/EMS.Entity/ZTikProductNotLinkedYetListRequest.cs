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
    public class ZTikProductNotLinkedYetListRequest
    {
        [DataMember]
        public int ShopID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZTikProductNotLinkedYetListResponse
    {
        [DataMember]
        public int ProductVariantID { get; set; }
        [DataMember]
        public string ProductVariantSKU { get; set; }
        [DataMember]
        public string Options { get; set; }
        [DataMember]
        public string ProductName { get; set; }
    }
}
