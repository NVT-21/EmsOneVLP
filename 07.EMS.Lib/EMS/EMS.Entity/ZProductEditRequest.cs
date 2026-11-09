using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductEditRequest
    {
        public int ID { get; set; }
        public int ProductGroupID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long PricesSell { get; set; }
        public double Weight { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public int UpdatedBy { get; set; }
        public int ShopID { get; set; }
        public int Unit { get; set; }
    }
    public class ZProductEditResonse
    {
        public int ID { get; set; }
        public int ProviderID { get; set; }
        public int ProductGroupID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long PricesOriginal { get; set; }
        public long PricesSell { get; set; }
    }
}
