using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;

namespace EMS.MVC.Frontend.Models
{
    public class DetailStockTakeViewModel
    {
        public int StockTakeID { get; set; }
        public string StockTakeCode { get; set; }
        public WareHouseModel WareHouse { get; set; }
        public string Note { get; set; }
        public BaseModel Status { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedName { get; set; }
        public string UpdatedDate { get; set; }
        public List<StockTakeDetailModel> ListStockTakeDetail { get; set; }
        public int TotalProduct { get; set; }
        public long TotalInventory { get; set; }
        public long TotalActual { get; set; }
        public long TotalDifferent { get; set; }
        public long TotalMoney { get; set; }

    }
}