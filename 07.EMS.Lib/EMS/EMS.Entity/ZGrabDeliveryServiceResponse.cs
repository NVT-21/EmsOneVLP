using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZGrabDeliveryServiceResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<ZGrabDeliveryServiceData> data { get; set; }
    }

    public class ZGrabDeliveryServiceData
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}
