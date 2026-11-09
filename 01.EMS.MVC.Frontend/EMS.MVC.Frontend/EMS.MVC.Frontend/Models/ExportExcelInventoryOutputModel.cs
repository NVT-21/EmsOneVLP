using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ExportExcelInventoryOutputModel
    {
        public string CustomerName { get; set; }
        public string WareHouseCode { get; set; }
        public string SKUCode { get; set; }
        public int NumberOfOutput { get; set; }
        public string InventoryOutputCode { get; set; }
        public string InventoryOutputDate { get; set; }
        public int NumberOfDateInWareHouse { get; set; }
        public string TermInWareHouse { get; set; }
        public string PalletCode { get; set; }
        public string MessageError { get; set; }
    }
}