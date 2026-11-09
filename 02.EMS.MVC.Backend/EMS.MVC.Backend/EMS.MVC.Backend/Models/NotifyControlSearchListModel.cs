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
    public class NotifyControlSearchListModel
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string IsLock { get; set; }
    }
    [Serializable]
    [DataContract]
    public class NotifyControlSearchListViewModel
    {
        [DataMember]
        public int NotifyID { get; set; }
        [DataMember]
        public string NotifyType { get; set; }
        [DataMember]
        public string NotifyTypeName { get; set; }
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
        public string NotifyReceiverTypeName { get; set; }
        [DataMember]
        public string NotifyReceiverCRM { get; set; }
        [DataMember]
        public string NotifyReceiverGroupID { get; set; }
        [DataMember]
        public string NotifyReceiverGroupName { get; set; }
        [DataMember]
        public string CreatedByName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public string IsSend { get; set; }
        [DataMember]
        public int NotifyStatisticalID { get; set; }
        [DataMember]
        public int NotifyQuantitySend { get; set; }
        [DataMember]
        public int NotifyQuantityViewed { get; set; }
        [DataMember]
        public int NotifyQuantityUnViewed { get; set; }
    }
}