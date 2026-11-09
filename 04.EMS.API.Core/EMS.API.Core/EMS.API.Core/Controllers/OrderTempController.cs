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

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("OrderTemp")]
    public class OrderTempController : ApiController
    {
        [Route("AddTemp")]
        public ZResponse AddTemp(ZOrderImportAddTempRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_TEMP_PKG.ADD_TEMP", new object[]
                {
                    0,
                    req.ImportCode,
                    req.Product,
                    req.FullName,
                    req.MobileNumber,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.Channel,
                    req.Message,
                    req.Note,
                    req.TotalCOD,
                    req.Transport,
                    req.OrderCode == null ? "" : req.OrderCode.ToUpper(),
                    req.TransportOrderName,
                    req.CollectionType,
                    req.TransportMainService,
                    req.TransportExtraService,
                    req.TransportLength,
                    req.TransportWidth,
                    req.TransportHeight,
                    req.TransportWeight,
                    req.NumberOfProduct == 0 ? 1 : req.NumberOfProduct,
                    req.OrderValue,
                    req.TransportPayer
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
                                    res.Message = "Không tồn tại file excel. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã đơn hàng đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mã đơn hàng đã tồn tại trong file Excel";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderTempController.AddTemp: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddCommit")]
        public ZResponse AddCommit(ZOrderImportAddCommitRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_TEMP_PKG.ADD_COMMIT", new object[]
                {
                    0,
                    req.ImportCode,
                    req.ShopID,
                    req.CreatedBy
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
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Thêm chi tiết đơn hàng thất bại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Kho mặc định không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Kho mặc định đã bị khoá.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Vui lòng cập nhật địa chỉ lấy hàng.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Lỗi tính phí vận chuyển.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Mã đơn hàng đã tồn tại.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderTempController.AddCommit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}