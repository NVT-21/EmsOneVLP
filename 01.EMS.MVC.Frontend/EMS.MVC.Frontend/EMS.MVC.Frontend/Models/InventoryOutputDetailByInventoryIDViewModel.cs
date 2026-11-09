using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class InventoryOutputDetailByInventoryIDViewModel
    {
        public int ID { get; set; }
        public XProductVariantModel ProductVariant { get; set; }
        public long Quantity { get; set; }
        public long Price { get; set; }
        public long IntoMoney { get; set; }
        public long WarehouseQuantity { get; set; }
    }
}