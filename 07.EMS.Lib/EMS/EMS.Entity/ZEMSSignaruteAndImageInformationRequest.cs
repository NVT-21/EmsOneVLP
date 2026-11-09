using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZEMSSignaruteAndImageInformationRequest
    {
        [DataMember]
        public string LadingCode { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZEMSSignaruteAndImageInformationResponse
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string LadingCode { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public string StatusDate { get; set; }
        [DataMember]
        public string StatusTime { get; set; }
        [DataMember]
        public string PoCode { get; set; }
        [DataMember]
        public string PosName { get; set; }
        [DataMember]
        public string ReasonCode { get; set; }
        [DataMember]
        public string ReasonName { get; set; }
        [DataMember]
        public string SolutionCode { get; set; }
        [DataMember]
        public string SolutionName { get; set; }
        [DataMember]
        public int PostManId { get; set; }
        [DataMember]
        public string PostManName { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int CollectAmount { get; set; }
        [DataMember]
        public string IsPaypost { get; set; }
        [DataMember]
        public string RouteCode { get; set; }
        [DataMember]
        public string RouteName { get; set; }
        [DataMember]
        public string Signature { get; set; }
        [DataMember]
        public List<string> FileNames { get; set; }
        [DataMember]
        public List<string> FileNamesAuth { get; set; }
        [DataMember]
        public string DeliveryDateTime { get; set; }
        [DataMember]
        public List<string> ImagePlus { get; set; }
    }
}
