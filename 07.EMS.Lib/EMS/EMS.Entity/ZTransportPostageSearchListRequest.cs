using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageSearchListRequest
    {
        [DataMember]
        public int Type { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageSearchListResponse
    {
        [DataMember]
        public int TransportPostageID { get; set; }
        [DataMember]
        public string TransportPostageCode { get; set; }
        [DataMember]
        public string TransportPostageName { get; set; }
        [DataMember]
        public string TransportServiceCode { get; set; }
        [DataMember]
        public string TransportServiceName { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public ZBaseResponse VAT { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string ListCrmOrPaypostCode { get; set; }
    }
    //public class ZTransportPostageStatus
    //{
    //    public string Code { get; set; }
    //    public string Name
    //    {
    //        get
    //        {
    //            return XData.TransportPostageGetStatusByValue(Code);
    //        }
    //    }
    //}
}
