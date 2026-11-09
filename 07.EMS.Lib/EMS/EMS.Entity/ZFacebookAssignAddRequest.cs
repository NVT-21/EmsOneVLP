using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAssignAddRequest
    {
        public string CreatedBy { get; set; }
        public string NameAssign { get; set; }
        public string PageID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZFacebookAssignDeleteRequest
    {
        public string ID { get; set; }
        public string UpdateBy { get; set; }
    }
    public class ZFacebookAssignEditRequest
    {
        public string ID { get; set; }
        public string NameAssign { get; set; }
        public string UpdateBy { get; set; }
    }
    public class ZFacebookDeleteAssignModelRequest
    {
        public string ConversationID { get; set; }
        public string UpdatedBy { get; set; }
    }
}
