using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace EMS.MVC.Backend.Models
{
    public class ShopViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CustomerCode { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string MobileNumber { get; set; }
        public string AccountStatus { get; set; }
        public List<SelectListItem> ListAccountStatus { get; set; }
        public string DateType { get; set; }
        public List<SelectListItem> ListDateType { get; set; }

        //public int No { get; set; }
        //public int ID { get; set; }
        //public string ContractNumber { get; set; }
        //public string Code { get; set; }
        //public string CustomerCode { get; set; }
        //public string FullName { get; set; }
        //public string StoreName { get; set; }
        //public string Email { get; set; }
        //public string MobileNumber { get; set; }
        //public string ProductBusiness { get; set; }
        //public string TypeBusiness { get; set; }
        //public int TypeShop { get; set; }
        //public string Note { get; set; }
        //public string Status { get; set; }
        //public string IsLock { get; set; }
        //public int AccountType { get; set; }
        //public string AccountTypeName
        //{
        //    get
        //    {
        //        return XData.AccountTypeDictionary[AccountType];
        //    }
        //}
        //public int NumberOfEmployees { get; set; }
        //public int ProvinceID { get; set; }
        //public int DistrictID { get; set; }
        //public int WardID { get; set; }
        //public string Street { get; set; }
        //public string Address { get; set; }
        //public string strNumberOfEmployees { get; set; }
        //public string ConfirmedName { get; set; }
        //public string ReasonReject { get; set; }
        //public int CollectionType { get; set; }
        //public string CollectionPostcode { get; set; }
        //public string ChildCustomerCode { get; set; }
        //public int CODPeriodType { get; set; }
        //public int FeePeriodType { get; set; }
        //public int PaymentMethod { get; set; }
        //public string FullNameOfPayment { get; set; }
        //public string NameOfPayment { get; set; }
        //public string BranchOfPayment { get; set; }
        //public string NumberOfPayment { get; set; }
        //public string EmailBCG { get; set; }
        //public string EmailCSKH { get; set; }
    }
}