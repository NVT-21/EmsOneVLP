using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XFeedModel
    {
        public int ID { get; set; }
        public string ChannelFeedID { get; set; }
        public string Content { get; set; }
        public string Avatar { get; set; }
        public string PeopleComment { get; set; }
        public int NumberOfComment { get; set; }
        public int FromWhom { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string FullPicture { get; set; }
    }
}
