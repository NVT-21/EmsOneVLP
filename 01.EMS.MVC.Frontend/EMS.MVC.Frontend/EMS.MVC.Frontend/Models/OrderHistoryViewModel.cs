using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class OrderHistoryViewModel
    {
        public int ID { get; set; }
        public string OrderCode { get; set; }
        public string ShippingCode { get; set; }
        public long TotalProductValue { get; set; }
        public string CreatedDate { get; set; }
    }
}