using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XCommentModel
    {
        public int ID { get; set; }
        public string ChannelCommentID { get; set; }
        public string Content { get; set; }
        public int Level { get; set; }
        public string ParentChannelCommentID { get; set; }
        public string CreatedBy { get; set; }
        public string Avatar { get; set; }
        public int FromWhom { get; set; }
        public string CreatedDate { get; set; }
        public string FormatCreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string IsLike { get; set; }
        public string IsHidden { get; set; }
        public List<XCommentModel> ListReplyComment { get; set; }
    }
}
