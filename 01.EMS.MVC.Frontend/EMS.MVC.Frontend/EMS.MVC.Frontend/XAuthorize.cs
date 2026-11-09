using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;

namespace EMS.MVC.Frontend
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class XAuthorize : AuthorizeAttribute
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
            var CheckSession = HttpContext.Current.Session[XHelper.SessionEmployee] as EmployeeModel;
            if (CheckSession == null)
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies["panelIdCookie"];
                if (authCookie != null)
                {
                    var _collection = authCookie.Values;

                    if (_collection != null && _collection.Count > 0)
                    {
                        string _MobileNumber = Security.MD5Decrypt(authCookie.Values[00], Security.PASSWORD_SALT);

                        var res = EmployeeRepository.SearchByMobileNumber(new ZEmployeeGetByMobileNumberRequest()
                        {
                            MobileNumber = _MobileNumber,
                        });

                        if (res.Code == "00")
                        {
                            var item = JsonConvert.DeserializeObject<ZEmployeeGetByMobileNumberResponse>(res.Data);
                            var Employee = new EmployeeModel()
                            {
                                ID = item.ID,
                                StoreOwner = item.StoreOwner,
                                UserName = item.UserName,
                                FullName = item.FullName,
                                MobileNumber = item.MobileNumber,
                                Email = item.Email,
                                IsLock = item.IsLock,
                                EmployeeLevel = item.EmployeeLevel,
                                GroupPermissionID = item.GroupPermissionID,
                                ReadyReceiveRequestOrder = item.ReadyReceiveRequestOrder,
                                ShopInformation = item.ShopInformation
                            };
                            HttpContext.Current.Session[XHelper.SessionEmployee] = Employee;
                        }
                    }
                    else
                    {
                        return false;
                    }

                    CheckSession = HttpContext.Current.Session[XHelper.SessionEmployee] as EmployeeModel;

                    if (CheckSession == null)
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            return true;
            //HttpContext ctx = HttpContext.Current;
            //if (ctx.Session != null)
            //{
            //    if (ctx.Session[XHelper.SessionEmployee] == null)
            //    {
            //        return false;
            //    }
            //    else
            //    {
            //        if (XHelper.Session.EmployeeProfile.StoreOwner == "Y")
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            var res = PermissionRepository.CheckFunctional(new ZFunctionalCheckRequest() 
            //            {
            //                EmployeeID = XHelper.Session.EmployeeProfile.ID,
            //                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
            //                FunctionalTitle = FunctionalTitle
            //            });
            //            if(res.Code == "00")
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //}
            //return false;
        }
    }
}