using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeGroupEditRequest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsLock { get; set; }
        public string strEmployeeID { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
