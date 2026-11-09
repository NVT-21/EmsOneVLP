using System.Collections.Generic;
using System.Web.Mvc;
namespace EMS.MVC.Frontend.Models
{
    public class AddDetailInventoryImportViewModel
    {
        public int NumberOfDetail { get; set; }
        public int WareHouseID { get; set; }
        public int ProductVariantID { get; set; }
        public List<ProductModel> ListProduct { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public string PalletCode { get; set; }
    }
}