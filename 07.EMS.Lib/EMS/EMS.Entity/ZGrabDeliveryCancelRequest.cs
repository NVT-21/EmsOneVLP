using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZGrabDeliveryCancelRequest
    {
        public string id { get; set; }
    }

    public class ZGrabDeliveryCancelResponse
    {
        public string code { get; set; }
        public string message { get; set; }
    }
}
