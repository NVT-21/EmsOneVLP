using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloSearchListFollowerRequest
    {
        public int offset { get; set; }
        public int count { get; set; }
        public string tag_name { get; set; }
        public string AccessToken { get; set; }
    }
    public class ZZaloSearchListFollowerResponse
    {
        public string error { get; set; }
        public string message { get; set; }
        public ZZaloSearchListFollowerDataResponse data { get; set; }
    }
    public class ZZaloSearchListFollowerDataResponse
    {
        public int total { get; set; }
        public List<XZaloFollowerModel> followers { get; set; }
    }
}
