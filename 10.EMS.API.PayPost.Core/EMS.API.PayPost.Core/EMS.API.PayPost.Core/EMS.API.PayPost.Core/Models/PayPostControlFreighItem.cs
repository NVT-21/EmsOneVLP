using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.PayPost.Core.Models
{
    public class PayPostControlFreighItem
    {
        public long CODMoney { get; set; }
        public string CustomerCode { get; set; }
        public string EmsBarCode { get; set; }
        public string ListCode { get; set; }
        public int ReturnID { get; set; }
        /// <summary>
        /// 1 -Phat sinh, 2 - Huy, 3 - Chuyen Hoan, 4 - Ontheway, 5 - Da thu, 6 - Da tra La trang thai status trong collection_on_delivery
        /// </summary>
        public int CODTransType { get; set; }
        public string UpdateTime { get; set; }
        public string IssueDate { get; set; }
    }
}