using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTiktokShopProductSyncRequest
    {
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public List<ZTiktokShopProductSyncProduct> ListProduct { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZTiktokShopProductSyncProduct
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
        public List<ZTiktokShopProductVariant> ListProductVariant { get; set; }
        [DataMember]
        public string Avatar { get; set; }
        [DataMember]
        public string Images { get; set; }
        [DataMember]
        public int ProductUnitID { get; set; }
        [DataMember]
        public string BatchNumber { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZTiktokShopProductVariant
    {
        [DataMember]
        public string SKUID { get; set; }
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
        public int ShopID { get; set; }
        [DataMember]
        public string BatchNumber { get; set; }
        [DataMember]
        public int NumberOfProduct { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZTiktokShopProductVariantStockInfos
    {
        [DataMember]
        public string WareHouseID { get; set; }
        [DataMember]
        public int AvailableStock { get; set; }
    }
}
