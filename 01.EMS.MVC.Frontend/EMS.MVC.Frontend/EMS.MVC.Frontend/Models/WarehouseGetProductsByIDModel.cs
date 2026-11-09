using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class WarehouseGetProductsByIDModel
    {
        public int WarehouseID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int ShopID { get; set; }
        public string SearchCode { get; set; }
    }
}