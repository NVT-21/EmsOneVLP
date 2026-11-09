using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    public class FAB_LiveChatViewModel
    {
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageAvatar { get; set; }
        public string ConversionID { get; set; }
        public int ConversionStickerID { get; set; }
        public List<FAB_SearchListConversationStickerViewModel> ListConversionSticker { get; set; }
    }
}