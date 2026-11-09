using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCustomerGroupAddRequest
    {
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string CRMCode { get; set; }
    }
}
