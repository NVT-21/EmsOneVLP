using EMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class DetailShopViewModel
    {
        public int ID { get; set; }
        public string ContractNumber { get; set; }
        public string Code { get; set; }
        public string CustomerCode { get; set; }
        public string FullName { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ProductBusiness { get; set; }
        public string TypeBusiness { get; set; }
        public int TypeShop { get; set; }
        //public string Note { get; set; }
        public string Status { get; set; }
        public string IsLock { get; set; }
        public int AccountType { get; set; }
        public string AccountTypeName
        {
            get
            {
                return XData.AccountTypeDictionary[AccountType];
            }
        }
        public int NumberOfEmployees { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Street { get; set; }
        //public string PickUpAddress { get; set; }
        public string MainTransportServiceID { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string ExtraTransportServiceID { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        public int IsExistExtraTransportServiceID { get; set; }
        public string strNumberOfEmployees { get; set; }
        public string ConfirmedName { get; set; }
        public string ReasonReject { get; set; }
        public int CollectionType { get; set; }
        public string CollectionTypeName { get; set; }
        //public string CollectionPostcode { get; set; }
        public int CollectionPostOfficeID { get; set; }
        public List<CollectionPostOfficeModel> ListCollectionPostOffice { get; set; }
        public string ChildCustomerCode { get; set; }
        public int CODPeriodType { get; set; }
        public string CODPeriodTypeName { get; set; }
        public int FeePeriodType { get; set; }
        public string FeePeriodTypeName { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public string FullNameOfPayment { get; set; }
        public string NameOfPayment { get; set; }
        public string BranchOfPayment { get; set; }
        public string NumberOfPayment { get; set; }
        public List<SelectListItem> ListAccountType { get; set; }
        public List<SelectListItem> ListNumberOfEmployee { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public List<WardModel> ListWard { get; set; }
        public List<SelectListItem> PeriodType { get; set; }
        public List<SelectListItem> ListPaymentMethod { get; set; }
        public List<SelectListItem> ListCollectionType { get; set; }
        public string EmailBCG { get; set; }
        public string EmailCSKH { get; set; }
        public string TaxCode { get; set; }
        public string CustomerToken { get; set; }
        public int PartnerID { get; set; }
        public string IsSercurity { get; set; } = "N";
        public List<PartnerModel> ListPartner { get; set; }
        public List<SelectListItem> ListIsSercurity { get; set; }
    }
}