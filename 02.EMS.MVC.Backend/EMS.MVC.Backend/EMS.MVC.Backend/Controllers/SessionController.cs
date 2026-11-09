using EMS.MVC.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Controllers
{
    public class SessionController : Controller
    {
        private UserProfile _oUserProfile = null;

        public UserProfile UserProfile
        {
            get
            {
                _oUserProfile = (UserProfile)System.Web.HttpContext.Current.Session[XHelper.SessionUser];
                return _oUserProfile;
            }

            set
            {
                _oUserProfile = value;
            }
        }
    }
}