using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderEditReceiverAddressRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
}
