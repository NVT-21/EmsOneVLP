using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCustomerGetByIDRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZCustomerGetByIDResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string BirthDay { get; set; }
        [DataMember]
        public string TaxCode { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Sticker { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string BlackList { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string GroupName { get; set; }
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
        public string Address { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        //[DataMember]
        //public int CountOrderSuccess { get; set; }
        //[DataMember]
        //public long TotalAmountSuccess { get; set; }
    }
}
