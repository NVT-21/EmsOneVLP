using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCommentDeleteRequest
    {
        public string Channel { get; set; }
        public string CommentID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
