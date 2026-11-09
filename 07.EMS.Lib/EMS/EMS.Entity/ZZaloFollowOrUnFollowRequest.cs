using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloFollowOrUnFollowRequest
    {
        public string app_id { get; set; }
        public string oa_id { get; set; }
        public string user_id_by_app { get; set; }
        public string event_name { get; set; }
        public string timestamp { get; set; }
        public string source { get; set; }
        public XZaloFollowerModel follower { get; set; }
    }
}
