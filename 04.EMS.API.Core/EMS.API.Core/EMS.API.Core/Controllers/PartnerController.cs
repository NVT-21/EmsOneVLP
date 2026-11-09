using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMS.API.Core.Repositories;
using System.Diagnostics;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Partner")]
    public class PartnerController : ApiController
    {
        #region DANH SÁCH ĐỐI TÁC
        [Route("SearchList")]
        public ZResponse SearchList(ZPartnerSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZPartnerSearchListResponse> lst = new List<ZPartnerSearchListResponse>();
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_PKG.SEARCH_LIST_PARTNER", new object[]
                {
                    req.IsLock,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if(result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if(result > 0)
                        {
                            using(IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while(dr.Read())
                                {
                                    lst.Add(new ZPartnerSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["PARTNER_ID"]),
                                        PartnerCode = Convert.ToString(dr["PARTNER_CODE"]),
                                        PartnerName = Convert.ToString(dr["PARTNER_NAME"]),
                                    });
                                }
                            }

                            if(lst.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
                        }
                        else
                        {
                            switch(result)
                            {
                                case -99:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.SearchList " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region CÁC API LIÊN QUAN ĐẾN ĐƠN HÀNG

        #region Tạo đơn hàng dùng cho tất cả đối tác
        [Route("OrderAdd")]
        public ZResponse OrderAdd(ZPartnerOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {

                var resCheckOrder = PartnerRepository.OrderAdd(req);
                if(resCheckOrder.Code == "00")
                {
                    ZResponse resOrderAdd = new ZResponse();
                    var itemCheckOrder = JsonConvert.DeserializeObject<ZOrderAddRequest>(resCheckOrder.Data);
                    if(itemCheckOrder.isSendTransport == "N")
                    {
                        resOrderAdd = OrderRepository.AddOrderV2(itemCheckOrder);
                    }
                    else if(itemCheckOrder.isSendTransport == "Y")
                    {
                        resOrderAdd = OrderRepository.AddOrderAndSendTransportV2(itemCheckOrder);
                    }
                    else
                    {
                        resOrderAdd.Code = "02";
                        resOrderAdd.Message = "Loại hình kinh doanh không hợp lệ.";
                    }

                    if(resOrderAdd.Code == "00")
                    {
                        ZPartnerOrderAddResponse item = null;
                        var resOrderInfo = OrderRepository.GetByIDV2(new ZOrderGetByIDRequest() { ID = Convert.ToInt32(resOrderAdd.Data) });
                        if(resOrderInfo.Code == "00")
                        {
                            var itemOrderInfo = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderInfo.Data);
                            item = new ZPartnerOrderAddResponse()
                            {
                                OrderID = itemOrderInfo.ID,
                                EMSOneCode = itemOrderInfo.EMSOneCode,
                                OrderCode = itemOrderInfo.OrderCode,
                                ShippingCode = itemOrderInfo.ShippingCode,
                                TransportFee = itemOrderInfo.TransportFee,
                                TransportFeeDetail = new ZTransportPostageChargedResponse
                                {
                                    MainFee = req.TransportInfo.MainFee,
                                    ExtraFee = req.TransportInfo.ExtraFee,
                                    Surcharge = req.TransportInfo.Surcharge,
                                    TotalAmount = req.TransportInfo.TotalAmount,
                                    MainFeeNotVAT = req.TransportInfo.MainFeeNotVAT,
                                    ExtraFeeNotVAT = req.TransportInfo.ExtraFeeNotVAT,
                                    SurchargeNotVAT = req.TransportInfo.SurchargeNotVAT,
                                    TotalAmountNotVAT = req.TransportInfo.TotalAmountNotVAT,
                                    TransportPostageExtraChargedDetail = req.TransportInfo.TransportPostageExtraChargedDetail
                                }
                            };

                            if(item != null)
                            {
                                res.Code = "00";
                                res.Message = resOrderAdd.Message;
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "03";
                                res.Message = "Lấy thông tin đơn hàng thất bại.";
                            }
                        }
                        else
                        {
                            res.Code = "03";
                            res.Message = "Lấy thông tin đơn hàng thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = resOrderAdd.Code;
                        res.Message = resOrderAdd.Message;
                    }
                }
                else
                {
                    res.Code = resCheckOrder.Code;
                    res.Message = resCheckOrder.Message;
                    res.Data = resCheckOrder.Data;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.OrderAdd: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("InternationalOrderAdd")]
        public ZResponse InternationalOrderAdd(ZPartnerInternalOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resCheckOrder = PartnerRepository.InternationalOrderAdd(req);
                if(resCheckOrder.Code == "00")
                {
                    ZResponse resOrderAdd = new ZResponse();
                    var itemCheckOrder = JsonConvert.DeserializeObject<ZInternationalOrderAddRequest>(resCheckOrder.Data);
                    resOrderAdd = OrderRepository.InternalOrderAdd(itemCheckOrder);
                    if(resOrderAdd.Code == "00")
                    {
                        res.Code = "00";
                        res.Message = "Success";
                        res.Data = resOrderAdd.Data;
                    }
                    else
                    {
                        res.Code = resOrderAdd.Code;
                        res.Message = resOrderAdd.Message;
                    }
                }
                else
                {
                    res.Code = resCheckOrder.Code;
                    res.Message = resCheckOrder.Message;
                    res.Data = resCheckOrder.Data;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message.ToString();
            }
            return res;
         }
        [Route("OrderAddProductBySKUVinaphone")]
        public ZResponse OrderAddProductBySKU(ZPartnerVinaphoneOrderAddRequest req)
        {
            // ProductID lấy theo ProductID của khách hàng, bắt buộc là số để map về ProductID trong EMSONE
            ZResponse res = new ZResponse();
            try
            {
                var resCheckOrder = PartnerRepository.OrderAddVinaphoneConvertSKU(req);
                if(resCheckOrder.Code == "00" || resCheckOrder.Code == "02")
                {
                    ZResponse resOrderAdd = new ZResponse();
                    var itemCheckOrder = JsonConvert.DeserializeObject<ZOrderAddRequest>(resCheckOrder.Data);
                    if(itemCheckOrder.isSendTransport == "N")
                    {
                        resOrderAdd = OrderRepository.AddOrderV2(itemCheckOrder);
                    }
                    else if(itemCheckOrder.isSendTransport == "Y")
                    {
                        resOrderAdd = OrderRepository.AddOrderAndSendTransportV2(itemCheckOrder);
                    }
                    else
                    {
                        resOrderAdd.Code = "02";
                        resOrderAdd.Message = "Loại hình kinh doanh không hợp lệ.";
                    }

                    if(resOrderAdd.Code == "00")
                    {
                        ZPartnerOrderAddResponse item = null;
                        var resOrderInfo = OrderRepository.GetByIDV2(new ZOrderGetByIDRequest() { ID = Convert.ToInt32(resOrderAdd.Data) });
                        if(resOrderInfo.Code == "00")
                        {
                            var itemOrderInfo = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderInfo.Data);
                            item = new ZPartnerOrderAddResponse()
                            {
                                OrderID = itemOrderInfo.ID,
                                EMSOneCode = itemOrderInfo.EMSOneCode,
                                OrderCode = itemOrderInfo.OrderCode,
                                ShippingCode = itemOrderInfo.ShippingCode,
                                TransportFee = itemOrderInfo.TransportFee,
                                TransportFeeDetail = new ZTransportPostageChargedResponse
                                {
                                    MainFee = req.TransportInfo.MainFee,
                                    ExtraFee = req.TransportInfo.ExtraFee,
                                    Surcharge = req.TransportInfo.Surcharge,
                                    TotalAmount = req.TransportInfo.TotalAmount,
                                    MainFeeNotVAT = req.TransportInfo.MainFeeNotVAT,
                                    ExtraFeeNotVAT = req.TransportInfo.ExtraFeeNotVAT,
                                    SurchargeNotVAT = req.TransportInfo.SurchargeNotVAT,
                                    TotalAmountNotVAT = req.TransportInfo.TotalAmountNotVAT,
                                    TransportPostageExtraChargedDetail = req.TransportInfo.TransportPostageExtraChargedDetail
                                }
                            };

                            if(item != null)
                            {
                                res.Code = "00";
                                res.Message = resOrderAdd.Message;
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "03";
                                res.Message = "Lấy thông tin đơn hàng thất bại.";
                            }
                        }
                        else
                        {
                            res.Code = "03";
                            res.Message = "Lấy thông tin đơn hàng thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = resOrderAdd.Code;
                        res.Message = resOrderAdd.Message;
                    }
                }
                else
                {
                    res.Code = resCheckOrder.Code;
                    res.Message = resCheckOrder.Message;
                    res.Data = resCheckOrder.Data;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.OrderAdd: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("OrderDeliveryInfor")]
        public ZResponse OrderDeliveryInfor(ZPartnerOrderInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                ZPartnerInfoResponse PartnerInfoData = null;
                req.CrmOrPaypostCode = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "" : req.CrmOrPaypostCode.Trim();
                req.CustomerToken = string.IsNullOrEmpty(req.CustomerToken) ? "" : req.CustomerToken.Trim();

                if(string.IsNullOrEmpty(req.CrmOrPaypostCode) && string.IsNullOrEmpty(req.CustomerToken))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập CrmOrPaypostCode";
                    return res;
                }

                req.OrderCode = string.IsNullOrEmpty(req.OrderCode) ? "" : req.OrderCode.ToUpper().Trim();
                req.ShippingCode = string.IsNullOrEmpty(req.ShippingCode) ? "" : req.ShippingCode.Trim();
                if(string.IsNullOrEmpty(req.OrderCode) && string.IsNullOrEmpty(req.ShippingCode))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập ShippingCode.";
                    return res;
                }

                var resPartnerInfo = PartnerRepository.PartnerInfo(new ZPartnerInfoRequest() { CrmOrPaypostCode = req.CrmOrPaypostCode.Trim() });
                if(resPartnerInfo.Code == "00")
                {
                    PartnerInfoData = JsonConvert.DeserializeObject<ZPartnerInfoResponse>(resPartnerInfo.Data);
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không tồn tại." });
                    return res;
                }

                int result = -1;
                ZPartnerOrderDetailByShippingCodeResponse item = null;
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.DETAIL_BY_SHIPPING_CODE", new object[]
                {
                    req.ShippingCode.ToUpper().Trim(),     //P_SHIPPING_CODE NUMBER
                    PartnerInfoData.ShopID,           //P_SHOP_ID       NUMBER
                    0,                    //P_RETURN_CODE   OUT NUMBER
                    0                     //P_OUT_CURSOR    OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if(result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if(result > 0)
                        {
                            using(IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                if(dr.Read())
                                {
                                    item = new ZPartnerOrderDetailByShippingCodeResponse()
                                    {
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        TransportMainServiceName = dr["TRANSPORT_MAIN_SERVICE_NAME"].ToString(),
                                        TransportExtraServiceCode = dr["TRANSPORT_EXTRA_SERVICE_CODE"].ToString(),
                                        TransportExtraServiceName = dr["TRANSPORT_EXTRA_SERVICE_NAME"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        TransportSizeLength = Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"]),
                                        TransportSizeWidth = Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"]),
                                        TransportSizeHeight = Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"]),
                                        TransportWeight = Convert.ToInt64(dr["TRANSPORT_WEIGHT"]),
                                        ExchangeTransportWeight = string.IsNullOrEmpty(dr["EXCHANGE_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToInt64(dr["EXCHANGE_TRANSPORT_WEIGHT"]),
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                        TotalPHU = Convert.ToInt64(dr["TOTAL_PHU"]),
                                        Message = dr["MESSAGE"].ToString(),

                                        PosCodeAccept = dr["POS_CODE_ACCEPT"].ToString(),
                                        PosNameAccept = dr["POS_NAME_ACCEPT"].ToString(),
                                        PosTelAccept = dr["POS_TEL_ACCEPT"].ToString(),

                                        SenderName = dr["SENDER_NAME"].ToString(),
                                        SenderPhone = dr["SENDER_PHONE"].ToString(),
                                        SenderFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),

                                        PosCodeDelivery = dr["POS_CODE_DELIVERY"].ToString(),
                                        PosNameDelivery = dr["POS_NAME_DELIVERY"].ToString(),
                                        PosTelDelivery = dr["POS_TEL_DELIVERY"].ToString(),

                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        ReceiverPhone = dr["RECEIVER_PHONE"].ToString(),
                                        ReceiverFullAddress = dr["RECEIVER_FULL_ADDRESS"].ToString(),

                                        StatusName = XData.GetNameOrderStatus(dr["STATUS"].ToString()),
                                        StatusChar = dr["STATUS"].ToString(),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        ParentListID = dr["PARENT_LIST_ID"].ToString(),

                                        ListSearchHistory = new List<ZOrderSearchHistoryResponse>(),
                                        ListSearchHistoryDelivery = new List<ZOrderSearchHistoryDeliveryResponse>(),
                                        ListUpdateInformation = new List<ZRequestSupportHistorySearchListUpdateInformationResponse>(),
                                        ListEMSSignaruteAndImageInfo = new List<ZEMSSignaruteAndImageInformationResponse>(),
                                        ListVNPSignaruteAndImageInfo = new List<ZVNPSignaruteAndImageInformationResponse>(),
                                    };
                                }
                            }

                            if(item != null)
                            {
                                if((PartnerInfoData.ShopID == item.ShopID ||
                                     (item.ParentListID?.Split(',').Contains(PartnerInfoData.ShopID.ToString()) ?? false)))
                                {
                                    item.SenderPhone = string.Format("{0}****{1}", item.SenderPhone.Substring(0, 3), item.SenderPhone.Substring(item.SenderPhone.Length - 3, 3));

                                    var SplitSenderFullAddress = item.SenderFullAddress.Split(',');
                                    if(SplitSenderFullAddress.Length > 1)
                                    {
                                        var TempSenderFullAddress = "";
                                        for(var i = SplitSenderFullAddress.Length - 2; i < SplitSenderFullAddress.Length; i++)
                                        {
                                            TempSenderFullAddress += SplitSenderFullAddress[i] + ",";
                                        }
                                        item.SenderFullAddress = string.Format("***, {0}", TempSenderFullAddress.Trim(','));
                                    }
                                    var SplitReceiverFullAddress = item.ReceiverFullAddress.Split(',');
                                    if(SplitReceiverFullAddress.Length > 1)
                                    {
                                        var TempReceiverFullAddress = "";
                                        for(var i = SplitReceiverFullAddress.Length - 2; i < SplitReceiverFullAddress.Length; i++)
                                        {
                                            TempReceiverFullAddress += SplitReceiverFullAddress[i] + ",";
                                        }
                                        item.ReceiverFullAddress = string.Format("***, {0}", TempReceiverFullAddress.Trim(','));
                                    }



                                    /*Lấy lịch sử đơn hàng*/
                                    var resSearchHistory = OrderRepository.SearchHistory(new ZOrderSearchHistoryRequest() { OrderID = item.OrderID });
                                    if(resSearchHistory.Code == "00")
                                    {
                                        item.ListSearchHistory = JsonConvert.DeserializeObject<List<ZOrderSearchHistoryResponse>>(resSearchHistory.Data);
                                    }

                                    /*Lấy lịch sử vận chuyển*/
                                    var resSearchHistoryDelivery = OrderRepository.SearchHistoryDelivery(new ZOrderSearchHistoryDeliveryRequest() { ShippingCode = item.ShippingCode });
                                    if(resSearchHistoryDelivery.Code == "00")
                                    {
                                        item.ListSearchHistoryDelivery = JsonConvert.DeserializeObject<List<ZOrderSearchHistoryDeliveryResponse>>(resSearchHistoryDelivery.Data);
                                    }



                                    if(
                                        (PartnerInfoData.ShopID == item.ShopID ||
                                         (item.ParentListID?.Split(',').Contains(PartnerInfoData.ShopID.ToString()) ?? false)))
                                    {
                                        //List<string> ListStatusForSearchSignaruteAndImage = new List<string>() { "F", "G", "H", "I", "U", "V", "Z"};
                                        List<string> ListStatusForSearchSignaruteAndImage = new List<string>() { "W", "P", "S", "A", "T", "F", "G", "H", "I", "U", "V", "Z" };
                                        if(ListStatusForSearchSignaruteAndImage.Contains(item.StatusChar) == true)
                                        {
                                            var isForwarding = false;
                                            var resEMS = EMSRepository.EMSSignaruteAndImageInformation(new ZEMSSignaruteAndImageInformationRequest() { LadingCode = item.ShippingCode });
                                            var resVNPPickUp = VNPRepository.VNPPickUpImageInformation(new ZVNPSignaruteAndImageInformationRequest() { itemCode = item.ShippingCode });
                                            var pickupStatus = JsonConvert.DeserializeObject<List<ZVNPPickupInformationResponse>>(resVNPPickUp.Data);
                                            if(resEMS.Code == "00")
                                            {
                                                item.ListEMSSignaruteAndImageInfo = JsonConvert.DeserializeObject<List<ZEMSSignaruteAndImageInformationResponse>>(resEMS.Data);
                                                foreach(var i in item.ListEMSSignaruteAndImageInfo)
                                                {
                                                    if(i.StatusCode != "C14")
                                                    {
                                                        isForwarding = true;
                                                    }
                                                }
                                            }

                                            if(item.ListEMSSignaruteAndImageInfo.Count == 0 || isForwarding)
                                            {
                                                var resVNP = VNPRepository.VNPSignaruteAndImageInformation(new ZVNPSignaruteAndImageInformationRequest() { itemCode = item.ShippingCode });

                                                if(resVNP.Code == "00")
                                                {
                                                    item.ListVNPSignaruteAndImageInfo = JsonConvert.DeserializeObject<List<ZVNPSignaruteAndImageInformationResponse>>(resVNP.Data);
                                                }
                                            }

                                            var resEMSEdata = EMSRepository.EMSEdataSignaruteAndImageInformation(
                                                                new ZEMSSignaruteAndImageInformationRequest { LadingCode = item.ShippingCode });


                                            if(resEMSEdata.Code == "00")
                                            {
                                                var edataImg = JsonConvert.DeserializeObject<List<ZVNPSignaruteAndImageInformationResponse>>(resEMSEdata.Data)
                                                                ?.Where(e => e.ImagesUpload.Count > 0)
                                                                .ToList();

                                                if(edataImg != null && edataImg.Count > 0)
                                                {
                                                    var imagesUpload = edataImg.SelectMany(e => e.ImagesUpload).ToList();

                                                    if(item.ListEMSSignaruteAndImageInfo?.Count > 0)
                                                    {
                                                        foreach(var itemEMSImg in item.ListEMSSignaruteAndImageInfo)
                                                        {
                                                            if(itemEMSImg.ImagePlus == null || itemEMSImg.ImagePlus.Count == 0) itemEMSImg.ImagePlus = new List<string>();
                                                            if(itemEMSImg.StatusCode == "C14")
                                                            {
                                                                itemEMSImg.ImagePlus.AddRange(imagesUpload);
                                                            }
                                                        }
                                                    }

                                                    if(item.ListVNPSignaruteAndImageInfo?.Count > 0)
                                                    {
                                                        foreach(var itemVNPImg in item.ListVNPSignaruteAndImageInfo)
                                                        {
                                                            if(itemVNPImg.ImagesPlus == null || itemVNPImg.ImagesPlus.Count == 0) itemVNPImg.ImagesPlus = new List<string>();
                                                            if(itemVNPImg.StatusCode == "C14")
                                                            {
                                                                itemVNPImg.ImagesPlus.AddRange(imagesUpload);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }

                                    res.Code = "00";
                                    res.Message = "Lấy dữ liệu thành công";
                                    res.Data = JsonConvert.SerializeObject(item);
                                }
                                else
                                {
                                    res.Code = "02";
                                    res.Message = "Bưu gửi này không thuộc về bạn! Xin vui lòng đăng nhập vào tài khoản sở hữu bưu gửi này để xem thông tin bưu gửi.";
                                }

                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
                            }
                        }
                        else
                        {
                            switch(result)
                            {
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.DetailByShippingCodeImg: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #endregion

        //#region Tạo đơn hàng V2
        //[Route("OrderAddV2")]
        //public ZResponse OrderAddV2(ZPartnerOrderAddV2Request req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var resCheckOrder = PartnerRepository.OrderAddV2(req);
        //        if (resCheckOrder.Code == "00" || resCheckOrder.Code == "02")
        //        {
        //            //ZResponse resOrderAdd = new ZResponse();
        //            var itemCheckOrder = JsonConvert.DeserializeObject<ZOrderAddRequest>(resCheckOrder.Data);
        //            var resOrderAdd = OrderRepository.AddOrderV2(itemCheckOrder);
        //            if (resOrderAdd.Code == "00")
        //            {
        //                ZPartnerOrderAddV2Response item = null;
        //                var resOrderInfo = OrderRepository.GetByIDV2(new ZOrderGetByIDRequest() { ID = Convert.ToInt32(resOrderAdd.Data) });
        //                if (resOrderInfo.Code == "00")
        //                {
        //                    var itemOrderInfo = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderInfo.Data);
        //                    item = new ZPartnerOrderAddV2Response()
        //                    {
        //                        OrderID = itemOrderInfo.ID,
        //                        EMSOneCode = itemOrderInfo.EMSOneCode,
        //                        OrderCode = itemOrderInfo.OrderCode,
        //                        ShippingCode = itemOrderInfo.ShippingCode,
        //                        TransportFee = itemOrderInfo.TransportFee
        //                    };

        //                    if (item != null)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = resOrderAdd.Message;
        //                        res.Data = JsonConvert.SerializeObject(item);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "03";
        //                        res.Message = "Lấy thông tin đơn hàng thất bại.";
        //                    }
        //                }
        //                else
        //                {
        //                    res.Code = "03";
        //                    res.Message = "Lấy thông tin đơn hàng thất bại.";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = resOrderAdd.Code;
        //                res.Message = resOrderAdd.Message;
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resCheckOrder.Code;
        //            res.Message = resCheckOrder.Message;
        //            res.Data = resCheckOrder.Data;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.OrderAddV2: " + ex.Message);
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //#endregion

        #region Tạo đơn hàng dùng riêng cho đối tác VIB Bank
        [Route("VIBOrderAdd")]
        public ZResponse VIBOrderAdd(ZPartnerVIBOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resCheckOrder = PartnerRepository.VIBOrderAdd(req);
                if(resCheckOrder.Code == "00" || resCheckOrder.Code == "02")
                {
                    var itemCheckOrder = JsonConvert.DeserializeObject<ZOrderAddRequest>(resCheckOrder.Data);
                    var resOrderAdd = new ZResponse();
                    if(itemCheckOrder.isSendTransport == "N")
                    {
                        resOrderAdd = OrderRepository.AddOrderV2(itemCheckOrder);
                    }
                    else
                    {
                        resOrderAdd = OrderRepository.AddOrderAndSendTransportV2(itemCheckOrder);
                    }
                    if(resOrderAdd.Code == "00")
                    {
                        ZPartnerVIBOrderAddResponse item = null;
                        var resOrderInfo = OrderRepository.GetByIDV2(new ZOrderGetByIDRequest() { ID = Convert.ToInt32(resOrderAdd.Data) });
                        if(resOrderInfo.Code == "00")
                        {
                            var itemOrderInfo = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderInfo.Data);
                            item = new ZPartnerVIBOrderAddResponse()
                            {
                                OrderID = itemOrderInfo.ID,
                                EMSOneCode = itemOrderInfo.EMSOneCode,
                                OrderCode = itemOrderInfo.OrderCode,
                                ShippingCode = itemOrderInfo.ShippingCode,
                                TransportFee = itemOrderInfo.TransportFee
                            };

                            if(item != null)
                            {
                                res.Code = resCheckOrder.Code;
                                res.Message = resCheckOrder.Message;
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "03";
                                res.Message = "Lấy thông tin đơn hàng thất bại.";
                            }
                        }
                        else
                        {
                            res.Code = "03";
                            res.Message = "Lấy thông tin đơn hàng thất bại.";
                        }
                    }
                    else
                    {
                        switch(resOrderAdd.Code)
                        {
                            case "99":
                                resOrderAdd.Code = "99";
                                break;
                            default:
                                res.Code = (Convert.ToInt32(resOrderAdd.Code) + 3).ToString().PadLeft(2, '0');
                                break;
                        }
                        res.Message = resOrderAdd.Message;
                    }
                }
                else
                {
                    res.Code = resCheckOrder.Code;
                    res.Message = resCheckOrder.Message;
                    res.Data = resCheckOrder.Data;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.VIBOrderAdd: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion
        [Route("AnalysticOrderAdd")]
        public ZResponse AnalysticOrderAdd(ZPartnerAnalysticOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resCheckOrder = PartnerRepository.AnalysticOrderAdd(req);
                if(resCheckOrder.Code == "00" || resCheckOrder.Code == "02")
                {
                    var itemCheckOrder = JsonConvert.DeserializeObject<ZOrderAddRequest>(resCheckOrder.Data);
                    var resOrderAdd = new ZResponse();
                    if(itemCheckOrder.isSendTransport == "N")
                    {
                        resOrderAdd = OrderRepository.AddOrderV2(itemCheckOrder);
                    }
                    else
                    {
                        resOrderAdd = OrderRepository.AddOrderAndSendTransportV2(itemCheckOrder);
                    }
                    if(resOrderAdd.Code == "00")
                    {
                        ZPartnerVIBOrderAddResponse item = null;
                        var resOrderInfo = OrderRepository.GetByIDV2(new ZOrderGetByIDRequest() { ID = Convert.ToInt32(resOrderAdd.Data) });
                        if(resOrderInfo.Code == "00")
                        {
                            var itemOrderInfo = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderInfo.Data);
                            item = new ZPartnerVIBOrderAddResponse()
                            {
                                OrderID = itemOrderInfo.ID,
                                EMSOneCode = itemOrderInfo.EMSOneCode,
                                OrderCode = itemOrderInfo.OrderCode,
                                ShippingCode = itemOrderInfo.ShippingCode,
                                TransportFee = itemOrderInfo.TransportFee
                            };

                            if(item != null)
                            {
                                res.Code = resCheckOrder.Code;
                                res.Message = resCheckOrder.Message;
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "03";
                                res.Message = "Lấy thông tin đơn hàng thất bại.";
                            }
                        }
                        else
                        {
                            res.Code = "03";
                            res.Message = "Lấy thông tin đơn hàng thất bại.";
                        }
                    }
                    else
                    {
                        switch(resOrderAdd.Code)
                        {
                            case "99":
                                resOrderAdd.Code = "99";
                                break;
                            default:
                                res.Code = (Convert.ToInt32(resOrderAdd.Code) + 3).ToString().PadLeft(2, '0');
                                break;
                        }
                        res.Message = resOrderAdd.Message;
                    }
                }
                else
                {
                    res.Code = resCheckOrder.Code;
                    res.Message = resCheckOrder.Message;
                    res.Data = resCheckOrder.Data;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.VIBOrderAdd: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #region Tạo đơn hàng dùng riêng cho đối tác H&M
        [Route("HMOrderAdd")]
        public ZResponse HMOrderAdd(ZPartnerAnalysticOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                ZResponse resOrderAdd = new ZResponse();

                var resCheckOrder = PartnerRepository.HMOrderAdd(req);

                if(resCheckOrder.Code == "00" || resCheckOrder.Code == "02")
                {


                    var itemCheckOrder = JsonConvert.DeserializeObject<ZOrderAddRequest>(resCheckOrder.Data);

                    if(itemCheckOrder.isSendTransport == "N" || itemCheckOrder.Status == "D")
                    {
                        resOrderAdd = OrderRepository.AddOrderV2(itemCheckOrder);
                    }
                    else if(itemCheckOrder.isSendTransport == "Y" )
                    {
                        resOrderAdd = OrderRepository.AddOrderAndSendTransportV2(itemCheckOrder);
                    }

                    if(resOrderAdd.Code == "00" || resOrderAdd.Code == "06")
                    {
                        ZPartnerHMOrderAddResponse item = null;
                        var resOrderInfo = OrderRepository.GetByOrderCode(new ZOrderGetByOrderCodeRequest() { OrderCode = itemCheckOrder.OrderCode, ShopID = itemCheckOrder.ShopID });

                        if(resOrderInfo.Code == "00")
                        {

                            var itemOrderInfo = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderInfo.Data);

                            var labelInforRes = EMSRepository.CreateLabelHM(new ZPartnerHMOrderCreateLabelRequest()
                            {
                                Poscode = "",
                                Sendingtime = DateTime.Now.ToString("dd/MM/yyyy hh:MM:ss"),
                                Trackingcode = itemOrderInfo.ShippingCode,
                                Sendername = "Cong Ty TNHH H&M Hennes & Mauritz Viet Nam",
                                ReceiverName = req.ReceiverInfo.FullName + " - " + req.ReceiverInfo.Street,
                                TotalAmount = Convert.ToInt32(req.TransportInfo.TotalCOD),
                                ReceiverPhone = req.ReceiverInfo.MobileNumber,
                                TotalFee = itemOrderInfo.TransportFee,
                                ProductDescription = req.Message,
                                Weight = Convert.ToInt32(req.TransportInfo.TransportWeight),
                                Service = req.TransportInfo.TransportExtraServiceID.Contains("108") ? "ZNS,COD" : "ZNS",
                                Note = req.Note,
                                Ordercode = req.OrderCode

                            });

                            var labelHM = "";
                            try
                            {
                                if(labelInforRes.Code == "00")
                                {
                                    var labelInfor = JsonConvert.DeserializeObject<ZPartnerHMOrderCreateLabelResponse>(labelInforRes.Data);
                                    labelHM = labelInfor.oOrderHMZcode;
                                }
                            }
                            catch(Exception ex)
                            {
                                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.HMOrderAdd.CreateLabel: " + ex.Message);
                            }


                            item = new ZPartnerHMOrderAddResponse()
                            {
                                OrderID = itemOrderInfo.ID,
                                EMSOneCode = itemOrderInfo.EMSOneCode,
                                OrderCode = itemOrderInfo.OrderCode,
                                ShippingCode = itemOrderInfo.ShippingCode,
                                TransportFee = itemOrderInfo.TransportFee,
                                Label = labelHM,
                                TransportFeeDetail = new ZTransportPostageChargedResponse
                                {
                                    MainFee = req.TransportInfo.MainFee,
                                    ExtraFee = req.TransportInfo.ExtraFee,
                                    Surcharge = req.TransportInfo.Surcharge,
                                    TotalAmount = req.TransportInfo.TotalAmount,
                                    MainFeeNotVAT = req.TransportInfo.MainFeeNotVAT,
                                    ExtraFeeNotVAT = req.TransportInfo.ExtraFeeNotVAT,
                                    SurchargeNotVAT = req.TransportInfo.SurchargeNotVAT,
                                    TotalAmountNotVAT = req.TransportInfo.TotalAmountNotVAT,
                                    TransportPostageExtraChargedDetail = req.TransportInfo.TransportPostageExtraChargedDetail
                                }
                            };

                            if(item != null)
                            {
                                if(resOrderAdd.Code != "00")
                                {
                                    res.Code = (resOrderAdd.Code == "06") ? "00" : resOrderAdd.Code;
                                    res.Message = (resOrderAdd.Code == "06") ? "OrderCode already exists." : resOrderAdd.Message;
                                    res.Data = JsonConvert.SerializeObject(item);
                                }
                                else
                                {
                                    res.Code = resCheckOrder.Code;
                                    res.Message = resCheckOrder.Message;
                                    res.Data = JsonConvert.SerializeObject(item);
                                }

                            }
                            else
                            {
                                res.Code = "03";
                                res.Message = "Failed to retrieve order information.";
                            }
                        }
                        else
                        {
                            res.Code = "03";
                            res.Message = "Failed to retrieve order information.";
                        }
                    }
                    else
                    {
                        switch(resOrderAdd.Code)
                        {
                            case "99":
                                resOrderAdd.Code = "99";
                                break;
                            default:
                                res.Code = (Convert.ToInt32(resOrderAdd.Code) + 3).ToString().PadLeft(2, '0');
                                break;
                        }
                        res.Message = resOrderAdd.Message;
                    }

                }
                else
                {
                    res.Code = resCheckOrder.Code;
                    res.Message = resCheckOrder.Message;
                    res.Data = resCheckOrder.Data;
                }
                
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "System error. Please contact the administrator.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.HMOrderAdd: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Tạo đơn hàng dùng riêng cho đối tác Temu
        [Route("TemuOrderAdd")]
        public ZResponse TemuOrderAdd(ZPartnerTemuOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resCheckOrder = PartnerRepository.TemuOrderAdd(req);
                if(resCheckOrder.Code == "00" || resCheckOrder.Code == "02")
                {
                    var itemCheckOrder = JsonConvert.DeserializeObject<ZOrderAddRequest>(resCheckOrder.Data);
                    var resOrderAdd = OrderRepository.AddOrderV2(itemCheckOrder);
                    if(resOrderAdd.Code == "00")
                    {
                        ZPartnerTemuOrderAddResponse item = null;
                        var resOrderInfo = OrderRepository.GetByIDV2(new ZOrderGetByIDRequest() { ID = Convert.ToInt32(resOrderAdd.Data) });
                        if(resOrderInfo.Code == "00")
                        {
                            var itemOrderInfo = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderInfo.Data);
                            item = new ZPartnerTemuOrderAddResponse()
                            {
                                OrderID = itemOrderInfo.ID,
                                EMSOneCode = itemOrderInfo.EMSOneCode,
                                OrderCode = itemOrderInfo.OrderCode,
                                ShippingCode = itemOrderInfo.ShippingCode,
                                TransportFee = itemOrderInfo.TransportFee
                            };

                            if(item != null)
                            {
                                res.Code = resCheckOrder.Code;
                                res.Message = resCheckOrder.Message;
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "03";
                                res.Message = "Lấy thông tin đơn hàng thất bại.";
                            }
                        }
                        else
                        {
                            res.Code = "03";
                            res.Message = "Lấy thông tin đơn hàng thất bại.";
                        }
                    }
                    else
                    {
                        switch(resOrderAdd.Code)
                        {
                            case "99":
                                resOrderAdd.Code = "99";
                                break;
                            default:
                                res.Code = (Convert.ToInt32(resOrderAdd.Code) + 3).ToString().PadLeft(2, '0');
                                break;
                        }
                        res.Message = resOrderAdd.Message;
                    }
                }
                else
                {
                    res.Code = resCheckOrder.Code;
                    res.Message = resCheckOrder.Message;
                    res.Data = resCheckOrder.Data;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.VIBOrderAdd: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Hủy đơn hàng
        [Route("OrderCancel")]
        public ZResponse OrderCancel(ZPartnerOrderCancelRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                req.CrmOrPaypostCode = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "" : req.CrmOrPaypostCode.Trim();
                req.CustomerToken = string.IsNullOrEmpty(req.CustomerToken) ? "" : req.CustomerToken.Trim();
                if(string.IsNullOrEmpty(req.CrmOrPaypostCode) && string.IsNullOrEmpty(req.CustomerToken))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập CrmOrPaypostCode hoặc CustomerToken.";
                    return res;
                }

                req.OrderCode = string.IsNullOrEmpty(req.OrderCode) ? "" : req.OrderCode.ToUpper().Trim();
                req.ShippingCode = string.IsNullOrEmpty(req.ShippingCode) ? "" : req.ShippingCode.Trim();
                if(string.IsNullOrEmpty(req.OrderCode) && string.IsNullOrEmpty(req.ShippingCode))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập OrderCode hoặc ShippingCode.";
                    return res;
                }

                req.ReasonCancel = string.IsNullOrEmpty(req.ReasonCancel) ? "" : req.ReasonCancel.Trim();
                if(string.IsNullOrEmpty(req.ReasonCancel))
                {
                    res.Code = "01";
                    res.Message = "Lý do không được bỏ trống.";
                    return res;
                }

                var resOrderInfo = PartnerRepository.OrderInfo(new ZPartnerOrderInfoRequest()
                {
                    CrmOrPaypostCode = req.CrmOrPaypostCode,
                    CustomerToken = req.CustomerToken,
                    OrderCode = req.OrderCode,
                    ShippingCode = req.ShippingCode
                });

                if(resOrderInfo.Code == "00")
                {
                    var ItemOrder = JsonConvert.DeserializeObject<ZPartnerOrderInfoResponse>(resOrderInfo.Data);

                    var resCancelOrderInfo = OrderRepository.CancelOrderInfo(new ZOrderCancelOrderInfoRequest()
                    {
                        OrderID = ItemOrder.OrderID,
                        ShopID = ItemOrder.ShopInfo.ID
                    });

                    if(resCancelOrderInfo.Code == "00")
                    {
                        var ItemOrderTransportInfo = JsonConvert.DeserializeObject<ZOrderCancelOrderInfoResponse>(resCancelOrderInfo.Data);

                        if(ItemOrderTransportInfo.OrderStatus == "W" || ItemOrderTransportInfo.OrderStatus == "P")
                        {
                            var resCancelTransport = OrderRepository.CancelTransport(new ZOrderCancelTransportRequest()
                            {
                                OrderID = ItemOrder.OrderID,
                                ShopID = ItemOrder.ShopInfo.ID,
                                CrmOrPaypostCode = ItemOrderTransportInfo.CrmOrPaypostCode,
                                ShippingCode = ItemOrderTransportInfo.ShippingCode,
                                UpdatedBy = ItemOrder.EmployeeInfo.ID
                            });

                            if(resCancelTransport.Code == "00")
                            {
                                var resCancelOrderStatus = OrderRepository.CancelOrderStatus(new ZOrderCancelOrderStatusRequest()
                                {
                                    OrderID = ItemOrder.OrderID,
                                    ReasonCancel = "PARTNER " + req.ReasonCancel,
                                    ShopID = ItemOrder.ShopInfo.ID,
                                    UpdatedBy = ItemOrder.EmployeeInfo.ID
                                });

                                if(resCancelOrderStatus.Code == "00")
                                {
                                    res.Code = "00";
                                    res.Message = "Hủy đơn hàng thành công.";
                                }
                                else
                                {
                                    res.Code = resCancelOrderStatus.Code;
                                    res.Message = resCancelOrderStatus.Message;
                                }
                            }
                            else
                            {
                                res.Code = resCancelTransport.Code;
                                res.Message = resCancelTransport.Message;
                            }
                        }
                        else if(ItemOrderTransportInfo.OrderStatus == "D" || ItemOrderTransportInfo.OrderStatus == "N")
                        {
                            var resCancelOrderStatus = OrderRepository.CancelOrderStatus(new ZOrderCancelOrderStatusRequest()
                            {
                                OrderID = ItemOrder.OrderID,
                                ReasonCancel = "PARTNER " + req.ReasonCancel,
                                ShopID = ItemOrder.ShopInfo.ID,
                                UpdatedBy = ItemOrder.EmployeeInfo.ID
                            });

                            if(resCancelOrderStatus.Code == "00")
                            {
                                res.Code = "00";
                                res.Message = "Hủy đơn hàng thành công.";
                            }
                            else
                            {
                                res.Code = resCancelOrderStatus.Code;
                                res.Message = resCancelOrderStatus.Message;
                            }
                        }
                        else
                        {
                            res.Code = "02";
                            res.Message = "Trạng thái không hợp lệ.";
                        }
                    }
                    else
                    {
                        res.Code = resCancelOrderInfo.Code;
                        res.Message = resCancelOrderInfo.Message;
                    }
                }
                else
                {
                    res.Code = resOrderInfo.Code;
                    res.Message = resOrderInfo.Message;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.OrderCancel: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Lấy thông tin đơn hàng
        [Route("OrderInfo")]
        public ZResponse OrderInfo(ZPartnerOrderInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                req.CrmOrPaypostCode = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "" : req.CrmOrPaypostCode.Trim();
                req.CustomerToken = string.IsNullOrEmpty(req.CustomerToken) ? "" : req.CustomerToken.Trim();

                if(string.IsNullOrEmpty(req.CrmOrPaypostCode) && string.IsNullOrEmpty(req.CustomerToken))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập CrmOrPaypostCode hoặc CustomerToken.";
                    return res;
                }

                req.OrderCode = string.IsNullOrEmpty(req.OrderCode) ? "" : req.OrderCode.ToUpper().Trim();
                req.ShippingCode = string.IsNullOrEmpty(req.ShippingCode) ? "" : req.ShippingCode.Trim();
                if(string.IsNullOrEmpty(req.OrderCode) && string.IsNullOrEmpty(req.ShippingCode))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập OrderCode hoặc ShippingCode.";
                    return res;
                }

                res = PartnerRepository.OrderInfo(new ZPartnerOrderInfoRequest()
                {
                    CrmOrPaypostCode = req.CrmOrPaypostCode,
                    CustomerToken = req.CustomerToken,
                    OrderCode = req.OrderCode,
                    ShippingCode = req.ShippingCode
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.OrderInfo: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region In đơn hàng
        [Route("OrderPrint")]
        public ZResponse OrderPrint(ZPartnerOrderPrintRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                req.CrmOrPaypostCode = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "" : req.CrmOrPaypostCode.Trim();
                req.CustomerToken = string.IsNullOrEmpty(req.CustomerToken) ? "" : req.CustomerToken.Trim();

                if(string.IsNullOrEmpty(req.CrmOrPaypostCode) && string.IsNullOrEmpty(req.CustomerToken))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập CrmOrPaypostCode hoặc CustomerToken.";
                    return res;
                }

                req.OrderCode = string.IsNullOrEmpty(req.OrderCode) ? "" : req.OrderCode.ToUpper().Trim();
                req.ShippingCode = string.IsNullOrEmpty(req.ShippingCode) ? "" : req.ShippingCode.Trim();
                if(string.IsNullOrEmpty(req.OrderCode) && string.IsNullOrEmpty(req.ShippingCode))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập OrderCode hoặc ShippingCode.";
                    return res;
                }

                req.PrintType = string.IsNullOrEmpty(req.PrintType) ? "" : req.PrintType.Trim();
                if(string.IsNullOrEmpty(req.PrintType))
                {
                    res.Code = "01";
                    res.Message = "Kiểu in không được bỏ trống.";
                    return res;
                }
                else
                {
                    List<string> ListPrintType = new List<string>() { "A4", "A6", "B1", "B2", "Label" };

                    if(!ListPrintType.Contains(req.PrintType))
                    {
                        res.Code = "01";
                        res.Message = "Kiểu in không hợp lệ.";
                        return res;
                    }
                }

                ZPartnerOrderPrintResponse item = null;
                var reqOrderInfo = PartnerRepository.OrderInfo(new ZPartnerOrderInfoRequest()
                {
                    CrmOrPaypostCode = req.CrmOrPaypostCode,
                    CustomerToken = req.CustomerToken,
                    OrderCode = req.OrderCode,
                    ShippingCode = req.ShippingCode
                });
                if(reqOrderInfo.Code == "00")
                {
                    var resOrderInfo = JsonConvert.DeserializeObject<ZPartnerOrderInfoResponse>(reqOrderInfo.Data);
                    var reqOrderPrint = OrderRepository.HandlePrint(new ZOrderPrintRequest()
                    {
                        EMSOneCode = resOrderInfo.EMSOneCode,
                        ShopID = resOrderInfo.ShopInfo.ID,
                        PrintType = req.PrintType
                    });
                    if(reqOrderPrint.Code == "00")
                    {
                        var resOrderPrint = JsonConvert.DeserializeObject<ZOrderPrintResponse>(reqOrderPrint.Data);
                        item = new ZPartnerOrderPrintResponse()
                        {
                            EMSOneCode = resOrderPrint.EMSOneCode,
                            OrderCode = resOrderPrint.OrderCode,
                            BatchCode = resOrderPrint.BatchCode,
                            OrderBarCode = resOrderPrint.OrderBarCode,
                            ShippingCode = resOrderPrint.ShippingCode,
                            ShippingBarCode = resOrderPrint.ShippingBarCode,
                            QRCode = resOrderPrint.QRCode,
                            OrderQuantity = Convert.ToInt64(resOrderPrint.TransportQuantity),
                            Message = resOrderPrint.Message,
                            Note = resOrderPrint.Note,
                            TransportInfo = new ZPartnerOrderPrintTransportInfoResponse()
                            {
                                PickUpProvinceName = resOrderPrint.PickUpProvinceName,
                                TransportMainService = new ZBaseResponse()
                                {
                                    Code = resOrderPrint.TransportMainServiceCode,
                                    Name = resOrderPrint.TransportMainServiceName,
                                },
                                TransportExtraService = new ZBaseResponse()
                                {
                                    Code = resOrderPrint.TransportExtraServiceCode,
                                    Name = resOrderPrint.TransportExtraServiceName,
                                },
                                TransportWeight = Convert.ToDouble(resOrderPrint.TransportWeight),
                                PaymentFreightVat = resOrderPrint.PaymentFreightVat,
                                TransportPayer = new ZBaseResponse()
                                {
                                    ID = resOrderPrint.TransportPayer,
                                    Name = XData.GetNameTransportPayer(resOrderPrint.TransportPayer.ToString()),
                                },
                                TransportFee = resOrderPrint.TransportFee,
                                TotalCOD = resOrderPrint.TotalCOD,
                                TotalPHU = resOrderPrint.TotalPHU,
                                PostOffice = resOrderPrint.PostOffice,
                                //PostOffice = new ZBaseResponse()
                                //{
                                //    Code = resOrderPrint.PostOfficeCode,
                                //    Name = resOrderPrint.PostOfficeName,
                                //},
                            },
                            SenderInfo = new ZPartnerOrderPrintSenderInfoResponse()
                            {
                                FullName = resOrderPrint.SenderFullName,
                                MobileNumber = resOrderPrint.SenderMobileNumber,
                                ProvinceName = resOrderPrint.SenderProvinceName,
                                DistrictName = resOrderPrint.SenderDistrictName,
                                FullAddress = resOrderPrint.SenderFullAddress,
                                SentDate = resOrderPrint.SenderSentDate,
                            },
                            ReceiverInfo = new ZPartnerOrderPrintReceiverInfoResponse()
                            {
                                FullName = resOrderPrint.ReceiverFullName,
                                MobileNumber = resOrderPrint.ReceiverMobileNumber,
                                ProvinceName = resOrderPrint.ReceiverProvinceName,
                                DistrictName = resOrderPrint.ReceiverDistrictName,
                                FullAddress = resOrderPrint.ReceiverFullAddress,
                            },
                        };

                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = JsonConvert.SerializeObject(item);
                    }
                    else
                    {
                        res.Code = reqOrderPrint.Code;
                        res.Message = reqOrderPrint.Message;
                    }
                }
                else
                {
                    res.Code = reqOrderInfo.Code;
                    res.Message = reqOrderInfo.Message;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.OrderPrint: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("OrderPrint_V2")]
        public ZResponse OrderPrint_V2(ZPartnerOrderPrint_V2Request req)
        {
            ZResponse res = new ZResponse();
            try
            {
                req.CrmOrPaypostCode = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "" : req.CrmOrPaypostCode.Trim();
                //req.CustomerToken = string.IsNullOrEmpty(req.CustomerToken) ? "" : req.CustomerToken.Trim();

                if(string.IsNullOrEmpty(req.CrmOrPaypostCode) /*&& string.IsNullOrEmpty(req.CustomerToken)*/)
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập CrmOrPaypostCode hoặc CustomerToken.";
                    return res;
                }

                req.OrderCode = string.IsNullOrEmpty(req.OrderCode) ? "" : req.OrderCode.ToUpper().Trim();
                req.ShippingCode = string.IsNullOrEmpty(req.ShippingCode) ? "" : req.ShippingCode.Trim();
                if(string.IsNullOrEmpty(req.OrderCode) && string.IsNullOrEmpty(req.ShippingCode))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập OrderCode hoặc ShippingCode.";
                    return res;
                }

                //req.PrintType = string.IsNullOrEmpty(req.PrintType) ? "" : req.PrintType.Trim();
                //if(string.IsNullOrEmpty(req.PrintType))
                //{
                //    res.Code = "01";
                //    res.Message = "Kiểu in không được bỏ trống.";
                //    return res;
                //}
                //else
                //{
                //    List<string> ListPrintType = new List<string>() { "A4", "A6", "B1", "B2", "Label" };

                //    if(!ListPrintType.Contains(req.PrintType))
                //    {
                //        res.Code = "01";
                //        res.Message = "Kiểu in không hợp lệ.";
                //        return res;
                //    }
                //}

                ZPartnerOrderPrintResponse item = null;
                var reqOrderInfo = PartnerRepository.OrderInfo(new ZPartnerOrderInfoRequest()
                {
                    CrmOrPaypostCode = req.CrmOrPaypostCode,
                    CustomerToken = "",
                    OrderCode = req.OrderCode,
                    ShippingCode = req.ShippingCode
                });
                if(reqOrderInfo.Code == "00")
                {
                    var resOrderInfo = JsonConvert.DeserializeObject<ZPartnerOrderInfoResponse>(reqOrderInfo.Data);
                    var reqOrderPrint = OrderRepository.HandlePrint(new ZOrderPrintRequest()
                    {
                        EMSOneCode = resOrderInfo.EMSOneCode,
                        ShopID = resOrderInfo.ShopInfo.ID,
                        PrintType = "A4" // FIX  giá trị để chạy 
                    });
                    if(reqOrderPrint.Code == "00")
                    {
                        var resOrderPrint = JsonConvert.DeserializeObject<ZOrderPrintResponse>(reqOrderPrint.Data);
                        item = new ZPartnerOrderPrintResponse()
                        {
                            EMSOneCode = resOrderPrint.EMSOneCode,
                            OrderCode = resOrderPrint.OrderCode,
                            BatchCode = resOrderPrint.BatchCode,
                            OrderBarCode = resOrderPrint.OrderBarCode,
                            ShippingCode = resOrderPrint.ShippingCode,
                            ShippingBarCode = resOrderPrint.ShippingBarCode,
                            QRCode = resOrderPrint.QRCode,
                            OrderQuantity = Convert.ToInt64(resOrderPrint.TransportQuantity),
                            Message = resOrderPrint.Message,
                            Note = resOrderPrint.Note,
                            TransportInfo = new ZPartnerOrderPrintTransportInfoResponse()
                            {
                                PickUpProvinceName = resOrderPrint.PickUpProvinceName,
                                TransportMainService = new ZBaseResponse()
                                {
                                    Code = resOrderPrint.TransportMainServiceCode,
                                    Name = resOrderPrint.TransportMainServiceName,
                                },
                                TransportExtraService = new ZBaseResponse()
                                {
                                    Code = resOrderPrint.TransportExtraServiceCode,
                                    Name = resOrderPrint.TransportExtraServiceName,
                                },
                                TransportWeight = Convert.ToDouble(resOrderPrint.TransportWeight),
                                PaymentFreightVat = resOrderPrint.PaymentFreightVat,
                                TransportPayer = new ZBaseResponse()
                                {
                                    ID = resOrderPrint.TransportPayer,
                                    Name = XData.GetNameTransportPayer(resOrderPrint.TransportPayer.ToString()),
                                },
                                TransportFee = resOrderPrint.TransportFee,
                                TotalCOD = resOrderPrint.TotalCOD,
                                TotalPHU = resOrderPrint.TotalPHU,
                                PostOffice = resOrderPrint.PostOffice,
                                //PostOffice = new ZBaseResponse()
                                //{
                                //    Code = resOrderPrint.PostOfficeCode,
                                //    Name = resOrderPrint.PostOfficeName,
                                //},
                            },
                            SenderInfo = new ZPartnerOrderPrintSenderInfoResponse()
                            {
                                FullName = resOrderPrint.SenderFullName,
                                MobileNumber = resOrderPrint.SenderMobileNumber,
                                ProvinceName = resOrderPrint.SenderProvinceName,
                                DistrictName = resOrderPrint.SenderDistrictName,
                                FullAddress = resOrderPrint.SenderFullAddress,
                                SentDate = resOrderPrint.SenderSentDate,
                            },
                            ReceiverInfo = new ZPartnerOrderPrintReceiverInfoResponse()
                            {
                                FullName = resOrderPrint.ReceiverFullName,
                                MobileNumber = resOrderPrint.ReceiverMobileNumber,
                                ProvinceName = resOrderPrint.ReceiverProvinceName,
                                DistrictName = resOrderPrint.ReceiverDistrictName,
                                FullAddress = resOrderPrint.ReceiverFullAddress,
                            },
                        };

                        var labelRequest = new ZPartnerOrderPrint_V2Request
                        {
                            CrmOrPaypostCode = req.CrmOrPaypostCode,
                            OrderCode = req.OrderCode,
                            ShippingCode = req.ShippingCode,
                            PosCode = item.TransportInfo.PostOffice,
                            SendingTime = item.SenderInfo.SentDate,
                            SenderName = item.SenderInfo.FullName + " - " + item.SenderInfo.MobileNumber + " - " + item.SenderInfo.FullAddress,
                            ReceiverName = item.ReceiverInfo.FullName + " - " + item.ReceiverInfo.FullAddress,
                            ReceiverPhone = item.ReceiverInfo.MobileNumber,
                            ProductDescription = item.Message,
                            Weight = item.TransportInfo.TransportWeight.ToString(),
                            CodAmount = item.TransportInfo.TotalCOD.ToString(),
                            DeliveryFee = item.TransportInfo.PaymentFreightVat.ToString(),
                            Note = item.Note,
                            PrintTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
                        };

                        ZResponse resLabel = new ZResponse();
                        var test = JsonConvert.SerializeObject(labelRequest);
                        resLabel = XHelper.InvokeLabel("POST", "/api/Label/generate", labelRequest);
                        var ResponseDataLabel = JsonConvert.DeserializeObject<ZPartnerOrderPrint_V2Response>(resLabel.Data);
                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = JsonConvert.SerializeObject(ResponseDataLabel);
                    }
                    else
                    {
                        res.Code = reqOrderPrint.Code;
                        res.Message = reqOrderPrint.Message;
                    }
                }
                else
                {
                    res.Code = reqOrderInfo.Code;
                    res.Message = reqOrderInfo.Message;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.OrderPrint: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Tạo mã bưu gửi
        [Route("CreateShippingCode")]
        public ZResponse CreateShippingCode(ZPartnerOrderCreateShippingCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                req.CrmOrPaypostCode = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "" : req.CrmOrPaypostCode.Trim();
                if(string.IsNullOrEmpty(req.CrmOrPaypostCode))
                {
                    res.Code = "01";
                    res.Message = "Mã CrmOrPaypostCode không được bỏ trống.";
                    return res;
                }

                var resShippingCode = TransportRepository.GetEmsBarCode(new ZTransportGetEmsBarCodeRequest()
                {
                    CustomerCode = req.CrmOrPaypostCode,
                    ProvinceID = 0,
                    OrderCode = "",
                    CreatedBy = "PARTNER",
                    AmndState = "A",
                    IsConsignment = 0,
                    ShippingID = "",
                    IsCommit = "Y",
                });
                if(resShippingCode.Code == "00")
                {
                    res.Code = resShippingCode.Code;
                    res.Message = resShippingCode.Message;
                    res.Data = JsonConvert.SerializeObject(new ZPartnerOrderCreateShippingCodeResponse() { ShippingCode = resShippingCode.Data });
                }
                else
                {
                    res.Code = resShippingCode.Code;
                    res.Message = resShippingCode.Message;
                    res.Data = resShippingCode.Data;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.CreateShippingCode: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Tạo đơn hàng lô
        [Route("OrderBatchAdd")]
        public ZResponse OrderBatchAdd(ZPartnerOrderBatchAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resCheckOrder = PartnerRepository.OrderBatchAdd(req);
                if(resCheckOrder.Code == "00")
                {
                    ZResponse resOrderAdd = new ZResponse();
                    var itemCheckOrder = JsonConvert.DeserializeObject<ZOrderBatchAddRequest>(resCheckOrder.Data);
                    var resAddOrderBatch = OrderBatchRepository.Add(itemCheckOrder);
                    if(resAddOrderBatch.Code == "00")
                    {
                        switch(req.ActionType)
                        {
                            case "D":
                            case "N":
                                res.Code = resAddOrderBatch.Code;
                                res.Message = resAddOrderBatch.Message;
                                res.Data = resAddOrderBatch.Data;
                                break;
                            case "W":
                                res = OrderBatchRepository.SendTransport(new ZOrderBatchSendTransportRequest() { AmndState = "I", BatchCode = resAddOrderBatch.Data, ShopID = itemCheckOrder.ShopID, CreatedBy = itemCheckOrder.CreatedBy });
                                break;
                            default:
                                res.Code = "01";
                                res.Message = "Thao tác tạo đơn hàng lô không hợp lệ.";
                                break;
                        };
                    }
                    else
                    {
                        res.Code = resAddOrderBatch.Code;
                        res.Message = resAddOrderBatch.Message;
                        res.Data = resAddOrderBatch.Data;
                    }
                }
                else
                {
                    res.Code = resCheckOrder.Code;
                    res.Message = resCheckOrder.Message;
                    res.Data = resCheckOrder.Data;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.OrderBatchAdd: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #endregion

        #region CÁC API LIÊN QUAN ĐẾN DỊCH VỤ

        #region Lấy danh sách dịch vụ vận chuyển chính + cộng thêm
        [Route("TransportServiceSearchList")]
        public ZResponse TransportServiceSearchList(ZPartnerTransportServiceSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                req.CrmOrPaypostCode = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "" : req.CrmOrPaypostCode.Trim();
                if(string.IsNullOrEmpty(req.CrmOrPaypostCode))
                {
                    res.Code = "01";
                    res.Message = "Mã CrmOrPaypostCode không được bỏ trống.";
                    return res;
                }

                List<int> ListParentID = new List<int>() { 0, 1, 2 };
                if(!ListParentID.Contains(req.ParentID))
                {
                    res.Code = "01";
                    res.Message = "ParentID không hợp lệ";
                    return res;
                }

                List<ZPartnerTransportServiceSearchListResponse> ListTransportService = new List<ZPartnerTransportServiceSearchListResponse>();
                var resShopInfo = ShopRepository.GetByCrmOrPaypostCode(req.CrmOrPaypostCode);
                if(resShopInfo.Code == "00")
                {
                    var itemShopInfo = JsonConvert.DeserializeObject<ZShopGetByCrmOrPaypostCodeResponse>(resShopInfo.Data);
                    var resSearchListByShopID = TransportServiceRepository.SearchListByShopID(new ZTransportServiceSearchListByShopIDRequest()
                    {
                        ShopID = itemShopInfo.ShopID,
                        ParentID = req.ParentID,
                        IsLock = "N"
                    });
                    if(resSearchListByShopID.Code == "00")
                    {
                        var ListTransportServiceSearchListByShopID = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(resSearchListByShopID.Data);
                        foreach(var item in ListTransportServiceSearchListByShopID)
                        {
                            ListTransportService.Add(new ZPartnerTransportServiceSearchListResponse()
                            {
                                ServiceID = item.ID,
                                ServiceCode = item.Code,
                                ServiceName = item.Name,
                                ParentID = item.ParentID,
                                IsDefault = item.IsDefault,
                            });
                        }
                    }
                    else
                    {
                        res.Code = resSearchListByShopID.Code;
                        res.Message = resSearchListByShopID.Message;
                    }
                }
                else
                {
                    var resTransportServiceSearchList = TransportServiceRepository.SearchList(new ZTransportServiceSearchListRequest()
                    {
                        ParentID = req.ParentID,
                        IsLock = "N"
                    });
                    if(resTransportServiceSearchList.Code == "00")
                    {
                        var ListTransportServiceSearchList = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListResponse>>(resTransportServiceSearchList.Data);
                        foreach(var item in ListTransportServiceSearchList)
                        {
                            ListTransportService.Add(new ZPartnerTransportServiceSearchListResponse()
                            {
                                ServiceID = item.ID,
                                ServiceCode = item.Code,
                                ServiceName = item.Name,
                                ParentID = item.ParentID,
                                IsDefault = "N",
                            });
                        }
                    }
                    else
                    {
                        res.Code = resTransportServiceSearchList.Code;
                        res.Message = resTransportServiceSearchList.Message;
                    }
                }

                if(ListTransportService.Count > 0)
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                    res.Data = JsonConvert.SerializeObject(ListTransportService);
                }
                else
                {
                    res.Code = "02";
                    res.Message = "Dữ liệu không tồn tại.";
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.TransportServiceSearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Tính phí vận chuyển
        [Route("TransportPostageCharged")]
        public ZResponse TransportPostageCharged(ZPartnerTransportPostageChargedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if(req.TransportMainServiceID == 0)
                {
                    res.Code = "01";
                    res.Message = "Dịch vụ chính không được bỏ trống.";
                    return res;
                }

                if(req.FromProvinceID == 0)
                {
                    res.Code = "01";
                    res.Message = "Chưa xác định nơi đi.";
                    return res;
                }

                if(req.ToProvinceID == 0)
                {
                    res.Code = "01";
                    res.Message = "Chưa xác định nơi đến.";
                    return res;
                }

                if(req.TransportWeight == 0)
                {
                    res.Code = "01";
                    res.Message = "Khối lượng không được bỏ trống.";
                    return res;
                }

                List<ZTransportServiceSearchListResponse> ListTransportService = new List<ZTransportServiceSearchListResponse>();
                if(!string.IsNullOrEmpty(req.CrmOrPaypostCode))
                {
                    var resShopInfo = ShopRepository.GetByCrmOrPaypostCode(req.CrmOrPaypostCode.Trim());
                    if(resShopInfo.Code == "00")
                    {
                        var itemShopInfo = JsonConvert.DeserializeObject<ZShopGetByCrmOrPaypostCodeResponse>(resShopInfo.Data);
                        var resTransportServiceSearchListByShopID = TransportServiceRepository.SearchListByShopID(new ZTransportServiceSearchListByShopIDRequest()
                        {
                            ShopID = itemShopInfo.ShopID,
                            ParentID = 0,
                            IsLock = "N"
                        });
                        if(resTransportServiceSearchListByShopID.Code == "00")
                        {
                            var ListTransportServiceSearchListByShopID = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(resTransportServiceSearchListByShopID.Data);
                            foreach(var item in ListTransportServiceSearchListByShopID)
                            {
                                ListTransportService.Add(new ZTransportServiceSearchListResponse()
                                {
                                    ID = item.ID,
                                    ParentID = item.ParentID
                                });
                            }
                        }
                        else
                        {
                            res.Code = resTransportServiceSearchListByShopID.Code;
                            res.Message = resTransportServiceSearchListByShopID.Message;
                        }
                    }
                    else
                    {
                        var resTransportServiceSearchList = TransportServiceRepository.SearchList(new ZTransportServiceSearchListRequest()
                        {
                            ParentID = 0,
                            IsLock = "N"
                        });
                        if(resTransportServiceSearchList.Code == "00")
                        {
                            ListTransportService = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListResponse>>(resTransportServiceSearchList.Data);
                        }
                        else
                        {
                            res.Code = resTransportServiceSearchList.Code;
                            res.Message = resTransportServiceSearchList.Message;
                        }
                    }
                }
                else
                {
                    var resTransportServiceSearchList = TransportServiceRepository.SearchList(new ZTransportServiceSearchListRequest()
                    {
                        ParentID = 0,
                        IsLock = "N"
                    });
                    if(resTransportServiceSearchList.Code == "00")
                    {
                        ListTransportService = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListResponse>>(resTransportServiceSearchList.Data);
                    }
                    else
                    {
                        res.Code = resTransportServiceSearchList.Code;
                        res.Message = resTransportServiceSearchList.Message;
                    }
                }

                if(ListTransportService.Count > 0)
                {
                    var isExistTransportMainService = ListTransportService.Where(m => m.ID == req.TransportMainServiceID && m.ParentID == 1).ToList();
                    if(isExistTransportMainService.Count == 0)
                    {
                        res.Code = "01";
                        res.Message = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "Dịch vụ vận chuyển chính không tồn tại." : "Cửa hàng không được phép sử dụng dịch vụ vận chuyển chính.";
                        return res;
                    }


                    if(!string.IsNullOrEmpty(req.TransportExtraServiceID))
                    {
                        var splitTransportExtraService = req.TransportExtraServiceID.Split(',');
                        foreach(var itemTransportExtraService in splitTransportExtraService)
                        {
                            var isExistTransportExtraService = ListTransportService.Where(m => m.ID == Convert.ToInt32(itemTransportExtraService) && m.ParentID == 2).ToList();
                            if(isExistTransportExtraService.Count == 0)
                            {
                                res.Code = "01";
                                res.Message = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "Dịch vụ vận chuyển cộng thêm không tồn tại." : "Cửa hàng không được phép sử dụng dịch vụ vận chuyển cộng thêm.";
                                return res;
                            }
                        }
                    }

                    var resCharged = TransportRepository.Charged(new ZTransportPostageChargedRequest()
                    {
                        MainServiceID = req.TransportMainServiceID,
                        ExtraServiceIDs = req.TransportExtraServiceID,
                        FromProvinceID = req.FromProvinceID,
                        FromDistrictID = req.FromDistrictID,
                        FromWardID = req.FromWardID,
                        ToProvinceID = req.ToProvinceID,
                        ToDistrictID = req.ToDistrictID,
                        ToWardID = req.ToWardID,
                        Weight = req.TransportWeight,
                        TransportSizeLength = req.TransportSizeLength,
                        TransportSizeWidth = req.TransportSizeWidth,
                        TransportSizeHeight = req.TransportSizeHeight,
                        Quantity = req.OrderQuantity,
                        Amount = req.OrderValue,
                        COD = req.TotalCOD,
                        CustomerCode = req.CrmOrPaypostCode,
                    });

                    if(resCharged.Code == "00")
                    {
                        var itemCharged = JsonConvert.DeserializeObject<ZTransportPostageChargedResponse>(resCharged.Data);

                        ZPartnerTransportPostageChargedResponse TransportFee = new ZPartnerTransportPostageChargedResponse()
                        {
                            TotalMainFee = itemCharged.MainFee,
                            TotalExtraFee = itemCharged.ExtraFee,
                            TotalSurcharge = itemCharged.Surcharge,
                            TotalAmount = itemCharged.TotalAmount,

                            //TotalMainFeeNotVAT = itemCharged.MainFeeNotVAT,
                            //TotalExtraFeeNotVAT = itemCharged.ExtraFeeNotVAT,
                            //TotalSurchargeNotVAT = itemCharged.SurchargeNotVAT,
                            //TotalAmountNotVAT = itemCharged.TotalAmountNotVAT,

                            TotalMainFeeNotVAT = Convert.ToInt64(itemCharged.MainFee / 1.08),
                            TotalExtraFeeNotVAT = Convert.ToInt64(itemCharged.ExtraFee / 1.08),
                            TotalSurchargeNotVAT = Convert.ToInt64(itemCharged.Surcharge / 1.08),
                        };

                        TransportFee.TotalAmountNotVAT = TransportFee.TotalMainFeeNotVAT + TransportFee.TotalExtraFeeNotVAT + TransportFee.TotalSurchargeNotVAT;

                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = JsonConvert.SerializeObject(TransportFee);
                    }
                    else
                    {
                        res.Code = resCharged.Code;
                        res.Message = resCharged.Message;
                    }
                }
                else
                {
                    res.Code = "02";
                    res.Message = "Dữ liệu không tồn tại.";
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.TransportServiceSearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #endregion

        #region CÁC API LIÊN QUAN ĐẾN KHO
        [Route("WareHouseSearchList")]
        public ZResponse WareHouseSearchList(ZPartnerWareHouseSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                req.CrmOrPaypostCode = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "" : req.CrmOrPaypostCode.Trim();
                if(string.IsNullOrEmpty(req.CrmOrPaypostCode))
                {
                    res.Code = "01";
                    res.Message = "Mã CrmOrPaypostCode không được bỏ trống.";
                    return res;
                }

                List<ZPartnerWareHouseSearchListResponse> listItem = new List<ZPartnerWareHouseSearchListResponse>();
                var reqWareHouseSearchList = WareHouseRepository.SearchList(new ZWareHouseSearchListRequest()
                {
                    CrmOrPaypostCode = req.CrmOrPaypostCode,
                    IsLock = "N",
                });

                if(reqWareHouseSearchList.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZWareHouseSearchListResponse>>(reqWareHouseSearchList.Data);
                    foreach(var item in list)
                    {
                        listItem.Add(new ZPartnerWareHouseSearchListResponse()
                        {
                            WareHouseID = item.ID,
                            WareHouseCode = item.WareHouseCode,
                            WareHouseName = item.WareHouseName,
                            IsDefault = item.IsDefault
                        });
                    }

                    if(listItem.Count > 0)
                    {
                        res.Code = "00";
                        res.Message = "Thành công.";
                        res.Data = JsonConvert.SerializeObject(listItem);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu.";
                    }
                }
                else
                {
                    res.Code = reqWareHouseSearchList.Code;
                    res.Message = reqWareHouseSearchList.Message;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.WareHouseSearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("WareHouseAdd")]
        public ZResponse WareHouseAdd(ZPartnerWareHouseAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resCheckWareHouse = PartnerRepository.WareHouseAdd(req);
                if(resCheckWareHouse.Code == "00")
                {
                    var itemCheckWareHouse = JsonConvert.DeserializeObject<ZWareHouseAddNewRequest>(resCheckWareHouse.Data);
                    var resWareHouseAdd = WareHouseRepository.AddNew(itemCheckWareHouse);
                    if(resWareHouseAdd.Code == "00")
                    {
                        res.Code = resWareHouseAdd.Code;
                        res.Message = resWareHouseAdd.Message;
                        res.Data = resWareHouseAdd.Data;
                    }
                    else
                    {
                        switch(res.Code)
                        {
                            case "99":
                                res.Code = "99";
                                break;
                            default:
                                res.Code = (Convert.ToInt32(resWareHouseAdd.Code) + 1).ToString().PadLeft(2, '0');
                                break;
                        }
                        res.Message = resWareHouseAdd.Message;
                        res.Data = resWareHouseAdd.Data;
                    }
                }
                else
                {
                    res.Code = resCheckWareHouse.Code;
                    res.Message = resCheckWareHouse.Message;
                    res.Data = resCheckWareHouse.Data;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.WareHouseAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region CÁC API LIÊN QUAN ĐẾN SẢN PHẨM
        [Route("ProductSearchList")]
        public ZResponse ProductSearchList(ZPartnerProductSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                req.CrmOrPaypostCode = string.IsNullOrEmpty(req.CrmOrPaypostCode) ? "" : req.CrmOrPaypostCode.Trim();
                req.CustomerToken = string.IsNullOrEmpty(req.CustomerToken) ? "" : req.CustomerToken.Trim();

                if(string.IsNullOrEmpty(req.CrmOrPaypostCode) && string.IsNullOrEmpty(req.CustomerToken))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập CrmOrPaypostCode hoặc CustomerToken.";
                    return res;
                }

                List<ZPartnerProductSearchListResponse> listItem = new List<ZPartnerProductSearchListResponse>();
                var reqPartnerInfo = PartnerRepository.PartnerInfo(new ZPartnerInfoRequest() { CrmOrPaypostCode = req.CrmOrPaypostCode, CustomerToken = req.CustomerToken });
                if(reqPartnerInfo.Code == "00")
                {
                    var ItemPartnerInfo = JsonConvert.DeserializeObject<ZPartnerInfoResponse>(reqPartnerInfo.Data);
                    var reqProductSearchList = OrderRepository.SearchProduct(new ZOrderSearchProductRequest()
                    {
                        ShopID = ItemPartnerInfo.ShopID
                    });

                    if(reqProductSearchList.Code == "00")
                    {
                        var list = JsonConvert.DeserializeObject<List<ZOrderSearchProductResponse>>(reqProductSearchList.Data);
                        foreach(var itemProduct in list)
                        {
                            List<ZPartnerProductVariantSearchListResponse> listProductVariant = new List<ZPartnerProductVariantSearchListResponse>();
                            foreach(var itemProductVariant in itemProduct.Variants)
                            {
                                listProductVariant.Add(new ZPartnerProductVariantSearchListResponse()
                                {
                                    ProductID = itemProductVariant.ProductID,
                                    ProductVariantID = itemProductVariant.VariantID,
                                    ProductVariantName = itemProductVariant.VariantName,
                                    ProductVariantImage = itemProductVariant.Image,
                                    ProductVariantPrice = itemProductVariant.Price,
                                    ProductVariantSKU = itemProductVariant.SKU,
                                    ProductVariantBarcode = itemProductVariant.Barcode,
                                    ProductVariantWeight = itemProductVariant.Weight,
                                    ProductVariantInStock = itemProductVariant.Quantity,
                                });
                            }

                            listItem.Add(new ZPartnerProductSearchListResponse()
                            {
                                ProductID = itemProduct.ProductID,
                                ProductName = itemProduct.ProductName,
                                ListProductVariant = listProductVariant
                            });
                        }

                        if(listItem.Count > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = JsonConvert.SerializeObject(listItem);
                        }
                        else
                        {
                            res.Code = "01";
                            res.Message = "Không tồn tại dữ liệu.";
                        }
                    }
                    else
                    {
                        res.Code = reqProductSearchList.Code;
                        res.Message = reqProductSearchList.Message;
                    }
                }
                else
                {
                    res.Code = reqPartnerInfo.Code;
                    res.Message = reqPartnerInfo.Message;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.ProductSearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region CÁC API LIÊN QUAN ĐẾN TỈNH/ QUẬN/ PHƯỜNG
        [Route("ProvinceSearchList")]
        public ZResponse ProvinceSearchList(ZProvinceListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZProvinceListResponse> listItem = new List<ZProvinceListResponse>();
                var resListProvince = ProvinceRepository.List(req);
                if(resListProvince.Code == "00")
                {
                    var listProvince = JsonConvert.DeserializeObject<List<ZProvinceListResponse>>(resListProvince.Data);
                    foreach(var item in listProvince)
                    {
                        listItem.Add(new ZProvinceListResponse()
                        {
                            ID = item.ID,
                            Title = item.Title,
                            Name = item.Description,
                            Description = item.Name
                        });
                    }

                    if(listItem.Count > 0)
                    {
                        res.Code = "00";
                        res.Message = "Lấy dữ liệu thành công";
                        res.Data = JsonConvert.SerializeObject(listItem);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu.";
                    }
                }
                else
                {
                    res.Code = resListProvince.Code;
                    res.Message = resListProvince.Message;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.ProvinceSearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("DistrictGetByProvinceID")]
        public ZResponse DistrictGetByProvinceID(ZDistrictGetByProvinceIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZDistrictGetByProvinceIDResponse> listItem = new List<ZDistrictGetByProvinceIDResponse>();
                var resListDistrict = DistrictRepository.GetByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = req.ProvinceID });
                if(resListDistrict.Code == "00")
                {
                    var listDistrict = JsonConvert.DeserializeObject<List<ZDistrictGetByProvinceIDResponse>>(resListDistrict.Data);
                    foreach(var item in listDistrict)
                    {
                        listItem.Add(new ZDistrictGetByProvinceIDResponse()
                        {
                            ID = item.ID,
                            Title = item.Title,
                            Name = item.Description,
                            Description = item.Name
                        });
                    }

                    if(listItem.Count > 0)
                    {
                        res.Code = "00";
                        res.Message = "Lấy dữ liệu thành công";
                        res.Data = JsonConvert.SerializeObject(listItem);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu.";
                    }
                }
                else
                {
                    res.Code = resListDistrict.Code;
                    res.Message = resListDistrict.Message;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.DistrictGetByProvinceID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("WardGetByDistrictID")]
        public ZResponse WardGetByDistrictID(ZDistrictGetByProvinceIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZWardGetByDistrictIDResponse> listItem = new List<ZWardGetByDistrictIDResponse>();
                var resListWard = WardRepository.GetByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = req.ProvinceID });
                if(resListWard.Code == "00")
                {
                    var listWard = JsonConvert.DeserializeObject<List<ZWardGetByDistrictIDResponse>>(resListWard.Data);
                    foreach(var item in listWard)
                    {
                        listItem.Add(new ZWardGetByDistrictIDResponse()
                        {
                            ID = item.ID,
                            Title = item.Title,
                            Name = item.Description,
                            Description = item.Name
                        });
                    }

                    if(listItem.Count > 0)
                    {
                        res.Code = "00";
                        res.Message = "Lấy dữ liệu thành công";
                        res.Data = JsonConvert.SerializeObject(listItem);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu.";
                    }
                }
                else
                {
                    res.Code = resListWard.Code;
                    res.Message = resListWard.Message;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.WardGetByDistrictID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("WardGetByProvinceID")]
        public ZResponse WardGetByProvinceID(ZDistrictGetByProvinceIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZWardGetByDistrictIDResponse> listItem = new List<ZWardGetByDistrictIDResponse>();
                var resListWard = WardRepository.GetByProvinceID(new ZWardGetByProvinceIDRequest() { ProvinceID = req.ProvinceID });
                if(resListWard.Code == "00")
                {
                    var listWard = JsonConvert.DeserializeObject<List<ZWardGetByDistrictIDResponse>>(resListWard.Data);
                    foreach(var item in listWard)
                    {
                        listItem.Add(new ZWardGetByDistrictIDResponse()
                        {
                            ID = item.ID,
                            Title = item.Title,
                            Name = item.Description,
                            Description = item.Name
                        });
                    }

                    if(listItem.Count > 0)
                    {
                        res.Code = "00";
                        res.Message = "Lấy dữ liệu thành công";
                        res.Data = JsonConvert.SerializeObject(listItem);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu.";
                    }
                }
                else
                {
                    res.Code = resListWard.Code;
                    res.Message = resListWard.Message;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.WardGetByDistrictID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region CÁC API LIÊN QUAN ĐẾN DANH MỤC QUỐC TẾ
        [Route("CountrySearchList")]
        public ZResponse CountrySearchList()
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZCountryListResponse> listItem = new List<ZCountryListResponse>();
                var resListProvince = CountryRepository.List();
                if(resListProvince.Code == "00")
                {
                    var listProvince = JsonConvert.DeserializeObject<List<ZCountryListResponse>>(resListProvince.Data);
                    foreach(var item in listProvince)
                    {
                        listItem.Add(new ZCountryListResponse()
                        {
                            ID = item.ID,
                            Name = item.Name,

                        });
                    }

                    if(listItem.Count > 0)
                    {
                        res.Code = "00";
                        res.Message = "Lấy dữ liệu thành công";
                        res.Data = JsonConvert.SerializeObject(listItem);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu.";
                    }
                }
                else
                {
                    res.Code = resListProvince.Code;
                    res.Message = resListProvince.Message;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerController.CountrySearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region CÁC API LIÊN QUAN ĐẾN VLP
        [Route("VLPStatusAdd")]
        public ZResponse VLPStatusAdd(ZPartnerVLPOrderStatusAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "SP_SYNC_ORDER_STATUS", new object[]
                {
                    req.AppCode,                        //P_APP_CODE
                    req.AppApiCode,                     //P_APP_API_CODE
                    req.SchemeCode,                     //P_SCHEME_CODE
                    req.OrderCode,                      //P_ORDER_CODE
                    req.OrderStatus,                    //P_ORDER_STATUS
                    req.OrderStatusCode,                //P_ORDER_STATUS_CODE
                    req.OperatorAccount,                //P_OPERATOR_ACCOUNT
                    req.OperatorName,                   //P_OPERATOR_NAME
                    req.OperatorTime,                   //P_OPERATOR_TIME
                    req.PosCode,                        //P_POS_CODE
                    req.FailReason,                     //P_FAIL_REASON
                    req.FailAction,                     //P_FAIL_ACTION
                    req.ReceiverName,                   //P_RECEIVER_NAME
                    req.Note,                           //P_NOTE
                    0,                                  //P_RESULT_CODE
                    0                                   //P_RESULT_MESSAGE
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if(result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RESULT_CODE"].Value);
                        if(result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
                        }
                        else
                        {
                            switch(result)
                            {
                                case -2:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu trùng";
                                    break;
                                case -99:
                                    res.Code = "01";
                                    res.Message = dbCommand.Parameters["P_RESULT_MESSAGE"].Value.ToString();
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerRepository.VLPStatusAdd: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}