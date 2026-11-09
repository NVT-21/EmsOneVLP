using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookSearchCommentRequest
    {
        public string ConversationID { get; set; }
        public int Phone { get; set; }
        public int Email { get; set; }
        public int Day { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PageID { get; set; }
        public string ContentFeed { get; set; }
    }

    public class ZFacebookSearchCommentResponse
    {
        public int totalComment { get; set; }
        public int totalCommentSeen { get; set; }
        public int totalCommentNotSeen { get; set; }
        public List<XFacebookCommentModel> ListComment { get; set; }
    }
}
