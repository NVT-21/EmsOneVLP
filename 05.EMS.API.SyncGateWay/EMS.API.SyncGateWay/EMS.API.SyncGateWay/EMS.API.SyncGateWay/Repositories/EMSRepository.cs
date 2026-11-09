using EMS.API.GateWay;
using EMS.API.GateWay.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.SyncGateWay.Repositories
{
    public class EMSRepository
    {
        //public static ZResponse EMSLogin(string data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var resEMSLogin = APILibraryRepository.EMSLogin(new ZRequest()
        //        {
        //            Code = "api/User_Customer/Login",
        //            Data = data
        //        });

        //        if(resEMSLogin.Code == "00")
        //        {
        //            var ResponseData = JsonConvert.DeserializeObject<ZEMSLoginResponse>(resEMSLogin.Data);
        //            switch (ResponseData.Code)
        //            {
        //                case "1":
        //                    res.Code = "00";
        //                    break;
        //                default:
        //                    res.Code = ResponseData.Code;
        //                    break;
        //            }
        //            res.Data = ResponseData.Token;
        //            res.Message = ResponseData.Message;
        //        }
        //        else
        //        {
        //            res.Code = resEMSLogin.Code;
        //            res.Message = resEMSLogin.Message;
        //            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSLogin: req={0}||res={1}", data, JsonConvert.SerializeObject(resEMSLogin)));
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSLogin: " + ex.Message);
        //    }
        //    return res;
        //}

        #region Gửi yêu cầu vận chuyển
        public static ZResponse EMSSendRequestTransport(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var callAPI = APILibraryRepository.InvokeEMS(new ZEMSRequest()
                {
                    Method = "POST",
                    RequestURL = APILibraryRepository.EMSApiURL,
                    RequestURI = "api/Pickup/CreatePickupv1",
                    APIKeyName = "APIKey",
                    APIKeyValue = APILibraryRepository.EMSApiKey,
                    AuthorizationName = "Basic",
                    AuthorizationValue = APILibraryRepository.EMSApiAuthorization,
                    Value = JsonConvert.DeserializeObject<ZTransportInfoShippingApiRequest>(data)
                });
                if (callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZEMSResponse>(callAPI.Data);
                    if(resEMS.Code == "00")
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        res.Data = resEMS.Value;
                    }
                    else
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSSendRequestTransport: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSSendRequestTransport: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }
        #endregion

        #region Hủy yêu cầu vận chuyển
        public static ZResponse EMSCancelRequestTransport(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var dataObject = JsonConvert.DeserializeObject<ZTransportShippingCancelRequest>(data);
                var callAPI = APILibraryRepository.InvokeEMS(new ZEMSRequest()
                {
                    Method = "POST",
                    RequestURL = APILibraryRepository.EMSGatewayUrl,
                    RequestURI = $"/api/Order/Merchant/Shipment/Cancel?ItemCode={dataObject.TrackingCode}",
                    APIKeyName = "API_KEY",
                    APIKeyValue = APILibraryRepository.EMSGatewayAuthor,
                    Value = JsonConvert.DeserializeObject<ZTransportShippingCancelRequest>(data)
                });
                if (callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZEMSResponse>(callAPI.Data);
                    if (resEMS.Code == "00")
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                    }
                    else
                    {
                        switch (resEMS.Code)
                        {
                            case "01":
                                res.Code = "01";
                                res.Message = "Không tìm thấy thông tin đơn hàng, vui lòng kiểm tra lại";
                                break;
                            case "02":
                                res.Code = "02";
                                res.Message = "Đơn hàng đã phát hành không thể hủy, vui lòng kiểm tra lại";
                                break;
                            case "102":
                                res.Code = "102";
                                res.Message = "Chữ ký xác thực không chính xác, vui lòng kiểm tra lại";
                                break;
                            default:
                                res.Code = "99";
                                res.Message = "Mã lỗi không trùng khớp, Vui lòng liên hệ quản trị.";
                                break;
                        }
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSCancelRequestTransport: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSCancelRequestTransport: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }
        #endregion

        #region Phân tích địa chỉ
        public static ZResponse EMSMapSearch(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZEMSMapSearchRequest>(data);
                var callAPI = APILibraryRepository.InvokeEMS(new ZEMSRequest()
                {
                    Method = "GET",
                    RequestURL = APILibraryRepository.EMSMapsMCSURL,
                    RequestURI = string.Format("api/address/search/?text={0}", req.AddressFull)
                });
                if (callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZEMSMapSearchResponse>(callAPI.Data);
                    if(resEMS.Code == "SUCCESS")
                    {
                        res.Code = "00";
                        res.Message = resEMS.Message;
                        res.Data = JsonConvert.SerializeObject(resEMS.data);
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSMapSearch: request={0}||response={1}||url={2}", data, JsonConvert.SerializeObject(res), APILibraryRepository.EMSMapsMCSURL+ string.Format("api/address/search/?text={0}", req.AddressFull)));
                    }
                    else
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSMapSearch: request={0}||response={1}||url={2}", data, JsonConvert.SerializeObject(res), APILibraryRepository.EMSMapsMCSURL + string.Format("api/address/search/?text={0}", req.AddressFull)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSMapSearch: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }

        public static ZResponse VNPOSTMapSearch(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZVNPOSTMapSearchRequest>(data);
                var callAPI = APILibraryRepository.InvokeEMS(new ZEMSRequest()
                {
                    Method = "POST",
                    RequestURL = APILibraryRepository.VNPOSTMapsURL,
                    RequestURI = string.Format("api/lookup/convert"),
                    APIKeyName = "API-key",
                    APIKeyValue = APILibraryRepository.VNPOSTMapsKey,
                    Value = req
                });
                if(callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZVNPOSTMapSearchResponse>(callAPI.Data);
                    if(resEMS.status == 200)
                    {
                        res.Code = "00";
                        res.Message = resEMS.message;
                        res.Data = JsonConvert.SerializeObject(resEMS.data);
                    }
                    else
                    {
                        res.Code = resEMS.status.ToString();
                        res.Message = resEMS.message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSMapSearch: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSMapSearch: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }

        public static ZResponse EMSVMapSearch(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZEMSMapSearchRequest>(data);
                var callAPI = APILibraryRepository.InvokeEMS(new ZEMSRequest()
                {
                    Method = "GET",
                    RequestURL = APILibraryRepository.EMSGatewayUrl,
                    APIKeyName = "API_KEY",
                    APIKeyValue = APILibraryRepository.EMSGatewayAuthor,
                    RequestURI = string.Format("api/Map/VMap/Search?text={0}", req.AddressFull)
                });
                if(callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZEMSMapSearchResponse>(callAPI.Data);
                   if(resEMS.Code == "SUCCESS")
                    {
                        res.Code = "00";
                        res.Message = resEMS.Message;
                        res.Data = JsonConvert.SerializeObject(resEMS.data);
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSVMapSearch: request={0}||response={1}||url={2}", data, JsonConvert.SerializeObject(res), APILibraryRepository.EMSGatewayUrl + string.Format("api/address/search/?text={0}", req.AddressFull)));
                    }
                    else
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSVMapSearch: request={0}||response={1}||url={2}", data, JsonConvert.SerializeObject(res), APILibraryRepository.EMSGatewayUrl + string.Format("api/address/search/?text={0}", req.AddressFull)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSMapSearch: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }
        #endregion

        #region API thông tin người nhận
        public static ZResponse EMSSendReceiverInfor(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var callAPI = APILibraryRepository.InvokeGatewayEMS(new ZEMSRequest()
                {
                    Method = "POST",
                    RequestURL = APILibraryRepository.EMSGatewayUrl,
                    RequestURI = "/Item/UpdateReceiverInfor",
                    APIKeyName = "API_KEY",
                    APIKeyValue = APILibraryRepository.EMSGatewayAuthor,
                    Value = JsonConvert.DeserializeObject<ZSendReceiverInforRequest>(data)
                });
                if(callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZEMSResponse>(callAPI.Data);
                    if(resEMS.Code == "00")
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        res.Data = resEMS.Value;
                    }
                    else
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSSendRequestTransport: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }

            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSSendReceiverInfor: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }

        public static ZResponse EMSGetReceiverInfor(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var callAPI = APILibraryRepository.InvokeGatewayEMS(new ZEMSRequest()
                {
                    Method = "POST",
                    RequestURL = APILibraryRepository.EMSGatewayUrl,
                    RequestURI = "/Item/GetListReceiverInfor",
                    APIKeyName = "API_KEY",
                    APIKeyValue = APILibraryRepository.EMSGatewayAuthor,
                    Value = JsonConvert.DeserializeObject<ZGetReceiverInforRequest>(data)
                });
                if(callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZBaseResponseAPI<List<ZReceiverInforDCSModel>>>(callAPI.Data);
                    if(resEMS.Code == "00")
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        res.Data = JsonConvert.SerializeObject(resEMS.Data);
                    }
                    else
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSGetRequestTransport: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }

            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSSendReceiverInfor: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }
        #endregion

        #region Đẩy thông tin sửa tiền COD sang EMS
        public static ZResponse EMSPushAffair(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                //var req = JsonConvert.DeserializeObject<List<ZEMSPushAffairRequest>>(data);
                var callAPI = APILibraryRepository.InvokeEMS(new ZEMSRequest()
                {
                    Method = "POST",
                    RequestURL = APILibraryRepository.EMSPushAffairUrl,
                    RequestURI = "api/Affair/PushAffair",
                    AuthorizationName = "Basic",
                    AuthorizationValue = APILibraryRepository.EMSPushAffairKey,
                    ListValue = JsonConvert.DeserializeObject<List<object>>(data),
                });
                if (callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZEMSPushAffairResponse>(callAPI.Data);
                    if(resEMS.Code == "00")
                    {
                        res.Code = resEMS.Code;
                        res.Data = JsonConvert.SerializeObject(resEMS.listResponseAFFAIR);
                        res.Message = resEMS.Message;
                    }
                    else
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSSendRequestTransport: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSPushAffair: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }
        #endregion

        #region PostageRefund - DebtNotices

        public static ZResponse EMSPostageRefund(string data)
        {
            var res = APILibraryRepository.EMSPostageRefund(new ZRequest()
            {
                Code = "api/emsone/thong_tin_cktm/hoan_cuoc",
                Data = data
            });

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZReconciliationSearchPostageRefundResponse>(res.Data);
                res.Code = ResponseData.Code;
                res.Data = JsonConvert.SerializeObject(ResponseData.Data);
                res.Message = ResponseData.Message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSPostageRefund: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse EMSDebtNotices(string data)
        {
            var res = APILibraryRepository.EMSDebtNotices(new ZRequest()
            {
                Code = "api/emsone/thong_tin_cktm/giay_bao_no",
                Data = data
            });

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZReconciliationSearchDebtNoticesResponse>(res.Data);
                res.Code = ResponseData.Code;
                res.Data = JsonConvert.SerializeObject(ResponseData.Data);
                res.Message = ResponseData.Message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSDebtNotices: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse EMSSummaryOfDebtNotices(string data)
        {
            var res = APILibraryRepository.EMSDebtNotices(new ZRequest()
            {
                Code = "api/emsone/thong_tin_cktm/giay_bao_no_tong_hop",
                Data = data
            });

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZReconciliationSearchSummaryOfDebtNoticesResponse>(res.Data);
                res.Code = ResponseData.Code;
                res.Data = JsonConvert.SerializeObject(ResponseData.Data);
                res.Message = ResponseData.Message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSSummaryOfDebtNotices: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
            }
            return res;
        }
        #endregion

        #region API trả trạng thái phát kèm thông tin chữ ký, hình ảnh báo phát EMS
        public static ZResponse EMSSignaruteAndImageInformation(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZEMSSignaruteAndImageInformationRequest>(data);
                var callAPI = APILibraryRepository.InvokeEMS(new ZEMSRequest()
                {
                    Method = "GET",
                    RequestURL = APILibraryRepository.EMSApiURL,
                    RequestURI = string.Format("api/TrackTrace/Lading?LadingCode={0}", req.LadingCode),
                    APIKeyName = "APIKey",
                    APIKeyValue = APILibraryRepository.EMSApiKey,
                });
                if (callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZEMSResponse>(callAPI.Data);
                    if (resEMS.Code == "00")
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        res.Data = JsonConvert.SerializeObject(resEMS.ListValue);
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSSignaruteAndImageInformation: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                    else
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSSignaruteAndImageInformation: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSSignaruteAndImageInformation: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }
        #endregion

        #region API Edata trả trạng thái phát kèm thông tin chữ ký, hình ảnh báo phát EMS
        public static ZResponse EdataSignaruteAndImageInformation(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZEMSSignaruteAndImageInformationRequest>(data);
                var reqOj = new { Itemcode = req.LadingCode };
                var callAPI = APILibraryRepository.InvokeVNP(new ZVNPRequest()
                {
                    Method = "POST",
                    RequestURL = APILibraryRepository.EdataImgUrl,
                    RequestURI = string.Format("api/TrackAndTrace/TrackingStatusVIB"),
                    APIKeyName = "Authorization-Token",
                    APIKeyValue = APILibraryRepository.EdataImgKey,
                    Value = reqOj
                });
                if(callAPI.Code == "00")
                {
                    var resVNP = JsonConvert.DeserializeObject<ZVNPResponse>(callAPI.Data);
                    if(resVNP.Code == "00")
                    {
                        res.Code = resVNP.Code;
                        res.Message = resVNP.Message;
                        res.Data = JsonConvert.SerializeObject(resVNP.ListValue);
                    }
                    else
                    {
                        res.Code = resVNP.Code;
                        res.Message = resVNP.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EdataSignaruteAndImageInformation: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EdataSignaruteAndImageInformation: request={0}||exception={1}", data, ex.Message));
            }
            return res;
        }
        #endregion\

        #region API in nhãn cho H&M
        public static ZResponse EMSPrintLabelHM(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var callAPI = APILibraryRepository.InvokeGatewayEMS(new ZEMSRequest()
                {
                    Method = "POST",
                    RequestURL = "http://192.168.68.161:200",
                    RequestURI = "/api/OrderHM",
                    //APIKeyName = "API_KEY",
                    //APIKeyValue = APILibraryRepository.EMSGatewayAuthor,
                    Value = JsonConvert.DeserializeObject<ZPartnerHMOrderCreateLabelRequest>(data)
                });
                if(callAPI.Code == "00")
                {
                    var resEMS = JsonConvert.DeserializeObject<ZEMSResponse>(callAPI.Data);
                    if(resEMS.Code == "00")
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        res.Data = callAPI.Data;
                    }
                    else
                    {
                        res.Code = resEMS.Code;
                        res.Message = resEMS.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("EMSRepository.EMSPrintLabelHM: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }

            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("EMSRepository.EMSSendReceiverInfor: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }
        #endregion
    }
}