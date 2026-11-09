using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductOptionGetByProductIDRequest
    {
        public int ProductID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZProductOptionGetByProductIDResponse
    {
        public List<XProductOptionModel> Option01 { get; set; }
        public List<XProductOptionModel> Option02 { get; set; }
        public List<XProductOptionModel> Option03 { get; set; }
    }
    public class XProductOptionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Sort { get; set; }
    }
}
