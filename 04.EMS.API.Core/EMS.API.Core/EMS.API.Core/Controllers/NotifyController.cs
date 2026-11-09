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
    [RoutePrefix("Notify")]
    public class NotifyController : ApiController
    {
        // GET: Notify
        [Route("NotifyList")]
        public ZResponse NotifyList(ZNotifyListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZNotifyListResponse> lst = new List<ZNotifyListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_PKG.LIST_NOTIFY", new object[]
                {
                    req.ShopID,
                    req.RowLimit,
                    req.FromDate,
                    req.ToDate,
                    req.NotifyType,
                    req.IsCheck,
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
                                    lst.Add(new ZNotifyListResponse()
                                    {
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        ID = Convert.ToInt32(dr["ID"]),
                                        NotifyType = Convert.ToString(dr["NOTIFY_TYPE"]),
                                        NotifyTypeName = XData.GetNameNotifyType(Convert.ToString(dr["NOTIFY_TYPE"])),
                                        NotifyName = Convert.ToString(dr["NOTIFY_NAME"]),
                                        NotifyContent = Convert.ToString(dr["NOTIFY_CONTENT"]),
                                        NotifyRedirectUrl = Convert.ToString(dr["NOTIFY_REDIRECT_URL"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        IsCheck = Convert.ToString(dr["IS_CHECK"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyController.NotifyList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("NotifyUpdate")]
        public ZResponse NotifyUpdate(ZNotifyUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_PKG.UPDATE_NOTIFY", new object[]
                {
                    0,
                    req.StrID,
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
                            res.Message = "Cập nhật thông báo thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyController.NotifyUpdate: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("NotifyRemove")]
        public ZResponse NotifyRemove(ZNotifyUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_PKG.REMOVE_NOTIFY", new object[]
                {
                    0,
                    req.ID,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyController.NotifyRemove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("NotifyGetByID")]
        public ZResponse NotifyGetByID(ZNotifyGetByIDRequest req)
        {
            int result = -1;
            ZNotifyListResponse item = null;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_NOTIFY_PKG.GET_BY_ID_NOTIFY", new object[]
                {
                    req.ShopID, //P_SHOP_ID       NUMBER
                    req.ID,     //P_NOTIFY_ID     NUMBER
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
                                    item = new ZNotifyListResponse()
                                    {
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        ID = Convert.ToInt32(dr["ID"]),
                                        NotifyType = Convert.ToString(dr["NOTIFY_TYPE"]),
                                        NotifyTypeName = XData.GetNameNotifyType(Convert.ToString(dr["NOTIFY_TYPE"])),
                                        NotifyName = Convert.ToString(dr["NOTIFY_NAME"]),
                                        NotifyContent = Convert.ToString(dr["NOTIFY_CONTENT"]),
                                        NotifyRedirectUrl = Convert.ToString(dr["NOTIFY_REDIRECT_URL"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        IsCheck = Convert.ToString(dr["IS_CHECK"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyController.NotifyGetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}