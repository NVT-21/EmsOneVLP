using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeRemoveRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZEmployeeRemoveViewRequest
    {
        public string ID { get; set; }
        public string ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
