using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductVariantAddModel
    {
        public int ProductID { get; set; }
        public long Price { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string SKU { get; set; }
        public string Avatar { get; set; }
        public string BarCode { get; set; }
        public string Options { get; set; }
        public string OptionName1 { get; set; }
        public string OptionValue1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionValue2 { get; set; }
        public string OptionName3 { get; set; }
        public string OptionValue3 { get; set; }
    }
}