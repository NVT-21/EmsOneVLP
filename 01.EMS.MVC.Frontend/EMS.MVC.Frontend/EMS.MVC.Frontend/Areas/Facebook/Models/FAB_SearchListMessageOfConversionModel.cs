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
    public class FAB_SearchListMessageOfConversionModel
    {
        [DataMember]
        public int ConversionID { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string Avatar { get; set; }
        [DataMember]
        public string ProfileLink { get; set; }
        [DataMember]
        public List<ZConversationStickerSearchListResponse> ListSticker { get; set; }
    }
}