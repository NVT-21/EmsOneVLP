using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ImportExcelCustomerModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Street { get; set; }
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public int WardID { get; set; }
        public string WardName { get; set; }
        public string BirthDay { get; set; }
        public string TaxCode { get; set; }
        public string Channel { get; set; }
        public string Note { get; set; }
        public string CustomerStickerID { get; set; }
        public string CustomerStickerName { get; set; }
        public string IsUpdate { get; set; }
        public string MessageError { get; set; }
    }
}