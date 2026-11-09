using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductOptionAddRequest
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
}
