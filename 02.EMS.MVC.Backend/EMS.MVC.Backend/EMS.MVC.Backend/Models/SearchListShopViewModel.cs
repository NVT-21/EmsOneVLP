using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace EMS.MVC.Backend.Models
{
    public class SearchListShopViewModel
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ProductBusiness { get; set; }
        public string TypeBusiness { get; set; }
        public string Code { get; set; }
        public string CustomerCode { get; set; }
        public BaseModel Status { get; set; }
        public string IsLock { get; set; }
        public BaseModel AccountType { get; set; }
        public BaseModel NumberOfEmployee { get; set; }
        public string CreatedName { get; set; }
        public string CreatedPhone { get; set; }
        public string CreatedEmail { get; set; }
        public string CreatedDate { get; set; }
        public string ConfirmedName { get; set; }
        public string ConfirmedPhone { get; set; }
        public string ConfirmedEmail { get; set; }
        public string ConfirmedDate { get; set; }
        public string ReasonReject { get; set; }
    }
}