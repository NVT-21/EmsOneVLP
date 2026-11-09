using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryOutputRejectRequest
    {
        [DataMember]
        public int InventoryOutputID { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
