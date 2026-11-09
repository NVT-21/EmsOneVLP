using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookSearchConversationAssignRequest
    {
        public string PageID { get; set; }
        public string CreateBy { get; set; }
        public int ShopID { get; set; }
    }
    public class ZFacebookSearchConversationAssignResponse
    {
        public int ID { get; set; }
        public string NameAssign { get; set; }
    }
}
