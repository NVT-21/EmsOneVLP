using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Areas.Zalo.Models;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using EMS.MVC.Frontend.Repositories;
namespace EMS.MVC.Frontend.Areas.Zalo.Controllers
{
    public class ZAL_LiveChatController : ZAL_BaseController
    {
        public ActionResult ZAL_LiveChatView()
        {
            ZAL_LiveChatViewModel model = new ZAL_LiveChatViewModel()
            {
                PageID = XHelper.Session.ZaloProfile.PageID,
                PageName = XHelper.Session.ZaloProfile.PageName,
                PageAvatar = XHelper.Session.ZaloProfile.PageAvatar
            };
            return View(model);
        }

        public ActionResult ZAL_SearchListConversionView()
        {
            List<ZAL_SearchListConversionViewModel> model = new List<ZAL_SearchListConversionViewModel>();
            var res = ConversionRepository.SearchListConversion(new ZConversionSearchListRequest()
            {
                PageID = XHelper.Session.ZaloProfile.PageID,
                Channel = "ZA",
                LimitRow = 10
            });
            if (res.Code == "00")
            {
                var index = 1;
                var list = JsonConvert.DeserializeObject<List<ZConversionSearchListResponse>>(res.Data);
                var ListConversionID = new List<int>();
                foreach (var item in list)
                {
                    if(ListConversionID.Exists(m => m == item.ConversionID))
                    {
                        continue;
                    }
                    ListConversionID.Add(item.ConversionID);
                    model.Add(new ZAL_SearchListConversionViewModel()
                    {
                        ConversionID = item.ConversionID,
                        UserID = item.UserID,
                        SenderID = item.SenderID,
                        DisplayName = item.DisplayName,
                        Avatar = item.Avatar,
                        MessageText = item.MessageText,
                        MessageType = item.MessageType,
                        NumberOfMessage = item.NumberOfMessage,
                        UpdatedDate = item.UpdatedDate,
                        ConversionIndex = index
                    });
                    if(index > 0)
                    {
                        index--;
                    }
                }
            }
            return PartialView(model);
        }

        public ActionResult ZAL_ConversionView(ZAL_SearchListConversionViewModel model)
        {
            return PartialView(model);
        }

        public ActionResult ZAL_SearchListMessageOfConversionView(int ConversionID, string UserID, string DisplayName, string Avatar)
        {
            ZAL_SearchListMessageOfConversionViewModel model = new ZAL_SearchListMessageOfConversionViewModel() 
            {
                ConversionID = ConversionID,
                UserID = UserID,
                UserName = DisplayName,
                UserAvatar = Avatar,
                PageName = XHelper.Session.ZaloProfile.PageName,
                PageAvatar = XHelper.Session.ZaloProfile.PageAvatar,
                ListMessage = new List<ZAL_MessageOfConversionViewModel>()
            };
            var res = MessageRepository.SearchListMessageOfConversion(new ZMessageSearchListRequest()
            {
                ConversionID = ConversionID,
                Channel = "ZA",
                LimitRow = 10
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZMessageSearchListResponse>>(res.Data);
                List<ZAL_MessageOfConversionViewModel> ListMessage = new List<ZAL_MessageOfConversionViewModel>();
                foreach (var item in list)
                {
                    ListMessage.Add(new ZAL_MessageOfConversionViewModel()
                    {
                        MessageID = item.MessageID,
                        DisplayName = item.DisplayName,
                        Avatar = item.Avatar,
                        ChannelMessageID = item.ChannelMessageID,
                        MessageText = item.MessageText,
                        StickerID = item.StickerID,
                        LinkURL = item.LinkURL,
                        Thumbnail = item.Thumbnail,
                        Description = item.Description,
                        Latitude = item.Latitude,
                        Longitude = item.Longitude,
                        MessageType = item.MessageType,
                        Status = item.Status,
                        FromWhom = item.FromWhom,
                        CreatedDate = item.CreatedDate,
                        OrderByCreatedDate = DateTime.ParseExact(item.CreatedDate, "dd/MM/yyyy HH:mm:ss", null),
                        AmndDate = item.AmndDate,
                        OrderByAmndDate = DateTime.ParseExact(item.AmndDate, "dd/MM/yyyy HH:mm:ss", null),
                    });
                }
                model.ListMessage = ListMessage.OrderBy(m => m.OrderByCreatedDate).ThenBy(m => m.OrderByAmndDate).ToList();
            }
            return PartialView(model);
        }

        public ActionResult ZAL_MessageOfConversionView(List<ZAL_MessageOfConversionViewModel> model)
        {
            return PartialView(model);
        }

        public JsonResult ZAL_JSendMessage(ZZaloSendMessageRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ZaloRepository.SendMessage(new ZZaloSendMessageRequest()
                {
                    recipient = new ZZaloSendMessageRecipientRequest()
                    {
                        user_id = req.recipient.user_id
                    },
                    message = new ZZaloSendMessageMessageRequest()
                    {
                        text = req.message.text
                    },
                    AccessToken = XHelper.Session.ZaloProfile.AccessToken
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloController.ZAL_JSendMessage: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}