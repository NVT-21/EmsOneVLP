using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ImportExcelProviderModel
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public string Note { get; set; }
        public string IsLock { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }

        public string msgError { get; set; }
    }
}