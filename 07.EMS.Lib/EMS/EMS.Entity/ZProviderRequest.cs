using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProviderRequest
    {
    }

    [Serializable]
    [DataContract]
    public class ZProviderResponse
    {
        [DataMember]
        public int ProviderID { get; set; }
        [DataMember]
        public string ProviderName { get; set; }
        [DataMember]
        public string ProviderCode { get; set; }
    }
}
