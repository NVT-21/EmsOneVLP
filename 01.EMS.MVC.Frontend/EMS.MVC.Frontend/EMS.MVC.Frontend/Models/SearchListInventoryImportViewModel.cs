using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListInventoryImportViewModel
    {
        public int InventoryImportID { get; set; }
        public string InventoryImportCode { get; set; }
        public WareHouseModel WareHouse { get; set; }
        public ProviderModel Provider { get; set; }
        public BaseModel Status { get; set; }
        public long TotalAmount { get; set; }
        public string DateOfInventoryImport { get; set; }
        public string CreaterName { get; set; }
        public string CreatedDate { get; set; }
        public string SearchKey { get; set; }
        
    }
}