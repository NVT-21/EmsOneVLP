using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ExtraServiceFeeConfigViewModel
    {
        public int PostageID { get; set; }
        public string ServiceName { get; set; }
        public int ChargingConditionsID { get; set; }
        public string ServicesUsed { get; set; }
    }
}