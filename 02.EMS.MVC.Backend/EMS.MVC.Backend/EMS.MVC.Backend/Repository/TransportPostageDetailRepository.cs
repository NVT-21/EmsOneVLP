using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Backend.Repository
{
    public class TransportPostageDetailRepository
    {
        #region Thêm chi tiết
        public static ZResponse Add(ZTransportPostageDetailAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_DETAIL_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageDetailRepository.Add: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Sửa chi tiết
        public static ZResponse Edit(ZTransportPostageDetailEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_DETAIL_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageDetailRepository.Edit: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Xóa chi tiết
        public static ZResponse Remove(ZTransportPostageDetailRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_DETAIL_REMOVE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageDetailRepository.Remove: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lấy thông tin cơ bản của bảng cước bằng ID
        public static ZResponse GetByTransportPostageID(ZTransportPostageDetailGetByTransportPostageIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_DETAIL_GET_BY_TRANSPORT_POSTAGE_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageDetailRepository.GetByTransportPostageID: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}