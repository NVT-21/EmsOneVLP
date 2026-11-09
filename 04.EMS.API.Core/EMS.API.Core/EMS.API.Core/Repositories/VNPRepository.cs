using System;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace EMS.API.Core.Repositories
{
    public class VNPRepository
    {
        #region API trả trạng thái phát kèm thông tin chữ ký, hình ảnh báo phát VNP
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
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse VNPPickUpImageInformation(ZVNPSignaruteAndImageInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "VNP_PICKUP_IMAGE_INFORMATION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "VNPRepository.VNPSignaruteAndImageInformation: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion
    }
}