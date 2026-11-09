using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    [Serializable]
    [DataContract]
    public class FAB_SearchOrderSuccessHistoryOfUserViewModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public XStatusModel Status { get; set; }
        [DataMember]
        public long TotalProductValue { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}