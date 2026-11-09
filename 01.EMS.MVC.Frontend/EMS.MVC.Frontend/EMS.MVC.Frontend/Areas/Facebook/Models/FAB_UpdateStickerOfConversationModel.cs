using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    [Serializable]
    [DataContract]
    public class FAB_UpdateStickerOfConversationModel
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public int ConversationID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public List<ZCStickerOfCUpdateItemRequest> ListItem { get; set; }
    }
}