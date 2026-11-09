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

namespace EMS.API.Core.Repositories
{
    public class TiktokProductRepository
    {
        public static ZResponse ProductUpdateConnection(ZTikProductUpdateConnectionListItemRequest req, string TypeUpdate, int ShopID, int UpdatedBy)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "TIK_PRODUCT_PKG.PRODUCT_UPDATE_CONNECTION", new object[]
                {
                    0,
                    TypeUpdate,
                    req.ProductID,
                    req.ProductVariantID,
                    req.ProductVariantSKU,
                    req.TiktokProductID,
                    req.TiktokSKUID,
                    ShopID,
                    UpdatedBy,
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
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "SKU đã được liên kết trước đó.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "SKU không tồn tại.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                            res.Data = JsonConvert.SerializeObject(new ZTikProductUpdateConnectionResponse()
                            {
                                ProductName = dbCommand.Parameters["P_OUT_PRODUCT_NAME"].Value.ToString(),
                                ProductVariantSKU = dbCommand.Parameters["P_OUT_PRODUCT_VARIANT_SKU"].Value.ToString(),
                                Description = res.Message
                            });
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TiktokProductRepository.ProductUpdateConnection: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}