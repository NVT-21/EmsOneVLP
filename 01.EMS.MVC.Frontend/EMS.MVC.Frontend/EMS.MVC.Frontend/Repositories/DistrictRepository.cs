using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
namespace EMS.MVC.Frontend.Repositories
{
    public class DistrictRepository
    {
        //public static ZResponse SearchListDistrictByProvinceID(ZDistrictListRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "DISTRICT_GET_BY_PROVINCE_ID",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "DistrictRepository.SearchListDistrictByProvinceID: " + ex.Message);
        //    }
        //    return res;
        //}
        //public static ZResponse SearchDistrictIDByDistrictName(ZPDWGetIDByPDWNameRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "DISTRICT_GET_DIS_ID_BY_DIS_NAME",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "DistrictRepository.SearchDistrictIDByDistrictName: " + ex.Message);
        //    }
        //    return res;
        //}
    }
}