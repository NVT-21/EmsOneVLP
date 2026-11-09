using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EMS.Entity;
using EMS.MVC.Frontend.Repositories;

namespace EMS.MVC.Frontend.Controllers
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeController : AuthorizeAttribute
    {
        //Custom named parameters for annotation
        public string FunctionalTitle { get; set; }

        //Called when access is denied
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx.Session != null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if (ctx.Session[XHelper.SessionEmployee] == null)
                    {
                        filterContext.HttpContext.Response.ClearContent();
                        filterContext.HttpContext.Items["AjaxSessionExpired"] = true;
                    }
                    else
                    {
                        filterContext.HttpContext.Response.ClearContent();
                        filterContext.HttpContext.Items["AjaxNotPermission"] = true;
                    }
                }
                else
                {
                    if (ctx.Session[XHelper.SessionEmployee] == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                               new RouteValueDictionary(new { controller = "Account", action = "LogOut" })
                       );
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(new { controller = "Error", action = "NotPermission" })
                        );
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                               new RouteValueDictionary(new { controller = "Account", action = "LogOut" })
                       );
            }
        }

        //Core authentication, called before each action
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx.Session != null)
            {
                if (ctx.Session[XHelper.SessionEmployee] == null)
                {
                    return false;
                }
                else
                {
                    if (XHelper.Session.EmployeeProfile.StoreOwner == "Y")
                    {
                        return true;
                    }
                    else
                    {
                        var res = PermissionRepository.CheckFunctional(new ZFunctionalCheckRequest()
                        {
                            EmployeeID = XHelper.Session.EmployeeProfile.ID,
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                            FunctionalTitle = FunctionalTitle
                        });
                        if (res.Code == "00")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
    }
}