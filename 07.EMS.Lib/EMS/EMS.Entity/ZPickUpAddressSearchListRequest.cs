using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPickUpAddressSearchListRequest
    {
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPickUpAddressSearchListResponse
    {
        [DataMember]
        public int PickUpAddressID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string PickUpName { get; set; }
        [DataMember]
        public int CollectionPostOfficeID { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public string ProvinceName { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public string DistrictName { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string WardName { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string IsDefault { get; set; }
        [DataMember]
        public int QuantityLabelPrint { get; set; }
    }
}
