using EMS.API.PayPost.Core.Models;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web.Http;

namespace EMS.API.PayPost.Core.Controllers
{
    [RoutePrefix("CODMoney")]
    public class CODMoneyController : ApiController
    {
        #region CODMoney

        #region GetItemInfoControlFreigh
        [HttpPost]
        [Route("GetItemInfoControlFreigh")]
        public ZResponse GetItemInfoControlFreigh(ZPayPostControlFreighGetItemInfoRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            var list = new List<PayPostControlFreighItem>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ONE_COD_PKG.GET_DATA_FOR_CONTROL_FREIGHT", new object[]
                {
                        req.CustomerCode,    // P_CUSTOMER_CODE IN  VARCHAR2       
                        req.FromDate,        // P_FROM_DATE     IN  NUMBER
                        req.ToDate,          // P_TO_DATE       IN  NUMBER,
                        0,                   // P_RETURN_CODE OUT NUMBER
                        0                    // P_OUT_CURSOR  OUT T_CURSOR
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
                                    list.Add(new PayPostControlFreighItem()
                                    {
                                        ReturnID = Convert.ToInt32(dr["ID"]),
                                        ListCode = dr["CODE"].ToString(),
                                        EmsBarCode = dr["LADING_CODE"].ToString(),
                                        CustomerCode = dr["CUSTOMER_CODE"].ToString(),
                                        CODMoney = Convert.ToInt64(dr["AMOUNT"]),
                                        CODTransType = Convert.ToInt32(dr["COD_TRANS_TYPE"]),
                                        UpdateTime = dr["UPDATE_DATE"].ToString(),
                                        IssueDate = dr["ISSUE_DATE"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CODMoneyController.GetItemInfoControlFreigh: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region UpdateControlFreigh
        [HttpPost]
        [Route("UpdateControlFreigh")]
        public ZResponse UpdateControlFreigh(ZPayPostControlFreighReturnResultRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ONE_COD_PKG.UPDATE_CONTROL_FREIGHT", new object[]
                {
                        0,
                        req.ReturnID,
                        req.IsAgree,
                        req.ReasonReject
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Trả kết quả đối soát thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CODMoneyController.UpdateControlFreigh: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #endregion
        [HttpPost]
        [Route("EditCOD")]
        public ZResponse EditCOD(EditCODRequest req)
        {
            string[] strItem = null;
            ZResponse res = new ZResponse();
            long lngResult;
            string strResult = string.Empty;
            //XHelper.LogToFile(LogFileType.MESSAGE, "EditCOD: \n" + strInput);
            try
            {
                DbCommand storedProcCommand = XHelper.Database.GetStoredProcCommand("vnp.EMS_ONE_COD_PKG.EDIT_COD_DETAIL_EMS",
                    new object[]
                        {
                            //P_PARCEL_CODE        VARCHAR2,
                            //P_TRANS_AMOUNT       NUMBER,
                            //P_EDIT_PO_CODE       VARCHAR2

                            0,
                            req.ParcelCode,
                            req.Amount,
                            req.PoCode
                        });


                XHelper.ExecuteNonQuery(storedProcCommand);
                lngResult = long.Parse(storedProcCommand.Parameters[0].Value.ToString());
                if (lngResult > 0)
                {
                    res.Code = "00";
                    res.Message = "Cập nhật dữ liệu thành công";
                }
                //strResult = "00|Cập nhật dữ liệu thành công|" + storedProcCommand.Parameters[0].Value.ToString();
                else
                    switch (lngResult)
                    {
                        case -1:
                            //strResult = lngResult.ToString() + "|Không tìm thấy thông tin đơn hàng trong cơ sở dữ liệu";
                            res.Code = "01";
                            res.Message = "Không tìm thấy thông tin đơn hàng trong cơ sở dữ liệu";
                            break;
                        case -2:
                            //strResult = lngResult.ToString() + "|Không tìm thấy mã tỉnh trả";
                            res.Code = "02";
                            res.Message = "Không tìm thấy mã tỉnh trả";
                            break;
                        case -3:
                            //strResult = lngResult.ToString() + "|Yêu cầu nhờ thu đã được phát hàng, không cho phép sửa thông tin";
                            res.Code = "03";
                            res.Message = "Yêu cầu nhờ thu đã được phát hàng, không cho phép sửa thông tin";
                            break;
                        case -4:
                            //strResult = lngResult.ToString() + "|Yêu cầu nhờ thu đã được thu tiền, không cho phép sửa thông tin";
                            res.Code = "04";
                            res.Message = "Yêu cầu nhờ thu đã được thu tiền, không cho phép sửa thông tin";
                            break;
                        case -5:
                            //strResult = lngResult.ToString() + "|Yêu cầu nhờ thu đã được trả tiền, không cho phép sửa thông tin";
                            res.Code = "05";
                            res.Message = "Yêu cầu nhờ thu đã được trả tiền, không cho phép sửa thông tin";
                            break;
                        case -6:
                            //strResult = lngResult.ToString() + "|Yêu cầu nhờ thu đã được chuyển hoàn, không cho phép sửa thông tin";
                            res.Code = "06";
                            res.Message = "Yêu cầu nhờ thu đã được chuyển hoàn, không cho phép sửa thông tin";
                            break;
                        case -7:
                            //strResult = lngResult.ToString() + "|Yêu cầu nhờ thu đã được hủy, không cho phép sửa thông tin";
                            res.Code = "07";
                            res.Message = "Yêu cầu nhờ thu đã được hủy, không cho phép sửa thông tin";
                            break;
                        case -8:
                            //strResult = lngResult.ToString() + "|Yêu cầu nhờ thu có trạng thái không xác định, không thể tiến hành sửa thông tin";
                            res.Code = "08";
                            res.Message = "Yêu cầu nhờ thu có trạng thái không xác định, không thể tiến hành sửa thông tin";
                            break;
                        case -9:
                            //strResult = lngResult.ToString() + "|Trùng số hiệu bưu gửi";
                            res.Code = "09";
                            res.Message = "Trùng số hiệu bưu gửi";
                            break;
                        case -10:
                            //strResult = lngResult.ToString() + "|Không phải là bưu cục phát hành, không có quyền sửa";
                            res.Code = "10";
                            res.Message = "Không phải là bưu cục phát hành, không có quyền sửa";
                            break;
                        case -11:
                            //strResult = lngResult.ToString() + "|Trùng số lô/bảng kê";
                            res.Code = "11";
                            res.Message = "Trùng số lô/bảng kê";
                            break;
                        case -96:
                            //strResult = lngResult.ToString() + "|User thực hiện không đúng";
                            res.Code = "96";
                            res.Message = "User thực hiện không đúng";
                            break;
                        case -95:
                            //strResult = lngResult.ToString() + "|Bưu cục thực hiện không hợp lệ";
                            res.Code = "95";
                            res.Message = "Bưu cục thực hiện không hợp lệ";
                            break;
                        default:
                            //strResult = lngResult.ToString() + "|Lỗi hệ thống, vui lòng liên hệ với quản trị";
                            res.Code = "99";
                            res.Message = "Lỗi hệ thống, vui lòng liên hệ với quản trị";
                            break;
                    }
            }
            catch (System.Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CODData.EDIT_CPPA_DETAIL: \n" + ex.ToString());
                //strResult = "-99|Lỗi hệ thống, vui lòng liên hệ với quản trị\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ với quản trị";
            }
            return res;
        }
    }
}