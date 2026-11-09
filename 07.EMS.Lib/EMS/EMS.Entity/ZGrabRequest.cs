using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZGrabRequest
    {
        [DataMember]
        public string RequestURI { get; set; }
        [DataMember]
        public object Value { get; set; }
        [DataMember]
        public List<object> ListValue { get; set; }
        [DataMember]
        public string Method { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabResponseV1
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public object data { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabResponseV2
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public List<object> data { get; set; }
    }
}
