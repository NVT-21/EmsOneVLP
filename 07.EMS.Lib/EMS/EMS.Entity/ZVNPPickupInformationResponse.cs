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
    public class ZVNPPickupInformationResponse
    {
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public List<ZVNPStatusPickUp> Status { get; set; }
        [DataMember]
        public List<ZVNPCallHistoryPickUp> CallHistory { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZVNPStatusPickUp
    {
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string PostmanTel { get; set; }
        [DataMember]
        public string ImagePickup { get; set; }
        [DataMember]
        public string ImageSignature { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZVNPCallHistoryPickUp
    {
        [DataMember]
        public string FromNumber { get; set; }
        [DataMember]
        public string ToNumber { get; set; }
        [DataMember]
        public string StartAt { get; set; }
        [DataMember]
        public string EndAt { get; set; }
        [DataMember]
        public string Duration { get; set; }
        [DataMember]
        public string AnswerDuration { get; set; }
        [DataMember]
        public string DialogueTime { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Record { get; set; }
    }
}
