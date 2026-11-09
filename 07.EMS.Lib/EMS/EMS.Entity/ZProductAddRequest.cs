using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductAddRequest
    {
        public int ProductGroupID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
        public string Size { get; set; }
        public int Weight { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Images { get; set; }
        public int Unit { get; set; }
        public string BatchNumber { get; set; }
        public List<ZProductVariantAddRequest> Variants { get; set; }
        //public List<ZProductOptionAdd> Options { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ZProductAddResponse
    {
        public int ProductID { get; set; }
    }
    //public class ZProductVariantAdd
    //{
    //    public string Options { get; set; }
    //    public string OptionName1 { get; set; }
    //    public string OptionValue1 { get; set; }
    //    public string OptionName2 { get; set; }
    //    public string OptionValue2 { get; set; }
    //    public string OptionName3 { get; set; }
    //    public string OptionValue3 { get; set; }
    //}
    //public class ZProductOptionAdd
    //{
    //    public string Name { get; set; }
    //    public string Values { get; set; }
    //}

    //public class ZProductDetailVariant
    //{
    //    public int ID { get; set; }
    //    public int ProductID { get; set; }
    //    public int ParentID { get; set; }
    //    public string ParentName { get; set; }
    //    public string ChildID { get; set; }
    //    public string ChildName { get; set; }
    //    public int WareHouseID { get; set; }
    //    public long PricesSell { get; set; }
    //    public int Quantity { get; set; }
    //}

    //public class ZVariantModel
    //{
    //    public int ID { get; set; }
    //    public string Name { get; set; }
    //    public List<ZVariantModel> ListChild { get; set; }
    //    public string IsLock { get; set; }
    //}

    public class ZInventoryNumberModel
    {
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
    }
}
