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
    public class ProductVariantRepository
    {
        public static ZResponse Add(ZProductVariantAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            string Avatar = XHelper.ImageDefault;
            ZProductVariantAddResponse item = new ZProductVariantAddResponse();
            try
            {
                if (req.Avatar != null && req.Avatar != "")
                {
                    Avatar = req.Avatar;
                };
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.ADD_NEW", new object[]
                {
                   0,                        //VARIANT_ID
                   req.ShopID,
                   req.ProductID,            //P_PRODUCT_ID       NUMBER,
                   req.Price,                //P_PRICE            NUMBER,
                   req.Size,
                   req.Weight,
                   Avatar,               //P_AVATAR            VARCHAR,
                   req.SKU,
                   req.Barcode,
                   req.BatchNumber,
                   req.Options,
                   req.OptionName1,
                   req.OptionValue1,
                   req.OptionName2,
                   req.OptionValue2,
                   req.OptionName3,
                   req.OptionValue3,
                   req.CreatedBy,            //P_CREATED_BY       NUMBER
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            var VariantID = Convert.ToInt32(dbCommand.Parameters[0].Value);
                            item = new ZProductVariantAddResponse()
                            {
                                VariantID = VariantID
                            };

                            res.Code = "00";
                            res.Message = "Thêm biến thể thành công.";
                            res.Data = JsonConvert.SerializeObject(item);
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tìm thấy sản phẩm.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã SKU biến thể đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mã BarCode biến thể đã tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Tên biến thể đã tồn tại.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thêm biến thể thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantRepository.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Thêm biến thể thất bại. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static List<string> GetProductNameByProductSKU(string ProductSKU, int ShopID)
        {
            int result = -1;
            List<string> ListProduct = new List<string>();
            var splitProduct = ProductSKU.Split('|');
            for (var i = 0; i < splitProduct.Length; i++)
            {
                var splitProductDetail = splitProduct[i].Split('-');
                try
                {
                    using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.GET_BY_VARIANT_SKU", new object[]
                    {
                        splitProductDetail[0],               //P_SKU       NUMBER
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
                                        if(dr["VARIANT_NAME"].ToString() != "")
                                        {
                                            ListProduct.Add(dr["PRODUCT_NAME"].ToString() + " (" + dr["VARIANT_NAME"].ToString() + ") x " + splitProductDetail[1]);
                                        }
                                        else
                                        {
                                            ListProduct.Add(dr["PRODUCT_NAME"].ToString() + " x " + splitProductDetail[1]);
                                        }
                                    }
                                    else
                                    {
                                        ListProduct.Add(splitProduct[i]);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    XHelper.LogToFile(LogFileType.EXCEPTION, "ProductVariantRepository.GetProductNameByProductSKU: " + ex.Message);
                }
            }
            return ListProduct;
        }
    }
}