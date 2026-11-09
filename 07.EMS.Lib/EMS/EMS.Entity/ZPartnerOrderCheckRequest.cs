using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPartnerOrderCheckRequest
    {
        public string CrmOrPaypostCode { get; set; }
        public string OrderCode { get; set; }
        public string TransportMainService { get; set; }
        public string TransportExtraService { get; set; }
    }
}
