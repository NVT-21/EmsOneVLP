using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZShopGetByCrmOrPaypostCodeRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZShopGetByCrmOrPaypostCodeResponse
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
        public int PickUpAddressID { get; set; }
        [DataMember]
        public string PickUpAddressFullName { get; set; }
        [DataMember]
        public string PickUpAddressMobileNumber { get; set; }
        [DataMember]
        public int PickUpAddressProvinceID { get; set; }
        [DataMember]
        public int PickUpAddressDistrictID { get; set; }
        [DataMember]
        public int PickUpAddressWardID { get; set; }
        [DataMember]
        public string PickUpAddressStreet { get; set; }
    }
}
