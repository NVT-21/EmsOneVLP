using EMS.Library;
using EMS.Entity;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
namespace EMS.MVC.Frontend.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (filterContext.RouteData.Values["controller"] + "").Trim();
            var action = (filterContext.RouteData.Values["action"] + "").Trim();

            // ✅ Nếu là DonHang/TaoDonHangLe thì bỏ qua check, cho chạy luôn
            if((controller.Equals("Order", StringComparison.OrdinalIgnoreCase) &&
                action.Equals("WalkInOrderView", StringComparison.OrdinalIgnoreCase)) ||
                (controller.Equals("Order", StringComparison.OrdinalIgnoreCase) &&
                action.Equals("WalkInTemporaryTransportServiceFeeView", StringComparison.OrdinalIgnoreCase)) ||
                (controller.Equals("Order", StringComparison.OrdinalIgnoreCase) &&
                action.Equals("JAddWalkInOrder", StringComparison.OrdinalIgnoreCase)) ||
                (controller.Equals("Order", StringComparison.OrdinalIgnoreCase) &&
                action.Equals("ReturnOrderHMView", StringComparison.OrdinalIgnoreCase)) ||
                (controller.Equals("Order", StringComparison.OrdinalIgnoreCase) &&
                action.Equals("JAddReturnOrderHM", StringComparison.OrdinalIgnoreCase)) ||
                (controller.Equals("Order", StringComparison.OrdinalIgnoreCase) &&
                action.Equals("PrintSingleOrderView", StringComparison.OrdinalIgnoreCase)) ||
                (controller.Equals("Order", StringComparison.OrdinalIgnoreCase) &&
                action.Equals("PrintSingleOrderTypeView", StringComparison.OrdinalIgnoreCase)) ||
                (controller.Equals("Order", StringComparison.OrdinalIgnoreCase) &&
                action.Equals("JUpdatePrintStatus", StringComparison.OrdinalIgnoreCase))
                )
            {
                base.OnActionExecuting(filterContext);
                return;
            }



            //XHelper.LogToFile(LogFileType.TRACE, "BaseController.OnActionExecuting: 1");
            var CheckSession = Session[XHelper.SessionEmployee] as EmployeeModel;
            if (CheckSession == null)
            {
                HttpCookie authCookie = Request.Cookies["panelIdCookie"];

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
                            Session[XHelper.SessionEmployee] = Employee;
                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(new { controller = "Account", action = "LogOut" })
                        );
                    }
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(new { controller = "Account", action = "LogOut" })
                        );
                }

                CheckSession = Session[XHelper.SessionEmployee] as EmployeeModel;

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
                           new RouteValueDictionary(new { controller = "Account", action = "LogOut" })
                        );
                    }
                }
            }
            else
            {
                //XHelper.LogToFile(LogFileType.TRACE, "BaseController.OnActionExecuting: 3");
                var res = EmployeeRepository.CheckEmployeeCondition(new ZEmployeeCheckConditionRequest() { EmployeeID = XHelper.Session.EmployeeProfile.ID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (res.Code != "00")
                {
                    //XHelper.LogToFile(LogFileType.TRACE, "BaseController.OnActionExecuting: 4 || " + JsonConvert.SerializeObject(res));
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.ClearContent();
                        filterContext.HttpContext.Items["AjaxCheckCondition"] = true;
                        filterContext.HttpContext.Items["AjaxCheckConditionCode"] = res.Code;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(new { controller = "Account", action = "LogOut" })
                        );
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if ((filterContext.ActionDescriptor.ActionName == "JLogin" || filterContext.ActionDescriptor.ActionName == "LoginView") && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Account")
        //    {

        //    }
        //    else
        //    {
        //        HttpContext ctx = System.Web.HttpContext.Current;
        //        if (ctx.Session != null)
        //        {
        //            //XHelper.LogToFile(LogFileType.TRACE, "BaseController.OnActionExecuting: 1");
        //            if (ctx.Session[XHelper.SessionEmployee] == null)
        //            {
        //                if (filterContext.HttpContext.Request.IsAjaxRequest())
        //                {
        //                    filterContext.HttpContext.Response.ClearContent();
        //                    filterContext.HttpContext.Items["AjaxSessionExpired"] = true;
        //                }
        //                else
        //                {
        //                    //XHelper.LogToFile(LogFileType.TRACE, "BaseController.OnActionExecuting: 2");
        //                    filterContext.Result = new RedirectToRouteResult(
        //                       new RouteValueDictionary(new { controller = "Account", action = "LogOut" })
        //                    );
        //                }
        //            }
        //            else
        //            {
        //                //XHelper.LogToFile(LogFileType.TRACE, "BaseController.OnActionExecuting: 3");
        //                var res = EmployeeRepository.CheckEmployeeCondition(new ZEmployeeCheckConditionRequest() { EmployeeID = XHelper.Session.EmployeeProfile.ID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
        //                if (res.Code != "00")
        //                {
        //                    //XHelper.LogToFile(LogFileType.TRACE, "BaseController.OnActionExecuting: 4 || " + JsonConvert.SerializeObject(res));
        //                    if (filterContext.HttpContext.Request.IsAjaxRequest())
        //                    {
        //                        filterContext.HttpContext.Response.ClearContent();
        //                        filterContext.HttpContext.Items["AjaxCheckCondition"] = true;
        //                        filterContext.HttpContext.Items["AjaxCheckConditionCode"] = res.Code;
        //                    }
        //                    else
        //                    {
        //                        filterContext.Result = new RedirectToRouteResult(
        //                                new RouteValueDictionary(new { controller = "Error", action = "NotPermission" })
        //                        );
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    base.OnActionExecuting(filterContext);
        //}
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if(filterContext.ActionDescriptor.ActionName == "JLogin" && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Account")
        //    {

        //    }
        //    else
        //    {
        //        HttpContext ctx = System.Web.HttpContext.Current;
        //        ZResponse res = new ZResponse();
        //        if (filterContext.HttpContext.Request.IsAjaxRequest())
        //        {
        //            if (ctx.Session != null)
        //            {
        //                if (ctx.Session[XHelper.SessionEmployee] == null)
        //                {
        //                    filterContext.HttpContext.Response.ClearContent();
        //                    filterContext.HttpContext.Items["AjaxSessionExpired"] = true;
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        res = EmployeeRepository.CheckEmployeeCondition(new ZEmployeeCheckConditionRequest() { EmployeeID = XHelper.Session.EmployeeProfile.ID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
        //                        if (res.Code != "00")
        //                        {
        //                            filterContext.HttpContext.Response.ClearContent();
        //                            filterContext.HttpContext.Items["AjaxCheckCondition"] = true;
        //                            filterContext.HttpContext.Items["AjaxCheckConditionCode"] = res.Code;
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        XHelper.LogToFile(LogFileType.EXCEPTION, "BaseController.OnActionExecuting: ajax request ||" + ex.Message);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                filterContext.HttpContext.Response.ClearContent();
        //                filterContext.HttpContext.Items["AjaxSessionExpired"] = true;
        //            }
        //        }
        //        else
        //        {
        //            if (ctx.Session != null)
        //            {
        //                if (ctx.Session[XHelper.SessionEmployee] == null)
        //                {
        //                    filterContext.Result = new RedirectToRouteResult(
        //                       new RouteValueDictionary(new { controller = "Account", action = "LogOut" })
        //                    );
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        res = EmployeeRepository.CheckEmployeeCondition(new ZEmployeeCheckConditionRequest() { EmployeeID = XHelper.Session.EmployeeProfile.ID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
        //                        if (res.Code != "00")
        //                        {
        //                            filterContext.Result = new RedirectToRouteResult(
        //                                    new RouteValueDictionary(new { controller = "Error", action = "NotPermission" })
        //                            );
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        XHelper.LogToFile(LogFileType.EXCEPTION, "BaseController.OnActionExecuting: action request ||" + ex.Message);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                filterContext.Result = new RedirectToRouteResult(
        //                       new RouteValueDictionary(new { controller = "Account", action = "LogOut" })
        //                    );
        //            }
        //        }
        //    }
        //    base.OnActionExecuting(filterContext);
        //}
    }
}