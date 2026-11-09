using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Areas.Facebook.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Facebook.Controllers
{
    public class FAB_HomeController : FAB_BaseController
    {
        public ActionResult FAB_DashboardView()
        {
            FAB_DashboardViewModel model = new FAB_DashboardViewModel()
            {
                PageID = XHelper.Session.FacebookProfile.PageID,
                PageName = XHelper.Session.FacebookProfile.PageName,
                PageAvatar = XHelper.Session.FacebookProfile.PageAvatar,
                PageCover = XHelper.Session.FacebookProfile.PageCover,
                PageCategory = XHelper.Session.FacebookProfile.PageCategory,
                PageAccessToken = XHelper.Session.FacebookProfile.PageAccessToken,
                UserAccessToken = XHelper.Session.FacebookProfile.UserAccessToken
            };
            List<FAB_SearchListConversionViewModel> ListConversion = new List<FAB_SearchListConversionViewModel>();
            //var resListConversion = FacebookRepository.ListConversion("FB",20);
            var resListConversion = ConversionRepository.SearchListConversion(new ZConversionSearchListRequest()
            {
                PageID = XHelper.Session.FacebookProfile.PageID,
                Channel = "FB",
                LimitRow = 20
            });
            if (resListConversion.Code == "00")
            {
                var index = 1;
                var list = JsonConvert.DeserializeObject<List<ZConversionSearchListResponse>>(resListConversion.Data);
                var ListConversionID = new List<int>();
                foreach (var item in list)
                {
                    if (ListConversionID.Exists(m => m == item.ConversionID))
                    {
                        continue;
                    }
                    ListConversionID.Add(item.ConversionID);
                    ListConversion.Add(new FAB_SearchListConversionViewModel()
                    {
                        ConversionID = item.ConversionID,
                        UserID = item.UserID,
                        SenderID = item.SenderID,
                        DisplayName = item.DisplayName,
                        Avatar = item.Avatar,
                        MessageText = item.MessageText,
                        NumberOfMessage = item.NumberOfMessage,
                        UpdatedDate = XData.TimeAgo(DateTime.ParseExact(item.UpdatedDate, "dd/MM/yyyy HH:mm:ss", null)),
                        ConversionIndex = index
                    });
                    if (index > 0)
                    {
                        index--;
                    }
                }
            }
            var ListConversionCheck = ListConversion.Where(m => m.NumberOfMessage > 0).Skip(0).Take(5).ToList();
            model.ListConversion = ListConversionCheck;
            List<FAB_ListFeedForCommentViewModel> ListFeed = new List<FAB_ListFeedForCommentViewModel>();
            var resListFeed = FacebookRepository.ListFeedForComment();
            if (resListFeed.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZFeedSearchListResponse>>(resListFeed.Data);
                foreach (var item in list)
                {
                    ListFeed.Add(new FAB_ListFeedForCommentViewModel()
                    {
                        ID = item.ID,
                        ChannelFeedID = item.ChannelFeedID,
                        Content = item.Content,
                        Avatar = item.Avatar,
                        NumberOfComment = item.NumberOfComment,
                        FromWhom = item.FromWhom,
                        Day = item.Day,
                        Month = item.Month,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = XData.TimeAgo(DateTime.ParseExact(item.UpdatedDate, "dd/MM/yyyy HH:mm:ss", null)),
                        PeopleComment = item.PeopleComment
                    });
                }
            }
            var ListFeedCheck = ListFeed.Where(m => m.NumberOfComment > 0).Skip(0).Take(5).ToList();
            model.ListFeed = ListFeedCheck;
            return View(model);
        }
    }
}