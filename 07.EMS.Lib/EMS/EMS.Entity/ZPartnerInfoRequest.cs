using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerInfoRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string CustomerToken { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerInfoResponse
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public int TypeShop { get; set; }
        [DataMember]
        public int DefaultWareHouseID { get; set; }
        [DataMember]
        public int StoreOwnerID { get; set; }
        [DataMember]
        public ZPickUpAddressRequest PickUpAddress { get; set; }
    }
}
