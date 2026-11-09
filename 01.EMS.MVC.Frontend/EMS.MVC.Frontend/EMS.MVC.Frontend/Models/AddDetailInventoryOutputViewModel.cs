using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class AddDetailInventoryOutputViewModel
    {
        public int NumberOfDetail { get; set; }
        public int WareHouseID { get; set; }
        public int ProductVariantID { get; set; }
        public List<ZProductVariantGetByWareHouseIDResponse> ListProductVariant { get; set; }
        public int Quantity { get; set; }
        public int NumberOfDateInWareHouse { get; set; }
        public string TermInWareHouse { get; set; }
        public string PalletCode { get; set; }
    }
}