using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Tiktok.Models
{
    [Serializable]
    [DataContract]
    public class TIK_ConfigProductViewModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string IsSyncProduct { get; set; }
        [DataMember]
        public string IsSyncProductPrice { get; set; }
        [DataMember]
        public string IsSyncProductStock { get; set; }
    }
}