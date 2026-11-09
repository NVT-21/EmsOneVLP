using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportPostageExtraServiceByIDRequest
    {
        public int PostageID { get; set; }
    }
    public class ZTransportPostageExtraServiceByIDResponse
    {
        public int PostageID { get; set; }
        public string VAT { get; set; }
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ChargingConditionsID { get; set; }
        public double ChargingConditionsValue { get; set; }
        public string ServicesUsed { get; set; }
        public long ChargingConditionsMinValue { get; set; }
    }
}
