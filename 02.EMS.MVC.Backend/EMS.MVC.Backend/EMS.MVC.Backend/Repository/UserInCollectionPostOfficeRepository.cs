using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Backend.Repository
{
    public class UserInCollectionPostOfficeRepository
    {
        //public static ZResponse SearchList(ZUserInCollectionPostOfficeSearchListRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "USER_IN_COLLECTION_POST_OFFICE_SEARCH_LIST",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "UserInCollectionPostOfficeRepository.SearchList: " + ex.Message);
        //    }
        //    return res;
        //}

        public static ZResponse SearchListByUserID(ZUserInCollectionPostOfficeSearchListByUserIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "USER_IN_COLLECTION_POST_OFFICE_SEARCH_LIST_BY_USER_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserInCollectionPostOfficeRepository.SearchListByUserID: " + ex.Message);
            }
            return res;
        }
    }
}