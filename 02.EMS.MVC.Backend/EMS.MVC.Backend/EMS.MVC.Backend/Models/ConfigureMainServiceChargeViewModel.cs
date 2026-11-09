using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ConfigureMainServiceChargeViewModel
    {
        public int PostageID { get; set; }
        public string IsStep { get; set; }
        public int Weight { get; set; }
        public int LocalFee { get; set; }
        public int IntercityFee1 { get; set; }
        public int IntercityFee2 { get; set; }
        public int IntercityFee3 { get; set; }
        public int IntercityFee4 { get; set; }
    }
}