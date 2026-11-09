using System.Collections.Generic;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using EMS.Entity;
using EMS.MVC.Frontend.Repositories;
using EMS.Library;
using System;
using Newtonsoft.Json;
using System.Web.Security;
using System.Web;
using System.Globalization;

namespace EMS.MVC.Frontend.Controllers
{
    public class AccountController : Controller
    {
        #region Đăng nhập
        public ActionResult LoginView(LoginViewModel model)
        {
            FormsAuthentication.SignOut();
            if (Request.Cookies["panelIdCookie"] != null)
            {
                var panelIdCookie = new HttpCookie("panelIdCookie");
                panelIdCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(panelIdCookie);
            }
            Session[XHelper.SessionEmployee] = null;
            Session.Clear();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult JLogin(LoginViewModel model)
        {
            ZResponse res = new ZResponse();
            try
            {
                EmployeeModel Employee = null;
                res = AccountRepository.Login(new ZEmployeeLoginRequest()
                {
                    MobileNumber = model.MobileNumber,
                    Password = model.Password,
                    //Password = Security.SHA256(model.MobileNumber + model.Password + Security.PASSWORD_SALT),
                });
                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<ZEmployeeLoginResponse>(res.Data);
                    Employee = new EmployeeModel()
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
                    //HttpContext.Session.Add(XHelper.SessionEmployee, Employee);
                    //System.Web.HttpContext.Current.Session[XHelper.SessionEmployee] = Employee;
                    //HttpContext.Session[XHelper.SessionEmployee] = Employee;
                    Session[XHelper.SessionEmployee] = Employee;


                    // 01/11/2022
                    Session.Timeout = 240;
                    FormsAuthentication.SetAuthCookie(model.MobileNumber, false);
                    var panelIdCookie = new HttpCookie("panelIdCookie");
                    panelIdCookie.Values.Add("UserID", Security.MD5Encrypt(model.MobileNumber, Security.PASSWORD_SALT));
                    panelIdCookie.Expires = DateTime.Now.AddDays(14);
                    Response.Cookies.Add(panelIdCookie);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AccountController.JLogin: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult RedirectHomeView(string req)
        //{
        //    var item = JsonConvert.DeserializeObject<ZEmployeeLoginResponse>(req);
        //    System.Web.HttpContext.Current.Session[XHelper.SessionEmployee] = new EmployeeModel() 
        //    {
        //        ID = item.ID,
        //        StoreOwner = item.StoreOwner,
        //        UserName = item.UserName,
        //        FullName = item.FullName,
        //        MobileNumber = item.MobileNumber,
        //        Email = item.Email,
        //        IsLock = item.IsLock,
        //        EmployeeLevel = item.EmployeeLevel,
        //        GroupPermissionID = item.GroupPermissionID,
        //        ReadyReceiveRequestOrder = item.ReadyReceiveRequestOrder,
        //        ShopInformation = item.ShopInformation
        //    };
        //    return RedirectToAction("Index", "Home", new { area = "" });
        //}
        #endregion

        #region Đăng ký
        public ActionResult RegisterView()
        {
            RegisterViewModel model = new RegisterViewModel() 
            {
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                ListDistrict = new List<DistrictModel>(),
                ListWard = new List<WardModel>(),
                ListCollectionPostOffice = XCommon.SearchListCollectionPostOffice(new ZCollectionPostOfficeSearchListRequest() { IsLock = "N" }),
            };
            return PartialView(model);
        }

        public JsonResult JRegister(RegisterViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = AccountRepository.Register(new ZShopRegisterRequest()
                {
                    FullName = req.FullName,
                    Email = req.Email,
                    MobileNumber = req.MobileNumber,
                    ProductBusiness = req.ProductBusiness,
                    StoreName = req.StoreName,
                    TypeBusiness = req.TypeBusiness,
                    TypeShop = req.TypeShop,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Address = req.Address,
                    CRMOrPaypostCode = req.CRMOrPaypostCode,
                    CollectionPostOfficeID = req.CollectionPostOfficeID,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AccountController.JRegister: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterSuccessView()
        {
            return PartialView();
        }
        #endregion

        #region Đăng xuất
        public ActionResult LogOut(string Message)
        {
            FormsAuthentication.SignOut();
            if (Request.Cookies["panelIdCookie"] != null)
            {
                var panelIdCookie = new HttpCookie("panelIdCookie");
                panelIdCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(panelIdCookie);
            }

            Session[XHelper.SessionEmployee] = null;
            Session.Clear();
            return RedirectToAction("LoginView", "Account", new { Message = Message });
        }
        #endregion

        #region Đổi mật khẩu
        public ActionResult ChangePasswordView()
        {
            return PartialView();
        }

        public JsonResult JChangePassword(ChangePasswordViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = AccountRepository.ChangePassword(new ZEmployeeChangePasswordRequest()
                {
                    ID = XHelper.Session.EmployeeProfile.ID,
                    UserName = XHelper.Session.EmployeeProfile.MobileNumber,
                    OldPassword = req.OldPassword,
                    NewPassword = req.NewPassword
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AccountController.JChangePassword: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Quên mật khẩu
        public ActionResult ForgetPasswordView()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult JForgetPassword(ForgetPasswordViewModel req, string captchaToken)
        {
            ZResponse res = new ZResponse();
            try
            {
                string captchaMessage;
                if (!CaptchaHelper.Verify(captchaToken, Request.UserHostAddress, out captchaMessage))
                {
                    res.Code = "01";
                    res.Message = captchaMessage;
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                res = AccountRepository.ForgetPasswordByEmail(new ZShopForgetPasswordByEmailRequest()
                {
                    Email = req.Email
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AccountController.JForgetPassword: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ForgetPasswordSuccessView(ForgetPasswordViewModel req)
        {
            ForgetPasswordSuccessViewModel model = new ForgetPasswordSuccessViewModel() 
            {
                Email = req.Email,
                OtpID = req.OtpID
            };
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult JVerifyOtp(ForgetPasswordSuccessViewModel req, string captchaToken)
        {
            ZResponse res = new ZResponse();
            try
            {
                string captchaMessage;
                if (!CaptchaHelper.Verify(captchaToken, Request.UserHostAddress, out captchaMessage))
                {
                    res.Code = "01";
                    res.Message = captchaMessage;
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                res = OtpRepository.VerifyCode(new ZOtpVerifyCodeRequest()
                {
                    OtpID = req.OtpID,
                    OtpCode = req.OtpCode,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AccountController.JVerifyOtp: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateForgetPasswordView(ForgetPasswordSuccessViewModel req)
        {
            UpdateForgetPasswordViewModel model = new UpdateForgetPasswordViewModel() 
            {
                EmployeeID = req.EmployeeID,
                MobileNumber = req.MobileNumber,
                OtpCode = req.OtpCode
            };
            return PartialView(model);
        }

        public JsonResult JUpdateForgetPassword(UpdateForgetPasswordViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = AccountRepository.ForgetPasswordUpdate(new ZShopForgetPasswordUpdateRequest() 
                {
                    EmployeeID = req.EmployeeID,
                    MobileNumber = req.MobileNumber,
                    Password = req.NewPassword,
                    OtpCode = req.OtpCode
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AccountController.JUpdateForgetPassword: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Đổi mật khẩu khi nhấn vào link truy cập từ email
        public ActionResult UpdatePasswordView(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var DecryptData = Security.MD5Decrypt(data.Replace(" ", "+"), Security.PASSWORD_SALT);
                var req = JsonConvert.DeserializeObject<UpdatePasswordViewModel>(DecryptData);
                var res = AccountRepository.Login(new ZEmployeeLoginRequest()
                {
                    MobileNumber = req.MobileNumber,
                    Password = req.Password,
                });
                if (res.Code == "00")
                {
                    var Item = JsonConvert.DeserializeObject<ZUserProfile>(res.Data);
                    UpdatePasswordViewModel model = new UpdatePasswordViewModel()
                    {
                        ID = Item.ID,
                        MobileNumber = req.MobileNumber,
                        Password = req.Password
                    };
                    return PartialView(model);
                }
                else
                {
                    return PartialView("~/Views/Error/StatusCode203.cshtml", new StatusCode203ViewModel() { Message = "Mật khẩu đã được thay đổi. Nếu không phải bạn thay đổi, vui lòng liên hệ quản trị." });
                }
            }
            return PartialView("~/Views/Error/StatusCode203.cshtml", new StatusCode203ViewModel() { Message = "Dữ liệu không hợp lệ, vui lòng liên hệ quản trị." });
        }

        public JsonResult JUpdatePassword(UpdatePasswordViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = AccountRepository.ChangePassword(new ZEmployeeChangePasswordRequest()
                {
                    ID = req.ID,
                    UserName = req.MobileNumber,
                    OldPassword = Security.DecodeBase64(req.Password),
                    NewPassword = req.NewPassword
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AccountController.JUpdatePassword: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Khóa tài khoản
 
        public JsonResult JLockAccount()
        {
            ZResponse res = new ZResponse();
            try
            {
                res = AccountRepository.ShopLock(new ZShopLockRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AccountController.JChangePassword: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa tài khoản

        public JsonResult JRemoveAccount()
        {
            ZResponse res = new ZResponse();
            try
            {
                res = AccountRepository.ShopDelete(new ZShopRemoveRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AccountController.JChangePassword: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}