using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XFacebookCommentModel
    {
        public long ID { get; set; }
        public string ConversationID { get; set; }
        public string ConversationDetailID { get; set; }
        public string FromID { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Message { get; set; }
        public string CreateTime { get; set; }
        public string FromPictureLink { get; set; }
        public int ConversationDetailSeen { get; set; }
        public int IsLike { get; set; }
        public int IsHidden { get; set; }
        public int IsFeedback { get; set; }
    }
}
