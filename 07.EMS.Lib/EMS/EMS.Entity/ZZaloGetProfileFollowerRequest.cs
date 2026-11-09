using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloGetProfileFollowerRequest
    {
    }
    public class ZZaloGetProfileFollowerResponse
    {
        public string error { get; set; }
        public string message { get; set; }
        public ZZaloGetProfileFollowerDataResponse data { get; set; }
    }
    public class ZZaloGetProfileFollowerDataResponse
    {
        public string user_gender { get; set; } // 0 la nu, 1 la nam
        public string user_id { get; set; }
        public string user_id_by_app { get; set; }
        public string avatar { get; set; }
        //public Dictionary<int, string> avatars { get; set; }
        public string display_name { get; set; }
        public string birth_date { get; set; }
        public ZZaloGetProfileFollowerSharedInfoResponse shared_info { get; set; }
        public ZZaloGetProfileFollowerTagsAndNotesInfoResponse tags_and_notes_info { get; set; }
        public string shop_id { get; set; }
        public string page_id { get; set; }
    }
    public class ZZaloGetProfileFollowerSharedInfoResponse
    {
        public string address { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
    }

    public class ZZaloGetProfileFollowerTagsAndNotesInfoResponse
    {
        public List<string> tag_names { get; set; }
        public List<string> notes { get; set; }
    }
}
