using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCustomerGroupEditRequest
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long UpdatedBy { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string CRMCode { get; set; }
    }
}
