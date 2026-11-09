using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using System.Data.Common;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;
using EMS.API.Core.Controllers;

namespace EMS.API.Core.Repositories
{
    public class OrderBatchRepository
    {
        public static ZResponse Add(ZOrderBatchAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.ADD_ORDER_BATCH", new object[]
                {
                    0,
                    req.CustomerID,
                    req.SenderInfo.FullName,
                    req.SenderInfo.MobileNumber,
                    req.SenderInfo.ProvinceID,
                    req.SenderInfo.DistrictID,
                    req.SenderInfo.WardID,
                    req.SenderInfo.Street,
                    req.SenderInfo.PickUpAddressID,
                    req.ReceiverInfo.FullName,
                    req.ReceiverInfo.MobileNumber,
                    req.ReceiverInfo.ProvinceID,
                    req.ReceiverInfo.DistrictID,
                    req.ReceiverInfo.WardID,
                    req.ReceiverInfo.Street,
                    req.ReceiverInfo.IsUpdatedCustomer,
                    req.WareHouseID,
                    req.CollectionType,
                    req.TransportPayer,
                    req.TransportMainServiceID,
                    req.TransportExtraServiceID,
                    req.TotalCOD,
                    req.Note,
                    req.Channel,
                    req.ListOrderInfo.ToXML(),
                    req.ActionType,
                    req.CreatedBy,
                    req.DeviceName,
                    req.ShopID,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[00].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
                            res.Data = dbCommand.Parameters["P_OUT_BATCH_CODE"].Value.ToString();
                        }
                        else
                        {
                            var ProductVariantName = dbCommand.Parameters["P_OUT_PRODUCT_VARIANT_NAME"].Value.ToString();
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Cửa hàng không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Khách hàng không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Dịch vụ chính CPN TMĐT Nhanh tiết kiệm chỉ áp dụng cho liên tỉnh - liên vùng. Vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Dịch vụ EMS Thỏa thuận không áp dụng cho nội tỉnh. Vui lòng kiểm tra lại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Thiếu thông tin người gửi.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Mã đơn hàng đã tồn tại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Sản phẩm không tồn tại.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = string.Format("Giá bán của sản phẩm {0} đã thay đổi.", ProductVariantName); ;
                                    break;
                                case -9:
                                    res.Code = "09";
                                    res.Message = string.Format("Sản phẩm {0} chưa được nhập kho.", ProductVariantName);
                                    break;
                                case -10:
                                    res.Code = "10";
                                    res.Message = string.Format("Số lượng của sản phẩm {0} trong kho không đủ lập đơn hàng.", ProductVariantName);
                                    break;
                                case -11:
                                    res.Code = "11";
                                    res.Message = "Thêm sản phẩm vào đơn hàng thất bại.";
                                    break;
                                case -12:
                                    res.Code = "12";
                                    res.Message = "Thêm sản phẩm vào phiếu xuất kho thất bại.";
                                    break;
                                case -13:
                                    res.Code = "13";
                                    res.Message = string.Format("Sản phẩm {0} không tồn tại trong kho.", ProductVariantName);
                                    break;
                                case -14:
                                    res.Code = "14";
                                    res.Message = string.Format("Sản phẩm {0} trong kho không đủ.", ProductVariantName);
                                    break;
                                case -15:
                                    res.Code = "15";
                                    res.Message = string.Format("Sản phẩm {0} trong kho đã hết.", ProductVariantName);
                                    break;
                                case -16:
                                    res.Code = "16";
                                    res.Message = "Lưu lịch sử phiếu xuất kho thất bại.";
                                    break;
                                case -17:
                                    res.Code = "17";
                                    res.Message = "Lưu lịch sử đơn hàng thất bại.";
                                    break;
                                case -18:
                                    res.Code = "18";
                                    res.Message = "Tạo mã bưu gửi thất bại.";
                                    break;
                                case -19:
                                    res.Code = "19";
                                    res.Message = "Dịch vụ Thương mại điện tử Đồng giá Nội tỉnh chỉ áp dụng cho nội tỉnh Thành phố Hồ Chí Minh. Vui lòng kiểm tra lại.";
                                    break;
                                case -20:
                                    res.Code = "20";
                                    res.Message = "Gói cước Dịch vụ vận chuyển chính không hợp lệ.";
                                    break;
                                case -21:
                                    break;
                                case -22:
                                    res.Code = "22";
                                    res.Message = "Thay đổi số lượng của Pallet thất bại.";
                                    break;
                                case -23:
                                    res.Code = "23";
                                    res.Message = "Mã lô đã tồn tại.";
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #region Gửi vận chuyển
        public static ZResponse SendTransportInfo(ZOrderBatchSendTransportInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZOrderBatchSendTransportInfoResponse> listItem = new List<ZOrderBatchSendTransportInfoResponse>();
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.SEND_TRANSPORT_INFO", new object[]
                {
                    req.BatchCode,
                    req.ShopID,
                    req.AmndState,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    listItem.Add(new ZOrderBatchSendTransportInfoResponse()
                                    {
                                        CrmOrPayPostCode = dr["CRM_OR_PAYPOST_CODE"].ToString(),
                                        ShopPhone = dr["SHOP_PHONE"].ToString(),
                                        ShopStoreName = dr["SHOP_STORE_NAME"].ToString(),
                                        ShopFullName = dr["SHOP_FULL_NAME"].ToString(),
                                        ShopEmail = dr["SHOP_EMAIL"].ToString(),
                                        ShopID = string.IsNullOrEmpty(dr["SHOP_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SHOP_ID"]),
                                        CollectionPostOfficeCode = dr["COLLECTION_POST_OFFICE_CODE"].ToString(),

                                        OrderID = string.IsNullOrEmpty(dr["ORDER_ID"].ToString()) ? 0 : Convert.ToInt32(dr["ORDER_ID"]),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        SenderName = dr["SENDER_NAME"].ToString(),
                                        SenderPhone = dr["SENDER_PHONE"].ToString(),
                                        SenderStreet = dr["SENDER_STREET"].ToString(),
                                        SenderFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                        SenderProvinceID = string.IsNullOrEmpty(dr["SENDER_PROVINCE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_PROVINCE_ID"]),
                                        SenderDistrictID = string.IsNullOrEmpty(dr["SENDER_DISTRICT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_DISTRICT_ID"]),
                                        SenderWardID = string.IsNullOrEmpty(dr["SENDER_WARD_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_WARD_ID"]),
                                        SenderEmail = dr["SENDER_EMAIL"].ToString(),
                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        ReceiverProvinceID = string.IsNullOrEmpty(dr["RECEIVER_PROVINCE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["RECEIVER_PROVINCE_ID"]),
                                        ReceiverPhone = dr["RECEIVER_PHONE"].ToString(),
                                        ReceiverDistrictID = string.IsNullOrEmpty(dr["RECEIVER_DISTRICT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["RECEIVER_DISTRICT_ID"]),
                                        ReceiverWardID = string.IsNullOrEmpty(dr["RECEIVER_WARD_ID"].ToString()) ? 0 : Convert.ToInt32(dr["RECEIVER_WARD_ID"]),
                                        ReceiverStreet = dr["RECEIVER_STREET"].ToString(),
                                        ReceiverFullAddress = dr["RECEIVER_FULL_ADDRESS"].ToString(),
                                        ProductName = dr["PRODUCT_NAME"].ToString(),
                                        ProductValue = string.IsNullOrEmpty(dr["PRODUCT_VALUE"].ToString()) ? 0 : Convert.ToInt64(dr["PRODUCT_VALUE"]),
                                        ProductItem = dr["PRODUCT_ITEM"].ToString(),
                                        ProductCode = dr["PRODUCT_CODE"].ToString(),
                                        ProductDescription = dr["PRODUCT_DESCRIPTION"].ToString(),
                                        ProductNote = dr["PRODUCT_NOTE"].ToString(),
                                        ProductQuantity = string.IsNullOrEmpty(dr["PRODUCT_QUANTITY"].ToString()) ? 0 : Convert.ToInt32(dr["PRODUCT_QUANTITY"]),
                                        Weight = string.IsNullOrEmpty(dr["WEIGHT"].ToString()) ? 0 : Convert.ToInt32(dr["WEIGHT"]),
                                        TotalAmount = string.IsNullOrEmpty(dr["TOTAL_AMOUNT"].ToString()) ? 0 : Convert.ToInt64(dr["TOTAL_AMOUNT"]),
                                        ServiceFee = string.IsNullOrEmpty(dr["SERVICE_FEE"].ToString()) ? 0 : Convert.ToInt64(dr["SERVICE_FEE"]),
                                        TransportFee = string.IsNullOrEmpty(dr["TRANSPORT_FEE"].ToString()) ? 0 : Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        OrderStatus = dr["ORDER_STATUS"].ToString(),
                                        CollectionType = string.IsNullOrEmpty(dr["COLLECTION_TYPE"].ToString()) ? 0 : Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                        Transport = dr["IS_TRANSPORT"].ToString(),
                                        CreatedBy = string.IsNullOrEmpty(dr["CREATED_BY"].ToString()) ? 0 : Convert.ToInt32(dr["CREATED_BY"]),

                                        EspeciallyServiceCode = dr["ESPECIALLY_SERVICE_CODE"].ToString(),
                                        EspeciallyServiceType = string.IsNullOrEmpty(dr["ESPECIALLY_SERVICE_TYPE"].ToString()) ? 0 : Convert.ToInt32(dr["ESPECIALLY_SERVICE_TYPE"]),
                                        Classification = new ZBaseResponse()
                                        {
                                            Code = dr["CLASSIFICATION"].ToString(),
                                            Name = XData.GetNameListTransportServiceClassification(dr["CLASSIFICATION"].ToString()),
                                        },

                                        AdditionService = dr["ADDITION_SERVICE"].ToString(),

                                        POCode = dr["PO_CODE"].ToString(),

                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        Consignment = new ZBaseResponse()
                                        {
                                            Code = dr["IS_CONSIGNMENT"].ToString(),
                                            Name = XData.GetNameConsignment(dr["IS_CONSIGNMENT"].ToString()),
                                        },

                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                        TransportSizeHeight = Convert.ToInt32(dr["TRANSPORT_SIZE_HEIGHT"]),
                                        TransportSizeWidth = Convert.ToInt32(dr["TRANSPORT_SIZE_WIDTH"]),
                                        TransportSizeLength = Convert.ToInt32(dr["TRANSPORT_SIZE_LENGTH"]),
                                        Note = dr["NOTE"].ToString(),
                                        TransportPayer = Convert.ToInt32(dr["TRANSPORT_PAYER"]),
                                    });
                                }
                            }
                            if (listItem.Count > 0)
                            {
                                if (listItem.Where(m => m.OrderStatus != "N" && m.OrderStatus != "L").Count() > 0)
                                {
                                    res.Code = "02";
                                    res.Message = "Trạng thái đơn hàng không hợp lệ";
                                }
                                else if (listItem.Where(m => m.ShippingCode == "").Count() > 0)
                                {
                                    res.Code = "03";
                                    res.Message = "Mã bưu gửi bị trống.";
                                }
                                else if (listItem.Count < 2)
                                {
                                    res.Code = "04";
                                    res.Message = "Bưu gửi lô phải đạt tối thiểu 2 đơn hàng.";
                                }
                                else
                                {
                                    res.Code = "00";
                                    res.Message = "Lấy dữ liệu thành công";
                                    res.Data = JsonConvert.SerializeObject(listItem);
                                }
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.SendTransportInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse SendTransport(ZOrderBatchSendTransportRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                ZResponse resUpdateTransport = new ZResponse();
                var resSendRequestTransport = XCommon.SendTransportOrderBatch(req.AmndState, req.BatchCode, req.ShopID);
                if (resSendRequestTransport.Code == "00")
                {
                    resUpdateTransport = SendTransportSuccess(new ZOrderBatchSendTransportSuccessRequest() { AmndState = req.AmndState, BatchCode = req.BatchCode, ShopID = req.ShopID, UpdatedBy = req.CreatedBy });
                }
                else
                {
                    resUpdateTransport = SendTransportError(new ZOrderBatchSendTransportErrorRequest() { AmndState = req.AmndState, BatchCode = req.BatchCode, ShopID = req.ShopID });
                }

                if (resUpdateTransport.Code == "00")
                {
                    res.Code = resSendRequestTransport.Code;
                    res.Message = resSendRequestTransport.Message;
                }
                else
                {
                    res.Code = resUpdateTransport.Code;
                    res.Message = resUpdateTransport.Message;
                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("OrderBatchController.SendTransport: request={0}||response={1}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(resUpdateTransport)));
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.SendTransport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SendTransportData(ZOrderBatchSendTransportDataRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZOrderBatchSendTransportDataResponse> listItem = new List<ZOrderBatchSendTransportDataResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.SEND_TRANSPORT_DATA", new object[]
                {
                    0,
                    req.BatchCode,  //P_BATCH_CODE
                    req.ShopID,     //P_SHOP_ID
                    req.UpdatedBy,  //P_UPDATED_BY
                    0,              //P_OUT_ERROR_CODE
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            if (result == -90)
                            {
                                var ListErrorCode = dbCommand.Parameters["P_OUT_ERROR_CODE"].Value.ToString();//123&-4,-5,-6;123&-4,-5,-6;

                                var ListOrderErrorCode = ListErrorCode.Trim(';').Split(';'); //123&-4,-5,-6

                                foreach (var ItemOrderErrorCode in ListOrderErrorCode)
                                {
                                    var ErrorDetail = ItemOrderErrorCode.Trim('&').Split('&');
                                    if (ErrorDetail.Length == 2)
                                    {
                                        var ErrorOrder = ItemOrderErrorCode.Trim('&').Split('&')[0];//123
                                        var ErrorCode = ItemOrderErrorCode.Trim('&').Split('&')[1].Trim(',').Split(',');//-4,-5,-6
                                        var ErrorMessage = "";

                                        foreach (var ItemCode in ErrorCode)
                                        {
                                            int ConvertItemCode = 0;
                                            var ProductVariantName = "";
                                            if (ItemCode.Contains("-5") == true || ItemCode.Contains("-6") == true || ItemCode.Contains("-7") == true)
                                            {
                                                ConvertItemCode = Convert.ToInt32(ItemCode.Substring(ItemCode.Length - 2, 2));
                                                ProductVariantName = ItemCode.Substring(0, ItemCode.Length - 2);
                                            }
                                            else
                                            {
                                                ConvertItemCode = Convert.ToInt32(ItemCode);
                                            }

                                            switch (ConvertItemCode)
                                            {
                                                case -3:
                                                    ErrorMessage += "Trạng thái không hợp lệ. \n";
                                                    break;
                                                case -4:
                                                    ErrorMessage += "Sản phẩm không tồn tại. \n";
                                                    break;
                                                case -5:
                                                    ErrorMessage += string.Format("Sản phẩm {0} không tồn tại trong kho. \n", ProductVariantName);
                                                    break;
                                                case -6:
                                                    ErrorMessage += string.Format("Sản phẩm {0} trong kho không đủ. \n", ProductVariantName);
                                                    break;
                                                case -7:
                                                    ErrorMessage += string.Format("Sản phẩm {0} trong kho đã hết. \n", ProductVariantName);
                                                    break;
                                                case -8:
                                                    ErrorMessage += "Tạo mã bưu gửi thất bại. \n";
                                                    break;
                                                default:
                                                    ErrorMessage += string.Format("Thất bại. Code: {0} \n", result);
                                                    break;
                                            };
                                        }
                                        listItem.Add(new ZOrderBatchSendTransportDataResponse()
                                        {
                                            OrderID = Convert.ToInt32(ErrorOrder),
                                            Description = ErrorMessage
                                        });
                                    }
                                }

                                res.Code = "90";
                                res.Message = "Lỗi dữ liệu đơn hàng, vui lòng kiểm tra lại.";
                                res.Data = JsonConvert.SerializeObject(listItem);
                            }
                            else
                            {
                                switch (result)
                                {
                                    case -1:
                                        res.Code = "01";
                                        res.Message = "Cửa hàng không tồn tại.";
                                        break;
                                    case -2:
                                        res.Code = "02";
                                        res.Message = "Đơn hàng không tồn tại.";
                                        break;
                                    case -3:
                                        res.Code = "03";
                                        res.Message = "Bưu gửi lô phải đạt tối thiểu 2 đơn hàng.";
                                        break;
                                    default:
                                        res.Code = "99";
                                        res.Message = "Thất bại.";
                                        break;
                                };
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.SendTransportData: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //public static ZResponse SendTransportData(ZOrderBatchSendTransportDataRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        List<ZOrderBatchSendTransportDataResponse> listItem = new List<ZOrderBatchSendTransportDataResponse>();
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.SEND_TRANSPORT_DATA", new object[]
        //        {
        //            0,
        //            req.BatchCode,  //P_BATCH_CODE
        //            req.ShopID,     //P_SHOP_ID
        //            req.UpdatedBy,  //P_UPDATED_BY
        //            0,              //P_OUT_ERROR_CODE
        //            0,
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
        //                }
        //                else
        //                {
        //                    var ProductVariantName = dbCommand.Parameters["P_OUT_PRODUCT_VARIANT_NAME"].Value.ToString();
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Cửa hàng không tồn tại.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Đơn hàng không tồn tại.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Trạng thái không hợp lệ.";
        //                            break;
        //                        case -4:
        //                            res.Code = "04";
        //                            res.Message = "Sản phẩm không tồn tại.";
        //                            break;
        //                        case -5:
        //                            res.Code = "05";
        //                            res.Message = string.Format("Sản phẩm {0} không tồn tại trong kho.", ProductVariantName);
        //                            break;
        //                        case -6:
        //                            res.Code = "06";
        //                            res.Message = string.Format("Sản phẩm {0} trong kho không đủ.", ProductVariantName);
        //                            break;
        //                        case -7:
        //                            res.Code = "07";
        //                            res.Message = string.Format("Sản phẩm {0} trong kho đã hết.", ProductVariantName);
        //                            break;
        //                        case -8:
        //                            res.Code = "08";
        //                            res.Message = "Tạo mã bưu gửi thất bại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại";
        //                            break;
        //                    }
        //                    res.Data = dbCommand.Parameters["P_OUT_ORDER_CODE"].Value.ToString();
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.SendTransportData: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        public static ZResponse SendTransportSuccess(ZOrderBatchSendTransportSuccessRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.SEND_TRANSPORT_SUCCESS", new object[]
                {
                    0,
                    req.BatchCode,
                    req.ShopID,
                    req.AmndState,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Đơn hàng không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lưu lịch sử đơn hàng thất bại";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Phiếu xuất kho không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Lưu lịch sử phiếu xuất kho thất bại.";
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.SendTransportSuccess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse SendTransportError(ZOrderBatchSendTransportErrorRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.SEND_TRANSPORT_ERROR", new object[]
                {
                    0,
                    req.BatchCode,
                    req.ShopID,
                    req.AmndState,
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Đơn hàng không tồn tại.";
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.UpdateTransportError: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion

        #region Hủy vận chuyển
        public static ZResponse CancelTransport(ZOrderBatchCancelTransportRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resCancelTransportData = OrderRepository.CancelTransportData(new ZOrderCancelTransportDataRequest()
                {
                    OrderID = req.OrderID,
                    ShopID = req.ShopID
                });

                if (resCancelTransportData.Code == "00")
                {
                    ZResponse resCancelTransportUpdate = new ZResponse();

                    var resCancelTransport = XCommon.CancelTransportOrder(req.CrmOrPaypostCode, req.ShippingCode);

                    if (resCancelTransport.Code == "00")
                    {
                        resCancelTransportUpdate = OrderRepository.CancelTransportSuccess(new ZOrderCancelTransportSuccessRequest()
                        {
                            OrderID = req.OrderID,
                            ShopID = req.ShopID,
                            UpdatedBy = req.UpdatedBy
                        });
                    }
                    else
                    {
                        resCancelTransportUpdate = OrderRepository.CancelTransportError(new ZOrderCancelTransportErrorRequest()
                        {
                            OrderID = req.OrderID,
                            ShopID = req.ShopID,
                        });
                    }

                    if (resCancelTransportUpdate.Code == "00")
                    {
                        res.Code = resCancelTransport.Code;
                        res.Message = resCancelTransport.Message;
                    }
                    else
                    {
                        res.Code = resCancelTransportUpdate.Code;
                        res.Message = resCancelTransportUpdate.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("OrderBatchRepository.CancelTransport: request={0}||response={1}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(resCancelTransportUpdate)));
                    }
                }
                else
                {
                    res.Code = resCancelTransportData.Code;
                    res.Message = resCancelTransportData.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.CancelTransport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse CancelTransportInfo(ZOrderBatchCancelTransportInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZOrderBatchCancelTransportInfoResponse> listItem = new List<ZOrderBatchCancelTransportInfoResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.CANCEL_TRANSPORT_INFO", new object[]
                {
                    req.BatchCode,
                    req.ShopID,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    listItem.Add(new ZOrderBatchCancelTransportInfoResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        OrderStatus = Convert.ToString(dr["ORDER_STATUS"]),
                                        CrmOrPaypostCode = Convert.ToString(dr["CRM_OR_PAYPOST_CODE"]),
                                        ShippingCode = Convert.ToString(dr["SHIPPING_CODE"]),
                                    });
                                }
                            }
                            if (listItem.Count > 0)
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
                            res.Code = "99";
                            res.Message = "Thất bại";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.CancelTransportInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse CancelTransportData(ZOrderBatchCancelTransportDataRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZOrderBatchCancelTransportDataResponse> listItem = new List<ZOrderBatchCancelTransportDataResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.CANCEL_TRANSPORT_DATA", new object[]
                {
                    0,
                    req.BatchCode,  //P_BATCH_CODE
                    req.ShopID,     //P_SHOP_ID
                    req.UpdatedBy,  //P_UPDATED_BY
                    0,              //P_OUT_ERROR_CODE
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            if (result == -90)
                            {
                                var ListErrorCode = dbCommand.Parameters["P_OUT_ERROR_CODE"].Value.ToString();//123&-4,-5,-6;123&-4,-5,-6;

                                var ListOrderErrorCode = ListErrorCode.Trim(';').Split(';'); //123&-4,-5,-6

                                foreach (var ItemOrderErrorCode in ListOrderErrorCode)
                                {
                                    var ErrorDetail = ItemOrderErrorCode.Trim('&').Split('&');
                                    if (ErrorDetail.Length == 2)
                                    {
                                        var ErrorOrder = ItemOrderErrorCode.Trim('&').Split('&')[0];//123
                                        var ErrorCode = ItemOrderErrorCode.Trim('&').Split('&')[1].Trim(',').Split(',');//-4,-5,-6
                                        var ErrorMessage = "";

                                        foreach (var ItemCode in ErrorCode)
                                        {
                                            switch (Convert.ToInt32(ItemCode))
                                            {
                                                case -2:
                                                    ErrorMessage += "Trạng thái đơn hàng không hợp lệ. \n";
                                                    break;
                                                case -3:
                                                    ErrorMessage += "Trạng thái phiếu xuất kho. \n";
                                                    break;
                                                case -4:
                                                    ErrorMessage += "Sản phẩm không tồn tại trong kho. \n";
                                                    break;
                                                default:
                                                    ErrorMessage += string.Format("Thất bại. Code: {0} \n", result);
                                                    break;
                                            };
                                        }

                                        listItem.Add(new ZOrderBatchCancelTransportDataResponse()
                                        {
                                            OrderID = Convert.ToInt32(ErrorOrder),
                                            Description = ErrorMessage
                                        });
                                    }
                                }

                                res.Code = "90";
                                res.Message = "Lỗi dữ liệu đơn hàng, vui lòng kiểm tra lại.";
                                res.Data = JsonConvert.SerializeObject(listItem);
                            }
                            else
                            {
                                switch (result)
                                {
                                    case -1:
                                        res.Code = "01";
                                        res.Message = "Đơn hàng không tồn tại.";
                                        break;
                                    default:
                                        res.Code = "99";
                                        res.Message = "Thất bại.";
                                        break;
                                };
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.CancelTransportData: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        public static ZResponse CancelOrderInfo(ZOrderBatchCancelOrderInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZOrderBatchCancelOrderInfoResponse> listItem = new List<ZOrderBatchCancelOrderInfoResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.CANCEL_ORDER_INFO", new object[]
                {
                    req.BatchCode,
                    req.ShopID,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    listItem.Add(new ZOrderBatchCancelOrderInfoResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        OrderStatus = Convert.ToString(dr["ORDER_STATUS"]),
                                        CrmOrPaypostCode = Convert.ToString(dr["CRM_OR_PAYPOST_CODE"]),
                                        ShippingCode = Convert.ToString(dr["SHIPPING_CODE"]),
                                    });
                                }
                            }
                            if (listItem.Count > 0)
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
                            res.Code = "99";
                            res.Message = "Thất bại";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.CancelOrderInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse NewStatus(ZOrderBatchNewStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZOrderBatchNewStatusResponse> listItem = new List<ZOrderBatchNewStatusResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.NEW_STATUS", new object[]
                {
                    0,
                    req.BatchCode,
                    req.ShopID,
                    req.UpdatedBy,
                    0,              //P_OUT_ERROR_CODE
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            if (result == -90)
                            {
                                var ListErrorCode = dbCommand.Parameters["P_OUT_ERROR_CODE"].Value.ToString();//123&-4,-5,-6;123&-4,-5,-6;

                                var ListOrderErrorCode = ListErrorCode.Trim(';').Split(';'); //123&-4,-5,-6

                                foreach (var ItemOrderErrorCode in ListOrderErrorCode)
                                {
                                    var ErrorDetail = ItemOrderErrorCode.Trim('&').Split('&');
                                    if (ErrorDetail.Length == 2)
                                    {
                                        var ErrorOrder = ItemOrderErrorCode.Trim('&').Split('&')[0];//123
                                        var ErrorCode = ItemOrderErrorCode.Trim('&').Split('&')[1].Trim(',').Split(',');//-4,-5,-6
                                        var ErrorMessage = "";

                                        foreach (var ItemCode in ErrorCode)
                                        {
                                            switch (Convert.ToInt32(ItemCode))
                                            {
                                                case -2:
                                                    res.Code = "02";
                                                    res.Message = "Sai trạng thái đơn hàng.";
                                                    break;
                                                case -3:
                                                    res.Code = "03";
                                                    res.Message = "Kho không tồn tại.";
                                                    break;
                                                case -4:
                                                    res.Code = "04";
                                                    res.Message = "Kho bị khoá.";
                                                    break;
                                                case -5:
                                                    ErrorMessage += "Lưu lịch sử phiếu xuất kho thất bại. \n";
                                                    break;
                                                case -6:
                                                    ErrorMessage += "Lưu lịch sử đơn hàng thất bại. \n";
                                                    break;
                                                case -7:
                                                    ErrorMessage += "Sản phẩm không tồn tại trong kho. \n";
                                                    break;
                                                case -8:
                                                    ErrorMessage += "Sản phẩm trong kho đã hết. \n";
                                                    break;
                                                case -9:
                                                    ErrorMessage += "Sản phẩm trong kho không đủ. \n";
                                                    break;
                                                case -10:
                                                    ErrorMessage += "Đơn hàng chưa đầy đủ thông tin. \n";
                                                    break;
                                                case -11:
                                                    ErrorMessage += "Dịch vụ chính CPN TMĐT Nhanh tiết kiệm chỉ áp dụng cho liên tỉnh - liên vùng. \n";
                                                    break;
                                                case -12:
                                                    ErrorMessage += "Thiếu thông tin khách hàng. \n";
                                                    break;
                                                //case -13:
                                                //    ErrorMessage += "Thiếu số tiền thu hộ - chi hộ (COD). \n";
                                                //    break;
                                                case -14:
                                                    ErrorMessage += "Thiếu tên đơn hàng. \n";
                                                    break;
                                                case -15:
                                                    ErrorMessage += "Thiếu số lượng sản phẩm. \n";
                                                    break;
                                                case -16:
                                                    ErrorMessage += "Thiếu giá trị đơn hàng. \n";
                                                    break;
                                                case -17:
                                                    ErrorMessage += "Thiếu trọng lượng đơn hàng. \n";
                                                    break;
                                                case -18:
                                                    ErrorMessage += "Thiếu dịch vụ vận chuyển chính. \n";
                                                    break;
                                                case -19:
                                                    ErrorMessage += "Dịch vụ EMS Thỏa thuận không áp dụng cho nội tỉnh. Vui lòng kiểm tra lại. \n";
                                                    break;
                                                case -20:
                                                    ErrorMessage += "Thiếu địa chỉ lấy hàng. \n";
                                                    break;
                                                case -21:
                                                    ErrorMessage += "Số tiền COD tối đa 50 triệu đồng/BG. \n";
                                                    break;
                                                case -22:
                                                    ErrorMessage += "Giá trị đơn hàng không vượt quá 100 triệu đồng. \n";
                                                    break;
                                                case -23:
                                                    ErrorMessage += "Thiếu số tiền thu phí hủy đơn (PHU). \n";
                                                    break;
                                                case -24:
                                                    ErrorMessage += "Gói cước dịch vụ chính không hợp lệ. \n";
                                                    break;
                                                case -25:
                                                    ErrorMessage += "Dịch vụ Thương mại điện tử Đồng giá Nội tỉnh chỉ áp dụng cho nội tỉnh Thành phố Hồ Chí Minh. Vui lòng kiểm tra lại. \n";
                                                    break;
                                                case -26:
                                                    ErrorMessage += "Thay đổi số lượng của Pallet thất bại. \n";
                                                    break;
                                                case -27:
                                                    ErrorMessage += "Thiếu thông tin người nhận. \n";
                                                    break;
                                                case -28:
                                                    ErrorMessage += "Thiếu thông tin người gửi. \n";
                                                    break;
                                                case -29:
                                                    ErrorMessage += "Thiếu thông tin sản phẩm. \n";
                                                    break;
                                                //case -30:
                                                //    ErrorMessage += "Thiếu giá bán sản phẩm. \n";
                                                //    break;
                                                case -31:
                                                    ErrorMessage += "Vui lòng chọn Dịch vụ thu hộ - chi hộ. \n";
                                                    break;
                                                case -32:
                                                    ErrorMessage += "Vui lòng sử dụng dịch vụ Thu phí ship hàng (PSH) để thu cước người nhận. \n";
                                                    break;
                                                case -33:
                                                    ErrorMessage += "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. \n";
                                                    break;
                                                case -34:
                                                    ErrorMessage += "Tạo mã bưu gửi thất bại. \n";
                                                    break;
                                                default:
                                                    ErrorMessage += string.Format("Thất bại. Code: {0} \n", result);
                                                    break;
                                            }
                                        }
                                        listItem.Add(new ZOrderBatchNewStatusResponse()
                                        {
                                            OrderID = Convert.ToInt32(ErrorOrder),
                                            Description = ErrorMessage
                                        });
                                    }
                                }

                                res.Code = "90";
                                res.Message = "Lỗi dữ liệu đơn hàng, vui lòng kiểm tra lại.";
                                res.Data = JsonConvert.SerializeObject(listItem);
                            }
                            else
                            {
                                switch (result)
                                {
                                    case -1:
                                        res.Code = "01";
                                        res.Message = "Đơn hàng không tồn tại.";
                                        break;
                                    case -2:
                                        res.Code = "02";
                                        res.Message = "Bưu gửi lô phải đạt tối thiểu 2 đơn hàng.";
                                        break;
                                    case -3:
                                        res.Code = "03";
                                        res.Message = "Tổng khối lượng của đơn hàng lô phải từ 2,000gr - 200,000gr.";
                                        break;
                                    case -4:
                                        res.Code = "04";
                                        res.Message = "Giá trị đơn lô không vượt quá 100 triệu đồng.";
                                        break;
                                    default:
                                        res.Code = "99";
                                        res.Message = "Thất bại.";
                                        break;
                                };
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.NewStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
    }
}