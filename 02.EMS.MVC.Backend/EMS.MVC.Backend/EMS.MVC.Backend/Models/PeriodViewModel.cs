using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class PeriodViewModel
    {
        public string Type { get; set; }
        public int PeriodType { get; set; }
        public int CODPeriodType { get; set; }
        public int FeePeriodType { get; set; }
        public int ShopID { get; set; }
    }
}