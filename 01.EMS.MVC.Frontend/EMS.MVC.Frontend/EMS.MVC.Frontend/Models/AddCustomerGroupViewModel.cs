using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class AddCustomerGroupViewModel
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int OrderSuccess { get; set; }
        public int PurchaseMoney { get; set; }
    }
}