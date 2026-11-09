using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class PartnerWebhookAddViewModel
    {
        [DataMember]
        public string PartnerName { get; set; }
        [DataMember]
        public string PartnerURL { get; set; }
        [DataMember]
        public string PartnerKey { get; set; }
    }
}