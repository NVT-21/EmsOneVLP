using System;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EMS.API.Core.Repositories
{
    public class EMSRepository
    {
        #region Gửi yêu cầu vận chuyển qua PNS
        public static ZResponse SendRequestTransport(ZTransportInfoShippingApiRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    //Code = "PNS_EMS_PICKUP_CREATE",
                    Code = "EMS_SEND_REQUEST_TRANSPORT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.SendRequestTransport: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Hủy yêu cầu vận chuyển qua PNS
        public static ZResponse CancelRequestTransport(ZTransportShippingCancelDataRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    //Code = "PNS_EMS_PICKUP_CANCEL",
                    Code = "EMS_CANCEL_REQUEST_TRANSPORT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.CancelRequestTransport: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Gửi thông báo qua EMS
        public static ZResponse PushAffair(List<ZEMSPushAffairRequest> req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "EMS_PUSH_AFFAIR",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.PushAffair: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Lấy dữ liệu hoàn cước
        public static ZResponse EMSPostageRefund(ZReconciliationSearchPostageRefundRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "EMS_TRADEDISCOUNT_POSTAGE_REFUND",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSPostageRefund: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Lấy dữ liệu nợ cước
        public static ZResponse EMSSummaryOfDebtNotices(ZReconciliationSearchDebtNoticesRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "EMS_TRADEDISCOUNT_SUMMARY_DEBT_NOTICES",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSSummaryOfDebtNotices: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse EMSDebtNotices(ZReconciliationSearchDebtNoticesRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "EMS_TRADEDISCOUNT_DEBT_NOTICES",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSDebtNotices: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Phân tích địa chỉ
        public static ZResponse MapSearch(ZEMSMapSearchRequest req)
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.MapSearch: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }


        public static ZResponse MapSearchVNPOST(ZVNPOSTMapSearchRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "VNPOST_MAP_SEARCH",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.MapSearch: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region API trả trạng thái phát kèm thông tin chữ ký, hình ảnh báo phát EMS
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
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region API trả trạng thái phát kèm thông tin chữ ký, hình ảnh báo phát EMS
        public static ZResponse EMSEdataSignaruteAndImageInformation(ZEMSSignaruteAndImageInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "EMS_EDATA_SIGNARUTE_AND_IMAGE_INFORMATION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSSignaruteAndImageInformation: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion
    }
}