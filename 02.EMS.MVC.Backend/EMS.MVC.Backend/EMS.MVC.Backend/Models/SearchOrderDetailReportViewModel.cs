using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class SearchOrderDetailReportViewModel
    {
        [DataMember]
        public long SumTempTransportFee { get; set; }
        [DataMember]
        public long SumTransportFee { get; set; }
        [DataMember]
        public long SumCODMoney { get; set; }
        [DataMember]
        public List<SearchOrderDetailReportDataViewModel> Data { get; set; }
    }

    [Serializable]
    [DataContract]
    public class SearchOrderDetailReportDataViewModel
    {
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string OrderCreatedDate { get; set; }
        [DataMember]
        public long TempTransportFee { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public long CODMoney { get; set; }
        [DataMember]
        public string PlaceOfOrderCreation { get; set; }
        [DataMember]
        public string StatusName { get; set; }
    }
}