using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCommentUpdateReactionRequest
    {
        public string Channel { get; set; }
        public string CommentID { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
    }
}
