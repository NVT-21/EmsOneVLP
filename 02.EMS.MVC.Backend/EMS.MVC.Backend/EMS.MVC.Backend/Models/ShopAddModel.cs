using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ShopAddModel
    {
        public string ContractNumber { get; set; }
        public string CustomerCode { get; set; }
        public string FullName { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string ProductBusiness { get; set; }
        public string TypeBusiness { get; set; }
        public int TypeShop { get; set; }
        public string Note { get; set; }
        public int AccountType { get; set; }
        public int NumberOfEmployees { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string Street { get; set; }
        public string ChildCustomerCode { get; set; }
        public int CollectionType { get; set; }
        public string CollectionPostcode { get; set; }
        public int CODPeriodType { get; set; }
        public int FeePeriodType { get; set; }
        public int PaymentMethod { get; set; }
        public string FullNameOfPayment { get; set; }
        public string NameOfPayment { get; set; }
        public string BranchOfPayment { get; set; }
        public string NumberOfPayment { get; set; }
    }
}