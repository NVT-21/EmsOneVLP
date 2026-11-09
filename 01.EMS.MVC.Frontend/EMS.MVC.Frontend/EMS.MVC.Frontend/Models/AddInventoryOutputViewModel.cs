using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    public class AddInventoryOutputViewModel
    {
        public int WareHouseID { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public string FullAddress { get; set; }
        public List<ZProductVariantGetByWareHouseIDResponse> ListProductVariant { get; set; }
        public string DateOfInventoryOutput { get; set; }
        public string ReceiverName { get; set; }
        public string MobileNumber { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }
        public int CreatedBy { get; set; }
        public int ShopID { get; set; }
        public int NumberOfDetail { get; set; }
        public List<AddDetailInventoryOutputViewModel> ListInventoryOutputDetail { get; set; }
    }
}