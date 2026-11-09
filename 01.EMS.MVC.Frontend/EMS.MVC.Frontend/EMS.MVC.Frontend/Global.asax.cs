using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EMS.MVC.Frontend
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_EndRequest()
        {
            if (Context.Items["AjaxSessionExpired"] is bool)
            {
                Context.Response.StatusCode = 408;
                Context.Response.End();
            }
            else if (Context.Items["AjaxCheckCondition"] is bool)
            {
                Context.Response.StatusCode = Convert.ToInt32(Context.Items["AjaxCheckConditionCode"]);
                Context.Response.End();
            }
            else if (Context.Items["AjaxNotPermission"] is bool)
            {
                Context.Response.StatusCode = 403;
                Context.Response.End();
            }
        }
        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        //    if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
        //    {
        //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization, APIKey");
        //        HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "‎17280000");
        //        HttpContext.Current.Response.End();
        //    }
        //}
    }
}
