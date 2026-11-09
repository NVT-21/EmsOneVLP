using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZNotifyControlAddRequest
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
        public string NotifyReceiverCRM { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZNotifyControlListRequest
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
    public class ZNotifyControlListResponse
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
    }
    [Serializable]
    [DataContract]
    public class ZNotifyControlChangeLockRequest
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
    public class ZNotifyControlRemoveRequest
    {
        [DataMember]
        public int NotifyID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZNotifyControlGetByIDRequest
    {
        [DataMember]
        public int NotifyID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZNotifyControlEditRequest
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
