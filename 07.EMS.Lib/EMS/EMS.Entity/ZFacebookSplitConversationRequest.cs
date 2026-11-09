using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookSplitConversationRequest
    {
        public string ConversationID { get; set; }
        public int EmployeeID { get; set; }
        public int UpdatedBy { get; set; }
        public double T { get; set; }
        public string PageID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZFacebookCountConversationSplitRequest
    {
        public string PageID { get; set; }
        public int ShopID { get; set; }
    }
}
