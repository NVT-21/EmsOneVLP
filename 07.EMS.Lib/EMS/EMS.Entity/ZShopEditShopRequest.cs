using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZShopEditShopRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string ContractNumber { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CustomerToken { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string ProductBusiness { get; set; }
        [DataMember]
        public string TypeBusiness { get; set; }
        [DataMember]
        public int TypeShop { get; set; }
        [DataMember]
        public int AccountType { get; set; }
        [DataMember]
        public int NumberOfEmployees { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string MainTransportServiceID { get; set; }
        [DataMember]
        public string ExtraTransportServiceID { get; set; }
        [DataMember]
        public int CODPeriodType { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public int CollectionPostOfficeID { get; set; }
        [DataMember]
        public string ChildCustomerCode { get; set; }
        [DataMember]
        public int FeePeriodType { get; set; }
        [DataMember]
        public int PaymentMethod { get; set; }
        [DataMember]
        public string FullNameOfPayment { get; set; }
        [DataMember]
        public string NameOfPayment { get; set; }
        [DataMember]
        public string BranchOfPayment { get; set; }
        [DataMember]
        public string NumberOfPayment { get; set; }
        [DataMember]
        public string EmailBCG { get; set; }
        [DataMember]
        public string EmailCSKH { get; set; }
        [DataMember]
        public string TaxCode { get; set; }
        [DataMember]
        public int PartnerID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
