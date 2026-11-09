using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryAddRequest
    {
        [DataMember]
        public int WarehouseID { get; set; }
        [DataMember]
        public int ProviderID { get; set; }
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int StockTakeID { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string ImplementationDate { get; set; }
        [DataMember]
        public string PaymentStatus { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public List<ZInventoryDetailAddRequest> ListInventoryDetail { get; set; }
        [DataMember]
        public string InventoryCode { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZInventoryAddResponse
    {
        [DataMember]
        public int ID { get; set; }
    }
}
