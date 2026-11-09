using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Payment")]
    public class PaymentController : ApiController
    {
        #region ListPaymentCOD
        [HttpPost]
        [Route("ListPaymentCOD")]
        public ZResponse ListPaymentCOD(ZPaymentListCODRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZPaymentListCODResponse> list = new List<ZPaymentListCODResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PAYMENT_PKG.LIST_PAYMENT_COD", new object[]
                {
                    req.CustomerCode,           
                    req.CustomerPPCRM,
                    req.Status,
                    req.PaymentStatus,
                    req.ApprovalDate,
                    req.OrderCode,
                    req.EmsBarCode,
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
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
                                    list.Add(new ZPaymentListCODResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        CustomerCode = dr["CUSTOMER_CODE"].ToString(),
                                        CustomerPPCRM = dr["CRM_PP_CUSTOMER_CODE"].ToString(),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        EmsBarCode = dr["EMSBARCODE"].ToString(),
                                        CODMoney = Convert.ToInt64(dr["COD_MONEY"]),
                                        IsAgree = dr["IS_AGREE"].ToString(),
                                        IsPayment = dr["IS_PAYMENT"].ToString(),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"])
                                    });
                                }
                            }
                            if (list.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(list);
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
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PaymentController.ListPaymentCOD: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region UpdatePaymentCOD
        [HttpPost]
        [Route("UpdatePaymentCOD")]
        public ZResponse UpdatePaymentCOD(ZPaymentUpdateCODRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PAYMENT_PKG.UPDATE_PAYMENT_COD", new object[]
                {
                    0,
                    req.ID,
                    req.UpdatedBy,
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
                            res.Message = "Xác nhận thanh toán thành công.";
                            res.Data = Convert.ToString(dbCommand.Parameters["P_EMSBARCODE"].Value);
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại. Vui lòng liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "Payment.UpdatePaymentCOD: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion
    }
}