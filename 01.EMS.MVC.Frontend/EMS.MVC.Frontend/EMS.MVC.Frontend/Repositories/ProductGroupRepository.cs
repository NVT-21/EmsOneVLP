using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;

namespace EMS.MVC.Frontend.Repositories
{
    public class ProductGroupRepository
    {
        public static ZResponse SearchListProductGroup(ZProductGroupListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_GROUP_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupRepository.SearchListProductGroup: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchProductGroupByID(ZProductGroupGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_GROUP_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerRepository.SearchProductGroupByID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddProductGroup(ZProductGroupAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_GROUP_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupRepository.AddProductGroup: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditProductGroup(ZProductGroupEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_GROUP_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupRepository.EditProductGroup: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListProduct(ZProductGroupSearchListProductRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_GROUP_SEARCH_LIST_PRODUCT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupRepository.SearchListProduct: " + ex.Message);
            }
            return res;
        }
    }
}