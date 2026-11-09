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
    [RoutePrefix("DeliveryNote")]
    public class DeliveryNoteController : ApiController
    {
        [Route("List")]
        public ZResponse List(ZDeliveryNoteListRequest req)
        {
            int result = -1;
            List<ZDeliveryNoteListResponse> lst = new List<ZDeliveryNoteListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.LIST", new object[]
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
                                    lst.Add(new ZDeliveryNoteListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = Convert.ToString(dr["CODE"]),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        Status = Convert.ToString(dr["STATUS"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        CreatedName = Convert.ToString(dr["CREATED_NAME"]),
                                        Transport = Convert.ToString(dr["TRANSPORT"]),
                                    });
                                }
                            }
                            if (lst.Count > 0 )
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.List " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetByID")]
        public ZResponse GetByID(int ID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZDeliveryNoteGetByIDResponse> list = new List<ZDeliveryNoteGetByIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.GET_BY_ID", new object[]
                {
                    ID,
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
                                    list.Add(new ZDeliveryNoteGetByIDResponse()
                                    {
                                        WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        WareHouseFullAddress = Convert.ToString(dr["WAREHOUSE_FULL_ADDRESS"]),
                                        ProductName = Convert.ToString(dr["PRODUCT_NAME"]),
                                        VariantName = Convert.ToString(dr["VARIANT_NAME"]),
                                        NumberOfProduct = Convert.ToInt32(dr["NUMBER_OF_PRODUCT"]),
                                    });
                                }
                            }
                            if (list.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetDeliveryNoteByOrderID")]
        public ZResponse GetDeliveryNoteByOrderID(int OrderID, int ShopID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZDeliveryNoteByOrderIDResponse> list = new List<ZDeliveryNoteByOrderIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.GET_DE_NO_BY_ORDER_ID", new object[]
                {
                    OrderID,
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
                                while (dr.Read())
                                {
                                    list.Add(new ZDeliveryNoteByOrderIDResponse()
                                    {
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                        ProductName = Convert.ToString(dr["PRODUCT_NAME"]),
                                        ProductQuantity = Convert.ToInt32(dr["PRODUCT_QUANTITY"]),
                                        WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        WareHouseAddress = Convert.ToString(dr["WAREHOUSE_ADDRESS"]),
                                        ProvinceName = Convert.ToString(dr["PROVINCE_NAME"]),
                                        DistrictName = Convert.ToString(dr["DISTRICT_NAME"]),
                                        WardName = Convert.ToString(dr["WARD_NAME"]),
                                        WareHouseQuantity = Convert.ToInt32(dr["WAREHOUSE_QUANTITY"]),
                                        WareHouseProductID = Convert.ToInt32(dr["WAREHOUSE_PRODUCT_ID"]),
                                        VariantID = Convert.ToInt32(dr["VARIANT_ID"]),
                                        VariantName = Convert.ToString(dr["VARIANT_NAME"]),
                                    });
                                }
                            }
                            if (list.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.GetDeliveryNoteByOrderID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        //[Route("Add")]
        //public ZResponse Add(ZDeliveryNoteAddNewRequest req)
        //{
        //    int result = -1;
        //    int DeliveryNoteID = 0;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.ADD_NEW", new object[]
        //        {
        //            0,
        //            req.OrderCode,
        //            req.ShopID,
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
        //                    DeliveryNoteID = Convert.ToInt32(dbCommand.Parameters["P_OUT_ID"].Value);
        //                    foreach(var item in req.listAddDetail)
        //                    {
        //                        res = AddDetail(DeliveryNoteID, item.WareHouseProductID, item.OutputQuantity);
        //                    }
        //                    if(res.Code == "00")
        //                    {
        //                        res = AcceptOrderAndAddDeliveryNote(DeliveryNoteID, req.OrderCode, req.BeforeStatus, req.AfterStatus, req.CreatedBy);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "99";
        //                        res.Message = "Thất bại";
        //                        XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.AddDetail: ");
        //                    }
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.Add: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //public ZResponse AddDetail(int DeliveryNoteID, int WareHouseProductID, int OutputQuantity)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.ADD_DETAIL", new object[]
        //        {
        //            0,
        //            DeliveryNoteID,
        //            WareHouseProductID,
        //            OutputQuantity
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.AddDetail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //public ZResponse AcceptOrderAndAddDeliveryNote(int DeliveryNoteID, string OrderCode, string BeforeStatus, string AfterStatus, int CreatedBy)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.ACCEPT_ORD_A_ADD_DE", new object[]
        //        {
        //            0,
        //            DeliveryNoteID,
        //            OrderCode,
        //            BeforeStatus,
        //            AfterStatus,
        //            CreatedBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Đơn hàng đã được xử lý.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.AcceptOrderAndAddDeliveryNote: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        [Route("ChangeStatus")]
        public ZResponse ChangeStatus(ZOrderChangeStatusRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.CHANGE_STATUS", new object[]
                {
                    0,
                    req.ID,
                    req.BeforeStatus,
                    req.AfterStatus,
                    req.ReasonCancel,
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
                                    res.Message = "Đơn hàng đã được xử lý.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Sai trạng thái.";
                                    break;
                                case -3:
                                    res.Code = "06";
                                    res.Message = "Lưu lịch sử thất bại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.ChangeStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("CompletedStatus")]
        public ZResponse CompletedStatus(ZOrderChangeStatusRequest req)
        {
            int result = -1;
            string ProductAndVariantName = "";
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.COMPLETED_STATUS", new object[]
                {
                    0,
                    req.ID,
                    req.BeforeStatus,
                    req.AfterStatus,
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
                            res = CustomerRepository.UpdateCustomerGroup(req.ID);
                            res.Code = "00";
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Trạng thái phiếu xuất kho đã được xử lý.";
                                    break;
                                case -2:
                                    ProductAndVariantName = Convert.ToString(dbCommand.Parameters["P_OUT_PRODUCT_AND_VARIANT_NAME"].Value);
                                    res.Code = "02";
                                    res.Message = "Sản phẩm " + ProductAndVariantName + " không tồn tại trong kho, vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Số lượng trong kho không đủ.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Cập nhật nhóm khách hàng thất bại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Sai trạng thái.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Lưu lịch sử thất bại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.CompletedStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}