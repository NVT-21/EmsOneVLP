using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using EMS.Library;
using EMS.Entity;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;

namespace EMS.MVC.Frontend.Areas.Zalo.Controllers
{
    public class ZAL_BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var CheckSession = Session[XHelper.SessionEmployee] as EmployeeModel;
            var CheckZalo = Session[XHelper.SessionZalo] as ZaloModel;
            if (CheckSession == null || CheckZalo == null)
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

                            string _ChannelID = Security.MD5Decrypt(authCookie.Values[01], Security.PASSWORD_SALT);

                            var resConnectPage = ChannelRepository.SearchListChannel(new ZConnectPageSearchListRequest()
                            {
                                ShopID = Employee.ShopInformation.ID,
                                PageID = _ChannelID,
                                Channel = "ZA",
                                IsActive = "Y"
                            });

                            if (resConnectPage.Code == "00")
                            {
                                var itemConnectPage = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(resConnectPage.Data).First();
                                Session[XHelper.SessionZalo] = new ZaloModel()
                                {
                                    PageID = itemConnectPage.PageID,
                                    PageName = itemConnectPage.PageName,
                                    PageAvatar = itemConnectPage.PageAvatar,
                                    PageCover = itemConnectPage.PageCover,
                                    PageDescription = itemConnectPage.PageDescription,
                                    AccessToken = itemConnectPage.AccessToken,
                                    RefreshToken = itemConnectPage.RefreshToken
                                };
                            }
                            else
                            {
                                filterContext.Result = new RedirectResult("/thong-tin-tai-khoan");
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
                else
                {
                    if (Session[XHelper.SessionZalo] == null)
                    {
                        filterContext.Result = new RedirectResult("/thong-tin-tai-khoan");
                    }
                }
            }
            else
            {
                HttpCookie authCookie = Request.Cookies["panelIdCookie"];
                if (authCookie != null)
                {
                    var _collection = authCookie.Values;
                    if (_collection != null && _collection.Count > 0)
                    {
                        string _ChannelID = Security.MD5Decrypt(authCookie.Values[01], Security.PASSWORD_SALT);
                        
                        if (_ChannelID != XHelper.Session.ZaloProfile.PageID)
                        {
                            var resConnectPage = ChannelRepository.SearchListChannel(new ZConnectPageSearchListRequest()
                            {
                                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                                PageID = _ChannelID,
                                Channel = "ZA",
                                IsActive = "Y"
                            });

                            if (resConnectPage.Code == "00")
                            {
                                var itemConnectPage = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(resConnectPage.Data).First();
                                Session[XHelper.SessionZalo] = new ZaloModel()
                                {
                                    PageID = itemConnectPage.PageID,
                                    PageName = itemConnectPage.PageName,
                                    PageAvatar = itemConnectPage.PageAvatar,
                                    PageCover = itemConnectPage.PageCover,
                                    PageDescription = itemConnectPage.PageDescription,
                                    AccessToken = itemConnectPage.AccessToken,
                                    RefreshToken = itemConnectPage.RefreshToken
                                };
                            }
                            else
                            {
                                filterContext.Result = new RedirectResult("/thong-tin-tai-khoan");
                            }
                        }
                    }
                }

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

            //HttpContext ctx = System.Web.HttpContext.Current;
            //if (ctx.Session[XHelper.SessionEmployee] == null)
            //{
            //    //Check is Ajax request  
            //    if (filterContext.HttpContext.Request.IsAjaxRequest())
            //    {
            //        filterContext.HttpContext.Response.ClearContent();
            //        filterContext.HttpContext.Items["AjaxSessionExpired"] = true;
            //    }
            //    // check if a new session id was generated  
            //    else
            //    {
            //        filterContext.Result = new RedirectResult("/dang-nhap");
            //    }
            //}
            //else
            //{
            //    if(ctx.Session[XHelper.SessionZalo] == null)
            //    {
            //        filterContext.Result = new RedirectResult("/thong-tin-tai-khoan");
            //    }
            //}
            //base.OnActionExecuting(filterContext);
        }
    }
}