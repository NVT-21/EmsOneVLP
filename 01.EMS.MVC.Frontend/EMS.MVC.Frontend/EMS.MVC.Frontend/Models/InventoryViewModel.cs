using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class InventoryViewModel
    {
        public string AllDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int WareHouseID { get; set; }
        public string WareHouseName { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public string SearchKey { get; set; }
    }
}