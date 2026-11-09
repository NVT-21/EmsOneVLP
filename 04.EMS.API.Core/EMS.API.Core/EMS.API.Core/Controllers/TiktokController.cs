using EMS.API.Core.Repositories;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Tiktok")]
    public class TiktokController : ApiController
    {
        #region Product
        [Route("ListProduct")]
        public ZResponse ListProduct(ZTikProductListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZTikProductListResponse> oList = new List<ZTikProductListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_PRODUCT_PKG.LIST_PRODUCT", new object[]
                {
                    req.ShopID,         //P_SHOP_ID          NUMBER
                    req.SearchKey,
                    0,                  //P_RETURN_CODE      OUT NUMBER
                    0,                  //P_OUT_CURSOR       OUT T_CURSOR
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
                                    oList.Add(new ZTikProductListResponse()
                                    {
                                        ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                        ProductCode = dr["PRODUCT_CODE"].ToString(),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        ProductAvatar = dr["PRODUCT_AVATAR"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        UpdatedDate = dr["UPDATED_DATE"].ToString(),
                                        Price = Convert.ToInt64(dr["PRICE"]),
                                        Options = dr["OPTIONS"].ToString(),
                                        TiktokSKUID = dr["TIK_SKU_ID"].ToString(),
                                        TiktokProductID = dr["TIK_PRODUCT_ID"].ToString(),
                                        Quantity = Convert.ToInt32(dr["QUANTITY"]),
                                        Channel = dr["CHANNEL"].ToString(),
                                        TiktokStatusUpdatePrice = dr["TIKTOK_STATUS_UPDATE_PRICE"].ToString(),
                                        TiktokStatusUpdateStock = dr["TIKTOK_STATUS_UPDATE_STOCK"].ToString()
                                    });
                                }
                            }

                            if (oList.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oList);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokController.ListProduct: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ListProductProcess")]
        public ZResponse ListProductProcess(ZTikProductProcessListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZTikProductProcessListResponse> oList = new List<ZTikProductProcessListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_PRODUCT_PKG.LIST_PRODUCT_PROCESS", new object[]
                {
                    req.ShopID,         //P_SHOP_ID          NUMBER
                    req.SearchKey,
                    0,                  //P_RETURN_CODE      OUT NUMBER
                    0,                  //P_OUT_CURSOR       OUT T_CURSOR
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
                                    oList.Add(new ZTikProductProcessListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        SKU = dr["SKU"].ToString(),
                                        ProductCode = dr["PRODUCT_CODE"].ToString(),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        ProductAvatar = dr["PRODUCT_AVATAR"].ToString(),
                                        Options = dr["OPTIONS"].ToString(),

                                        TiktokProductID = dr["TIK_PRODUCT_ID"].ToString(),
                                        TiktokProductName = dr["TIK_PRODUCT_NAME"].ToString(),
                                        TiktokProductAvatar = dr["TIK_PRODUCT_AVATAR"].ToString(),
                                        TiktokSKUID = dr["TIK_SKU_ID"].ToString(),
                                        TiktokSellerSKU = dr["TIK_SELLER_SKU"].ToString(),
                                        TiktokOptions = dr["TIK_OPTIONS"].ToString(),
                                        Status = dr["STATUS"].ToString(),
                                        StatusName = XData.GetNameTikProductProcessStatus(dr["STATUS"].ToString())
                                    });
                                }
                            }

                            if (oList.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oList);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokController.ListProductProcess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ListProductNotLinkedYet")]
        [HttpPost]
        public ZResponse ListProductNotLinkedYet(ZTikProductNotLinkedYetListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZTikProductNotLinkedYetListResponse> list = new List<ZTikProductNotLinkedYetListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_PRODUCT_PKG.LIST_PRODUCT_NOT_LINKED_YET", new object[]
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
                                    list.Add(new ZTikProductNotLinkedYetListResponse()
                                    {
                                        ProductVariantID = Convert.ToInt32(dr["PRODUCT_VARIANT_ID"]),
                                        ProductVariantSKU = dr["PRODUCT_VARIANT_SKU"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokController.ListProductNotLinkedYet: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ProductUpdateConnection")]
        public ZResponse ProductUpdateConnection(ZTikProductUpdateConnectionRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZTikProductUpdateConnectionResponse> ListError = new List<ZTikProductUpdateConnectionResponse>();
            try
            {
                foreach (var item in req.ListItem)
                {
                    res = TiktokProductRepository.ProductUpdateConnection(
                        new ZTikProductUpdateConnectionListItemRequest()
                        {
                            ProductID = item.ProductID,
                            ProductVariantID = item.ProductVariantID,
                            ProductVariantSKU = item.ProductVariantSKU,
                            TiktokProductID = item.TiktokProductID,
                            TiktokSKUID = item.TiktokSKUID
                        },
                        req.TypeUpdate, req.ShopID, req.UpdatedBy);
                    if (res.Code != "00")
                    {
                        var data = JsonConvert.DeserializeObject<ZTikProductUpdateConnectionResponse>(res.Data);
                        ListError.Add(data);
                    }
                }
                if (ListError.Count > 0)
                {
                    var TotalSuccess = req.ListItem.Count - ListError.Count;
                    res.Code = "01";
                    res.Message = "Cập nhật liên kết thành công " + TotalSuccess + "/" + req.ListItem.Count + " sản phẩm SKU, vui lòng nhấn vào Xem chi tiết để kiểm tra.";
                    res.Data = JsonConvert.SerializeObject(ListError);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokController.ProductUpdateConnection: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Config
        [Route("ConfigProductByPageID")]
        public ZResponse ConfigProductByPageID(ZTikConfigProductRequest req)
        {
            int result = -1;
            ZTikConfigProductResponse item = null;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_CONFIGURATION_PKG.TIK_CONFIG_PRODUCT", new object[]
                {
                    req.ShopID,
                    req.PageID,
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
                                    item = new ZTikConfigProductResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        IsSyncProduct = Convert.ToString(dr["IS_SYNC_PRODUCT"]),
                                        IsSyncProductPrice = Convert.ToString(dr["IS_SYNC_PRODUCT_PRICE"]),
                                        IsSyncProductStock = Convert.ToString(dr["IS_SYNC_PRODUCT_STOCK"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokController.ConfigProductByPageID" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("ConfigProductChange")]
        public ZResponse ConfigProductChange(ZTikConfigProductChangeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_CONFIGURATION_PKG.TIK_CONFIG_PRODUCT_CHANGE", new object[]
                {
                    0,
                    req.ID,
                    req.Status,
                    req.Type,
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
                            res.Message = "Đã thay đổi trạng thái thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokController.ConfigProductChange: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion
    }
}