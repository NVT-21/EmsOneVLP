using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class EditDetailInventoryImportViewModel
    {
        public int NumberOfDetail { get; set; }
        public int ProductVariantID { get; set; }
        public List<ProductModel> ListProduct { get; set; }
        public List<InventoryDetailModel> ListInventoryImportDetail { get; set; }
    }
}