using EMS.Entity;
using System.Collections.Generic;
using System.Web.Mvc;
namespace EMS.MVC.Frontend.Models
{
    public class EditInventoryOutputViewModel
    {
        public int InventoryOutputID { get; set; }
        public string InventoryOutputCode { get; set; }
        public int WareHouseID { get; set; }
        public WareHouseModel WareHouse { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public string DateOfInventoryOutput { get; set; }
        public string ReceiverName { get; set; }
        public string MobileNumber { get; set; }
        public string Note { get; set; }
        public List<InventoryDetailModel> ListInventoryOutputDetail { get; set; }
        public int NumberOfDetail { get; set; }
    }
}