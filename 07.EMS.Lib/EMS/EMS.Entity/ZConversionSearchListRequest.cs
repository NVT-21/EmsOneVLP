using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZConversionSearchListRequest
    {
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public int LimitRow { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
        [DataMember]
        public string StickerID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZConversionSearchListResponse
    {
        [DataMember]
        public int ConversionID { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string SenderID { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string Avatar { get; set; }
        [DataMember]
        public string ProfileLink { get; set; }
        [DataMember]
        public string MessageText { get; set; }
        [DataMember]
        public string MessageType { get; set; }
        [DataMember]
        public int NumberOfMessage { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public List<ZConversationStickerSearchListResponse> ListSticker { get; set; }
    }
}
