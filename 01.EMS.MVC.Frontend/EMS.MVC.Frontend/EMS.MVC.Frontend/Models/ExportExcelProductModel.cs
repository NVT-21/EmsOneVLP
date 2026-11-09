using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ExportExcelProductModel
    {
        public string WareHouseName { get; set; }
        public string ProductGroupName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public long Price { get; set; }
        public string OptionName1 { get; set; }
        public string OptionValue1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionValue2 { get; set; }
        public string OptionName3 { get; set; }
        public string OptionValue3 { get; set; }
        public string Images { get; set; }
        public string Long { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string ProductUnitName { get; set; }
        public string BatchNumber { get; set; }
        public string NumberOfProduct { get; set; }
        public string PalletCode { get; set; }
        public string MessageError { get; set; }
    }
}