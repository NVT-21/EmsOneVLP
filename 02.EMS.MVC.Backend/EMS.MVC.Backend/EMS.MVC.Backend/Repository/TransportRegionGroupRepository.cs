using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Backend.Repository
{
    public class TransportRegionGroupRepository
    {
        public static ZResponse SearchList(ZTransportRegionGroupSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_REGION_GROUP_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRegionGroupRepository.SearchList: " + ex.Message);
            }
            return res;
        }
    }
}