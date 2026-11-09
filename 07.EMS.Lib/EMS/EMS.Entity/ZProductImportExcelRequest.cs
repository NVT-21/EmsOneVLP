using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductImportExcelRequest
    {
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public List<ZProductImportExcelProductRequest> ListProduct { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZProductImportExcelProductRequest
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
        public List<ZProductVariantAddRequest> ListProductVariant { get; set; }
        [DataMember]
        public string Images { get; set; }
        [DataMember]
        public int ProductUnitID { get; set; }
        [DataMember]
        public string BatchNumber { get; set; }
        [DataMember]
        public string NumberOfProduct { get; set; }
        [DataMember]
        public string PalletCode { get; set; }
    }
}
