using System;
using System.Runtime.Serialization;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class FacebookModel
    {
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string PageName { get; set; }
        [DataMember]
        public string PageAvatar { get; set; }
        [DataMember]
        public string PageCover { get; set; }
        [DataMember]
        public string PageCategory { get; set; }
        [DataMember]
        public string UserAccessToken { get; set; }
        [DataMember]
        public string PageAccessToken { get; set; }
    }
}