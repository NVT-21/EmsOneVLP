using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class ShopIndexViewModel
    {   
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CustomerCode { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string MobileNumber { get; set; }
        public string AccountStatus { get; set; }
        public List<SelectListItem> ListAccountStatus { get; set; }
    }
}