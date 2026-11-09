using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFeedDeleteRequest
    {
        public string PageID { get; set; }
        public string Channel { get; set; }
        public string FeedID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
