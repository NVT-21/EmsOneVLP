using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace EMS.API.Core.Repositories
{
    public class InventoryDetailRepository
    {
        public static ZResponse Add(ZInventoryDetailAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_DETAIL_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.ShopID,
                    req.InventoryID,
                    req.ProductVariantID,
                    req.Quantity,
                    req.Price,
                    req.CreatedBy,
                    req.NumberOfDateInWareHouse,
                    req.TermInWareHouse
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
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Phiếu nhập kho không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái phiếu nhập đã thay đổi. Vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Sản phẩm đã tồn tại. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryDetailController.AddNew: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse UpdateInActiveAmndState(int InventoryID, int ShopID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_DETAIL_PKG.UPDATE_INACTIVE_AMND_STATE", new object[]
                {
                    0,
                    InventoryID,
                    ShopID,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryDetailRepository.UpdateInActiveAmndState: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse GetByInventoryID(ZInventoryDetailGetByInventoryIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZInventoryDetailGetByInventoryIDResponse> list = new List<ZInventoryDetailGetByInventoryIDResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_DETAIL_PKG.GET_DATA_BY_INVENTORY_ID", new object[]
                {
                    req.InventoryID,    //P_INVENTORY_ID NUMBER
                    req.ShopID,         //P_SHOP_ID      NUMBER
                    0,                  //P_RETURN_CODE  OUT NUMBER
                    0                   //P_OUT_CURSOR   OUT T_CURSOR
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
                                    list.Add(new ZInventoryDetailGetByInventoryIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        ProductVariant = new XProductVariantModel()
                                        {
                                            ID = Convert.ToInt32(dr["PRODUCT_VARIANT_ID"]),
                                            ProductName = dr["PRODUCT_NAME"].ToString(),
                                            Options = dr["OPTIONS"].ToString()
                                        },
                                        Quantity = Convert.ToInt64(dr["QUANTITY"]),
                                        Price = Convert.ToInt64(dr["PRICE"]),
                                        WarehouseQuantity = Convert.ToInt64(dr["WAREHOUSE_QUANTITY"]),
                                        StockTakeQuantity = Convert.ToInt64(dr["STOCK_TAKE_QUANTITY"]),
                                        InventoryQuantity = Convert.ToInt64(dr["INVENTORY_QUANTITY"])
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
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryDetailRepository.GetByInventoryID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}