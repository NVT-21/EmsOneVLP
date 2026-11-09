using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeChangeIsLockRequest
    {
        public int ID { get; set; }
        public string IsLock { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZEmployeeChangeIsLockViewRequest
    {
        public string ID { get; set; }
        public string IsLock { get; set; }
        public int UpdatedBy { get; set; }
    }
}
