using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductGetByIDRequest
    {
        public int ProductID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZProductGetByIDResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }
        public long PricesSell { get; set; }
        public double Weight { get; set; }
        public string Size { get; set; }
        public int Unit { get; set; }
        public string UnitName { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByFullName { get; set; }
        public List<ZImageModel> Images { get; set; }
        public List<ZVariantByProductID> Variant { get; set; }
    }
    public class ZImageModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
    }
    public class ZVariantByProductID
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string SKU { get; set; }
        public string Bracode { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public int WarehouseNumber { get; set; }
    }
}
