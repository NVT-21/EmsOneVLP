using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using System.Data.Common;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;
using EMS.API.Core.Controllers;
namespace EMS.API.Core.Repositories
{
    public class RequestOrderRepository
    {
        public static ZResponse ListDivisionEqually(int ShopID, int EmployeeID, int NumberOfEmployeeOrder, string FromDate, string ToDate)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.LIST_DIVISION_EQUALLY", new object[]
                {
                        0,
                        ShopID,
                        EmployeeID,
                        NumberOfEmployeeOrder,
                        FromDate,
                        ToDate
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Chia đơn thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderRepository.ListDivisionEqually: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse ListDivisionEmployee(int ShopID, int Quantity, int EmployeeID, string FromDate, string ToDate)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.LIST_DIVISION_CHECK_EMPLOYEE", new object[]
                {
                    0,
                    ShopID,
                    EmployeeID,
                    Quantity,
                    FromDate,
                    ToDate,
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
                            res.Message = "Chia đơn thành công";
                            res.Data = Convert.ToString(dbCommand.Parameters["P_OUT_COUNT_UPDATED"].Value);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderRepository.ListDivisionEmployee: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //public static ZResponse ExcelDivisionEqually(ZRequestOrderImportAddCommitRequest req, int EmployeeID, int NumberOfEmployeeOrder)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.EXCEL_DIVISION_EQUALLY", new object[]
        //        {
        //            0,
        //            req.ShopID,
        //            EmployeeID,
        //            NumberOfEmployeeOrder,
        //            req.ImportCode,
        //            req.CreatedBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Chia đơn thành công.";
        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Thất bại";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderRepository.ExcelDivisionEqually: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //public static ZResponse ExcelDivisionEmployee(ZRequestOrderImportAddCommitRequest req, int EmployeeID)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.EXCEL_DIVISION_CHECK_EMPLOYEE", new object[]
        //        {
        //            0,
        //            req.ShopID,
        //            EmployeeID,
        //            req.Quantity,
        //            req.ImportCode,
        //            req.CreatedBy,
        //            0
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Chia đơn thành công.";
        //                    res.Data = Convert.ToString(dbCommand.Parameters["P_OUT_COUNT_UPDATED"].Value);
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Dữ liệu không tồn tại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại";
        //                            break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderRepository.ExcelDivisionEmployee: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //public static ZResponse ExcelDivisionEmployee(ZRequestOrderImportAddCommitRequest req)
        //{
        //    int result = -1;
        //    int resultUpdateRow = 0;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.EXCEL_DIVISION_CHECK_EMPLOYEE", new object[]
        //        {
        //            0,
        //            req.ShopID,
        //            req.Quantity,
        //            req.ImportCode,
        //            req.CreatedBy,
        //            0
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    List<ZRequestOrderDivisionRequest> listEmployee = new List<ZRequestOrderDivisionRequest>();
        //                    resultUpdateRow = Convert.ToInt32(dbCommand.Parameters["P_OUT_COUNT_UPDATED"].Value);
        //                    var total = req.Quantity - resultUpdateRow;
        //                    if (total > 0)
        //                    {
        //                        var splitEmployeeID = req.strEmployeeID.Split(',');
        //                        int RecieverRowInt = total / (splitEmployeeID.Length); //phần nguyên
        //                        int RecieverRowRemainder = total % (splitEmployeeID.Length);//phần dư
        //                        for (var i = 0; i < splitEmployeeID.Length; i++)
        //                        {
        //                            if (RecieverRowRemainder != 0)
        //                            {
        //                                listEmployee.Add(new ZRequestOrderDivisionRequest()
        //                                {
        //                                    EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
        //                                    NumberOfEmployeeOrder = RecieverRowInt + 1
        //                                });
        //                                RecieverRowRemainder--;
        //                            }
        //                            else
        //                            {
        //                                listEmployee.Add(new ZRequestOrderDivisionRequest()
        //                                {
        //                                    EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
        //                                    NumberOfEmployeeOrder = RecieverRowInt
        //                                });
        //                            }
        //                        }
        //                        foreach (var item in listEmployee)
        //                        {
        //                            res = ExcelDivisionEqually(req, item.EmployeeID, item.NumberOfEmployeeOrder);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Chia đơn thành công.";
        //                    }

        //                    if (req.Quantity < req.TotalRowExcel)
        //                    {
        //                        res.Code = "02";
        //                        res.Message = "Số lượng đơn được chia ít hơn số dòng Excel";
        //                        //res = RequestOrderController.AddNewCommit(req);
        //                    }
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Dữ liệu không tồn tại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại";
        //                            break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderRepository.ExcelDivisionEmployee: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        public static ZResponse AcceptRequestOrder(ZOrderAddRequest req, int OrderID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.ACCEPT", new object[]
                {
                    0,
                    OrderID,
                    req.RequestOrderID,
                    req.WareHouseID,
                    req.CreatedBy,
                    req.CustomerID,
                    req.ShopID,
                    req.ShipmentDetails.ReceiverName,
                    req.ShipmentDetails.MobileNumber,
                    req.ShipmentDetails.Channel,
                    req.ShipmentDetails.Email,
                    req.ShipmentDetails.ProvinceID,
                    req.ShipmentDetails.DistrictID,
                    req.ShipmentDetails.WardID,
                    req.ShipmentDetails.Address,
                    req.Channel,
                    req.ProductSKU,
                    req.Message,
                    req.Note,
                    req.Transport,
                    req.PaymentMethod,
                    req.PaymentStatus,
                    req.TransportWeight,
                    req.TransportSize,
                    req.TransportQuantity,
                    req.TransportOrderValue,
                    req.TransportOrderName,
                    req.TransportMainService,
                    req.TransportExtraService,
                    req.isUpdatedCustomer
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thành công.";
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
                                    res.Message = "Yêu cầu đặt hàng đã được xử lý.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Cập nhật khách hàng thất bại";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Cập nhật đơn hàng thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderRepository.AcceptRequestOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse ListDivisionAgain(int ShopID, int EmployeeID, int RequestOrderID, int UpdatedBy)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.LIST_DIVISION_AGAIN", new object[]
                {
                    0,
                    ShopID,
                    EmployeeID,
                    RequestOrderID,
                    UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Chia đơn thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Yêu cầu đặt hàng đã được xử lý.";
                                    break;
                                case -99:
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderRepository.ListDivisionAgain: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

    }
}