using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductDataImagesByIDRequest
    {
        public int ProductID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZProductDataImagesByIDResponse
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Images { get; set; }
    }
}
