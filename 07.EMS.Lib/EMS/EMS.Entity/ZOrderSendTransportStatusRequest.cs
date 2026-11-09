using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOrderSendTransportStatusRequest
    {
        public string EMSOneCode { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
    }
}
