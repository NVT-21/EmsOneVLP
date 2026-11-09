using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZAutoSendMessageAddRequest
    {
        public string MessageContent { get; set; }
        public string MessageType { get; set; }
        public string SendTime { get; set; }
        public string PageID { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
        public string Channel { get; set; }
        public List<ZAutoSendMessageDetailAddRequest> ListDetail { get; set; }
        public List<string> ListUserID { get; set; }
    }
}
