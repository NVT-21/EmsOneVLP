using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using EMS.Entity;

namespace EMS.MVC.Frontend.Controllers
{
    public class SessionController : Controller
    {
        #region Employee
        private EmployeeModel Employee = null;

        public EmployeeModel EmployeeProfile
        {
            get
            {
                Employee = (EmployeeModel)System.Web.HttpContext.Current.Session[XHelper.SessionEmployee];
                return Employee;
            }

            set
            {
                Employee = value;
            }
        }
        #endregion

        #region Zalo
        private ZaloModel Zalo = null;

        public ZaloModel ZaloProfile
        {
            get
            {
                Zalo = (ZaloModel)System.Web.HttpContext.Current.Session[XHelper.SessionZalo];
                return Zalo;
            }

            set
            {
                Zalo = value;
            }
        }
        #endregion

        #region Facebook
        private FacebookModel Facebook = null;

        public FacebookModel FacebookProfile
        {
            get
            {
                Facebook = (FacebookModel)System.Web.HttpContext.Current.Session[XHelper.SessionFacebook];
                return Facebook;
            }

            set
            {
                Facebook = value;
            }
        }
        #endregion

        #region Tiktok
        private TiktokModel Tiktok = null;

        public TiktokModel TiktokProfile
        {
            get
            {
                Tiktok = (TiktokModel)System.Web.HttpContext.Current.Session[XHelper.SessionTiktok];
                return Tiktok;
            }

            set
            {
                Tiktok = value;
            }
        }
        #endregion
    }
}