using EMS.API.Core.Repositories;
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
    [RoutePrefix("PartnerWebhook")]
    public class PartnerWebhookController : ApiController
    {
        // GET: PartnerWebhook
        [Route("AddPartnerRegister")]
        public ZResponse AddPartnerRegister(ZPartnerWebhookAddPartnerRegisterRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_WEBHOOK_PKG.ADD_PARTNER_REGISTER", new object[]
                {
                    0,
                    req.PartnerName,
                    req.PartnerCode,
                    req.PartnerURL,
                    req.PartnerKey,
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
                            res.Message = "Thêm mới đối tác thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã PP/CRM đã đăng ký trước đó.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.AddPartnerRegister: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("EditPartnerRegister")]
        public ZResponse EditPartnerRegister(ZPartnerWebhookEditPartnerRegisterRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_WEBHOOK_PKG.EDIT_PARTNER_REGISTER", new object[]
                {
                    0,
                    req.ID,
                    req.PartnerName,
                    req.PartnerCode,
                    req.PartnerURL,
                    req.PartnerKey,
                    req.Status,
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
                            res.Message = "Cập nhật thông tin đối tác thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.EditPartnerRegister: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("DeletePartnerRegister")]
        public ZResponse DeletePartnerRegister(ZPartnerWebhookDeletePartnerRegisterRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_WEBHOOK_PKG.DELETE_PARTNER_REGISTER", new object[]
                {
                    0,
                    req.ID,
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
                            res.Message = "Đã xoá đối tác thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.DeletePartnerRegister: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListPartnerRegister")]
        public ZResponse ListPartnerRegister(ZPartnerWebhookListPartnerRegisterRequest req)
        {
            int result = -1;
            List<ZPartnerWebhookListPartnerRegisterResponse> lst = new List<ZPartnerWebhookListPartnerRegisterResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_WEBHOOK_PKG.LIST_PARTNER_REGISTER", new object[]
                {
                    req.PartnerCode,
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
                                    lst.Add(new ZPartnerWebhookListPartnerRegisterResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PartnerName = Convert.ToString(dr["PARTNER_NAME"]),
                                        PartnerCode = Convert.ToString(dr["PARTNER_CODE"]),
                                        PartnerURL = Convert.ToString(dr["PARTNER_URL"]),
                                        PartnerKey = Convert.ToString(dr["PARTNER_KEY"]),
                                        Status = Convert.ToString(dr["STATUS"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.ListPartnerRegister " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("GetPartnerRegisterByID")]
        public ZResponse GetPartnerRegisterByID(ZPartnerWebhookGetPartnerRegisterByIDRequest req)
        {
            int result = -1;
            ZPartnerWebhookListPartnerRegisterResponse item = new ZPartnerWebhookListPartnerRegisterResponse();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_WEBHOOK_PKG.PARTNER_REGISTER_BY_ID", new object[]
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
                                    item = new ZPartnerWebhookListPartnerRegisterResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PartnerName = Convert.ToString(dr["PARTNER_NAME"]),
                                        PartnerCode = Convert.ToString(dr["PARTNER_CODE"]),
                                        PartnerURL = Convert.ToString(dr["PARTNER_URL"]),
                                        PartnerKey = Convert.ToString(dr["PARTNER_KEY"]),
                                        Status = Convert.ToString(dr["STATUS"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                    };
                                }
                            }
                            if (item.PartnerName != null)
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.GetPartnerRegisterByID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        //
        [Route("ListHistoryStatus")]
        public ZResponse ListHistoryStatus(ZPartnerWebhookListHistoryStatusRequest req)
        {
            int result = -1;
            List<ZPartnerWebhookListHistoryStatusResponse> lst = new List<ZPartnerWebhookListHistoryStatusResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_WEBHOOK_PKG.LIST_HISTORY_STATUS", new object[]
                {
                    req.PartnerCode,
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
                                    lst.Add(new ZPartnerWebhookListHistoryStatusResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PartnerCode = Convert.ToString(dr["PARTNER_CODE"]),
                                        ItemCode = Convert.ToString(dr["ITEM_CODE"]),
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        StatusCode = Convert.ToString(dr["STATUS_CODE"]),
                                        StatusTime = Convert.ToString(dr["STATUS_TIME"]),
                                        StatusNote = Convert.ToString(dr["STATUS_NOTE"]),
                                        PosCode = Convert.ToString(dr["POSCODE"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.ListHistoryStatus " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateHistoryStatus")]
        public ZResponse UpdateHistoryStatus(ZPartnerWebhookUpdateHistoryStatusRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_WEBHOOK_PKG.UPDATE_HISTORY_STATUS", new object[]
                {
                    0,
                    req.ID,
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
                            res.Message = "Cập nhật lịch sử trạng thái thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerWebhookController.UpdateHistoryStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

    }
}