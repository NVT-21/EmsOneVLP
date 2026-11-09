using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class DebtNoticesOverViewDataViewModel
    {
        [DataMember]
        public string Explain { get; set; }
        [DataMember]
        public int NumberOfParcels { get; set; }
        [DataMember]
        public long FeeBeforeVAT { get; set; }
        [DataMember]
        public long VAT { get; set; }
        [DataMember]
        public long FeeAfterVAT { get; set; }
    }
}