using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
namespace EMS.MVC.Backend.Repository
{
    public class ShopRepository
    {
        public static ZResponse SearchListShop(ZShopSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.SearchListShop: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddShop(ZShopAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.AddShop: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditShop(ZShopEditShopRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_EDIT_SHOP",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.EditShop: " + ex.Message);
            }
            return res;
        }

        public static ZResponse GetByID(ZShopGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.GetByID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse GetHistoryByID(ZShopGetHistoryByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GET_HISTORY_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.GetHistoryByID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse ApprovedShop(ZShopApprovedShopRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_APPROVED_SHOP",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.ApprovedShop: " + ex.Message);
            }
            return res;
        }

        public static ZResponse RejectShop(ZShopRejectShopRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_REJECT_SHOP",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.RejectShop: " + ex.Message);
            }
            return res;
        }
        public static ZResponse ChangeIsLock(ZShopChangeIsLockRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_CHANGE_IS_LOCK",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.ChangeIsLock: " + ex.Message);
            }
            return res;
        }

        public static ZResponse ResetPasswordShop(ZShopResetPasswordShopRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_RESET_PASSWORD_SHOP",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.ResetPasswordShop: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SendEmailRegiterSuccess(ZShopSearchInfoSendEmailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_SEND_EMAIL_REGISTER_SUCCESS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.SendEmailRegiterSuccess: " + ex.Message);
            }
            return res;
        }
    }
}