using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.MVC.Frontend.Models;

namespace EMS.MVC.Frontend.Models
{
    public class ImportExcelInventoryOutputModel
    {
        public string CustomerName { get; set; }
        public int WareHouseID { get; set; }
        public int SKUID { get; set; }
        public int SKUInventory { get; set; }
        public int NumberOfOutput { get; set; }
        public string InventoryOutputCode { get; set; }
        public string InventoryOutputDate { get; set; }
        public int NumberOfDateInWareHouse { get; set; }
        public string TermInWareHouse { get; set; }
        public string PalletCode { get; set; }
        public string SKUName { get; set; }
        public string NumberOfRow { get; set; }
        public List<ImportExcelInventoryOutputDetailModel> ListInventoryOutputDetail { get; set; }
    }
}