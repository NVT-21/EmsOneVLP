using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZMessageSearchListRequest
    {
        public int ConversionID { get; set; }
        public string UserID { get; set; }
        public string Channel { get; set; }
        public int LimitRow { get; set; }
        public string CreatedDate { get; set; }
    }
    public class ZMessageSearchListResponse
    {
        public int MessageID { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string ChannelMessageID { get; set; }
        public string MessageText { get; set; }
        public string StickerID { get; set; }
        public string LinkURL { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MessageType { get; set; }
        public XStatusModel Status { get; set; }
        public int FromWhom { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string AmndDate { get; set; }
    }
}
