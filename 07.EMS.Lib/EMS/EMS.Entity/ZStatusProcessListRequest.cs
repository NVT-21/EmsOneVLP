using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStatusProcessListRequest
    {
        public int ShopID { get; set; }
        public string IsLock { get; set; }
    }
    public class ZStatusProcessListResponse
    {
        public int ID { get; set; }
        public string Note { get; set; }
        public string IsLock { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
}
