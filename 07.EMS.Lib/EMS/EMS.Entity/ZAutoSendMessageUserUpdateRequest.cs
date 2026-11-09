using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZAutoSendMessageUserUpdateRequest
    {
        public int ID { get; set; }
        public string ChannelUserID { get; set; }
        public string MessageID { get; set; }
    }
}
