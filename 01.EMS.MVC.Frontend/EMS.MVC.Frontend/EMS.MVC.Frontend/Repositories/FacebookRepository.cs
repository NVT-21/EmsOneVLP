using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Areas.Facebook.Models;
using EMS.MVC.Frontend.Repositories;
using Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Repositories
{
    public class FacebookRepository
    {

        #region SplitList
        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
        #endregion

        #region FilterFromMessage
        public static string FilterEmailFromMessage(string Message)
        {
            var email = "";
            const string _MatchEmailPattern = @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                              + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                              + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                              + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
            Regex _rx = new Regex(_MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection _matches = _rx.Matches(Message);
            int _noOfMatches = _matches.Count;
            if (_noOfMatches == 1)
            {
                foreach (Match _match in _matches)
                {
                    email = _match.Value.ToString();
                }
            }
            return email;
        }
        public static string FilterPhoneFromMessage(string Message)
        {
            var phone = "";
            var _exp = new Regex(@"(\(?[0-9]{3}\)?)?\-?[0-9]{3}\-?[0-9]{4}", RegexOptions.IgnoreCase);
            MatchCollection _matchPhone = _exp.Matches(Message);
            if (_matchPhone.Count > 0)
            {
                foreach (Match _matchphone in _matchPhone)
                {
                    if (_matchphone.Value.ToString().Length == 10)
                    {
                        phone = _matchphone.Value.ToString();
                    }
                }
            }
            return phone;
        }
        #endregion

        #region SyncData
        public static void SyncFeed()
        {
            try
            {
                var resListPage = ListPageConnectV2(new ZConnectPageSearchListRequest()
                {
                    Channel = "FB",
                    IsActive = "Y",
                    PageID = XHelper.Session.FacebookProfile.PageID
                });
                if (resListPage.Code == "00")
                {
                    var ListPage = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(resListPage.Data);
                    if (ListPage.Count > 0)
                    {
                        foreach (var ItemListPage in ListPage)
                        {
                            var resFeed = GetFeed(new ZFacebookGetFeedRequest() { PageID = ItemListPage.PageID, PageAccessToken = ItemListPage.PageAccessToken });
                            if (resFeed.Code == "00")
                            {
                                var ListFeed = JsonConvert.DeserializeObject<ZFacebookGetFeedResponse>(resFeed.Data);
                                DateTime _timeBefore = DateTime.Now.AddMinutes(-60);
                                List<ZFeedAddSyncRequest> ListAddSyncFeed = new List<ZFeedAddSyncRequest>();
                                foreach (var ItemListFeed in ListFeed.Data)
                                {
                                    if (ItemListFeed.CreatedTime >= _timeBefore)
                                    {
                                        ListAddSyncFeed.Add(new ZFeedAddSyncRequest()
                                        {
                                            PageID = ItemListPage.PageID,
                                            ChannelFeedID = ItemListFeed.ID,
                                            Content = ItemListFeed.Message,
                                            FromWhom = ItemListFeed.From.ID == ItemListPage.PageID ? 0 : 1,
                                            Channel = "FB",
                                            CreatedBy = ItemListFeed.From.ID,
                                            CreatedDate = ItemListFeed.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                            UpdatedBy = ItemListFeed.From.ID,
                                            UpdatedDate = ItemListFeed.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                            FullPicture = ItemListFeed.FullPicture
                                        });
                                    }
                                }
                                if (ListAddSyncFeed.Count > 0)
                                {
                                    var resAddSyncFeed = AddSyncFeed(ListAddSyncFeed);
                                    if (resAddSyncFeed.Code != "00")
                                    {
                                        XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncFeed: Đồng bộ bài viết mới nhất thất bại.");
                                    }
                                }
                                else
                                {
                                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncFeed: Không có bài viết mới nhất.");
                                }
                            }
                            else
                            {
                                XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncFeed: Lỗi lấy danh sách bài viết từ Facebook.");
                            }
                        }
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncFeed: Trang không tồn tại.");
                    }
                }
                else
                {
                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncFeed: resListPage= " + JsonConvert.SerializeObject(resListPage));
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.SyncFeed: " + ex.Message);
            }
        }
        public static void SyncComment()
        {
            try
            {
                var resListPage = ListPageConnectV2(new ZConnectPageSearchListRequest()
                {
                    Channel = "FB",
                    IsActive = "Y",
                    PageID = XHelper.Session.FacebookProfile.PageID
                });
                if (resListPage.Code == "00")
                {
                    var ListPage = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(resListPage.Data);
                    if (ListPage.Count > 0)
                    {
                        foreach (var ItemListPage in ListPage)
                        {
                            var resListFeed = SearchListFeed(new ZFeedSearchListRequest() { PageID = ItemListPage.PageID, Channel = "FB" });
                            if (resListFeed.Code == "00")
                            {
                                var ListFeed = JsonConvert.DeserializeObject<List<ZFeedSearchListResponse>>(resListFeed.Data);
                                if (ListFeed.Count > 0)
                                {
                                    foreach (var ItemListFeed in ListFeed)
                                    {
                                        if (string.IsNullOrEmpty(ItemListFeed.ChannelFeedID))
                                        {
                                            continue;
                                        }
                                        var resComment = GetComment(new ZFacebookGetCommentRequest() { FeedID = ItemListFeed.ChannelFeedID, PageAccessToken = ItemListPage.PageAccessToken });
                                        if (resComment.Code == "00")
                                        {
                                            var ListComment = JsonConvert.DeserializeObject<ZFacebookGetCommentResponse>(resComment.Data);
                                            DateTime _timeBefore = DateTime.Now.AddMinutes(-60);
                                            List<ZCommentAddSyncRequest> ListAddSyncComment = new List<ZCommentAddSyncRequest>();
                                            foreach (var ItemListComment in ListComment.Data)
                                            {
                                                if (ItemListComment.CreatedTime >= _timeBefore)
                                                {
                                                    var CheckFrom = ItemListComment.From != null ? ItemListComment.From.ID : "";
                                                    ListAddSyncComment.Add(new ZCommentAddSyncRequest()
                                                    {
                                                        FeedID = ItemListFeed.ID,
                                                        ChannelCommentID = ItemListComment.ID,
                                                        Content = ItemListComment.Message,
                                                        ContentEmail = FilterEmailFromMessage(ItemListComment.Message.ToString()),
                                                        ContentPhone = FilterPhoneFromMessage(ItemListComment.Message.ToString()),
                                                        CreatedBy = CheckFrom,
                                                        CreatedDate = ItemListComment.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                                        UpdatedBy = CheckFrom,
                                                        UpdatedDate = ItemListComment.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                                        FromWhom = CheckFrom == ItemListPage.PageID ? 0 : 1,
                                                        ReplyCommentCount = ItemListComment.CommentCount,
                                                        CommentLevel = 0,
                                                        ParentChannelCommentID = ""
                                                    });
                                                }
                                            }

                                            if (ListAddSyncComment.Count > 0)
                                            {
                                                var resAddSyncComment = AddSyncComment(ListAddSyncComment);
                                                if (resAddSyncComment.Code == "00")
                                                {
                                                    var ListReplyCommentLevel0 = ListAddSyncComment.Where(m => m.ReplyCommentCount > 0);
                                                    foreach (var ItemListReplyCommentLevel0 in ListReplyCommentLevel0)
                                                    {
                                                        var resReplyComment = GetComment(new ZFacebookGetCommentRequest() { FeedID = ItemListReplyCommentLevel0.ChannelCommentID, PageAccessToken = ItemListPage.PageAccessToken });
                                                        if (resReplyComment.Code == "00")
                                                        {
                                                            var ListReplyComment = JsonConvert.DeserializeObject<ZFacebookGetCommentResponse>(resReplyComment.Data);
                                                            List<ZCommentAddSyncRequest> ListAddSyncReplyComment = new List<ZCommentAddSyncRequest>();
                                                            foreach (var ItemListReplyCommentLevel1 in ListReplyComment.Data)
                                                            {
                                                                if (ItemListReplyCommentLevel1.CreatedTime >= _timeBefore)
                                                                {
                                                                    var CheckFrom = ItemListReplyCommentLevel1.From != null ? ItemListReplyCommentLevel1.From.ID : "";
                                                                    ListAddSyncReplyComment.Add(new ZCommentAddSyncRequest()
                                                                    {
                                                                        FeedID = ItemListFeed.ID,
                                                                        ChannelCommentID = ItemListReplyCommentLevel1.ID,
                                                                        Content = ItemListReplyCommentLevel1.Message,
                                                                        ContentEmail = FilterEmailFromMessage(ItemListReplyCommentLevel1.Message.ToString()),
                                                                        ContentPhone = FilterPhoneFromMessage(ItemListReplyCommentLevel1.Message.ToString()),
                                                                        CreatedBy = CheckFrom,
                                                                        CreatedDate = ItemListReplyCommentLevel1.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                                                        UpdatedBy = CheckFrom,
                                                                        UpdatedDate = ItemListReplyCommentLevel1.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                                                        FromWhom = CheckFrom == ItemListPage.PageID ? 0 : 1,
                                                                        CommentLevel = 1,
                                                                        ParentChannelCommentID = ItemListReplyCommentLevel0.ChannelCommentID
                                                                    });
                                                                }
                                                            }
                                                            if (ListAddSyncReplyComment.Count > 0)
                                                            {
                                                                var resAddSyncReplyComment = AddSyncComment(ListAddSyncReplyComment);
                                                                if (resAddSyncReplyComment.Code != "00")
                                                                {
                                                                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: Đồng bộ trả lời bình luận mới nhất từ FB thất bại.");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: Không có trả lời bình luận mới nhất từ FB.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: Lỗi lấy danh sách trả lời comment từ FB.");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: Đồng bộ bình luận mới nhất thất bại.");
                                                }
                                            }
                                            else
                                            {
                                                XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: Không có bình luận mới nhất từ FB.");
                                            }
                                        }
                                        else
                                        {
                                            XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: Lỗi lấy danh sách comment từ FB.");
                                        }
                                    }
                                }
                                else
                                {
                                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: Bài viết không tồn tại.");
                                }
                            }
                            else
                            {
                                XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: resListFeed= " + JsonConvert.SerializeObject(resListFeed));
                            }
                        }
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: Trang không tồn tại.");
                    }
                }
                else
                {
                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncComment: resListPage= " + JsonConvert.SerializeObject(resListPage));
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.SyncComment: " + ex.Message);
            }
        }
        public static void SyncMessage()
        {
            List<ZMessageAddSyncRequest> ListAddSyncMessage = new List<ZMessageAddSyncRequest>();
            try
            {
                var resListPage = ListPageConnectV2(new ZConnectPageSearchListRequest()
                {
                    Channel = "FB",
                    IsActive = "Y",
                    PageID = XHelper.Session.FacebookProfile.PageID
                });
                if (resListPage.Code == "00")
                {
                    var ListPage = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(resListPage.Data);
                    if (ListPage.Count > 0)
                    {
                        foreach (var ItemListPage in ListPage)
                        {
                            var resConversation = GetConversation(new ZFacebookGetConversationRequest() { 
                                PageID = ItemListPage.PageID,
                                PageAccessToken = ItemListPage.PageAccessToken
                            });
                            if (resConversation.Code == "00")
                            {
                                var ListConversation = JsonConvert.DeserializeObject<ZFacebookGetConversationResponse>(resConversation.Data);
                                DateTime _timeBefore = DateTime.Now.AddDays(-1);
                                foreach (var ItemListConversation in ListConversation.Conversations.Data)
                                {
                                    if (ItemListConversation != null)
                                    {
                                        if (ItemListConversation.UpdatedTime >= _timeBefore)
                                        {
                                            var resMessage = GetMessage(new ZFacebookGetMessageRequest() { 
                                                ConversationID = ItemListConversation.ID,
                                                PageAccessToken = ItemListPage.PageAccessToken
                                            });
                                            if (resMessage.Code == "00")
                                            {
                                                var ListMessage = JsonConvert.DeserializeObject<ZFacebookGetMessageResponse>(resMessage.Data);
                                                foreach (var ItemListMessage in ListMessage.Messages.Data)
                                                {
                                                    ListAddSyncMessage.Add(new ZMessageAddSyncRequest()
                                                    {
                                                        PageID = ItemListPage.PageID,
                                                        ChannelMessageID = ItemListMessage.ID,
                                                        SenderID = ItemListMessage.From.ID,
                                                        RecipientID = ItemListMessage.To.Data[0].ID,
                                                        FromWhom = ItemListMessage.From.ID == ItemListPage.PageID ? 1 : 0,
                                                        Message = ItemListMessage.Message,
                                                        Channel = "FB",
                                                        CreatedBy = ItemListMessage.From.ID,
                                                        CreatedDate = ItemListMessage.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                                        UpdatedBy = ItemListMessage.From.ID,
                                                        UpdatedDate = ItemListMessage.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                                    });
                                                }
                                                if (!string.IsNullOrEmpty(ListMessage.Messages.Paging.Cursors.After))
                                                {
                                                    var resMessageAfter = GetMessage(new ZFacebookGetMessageRequest() { 
                                                        ConversationID = ItemListConversation.ID,
                                                        PageAccessToken = ItemListPage.PageAccessToken,
                                                        PageAfter = ListMessage.Messages.Paging.Cursors.After
                                                    });
                                                    if (resMessageAfter.Code == "00")
                                                    {
                                                        var ListMessageAfter = JsonConvert.DeserializeObject<ZFacebookGetMessagesResponse>(resMessageAfter.Data);
                                                        foreach (var ItemListMessageAfter in ListMessageAfter.Data)
                                                        {
                                                            ListAddSyncMessage.Add(new ZMessageAddSyncRequest()
                                                            {
                                                                PageID = ItemListPage.PageID,
                                                                ChannelMessageID = ItemListMessageAfter.ID,
                                                                SenderID = ItemListMessageAfter.From.ID,
                                                                RecipientID = ItemListMessageAfter.To.Data[0].ID,
                                                                FromWhom = ItemListMessageAfter.From.ID == ItemListPage.PageID ? 0 : 1,
                                                                Message = ItemListMessageAfter.Message,
                                                                Channel = "FB",
                                                                CreatedBy = ItemListMessageAfter.From.ID,
                                                                CreatedDate = ItemListMessageAfter.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                                                UpdatedBy = ItemListMessageAfter.From.ID,
                                                                UpdatedDate = ItemListMessageAfter.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss"),
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncMessage: Lỗi lấy danh sách tin nhắn từ Facebook.");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncMessage: Lỗi lấy danh sách tin nhắn từ Facebook.");
                                            }
                                        }
                                    }
                                }

                                if (ListAddSyncMessage.Count > 0)
                                {
                                    var resAddSyncMessage = AddSyncMessage(ListAddSyncMessage);
                                    if (resAddSyncMessage.Code != "00")
                                    {
                                        XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncMessage: Đồng bộ tin nhắn mới nhất thất bại.");
                                    }
                                }
                                else
                                {
                                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncMessage: Không có tin nhắn mới nhất.");
                                }
                            }
                            else
                            {
                                XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncMessage: Lỗi lấy danh sách hội thoại từ Facebook.");
                            }
                        }
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncMessage: Trang không tồn tại.");
                    }
                }
                else
                {
                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SyncMessage: resListPage= " + JsonConvert.SerializeObject(resListPage));
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.SyncMessage: " + ex.Message);
            }
        }
        #endregion

        #region API EMSOne

        #region PageConnect
        public static List<SelectListItem> ListPageConnect(string Channel)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            var res = ChannelRepository.SearchListChannel(new ZConnectPageSearchListRequest()
            {
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                Channel = "FB",
                IsActive = "Y"
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZConnectPageSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    if (item.Channel == Channel && item.IsActiveCheck == "Y")
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.PageName,
                            Value = item.PageID
                        });
                    }
                }
            }
            return listItem;
        }
        public static ZResponse ListPageConnectV2(ZConnectPageSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CONNECT_PAGE_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
                if (res.Code != "00")
                {
                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.ListPageConnectV2: res=" + JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.ListPageConnectV2: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Sync
        public static ZResponse AddSyncFeed(List<ZFeedAddSyncRequest> req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "FACEBOOK_FEED_SYNC_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
                if (res.Code != "00")
                {
                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.AddSyncFeed: res=" + JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.AddSyncFeed: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SearchListFeed(ZFeedSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "FEED_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
                if (res.Code != "00")
                {
                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.SearchList: res=" + JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.SearchList: " + ex.Message);
            }
            return res;
        }
        public static ZResponse AddSyncComment(List<ZCommentAddSyncRequest> req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "FACEBOOK_COMMENT_SYNC_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
                if (res.Code != "00")
                {
                    XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.AddSyncComment: res=" + JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.AddSyncComment: " + ex.Message);
            }
            return res;
        }
        public static ZResponse AddSyncMessage(List<ZMessageAddSyncRequest> req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var listSplit = FacebookRepository.SplitList(req, 50);
                foreach (var item in listSplit)
                {
                    res = XHelper.Invoke(new ZRequest()
                    {
                        Code = "MESSAGE_SYNC_ADD",
                        Data = JsonConvert.SerializeObject(req)
                    });
                    if (res.Code != "00")
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, "FacebookRepository.AddSyncMessage: res=" + JsonConvert.SerializeObject(res));
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.AddSyncMessage: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Follower
        public static List<FAB_SearchListFollowerViewModel> ListFollower()
        {
            List<FAB_SearchListFollowerViewModel> listData = new List<FAB_SearchListFollowerViewModel>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "USER_CHANNEL_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(new ZUserChannelSearchListRequest()
                    {
                        PageID = XHelper.Session.FacebookProfile.PageID,
                        Channel = "FB"
                    })
                });

                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZUserChannelSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listData.Add(new FAB_SearchListFollowerViewModel()
                        {
                            UserChannelID = item.UserChannelID,
                            ChannelUserID = item.ChannelUserID,
                            ChannelUserIDByApp = item.ChannelUserIDByApp,
                            FullName = item.FullName,
                            UserGender = item.UserGender,
                            MobileNumber = item.MobileNumber,
                            Avatar = item.Avatar,
                            BirthDay = item.BirthDay,
                            DisplayName = item.DisplayName,
                            ProvinceName = item.ProvinceName,
                            DistrictName = item.DistrictName,
                            WardName = item.WardName,
                            Street = item.Street,
                            FullAddress = item.FullAddress,
                            CreatedDate = item.CreatedDate,
                            AuthorizationCode = item.AuthorizationCode,
                            AccessToken = item.AccessToken,
                            RefreshToken = item.RefreshToken,
                            UserAccessToken = item.UserAccessToken,
                            PageAccessToken = item.PageAccessToken
                        });
                    }
                }
                else
                {
                    throw new Exception("Tải danh sách nhóm khách hàng thất bại!");
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.ListFollower: " + ex.Message);
            }
            return listData;
        }
        #endregion

        #region Conversation Sticker
        public static List<FAB_SearchListConversationStickerViewModel> ListConversationSticker(ZConversationStickerSearchListRequest req)
        {
            List<FAB_SearchListConversationStickerViewModel> ListItem = new List<FAB_SearchListConversationStickerViewModel>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CONVERSATION_STICKER_LIST",
                    Data = JsonConvert.SerializeObject(new ZConversationStickerSearchListRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        PageID = XHelper.Session.FacebookProfile.PageID,
                        Channel = "FB",
                        SearchKey = req.SearchKey
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZConversationStickerSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new FAB_SearchListConversationStickerViewModel()
                        {
                            StickerID = item.StickerID,
                            StickerName = item.StickerName,
                            StickerColor = item.StickerColor
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.ListConversationSticker: " + ex.Message);
            }
            return ListItem;
        }
        public static List<FAB_SearchListConversationStickerViewModel> ListCStickerOfC(ZConversationStickerSearchListRequest req)
        {
            List<FAB_SearchListConversationStickerViewModel> ListItem = new List<FAB_SearchListConversationStickerViewModel>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "C_STICKER_OF_C_LIST",
                    Data = JsonConvert.SerializeObject(new ZConversationStickerSearchListRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        PageID = XHelper.Session.FacebookProfile.PageID,
                        Channel = "FB",
                        ConversationID = req.ConversationID,
                        TypeList = "IS_CHOOSE"
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZConversationStickerSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        ListItem.Add(new FAB_SearchListConversationStickerViewModel()
                        {
                            StickerID = item.StickerID,
                            StickerName = item.StickerName,
                            StickerColor = item.StickerColor,
                            IsChoose = item.IsChoose
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.ListCStickerOfC: " + ex.Message);
            }
            return ListItem;
        }
        public static ZResponse UpdateCStickerOfC(ZCStickerOfCUpdateRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "C_STICKER_OF_C_UPDATE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.UpdateCStickerOfC: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region GetUserChannelByChannelUserID
        public static ZResponse GetUserChannelByChannelUserID(ZUserChannelGetByChannelUserIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "USER_CHANNEL_SEARCH_BY_CHANNEL_USER_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.GetUserChannelByChannelUserID: " + ex.Message);
            }
            return res;

        }
        #endregion

        #region GetListGroupCustomer
        public static List<SelectListItem> GetListGroupCustomer()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SEARCH_GROUP",
                    Data = JsonConvert.SerializeObject(new ZCustomerSearchGroupRequest()
                    {
                        GroupCode = "",
                        GroupName = "",
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCustomerSearchGroupResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.GroupName,
                            Value = item.ID.ToString()
                        });
                    }
                }
                else
                {
                    throw new Exception("Tải danh sách nhóm khách hàng thất bại!");
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.GetListGroupCustomer: " + ex.Message);
            }
            return listItem;
        }
        #endregion

        #region Feed
        public static ZResponse ListFeedForComment()
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "FEED_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(new ZFeedSearchListRequest()
                    {
                        PageID = XHelper.Session.FacebookProfile.PageID,
                        Channel = "FB"
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.ListFeedForComment: " + ex.Message);
            }
            return res;
        }
        public static ZResponse CommentOfFeed(int FeedID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "COMMENT_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(new ZCommentSearchListRequest()
                    {
                        FeedID = FeedID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.CommentOfFeed: " + ex.Message);
            }
            return res;
        }
        public static ZResponse DeleteCommentEMSOne(string ChannelCommentID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "COMMENT_DELETE",
                    Data = JsonConvert.SerializeObject(new ZCommentDeleteRequest()
                    {
                        Channel = "FB",
                        CommentID = ChannelCommentID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.DeleteCommentEMSOne: " + ex.Message);
            }
            return res;
        }
        public static ZResponse UpdateLikeCommentEMSOne(string ChannelCommentID, string Status)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "COMMENT_UPDATE_LIKE",
                    Data = JsonConvert.SerializeObject(new ZCommentUpdateReactionRequest()
                    {
                        CommentID = ChannelCommentID,
                        Channel = "FB",
                        Status = Status,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.UpdateLikeCommentEMSOne: " + ex.Message);
            }
            return res;
        }
        public static ZResponse UpdateHiddenCommentEMSOne(string ChannelCommentID, string Status)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "COMMENT_UPDATE_HIDDEN",
                    Data = JsonConvert.SerializeObject(new ZCommentUpdateReactionRequest()
                    {
                        CommentID = ChannelCommentID,
                        Channel = "FB",
                        Status = Status,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.UpdateHiddenCommentEMSOne: " + ex.Message);
            }
            return res;
        }
        public static ZResponse DeleteFeedEMSOne(string FeedID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "FEED_DELETE",
                    Data = JsonConvert.SerializeObject(new ZFeedDeleteRequest()
                    {
                        PageID = XHelper.Session.FacebookProfile.PageID,
                        Channel = "FB",
                        FeedID = FeedID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.DeleteFeedEMSOne: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region UpdateInformationUserChannel
        public static ZResponse UpdateInformationUserChannel(ZUserChannelWebUpdateInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "USER_CHANNEL_WEB_UPDATE_INFORMATION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.UpdateInformationUserChannel: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GetCustomerByMobileNumber(ZCustomerGetByMobileNumberRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "GET_BY_MOBILE_NUMBER",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.GetCustomerByMobileNumber: " + ex.Message);
            }
            return res;

        }
        #endregion

        #region PageConfiguration

        #region Like
        public static ZFacebookPageAutoLikeResponse PageAutoLike()
        {
            ZFacebookPageAutoLikeResponse model = new ZFacebookPageAutoLikeResponse();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "FACEBOOK_PAGE_AUTOLIKE",
                    Data = JsonConvert.SerializeObject(new ZFacebookSearchPageAutoLikeRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        PageID = XHelper.Session.FacebookProfile.PageID
                    })
                });
                if (res.Code == "00")
                {
                    model = JsonConvert.DeserializeObject<ZFacebookPageAutoLikeResponse>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.PageAutoLike: " + ex.Message);
            }
            return model;
        }
        public static ZResponse ChangeAutoLike(ZFacebookReactionPageAutoLikeRequest model)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "FACEBOOK_PAGE_AUTOLIKE_CHANGE_STATUS",
                    Data = JsonConvert.SerializeObject(new ZFacebookReactionPageAutoLikeRequest()
                    {
                        ID = model.ID,
                        Status = model.Status,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.ChangeAutoLike: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Hidden
        public static ZFacebookSearchPageAutoHiddenResponse PageAutoHidden()
        {
            ZFacebookSearchPageAutoHiddenResponse model = new ZFacebookSearchPageAutoHiddenResponse();
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "FACEBOOK_PAGE_AUTO_HIDDEN",
                    Data = JsonConvert.SerializeObject(new ZFacebookSearchPageAutoLikeRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        PageID = XHelper.Session.FacebookProfile.PageID
                    })
                });
                if (res.Code == "00")
                {
                    model = JsonConvert.DeserializeObject<ZFacebookSearchPageAutoHiddenResponse>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.PageAutoHidden: " + ex.Message);
            }
            return model;
        }
        public static ZResponse UpdateAutoHidden(ZFacebookEditPageAutoHiddenRequest model)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "FACEBOOK_PAGE_AUTO_HIDDEN_EDIT",
                    Data = JsonConvert.SerializeObject(new ZFacebookEditPageAutoHiddenRequest()
                    {
                        ID = model.ID,
                        IsAll = model.IsAll,
                        Text = model.Text,
                        AvailableDate = model.AvailableDate,
                        IsEmail = model.IsEmail,
                        IsPhone = model.IsPhone,
                        IsText = model.IsText,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.UpdateAutoHidden: " + ex.Message);
            }
            return res;
        }
        #endregion

        #endregion

        #endregion

        #region New API Facebook v18.0
        public static ZResponse ConnectPage(string Code)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_CONNECT_PAGE",
                    Data = JsonConvert.SerializeObject(new ZFacebookConnectRequest()
                    {
                        Code = Code
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.ConnectPage: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GetAccountProfile(ZFacebookGetProfileRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_ACCOUNT_PROFILE_GET",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.GetAccountProfile: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GetPageInfo(ZFacebookGetProfileRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_PAGE_INFO_GET",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.GetPageInfo: " + ex.Message);
            }
            return res;
        }
        public static ZResponse Subcribed(ZFacebookSubcribedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_SUBCRIBED",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.Subcribed: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GetFeed(ZFacebookGetFeedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_FEED_GET",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.GetFeed: " + ex.Message);
            }
            return res;
        }
        public static ZResponse DeleteFeed(string FeedID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_FEED_DELETE",
                    Data = JsonConvert.SerializeObject(new ZFacebookDeleteFeedRequest()
                    {
                        FeedID = FeedID,
                        PageAccessToken = XHelper.Session.FacebookProfile.PageAccessToken
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.DeleteFeed: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GetComment(ZFacebookGetCommentRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_COMMENT_GET",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.GetComment: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GetConversation(ZFacebookGetConversationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_CONVERSATION_GET",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.GetConversation: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GetMessage(ZFacebookGetMessageRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_MESSAGE_GET",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.GetMessage: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SendMessage(object req)
        {
            ZResponse res = new ZResponse();
            try
            {
                object obj = new
                {
                    PageAccessToken = XHelper.Session.FacebookProfile.PageAccessToken,
                    req = req
                };
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_MESSAGE_SEND",
                    Data = JsonConvert.SerializeObject(obj)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.SendMessage: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SendComment(string ChannelFeedID, string Message)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_COMMENT_SEND",
                    Data = JsonConvert.SerializeObject(new ZFacebookSendCommentRequest() { 
                       ID = ChannelFeedID,
                       RequestData = new ZFacebookReplyCommentRequest()
                       {
                           Message = Message,
                           AccessToken = XHelper.Session.FacebookProfile.PageAccessToken
                       }
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.SendComment: " + ex.Message);
            }
            return res;
        }
        public static ZResponse DeleteComment(string ChannelCommentID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_COMMENT_DELETE",
                    Data = JsonConvert.SerializeObject(new ZFacebookDeleteCommentRequest()
                    {
                        CommentID = ChannelCommentID,
                        PageAccessToken = XHelper.Session.FacebookProfile.PageAccessToken
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.DeleteComment: " + ex.Message);
            }
            return res;
        }
        public static ZResponse LikeComment(string ChannelCommentID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_COMMENT_LIKE",
                    Data = JsonConvert.SerializeObject(new ZFacebookLikeCommentRequest()
                    {
                        CommentID = ChannelCommentID,
                        PageAccessToken = XHelper.Session.FacebookProfile.PageAccessToken
                    })
                });
                if (res.Code == "00")
                {
                    var data = JsonConvert.DeserializeObject<ZFacebookLikeCommentResponse>(res.Data);
                    if (data.Success)
                    {
                        res.Code = "00";
                        res.Message = "Thích bình luận thành công.";
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.LikeComment: " + ex.Message);
            }
            return res;
        }
        public static ZResponse DeleteLikeComment(string ChannelCommentID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_COMMENT_DELETE_LIKE",
                    Data = JsonConvert.SerializeObject(new ZFacebookDeleteLikeCommentRequest()
                    {
                        CommentID = ChannelCommentID,
                        PageAccessToken = XHelper.Session.FacebookProfile.PageAccessToken
                    })
                });
                if (res.Code == "00")
                {
                    var data = JsonConvert.DeserializeObject<ZFacebookLikeCommentResponse>(res.Data);
                    if (data.Success)
                    {
                        res.Code = "00";
                        res.Message = "Bỏ thích bình luận thành công.";
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.DeleteLikeComment: " + ex.Message);
            }
            return res;
        }
        public static ZResponse HiddenComment(string ChannelCommentID, string Type)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "FACEBOOK_COMMENT_HIDDEN",
                    Data = JsonConvert.SerializeObject(new ZFacebookHiddenCommentRequest()
                    {
                        CommentID = ChannelCommentID,
                        Type = Type,
                        PageAccessToken = XHelper.Session.FacebookProfile.PageAccessToken
                    })
                });
                if (res.Code == "00")
                {
                    dynamic dyn = JsonConvert.DeserializeObject<object>(res.Data);
                    if (dyn.success == true)
                    {
                        res.Code = "00";
                        if (Type == "true")
                        {
                            res.Message = "Ẩn bình luận thành công.";
                        }
                        else
                        {
                            res.Message = "Bỏ ẩn bình luận thành công.";
                        }
                    }
                    else if (dyn.error.code == 1)
                    {
                        res.Code = "01";
                        res.Message = "Yêu cầu đánh dấu spam trùng lặp.";
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookRepository.HiddenComment: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}