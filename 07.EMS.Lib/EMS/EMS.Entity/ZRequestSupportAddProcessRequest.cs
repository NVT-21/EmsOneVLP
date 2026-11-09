using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestSupportAddProcessRequest
    {
        public string RequestSupportCode { get; set; }
        public int ShopID { get; set; }
        public string AmndState { get; set; }
        public string ActID { get; set; }
        public string ActType { get; set; }
        public string ActContent { get; set; }
        public string ActAttachFile { get; set; }
        public string CreatedUser { get; set; }
        public string CreatedOrg { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyDate { get; set; }
        public string ActResult { get; set; }
        public string NextOrg { get; set; }
        public string NextOrgName { get; set; }
        public string RefOrg { get; set; }
        public string RefOrgName { get; set; }
    }
}
