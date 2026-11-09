using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportServiceCheckByCodeRequest
    {
        public int ShopID { get; set; }
        public string TransportServiceCode { get; set; }
    }
    public class ZTransportServiceCheckByCodeResponse
    {
        public string TransportServiceID { get; set; }
    }
}
