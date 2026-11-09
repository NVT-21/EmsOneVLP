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
    public class FAB_OrderSuccessHistoryOfUserViewModel
    {
        [DataMember]
        public string AllDate { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
    }
}