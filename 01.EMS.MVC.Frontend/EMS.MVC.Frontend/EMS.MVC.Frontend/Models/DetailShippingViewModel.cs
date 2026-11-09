using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class DetailShippingViewModel
    {
        public string ShippingCode { get; set; }
        public string ShopID { get; set; }
        public int EmployeeID { get; set; }
    }
}