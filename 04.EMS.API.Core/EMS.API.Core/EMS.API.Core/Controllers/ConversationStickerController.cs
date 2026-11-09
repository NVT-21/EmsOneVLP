using EMS.API.Core.Repositories;
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
    [RoutePrefix("ConversationSticker")]
    public class ConversationStickerController : ApiController
    {
        // GET: ConversationSticker
        [Route("Add")]
        public ZResponse Add(ZConversationStickerAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONVERSATION_STICKER_PKG.ADD_CONVERSATION_STICKER", new object[]
                {
                    0,
                    req.ShopID,
                    req.PageID,
                    req.Channel,
                    req.StickerName,
                    req.StickerColor,
                    req.CreatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[00].Value);
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
                                    res.Message = "Tên nhãn đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConversationStickerController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("SearchList")]
        public ZResponse SearchList(ZConversationStickerSearchListRequest req)
        {
            int result = -1;
            List<ZConversationStickerSearchListResponse> oList = new List<ZConversationStickerSearchListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONVERSATION_STICKER_PKG.LIST_CONVERSATION_STICKER", new object[]
                {
                    req.ShopID,
                    req.PageID,
                    req.Channel,
                    req.SearchKey,
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
                                    oList.Add(new ZConversationStickerSearchListResponse()
                                    {
                                        StickerID = Convert.ToInt32(dr["STICKER_ID"]),
                                        StickerName = Convert.ToString(dr["STICKER_NAME"]),
                                        StickerColor = Convert.ToString(dr["STICKER_COLOR"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConversationStickerController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("GetByID")]
        public ZResponse GetByID(ZConversationStickerByIDRequest req)
        {
            int result = -1;
            ZConversationStickerByIDResponse item = new ZConversationStickerByIDResponse();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONVERSATION_STICKER_PKG.CONVERSATION_STICKER_BY_ID", new object[]
                {
                    req.ShopID,
                    req.PageID,
                    req.Channel,
                    req.StickerID,
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
                                    item = new ZConversationStickerByIDResponse()
                                    {
                                        StickerID = Convert.ToInt32(dr["STICKER_ID"]),
                                        StickerName = Convert.ToString(dr["STICKER_NAME"]),
                                        StickerColor = Convert.ToString(dr["STICKER_COLOR"])
                                    };
                                }
                            }
                            if (item.StickerName != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConversationStickerController.GetByID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("Edit")]
        public ZResponse Edit(ZConversationStickerEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONVERSATION_STICKER_PKG.EDIT_CONVERSATION_STICKER", new object[]
                {
                    0,
                    req.ShopID,
                    req.PageID,
                    req.Channel,
                    req.StickerID,
                    req.StickerName,
                    req.StickerColor,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[00].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thông tin thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên nhãn đã tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConversationStickerController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("Delete")]
        public ZResponse Delete(ZConversationStickerRemoveRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONVERSATION_STICKER_PKG.REMOVE_CONVERSATION_STICKER", new object[]
                {
                    0,
                    req.ShopID,
                    req.PageID,
                    req.Channel,
                    req.StickerID,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[00].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Đã xoá thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConversationStickerController.Delete: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchListCStickerOfC")]
        public ZResponse SearchListCStickerOfC(ZConversationStickerSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ConversationStickerRepository.ListCStickerOfC(new ZConversationStickerSearchListRequest() { 
                   ShopID = req.ShopID,
                   PageID = req.PageID,
                   Channel = req.Channel,
                   ConversationID = req.ConversationID,
                   TypeList = req.TypeList
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConversationStickerController.SearchListCStickerOfC" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("UpdateCStickerOfC")]
        public ZResponse UpdateCStickerOfC(ZCStickerOfCUpdateRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONVERSATION_STICKER_PKG.UPDATE_C_STICKER_OF_C", new object[]
                {
                    0,
                    req.ShopID,
                    req.PageID,
                    req.ConversationID,
                    req.ListItem.ToXML(),
                    req.Channel,
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
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Cửa hàng không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Nhân viên không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Cuộc hội thoại không tồn tại.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                                    break;
                            };
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConversationStickerController.UpdateCStickerOfC: " + ex.Message);
            }
            return res;
        }
    }
}