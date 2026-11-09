using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListChannelViewModel
    {
        public int ID { get; set; }
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageCategory { get; set; }
        public string PageDescription { get; set; }
        public string PageAvatar { get; set; }
        public string PageCover { get; set; }
        public string IsActive { get; set; }
        public string Channel { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public string IsActiveCheck { get; set; }
        public string AccountID { get; set; }
    }
}