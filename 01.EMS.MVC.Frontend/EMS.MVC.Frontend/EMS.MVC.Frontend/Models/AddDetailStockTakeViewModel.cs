using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class AddDetailStockTakeViewModel
    {
        public int NumberOfDetail { get; set; }
        public int ProductVariantID { get; set; }
        public int Quantity { get; set; }
        public List<ZProductVariantGetByWareHouseIDResponse> ListProductVariant { get; set; }
    }
}