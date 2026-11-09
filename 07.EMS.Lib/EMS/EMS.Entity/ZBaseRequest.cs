using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZBaseRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int Title { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZBaseResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int Title { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
