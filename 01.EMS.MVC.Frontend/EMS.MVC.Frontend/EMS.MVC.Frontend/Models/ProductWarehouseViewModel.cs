using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductWarehouseViewModel
    {
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
        public string IsChecked { get; set; }
    }
}