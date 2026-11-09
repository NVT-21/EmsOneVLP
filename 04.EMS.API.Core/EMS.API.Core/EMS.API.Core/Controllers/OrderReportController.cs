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
using System.Globalization;
using System.Web;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("OrderReport")]
    public class OrderReportController : ApiController
    {
        // GET: OrderReport
        [Route("ListOrderReport")]
        public ZResponse ListOrderReport(ZOrderReportRequest req)
        {
            List<ZOrderReportResponse> lst = new List<ZOrderReportResponse>();
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_REPORT_PKG.LIST_REPORT_ORDER", new object[]
                {
                    req.Time,
                    req.SearchKey,
                    req.PlaceOfCreation,
                    req.FromDate,
                    req.ToDate,
                    req.PartnerID,
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
                                    lst.Add(new ZOrderReportResponse()
                                    {
                                        StoreName = Convert.ToString(dr["STORE_NAME"]),
                                        CustomerCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        ConfirmDate = Convert.ToString(dr["CONFIRM_DATE"]),
                                        AreaName = Convert.ToString(dr["AREA_NAME"]),
                                        OrderMonthQuantity = Convert.ToInt32(dr["ORDER_MONTH_QUANTITY"]),
                                        OrderMonthTurnover = Convert.ToInt64(dr["ORDER_TURNOVER_MONTH"]),
                                        PartnerID = Convert.ToInt32(dr["PARTNER_ID"]),
                                        PartnerName = Convert.ToString(dr["PARTNER_NAME"])
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
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderReport.ListOrderReport " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListOrderDetailReport")]
        public ZResponse ListOrderDetailReport(ZOrderDetailReportRequest req)
        {
            List<ZOrderDetailReportResponse> lst = new List<ZOrderDetailReportResponse>();
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_REPORT_PKG.LIST_REPORT_ORDER_DETAIL", new object[]
                {
                    req.Time,
                    req.SearchKey,
                    req.PlaceOfCreation,
                    req.FromDate,
                    req.ToDate,
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
                                    lst.Add(new ZOrderDetailReportResponse()
                                    {
                                        ItemCode = Convert.ToString(dr["ITEMCODE"]),
                                        OrderCreatedDate = Convert.ToString(dr["ORDER_CREATED_DATE"]),
                                        TempTransportFee = Convert.ToInt64(dr["TEMP_TRANSPORT_FEE"]),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        CODMoney = Convert.ToInt64(dr["COD_MONEY"]),
                                        PlaceOfOrderCreation = Convert.ToString(dr["PLACE_OF_ORDER_CREATION"]) == "API Partner" ? "API Đối tác" : Convert.ToString(dr["PLACE_OF_ORDER_CREATION"]),
                                        StatusName = XData.GetNameOrderStatus(Convert.ToString(dr["STATUS"]))
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
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderReport.ListOrderDetailReport " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}