using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAddFeedbackAutoRequest
    {
        public int CreatedBy { get; set; }
        public string PageID { get; set; }
        public int ShopID { get; set; }
        public int ScriptType { get; set; }
        public string ScriptName { get; set; }
        public string Message { get; set; }
        public string ResponseTime { get; set; }
        public int ExecutionTimePeriod { get; set; }
        public string FromTime { get; set; }
        public string EndTime { get; set; }
    }

    public class ZFacebookSearchFeedbackAutoRequest
    {
        public int ShopID { get; set; }
        public string PageID { get; set; }
        public int ScriptType { get; set; }
    }

    public class ZFacebookSearchFeedbackAutoResponse
    {
        public string PageName { get; set; }
        public int ID { get; set; }
        public int ShopID { get; set; }
        public string PageID { get; set; }
        public int ScriptType { get; set; }
        public string ScriptName { get; set; }
        public string Message { get; set; }
        public string ResponseTime { get; set; }
        public int ExecutionTimePeriod { get; set; }
        public int Status { get; set; }
        public string FromTime { get; set; }
        public string EndTime { get; set; }
    }

    public class ZFacebookUpdateFeedbackAutoRequest
    {
        public int ID { get; set; }
        public int Status { get; set; }
        public int ShopID { get; set; }
        public int ScriptType { get; set; }
        public int ExecutionTimePeriod { get; set; }
        public int UpdatedBy { get; set; }
        public string PageID { get; set; }
    }
}
