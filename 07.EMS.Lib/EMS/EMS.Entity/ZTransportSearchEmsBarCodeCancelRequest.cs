using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportSearchEmsBarCodeCancelRequest
    {
        public string EMSOneCode { get; set; }
    }
    public class ZTransportSearchEmsBarCodeCancelResponse
    {
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public string Code { get; set; }
    }
}
