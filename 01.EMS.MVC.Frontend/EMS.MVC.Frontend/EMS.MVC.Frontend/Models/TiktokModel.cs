using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class TiktokModel
    {
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string PageName { get; set; }
        [DataMember]
        public string PageAvatar { get; set; }
        [DataMember]
        public string PageCategory { get; set; }
        [DataMember]
        public string UserAccessToken { get; set; }
        [DataMember]
        public string PageAccessToken { get; set; }
    }
}