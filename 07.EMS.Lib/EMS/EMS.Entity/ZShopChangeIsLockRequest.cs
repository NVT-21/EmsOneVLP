using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopChangeIsLockRequest
    {
        public int ID { get; set; }
        public string IsLock { get; set; }
        public int UpdatedBy { get; set; }
    }
}
