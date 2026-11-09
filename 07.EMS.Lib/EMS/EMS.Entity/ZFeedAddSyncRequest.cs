using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFeedAddSyncRequest
    {
        public string PageID { get; set; }
        public string ChannelFeedID { get; set; }
        public string Content { get; set; }
        public int FromWhom { get; set; }
        public string Channel { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string FullPicture { get; set; }
    }
}
