using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class XShopInformationModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string TaxCode { get; set; }
        [DataMember]
        public string CrmOrPayPostCode { get; set; }
        [DataMember]
        public string ContractNumber { get; set; }
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int PickupAddressID { get; set; }
        [DataMember]
        public string PickupName { get; set; }
        [DataMember]
        public string PickupFullName { get; set; }
        [DataMember]
        public string PickupMobileNumber { get; set; }
        [DataMember]
        public int PickupProvinceID { get; set; }
        [DataMember]
        public string PickupProvinceName { get; set; }
        [DataMember]
        public int PickupDistrictID { get; set; }
        [DataMember]
        public string PickupDistrictName { get; set; }
        [DataMember]
        public int PickupWardID { get; set; }
        [DataMember]
        public string PickupWardName { get; set; }
        [DataMember]
        public string PickupStreet { get; set; }
        [DataMember]
        public string PickupFullAddress { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public string PrintTypeDefault { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int TypeShop { get; set; }
    }


}
