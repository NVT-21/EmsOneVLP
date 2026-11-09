using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZAutoSendMessageDetailSearchListRequest
    {
        public int AutoSendMessageID { get; set; }
    }
    public class ZAutoSendMessageDetailSearchListResponse
    {
        public int ID { get; set; }
        public string MessageTitle { get; set; }
        public string MessageSubTitle { get; set; }
        public string ImageURL { get; set; }
        public string LinkURL { get; set; }
        public string FileToken { get; set; }
    }
}
