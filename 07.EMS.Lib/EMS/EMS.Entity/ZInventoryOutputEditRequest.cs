using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryOutputEditRequest
    {
        [DataMember]
        public int InventoryID { get; set; }
        [DataMember]
        public string DateOfInventoryOutput { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public List<ZInventoryOutputDetailEditRequest> ListInventoryOutputDetail { get; set; }
    }
}
