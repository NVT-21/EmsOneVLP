using EMS.API.Core.Repositories;
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
using System.Web;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Warehouse")]
    public class WarehouseController : ApiController
    {
        [Route("AddNew")]
        public ZResponse AddNew(ZWareHouseAddNewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = WareHouseRepository.AddNew(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.AddNew " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchList")]
        public ZResponse SearchList(ZWareHouseSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = WareHouseRepository.SearchList(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListProductByWareHouseID")]
        public ZResponse ListProductByWareHouseID(ZWareHouseListProductByWareHouseIDRequest req) //ZShopGetAllRequest req
        {
            int result = -1;
            List<ZWareHouseListProductByWareHouseIDResponse> lst = new List<ZWareHouseListProductByWareHouseIDResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.LIST_PRODUCT_BY_WAREHOUSE_ID", new object[]
                {
                    req.WareHouseID,
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
                                    lst.Add(new ZWareHouseListProductByWareHouseIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        WareHouseCode = Convert.ToString(dr["WAREHOUSE_CODE"]),
                                        WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        Code = Convert.ToString(dr["PRODUCT_CODE"]),
                                        Name = Convert.ToString(dr["PRODUCT_NAME"]),
                                        Quantity = Convert.ToInt32(dr["QUANTITY"])
                                    });
                                }
                            }
                            if (lst.Count > 0)
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.ListProductByWareHouseID" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int ID, int ShopID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZWareHouseGetByIDResponse item = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.GET_BY_ID", new object[]
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
                                    item = new ZWareHouseGetByIDResponse()
                                    {
                                        WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                        WareHouseCode = Convert.ToString(dr["WAREHOUSE_CODE"]),
                                        WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        ProvinceName = Convert.ToString(dr["PROVINCE_NAME"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        DistrictName = Convert.ToString(dr["DISTRICT_NAME"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        WardName = Convert.ToString(dr["WARD_NAME"]),
                                        Street = Convert.ToString(dr["ADDRESS"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        IsDefault = Convert.ToString(dr["IS_DEFAULT"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                    };
                                }
                            }
                            if(item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListProduct")]
        public ZResponse ListProduct(ZWareHouseListProductRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZWareHouseListProductResponse> lst = new List<ZWareHouseListProductResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.LIST_WAREHOUSE_PRODUCT", new object[]
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
                                    lst.Add(new ZWareHouseListProductResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        WareHouseCode = Convert.ToString(dr["WAREHOUSE_CODE"]),
                                        WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        Code = Convert.ToString(dr["CODE"]),
                                        Name = Convert.ToString(dr["NAME"]),
                                        Quantity = Convert.ToInt32(dr["QUANTITY"])

                                    });
                                }

                            }
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = JsonConvert.SerializeObject(lst);
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseController.ListProduct: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddInventory")]
        public ZResponse AddInventory(ZWareHouseAddInventoryRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.ADD_INVENTORY", new object[]
                {
                    0,
                    req.ID,
                    req.Code,
                    req.Quantity,
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
                            res.Message = "Chỉnh sửa thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.AddInventory: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("Edit")]
        //public ZResponse Edit(ZWareHouseEditRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.EDIT", new object[]
        //        {
        //            0,
        //            req.ID,
        //            req.Code,
        //            req.Quantity,
        //            req.UpdatedBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Chỉnh sửa thành công.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.Edit: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("Edit")]
        public ZResponse Edit(ZWareHouseEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.WAREHOUSE_EDIT", new object[]
                {
                    0,
                    req.WareHouseID,
                    req.WareHouseName,
                    req.FullName,
                    req.MobileNumber,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Street,
                    req.ShopID,
                    req.UpdatedBy,
                    req.IsDefault
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
                                    res.Message = "Tên kho đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Kho bị khoá nên không thể chọn làm kho mặc định.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Không thể bỏ chọn kho mặc định.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("IsDefault")]
        public ZResponse IsDefault(ZWarehouseIsDefaultRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.IS_DEFAULT", new object[]
                {
                    0,
                    req.ID,
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
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Kho bị khoá nên không thể chọn làm kho mặc định.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //-------------------C------------------>
        [Route("GetProductsByID")]
        [HttpPost]
        public ZResponse GetProductsByID(ZWarehouseGetProductsByIDRequest req)
        {
            int result = -1;
            List<ZWarehouseGetProductsByIDResponse> list = new List<ZWarehouseGetProductsByIDResponse>();
            List<int> ListProductVariantID = new List<int>();
            List<ZWarehouseProductGetByVariantIDResponse> ListWareHouse = new List<ZWarehouseProductGetByVariantIDResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PRODUCT_PKG.GET_BY_WAREHOUSE_ID_V2", new object[]
                {
                    req.WarehouseID,
                    req.FromDate,
                    req.ToDate,
                    req.ShopID,
                    req.SearchCode,
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
                                    if (!ListProductVariantID.Exists(m => m == Convert.ToInt32(dr["VARIANT_ID"])))
                                    {
                                        if (req.WarehouseID == 0)
                                        {
                                            ZWarehouseProductGetByVariantIDRequest req1 = new ZWarehouseProductGetByVariantIDRequest()
                                            {
                                                VariantID = Convert.ToInt32(dr["VARIANT_ID"]),
                                                FromDate = req.FromDate,
                                                ToDate = req.ToDate,
                                                ShopID = req.ShopID
                                            };
                                            var resListWareHouse = WareHouseProductRepository.GetByVariantID(req1);
                                            ListWareHouse = JsonConvert.DeserializeObject<List<ZWarehouseProductGetByVariantIDResponse>>(resListWareHouse.Data);
                                        }
                                        list.Add(new ZWarehouseGetProductsByIDResponse()
                                        {
                                            WarehouseProductID = Convert.ToInt32(dr["ID"]),
                                            ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                            ProductName = Convert.ToString(dr["PRODUCT_NAME"]),
                                            ProductCode = Convert.ToString(dr["PRODUCT_CODE"]),
                                            VariantID = Convert.ToInt32(dr["VARIANT_ID"]),
                                            VariantName = Convert.ToString(dr["VARIANT_NAME"]),
                                            VariantImage = Convert.ToString(dr["VARIANT_IMAGE"]),
                                            SKU = Convert.ToString(dr["SKU"]),
                                            Barcode = Convert.ToString(dr["BARCODE"]),
                                            FirstQuantity = Convert.ToInt32(dr["CURRENT_QUANTITY"]),
                                            QuantityIn = Convert.ToInt32(dr["QUANTITY_IN"]),
                                            QuantityOut = Convert.ToInt32(dr["QUANTITY_OUT"]),
                                            QuantityAdjust = Convert.ToInt32(dr["QUANTITY_ADJUST"]),
                                            FinalQuantity = Convert.ToInt32(dr["FINAL_QUANTITY"]),
                                            TotalInventory = ListWareHouse.Sum(m => m.FinalQuantity),
                                            PalletCode = Convert.ToString(dr["PALLET_CODE"]),
                                            ListWareHouse = ListWareHouse
                                        });
                                        ListProductVariantID.Add(Convert.ToInt32(dr["VARIANT_ID"]));
                                    }
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
                                res.Message = "Không tìm thấy dữ liệu kho.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tìm thấy dữ liệu tồn đầu kỳ.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị";
                                    break;
                            }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseController.GetProductsByWarehouseID" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetDefaultWarehouse")]
        [HttpPost]
        public ZResponse GetDefaultWarehouse(ZWarehouseGetDefaultWarehouseRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZWarehouseGetDefaultWarehouseResponse model = new ZWarehouseGetDefaultWarehouseResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.GET_DEFAULT_WAREHOUSE", new object[]
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
                                if (dr.Read())
                                {
                                    model = new ZWarehouseGetDefaultWarehouseResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = Convert.ToString(dr["WAREHOUSE_CODE"]),
                                        Name = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        Address = Convert.ToString(dr["ADDRESS"])
                                    };
                                }
                            }
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = JsonConvert.SerializeObject(model);
                        }
                        else
                        {
                            switch (result)
                            {
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseController.GetDefaultWarehouse: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeIsLock")]
        public ZResponse ChangeIsLock(ZWareHouseChangeIsLockRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.CHANGE_IS_LOCK", new object[]
                {
                    0,
                    req.ID,
                    req.IsLock,
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
                                    res.Message = "Sai trạng thái.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Kho còn hàng tồn, không được phép khóa kho hàng.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Không được khoá kho mặc định.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.ChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByCode")]
        public ZResponse GetByCode(int WareHouseCode, int ShopID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZWareHouseGetByCodeResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.GET_BY_CODE", new object[]
                {
                    WareHouseCode,
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
                                    item = new ZWareHouseGetByCodeResponse()
                                    {
                                        WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseController.GetByCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
