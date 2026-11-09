using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZAutoSendMessageUserSearchListRequest
    {
        public int AutoSendMessageID { get; set; }
    }
    public class ZAutoSendMessageUserSearchListResponse
    {
        public int ID { get; set; }
        public string ChannelUserID { get; set; }
    }
}
