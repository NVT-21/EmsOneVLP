using System;
using System.Runtime.Serialization;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class ZaloModel
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
        public string PageDescription { get; set; }
        [DataMember]
        public string AccessToken { get; set; }
        [DataMember]
        public string RefreshToken { get; set; }
    }
}