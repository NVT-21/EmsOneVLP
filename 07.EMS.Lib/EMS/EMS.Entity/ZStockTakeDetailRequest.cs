using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    public class ZStockTakeDetailRequest
    {
    }

    [Serializable]
    [DataContract]
    public class ZStockTakeDetailResponse
    {
        [DataMember]
        public int StockTakeDetailID { get; set; }
        [DataMember]
        public ZProductResponse Product { get; set; }
        [DataMember]
        public ZProductVariantResponse ProductVariant { get; set; }
        [DataMember]
        public long NumberOfInventory { get; set; }
        [DataMember]
        public long NumberOfActual { get; set; }
        [DataMember]
        public long NumberOfDifferent { get; set; }
        [DataMember]
        public long MoneyOfDifferent { get; set; }
    }
}
