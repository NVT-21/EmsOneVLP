using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPayPostControlFreighGetItemInfoRequest
    {
        public string Signature { get; set; }
        public string CustomerCode { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }
    }
    public class ZPayPostControlFreighGetItemInfoResponse
    {
        public int ReturnID { get; set; }
        public string CustomerCode { get; set; }
        public string ListCode { get; set; }
        public string EmsBarCode { get; set; }
        public long CODMoney { get; set; }
        public string IssueDate { get; set; }
        public int CODTransType { get; set; }
        public string UpdateTime { get; set; }
    }
    public class ZPayPostControlFreighReturnResultRequest
    {
        public string Signature { get; set; }
        public int ReturnID { get; set; }
        public int IsAgree { get; set; }
        public string ReasonReject { get; set; }
    }
}
