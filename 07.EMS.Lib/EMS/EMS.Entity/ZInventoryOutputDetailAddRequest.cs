using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryOutputDetailAddRequest
    {
        [DataMember]
        public int ProductVariantID { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int NumberOfDateInWareHouse { get; set; }
        [DataMember]
        public string TermInWareHouse { get; set; }
        [DataMember]
        public string PalletCode { get; set; }
    }
}
