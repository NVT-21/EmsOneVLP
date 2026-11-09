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
    [RoutePrefix("Order")]
    public class OrderController : ApiController
    {
        [Route("SearchProduct")]
        public ZResponse SearchProduct(ZOrderSearchProductRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = OrderRepository.SearchProduct(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.SearchProduct: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("Add")]
        //public ZResponse Add(ZOrderAddRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    ZResponse resAddInventory = new ZResponse();
        //    try
        //    {
        //        var resAddOrder = OrderRepository.AddOrder(req);
        //        if (resAddOrder.Code == "00")
        //        {
        //            var OrderID = Convert.ToInt32(resAddOrder.Data);
        //            if (req.Status == "D")
        //            {
        //                //Trạng thái đơn hàng là nháp nên không cần tạo phiếu xuất kho
        //                resAddInventory.Code = "00";
        //                resAddInventory.Message = "Thành công";
        //                resAddInventory.Data = "0";
        //            }
        //            else
        //            {
        //                if (req.OrderedProducts != null && req.OrderedProducts.Count > 0)
        //                {
        //                    if (!string.IsNullOrEmpty(req.DeliveryNoteCode))
        //                    {
        //                        resAddInventory = InventoryRepository.Edit(new ZInventoryEditRequest()
        //                        {
        //                            ID = req.DeliveryNoteID,
        //                            WarehouseID = req.WareHouseID,
        //                            OrderID = OrderID,
        //                            Type = "O",
        //                            ReceiverName = req.ShipmentDetails.ReceiverName,
        //                            MobileNumber = req.ShipmentDetails.MobileNumber,
        //                            UpdatedBy = req.CreatedBy,
        //                            ShopID = req.ShopID,
        //                            ListInventoryDetail = req.ListInventoryDetail
        //                        });
        //                    }
        //                    else
        //                    {
        //                        resAddInventory = InventoryRepository.Add(new ZInventoryAddRequest()
        //                        {
        //                            OrderID = OrderID,
        //                            ShopID = req.ShopID,
        //                            WarehouseID = req.WareHouseID,
        //                            CreatedBy = req.CreatedBy,
        //                            Type = "O", //Tạo phiếu xuất kho
        //                            ReceiverName = req.ShipmentDetails.ReceiverName,
        //                            MobileNumber = req.ShipmentDetails.MobileNumber,
        //                            ListInventoryDetail = req.ListInventoryDetail
        //                        });
        //                    }
        //                }
        //                else
        //                {
        //                    resAddInventory.Code = "00";
        //                    resAddInventory.Message = "Thành công";
        //                    resAddInventory.Data = "0";
        //                }
        //            }


        //            if (resAddInventory.Code == "00")
        //            {
        //                var InventoryID = Convert.ToInt32(resAddInventory.Data);
        //                var resUpdateAmndStateOrder = OrderRepository.UpdateAmndState(new ZOrderUpdateAmndStateRequest()
        //                {
        //                    OrderID = OrderID,
        //                    AmndState = "A",
        //                    IsRequestOrder = req.isRequestOrder,
        //                    RequestOrderID = req.RequestOrderID,
        //                    IsUpdatedCustomer = req.isUpdatedCustomer,
        //                    CustomerID = req.CustomerID,
        //                    CustomerFullName = req.ShipmentDetails.ReceiverName,
        //                    CustomerProvinceID = req.ShipmentDetails.ProvinceID,
        //                    CustomerDistrictID = req.ShipmentDetails.DistrictID,
        //                    CustomerWardID = req.ShipmentDetails.WardID,
        //                    CustomerAddress = req.ShipmentDetails.Address,
        //                    ShopID = req.ShopID,
        //                    UpdatedBy = req.CreatedBy
        //                });
        //                if (resUpdateAmndStateOrder.Code == "00")
        //                {
        //                    if (InventoryID > 0 && req.Transport == "N")
        //                    {
        //                        var resInventoryConfirm = InventoryRepository.Confirm(new ZInventoryConfirmRequest()
        //                        {
        //                            ID = InventoryID,
        //                            ShopID = req.ShopID,
        //                            UpdatedBy = req.CreatedBy
        //                        });
        //                        if (resInventoryConfirm.Code == "00")
        //                        {
        //                            res.Code = "00";
        //                            res.Message = "Tạo đơn hàng thành công";
        //                            res.Data = resUpdateAmndStateOrder.Data;
        //                        }
        //                        else
        //                        {
        //                            resUpdateAmndStateOrder = OrderRepository.UpdateAmndState(new ZOrderUpdateAmndStateRequest()
        //                            {
        //                                OrderID = OrderID,
        //                                AmndState = "I",
        //                                IsRequestOrder = "N",
        //                                IsUpdatedCustomer = "N"
        //                            });
        //                            res.Code = resInventoryConfirm.Code;
        //                            res.Message = resInventoryConfirm.Message;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Tạo đơn hàng thành công";
        //                        res.Data = resUpdateAmndStateOrder.Data;
        //                    }
        //                }
        //                else
        //                {
        //                    res.Code = resUpdateAmndStateOrder.Code;
        //                    res.Message = resUpdateAmndStateOrder.Message;
        //                }
        //            }
        //            else
        //            {
        //                res.Code = resAddInventory.Code;
        //                res.Message = resAddInventory.Message;
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resAddOrder.Code;
        //            res.Message = resAddOrder.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.Add: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("AddV2")]
        public ZResponse AddV2(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = OrderRepository.AddOrderV2(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.AddV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("AddOrderAndSendTransport")]
        //public ZResponse AddOrderAndSendTransport(ZOrderAddRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        if (req.Transport == "N")
        //        {
        //            res.Code = "01";
        //            res.Message = "Đơn hàng không có vận chuyển, Vui lòng kiểm tra lại.";
        //            return res;
        //        }
        //        var resAddOrderAndAcceptInventory = OrderRepository.AddOrderAndAcceptInventory(req);
        //        if (resAddOrderAndAcceptInventory.Code == "00")
        //        {
        //            var resSearchInfoSendRequestTransport = OrderRepository.SearchInfoSendRequestTransport(new ZOrderSearchInfoSendRequestTransportRequest()
        //            {
        //                EMSOneCode = resAddOrderAndAcceptInventory.Data,
        //                ShopID = req.ShopID
        //            });
        //            if (resSearchInfoSendRequestTransport.Code == "00")
        //            {
        //                var dataSearchInfo = JsonConvert.DeserializeObject<ZOrderSearchInfoSendRequestTransportResponse>(resSearchInfoSendRequestTransport.Data);
        //                res = XCommon.SendRequestTransport(dataSearchInfo, req.CreatedBy);
        //            }
        //            else
        //            {
        //                res.Code = resSearchInfoSendRequestTransport.Code;
        //                res.Message = resSearchInfoSendRequestTransport.Message;
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resAddOrderAndAcceptInventory.Code;
        //            res.Message = resAddOrderAndAcceptInventory.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.AddOrderAndSendTransport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("AddOrderAndSendTransportV2")]
        public ZResponse AddOrderAndSendTransportV2(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = OrderRepository.AddOrderAndSendTransportV2(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.AddOrderAndSendTransportV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZOrderEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                if (req.ShipmentDetails == null)
                {
                    req.ShipmentDetails = new ZOrderEditShipmentInfoRequest();
                }

                if (req.TransportPayer == 0)
                {
                    req.TransportPayer = 1;
                }

                if (req.TransportQuantity == 0)
                {
                    req.TransportQuantity = 1;
                }

                if (req.TransportWeight == 0)
                {
                    req.TransportWeight = 100;
                }

                req.OrderCode = string.IsNullOrEmpty(req.OrderCode) == true ? "" : req.OrderCode.ToUpper();

                // Dịch vụ PPA thì mặc định là người nhận trả -- tạm thời bỏ
                //if (!string.IsNullOrEmpty(req.TransportExtraService))
                //{
                //    //req.TransportPayer = req.TransportExtraService.Contains("221") == true ? 2 : req.TransportPayer;
                //    if (req.TransportExtraService.Contains("108") == true && req.TotalCOD > 50000000 && string.IsNullOrEmpty(req.Status))
                //    {
                //        res.Code = "09";
                //        res.Message = "Số tiền COD tối đa 50 triệu đồng/BG.";
                //        return res;
                //    }
                //}

                //if (req.TransportOrderValue > 100000000 && string.IsNullOrEmpty(req.Status))
                //{
                //    res.Code = "10";
                //    res.Message = "Giá trị đơn hàng không vượt quá 100 triệu đồng.";
                //    return res;
                //}

                if (req.OrderedProducts == null || req.OrderedProducts.Count == 0)
                {
                    req.OrderedProducts = new List<ZOrderEditItemRequest>();
                    if (req.TotalCOD > 0)
                    {
                        req.TotalAmount = req.TotalCOD;
                    }
                    else
                    {
                        req.TotalAmount = req.TransportOrderValue;
                    }
                }

                double ExchangeTransportWeight = XCommon.ExchangeTransportWeight(new ZTransportExchangeTransportWeightRequest()
                {
                    TransportMainServiceID = req.TransportMainService,
                    TransportSizeLength = req.TransportSizeLength,
                    TransportSizeWidth = req.TransportSizeWidth,
                    TransportSizeHeight = req.TransportSizeHeight,
                    TransportWeight = 0
                });

                //double ExchangeTransportWeight = 0;
                //if (req.TransportSizeLength > 0 && req.TransportSizeWidth > 0 && req.TransportSizeHeight > 0)
                //{
                //    var convertTransportWeight = Math.Round((req.TransportSizeLength * req.TransportSizeWidth * req.TransportSizeHeight) / 6);
                //    //TMD - TMD_B
                //    if (req.TransportMainService == 141 || req.TransportMainService == 360)
                //    {
                //        List<double> ListLWH = new List<double>() { req.TransportSizeLength, req.TransportSizeWidth, req.TransportSizeHeight };
                //        var getMax = ListLWH.Max();
                //        if (getMax > 50)
                //        {
                //            ExchangeTransportWeight = convertTransportWeight;
                //        }
                //    }
                //    else
                //    {
                //        if (convertTransportWeight > req.TransportWeight)
                //        {
                //            ExchangeTransportWeight = convertTransportWeight;
                //        }
                //    }
                //}

                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.ORDER_EDIT", new object[]
                {
                    0,
                    req.EMSOneCode,
                    req.OrderCode,                 //P_ORDER_CODE         VARCHAR,
                    req.TotalAmount,                         //P_TOTAL_AMOUNT       NUMBER,
                    req.TotalDiscount,                       //P_TOTAL_DISCOUNT     NUMBER,
                    req.TotalCOD,                            //P_TOTAL_COD          NUMBER,
                    req.TotalPHU,                            //P_TOTAL_PHU          NUMBER,
                    req.Channel,                             //P_CHANNEL            NUMBER,
                    req.Note,                                //P_NOTE               VARCHAR,
                    req.Transport,                           //P_TRANSPORT          VARCHAR,
                    req.Message,                             //P_MESSAGE             VARCHAR,
                    req.Status,
                    req.TransportOrderName,
                    req.TransportWeight,
                    ExchangeTransportWeight,
                    req.TransportSizeLength,
                    req.TransportSizeWidth,
                    req.TransportSizeHeight,
                    req.TransportQuantity,
                    req.TransportOrderValue,
                    req.TransportMainService,
                    req.TransportExtraService,
                    req.TransportFee,                        //P_TRANSPORT_FEE      NUMBER,
                    req.TransportPayer,
                    req.CollectionType,
                    req.WareHouseID,
                    req.OrderedProducts.ToXML(),
                    req.CustomerID,                          //P_CUSTOMER_ID        NUMBER,
                    req.ShipmentDetails.ReceiverName,        //P_RECEIVER_NAME      VARCHAR,
                    req.ShipmentDetails.MobileNumber,        //P_MOBILE_NUMBER      VARCHAR,
                    req.ShipmentDetails.ProvinceID,          //P_PROVINCE_ID        NUMBER,
                    req.ShipmentDetails.DistrictID,          //P_DISTRICT           NUMBER,
                    req.ShipmentDetails.WardID,              //P_WARD_ID            NUMBER,
                    req.ShipmentDetails.Address,             //P_ADDRESS            VARCHAR,
                    //req.ShipmentDetails.FullAddress,       //P_FULL_ADDRESS       VARCHAR,
                    req.ShipmentDetails.PickUpAddressID,     //P_PICK_UP_ADDRESS_ID NUMBER
                    req.ShipmentDetails.PickupFullName,      //SENDER_NAME          VARCHAR
                    req.ShipmentDetails.PickupMobileNumber,  //SENDER_PHONE         VARCHAR
                    req.ShipmentDetails.PickupProvinceID,    //SENDER_PROVINCE_ID   NUMBER
                    req.ShipmentDetails.PickupDistrictID,    //SENDER_DISTRICT_ID   NUMBER
                    req.ShipmentDetails.PickupWardID,        //SENDER_WARD_ID       NUMBER
                    req.ShipmentDetails.PickupStreet,        //SENDER_STREET        VARCHAR
                    //req.ShipmentDetails.PickupFullAddress, //SENDER_FULL_ADDRESS  VARCHAR
                    req.ShopID,                              //P_SHOP_ID            NUMBER,
                    req.CreatedBy,                           //P_CREATED_BY         NUMBER,
                    req.DeviceName,                          //P_DEVICE_NAME        VARCHAR
                    req.isUpdatedCustomer,
                    req.ExpectedDeliveryDate,
                    req.CollectionPostOfficeRecentID,
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
                            res.Message = "Sửa đơn hàng thành công";
                        }
                        else
                        {
                            var ProductVariantName = dbCommand.Parameters["P_OUT_PRODUCT_VARIANT_NAME"].Value.ToString();
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Đơn hàng không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái đơn hàng không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Trạng thái phiếu xuất kho không hợp lệ.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Hoàn trả sản phẩm vào kho thất bại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Khách hàng không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Dịch vụ chính CPN TMĐT Nhanh tiết kiệm chỉ áp dụng cho liên tỉnh - liên vùng. Vui lòng kiểm tra lại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Dịch vụ EMS Thỏa thuận không áp dụng cho nội tỉnh. Vui lòng kiểm tra lại.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Thiếu thông tin người gửi.";
                                    break;
                                case -9:
                                    res.Code = "09";
                                    res.Message = "Mã đơn hàng đã tồn tại.";
                                    break;
                                case -10:
                                    res.Code = "10";
                                    res.Message = "Sản phẩm không tồn tại.";
                                    break;
                                case -11:
                                    res.Code = "11";
                                    res.Message = string.Format("Giá bán của sản phẩm {0} đã thay đổi.", ProductVariantName); ;
                                    break;
                                case -12:
                                    res.Code = "12";
                                    res.Message = string.Format("Sản phẩm {0} chưa được nhập kho", ProductVariantName);
                                    break;
                                case -13:
                                    res.Code = "13";
                                    res.Message = string.Format("Số lượng của sản phẩm {0} trong kho không đủ lập đơn hàng.", ProductVariantName);
                                    break;
                                case -14:
                                    res.Code = "14";
                                    res.Message = "Thêm sản phẩm vào đơn hàng thất bại.";
                                    break;
                                case -15:
                                    res.Code = "15";
                                    res.Message = "Thêm sản phẩm vào phiếu xuất kho thất bại.";
                                    break;
                                case -16:
                                    res.Code = "16";
                                    res.Message = string.Format("Sản phẩm {0} không tồn tại trong kho.", ProductVariantName);
                                    break;
                                case -17:
                                    res.Code = "17";
                                    res.Message = string.Format("Sản phẩm {0} trong kho không đủ.", ProductVariantName);
                                    break;
                                case -18:
                                    res.Code = "18";
                                    res.Message = string.Format("Sản phẩm {0} trong kho đã hết.", ProductVariantName);
                                    break;
                                case -19:
                                    res.Code = "19";
                                    res.Message = "Lưu lịch sử phiếu xuất kho thất bại.";
                                    break;
                                case -20:
                                    res.Code = "20";
                                    res.Message = "Lưu lịch sử đơn hàng thất bại.";
                                    break;
                                case -21:
                                    res.Code = "21";
                                    res.Message = "Dịch vụ Thương mại điện tử Đồng giá Nội tỉnh chỉ áp dụng cho nội tỉnh Thành phố Hồ Chí Minh. Vui lòng kiểm tra lại.";
                                    break;
                                case -22:
                                    res.Code = "22";
                                    res.Message = "Gói cước Dịch vụ vận chuyển chính không hợp lệ.";
                                    break;
                                case -23:
                                    res.Code = "23";
                                    res.Message = "Hoàn trả sản phẩm vào Pallet thất bại.";
                                    break;
                                case -24:
                                    res.Code = "24";
                                    res.Message = "Thay đổi số lượng của Pallet thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("SendTransport")]
        //public ZResponse SendTransport(ZOrderSendTransportRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var resUpdateSendTransportData = OrderRepository.SendTransportData(new ZOrderSendTransportDataRequest() { OrderID = 0, EMSOneCode = req.EMSOneCode, ShopID = req.ShopID, UpdatedBy = req.UpdatedBy });
        //        if (resUpdateSendTransportData.Code == "00")
        //        {
        //            var resSendTransport = OrderRepository.SendTransport(new ZOrderSendTransportRequest() { AmndState = "A", OrderID = Convert.ToInt32(resUpdateSendTransportData.Data), ShopID = req.ShopID, CreatedBy = req.UpdatedBy });
        //            if (resSendTransport.Code == "00")
        //            {
        //                res.Code = "00";
        //                res.Message = "Thành công";
        //            }
        //            else
        //            {
        //                res.Code = resSendTransport.Code;
        //                res.Message = resSendTransport.Message;
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resUpdateSendTransportData.Code;
        //            res.Message = resUpdateSendTransportData.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.SendTransport: " + ex.Message);
        //    }
        //    return res;
        //}

        [Route("SendTransport")]
        public ZResponse SendTransport(ZOrderMultipleSendTransportRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (req.ListOrderInfo.Count > 0)
                {
                    var ListOrderStatusInvalid = req.ListOrderInfo.Where(m => m.OrderStatus != "N").ToList();
                    var ListOrder = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == true && m.OrderStatus == "N").ToList();
                    var ListOrderBatch = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == false && m.OrderStatus == "N").ToList();
                    List<ZOrderMultipleSendTransportOrderInfoRequest> ListGroupByOrderBatch = new List<ZOrderMultipleSendTransportOrderInfoRequest>();
                    List<ZErrorDataListResponse> ListErrorData = new List<ZErrorDataListResponse>();

                    Task ListOrderStatusInvalidHandle = new Task(() =>
                    {
                        if (ListOrderStatusInvalid.Count > 0)
                        {
                            foreach (var item in ListOrderStatusInvalid)
                            {
                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = string.Format("Đơn hàng đang ở trạng thái {0}, không được gửi vận chuyển.", XData.GetNameOrderStatus(item.OrderStatus)) });
                            }
                        }
                    });

                    Task ListOrderHandle = new Task(() =>
                    {
                        if (ListOrder.Count > 0)
                        {
                            foreach (var item in ListOrder)
                            {
                                var resUpdateSendTransportData = OrderRepository.SendTransportData(new ZOrderSendTransportDataRequest() { OrderID = item.OrderID, EMSOneCode = "", ShopID = req.ShopID, UpdatedBy = req.UpdatedBy });
                                if (resUpdateSendTransportData.Code == "00")
                                {
                                    var resSendTransport = OrderRepository.SendTransport(new ZOrderSendTransportRequest() { AmndState = "A", OrderID = item.OrderID, ShopID = req.ShopID, CreatedBy = req.UpdatedBy });
                                    if (resSendTransport.Code != "00")
                                    {
                                        ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = resSendTransport.Message });
                                    }
                                }
                                else
                                {
                                    ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = resUpdateSendTransportData.Message });
                                }
                            }
                        }
                    });

                    Task ListOrderBatchHandle = new Task(() =>
                    {
                        if (ListOrderBatch.Count > 0)
                        {
                            ListGroupByOrderBatch = ListOrderBatch.GroupBy(x => x.BatchCode).Select(x => x.First()).ToList();
                            foreach (var item in ListGroupByOrderBatch)
                            {
                                var resUpdateSendTransportData = OrderBatchRepository.SendTransportData(new ZOrderBatchSendTransportDataRequest() { BatchCode = item.BatchCode, ShopID = req.ShopID, UpdatedBy = req.UpdatedBy });
                                if (resUpdateSendTransportData.Code == "00")
                                {
                                    var resSendTransport = OrderBatchRepository.SendTransport(new ZOrderBatchSendTransportRequest() { AmndState = "A", BatchCode = item.BatchCode, ShopID = req.ShopID, CreatedBy = req.UpdatedBy });
                                    if (resSendTransport.Code != "00")
                                    {
                                        ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = "", Description = resSendTransport.Message });
                                    }
                                }
                                else if (resUpdateSendTransportData.Code == "90")
                                {
                                    var ListErrorOrder = JsonConvert.DeserializeObject<List<ZOrderBatchSendTransportDataResponse>>(resUpdateSendTransportData.Data);
                                    foreach (var ItemOrderError in ListErrorOrder)
                                    {
                                        ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = XCommon.ConvertToOrderCode(new ZOrderConvertToOrderCodeRequest() { OrderID = ItemOrderError.OrderID, ShopID = req.ShopID }), Description = ItemOrderError.Description });
                                    }
                                }
                                else
                                {
                                    ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = "", Description = resUpdateSendTransportData.Message });
                                }
                            }
                        }
                    });

                    ListOrderStatusInvalidHandle.Start();
                    ListOrderHandle.Start();
                    ListOrderBatchHandle.Start();

                    Task.WaitAll(ListOrderStatusInvalidHandle, ListOrderHandle, ListOrderBatchHandle);

                    if (ListErrorData.Count == 0)
                    {
                        res.Code = "00";
                        res.Message = "Gửi đơn vị vận chuyển thành công.";
                    }
                    else
                    {
                        res.Code = "10";
                        res.Message = "Gửi đơn vị vận chuyển thất bại, vui lòng xem chi tiết.";
                        res.Data = JsonConvert.SerializeObject(ListErrorData);
                    }
                }
                else if (string.IsNullOrEmpty(req.EMSOneCode) == false)
                {
                    var resUpdateSendTransportData = OrderRepository.SendTransportData(new ZOrderSendTransportDataRequest() { OrderID = 0, EMSOneCode = req.EMSOneCode, ShopID = req.ShopID, UpdatedBy = req.UpdatedBy });
                    if (resUpdateSendTransportData.Code == "00")
                    {
                        var resSendTransport = OrderRepository.SendTransport(new ZOrderSendTransportRequest() { AmndState = "A", OrderID = Convert.ToInt32(resUpdateSendTransportData.Data), ShopID = req.ShopID, CreatedBy = req.UpdatedBy });
                        if (resSendTransport.Code == "00")
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
                        }
                        else
                        {
                            res.Code = resSendTransport.Code;
                            res.Message = resSendTransport.Message;
                        }
                    }
                    else
                    {
                        res.Code = resUpdateSendTransportData.Code;
                        res.Message = resUpdateSendTransportData.Message;
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn đơn hàng cần xử lý.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.SendTransport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("CancelTransport")]
        //public ZResponse CancelTransport(ZOrderCancelTransportRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        //res = OrderRepository.CancelTransport(req);
        //        var resUpdateCancelTransportData = OrderRepository.CancelTransportData(new ZOrderCancelTransportDataRequest() { OrderID = req.OrderID, ShopID = req.ShopID });
        //        if(resUpdateCancelTransportData.Code == "00")
        //        {
        //            var resCancelTransport = OrderRepository.CancelTransport(new ZOrderCancelTransportRequest() { OrderID =req.OrderID, ShopID =req.ShopID, UpdatedBy = req.UpdatedBy });
        //            if (resCancelTransport.Code == "00")
        //            {
        //                res.Code = "00";
        //                res.Message = "Thành công";
        //            }
        //            else
        //            {
        //                res.Code = resCancelTransport.Code;
        //                res.Message = resCancelTransport.Message;
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resUpdateCancelTransportData.Code;
        //            res.Message = resUpdateCancelTransportData.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.CancelTransport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}

        [Route("CancelTransport")]
        public ZResponse CancelTransport(ZOrderMultipleCancelTransportRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {

                if (req.ListOrderInfo.Count > 0)
                {
                    var ListOrderStatusInvalid = req.ListOrderInfo.Where(m => m.OrderStatus != "W" && m.OrderStatus != "P").ToList();
                    var ListOrder = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == true && (m.OrderStatus == "W" || m.OrderStatus == "P")).ToList();
                    var ListOrderBatch = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == false && (m.OrderStatus == "W" || m.OrderStatus == "P")).ToList();
                    List<ZErrorDataListResponse> ListErrorData = new List<ZErrorDataListResponse>();
                    List<ZOrderMultipleCancelTransportOrderInfoRequest> ListGroupByOrderBatch = new List<ZOrderMultipleCancelTransportOrderInfoRequest>();

                    Task ListOrderStatusInvalidHandle = new Task(() =>
                    {
                        if (ListOrderStatusInvalid.Count > 0)
                        {
                            foreach (var item in ListOrderStatusInvalid)
                            {
                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = string.Format("Đơn hàng đang ở trạng thái {0}, không được hủy vận chuyển.", XData.GetNameOrderStatus(item.OrderStatus)) });
                            }
                        }
                    });

                    Task ListOrderHandle = new Task(() =>
                    {
                        if (ListOrder.Count > 0)
                        {
                            foreach (var item in ListOrder)
                            {
                                var resCancelTransportInfo = OrderRepository.CancelTransportInfo(new ZOrderCancelTransportInfoRequest()
                                {
                                    OrderID = item.OrderID,
                                    ShopID = req.ShopID
                                });

                                if (resCancelTransportInfo.Code == "00")
                                {
                                    var ItemOrderTransportInfo = JsonConvert.DeserializeObject<ZOrderCancelTransportInfoResponse>(resCancelTransportInfo.Data);

                                    if (ItemOrderTransportInfo.Status == "W" || ItemOrderTransportInfo.Status == "P")
                                    {
                                        var resCancelTransport = OrderRepository.CancelTransport(new ZOrderCancelTransportRequest()
                                        {
                                            OrderID = ItemOrderTransportInfo.OrderID,
                                            ShopID = req.ShopID,
                                            CrmOrPaypostCode = ItemOrderTransportInfo.CrmOrPaypostCode,
                                            ShippingCode = ItemOrderTransportInfo.ShippingCode,
                                            UpdatedBy = req.UpdatedBy
                                        });

                                        if (resCancelTransport.Code != "00")
                                        {
                                            ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = ItemOrderTransportInfo.OrderCode, Description = resCancelTransport.Message });
                                        }
                                    }
                                    else
                                    {
                                        ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = ItemOrderTransportInfo.OrderCode, Description = "Trạng thái không hợp lệ." });
                                    }
                                }
                                else
                                {
                                    ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = resCancelTransportInfo.Message });
                                }
                            }
                        }
                    });

                    Task ListOrderBatchHandle = new Task(() =>
                    {
                        if (ListOrderBatch.Count > 0)
                        {
                            ListGroupByOrderBatch = ListOrderBatch.GroupBy(x => x.BatchCode).Select(x => x.First()).ToList();
                            foreach (var item in ListGroupByOrderBatch)
                            {
                                var resCancelTransportInfo = OrderBatchRepository.CancelTransportInfo(new ZOrderBatchCancelTransportInfoRequest()
                                {
                                    BatchCode = item.BatchCode,
                                    ShopID = req.ShopID
                                });

                                if (resCancelTransportInfo.Code == "00")
                                {
                                    var FullOrderInOrderBatch = JsonConvert.DeserializeObject<List<ZOrderBatchCancelTransportInfoResponse>>(resCancelTransportInfo.Data);

                                    foreach (var ItemOrderInOrderBatch in FullOrderInOrderBatch)
                                    {
                                        if (ItemOrderInOrderBatch.OrderStatus == "W" || ItemOrderInOrderBatch.OrderStatus == "P")
                                        {
                                            var resCancelTransport = OrderBatchRepository.CancelTransport(new ZOrderBatchCancelTransportRequest()
                                            {
                                                OrderID = ItemOrderInOrderBatch.OrderID,
                                                ShopID = req.ShopID,
                                                CrmOrPaypostCode = ItemOrderInOrderBatch.CrmOrPaypostCode,
                                                ShippingCode = ItemOrderInOrderBatch.ShippingCode,
                                                UpdatedBy = req.UpdatedBy
                                            });

                                            if (resCancelTransport.Code != "00")
                                            {
                                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = ItemOrderInOrderBatch.OrderCode, Description = resCancelTransport.Message });
                                            }
                                        }
                                        else
                                        {
                                            ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = ItemOrderInOrderBatch.OrderCode, Description = "Trạng thái không hợp lệ." });
                                        }
                                    }
                                }
                                else
                                {
                                    ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = "", Description = resCancelTransportInfo.Message });
                                }
                            }
                        }
                    });

                    ListOrderStatusInvalidHandle.Start();
                    ListOrderHandle.Start();
                    ListOrderBatchHandle.Start();

                    Task.WaitAll(ListOrderStatusInvalidHandle, ListOrderHandle, ListOrderBatchHandle);

                    if (ListErrorData.Count == 0)
                    {
                        res.Code = "00";
                        res.Message = "Hủy yêu cầu vận chuyển thành công.";
                    }
                    else
                    {
                        res.Code = "10";
                        res.Message = "Hủy yêu cầu vận chuyển thất bại, vui lòng xem chi tiết.";
                        res.Data = JsonConvert.SerializeObject(ListErrorData);
                    }
                }
                else if (req.OrderID > 0)
                {
                    var resCancelTransportInfo = OrderRepository.CancelTransportInfo(new ZOrderCancelTransportInfoRequest()
                    {
                        OrderID = req.OrderID,
                        ShopID = req.ShopID
                    });

                    if (resCancelTransportInfo.Code == "00")
                    {
                        var ItemOrderTransportInfo = JsonConvert.DeserializeObject<ZOrderCancelTransportInfoResponse>(resCancelTransportInfo.Data);

                        if (ItemOrderTransportInfo.Status == "W" || ItemOrderTransportInfo.Status == "P")
                        {
                            var resCancelTransport = OrderRepository.CancelTransport(new ZOrderCancelTransportRequest()
                            {
                                OrderID = req.OrderID,
                                ShopID = req.ShopID,
                                CrmOrPaypostCode = ItemOrderTransportInfo.CrmOrPaypostCode,
                                ShippingCode = ItemOrderTransportInfo.ShippingCode,
                                UpdatedBy = req.UpdatedBy
                            });

                            if (resCancelTransport.Code == "00")
                            {
                                res.Code = "00";
                                res.Message = "Hủy yêu cầu vận chuyển thành công.";
                            }
                            else
                            {
                                res.Code = resCancelTransport.Code;
                                res.Message = resCancelTransport.Message;
                            }
                        }
                        else
                        {
                            res.Code = "02";
                            res.Message = "Trạng thái không hợp lệ";
                        }
                    }
                    else
                    {
                        res.Code = resCancelTransportInfo.Code;
                        res.Message = resCancelTransportInfo.Message;
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn đơn hàng cần xử lý.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.CancelTransport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        //[Route("CancelTransportWEB")]
        //public ZResponse CancelTransportWEB(ZOrderMultipleCancelTransportRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        if (req.ListOrderInfo.Count > 0)
        //        {
        //            var ListOrderStatusInvalid = req.ListOrderInfo.Where(m => m.OrderStatus != "W" && m.OrderStatus != "P").ToList();
        //            var ListOrder = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == true && (m.OrderStatus == "W" || m.OrderStatus == "P")).ToList();
        //            var ListOrderBatch = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == false && (m.OrderStatus == "W" || m.OrderStatus == "P")).ToList();
        //            List<ZOrderMultipleCancelTransportOrderInfoRequest> ListGroupByOrderBatch = new List<ZOrderMultipleCancelTransportOrderInfoRequest>();
        //            List<ZErrorDataListResponse> ListErrorData = new List<ZErrorDataListResponse>();

        //            Task ListOrderStatusInvalidHandle = new Task(() =>
        //            {
        //                if (ListOrderStatusInvalid.Count > 0)
        //                {
        //                    foreach (var item in ListOrderStatusInvalid)
        //                    {
        //                        ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = string.Format("Đơn hàng đang ở trạng thái {0}, không được hủy vận chuyển.", XData.GetNameOrderStatus(item.OrderStatus)) });
        //                    }
        //                }
        //            });

        //            Task ListOrderHandle = new Task(() =>
        //            {
        //                if (ListOrder.Count > 0)
        //                {
        //                    foreach (var item in ListOrder)
        //                    {
        //                        var resCancelTransport = OrderRepository.CancelTransport(new ZOrderCancelTransportRequest() { OrderID = item.OrderID, ShopID = req.ShopID, UpdatedBy = req.UpdatedBy });
        //                        if (resCancelTransport.Code != "00")
        //                        {
        //                            ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = resCancelTransport.Message });
        //                        }
        //                    }
        //                }
        //            });

        //            Task ListOrderBatchHandle = new Task(() =>
        //            {
        //                if (ListOrderBatch.Count > 0)
        //                {
        //                    ListGroupByOrderBatch = ListOrderBatch.GroupBy(x => x.BatchCode).Select(x => x.First()).ToList();
        //                    foreach (var item in ListGroupByOrderBatch)
        //                    {
        //                        var resUpdateCancelTransportData = OrderBatchRepository.CancelTransportData(new ZOrderBatchCancelTransportDataRequest() { BatchCode = item.BatchCode, ShopID = req.ShopID, UpdatedBy = req.UpdatedBy });
        //                        if (resUpdateCancelTransportData.Code == "00")
        //                        {
        //                            var resSendTransport = OrderBatchRepository.SendTransport(new ZOrderBatchSendTransportRequest() { AmndState = "A", BatchCode = item.BatchCode, ShopID = req.ShopID, CreatedBy = req.UpdatedBy });
        //                            if (resSendTransport.Code != "00")
        //                            {
        //                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = "" /*OrderCode = string.Join(",", ListOrderBatch.Where(m => m.BatchCode == item.BatchCode).Select(x => x.OrderCode).ToList())*/, Description = resSendTransport.Message });
        //                            }
        //                        }
        //                        else if (resUpdateCancelTransportData.Code == "90")
        //                        {
        //                            var ListErrorOrder = JsonConvert.DeserializeObject<List<ZOrderBatchSendTransportDataResponse>>(resUpdateCancelTransportData.Data);
        //                            foreach (var ItemOrderError in ListErrorOrder)
        //                            {
        //                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = XCommon.ConvertToOrderCode(new ZOrderConvertToOrderCodeRequest() { OrderID = ItemOrderError.OrderID, ShopID = req.ShopID }) /*OrderCode = ListOrderBatch.Where(m => m.OrderID == ItemOrderError.OrderID).Select(m => m.OrderCode).First()*/, Description = ItemOrderError.Description });
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = "", Description = resUpdateCancelTransportData.Message });
        //                        }
        //                    }
        //                }
        //            });

        //            ListOrderHandle.Start();
        //            ListOrderBatchHandle.Start();

        //            Task.WaitAll(ListOrderHandle, ListOrderBatchHandle);

        //            if (ListErrorData.Count == 0)
        //            {
        //                res.Code = "00";
        //                res.Message = "Hủy yêu cầu vận chuyển thành công.";
        //            }
        //            else
        //            {
        //                res.Code = "10";
        //                res.Message = "Hủy yêu cầu vận chuyển thất bại, vui lòng xem chi tiết.";
        //                res.Data = JsonConvert.SerializeObject(ListErrorData);
        //            }
        //        }
        //        else
        //        {
        //            res.Code = "01";
        //            res.Message = "Vui lòng chọn đơn hàng cần xử lý.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.CancelTransportWEB: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}

        [Route("FESearchList")]
        public ZResponse FESearchList(ZOrderFESearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZOrderFESearchListResponse> listItem = new List<ZOrderFESearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.FE_SEARCH_LIST", new object[]
                {
                    req.FromDate.Split(' ').Length == 1 ? string.Format("{0} 00:00:00", req.FromDate) : req.FromDate,   //P_FROM_DATE                   VARCHAR 
                    req.ToDate.Split(' ').Length == 1 ? string.Format("{0} 23:59:59", req.ToDate) : req.ToDate,         //P_TO_DATE                     VARCHAR
                    req.ListCustomerCode,       //P_LIST_CRM_OR_PAYPOST_CODE    VARCHAR
                    req.OrderStatus,            //P_ORDER_STATUS                VARCHAR
                    req.PrintStatus,            //P_PRINT_STATUS                VARCHAR
                    req.PostalType,             //P_POSTAL_TYPE                 NUMBER
                    req.MainTransportService,   //P_MAIN_TRANSPORT_SERVICE      VARCHAR
                    string.IsNullOrEmpty(req.ExtraTransportService) ? "" : req.ExtraTransportService.Replace(',','|'),  //P_EXTRA_TRANSPORT_SERVICE     VARCHAR
                    req.SearchKey,              //P_SEARCH_KEY                  VARCHAR
                    req.ShopID,                 //P_SHOP_ID                     NUMBER
                    req.Channel,                //P_CHANNEL                     VARCHAR
                    0,                          //P_RETURN_CODE                 OUT NUMBER
                    0                           //P_OUT_CURSOR                  OUT T_CURSOR
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
                                    listItem.Add(new ZOrderFESearchListResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        BatchCode = dr["BATCH_CODE"].ToString(),
                                        SenderName = dr["SENDER_NAME"].ToString(),
                                        SenderPhone = dr["SENDER_PHONE"].ToString(),
                                        SenderProvinceID = Convert.ToInt32(dr["SENDER_PROVINCE_ID"]),
                                        SenderDistrictID = Convert.ToInt32(dr["SENDER_DISTRICT_ID"]),
                                        SenderWardID = Convert.ToInt32(dr["SENDER_WARD_ID"]),
                                        SenderAddress = dr["SENDER_STREET"].ToString(),
                                        SenderFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        ReceiverPhone = dr["RECEIVER_PHONE"].ToString(),
                                        ReceiverProvinceID = Convert.ToInt32(dr["RECEIVER_PROVINCE_ID"]),
                                        ReceiverProvinceName = dr["RECEIVER_PROVINCE_NAME"].ToString(),
                                        ReceiverDistrictID = Convert.ToInt32(dr["RECEIVER_DISTRICT_ID"]),
                                        ReceiverDistrictName = dr["RECEIVER_DISTRICT_NAME"].ToString(),
                                        ReceiverWardID = Convert.ToInt32(dr["RECEIVER_WARD_ID"]),
                                        ReceiverWardName = dr["RECEIVER_WARDS_NAME"].ToString(),
                                        ReceiverAddress = dr["RECEIVER_ADDRESS"].ToString(),
                                        ReceiverFullAddress = dr["RECEIVER_FULL_ADDRESS"].ToString(),
                                        Message = dr["MESSAGE"].ToString(),
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                        TotalOrderValue = Convert.ToInt64(dr["TOTAL_ORDER_VALUE"]),
                                        TotalProductValue = Convert.ToInt64(dr["TOTAL_PRODUCT_VALUE"]),
                                        OrderStatus = dr["ORDER_STATUS"].ToString(),
                                        OrderStatusName = XData.GetNameOrderStatus(dr["ORDER_STATUS"].ToString()),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        CreatedOrderDate = dr["AMND_DATE"].ToString(),
                                        AcceptOrderDate = dr["ACCEPT_DATE"].ToString(),
                                        Transport = dr["TRANSPORT"].ToString(),
                                        TransportOrderName = dr["TRANSPORT_ORDER_NAME"].ToString(),
                                        TransportOrderValue = string.IsNullOrEmpty(dr["TRANSPORT_ORDER_VALUE"].ToString()) ? 0 : Convert.ToInt64(dr["TRANSPORT_ORDER_VALUE"].ToString()),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        TransportWeight = Convert.ToInt64(dr["TRANSPORT_WEIGHT"]),
                                        TransportMainServiceCode = dr["TRANSPORT_MAIN_SERVICE_CODE"].ToString(),
                                        TransportMainServiceName = dr["TRANSPORT_MAIN_SERVICE_NAME"].ToString(),
                                        TransportExtraServiceCode = dr["TRANSPORT_EXTRA_SERVICE_CODE"].ToString(),
                                        TransportExtraServiceName = dr["TRANSPORT_EXTRA_SERVICE_NAME"].ToString(),
                                        PrintStatus = XData.GetNamePrintStatus(dr["PRINT_STATUS"].ToString()),
                                        Channel = new ZBaseResponse()
                                        {
                                            Code = dr["CHANNEL"].ToString(),
                                            Name = XData.GetNameChannel(dr["CHANNEL"].ToString())
                                        },
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        ShopCode = dr["SHOP_CODE"].ToString(),
                                        CrmOrPaypostCode = dr["CRM_OR_PAYPOST_CODE"].ToString(),

                                        CustomerCode = dr["CUSTOMER_CODE"].ToString(),
                                        CustomerName = dr["CUSTOMER_NAME"].ToString(),
                                        CustomerPhone = dr["CUSTOMER_PHONE"].ToString(),
                                        CustomerFullAddress = dr["CUSTOMER_FULL_ADDRESS"].ToString(),

                                        UpdatedName = dr["UPDATED_NAME"].ToString(),
                                        UpdatedDate = dr["UPDATED_DATE"].ToString(),
                                        ReasonCancel = dr["REASON_CANCEL"].ToString(),

                                        ShippingID = dr["SHIPPING_ID"].ToString(),
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),

                                        PaymentFreightVat = Convert.ToInt64(dr["PAYMENT_FREIGHT_VAT"]),

                                        TiktokReverseOrderCode = dr["TIKTOK_REVERSE_ORDER_CODE"].ToString(),
                                        TiktokOrderInventoryStatus = new XStatusModel()
                                        {
                                            Code = dr["TIK_ORDER_INVENTORY_STATUS"].ToString(),
                                            Name = XData.GetNameTiktokOrderInventoryStatus(dr["TIK_ORDER_INVENTORY_STATUS"].ToString())
                                        }
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
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.FESearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("BESearchList")]
        public ZResponse BESearchList(ZOrderBESearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZOrderBESearchListResponse> list = new List<ZOrderBESearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.BE_SEARCH_LIST", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.OrderStatus,
                    req.OrderCode,
                    req.ShippingCode,
                    req.ShopCode,
                    req.CrmOrPaypostCode,
                    req.ReceiverProvinceID,
                    req.TransportMainService,
                    req.TransportExtraService,
                    req.PlaceOfCreateOrder,
                    req.PostalType,
                    req.PartnerID,
                    req.UserID,
                    req.Channel,
                    0,                          //P_RETURN_CODE OUT NUMBER
                    0                           //P_OUT_CURSOR  OUT T_CURSOR
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
                                    list.Add(new ZOrderBESearchListResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        BatchCode = dr["BATCH_CODE"].ToString(),
                                        SenderName = dr["SENDER_NAME"].ToString(),
                                        SenderPhone = dr["SENDER_PHONE"].ToString(),
                                        SenderFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        ReceiverPhone = dr["RECEIVER_PHONE"].ToString(),
                                        ReceiverFullAddress = dr["RECEIVER_FULL_ADDRESS"].ToString(),
                                        Message = dr["MESSAGE"].ToString(),
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                        Transport = dr["TRANSPORT"].ToString(),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        ProvisionalTransportWeight = Convert.ToInt64(dr["PROVISIONAL_TRANSPORT_WEIGHT"]),
                                        ActualTransportWeight = Convert.ToInt64(dr["ACTUAL_TRANSPORT_WEIGHT"]),
                                        OrderStatus = new ZBaseResponse()
                                        {
                                            Code = dr["ORDER_STATUS"].ToString(),
                                            Name = XData.GetNameOrderStatus(dr["ORDER_STATUS"].ToString())
                                        },
                                        TransportMainService = new ZBaseResponse()
                                        {
                                            Code = dr["TRANSPORT_MAIN_SERVICE_CODE"].ToString(),
                                            Name = dr["TRANSPORT_MAIN_SERVICE_NAME"].ToString(),
                                        },
                                        TransportExtraService = new ZBaseResponse()
                                        {
                                            Code = dr["TRANSPORT_EXTRA_SERVICE_CODE"].ToString(),
                                            Name = dr["TRANSPORT_EXTRA_SERVICE_NAME"].ToString(),
                                        },
                                        CreatedDate = dr["CREATED_DATE"].ToString(),

                                        UpdatedName = dr["UPDATED_NAME"].ToString(),
                                        UpdatedDate = dr["UPDATED_DATE"].ToString(),
                                        ReasonCancel = dr["REASON_CANCEL"].ToString(),

                                        PostalType = string.IsNullOrEmpty(dr["BATCH_CODE"].ToString()) ? XData.GetNamePostalType("1") : XData.GetNamePostalType("2"),

                                        ShippingID = dr["SHIPPING_ID"].ToString(),
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),

                                        PaymentFreightVat = string.IsNullOrEmpty(dr["PAYMENT_FREIGHT_VAT"].ToString()) ? 0 : Convert.ToInt64(dr["PAYMENT_FREIGHT_VAT"]),
                                        TrackingItemInfoCreatedDate = Convert.ToString(dr["CREATED_DATE_TRACKING_ITEM"]),
                                        AcceptancePosCode = Convert.ToString(dr["ACCEPTANCE_POSCODE"]),
                                        DeliveryPosCode = Convert.ToString(dr["DELIVERY_POSCODE"]),

                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        ShopCode = dr["SHOP_CODE"].ToString(),
                                        CrmOrPaypostCode = dr["CRM_OR_PAYPOST_CODE"].ToString(),
                                        PrintTypeDefault = dr["PRINT_TYPE_DEFAULT"].ToString(),
                                        StoreName = dr["STORE_NAME"].ToString(),
                                        PartnerName = dr["PARTNER_NAME"].ToString(),

                                        CustomerName = dr["CUSTOMER_NAME"].ToString(),
                                        CustomerPhone = dr["CUSTOMER_PHONE"].ToString(),
                                        CustomerFullAddress = dr["CUSTOMER_FULL_ADDRESS"].ToString(),
                                    });
                                }
                            }
                            if (list.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(list);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.BESearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("ChangeStatus")]
        //public ZResponse ChangeStatus(ZOrderChangeStatusRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CHANGE_STATUS", new object[]
        //        {
        //            0,
        //            req.ID,
        //            req.BeforeStatus,
        //            req.AfterStatus,
        //            req.ReasonCancel,
        //            req.UpdatedBy,
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
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Đơn hàng đã được xử lý.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Đã xuất kho không thể huỷ đơn hàng.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại";
        //                            break;
        //                    }
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.ChangeStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("CancelStatus")]
        public ZResponse CancelStatus(ZOrderMultipleCancelStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (req.ListOrderInfo.Count > 0)
                {
                    var ListOrderStatusInvalid = req.ListOrderInfo.Where(m => m.OrderStatus != "W" && m.OrderStatus != "P" && m.OrderStatus != "N" && m.OrderStatus != "D").ToList();
                    var ListOrder = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == true && (m.OrderStatus == "W" || m.OrderStatus == "P" || m.OrderStatus == "N" || m.OrderStatus == "D")).ToList();
                    var ListOrderBatch = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == false && (m.OrderStatus == "W" || m.OrderStatus == "P" || m.OrderStatus == "N" || m.OrderStatus == "D")).ToList();
                    List<ZOrderMultipleCancelStatusOrderInfoRequest> ListGroupByOrderBatch = new List<ZOrderMultipleCancelStatusOrderInfoRequest>();
                    List<ZErrorDataListResponse> ListErrorData = new List<ZErrorDataListResponse>();

                    Task ListOrderStatusInvalidHandle = new Task(() =>
                    {
                        if (ListOrderStatusInvalid.Count > 0)
                        {
                            foreach (var item in ListOrderStatusInvalid)
                            {
                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = string.Format("Đơn hàng đang ở trạng thái {0}, không được hủy đơn hàng.", XData.GetNameOrderStatus(item.OrderStatus)) });
                            }
                        }
                    });

                    Task ListOrderHandle = new Task(() =>
                    {
                        if (ListOrder.Count > 0)
                        {
                            foreach (var item in ListOrder)
                            {
                                var resCancelOrderInfo = OrderRepository.CancelOrderInfo(new ZOrderCancelOrderInfoRequest()
                                {
                                    OrderID = item.OrderID,
                                    ShopID = req.ShopID
                                });

                                if (resCancelOrderInfo.Code == "00")
                                {
                                    var ItemOrderTransportInfo = JsonConvert.DeserializeObject<ZOrderCancelOrderInfoResponse>(resCancelOrderInfo.Data);

                                    if (ItemOrderTransportInfo.OrderStatus == "W" || ItemOrderTransportInfo.OrderStatus == "P")
                                    {
                                        var resCancelTransport = OrderRepository.CancelTransport(new ZOrderCancelTransportRequest()
                                        {
                                            OrderID = ItemOrderTransportInfo.OrderID,
                                            ShopID = req.ShopID,
                                            CrmOrPaypostCode = ItemOrderTransportInfo.CrmOrPaypostCode,
                                            ShippingCode = ItemOrderTransportInfo.ShippingCode,
                                            UpdatedBy = req.UpdatedBy
                                        });

                                        if (resCancelTransport.Code == "00")
                                        {
                                            var resCancelOrderStatus = OrderRepository.CancelOrderStatus(new ZOrderCancelOrderStatusRequest()
                                            {
                                                OrderID = ItemOrderTransportInfo.OrderID,
                                                ReasonCancel = req.ReasonCancel,
                                                ShopID = req.ShopID,
                                                UpdatedBy = req.UpdatedBy
                                            });

                                            if (resCancelOrderStatus.Code != "00")
                                            {
                                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = ItemOrderTransportInfo.OrderCode, Description = resCancelOrderStatus.Message });
                                            }
                                        }
                                        else
                                        {
                                            ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = ItemOrderTransportInfo.OrderCode, Description = resCancelTransport.Message });
                                        }
                                    }
                                    else if (ItemOrderTransportInfo.OrderStatus == "D" || ItemOrderTransportInfo.OrderStatus == "N")
                                    {
                                        var resCancelOrderStatus = OrderRepository.CancelOrderStatus(new ZOrderCancelOrderStatusRequest()
                                        {
                                            OrderID = ItemOrderTransportInfo.OrderID,
                                            ReasonCancel = req.ReasonCancel,
                                            ShopID = req.ShopID,
                                            UpdatedBy = req.UpdatedBy
                                        });

                                        if (resCancelOrderStatus.Code != "00")
                                        {
                                            ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = ItemOrderTransportInfo.OrderCode, Description = resCancelOrderStatus.Message });
                                        }
                                    }
                                    else
                                    {
                                        ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = ItemOrderTransportInfo.OrderCode, Description = "Trạng thái không hợp lệ." });
                                    }
                                }
                                else
                                {
                                    ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = resCancelOrderInfo.Message });
                                }
                            }
                        }
                    });

                    Task ListOrderBatchHandle = new Task(() =>
                    {
                        if (ListOrderBatch.Count > 0)
                        {
                            ListGroupByOrderBatch = ListOrderBatch.GroupBy(x => x.BatchCode).Select(x => x.First()).ToList();
                            foreach (var item in ListGroupByOrderBatch)
                            {
                                var resCancelOrderInfo = OrderBatchRepository.CancelOrderInfo(new ZOrderBatchCancelOrderInfoRequest()
                                {
                                    BatchCode = item.BatchCode,
                                    ShopID = req.ShopID
                                });

                                if (resCancelOrderInfo.Code == "00")
                                {
                                    var FullOrderInOrderBatch = JsonConvert.DeserializeObject<List<ZOrderBatchCancelOrderInfoResponse>>(resCancelOrderInfo.Data);

                                    foreach (var ItemOrderInOrderBatch in FullOrderInOrderBatch)
                                    {
                                        if (ItemOrderInOrderBatch.OrderStatus == "W" || ItemOrderInOrderBatch.OrderStatus == "P")
                                        {
                                            var resCancelTransport = OrderRepository.CancelTransport(new ZOrderCancelTransportRequest()
                                            {
                                                OrderID = ItemOrderInOrderBatch.OrderID,
                                                ShopID = req.ShopID,
                                                CrmOrPaypostCode = ItemOrderInOrderBatch.CrmOrPaypostCode,
                                                ShippingCode = ItemOrderInOrderBatch.ShippingCode,
                                                UpdatedBy = req.UpdatedBy
                                            });

                                            if (resCancelTransport.Code == "00")
                                            {
                                                var resCancelOrderStatus = OrderRepository.CancelOrderStatus(new ZOrderCancelOrderStatusRequest()
                                                {
                                                    OrderID = ItemOrderInOrderBatch.OrderID,
                                                    ReasonCancel = req.ReasonCancel,
                                                    ShopID = req.ShopID,
                                                    UpdatedBy = req.UpdatedBy
                                                });

                                                if (resCancelOrderStatus.Code != "00")
                                                {
                                                    ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = ItemOrderInOrderBatch.OrderCode, Description = resCancelOrderStatus.Message });
                                                }
                                            }
                                            else
                                            {
                                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = ItemOrderInOrderBatch.OrderCode, Description = resCancelTransport.Message });
                                            }
                                        }
                                        else if (ItemOrderInOrderBatch.OrderStatus == "D" || ItemOrderInOrderBatch.OrderStatus == "N")
                                        {
                                            var resCancelOrderStatus = OrderRepository.CancelOrderStatus(new ZOrderCancelOrderStatusRequest()
                                            {
                                                OrderID = ItemOrderInOrderBatch.OrderID,
                                                ReasonCancel = req.ReasonCancel,
                                                ShopID = req.ShopID,
                                                UpdatedBy = req.UpdatedBy
                                            });

                                            if (resCancelOrderStatus.Code != "00")
                                            {
                                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = ItemOrderInOrderBatch.OrderCode, Description = resCancelOrderStatus.Message });
                                            }
                                        }
                                        else
                                        {
                                            ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = ItemOrderInOrderBatch.OrderCode, Description = "Trạng thái không hợp lệ" });
                                        }
                                    }
                                }
                                else
                                {
                                    ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = "", Description = resCancelOrderInfo.Message });
                                }
                            }
                        }
                    });

                    ListOrderStatusInvalidHandle.Start();
                    ListOrderHandle.Start();
                    ListOrderBatchHandle.Start();

                    Task.WaitAll(ListOrderStatusInvalidHandle, ListOrderHandle, ListOrderBatchHandle);

                    if (ListErrorData.Count == 0)
                    {
                        res.Code = "00";
                        res.Message = "Hủy đơn hàng thành công.";
                    }
                    else
                    {
                        res.Code = "10";
                        res.Message = "Hủy đơn hàng thất bại, vui lòng xem chi tiết.";
                        res.Data = JsonConvert.SerializeObject(ListErrorData);
                    }
                }
                else if (req.OrderID > 0)
                {
                    var resCancelOrderInfo = OrderRepository.CancelOrderInfo(new ZOrderCancelOrderInfoRequest()
                    {
                        OrderID = req.OrderID,
                        ShopID = req.ShopID
                    });

                    if (resCancelOrderInfo.Code == "00")
                    {
                        var ItemOrderTransportInfo = JsonConvert.DeserializeObject<ZOrderCancelOrderInfoResponse>(resCancelOrderInfo.Data);

                        if (ItemOrderTransportInfo.OrderStatus == "W" || ItemOrderTransportInfo.OrderStatus == "P")
                        {
                            var resCancelTransport = OrderRepository.CancelTransport(new ZOrderCancelTransportRequest()
                            {
                                OrderID = req.OrderID,
                                ShopID = req.ShopID,
                                CrmOrPaypostCode = ItemOrderTransportInfo.CrmOrPaypostCode,
                                ShippingCode = ItemOrderTransportInfo.ShippingCode,
                                UpdatedBy = req.UpdatedBy
                            });

                            if (resCancelTransport.Code == "00")
                            {
                                var resCancelOrderStatus = OrderRepository.CancelOrderStatus(new ZOrderCancelOrderStatusRequest()
                                {
                                    OrderID = req.OrderID,
                                    ReasonCancel = req.ReasonCancel,
                                    ShopID = req.ShopID,
                                    UpdatedBy = req.UpdatedBy
                                });

                                if (resCancelOrderStatus.Code == "00")
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
                        else if (ItemOrderTransportInfo.OrderStatus == "D" || ItemOrderTransportInfo.OrderStatus == "N")
                        {
                            var resCancelOrderStatus = OrderRepository.CancelOrderStatus(new ZOrderCancelOrderStatusRequest()
                            {
                                OrderID = req.OrderID,
                                ReasonCancel = req.ReasonCancel,
                                ShopID = req.ShopID,
                                UpdatedBy = req.UpdatedBy
                            });

                            if (resCancelOrderStatus.Code == "00")
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
                    res.Code = "01";
                    res.Message = "Vui lòng chọn đơn hàng cần xử lý.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.CancelStatus: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("CancelStatus")]
        //public ZResponse CancelStatus(ZOrderCancelStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = OrderRepository.CancelOrder(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.CancelStatus: " + ex.Message);
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[Route("NewStatus")]
        //public ZResponse NewStatus(ZOrderMultipleNewStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        List<ZOrderNewStatusResponse> ListError = new List<ZOrderNewStatusResponse>();
        //        var splitID = req.strID.Split(',');
        //        for (var i = 0; i < splitID.Length; i++)
        //        {
        //            res = OrderRepository.NewStatus(Convert.ToInt32(splitID[i]), req.ShopID, req.UpdatedBy);
        //            if (res.Code != "00")
        //            {
        //                var data = JsonConvert.DeserializeObject<ZOrderNewStatusResponse>(res.Data);
        //                ListError.Add(new ZOrderNewStatusResponse()
        //                {
        //                    OrderCode = data.OrderCode,
        //                    Description = data.Description
        //                });
        //            }
        //        }
        //        if (ListError.Count > 0)
        //        {
        //            var TotalSuccess = splitID.Length - ListError.Count;
        //            res.Code = "01";
        //            res.Message = "Duyệt đơn hàng thành công " + TotalSuccess + "/" + splitID.Length + " đơn hàng, vui lòng nhấn vào Xem chi tiết để kiểm tra.";
        //            res.Data = JsonConvert.SerializeObject(ListError);
        //        }
        //        else
        //        {
        //            res.Code = "00";
        //            res.Message = "Thành công";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.NewStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("NewStatus")]
        public ZResponse NewStatus(ZOrderMultipleNewStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (req.ListOrderInfo.Count > 0)
                {
                    var ListOrderStatusInvalid = req.ListOrderInfo.Where(m => m.OrderStatus != "D").ToList();
                    var ListOrder = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == true && m.OrderStatus == "D").ToList();
                    var ListOrderBatch = req.ListOrderInfo.Where(m => string.IsNullOrEmpty(m.BatchCode) == false && m.OrderStatus == "D").ToList();
                    List<ZOrderMultipleNewStatusOrderInfoRequest> ListGroupByOrderBatch = new List<ZOrderMultipleNewStatusOrderInfoRequest>();
                    List<ZErrorDataListResponse> ListErrorData = new List<ZErrorDataListResponse>();

                    Task ListOrderStatusInvalidHandle = new Task(() =>
                    {
                        if (ListOrderStatusInvalid.Count > 0)
                        {
                            foreach (var item in ListOrderStatusInvalid)
                            {
                                ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = string.Format("Đơn hàng đang ở trạng thái {0}, không được duyệt.", XData.GetNameOrderStatus(item.OrderStatus)) });
                            }
                        }
                    });

                    Task ListOrderHandle = new Task(() =>
                    {
                        if (ListOrder.Count > 0)
                        {
                            foreach (var item in ListOrder)
                            {
                                var resNewStatus = OrderRepository.NewStatus(new ZOrderNewStatusRequest() { OrderID = item.OrderID, ShopID = req.ShopID, UpdatedBy = req.UpdatedBy });
                                if (resNewStatus.Code != "00")
                                {
                                    ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = "", OrderCode = item.OrderCode, Description = resNewStatus.Message });
                                }
                            }
                        }
                    });

                    Task ListOrderBatchHandle = new Task(() =>
                    {
                        if (ListOrderBatch.Count > 0)
                        {
                            ListGroupByOrderBatch = ListOrderBatch.GroupBy(x => x.BatchCode).Select(x => x.First()).ToList();
                            foreach (var item in ListGroupByOrderBatch)
                            {
                                var resNewStatus = OrderBatchRepository.NewStatus(new ZOrderBatchNewStatusRequest() { BatchCode = item.BatchCode, ShopID = req.ShopID, UpdatedBy = req.UpdatedBy });
                                if (resNewStatus.Code != "00")
                                {
                                    if (resNewStatus.Code == "90")
                                    {
                                        var ListErrorOrder = JsonConvert.DeserializeObject<List<ZOrderBatchNewStatusResponse>>(resNewStatus.Data);
                                        foreach (var ItemOrderError in ListErrorOrder)
                                        {
                                            ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = XCommon.ConvertToOrderCode(new ZOrderConvertToOrderCodeRequest() { OrderID = ItemOrderError.OrderID, ShopID = req.ShopID }), Description = ItemOrderError.Description });
                                        }
                                    }
                                    else
                                    {
                                        ListErrorData.Add(new ZErrorDataListResponse() { BatchCode = item.BatchCode, OrderCode = "", Description = resNewStatus.Message });
                                    }
                                }
                            }
                        }
                    });

                    ListOrderStatusInvalidHandle.Start();
                    ListOrderHandle.Start();
                    ListOrderBatchHandle.Start();

                    Task.WaitAll(ListOrderStatusInvalidHandle, ListOrderHandle, ListOrderBatchHandle);

                    if (ListErrorData.Count == 0)
                    {
                        res.Code = "00";
                        res.Message = "Duyệt đơn hàng thành công.";
                    }
                    else
                    {
                        res.Code = "10";
                        res.Message = "Duyệt đơn hàng thất bại, vui lòng xem chi tiết";
                        res.Data = JsonConvert.SerializeObject(ListErrorData);
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn đơn hàng cần xử lý.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.NewStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int ID, int ShopID)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resOrderGetByID = OrderRepository.GetByID(new ZOrderGetByIDRequest() { ID = ID, ShopID = ShopID });
                if (resOrderGetByID.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderGetByID.Data);

                    item.PostalType = string.IsNullOrEmpty(item.BatchCode) ? XData.GetNamePostalType("1") : XData.GetNamePostalType("2");

                    if (string.IsNullOrEmpty(item.DeviceName) || item.DeviceName == "WEB")
                    {
                        item.PlaceOfCreation = "Web EMSOne";
                    }
                    else if (item.EMSOneCode.Contains("EE") || item.DeviceName.ToUpper() == "EMSENTERPRISE")
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

                    /*Lấy các sản phẩm trong đơn hàng*/
                    var resGetOrderItemByOrderID = OrderItemRepository.GetOrderItemByOrderID(new ZOrderItemGetByOrderIDRequest() { OrderID = item.ID });
                    if (resGetOrderItemByOrderID.Code == "00")
                    {
                        item.OrderItems = JsonConvert.DeserializeObject<List<ZOrderItemGetByOrderIDResponse>>(resGetOrderItemByOrderID.Data);
                    }

                    /*Lấy các thông tin thay đổi từ việc gửi yêu cầu hỗ trợ*/
                    //var resSearchListUpdateInformation = RequestSupportHistoryRepository.SearchListUpdateInformation(new ZRequestSupportHistorySearchListUpdateInformationRequest() { EShippingCode = item.ShippingCode, ShopID = ShopID });
                    //if(resSearchListUpdateInformation.Code == "00")
                    //{
                    //    item.ListUpdateInformation = JsonConvert.DeserializeObject<List<ZRequestSupportHistorySearchListUpdateInformationResponse>>(resSearchListUpdateInformation.Data);
                    //}

                    res.Code = "00";
                    res.Message = "Lấy dữ liệu thành công";
                    res.Data = JsonConvert.SerializeObject(item);
                }
                else
                {
                    res.Code = resOrderGetByID.Code;
                    res.Message = resOrderGetByID.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("CheckOrderTransport")]
        //public ZResponse CheckOrderTransport(ZOrderCheckTransportRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CHECK_ORDER_TRANSPORT", new object[]
        //        {
        //            0,
        //            req.OrderCode,
        //            req.ShopID
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
        //                    res.Code = "01";
        //                    res.Message = "Không tồn tại đơn hàng.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.CheckOrderTransport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("CoppyOrder")]
        public ZResponse CoppyOrder(ZOrderCoppyRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.COPPY_ORDER", new object[]
                {
                    0,
                    req.ID,
                    req.ShopID,
                    req.CreatedBy
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
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Thêm lịch sử phiếu xuất kho thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.CoppyOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByShippingCode")]
        public ZResponse GetByShippingCode(string ShippingCode, int ShopID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderGetByShippingCodeResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.GET_BY_SHIPPING_CODE", new object[]
                {
                    ShippingCode,     //P_SHIPPING_CODE NUMBER
                    ShopID,           //P_SHOP_ID       NUMBER
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
                                if (dr.Read())
                                {
                                    item = new ZOrderGetByShippingCodeResponse()
                                    {
                                        ReceiverPhone = dr["RECEIVER_PHONE"].ToString(),
                                        ReceiverProvinceID = Convert.ToInt32(dr["RECEIVER_PROVINCE_ID"]),
                                        ReceiverProvinceName = Convert.ToString(dr["RECEIVER_PROVINCE_NAME"]),
                                        ReceiverDistrictID = Convert.ToInt32(dr["RECEIVER_DISTRICT_ID"]),
                                        ReceiverDistrictName = Convert.ToString(dr["RECEIVER_DISTRICT_NAME"]),
                                        ReceiverWardID = Convert.ToInt32(dr["RECEIVER_WARD_ID"]),
                                        ReceiverWardName = Convert.ToString(dr["RECEIVER_WARD_NAME"]),
                                        ReceiverAddress = dr["RECEIVER_ADDRESS"].ToString(),
                                        SenderProvinceID = Convert.ToInt32(dr["SENDER_PROVINCE_ID"]),
                                        SenderProvinceName = Convert.ToString(dr["SENDER_PROVINCE_NAME"]),
                                        SenderDistrictID = Convert.ToInt32(dr["SENDER_DISTRICT_ID"]),
                                        SenderDistrictName = Convert.ToString(dr["SENDER_DISTRICT_NAME"]),
                                        SenderWardID = Convert.ToInt32(dr["SENDER_WARD_ID"]),
                                        SenderWardName = Convert.ToString(dr["SENDER_WARD_NAME"]),
                                        SenderAddress = dr["SENDER_STREET"].ToString(),
                                        IsCheckCOD = dr["TRANSPORT_EXTRA_SERVICE"].ToString().Contains("108") == true ? "Y" : "N",
                                        TotalProductValue = Convert.ToInt64(dr["TOTAL_PRODUCT_VALUE"]),
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Mã bưu gửi không tồn tại.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã bưu gửi không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã đơn hàng không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.GetByShippingCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("DetailByOrderID")]
        public ZResponse DetailByOrderID(ZOrderDetailByOrderIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                ZOrderDetailByOrderIDResponse item = null;
                var resOrderGetByID = OrderRepository.GetByID(new ZOrderGetByIDRequest() { ID = req.ID, ShopID = req.ShopID });
                if (resOrderGetByID.Code == "00")
                {
                    item = JsonConvert.DeserializeObject<ZOrderDetailByOrderIDResponse>(resOrderGetByID.Data);

                    item.ListEMSSignaruteAndImageInfo = new List<ZEMSSignaruteAndImageInformationResponse>();
                    item.ListVNPSignaruteAndImageInfo = new List<ZVNPSignaruteAndImageInformationResponse>();

                    item.PostalType = string.IsNullOrEmpty(item.BatchCode) ? XData.GetNamePostalType("1") : XData.GetNamePostalType("2");

                    if (string.IsNullOrEmpty(item.DeviceName) || item.DeviceName == "WEB")
                    {
                        item.PlaceOfCreation = "Web EMSOne";
                    }
                    else if (item.EMSOneCode.Contains("EE") || item.DeviceName.ToUpper() == "EMSENTERPRISE")
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

                    /*Lấy các sản phẩm trong đơn hàng*/
                    var resGetOrderItemByOrderID = OrderItemRepository.GetOrderItemByOrderID(new ZOrderItemGetByOrderIDRequest() { OrderID = item.ID });
                    if (resGetOrderItemByOrderID.Code == "00")
                    {
                        item.OrderItems = JsonConvert.DeserializeObject<List<ZOrderItemGetByOrderIDResponse>>(resGetOrderItemByOrderID.Data);
                    }

                    /*Lấy các thông tin thay đổi từ việc gửi yêu cầu hỗ trợ*/
                    var resSearchListUpdateInformation = RequestSupportHistoryRepository.SearchListUpdateInformation(new ZRequestSupportHistorySearchListUpdateInformationRequest() { EShippingCode = item.ShippingCode, ShopID = req.ShopID });
                    if (resSearchListUpdateInformation.Code == "00")
                    {
                        item.ListUpdateInformation = JsonConvert.DeserializeObject<List<ZRequestSupportHistorySearchListUpdateInformationResponse>>(resSearchListUpdateInformation.Data);
                    }

                    List<string> ListStatusForSearchSignaruteAndImage = new List<string>() { "F", "G", "H", "I", "U", "V", "Z" };
                    if (ListStatusForSearchSignaruteAndImage.Contains(item.Status) == true)
                    {
                        var resEMS = EMSRepository.EMSSignaruteAndImageInformation(new ZEMSSignaruteAndImageInformationRequest() { LadingCode = item.ShippingCode });
                        if (resEMS.Code == "00")
                        {
                            item.ListEMSSignaruteAndImageInfo = JsonConvert.DeserializeObject<List<ZEMSSignaruteAndImageInformationResponse>>(resEMS.Data);
                        }

                        if (item.ListEMSSignaruteAndImageInfo.Count == 0)
                        {
                            var resVNP = VNPRepository.VNPSignaruteAndImageInformation(new ZVNPSignaruteAndImageInformationRequest() { itemCode = item.ShippingCode });
                            if (resVNP.Code == "00")
                            {
                                item.ListVNPSignaruteAndImageInfo = JsonConvert.DeserializeObject<List<ZVNPSignaruteAndImageInformationResponse>>(resVNP.Data);
                            }
                        }
                    }

                    res.Code = "00";
                    res.Message = "Lấy dữ liệu thành công";
                    res.Data = JsonConvert.SerializeObject(item);
                }
                else
                {
                    res.Code = resOrderGetByID.Code;
                    res.Message = resOrderGetByID.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.DetailByOrderID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("DetailByShippingCode")]
        public ZResponse DetailByShippingCode(ZOrderDetailByShippingCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderDetailByShippingCodeResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.DETAIL_BY_SHIPPING_CODE", new object[]
                {
                    req.ShippingCode.ToUpper().Trim(),     //P_SHIPPING_CODE NUMBER
                    req.ShopID,           //P_SHOP_ID       NUMBER
                    0,                    //P_RETURN_CODE   OUT NUMBER
                    0                     //P_OUT_CURSOR    OUT T_CURSOR
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
                                if (dr.Read())
                                {
                                    item = new ZOrderDetailByShippingCodeResponse()
                                    {
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
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

                            if (item != null)
                            {
                                if((req.ShopID == item.ShopID ||
                                     (item.ParentListID?.Split(',').Contains(req.ShopID.ToString()) ?? false)))
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

                                    //item.ReceiverPhone = string.Format("{0}****{1}", item.ReceiverPhone.Substring(0, 3), item.ReceiverPhone.Substring(item.ReceiverPhone.Length - 3, 3));

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

                                    /*Lấy các thông tin thay đổi từ việc gửi yêu cầu hỗ trợ*/
                                    //var resListUpdateInformation = RequestSupportHistoryRepository.SearchListUpdateInformation(new ZRequestSupportHistorySearchListUpdateInformationRequest() { EShippingCode = item.ShippingCode, ShopID = item.ShopID });
                                    //if (resListUpdateInformation.Code == "00")
                                    //{
                                    //    item.ListUpdateInformation = JsonConvert.DeserializeObject<List<ZRequestSupportHistorySearchListUpdateInformationResponse>>(resListUpdateInformation.Data);
                                    //}

                                    if(req.AuthorizationCode == "NEKO" &&
                                        (req.ShopID == item.ShopID ||
                                         (item.ParentListID?.Split(',').Contains(req.ShopID.ToString()) ?? false)))
                                    {
                                        //List<string> ListStatusForSearchSignaruteAndImage = new List<string>() { "F", "G", "H", "I", "U", "V", "Z"};
                                        List<string> ListStatusForSearchSignaruteAndImage = new List<string>() { "W", "P", "S", "A", "T", "F", "G", "H", "I", "U", "V", "Z"};
                                        if(ListStatusForSearchSignaruteAndImage.Contains(item.StatusChar) == true)
                                        {
                                            var resEMS = EMSRepository.EMSSignaruteAndImageInformation(new ZEMSSignaruteAndImageInformationRequest() { LadingCode = item.ShippingCode });
                                            if(resEMS.Code == "00")
                                            {
                                                item.ListEMSSignaruteAndImageInfo = JsonConvert.DeserializeObject<List<ZEMSSignaruteAndImageInformationResponse>>(resEMS.Data);
                                            }

                                            if(item.ListEMSSignaruteAndImageInfo.Count == 0)
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
                                                            itemEMSImg.ImagePlus = new List<string>();
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
                                                            itemVNPImg.ImagesPlus = new List<string>();
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
                            switch (result)
                            {
                                //case -1:
                                //    res.Code = "01";
                                //    res.Message = "Mã bưu gửi không tồn tại.";
                                //    break;
                                //case -2:
                                //    res.Code = "02";
                                //    res.Message = "Mã đơn hàng không tồn tại.";
                                //    break;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.DetailByShippingCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("SearchInfoSendRequestTransport")]
        //public ZResponse SearchInfoSendRequestTransport(ZOrderSearchInfoSendRequestTransportRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = OrderRepository.SearchInfoSendRequestTransport(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.SearchInfoSendRequestTransport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[Route("CancelRequestTransport")]
        //public ZResponse CancelRequestTransport(ZOrderCancelRequestTransportRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CANCEL_REQUEST_TRANSPORT", new object[]
        //        {
        //            0,
        //            req.EMSOneCode,
        //            req.ShopID,
        //            req.UpdatedBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Đã huỷ yêu cầu vận chuyển thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Đơn hàng không tồn tại.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Sai trạng thái đơn hàng.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Phiếu xuất kho đã được xử lý.";
        //                            break;
        //                        case -4:
        //                            res.Code = "04";
        //                            res.Message = "Sản phẩm không tồn tại.";
        //                            break;
        //                        case -5:
        //                            res.Code = "05";
        //                            res.Message = "Lưu lịch sử phiếu xuất kho thất bại.";
        //                            break;
        //                        case -6:
        //                            res.Code = "06";
        //                            res.Message = "Lưu lịch sử đơn hàng thất bại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại";
        //                            break;
        //                    }
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.CancelRequestTransport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("SuccessHistory")]
        public ZResponse SuccessHistory(ZOrderSuccessHistoryRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZOrderSuccessHistoryResponse> oListItem = new List<ZOrderSuccessHistoryResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.ORDER_SUCCESS_HISTORY", new object[]
                {
                    req.ShopID,
                    req.MobileNumber,
                    req.FromDate,
                    req.ToDate,
                    req.SearchKey,
                    0,                    //P_RETURN_CODE   OUT NUMBER
                    0                     //P_OUT_CURSOR    OUT T_CURSOR
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
                                    oListItem.Add(new ZOrderSuccessHistoryResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        TotalProductValue = Convert.ToInt64(dr["TOTAL_PRODUCT_VALUE"]),
                                        Status = new XStatusModel()
                                        {
                                            Code = dr["STATUS"].ToString(),
                                            Name = XData.GetNameOrderStatus(dr["STATUS"].ToString())
                                        },
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                    });
                                }
                            }
                            if (oListItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oListItem);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.SuccessHistory: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Print")]
        public ZResponse Print(ZOrderPrintRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = OrderRepository.HandlePrint(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.Print: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchHistory")]
        public ZResponse SearchHistory(ZOrderSearchHistoryRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = OrderRepository.SearchHistory(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.SearchHistory: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("CheckOrderCode")]
        //public ZResponse CheckOrderCode(ZOrderCheckCodeRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = OrderRepository.CheckOrderCode(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.CheckOrderCode: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("UpdatePrintStatus")]
        public ZResponse UpdatePrintStatus(ZOrderUpdatePrintStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int count = 0;
                var splitID = req.IDs.Split(',');
                for (var i = 0; i < splitID.Length; i++)
                {
                    res = OrderRepository.UpdatePrintStatus(Convert.ToInt32(splitID[i]), req.UpdatedBy);
                    if (res.Code == "00")
                    {
                        count++;
                    }
                }
                if (splitID.Length == count)
                {
                    res.Code = "00";
                    res.Message = "Thành công " + count + "/" + splitID.Length;
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Thành công " + count + "/" + splitID.Length;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.UpdatePrintStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("AddMultiple")]
        //public ZResponse AddMultiple(ZOrderAddMultipleRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.ADD_MULTIPLE", new object[]
        //        {
        //            0,
        //            req.Products,
        //            req.OrderCode == null ? "" : req.OrderCode.ToUpper(),
        //            req.Pickup.Name,
        //            req.Pickup.MobileNumber,
        //            req.Pickup.Street,
        //            req.Pickup.ProvinceID,
        //            req.Pickup.DistrictID,
        //            req.Pickup.WardID,
        //            req.Pickup.FullAddress,
        //            req.Deliver.Name,
        //            req.Deliver.MobileNumber,
        //            req.Deliver.Street,
        //            req.Deliver.ProvinceID,
        //            req.Deliver.DistrictID,
        //            req.Deliver.WardID,
        //            req.Deliver.FullAddress,
        //            req.Message,
        //            req.NumberOfProduct,
        //            req.Weight,
        //            req.TotalCOD,
        //            req.OrderValue,
        //            req.Note,
        //            req.TransportMainService,
        //            req.TransportExtraService,
        //            req.CollectionType,
        //            req.TransportPayer,
        //            req.Long,
        //            req.Width,
        //            req.Height,
        //            req.Channel,
        //            req.WareHouseID,
        //            req.CreatedBy,
        //            req.ShopID,
        //            req.DeviceName,
        //            0                                        //P_ORDER_ID          OUT NUMBER
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    var OrderID = Convert.ToInt32(dbCommand.Parameters["P_ORDER_ID"].Value);
        //                    var CheckOrderItemSuccess = 0;
        //                    if (req.OrderedProducts != null && req.OrderedProducts.Count > 0)
        //                    {
        //                        foreach (var item in req.OrderedProducts)
        //                        {
        //                            var resAddOrderItem = AddOrderItem(new ZOrderItemAddRequest()
        //                            {
        //                                OrderID = OrderID,
        //                                ProductID = item.ProductID,
        //                                VariantID = item.VariantID,
        //                                NumberOfProduct = item.NumberOfProduct,
        //                                Price = item.Price,
        //                                Discount = item.Discount
        //                            });
        //                            if (resAddOrderItem.Code == "00")
        //                            {
        //                                CheckOrderItemSuccess++;
        //                            }
        //                            else
        //                            {
        //                                res.Code = resAddOrderItem.Code;
        //                                res.Message = resAddOrderItem.Message;
        //                                return res;
        //                            }
        //                        }
        //                        if (CheckOrderItemSuccess == req.OrderedProducts.Count)
        //                        {
        //                            res.Code = "00";
        //                            res.Message = "Thành công.";
        //                            res.Data = OrderID.ToString();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Thành công.";
        //                        res.Data = OrderID.ToString();
        //                    }
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Không tìm thấy thông tin khách hàng. Vui lòng kiểm tra lại.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Không tìm thấy thông tin cửa hàng. Vui lòng liên hệ quản trị.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Dữ liệu không tồn tại.";
        //                            break;
        //                        case -4:
        //                            res.Code = "04";
        //                            res.Message = "Trạng thái đã được xử lý.";
        //                            break;
        //                        case -5:
        //                            res.Code = "05";
        //                            res.Message = "Dịch vụ chính CPN TMĐT Nhanh tiết kiệm chỉ áp dụng cho liên tỉnh - liên vùng. Vui lòng kiểm tra lại.";
        //                            break;
        //                        case -6:
        //                            res.Code = "06";
        //                            res.Message = "Mã đơn hàng đã tồn tại.";
        //                            break;
        //                        case -7:
        //                            res.Code = "07";
        //                            res.Message = "Mã đơn hàng không được để trống.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại";
        //                            break;
        //                    }
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.AddOrder: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("EditReceiverAddress")]
        public ZResponse EditReceiverAddress(ZOrderEditReceiverAddressRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.EDIT_RECEIVER_ADDRESS", new object[]
                {
                    0,
                    req.OrderID,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Street,
                    req.ShopID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thành công.";
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
                                    res.Message = "Trạng thái không hợp lệ.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.EditReceiverAddress " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("Pagination")]
        //public ZResponse Pagination(ZOrderPaginationRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        List<ZOrderPaginationItemResponse> listItem = new List<ZOrderPaginationItemResponse>();
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DEV_PKG.SEARCH_ORDER_PAGINATION", new object[]
        //        {
        //            req.FromDate,   //P_FROM_DATE                   VARCHAR 
        //            req.ToDate,         //P_TO_DATE                     VARCHAR             //P_SEARCH_KEY                  VARCHAR
        //            req.ShopID,                 //P_SHOP_ID                     NUMBER
        //            req.PageNumber,
        //            req.PageLimit,
        //            0,
        //            0,                          //P_RETURN_CODE                 OUT NUMBER
        //            0                           //P_OUT_CURSOR                  OUT T_CURSOR
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
        //                if (result > 0)
        //                {
        //                    using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            listItem.Add(new ZOrderPaginationItemResponse()
        //                            {
        //                                OrderID = Convert.ToInt32(dr["ORDER_ID"]),
        //                                EMSOneCode = dr["EMSONE_CODE"].ToString(),
        //                                OrderCode = dr["ORDER_CODE"].ToString(),
        //                                OrderStatus = dr["ORDER_STATUS"].ToString(),
        //                                CreatedDate = dr["CREATED_DATE"].ToString(),
        //                            });
        //                        }
        //                    }
        //                    if (listItem.Count > 0)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(new ZOrderPaginationResponse() { Order = listItem, TotalRow = Convert.ToInt32(dbCommand.Parameters["P_RETURN_DATA_ROW"].Value) });
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
        //                    }
        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Thất bại.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.Pagination: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
    }
}
