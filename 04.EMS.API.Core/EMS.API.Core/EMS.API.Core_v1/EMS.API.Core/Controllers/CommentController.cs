using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Comment")]
    public class CommentController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZCommentSearchListRequest req)
        {
            int result = -1;
            List<XCommentModel> ListCommentTemp = new List<XCommentModel>();
            List<XCommentModel> ListComment = new List<XCommentModel>();
            ZCommentSearchListResponse model = null;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FEED_COMMENT_PKG.SEARCH_LIST_COMMENT", new object[]
                {
                    req.FeedID,
                    0,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_FEED_OUT_CURSOR"].Value)
                            {
                                if (dr.Read())
                                {
                                    model = new ZCommentSearchListResponse()
                                    {
                                        Feed = new XFeedModel() 
                                        {
                                            ID = Convert.ToInt32(dr["FEED_ID"]),
                                            ChannelFeedID = Convert.ToString(dr["CHANNEL_FEED_ID"]),
                                            Content = Convert.ToString(dr["FEED_CONTENT"]),
                                            Avatar = string.IsNullOrEmpty(Convert.ToString(dr["AVATAR"])) ? XHelper.UrlAvatarDefault : Convert.ToString(dr["AVATAR"]),
                                            CreatedBy = string.IsNullOrEmpty(Convert.ToString(dr["CREATED_BY"])) ? XHelper.DisplayNameDefault : Convert.ToString(dr["CREATED_BY"]),
                                            NumberOfComment = Convert.ToInt32(dr["NUMBER_OF_COMMENT"]),
                                            FromWhom = Convert.ToInt32(dr["FROM_WHOM"]),
                                            CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                            UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                            Day = DateTime.ParseExact(Convert.ToString(dr["CREATED_DATE"]), "dd/MM/yyyy HH:mm:ss", null).Day.ToString(),
                                            Month = DateTime.ParseExact(Convert.ToString(dr["CREATED_DATE"]), "dd/MM/yyyy HH:mm:ss", null).Month.ToString(),
                                            FullPicture = Convert.ToString(dr["FEED_PICTURE"])
                                        }
                                    };
                                }
                            }

                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_COMMENT_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    ListCommentTemp.Add(new XCommentModel()
                                    {
                                        ID = Convert.ToInt32(dr["COMMENT_ID"]),
                                        ChannelCommentID = Convert.ToString(dr["CHANNEL_COMMENT_ID"]),
                                        Content = Convert.ToString(dr["COMMENT_CONTENT"]),
                                        Level = Convert.ToInt32(dr["COMMENT_LEVEL"]),
                                        ParentChannelCommentID = Convert.ToString(dr["PARENT_CHANNEL_COMMENT_ID"]),
                                        Avatar = string.IsNullOrEmpty(Convert.ToString(dr["AVATAR"])) ? XHelper.UrlAvatarDefault : Convert.ToString(dr["AVATAR"]),
                                        CreatedBy = string.IsNullOrEmpty(Convert.ToString(dr["CREATED_BY"])) ? XHelper.DisplayNameDefault : Convert.ToString(dr["CREATED_BY"]),
                                        FromWhom = Convert.ToInt32(dr["FROM_WHOM"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        FormatCreatedDate = XData.TimeAgo(DateTime.ParseExact(Convert.ToString(dr["CREATED_DATE"]), "dd/MM/yyyy HH:mm:ss", null)),
                                        IsLike = Convert.ToString(dr["IS_LIKE"]),
                                        IsHidden = Convert.ToString(dr["IS_HIDDEN"])
                                    });
                                }
                            }


                            if (ListCommentTemp.Count > 0)
                            {
                                var ListCommentLevel0 = ListCommentTemp.Where(m => m.Level == 0);
                                foreach (var ItemCommentLevel0 in ListCommentLevel0)
                                {
                                    var ListCommentLevel1 = ListCommentTemp.Where(m => m.Level == 1 && m.ParentChannelCommentID == ItemCommentLevel0.ChannelCommentID);
                                    ListComment.Add(new XCommentModel()
                                    {
                                        ID = ItemCommentLevel0.ID,
                                        ChannelCommentID = ItemCommentLevel0.ChannelCommentID,
                                        Content = ItemCommentLevel0.Content,
                                        Level = ItemCommentLevel0.Level,
                                        ParentChannelCommentID = ItemCommentLevel0.ParentChannelCommentID,
                                        Avatar = ItemCommentLevel0.Avatar,
                                        CreatedBy = ItemCommentLevel0.CreatedBy,
                                        FromWhom = ItemCommentLevel0.FromWhom,
                                        CreatedDate = ItemCommentLevel0.CreatedDate,
                                        UpdatedDate = ItemCommentLevel0.UpdatedDate,
                                        FormatCreatedDate = ItemCommentLevel0.FormatCreatedDate,
                                        IsLike = ItemCommentLevel0.IsLike,
                                        IsHidden = ItemCommentLevel0.IsHidden,
                                        ListReplyComment = ListCommentLevel1.ToList()
                                    });
                                }

                                if (ListComment.Count > 0)
                                {
                                    model.Comment = ListComment;
                                }
                            }

                            if(model != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CommentController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddSyncComment")]
        public ZResponse AddSyncComment(List<ZCommentAddSyncRequest> req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                var ConvertXML = req.ToXML();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SYNC_FEED_COMMENT_PKG.ADD_SYNC_DATA", new object[]
                {
                    0,
                    ConvertXML
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CommentController.AddSyncComment" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("DeleteComment")]
        public ZResponse DeleteComment(ZCommentDeleteRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FEED_COMMENT_PKG.DELETE_COMMENT", new object[]
                {
                    0,
                    req.Channel,
                    req.CommentID,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Xoá bình luận thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CommentController.DeleteComment" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("UpdateLikeComment")]
        public ZResponse UpdateLikeComment(ZCommentUpdateReactionRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FEED_COMMENT_PKG.UPDATE_LIKE_COMMENT", new object[]
                {
                    0,
                    req.Channel,
                    req.CommentID,
                    req.Status,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thích bình luận thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CommentController.UpdateLikeComment" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("UpdateHiddenComment")]
        public ZResponse UpdateHiddenComment(ZCommentUpdateReactionRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FEED_COMMENT_PKG.UPDATE_HIDDEN_COMMENT", new object[]
                {
                    0,
                    req.Channel,
                    req.CommentID,
                    req.Status,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật ẩn/hiện bình luận thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CommentController.UpdateHiddenComment" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}