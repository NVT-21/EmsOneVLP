using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
namespace EMS.MVC.Backend.Repository
{
    public class PWDRepository
    {
        #region Tỉnh/ Thành phố
        public static ZResponse SearchListProvince(ZProvinceListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PROVINCE_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PWDRepository.SearchListProvince: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Quận/ Huyện

        public static ZResponse SearchListDistrictByProvinceID(ZDistrictGetByProvinceIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "DISTRICT_GET_BY_PROVINCE_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PWDRepository.SearchListDistrictByProvinceID: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SearchDistrictIDByDistrictName(ZPDWGetIDByPDWNameRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "DISTRICT_GET_DIS_ID_BY_DIS_NAME",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PWDRepository.SearchDistrictIDByDistrictName: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Phường/ Xã
        public static ZResponse SearchListWardByDistrictID(ZWardGetByDistrictIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WARD_GET_BY_DISTRICT_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PWDRepository.SearchListWardByDistrictID: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SearchWardIDByWardName(ZPDWGetIDByPDWNameRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WARD_GET_WARD_ID_BY_WARD_NAME",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PWDRepository.SearchWardIDByWardName: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}