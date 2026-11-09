using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class PartnerWebhookAddPartnerRegisterModel
    {
        [DataMember]
        public string PartnerName { get; set; }
        [DataMember]
        public string PartnerCode { get; set; }
        [DataMember]
        public string PartnerURL { get; set; }
        [DataMember]
        public string PartnerKey { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
    [Serializable]
    [DataContract]
    public class PartnerWebhookEditPartnerRegisterModel
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string PartnerName { get; set; }
        [DataMember]
        public string PartnerCode { get; set; }
        [DataMember]
        public string PartnerURL { get; set; }
        [DataMember]
        public string PartnerKey { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
    [Serializable]
    [DataContract]
    public class PartnerWebhookDeletePartnerRegisterModel
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
