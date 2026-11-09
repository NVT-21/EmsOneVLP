using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductVariantModel
    {
        public int ID { get; set; }
        //public ProductModel Product { get; set; }
        public string Options { get; set; }
        public string OptionName1 { get; set; }
        public string OptionValue1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionValue2 { get; set; }
        public string OptionName3 { get; set; }
        public string OptionValue3 { get; set; }
        public string SKU { get; set; }
        public string Avatar { get; set; }
        public string BatchNumber { get; set; }
        public long Price { get; set; }
        public PalletModel Pallet { get; set; }

    }
}