using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryOutputGetByIDRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZInventoryOutputGetByIDResponse
    {
        [DataMember]
        public int InventoryOutputID { get; set; }
        [DataMember]
        public string InventoryOutputCode { get; set; }
        [DataMember]
        public string DateOfInventoryOutput { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public string TotalAmountByText { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public ZWareHouseResponse WareHouse { get; set; }
        [DataMember]
        public ZOrderResponse Order { get; set; }
        [DataMember]
        public List<ZInventoryOutputDetailGetByInventoryIDResponse> ListInventoryOutputDetail { get; set; }
    }
}
