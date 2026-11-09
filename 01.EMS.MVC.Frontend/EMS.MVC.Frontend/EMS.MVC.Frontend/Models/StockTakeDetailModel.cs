using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class StockTakeDetailModel
    {
        public ProductModel Product { get; set; }
        public int ProductVariantID { get; set; }
        public ProductVariantModel ProductVariant { get; set; }
        public int Quantity { get; set; }
        public long NumberOfInventory { get; set; }
        public long NumberOfActual { get; set; }
        public long NumberOfDifferent { get; set; }
        public long MoneyOfDifferent { get; set; }
    }
}