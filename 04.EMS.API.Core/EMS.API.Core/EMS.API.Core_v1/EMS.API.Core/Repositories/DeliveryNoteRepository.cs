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

namespace EMS.API.Core.Repositories
{
    public class DeliveryNoteRepository
    {
        public static ZResponse Add(ZDeliveryNoteAddNewRequest req)
        {
            int result = -1;
            int DeliveryNoteID = 0;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.OrderID,
                    req.DeliveryNoteCode,
                    req.ShopID,
                    req.WareHouseID,
                    req.CreatedBy,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            DeliveryNoteID = Convert.ToInt32(dbCommand.Parameters["P_OUT_ID"].Value);
                            foreach (var item in req.listAddDetail)
                            {
                                res = AddDetail(DeliveryNoteID, item.ProductID, item.VariantID, item.NumberOfProduct);
                            }
                            if (res.Code == "00")
                            {
                                //res = AcceptOrderAndAddDeliveryNote(DeliveryNoteID, req.OrderCode, req.BeforeStatus, req.AfterStatus, req.CreatedBy);
                                if(req.DeliveryNoteCode == null)
                                {
                                    res = UpdateAmndState(DeliveryNoteID, req.OrderID, req.CreatedBy);
                                    res.Data = DeliveryNoteID.ToString();
                                }
                            }
                            else
                            {
                                res.Code = "99";
                                res.Message = "Thất bại";
                                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.AddDetail: ");
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case 1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case 2:
                                    res.Code = "02";
                                    res.Message = "Lưu lịch sử thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse AddDetail(int DeliveryNoteID, int ProductID, int VariantID, int NumberOfProduct)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.ADD_DETAIL", new object[]
                {
                    0,
                    DeliveryNoteID,
                    ProductID,
                    VariantID,
                    NumberOfProduct
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.AddDetail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        //public static ZResponse AcceptOrderAndAddDeliveryNote(int DeliveryNoteID, string OrderCode, string BeforeStatus, string AfterStatus, int CreatedBy)
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
        public static ZResponse UpdateAmndState(int DeliveryNoteID, int OrderID, int UpdatedBy)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.UPDATE_AMND_STATE", new object[]
                {
                    0,
                    DeliveryNoteID,
                    OrderID,
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
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Lưu lịch sử thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteController.UpdateAmndState: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse GetByOrderID(int OrderID, int ShopID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZDeliveryNoteGetByOrderIDResponse> list = new List<ZDeliveryNoteGetByOrderIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DELIVERY_NOTE_PKG.GET_BY_ORDER_ID", new object[]
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
                                    list.Add(new ZDeliveryNoteGetByOrderIDResponse()
                                    {
                                        WareHouseProductID = Convert.ToInt32(dr["WAREHOUSE_PRODUCT_ID"]),
                                        VariantID = Convert.ToInt32(dr["VARIANT_ID"]),
                                        Quantity = Convert.ToInt32(dr["QUANTITY"]),
                                        WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        OutputQuantity = Convert.ToInt32(dr["OUTPUT_QUANTITY"]),
                                    });
                                }
                            }
                            if (list != null)
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DeliveryNoteRepository.GetByOrderID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}