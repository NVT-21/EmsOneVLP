using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZStockTakeDetailAddRequest
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int ProductVariantID { get; set; }
        [DataMember]
        public int ProductVariantPrice { get; set; }
        [DataMember]
        public int NumberOfInventory { get; set; }
        [DataMember]
        public int NumberOfActual { get; set; }
        [DataMember]
        public int NumberOfDifferent { get; set; }
        [DataMember]
        public int MoneyOfDifferent { get; set; }
        [DataMember]
        public int Quantity { get; set; }
    }
}
