using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloListConversionRequest
    {
    }
    public class ZZaloListConversionResponse
    {
        public List<ZZaloListConversionDataResponse> data { get; set; }
        public int error { get; set; }
        public string message { get; set; }
    }
    public class ZZaloListConversionDataResponse
    {
        //https://developers.zalo.me/docs/api/official-account-api/quan-ly-tin-nhan-nguoi-quan-tam/lay-danh-sach-cac-hoi-thoai-gan-nhat-post-5144
        public int src { get; set; }
        public long time { get; set; }
        public string type { get; set; }
        public string thumb { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string message { get; set; }
        public string message_id { get; set; }
        public string from_id { get; set; }
        public string to_id { get; set; }
        public string from_display_name { get; set; }
        public string from_avatar { get; set; }
        public string to_display_name { get; set; }
        public string to_avatar { get; set; }
    }
}
