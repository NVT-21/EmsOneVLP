using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class EditInventoryImportViewModel
    {
        public int InventoryID { get; set; }
        public string InventoryCode { get; set; }
        public int ProviderID { get; set; }
        public List<ProviderModel> ListProvider { get; set; }
        public int WareHouseID { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public string DateOfInventoryImport { get; set; }
        public string ImporterName { get; set; }
        public string MobileNumber { get; set; }
        public string Note { get; set; }
        public List<InventoryDetailModel> ListInventoryImportDetail { get; set; }
        public int NumberOfDetail { get; set; }
    }
}