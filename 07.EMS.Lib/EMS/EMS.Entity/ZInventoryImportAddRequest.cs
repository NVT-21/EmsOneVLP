using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryImportAddRequest
    {
        [DataMember]
        public int ProviderID { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public string DateOfInventoryImport { get; set; }
        [DataMember]
        public string ImporterName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string PalletCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public List<ZInventoryImportDetailAddRequest> ListInventoryImportDetail { get; set; }
    }
}
