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
    [RoutePrefix("InventoryImport")]
    public class InventoryImportController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZInventoryImportSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZInventoryImportSearchListResponse> listItem = new List<ZInventoryImportSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_IMPORT_PKG.SEARCH_LIST", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.DateType,
                    req.InventoryImportCode,
                    req.WarehouseID,
                    req.ProviderID,
                    req.Status,
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
                                    listItem.Add(new ZInventoryImportSearchListResponse()
                                    {
                                        InventoryImportID = Convert.ToInt32(dr["INVENTORY_IMPORT_ID"]),
                                        InventoryImportCode = dr["INVENTORY_IMPORT_CODE"].ToString(),
                                        WareHouse = new ZWareHouseResponse()
                                        {
                                            WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                            WareHouseCode = dr["WAREHOUSE_CODE"].ToString(),
                                            WareHouseName = dr["WAREHOUSE_NAME"].ToString()
                                        },
                                        Provider = new ZProviderResponse()
                                        {
                                            ProviderID = Convert.ToInt32(dr["PROVIDER_ID"]),
                                            ProviderCode = dr["PROVIDER_CODE"].ToString(),
                                            ProviderName = dr["PROVIDER_NAME"].ToString()
                                        },
                                        Status = new ZBaseResponse()
                                        {
                                            Code = dr["INVENTORY_IMPORT_STATUS"].ToString(),
                                            Name = XData.GetNameInventoryImportStatus(dr["INVENTORY_IMPORT_STATUS"].ToString())
                                        },
                                        TotalAmount = Convert.ToInt64(dr["TOTAL_AMOUNT"]),
                                        DateOfInventoryImport = dr["DATE_OF_INVENTORY_IMPORT"].ToString(),
                                        CreatedName = dr["CREATED_NAME"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                    });
                                }
                            }
                            if (listItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(listItem);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.SearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZInventoryImportAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_IMPORT_PKG.ADD_INVENTORY_IMPORT", new object[]
                {
                    0,
                    req.ProviderID,
                    req.WareHouseID,
                    req.DateOfInventoryImport,
                    req.ImporterName,
                    req.MobileNumber,
                    req.Note,
                    req.CreatedBy,
                    req.ShopID,
                    req.ListInventoryImportDetail.ToXML(),
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Tạo phiếu nhập kho thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Cửa hàng không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Kho không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Biến thể không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Sản phẩm không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Nhà cung cấp không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Lưu mã Pallet thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.Add: " + ex.Message);
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
                ZInventoryImportGetByIDResponse item = null;
                List<ZInventoryImportDetailGetByInventoryIDResponse> listItem = new List<ZInventoryImportDetailGetByInventoryIDResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_IMPORT_PKG.GET_BY_INVENTORY_IMPORT_ID", new object[]
                {
                    ID,         //P_ID            NUMBER
                    ShopID,     //P_SHOP_ID       NUMBER
                    0,          //P_RETURN_CODE   OUT NUMBER
                    0,          //P_OUT_CURSOR_1  OUT T_CURSOR
                    0           //P_OUT_CURSOR_2  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR_2"].Value)
                            {
                                while (dr.Read())
                                {
                                    listItem.Add(new ZInventoryImportDetailGetByInventoryIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["INVENTORY_DETAIL_ID"]),
                                        Product = new ZProductResponse()
                                        {
                                            ProductName = dr["PRODUCT_NAME"].ToString(),
                                            ProductVariant = new ZProductVariantResponse()
                                            {
                                                ProductVariantID = Convert.ToInt32(dr["PRODUCT_VARIANT_ID"]),
                                                ProductVariantSKU = dr["PRODUCT_VARIANT_SKU"].ToString(),
                                                ProductVariantOptions = dr["OPTIONS"].ToString(),
                                                Pallet = new ZPalletResponse()
                                                {
                                                    PalletCode = dr["PALLET_CODE"].ToString(),
                                                },
                                            },
                                        },
                                        Quantity = Convert.ToInt64(dr["QUANTITY"]),
                                        Price = Convert.ToInt64(dr["PRICE"]),
                                        TotalAmount = Convert.ToInt64(dr["QUANTITY"]) * Convert.ToInt64(dr["PRICE"]),
                                    });
                                }
                            }

                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR_1"].Value)
                            {
                                if (dr.Read())
                                {
                                    item = new ZInventoryImportGetByIDResponse()
                                    {
                                        InventoryID = Convert.ToInt32(dr["INVENTORY_IMPORT_ID"]),
                                        InventoryCode = dr["INVENTORY_IMPORT_CODE"].ToString(),
                                        DateOfInventoryImport = dr["DATE_OF_INVENTORY_IMPORT"].ToString(),
                                        ImporterName = dr["IMPORTER_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        Reason = dr["REASON"].ToString(),
                                        WareHouse = new ZWareHouseResponse()
                                        {
                                            WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                            WareHouseName = dr["WAREHOUSE_NAME"].ToString(),
                                        },
                                        Provider = new ZProviderResponse()
                                        {
                                            ProviderID = Convert.ToInt32(dr["PROVIDER_ID"]),
                                            ProviderName = dr["PROVIDER_NAME"].ToString(),
                                        },
                                        Status = new ZBaseResponse()
                                        {
                                            Code = dr["STATUS"].ToString(),
                                            Name = XData.GetNameInventoryImportStatus(dr["STATUS"].ToString()),
                                        }
                                    };
                                    item.TotalAmount = listItem.Sum(m => m.TotalAmount);
                                    item.ListInventoryImportDetail = listItem;
                                }
                            }

                            if (item != null)
                            {
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZInventoryImportEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_IMPORT_PKG.EDIT_INVENTORY_IMPORT", new object[]
                {
                    0,
                    req.InventoryImportID,
                    req.ProviderID,
                    req.DateOfInventoryImport,
                    req.ImporterName,
                    req.MobileNumber,
                    req.Note,
                    req.UpdatedBy,
                    req.ShopID,
                    req.ListInventoryImportDetail.ToXML(),
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Sửa phiếu nhập kho thành công.";
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
                                    res.Message = "Trạng thái phiếu nhập kho không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Nhà cung cấp không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Biến thể không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Sản phẩm không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Lưu mã Pallet thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Accept")]
        public ZResponse Accept(ZInventoryImportAcceptRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_IMPORT_PKG.ACCEPT_STATUS", new object[]
                {
                    0,
                    req.InventoryImportID,
                    req.ShopID,
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
                            res.Message = "Xác nhận phiếu nhập kho thành công.";
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
                                    res.Message = "Trạng thái phiếu nhập kho không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Thay đổi số lượng của Pallet thất bại.";
                                    break;
                                case -4:
                                    res.Code = "04";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.Accept: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Reject")]
        public ZResponse Reject(ZInventoryImportRejectRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_IMPORT_PKG.REJECT_STATUS", new object[]
                {
                    0,
                    req.InventoryImportID,
                    req.Reason,
                    req.ShopID,
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
                            res.Message = "Từ chối phiếu nhập kho thành công.";
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
                                    res.Message = "Trạng thái phiếu nhập kho không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.Reject: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Remove")]
        public ZResponse Remove(ZInventoryImportRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_IMPORT_PKG.REMOVE_STATUS", new object[]
                {
                    0,
                    req.InventoryImportID,
                    req.ShopID,
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
                            res.Message = "Xóa phiếu nhập kho thành công.";
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
                                    res.Message = "Trạng thái phiếu nhập kho không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.Remove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}