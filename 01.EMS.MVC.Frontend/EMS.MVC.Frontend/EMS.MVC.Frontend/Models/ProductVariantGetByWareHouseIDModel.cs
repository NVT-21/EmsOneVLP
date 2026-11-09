using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductVariantGetByWareHouseIDModel
    {
        public string DetailID { get; set; }
        public string DetailName { get; set; }
        public string ProductVariantSKU { get; set; }
        public long ProductVariantPrice { get; set; }
        public int NumberOfInventory { get; set; }
    }
}