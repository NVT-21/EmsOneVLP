using System;
using System.Collections.Generic;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using System.Data.Common;
using System.Data;
using Newtonsoft.Json;
namespace EMS.API.Core.Repositories
{
    public class OrderItemRepository
    {
        public static ZResponse GetOrderItemByOrderID(ZOrderItemGetByOrderIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1; 
                List<ZOrderItemGetByOrderIDResponse> list = new List<ZOrderItemGetByOrderIDResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_ITEM_PKG.GET_BY_ORDER_ID", new object[]
                {
                    req.OrderID,    //P_ORDER_ID      NUMBER
                    0,              //P_RETURN_CODE   OUT NUMBER
                    0               //P_OUT_CURSOR    OUT T_CURSOR
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
                                    list.Add(new ZOrderItemGetByOrderIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                        ProductCode = dr["PRODUCT_CODE"].ToString(),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        VariantID = Convert.ToInt32(dr["VARIANT_ID"]),
                                        VariantSKU = dr["VARIANT_SKU"].ToString(),
                                        VariantName = dr["VARIANT_NAME"].ToString(),
                                        NumberOfProduct = string.IsNullOrEmpty(dr["NUMBER_OF_PRODUCT"].ToString()) ? 0 : Convert.ToInt32(dr["NUMBER_OF_PRODUCT"].ToString()),
                                        Price = string.IsNullOrEmpty(dr["PRICE"].ToString()) ? 0 : Convert.ToInt64(dr["PRICE"].ToString()),
                                        Discount = string.IsNullOrEmpty(dr["DISCOUNT"].ToString()) ? 0 : Convert.ToInt32(dr["DISCOUNT"].ToString()),
                                        Weight = string.IsNullOrEmpty(dr["WEIGHT"].ToString()) ? 0 : Convert.ToInt32(dr["WEIGHT"].ToString()),
                                        ProductVariantImage = dr["PRODUCT_VARIANT_IMAGE"].ToString(),
                                        WareHouseQuantity = string.IsNullOrEmpty(dr["WAREHOUSE_QUANTITY"].ToString()) ? 0 : Convert.ToInt32(dr["WAREHOUSE_QUANTITY"].ToString()),
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
                                res.Data = null;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderItemRepository.GetOrderItemByOrderID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}