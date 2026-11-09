using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using EMS.MVC.Frontend.Repositories;
using System.Configuration;
using Facebook;
using System.Text;
using System.Security.Cryptography;


namespace EMS.MVC.Frontend.Controllers
{
    public class ChannelController : BaseController
    {
        // GET: Channel
        public ActionResult ChannelView()
        {
            ChannelViewModel model = new ChannelViewModel();
            model.ZaloURL = XHelper.ZaloConnectURL;
            model.FacebookURL = XHelper.FacebookConnectURL;
            model.TiktokURL = XHelper.TiktokServiceURL;
            return PartialView(model);
        }

        public ActionResult SearchListChannelView(ChannelViewModel req)
        {
            List<SearchListChannelViewModel> model = new List<SearchListChannelViewModel>();
            var res = ChannelRepository.SearchListChannel(new ZConnectPageSearchListRequest()
            {
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var ListItem = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(res.Data);
                foreach (var Item in ListItem)
                {
                    model.Add(new SearchListChannelViewModel()
                    {
                        ID = Item.ID,
                        PageID = Item.PageID,
                        PageName = Item.PageName,
                        PageCategory = Item.PageCategory,
                        PageDescription = Item.PageDescription,
                        PageAvatar = Item.PageAvatar,
                        PageCover = Item.PageCover,
                        IsActive = Item.IsActive,
                        Channel = Item.Channel,
                        CreatedName = Item.CreatedName,
                        CreatedDate = Item.CreatedDate,
                        IsActiveCheck = Item.IsActiveCheck,
                        AccountID = Item.AccountID
                    });
                }
            }
            return PartialView(model);
        }

        #region Zalo
        public ActionResult ConnectZalo()
        {
            ZResponse res = new ZResponse();
            try
            {
                string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                Uri myUri = new Uri(url);
                string code = HttpUtility.ParseQueryString(myUri.Query).Get("code");
                //string oa_id = HttpUtility.ParseQueryString(myUri.Query).Get("oa_id");
                if (!string.IsNullOrEmpty(code))
                {
                    res = XHelper.InvokeZaloV2(new ZRequest()
                    {
                        Code = "https://oauth.zaloapp.com/v4/oa/access_token",
                        Data = string.Format("code={0}&app_id={1}&grant_type={2}&code_verifier={3}", code, long.Parse(XHelper.ZaloAppID), "authorization_code", string.Empty)
                    });
                    if (res.Code == "00")
                    {
                        var ItemZaloConnect = JsonConvert.DeserializeObject<ZZaloConnectResponse>(res.Data);
                        res = XHelper.InvokeZaloV1(new ZRequest()
                        {
                            Code = "https://openapi.zalo.me/v2.0/oa/getoa",
                            Data = ""
                        }, "GET", ItemZaloConnect.access_token);
                        if (res.Code == "00")
                        {
                            var ItemGetInfoOA = JsonConvert.DeserializeObject<ZZaloGetInfoOAResponse>(res.Data);
                            res = ChannelRepository.AddChannel(new ZConnectPageAddRequest()
                            {
                                PageID = ItemGetInfoOA.data.oa_id,
                                PageName = ItemGetInfoOA.data.name,
                                PageAvatar = ItemGetInfoOA.data.avatar,
                                PageCover = ItemGetInfoOA.data.cover,
                                PageCategory = "",
                                PageDescription = ItemGetInfoOA.data.description,
                                IsVerified = ItemGetInfoOA.data.is_verified == true ? "Y" : "N",
                                AuthorizationCode = code,
                                AccessToken = ItemZaloConnect.access_token,
                                RefreshToken = ItemZaloConnect.refresh_token,
                                Channel = "ZA",
                                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                                CreatedBy = XHelper.Session.EmployeeProfile.ID
                            });
                        }
                        else
                        {
                            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("ChannelController.ConnectZalo: Lỗi lấy thông tin của trang || res={0}", JsonConvert.SerializeObject(res)));
                            res.Code = "97";
                            res.Message = "Lỗi lấy thông tin của trang.";
                        }
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("ChannelController.ConnectZalo: Lỗi lấy access_token từ AuthorizationCode || res={0}", JsonConvert.SerializeObject(res)));
                        res.Code = "97";
                        res.Message = "Lỗi lấy AccessToken.";
                    }
                }
                else
                {
                    res.Code = "98";
                    res.Message = "Lỗi kết nối đến ZaloConnectURL.";
                    XHelper.LogToFile(LogFileType.MESSAGE, "ChannelController.ConnectZalo: Lỗi kết nối đến ZaloConnectURL: " + XHelper.ZaloConnectURL);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ChannelController.ConnectZalo: " + ex.Message);
            }
            return RedirectToAction("ShopView", "Shop", new { Message = res.Message });
        }

        public JsonResult JRedirectZalo(SearchListChannelViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ChannelRepository.SearchListChannel(new ZConnectPageSearchListRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    PageID = req.PageID,
                    Channel = "ZA",
                    IsActive = "Y"
                });
                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(res.Data).First();

                    Session[XHelper.SessionZalo] = new ZaloModel()
                    {
                        PageID = item.PageID,
                        PageName = item.PageName,
                        PageAvatar = item.PageAvatar,
                        PageCover = item.PageCover,
                        PageDescription = item.PageDescription,
                        AccessToken = item.AccessToken,
                        RefreshToken = item.RefreshToken
                    };

                    HttpCookie authCookie = Request.Cookies["panelIdCookie"];
                    var isChannelID = authCookie.Values["ChannelID"];
                    if (isChannelID == null)
                    {
                        authCookie.Values.Add("ChannelID", Security.MD5Encrypt(item.PageID, Security.PASSWORD_SALT));
                    }
                    else
                    {
                        authCookie.Values["ChannelID"] = Security.MD5Encrypt(item.PageID, Security.PASSWORD_SALT);
                    }
                    Response.Cookies.Add(authCookie);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ChannelController.JRedirectZalo: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Facebook
        public ActionResult AddConnectPage(string code)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resConnect = FacebookRepository.ConnectPage(code);
                if (resConnect.Code == "00")
                {
                    var ItemConnect = JsonConvert.DeserializeObject<ZFacebookConnectResponse>(resConnect.Data);
                    var resAccountProfile = FacebookRepository.GetAccountProfile(new ZFacebookGetProfileRequest() { AccessToken = ItemConnect.AccessToken });
                    if (resAccountProfile.Code == "00")
                    {
                        var ItemAccountProfile = JsonConvert.DeserializeObject<ZFacebookGetProfileResponse>(resAccountProfile.Data);
                        var resPage = FacebookRepository.GetPageInfo(new ZFacebookGetProfileRequest() { AccessToken = ItemConnect.AccessToken });
                        if (resPage.Code == "00")
                        {
                            var ItemPage = JsonConvert.DeserializeObject<ZFacebookConnectPageResponse>(resPage.Data);
                            var resPageReset = ChannelRepository.ChannelReset(new ZConnectPageResetRequest()
                            {
                                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                                Channel = "FB",
                                AccountID = ItemAccountProfile.FacebookAccountID,
                                UpdatedBy = XHelper.Session.EmployeeProfile.ID
                            });
                            if (resPageReset.Code == "00")
                            {
                                if (ItemPage.Data.Count > 0)
                                {
                                    var checkAddConnect = 0;
                                    foreach (var item in ItemPage.Data)
                                    {
                                        var resSubcribed = FacebookRepository.Subcribed(new ZFacebookSubcribedRequest()
                                        {
                                            PageID = item.PageID,
                                            AccessToken = item.AccessToken
                                        });
                                        if (resSubcribed.Code == "00")
                                        {
                                            var ItemSubcribed = JsonConvert.DeserializeObject<ZFacebookSubcribedResponse>(resSubcribed.Data);
                                            if (ItemSubcribed.Success)
                                            {
                                                var resAddConect = ChannelRepository.AddChannel(new ZConnectPageAddRequest()
                                                {
                                                    AccountID = ItemAccountProfile.FacebookAccountID,
                                                    PageID = item.PageID,
                                                    PageName = item.PageName,
                                                    PageAvatar = "https://emsone.com.vn/assets/images/logo/logo_ems_app.jpg",
                                                    PageCover = "",
                                                    PageCategory = item.Category,
                                                    PageDescription = "",
                                                    IsVerified = "",
                                                    AuthorizationCode = code,
                                                    AccessToken = "",
                                                    RefreshToken = "",
                                                    UserAccessToken = ItemConnect.AccessToken,
                                                    PageAccessToken = item.AccessToken,
                                                    Channel = "FB",
                                                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                                                    CreatedBy = XHelper.Session.EmployeeProfile.ID
                                                });
                                                if (resAddConect.Code == "00")
                                                {
                                                    checkAddConnect++;
                                                }
                                            }
                                            else
                                            {
                                                return RedirectToAction("ShopView", "Shop", new { Code = "FB_Subcribed", Message = "Lỗi đăng ký App." });
                                            }
                                        }
                                        else
                                        {
                                            return RedirectToAction("ShopView", "Shop", new { Code = "FB_Subcribed", Message = "Lỗi đăng ký App." });
                                        }
                                    }
                                    if (checkAddConnect == ItemPage.Data.Count)
                                    {
                                        res.Code = "00";
                                        res.Message = "Thành công";
                                    }
                                    else
                                    {
                                        res.Code = "01";
                                        res.Message = "Thêm kết nối thành công " + checkAddConnect + "/" + ItemPage.Data.Count;
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("ShopView", "Shop", new { Code = "FB_Add", Message = "Chưa chọn trang để liên kết." });
                                }
                            }
                            else
                            {
                                res.Code = resPageReset.Code;
                                res.Message = resPageReset.Message;
                                XHelper.LogToFile(LogFileType.MESSAGE, "ChannelController.AddConnectPage: resPageReset||" + JsonConvert.SerializeObject(res));
                            }
                        }
                        else
                        {
                            res.Code = resPage.Code;
                            res.Message = resPage.Message;
                            XHelper.LogToFile(LogFileType.MESSAGE, "ChannelController.AddConnectPage: resPage||" + JsonConvert.SerializeObject(res));
                        }
                    }
                    else
                    {
                        res.Code = resAccountProfile.Code;
                        res.Message = resAccountProfile.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, "ChannelController.AddConnectPage: resAccountProfile||" + JsonConvert.SerializeObject(res));
                    }
                }
                else
                {
                    res.Code = resConnect.Code;
                    res.Message = resConnect.Message;
                    XHelper.LogToFile(LogFileType.MESSAGE, "ChannelController.AddConnectPage: resConnect||" + JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ChannelController.AddConnectPage: " + ex.Message);
            }
            return RedirectToAction("ShopView", "Shop", new { Message = res.Message });
        }

        public JsonResult JRedirectFacebook(SearchListChannelViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ChannelRepository.SearchListChannel(new ZConnectPageSearchListRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    PageID = req.PageID,
                    Channel = "FB",
                    IsActive = "Y"
                });
                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(res.Data).First();
                    Session[XHelper.SessionFacebook] = new FacebookModel()
                    {
                        PageID = item.PageID,
                        PageName = item.PageName,
                        PageAvatar = item.PageAvatar,
                        PageCover = item.PageCover,
                        PageCategory = item.PageCategory,
                        UserAccessToken = item.UserAccessToken,
                        PageAccessToken = item.PageAccessToken,
                    };

                    HttpCookie authCookie = Request.Cookies["panelIdCookie"];
                    var isChannelID = authCookie.Values["ChannelID"];
                    if(isChannelID == null)
                    {
                        authCookie.Values.Add("ChannelID", Security.MD5Encrypt(item.PageID, Security.PASSWORD_SALT));
                    }
                    else
                    {
                        authCookie.Values["ChannelID"] = Security.MD5Encrypt(item.PageID, Security.PASSWORD_SALT);
                    }
                    Response.Cookies.Add(authCookie);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ChannelController.JRedirectFacebook: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult JDeleteConnectPage(ZFacebookDeleteConnectRequest model)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CONNECT_PAGE_DELETE",
                    Data = JsonConvert.SerializeObject(new ZFacebookDeleteConnectRequest()
                    {
                        PageID = model.PageID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        Channel = model.Channel,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ChannelController.JDeleteConnectPage: " + ex.Message);
            }
            jResult = Json(res, JsonRequestBehavior.AllowGet);
            return jResult;
        }
        #endregion

        #region Tiktok
        public ActionResult ConnectTiktok(string code, string state)
        {
            ZResponse res = new ZResponse();
            try
            {
                string apiString = "/api/v2/token/get";
                var resConnect = XHelper.InvokeTiktok(new ZRequest()
                {
                    Code =  apiString + "?app_key=" + XHelper.TiktokAppKey 
                                      + "&app_secret=" + XHelper.TiktokAppSecret 
                                      + "&auth_code=" + code 
                                      + "&grant_type=authorized_code",
                    Data = ""
                }, "GET", "Verify_Permissions", "");
                var ItemConnect = JsonConvert.DeserializeObject<ZTiktokShopAccessTokenResponse>(resConnect.Data);
                if (ItemConnect.Code == 0 && ItemConnect.Message == "success")
                {
                    var timestamp = TiktokRepository.GetTimeStamp();
                    string apiStringShop = "/authorization/202309/shops";
                    Dictionary<string, string> queriesShop = new Dictionary<string, string>()
                    {
                        { "app_key", XHelper.TiktokAppKey },
                        { "timestamp", timestamp}
                    };
                    string SignatureShop = XHelper.TiktokSignature(apiStringShop, queriesShop, XHelper.TiktokAppSecret);
                    var resShop = XHelper.InvokeTiktok(new ZRequest()
                    {
                        Code = apiStringShop + "?app_key=" + XHelper.TiktokAppKey
                                             + "&access_token=" + ItemConnect.Data.AccessToken
                                             + "&sign=" + SignatureShop 
                                             + "&timestamp=" + timestamp
                    }, "GET", "", ItemConnect.Data.AccessToken);
                    var ItemShop = JsonConvert.DeserializeObject<ZTiktokGetShopResponse>(resShop.Data);
                    if (ItemShop.Code == 0 && ItemShop.Message == "Success")
                    {
                        if (ItemShop.Data.Shops.Count > 0)
                        {
                            string apiStringShopActive = "/seller/202309/shops";
                            Dictionary<string, string> queriesShopActive = new Dictionary<string, string>()
                            {
                                { "app_key", XHelper.TiktokAppKey },
                                { "timestamp", timestamp}
                            };
                            string SignatureShopActive = XHelper.TiktokSignature(apiStringShopActive, queriesShopActive, XHelper.TiktokAppSecret);
                            var resShopActive = XHelper.InvokeTiktok(new ZRequest()
                            {
                                Code = apiStringShopActive + "?app_key=" + XHelper.TiktokAppKey
                                                           + "&access_token=" + ItemConnect.Data.AccessToken
                                                           + "&sign=" + SignatureShopActive 
                                                           + "&timestamp=" + timestamp
                            }, "GET", "", ItemConnect.Data.AccessToken);
                            var ItemShopActive = JsonConvert.DeserializeObject<ZTiktokGetShopResponse>(resShopActive.Data);
                            List<string> Distinct = new List<string>();
                            ZTiktokGetShopListDataResponse DistinctAccept = new ZTiktokGetShopListDataResponse();
                            string Shop_ID;
                            for (int i = 0; i < ItemShopActive.Data.Shops.Count(); i++)
                            {
                                Distinct.Add(
                                    Shop_ID = ItemShopActive.Data.Shops[i].ID.ToString()
                                );
                            }
                            for (int j = 0; j < ItemShop.Data.Shops.Count(); j++)
                            {
                                if (Distinct.Contains(ItemShop.Data.Shops[j].ID))
                                {
                                    DistinctAccept = new ZTiktokGetShopListDataResponse()
                                    {
                                        ID = ItemShop.Data.Shops[j].ID,
                                        Code = ItemShop.Data.Shops[j].Code,
                                        Name = ItemShop.Data.Shops[j].Name,
                                        Region = ItemShop.Data.Shops[j].Region,
                                        SellerType = ItemShop.Data.Shops[j].SellerType,
                                        Cipher = ItemShop.Data.Shops[j].Cipher
                                    };
                                }
                            }
                            if (DistinctAccept.ID != null)
                            {
                                var resAddConect = ChannelRepository.AddChannel(new ZConnectPageAddRequest()
                                {
                                    AccountID = ItemConnect.Data.OpenID,
                                    PageID = DistinctAccept.ID,
                                    PageName = DistinctAccept.Name,
                                    PageAvatar = "https://emsone.com.vn/assets/images/logo/logo_ems_app.jpg",
                                    PageCover = "",
                                    PageCategory = "",
                                    PageDescription = DistinctAccept.SellerType == "CROSS_BORDER" ? "Toàn thế giới" : "Trong nước",
                                    IsVerified = "",
                                    AuthorizationCode = code,
                                    AccessToken = ItemConnect.Data.AccessToken,
                                    RefreshToken = ItemConnect.Data.RefreshToken,
                                    Channel = "TIK",
                                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                                    PageCipher = DistinctAccept.Cipher
                                });
                                if (resAddConect.Code == "00")
                                {
                                    res.Code = "00";
                                    res.Message = "Thành công";
                                }
                                else
                                {
                                    res.Code = "01";
                                    res.Message = "Thất bại";
                                }
                            }
                            else
                            {
                                res.Code = "99";
                                res.Message = "Không tìm thấy thông tin Shop.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Không tìm thấy thông tin Shop.";
                        }
                    }
                    else
                    {
                        res.Code = ItemShop.Code.ToString();
                        res.Message = ItemShop.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, "ChannelController.ConnectTiktok: ItemShop ||" + JsonConvert.SerializeObject(ItemShop));
                    }
                }
                else
                {
                    res.Code = ItemConnect.Code.ToString();
                    res.Message = ItemConnect.Message;
                    XHelper.LogToFile(LogFileType.MESSAGE, "ChannelController.ConnectTiktok: ItemConnect ||" + JsonConvert.SerializeObject(ItemConnect));
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ChannelController.ConnectTiktok: " + ex.Message);
            }
            return RedirectToAction("ShopView", "Shop", new { Message = res.Message });
        }
        public JsonResult JRedirectTiktok(SearchListChannelViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ChannelRepository.SearchListChannel(new ZConnectPageSearchListRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    PageID = req.PageID,
                    Channel = "TIK",
                    IsActive = "Y"
                });
                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(res.Data).First();
                    Session[XHelper.SessionTiktok] = new TiktokModel()
                    {
                        PageID = item.PageID,
                        PageName = item.PageName,
                        PageAvatar = item.PageAvatar,
                        PageCategory = item.PageCategory,
                        UserAccessToken = item.UserAccessToken,
                        PageAccessToken = item.PageAccessToken,
                    };

                    HttpCookie authCookie = Request.Cookies["panelIdCookie"];
                    var isChannelID = authCookie.Values["ChannelID"];
                    if (isChannelID == null)
                    {
                        authCookie.Values.Add("ChannelID", Security.MD5Encrypt(item.PageID, Security.PASSWORD_SALT));
                    }
                    else
                    {
                        authCookie.Values["ChannelID"] = Security.MD5Encrypt(item.PageID, Security.PASSWORD_SALT);
                    }
                    Response.Cookies.Add(authCookie);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ChannelController.JRedirectTiktok: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}