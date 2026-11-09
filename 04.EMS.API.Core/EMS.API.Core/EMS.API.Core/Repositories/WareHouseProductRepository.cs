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
    public class WareHouseProductRepository
    {
        public static ZResponse GetByVariantID(ZWarehouseProductGetByVariantIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZWarehouseProductGetByVariantIDResponse> list = new List<ZWarehouseProductGetByVariantIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PRODUCT_PKG.GET_BY_VARIANT_ID_V2", new object[]
                {
                    req.VariantID,  //P_VARIANT_ID  NUMBER
                    req.FromDate,
                    req.ToDate,
                    req.ShopID,
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
                                    list.Add(new ZWarehouseProductGetByVariantIDResponse()
                                    {
                                        WarehouseProductID = Convert.ToInt32(dr["ID"]),
                                        WarehouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                        WarehouseName = dr["WAREHOUSE_NAME"].ToString(),
                                        FirstQuantity = Convert.ToInt32(dr["CURRENT_QUANTITY"]),
                                        QuantityIn = Convert.ToInt32(dr["QUANTITY_IN"]),
                                        QuantityOut = Convert.ToInt32(dr["QUANTITY_OUT"]),
                                        QuantityAdjust = Convert.ToInt32(dr["QUANTITY_ADJUST"]),
                                        FinalQuantity = Convert.ToInt32(dr["FINAL_QUANTITY"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseProductController.GetByVariantID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}