using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportDetailRequest
    {
        [DataMember]
        public int RequestSupportID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportDetailResponse
    {
        [DataMember]
        public int RequestSupportID { get; set; }
        [DataMember]
        public string RequestSupportCode { get; set; }
        [DataMember]
        public string RequestGroup { get; set; }
        [DataMember]
        public string RequestType { get; set; }
        [DataMember]
        public string RequestSupportDescription { get; set; }
        [DataMember]
        public string RequestSupportDocument { get; set; }
        [DataMember]
        public string EmployeeName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public ZBaseResponse RequestSupportStatus { get; set; }
        [DataMember]
        public string UpdateInformation { get; set; }
        [DataMember]
        public string OldUpdateInformation { get; set; }
        [DataMember]
        public string PosC { get; set; }

        [DataMember]
        public string EShippingCode { get; set; }
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderMessage { get; set; }
        [DataMember]
        public double WeightBefore { get; set; }
        [DataMember]
        public double WeightAfter { get; set; }
        [DataMember]
        public string SenderName { get; set; }
        [DataMember]
        public string SenderPhone { get; set; }
        [DataMember]
        public string SenderFullAddress { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string ReceiverPhone { get; set; }
        [DataMember]
        public string ReceiverFullAddress { get; set; }
        [DataMember]
        public ZBaseResponse OrderStatus { get; set; }
        [DataMember]
        public long TransportOrderValue { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public string TransportMainService { get; set; }
        [DataMember]
        public string TransportExtraService { get; set; }

        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string ShopCode { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string ShopPhone { get; set; }
        [DataMember]
        public string ShopFullAddress { get; set; }

        [DataMember]
        public List<ZRequestSupportSearchHistoryResponse> ListSearchHistory { get; set; }

        [DataMember]
        public List<ZRequestSupportSearchProcessResponse> ListSearchProcess { get; set; }
    }
}
