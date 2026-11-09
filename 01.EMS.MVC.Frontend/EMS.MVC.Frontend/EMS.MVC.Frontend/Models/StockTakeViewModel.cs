using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    public class StockTakeViewModel
    {
        public string AllDate { get; set; }
        public string StockTakeCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public List<SelectListItem> ListStockTakeStatus { get; set; }
        public int WareHouseID { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public string DateType { get; set; }
        public List<SelectListItem> ListDateType { get; set; }
    }
}