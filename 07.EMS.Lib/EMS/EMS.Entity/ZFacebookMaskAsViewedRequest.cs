using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookMaskAsViewedRequest
    {
        public int Type { get; set; }
        public string ConversationID { get; set; }
        public string ConversationDetailID { get; set; }
        public string UpdatedBy { get; set; }
    }
}
