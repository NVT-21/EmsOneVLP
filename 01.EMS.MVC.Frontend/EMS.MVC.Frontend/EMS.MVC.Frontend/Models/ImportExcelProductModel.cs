using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ImportExcelProductModel
    {
        public WareHouseModel WareHouse { get; set; }
        public List<ImportExcelProductDetailModel> ListProduct { get; set; }
    }
}