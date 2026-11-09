using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOptionDataByVariantIDRequest
    {
        public int VariantID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZOptionDataByVariantIDResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
    }
}
