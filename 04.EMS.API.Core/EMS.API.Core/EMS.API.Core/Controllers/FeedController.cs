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
    [RoutePrefix("Feed")]
    public class FeedController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZFeedSearchListRequest req)
        {
            int result = -1;
            List<ZFeedSearchListResponse> ListFeed = new List<ZFeedSearchListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FEED_PKG.SEARCH_LIST_FEED", new object[]
                {
                    req.PageID,
                    req.Channel,
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
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    var ListPeopleComment = Convert.ToString(dr["PEOPLE_COMMENT"]);
                                    var SplitListPeopleComment = ListPeopleComment.Split(',');
                                    var CheckDuplicate = SplitListPeopleComment.Distinct().ToList();
                                    var FinalPeopleComment = "";
                                    if (CheckDuplicate[0] == "")
                                    {
                                        FinalPeopleComment = "Chưa có bình luận về bài viết của bạn";
                                    }
                                    else if(CheckDuplicate.Count == 1 && CheckDuplicate[0] != "")
                                    {
                                        FinalPeopleComment = (string.IsNullOrEmpty(CheckDuplicate[0]) ? XHelper.DisplayNameDefault : CheckDuplicate[0]) + " đã bình luận bài viết của bạn";
                                    }
                                    else if(CheckDuplicate.Count == 2)
                                    {
                                        FinalPeopleComment = (string.IsNullOrEmpty(CheckDuplicate[0]) ? XHelper.DisplayNameDefault : CheckDuplicate[0]) + " và " + (string.IsNullOrEmpty(CheckDuplicate[1]) ? XHelper.DisplayNameDefault : CheckDuplicate[1]) + " đã bình luận về bài viết của bạn";
                                    }
                                    else
                                    {
                                        FinalPeopleComment = (string.IsNullOrEmpty(CheckDuplicate[0]) ? XHelper.DisplayNameDefault : CheckDuplicate[0]) + ", " + (string.IsNullOrEmpty(CheckDuplicate[1]) ? XHelper.DisplayNameDefault : CheckDuplicate[1]) + " và " + (CheckDuplicate.Count - 2) + " người khác đã bình luận về bài viết của bạn";
                                    }

                                    ListFeed.Add(new ZFeedSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["FEED_ID"]),
                                        ChannelFeedID = Convert.ToString(dr["CHANNEL_FEED_ID"]),
                                        Content = Convert.ToString(dr["FEED_CONTENT"]),
                                        Avatar = string.IsNullOrEmpty(Convert.ToString(dr["AVATAR"])) ? XHelper.UrlAvatarDefault : Convert.ToString(dr["AVATAR"]),
                                        CreatedBy = string.IsNullOrEmpty(Convert.ToString(dr["CREATED_BY"])) ? XHelper.DisplayNameDefault : Convert.ToString(dr["CREATED_BY"]),
                                        PeopleComment = FinalPeopleComment,
                                        NumberOfComment = Convert.ToInt32(dr["NUMBER_OF_COMMENT"]),
                                        FromWhom = Convert.ToInt32(dr["FROM_WHOM"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        Day = DateTime.ParseExact(Convert.ToString(dr["CREATED_DATE"]), "dd/MM/yyyy HH:mm:ss", null).Day.ToString(),
                                        Month = DateTime.ParseExact(Convert.ToString(dr["CREATED_DATE"]), "dd/MM/yyyy HH:mm:ss", null).Month.ToString(),
                                    });
                                }
                            }
                            if (ListFeed.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(ListFeed);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FeedController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddSyncFeed")]
        public ZResponse AddSyncFeed(List<ZFeedAddSyncRequest> req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                var ConvertXML = req.ToXML();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SYNC_FEED_PKG.ADD_SYNC_DATA", new object[]
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FeedController.AddSyncFeed" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("DeleteFeed")]
        public ZResponse DeleteComment(ZFeedDeleteRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FEED_PKG.DELETE_FEED", new object[]
                {
                    0,
                    req.PageID,
                    req.Channel,
                    req.FeedID,
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
                            res.Message = "Xoá bài đăng thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CommentController.DeleteFeed" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}