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
using EMS.API.Core.Repositories;
namespace EMS.API.Core.Controllers
{
    [RoutePrefix("AutoSendMessage")]
    public class AutoSendMessageController : ApiController
    {
        #region FRONT_END
        [Route("SearchList")]
        public ZResponse SearchList(ZAutoSendMessageSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZAutoSendMessageSearchListResponse> oList = new List<ZAutoSendMessageSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_AUTO_SEND_MESSAGE_PKG.SEARCH_LIST", new object[]
                {
                    req.PageID,
                    req.ShopID,
                    req.Status,
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
                                    oList.Add(new ZAutoSendMessageSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        MessageContent = Convert.ToString(dr["MESSAGE_CONTENT"]),
                                        MessageType = Convert.ToString(dr["MESSAGE_TYPE"]),
                                        SendTime = Convert.ToString(dr["SEND_TIME"]),
                                        Status = XData.GetNameZaloAutoSendMessageStatus(Convert.ToString(dr["STATUS"])),
                                        SendStatus = Convert.ToInt32(dr["SEND_STATUS"]),
                                        RecieveStatus = Convert.ToInt32(dr["RECIEVE_STATUS"]),
                                        SeenStatus = Convert.ToInt32(dr["SEEN_STATUS"]),
                                        CreatedName = Convert.ToString(dr["CREATED_NAME"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "AutoSendMessageController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddAutoSendMessage")]
        public ZResponse AddAutoSendMessage(ZAutoSendMessageAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_AUTO_SEND_MESSAGE_PKG.ADD_AUTO_SEND_MESSAGE", new object[]
                {
                    0,
                    req.MessageContent,
                    req.MessageType,
                    req.SendTime,
                    req.PageID,
                    req.ShopID,
                    req.CreatedBy,
                    req.Channel,
                    req.ListDetail.ToXML(),
                    req.ListUserID.ToXML()
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Lỗi thêm chi tiết tin nhắn tự dộng";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lỗi thêm người tin nhắn tự dộng";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "AutoSendMessageController.AddAutoSendMessage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region SERVICE
        [Route("ServiceSearchList")]
        public ZResponse ServiceSearchList(ZAutoSendMessageServiceSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZAutoSendMessageServiceSearchListResponse> oList = new List<ZAutoSendMessageServiceSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_AUTO_SEND_MESSAGE_PKG.SERVICE_SEARCH_LIST", new object[]
                {
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
                                    oList.Add(new ZAutoSendMessageServiceSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        MessageContent = Convert.ToString(dr["MESSAGE_CONTENT"]),
                                        MessageType = Convert.ToString(dr["MESSAGE_TYPE"]),
                                        AccessToken = Convert.ToString(dr["ACCESS_TOKEN"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "AutoSendMessageController.ServiceSearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateStatusSuccess")]
        public ZResponse UpdateStatusSuccess(ZAutoSendMessageUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_AUTO_SEND_MESSAGE_PKG.UPDATE_STATUS_SUCCESS", new object[]
                {
                    0,
                    req.ID,
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
                            res.Code = "00";
                            res.Message = "Thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "AutoSendMessageController.UpdateStatusSuccess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion
    }
}