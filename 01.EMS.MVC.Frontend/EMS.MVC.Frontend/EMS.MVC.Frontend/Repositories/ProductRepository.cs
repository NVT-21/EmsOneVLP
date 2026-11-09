using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Repositories
{
    public class ProductRepository
    {
        public static string ConvertSize(int Length, int Width, int Height)
        {
            string Size = string.Empty;
            if (Length != 0)
            {
                Size += Length + "D";
            }
            if (Width != 0)
            {
                Size += Width + "R";
            }
            if (Height != 0)
            {
                Size += Height + "C";
            }
            return Size;
        }
        //public static List<SelectListItem> SearchListProductGroupName()
        //{
        //    List<SelectListItem> listItem = new List<SelectListItem>();
        //    try
        //    {
        //        var res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "PRODUCT_GROUP_LIST",
        //            Data = JsonConvert.SerializeObject(new ZProductGroupListRequest()
        //            {
        //                Code = "",
        //                Name = "",
        //                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //            })
        //        });
        //        if (res.Code == "00")
        //        {
        //            var list = JsonConvert.DeserializeObject<List<ZProductGroupListResponse>>(res.Data);
        //            foreach (var item in list)
        //            {
        //                listItem.Add(new SelectListItem()
        //                {
        //                    Text = item.Name,
        //                    Value = item.ID.ToString()
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.GetListProductGroupName: " + ex.Message);
        //        listItem = null;
        //    }
        //    return listItem;
        //}
        //public static List<SelectListItem> SearchListProductGroupCode()
        //{
        //    List<SelectListItem> listItem = new List<SelectListItem>();
        //    try
        //    {
        //        var res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "PRODUCT_GROUP_LIST",
        //            Data = JsonConvert.SerializeObject(new ZProductGroupListRequest()
        //            {
        //                Code = "",
        //                Name = "",
        //                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //            })
        //        });
        //        if (res.Code == "00")
        //        {
        //            var list = JsonConvert.DeserializeObject<List<ZProductGroupListResponse>>(res.Data);
        //            foreach (var item in list)
        //            {
        //                listItem.Add(new SelectListItem()
        //                {
        //                    Text = item.Code,
        //                    Value = item.ID.ToString()
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.SearchListProductGroupCode: " + ex.Message);
        //        listItem = null;
        //    }
        //    return listItem;
        //}
        //public static List<ZProductGroupListResponse> SearchListProductGroup()
        //{
        //    List<ZProductGroupListResponse> model = new List<ZProductGroupListResponse>();
        //    try
        //    {
        //        var res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "PRODUCT_GROUP_LIST",
        //            Data = JsonConvert.SerializeObject(new ZProductGroupListRequest()
        //            {
        //                Code = "",
        //                Name = "",
        //                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //            })
        //        });
        //        if (res.Code == "00")
        //        {
        //            model = JsonConvert.DeserializeObject<List<ZProductGroupListResponse>>(res.Data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.SearchListProductGroup: " + ex.Message);
        //    }
        //    return model;
        //}

        //public static ZProductGetByIDResponse ProductGetByID(ZProductGetByIDRequest req) // Lấy thông tin sản phẩm
        //{
        //    ZProductGetByIDResponse model = new ZProductGetByIDResponse();
        //    try
        //    {
        //        var res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "PRODUCT_GET_BY_ID",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //        if (res.Code == "00")
        //        {
        //            model = JsonConvert.DeserializeObject<ZProductGetByIDResponse>(res.Data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.ProductGetByID: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
        //    }
        //    return model;
        //}

        //public static List<ProductVariantModel> ProductVariantGetByProductID(int ProductID)
        //{
        //    List<ProductVariantModel> model = new List<ProductVariantModel>();
        //    var res = ProductVariantRepository.GetByProductID(new ZProductVariantGetByProductIDRequest() 
        //    {
        //        ProductID = ProductID,
        //        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //    });
        //    if (res.Code == "00")
        //    {
        //        var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByProductIDResponse>>(res.Data);
        //        foreach (var item in list)
        //        {
        //            model.Add(new ProductVariantModel()
        //            {
        //                ID = item.ID,
        //                Options = item.Options,
        //                Avatar = item.Avatar,
        //                Price = item.Price
        //            });
        //        }
        //    }
        //    return model;
        //}

        public static ZResponse SearchListProduct(ZProductListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.SearchListProduct: " + ex.Message);
            }
            return res;
        }

        public static ZResponse GetByProductID(ZProductGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.GetByProductID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddProduct(ZProductAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.AddProduct: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditProduct(ZProductEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.EditProduct: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddProductToProductGroup(ZProductUpdateProductGroupRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_UPDATE_PRODUCT_GROUP",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.AddProductToProductGroup: " + ex.Message);
            }
            return res;
        }

        public static ZResponse ImportExcel(ZProductImportExcelRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_IMPORT_EXCEL", //
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductRepository.ImportExcel: " + ex.Message);
            }
            return res;
        }
    }
}