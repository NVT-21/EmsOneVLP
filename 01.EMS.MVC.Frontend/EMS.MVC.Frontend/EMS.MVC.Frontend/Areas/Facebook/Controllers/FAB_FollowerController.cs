using EMS.MVC.Frontend.Areas.Facebook.Models;
using EMS.MVC.Frontend.Controllers;
using EMS.MVC.Frontend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Facebook.Controllers
{
    public class FAB_FollowerController : FAB_BaseController
    {
        // GET: Facebook/FAB_Follower
        public ActionResult FAB_FollowerView()
        {
            return View();
        }
        public ActionResult FAB_SearchListFollowerView()
        {
            List<FAB_SearchListFollowerViewModel> model = new List<FAB_SearchListFollowerViewModel>();
            model = FacebookRepository.ListFollower();
            return PartialView(model);
        }
    }
}