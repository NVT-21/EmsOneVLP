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
    [RoutePrefix("ProductOption")]
    public class ProductOptionController : ApiController
    {
        [Route("Add")]
        public ZResponse Add(ZProductOptionAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_OPTION_PKG.ADD_NEW", new object[]
                {
                   0,                    //--> Return ID
                   req.ShopID,
                   req.ProductID,
                   req.Name,
                   req.Value,
                   req.CreatedBy         //P_CREATED_BY         NUMBER
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
                            res.Data = JsonConvert.SerializeObject(new ZOptionAddResponse()
                            {
                                ID = result
                            });
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tìm thấy thông tin sản phẩm. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Thuộc tính đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Tối đa 3 thuộc tính. Vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Lỗi thêm biến thể. Vui lòng thử lại hoặc liên hệ quản trị.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Lỗi xác định vị trí của thuộc tính. Vui lòng thử lại hoặc liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OptionController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetOptionByShopID")]
        [HttpPost]
        public ZResponse GetOptionByShopID(ZOptionGetDataByShopID req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZProductVariantListResponse> oItem = new List<ZProductVariantListResponse>();
            List<ZProductVariantListResponse> oListVariant = new List<ZProductVariantListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_OPTION_PKG.GET_DATA_BY_SHOP_ID", new object[]
                {
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
                                    oItem.Add(new ZProductVariantListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = dr["NAME"].ToString(),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        IsLock = dr["IS_LOCK"].ToString()
                                    });
                                }
                            }

                            if (oItem != null)
                            {
                                var listParent = oItem.Where(n => n.ParentID == 0).ToList();
                                foreach (var itemParent in listParent)
                                {
                                    //Tạm chưa sửa
                                    //foreach(var item in oItem)
                                    //{
                                    //    if(item.ParentID == itemParent.ID)
                                    //    {
                                    //        oListVariant.Add(new ZProductVariantListResponse()
                                    //        {
                                    //            ID = itemParent.ID,
                                    //            Name = itemParent.Name,
                                    //            ListChild =
                                    //        });
                                    //    }
                                    //}

                                    var listChild = oItem.Where(n => n.ParentID == itemParent.ID).ToList();
                                    oListVariant.Add(new ZProductVariantListResponse()
                                    {
                                        ID = itemParent.ID,
                                        Name = itemParent.Name,
                                        ListChild = listChild
                                    });
                                }

                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oListVariant);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OptionController.GetOptionByShopID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetDataByParentID")]
        [HttpPost]
        public ZResponse GetDataByParentID(ZOptionGetDataByParentIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZOptionGetDataByParentIDResponse> list = new List<ZOptionGetDataByParentIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_OPTION_PKG.GET_DATA_BY_PARENT_ID", new object[]
                {
                    req.ParentID,
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
                                    list.Add(new ZOptionGetDataByParentIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = dr["NAME"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductOptionController.GetDataByParentID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetByProductID")]
        [HttpPost]
        public ZResponse GetByProductID(ZProductOptionGetByProductIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZProductOptionGetByProductIDResponse model = new ZProductOptionGetByProductIDResponse();
            List<XProductOptionModel> list = new List<XProductOptionModel>();
            List<XProductOptionModel> Option01 = new List<XProductOptionModel>();
            List<XProductOptionModel> Option02 = new List<XProductOptionModel>();
            List<XProductOptionModel> Option03 = new List<XProductOptionModel>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_OPTION_PKG.GET_BY_PRODUCT_ID", new object[]
                {
                    req.ProductID,
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
                                    list.Add(new XProductOptionModel()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = dr["NAME"].ToString(),
                                        Value = dr["VALUE"].ToString(),
                                        Sort = Convert.ToInt32(dr["SORT"])
                                    });
                                }
                                foreach (var item in list)
                                {
                                    if(item.Sort == 1)
                                    {
                                        Option01.Add(item);
                                    }
                                    else if (item.Sort == 2)
                                    {
                                        Option02.Add(item);
                                    }
                                    else if (item.Sort == 3)
                                    {
                                        Option03.Add(item);
                                    }
                                }
                                model = new ZProductOptionGetByProductIDResponse()
                                {
                                    Option01 = Option01,
                                    Option02 = Option02,
                                    Option03 = Option03
                                };
                            }
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(model);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductOptionController.GetByProductID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetNameByProductID")]
        [HttpPost]
        public ZResponse GetNameByProductID(ZProductOptionGetNameByProductIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZProductOptionGetNameByProductIDResponse> list = new List<ZProductOptionGetNameByProductIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_OPTION_PKG.GET_NAME_BY_PRODUCT_ID", new object[]
                {
                    req.ProductID,
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
                                    list.Add(new ZProductOptionGetNameByProductIDResponse()
                                    {
                                        Name = dr["NAME"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductOptionController.GetNameByProductID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("DataByVariantID")]
        public ZResponse DataByVariantID(ZOptionDataByVariantIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZOptionDataByVariantIDResponse> list = new List<ZOptionDataByVariantIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_OPTION_PKG.DATA_BY_VARIANT_ID", new object[]
                {
                    req.VariantID,
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
                                    list.Add(new ZOptionDataByVariantIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = dr["NAME"].ToString(),
                                        AttributeID = Convert.ToInt32(dr["PARENT_ID"]),
                                        AttributeName = dr["PARENT_NAME"].ToString(),

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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OptionController.DataByVariantID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("RemoveByName")]
        public ZResponse RemoveByName(ZProductOptionRemoveByNameRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_OPTION_PKG.REMOVE_BY_NAME", new object[]
                {
                   0,
                   req.Name,
                   req.ShopID,
                   req.ProductID,
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
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tìm thấy thông tin sản phẩm.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Không tìm thấy thuộc tính.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lỗi xoá biến thể. Vui lòng liên hệ quản trị.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Lỗi tạo biến thể. Vui lòng liên hệ quản trị.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Thuộc tính cuối cùng, không thể xoá.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Sản phẩm đã có trong kho. Không thể xoá thuộc tính.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Thuộc tính cuối cùng. Không thể xoá thuộc tính.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OptionController.RemoveByName: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
