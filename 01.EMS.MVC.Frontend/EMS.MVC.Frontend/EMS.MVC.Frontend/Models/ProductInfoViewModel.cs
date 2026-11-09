using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductInfoViewModel
    {
        //public int NumberOfDetail { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int VariantID { get; set; }
        public string VariantName { get; set; }
        public int NumberOfProduct { get; set; }
        public long Price { get; set; }
        public long Discount { get; set; }
        public double Weight { get; set; }
    }
}