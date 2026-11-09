using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ConfigMainFeeViewModel
    {
        public int TransportPostageID { get; set; }
        public string TransportPostageCode { get; set; }
        public string TransportPostageName { get; set; }
        public int TransportPostageRange { get; set; }
        public string TransportServiceName { get; set; }
        public int ChargingConditionsID { get; set; }
        public string ServicesUsed { get; set; }
    }
}