using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductListRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int ProviderID { get; set; }
        public int ShopID { get; set; }
        public int ProductGroupID { get; set; }
        public string SearchKey { get; set; }
    }
    public class ZProductListResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public string ProductGroupName { get; set; }
        public string Avatar { get; set; }
        public long PricesSell { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByFullName { get; set; }
        public int VariantNumber { get; set; }
        public int Quantity { get; set; }
    }
}
