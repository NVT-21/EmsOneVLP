using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ImportExcelInventoryOutputDetailModel
    {
        public int SKUID { get; set; }
        public string SKUName { get; set; }
        public int SKUInventory { get; set; }
        public int NumberOfOutput { get; set; }
        public int NumberOfDateInWareHouse { get; set; }
        public string TermInWareHouse { get; set; }
        public string PalletCode { get; set; }
    }
}