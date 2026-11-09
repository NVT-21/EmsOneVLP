using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAddWorkingTimeRequest
    {
        public string PageID { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ZFacebookEditWorkingTimeRequest
    {
        public int ID { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public string Note { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZFacebookDeleteWorkingTimeRequest
    {
        public int ID { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZFacebookSearchWorkingTimeRequest
    {
        public int ShopID { get; set; }
        public string PageID { get; set; }
    }

    public class ZFacebookSearchWorkingTimeResponse
    {
        public int ID { get; set; }
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageCategory { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
    }

    public class ZFacebookGetWorkingTimeByIDRequest
    {
        public int ID { get; set; }
    }
}
