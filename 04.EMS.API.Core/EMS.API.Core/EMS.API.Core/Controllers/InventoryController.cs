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
    [RoutePrefix("Inventory")]
    public class InventoryController : ApiController
    {
        [Route("Add")]
        public ZResponse Add(ZInventoryAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
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
                            res.Code = "00";
                            res.Message = "Thêm mới thành công.";
                            res.Data = JsonConvert.SerializeObject(new ZInventoryAddResponse()
                            {
                                ID = Convert.ToInt32(dbCommand.Parameters["P_OUT_ID"].Value)
                            });
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddV2")]
        public ZResponse AddV2(ZInventoryAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.Add(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.AddV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZInventoryEditRequest req)
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
                            res.Message = "Cập nhật thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("EditV2")]
        public ZResponse EditV2(ZInventoryEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
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
                    req.ShopID,
                    req.ListInventoryDetail,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.EditV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchList")]
        public ZResponse SearchList(ZInventorySearchListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZInventorySearchListResponse> list = new List<ZInventorySearchListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.SEARCH_BY_SHOP_ID", new object[]
                {
                    req.ShopID,     //P_SHOP_ID  NUMBER
                    req.Type,
                    req.Code,
                    req.WarehouseID,
                    req.ProviderID,
                    req.OrderCode,
                    req.StockTakeCode,
                    req.Status,
                    XCommon.FormatFromDate(req.FromDate),
                    XCommon.FormatToDate(req.ToDate),
                    req.DateType,
                    req.SearchKey,
                    0,              //P_RETURN_CODE OUT NUMBER
                    0               //P_OUT_CURSOR  OUT T_CURSOR
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
                                    list.Add(new ZInventorySearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["Code"].ToString(),
                                        Warehouse = new XWarehouseModel()
                                        {
                                            ID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                            Code = dr["WAREHOUSE_CODE"].ToString(),
                                            Name = dr["WAREHOUSE_NAME"].ToString()
                                        },
                                        Provider = new XProviderModel()
                                        {
                                            ID = Convert.ToInt32(dr["PROVIDER_ID"]),
                                            Code = dr["PROVIDER_CODE"].ToString(),
                                            Name = dr["PROVIDER_NAME"].ToString()
                                        },
                                        Order = new XOrderModel()
                                        {
                                            ID = Convert.ToInt32(dr["ORDER_ID"]),
                                            EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                            OrderCode = dr["ORDER_CODE"].ToString(),
                                            Transport = dr["TRANSPORT"].ToString(),
                                            ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        },
                                        StockTake = new XStockTakeModel()
                                        {
                                            ID = Convert.ToInt32(dr["STOCK_TAKE_ID"]),
                                            Code = dr["STOCK_TAKE_CODE"].ToString()
                                        },
                                        Status = new XStatusModel()
                                        {
                                            Code = dr["STATUS"].ToString(),
                                            Name = XData.GetNameInventoryStatus(dr["STATUS"].ToString())
                                        },
                                        TotalAmount = Convert.ToInt64(dr["TOTAL_AMOUNT"]),
                                        ImplementationDate = dr["IMPLEMENTATION_DATE"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        Reason = dr["REASON"].ToString(),
                                        CreaterName = dr["CREATER_NAME"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        UpdaterName = dr["UPDATER_NAME"].ToString(),
                                        UpdatedDate = dr["UPDATED_DATE"].ToString(),
                                        Type = dr["TYPE"].ToString()
                                    });
                                }
                            }
                            if(list.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(list);
                            }
                            else
                            {
                                res.Code = "02";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Thời gian tìm kiếm không được quá 1 tháng.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.SearchList: " + ex.Message);
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
            try
            {
                int result = -1;
                ZInventoryGetByIDResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.GET_DATA_BY_ID", new object[]
                {
                    ID,        //P_ID            NUMBER
                    ShopID,    //P_SHOP_ID       NUMBER
                    0,         //P_RETURN_CODE   OUT NUMBER
                    0          //P_OUT_CURSOR    OUT T_CURSOR
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
                                    item = new ZInventoryGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        Provider = new XProviderModel()
                                        {
                                            ID = Convert.ToInt32(dr["PROVIDER_ID"]),
                                            Code = dr["PROVIDER_CODE"].ToString(),
                                            Name = dr["PROVIDER_NAME"].ToString()
                                        },
                                        Warehouse = new XWarehouseModel()
                                        {
                                            ID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                            Code = dr["WAREHOUSE_CODE"].ToString(),
                                            Name = dr["WAREHOUSE_NAME"].ToString(),
                                            FullAddress = dr["WAREHOUSE_FULL_ADDRESS"].ToString(),
                                        },
                                        Order = new XOrderModel()
                                        {
                                            ID = Convert.ToInt32(dr["ORDER_ID"]),
                                            Code = dr["ORDER_CODE"].ToString(),
                                            Transport = dr["TRANSPORT"].ToString()
                                        },
                                        StockTake = new XStockTakeModel()
                                        {
                                            ID = Convert.ToInt32(dr["STOCK_TAKE_ID"]),
                                            Code = dr["STOCK_TAKE_CODE"].ToString(),
                                            CreaterName = dr["STOCK_TAKE_CREATER_NAME"].ToString(),
                                            CreatedDate = dr["STOCK_TAKE_CREATED_DATE"].ToString(),
                                            UpdaterName = dr["STOCK_TAKE_UPDATER_NAME"].ToString(),
                                            UpdatedDate = dr["STOCK_TAKE_UPDATED_DATE"].ToString(),
                                        },
                                        Status = new XStatusModel()
                                        {
                                            Code = dr["STATUS"].ToString(),
                                            Name = XData.GetNameInventoryStatus(dr["STATUS"].ToString())
                                        },
                                        TotalAmount = Convert.ToInt64(dr["TOTAL_AMOUNT"]),
                                        ImplementationDate = dr["IMPLEMENTATION_DATE"].ToString(),
                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        Reason = dr["REASON"].ToString(),
                                        CreaterName = dr["CREATER_NAME"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        UpdaterName = dr["UPDATER_NAME"].ToString(),
                                        UpdatedDate = dr["UPDATED_DATE"].ToString()
                                    };
                                }
                            }
                            if(item != null)
                            {
                                var resInventoryDetail = InventoryDetailRepository.GetByInventoryID(new ZInventoryDetailGetByInventoryIDRequest() { InventoryID = ID, ShopID = ShopID });
                                if (resInventoryDetail.Code == "00")
                                {
                                    item.ListInventoryDetail = JsonConvert.DeserializeObject<List<ZInventoryDetailGetByInventoryIDResponse>>(resInventoryDetail.Data);
                                }
                                else
                                {
                                    item.ListInventoryDetail = new List<ZInventoryDetailGetByInventoryIDResponse>();
                                }
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateStatus")]
        public ZResponse UpdateStatus(ZInventoryUpdateStatusRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.UPDATE_STATUS", new object[]
                {
                    0,
                    req.ID,
                    req.Status,
                    req.Discription,
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
                                    res.Message = "Phiếu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái phiếu đã thay đổi. Vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lỗi lưu lịch sử phiếu. Vui lòng thử lại hoặc liên hệ quản trị.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Phiếu không có sản phẩm. Vui lòng kiểm tra lại.";
                                    break;
                                case -90:
                                    res.Code = "90";
                                    res.Message = "Không cho phép cập nhật trạng thái này.";
                                    break;
                                case -91:
                                    res.Code = "91";
                                    res.Message = "Không xác định được loại phiếu. Vui lòng thử lại hoặc liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.UpdateStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Confirm")]
        public ZResponse Confirm(ZInventoryConfirmRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.Confirm(req);
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

        [Route("Reject")]
        public ZResponse Reject(ZInventoryRejectRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.REJECT", new object[]
                {
                    0,
                    req.ID,
                    req.Reason,
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
                            res.Message = "Từ chối thành công.";
                        }
                        else
                        {
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
                                    res.Message = "Lỗi lưu lịch sử phiếu. Vui lòng thử lại hoặc liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.Reject: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Cancel")]
        public ZResponse Cancel(ZInventoryCancelRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.CANCEL", new object[]
                {
                    0,
                    req.ID,
                    req.Reason,
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
                            res.Message = "Huỷ thành công.";
                        }
                        else
                        {
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
                                    res.Message = "Lỗi lưu lịch sử phiếu. Vui lòng thử lại hoặc liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.Cancel: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetProductVariantByID")]
        [HttpPost]
        public ZResponse GetProductVariantByID(ZInventoryGetProductVariantByIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZInventoryGetProductVariantByIDResponse> model = new List<ZInventoryGetProductVariantByIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.GET_PRODUCT_VARIANT_BY_ID", new object[]
                {
                    req.ID,
                    req.ShopID,
                    0,                    //P_RETURN_CODE   OUT NUMBER
                    0                     //P_OUT_CURSOR    OUT T_CURSOR
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
                                    model.Add(new ZInventoryGetProductVariantByIDResponse()
                                    {
                                        ProductVariantID = Convert.ToInt32(dr["PRODUCT_VARIANT_ID"]),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        ProductVariantOptions = dr["PRODUCT_VARIANT_OPTIONS"].ToString(),
                                        NumberOfInventory = Convert.ToInt32(dr["NUMBER_OF_INVENTORY"]),
                                        NumberOfDifferent = Convert.ToInt32(dr["NUMBER_OF_DIFFRENCE"])
                                    });
                                }
                            }
                            if (model.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(model);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.GetProductVariantByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("Delete")]
        public ZResponse Delete(ZInventoryDeleteRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_PKG.INVENTORY_DELETE", new object[]
                {
                    0,
                    req.ID,
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
                            res.Message = "Xoá thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Phiếu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lỗi lưu lịch sử phiếu. Vui lòng thử lại hoặc liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryController.Delete: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
