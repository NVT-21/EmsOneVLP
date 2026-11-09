using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("NotifyControl")]
    public class NotifyControlController : ApiController
    {
        // GET: NotifyControl
        [Route("NotifyAdd")]
        public ZResponse NotifyAdd(ZNotifyControlAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_CONTROL_PKG.ADD_NOTIFY", new object[]
                {
                    0,
                    req.NotifyType,
                    req.NotifyName,
                    req.NotifyContent,
                    req.NotifyRedirectUrl,
                    req.NotifySendDate,
                    req.NotifyReceiverType,
                    req.NotifyReceiverGroupID,
                    req.NotifyReceiverCRM,
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
                            res.Message = "Thêm mới thông báo thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlController.NotifyAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("NotifyList")]
        public ZResponse NotifyList(ZNotifyControlListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZNotifyControlListResponse> lst = new List<ZNotifyControlListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_CONTROL_PKG.LIST_NOTIFY", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.IsLock,
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
                                    lst.Add(new ZNotifyControlListResponse()
                                    {
                                        NotifyID = Convert.ToInt32(dr["NOTIFY_ID"]),
                                        NotifyType = Convert.ToString(dr["NOTIFY_TYPE"]),
                                        NotifyTypeName = XData.GetNameNotifyType(Convert.ToString(dr["NOTIFY_TYPE"])),
                                        NotifyName = Convert.ToString(dr["NOTIFY_NAME"]),
                                        NotifyContent = Convert.ToString(dr["NOTIFY_CONTENT"]),
                                        NotifyRedirectUrl = Convert.ToString(dr["NOTIFY_REDIRECT_URL"]),
                                        NotifySendDate = Convert.ToString(dr["NOTIFY_SEND_DATE"]),
                                        NotifyReceiverType = Convert.ToString(dr["NOTIFY_RECEIVER_TYPE"]),
                                        NotifyReceiverTypeName = Convert.ToString(dr["NOTIFY_RECEIVER_TYPE"]) == "ALL" 
                                                                ? "Tất cả" : Convert.ToString(dr["NOTIFY_RECEIVER_TYPE"]) == "CRM"
                                                                ? "Theo mã CRM" : Convert.ToString(dr["NOTIFY_RECEIVER_TYPE"]) == "GROUP"
                                                                ? "Theo nhóm khách hàng" : "",
                                        NotifyReceiverCRM = Convert.ToString(dr["NOTIFY_RECEIVER_CRM"]),
                                        NotifyReceiverGroupName = Convert.ToString(dr["NOTIFY_RECEIVER_GROUP_NAME"]),
                                        CreatedByName = Convert.ToString(dr["CREATED_BY_NAME"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        IsSend = Convert.ToString(dr["IS_SEND"]),
                                        NotifyStatisticalID = Convert.ToInt32(dr["NOTIFY_CONTROL_STATISTICAL_ID"]),
                                        NotifyQuantitySend = Convert.ToInt32(dr["QUANTITY_NOTIFY_SEND"]),
                                        NotifyQuantityViewed = Convert.ToInt32(dr["QUANTITY_NOTIFY_VIEWED"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlController.NotifyList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("NotifyChangeLock")]
        public ZResponse NotifyChangeLock(ZNotifyControlChangeLockRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_CONTROL_PKG.CHANGE_LOCK_NOTIFY", new object[]
                {
                    0,
                    req.NotifyID,
                    req.IsLock,
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
                            res.Message = req.IsLock == "N" ? "Mở khoá thông báo thành công." : "Khoá thông báo thành công.";
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
                                    res.Message = "Thông báo đang trong quá trình xử lý.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Thông báo đã được gửi đi.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlController.NotifyChangeLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("NotifyRemove")]
        public ZResponse NotifyRemove(ZNotifyControlRemoveRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_CONTROL_PKG.REMOVE_NOTIFY", new object[]
                {
                    0,
                    req.NotifyID,
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
                            res.Message = "Đã xoá thông báo thành công.";
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
                                    res.Message = "Thông báo đang trong quá trình xử lý.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlController.NotifyRemove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("NotifyGetByID")]
        public ZResponse NotifyGetByID(ZNotifyControlGetByIDRequest req)
        {
            int result = -1;
            ZNotifyControlListResponse item = null;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_CONTROL_PKG.GET_BY_ID_NOTIFY", new object[]
                {
                    req.NotifyID,     //P_NOTIFY_ID      NUMBER
                    0,          //P_RETURN_CODE   OUT NUMBER
                    0           //P_OUT_CURSOR    OUT T_CURSOR
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
                                if (dr.Read())
                                {
                                    item = new ZNotifyControlListResponse()
                                    {
                                        NotifyID = Convert.ToInt32(dr["NOTIFY_ID"]),
                                        NotifyType = Convert.ToString(dr["NOTIFY_TYPE"]),
                                        NotifyTypeName = XData.GetNameNotifyType(Convert.ToString(dr["NOTIFY_TYPE"])),
                                        NotifyName = Convert.ToString(dr["NOTIFY_NAME"]),
                                        NotifyContent = Convert.ToString(dr["NOTIFY_CONTENT"]),
                                        NotifyRedirectUrl = Convert.ToString(dr["NOTIFY_REDIRECT_URL"]),
                                        NotifySendDate = Convert.ToString(dr["NOTIFY_SEND_DATE"]),
                                        NotifyReceiverType = Convert.ToString(dr["NOTIFY_RECEIVER_TYPE"]),
                                        NotifyReceiverTypeName = Convert.ToString(dr["NOTIFY_RECEIVER_TYPE"]) == "ALL"
                                                                ? "Tất cả" : Convert.ToString(dr["NOTIFY_RECEIVER_TYPE"]) == "CRM"
                                                                ? "Theo mã CRM" : Convert.ToString(dr["NOTIFY_RECEIVER_TYPE"]) == "GROUP"
                                                                ? "Theo nhóm khách hàng" : "",
                                        NotifyReceiverGroupID = Convert.ToString(dr["NOTIFY_RECEIVER_GROUP_ID"]),
                                        NotifyReceiverCRM = Convert.ToString(dr["NOTIFY_RECEIVER_CRM"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        IsSend = Convert.ToString(dr["IS_SEND"])
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu";
                            }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlController.NotifyGetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("NotifyEdit")]
        public ZResponse NotifyEdit(ZNotifyControlEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_CONTROL_PKG.EDIT_NOTIFY", new object[]
                {
                    0,
                    req.NotifyID,
                    req.NotifyType,
                    req.NotifyName,
                    req.NotifyContent,
                    req.NotifyRedirectUrl,
                    req.NotifySendDate,
                    req.NotifyReceiverType,
                    req.NotifyReceiverGroupID,
                    req.NotifyReceiverCRM,
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
                            res.Message = "Chỉnh sửa thông báo thành công.";
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
                                    res.Message = "Thông báo đang trong quá trình xử lý.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Thông báo đã được gửi đi.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlController.NotifyEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}