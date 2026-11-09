using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZGrabDeliveryStatusResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<ZGrabDeliveryStatusData> data { get; set; }
    }
    public class ZGrabDeliveryStatusData
    {
        public string code { get; set; }
        public string note { get; set; }
    }
}
