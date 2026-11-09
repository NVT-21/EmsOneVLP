using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Zalo.Models
{
    public class ZAL_DashboardViewModel
    {
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageAvatar { get; set; }
        public string PageCover { get; set; }
        public string PageDescription { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}