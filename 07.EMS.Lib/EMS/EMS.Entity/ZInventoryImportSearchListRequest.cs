using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryImportSearchListRequest
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string DateType { get; set; }
        [DataMember]
        public string InventoryImportCode { get; set; }
        [DataMember]
        public int WarehouseID { get; set; }
        [DataMember]
        public int ProviderID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZInventoryImportSearchListResponse
    {
        [DataMember]
        public int InventoryImportID { get; set; }
        [DataMember]
        public string InventoryImportCode { get; set; }
        [DataMember]
        public ZWareHouseResponse WareHouse { get; set; }
        [DataMember]
        public ZProviderResponse Provider { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public string DateOfInventoryImport { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
