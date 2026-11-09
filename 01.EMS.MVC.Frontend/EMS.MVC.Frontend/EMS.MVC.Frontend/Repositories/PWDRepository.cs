using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
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

        #region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo VMapCode (từ gợi ý địa chỉ của VNP)
        public static ZResponse SearchPWDByVMapCode(ZMapsGetByVMapCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "MAPS_GET_BY_V_MAP_CODE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PWDRepository.SearchPWDByVMapCode: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lấy ID Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã bằng Tên
        public static ZResponse SearchPDWIDByPDWName(ZMapsGetByPWDNameRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "MAPS_GET_BY_PWD_NAME",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PWDRepository.SearchPDWIDByPDWName: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo MBCCode (từ gợi ý địa chỉ của EMS)
        public static ZResponse SearchPWDByMBC2017Code(ZMapsGetByMBC2017CodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "MAPS_GET_BY_MBC_2017_CODE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PWDRepository.SearchPWDByMBC2017Code: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SearchPWDByMBCCode(ZMapsGetByMBC2017CodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "MAPS_GET_BY_MBC_CODE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PWDRepository.SearchPWDByMBC2017Code: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}