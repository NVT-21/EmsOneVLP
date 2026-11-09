using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class EMSRepository
    {
        #region Gợi ý địa chỉ dạng chính xác
        public static ZResponse EMSMapSearch(ZEMSMapSearchRequest req)
        {
           
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "EMS_MAP_SEARCH",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSMapSearch: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EMSVMapSearch(ZEMSMapSearchRequest req)
        {

            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "EMS_VMAP_SEARCH",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSMapSearch: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region  API trả trạng thái phát kèm thông tin chữ ký, hình ảnh báo phát EMS
        public static ZResponse EMSSignaruteAndImageInformation(ZEMSSignaruteAndImageInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "EMS_SIGNARUTE_AND_IMAGE_INFORMATION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSSignaruteAndImageInformation: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}