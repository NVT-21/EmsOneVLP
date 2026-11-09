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
    public class ZTikProductUpdateFromServiceRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string TiktokSKUID { get; set; }
        [DataMember]
        public string TiktokProductID { get; set; }
        [DataMember]
        public string TypeUpdate { get; set; }
        [DataMember]
        public string UpdateSuccess { get; set; }
    }
}
