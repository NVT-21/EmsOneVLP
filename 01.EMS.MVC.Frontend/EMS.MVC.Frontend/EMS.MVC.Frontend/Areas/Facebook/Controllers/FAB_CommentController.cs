using EMS.Entity;
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
    public class FAB_CommentController : FAB_BaseController
    {
        // GET: Facebook/FAB_Comment
        public ActionResult FAB_CommentView()
        {
            FAB_CommentViewModel model = new FAB_CommentViewModel()
            {
                PageID = XHelper.Session.FacebookProfile.PageID,
                PageName = XHelper.Session.FacebookProfile.PageName,
                PageAvatar = XHelper.Session.FacebookProfile.PageAvatar
            };
            return View(model);
        }
        public ActionResult FAB_ListFeedForCommentView()
        {
            List<FAB_ListFeedForCommentViewModel> model = new List<FAB_ListFeedForCommentViewModel>();
            var res = FacebookRepository.ListFeedForComment();
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZFeedSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new FAB_ListFeedForCommentViewModel()
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
                        UpdatedDate = item.UpdatedDate,
                        PeopleComment = item.PeopleComment
                    });
                }
            }
            return PartialView(model);
        }
        public ActionResult FAB_CommentOfFeed(int FeedID)
        {
            FAB_CommentOfFeedViewModel model = null;
            var res = FacebookRepository.CommentOfFeed(FeedID);
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZCommentSearchListResponse>(res.Data);
                model = new FAB_CommentOfFeedViewModel()
                {
                    Feed = item.Feed,
                    Comment = item.Comment
                };
            }
            return PartialView(model);
        }
        public JsonResult FAB_SendComment(string ChannelFeedID, string Message)
        {
            var res = FacebookRepository.SendComment(ChannelFeedID, Message);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FAB_SendReplyComment(string ChannelCommentID, string Message)
        {
            var res = FacebookRepository.SendComment(ChannelCommentID, Message);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FAB_DeleteComment(string ChannelCommentID)
        {
            var res = FacebookRepository.DeleteComment(ChannelCommentID);
            if (res.Code == "00")
            {
                res = FacebookRepository.DeleteCommentEMSOne(ChannelCommentID);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FAB_DeleteFeed(string FeedID)
        {
            var res = FacebookRepository.DeleteFeed(FeedID);
            if (res.Code == "00")
            {
                res = FacebookRepository.DeleteFeedEMSOne(FeedID);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FAB_LikeComment(string ChannelCommentID)
        {
            var res = FacebookRepository.LikeComment(ChannelCommentID);
            if (res.Code == "00")
            {
                res = FacebookRepository.UpdateLikeCommentEMSOne(ChannelCommentID,"Y");
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FAB_DeleteLikeComment(string ChannelCommentID)
        {
            var res = FacebookRepository.DeleteLikeComment(ChannelCommentID);
            if (res.Code == "00")
            {
                res = FacebookRepository.UpdateLikeCommentEMSOne(ChannelCommentID, "N");
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FAB_HiddentComment(string ChannelCommentID, string Type)
        {
            var res = FacebookRepository.HiddenComment(ChannelCommentID,Type);
            var Status = Type == "true" ? "Y" : "N";  
            if (res.Code == "00" || res.Code == "01")
            {
                res = FacebookRepository.UpdateHiddenCommentEMSOne(ChannelCommentID, Status);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}