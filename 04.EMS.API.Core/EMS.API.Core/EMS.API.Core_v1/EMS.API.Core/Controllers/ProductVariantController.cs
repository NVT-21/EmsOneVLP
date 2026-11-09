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
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("ProductVariant")]
    public class ProductVariantController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZProductVariantSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZProductVariantSearchListResponse> listItem = new List<ZProductVariantSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.SEARCH_LIST", new object[]
                {
                    req.ShopID,
                    req.IsLock,
                    0,                          //P_RETURN_CODE                 OUT NUMBER
                    0                           //P_OUT_CURSOR                  OUT T_CURSOR
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
                                    listItem.Add(new ZProductVariantSearchListResponse() 
                                    {
                                        Product = new ZProductResponse() 
                                        {
                                            ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                            ProductVariant = new ZProductVariantResponse() 
                                            {
                                                ProductVariantID = Convert.ToInt32(dr["VARIANT_ID"]),
                                                ProductVariantSKU = dr["VARIANT_SKU"].ToString(),
                                                ProductVariantPrice = Convert.ToInt64(dr["VARIANT_PRICE"]),
                                                ProductVariantWeight = Convert.ToDouble(dr["VARIANT_WEIGHT"]),
                                            },
                                        },
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
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantController.SearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByProductID")]
        [HttpPost]
        public ZResponse GetByProductID(ZProductVariantGetByProductIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZProductVariantGetByProductIDResponse> Model = new List<ZProductVariantGetByProductIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.GET_BY_PRODUCT_ID", new object[]
                {
                    req.ProductID,
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
                                    Model.Add(new ZProductVariantGetByProductIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Avatar = dr["Avatar"].ToString(),
                                        Options = dr["Options"].ToString(),
                                        Price = Convert.ToInt64(dr["PRICE"])
                                    });
                                }
                            }
                            if (Model.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(Model);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantController.GetByProductID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByIDs")]
        [HttpPost]
        public ZResponse GetByIDs(ZProductVariantGetByIDsRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZProductVariantGetByIDsResponse> list = new List<ZProductVariantGetByIDsResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.GET_BY_IDS", new object[]
                {
                    req.IDs,               //P_VARIANT       NUMBER
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
                                    list.Add(new ZProductVariantGetByIDsResponse()
                                    {
                                        ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        VariantID = Convert.ToInt32(dr["VARIANT_ID"]),
                                        VariantName = dr["VARIANT_NAME"].ToString(),
                                        VariantImage = dr["VARIANT_IMAGE"].ToString(),
                                        Price = Convert.ToInt64(dr["PRICE"]),
                                        PurchasePrice = Convert.ToInt64(dr["PURCHASE_PRICE"]),
                                        SKU = dr["SKU"].ToString(),
                                        Barcode = dr["BARCODE"].ToString(),
                                        Weight = dr["WEIGHT"].ToString(),
                                        Quantity = Convert.ToInt32(!string.IsNullOrEmpty(dr["QUANTITY"].ToString()) ? dr["QUANTITY"].ToString() : "0"),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.GetByIDs: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        [HttpPost]
        public ZResponse GetByID(ZProductVariantGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            ZProductVariantGetByIDResponse item = null;
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.GET_BY_ID", new object[]
                {
                    req.ID,               //P_VARIANT_ID       NUMBER
                    req.ShopID,           //P_SHOP_ID          NUMBER
                    0,                    //P_RETURN_CODE      OUT NUMBER
                    0                     //P_OUT_CURSOR       OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            //var OptionsID = "";
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                if (dr.Read())
                                {
                                    item = new ZProductVariantGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        Options = dr["OPTIONS"].ToString(),
                                        OptionName1 = dr["OPTION_NAME_01"].ToString(),
                                        OptionValue1 = dr["OPTION_VALUE_01"].ToString(),
                                        OptionName2 = dr["OPTION_NAME_02"].ToString(),
                                        OptionValue2 = dr["OPTION_VALUE_02"].ToString(),
                                        OptionName3 = dr["OPTION_NAME_03"].ToString(),
                                        OptionValue3 = dr["OPTION_VALUE_03"].ToString(),
                                        Avatar = dr["AVATAR"].ToString(),
                                        Price = Convert.ToInt64(dr["PRICE"]),
                                        SKU = dr["SKU"].ToString(),
                                        Barcode = dr["BARCODE"].ToString(),
                                        Weight = Convert.ToInt32(dr["WEIGHT"]),
                                        Size = dr["SIZE_PRODUCT"].ToString(),
                                        WareHouseQuantity = Convert.ToInt64(dr["WAREHOUSE_QUANTITY"]),
                                        CurrentQuantity = Convert.ToInt64(dr["CURRENT_QUANTITY"]),
                                        BatchNumber = dr["BATCH_NUMBER"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetBySKU")]
        public ZResponse GetBySKU(string SKU, int ShopID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZProductVariantGetBySKUResponse item = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.GET_BY_VARIANT_SKU", new object[]
                {
                    SKU,               //P_SKU       NUMBER
                    ShopID,
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
                                if (dr.Read())
                                {
                                    item = new ZProductVariantGetBySKUResponse()
                                    {
                                        ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        VariantID = Convert.ToInt32(dr["VARIANT_ID"]),
                                        VariantName = dr["VARIANT_NAME"].ToString(),
                                        VariantImage = dr["VARIANT_IMAGE"].ToString(),
                                        Price = Convert.ToInt64(dr["PRICE"]),
                                        PurchasePrice = Convert.ToInt64(dr["PURCHASE_PRICE"]),
                                        SKU = dr["SKU"].ToString(),
                                        Barcode = dr["BARCODE"].ToString(),
                                        Weight = Convert.ToInt32(dr["WEIGHT"]),
                                        WareHouseQuantity = Convert.ToInt64(dr["WAREHOUSE_QUANTITY"]),
                                        CurrentQuantity = Convert.ToInt64(dr["CURRENT_QUANTITY"]),
                                    };
                                }
                            }
                            if (item != null)
                            {
                                //var resTemp = WareHouseProductRepository.GetByVariantID(item.VariantID);
                                //var lstWareHouse = JsonConvert.DeserializeObject<List<ZWarehouseProductGetByVariantIDResponse>>(resTemp.Data);
                                //item.listWareHouse = lstWareHouse;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantController.GetBySKU: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByShopID")]
        [HttpPost]
        public ZResponse GetByShopID(ZProductVariantGetByShopIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZProductVariantGetByShopIDResponse> list = new List<ZProductVariantGetByShopIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.GET_BY_SHOP_ID", new object[]
                {
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
                                    list.Add(new ZProductVariantGetByShopIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        SKU = dr["SKU"].ToString(),
                                        Options = dr["OPTIONS"].ToString(),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantController.GetByShopID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateImage")]
        public ZResponse UpdateImage(ZProductVariantUpdateImageRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.UPDATE_IMAGE", new object[]
                {
                   0,
                   req.ID,         //P_ID
                   req.FileName    //P_AVATAR
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.UpdateAvatar: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZProductVariantAddRequest req)
        {
            return ProductVariantRepository.Add(req);
        }

        //[Route("Add")]
        //public ZResponse Add(ZProductVariantAddRequest req)
        //{
        //    int result = -1;
        //    string OptionsID = "";
        //    ZResponse res = new ZResponse();
        //    ZProductVariantAddResponse item = new ZProductVariantAddResponse();
        //    ZResponse resAddImage = new ZResponse();
        //    try
        //    {
        //        if(req.Image == null || req.Image == "")
        //        {
        //            req.Image = XHelper.ImageDefault;
        //        }
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.ADD_VARIANT", new object[]
        //        {
        //           req.Name,                 //P_NAME             VARCHAR,
        //           req.Price,                //P_PRICE            NUMBER,
        //           req.PurchasePrice,        //P_PURCHASE_PRICE   NUMBER,
        //           req.SKU,                  //P_SKU              VARCHAR,
        //           req.Barcode,              //P_BARCODE          VARCHAR,
        //           OptionsID,                //P_OPTIONS_ID       VARCHAR,
        //           req.ProductID,            //P_PRODUCT_ID       NUMBER,
        //           req.Image,                //P_IMAGE            VARCHAR,
        //           req.CreatedBy,            //P_CREATED_BY       NUMBER,
        //           req.ShopID,               //P_SHOP_ID          NUMBER,
        //           0,                        //P_RETURN_CODE    OUT NUMBER
        //           0                         //P_VARIANT_ID       OUT NUMBER
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
        //                if (result > 0)
        //                {
        //                    var VariantID = Convert.ToInt32(dbCommand.Parameters["P_VARIANT_ID"].Value);
        //                    item = new ZProductVariantAddResponse()
        //                    {
        //                        VariantID = VariantID
        //                    };
        //                    ////Thêm sản phẩm vào kho
        //                    //if (req.WarehouseProducts != null)
        //                    //{
        //                    //    foreach (var itemWP in req.WarehouseProducts)
        //                    //    {
        //                    //        using (DbCommand dbCommand2 = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PRODUCT_PKG.ADD_NEW", new object[]
        //                    //        {
        //                    //            0,
        //                    //            itemWP.WarehouseID,      //P_WAREHOUSE_ID NUMBER,
        //                    //            req.ProductID,           //P_PRODUCT_ID   NUMBER,
        //                    //            VariantID,               //P_VARIANT_ID   NUMBER,
        //                    //            itemWP.Quantity,         //P_QUANTITY     NUMBER,
        //                    //            req.CreatedBy            //P_CREATED_BY   NUMBER
        //                    //        }))
        //                    //        {
        //                    //            result = XHelper.ExecuteNonQuery(dbCommand2);
        //                    //            if (result >= 0)
        //                    //            {
        //                    //                result = Convert.ToInt32(dbCommand2.Parameters[0].Value);
        //                    //                if (result < 0)
        //                    //                {
        //                    //                    // Thêm tồn kho thất bại - Xoá sản phẩm vừa thêm
        //                    //                    res.Code = "03";
        //                    //                    res.Message = "Thêm tồn kho thất bại. Tạo biến thể không thành công.";
        //                    //                    return res;
        //                    //                }
        //                    //            }
        //                    //            else
        //                    //            {
        //                    //                res.Code = "99";
        //                    //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //                    //                return res;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //}
        //                    res.Code = "00";
        //                    res.Message = "Thêm biến thể thành công.";
        //                    res.Data = JsonConvert.SerializeObject(item);
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Không tìm thấy sản phẩm.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Mã SKU đã tồn tại.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Mã BarCode đã tồn tại.";
        //                            break;
        //                        case -4:
        //                            res.Code = "04";
        //                            res.Message = "Biến thể đã tồn tại.";
        //                            break;
        //                        case -5:
        //                            res.Code = "05";
        //                            res.Message = "Đang có biến thể Chờ Cấu Hình. Vui lòng kiểm tra lại.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.Add: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("Edit")]
        public ZResponse Edit(ZProductVariantEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZResponse resAddImage = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.EDIT", new object[]
                {
                   0,
                   req.ID,              //P_ID               NUMBER,
                   req.Options,
                   req.OptionValue1,
                   req.OptionValue2,
                   req.OptionValue3,
                   req.Price,           //P_PRICE            NUMBER,
                   req.SKU,             //P_SKU              VARCHAR,
                   req.Barcode,         //P_BARCODE          VARCHAR,
                   req.BatchNumber,
                   req.Avatar,         //P_BARCODE          VARCHAR,
                   req.Weight,
                   req.Size,
                   req.UpdatedBy,       //P_UPDATED_BY       NUMBER
                   req.ShopID           //P_SHOP_ID          NUMBER
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thông tin sản phẩm thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã SKU đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mã BarCode đã tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Thuộc tính đã tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("Remove")]
        public ZResponse Remove(ZProductVariantRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.REMOVE", new object[]
                {
                   0,
                   req.ID,              //P_ID               NUMBER,
                   req.UpdatedBy,       //P_UPDATED_BY       NUMBER
                   req.ShopID           //P_SHOP_ID          NUMBER
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Xoá biến thể thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại dữ liệu. Vui lòng liên hệ quản trị.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Không thể xoá biến thể cuối cùng của sản phẩm.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Biến thể đã tồn tại trong kho. Không thể xoá.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại. Vui lòng liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantController.Remove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #region sử dụng trong ProductVariant
        public ZResponse GetOptionByID(int OptionID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZVariantOptionModel item = new ZVariantOptionModel();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_OPTION_PKG.GET_BY_ID", new object[]
                {
                    OptionID,      //P_PRODUCT_ID  NUMBER
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
                                    item = new ZVariantOptionModel()
                                    {
                                        OptionID = Convert.ToInt32(dr["ID"]),
                                        OptionName = dr["NAME"].ToString(),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        ParentName = dr["PARENT_NAME"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.GetOptionByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #endregion

        [Route("GetByWareHouseID")]
        [HttpPost]
        public ZResponse GetByWareHouseID(ZProductVariantGetByWareHouseIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZProductVariantGetByWareHouseIDResponse> list = new List<ZProductVariantGetByWareHouseIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.GET_BY_WAREHOUSE_ID", new object[]
                {
                    req.WareHouseID,
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
                                    list.Add(new ZProductVariantGetByWareHouseIDResponse()
                                    {
                                        ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        ProductVariantID = Convert.ToInt32(dr["PRODUCT_VARIANT_ID"]),
                                        ProductVariantName = dr["PRODUCT_VARIANT_NAME"].ToString(),
                                        ProductVariantSKU = dr["PRODUCT_VARIANT_SKU"].ToString(),
                                        ProductVariantPrice = Convert.ToInt64(dr["PRODUCT_VARIANT_PRICE"]),
                                        NumberOfInventory = Convert.ToInt32(dr["NUMBER_OF_INVENTORY"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantController.GetByWareHouseID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("CheckSKU")]
        public ZResponse CheckSKU(ZProductVariantCheckSKURequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.CHECK_SKU", new object[]
                {
                    0,
                    req.ProductVariantSKU,
                    req.ShopID,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Dữ liệu hợp lệ.";
                            res.Data = dbCommand.Parameters["P_OUT_PRODUCT_NAME"].Value.ToString();
                        }
                        else
                        {
                            var ProductVariantSKU = dbCommand.Parameters["P_OUT_PRODUCT_VARIANT_SKU"].Value.ToString();
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã SKU: " + ProductVariantSKU + " chưa nhập số lượng.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã SKU: " + ProductVariantSKU + " không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Sản phẩm đã bị khoá.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Mã SKU: " + ProductVariantSKU + " sai kiểu dữ liệu số lượng.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại. Vui lòng liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantController.CheckSKU: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
