using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductVariantAddRequest
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public string Size { get; set; }
        [DataMember]
        public int Weight { get; set; }
        [DataMember]
        public string Avatar { get; set; }
        [DataMember]
        public string SKU { get; set; }
        [DataMember]
        public string Barcode { get; set; }
        [DataMember]
        public string Options { get; set; }
        [DataMember]
        public string OptionName1 { get; set; }
        [DataMember]
        public string OptionValue1 { get; set; }
        [DataMember]
        public string OptionName2 { get; set; }
        [DataMember]
        public string OptionValue2 { get; set; }
        [DataMember]
        public string OptionName3 { get; set; }
        [DataMember]
        public string OptionValue3 { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string BatchNumber { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZProductVariantAddResponse
    {
        [DataMember]
        public int VariantID { get; set; }
    }
    //public class ZWarehouseProductModel
    //{
    //    public int WarehouseID { get; set; }
    //    public int VariantID { get; set; }
    //    public string WarehouseCode { get; set; }
    //    public string WarehouseName { get; set; }
    //    public int Quantity { get; set; }
    //}
}
