using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportShippingCancelDataRequest
    {
        public string TrackingCode { get; set; }
        public string CustomerCode { get; set; }
        public string SignatureCode { get; set; }
    }
}
