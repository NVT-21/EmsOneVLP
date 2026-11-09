using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using EMS.API.Core.Repositories;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("StockTake")]
    public class StockTakeController : ApiController
    {
        [Route("List")]
        public ZResponse List(ZStockTakeSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZStockTakeSearchListResponse> oItem = new List<ZStockTakeSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STOCK_TAKE_PKG.LIST_STOCK_TAKE", new object[]
                {
                    req.StockTakeCode,
                    req.DateType,
                    req.FromDate,
                    req.ToDate,
                    req.Status,
                    req.WareHouseID,
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
                                    oItem.Add(new ZStockTakeSearchListResponse()
                                    {
                                        StockTakeID = Convert.ToInt32(dr["STOCK_TAKE_ID"]),
                                        StockTakeCode = Convert.ToString(dr["STOCK_TAKE_CODE"]),
                                        WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        Status = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["STATUS"]),
                                            Name = XData.GetNameStockTakeStatus(Convert.ToString(dr["STATUS"])),
                                        },
                                        TotalProduct = Convert.ToInt32(dr["TOTAL_PRODUCT"]),
                                        TotalInventory = Convert.ToInt64(dr["TOTAL_INVENTORY"]),
                                        TotalActual = Convert.ToInt64(dr["TOTAL_ACTUAL"]),
                                        TotalDifferent = Convert.ToInt64(dr["TOAL_DIFFERENT"]),
                                        TotalMoneyDifferent = Convert.ToInt64(dr["TOTAL_MONEY_DIFFERENT"]),
                                        CreatedName = Convert.ToString(dr["CREATED_NAME"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedName = Convert.ToString(dr["UPDATED_NAME"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                    });
                                }
                            }
                            if (oItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oItem);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.List: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZStockTakeAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STOCK_TAKE_PKG.ADD_STOCK_TAKE", new object[]
                {
                    0,
                    req.WareHouseID,
                    req.Note,
                    req.Status,
                    req.ShopID,
                    req.CreatedBy,
                    req.ListDetailStockTake.ToXML()
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = req.Status == "D" ? "Lưu nháp phiếu kiểm kho thành công." : "Tạo phiếu kiểm kho thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Sai trạng thái phiếu kiểm kho.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Cửa hàng không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Kho không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Biển thể không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Sản phẩm không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Sản phẩm không nằm trong kho.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZStockTakeEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STOCK_TAKE_PKG.EDIT_STOCK_TAKE", new object[]
                {
                    0,
                    req.StockTakeID,
                    req.Note,
                    req.Status,
                    req.ShopID,
                    req.UpdatedBy,
                    req.ListDetailStockTake.ToXML()
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Sửa phiếu kiểm kho thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Trạng thái phiếu kiểm kho không hợp lệ.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Trạng thái phiếu kiểm kho đã được xử lý.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Biển thể không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Sản phẩm không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Sản phẩm không nằm trong kho.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.Edit: " + ex.Message);
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
                ZStockTakeGetByIDResponse item = null;
                List<ZStockTakeDetailResponse> listItem = new List<ZStockTakeDetailResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STOCK_TAKE_PKG.GET_BY_ID", new object[]
                {
                    ID,
                    ShopID,
                    0,
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
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR_STOCK_TAKE_DETAIL"].Value)
                            {
                                while (dr.Read())
                                {
                                    listItem.Add(new ZStockTakeDetailResponse()
                                    {
                                        StockTakeDetailID = Convert.ToInt32(dr["STOCK_TAKE_DETAIL_ID"]),
                                        NumberOfInventory = Convert.ToInt32(dr["NUMBER_OF_INVENTORY"]),
                                        NumberOfActual = Convert.ToInt32(dr["NUMBER_OF_ACTUAL"]),
                                        NumberOfDifferent = Convert.ToInt32(dr["NUMBER_OF_DIFFRENCE"]),
                                        MoneyOfDifferent = Convert.ToInt64(dr["MONEY_OF_DIFFRENCE"]),
                                        Product = new ZProductResponse()
                                        {
                                            ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                            ProductName = Convert.ToString(dr["PRODUCT_NAME"]),
                                        },
                                        ProductVariant = new ZProductVariantResponse()
                                        {
                                            ProductVariantID = Convert.ToInt32(dr["PRODUCT_VARIANT_ID"]),
                                            ProductVariantOptions = Convert.ToString(dr["PRODUCT_VARIANT_OPTIONS"]),
                                            ProductVariantSKU = Convert.ToString(dr["PRODUCT_VARIANT_SKU"]),
                                            ProductVariantPrice = Convert.ToInt64(dr["PRODUCT_VARIANT_PRICE"]),
                                        },
                                    });
                                }
                            }
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR_STOCK_TAKE"].Value)
                            {
                                if (dr.Read())
                                {
                                    item = new ZStockTakeGetByIDResponse()
                                    {
                                        StockTakeID = Convert.ToInt32(dr["STOCK_TAKE_ID"]),
                                        StockTakeCode = Convert.ToString(dr["STOCK_TAKE_CODE"]),
                                        WareHouse = new ZWareHouseResponse()
                                        {
                                            WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                            WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        },
                                        Note = Convert.ToString(dr["NOTE"]),
                                        Status = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["STATUS"]),
                                            Name = XData.GetNameStockTakeStatus(Convert.ToString(dr["STATUS"])),
                                        },
                                        CreatedName = Convert.ToString(dr["CREATED_NAME"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedName = Convert.ToString(dr["UPDATED_NAME"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        ListStockTakeDetail = listItem
                                    };
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
                                res.Message = "Không tồn tại dữ liệu.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Accept")]
        public ZResponse Accept(ZStockTakeAcceptRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STOCK_TAKE_PKG.ACCEPT_STOCK_TAKE", new object[]
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
                                    res.Message = "Phiếu đã được duyệt.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Chưa hoàn thành phiếu kiểm kê.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.Accept: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Remove")]
        public ZResponse Remove(ZStockTakeRemoveRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STOCK_TAKE_PKG.REMOVE_STOCK_TAKE", new object[]
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
                                    res.Message = "Phiếu đã được duyệt.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.Remove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("Detail")]
        //public ZResponse Detail(ZStockTakeDetailRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    ZStockTakeDetailResponse oItem = null;
        //    List<ZStockTakeDetailModel> oListItem = new List<ZStockTakeDetailModel>();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STOCK_TAKE_PKG.DETAIL_STOCK_TAKE", new object[]
        //        {
        //            req.ID,
        //            req.ShopID,
        //            0,
        //            0,
        //            0
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
        //                if (result > 0)
        //                {
        //                    using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR_STOCK_TAKE_DETAIL"].Value)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            oListItem.Add(new ZStockTakeDetailModel()
        //                            {
        //                                ProductName = Convert.ToString(dr["PRODUCT_NAME"]),
        //                                ProductVariantName = Convert.ToString(dr["PRODUCT_VARIANT_NAME"]),
        //                                ProductVariantSKU = Convert.ToString(dr["PRODUCT_VARIANT_SKU"]),
        //                                NumberOfInventory = Convert.ToInt64(dr["NUMBER_OF_INVENTORY"]),
        //                                NumberOfActual = Convert.ToInt64(dr["NUMBER_OF_ACTUAL"]),
        //                                NumberOfDifferent = Convert.ToInt64(dr["NUMBER_OF_DIFFRENCE"]),
        //                                MoneyOfDifferent = Convert.ToInt64(dr["MONEY_OF_DIFFRENCE"])
        //                            });
        //                        }
        //                    }
        //                    using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR_STOCK_TAKE"].Value)
        //                    {
        //                        if (dr.Read())
        //                        {
        //                            oItem = new ZStockTakeDetailResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                Code = Convert.ToString(dr["CODE"]),
        //                                WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
        //                                Note = Convert.ToString(dr["NOTE"]),
        //                                Status = Convert.ToString(dr["STATUS"]),
        //                                CreatedName = Convert.ToString(dr["CREATED_NAME"]),
        //                                CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
        //                                UpdatedName = Convert.ToString(dr["UPDATED_NAME"]),
        //                                UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
        //                                ListStockTakeDetail = oListItem
        //                            };
        //                        }
        //                    }
        //                    if (oItem != null)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(oItem);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
        //                    }
        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Thất bại.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.Detail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("Reject")]
        public ZResponse Reject(ZStockTakeRejectRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_STOCK_TAKE_PKG.REJECT_STOCK_TAKE", new object[] {
                    0,
                    req.ID,
                    req.ShopID,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result > 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
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
                                    res.Message = "Phiếu đã được duyệt.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Phiếu đã bị từ chối.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Chưa hoàn thành phiếu.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.Reject: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}