using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class ShopRepository
    {
        #region Danh sách mã PP/CRM
        public static ZResponse SearchListCrmOrPaypostCode(ZShopGetListCrmOrPaypostCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GET_LIST_CRM_OR_PP_CODE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.SearchListCrmOrPaypostCode: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lấy thông tin cửa hàng theo ID
        public static ZResponse GetByShopID(ZShopGetByIDRequest req)
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
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.GetByShopID: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Cập nhật thông tin thanh toán của cửa hàng
        public static ZResponse UpdatePaymentInformation(ZShopUpdatePaymentInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_UPDATE_PAYMENT_INFORMATION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.UpdatePaymentInformation: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Đổi địa chỉ lấy hàng
        public static ZResponse ChangePickUpAddress(ZShopChangePickUpAddressRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_CHANGE_PICK_UP_ADDRESS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.ChangePickUpAddress: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lấy thông tin địa chỉ lấy hàng
        public static ZResponse SearchPickUpInformation(ZShopGetPickupInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GET_PICKUP_INFORMATION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.SearchPickUpInformation: " + ex.Message);
            }
            return res;
        }
        #endregion

        public static ZResponse ShopLevelEdit(ZShopLevelEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_LEVEL_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.ShopLevelEdit: " + ex.Message);
            }
            return res;
        }
    }
}