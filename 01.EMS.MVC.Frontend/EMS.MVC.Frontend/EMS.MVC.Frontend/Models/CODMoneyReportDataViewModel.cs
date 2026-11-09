using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class CODMoneyReportDataViewModel
    {
        [DataMember]
        public string TimeRelease { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public long AmountOfMoney { get; set; }
    }
}