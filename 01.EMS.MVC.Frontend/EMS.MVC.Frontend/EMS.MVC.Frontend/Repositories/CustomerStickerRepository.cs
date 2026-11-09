using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class CustomerStickerRepository
    {
        public static ZResponse SearchListCustomerSticker(ZCustomerStickerListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CUSTOMER_STICKER_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerStickerRepository.SearchListCustomerSticker: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddCustomerSticker(ZCustomerStickerAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CUSTOMER_STICKER_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerStickerRepository.SearchListCustomerSticker: " + ex.Message);
            }
            return res;
        }

        public static ZResponse RemoveCustomerSticker(ZCustomerStickerRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "CUSTOMER_STICKER_REMOVE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerStickerRepository.RemoveCustomerSticker: " + ex.Message);
            }
            return res;
        }
    }
}