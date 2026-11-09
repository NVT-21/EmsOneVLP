using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace EMS.MVC.Frontend.Models
{
    public class ImportExcelViewModel
    {
        public string Version { get; set; }
        public string Document { get; set; }
        public string FileName { get; set; }
        public string License { get; set; }
        public int TypeShop { get; set; }
        public List<JSpreadSheetModel> ListProvince { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListProduct { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListProductGroup { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListProductUnit { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListChannel { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListActive { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListCollectionType { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListMainTransportService { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListExtraTransportService { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListTransportPayer { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListWareHouse { get; set; } = new List<JSpreadSheetModel>();
        public List<JSpreadSheetModel> ListCustomerSticker { get; set; } = new List<JSpreadSheetModel>();
    }
}