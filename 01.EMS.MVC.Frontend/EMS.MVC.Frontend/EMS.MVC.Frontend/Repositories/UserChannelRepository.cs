using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class UserChannelRepository
    {
        public static ZResponse SearchListUserChannel(ZUserChannelSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "USER_CHANNEL_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserChannelRepository.SearchListUserChannel: " + ex.Message);
            }
            return res;
        }
    }
}