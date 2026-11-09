using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("ProductGroup")]
    public class ProductGroupController : ApiController
    {
        [Route("List")]
        public ZResponse List(ZProductGroupListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZProductGroupListResponse> listItem = new List<ZProductGroupListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_GROUP_PKG.SEARCH_LIST", new object[]
                {
                    req.Code,   //P_ID          VARCHAR
                    req.Name,   //P_NAME        VARCHAR
                    req.ShopID, //P_SHOP_ID     NUMBER
                    0,          //P_RETURN_CODE OUT NUMBER
                    0           //P_OUT_CURSOR  OUT T_CURSOR
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
                                    listItem.Add(new ZProductGroupListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        Name = dr["NAME"].ToString(),
                                        Description = dr["DESCRIPTION"].ToString(),
                                        EmployeeName = dr["EMPLOYEE_NAME"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        CountProduct = Convert.ToInt32(dr["COUNT_PRODUCT"]),
                                        TotalPriceProduct = Convert.ToInt64(dr["TOTAL_PRICE_PRODUCT"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupController.List: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZProductGroupAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_GROUP_PKG.ADD_PRODUCT_GROUP", new object[]
                {
                   0,
                   req.ProductGroupCode,        //P_CODE        VARCHAR,
                   req.ProductGroupName,        //P_NAME        VARCHAR,
                   req.Description, //P_DESCRIPTION VARCHAR,
                   req.ShopID,      //P_SHOP_ID     NUMBER,
                   req.CreatedBy    //P_CREATED_BY  NUMBER
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
                                    res.Message = "Mã nhóm sản phẩm đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int ProductGroupID, int ShopID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZProductGroupGetByIDResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_GROUP_PKG.GET_BY_ID", new object[]
                {
                    ProductGroupID,     //P_PRODUCT_GROUP_ID    NUMBER
                    ShopID,             //P_SHOP_ID             NUMBER
                    0,                  //P_RETURN_CODE         OUT NUMBER
                    0                   //P_OUT_CURSOR          OUT T_CURSOR
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
                                    item = new ZProductGroupGetByIDResponse()
                                    {
                                        ProductGroupID = Convert.ToInt32(dr["PRODUCT_GROUP_ID"]),
                                        ProductGroupCode = dr["PRODUCT_GROUP_CODE"].ToString(),
                                        ProductGroupName = dr["PRODUCT_GROUP_NAME"].ToString(),
                                        Description = dr["DESCRIPTION"].ToString()
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupController.GetByID: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("GetByCode")]
        public ZResponse GetByCode(ZProductGroupGetByCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZProductGroupGetByCodeResponse oItem = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_GROUP_PKG.GET_BY_CODE", new object[]
                {
                    req.Code,   //P_ID            NUMBER
                    req.ShopID,
                    0,    //P_RETURN_CODE   OUT NUMBER
                    0     //P_OUT_CURSOR    OUT T_CURSOR
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
                                    oItem = new ZProductGroupGetByCodeResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        Name = dr["NAME"].ToString(),
                                        Description = dr["DESCRIPTION"].ToString()
                                    };
                                }
                            }
                            if (oItem != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oItem);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupController.GetByCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZProductGroupEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_GROUP_PKG.EDIT_PRODUCT_GROUP", new object[]
                {
                   0,
                   req.ProductGroupID,      //P_PRODUCT_GROUP_ID    NUMBER,
                   req.ProductGroupName,    //P_PRODUCT_GROUP_NAME  VARCHAR,
                   req.Description,         //P_DESCRIPTION         VARCHAR,
                   req.ShopID,              //P_SHOP_ID             NUMBER,
                   req.UpdatedBy            //P_UPDATED_BY          NUMBER
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
                                    res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchListProduct")]
        public ZResponse SearchListProduct(ZProductGroupSearchListProductRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZProductGroupSearchListProductResponse> listItem = new List<ZProductGroupSearchListProductResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_GROUP_PKG.SEARCH_LIST_PRODUCT", new object[]
                {
                    req.ProductGroupID, //P_PRODUCT_GROUP_ID NUMBER
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
                                    listItem.Add(new ZProductGroupSearchListProductResponse()
                                    {
                                        ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
                                        ProductCode = dr["PRODUCT_CODE"].ToString(),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        ProductPriceSell = Convert.ToInt64(dr["PRODUCT_PRICE_SELL"]),
                                        Description = dr["DESCRIPTION"].ToString(),
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
                                res.Message = "Dữ liệu không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupController.SearchListProduct: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}