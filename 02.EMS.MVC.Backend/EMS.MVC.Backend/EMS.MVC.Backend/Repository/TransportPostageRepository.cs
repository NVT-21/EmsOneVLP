using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Backend.Repository
{
    public class TransportPostageRepository
    {
        #region Danh sách bảng cước
        public static ZResponse SearchList(ZTransportPostageSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.SearchList: " + ex.Message);
            }
            return res;
        }

        #endregion

        #region Dịch vụ chính

        #region Thêm bảng cước
        public static ZResponse MainServiceBasicInfoAdd(ZTransportPostageMainServiceBasicAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_MAIN_SERVICE_BASIC_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.MainServiceBasicInfoAdd: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Cập nhật tab thông tin chung của bảng cước
        public static ZResponse MainServiceBasicInfoEdit(ZTransportPostageMainServiceBasicEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_MAIN_SERVICE_BASIC_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.MainServiceBasicInfoEdit: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Cập nhật tab khai báo cước chính của bảng cước
        public static ZResponse MainServiceFeeEdit(ZTransportPostageMainServiceFeeEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_MAIN_SERVICE_FEE_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.MainServiceFeeEdit: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Cập nhật tab khai báo phụ phí của bảng cước

        public static ZResponse MainServiceSurchargeEdit(ZTransportPostageSurchargeEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_MAIN_SERVICE_SURCHARGE_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.MainServiceSurchargeEdit: " + ex.Message);
            }
            return res;
        }
        #endregion
        #endregion

        #region Dịch vụ cộng thêm

        #region Thêm bảng cước
        public static ZResponse ExtraServiceBasicInfoAdd(ZTransportPostageExtraServiceBasicAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_EXTRA_SERVICE_BASIC_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.ExtraServiceBasicInfoAdd: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Sửa bảng cước
        public static ZResponse ExtraServiceBasicInfoEdit(ZTransportPostageExtraServiceEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_EXTRA_SERVICE_BASIC_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.ExtraServiceBasicInfoEdit: " + ex.Message);
            }
            return res;
        }
        #endregion
        #endregion



        #region Lấy thông tin cơ bản của bảng cước bằng ID
        public static ZResponse GetByTransportPostageID(ZTransportPostageGetByTransportPostageIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_GET_BY_TRANSPORT_POSTAGE_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.GetByTransportPostageID: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Thay đổi trạng thái Mở/ Khóa bảng cước
        public static ZResponse ChangeIsLock(ZTransportPostageChangeIsLockRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_CHANGE_IS_LOCK",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.ChangeIsLock: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Xác nhận bảng cước
        public static ZResponse ConfirmTransportPostage(ZTransportPostageConfirmRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_CONFIRM",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.ConfirmTransportPostage: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Bỏ xác nhận bảng cước
        public static ZResponse UnConfirmTransportPostage(ZTransportPostageUnConfirmRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_UNCONFIRM",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.UnConfirmTransportPostage: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lịch sử
        public static ZResponse SearchHistory(ZTransportPostageSearchHistoryRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_SEARCH_HISTORY",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.SearchHistory: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion








    }
}