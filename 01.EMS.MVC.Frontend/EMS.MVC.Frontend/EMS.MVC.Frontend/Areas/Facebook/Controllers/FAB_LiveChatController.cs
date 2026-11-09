using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Areas.Facebook.Models;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Facebook.Controllers
{
    public class FAB_LiveChatController : FAB_BaseController
    {
        // GET: Facebook/FAB_LiveChat
        public ActionResult FAB_LiveChatView()
        {
            FAB_LiveChatViewModel model = new FAB_LiveChatViewModel()
            {
                PageID = XHelper.Session.FacebookProfile.PageID,
                PageName = XHelper.Session.FacebookProfile.PageName,
                PageAvatar = XHelper.Session.FacebookProfile.PageAvatar,
                ListConversionSticker = FacebookRepository.ListConversationSticker(new ZConversationStickerSearchListRequest())
            };
            return View(model);
        }

        #region FAB_Conversion
        public ActionResult FAB_SearchListConversionView(FAB_SearchListConversionModel req)
        {
            List<FAB_SearchListConversionViewModel> model = new List<FAB_SearchListConversionViewModel>();
            var res = ConversionRepository.SearchListConversion(new ZConversionSearchListRequest()
            {
                PageID = XHelper.Session.FacebookProfile.PageID,
                Channel = "FB",
                //LimitRow = 10,
                SearchKey = req.SearchKey,
                StickerID = req.StickerID
            });
            if (res.Code == "00")
            {
                var index = 1;
                var list = JsonConvert.DeserializeObject<List<ZConversionSearchListResponse>>(res.Data);
                var ListConversionID = new List<int>();
                foreach (var item in list)
                {
                    if (ListConversionID.Exists(m => m == item.ConversionID))
                    {
                        continue;
                    }
                    ListConversionID.Add(item.ConversionID);
                    model.Add(new FAB_SearchListConversionViewModel()
                    {
                        ConversionID = item.ConversionID,
                        UserID = item.UserID,
                        SenderID = item.SenderID,
                        DisplayName = item.DisplayName,
                        Avatar = item.Avatar,
                        ProfileLink = item.ProfileLink,
                        MessageText = item.MessageText,
                        MessageType = item.MessageType,
                        NumberOfMessage = item.NumberOfMessage,
                        UpdatedDate = item.UpdatedDate,
                        ConversionIndex = index,
                        ListSticker = item.ListSticker
                    });
                    if (index > 0)
                    {
                        index--;
                    }
                }
            }
            return PartialView(model);
        }
        public ActionResult FAB_ConversionView(FAB_SearchListConversionViewModel model)
        {
            return PartialView(model);
        }
        #endregion

        #region FAB_MessageOfConversion
        public ActionResult FAB_SearchListMessageOfConversionView(FAB_SearchListMessageOfConversionModel req)
        {
            FAB_SearchListMessageOfConversionViewModel model = new FAB_SearchListMessageOfConversionViewModel()
            {
                ConversionID = req.ConversionID,
                UserID = req.UserID,
                UserName = req.DisplayName,
                UserAvatar = req.Avatar,
                UserLink = req.ProfileLink,
                ListSticker = req.ListSticker,
                PageName = XHelper.Session.FacebookProfile.PageName,
                PageAvatar = XHelper.Session.FacebookProfile.PageAvatar,
                ListMessage = new List<FAB_MessageOfConversionViewModel>()
            };
            var res = MessageRepository.SearchListMessageOfConversion(new ZMessageSearchListRequest()
            {
                ConversionID = req.ConversionID,
                Channel = "FB",
                LimitRow = 100
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZMessageSearchListResponse>>(res.Data);
                List<FAB_MessageOfConversionViewModel> ListMessage = new List<FAB_MessageOfConversionViewModel>();
                foreach (var item in list)
                {
                    ListMessage.Add(new FAB_MessageOfConversionViewModel()
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
        public ActionResult FAB_MessageOfConversionView(List<FAB_MessageOfConversionViewModel> model)
        {
            return PartialView(model);
        }
        #endregion

        #region FAB_JSend - File - Message
        public JsonResult FAB_JSendMessage(ZFacebookSendMessageRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = FacebookRepository.SendMessage(new ZFacebookSendMessageRequest()
                {
                    recipient = new ZFacebookSendMessageRecipientRequest()
                    {
                        id = req.recipient.id
                    },
                    message = new ZFacebookSendMessageMessageRequest()
                    {
                        text = req.message.text
                    },
                    messaging_type = "MESSAGE_TAG",
                    tag = "POST_PURCHASE_UPDATE",
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FAB_LiveChatController.FAB_JSendMessage: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FAB_JSendFile(ZFacebookSendFileRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = FacebookRepository.SendMessage(new ZFacebookSendFileRequest()
                {
                    recipient = new ZFacebookSendMessageRecipientRequest()
                    {
                        id = req.recipient.id
                    },
                    message = new ZFacebookSendFileMessageRequest()
                    {
                        attachment = new ZFacebookSendFileMessageAttachmentRequest()
                        {
                            type = req.message.attachment.type,
                            payload = new ZFacebookSendFileMessageAttachmentPayloadRequest()
                            {
                                is_reusable = req.message.attachment.payload.is_reusable,
                                url = req.message.attachment.payload.url
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FAB_LiveChatController.FAB_JSendFile: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FAB_OrderSuccessHistoryOfUser - FAB_RequestOrderHistoryOfUser
        public ActionResult FAB_OrderSuccessHistoryOfUserView(FAB_OrderSuccessHistoryOfUserViewModel req)
        {
            FAB_OrderSuccessHistoryOfUserViewModel model = null;
            model = new FAB_OrderSuccessHistoryOfUserViewModel()
            {
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                MobileNumber = req.MobileNumber
            };
            return PartialView(model);
        }
        public ActionResult FAB_SearchOrderSuccessHistoryOfUserView(FAB_OrderSuccessHistoryOfUserViewModel req)
        {
            List<FAB_SearchOrderSuccessHistoryOfUserViewModel> oListItem = new List<FAB_SearchOrderSuccessHistoryOfUserViewModel>();
            var res = OrderRepository.SearchOrderSuccessHistory(new ZOrderSuccessHistoryRequest()
            {
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                MobileNumber = req.MobileNumber,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                SearchKey = req.SearchKey
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZOrderSuccessHistoryResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new FAB_SearchOrderSuccessHistoryOfUserViewModel()
                    {
                        ID = item.OrderID,
                        OrderCode = item.OrderCode,
                        ShippingCode = item.ShippingCode,
                        Status = item.Status,
                        TotalProductValue = item.TotalProductValue,
                        CreatedDate = item.CreatedDate,
                    });
                }
            }
            return PartialView(oListItem);
        }
        public ActionResult FAB_RequestOrderHistoryOfUser(string MobileNumber)
        {
            List<FAB_RequestOrderHistoryOfUserViewModel> oListItem = new List<FAB_RequestOrderHistoryOfUserViewModel>();
            var res = RequestOrderRepository.SearchListRequestOrder(new ZRequestOrderListRequest()
            {
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                MobileNumber = MobileNumber
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZRequestOrderListResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new FAB_RequestOrderHistoryOfUserViewModel()
                    {
                        ID = item.ID,
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        Product = item.Product,
                        Status = XData.GetNameRequestOrderStatus(item.Status),
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate
                    });
                }
            }
            return PartialView(oListItem);
        }
        #endregion

        #region FAB_UserInformation
        public ActionResult FAB_UserInformationOfConversion(FAB_UserInformationOfConversionModel req)
        {
            FAB_UserInformationOfConversionViewModel model = null;
            var res = FacebookRepository.GetUserChannelByChannelUserID(new ZUserChannelGetByChannelUserIDRequest()
            {
                ChannelUserID = req.ChannelUserID,
                Channel = "FB",
                PageID = XHelper.Session.FacebookProfile.PageID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZUserChannelGetByChannelUserIDResponse>(res.Data);
                model = new FAB_UserInformationOfConversionViewModel()
                {
                    UserChannelID = item.UserChannelID,
                    ChannelUserID = item.ChannelUserID,
                    DisplayName = item.DisplayName,
                    Avatar = item.Avatar,
                    FullName = item.FullName,
                    MobileNumber = item.MobileNumber,
                    Email = item.Email,
                    Province = item.Province,
                    District = item.District,
                    Ward = item.Ward,
                    Address = item.Address,
                    FullAddress = item.FullAddress,
                    IsBlackList = item.IsBlackList
                };
            }
            ViewBag.ChannelUserID = req.ChannelUserID;
            ViewBag.DisplayName = req.DisplayName;
            ViewBag.Avatar = req.Avatar;
            ViewBag.ProfileLink = req.ProfileLink;
            //TempData["MobileNumber"] = model.MobileNumber;
            return PartialView(model);
        }
        public ActionResult FAB_UpdateUserInformationView(FAB_UserInformationOfConversionViewModel data)
        {
            FAB_UpdateUserInformationViewModel model = new FAB_UpdateUserInformationViewModel()
            {
                UserChannelID = data.UserChannelID,
                ChannelUserID = data.ChannelUserID,
                DisplayName = data.DisplayName,
                Avatar = data.Avatar,
                FullName = data.FullName,
                MobileNumber = data.MobileNumber,
                Email = data.Email,
                Province = data.Province,
                District = data.District,
                Ward = data.Ward,
                Address = data.Address,
                IsBlackList = data.IsBlackList,

            };
            ViewBag.ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());
            ViewBag.ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = data.Province != null ? data.Province.ID : 0 });
            ViewBag.ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = data.District != null ? data.District.ID : 0 });
            return PartialView(model);
        }
        public JsonResult FAB_UpdateUserInformation(FAB_UpdateUserInformationViewModel model)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = FacebookRepository.UpdateInformationUserChannel(new ZUserChannelWebUpdateInformationRequest()
                {
                    UserChannelID = model.UserChannelID,
                    FullName = model.FullName,
                    MobileNumber = model.MobileNumber,
                    Email = model.Email,
                    ProvinceID = model.Province.ID,
                    DistrictID = model.District.ID,
                    WardID = model.Ward.ID,
                    Address = model.Address,
                    Channel = "FB",
                    IsBlackList = model.IsBlackList,
                    isUpdatedCustomer = model.isUpdatedCustomer,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FAB_LiveChat.FAB_UpdateUserInformation: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FAB_GetCustomerByMobileNumber(string MobileNumber)
        {
            ZResponse res = new ZResponse();
            res = FacebookRepository.GetCustomerByMobileNumber(new ZCustomerGetByMobileNumberRequest()
            {
                MobileNumber = MobileNumber,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FAB_UserInformationView()
        {
            return PartialView();
        }
        #endregion

        #region FAB_AddOrder
        public ActionResult FAB_AddOrderView()
        {
            AddOrderViewModel model = new AddOrderViewModel();
            if (TempData.ContainsKey("MobileNumber"))
            {
                model.MobileNumber = TempData["MobileNumber"].ToString();
            }
            model.ListChannel = XData.GetListChannel();
            model.ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            model.WareHouseID = model.ListWareHouse.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListWareHouse.Where(m => m.IsDefault == "Y").First().ID : 0;
            model.CollectionType = new XBaseModel() { ID = XHelper.Session.EmployeeProfile.ShopInformation.CollectionType };
            model.Weight = 100;
            model.Quantity = 1;
            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                if (model.ListMainTransportService.Count > 0)
                {
                    model.MainTransportServiceID = model.ListMainTransportService.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListMainTransportService.Where(m => m.IsDefault == "Y").First().ID : 0;
                }
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
                if (model.ListExtraTransportService.Count > 0)
                {
                    model.ExtraTransportServiceID = model.ListExtraTransportService.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListExtraTransportService.Where(m => m.IsDefault == "Y").First().ID.ToString() : "0";
                }
            }
            else
            {
                model.ListMainTransportService = new List<TransportServiceModel>();
                model.ListExtraTransportService = new List<TransportServiceModel>();
            }
            return PartialView(model);
        }
        #endregion

        #region FAB_Emoji - FAB_Sticker View
        public ActionResult FAB_LiveChatEmojiView()
        {
            return PartialView();
        }
        public ActionResult FAB_LiveChatStickerView(FAB_LiveChatStickerViewModel model)
        {
            return PartialView(model);
        }
        #endregion

        #region FAB_StickerOfConversationView
        public ActionResult FAB_StickerOfConversationView()
        {
            return PartialView();
        }
        public ActionResult FAB_SearchStickerOfConversationView(FAB_ConversationStickerModel req)
        {
            List<FAB_SearchListConversationStickerViewModel> model =
                FacebookRepository.ListCStickerOfC(
                    new ZConversationStickerSearchListRequest()
                    {
                        ConversationID = req.ConversationID
                    }
                );
            return PartialView(model);
        }
        public JsonResult FAB_UpdateStickerOfConversation(FAB_UpdateStickerOfConversationModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = FacebookRepository.UpdateCStickerOfC(new ZCStickerOfCUpdateRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    PageID = XHelper.Session.FacebookProfile.PageID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    Channel = "FB",
                    ConversationID = req.ConversationID,
                    ListItem = req.ListItem
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FAB_LiveChat.FAB_UpdateStickerOfConversation: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}