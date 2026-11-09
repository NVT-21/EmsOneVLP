using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAddMessageCampaignRequest
    {
        public int CreatedBy { get; set; }
        public string NameCampaign { get; set; }
        public int ShopID { get; set; }
        public string PageID { get; set; }
        public string Message { get; set; }
        public string SendDate { get; set; }
    }

    public class ZFacebookListMessageCampaignRequest
    {
        public int ShopID { get; set; }
        public string PageID { get; set; }
    }

    public class ZFacebookListMessageCampaignResponse
    {
        public int ID { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string NameCampaign { get; set; }
        public string Message { get; set; }
        public string SendDate { get; set; }
        public string EmployeeName { get; set; }
    }
}
