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
    [RoutePrefix("InventoryOutput")]
    public class InventoryOutputController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZInventoryOutputSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZInventoryOutputSearchListResponse> listItem = new List<ZInventoryOutputSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_OUTPUT_PKG.SEARCH_LIST", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.SearchKey,
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
                                    listItem.Add(new ZInventoryOutputSearchListResponse()
                                    {
                                        InventoryOutputID = Convert.ToInt32(dr["INVENTORY_OUTPUT_ID"]),
                                        InventoryOutputCode = dr["INVENTORY_OUTPUT_CODE"].ToString(),
                                        Order = new ZOrderResponse()
                                        {
                                            OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                            EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                            OrderCode = dr["ORDER_CODE"].ToString(),
                                            BatchCode = dr["BATCH_CODE"].ToString(),
                                            IsTransport = dr["TRANSPORT"].ToString(),
                                            ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        },
                                        Status = new ZBaseResponse()
                                        {
                                            Code = dr["INVENTORY_OUTPUT_STATUS"].ToString(),
                                            Name = XData.GetNameInventoryOutputStatus(dr["INVENTORY_OUTPUT_STATUS"].ToString())
                                        },
                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        CreatedName = dr["CREATED_NAME"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        UpdatedName = dr["UPDATED_NAME"].ToString(),
                                        UpdatedDate = dr["UPDATED_DATE"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.SearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZInventoryOutputAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryOutputRepository.AddInventoryOutput(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.Add: " + ex.Message);
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
                ZInventoryOutputGetByIDResponse item = null;
                List<ZInventoryOutputDetailGetByInventoryIDResponse> listItem = new List<ZInventoryOutputDetailGetByInventoryIDResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_OUTPUT_PKG.GET_BY_INVENTORY_OUTPUT_ID", new object[]
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
                                    listItem.Add(new ZInventoryOutputDetailGetByInventoryIDResponse()
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
                                        WarehouseQuantity = Convert.ToInt64(dr["WAREHOUSE_QUANTITY"]),
                                    });
                                }
                            }

                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR_1"].Value)
                            {
                                if (dr.Read())
                                {
                                    item = new ZInventoryOutputGetByIDResponse()
                                    {
                                        InventoryOutputID = Convert.ToInt32(dr["INVENTORY_OUTPUT_ID"]),
                                        InventoryOutputCode = dr["INVENTORY_OUTPUT_CODE"].ToString(),
                                        WareHouse = new ZWareHouseResponse()
                                        {
                                            WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                            WareHouseCode = dr["WAREHOUSE_CODE"].ToString(),
                                            WareHouseName = dr["WAREHOUSE_NAME"].ToString(),
                                            FullAddress = dr["WAREHOUSE_FULL_ADDRESS"].ToString(),
                                        },
                                        DateOfInventoryOutput = dr["DATE_OF_INVENTORY_OUTPUT"].ToString(),
                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        Status = new ZBaseResponse()
                                        {
                                            Code = dr["INVENTORY_OUTPUT_STATUS"].ToString(),
                                            Name = XData.GetNameInventoryOutputStatus(dr["INVENTORY_OUTPUT_STATUS"].ToString()),
                                        },
                                        Order = new ZOrderResponse()
                                        {
                                            OrderID = string.IsNullOrEmpty(dr["ORDER_ID"].ToString()) ? 0 : Convert.ToInt32(dr["ORDER_ID"]),
                                            EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                            BatchCode = dr["BATCH_CODE"].ToString(),
                                            OrderStatus = new ZBaseResponse()
                                            {
                                                Code = dr["ORDER_STATUS"].ToString(),
                                                Name = XData.GetNameOrderStatus(dr["ORDER_STATUS"].ToString()),
                                            },
                                        },
                                    };
                                    item.TotalAmount = listItem.Sum(m => m.TotalAmount);
                                    item.TotalAmountByText = item.TotalAmount == 0 ? "" : XData.NumberToText(item.TotalAmount);
                                    item.ListInventoryOutputDetail = listItem;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZInventoryOutputEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_OUTPUT_PKG.EDIT_INVENTORY_OUTPUT", new object[]
                {
                    0,
                    req.InventoryID,
                    req.DateOfInventoryOutput,
                    req.ReceiverName,
                    req.MobileNumber,
                    req.Note,
                    req.UpdatedBy,
                    req.ShopID,
                    req.ListInventoryOutputDetail.ToXML(),
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Sửa phiếu xuất kho thành công.";
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
                                    res.Message = "Biến thể không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Sản phẩm không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Sản phẩm không có trong kho.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Sản phẩm trong kho không đủ.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Trạng thái phiếu xuất kho không hợp lệ.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Lưu mã Pallet thất bại.";
                                    break;
                                case -8:
                                    res.Code = "08";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Process")]
        public ZResponse Process(ZInventoryOutputProcessRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_OUTPUT_PKG.PROCESS_STATUS", new object[]
                {
                    0,
                    req.InventoryOutputID,
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
                            res.Message = "Xử lý phiếu xuất kho thành công.";
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
                                    res.Message = "Trạng thái phiếu xuất kho không hợp lệ.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.Process: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Accept")]
        public ZResponse Accept(ZInventoryOutputAcceptRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_OUTPUT_PKG.ACCEPT_STATUS", new object[]
                {
                    0,
                    req.InventoryOutputID,
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
                            res.Message = "Phê duyệt phiếu xuất kho thành công.";
                        }
                        else
                        {
                            var ProductAndVariantName = Convert.ToString(dbCommand.Parameters[04].Value);
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái phiếu xuất kho không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Sản phẩm " + ProductAndVariantName + " trong kho không đủ, vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Sản phẩm " + ProductAndVariantName + " trong kho đã hết, vui lòng kiểm tra lại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Sản phẩm " + ProductAndVariantName + " không tồn tại trong kho, vui lòng kiểm tra lại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Thay đổi số lượng của Pallet thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.Accept: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Reject")]
        public ZResponse Reject(ZInventoryOutputRejectRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_OUTPUT_PKG.REJECT_STATUS", new object[]
                {
                    0,
                    req.InventoryOutputID,
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
                            res.Message = "Từ chối phiếu xuất kho thành công.";
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
                                    res.Message = "Trạng thái phiếu xuất kho không hợp lệ.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.Reject: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Info")]
        public ZResponse Info(ZInventoryOutputInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZInventoryOutputInfoResponse> ListItem = new List<ZInventoryOutputInfoResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_OUTPUT_PKG.INTENTORY_INFO", new object[]
                {
                    req.InventoryOutputID,  //P_INVENTORY_ID  NUMBER
                    req.ShopID,             //P_SHOP_ID       NUMBER
                    0,                      //P_RETURN_CODE   OUT NUMBER
                    0,                      //P_OUT_CURSOR    OUT T_CURSOR
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
                                    long TotalAmount = 0;
                                    List<ZInventoryOutputDetailInfoResponse> ListInventoryDetail = new List<ZInventoryOutputDetailInfoResponse>();
                                    var resInventoryDetailInfo = InventoryOutputRepository.InventoryDetailInfo(new ZInventoryOutputDetailInfoRequest() { InventoryOutputID = Convert.ToInt32(dr["INVENTORY_OUTPUT_ID"]), ShopID = req.ShopID });
                                    if (resInventoryDetailInfo.Code == "00")
                                    {
                                        ListInventoryDetail = JsonConvert.DeserializeObject<List<ZInventoryOutputDetailInfoResponse>>(resInventoryDetailInfo.Data);
                                        TotalAmount = ListInventoryDetail.Sum(m => m.TotalAmount);
                                    }

                                    ListItem.Add(new ZInventoryOutputInfoResponse()
                                    {
                                        InventoryOutputID = Convert.ToInt32(dr["INVENTORY_OUTPUT_ID"]),
                                        InventoryOutputCode = dr["INVENTORY_OUTPUT_CODE"].ToString(),
                                        WareHouse = new ZWareHouseResponse()
                                        {
                                            WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                            WareHouseCode = dr["WAREHOUSE_CODE"].ToString(),
                                            WareHouseName = dr["WAREHOUSE_NAME"].ToString(),
                                            FullAddress = dr["WAREHOUSE_FULL_ADDRESS"].ToString(),
                                        },
                                        DateOfInventoryOutput = dr["DATE_OF_INVENTORY_OUTPUT"].ToString(),
                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        Status = new ZBaseResponse()
                                        {
                                            Code = dr["INVENTORY_OUTPUT_STATUS"].ToString(),
                                            Name = XData.GetNameInventoryOutputStatus(dr["INVENTORY_OUTPUT_STATUS"].ToString()),
                                        },
                                        Order = new ZOrderResponse()
                                        {
                                            OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                            OrderCode = dr["ORDER_CODE"].ToString(),
                                            EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                            BatchCode = dr["BATCH_CODE"].ToString(),
                                            OrderStatus = new ZBaseResponse()
                                            {
                                                Code = dr["ORDER_STATUS"].ToString(),
                                                Name = XData.GetNameOrderStatus(dr["ORDER_STATUS"].ToString()),
                                            },
                                        },
                                        TotalAmount = TotalAmount,
                                        TotalAmountByText = TotalAmount == 0 ? "" : XData.NumberToText(TotalAmount),
                                        ListInventoryOutputDetail = ListInventoryDetail
                                    });
                                }
                            }

                            if (ListItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(ListItem);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.Info: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
