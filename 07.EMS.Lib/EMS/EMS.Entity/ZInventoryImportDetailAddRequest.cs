using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryImportDetailAddRequest
    {
        [DataMember]
        public int ProductVariantID { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public string PalletCode { get; set; }
    }
}
