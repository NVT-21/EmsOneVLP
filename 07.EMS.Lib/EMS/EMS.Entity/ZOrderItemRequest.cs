using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderItemRequest
    {
    }

    [Serializable]
    [DataContract]
    public class ZOrderItemResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public int VariantID { get; set; }
        [DataMember]
        public string VariantName { get; set; }
        [DataMember]
        public int NumberOfProduct { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public int Discount { get; set; }
        [DataMember]
        public int Weight { get; set; }
        [DataMember]
        public string ProductVariantImage { get; set; }
        [DataMember]
        public int WareHouseQuantity { get; set; }
    }
}
