using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;

namespace EMS.MVC.Frontend.Models
{
    public class EditStockTakeViewModel
    {
        public int StockTakeID { get; set; }
        public string StockTakeCode { get; set; }
        public int WareHouseID { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public string Note { get; set; }
        public BaseModel Status { get; set; }
        public List<StockTakeDetailModel> ListDetailStockTake { get; set; }
        public string EditType { get; set; }
        public int NumberOfDetail { get; set; }
        public int TotalProduct { get; set; }
        public long TotalInventory { get; set; }
        public long TotalActual { get; set; }
        public long TotalDifferent { get; set; }
        public long TotalMoney { get; set; }
    }
}