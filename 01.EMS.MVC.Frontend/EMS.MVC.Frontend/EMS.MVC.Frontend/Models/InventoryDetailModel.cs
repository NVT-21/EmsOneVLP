using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class InventoryDetailModel
    {
        public int ID { get; set; }
        public int InventoryID { get; set; }
        public int ProductVariantID { get; set; }
        public ProductModel Product { get; set; }
        public long Quantity { get; set; }
        public long Price { get; set; }
        public long TotalAmount { get; set; }
        public int NumberOfDateInWareHouse { get; set; }
        public string TermInWareHouse { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
        public long WareHouseQuantity { get; set; }
        public string PalletCode { get; set; }
    }
}