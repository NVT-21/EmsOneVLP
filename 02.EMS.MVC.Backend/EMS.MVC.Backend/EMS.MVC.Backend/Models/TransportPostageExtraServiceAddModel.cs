using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageExtraServiceAddModel
    {
        public List<int> ExtraServiceIDs { get; set; }
        public string VAT { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ChargingConditionsID { get; set; }
        public double ChargingConditionsValue { get; set; }
        public long ChargingConditionsMinValue { get; set; }
        public string ServicesUsed { get; set; }
    }
}