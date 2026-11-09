using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    public class FAB_DashboardViewModel
    {
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageAvatar { get; set; }
        public string PageCover { get; set; }
        public string PageCategory { get; set; }
        public string UserAccessToken { get; set; }
        public string PageAccessToken { get; set; }
        public List<FAB_SearchListConversionViewModel> ListConversion { get; set; }
        public List<FAB_ListFeedForCommentViewModel> ListFeed { get; set; }
    }
}