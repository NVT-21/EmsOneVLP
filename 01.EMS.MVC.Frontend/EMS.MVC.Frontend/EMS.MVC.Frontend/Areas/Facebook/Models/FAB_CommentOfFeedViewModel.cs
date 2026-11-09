using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    public class FAB_CommentOfFeedViewModel
    {
        public XFeedModel Feed { get; set; }
        public List<XCommentModel> Comment { get; set; }
    }
}