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
    public class FAB_SearchListMessageOfConversionViewModel
    {
        [DataMember]
        public int ConversionID { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string UserAvatar { get; set; }
        [DataMember]
        public string UserLink { get; set; }
        [DataMember]
        public string PageName { get; set; }
        [DataMember]
        public string PageAvatar { get; set; }
        [DataMember]
        public List<FAB_MessageOfConversionViewModel> ListMessage { get; set; }
        [DataMember]
        public List<ZConversationStickerSearchListResponse> ListSticker { get; set; }
    }
}