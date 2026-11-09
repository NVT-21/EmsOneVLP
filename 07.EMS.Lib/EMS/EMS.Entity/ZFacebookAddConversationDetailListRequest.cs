using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAddConversationDetailListRequest
    {
        public string CreateBy { get; set; }
        public string ConversationID { get; set; }
        public string ConversationDetailID { get; set; }
        public string FromID { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Message { get; set; }
        public string CreateTime { get; set; }
        public string FromPictureLink { get; set; }
        public string EmailMessage { get; set; }
        public string PhoneMessage { get; set; }
    }

    public class ZFacebookAddConversationDetailXMLRequest
    {
        public string DataInput { get; set; }
        public List<ZFacebookAddConversationDetailListRequest> ListData { get; set; }
    }

}
