using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryImportDetailGetByInventoryIDRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZInventoryImportDetailGetByInventoryIDResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public ZProductResponse Product { get; set; }
        [DataMember]
        public long Quantity { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        //[DataMember]
        //public string PalletCode { get; set; }
    }
}
