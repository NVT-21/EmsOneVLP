using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZShopTokenAccessUpdateRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string TokenAccess  { get; set; }
        [DataMember]
        public DateTime Expired { get; set; }
    }
}
