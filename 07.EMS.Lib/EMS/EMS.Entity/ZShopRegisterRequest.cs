using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZShopRegisterRequest
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string ProductBusiness { get; set; }
        [DataMember]
        public string TypeBusiness { get; set; }
        [DataMember]
        public int TypeShop { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string CRMOrPaypostCode { get; set; }
        [DataMember]
        public string CollectionPostcode { get; set; }
        [DataMember]
        public int CollectionPostOfficeID { get; set; }
    }
}
