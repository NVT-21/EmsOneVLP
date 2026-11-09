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
    [RoutePrefix("ZaloMessage")]
    public class ZaloMessageController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZZaloMessageSearchListRequest req)
        {
            int result = -1;
            List<ZZaloMessageSearchListResponse> oList = new List<ZZaloMessageSearchListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_MESSAGE_PKG.SEARCH_LIST_MESSAGE", new object[]
                {
                    req.OfficialAccountID,
                    req.UserID,
                    req.LimitRow,
                    req.CreatedDate,
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
                                    oList.Add(new ZZaloMessageSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        DisplayName = Convert.ToString(dr["DISPLAY_NAME"]) == "" ? "Ẩn danh" : Convert.ToString(dr["DISPLAY_NAME"]),
                                        Avatar = Convert.ToString(dr["AVATAR"]) == "" ? "http://124.158.6.19:1204/Assets/Images/image-default/avatar_default.jpg" : Convert.ToString(dr["AVATAR"]),
                                        MessageID = Convert.ToString(dr["MESSAGE_ID"]),
                                        MessageText = Convert.ToString(dr["MESSAGE_TEXT"]),
                                        StickerID = Convert.ToString(dr["STICKER_ID"]),
                                        LinkURL = Convert.ToString(dr["LINK_URL"]),
                                        Thumbnail = Convert.ToString(dr["THUMBNAIL"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        Latitude = Convert.ToString(dr["LATITUDE"]),
                                        Longitude = Convert.ToString(dr["LONGITUDE"]),
                                        MessageType = Convert.ToString(dr["MESSAGE_TYPE"]),
                                        //IsStatusChar = Convert.ToString(dr["IS_STATUS"]),
                                        //IsStatusName = XData.GetNameZaloMessageStatus(Convert.ToString(dr["IS_STATUS"])),
                                        FromWhom = Convert.ToInt32(dr["FROM_WHOM"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        AmndDate = Convert.ToString(dr["AMND_DATE"]),
                                    });
                                }
                            }
                            if (oList.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(oList);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloMessageController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UserSendMessageToOA")]
        public ZResponse UserSendMessageToOA(ZZaloUserSendMessageToOARequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_MESSAGE_PKG.USER_SEND_MESSAGE_TO_OA", new object[]
                {
                    0,
                    req.app_id,
                    req.user_id_by_app,
                    req.sender.id,
                    req.recipient.id,
                    req.message.msg_id,
                    req.message.text,
                    req.message.attachments != null ? req.message.attachments[0].payload.id : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.url : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.thumbnail : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.description : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.coordinates != null ? req.message.attachments[0].payload.coordinates.latitude : "" : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.coordinates != null ? req.message.attachments[0].payload.coordinates.longitude : "" : "",
                    req.message.react_icon,
                    req.message.attachments != null ? req.message.attachments[0].type : "",
                    XData.ConvertLongTypeDateTime(long.Parse(req.timestamp))
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
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
                                    res.Message = "Lỗi thêm tin nhắn"; 
                                    res.Data = req.event_name;
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lỗi thả biểu tượng cảm xúc lên tin nhắn";
                                    res.Data = req.event_name;
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lỗi thêm chi tiết tin nhắn";
                                    res.Data = req.event_name;
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                            XHelper.LogToFile(LogFileType.MESSAGE, "ZaloMessageController.UserSendMessageToOA: req: " + JsonConvert.SerializeObject(req) + " | res: " + JsonConvert.SerializeObject(res));
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloMessageController.UserSendMessageToOA: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UserReceivedMessage")]
        public ZResponse UserReceivedMessage(ZZaloUserReceivedMessageRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_MESSAGE_PKG.USER_RECEIVED_MESSAGE", new object[]
                {
                   0,
                   req.message.msg_id
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloMessageController.UserReceivedMessage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UserSeenMessage")]
        public ZResponse UserSeenMessage(ZZaloUserSeenMessageRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                var str_message_id = string.Join(",", req.message.msg_ids);
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_MESSAGE_PKG.USER_SEEN_MESSAGE", new object[]
                {
                    0,
                    str_message_id
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloMessageController.UserSeenMessage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("OASendTextToUser")]
        public ZResponse OASendTextToUser(ZZaloOASendTextToUserRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_MESSAGE_PKG.OA_SEND_MESSAGE_TO_USER", new object[]
                {
                    0,
                    req.app_id,
                    req.user_id_by_app,
                    req.sender.id,
                    req.recipient.id,
                    req.message.msg_id,
                    req.message.text,
                    req.message.attachments != null ? req.message.attachments[0].payload.id : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.url : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.thumbnail : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.description : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.coordinates != null ? req.message.attachments[0].payload.coordinates.latitude : "" : "",
                    req.message.attachments != null ? req.message.attachments[0].payload.coordinates != null ? req.message.attachments[0].payload.coordinates.longitude : "" : "",
                    req.message.react_icon,
                    req.message.attachments != null ? req.message.attachments[0].type : "",
                    XData.ConvertLongTypeDateTime(long.Parse(req.timestamp))
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
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
                                    res.Message = "Lỗi thêm tin nhắn";
                                    res.Data = req.event_name;
                                    XHelper.LogToFile(LogFileType.MESSAGE, "ZaloMessageController.OASendTextToUser: req: " + JsonConvert.SerializeObject(req) + " | res: " + JsonConvert.SerializeObject(res));
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lỗi thả biểu tượng cảm xúc lên tin nhắn";
                                    res.Data = req.event_name;
                                    XHelper.LogToFile(LogFileType.MESSAGE, "ZaloMessageController.OASendTextToUser: req: " + JsonConvert.SerializeObject(req) + " | res: " + JsonConvert.SerializeObject(res));
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lỗi thêm chi tiết tin nhắn";
                                    res.Data = req.event_name;
                                    XHelper.LogToFile(LogFileType.MESSAGE, "ZaloMessageController.OASendTextToUser: req: " + JsonConvert.SerializeObject(req) + " | res: " + JsonConvert.SerializeObject(res));
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloMessageController.OASendTextToUser: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}