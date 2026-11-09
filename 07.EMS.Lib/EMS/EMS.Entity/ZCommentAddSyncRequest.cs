using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCommentAddSyncRequest
    {
        public int FeedID { get; set; }
        public int ReplyCommentCount { get; set; }
        public string ChannelCommentID { get; set; }
        public string ParentChannelCommentID { get; set; }
        public string Content { get; set; }
        public string ContentEmail { get; set; }
        public string ContentPhone { get; set; }
        public int FromWhom { get; set; }
        public int CommentLevel { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
}
