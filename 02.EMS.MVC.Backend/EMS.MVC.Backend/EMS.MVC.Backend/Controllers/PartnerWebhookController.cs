using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Controllers
{
    public class PartnerWebhookController : BaseController
    {
        // GET: PartnerWebhook
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListData()
        {
            List<PartnerWebhookViewModel> model = new List<PartnerWebhookViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PARTNER_WEBHOOK_REGISTER_LIST",
                Data = JsonConvert.SerializeObject(new ZPartnerWebhookListPartnerRegisterRequest()
                {
                    PartnerCode = ""
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZPartnerWebhookListPartnerRegisterResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new PartnerWebhookViewModel()
                    {
                        ID = item.ID,
                        PartnerName = item.PartnerName,
                        PartnerCode = item.PartnerCode,
                        PartnerURL = item.PartnerURL,
                        PartnerKey = item.PartnerKey,
                        Status = item.Status,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return View(model);
        }
        [AuthorizeController]
        public ActionResult Add()
        {
            //ViewBag.TT = "Thêm mới thông tin đối tác Webhook";
            return PartialView();
        }
        [HttpPost]
        public JsonResult JAdd(ZPartnerWebhookAddPartnerRegisterRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PARTNER_WEBHOOK_REGISTER_ADD",
                    Data = JsonConvert.SerializeObject(new ZPartnerWebhookAddPartnerRegisterRequest()
                    {
                        PartnerName = req.PartnerName,
                        PartnerCode = req.PartnerCode,
                        PartnerURL = req.PartnerURL,
                        PartnerKey = req.PartnerKey,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.JAdd: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeController]
        public ActionResult Edit(int ID)
        {
            //ViewBag.TT = "Chỉnh sửa thông tin đối tác Webhook";
            PartnerWebhookViewModel model = new PartnerWebhookViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PARTNER_WEBHOOK_REGISTER_BY_ID",
                Data = JsonConvert.SerializeObject(new ZPartnerWebhookGetPartnerRegisterByIDRequest()
                {
                    ID = ID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZPartnerWebhookListPartnerRegisterResponse>(res.Data);
                model = new PartnerWebhookViewModel()
                {
                    ID = item.ID,
                    PartnerName = item.PartnerName,
                    PartnerCode = item.PartnerCode,
                    PartnerURL = item.PartnerURL,
                    PartnerKey = item.PartnerKey,
                    Status = item.Status,
                    CreatedDate = item.CreatedDate
                };
            }
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult JEdit(ZPartnerWebhookEditPartnerRegisterRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PARTNER_WEBHOOK_REGISTER_EDIT",
                    Data = JsonConvert.SerializeObject(new ZPartnerWebhookEditPartnerRegisterRequest()
                    {
                        ID = req.ID,
                        PartnerName = req.PartnerName,
                        PartnerCode = req.PartnerCode,
                        PartnerURL = req.PartnerURL,
                        PartnerKey = req.PartnerKey,
                        Status = req.Status,
                        UpdatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.JEdit: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult JDelete(ZPartnerWebhookDeletePartnerRegisterRequest model)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PARTNER_WEBHOOK_REGISTER_DELETE",
                    Data = JsonConvert.SerializeObject(new ZPartnerWebhookDeletePartnerRegisterRequest()
                    {
                        ID = model.ID,
                        UpdatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.JDelete: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}