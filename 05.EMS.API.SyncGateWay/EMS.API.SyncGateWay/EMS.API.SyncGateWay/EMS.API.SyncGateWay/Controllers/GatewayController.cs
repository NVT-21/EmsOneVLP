using EMS.API.GateWay;
using EMS.API.SyncGateWay.Repositories;
using EMS.Entity;
using EMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.API.SyncGateWay.Controllers
{
    public class GatewayController : ApiController
    {
        [HttpPost]
        [Route("Execute")]
        public ZResponse Execute(ZRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                switch (req.Code)
                {
                    //#region API PNS
                    //case "PNS_EMS_PICKUP_CREATE":
                    //    return PNSRepository.CreatePickupEMS(req.Data);
                    //case "PNS_EMS_PICKUP_CANCEL":
                    //    return PNSRepository.CancelPickupEMS(req.Data);
                    //#endregion

                    #region API ZOHO
                    case "ZOHO_ACCESS_TOKEN_GET":
                        return ZohoRepository.GetAccessTokenZoho();
                    case "ZOHO_LEADS_PHONE_GET":
                        return ZohoRepository.GetZohoCRMLeadsPhone(req.Data);
                    case "ZOHO_LEADS_EMAIL_GET":
                        return ZohoRepository.GetZohoCRMLeadsEmail(req.Data);
                    case "ZOHO_LEADS_ADD":
                        return ZohoRepository.CreateLeadsZohoCRM(req.Data);
                    case "ZOHO_SEND_MAIL":
                        return ZohoRepository.SendMail(req.Data);
                    #endregion

                    #region API Hệ thống CMS (Gửi yêu cầu hỗ trợ)
                    case "CMS_ADD_REQUEST_SUPPORT":
                        return CMSRepository.CMSAddRequestSupport(req.Data);
                    case "CMS_RESULT_REQUEST_SUPPORT":
                        return CMSRepository.CMSResultRequestSupport(req.Data);
                    #endregion

                    #region API Maps VNPost
                    case "VNP_MAPS_AUTOCOMPLETE":
                        return VNPRepository.Autocomplete(req.Data);
                    case "VNP_MAPS_SEARCH":
                        return VNPRepository.Search(req.Data);
                    case "VNP_SIGNARUTE_AND_IMAGE_INFORMATION":
                        return VNPRepository.VNPSignaruteAndImageInformation(req.Data);
                    case "VNP_PICKUP_IMAGE_INFORMATION":
                        return VNPRepository.VNPPickupImageInformation(req.Data);
                    case "VNPOST_MAP_SEARCH":
                        return EMSRepository.VNPOSTMapSearch(req.Data);
                    #endregion

                    #region API PAYPOST
                    case "PAYPOST_EDIT_COD_MONEY":
                        return PaypostRepository.EditCODMoney(req.Data);
                    #endregion

                    #region API EMS
                    //case "EMS_LOGIN":
                    //    return EMSRepository.EMSLogin(req.Data);
                    case "EMS_SEND_REQUEST_TRANSPORT":
                        return EMSRepository.EMSSendRequestTransport(req.Data);
                    case "EMS_CANCEL_REQUEST_TRANSPORT":
                        return EMSRepository.EMSCancelRequestTransport(req.Data);
                    case "EMS_MAP_SEARCH":
                        return EMSRepository.EMSMapSearch(req.Data);
                    case "EMS_VMAP_SEARCH":
                        return EMSRepository.EMSVMapSearch(req.Data);
                    case "EMS_PRINT_LABEL_HM":
                        return EMSRepository.EMSPrintLabelHM(req.Data);
                    case "EMS_PUSH_AFFAIR":
                        return EMSRepository.EMSPushAffair(req.Data);
                    case "EMS_TRADEDISCOUNT_POSTAGE_REFUND":
                        return EMSRepository.EMSPostageRefund(req.Data);
                    case "EMS_TRADEDISCOUNT_DEBT_NOTICES":
                        return EMSRepository.EMSDebtNotices(req.Data);
                    case "EMS_TRADEDISCOUNT_SUMMARY_DEBT_NOTICES":
                        return EMSRepository.EMSSummaryOfDebtNotices(req.Data);
                    case "EMS_SIGNARUTE_AND_IMAGE_INFORMATION":
                        return EMSRepository.EMSSignaruteAndImageInformation(req.Data);
                    case "EMS_EDATA_SIGNARUTE_AND_IMAGE_INFORMATION":
                        return EMSRepository.EdataSignaruteAndImageInformation(req.Data);
                    case "EMS_SEND_RECEIVER_INFOR":
                        return EMSRepository.EMSSendReceiverInfor(req.Data);
                    case "EMS_GET_RECEIVER_INFOR":
                        return EMSRepository.EMSGetReceiverInfor(req.Data);
                    #endregion

                    #region API GRAB
                    case "GRAB_DELIVERY_SERVICE":
                        return GrabRepository.GrabDeliveryService();
                    case "GRAB_DELIVERY_STATUS":
                        return GrabRepository.GrabDeliveryStatus();
                    case "GRAB_DELIVERY_PROVINCE":
                        return GrabRepository.GrabDeliveryProvince();
                    case "GRAB_DELIVERY_DISTRICT":
                        return GrabRepository.GrabDeliveryDistrict();
                    case "GRAB_DELIVERY_WARD":
                        return GrabRepository.GrabDeliveryWard();
                    case "GRAB_DELIVERY_QUOTATION":
                        return GrabRepository.GrabDeliveryQuotation(req.Data);
                    case "GRAB_DELIVERY_ORDER":
                        return GrabRepository.GrabDeliveryOrder(req.Data);
                    case "GRAB_DELIVERY_DETAIL":
                        return GrabRepository.GrabDeliveryDetail(req.Data);
                    case "GRAB_DELIVERY_CANCEL":
                        return GrabRepository.GrabDeliveryCancel(req.Data);
                    #endregion

                    #region API FACEBOOK
                    case "FACEBOOK_CONNECT_PAGE":
                        return FacebookRepository.ConnectPage(req.Data);
                    case "FACEBOOK_ACCOUNT_PROFILE_GET":
                        return FacebookRepository.GetAccountProfile(req.Data);
                    case "FACEBOOK_USER_INFO_BY_ID_GET":
                        return FacebookRepository.GetUserInfoByUserID(req.Data);
                    case "FACEBOOK_PAGE_INFO_GET":
                        return FacebookRepository.GetPageInfo(req.Data);
                    case "FACEBOOK_SUBCRIBED":
                        return FacebookRepository.Subcribed(req.Data);
                    case "FACEBOOK_FEED_GET":
                        return FacebookRepository.GetFeed(req.Data);
                    case "FACEBOOK_FEED_DELETE":
                        return FacebookRepository.DeleteFeed(req.Data);
                    case "FACEBOOK_COMMENT_GET":
                        return FacebookRepository.GetComment(req.Data);
                    case "FACEBOOK_CONVERSATION_GET":
                        return FacebookRepository.GetConversation(req.Data);
                    case "FACEBOOK_MESSAGE_GET":
                        return FacebookRepository.GetMessage(req.Data);
                    case "FACEBOOK_MESSAGE_SEND":
                        return FacebookRepository.SendMessage(req.Data);
                    case "FACEBOOK_COMMENT_SEND":
                        return FacebookRepository.SendComment(req.Data);
                    case "FACEBOOK_COMMENT_DELETE":
                        return FacebookRepository.DeleteComment(req.Data);
                    case "FACEBOOK_COMMENT_LIKE":
                        return FacebookRepository.LikeComment(req.Data);
                    case "FACEBOOK_COMMENT_DELETE_LIKE":
                        return FacebookRepository.DeleteLikeComment(req.Data);
                    case "FACEBOOK_COMMENT_HIDDEN":
                        return FacebookRepository.HiddenComment(req.Data);
                    #endregion

                    default:
                        res.Code = "01";
                        res.Message = "Không tìm thấy thông tin mã hàm.";
                        break;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "SyncGateWayController.Execute: " + ex.Message);
            }
            return res;
        }
    }
}