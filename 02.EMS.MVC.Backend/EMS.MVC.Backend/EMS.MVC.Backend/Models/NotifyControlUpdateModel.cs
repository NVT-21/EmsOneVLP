using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class NotifyControlChangeLockModel
    {
        [DataMember]
        public int NotifyID { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
    [Serializable]
    [DataContract]
    public class NotifyControlRemoveModel
    {
        [DataMember]
        public int NotifyID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
    [Serializable]
    [DataContract]
    public class NotifyControlEditModel
    {
        [DataMember]
        public int NotifyID { get; set; }
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
        public string NotifyReceiverCRM { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}