using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
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
    [RoutePrefix("Message")]
    public class MessageController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZMessageSearchListRequest req)
        {
            int result = -1;
            List<ZMessageSearchListResponse> oList = new List<ZMessageSearchListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MESSAGES_PKG.SEARCH_LIST_MESSAGE", new object[]
                {
                    req.ConversionID,
                    req.Channel,
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
                                    oList.Add(new ZMessageSearchListResponse()
                                    {
                                        MessageID = Convert.ToInt32(dr["ID"]),
                                        DisplayName = string.IsNullOrEmpty(Convert.ToString(dr["DISPLAY_NAME"])) ? XHelper.DisplayNameDefault : Convert.ToString(dr["DISPLAY_NAME"]),
                                        Avatar = string.IsNullOrEmpty(Convert.ToString(dr["AVATAR"])) ? XHelper.UrlAvatarDefault : Convert.ToString(dr["AVATAR"]),
                                        ChannelMessageID = Convert.ToString(dr["CHANNEL_MESSAGE_ID"]),
                                        MessageText = Convert.ToString(dr["MESSAGE_TEXT"]),
                                        StickerID = Convert.ToString(dr["STICKER_ID"]),
                                        LinkURL = Convert.ToString(dr["LINK_URL"]),
                                        Thumbnail = Convert.ToString(dr["THUMBNAIL"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        Latitude = Convert.ToString(dr["LATITUDE"]),
                                        Longitude = Convert.ToString(dr["LONGITUDE"]),
                                        MessageType = Convert.ToString(dr["MESSAGE_TYPE"]),
                                        Status = new XStatusModel()
                                        {
                                            Code = Convert.ToString(dr["STATUS"]),
                                            Name = XData.GetNameMessageStatus(Convert.ToString(dr["STATUS"]))
                                        },
                                        FromWhom = Convert.ToInt32(dr["FROM_WHOM"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "MessageController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #region FACEBOOK

        [Route("AddMessageFacebook")]
        public ZResponse AddMessageFacebook(ZFacebookAddMessageRequest req)
        {

            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                var FromWhom = 0;
                if (req.Entry[0].ID == req.Entry[0].Messaging[0].Sender.ID)
                {
                    //Page gửi cho User
                    FromWhom = 1;
                }
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MESSAGES_PKG.FAB_ADD_MESSAGE", new object[]
                {
                    0,
                    req.Entry[0].Messaging[0].Message.MID,
                    req.Entry[0].Messaging[0].Sender.ID,
                    req.Entry[0].Messaging[0].Recipient.ID,
                    req.Entry[0].Messaging[0].Message.Text,
                    req.Entry[0].Messaging[0].Message.Attachments != null ? req.Entry[0].Messaging[0].Message.Attachments[0].Payload.Url : "",
                    req.Entry[0].Messaging[0].Message.Attachments != null ? req.Entry[0].Messaging[0].Message.Attachments[0].Payload.Title : "",
                    req.Entry[0].Messaging[0].Message.Attachments != null ? req.Entry[0].Messaging[0].Message.Attachments[0].Type : "TEXT",
                    FromWhom,
                    XData.ConvertLongTypeDateTime(long.Parse(req.Entry[0].Time))
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
                                    res.Message = "Lỗi thêm cuộc hội thoại";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lỗi thêm tin nhắn";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lỗi thêm chi tiết tin nhắn";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Tin nhắn đã tồn tại";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("MessageController.AddMessageFacebook: request={0} || response={1}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(res)));
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "MessageController.AddMessageFacebook: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("RecievedMessageFacebook")]
        public ZResponse RecievedMessageFacebook(ZFacebookReceivedMessageRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MESSAGES_PKG.FAB_RECEIVED_MESSAGE", new object[]
                {
                    0,
                    req.Entry[0].Messaging[0].Sender.ID, //user_id
                    req.Entry[0].Messaging[0].Recipient.ID, //page_id,
                    XData.ConvertLongTypeDateTime(long.Parse(req.Entry[0].Messaging[0].Delivery.Watermark))
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
                            XHelper.LogToFile(LogFileType.MESSAGE, "MessageController.RecievedMessageFacebook: req: " + JsonConvert.SerializeObject(req) + " | res: " + JsonConvert.SerializeObject(res));
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "MessageController.RecievedMessageFacebook: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SeenMessageFacebook")]
        public ZResponse SeenMessageFacebook(ZFacebookSeenMessageRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MESSAGES_PKG.FAB_SEEN_MESSAGE", new object[]
                {
                    0,
                    req.Entry[0].Messaging[0].Sender.ID, //user_id
                    req.Entry[0].Messaging[0].Recipient.ID, //page_id,
                    XData.ConvertLongTypeDateTime(long.Parse(req.Entry[0].Messaging[0].Read.Watermark))
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
                            XHelper.LogToFile(LogFileType.MESSAGE, "MessageController.SeenMessageFacebook: req: " + JsonConvert.SerializeObject(req) + " | res: " + JsonConvert.SerializeObject(res));
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "MessageController.SeenMessageFacebook: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddSyncMessage")]
        public ZResponse AddSyncMessage(List<ZMessageAddSyncRequest> req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                var _parameters = new OracleParameter[]
                {
                    new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32},

                    new OracleParameter() { ParameterName = "P_DATA_INPUT", Direction = ParameterDirection.Input, Value = req.ToXML(), OracleDbType = OracleDbType.Clob}
                };

                XHelper.ExecuteNonQueryV2(XHelper.Schema + "EMS_SYNC_MESSAGE_PKG.ADD_SYNC_DATA", _parameters, CommandType.StoredProcedure);

                result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters[00].Value).Value;
                if (result > 0)
                {
                    res.Code = "00";
                    res.Message = "Cập nhật thành công.";
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "MessageController.AddSyncMessage" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region ZALO

        [Route("AddMessageZalo")]
        public ZResponse AddMessageZalo(ZZaloAddMessageRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MESSAGES_PKG.ZAL_ADD_MESSAGE", new object[]
                {
                    0,
                    req.message.msg_id,
                    req.sender.id,
                    req.recipient.id,
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
                                    res.Message = "Lỗi thêm cuộc hội thoại";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lỗi thêm tin nhắn";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lỗi thêm chi tiết tin nhắn";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Lỗi thêm biểu tượng cảm xúc";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Tin nhắn của BOT nên không insert";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("MessageController.AddMessageZalo: request={0} || response={1}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(res)));
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "MessageController.AddMessageZalo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("RecievedMessageZalo")]
        public ZResponse RecievedMessageZalo(ZZaloReceivedMessageRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MESSAGES_PKG.ZAL_RECEIVED_MESSAGE", new object[]
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloMessageController.RecievedMessageZalo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SeenMessageZalo")]
        public ZResponse SeenMessageZalo(ZZaloSeenMessageRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                var str_message_id = string.Join(",", req.message.msg_ids);
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MESSAGES_PKG.ZAL_SEEN_MESSAGE", new object[]
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloMessageController.SeenMessageZalo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #endregion
    }
}