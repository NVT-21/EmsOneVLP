using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using Newtonsoft.Json;
using SRVTextToImage;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EMS.MVC.Backend.Repository;

namespace EMS.MVC.Backend.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            if (Request.Cookies["be_panelIdCookie"] != null)
            {
                var panelIdCookie = new HttpCookie("be_panelIdCookie");
                panelIdCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(panelIdCookie);
            }
            Session[XHelper.SessionUser] = null;
            Session.Clear();
            LoginViewModel model = new LoginViewModel();
            if (Request.Cookies["BE_UserName"] != null && Request.Cookies["BE_Password"] != null)
            {
                model.UserName = Security.MD5Decrypt(Request.Cookies["BE_UserName"].Value, Security.PASSWORD_SALT);
                model.Password = Security.MD5Decrypt(Request.Cookies["BE_Password"].Value, Security.PASSWORD_SALT);
                model.RememberMe = true;
            }
            return PartialView(model);
        }

        public JsonResult JLogin(LoginViewModel req)
        {


            ZResponse res = new ZResponse();
            try
            {
                if (Request.Cookies["BE_UserName"] != null && Request.Cookies["BE_Password"] != null)
                {
                    var Password = Request.Cookies["BE_Password"].Value;
                    if (req.Password == Security.MD5Encrypt(Password, Security.PASSWORD_SALT))
                    {
                        req.Password = Security.MD5Decrypt(Password, Security.PASSWORD_SALT);
                    }
                }

                res = UserRepository.Login(new ZUserLoginRequest() 
                {
                    UserName = req.UserName.Trim(),
                    Password = req.Password.Trim()
                });

                if(res.Code == "00")
                {
                    if (req.RememberMe)
                    {
                        Response.Cookies["BE_UserName"].Expires = DateTime.Now.AddDays(7);
                        Response.Cookies["BE_Password"].Expires = DateTime.Now.AddDays(7);

                        Response.Cookies["BE_UserName"].Value = Security.MD5Encrypt(req.UserName, Security.PASSWORD_SALT);
                        Response.Cookies["BE_Password"].Value = Security.MD5Encrypt(req.Password, Security.PASSWORD_SALT);
                    }
                    else
                    {
                        Response.Cookies["BE_UserName"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["BE_Password"].Expires = DateTime.Now.AddDays(-1);
                        FormsAuthentication.SignOut();
                    }

                    FormsAuthentication.SetAuthCookie(req.UserName, req.RememberMe);

                    var item = JsonConvert.DeserializeObject<ZUserLoginResponse>(res.Data);
                    UserProfile user = new UserProfile() 
                    {
                        ID = item.ID,
                        GroupName = item.GroupName,
                        UserName = item.UserName,
                        FullName = item.FullName,
                        MobileNumber = item.MobileNumber,
                        Email = item.Email,
                        GroupCode = item.GroupCode
                    };
                    Session[XHelper.SessionUser] = user;

                    // 01/11/2022
                    Session.Timeout = 240;
                    FormsAuthentication.SetAuthCookie(req.UserName, false);
                    var panelIdCookie = new HttpCookie("be_panelIdCookie");
                    panelIdCookie.Values.Add("UserID", Security.MD5Encrypt(req.UserName, Security.PASSWORD_SALT));
                    panelIdCookie.Expires = DateTime.Now.AddDays(14);
                    Response.Cookies.Add(panelIdCookie);
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "LoginController.JLogin: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOut(string Message)
        {
            FormsAuthentication.SignOut();
            if (Request.Cookies["be_panelIdCookie"] != null)
            {
                var panelIdCookie = new HttpCookie("be_panelIdCookie");
                panelIdCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(panelIdCookie);
            }
            Session[XHelper.SessionUser] = null;
            Session.Clear();
            return RedirectToAction("Index", "Login", new { Message = Message });
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = int.MaxValue, VaryByParam = "None", Location = System.Web.UI.OutputCacheLocation.Client)]
        public FileResult CreateCaptchaImage(string CaptchaText)
        {
            CaptchaRandomImage CI = new CaptchaRandomImage();
            //Session["CaptchaImageText"] = CI.GetRandomString(5);
            //CI.GenerateImage(Session["CaptchaImageText"].ToString(), 300, 75, Color.DarkGray, Color.White);
            CI.GenerateImage(CaptchaText, 300, 75, Color.DarkGray, Color.White);
            MemoryStream stream = new MemoryStream();
            CI.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "image/png");
        }
        public JsonResult GetCaptchaText()
        {
            return Json(Session["CaptchaImageText"], JsonRequestBehavior.AllowGet);
        }
        public ActionResult CaptchaImage()
        {
            Response.RemoveOutputCacheItem(Url.Action("CreateCaptchaImage", "Login"));
            CaptchaRandomImage CI = new CaptchaRandomImage();
            var RandomString = CI.GetRandomString(5);
            ViewBag.CaptchaText = RandomString;
            return PartialView();
        }
    }
}