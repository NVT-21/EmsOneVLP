using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZAutoSendMessageSearchListRequest
    {
        public string PageID { get; set; }
        public int ShopID { get; set; }
        public string Channel { get; set; }
        public string Status { get; set; }
    }
    public class ZAutoSendMessageSearchListResponse
    {
        public int ID { get; set; }
        public string MessageContent { get; set; }
        public string MessageType { get; set; }
        public string SendTime { get; set; }
        public string Status { get; set; }
        public int SendStatus { get; set; }
        public int RecieveStatus { get; set; }
        public int SeenStatus { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
    }
}
