using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ChargedModel
    {
        public int MainServiceID { get; set; }
        public string ExtraServiceIDs { get; set; }
        public int FromProvinceID { get; set; }
        public int ToProvinceID { get; set; }
        public int ToDistrictID { get; set; }
        public int ToWardID { get; set; }
        public int Weight { get; set; }
        public string CustomerCode { get; set; }
        public int Quantity { get; set; }
        public long Amount { get; set; }
        public long COD { get; set; }
    }
}