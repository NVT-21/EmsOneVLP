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
using System.Web;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("RequestOrder")]
    public class RequestOrderController : ApiController
    {
        [Route("ListRequestOrder")]
        public ZResponse ListRequestOrder(ZRequestOrderListRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZRequestOrderListResponse> list = new List<ZRequestOrderListResponse>();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.LIST", new object[]
                {
                    req.ShopID,            //P_SHOP_ID        NUMBER
                    req.RecieverID,        //P_RECIEVER_ID     NUMBER      
                    req.StoreOwner,        //P_GROUP_PERMISSION_ID VARCHAR
                    req.FromDate,          //P_FROM_DATE    VARCHAR
                    req.ToDate,            //P_TO_DATE    VARCHAR
                    req.Status,            //P_STATUS    VARCHAR
                    req.ProcessDivision,   //P_PROCESS_DIVISION VARCHAR
                    req.MobileNumber,      //P_MOBILE_NUMBER    VARCHAR,
                    0,                     //P_RETURN_CODE OUT NUMBER
                    0,                     //P_OUT_CURSOR  OUT T_CURSOR
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
                                    list.Add(new ZRequestOrderListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        Channel = Convert.ToString(dr["CHANNEL"]),
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        //ProductName = ProductVariantRepository.GetProductNameByProductSKU(Convert.ToString(dr["PRODUCT"]), req.ShopID),
                                        Product = Convert.ToString(dr["PRODUCT_NAME"]),
                                        Note = Convert.ToString(dr["NOTE"]),
                                        RecieverID = Convert.ToInt32(dr["RECEIVER_ID"]),
                                        RecieverName = Convert.ToString(dr["RECEIVER_NAME"]),
                                        ProcessDivisionRequest = Convert.ToString(dr["PROCESS_DIVISION_REQUEST"]),
                                        Status = Convert.ToString(dr["STATUS"]),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        Reason = Convert.ToString(dr["REASON"])
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
                                res.Message = "Dữ liệu không tồn tại.";
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
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.ListRequestOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int ID, int ShopID)
        {
            ZResponse res = new ZResponse();
            ZRequestOrderGetByIDResponse model = null;
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.GET_BY_ID", new object[]
                {
                    ID,
                    ShopID,
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
                                if (dr.Read())
                                {
                                    model = new ZRequestOrderGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        Channel = Convert.ToString(dr["CHANNEL"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        Product = Convert.ToString(dr["PRODUCT"]),
                                        Note = Convert.ToString(dr["NOTE"])
                                    };
                                }
                            }
                            if (model != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại";
                                res.Data = null;
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -2:
                                    res.Code = "02";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("AddNewTemp")]
        //public ZResponse AddNewTemp(ZRequestOrderImportAddTempRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.ADD_NEW_TEMP", new object[]
        //        {
        //            0,
        //            req.ImportCode,
        //            req.FullName,
        //            req.MobileNumber,
        //            req.Email,
        //            req.Channel,
        //            req.ProvinceID,
        //            req.DistrictID,
        //            req.WardID,
        //            req.Address,
        //            req.Product,
        //            req.Note
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thêm mới thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Số điện thoại không được để trống.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.AddNewTemp: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[Route("AddNewCommit")]
        //public ZResponse AddNewCommit(ZRequestOrderImportAddCommitRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.ADD_NEW_COMMIT", new object[]
        //        {
        //            0,
        //            req.ImportCode,
        //            req.ShopID,
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
        //                    res.Message = "Thêm mới thành công.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.AddNewCommit: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("AddNew")]
        public ZResponse AddNew(ZRequestOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.FullName,
                    req.MobileNumber,
                    req.ProductSKU,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.Channel,
                    req.Note,
                    req.ShopID,
                    req.CreatedBy,
                    XHelper.DeviceName
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            res.Code = "01";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.AddNew: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddNewV2")]
        public ZResponse AddNewV2(ZRequestOrderAddV2Request req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.ADD_NEW_V2", new object[]
                {
                    0,
                    req.FullName,
                    req.MobileNumber,
                    req.ProductSKU,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.Channel,
                    req.Note,
                    req.isUpdatedCustomer,
                    req.UserChannelID,
                    req.ShopID,
                    req.CreatedBy,
                    XHelper.DeviceName
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            res.Code = "01";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.AddNewV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListDivision")]
        public ZResponse ListDivision(ZRequestOrderDivisionRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZRequestOrderDivisionRequest> listEmployee = new List<ZRequestOrderDivisionRequest>();
            var splitEmployeeID = req.strEmployeeID.Split(',');
            var RequestOrderUpdated = 0;
            if (req.Criteria == "DivisionEqually")
            {
                int RecieverRowInt = req.Quantity / (splitEmployeeID.Length); //phần nguyên
                int RecieverRowRemainder = req.Quantity % (splitEmployeeID.Length);//phần dư
                for (var i = 0; i < splitEmployeeID.Length; i++)
                {
                    if (RecieverRowRemainder > 0)
                    {
                        listEmployee.Add(new ZRequestOrderDivisionRequest()
                        {
                            EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
                            NumberOfEmployeeOrder = RecieverRowInt + 1
                        });
                        RecieverRowRemainder--;
                    }
                    else
                    {
                        listEmployee.Add(new ZRequestOrderDivisionRequest()
                        {
                            EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
                            NumberOfEmployeeOrder = RecieverRowInt
                        });
                    }
                }
                foreach (var item in listEmployee)
                {
                    res = RequestOrderRepository.ListDivisionEqually(req.ShopID, item.EmployeeID, item.NumberOfEmployeeOrder, req.FromDate, req.ToDate);
                }
            }
            else
            {
                for (var i = 0; i < splitEmployeeID.Length; i++)
                {
                    res = RequestOrderRepository.ListDivisionEmployee(req.ShopID, req.Quantity, Convert.ToInt32(splitEmployeeID[i]), req.FromDate, req.ToDate);
                    if (res.Code == "00")
                    {
                        RequestOrderUpdated += Convert.ToInt32(res.Data);
                    }
                }
                var TotalUpdated = req.Quantity - RequestOrderUpdated;
                if (TotalUpdated > 0)
                {
                    int RecieverRowInt = TotalUpdated / (splitEmployeeID.Length); //phần nguyên
                    int RecieverRowRemainder = TotalUpdated % (splitEmployeeID.Length);//phần dư
                    for (var i = 0; i < splitEmployeeID.Length; i++)
                    {
                        if (RecieverRowRemainder > 0)
                        {
                            listEmployee.Add(new ZRequestOrderDivisionRequest()
                            {
                                EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
                                NumberOfEmployeeOrder = RecieverRowInt + 1
                            });
                            RecieverRowRemainder--;
                        }
                        else
                        {
                            listEmployee.Add(new ZRequestOrderDivisionRequest()
                            {
                                EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
                                NumberOfEmployeeOrder = RecieverRowInt
                            });
                        }
                    }
                    foreach (var item in listEmployee)
                    {
                        res = RequestOrderRepository.ListDivisionEqually(req.ShopID, item.EmployeeID, item.NumberOfEmployeeOrder, req.FromDate, req.ToDate);
                    }
                }
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("ExcelDivision")]
        //public ZResponse ExcelDivision(ZRequestOrderImportAddCommitRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    List<ZRequestOrderImportAddCommitRequest> listEmployee = new List<ZRequestOrderImportAddCommitRequest>();
        //    var splitEmployeeID = req.strEmployeeID.Split(',');
        //    var RequestOrderUpdated = 0;
        //    if (req.Criteria == "DivisionEqually")
        //    {
        //        int RecieverRowInt = req.Quantity / (splitEmployeeID.Length); //phần nguyên
        //        int RecieverRowRemainder = req.Quantity % (splitEmployeeID.Length);//phần dư
        //        for (var i = 0; i < splitEmployeeID.Length; i++)
        //        {
        //            if (RecieverRowRemainder > 0)
        //            {
        //                listEmployee.Add(new ZRequestOrderImportAddCommitRequest()
        //                {
        //                    EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
        //                    NumberOfEmployeeOrder = RecieverRowInt + 1
        //                });
        //                RecieverRowRemainder--;
        //            }
        //            else
        //            {
        //                listEmployee.Add(new ZRequestOrderImportAddCommitRequest()
        //                {
        //                    EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
        //                    NumberOfEmployeeOrder = RecieverRowInt
        //                });
        //            }
        //        }

        //        foreach (var item in listEmployee)
        //        {
        //            res = RequestOrderRepository.ExcelDivisionEqually(req, item.EmployeeID, item.NumberOfEmployeeOrder);
        //        }
        //    }
        //    else
        //    {
        //        for(var i = 0; i < splitEmployeeID.Length; i++)
        //        {
        //            res = RequestOrderRepository.ExcelDivisionEmployee(req, Convert.ToInt32(splitEmployeeID[i]));
        //            if(res.Code == "00")
        //            {
        //                RequestOrderUpdated += Convert.ToInt32(res.Data);
        //            }
        //        }
        //        var TotalUpdated = req.Quantity - RequestOrderUpdated;
        //        if (TotalUpdated > 0)
        //        {
        //            int RecieverRowInt = TotalUpdated / (splitEmployeeID.Length); //phần nguyên
        //            int RecieverRowRemainder = TotalUpdated % (splitEmployeeID.Length);//phần dư
        //            for (var i = 0; i < splitEmployeeID.Length; i++)
        //            {
        //                if (RecieverRowRemainder > 0)
        //                {
        //                    listEmployee.Add(new ZRequestOrderImportAddCommitRequest()
        //                    {
        //                        EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
        //                        NumberOfEmployeeOrder = RecieverRowInt + 1
        //                    });
        //                    RecieverRowRemainder--;
        //                }
        //                else
        //                {
        //                    listEmployee.Add(new ZRequestOrderImportAddCommitRequest()
        //                    {
        //                        EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
        //                        NumberOfEmployeeOrder = RecieverRowInt
        //                    });
        //                }
        //            }
        //            foreach (var item in listEmployee)
        //            {
        //                res = RequestOrderRepository.ExcelDivisionEqually(req, item.EmployeeID, item.NumberOfEmployeeOrder);
        //            }
        //        }
        //    }
        //    if (req.Quantity < req.TotalRowExcel)
        //    {
        //        res = AddNewCommit(req);
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("ChangeStatus")]
        public ZResponse ChangeStatus(ZRequestOrderRejectRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.CHANGE_STATUS", new object[]
                {
                    0,
                    req.ID,
                    req.ShopID,
                    req.Status,
                    req.UpdatedBy,
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
                                    res.Message = "Yêu cầu đặt hàng đã được xử lý.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.ChangeStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeReason")]
        public ZResponse ChangeReason(ZRequestOrderRejectRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.CHANGE_REASON", new object[]
                {
                    0,
                    req.ID,
                    req.ShopID,
                    req.Status,
                    req.UpdatedBy,
                    req.DifferentStatus,
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
                                    res.Message = "Yêu cầu đơn hàng đã được tạo đơn hàng.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Yêu cầu đơn hàng đã được xử lý.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.ChangeReason: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListDivisionAgain")]
        public ZResponse ListDivisionAgain(ZRequestOrderDivisionAgainRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZRequestOrderDivisionAgainRequest> listEmployee = new List<ZRequestOrderDivisionAgainRequest>();
            int RecieverRowInt = req.arrRequestOrderID.Length / req.arrEmployeeID.Length; //phần nguyên
            int RecieverRowRemainder = req.arrRequestOrderID.Length % req.arrEmployeeID.Length;//phần dư
            for (var i = 0; i < req.arrEmployeeID.Length; i++)
            {
                if (RecieverRowRemainder != 0)
                {
                    listEmployee.Add(new ZRequestOrderDivisionAgainRequest()
                    {
                        EmployeeID = Convert.ToInt32(req.arrEmployeeID[i]),
                        NumberOfEmployeeOrder = RecieverRowInt + 1
                    });
                    RecieverRowRemainder--;
                }
                else
                {
                    listEmployee.Add(new ZRequestOrderDivisionAgainRequest()
                    {
                        EmployeeID = Convert.ToInt32(req.arrEmployeeID[i]),
                        NumberOfEmployeeOrder = RecieverRowInt
                    });
                }
            }
            foreach (var item in listEmployee)
            {
                for (var i = 0; i < item.NumberOfEmployeeOrder; i++)
                {
                    res = RequestOrderRepository.ListDivisionAgain(req.ShopID, item.EmployeeID, req.arrRequestOrderID[0], req.UpdatedBy);
                    if (res.Code == "00")
                    {
                        List<int> list = new List<int>(req.arrRequestOrderID);
                        list.RemoveAt(0);
                        req.arrRequestOrderID = list.ToArray();
                    }
                    else
                    {
                        return res;
                    }
                }
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Update")]
        public ZResponse Update(ZRequestOrderUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_PKG.REQUEST_ORDER_UPDATE", new object[]
                {
                    0,
                    req.ID,
                    req.FullName,
                    req.MobileNumber,
                    req.ProductSKU,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.Channel,
                    req.Note,
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
                                    res.Message = "Yêu cầu đặt hàng đã được tạo đơn hàng.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Yêu cầu đặt hàng khác người xử lý.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.Update: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}