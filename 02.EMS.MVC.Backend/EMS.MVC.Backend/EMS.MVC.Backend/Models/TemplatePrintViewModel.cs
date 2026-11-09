using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TemplatePrintViewModel
    {
        public int TemplatePrintID { get; set; }
        public int PickUpAddressID { get; set; }
        public string TemplateName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Street { get; set; }
        public string FullAddress { get; set; }
        public string IsDefault { get; set; }
    }
}