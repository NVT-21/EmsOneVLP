using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderReportRequest
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
        public int PartnerID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderReportResponse
    {
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string ConfirmDate { get; set; }
        [DataMember]
        public string AreaName { get; set; }
        [DataMember]
        public int OrderMonthQuantity { get; set; }
        [DataMember]
        public long OrderMonthTurnover { get; set; }
        [DataMember]
        public int PartnerID { get; set; }
        [DataMember]
        public string PartnerName { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderDetailReportRequest
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
    }

    [Serializable]
    [DataContract]
    public class ZOrderDetailReportResponse
    {
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string OrderCreatedDate { get; set; }
        [DataMember]
        public long TempTransportFee{ get; set; }
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
