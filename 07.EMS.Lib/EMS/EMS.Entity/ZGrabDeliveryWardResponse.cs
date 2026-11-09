using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZGrabDeliveryWardResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<ZGrabDeliveryWardData> data { get; set; }
    }

    public class ZGrabDeliveryWardData
    {
        public string code { get; set; }
        public string name { get; set; }
        public string province_code { get; set; }
        public string district_code { get; set; }
    }
}
