using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportPostageExtraServiceAddRequest
    {
        public int ExtraServiceID { get; set; }
        public string VAT { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ChargingConditionsID { get; set; }
        public double ChargingConditionsValue { get; set; }
        public long ChargingConditionsMinValue { get; set; }
        public string ServicesUsed { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ZTransportPostageExtraServiceAddResponse
    {
        public string ExtraServiceName { get; set; }
    }
}
