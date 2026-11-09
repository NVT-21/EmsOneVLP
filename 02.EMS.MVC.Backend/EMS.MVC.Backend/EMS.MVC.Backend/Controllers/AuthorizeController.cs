using EMS.Entity;
using EMS.MVC.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EMS.MVC.Backend.Controllers
{
    public class AuthorizeController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ZResponse res = new ZResponse();
            if ((HttpContext.Current.Session[XHelper.SessionUser] as UserProfile) != null)
            {
                if ((HttpContext.Current.Session[XHelper.SessionUser] as UserProfile).GroupCode == ".0.0.")
                {
                    return;
                }
                else if((HttpContext.Current.Session[XHelper.SessionUser] as UserProfile).GroupCode == ".3.0.")
                {
                    string CurrentControllerName = Convert.ToString(HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"]);
                    switch (CurrentControllerName)
                    {
                        case "User":
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary { { "Area", "" }, { "Controller", "Home" }, { "Action", "Index" }, { "returnUrl", filterContext.HttpContext.Request.RawUrl } });
                            break;
                        case "Shop":
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary { { "Area", "" }, { "Controller", "Home" }, { "Action", "Index" }, { "returnUrl", filterContext.HttpContext.Request.RawUrl } });
                            break;
                        case "ShopGroupPermission":
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary { { "Area", "" }, { "Controller", "Home" }, { "Action", "Index" }, { "returnUrl", filterContext.HttpContext.Request.RawUrl } });
                            break;
                        case "Reconciliation":
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary { { "Area", "" }, { "Controller", "Home" }, { "Action", "Index" }, { "returnUrl", filterContext.HttpContext.Request.RawUrl } });
                            break;
                        case "Payment":
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary { { "Area", "" }, { "Controller", "Home" }, { "Action", "Index" }, { "returnUrl", filterContext.HttpContext.Request.RawUrl } });
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "Area", "" }, { "Controller", "Login" }, { "Action", "Index" }, { "returnUrl", filterContext.HttpContext.Request.RawUrl } });
            }
        }
    }
}