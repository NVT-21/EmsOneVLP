using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class EditDetailStockTakeViewModel
    {
        public int NumberOfDetail { get; set; }
        public List<StockTakeDetailModel> ListDetailStockTake { get; set; }
        public List<ZProductVariantGetByWareHouseIDResponse> ListProductVariant { get; set; }
    }
}