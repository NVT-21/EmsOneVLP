using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class OrderDetailReportViewModel
    {
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
        [DataMember]
        public string PlaceOfCreation { get; set; }
        [DataMember]
        public string PlaceOfCreationName { get; set; }
    }
}