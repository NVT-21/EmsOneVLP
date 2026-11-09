using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryImportEditRequest
    {
        [DataMember]
        public int InventoryImportID { get; set; }
        [DataMember]
        public int ProviderID { get; set; }
        [DataMember]
        public string DateOfInventoryImport { get; set; }
        [DataMember]
        public string ImporterName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public List<ZInventoryImportDetailEditRequest> ListInventoryImportDetail { get; set; }
    }
}
