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
    public class ReconciliationCODMoneyViewModel
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public int CODTransType { get; set; }
        [DataMember]
        public List<SelectListItem> ListCODTransType { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public List<SelectListItem> ListStatus { get; set; }
        [DataMember]
        public string TypeDate { get; set; }
        [DataMember]
        public List<SelectListItem> ListTypeDate { get; set; }
    }
}