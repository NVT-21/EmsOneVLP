using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductGroupName { get; set; }
        public int VariantID { get; set; }
        public string VariantName { get; set; }
        public int WareHouseQuantity { get; set; }
        public int NumberOfProduct { get; set; }
        public long PriceProduct { get; set; }
        public long TotalPrice { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int NumberOfVariant { get; set; }
    }
}