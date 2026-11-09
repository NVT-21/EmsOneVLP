using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageSearchHistoryRequest
    {
        [DataMember]
        public int TransportPostageID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageSearchHistoryResponse
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
