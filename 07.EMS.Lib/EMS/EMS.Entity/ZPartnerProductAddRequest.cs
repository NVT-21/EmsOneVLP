using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerProductAddRequest
    {
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public List<ZPartnerProductAddProductInfoRequest> ListProductInfo { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerProductAddProductInfoRequest
    {
        [DataMember]
        public int ProductGroupID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public string Size { get; set; }
        [DataMember]
        public List<ZPartnerProductAddProductVariantInfoRequest> ListProductVariant { get; set; }
        [DataMember]
        public string Images { get; set; }
        [DataMember]
        public int ProductUnitID { get; set; }
        [DataMember]
        public string BatchNumber { get; set; }
        [DataMember]
        public string NumberOfProduct { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerProductAddProductVariantInfoRequest
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
}
