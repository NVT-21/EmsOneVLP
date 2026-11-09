using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageMainServiceFeeEditRequest
    {
        [DataMember]
        public int TransportPostageID { get; set; }
        [DataMember]
        public int MainFeeRange { get; set; }
        [DataMember]
        public string MainFeeVAT { get; set; }
        [DataMember]
        public int UpdateBy { get; set; }
    }
    public class XTransportPostageRangeModel
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int PostageID { get; set; }
        public int FromRange { get; set; }
        public int FromRangeValue { get; set; }
        public string FromRangeName { get; set; }
        public int ToRange { get; set; }
        public int ToRangeValue { get; set; }
        public string ToRangeName { get; set; }
        public string TwoWay { get; set; }
        public int Unit { get; set; }
        public long Value { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public List<XTransportPostageWeightModel> TransportPostageWeight { get; set; }
    }
    public class XTransportPostageWeightModel
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int PostageID { get; set; }
        public int ParentID { get; set; }
        public int FromWeight { get; set; }
        public int ToWeight { get; set; }
        public int Unit { get; set; }
        public long Value { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
    }
}
