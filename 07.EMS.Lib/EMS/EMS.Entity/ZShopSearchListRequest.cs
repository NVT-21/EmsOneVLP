using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZShopSearchListRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public string DateType { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZShopSearchListResponse
    {
        [DataMember]
        public int ID { get; set; }
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
        public string Code { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public ZBaseResponse AccountType { get; set; }
        [DataMember]
        public ZBaseResponse NumberOfEmployees { get; set; }
        [DataMember]
        public string ReasonReject { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string CollectionPostOfficeCode { get; set; }
        [DataMember]
        public ZBaseResponse TypeShop { get; set; }
        [DataMember]
        public string ChildCustomerCode { get; set; }
        [DataMember]
        public string EmailBCG { get; set; }
        [DataMember]
        public string EmailCSKH { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string CreatedPhone { get; set; }
        [DataMember]
        public string CreatedEmail { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string ConfirmedName { get; set; }
        [DataMember]
        public string ConfirmedPhone { get; set; }
        [DataMember]
        public string ConfirmedEmail { get; set; }
        [DataMember]
        public string ConfirmedDate { get; set; }
        
    }
}
