using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class VNPRepository
    {
        #region Gợi ý địa chỉ dạng Autocomplete
        public static ZResponse VNPMapsAutocomplete(ZVNPMapsAutocompleteRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "VNP_MAPS_AUTOCOMPLETE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "VNPRepository.VNPMapsAutocomplete: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Gợi ý địa chỉ dạng chính xác
        public static ZResponse VNPMapsSearch(ZVNPMapsSearchRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "VNP_MAPS_SEARCH",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "VNPRepository.VNPMapsSearch: " + ex.Message);
            }
            return res;
        }
        #endregion

        //#region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo VMapCode (từ gợi ý địa chỉ của VNP)
        //public static ZResponse SearchPWDByVMapCode(ZMapsGetByVMapCodeRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "MAPS_GET_BY_V_MAP_CODE",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "VNPRepository.SearchPWDByVMapCode: " + ex.Message);
        //    }
        //    return res;
        //}
        //#endregion

        //#region Lấy tên Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã bằng id
        //public static ZResponse SearchPDWIDByPDWName(ZMapsGetByPWDNameRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "MAPS_GET_BY_PWD_NAME",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "VNPRepository.SearchPDWIDByPDWName: " + ex.Message);
        //    }
        //    return res;
        //}
        //#endregion

        #region  API trả trạng thái phát kèm thông tin chữ ký, hình ảnh báo phát VNP
        public static ZResponse VNPSignaruteAndImageInformation(ZVNPSignaruteAndImageInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "VNP_SIGNARUTE_AND_IMAGE_INFORMATION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "VNPRepository.VNPSignaruteAndImageInformation: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}