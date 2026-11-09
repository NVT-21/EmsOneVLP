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
    public class InventoryRepository
    {
        public static ZResponse Add(ZInventoryAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.WarehouseID,
                    req.ProviderID,
                    req.OrderID,
                    req.StockTakeID,
                    req.Type,
                    req.ImplementationDate,
                    req.PaymentStatus,
                    req.Note,
                    req.CreatedBy,
                    req.ShopID,
                    req.ReceiverName,
                    req.MobileNumber,
                    req.InventoryCode,
                    0                      //P_OUT_ID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            var InventoryID = Convert.ToInt32(dbCommand.Parameters["P_OUT_ID"].Value);
                            var CheckInventoryDetailSuccess = 0;
                            foreach (var item in req.ListInventoryDetail)
                            {
                                ZInventoryDetailAddRequest reqInventoryDetailAdd = new ZInventoryDetailAddRequest()
                                {
                                    ShopID = req.ShopID,
                                    InventoryID = InventoryID,
                                    ProductVariantID = item.ProductVariantID,
                                    Quantity = item.Quantity,
                                    Price = item.Price,
                                    CreatedBy = req.CreatedBy,
                                    NumberOfDateInWareHouse = item.NumberOfDateInWareHouse,
                                    TermInWareHouse = item.TermInWareHouse
                                };
                                var resInventoryDetail = InventoryDetailRepository.Add(reqInventoryDetailAdd);
                                if (resInventoryDetail.Code == "00")
                                {
                                    CheckInventoryDetailSuccess++;
                                }
                                else
                                {
                                    res = UpdateInActiveAmndState(InventoryID, req.ShopID);
                                    res.Code = resInventoryDetail.Code;
                                    res.Message = resInventoryDetail.Message;
                                    break;
                                }
                            }
                            if (CheckInventoryDetailSuccess == req.ListInventoryDetail.Count)
                            {
                                res.Code = "00";
                                res.Message = "Thành công";
                                res.Data = InventoryID.ToString();
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Kho hàng không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Nhà cung cấp không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Đơn hàng không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Phiếu kiểm kho không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Lưu lịch sử thất bại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Nhân viên không tồn tại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Mã phiếu xuất kho đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse UpdateInActiveAmndState(int ID, int ShopID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.UPDATE_INACTIVE_AMND_STATE", new object[]
                {
                    0,
                    ID,
                    ShopID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res = InventoryDetailRepository.UpdateInActiveAmndState(ID, ShopID);
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Phiếu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.UpdateInActiveAmndState: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse Edit(ZInventoryEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.EDIT", new object[]
                {
                    0,
                    req.ID,
                    req.WarehouseID,
                    req.ProviderID,
                    req.OrderID,
                    req.StockTakeID,
                    req.ImplementationDate,
                    req.ReceiverName,
                    req.MobileNumber,
                    req.Note,
                    req.UpdatedBy,
                    req.ShopID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
                            res.Data = req.ID.ToString();
                            //res = InventoryDetailRepository.UpdateInActiveAmndState(req.ID, req.ShopID);
                            //if(res.Code == "00")
                            //{
                            //    foreach (var item in req.ListInventoryDetail)
                            //    {
                            //        ZInventoryDetailAddRequest reqInventoryDetailAdd = new ZInventoryDetailAddRequest()
                            //        {
                            //            ShopID = req.ShopID,
                            //            InventoryID = req.ID,
                            //            ProductVariantID = item.ProductVariantID,
                            //            Quantity = item.Quantity,
                            //            CreatedBy = req.UpdatedBy
                            //        };
                            //        res = InventoryDetailRepository.Add(reqInventoryDetailAdd);
                            //        if (res.Code != "00")
                            //        {
                            //            XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryDetailRepository.Add: " + res.Message);
                            //            break;
                            //        }
                            //    }
                            //    if (res.Code == "00")
                            //    {
                            //        res.Code = "00";
                            //        res.Message = "Thành công";
                            //        res.Data = req.ID.ToString();
                            //    }
                            //    else
                            //    {
                            //        res.Code = "99";
                            //        res.Message = "Thất bại";
                            //    }
                            //}
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tìm thấy thông tin.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Kho hàng không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Trạng thái phiếu đã thay đổi. Vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Nhà cung cấp không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Đơn hàng không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Phiếu kiểm kho không tồn tại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Lưu lịch sử thất bại.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Lỗi cập nhật số lượng tồn kho.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse Confirm(ZInventoryConfirmRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.CONFIRM", new object[]
                {
                    0,
                    req.ID,
                    req.ShopID,
                    req.UpdatedBy,
                    ""
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Xác nhận thành công.";
                        }
                        else
                        {
                            var ProductAndVariantName = Convert.ToString(dbCommand.Parameters["P_OUT_PRODUCT_AND_VARIANT_NAME"].Value);
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Phiếu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái phiếu đã thay đổi. Vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Phiếu không có sản phẩm. Vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Không xác định được loại phiếu. Vui lòng thử lại hoặc liên hệ quản trị.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Lỗi cập nhật số lượng. Vui lòng thử lại hoặc liên hệ quản trị.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Lỗi lưu lịch sử phiếu. Vui lòng thử lại hoặc liên hệ quản trị.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Số lượng trong kho không đủ.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Sản phẩm " + ProductAndVariantName + " không tồn tại trong kho, vui lòng kiểm tra lại.";
                                    break;
                                case -9:
                                    res.Code = "09";
                                    res.Message = "Lỗi cập nhật tồn kho hiện tại của sản phẩm. Vui lòng thử lại hoặc liên hệ quản trị.";
                                    break;
                                case -10:
                                    res.Code = "08";
                                    res.Message = "Sản phẩm " + ProductAndVariantName + " trong kho đã hết, vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.Confirm: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}