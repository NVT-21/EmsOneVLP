using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFeedSearchListRequest
    {
        public string PageID { get; set; }
        public string Channel { get; set; }
    }
    public class ZFeedSearchListResponse: XFeedModel
    {
    }
}
