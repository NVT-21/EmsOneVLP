using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloUserAuthenticationRequest
    {
        public XZaloHostModel host { get; set; }
        public string event_name { get; set; }
        public XZaloInfoModel user { get; set; }
        public string timestamp { get; set; }
    }
}
