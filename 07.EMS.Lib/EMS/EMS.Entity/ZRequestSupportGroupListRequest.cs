using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestSupportGroupListRequest
    {
        public int ParentID { get; set; }
    }
    public class ZRequestSupportGroupListResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string CmsReasonForComplaint { get; set; }
    }
}
