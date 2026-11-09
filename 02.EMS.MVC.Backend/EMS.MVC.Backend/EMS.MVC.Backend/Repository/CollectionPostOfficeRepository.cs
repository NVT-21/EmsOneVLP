using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Backend.Repository
{
    public class CollectionPostOfficeRepository
    {
        public static ZResponse SearchList(ZCollectionPostOfficeSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "COLLECTION_POST_OFFICE_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeRepository.SearchList: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListByParentID(ZCollectionPostOfficeSearchListByParentIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "COLLECTION_POST_OFFICE_SEARCH_LIST_BY_PARENT_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeRepository.SearchListByParentID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse GetByID(ZCollectionPostOfficeGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "COLLECTION_POST_OFFICE_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeRepository.GetByID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddCollectionPostOffice(ZCollectionPostOfficeAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "COLLECTION_POST_OFFICE_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeRepository.AddCollectionPostOffice: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditCollectionPostOffice(ZCollectionPostOfficeEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "COLLECTION_POST_OFFICE_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeRepository.EditCollectionPostOffice: " + ex.Message);
            }
            return res;
        }
    }
}