using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloGetInfoOARequest
    {

    }
    public class ZZaloGetInfoOAResponse
    {
        public ZZaloGetInfoOAData data { get; set; }
        public string error { get; set; }
        public string message { get; set; }
    }
    public class ZZaloGetInfoOAData
    {
        public string oa_id { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string cover { get; set; }
        public bool is_verified { get; set; }
        public string description { get; set; }
    }
}
