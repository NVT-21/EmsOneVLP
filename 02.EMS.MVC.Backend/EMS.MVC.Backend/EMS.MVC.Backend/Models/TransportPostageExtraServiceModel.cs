using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageExtraServiceModel
    {
        public int PostageID { get; set; }
        public string VAT { get; set; }
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ChargingConditionsID { get; set; }
        public double ChargingConditionsValue { get; set; }
        public long ChargingConditionsMinValue { get; set; }
        public string ServicesUsed { get; set; }
        public int UpdateBy { get; set; }
    }
}