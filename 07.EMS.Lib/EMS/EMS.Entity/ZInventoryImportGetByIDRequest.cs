using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryImportGetByIDRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZInventoryImportGetByIDResponse
    {
        [DataMember]
        public int InventoryID { get; set; }
        [DataMember]
        public string InventoryCode { get; set; }
        [DataMember]
        public string DateOfInventoryImport { get; set; }
        [DataMember]
        public string ImporterName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public ZProviderResponse Provider { get; set; }
        [DataMember]
        public ZWareHouseResponse WareHouse { get; set; }
        [DataMember]
        public List<ZInventoryImportDetailGetByInventoryIDResponse> ListInventoryImportDetail { get; set; }
    }
}
