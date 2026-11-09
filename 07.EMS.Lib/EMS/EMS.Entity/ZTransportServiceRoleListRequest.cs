using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportServiceRoleListRequest
    {
        public int ShopID { get; set; }
    }
    public class ZTransportServiceRoleListResponse
    {
        public int TransportServiceRoleID { get; set; }
        public int TransportServiceID { get; set; }
        public int TransportServiceIndex { get; set; }
        public int TransportServiceLevel { get; set; }
        public string IsShow { get; set; }
        public string IsChoose { get; set; }
    }
}
