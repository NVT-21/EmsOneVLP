using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCommentSearchListRequest
    {
        public int FeedID { get; set; }
    }
    public class ZCommentSearchListResponse
    {
        public XFeedModel Feed { get; set; }
        public List<XCommentModel> Comment { get; set; }
    }
}
