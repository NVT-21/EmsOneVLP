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
    [RoutePrefix("WarehouseProduct")]
    public class WarehouseProductController : ApiController
    {
        [Route("Update")]
        public ZResponse Update(ZWarehouseProductUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                if (req.ID > 0)
                {
                    using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PRODUCT_PKG.UPDATE_BY_ID", new object[]
                    {
                    0,
                    req.ID,
                    req.OldQuantity,
                    req.Quantity,
                    req.Note,
                    req.Status,
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
                                res.Message = "Cập nhật tồn kho thành công.";
                            }
                            else
                            {
                                switch (result)
                                {
                                    case -1:
                                        res.Code = "01";
                                        res.Message = "Không tìm thấy thông tin phù hợp.";
                                        break;
                                    default:
                                        res.Code = "99";
                                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
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
                else
                {
                    using (DbCommand dbCommand2 = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PRODUCT_PKG.ADD_NEW", new object[]
                    {
                        0,
                        req.WarehouseID,    //P_WAREHOUSE_ID NUMBER,
                        req.ProductID,      //P_PRODUCT_ID   NUMBER,
                        req.VariantID,      //P_VARIANT_ID   NUMBER,
                        req.Quantity,       //P_QUANTITY     NUMBER,
                        req.UpdatedBy       //P_CREATED_BY   NUMBER
                    }))
                    {
                        result = XHelper.ExecuteNonQuery(dbCommand2);
                        if (result >= 0)
                        {
                            result = Convert.ToInt32(dbCommand2.Parameters[0].Value);

                            if (result > 0)
                            {
                                res.Code = "00";
                                res.Message = "Cập nhật tồn kho thành công.";
                            }
                            else
                            {
                                switch (result)
                                {
                                    case -1:
                                        res.Code = "01";
                                        res.Message = "Cập nhật tồn kho thất bại. Vui lòng liên hệ quản trị.";
                                        break;
                                    default:
                                        res.Code = "99";
                                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
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
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "WarehouseProductController.Update: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByVariantID")]
        [HttpPost]
        public ZResponse GetByVariantID(ZWarehouseProductGetByVariantIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZWarehouseProductGetByVariantIDResponse> list = new List<ZWarehouseProductGetByVariantIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PRODUCT_PKG.GET_BY_VARIANT_ID", new object[]
                {
                    req.VariantID,  //P_VARIANT_ID  NUMBER
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
                                        Quantity = Convert.ToInt32(dr["QUANTITY"])
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
