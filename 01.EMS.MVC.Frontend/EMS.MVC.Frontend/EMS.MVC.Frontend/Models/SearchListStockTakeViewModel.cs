using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListStockTakeViewModel
    {
        public int StockTakeID { get; set; }
        public string StockTakeCode { get; set; }
        public string WareHouseName { get; set; }
        public int TotalProduct { get; set; }
        public long TotalInventory { get; set; }
        public long TotalActual { get; set; }
        public long TotalDifferent { get; set; }
        public long TotalMoneyDifferent { get; set; }
        public BaseModel Status { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedName { get; set; }
        public string UpdatedDate { get; set; }
        public string SearchKey { get; set; }
    }
}