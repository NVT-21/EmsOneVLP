using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XFacebookFeedModel
    {
        public long ID { get; set; }
        public string PageID { get; set; }
        public string ConversationID { get; set; }
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Snippet { get; set; }
        public string UpdateTime { get; set; }
        public string CreatedDate { get; set; }
        public int ConversationSeen { get; set; }
        public int ConversationType { get; set; }
        public string PictureLink { get; set; }
        public string AssignID { get; set; }
        public string NameAssign { get; set; }
        public string EmployeeName { get; set; }
        public int TotalComment { get; set; }
        public int TotalCommentNotSeen { get; set; }
    }
}
