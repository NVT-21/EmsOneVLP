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
using System.Threading.Tasks;
using EMS.API.Core.Repositories;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("OrderBatch")]
    public class OrderBatchController : ApiController
    {
        [Route("Add")]
        public ZResponse Add(ZOrderBatchAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resAddOrderBatch = OrderBatchRepository.Add(req);
                if (resAddOrderBatch.Code == "00")
                {
                    switch (req.ActionType)
                    {
                        case "D":
                        case "N":
                            res.Code = resAddOrderBatch.Code;
                            res.Message = resAddOrderBatch.Message;
                            res.Data = resAddOrderBatch.Data;
                            break;
                        case "W":
                            res = OrderBatchRepository.SendTransport(new ZOrderBatchSendTransportRequest() { AmndState = "I", BatchCode = resAddOrderBatch.Data, ShopID = req.ShopID, CreatedBy = req.CreatedBy });
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
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("EditInfo")]
        public ZResponse EditInfo(ZOrderBatchEditInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                long TotalCOD = 0;
                ZOrderBatchEditInfoResponse item = null;
                List<ZOrderBatchEditInfoOrderInfoResponse> ListOrderInfo = new List<ZOrderBatchEditInfoOrderInfoResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.EDIT_ORDER_BATCH_INFO", new object[]
                {
                    req.BatchCode,    //P_SHIPPING_CODE NUMBER
                    req.ShopID,       //P_SHOP_ID       NUMBER
                    0,                //P_RETURN_CODE   OUT NUMBER
                    0                 //P_OUT_CURSOR    OUT T_CURSOR
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
                                    var OrderStatus = Convert.ToString(dr["ORDER_STATUS"]);

                                    if (OrderStatus != "D" && OrderStatus != "N")
                                    {
                                        res.Code = "02";
                                        res.Message = "Trạng thái không hợp lệ";
                                        return res;
                                    }
                                    else if (item != null)
                                    {
                                        if (item.OrderStatus != OrderStatus)
                                        {
                                            res.Code = "03";
                                            res.Message = "Trạng thái đơn không đồng nhất, vui lòng kiểm tra lại.";
                                            return res;
                                        }
                                    }

                                    if (Convert.ToInt64(dr["TOTAL_COD"]) > 0)
                                    {
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]);
                                    }

                                    List<ZOrderBatchEditInfoOrderInfoProductInfoResponse> ListProductInfo = new List<ZOrderBatchEditInfoOrderInfoProductInfoResponse>();
                                    var resOrderItem = OrderItemRepository.GetOrderItemByOrderID(new ZOrderItemGetByOrderIDRequest() { OrderID = Convert.ToInt32(dr["ORDER_ID"]) });
                                    if (resOrderItem.Code == "00")
                                    {
                                        var ListProduct = JsonConvert.DeserializeObject<List<ZOrderItemGetByOrderIDResponse>>(resOrderItem.Data);
                                        foreach (var itemProduct in ListProduct)
                                        {
                                            ListProductInfo.Add(new ZOrderBatchEditInfoOrderInfoProductInfoResponse()
                                            {
                                                ProductID = itemProduct.ProductID,
                                                ProductName = itemProduct.ProductName,
                                                VariantID = itemProduct.VariantID,
                                                VariantName = itemProduct.VariantName,
                                                NumberOfProduct = itemProduct.NumberOfProduct,
                                                Price = itemProduct.Price,
                                                Weight = itemProduct.Weight,
                                                Discount = itemProduct.Discount
                                            });
                                        }
                                    }

                                    ListOrderInfo.Add(new ZOrderBatchEditInfoOrderInfoResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        OrderName = Convert.ToString(dr["ORDER_NAME"]),
                                        OrderStatus = OrderStatus,
                                        OrderQuantity = Convert.ToInt32(dr["ORDER_QUANTITY"]),
                                        OrderValue = Convert.ToInt32(dr["ORDER_VALUE"]),
                                        TransportFee = Convert.ToInt32(dr["TRANSPORT_FEE"]),
                                        TotalAmount = Convert.ToInt32(dr["TOTAL_AMOUNT"]),
                                        Length = Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"]),
                                        Width = Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"]),
                                        Height = Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"]),
                                        Weight = Convert.ToDouble(dr["TRANSPORT_WEIGHT"]),
                                        TotalPHU = Convert.ToInt64(dr["TOTAL_PHU"]),
                                        Message = Convert.ToString(dr["MESSAGE"]),
                                        ListProductInfo = ListProductInfo,
                                    });

                                    if (item == null)
                                    {
                                        item = new ZOrderBatchEditInfoResponse()
                                        {
                                            BatchCode = Convert.ToString(dr["BATCH_CODE"]),
                                            BuyerInfo = new ZOrderBatchEditInfoBuyerInfoResponse()
                                            {
                                                MobileNumber = Convert.ToString(dr["BUYER_MOBILE_NUMBER"]),
                                                FullName = Convert.ToString(dr["BUYER_NAME"]),
                                            },
                                            SenderInfo = new ZOrderBatchEditInfoSenderInfoResponse()
                                            {
                                                PickUpAddressID = Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"]),
                                                FullName = Convert.ToString(dr["SENDER_NAME"]),
                                                MobileNumber = Convert.ToString(dr["SENDER_PHONE"]),
                                                ProvinceID = Convert.ToInt32(dr["SENDER_PROVINCE_ID"]),
                                                DistrictID = Convert.ToInt32(dr["SENDER_DISTRICT_ID"]),
                                                WardID = Convert.ToInt32(dr["SENDER_WARD_ID"]),
                                                Street = Convert.ToString(dr["SENDER_STREET"]),
                                                FullAddress = Convert.ToString(dr["SENDER_FULL_ADDRESS"]),
                                            },
                                            ReceiverInfo = new ZOrderBatchEditInfoReceiverInfoResponse()
                                            {
                                                FullName = Convert.ToString(dr["RECEIVER_NAME"]),
                                                MobileNumber = Convert.ToString(dr["RECEIVER_PHONE"]),
                                                ProvinceID = Convert.ToInt32(dr["RECEIVER_PROVINCE_ID"]),
                                                DistrictID = Convert.ToInt32(dr["RECEIVER_DISTRICT_ID"]),
                                                WardID = Convert.ToInt32(dr["RECEIVER_WARD_ID"]),
                                                Street = Convert.ToString(dr["RECEIVER_STREET"]),
                                                FullAddress = Convert.ToString(dr["RECEIVER_FULL_ADDRESS"]),
                                            },
                                            WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                            CollectionType = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                            TransportPayer = Convert.ToInt32(dr["TRANSPORT_PAYER"]),
                                            MainTransportServiceID = Convert.ToInt32(dr["TRANSPORT_MAIN_SERVICE"]),
                                            ExtraTransportServiceID = Convert.ToString(dr["TRANSPORT_EXTRA_SERVICE"]),
                                            TotalCOD = 0,
                                            Note = Convert.ToString(dr["NOTE"]),
                                            Channel = Convert.ToString(dr["CHANNEL"]),
                                            OrderStatus = OrderStatus
                                        };
                                    }
                                }
                            }

                            if (item != null)
                            {
                                item.TotalCOD = TotalCOD;
                                item.ListOrderInfo = ListOrderInfo;
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
                            }
                        }
                        else
                        {
                            switch (result)
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchController.EditInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZOrderBatchEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.EDIT_ORDER_BATCH", new object[]
                {
                    0,
                    req.BatchCode,
                    req.CustomerID,
                    req.SenderInfo.FullName,
                    req.SenderInfo.MobileNumber,
                    req.SenderInfo.ProvinceID,
                    //req.SenderInfo.DistrictID,
                    req.SenderInfo.WardID,
                    req.SenderInfo.Street,
                    req.SenderInfo.PickUpAddressID,
                    req.ReceiverInfo.FullName,
                    req.ReceiverInfo.MobileNumber,
                    req.ReceiverInfo.ProvinceID,
                    //req.ReceiverInfo.DistrictID,
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
                    req.UpdatedBy,
                    req.DeviceName,
                    req.ShopID,
                    0,
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
                        }
                        else
                        {
                            var ProductVariantName = dbCommand.Parameters["P_OUT_PRODUCT_VARIANT_NAME"].Value.ToString();
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại.";
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
                                    res.Message = string.Format("Không tìm thấy sản phẩm {0} trong kho.", ProductVariantName);
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
                                    res.Message = "Trạng thái không hợp lệ.";
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
                                    res.Code = "21";
                                    res.Message = "Thao tác sửa đơn hàng không hợp lệ.";
                                    break;
                                case -22:
                                    res.Code = "22";
                                    res.Message = "Thay đổi số lượng của Pallet thất bại.";
                                    break;
                                case -23:
                                    res.Code = "23";
                                    res.Message = "Trạng thái phiếu xuất kho không hợp lệ.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("CoppyInfo")]
        public ZResponse CoppyInfo(ZOrderBatchCoppyInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                long TotalCOD = 0;
                ZOrderBatchCoppyInfoResponse item = null;
                List<ZOrderBatchCoppyInfoOrderInfoResponse> ListOrderInfo = new List<ZOrderBatchCoppyInfoOrderInfoResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.COPPY_ORDER_BATCH_INFO", new object[]
                {
                    req.BatchCode,    //P_SHIPPING_CODE NUMBER
                    req.ShopID,       //P_SHOP_ID       NUMBER
                    0,                //P_RETURN_CODE   OUT NUMBER
                    0                 //P_OUT_CURSOR    OUT T_CURSOR
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
                                    if (Convert.ToInt64(dr["TOTAL_COD"]) > 0)
                                    {
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]);
                                    }

                                    List<ZOrderBatchCoppyInfoOrderInfoProductInfoResponse> ListProductInfo = new List<ZOrderBatchCoppyInfoOrderInfoProductInfoResponse>();
                                    var resOrderItem = OrderItemRepository.GetOrderItemByOrderID(new ZOrderItemGetByOrderIDRequest() { OrderID = Convert.ToInt32(dr["ORDER_ID"]) });
                                    if (resOrderItem.Code == "00")
                                    {
                                        var ListProduct = JsonConvert.DeserializeObject<List<ZOrderItemGetByOrderIDResponse>>(resOrderItem.Data);
                                        foreach (var itemProduct in ListProduct)
                                        {
                                            ListProductInfo.Add(new ZOrderBatchCoppyInfoOrderInfoProductInfoResponse()
                                            {
                                                ProductID = itemProduct.ProductID,
                                                ProductName = itemProduct.ProductName,
                                                VariantID = itemProduct.VariantID,
                                                VariantName = itemProduct.VariantName,
                                                NumberOfProduct = itemProduct.NumberOfProduct,
                                                Price = itemProduct.Price,
                                                Weight = itemProduct.Weight,
                                                Discount = itemProduct.Discount
                                            });
                                        }
                                    }

                                    ListOrderInfo.Add(new ZOrderBatchCoppyInfoOrderInfoResponse()
                                    {
                                        OrderName = Convert.ToString(dr["ORDER_NAME"]),
                                        OrderQuantity = Convert.ToInt32(dr["ORDER_QUANTITY"]),
                                        OrderValue = Convert.ToInt32(dr["ORDER_VALUE"]),
                                        TransportFee = Convert.ToInt32(dr["TRANSPORT_FEE"]),
                                        TotalAmount = Convert.ToInt32(dr["TOTAL_AMOUNT"]),
                                        Length = Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"]),
                                        Width = Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"]),
                                        Height = Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"]),
                                        Weight = Convert.ToDouble(dr["TRANSPORT_WEIGHT"]),
                                        TotalPHU = Convert.ToInt64(dr["TOTAL_PHU"]),
                                        Message = Convert.ToString(dr["MESSAGE"]),
                                        ListProductInfo = ListProductInfo,
                                    });

                                    if (item == null)
                                    {
                                        item = new ZOrderBatchCoppyInfoResponse()
                                        {
                                            BuyerInfo = new ZOrderBatchCoppyInfoBuyerInfoResponse()
                                            {
                                                MobileNumber = Convert.ToString(dr["BUYER_MOBILE_NUMBER"]),
                                                FullName = Convert.ToString(dr["BUYER_NAME"]),
                                            },
                                            SenderInfo = new ZOrderBatchCoppyInfoSenderInfoResponse()
                                            {
                                                PickUpAddressID = Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"]),
                                                FullName = Convert.ToString(dr["SENDER_NAME"]),
                                                MobileNumber = Convert.ToString(dr["SENDER_PHONE"]),
                                                ProvinceID = Convert.ToInt32(dr["SENDER_PROVINCE_ID"]),
                                                DistrictID = Convert.ToInt32(dr["SENDER_DISTRICT_ID"]),
                                                WardID = Convert.ToInt32(dr["SENDER_WARD_ID"]),
                                                Street = Convert.ToString(dr["SENDER_STREET"]),
                                                FullAddress = Convert.ToString(dr["SENDER_FULL_ADDRESS"]),
                                            },
                                            ReceiverInfo = new ZOrderBatchCoppyInfoReceiverInfoResponse()
                                            {
                                                FullName = Convert.ToString(dr["RECEIVER_NAME"]),
                                                MobileNumber = Convert.ToString(dr["RECEIVER_PHONE"]),
                                                ProvinceID = Convert.ToInt32(dr["RECEIVER_PROVINCE_ID"]),
                                                DistrictID = Convert.ToInt32(dr["RECEIVER_DISTRICT_ID"]),
                                                WardID = Convert.ToInt32(dr["RECEIVER_WARD_ID"]),
                                                Street = Convert.ToString(dr["RECEIVER_STREET"]),
                                                FullAddress = Convert.ToString(dr["RECEIVER_FULL_ADDRESS"]),
                                            },
                                            WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
                                            CollectionType = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                            TransportPayer = Convert.ToInt32(dr["TRANSPORT_PAYER"]),
                                            MainTransportServiceID = Convert.ToInt32(dr["TRANSPORT_MAIN_SERVICE"]),
                                            ExtraTransportServiceID = Convert.ToString(dr["TRANSPORT_EXTRA_SERVICE"]),
                                            TotalCOD = 0,
                                            Note = Convert.ToString(dr["NOTE"]),
                                            Channel = Convert.ToString(dr["CHANNEL"]),
                                        };
                                    }
                                }
                            }

                            if (item != null)
                            {
                                item.TotalCOD = TotalCOD;
                                item.ListOrderInfo = ListOrderInfo;
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
                            }
                        }
                        else
                        {
                            switch (result)
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchController.CoppyInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("DetailByBatchCode")]
        public ZResponse DetailByBatchCode(ZOrderBatchDetailByBatchCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                long ProvisionalTotalCOD = 0;
                long ActualTotalCOD = 0;
                ZOrderBatchDetailByBatchCodeResponse item = null;
                List<ZOrderBatchDetailByBatchCodeOrderInfoResponse> ListOrderInfo = new List<ZOrderBatchDetailByBatchCodeOrderInfoResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_BATCH_PKG.DETAIL_ORDER_BATCH", new object[]
                {
                    req.BatchCode,    //P_SHIPPING_CODE NUMBER
                    req.ShopID,       //P_SHOP_ID       NUMBER
                    0,                //P_RETURN_CODE   OUT NUMBER
                    0                 //P_OUT_CURSOR    OUT T_CURSOR
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
                                    if (Convert.ToInt64(dr["PROVISIONAL_TOTAL_COD"]) > 0)
                                    {
                                        ProvisionalTotalCOD = Convert.ToInt64(dr["PROVISIONAL_TOTAL_COD"]);
                                        if (Convert.ToInt64(dr["ACTUAL_TOTAL_COD"]) > 0)
                                        {
                                            ActualTotalCOD = Convert.ToInt64(dr["ACTUAL_TOTAL_COD"]);
                                        }
                                    }

                                    List<ZOrderBatchDetailByBatchCodeOrderInfoProductInfoResponse> ListProductInfo = new List<ZOrderBatchDetailByBatchCodeOrderInfoProductInfoResponse>();
                                    Task ListProductInfoHandle = new Task(() =>
                                    {
                                        var resOrderItem = OrderItemRepository.GetOrderItemByOrderID(new ZOrderItemGetByOrderIDRequest() { OrderID = Convert.ToInt32(dr["ORDER_ID"]) });
                                        if (resOrderItem.Code == "00")
                                        {
                                            var ListProduct = JsonConvert.DeserializeObject<List<ZOrderItemGetByOrderIDResponse>>(resOrderItem.Data);
                                            foreach (var itemProduct in ListProduct)
                                            {
                                                ListProductInfo.Add(new ZOrderBatchDetailByBatchCodeOrderInfoProductInfoResponse()
                                                {
                                                    ProductName = itemProduct.ProductName,
                                                    VariantSKU = itemProduct.VariantSKU,
                                                    VariantName = itemProduct.VariantName,
                                                    NumberOfProduct = itemProduct.NumberOfProduct,
                                                    Price = itemProduct.Price,
                                                    Weight = itemProduct.Weight,
                                                    Discount = itemProduct.Discount,
                                                    TotalPrice = itemProduct.NumberOfProduct * itemProduct.Price
                                                });
                                            }
                                        }
                                    });

                                    List<ZRequestSupportHistorySearchListUpdateInformationResponse> ListRequestSupportInfo = new List<ZRequestSupportHistorySearchListUpdateInformationResponse>();
                                    Task ListRequestSupportInfoHandle = new Task(() =>
                                    {
                                        if (string.IsNullOrEmpty(Convert.ToString(dr["SHIPPING_CODE"])) == false)
                                        {
                                            var resListUpdateInformation = RequestSupportHistoryRepository.SearchListUpdateInformation(new ZRequestSupportHistorySearchListUpdateInformationRequest() { EShippingCode = Convert.ToString(dr["SHIPPING_CODE"]), ShopID = req.ShopID });
                                            if (resListUpdateInformation.Code == "00")
                                            {
                                                ListRequestSupportInfo = JsonConvert.DeserializeObject<List<ZRequestSupportHistorySearchListUpdateInformationResponse>>(resListUpdateInformation.Data);
                                            }
                                        }
                                    });

                                    var isSearchSignaruteAndImage = false;
                                    List<string> ListStatusForSearchSignaruteAndImage = new List<string>() { "F", "G", "H", "I", "U", "V", "Z" };
                                    if (ListStatusForSearchSignaruteAndImage.Contains(Convert.ToString(dr["ORDER_STATUS"])) == true)
                                    {
                                        isSearchSignaruteAndImage = true;
                                    }

                                    List<ZEMSSignaruteAndImageInformationResponse> ListEMSSignaruteAndImageInfo = new List<ZEMSSignaruteAndImageInformationResponse>();
                                    Task ListInformationSignaruteAndImageEMSHandle = new Task(() =>
                                    {
                                        if (isSearchSignaruteAndImage == true)
                                        {
                                            var resEMSInformationSignaruteAndImage = EMSRepository.EMSSignaruteAndImageInformation(new ZEMSSignaruteAndImageInformationRequest() { LadingCode = Convert.ToString(dr["SHIPPING_CODE"]) });
                                            if (resEMSInformationSignaruteAndImage.Code == "00")
                                            {
                                                ListEMSSignaruteAndImageInfo = JsonConvert.DeserializeObject<List<ZEMSSignaruteAndImageInformationResponse>>(resEMSInformationSignaruteAndImage.Data);
                                            }
                                        }
                                    });

                                    List<ZVNPSignaruteAndImageInformationResponse> ListVNPSignaruteAndImageInfo = new List<ZVNPSignaruteAndImageInformationResponse>();
                                    Task ListInformationSignaruteAndImageVNPHandle = new Task(() =>
                                    {
                                        if (isSearchSignaruteAndImage == true)
                                        {
                                            var resVNPInformationSignaruteAndImage = VNPRepository.VNPSignaruteAndImageInformation(new ZVNPSignaruteAndImageInformationRequest() { itemCode = Convert.ToString(dr["SHIPPING_CODE"]) });
                                            if (resVNPInformationSignaruteAndImage.Code == "00")
                                            {
                                                ListVNPSignaruteAndImageInfo = JsonConvert.DeserializeObject<List<ZVNPSignaruteAndImageInformationResponse>>(resVNPInformationSignaruteAndImage.Data);
                                            }
                                        }
                                    });

                                    ListProductInfoHandle.Start();
                                    ListRequestSupportInfoHandle.Start();
                                    ListInformationSignaruteAndImageEMSHandle.Start();
                                    ListInformationSignaruteAndImageVNPHandle.Start();

                                    Task.WaitAll(ListProductInfoHandle, ListRequestSupportInfoHandle, ListInformationSignaruteAndImageEMSHandle, ListInformationSignaruteAndImageVNPHandle);

                                    ListOrderInfo.Add(new ZOrderBatchDetailByBatchCodeOrderInfoResponse()
                                    {
                                        ShippingCode = Convert.ToString(dr["SHIPPING_CODE"]),
                                        EMSOneCode = Convert.ToString(dr["EMSONE_CODE"]),
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        OrderName = Convert.ToString(dr["ORDER_NAME"]),
                                        OrderStatus = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["ORDER_STATUS"]),
                                            Name = XData.GetNameOrderStatus(Convert.ToString(dr["ORDER_STATUS"])),
                                        },
                                        OrderQuantity = Convert.ToInt32(dr["ORDER_QUANTITY"]),
                                        OrderValue = Convert.ToInt32(dr["ORDER_VALUE"]),
                                        ProvisionalTransportFee = Convert.ToInt64(dr["PROVISIONAL_TRANSPORT_FEE"]),
                                        ActualTransportFee = Convert.ToInt64(dr["ACTUAL_TRANSPORT_FEE"]),

                                        Length = Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"]),
                                        Width = Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"]),
                                        Height = Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"]),
                                        ProvisionalTransportWeight = Convert.ToDouble(dr["PROVISIONAL_TRANSPORT_WEIGHT"]),
                                        ActualTransportWeight = Convert.ToDouble(dr["ACTUAL_TRANSPORT_WEIGHT"]),
                                        ExchangeTransportWeight = Convert.ToDouble(dr["EXCHANGE_TRANSPORT_WEIGHT"]),
                                        TotalPHU = Convert.ToInt64(dr["TOTAL_PHU"]),
                                        Message = Convert.ToString(dr["MESSAGE"]),
                                        AcceptancePosCode = Convert.ToString(dr["ACCEPTANCE_POSCODE"]),
                                        ReasonCancel = Convert.ToString(dr["REASON_CANCEL"]),
                                        UpdatedName = Convert.ToString(dr["UPDATED_NAME"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),

                                        TotalAmount = Convert.ToInt64(dr["TOTAL_AMOUNT"]),
                                        TotalProductQuantity = ListProductInfo.Sum(m => m.NumberOfProduct),
                                        TotalProductValue = Convert.ToInt64(dr["TOTAL_PRODUCT_VALUE"]),
                                        TotalOrderValue = Convert.ToInt64(dr["TOTAL_ORDER_VALUE"]),
                                        TotalDiscount = Convert.ToInt64(dr["TOTAL_DISCOUNT"]),
                                        ListProductInfo = ListProductInfo,
                                        ListRequestSupportInfo = ListRequestSupportInfo,
                                        ListEMSSignaruteAndImageInfo = ListEMSSignaruteAndImageInfo,
                                        ListVNPSignaruteAndImageInfo = ListVNPSignaruteAndImageInfo
                                    });

                                    if (item == null)
                                    {
                                        item = new ZOrderBatchDetailByBatchCodeResponse()
                                        {
                                            BuyerInfo = new ZOrderBatchDetailByBatchCodeBuyerInfoResponse()
                                            {
                                                FullName = Convert.ToString(dr["BUYER_NAME"]),
                                                MobileNumber = Convert.ToString(dr["BUYER_PHONE"]),
                                            },
                                            ReceiverInfo = new ZOrderBatchDetailByBatchCodeReceiverInfoResponse()
                                            {
                                                FullName = Convert.ToString(dr["RECEIVER_NAME"]),
                                                MobileNumber = Convert.ToString(dr["RECEIVER_PHONE"]),
                                                FullAddress = Convert.ToString(dr["RECEIVER_FULL_ADDRESS"]),
                                            },
                                            WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                            CollectionType = new ZBaseResponse()
                                            {
                                                Code = Convert.ToString(dr["COLLECTION_TYPE"]),
                                                Name = XData.GetNameCollectionType(Convert.ToString(dr["COLLECTION_TYPE"])),
                                            },
                                            TransportPayer = new ZBaseResponse()
                                            {
                                                Code = Convert.ToString(dr["TRANSPORT_PAYER"]),
                                                Name = XData.GetNameTransportPayer(Convert.ToString(dr["TRANSPORT_PAYER"])),
                                            },
                                            MainTransportServiceID = new ZBaseResponse()
                                            {
                                                Code = Convert.ToString(dr["TRANSPORT_MAIN_SERVICE_ID"]),
                                                Name = Convert.ToString(dr["TRANSPORT_MAIN_SERVICE_NAME"])
                                            },
                                            ExtraTransportServiceID = new ZBaseResponse()
                                            {
                                                Code = Convert.ToString(dr["TRANSPORT_EXTRA_SERVICE_ID"]),
                                                Name = Convert.ToString(dr["TRANSPORT_EXTRA_SERVICE_NAME"])
                                            },
                                            ProvisionalTotalCOD = 0,
                                            ActualTotalCOD = 0,
                                            Note = Convert.ToString(dr["NOTE"]),
                                            Channel = new ZBaseResponse()
                                            {
                                                Code = Convert.ToString(dr["CHANNEL"]),
                                                Name = XData.GetNameChannel(Convert.ToString(dr["CHANNEL"]))
                                            },
                                            DeviceName = Convert.ToString(dr["DEVICE_NAME"]),
                                        };

                                        if (string.IsNullOrEmpty(item.DeviceName) || item.DeviceName == "WEB")
                                        {
                                            item.PlaceOfCreation = "Web EMSOne";
                                        }
                                        else if (item.DeviceName.ToUpper() == "EMSENTERPRISE")
                                        {
                                            item.PlaceOfCreation = "EMSEnterprise";
                                        }
                                        else if (item.DeviceName.ToUpper() == "ANDROID" || item.DeviceName.ToUpper() == "IOS")
                                        {
                                            item.PlaceOfCreation = "App EMSOne";
                                        }
                                        else if (item.DeviceName == "PARTNER")
                                        {
                                            item.PlaceOfCreation = "API Đối tác";
                                        }
                                    }
                                }
                            }

                            if (item != null)
                            {
                                item.ProvisionalTotalCOD = ProvisionalTotalCOD;
                                item.ActualTotalCOD = ActualTotalCOD;
                                item.ListOrderInfo = ListOrderInfo;

                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
                            }
                        }
                        else
                        {
                            switch (result)
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchController.DetailByBatchCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
    }
}