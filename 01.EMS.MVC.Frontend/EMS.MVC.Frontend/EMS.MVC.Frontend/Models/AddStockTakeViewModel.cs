using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class AddStockTakeViewModel
    {
        public string Note { get; set; }
        public int WareHouseID { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public int NumberOfDetail { get; set; }
        public string CreateType { get; set; }
        public List<AddDetailStockTakeViewModel> ListDetailStockTake { get; set; }
    }
}