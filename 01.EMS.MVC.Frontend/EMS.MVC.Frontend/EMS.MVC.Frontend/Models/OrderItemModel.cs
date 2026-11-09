using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class OrderItemModel
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int VariantID { get; set; }
        public string VariantSKU { get; set; }
        public string VariantName { get; set; }
        public int NumberOfProduct { get; set; }
        public long Price { get; set; }
        public long TotalPrice { get; set; }
        public int Discount { get; set; }
        public int Weight { get; set; }
        public string ProductVariantImage { get; set; }
        public int WareHouseQuantity { get; set; }
    }
}