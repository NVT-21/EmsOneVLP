using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    [Serializable]
    [DataContract]
    public class FAB_EditConversationStickerViewModel
    {
        [DataMember]
        public int StickerID { get; set; }
        [DataMember]
        public string StickerName { get; set; }
        [DataMember]
        public string StickerColor { get; set; }
    }
}