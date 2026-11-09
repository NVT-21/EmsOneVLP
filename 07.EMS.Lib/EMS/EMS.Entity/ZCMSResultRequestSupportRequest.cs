using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCMSResultRequestSupportRequest
    {
        public string RequestSupportCode { get; set; }
    }
    public class ZCMSResultRequestSupportResponse
    {
        public string RequestSupportCode { get; set; }
        public string ttkId { get; set; }
        public string ttkStatus { get; set; }
        public List<ZCMSResultRequestSupportTicketActionResponse> lstTTicketActionModel { get; set; }
    }
    public class ZCMSResultRequestSupportTicketActionResponse
    {
        public string RequestSupportCode { get; set; }
        public string actId { get; set; }
        public string ttkId { get; set; }
        public string actType { get; set; } //Trạng thái
        public string actContent { get; set; } 
        public string actAttachFile { get; set; } 
        public string createdUser { get; set; } 
        public string createdOrg { get; set; }
        public string createdDate { get; set; } 
        public string modifyDate { get; set; } 
        public string actResult { get; set; } 
        public string nextOrg { get; set; } 
        public string nextOrgName { get; set; } 
        public string refOrg { get; set; } 
        public string refOrgName { get; set; } 
    }
}
