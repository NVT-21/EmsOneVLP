using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Backend.Models
{
    public class NotifyListViewModel
    {
        public int MaxRequestSupportID { get; set; }
        public int CountRequestSupport { get; set; }
        public List<ZRequestSupportNotifyListDetail> ListRequestSupportNotify { get; set; }
    }
}