using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestOrderRejectRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public int Status { get; set; }
        public int UpdatedBy { get; set; }
        public string DifferentStatus { get; set; }
        public string isCheckInsertReason { get; set; }
    }
}
