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
using EMS.API.Core.Controllers;
using EMS.API.Core.Repositories;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Product")]
    public class ProductController : ApiController
    {
        //[Route("Add")]
        //public ZResponse Add(ZProductAddRequest req)
        //{
        //    int result = -1;
        //    int ProductID = 0;
        //    ZResponse res = new ZResponse();
        //    string imageDefault = XHelper.ImageDefault;
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.ADD_NEW", new object[]
        //        {
        //           0,
        //           req.ProductGroupID,          //P_PRODUCT_GROUP_ID NUMBER,
        //           req.Code,                    //P_CODE             VARCHAR,
        //           req.Name,                    //P_NAME             VARCHAR,
        //           req.PricesSell,              //P_PRICES_SELL      NUMBER,
        //           req.Size,                    //P_SIZE             VARCHAR,
        //           req.Weight,                  //P_WEIGHT           NUMBER,
        //           req.Description,             //P_DESCRIPTION      VARCHAR,
        //           imageDefault,                //P_AVATAR           VARCHAR,
        //           req.ShopID,                  //P_SHOP_ID          NUMBER,
        //           req.CreatedBy,               //P_CREATED_BY       NUMBER
        //           0
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    ProductID = Convert.ToInt32(dbCommand.Parameters["P_OUT_PRODUCT_ID"].Value);
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
        //                    res.Data = JsonConvert.SerializeObject(new ZProductAddResponse { ProductID = ProductID });
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Mã sản phẩm đã tồn tại. Vui lòng nhập Mã sản phẩm khác.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //                            break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.Add: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("Add")]
        public ZResponse Add(ZProductAddRequest req)
        {
            
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                string Avatar = XHelper.ImageDefault;
                if (!string.IsNullOrEmpty(req.Avatar))
                {
                    Avatar = req.Avatar;
                }
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.ADD_NEW", new object[]
                {
                   0,
                   req.ProductGroupID,          //P_PRODUCT_GROUP_ID NUMBER,
                   req.Code,                    //P_CODE             VARCHAR,
                   req.Name,                    //P_NAME             VARCHAR,
                   req.Price,                   //P_PRICE     NUMBER,
                   req.Size,                    //P_SIZE             VARCHAR,
                   req.Weight,                  //P_WEIGHT           NUMBER,
                   req.Unit,                    //P_UNIT             NUMBER,
                   req.Description,             //P_DESCRIPTION      VARCHAR,
                   Avatar,                      //P_AVATAR           VARCHAR,
                   req.Images,                  //P_IMAGES           VARCHAR,
                   req.ShopID,                  //P_SHOP_ID          NUMBER,
                   req.CreatedBy,               //P_CREATED_BY       NUMBER
                   XHelper.DeviceName,
                   0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            var ProductID = Convert.ToInt32(dbCommand.Parameters["P_OUT_PRODUCT_ID"].Value);
                            if (req.Variants != null && req.Variants.Count > 0)
                            {
                                foreach (var item in req.Variants)
                                {
                                    var resOP = ProductVariantRepository.Add(new ZProductVariantAddRequest()
                                    {
                                        ProductID = ProductID,
                                        Options = item.Options,
                                        Price = req.Price,
                                        Size = req.Size,
                                        Weight = req.Weight,
                                        BatchNumber = req.BatchNumber,
                                        Avatar = Avatar,
                                        OptionName1 = item.OptionName1,
                                        OptionValue1 = item.OptionValue1,
                                        OptionName2 = item.OptionName2,
                                        OptionValue2 = item.OptionValue2,
                                        OptionName3 = item.OptionName3,
                                        OptionValue3 = item.OptionValue3,
                                        ShopID = req.ShopID,
                                        CreatedBy = req.CreatedBy
                                    });
                                    if (resOP.Code != "00")
                                    {
                                        res.Code = resOP.Code;
                                        res.Message = "Thêm sản phẩm thất bại. " + resOP.Message;
                                        res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                        return res;
                                    }
                                }

                                //Add Option
                                //if (req.Options != null)
                                //{
                                //    int Sort = 1;
                                //    foreach (var item in req.Options)
                                //    {
                                //        var check = ProductOptionAdd(new ZOptionAddRequest()
                                //        {
                                //            ShopID = req.ShopID,
                                //            ProductID = ProductID,
                                //            Name = item.Name,
                                //            Values = item.Values,
                                //            Sort = Sort,
                                //            CreatedBy = req.CreatedBy
                                //        });
                                //        Sort++;
                                //    }
                                //}
                            }
                            // Không có biến thể
                            else
                            {
                                var resOP = ProductVariantRepository.Add(new ZProductVariantAddRequest()
                                {
                                    ProductID = ProductID,
                                    Options = "Mặc định",
                                    Price = req.Price,
                                    Size = req.Size,
                                    Weight = req.Weight,
                                    BatchNumber = req.BatchNumber,
                                    Avatar = Avatar,
                                    OptionName1 = "Tiêu đề",
                                    OptionValue1 = "Mặc định",
                                    OptionName2 = "",
                                    OptionValue2 = "",
                                    OptionName3 = "",
                                    OptionValue3 = "",
                                    ShopID = req.ShopID,
                                    CreatedBy = req.CreatedBy
                                }); ;
                                if (resOP.Code != "00")
                                {
                                    res.Code = resOP.Code;
                                    res.Message = "Thêm sản phẩm thất bại. " + resOP.Message;
                                    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                    return res;
                                }

                                //add option
                                //var check = ProductOptionAdd(new ZOptionAddRequest()
                                //{
                                //    ShopID = req.ShopID,
                                //    ProductID = ProductID,
                                //    Name = "Tiêu đề",
                                //    Values = "Mặc định",
                                //    Sort = 1,
                                //    CreatedBy = req.CreatedBy
                                //});
                            }

                            res = AcceptProduct(ProductID);
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã sản phẩm đã tồn tại. Vui lòng nhập Mã sản phẩm khác.";
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        [HttpPost]
        public ZResponse GetByID(ZProductGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1; 
                ZProductGetByIDResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.GET_BY_ID", new object[]
                {
                    req.ProductID, //P_PRODUCT_ID    NUMBER
                    req.ShopID,    //P_SHOP_ID       NUMBER
                    0,             //P_RETURN_CODE   OUT NUMBER
                    0              //P_OUT_CURSOR    OUT T_CURSOR
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
                                    item = new ZProductGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        Name = dr["NAME"].ToString(),
                                        Avatar = dr["AVATAR"].ToString(),
                                        ProductGroupID = Convert.ToInt32(dr["PRODUCT_GROUP_ID"]),
                                        ProductGroupName = dr["PRODUCT_GROUP_NAME"].ToString(),
                                        PricesSell = Convert.ToInt64(dr["PRICES_SELL"]),
                                        Weight = Convert.ToDouble(dr["WEIGHT"]),
                                        Size = dr["SIZE_PRODUCT"].ToString(),
                                        Unit = Convert.ToInt32(dr["PRODUCT_UNIT_ID"]),
                                        UnitName = dr["PRODUCT_UNIT_NAME"].ToString(),
                                        Description = dr["DESCRIPTION"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        CreatedByFullName = dr["CREATED_BY_FULL_NAME"].ToString(),
                                    };
                                }
                            }
                            //Lấy ảnh
                            //var resImages = GetImagesByProductID(item.ID);
                            //if (resImages.Code != "00")
                            //{
                            //    res.Code = "01";
                            //    res.Message = "Lấy ảnh của sản phẩm thất bại.";
                            //    return res;
                            //}
                            //item.Images = JsonConvert.DeserializeObject<List<ZImageModel>>(resImages.Data);
                            //Lấy biến thể
                            //var resVariant = GetVariantByProductID(item.ID);
                            //if (resVariant.Code != "00")
                            //{
                            //    res.Code = "01";
                            //    res.Message = "Lấy ảnh của sản phẩm thất bại.";
                            //    return res;
                            //}
                            //item.Variant = JsonConvert.DeserializeObject<List<ZVariantByProductID>>(resVariant.Data);
                            if(item != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZProductEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZResponse resAddImage = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.EDIT", new object[]
                {
                   0,
                   req.ID,              //P_ID               NUMBER,
                   req.Code,            //P_CODE             VARCHAR,
                   req.Name,            //P_NAME             VARCHAR,
                   req.ProductGroupID,  //P_PRODUCT_GROUP_ID NUMBER,
                   req.PricesSell,
                   req.Weight,
                   req.Size,
                   req.Description,
                   req.Unit,
                   req.UpdatedBy,       //P_UPDATED_BY       NUMBER
                   req.ShopID           //P_SHOP_ID          NUMBER
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thông tin sản phẩm thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã sản phẩm đã tồn tại. Vui lòng nhập Mã sản phẩm khác.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("List")]
        public ZResponse List(ZProductListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZProductListResponse> oList = new List<ZProductListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.LIST", new object[]
                {
                    req.Code,           //P_ID               VARCHAR
                    req.Name,           //P_NAME             VARCHAR
                    req.ProviderID,     //P_PROVIDER_ID      NUMBER
                    req.ProductGroupID, //P_PRODUCT_GROUP_ID NUMBER
                    req.ShopID,         //P_SHOP_ID          NUMBER
                    req.SearchKey,
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
                                    oList.Add(new ZProductListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        ProductGroupName = dr["PRODUCT_GROUP_NAME"].ToString(),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        Avatar = dr["AVATAR"].ToString(),
                                        PricesSell = Convert.ToInt64(dr["PRICES_SELL"]),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        CreatedByFullName = dr["CREATED_BY_FULL_NAME"].ToString(),
                                        VariantNumber = Convert.ToInt32(dr["VARIANT_NUMBER"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.List: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("EditImages")]
        public ZResponse EditImages(ZProductEditImagesRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.EDIT_IMAGES", new object[]
                {
                   0,
                   req.ProductID,
                   req.Images,                  //P_IMAGES           VARCHAR,
                   req.ShopID,                  //P_SHOP_ID          NUMBER,
                   req.CreatedBy,               //P_CREATED_BY       NUMBER
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
                                    res.Message = "Sản phẩm không tồn tại. Vui lòng kiểm tra lại.";
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.EditImages: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("DataImagesByID")]
        public ZResponse DataImagesByID(ZProductDataImagesByIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZProductDataImagesByIDResponse model = new ZProductDataImagesByIDResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.DATA_IMAGES_BY_ID", new object[]
                {
                    req.ProductID,        //P_ID            NUMBER\
                    req.ShopID,     //
                    0,             //P_RETURN_CODE   OUT NUMBER
                    0              //P_OUT_CURSOR    OUT T_CURSOR
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
                                    model = new ZProductDataImagesByIDResponse()
                                    {
                                        ProductID = Convert.ToInt32(dr["ID"]),
                                        ProductName = dr["NAME"].ToString(),
                                        Images = dr["IMAGES"].ToString(),
                                    };
                                }
                            }
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(model);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.DataImagesByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Remove")]
        public ZResponse Remove(ZProductRemoveRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.REMOVE", new object[]
                {
                   0,
                   req.ID,
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
                                    res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Sản phẩm đã tồn tại trong kho. Không thể xoá.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.Remove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddImages")]
        public ZResponse AddImages(ZProductAddImagesRequest req)
        {
            int result = -1;
            int success = 0;
            int fail = 0;
            int count = req.FileNames.Count;
            ZResponse res = new ZResponse();
            try
            {
                foreach (var item in req.FileNames)
                {
                    using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.ADD_IMAGE", new object[]
                    {
                        0,
                        req.ProductID,
                        item.FileName
                    }))
                    {
                        result = XHelper.ExecuteNonQuery(dbCommand);
                        if (result >= 0)
                        {
                            result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                            if (result > 0)
                            {
                                success++;
                            }
                            else
                            {
                                switch (result)
                                {
                                    case -1:
                                        res.Code = "01";
                                        res.Message = "Thêm thành công " + success + "/" + count + " hình ảnh .Sản phẩm đã tối đa 10 hình ảnh.";
                                        return res;
                                    default:
                                        fail++;
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

                if (success == count)
                {
                    res.Code = "00";
                    res.Message = "Thêm mới hình ảnh thành công!";
                }
                else if (fail == count)
                {
                    res.Code = "99";
                    res.Message = "Thêm mới hình ảnh thất bại!";
                }
                else
                {
                    res.Code = "02";
                    res.Message = "Thêm thành công " + success + "/" + count + " hình ảnh .";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.AddImage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListImage")]
        public ZResponse ListImage(ZProductImageListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZProductImageListResponse> oListItem = new List<ZProductImageListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.LIST_IMAGE", new object[]
                {
                    req.ProductID,  //P_PRODUCT_ID  NUMBER
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
                                    oListItem.Add(new ZProductImageListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Image = dr["FILE_NAME"].ToString()
                                    });
                                }
                            }

                            if (oListItem != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oListItem);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.ListImage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("RemoveImage")]
        public ZResponse RemoveImage(ZProductRemoveRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.REMOVE_IMAGE", new object[]
                {
                   0,
                   req.ID,
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
                                    res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.RemoveImage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateAvatar")]
        public ZResponse UpdateAvatar(ZProductUpdateAvatarRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.UPDATE_AVATAR", new object[]
                {
                   0,
                   req.ID,         //P_ID
                   req.FileName    //P_AVATAR
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
                                    res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.UpdateAvatar: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("SearchOption")]
        //public ZResponse SearchOption(ZOptionAddNewRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_OPTION_PKG.SEARCH", new object[]
        //        {
        //            0,
        //            req.ParentID,
        //            req.Name,
        //            req.ShopID,
        //            0
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
        //                    res.Data = Convert.ToString(dbCommand.Parameters["P_OUT_OPTION_ID"].Value);
        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Lỗi biến thể.";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.Edit: " + ex.Message);
        //        result = -99;
        //    }
        //    return res;
        //}

        [Route("AcceptProduct")]
        public ZResponse AcceptProduct(int ProductID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.ACCEPT_PRODUCT", new object[]
                {
                    0,
                    ProductID
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
                            res.Data = JsonConvert.SerializeObject(new { ID = ProductID });
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tìm thấy sản phẩm để duyệt. Tạo sản phẩm thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.Edit: " + ex.Message);
                result = -99;
            }
            return res;
        }

        [Route("UpdateProductGroup")]
        public ZResponse UpdateProductGroup(ZProductUpdateProductGroupRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZProductUpdateProductGroupResponse> ListError = new List<ZProductUpdateProductGroupResponse>();
            try
            {
                var splitID = req.IDs.Split(',');
                for (var i = 0; i < splitID.Length; i++)
                {
                    res = ProductRepository.UpdateProductGroupByID(Convert.ToInt32(splitID[i]), req.ProductGroupID, req.ShopID, req.UpdatedBy);
                    if (res.Code != "00")
                    {
                        var data = JsonConvert.DeserializeObject<ZProductUpdateProductGroupResponse>(res.Data);
                        ListError.Add(data);
                    }
                }
                if (ListError.Count > 0)
                {
                    var TotalSuccess = splitID.Length - ListError.Count;
                    res.Code = "01";
                    res.Message = "Cập nhật thành công " + TotalSuccess + "/" + splitID.Length + " sản phẩm, vui lòng nhấn vào Xem chi tiết để kiểm tra.";
                    res.Data = JsonConvert.SerializeObject(ListError);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.UpdateProductGroup: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #region Sử dụng trong Product
        public ZResponse GetVariantByProductID(int ProductID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZVariantByProductID> list = new List<ZVariantByProductID>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_VARIANT_PKG.GET_VARIANT_BY_PRODUCT_ID", new object[]
                {
                    ProductID,      //P_PRODUCT_ID  NUMBER
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
                                    list.Add(new ZVariantByProductID()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = dr["NAME"].ToString(),
                                        Image = dr["IMAGE"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        Bracode = dr["BARCODE"].ToString(),
                                        Price = Convert.ToInt64(dr["PRICE"]),
                                        Quantity = Convert.ToInt32(dr["QUANTITY"]),
                                        WarehouseNumber = Convert.ToInt32(dr["WAREHOUSE_NUMBER"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.GetVariantByProductID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public ZResponse GetImagesByProductID(int ProductID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZImageModel> list = new List<ZImageModel>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.LIST_IMAGE", new object[]
                {
                    ProductID,      //P_PRODUCT_ID  NUMBER
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
                                    list.Add(new ZImageModel()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = dr["NAME"].ToString(),
                                        FileName = dr["FILE_NAME"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.GetImagesByProductID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public int ProductOptionAdd(ZOptionAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.PRODUCT_OPTION_ADD_NEW", new object[]
                {
                    0,
                    req.ShopID,
                    req.ProductID,
                    req.Name,
                    req.Values, /*Chuỗi giá trị*/
                    req.Sort,
                    req.CreatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                    }
                    else
                    {
                        result = -2;
                        XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.ProductOptionAdd: Lỗi hệ thống, vui lòng liên hệ quản trị.");
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.ProductOptionAdd: " + ex.Message);
                result = -99;
            }
            return result;
        }
        #endregion

        [Route("ImportExcel")]
        public ZResponse ImportExcel(ZProductImportExcelRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PRODUCT_PKG.PRODUCT_IMPORT_EXCEL", new object[]
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
                                    res.Message = "Mã sản phẩm đã tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Tên biến thể đã tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Lưu lịch sử phiếu xuất kho thất bại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Lưu mã Pallet thất bại.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Thay đổi số lượng của Pallet thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.ImportExcel: " + ex.Message);
            }
            return res;
        }
    }
}