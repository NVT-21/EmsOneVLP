using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZVNPSignaruteAndImageInformationRequest
    {
        [DataMember]
        public string itemCode { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZVNPSignaruteAndImageInformationResponse
    {
        [DataMember]
        public string PODeliveryCode { get; set; }
        [DataMember]
        public string DeliveryDateTime { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public string ReasonCode { get; set; }
        [DataMember]
        public string ReasonName { get; set; }
        [DataMember]
        public List<string> ImagesDelivery { get; set; }
        [DataMember]
        public List<string> Images { get; set; }
        [DataMember]
        public string SolutionCode { get; set; }
        [DataMember]
        public string SolutionName { get; set; }
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string ReceiverPId { get; set; }
        [DataMember]
        public List<string> ImagesPlus { get; set; }
        [DataMember]
        public List<string> ImagesSignature { get; set; }
        [DataMember]
        public List<string> ImagesUpload { get; set; }
    }
}
