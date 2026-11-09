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
    [RoutePrefix("TiktokSync")]
    public class TiktokSyncController : ApiController
    {
        [Route("TiktokSyncProduct")]
        public ZResponse TiktokSyncProduct(ZTiktokShopProductSyncRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var a = req.ListProduct.ToXML();
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_PRODUCT_SYNC_DATA_PKG.PRODUCT_SYNC_DATA", new object[]
                {
                    0,
                    req.WareHouseID,
                    req.ListProduct.ToXML(),
                    req.ShopID,
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
                            res.Message = "Thành công.";
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
                                    res.Message = "Nhân viên không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Kho không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Thêm phiếu nhập kho thất bại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Thêm chi tiết phiếu nhập kho thất bại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Thêm sản phẩm đồng bộ lỗi thất bại.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị. Tạo sản phẩm thất bại.";
                                    break;
                            };
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokSyncController.TiktokSyncProduct: " + ex.Message);
            }
            return res;
        }
        [Route("ListProduct")]
        public ZResponse ListProduct(ZTikProductListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZTikProductListResponse> oList = new List<ZTikProductListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_PRODUCT_SYNC_DATA_PKG.PRODUCT_LIST", new object[]
                {
                    req.ShopID,         //P_SHOP_ID          NUMBER
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
                                        SKU = dr["SKU"].ToString(),
                                        Price = Convert.ToInt64(dr["PRICE"]),
                                        TiktokSKUID = dr["TIK_SKU_ID"].ToString(),
                                        TiktokProductID = dr["TIK_PRODUCT_ID"].ToString(),
                                        Quantity = Convert.ToInt32(dr["QUANTITY"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokSyncController.ListProduct: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("UpdateProduct")]
        public ZResponse UpdateProduct(ZTikProductUpdateFromServiceRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {

                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_PRODUCT_SYNC_DATA_PKG.PRODUCT_UPDATE", new object[]
                {
                    0,
                    req.ShopID,
                    req.TiktokProductID,
                    req.TiktokSKUID,
                    req.TypeUpdate,
                    req.UpdateSuccess
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
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokSyncController.UpdateProduct: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("TiktokSyncOrder")]
        public ZResponse TiktokSyncOrder(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_ORDER_INVENTORY_SYNC_PKG.ORDER_SYNC_DATA", new object[]
                {
                    0,
                    req.OrderCode == null ? "" : req.OrderCode.ToUpper(),                 //P_ORDER_CODE        VARCHAR,
                    req.ShipmentDetails.ReceiverName,        //P_RECEIVER_NAME     VARCHAR,
                    req.ShipmentDetails.MobileNumber,        //P_MOBILE_NUMBER     VARCHAR,
                    req.ShipmentDetails.Address,             //P_ADDRESS           VARCHAR,
                    req.ShipmentDetails.FullAddress,         //P_FULL_ADDRESS      VARCHAR,
                    req.TotalAmount,                         //P_TOTAL_AMOUNT      NUMBER,
                    req.TotalDiscount,                       //P_TOTAL_DISCOUNT      NUMBER,
                    req.TransportFee,                        //P_TRANSPORT_FEE     NUMBER,
                    req.CreatedBy,                           //P_CREATED_BY        NUMBER,
                    req.ShopID,                              //P_SHOP_ID           NUMBER,
                    //req.PageID,                              //P_PAGE_ID           VARCHAR,
                    req.Channel,                             //P_CHANNEL           NUMBER,
                    req.Transport,                           //P_TRANSPORT         VARCHAR,
                    req.Message,
                    req.Status,
                    req.TransportOrderName,
                    req.TransportWeight,
                    req.TransportQuantity == 0 ? 1 : req.TransportQuantity,
                    req.TransportOrderValue,
                    req.TransportPayer,
                    req.CollectionType,
                    req.DeviceName,
                    req.Note,
                    req.CreateTime
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
                                    res.Message = "Cửa hàng không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã đơn hàng đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mã đơn hàng TiktokShop bị trống.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                                    break;
                            };
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokSyncController.TiktokSyncOrder: " + ex.Message);
            }
            return res;
        }
        [Route("TiktokSyncOrderInventory")]
        public ZResponse TiktokSyncOrderInventory(ZTikOrderInventorySyncRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_ORDER_INVENTORY_SYNC_PKG.ORDER_INVENTORY_SYNC_DATA", new object[]
                {
                    0,
                    req.ShopID,
                    //req.PageID,
                    req.TiktokOrderID,
                    req.ListItem.ToXML(),
                    req.Note,
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
                            res.Message = "Thành công.";
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
                                    res.Message = "Nhân viên không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Biến thể không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Sản phẩm không nằm trong kho.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Số lượng sản phẩm trong kho không đủ.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Thêm phiếu xuất kho thất bại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Đơn hàng không tồn tại.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Thêm chi tiết phiếu xuất kho thất bại.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                                    break;
                            };
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokSyncController.TiktokSyncOrderInventory: " + ex.Message);
            }
            return res;
        }
    }
}