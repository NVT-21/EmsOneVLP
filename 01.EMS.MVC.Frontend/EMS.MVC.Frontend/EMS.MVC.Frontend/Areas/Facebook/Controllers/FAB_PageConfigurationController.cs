using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Areas.Facebook.Models;
using EMS.MVC.Frontend.Controllers;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Facebook.Controllers
{
    public class FAB_PageConfigurationController : FAB_BaseController
    {
        // GET: Facebook/FAB_PageConfiguration
        public ActionResult FAB_PageConfigurationView()
        {
            var datePlus = DateTime.Now.AddDays(1);
            ViewBag.ListGroupCustomer = FacebookRepository.GetListGroupCustomer();
            ViewBag.ListSelSendTime = XData.GetListSelSendTime();
            ViewBag.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.ToDatePlus = datePlus.ToString("dd/MM/yyyy");
            return View();
        }

        #region AutoLike
        public ActionResult FAB_AutoLike()
        {
            var itemLike = FacebookRepository.PageAutoLike();
            ViewBag.idLike = itemLike.ID;
            ViewBag.IsLikeComment = itemLike.IsLikeComment;
            return PartialView();
        }
        [HttpPost]
        public JsonResult FAB_JChangeAutoLike(ZFacebookReactionPageAutoLikeRequest model)
        {
            var res = FacebookRepository.ChangeAutoLike(model);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AutoHidden
        public ActionResult FAB_AutoHidden()
        {
            var itemHidden = FacebookRepository.PageAutoHidden();
            ViewBag.itemHidden = itemHidden;
            ViewBag.ListAvailableDate = XData.GetListSelAvailableDate();
            return PartialView();
        }
        public JsonResult FAB_JUpdateAutoHidden(ZFacebookEditPageAutoHiddenRequest model)
        {
            var res = FacebookRepository.UpdateAutoHidden(model);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ConversationStickers
        public ActionResult FAB_ConversationStickers()
        {
            return PartialView();
        }
        public ActionResult FAB_SearchListConversationStickerView(FAB_ConversationStickerModel req)
        {
            List<FAB_SearchListConversationStickerViewModel> model = 
                FacebookRepository.ListConversationSticker(
                    new ZConversationStickerSearchListRequest()
                    {
                        SearchKey = req.SearchKey
                    }
                );
            return PartialView(model);
        }
        [AuthorizeController]
        public ActionResult FAB_AddConversationStickerView()
        {
            FAB_AddConversationStickerModel model = new FAB_AddConversationStickerModel()
            {
                StickerColor = "#FFCC00"
            };
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult FAB_JAddConversationSticker(FAB_AddConversationStickerModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CONVERSATION_STICKER_ADD",
                    Data = JsonConvert.SerializeObject(new ZConversationStickerAddRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        PageID = XHelper.Session.FacebookProfile.PageID,
                        Channel = "FB",
                        StickerName = req.StickerName,
                        StickerColor = req.StickerColor,
                        CreatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FAB_PageConfigurationController.FAB_JAddConversationSticker: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeController]
        public ActionResult FAB_EditConversationStickerView(FAB_EditConversationStickerModel req)
        {
            FAB_EditConversationStickerViewModel model = new FAB_EditConversationStickerViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "CONVERSATION_STICKER_BY_ID",
                Data = JsonConvert.SerializeObject(new ZConversationStickerByIDRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    PageID = XHelper.Session.FacebookProfile.PageID,
                    Channel = "FB",
                    StickerID = req.StickerID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZConversationStickerByIDResponse>(res.Data);
                model = new FAB_EditConversationStickerViewModel()
                {
                    StickerID = item.StickerID,
                    StickerName = item.StickerName,
                    StickerColor = item.StickerColor
                };
            }
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult FAB_JEditConversationSticker(FAB_EditConversationStickerModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CONVERSATION_STICKER_EDIT",
                    Data = JsonConvert.SerializeObject(new ZConversationStickerEditRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        PageID = XHelper.Session.FacebookProfile.PageID,
                        Channel = "FB",
                        StickerID = req.StickerID,
                        StickerName =  req.StickerName,
                        StickerColor = req.StickerColor,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FAB_PageConfigurationController.FAB_JEditConversationSticker: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FAB_JDeleteConversationSticker(FAB_DeleteConversationStickerModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CONVERSATION_STICKER_DELETE",
                    Data = JsonConvert.SerializeObject(new ZConversationStickerRemoveRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        PageID = XHelper.Session.FacebookProfile.PageID,
                        Channel = "FB",
                        StickerID = req.StickerID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FAB_PageConfigurationController.FAB_JDeleteConversationSticker: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}