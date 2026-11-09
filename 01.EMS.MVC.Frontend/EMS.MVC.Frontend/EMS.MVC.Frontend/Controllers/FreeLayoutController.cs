using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Controllers
{
    public class FreeLayoutController : Controller
    {
        // GET: FreeLayout
        public ActionResult DeleteAccount()
        {
            return PartialView();
        }
    }
}