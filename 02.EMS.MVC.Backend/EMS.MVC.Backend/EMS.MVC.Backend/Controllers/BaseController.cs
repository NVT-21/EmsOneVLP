using EMS.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using System.Web;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
using EMS.Library;
namespace EMS.MVC.Backend.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //HttpContext ctx = System.Web.HttpContext.Current;
            //// check if session is supported  
            //if (ctx.Session != null)
            //{
            //    // check if a new session id was generated  
            //    if (ctx.Session[XHelper.SessionUser] == null)
            //    {
            //        //Check is Ajax request  
            //        if (filterContext.HttpContext.Request.IsAjaxRequest())
            //        {
            //            filterContext.HttpContext.Response.ClearContent();
            //            filterContext.HttpContext.Items["AjaxSessionExpired"] = true;
            //        }
            //        // check if a new session id was generated  
            //        else
            //        {
            //            filterContext.Result = new RedirectResult("~/Login/Index");
            //        }
            //    }
            //    else
            //    {
            //        var res = XHelper.Invoke(new ZRequest()
            //        {
            //            Code = "USER_CHECK_CONDITION",
            //            Data = JsonConvert.SerializeObject(new ZUserCheckConditionRequest()
            //            {
            //                UserID = XHelper.Session.UserProfile.ID,
            //            })
            //        });
            //        if (res.Code != "00")
            //        {
            //            filterContext.HttpContext.Response.ClearContent();
            //            filterContext.HttpContext.Items["AjaxCheckCondition"] = true;
            //            filterContext.HttpContext.Items["AjaxCheckConditionCode"] = res.Code;
            //        }
            //    }
            //}
            var CheckSession = Session[XHelper.SessionUser] as UserProfile;
            if (CheckSession == null)
            {
                HttpCookie authCookie = Request.Cookies["be_panelIdCookie"];
                if (authCookie != null)
                {
                    var _collection = authCookie.Values;
                    if (_collection != null && _collection.Count > 0)
                    {
                        string _UserName = Security.MD5Decrypt(authCookie.Values[00], Security.PASSWORD_SALT);

                        var res = UserRepository.GetByUserName(new ZUserGetByUserNameRequest()
                        {
                            UserName = _UserName,
                        });
                        if (res.Code == "00")
                        {
                            var item = JsonConvert.DeserializeObject<ZUserLoginResponse>(res.Data);
                            var User = new UserProfile()
                            {
                                ID = item.ID,
                                GroupName = item.GroupName,
                                UserName = item.UserName,
                                FullName = item.FullName,
                                MobileNumber = item.MobileNumber,
                                Email = item.Email,
                                GroupCode = item.GroupCode
                            };
                            Session[XHelper.SessionUser] = User;
                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(new { controller = "Login", action = "LogOut" })
                        );
                    }
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary(new { controller = "Login", action = "LogOut" })
                    );
                }

                CheckSession = Session[XHelper.SessionUser] as UserProfile;

                if (CheckSession == null)
                {
                    //XHelper.LogToFile(LogFileType.TRACE, "BaseController.OnActionExecuting: 2");
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.ClearContent();
                        filterContext.HttpContext.Items["AjaxSessionExpired"] = true;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(new { controller = "Login", action = "LogOut" })
                        );
                    }
                }
            }
            else
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "USER_CHECK_CONDITION",
                    Data = JsonConvert.SerializeObject(new ZUserCheckConditionRequest()
                    {
                        UserID = XHelper.Session.UserProfile.ID,
                    })
                });
                if (res.Code != "00")
                {
                    filterContext.HttpContext.Response.ClearContent();
                    filterContext.HttpContext.Items["AjaxCheckCondition"] = true;
                    filterContext.HttpContext.Items["AjaxCheckConditionCode"] = res.Code;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}