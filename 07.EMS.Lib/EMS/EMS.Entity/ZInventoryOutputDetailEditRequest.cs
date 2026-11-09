using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryOutputDetailEditRequest
    {
        [DataMember]
        public int ProductVariantID { get; set; }
        [DataMember]
        public long Quantity { get; set; }
        [DataMember]
        public int NumberOfDateInWareHouse { get; set; }
        [DataMember]
        public string TermInWareHouse { get; set; }
        [DataMember]
        public string PalletCode { get; set; }
    }
}
