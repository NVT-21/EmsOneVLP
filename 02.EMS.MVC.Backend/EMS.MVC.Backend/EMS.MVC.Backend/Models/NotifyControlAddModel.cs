using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class NotifyControlAddModel
    {
        [DataMember]
        public string NotifyType { get; set; }
        [DataMember]
        public string NotifyName { get; set; }
        [DataMember]
        public string NotifyContent { get; set; }
        [DataMember]
        public string NotifyRedirectUrl { get; set; }
        [DataMember]
        public string NotifySendDate { get; set; }
        [DataMember]
        public string NotifyReceiverType { get; set; }
        [DataMember]
        public string NotifyReceiverGroupID { get; set; }
        [DataMember]
        public List<SelectListItem> ListNotifyReceiverGroupID { get; set; }
        [DataMember]
        public string NotifyReceiverCRM { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}