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
    public class ZOrderImportSearchListRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public long JobID { get; set; }
    }
}
