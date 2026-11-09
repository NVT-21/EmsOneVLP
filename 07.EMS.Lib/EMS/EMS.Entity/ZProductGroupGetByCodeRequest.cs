using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductGroupGetByCodeRequest
    {
        public string Code { get; set; }
        public int ShopID { get; set; }
    }
    public class ZProductGroupGetByCodeResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
