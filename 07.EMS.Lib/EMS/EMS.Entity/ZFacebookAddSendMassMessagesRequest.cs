using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAddSendMassMessagesRequest
    {
        public int CreatedBy { get; set; }
        //public int ShopID { get; set; }
        //public string PageID { get; set; }
        public int CampaignID { get; set; }
        public string FacebookID { get; set; }
        //public string Message { get; set; }
        //public string SendDate { get; set; }
        public int Status { get; set; }
    }

    public class ZFacebookSearchSendMassMessagesRequest
    {
        public int ShopID { get; set; }
        public string PageID { get; set; }
        public int CampaignID { get; set; }
    }

    public class ZFacebookSearchSendMassMessagesResponse
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public string PageID { get; set; }
        public string FacebookID { get; set; }
        public string Message { get; set; }
        public string SendDate { get; set; }
        public int Status { get; set; }
        public int Received { get; set; }
        public string EmpName { get; set; }
        public string CustomerName { get; set; }
    }
}
