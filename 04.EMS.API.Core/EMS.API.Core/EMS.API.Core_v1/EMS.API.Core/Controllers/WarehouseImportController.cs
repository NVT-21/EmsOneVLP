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

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("WarehouseImport")]
    public class WarehouseImportController : ApiController
    {
        [Route("Add")]
        public ZResponse Add(ZWarehouseImportAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            int ID = 0;
            ZWarehouseImportAddResponse model = new ZWarehouseImportAddResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_IMPORT_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.ProviderID,
                    req.DateImport,
                    req.WarehouseID,
                    req.Note,
                    req.CreatedBy,
                    req.ShopID,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            ID = Convert.ToInt32(dbCommand.Parameters["P_OUT_ID"].Value);
                            res.Code = "00";
                            res.Message = "Thêm mới thành công.";
                            res.Data = JsonConvert.SerializeObject(new ZWarehouseImportAddResponse()
                            {
                                ID = ID
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("AddDetail")]
        public ZResponse AddDetail(ZWarehouseImportAddDetailRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_IMPORT_PKG.ADD_DETAIL", new object[]
                {
                    0,
                    req.ShopID,
                    req.WarehouseImportID,
                    req.ProductVariantID,
                    req.Quantity,
                    req.Price,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetDataByShopID")]
        [HttpPost]
        public ZResponse GetDataByShopID(ZWarehouseImportGetDataByShopIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZWarehouseImportGetDataByShopIDResponse> list = new List<ZWarehouseImportGetDataByShopIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_IMPORT_PKG.GET_DATA_BY_SHOP_ID", new object[]
                {
                    req.ShopID,     //P_SHOP_ID  NUMBER
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
                                    list.Add(new ZWarehouseImportGetDataByShopIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["Code"].ToString(),
                                        ProviderID = Convert.ToInt32(dr["PROVIDER_ID"]),
                                        ProviderName = dr["PROVIDER_NAME"].ToString(),
                                        StatusCode = dr["STATUS"].ToString(),
                                        StatusName = XData.GetNameInventoryStatus(dr["STATUS"].ToString()),
                                        TotalAmount = Convert.ToInt64(dr["TOTAL_AMOUNT"]),
                                        DateImport = dr["DATE_IMPORT"].ToString(),
                                        CreaterName = dr["CREATER_NAME"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString()
                                    });
                                }
                            }
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(list);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseImportController.GetDataByShopID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetDataByID")]
        [HttpPost]
        public ZResponse GetDataByID(ZWarehouseImportGetDataByIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZWarehouseImportGetDataByIDResponse item = new ZWarehouseImportGetDataByIDResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_IMPORT_PKG.GET_DATA_BY_ID", new object[]
                {
                    req.ID,        //P_ID            NUMBER\
                    req.ShopID,     //
                    0,             //P_RETURN_CODE   OUT NUMBER
                    0              //P_OUT_CURSOR    OUT T_CURSOR
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
                                    item = new ZWarehouseImportGetDataByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        ProviderID = Convert.ToInt32(dr["PROVIDER_ID"]),
                                        ProviderName = dr["PROVIDER_NAME"].ToString(),
                                        WarehouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                        WarehouseName = dr["WAREHOUSE_NAME"].ToString(),
                                        TotalAmount = Convert.ToInt64(dr["TOTAL_AMOUNT"]),
                                        DateImport = dr["DATE_IMPORT"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        CreaterName = dr["CREATER_NAME"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString()
                                    };
                                }
                            }
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(item);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseImportController.GetDataByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetDetailByID")]
        [HttpPost]
        public ZResponse GetDetailByID(ZWarehouseImportGetDetailByIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZWarehouseImportGetDetailByIDResponse> list = new List<ZWarehouseImportGetDetailByIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_IMPORT_PKG.GET_DETAIL_BY_ID", new object[]
                {
                    req.ID,         //P_ID  NUMBER
                    req.ShopID,     //P_SHOP_ID  NUMBER
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
                                    list.Add(new ZWarehouseImportGetDetailByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        ProductVariantName = dr["PRODUCT_VARIANT_NAME"].ToString(),
                                        Quantity = Convert.ToInt32(dr["QUANTITY"]),
                                        Price = Convert.ToInt64(dr["PRICE"])
                                    });
                                }
                            }
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(list);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseImportController.GetDetailByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("Remove")]
        public ZResponse Remove(ZWarehouseImportRemoveRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_IMPORT_PKG.REMOVE", new object[]
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
                                    res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái phiếu nhập đã thay đổi. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseImportController.Remove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("RemoveDetail")]
        public ZResponse RemoveDetail(ZWarehouseImportRemoveDetailRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_IMPORT_PKG.REMOVE_DETAIL", new object[]
                {
                   0,
                   req.DetailID,
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
                                    res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái phiếu nhập đã thay đổi. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseImportController.RemoveDetail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("Complete")]
        public ZResponse Complete(ZWarehouseImportCompleteRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_IMPORT_PKG.COMPLETE", new object[]
                {
                   0,
                   req.ID,
                   req.ProviderID,
                   req.WarehouseID,
                   req.DateImport,
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
                                    res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái phiếu nhập đã thay đổi. Vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Có lỗi trong quá trình xử lý dữ liệu. Vui lòng liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseImportController.Complete: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
