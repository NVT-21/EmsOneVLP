using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    public class AddInventoryImportViewModel
    {
        public int ProviderID { get; set; }
        public string DateOfInventoryImport { get; set; }
        public List<ProviderModel> ListProvider { get; set; }
        public int WareHouseID { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public List<SelectListItem> ProductVariantList { get; set; }
        public string Note { get; set; }
        public string ImporterName { get; set; }
        public string MobileNumber { get; set; }
        public int NumberOfDetail { get; set; }
        public List<AddDetailInventoryImportViewModel> ListInventoryImportDetail { get; set; }
    }
}