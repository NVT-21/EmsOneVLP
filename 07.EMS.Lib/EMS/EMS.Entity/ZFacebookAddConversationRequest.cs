using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAddConversationRequest
    {
        public string CreateBy { get; set; }
        public string PageID { get; set; }
        public string ConversationID { get; set; }
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Snippet { get; set; }
        public string UpdateTime { get; set; }
        public int ConversationType { get; set; }
    }

}
