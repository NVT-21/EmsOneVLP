using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EMS.MVC.Backend
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_EndRequest()
        {
            if (Context.Items["AjaxSessionExpired"] is bool)
            {
                Context.Response.StatusCode = 401;
                Context.Response.End();
            }
            if (Context.Items["AjaxCheckCondition"] is bool)
            {
                Context.Response.StatusCode = Convert.ToInt32(Context.Items["AjaxCheckConditionCode"]);
                Context.Response.End();
            }
        }
    }
}
