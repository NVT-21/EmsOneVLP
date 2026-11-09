using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
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
                    Code = CommandCodeConst.COLLECTION_POST_OFFICE_SEARCH_LIST,
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
        public static ZResponse SearchListRecent(ZCollectionPostOfficeSearchListRecentRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = CommandCodeConst.COLLECTION_POST_OFFICE_SEARCH_LIST_RECENT,
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeRepository.SearchListRecent: " + ex.Message);
            }
            return res;
        }
        
    }
}