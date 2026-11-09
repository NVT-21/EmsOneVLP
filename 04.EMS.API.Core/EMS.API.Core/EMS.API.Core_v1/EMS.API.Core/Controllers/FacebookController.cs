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
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Facebook")]
    public class FacebookController : ApiController
    {

        #region Feed
        [Route("ListFeed")]
        public ZResponse ListFeed(ZFacebookSearchFeedRequest req)
        {
            int result = -1;
            List<XFacebookFeedModel> ListFeed = new List<XFacebookFeedModel>();
            ZFacebookSearchFeedResponse Item = null;
            ZResponse res = new ZResponse();
            try
            {
                //if (req.FromDate == null)
                //{
                //    req.FromDate = DateTime.Now.ToString("01/MM/yyyy");
                //}
                //if (req.ToDate == null)
                //{
                //    req.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
                //}
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.LIST_CONVERSATION", new object[]
                {
                    req.PageID,
                    req.ConversationType,
                    req.ConversationSeen,
                    req.ConversationAssign,
                    req.Day,
                    req.Week,
                    req.Month,
                    req.FromDate,
                    req.ToDate,
                    req.EmployeeID,
                    req.ShopID,
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
                                    ListFeed.Add(new XFacebookFeedModel()
                                    {
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        ConversationID = Convert.ToString(dr["CONVERSATION_ID"]),
                                        SenderID = Convert.ToString(dr["SENDER_ID"]),
                                        SenderName = Convert.ToString(dr["ACCOUNT_NAME"]),
                                        SenderEmail = Convert.ToString(dr["ACCOUNT_EMAIL"]),
                                        Snippet = Convert.ToString(dr["SNIPPET"]),
                                        UpdateTime = Convert.ToString(dr["UPDATED_TIME"]),
                                        ConversationSeen = Convert.ToInt32(dr["CONVERSATION_SEEN"]),
                                        ConversationType = Convert.ToInt32(dr["CONVERSATION_TYPE"]),
                                        PictureLink = Convert.ToString(dr["ACCOUNT_PICTURE_LINK"]),
                                        AssignID = Convert.ToString(dr["ASSIGN_ID"]),
                                        NameAssign = Convert.ToString(dr["NAME_ASSIGN"]),
                                        EmployeeName = Convert.ToString(dr["FULL_NAME"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        TotalComment = Convert.ToInt32(dr["TOTAL_CON_DETAIL"]),
                                        TotalCommentNotSeen = Convert.ToInt32(dr["TOTAL_CON_DETAIL_NOT_SEEN"])
                                    });
                                }
                            }
                            if(ListFeed.Count > 0)
                            {
                                Item = new ZFacebookSearchFeedResponse()
                                {
                                    ListFeed = ListFeed,
                                    TotalFeed = ListFeed.Count(),
                                    TodayFeed = ListFeed.Where(m => m.UpdateTime.Substring(0, 10) == DateTime.Now.ToString("dd/MM/yyyy")).Count(),
                                    MonthFeed = ListFeed.Where(m => (m.UpdateTime.Substring(0, 10)).Substring(3, 7) == DateTime.Now.ToString("dd/MM/yyyy").Substring(3, 7)).Count()
                                };
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(Item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -99:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ListFeed" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Conversation Detail
        [Route("ListComment")]
        public ZResponse ListComment(ZFacebookSearchCommentRequest req)
        {
            int result = -1;
            List<XFacebookCommentModel> ListComment = new List<XFacebookCommentModel>();
            ZFacebookSearchCommentResponse Item = new ZFacebookSearchCommentResponse();
            ZResponse res = new ZResponse();
            string type = req.ConversationID.Substring(0, 2);
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.LIST_CONVERSATION_DETAIL", new object[]
                {
                    req.ConversationID,
                    req.Day,
                    req.Week,
                    req.Month,
                    req.FromDate,
                    req.ToDate,
                    req.Email,
                    req.Phone,
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
                                    if (type != "t_")
                                    {
                                        ListComment.Add(new XFacebookCommentModel()
                                        {
                                            ConversationID = Convert.ToString(dr["CONVERSATION_ID"]),
                                            ConversationDetailID = Convert.ToString(dr["CONVERSATION_DETAIL_ID"]),
                                            FromID = Convert.ToString(dr["FROM_ID"]),
                                            FromName = Convert.ToString(dr["ACCOUNT_NAME"]),
                                            FromEmail = Convert.ToString(dr["ACCOUNT_EMAIL"]),
                                            Message = Convert.ToString(dr["MESSAGE"]),
                                            CreateTime = Convert.ToString(dr["CREATED_TIME"]),
                                            FromPictureLink = Convert.ToString(dr["PICTURE"]),
                                            ConversationDetailSeen = Convert.ToInt32(dr["CONVERSATION_DETAIL_SEEN"]),
                                            IsLike = Convert.ToInt32(dr["IS_LIKE"]),
                                            IsHidden = Convert.ToInt32(dr["IS_HIDDEN"]),
                                            IsFeedback = Convert.ToInt32(dr["IS_FEEDBACK"])
                                        });
                                    }
                                }
                            }
                            if (ListComment.Count > 0)
                            {
                                Item = new ZFacebookSearchCommentResponse()
                                {
                                    ListComment = ListComment,
                                    totalComment = ListComment.Count(),
                                    totalCommentSeen = ListComment.Where(m => m.ConversationDetailSeen == 1).Count(),
                                    totalCommentNotSeen = ListComment.Where(m => m.ConversationDetailSeen == 0).Count()
                                };
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(Item);
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
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ListComment" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("AddAssignToConversation")]
        public ZResponse AddAssignToConversation(ZFacebookConverAssignToConversationRequest req)
        {

            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.ADD_ASSIGN_TO_CONVERSATION", new object[]
                {
                    req.CreatedBy,
                    req.NameAssign,
                    req.PageID,
                    req.AssignID,
                    req.ConversationID,
                    req.ShopID,
                    0,
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[6].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Thông tin đã tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.AddAssignToConversation" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("DeleteAssign")]
        public ZResponse DeleteAssign(ZFacebookDeleteAssignModelRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.DELETE_ASSIGN", new object[]
                {
                    0,
                    req.ConversationID,
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
                            res.Message = "Xoá thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.DeleteAssign: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Seen - Not Seen
        [Route("MaskAsViewed")]
        public ZResponse MaskAsViewed(ZFacebookMaskAsViewedRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.MARK_AS_VIEWED", new object[]
                {
                    0,
                    req.Type,
                    req.ConversationID,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            if (result == 1)
                            {
                                res.Code = "00";
                                res.Message = "Đã đánh dấu đã đọc thành công.";
                            }
                            else
                            {
                                res.Code = "00";
                                res.Message = "Đã đánh dấu đã đọc tất cả thành công.";
                            }

                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.MaskAsViewed: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("MaskAsRead")]
        public ZResponse MaskAsRead(ZFacebookMaskAsReadRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.MARK_AS_READ", new object[]
                {
                    0,
                    req.Type,
                    req.ConversationID,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            if (result == 1)
                            {
                                res.Code = "00";
                                res.Message = "Đã đánh dấu chưa đọc thành công.";
                            }
                            else
                            {
                                res.Code = "00";
                                res.Message = "Đã đánh dấu chưa đọc tất cả thành công.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.MaskAsRead: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("MaskAsViewedDetail")]
        public ZResponse MaskAsViewedDetail(ZFacebookMaskAsViewedRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.MARK_AS_VIEWED_DETAIL", new object[]
                {
                    0,
                    req.Type,
                    req.ConversationID,
                    req.ConversationDetailID,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            if (result == 1)
                            {
                                res.Code = "00";
                                res.Message = "Đã đánh dấu đã đọc thành công.";
                            }
                            else
                            {
                                res.Code = "00";
                                res.Message = "Đã đánh dấu đã đọc tất cả thành công.";
                            }

                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.MaskAsViewedDetail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("MaskAsReadDetail")]
        public ZResponse MaskAsReadDetail(ZFacebookMaskAsReadRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.MARK_AS_READ_DETAIL", new object[]
                {
                    0,
                    req.Type,
                    req.ConversationID,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            if (result == 1)
                            {
                                res.Code = "00";
                                res.Message = "Đã đánh dấu chưa đọc thành công.";
                            }
                            else
                            {
                                res.Code = "00";
                                res.Message = "Đã đánh dấu chưa đọc tất cả thành công.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.MaskAsReadDetail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region CheckCustomerInfo
        [Route("CheckCustomerInfo")]
        public ZResponse CheckCustomerInfo(ZFacebookCheckCustomerInfoRequestData req)
        {
            int result = -1;
            ZFacebookCheckCustomerInfoResponse item = new ZFacebookCheckCustomerInfoResponse();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.CHECK_CUSTOMER_INFO", new object[]
                {
                    req.Email,
                    req.Phone,
                    req.AccountChannelID,
                    req.ShopID,
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
                                    item = new ZFacebookCheckCustomerInfoResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        BirthDay = Convert.ToString(dr["BIRTH_DAY"]),
                                        TaxCode = Convert.ToString(dr["TAX_CODE"]),
                                        Channel = Convert.ToString(dr["CHANNEL"]),
                                        Note = Convert.ToString(dr["NOTE"]),
                                        CustomerCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                        BlackList = Convert.ToString(dr["BLACK_LIST"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        Sticker = Convert.ToString(dr["STICKER"])
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
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
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.SearchConversation" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Split Conversation Employee
        [Route("SplitConversation")]
        public ZResponse SplitConversation(ZFacebookSplitConversationRequest req)
        {
            ZResponse res = new ZResponse();
            int result = -1;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.SPLIT_CONVERSATION", new object[]
                {
                    0,
                    req.ConversationID,
                    req.EmployeeID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Phân chia hội thoại thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.SplitConversation: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            return res;
        }
        [Route("CountConversationSplit")]
        public ZResponse CountConversationSplit(ZFacebookCountConversationSplitRequest req)
        {
            ZResponse res = new ZResponse();
            int result = -1;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.COUNT_CONVERSATION", new object[]
                {
                    0,
                    req.PageID,
                    req.ShopID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Data = Convert.ToString(result);
                        }
                        else
                        {
                            res.Data = Convert.ToString(result);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.CountConversationSplit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            return res;
        }
        [Route("SplitConversationMulti")]
        public ZResponse SplitConversationMulti(ZFacebookSplitConversationRequest req)
        {
            ZResponse res = new ZResponse();
            int result = -1;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.SPLIT_CONVERSATION_MULTI", new object[]
                {
                    0,
                    req.EmployeeID,
                    req.PageID,
                    req.ShopID,
                    req.T
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Phân chia hội thoại thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lỗi chia hội thoại.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.SplitConversationMulti: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            return res;
        }
        #endregion

        #region AutoLike 
        [Route("PageAutoLike")]
        public ZResponse PageAutoLike(ZFacebookSearchPageAutoLikeRequest req)
        {
            int result = -1;
            ZFacebookPageAutoLikeResponse item = new ZFacebookPageAutoLikeResponse();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.PAGE_AUTOLIKE", new object[]
                {
                    req.ShopID,
                    req.PageID,
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
                                    item = new ZFacebookPageAutoLikeResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        IsLikeComment = Convert.ToString(dr["IS_LIKE_COMMENT"])
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.PageAutoLike" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ChangeStatusPageAutoLike")]
        public ZResponse ChangeStatusPageAutoLike(ZFacebookReactionPageAutoLikeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.CHANGE_STATUS_PAGE_AUTOLIKE", new object[]
                {
                    0,
                    req.ID,
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
                            res.Message = "Đã thay đổi trạng thái thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ChangeStatusPageAutoLike: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Auto Hidden
        [Route("PageAutoHidden")]
        public ZResponse PageAutoHidden(ZFacebookSearchPageAutoHiddenRequest req)
        {
            int result = -1;
            ZFacebookSearchPageAutoHiddenResponse item = new ZFacebookSearchPageAutoHiddenResponse();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.PAGE_AUTO_HIDDEN", new object[]
                {
                    req.ShopID,
                    req.PageID,
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
                                    item = new ZFacebookSearchPageAutoHiddenResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        IDHidden = Convert.ToInt32(dr["ID_HIDDEN_COMMENT"]),
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        IsHiddenComment = Convert.ToInt32(dr["IS_ALL"]),
                                        TextInComment = Convert.ToString(dr["TEXT_IN_COMMENT"]),
                                        IsEmail = Convert.ToInt32(dr["IS_EMAIL"]),
                                        IsPhoneNumber = Convert.ToInt32(dr["IS_PHONENUMBER"]),
                                        IsText = Convert.ToInt32(dr["IS_TEXT"]),
                                        AvailableDate = Convert.ToInt32(dr["AVAILABLE_DATE"])
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.PageAutoHidden" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ChangeStatusPageAutoHidden")]
        public ZResponse ChangeStatusPageAutoHidden(ZFacebookChangeStatusPageAutoHiddenRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.CHANGE_STATUS_AUTO_HIDDEN", new object[]
                {
                    0,
                    req.ID,
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
                            res.Message = "Đã thay đổi trạng thái thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ChangeStatusPageAutoHidden: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("GetPageAutoHiddenByID")]
        public ZResponse GetPageAutoHiddenByID(ZFacebookGetAutoHiddenByIDRequest req)
        {
            int result = -1;
            ZFacebookPageAutoHiddenByIDResponse item = new ZFacebookPageAutoHiddenByIDResponse();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.GET_PAGE_AUTO_HIDDEN_BY_ID", new object[]
                {
                    req.ID,
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
                                    item = new ZFacebookPageAutoHiddenByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        PageName = Convert.ToString(dr["PAGE_NAME"]),
                                        PageCategory = Convert.ToString(dr["PAGE_CATEGORY"]),
                                        PageAutoHiddenStatus = Convert.ToInt32(dr["PAGE_AUTO_HIDDEN_STATUS"]),
                                        TextInComment = Convert.ToString(dr["TEXT_IN_COMMENT"]),
                                        IsEmail = Convert.ToInt32(dr["IS_EMAIL"]),
                                        IsPhoneNumber = Convert.ToInt32(dr["IS_PHONENUMBER"]),
                                        IsText = Convert.ToInt32(dr["IS_TEXT"]),
                                        AvailableDate = Convert.ToInt32(dr["AVAILABLE_DATE"]),
                                        
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
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
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.GetPageAutoHiddenByID" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("EditPageAutoHidden")]
        public ZResponse EditPageAutoHidden(ZFacebookEditPageAutoHiddenRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.EDIT_PAGE_AUTO_HIDDEN", new object[]
                {
                    0,
                    req.ID,
                    req.IsAll,
                    req.Text,
                    req.IsEmail,
                    req.IsPhone,
                    req.IsText,
                    req.AvailableDate,
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
                            res.Message = "Đã cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.EditPageHidden: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region FB Working Time
        [Route("EditWorkingTime")]
        public ZResponse EditWorkingTime(ZFacebookEditWorkingTimeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_WORKING_PKG.EDIT_WORKING_TIME", new object[]
                {
                    0,
                    req.ID,
                    req.Monday,
                    req.Tuesday,
                    req.Wednesday,
                    req.Thursday,
                    req.Friday,
                    req.Saturday,
                    req.Sunday,
                    req.Note,
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
                            res.Message = "Đã cập nhật thành công.";




                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.EditWorkingTime: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("DeleteWorkingTime")]
        public ZResponse DeleteWorkingTime(ZFacebookDeleteWorkingTimeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_WORKING_PKG.DELETE_WORKING_TIME", new object[]
                {
                    0,
                    req.ID,
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
                            res.Message = "Đã xoá thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.DeleteWorkingTime: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ListWorkingTime")]
        public ZResponse ListWorkingTime(ZFacebookSearchWorkingTimeRequest req)
        {
            int result = -1;
            List<ZFacebookSearchWorkingTimeResponse> lst = new List<ZFacebookSearchWorkingTimeResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_WORKING_PKG.LIST_WORKING_TIME", new object[]
                {
                    req.ShopID,
                    req.PageID,
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
                                    lst.Add(new ZFacebookSearchWorkingTimeResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        PageName = Convert.ToString(dr["PAGE_NAME"]),
                                        PageCategory = Convert.ToString(dr["PAGE_CATEGORY"]),
                                        Monday = Convert.ToString(dr["MONDAY"]),
                                        Tuesday = Convert.ToString(dr["TUESDAY"]),
                                        Wednesday = Convert.ToString(dr["WEDNESDAY"]),
                                        Thursday = Convert.ToString(dr["THURSDAY"]),
                                        Friday = Convert.ToString(dr["FRIDAY"]),
                                        Saturday = Convert.ToString(dr["SATURDAY"]),
                                        Sunday = Convert.ToString(dr["SUNDAY"]),
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ListWorkingTime" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("GetWorkingTimeByID")]
        public ZResponse GetWorkingTimeByID(ZFacebookGetWorkingTimeByIDRequest req)
        {
            int result = -1;
            ZFacebookSearchWorkingTimeResponse item = new ZFacebookSearchWorkingTimeResponse();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_WORKING_PKG.GET_WORKING_TIME_BY_ID", new object[]
                {
                    req.ID,
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
                                    item = new ZFacebookSearchWorkingTimeResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Monday = Convert.ToString(dr["MONDAY"]),
                                        Tuesday = Convert.ToString(dr["TUESDAY"]),
                                        Wednesday = Convert.ToString(dr["WEDNESDAY"]),
                                        Thursday = Convert.ToString(dr["THURSDAY"]),
                                        Friday = Convert.ToString(dr["FRIDAY"]),
                                        Saturday = Convert.ToString(dr["SATURDAY"]),
                                        Sunday = Convert.ToString(dr["SUNDAY"]),
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
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
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.GetWorkingTimeByID" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region FB Working Time Detail
        [Route("AddWorkingTimeDetail")]
        public ZResponse AddWorkingTimeDetail(ZFacebookAddWorkingTimeDetailRequest req)
        {

            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_WORKING_PKG.ADD_NEW_WORKING_TIME_DETAIL", new object[]
                {
                    0,
                    req.CreatedBy,
                    req.IDWorkingTime,
                    req.Day,
                    req.StartTime,
                    req.EndTime,
                    "Y"
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới chi tiết thời gian làm việc thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Thông tin chi tiết thời gian làm việc đã tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.AddWorkingTimeDetail" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("EditWorkingTimeDetail")]
        public ZResponse EditWorkingTimeDetail(ZFacebookEditWorkingTimeDetailRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_WORKING_PKG.EDIT_WORKING_TIME_DETAIL", new object[]
                {
                    0,
                    req.ID,
                    req.StartTime,
                    req.EndTime,
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
                            res.Message = "Đã cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.EditWorkingTimeDetail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("DeleteWorkingTimeDetail")]
        public ZResponse DeleteWorkingTimeDetail(ZFacebookDeleteWorkingTimeDetailRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_WORKING_PKG.DELETE_WORKING_TIME_DETAIL", new object[]
                {
                    0,
                    req.ID,
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
                            res.Message = "Đã xoá thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.DeleteWorkingTimeDetail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("WorkingTimeDetail")]
        public ZResponse WorkingTimeDetail(ZFacebookSearchWorkingTimeDetailRequest req)
        {
            int result = -1;
            List<ZFacebookSearchWorkingTimeDetailResponse> lst = new List<ZFacebookSearchWorkingTimeDetailResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_WORKING_PKG.GET_WORKING_TIME_DETAIL", new object[]
                {
                    req.ID,
                    req.Day,
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
                                    lst.Add(new ZFacebookSearchWorkingTimeDetailResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        WorkingTimeID = Convert.ToInt32(dr["ID_WORKING_TIME"]),
                                        Day = Convert.ToString(dr["DAY"]),
                                        StartTime = Convert.ToString(dr["FROM_TIME"]),
                                        EndTime = Convert.ToString(dr["END_TIME"])
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
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
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.WorkingTimeDetail" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region SendMassMessages
        [Route("AddMessagesCampaign")]
        public ZResponse AddMessagesCampaign(ZFacebookAddMessageCampaignRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.ADD_MESSAGES_CAMPAIGN", new object[]
                {
                    0,
                    req.CreatedBy,
                    req.NameCampaign,
                    req.ShopID,
                    req.PageID,
                    req.Message,
                    req.SendDate,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới chiến dịch thành công.";
                            res.Data = Convert.ToString(dbCommand.Parameters["P_ID_CAMPAIGN"].Value);
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.AddMessagesCampaign" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ListMessagesCampaign")]
        public ZResponse ListMessagesCampaign(ZFacebookListMessageCampaignRequest req)
        {
            int result = -1;
            List<ZFacebookListMessageCampaignResponse> lst = new List<ZFacebookListMessageCampaignResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.LIST_MESSAGES_CAMPAIGN", new object[]
                {
                    req.ShopID,
                    req.PageID,
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
                                    lst.Add(new ZFacebookListMessageCampaignResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        NameCampaign = Convert.ToString(dr["NAME_CAMPAIGN"]),
                                        Message = Convert.ToString(dr["MESSAGE"]),
                                        SendDate = Convert.ToString(dr["SEND_DATE"]),
                                        EmployeeName = Convert.ToString(dr["EMP_NAME"])
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
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
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ListMessagesCampaign" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("AddSendMassMessages")]
        public ZResponse AddSendMassMessages(ZFacebookAddSendMassMessagesRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.ADD_SEND_MASS_MESSAGES", new object[]
                {
                    0,
                    req.CreatedBy,
                    req.CampaignID,
                    req.FacebookID,
                    req.Status
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới chi tiết gửi tin nhắn thành công.";
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.AddSendMassMessages" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ListSendMassMessages")]
        public ZResponse ListSendMassMessages(ZFacebookSearchSendMassMessagesRequest req)
        {
            int result = -1;
            List<ZFacebookSearchSendMassMessagesResponse> lst = new List<ZFacebookSearchSendMassMessagesResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.LIST_SEND_MASS_MESSAGES", new object[]
                {
                    req.ShopID,
                    req.PageID,
                    req.CampaignID,
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
                                    lst.Add(new ZFacebookSearchSendMassMessagesResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        FacebookID = Convert.ToString(dr["FACEBOOK_ID"]),
                                        Message = Convert.ToString(dr["MESSAGE"]),
                                        SendDate = Convert.ToString(dr["SEND_DATE"]),
                                        Status = Convert.ToInt32(dr["STATUS"]),
                                        Received = Convert.ToInt32(dr["IS_RECEIVED"]),
                                        EmpName = Convert.ToString(dr["EMP_NAME"]),
                                        CustomerName = Convert.ToString(dr["CUSTOMER_NAME"]),
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
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
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ListSendMassMessages" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region AddPageFeedbackAuto
        [Route("AddPageFeedbackAuto")]
        public ZResponse AddPageFeedbackAuto(ZFacebookAddFeedbackAutoRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.ADD_PAGE_FEEDBACK_SCRIPT", new object[]
                {
                    0,
                    req.CreatedBy,
                    req.PageID,
                    req.ShopID,
                    req.ScriptType,
                    req.ScriptName,
                    req.Message,
                    req.ResponseTime,
                    req.ExecutionTimePeriod,
                    req.FromTime,
                    req.EndTime
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới kịch bản thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Kịch bản đã tồn tại, Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Nội dung phản hồi không được lớn hơn 2000 ký tự.";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.AddPageFeedbackAuto" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ListFeedbackAuto")]
        public ZResponse ListFeedbackAuto(ZFacebookSearchFeedbackAutoRequest req)
        {
            int result = -1;
            List<ZFacebookSearchFeedbackAutoResponse> lst = new List<ZFacebookSearchFeedbackAutoResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.LIST_FEEDBACK_SCRIPT", new object[]
                {
                    req.ShopID,
                    req.PageID,
                    req.ScriptType,
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
                                    lst.Add(new ZFacebookSearchFeedbackAutoResponse()
                                    {
                                        PageName = Convert.ToString(dr["PAGE_NAME"]),
                                        ID = Convert.ToInt32(dr["ID"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        ScriptType = Convert.ToInt32(dr["SCRIPT_TYPE"]),
                                        ScriptName = Convert.ToString(dr["SCRIPT_NAME"]),
                                        Message = Convert.ToString(dr["MESSAGE"]),
                                        ResponseTime = Convert.ToString(dr["RESPONSE_TIME"]),
                                        ExecutionTimePeriod = Convert.ToInt32(dr["EXECUTION_TIME_PERIOD"]),
                                        FromTime = Convert.ToString(dr["FROM_TIME"]),
                                        EndTime = Convert.ToString(dr["END_TIME"]),
                                        Status = Convert.ToInt32(dr["STATUS"])
                                    });
                                }
                            }
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = JsonConvert.SerializeObject(lst);
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -99:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ListFeedbackAuto" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ChangeFeedbackAuto")]
        public ZResponse ChangeFeedbackAuto(ZFacebookUpdateFeedbackAutoRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.CHANGE_FEEDBACK_SCRIPT", new object[]
                {
                    0,
                    req.ID,
                    req.Status,
                    req.ShopID,
                    req.PageID,
                    req.ScriptType,
                    req.UpdatedBy,
                    req.ExecutionTimePeriod
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Đã thay đổi trạng thái thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Bạn đã chọn thời gian thực thi toàn thời gian trước đó, Vui lòng tắt thời gian thực thi toàn thời gian và thử lại.";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ChangeFeedbackAuto: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("DeleteFeedbackAuto")]
        public ZResponse DeleteFeedbackAuto(ZFacebookUpdateFeedbackAutoRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_AUTO_PKG.DELETE_FEEDBACK_SCRIPT", new object[]
                {
                    0,
                    req.ID,
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
                            res.Message = "Bạn đã xóa kịch bản thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.DeleteFeedbackAuto: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Add XML Data Coversation - Conversation Detail
        [HttpPost]
        [Route("AddXMLConversation")]
        public ZResponse AddXMLConversation(ZFacebookAddConversationXMLRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                var _parameters = new OracleParameter[]
                {
                    new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32},

                    new OracleParameter() { ParameterName = "P_DATA_INPUT", Direction = ParameterDirection.Input, Value = req.DataInput, OracleDbType = OracleDbType.Clob}
                };
                XHelperOMDA.ExecuteNonQuery(XHelper.Schema +
                    "EMS_FACEBOOK_SYNC_PKG.ADD_XML_CONVERSATION", _parameters, CommandType.StoredProcedure);
                result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters[00].Value).Value;
                if (result > 0)
                {
                    res.Code = "00";
                    res.Message = "Thành công.";
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.AddXMLConversation: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public class ConversationDetailModel
        {
            public string CreateBy { get; set; }
            public string ConversationID { get; set; }
            public string ConversationDetailID { get; set; }
            public string FromID { get; set; }
            public string FromName { get; set; }
            public string FromEmail { get; set; }
            public string PictureLink { get; set; }
            public string Message { get; set; }
            public string CreateTime { get; set; }
            public string FromPictureLink { get; set; }
            public string EmailMessage { get; set; }
            public string PhoneMessage { get; set; }
        }
        [HttpPost]
        [Route("AddXMLConversationDetail")]
        public ZResponse AddXMLConversationDetail(ZFacebookAddConversationDetailXMLRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ConversationDetailModel> temp = new List<ConversationDetailModel>();
            foreach(var item in req.ListData)
            {
                temp.Add(new ConversationDetailModel()
                {
                    CreateBy = item.CreateBy,
                    ConversationID = item.ConversationID,
                    ConversationDetailID = item.ConversationDetailID,
                    FromID = item.FromID,
                    FromName = item.FromName,
                    FromEmail = item.FromEmail,
                    Message = item.Message,
                    CreateTime = item.CreateTime,
                    FromPictureLink = item.FromPictureLink
                });
            }
            //var _values = JsonConvert.DeserializeObject<List<ConversationDetailModel>>(( req.ListData));
            try
            {
                var _parameters = new OracleParameter[]
                {
                    new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32},

                    new OracleParameter() { ParameterName = "P_DATA_INPUT", Direction = ParameterDirection.Input, Value = temp.ToXML(), OracleDbType = OracleDbType.Clob}
                };
                XHelperOMDA.ExecuteNonQuery(XHelper.Schema + "EMS_FACEBOOK_SYNC_PKG.ADD_XML_CONVERSATION_DETAIL", _parameters, CommandType.StoredProcedure);
                result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters[00].Value).Value;
                if (result > 0)
                {
                    res.Code = "00";
                    res.Message = "Thành công.";
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.AddXMLConversationDetail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region AddConnectPage_V2
        [Route("ResetConnectPage")]
        public ZResponse ResetConnectPage(ZFacebookResetConnectPageRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.RESET_CONNECT_PAGE", new object[]
                {
                    0,
                    req.ShopID,
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
                            res.Message = "Đã thay đổi trạng thái thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.ResetConnectPage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("AddConnectPage_V2")]
        public ZResponse AddConnectPage_V2(ZFacebookAddConnectRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_PKG.ADD_CONNECT_PAGE_V2", new object[]
                {
                    0,
                    req.FBID,
                    req.PageID,
                    req.PageName,
                    req.PageCategory,
                    req.ShopID,
                    req.PageAccessToken
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[5].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Cập nhật thành công.";
                                    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FacebookController.AddConnectPage_V2" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

    }
}
