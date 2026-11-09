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
    public class ZConversationStickerAddRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string StickerName { get; set; }
        [DataMember]
        public string StickerColor { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZConversationStickerEditRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public int StickerID { get; set; }
        [DataMember]
        public string StickerName { get; set; }
        [DataMember]
        public string StickerColor { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZConversationStickerRemoveRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public int StickerID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZConversationStickerSearchListRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
        [DataMember]
        public int ConversationID { get; set; }
        [DataMember]
        public string TypeList { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZConversationStickerSearchListResponse
    {
        [DataMember]
        public int StickerID { get; set; }
        [DataMember]
        public string StickerName { get; set; }
        [DataMember]
        public string StickerColor { get; set; }
        [DataMember]
        public string IsChoose { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZConversationStickerByIDRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public int StickerID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZConversationStickerByIDResponse
    {
        [DataMember]
        public int StickerID { get; set; }
        [DataMember]
        public string StickerName { get; set; }
        [DataMember]
        public string StickerColor { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZCStickerOfCUpdateRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public int ConversationID { get; set; }
        [DataMember]
        public List<ZCStickerOfCUpdateItemRequest> ListItem { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZCStickerOfCUpdateItemRequest
    {
        [DataMember]
        public int StickerID { get; set; }
        [DataMember]
        public string IsChoose { get; set; }
    }
}
