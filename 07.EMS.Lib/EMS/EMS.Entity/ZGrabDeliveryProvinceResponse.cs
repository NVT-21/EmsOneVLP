using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZGrabDeliveryProvinceResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<ZGrabDeliveryProvinceData> data { get; set; }
    }

    public class ZGrabDeliveryProvinceData
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}
