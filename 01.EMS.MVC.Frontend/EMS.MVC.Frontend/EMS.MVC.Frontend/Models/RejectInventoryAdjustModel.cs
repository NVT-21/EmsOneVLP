using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class RejectInventoryAdjustModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Reason { get; set; }
    }
}