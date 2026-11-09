using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;

namespace EMS.MVC.Frontend.Controllers
{
    public class PartnerWebhookController : BaseController
    {
        // GET: PartnerWebhook
        public ActionResult PartnerWebhookView()
        {
            return PartialView();
        }
        public ActionResult SearchListPartnerWebhookView()
        {
            List<SearchListPartnerWebhookViewModel> model = new List<SearchListPartnerWebhookViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PARTNER_WEBHOOK_REGISTER_LIST",
                Data = JsonConvert.SerializeObject(new ZPartnerWebhookListPartnerRegisterRequest()
                {
                    PartnerCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode
                })
            });
            if (res.Code == "00")
            {
                var ListItem = JsonConvert.DeserializeObject<List<ZPartnerWebhookListPartnerRegisterResponse>>(res.Data);
                foreach (var Item in ListItem)
                {
                    model.Add(new SearchListPartnerWebhookViewModel()
                    {
                        ID = XHelper.Encrypt(Item.ID.ToString()),
                        PartnerCode = Item.PartnerCode,
                        PartnerName = Item.PartnerName,
                        PartnerURL = Item.PartnerURL,
                        PartnerKey = Item.PartnerKey,
                        Status = Item.Status,
                        CreatedDate = Item.CreatedDate
                    });
                }
            }
            return PartialView(model);
        }
        [AuthorizeController]
        public ActionResult AddPartnerWebhookView()
        {
            PartnerWebhookAddViewModel model = new PartnerWebhookAddViewModel() {
                PartnerName = XHelper.Session.EmployeeProfile.ShopInformation.StoreName
            };
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult JAdd(PartnerWebhookAddPartnerRegisterModel req)
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
                        PartnerCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                        PartnerURL = req.PartnerURL,
                        PartnerKey = req.PartnerKey,
                        CreatedBy = XHelper.Session.EmployeeProfile.ID
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
        public ActionResult EditPartnerWebhookView(string ID)
        {
            PartnerWebhookEditViewModel model = new PartnerWebhookEditViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PARTNER_WEBHOOK_REGISTER_BY_ID",
                Data = JsonConvert.SerializeObject(new ZPartnerWebhookGetPartnerRegisterByIDRequest()
                {
                    ID = Convert.ToInt32(XHelper.Decrypt(ID))
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZPartnerWebhookListPartnerRegisterResponse>(res.Data);
                model = new PartnerWebhookEditViewModel()
                {
                    ID = XHelper.Encrypt(item.ID.ToString()),
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
        public JsonResult JEdit(PartnerWebhookEditPartnerRegisterModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PARTNER_WEBHOOK_REGISTER_EDIT",
                    Data = JsonConvert.SerializeObject(new ZPartnerWebhookEditPartnerRegisterRequest()
                    {
                        ID = Convert.ToInt32(XHelper.Decrypt(req.ID)),
                        PartnerName = req.PartnerName,
                        PartnerCode = req.PartnerCode,
                        PartnerURL = req.PartnerURL,
                        PartnerKey = req.PartnerKey,
                        Status = req.Status,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
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
        public JsonResult JDelete(PartnerWebhookDeletePartnerRegisterModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PARTNER_WEBHOOK_REGISTER_DELETE",
                    Data = JsonConvert.SerializeObject(new ZPartnerWebhookDeletePartnerRegisterRequest()
                    {
                        ID = Convert.ToInt32(XHelper.Decrypt(req.ID)),
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
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