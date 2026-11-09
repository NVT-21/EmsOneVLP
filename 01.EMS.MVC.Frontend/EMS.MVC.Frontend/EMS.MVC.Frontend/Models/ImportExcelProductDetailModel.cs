using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ImportExcelProductDetailModel
    {
        public ProductGroupModel ProductGroup { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public long Price { get; set; }
        public BaseModel Option1 { get; set; }
        public BaseModel Option2 { get; set; }
        public BaseModel Option3 { get; set; }
        public List<ProductVariantModel> ListProductVariant { get; set; }
        public string Images { get; set; }
        public string Long { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Size { get; set; }
        public ProductUnitModel ProductUnit { get; set; }
        public string BatchNumber { get; set; }
        public string PalletCode { get; set; }
        public string NumberOfProduct { get; set; }
        public string MessageError { get; set; }
    }
}