using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookSearchFeedRequest
    {
        public string PageID { get; set; }
        public int ConversationType { get; set; }
        public int ConversationSeen { get; set; }
        public int ConversationAssign { get; set; }
        public int Phone { get; set; }
        public int Email { get; set; }
        public int Day { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int EmployeeID { get; set; }
        public int ShopID { get; set; }
    }

    public class ZFacebookSearchFeedResponse
    {
        public int TotalFeed { get; set; }
        public int TodayFeed { get; set; }
        public int MonthFeed { get; set; }
        public List<XFacebookFeedModel> ListFeed { get; set; }
    }
}
