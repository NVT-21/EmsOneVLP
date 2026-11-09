using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZAutoSendMessageServiceSearchListRequest
    {
        public string Channel { get; set; }  
    }
    public class ZAutoSendMessageServiceSearchListResponse
    {
        public int ID { get; set; }
        public string PageID { get; set; }
        public int ShopID { get; set; }
        public string MessageContent { get; set; }
        public string MessageType { get; set; }
        public string AccessToken { get; set; }
    }
}
