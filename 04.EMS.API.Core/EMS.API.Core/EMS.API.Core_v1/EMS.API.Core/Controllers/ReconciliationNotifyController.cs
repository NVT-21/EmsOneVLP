using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{

    [RoutePrefix("ReconciliationNotify")]
    public class ReconciliationNotifyController : ApiController
    {
        [Route("ListNotify")]
        public ZResponse ListNotify(ZReconciliationListNotifyRequest req)
        {
            int result = -1;
            List<ZReconciliationListNotifyResponse> lst = new List<ZReconciliationListNotifyResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_NOTIFY_PKG.LIST_NOTIFY", new object[]
                {
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
                                    var messageTypeText = Convert.ToString(dr["TYPE"]) == "COD" ? "Tiền COD" : "Cước vận chuyển";
                                    var messageDateText = Convert.ToString(dr["CREATED_DATE"]);
                                    lst.Add(new ZReconciliationListNotifyResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Type = Convert.ToString(dr["TYPE"]),
                                        TypeText = Convert.ToString(dr["TYPE"]) == "COD" ? "Tiền COD" : "Cước vận chuyển",
                                        FromDate = Convert.ToString(dr["FROM_DATE"]),
                                        ToDate = Convert.ToString(dr["TO_DATE"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        NotifyDate = Convert.ToString(dr["NOTIFY_DATE"]),
                                        NotifyMessage = "Đã có thông tin đối soát " + messageTypeText + " ngày " + messageDateText,
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationNotifyController.ListNotify " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateNotify")]
        public ZResponse UpdateNotify(ZReconciliationUpdateNotifyRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_NOTIFY_PKG.UPDATE_NOTIFY", new object[]
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationNotifyController.UpdateNotify: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}