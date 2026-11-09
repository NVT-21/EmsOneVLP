using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.MVC.Frontend.Areas.Zalo.Models;
namespace EMS.MVC.Frontend.Areas.Zalo.Models
{
    public class ZAL_SearchListMessageOfConversionViewModel
    {
        public int ConversionID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string PageName { get; set; }
        public string PageAvatar { get; set; }
        public List<ZAL_MessageOfConversionViewModel> ListMessage { get; set; }
    }
}