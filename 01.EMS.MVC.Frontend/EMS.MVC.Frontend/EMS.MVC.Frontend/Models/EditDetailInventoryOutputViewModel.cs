using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class EditDetailInventoryOutputViewModel
    {
        public int NumberOfDetail { get; set; }
        public List<InventoryDetailModel> ListInventoryOutputDetail { get; set; }
        public List<ZProductVariantGetByWareHouseIDResponse> ListProductVariant { get; set; }
    }
}