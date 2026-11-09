using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Tiktok.Models
{
    public class TIK_DashboardViewModel
    {
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageAvatar { get; set; }
        public string PageCategory { get; set; }
        public string UserAccessToken { get; set; }
        public string PageAccessToken { get; set; }
    }
}