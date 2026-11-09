using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Zalo.Models
{
    public class ZAL_SearchListConversionViewModel
    {
        public int ConversionID { get; set; }
        public string UserID { get; set; }
        public string SenderID { get; set; }
        public string DisplayName { get; set; }
        public string MobileNumber { get; set; }
        public string Avatar { get; set; }
        public string MessageText { get; set; }
        public string MessageType { get; set; }
        public int NumberOfMessage { get; set; }
        public string UpdatedDate { get; set; }
        public int ConversionIndex { get; set; }
    }
}