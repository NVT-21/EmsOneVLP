using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ShopProfileViewModel
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string StoreName { get; set; }
        public string FullNameOfPayment { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ProductBusiness { get; set; }
        public string TypeBusiness { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int NumberOfEmployees { get; set; }
        public int NumberOfEmployeesCreated { get; set; }
        public string PickUpFullAddress { get; set; }
        public string AccountType { get; set; }
        public int PaymentMethod { get; set; }
        public int PaymentType { get; set; }
        public string MobileNumberOfPayment { get; set; }
        public string NameOfPayment { get; set; }
        public string BranchOfPayment { get; set; }
        public string NumberOfPayment { get; set; }
        public int CollectionType { get; set; }
        public string CollectionTypeName { get; set; }
        public int PickUpID { get; set; }
        public string ChildCustomerCode { get; set; }
        public int ChildCustomerCodeQuantity { get; set; }
    }
}