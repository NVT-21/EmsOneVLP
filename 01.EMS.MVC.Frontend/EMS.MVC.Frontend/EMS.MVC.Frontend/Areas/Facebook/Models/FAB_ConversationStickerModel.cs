using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    [Serializable]
    [DataContract]
    public class FAB_ConversationStickerModel
    {
        [DataMember]
        public string SearchKey { get; set; }
        [DataMember]
        public int ConversationID { get; set; }
    }
}