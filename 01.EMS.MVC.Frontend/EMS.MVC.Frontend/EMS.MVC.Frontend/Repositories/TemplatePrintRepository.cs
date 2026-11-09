using System;
using System.Collections.Generic;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class TemplatePrintRepository
    {
        public static ZResponse SearchListTemplatePrint(ZTemplatePrintSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TEMPLATE_PRINT_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TemplatePrintRepository.SearchListTemplatePrint: " + ex.Message);
            }
            return res;
        }
    }
}