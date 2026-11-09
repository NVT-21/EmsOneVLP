using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductOptionGetNameByProductIDRequest
    {
        public int ProductID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZProductOptionGetNameByProductIDResponse
    {
        public string Name { get; set; }
    }
}
