using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZShopGetByIDRequest
    {
        [DataMember]
        public int ID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZShopGetByIDResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
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
        public string Status { get; set; }
        [DataMember]
        public int AccountType { get; set; }
        [DataMember]
        public int NumberOfEmployees { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int NumberOfEmployeesCreated { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ShopLevel { get; set; }
        [DataMember]
        public int PickUpID { get; set; }
        [DataMember]
        public string PickUpFullName { get; set; }
        [DataMember]
        public string PickUpMobileNumber { get; set; }
        [DataMember]
        public int PickUpProvinceID { get; set; }
        [DataMember]
        public int PickUpDistrictID { get; set; }
        [DataMember]
        public int PickUpWardID { get; set; }
        [DataMember]
        public string PickUpStreet { get; set; }
        [DataMember]
        public string PickUpFullAddress { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public string ContractNumber { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CustomerToken { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string MainTransportServiceID { get; set; }
        [DataMember]
        public string ExtraTransportServiceID { get; set; }
        [DataMember]
        public int IsExistExtraTransportServiceID { get; set; }
        [DataMember]
        public int PaymentMethod { get; set; }
        [DataMember]
        public int PaymentType { get; set; }
        [DataMember]
        public string FullNameOfPayment { get; set; }
        [DataMember]
        public string MobileNumberOfPayment { get; set; }
        [DataMember]
        public string NameOfPayment { get; set; }
        [DataMember]
        public string BranchOfPayment { get; set; }
        [DataMember]
        public string NumberOfPayment { get; set; }
        [DataMember]
        public string AutoDivision { get; set; }
        [DataMember]
        public string EmployeeReadyDivision { get; set; }
        [DataMember]
        public string EmployeeGroupPermissionID { get; set; }
        [DataMember]
        public int DateProcessRequestOrder { get; set; }
        [DataMember]
        public int AutoCriteria { get; set; }
        [DataMember]
        public string AutoTimeDivision { get; set; }
        [DataMember]
        public int CODPeriodType { get; set; }
        [DataMember]
        public int FeePeriodType { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public string CollectionTypeName { get; set; }
        [DataMember]
        public string CollectionPostcode { get; set; }
        [DataMember]
        public int CollectionPostOfficeID { get; set; }
        [DataMember]
        public string ChildCustomerCode { get; set; }
        [DataMember]
        public string EmailBCG { get; set; }
        [DataMember]
        public string EmailCSKH { get; set; }
        [DataMember]
        public string TaxCode { get; set; }
        [DataMember]
        public int PartnerID { get; set; }
    }
}
