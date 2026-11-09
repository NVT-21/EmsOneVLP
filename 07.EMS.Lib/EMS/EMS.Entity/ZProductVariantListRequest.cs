using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductVariantListRequest
    {

    }
    public class ZProductVariantListResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public List<ZProductVariantListResponse> ListChild { get; set; }
        public string IsLock { get; set; }
    }
    public class ZProductVariantModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
        public long PurchasePrice { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public List<ZAttributeValueModel> AttributeValues { get; set; }
        public List<ZInventoryNumberModel> InventoryNumber { get; set; }
        public string IsLock { get; set; }
    }
    public class ZAttributeValueModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
        public string IsLock { get; set; }
    }
}
