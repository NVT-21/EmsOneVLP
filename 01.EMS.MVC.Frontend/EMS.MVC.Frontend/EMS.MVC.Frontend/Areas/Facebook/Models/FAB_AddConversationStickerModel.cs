using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    [Serializable]
    [DataContract]
    public class FAB_AddConversationStickerModel
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
    public class FAB_EditConversationStickerModel
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
    public class FAB_DeleteConversationStickerModel
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
}