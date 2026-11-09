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

namespace EMS.MVC.Frontend.Areas.Tiktok.Controllers
{
    public class TIK_BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var CheckSession = Session[XHelper.SessionEmployee] as EmployeeModel;
            var CheckTiktok = Session[XHelper.SessionTiktok] as TiktokModel;
            if (CheckSession == null || CheckTiktok == null)
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
                                Channel = "TIK",
                                IsActive = "Y"
                            });

                            if (resConnectPage.Code == "00")
                            {
                                var itemConnectPage = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(resConnectPage.Data).First();
                                Session[XHelper.SessionTiktok] = new TiktokModel()
                                {
                                    PageID = itemConnectPage.PageID,
                                    PageName = itemConnectPage.PageName,
                                    PageAvatar = itemConnectPage.PageAvatar,
                                    PageCategory = itemConnectPage.PageCategory,
                                    UserAccessToken = itemConnectPage.UserAccessToken,
                                    PageAccessToken = itemConnectPage.PageAccessToken,
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
                           new RouteValueDictionary(new { controller = "Account", action = "LogOut", area = "" })
                        );
                    }
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(new { controller = "Account", action = "LogOut", area = "" })
                        );
                }

                CheckSession = Session[XHelper.SessionEmployee] as EmployeeModel;

                if (CheckSession == null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.ClearContent();
                        filterContext.HttpContext.Items["AjaxSessionExpired"] = true;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary(new { controller = "Account", action = "LogOut", area = "" })
                        );
                    }
                }
                else
                {
                    if (Session[XHelper.SessionTiktok] == null)
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

                        if (_ChannelID != XHelper.Session.TiktokProfile.PageID)
                        {
                            var resConnectPage = ChannelRepository.SearchListChannel(new ZConnectPageSearchListRequest()
                            {
                                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                                PageID = _ChannelID,
                                Channel = "TIK",
                                IsActive = "Y"
                            });

                            if (resConnectPage.Code == "00")
                            {
                                var itemConnectPage = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(resConnectPage.Data).First();
                                Session[XHelper.SessionTiktok] = new TiktokModel()
                                {
                                    PageID = itemConnectPage.PageID,
                                    PageName = itemConnectPage.PageName,
                                    PageAvatar = itemConnectPage.PageAvatar,
                                    PageCategory = itemConnectPage.PageCategory,
                                    UserAccessToken = itemConnectPage.UserAccessToken,
                                    PageAccessToken = itemConnectPage.PageAccessToken,
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
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.ClearContent();
                        filterContext.HttpContext.Items["AjaxCheckCondition"] = true;
                        filterContext.HttpContext.Items["AjaxCheckConditionCode"] = res.Code;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(new { controller = "Account", action = "LogOut", area = "" })
                        );
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}