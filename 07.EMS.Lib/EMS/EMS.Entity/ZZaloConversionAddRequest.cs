using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloConversionAddRequest
    {
        //public string PageID { get; set; }
        //public string UserID { get; set; }
        public string MessageID { get; set; }
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderAvatar { get; set; }
        public string RecipientID { get; set; }
        public string RecipientName { get; set; }
        public string RecipientAvatar { get; set; }
        public int FromWhom { get; set; }
        public string Description { get; set; }
        public string MessageType { get; set; }
        public string MessageText { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public DateTime OrderByUpdateDate { get; set; }
    }
}
