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
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Reconciliation")]
    public class ReconciliationController : ApiController
    {

        #region List

        #region Fee
        [HttpPost]
        [Route("ListTransportFee")]
        public ZResponse ListTransportFee(ZReconciliationSearchListFeeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationSearchListFeeResponse> list = new List<ZReconciliationSearchListFeeResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_PKG.LIST_RECONCILIATION_FEE", new object[]
                {
                    req.ShopID,      //P_SHOP_ID NUMBER            
                    req.FromDate,
                    req.ToDate,
                    req.EmsBarCode,
                    req.OrderCode,
                    req.SearchCode,
                    req.TypeItem,
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
                                    list.Add(new ZReconciliationSearchListFeeResponse()
                                    {
                                        AmndDate = dr["AMND_DATE"].ToString(),
                                        CustomerCode = dr["CUSTOMER_CODE"].ToString(),
                                        EmsBarCode = dr["EMSBARCODE"].ToString(),
                                        TimeRelease = dr["TIME_RELEASE"].ToString(),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        AdditionService = dr["ADDITION_SERVICE"].ToString(),
                                        SpecialService = dr["SPECIAL_SERVICE"].ToString(),
                                        ReceiveProvice = dr["POSTAL_CODE_RETURN"].ToString(),
                                        ProvisionalVolume = Convert.ToInt32(dr["PROVISIONAL_VOLUME"]),
                                        ChargeableVolume = Convert.ToInt32(dr["CHARGEABLE_VOLUME"]),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = dr["ORDERCODE"].ToString(),
                                        TempTransportFee = Convert.ToInt64(dr["TEMP_TRANSPORT_FEE"]),
                                        CreatedDateOrder = dr["CREATED_DATE_ORDER"].ToString(),
                                        TransportFeePublic = Convert.ToInt64(dr["TRANSPORT_FEE_PUBLIC"]),
                                        FeePublic = Convert.ToInt64(dr["FEE_PUBLIC"]),
                                        ServiceFee = Convert.ToInt64(dr["SERVICE_FEE"]),
                                        MainFee = Convert.ToInt64(dr["MAIN_FEE"]),
                                        ReducedRates = Convert.ToInt64(dr["REDUCED_RATES"]),
                                        TypeItem = dr["TYPE_ITEM"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListTransportFee: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("ListTransportFeePeriod")]
        public ZResponse ListTransportFeePeriod(ZReconciliationSearchListFeeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationSearchListFeePeriodResponse> list = new List<ZReconciliationSearchListFeePeriodResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_PKG.LIST_RECONCILIATION_PERIOD_FEE", new object[]
                {
                    req.ShopID,      //P_SHOP_ID NUMBER            
                    req.FromDate,
                    req.ToDate,
                    req.Month,
                    req.Year,
                    req.Status,
                    req.EmsBarCode,
                    req.ListCode,
                    req.OrderCode,
                    req.SearchCode,
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
                                    list.Add(new ZReconciliationSearchListFeePeriodResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        EmsBarCode = dr["EMSBARCODE"].ToString(),
                                        TimeRelease = dr["TIME_RELEASE"].ToString(),
                                        ReceiveProvice = dr["POSTAL_CODE_RETURN"].ToString(),
                                        ProvisionalVolume = Convert.ToInt32(dr["PROVISIONAL_VOLUME"]),
                                        ChargeableVolume = Convert.ToInt32(dr["CHARGEABLE_VOLUME"]),
                                        //CODFee = Convert.ToInt64(dr["COD_FEE"]),
                                        ServiceFee = Convert.ToInt64(dr["SERVICE_FEE"]),
                                        MainFee = Convert.ToInt64(dr["MAIN_FEE"]),
                                        ReducedRates = Convert.ToInt64(dr["REDUCED_RATES"]),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        Note = dr["NOTE"].ToString(),
                                        IsAgree = dr["IS_AGREE"].ToString(),
                                        IsPayment = dr["IS_PAYMENT"].ToString(),
                                        TransportFeePublic = Convert.ToInt64(dr["TRANSPORT_FEE_PUBLIC"]),
                                        FeePublic = Convert.ToInt64(dr["FEE_PUBLIC"]),
                                        ApprovalDate = dr["APPROVAL_DATE"].ToString(),
                                        ReasonReject = dr["REASON_REJECT"].ToString(),
                                        PeriodName = dr["PERIOD_NAME"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListTransportFeePeriod: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("ListTransportFeeTradeDiscounts")]
        public ZResponse ListTransportFeeTradeDiscounts(ZReconciliationSearchListFeeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationSearchListFeeTradeDiscountsResponse> list = new List<ZReconciliationSearchListFeeTradeDiscountsResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_PKG.LIST_RECONCILIATION_TRADE_FEE", new object[]
                {
                    req.ShopID,      //P_SHOP_ID NUMBER            
                    req.FromDate,
                    req.ToDate,
                    req.Month,
                    req.Year,
                    req.Status,
                    req.EmsBarCode,
                    req.ListCode,
                    req.OrderCode,
                    req.SearchCode,
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
                                    list.Add(new ZReconciliationSearchListFeeTradeDiscountsResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        EmsBarCode = dr["EMSBARCODE"].ToString(),
                                        TimeRelease = dr["TIME_RELEASE"].ToString(),
                                        ReceiveProvice = dr["POSTAL_CODE_RETURN"].ToString(),
                                        ProvisionalVolume = Convert.ToInt32(dr["PROVISIONAL_VOLUME"]),
                                        ChargeableVolume = Convert.ToInt32(dr["CHARGEABLE_VOLUME"]),
                                        CODFee = Convert.ToInt64(dr["COD_FEE"]),
                                        ServiceFee = Convert.ToInt64(dr["SERVICE_FEE"]),
                                        MainFee = Convert.ToInt64(dr["MAIN_FEE"]),
                                        ReducedRates = Convert.ToInt64(dr["REDUCED_RATES"]),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        Note = dr["NOTE"].ToString(),
                                        TransportFeePublic = Convert.ToInt64(dr["TRANSPORT_FEE_PUBLIC"]),
                                        FeePublic = Convert.ToInt64(dr["FEE_PUBLIC"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListTransportFeeTradeDiscounts: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region COD
        [HttpPost]
        [Route("ListCOD")]
        public ZResponse ListCOD(ZReconciliationSearchListCODRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationSearchListCODResponse> list = new List<ZReconciliationSearchListCODResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_PKG.LIST_RECONCILIATION_COD", new object[]
                {
                    req.ShopID,      //P_SHOP_ID NUMBER            
                    req.FromDate,
                    req.ToDate,
                    req.Month,
                    req.Year,
                    req.Status,
                    req.EmsBarCode,
                    req.ListCode,
                    req.OrderCode,
                    req.SearchCode,
                    req.CODTransType,
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
                                    list.Add(new ZReconciliationSearchListCODResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        CustomerCode = dr["CUSTOMER_CODE"].ToString(),
                                        ListCode = dr["LIST_CODE"].ToString(),
                                        EmsBarCode = dr["EMSBARCODE"].ToString(),
                                        CODMoney = Convert.ToInt64(dr["COD_MONEY"]),
                                        //IsAgree = dr["IS_AGREE"].ToString(),
                                        TimeRelease = dr["TIME_RELEASE"].ToString(),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = dr["ORDERCODE"].ToString(),
                                        CreatedDateOrder = dr["CREATED_DATE_ORDER"].ToString(),
                                        CODTransTypeName = XData.GetNameCODTransType((Convert.ToInt32(dr["COD_TRANSTYPE"])).ToString())
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListCOD: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("ListCODPeriod")]
        public ZResponse ListCODPeriod(ZReconciliationSearchListCODRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationSearchListCODResponse> list = new List<ZReconciliationSearchListCODResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_PKG.LIST_RECONCILIATION_PERIOD_COD", new object[]
                {
                    req.ShopID,      //P_SHOP_ID NUMBER            
                    req.FromDate,
                    req.ToDate,
                    req.Month,
                    req.Year,
                    req.Status,
                    req.EmsBarCode,
                    req.ListCode,
                    req.OrderCode,
                    req.SearchCode,
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
                                    list.Add(new ZReconciliationSearchListCODResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        CustomerCode = dr["CUSTOMER_CODE"].ToString(),
                                        ListCode = dr["LIST_CODE"].ToString(),
                                        EmsBarCode = dr["EMSBARCODE"].ToString(),
                                        CODMoney = Convert.ToInt64(dr["COD_MONEY"]),
                                        IsAgree = dr["IS_AGREE"].ToString(),
                                        IsPayment = dr["IS_PAYMENT"].ToString(),
                                        TimeRelease = dr["TIME_RELEASE"].ToString(),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = dr["ORDERCODE"].ToString(),
                                        CreatedDateOrder = dr["CREATED_DATE_ORDER"].ToString(),
                                        ApprovalDate = dr["APPROVAL_DATE"].ToString(),
                                        ReasonReject = dr["REASON_REJECT"].ToString(),
                                        PeriodName = dr["PERIOD_NAME"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListCODPeriod: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #endregion

        #region UpdateAgree
        [HttpPost]
        [Route("UpdateAgreeTransportFee")]
        public ZResponse UpdateAgreeTransportFee(ZReconciliationUpdateAgreeFeeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_PKG.UPDATE_AGREE_FEE", new object[]
                {
                    0,
                    req.ID,
                    req.Agree,
                    req.ReasonReject,
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
                            res.Message = "Đã đối soát thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.UpdateAgreeTransportFee: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("UpdateAgreeCOD")]
        public ZResponse UpdateAgreeCOD(ZReconciliationUpdateAgreeCODRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_PKG.UPDATE_AGREE_COD", new object[]
                {
                    0,
                    req.ID,
                    req.Agree,
                    req.ReasonReject,
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
                            res.Message = "Đã đối soát thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.UpdateAgreeCOD: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Control_Period
        [HttpPost]
        [Route("ListControlPeriod")]
        public ZResponse ListControlPeriod(ZControlPeriodRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZControlPeriodResponse> list = new List<ZControlPeriodResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_PKG.LIST_CONTROL_PERIOD", new object[]
                {
                    req.ShopID,      //P_SHOP_ID NUMBER
                    req.Type,        //P_TYPE VARCHAR2
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
                                    list.Add(new ZControlPeriodResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PeriodName = dr["PERIOD_NAME"].ToString()
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
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListControlPeriod: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("ControlPeriodDateID")]
        public ZResponse ControlPeriodDateID(ZControlPeriodDateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZControlPeriodDateResponse item = new ZControlPeriodDateResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_PKG.LIST_CONTROL_PERIOD_DATE", new object[]
                {
                    req.ShopID,
                    req.ID,          //P_SHOP_ID NUMBER            
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
                                    item = new ZControlPeriodDateResponse()
                                    {
                                        FromDate = dr["FROM_DATE"].ToString(),
                                        ToDate = dr["TO_DATE"].ToString()
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
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ControlPeriodDateID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region ListCODFollow - ListFeeFollow
        [HttpPost]
        [Route("ListCODFollow")]
        public ZResponse ListCODFollow(ZReconciliationSearchListCODFollowRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationSearchListCODFollowResponse> list = new List<ZReconciliationSearchListCODFollowResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_FOLLOW_PKG.LIST_RECONCILIATION_COD", new object[]
                {
                    req.TypeDate,
                    req.FromDate,
                    req.ToDate,
                    req.CustomerCode,
                    req.CustomerPPCRM,
                    req.CODTransType,
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
                                    list.Add(new ZReconciliationSearchListCODFollowResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        CustomerCode = dr["CUSTOMER_CODE"].ToString(),
                                        CustomerPPCRM = dr["CRM_PP_CUSTOMER_CODE"].ToString(),
                                        ListCode = dr["LIST_CODE"].ToString(),
                                        EmsBarCode = dr["EMSBARCODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        CreatedDateOrder = dr["CREATED_DATE_ORDER"].ToString(),
                                        TimeRelease = dr["TIME_RELEASE"].ToString(),
                                        //ApprovalDate = dr["APPROVAL_DATE"].ToString(),
                                        CODMoney = Convert.ToInt64(dr["COD_MONEY"]),
                                        //IsAgree = dr["IS_AGREE"].ToString(),
                                        //IsPayment = dr["IS_PAYMENT"].ToString(),
                                        //ReasonReject = dr["REASON_REJECT"].ToString(),
                                        //PeriodName = dr["PERIOD_NAME"].ToString(),
                                        CODTransTypeName = XData.GetNameCODTransType((Convert.ToInt32(dr["COD_TRANSTYPE"])).ToString())
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListCODFollow: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [HttpPost]
        [Route("ListFeeFollow")]
        public ZResponse ListFeeFollow(ZReconciliationSearchListFeeFollowRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationSearchListFeeFollowResponseV2> list = new List<ZReconciliationSearchListFeeFollowResponseV2>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_FOLLOW_PKG.LIST_RECONCILIATION_FEE_V2", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.CustomerCode,
                    req.CustomerPPCRM,
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
                                    list.Add(new ZReconciliationSearchListFeeFollowResponseV2()
                                    {
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        CustomerCode = dr["CUSTOMER_CODE"].ToString(),
                                        CustomerPPCRM = dr["CUSTOMER_CODE_PP_CRM"].ToString(),
                                        EmsBarCode = dr["EMSBARCODE"].ToString(),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = dr["ORDERCODE"].ToString(),
                                        TimeRelease = dr["TIME_RELEASE"].ToString(),
                                        TempTransportFee = Convert.ToInt64(dr["TEMP_TRANSPORT_FEE"]),
                                        ServiceFee = Convert.ToInt64(dr["SERVICE_FEE"]),
                                        MainFee = Convert.ToInt64(dr["MAIN_FEE"]),
                                        ReducedRates = Convert.ToInt64(dr["REDUCED_RATES"]),
                                        TransportFeePublic = Convert.ToInt64(dr["TRANSPORT_FEE_PUBLIC"]),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        TypeItem = dr["TYPE_ITEM"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListFeeFollow: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region List Fee Report
        [HttpPost]
        [Route("ListFeeReport")]
        public ZResponse ListFeeReport(ZReconciliationSearchFeeReportRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationSearchFeeReportResponse> list = new List<ZReconciliationSearchFeeReportResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_FOLLOW_PKG.LIST_REPORT_FEE", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.CustomerPPCRM,
                    req.PostOffice,
                    req.TypeItem,
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
                                    list.Add(new ZReconciliationSearchFeeReportResponse()
                                    {
                                        PostOffice = dr["POST_OFFICE"].ToString(),
                                        TotalOrder = Convert.ToInt64(dr["QUANTITY"]),
                                        TotalWeight = Convert.ToInt64(dr["CHARGEABLE_VOLUME"]),
                                        TotalFee = Convert.ToInt64(dr["TOTAL_TRANSPORT_FEE"]),
                                        TypeItem = dr["TYPE_ITEM"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListFeeReport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("ListFeePostOfficeReport")]
        public ZResponse ListFeePostOfficeReport(ZReconciliationSearchFeePostOfficeReportRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationSearchFeePostOfficeReportResponse> list = new List<ZReconciliationSearchFeePostOfficeReportResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_FOLLOW_PKG.LIST_REPORT_POSTOFFICE_FEE", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.CustomerPPCRM,
                    req.PostOffice,
                    req.TypeItem,
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
                                    list.Add(new ZReconciliationSearchFeePostOfficeReportResponse()
                                    {
                                        CustomerPPCRM = dr["CUSTOMER_CODE"].ToString(),
                                        TotalOrder = Convert.ToInt64(dr["QUANTITY"]),
                                        TotalWeight = Convert.ToInt64(dr["CHARGEABLE_VOLUME"]),
                                        TotalFee = Convert.ToInt64(dr["TOTAL_TRANSPORT_FEE"]),
                                        TypeItem = dr["TYPE_ITEM"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListFeePostOfficeReport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region ListFeeAdjustingJournalEntries
        [HttpPost]
        [Route("ListFeeAdjustingJournalEntries")]
        public ZResponse ListFeeAdjustingJournalEntries(ZReconciliationFeeAdjustingJournalEntriesRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZReconciliationFeeAdjustingJournalEntriesResponse> list = new List<ZReconciliationFeeAdjustingJournalEntriesResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_FOLLOW_PKG.LIST_ADJ_JOURNAL_ENTRIES", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.CustomerPPCRM,
                    req.AdjustingPostOffice,
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
                                    list.Add(new ZReconciliationFeeAdjustingJournalEntriesResponse()
                                    {
                                        AdjustingID = Convert.ToInt32(dr["ADJUSTING_ID"]),
                                        AdjustingDate = dr["ADJUSTING_DATE"].ToString(),
                                        AdjustingPostOffice = dr["ADJUSTING_POST_OFFICE"].ToString(),
                                        ItemCode = dr["ITEM_CODE"].ToString(),
                                        TimeRelease = dr["TIME_RELEASE"].ToString(),
                                        ProvisionalVolume = Convert.ToInt32(dr["PROVISIONAL_VOLUME"]),
                                        ChargeableVolume = Convert.ToInt32(dr["CHARGEABLE_VOLUME"]),
                                        MainFee = Convert.ToInt64(dr["MAIN_FEE"]),
                                        ServiceFee = Convert.ToInt64(dr["SERVICE_FEE"]),
                                        TransportFeePublic = Convert.ToInt64(dr["TRANSPORT_FEE_PUBLIC"]),
                                        FeePublic = Convert.ToInt64(dr["FEE_PUBLIC"]),
                                        Reason = dr["REASON"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListFeeAdjustingJournalEntries: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region ListDetailEditedItemCode
        [HttpPost]
        [Route("ListDetailEditedItemCode")]
        public ZResponse ListDetailEditedItemCode(ZReconciliationListDetailEditedItemCodeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<DetailEditedItemCode> listItemTemp = new List<DetailEditedItemCode>();
            List<ZReconciliationListDetailEditedItemCodeResponse> list = new List<ZReconciliationListDetailEditedItemCodeResponse>();
            List<ZReconciliationListDetailEditedItemCodeResponse> listResponse = new List<ZReconciliationListDetailEditedItemCodeResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_RECONCILIATION_FOLLOW_PKG.LIST_DETAIL_EDITED_ITEMCODE", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.PostOffice,
                    req.CustomerPPCRM,
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
                                    listItemTemp.Add(new DetailEditedItemCode()
                                    {
                                        AdjustingID = Convert.ToInt32(dr["ADJUSTING_ID"]),
                                        AdjustingDate = dr["ADJUSTING_DATE"].ToString(),
                                        AdjustingPostOffice = dr["ADJUSTING_POST_OFFICE"].ToString(),
                                        ItemCode = dr["ITEM_CODE"].ToString(),
                                        TimeRelease = dr["TIME_RELEASE"].ToString(),
                                        ProvisionalVolume = Convert.ToInt32(dr["PROVISIONAL_VOLUME"]),
                                        ChargeableVolume = Convert.ToInt32(dr["CHARGEABLE_VOLUME"]),
                                        MainFee = Convert.ToInt64(dr["MAIN_FEE"]),
                                        ServiceFee = Convert.ToInt64(dr["SERVICE_FEE"]),
                                        TransportFeePublic = Convert.ToInt64(dr["TRANSPORT_FEE_PUBLIC"]),
                                        FeePublic = Convert.ToInt64(dr["FEE_PUBLIC"]),
                                        Reason = dr["REASON"].ToString(),
                                        IsRelease = Convert.ToInt32(dr["IS_RELEASE"]),
                                        CustomerCode = dr["CUSTOMER_CODE"].ToString()
                                    });
                                }
                            }
                            if (listItemTemp.Count > 0)
                            {
                                var listGroupBy = listItemTemp.GroupBy(item => item.ItemCode)
                                 .Select(group => new { ItemCode = group.Key, Items = group.ToList(), Decending = group.OrderBy(m => m.AdjustingID) })
                                 .ToList();
                                foreach (var i in listGroupBy)
                                {
                                    List<DetailEditedItemCode> listItemAdd = new List<DetailEditedItemCode>();
                                    var iItem = i.Items.OrderBy(m => m.AdjustingID).ToList();
                                    for (int j = 0; j <= iItem.Count - 1; j++)
                                    {
                                        listItemAdd.Add(new DetailEditedItemCode()
                                        {
                                            AdjustingID = iItem[j].AdjustingID,
                                            AdjustingDate = iItem[j].AdjustingDate,
                                            AdjustingPostOffice = iItem[j].AdjustingPostOffice,
                                            ItemCode = iItem[j].ItemCode,
                                            TimeRelease = iItem[j].TimeRelease,
                                            ProvisionalVolume = iItem[j].ProvisionalVolume,
                                            ChargeableVolume = iItem[j].ChargeableVolume,
                                            MainFee = iItem[j].MainFee,
                                            ServiceFee = iItem[j].ServiceFee,
                                            TransportFeePublic = iItem[j].TransportFeePublic,
                                            FeePublic = iItem[j].FeePublic,
                                            Reason = iItem[j].Reason,
                                            IsRelease = iItem[j].IsRelease,
                                            CustomerCode = iItem[j].CustomerCode
                                        });
                                    }
                                    list.Add(new ZReconciliationListDetailEditedItemCodeResponse()
                                    {
                                        ItemCode = i.ItemCode,
                                        ItemCodeDetail = listItemAdd,
                                    });
                                }
                                var stt = 1;
                                foreach (var v in list)
                                {
                                    if (v.ItemCodeDetail.Count == 2)
                                    {
                                        listResponse.Add(new ZReconciliationListDetailEditedItemCodeResponse()
                                        {
                                            NumRow = stt++,
                                            ItemCode = v.ItemCode,
                                            ItemCodeDetail = v.ItemCodeDetail,
                                            AdjBalanceProvisionalVolume = (v.ItemCodeDetail[v.ItemCodeDetail.Count - 1].ProvisionalVolume - v.ItemCodeDetail[v.ItemCodeDetail.Count - 2].ProvisionalVolume),
                                            AdjBalanceChargeableVolume = (v.ItemCodeDetail[v.ItemCodeDetail.Count - 1].ChargeableVolume - v.ItemCodeDetail[v.ItemCodeDetail.Count - 2].ChargeableVolume),
                                            AdjBalanceMainFee = (v.ItemCodeDetail[v.ItemCodeDetail.Count - 1].MainFee - v.ItemCodeDetail[v.ItemCodeDetail.Count - 2].MainFee),
                                            AdjBalanceServiceFee = (v.ItemCodeDetail[v.ItemCodeDetail.Count - 1].ServiceFee - v.ItemCodeDetail[v.ItemCodeDetail.Count - 2].ServiceFee),
                                            AdjBalanceTransportFeePublic = (v.ItemCodeDetail[v.ItemCodeDetail.Count - 1].TransportFeePublic - v.ItemCodeDetail[v.ItemCodeDetail.Count - 2].TransportFeePublic),
                                            AdjBalanceFeePublic = (v.ItemCodeDetail[v.ItemCodeDetail.Count - 1].FeePublic - v.ItemCodeDetail[v.ItemCodeDetail.Count - 2].FeePublic),
                                        });
                                    }
                                    else if(v.ItemCodeDetail.Count > 2)
                                    {
                                        for (var h = 0; h < v.ItemCodeDetail.Count - 1; h++)
                                        {
                                            List<DetailEditedItemCode> listTemp = new List<DetailEditedItemCode>();
                                            listTemp.Add(new DetailEditedItemCode()
                                            {
                                                AdjustingID = v.ItemCodeDetail[h].AdjustingID,
                                                AdjustingDate = v.ItemCodeDetail[h].AdjustingDate,
                                                AdjustingPostOffice = v.ItemCodeDetail[h].AdjustingPostOffice,
                                                ItemCode = v.ItemCodeDetail[h].ItemCode,
                                                TimeRelease = v.ItemCodeDetail[h].TimeRelease,
                                                ProvisionalVolume = v.ItemCodeDetail[h].ProvisionalVolume,
                                                ChargeableVolume = v.ItemCodeDetail[h].ChargeableVolume,
                                                MainFee = v.ItemCodeDetail[h].MainFee,
                                                ServiceFee = v.ItemCodeDetail[h].ServiceFee,
                                                TransportFeePublic = v.ItemCodeDetail[h].TransportFeePublic,
                                                FeePublic = v.ItemCodeDetail[h].FeePublic,
                                                Reason = v.ItemCodeDetail[h].Reason,
                                                IsRelease = v.ItemCodeDetail[h].IsRelease,
                                                CustomerCode = v.ItemCodeDetail[h].CustomerCode
                                            });
                                            listTemp.Add(new DetailEditedItemCode()
                                            {
                                                AdjustingID = v.ItemCodeDetail[h + 1].AdjustingID,
                                                AdjustingDate = v.ItemCodeDetail[h + 1].AdjustingDate,
                                                AdjustingPostOffice = v.ItemCodeDetail[h + 1].AdjustingPostOffice,
                                                ItemCode = v.ItemCodeDetail[h + 1].ItemCode,
                                                TimeRelease = v.ItemCodeDetail[h + 1].TimeRelease,
                                                ProvisionalVolume = v.ItemCodeDetail[h + 1].ProvisionalVolume,
                                                ChargeableVolume = v.ItemCodeDetail[h + 1].ChargeableVolume,
                                                MainFee = v.ItemCodeDetail[h + 1].MainFee,
                                                ServiceFee = v.ItemCodeDetail[h + 1].ServiceFee,
                                                TransportFeePublic = v.ItemCodeDetail[h + 1].TransportFeePublic,
                                                FeePublic = v.ItemCodeDetail[h + 1].FeePublic,
                                                Reason = v.ItemCodeDetail[h + 1].Reason,
                                                IsRelease = v.ItemCodeDetail[h + 1].IsRelease,
                                                CustomerCode = v.ItemCodeDetail[h + 1].CustomerCode
                                            });
                                            listResponse.Add(new ZReconciliationListDetailEditedItemCodeResponse()
                                            {
                                                NumRow = stt++,
                                                ItemCode = v.ItemCode,
                                                ItemCodeDetail = listTemp,
                                                AdjBalanceProvisionalVolume = (listTemp[listTemp.Count - 1].ProvisionalVolume - listTemp[listTemp.Count - 2].ProvisionalVolume),
                                                AdjBalanceChargeableVolume = (listTemp[listTemp.Count - 1].ChargeableVolume - listTemp[listTemp.Count - 2].ChargeableVolume),
                                                AdjBalanceMainFee = (listTemp[listTemp.Count - 1].MainFee - listTemp[listTemp.Count - 2].MainFee),
                                                AdjBalanceServiceFee = (listTemp[listTemp.Count - 1].ServiceFee - listTemp[listTemp.Count - 2].ServiceFee),
                                                AdjBalanceTransportFeePublic = (listTemp[listTemp.Count - 1].TransportFeePublic - listTemp[listTemp.Count - 2].TransportFeePublic),
                                                AdjBalanceFeePublic = (listTemp[listTemp.Count - 1].FeePublic - listTemp[listTemp.Count - 2].FeePublic),
                                            });
                                        }
                                    }
                                }

                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(listResponse);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListFeeAdjustingJournalEntries: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region List PostageRefund
        [HttpPost]
        [Route("ListPostageRefund")]
        public ZResponse ListPostageRefund(ZReconciliationSearchPostageRefundV1Request req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var reqEMSPostageRefund = new ZReconciliationSearchPostageRefundRequest()
                {
                    ma_kh = req.CustomerCode,
                    thang_no = req.MonthYear
                };
                var resPostageRefund = EMSRepository.EMSPostageRefund(reqEMSPostageRefund);
                if (resPostageRefund.Code == "success")
                {
                    res.Code = "00";
                    res.Message = "Lấy dữ liệu thành công";
                    res.Data = resPostageRefund.Data;
                }
                else
                {
                    res.Code = resPostageRefund.Code;
                    res.Message = resPostageRefund.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListPostageRefund: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region List DebtNotices
        [HttpPost]
        [Route("ListDebtNotices")]
        public ZResponse ListDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var reqEMSDebtNotices = new ZReconciliationSearchDebtNoticesRequest()
                {
                    ma_kh = req.CustomerCode,
                    thang_no = req.MonthYear
                };
                var resDebtNotices = EMSRepository.EMSDebtNotices(reqEMSDebtNotices);
                if (resDebtNotices.Code == "success")
                {
                    res.Code = "00";
                    res.Message = "Lấy dữ liệu thành công";
                    res.Data = resDebtNotices.Data;
                }
                else
                {
                    res.Code = resDebtNotices.Code;
                    res.Message = resDebtNotices.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListDebtNotices: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("ListSummaryOfDebtNotices")]
        public ZResponse ListSummaryOfDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var reqEMSSummaryOfDebtNotices = new ZReconciliationSearchDebtNoticesRequest()
                {
                    ma_kh = req.CustomerCode,
                    thang_no = req.MonthYear
                };
                var resSummaryOfDebtNotices = EMSRepository.EMSSummaryOfDebtNotices(reqEMSSummaryOfDebtNotices);
                if (resSummaryOfDebtNotices.Code == "success")
                {
                    res.Code = "00";
                    res.Message = "Lấy dữ liệu thành công";
                    res.Data = resSummaryOfDebtNotices.Data;
                }
                else
                {
                    res.Code = resSummaryOfDebtNotices.Code;
                    res.Message = resSummaryOfDebtNotices.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReconciliationController.ListSummaryOfDebtNotices: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion
    }
}