using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZGrabDeliveryDistrictResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<ZGrabDeliveryDistrictData> data { get; set; }
    }

    public class ZGrabDeliveryDistrictData
    {
        public string code { get; set; }
        public string name { get; set; }
        public string province_code { get; set; }
    }
}
