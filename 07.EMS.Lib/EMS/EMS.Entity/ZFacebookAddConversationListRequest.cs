using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZSendersDataModel
    {
        public string SendersName { get; set; }
        public string SendersEmail { get; set; }
        public string SendersId { get; set; }
        public string PictureLink { get; set; }
    }
    public class ZFacebookAddConversationTotalXMLRequest
    {
        public string CreateBy { get; set; }
        public string PageID { get; set; }
        public string ConversationID { get; set; }
        public long MessageCount { get; set; }
        public string SendersName { get; set; }
        public string SendersEmail { get; set; }
        public string SendersId { get; set; }
        public string PictureLink { get; set; }
        public string Snippet { get; set; }
        public long UnreadCount { get; set; }
        public string UpdatedTime { get; set; }
        public long ConversationType { get; set; }
        public int ShopID { get; set; }
    }
    public class ZFacebookAddConversationListRequest
    {
        public string CreateBy { get; set; }
        public string PageID { get; set; }
        public string ConversationID { get; set; }
        public long MessageCount { get; set; }
        public List<ZSendersDataModel> Senders { get; set; }
        public string Snippet { get; set; }
        public long UnreadCount { get; set; }
        public string UpdatedTime { get; set; }
        public long ConversationType { get; set; }
        public int ShopID { get; set; }
    }
    public class ZFacebookAddConversationXMLRequest
    {
        public string DataInput { get; set; }
    }
}
