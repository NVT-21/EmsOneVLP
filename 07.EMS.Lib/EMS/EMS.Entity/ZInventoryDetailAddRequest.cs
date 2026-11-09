using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryDetailAddRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int InventoryID { get; set; }
        [DataMember]
        public int ProductVariantID { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int NumberOfDateInWareHouse { get; set; }
        [DataMember]
        public string TermInWareHouse { get; set; }
    }
}
