using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using System.Data.Common;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;
using EMS.API.Core.Controllers;

namespace EMS.API.Core.Repositories
{
    public class InventoryOutputRepository
    {
        public static ZResponse AddInventoryOutput(ZInventoryOutputAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_OUTPUT_PKG.ADD_INVENTORY_OUTPUT", new object[]
                {
                    0,
                    req.InventoryCode,
                    req.WareHouseID,
                    //req.OrderID,
                    req.DateOfInventoryOutput,
                    req.ReceiverName,
                    req.MobileNumber,
                    req.Note,
                    req.CreatedBy,
                    req.ShopID,
                    //req.AmndState,
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
                            res.Message = "Tạo phiếu xuất kho thành công.";
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
                                    res.Message = "Sản phẩm không có trong kho.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Sản phẩm trong kho không đủ.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Giá của sản phẩm phải lớn hơn 0.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Mã phiếu xuất kho đã tồn tại.";
                                    break;
                                case -9:
                                    res.Code = "09";
                                    res.Message = "Lưu mã Pallet thất bại.";
                                    break;
                                case -10:
                                    res.Code = "10";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputRepository.AddInventoryOutput: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse InventoryDetailInfo(ZInventoryOutputDetailInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZInventoryOutputDetailInfoResponse> listItem = new List<ZInventoryOutputDetailInfoResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_INVENTORY_OUTPUT_PKG.INTENTORY_DETAIL_INFO", new object[]
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
                                    listItem.Add(new ZInventoryOutputDetailInfoResponse()
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

                            if (listItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(listItem);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.InventoryDetailInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

    }
}