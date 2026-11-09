using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryOutputAddRequest
    {
        [DataMember]
        public string AmndState { get; set; }
        [DataMember]
        public string InventoryCode { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public int OrderID { get; set; }
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
        public int CreatedBy { get; set; }
        [DataMember]
        public List<ZInventoryOutputDetailAddRequest> ListInventoryOutputDetail { get; set; }
    }
}
