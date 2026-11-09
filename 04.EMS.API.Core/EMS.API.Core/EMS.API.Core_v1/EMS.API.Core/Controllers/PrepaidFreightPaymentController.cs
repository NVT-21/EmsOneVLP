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
    [RoutePrefix("PrepaidFreightPayment")]
    public class PrepaidFreightPaymentController : ApiController
    {
        // GET: PrepaidFreightPayment
        [HttpPost]
        [Route("ListPrepaidFreightPayment")]
        public ZResponse ListPrepaidFreightPayment(ZPrepaidFreightPaymentRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZPrepaidFreightPaymentResponse> list = new List<ZPrepaidFreightPaymentResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PREPAID_FEE_PAYMENT_PKG.LIST", new object[]
                {
                    req.ShopID,      //P_SHOP_ID NUMBER            
                    req.FromDate,
                    req.ToDate,
                    req.EmsBarCode,
                    req.OrderCode,
                    req.Status,
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
                                    list.Add(new ZPrepaidFreightPaymentResponse()
                                    {
                                        AmndDate = dr["AMND_DATE"].ToString(),
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
                                        SumAll = Convert.ToInt64(dr["SUM_ALL"]),
                                        Note = dr["NOTE"].ToString(),
                                        IsAgree = dr["IS_AGREE"].ToString()
                                    });
                                }
                            }
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(list);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PrepaidFreightPayment.ListPrepaidFreightPayment: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}