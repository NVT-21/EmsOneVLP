using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class WardRepository
    {
        //public static ZResponse SearchListWardByDistrictID(ZWardListRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "WARD_GET_BY_DISTRICT_ID",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "WardRepository.SearchListWardByDistrictID: " + ex.Message);
        //    }
        //    return res;
        //}
        //public static ZResponse SearchWardIDByWardName(ZPDWGetIDByPDWNameRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "WARD_GET_WARD_ID_BY_WARD_NAME",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "WardRepository.SearchWardIDByWardName: " + ex.Message);
        //    }
        //    return res;
        //}
    }
}