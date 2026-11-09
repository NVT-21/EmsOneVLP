using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Tiktok.Models
{
    [Serializable]
    [DataContract]
    public class TIK_ConfigProductChangeViewModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}