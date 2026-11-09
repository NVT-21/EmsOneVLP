using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOptionGetDataByParentIDRequest
    {
        public int ParentID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZOptionGetDataByParentIDResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
