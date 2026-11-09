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
using System.Threading.Tasks;
using EMS.API.Core.MongoRepository;

namespace EMS.API.Core.Repositories
{
    public class OrderRepository
    {
        

        public static ZResponse AddOrderV2(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                if (req.ShipmentDetails == null)
                {
                    req.ShipmentDetails = new ZOrderAddShipmentInfoRequest();
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

                if (req.Transport == "N" && req.isSendTransport == "Y")
                {
                    res.Code = "21";
                    res.Message = "Đơn hàng không được chọn vận chuyển.";
                    return res;
                }

                //if (req.TransportOrderValue > 100000000 && string.IsNullOrEmpty(req.Status))
                //{
                //    res.Code = "10";
                //    res.Message = "Giá trị đơn hàng không vượt quá 100 triệu đồng.";
                //    return res;
                //}

                if (req.OrderedProducts == null || req.OrderedProducts.Count == 0)
                {
                    req.OrderedProducts = new List<ZOrderAddItemRequest>();
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
                //var IsNewAddress = 0;
                //if(req.ShipmentDetails.IsReceiverNewAddress == "Y")
                //{
                //    var reqSearchPWDByEMSMapsSearch = XCommon.SearchPWDByVNPOSTSearch(new ZVNPOSTMapSearchRequest {
                //        direction = 2,
                //        provinceCode = req.ShipmentDetails.ProvinceID.ToString(),
                //        wardCode = req.ShipmentDetails.WardID.ToString(),
                //        territory = 2,
                //        inputText = req.ShipmentDetails.Address
                //    });
                //    if(reqSearchPWDByEMSMapsSearch.Code == "00")
                //    {
                //        var resSearchPWDByEMSMapsSearch = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeResponse>(reqSearchPWDByEMSMapsSearch.Data);
                //        req.ShipmentDetails.ProvinceID = resSearchPWDByEMSMapsSearch.ProvinceID;
                //        req.ShipmentDetails.DistrictID = resSearchPWDByEMSMapsSearch.DistrictID;
                //        req.ShipmentDetails.WardID = resSearchPWDByEMSMapsSearch.WardID;
                //        req.ShipmentDetails.FullAddress = "(VNPOSTmap) " + resSearchPWDByEMSMapsSearch.Street;
                //    }
                //    else
                //    {
                //        req.ShipmentDetails.ProvinceID = 0;
                //        req.ShipmentDetails.DistrictID = 0;
                //        req.ShipmentDetails.WardID = 0;
                //    }
                //    req.ShipmentDetails.Address = req.ShipmentDetails.Address + ", " + req.ShipmentDetails.WardName + ", " + req.ShipmentDetails.ProvinceName;
                //    IsNewAddress = 1;
                //}


                if (req.TransportMainService > 0 && string.IsNullOrEmpty(req.TransportExtraService) == false)
                {
                    switch (req.TransportMainService)
                    {
                        case 102: //Phát hỏa tốc
                        case 141: //CPN TMĐT đồng giá TMĐ
                        case 502: //CPN Tài liệu
                        case 541: //EMS TCNH Bạc
                        case 542: //EMS TCNH Vàng
                        case 543: //EMS TCNH Tiêu chuẩn
                            if(req.TransportExtraService.Contains("104") == true || req.TransportExtraService.Contains("105") || req.TransportExtraService.Contains("106")){
                                res.Code = "23";
                                res.Message = "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. Vui lòng điều chỉnh";
                                return res;
                            }
                            break;
                    }
                }

                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.ORDER_ADD", new object[]
                {
                    0,
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
                    req.MainFee,
                    req.ExtraFee,
                    req.SurchargeFee,
                    req.TransportPayer,
                    req.CollectionType,
                    req.WareHouseID,
                    req.OrderedProducts.ToXML(),
                    req.CustomerID,                          //P_CUSTOMER_ID        NUMBER,
                    req.ShipmentDetails.ReceiverName,        //P_RECEIVER_NAME      VARCHAR,
                    req.ShipmentDetails.MobileNumber,        //P_MOBILE_NUMBER      VARCHAR,
                    req.ShipmentDetails.ProvinceID,          //P_PROVINCE_ID        NUMBER,
                    req.ShipmentDetails.WardID,              //P_WARD_ID            NUMBER,
                    req.ShipmentDetails.Address,             //P_ADDRESS            VARCHAR,
                    //req.ShipmentDetails.FullAddress,       //P_FULL_ADDRESS       VARCHAR,
                    req.ShipmentDetails.PickUpAddressID,     //P_PICK_UP_ADDRESS_ID NUMBER
                    req.ShipmentDetails.PickupFullName,      //SENDER_NAME          VARCHAR
                    req.ShipmentDetails.PickupMobileNumber,  //SENDER_PHONE         VARCHAR
                    req.ShipmentDetails.PickupProvinceID,    //SENDER_PROVINCE_ID   NUMBER
                    req.ShipmentDetails.PickupWardID,        //SENDER_WARD_ID       NUMBER
                    req.ShipmentDetails.PickupStreet,        //SENDER_STREET        VARCHAR
                    //req.ShipmentDetails.PickupFullAddress, //SENDER_FULL_ADDRESS  VARCHAR
                    req.ShopID,                              //P_SHOP_ID            NUMBER,
                    req.CreatedBy,                           //P_CREATED_BY         NUMBER,
                    req.DeviceName,                          //P_DEVICE_NAME        VARCHAR
                    req.isUpdatedCustomer,
                    req.isSendTransport,
                    req.ExpectedDeliveryDate,
                    req.CollectionPostOfficeRecentID,
                    0,
                    0,
                    0,
                    0,
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
                            res.Message = string.IsNullOrEmpty(req.Status) == true ? "Tạo đơn hàng thành công" : "Tạo đơn nháp thành công";
                            res.Data = dbCommand.Parameters["P_OUT_ORDER_ID"].Value.ToString();
                            Task.Run(() =>
                            {
                                try
                                {
                                    ZSendReceiverInforRequest reqSendReceiverInfor = new ZSendReceiverInforRequest()
                                    {
                                        ItemCode = dbCommand.Parameters["P_OUT_EMS_ESHIPPING_BARCODE"].Value.ToString(),
                                        OrderCode = dbCommand.Parameters["P_OUT_EMSONE_CODE"].Value.ToString(),
                                        CustomerCode = dbCommand.Parameters["P_OUT_CRM_OR_PAYPOST_CODE"].Value.ToString(),
                                        SenderName = req.ShipmentDetails.PickupFullName,
                                        SenderPhone = req.ShipmentDetails.PickupMobileNumber,
                                        SenderAddress = (req.ShipmentDetails.PickupFullAddress == null) ? req.ShipmentDetails.PickupStreet : req.ShipmentDetails.PickupFullAddress,
                                        ReceiverName = req.ShipmentDetails.ReceiverName,
                                        ReceiverPhone = req.ShipmentDetails.MobileNumber,
                                        ReceiverAddress = (req.ShipmentDetails.FullAddress == null) ? req.ShipmentDetails.Address : req.ShipmentDetails.FullAddress,
                                        Channel = "EMSONE",
                                        Extend_1 = req.TransportOrderName + req.Message,                                     

                                    };
                                    var resSendReceiverInfor = EMSRepository.SendReceiverInfor(reqSendReceiverInfor);
                                }
                                catch
                                {
                                    // Bỏ qua luôn hoặc ghi log nếu muốn
                                }
                            });

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
                                    res.Code = "21";
                                    res.Message = "Đơn hàng đã tạo quá số lần đổi trả";
                                    break;
                                case -22:
                                    res.Code = "22";
                                    res.Message = "Thay đổi số lượng của Pallet thất bại.";
                                    break;
                                case -23:
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.AddOrderV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse AddOrderAndSendTransportV2(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resAddOrder = AddOrderV2(req);
                if (resAddOrder.Code == "00")
                {
                    var resSendTransport = SendTransport(new ZOrderSendTransportRequest() { AmndState = "I", OrderID = Convert.ToInt32(resAddOrder.Data), ShopID = req.ShopID, CreatedBy = req.CreatedBy });
                    if (resSendTransport.Code == "00")
                    {
                        res.Code = resSendTransport.Code;
                        res.Message = resSendTransport.Message;
                        res.Data = resAddOrder.Data;
                    }
                    else
                    {
                        res.Code = resSendTransport.Code;
                        res.Message = resSendTransport.Message;
                    }
                }
                else
                {
                    res.Code = resAddOrder.Code;
                    res.Message = resAddOrder.Message;
                    res.Data = resAddOrder.Data;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.AddOrderAndSendTransportV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //public static ZResponse AddOrderItem(ZOrderAddItemRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.ADD_ORDER_ITEM", new object[]
        //        {
        //            0,
        //            req.OrderID,                 //P_ORDER_ID          NUMBER,
        //            req.ProductID,          //P_PRODUCT_ID        NUMBER,
        //            req.VariantID,          //P_VARIANT_ID        NUMBER,
        //            req.NumberOfProduct,    //P_NUMBER_OF_PRODUCT NUMBER,
        //            req.Price,              //P_PRICE             NUMBER,
        //            req.Discount            //P_DISCOUNT          NUMBER
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
        //                            res.Message = "Không tìm thấy đơn hàng. Thêm dữ liệu vào đơn hàng thất bại.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Không tìm thấy sản phẩm trong kho. Thêm dữ liệu vào đơn hàng thất bại.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Số lượng sản phẩm trong kho không đủ lập đơn hàng. Vui lòng kiểm tra lại.";
        //                            break;
        //                        case -4:
        //                            res.Code = "04";
        //                            res.Message = "Giá bán sản phẩm đã thay đổi. Vui lòng kiểm tra lại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thêm sản phẩm vào đơn hàng thất bại. Vui lòng liên hệ quản trị.";
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

        //public static ZResponse AddOrderAndAcceptInventory(ZOrderAddRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    ZResponse resAddInventory = new ZResponse();
        //    try
        //    {
        //        var resAddOrder = AddOrder(req);
        //        if (resAddOrder.Code == "00")
        //        {
        //            var OrderID = Convert.ToInt32(resAddOrder.Data);
        //            if (req.OrderedProducts != null && req.OrderedProducts.Count > 0)
        //            {
        //                resAddInventory = InventoryRepository.Add(new ZInventoryAddRequest()
        //                {
        //                    OrderID = OrderID,
        //                    ShopID = req.ShopID,
        //                    WarehouseID = req.WareHouseID,
        //                    CreatedBy = req.CreatedBy,
        //                    Type = "O", //Tạo phiếu xuất kho
        //                    ListInventoryDetail = req.ListInventoryDetail
        //                });
        //            }
        //            else
        //            {
        //                resAddInventory.Code = "00";
        //                resAddInventory.Message = "Thành công";
        //            }

        //            if (resAddInventory.Code == "00")
        //            {
        //                var InventoryID = Convert.ToInt32(resAddInventory.Data);
        //                var resUpdateAmndStateOrder = UpdateAmndState(new ZOrderUpdateAmndStateRequest()
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
        //                    if (InventoryID > 0)
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
        //                            res.Message = "Tạo đơn hàng và duyệt phiếu xuất kho thành công";
        //                            res.Data = resUpdateAmndStateOrder.Data;
        //                        }
        //                        else
        //                        {
        //                            resUpdateAmndStateOrder = UpdateAmndState(new ZOrderUpdateAmndStateRequest()
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.AddOrderAndAcceptInventory: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}

        //public static ZResponse UpdateAmndState(ZOrderUpdateAmndStateRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.UPDATE_AMND_STATE", new object[]
        //        {
        //            0,
        //            req.OrderID,            //P_ORDER_ID                NUMBER
        //            req.AmndState,          //P_VALUE                   VARCHAR
        //            req.IsRequestOrder,     //P_IS_REQUEST_ORDER        VARCHAR
        //            req.RequestOrderID,     //P_REQUEST_ORDER_ID        NUMBER
        //            req.IsUpdatedCustomer,  //P_IS_UPDATED_CUSTOMER     VARCHAR
        //            req.CustomerID,         //P_CUSTOMER_ID             NUMBER
        //            req.CustomerFullName,   //P_CUSTOMER_FULL_NAME      VARCHAR
        //            req.CustomerProvinceID, //P_CUSTOMER_PROVINCE_ID    NUMBER
        //            req.CustomerDistrictID, //P_CUSTOMER_DISTRICT_ID    NUMBER
        //            req.CustomerWardID,     //P_CUSTOMER_WARD_ID        NUMBER
        //            req.CustomerAddress,    //P_CUSTOMER_ADDRESS        VARCHAR
        //            req.ShopID,             //P_SHOP_ID                 NUMBER
        //            req.UpdatedBy,          //P_UPDATED_BY              NUMBER
        //            ""                      //P_OUT_EMSONE_CODE         VARCHAR
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
        //                    res.Data = Convert.ToString(dbCommand.Parameters["P_OUT_EMSONE_CODE"].Value);
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Yêu cầu đặt hàng đã được xử lý.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Yêu cầu đặt hàng khác người xử lý.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Yêu cầu đặt hàng không tồn tại.";
        //                            break;
        //                        case -4:
        //                            res.Code = "04";
        //                            res.Message = "Khách hàng không tồn tại.";
        //                            break;
        //                        case -5:
        //                            res.Code = "05";
        //                            res.Message = "Lưu lịch sử đơn hàng thất bại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Duyệt đơn hàng thất bại. Vui lòng liên hệ quản trị.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.UpdateAmndState: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}

        //public static ZResponse GetOrderItemByOrderID(int OrderID)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    List<ZOrderItemResponse> list = new List<ZOrderItemResponse>();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_ITEM_PKG.GET_BY_ID", new object[]
        //        {
        //            OrderID,              //P_ORDER_ID      NUMBER
        //            0,                    //P_RETURN_CODE   OUT NUMBER
        //            0                     //P_OUT_CURSOR    OUT T_CURSOR
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
        //                            list.Add(new ZOrderItemResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                ProductID = Convert.ToInt32(dr["PRODUCT_ID"]),
        //                                ProductName = dr["PRODUCT_NAME"].ToString(),
        //                                VariantID = Convert.ToInt32(dr["VARIANT_ID"]),
        //                                VariantName = dr["VARIANT_NAME"].ToString(),
        //                                NumberOfProduct = Convert.ToInt32(string.IsNullOrEmpty(dr["NUMBER_OF_PRODUCT"].ToString()) ? "0" : dr["NUMBER_OF_PRODUCT"].ToString()),
        //                                Price = Convert.ToInt64(string.IsNullOrEmpty(dr["PRICE"].ToString()) ? "0" : dr["PRICE"].ToString()),
        //                                Discount = Convert.ToInt32(string.IsNullOrEmpty(dr["DISCOUNT"].ToString()) ? "0" : dr["DISCOUNT"].ToString()),
        //                                Weight = Convert.ToInt32(string.IsNullOrEmpty(dr["WEIGHT"].ToString()) ? "0" : dr["WEIGHT"].ToString()),
        //                                ProductVariantImage = dr["PRODUCT_VARIANT_IMAGE"].ToString(),
        //                                WareHouseQuantity = Convert.ToInt32(string.IsNullOrEmpty(dr["WAREHOUSE_QUANTITY"].ToString()) ? "0" : dr["WAREHOUSE_QUANTITY"].ToString()),
        //                            });
        //                        }
        //                    }
        //                    if (list.Count > 0)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(list);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
        //                        res.Data = null;
        //                    }

        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Thất bại";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.GetOrderItemByOrderID: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        /// <summary>
        /// Create international order
        /// </summary>
        public static ZResponse InternalOrderAdd(ZInternationalOrderAddRequest req)
        {
            ZResponse res = new ZResponse();

            try
            {
                int result = -1;

                #region Validate request

                if(req == null)
                {
                    res.Code = "01";
                    res.Message = "Invalid order data.";
                    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    return res;
                }

                

                if(req.ProductList == null || req.ProductList.Count == 0)
                {
                    res.Code = "08";
                    res.Message = "Product list is required.";
                    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    return res;
                }

                if(req.ShopID <= 0)
                {
                    res.Code = "01";
                    res.Message = "Invalid Shop ID.";
                    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    return res;
                }

                if(string.IsNullOrWhiteSpace(req.ReceiverName) ||
                    string.IsNullOrWhiteSpace(req.ReceiverPhone) ||
                    string.IsNullOrWhiteSpace(req.ReceiverAddress) ||
                    string.IsNullOrWhiteSpace(req.ReceiverCountryCode) ||
                    string.IsNullOrWhiteSpace(req.ReceiverCountryName) ||
                    string.IsNullOrWhiteSpace(req.ReceiverCity))
                {
                    res.Code = "02";
                    res.Message = "Receiver information is required.";
                    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    return res;
                }

                

                

                if(req.TotalWeight <= 0)
                {
                    res.Code = "05";
                    res.Message = "Invalid shipment weight.";
                    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    return res;
                }

                if(req.DeclareValue < 0)
                {
                    res.Code = "06";
                    res.Message = "Invalid declared value.";
                    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    return res;
                }

                if(!string.Equals(
                    req.IsAgreeCustoms,
                    "Y",
                    StringComparison.OrdinalIgnoreCase))
                {
                    res.Code = "07";
                    res.Message = "Customs declaration agreement is required.";
                    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    return res;
                }

                #endregion


                #region Default values

                req.OrderCode = string.IsNullOrWhiteSpace(req.OrderCode)
                    ? ""
                    : req.OrderCode.Trim().ToUpper();

                req.Channel = string.IsNullOrWhiteSpace(req.Channel)
                    ? "WEB"
                    : req.Channel.Trim().ToUpper();

                req.DeviceName = string.IsNullOrWhiteSpace(req.DeviceName)
                    ? "WEBFE"
                    : req.DeviceName.Trim();

                req.CurrencyCode = string.IsNullOrWhiteSpace(req.CurrencyCode)
                    ? "USD"
                    : req.CurrencyCode.Trim().ToUpper();

                req.GoodsType = string.IsNullOrWhiteSpace(req.GoodsType)
                    ? "COMMERCIAL"
                    : req.GoodsType.Trim().ToUpper();

                if(req.Quantity <= 0)
                {
                    req.Quantity = req.ProductList.Sum(x => x.Quantity);
                }

                

                if(string.IsNullOrWhiteSpace(req.DeclareCurrency))
                {
                    req.DeclareCurrency = req.CurrencyCode;
                }

                if(string.IsNullOrWhiteSpace(req.OriginCountryCode))
                {
                    req.OriginCountryCode = "VN";
                }

                if(string.IsNullOrWhiteSpace(req.OriginCountryName))
                {
                    req.OriginCountryName = "VIETNAM";
                }

                #endregion


                #region Product XML

                string productXml = req.ProductList.ToXML();

                #endregion


                using(
                    DbCommand dbCommand =
                        XHelper.Database.GetStoredProcCommand(
                            XHelper.Schema +
                            "EMS_ORDER_PKG.ORDER_ADD_INTERNATIONAL",
                            new object[]
                            {
                                /*
                                 * Function return value
                                 */
                                0,

                                /*
                                 * Order information
                                 */
                                req.OrderCode,
                                // P_ORDER_CODE

                                req.Channel,
                                // P_CHANNEL

                                req.Note,
                                // P_NOTE

                                req.Message,
                                // P_MESSAGE


                                /*
                                 * Customer
                                 */
                                req.CustomerID,
                                // P_CUSTOMER_ID


                                /*
                                 * Receiver
                                 */
                                req.ReceiverName,
                                // P_RECEIVER_NAME

                                req.ReceiverPhone,
                                // P_RECEIVER_PHONE

                                req.ReceiverAddress,
                                // P_RECEIVER_ADDRESS

                                req.ReceiverCountryCode,
                                // P_RECEIVER_COUNTRY_CODE

                                req.ReceiverCountryName,
                                // P_RECEIVER_COUNTRY_NAME

                                req.ReceiverCity,
                                // P_RECEIVER_CITY

                                req.ReceiverZipCode,
                                // P_RECEIVER_ZIPCODE


                                /*
                                 * Sender
                                 */
                                req.SenderName,
                                // P_SENDER_NAME

                                req.SenderPhone,
                                // P_SENDER_PHONE

                                req.SenderProvinceID,
                                // P_SENDER_PROVINCE_ID

                                req.SenderWardID,
                                // P_SENDER_WARD_ID

                                req.SenderStreet,
                                // P_SENDER_STREET


                                /*
                                 * International service
                                 */
                                "",
                                // P_SERVICE_CODE

                                "",
                                // P_SERVICE_NAME

                                req.GoodsType,
                                // P_GOODS_TYPE

                                req.CurrencyCode,
                                // P_CURRENCY_CODE


                                /*
                                 * Package
                                 */
                                req.TotalWeight,
                                // P_TOTAL_WEIGHT

                                req.Length,
                                // P_LENGTH

                                req.Width,
                                // P_WIDTH

                                req.Height,
                                // P_HEIGHT

                                req.Quantity,
                                // P_QUANTITY


                                /*
                                 * Customs
                                 */
                                req.DeclareValue,
                                // P_DECLARE_VALUE

                                req.CustomsGoodsType,
                                // P_CUSTOMS_GOODS_TYPE

                                req.OriginCountryCode,
                                // P_ORIGIN_COUNTRY_CODE

                                req.OriginCountryName,
                                // P_ORIGIN_COUNTRY_NAME

                                req.IsAgreeCustoms,
                                // P_IS_AGREE_CUSTOMS


                                /*
                                 * Fee
                                 */
                                req.TransportFee,
                                // P_TRANSPORT_FEE

                                req.MainFee,
                                // P_MAIN_FEE

                                req.ExtraFee,
                                // P_EXTRA_FEE

                                req.SurchargeFee,
                                // P_SURCHARGE_FEE


                                /*
                                 * Products
                                 */
                                productXml,
                                // P_LIST_PRODUCT


                                /*
                                 * System information
                                 */
                                req.ShopID,
                                // P_SHOP_ID

                                req.CreatedBy,
                                // P_CREATED_BY

                                req.DeviceName,
                                // P_DEVICE_NAME


                                /*
                                 * OUT parameters
                                 */
                                0,
                                // P_OUT_ORDER_ID

                                "",
                                // P_OUT_EMSONE_CODE

                                ""
                                // P_OUT_EMS_BARCODE
                            }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);

                    if(result >= 0)
                    {
                        /*
                         * Oracle function return value
                         */
                        result = Convert.ToInt32(
                            dbCommand.Parameters[0].Value);

                        if(result > 0)
                        {
                            string orderID = "";

                            string emsOneCode = "";

                            string emsBarcode = "";


                            if(dbCommand.Parameters[
                                    "P_OUT_ORDER_ID"].Value != DBNull.Value)
                            {
                                orderID = dbCommand.Parameters[
                                    "P_OUT_ORDER_ID"].Value.ToString();
                            }


                            if(dbCommand.Parameters[
                                    "P_OUT_EMSONE_CODE"].Value != DBNull.Value)
                            {
                                emsOneCode = dbCommand.Parameters[
                                    "P_OUT_EMSONE_CODE"].Value.ToString();
                            }


                            if(dbCommand.Parameters[
                                    "P_OUT_EMS_BARCODE"].Value != DBNull.Value)
                            {
                                emsBarcode = dbCommand.Parameters[
                                    "P_OUT_EMS_BARCODE"].Value.ToString();
                            }


                            res.Code = "00";

                            res.Message =
                                "International order created successfully.";

                            /*
                             * Giữ giống AddOrderV2:
                             * Data trả về OrderID
                             */
                            var resp = new ZPartnerAnalysticOrderAddResponse
                            {
                                OrderID = Convert.ToInt32(orderID),
                                EMSOneCode = emsOneCode,
                                ShippingCode = emsBarcode

                            };
                            res.Data = JsonConvert.SerializeObject(resp);


                            /*
                             * Nếu ZResponse.Data hỗ trợ object
                             * thì có thể dùng:
                             *
                             * res.Data =
                             *     new ZInternationalOrderAddResponse()
                             *     {
                             *         OrderID =
                             *             Convert.ToInt32(orderID),
                             *
                             *         OrderCode =
                             *             req.OrderCode,
                             *
                             *         EMSOneCode =
                             *             emsOneCode,
                             *
                             *         ShippingCode =
                             *             emsBarcode,
                             *
                             *         Message =
                             *             "International order created successfully.",
                             *
                             *         Status =
                             *             "SUCCESS"
                             *     };
                             */
                        }
                        else
                        {
                            #region Oracle error mapping

                            switch(result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message =
                                        "Invalid Shop ID.";
                                    break;


                                case -2:
                                    res.Code = "02";
                                    res.Message =
                                        "Receiver information is required.";
                                    break;


                                case -3:
                                    res.Code = "03";
                                    res.Message =
                                        "Sender information is required. (ProvinceID, WardID)";
                                    break;


                                case -4:
                                    res.Code = "04";
                                    res.Message =
                                        "International service is required.";
                                    break;


                                case -5:
                                    res.Code = "05";
                                    res.Message =
                                        "Invalid shipment weight.";
                                    break;


                                case -6:
                                    res.Code = "06";
                                    res.Message =
                                        "Invalid declared value.";
                                    break;


                                case -7:
                                    res.Code = "07";
                                    res.Message =
                                        "Customs declaration agreement is required.";
                                    break;


                                case -8:
                                    res.Code = "08";
                                    res.Message =
                                        "Product list is required.";
                                    break;


                                case -9:
                                    res.Code = "09";
                                    res.Message =
                                        "Shop does not exist.";
                                    break;


                                case -10:
                                    res.Code = "10";
                                    res.Message =
                                        "Order code already exists.";
                                    break;


                                case -11:
                                    res.Code = "11";
                                    res.Message =
                                        "Customer does not exist.";
                                    break;


                                case -12:
                                    res.Code = "12";
                                    res.Message =
                                        "Invalid product information.";
                                    break;


                                case -13:
                                    res.Code = "13";
                                    res.Message =
                                        "No products found in the order.";
                                    break;


                                case -14:
                                    res.Code = "14";
                                    res.Message =
                                        "Failed to save order history.";
                                    break;


                                case -15:
                                    res.Code = "15";
                                    res.Message =
                                        "Failed to generate international shipment barcode.";
                                    break;


                                case -99:
                                    res.Code = "99";
                                    res.Message =
                                        "An error occurred while creating the international order.";
                                    break;


                                default:
                                    res.Code = "99";
                                    res.Message =
                                        "Failed to create the international order.";
                                    break;
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        res.Code = "99";

                        res.Message =
                            "System error. Please contact the administrator.";
                    }
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(
                    LogFileType.EXCEPTION,
                    "OrderRepository.InternalOrderAdd: " +
                    ex.Message);

                res.Code = "99";

                res.Message =
                    "System error. Please contact the administrator.";
            }

            res.Time =
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            return res;
        }
        public static ZResponse NewStatus(ZOrderNewStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.NEW_STATUS", new object[]
                {
                    0,
                    req.OrderID,    //P_ID
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
                                var ListErrorCode = dbCommand.Parameters["P_OUT_ERROR_CODE"].Value.ToString().Trim(',').Split(',');
                                var ErrorMessage = "";
                                foreach (var item in ListErrorCode)
                                {
                                    switch (Convert.ToInt32(item))
                                    {
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
                                            ErrorMessage += "Vui lòng sử dụng dịch vụ Thu phí ship hàng (PSH) hoặc dịch vụ PPA (PPA) để thu cước người nhận. \n";
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

                                res.Code = "05";
                                res.Message = ErrorMessage;
                                //res.Message = "Lỗi dữ liệu đơn hàng, vui lòng kiểm tra lại.";
                                //res.Data = ErrorMessage;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.NewStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse CancelOrderStatusGrab(ZOrderCancelStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CANCEL_ORDER_STATUS_GRAB", new object[]
                {
                    0,
                    req.OrderID,
                    req.ReasonCancel,
                    req.ShopID,
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
                                    res.Message = "Sai trạng thái đơn hàng";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Sản phẩm không tồn tại trong kho.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Lưu lịch sử phiếu xuất kho thất bại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Lưu lịch sử đơn hàng thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelOrderStatusGrab: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #region Hủy đơn hàng

        //public static ZResponse CancelOrder(ZOrderCancelStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        List<ZOrderCancelStatusResponse> ListError = new List<ZOrderCancelStatusResponse>();
        //        var resSearchInfoCancelRequestTransport = CancelTransportInfo(new ZOrderCancelTransportInfoRequest()
        //        {
        //            //EMSOne truyền sang
        //            OrderID = req.OrderID,
        //            ShopID = req.ShopID,
        //            //Partner truyền sang
        //            CrmOrPaypostCode = req.CrmOrPaypostCode,
        //            OrderCode = req.OrderCode,
        //            ShippingCode = req.ShippingCode
        //        });
        //        if (resSearchInfoCancelRequestTransport.Code == "00")
        //        {
        //            var itemSearchInfoCancelRequestTransport = JsonConvert.DeserializeObject<ZOrderCancelTransportInfoResponse>(resSearchInfoCancelRequestTransport.Data);
        //            ZOrderCancelStatusRequest reqOrderCancelStatus = new ZOrderCancelStatusRequest()
        //            {
        //                OrderID = itemSearchInfoCancelRequestTransport.OrderID,
        //                ReasonCancel = req.ReasonCancel,
        //                ShopID = req.ShopID == 0 ? itemSearchInfoCancelRequestTransport.ShopID : req.ShopID,
        //                UpdatedBy = req.UpdatedBy == 0 ? itemSearchInfoCancelRequestTransport.EmployeeID : req.UpdatedBy
        //            };
        //            if (itemSearchInfoCancelRequestTransport.Status == "W" || itemSearchInfoCancelRequestTransport.Status == "P")
        //            {
        //                var resCancelRequestTransport = XCommon.CancelRequestTransport(new ZTransportShippingCancelDataRequest()
        //                {
        //                    CustomerCode = itemSearchInfoCancelRequestTransport.CrmOrPaypostCode,
        //                    TrackingCode = itemSearchInfoCancelRequestTransport.ShippingCode,
        //                    SignatureCode = Security.SHA256(itemSearchInfoCancelRequestTransport.ShippingCode + itemSearchInfoCancelRequestTransport.CrmOrPaypostCode + XHelper.EMSTransportKey).ToUpper()
        //                });

        //                if (resCancelRequestTransport.Code == "00")
        //                {
        //                    var resCancelStatus = CancelStatus(reqOrderCancelStatus);
        //                    if (resCancelStatus.Code == "00")
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Hủy đơn hàng thành công";
        //                    }
        //                    else
        //                    {
        //                        res.Code = resCancelStatus.Code;
        //                        res.Message = resCancelStatus.Message;
        //                        res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = resCancelStatus.Message });
        //                    }
        //                }
        //                else
        //                {
        //                    res.Code = resCancelRequestTransport.Code;
        //                    res.Message = resCancelRequestTransport.Message;
        //                    res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = resCancelRequestTransport.Message });
        //                }
        //            }
        //            else if (itemSearchInfoCancelRequestTransport.Status == "D" || itemSearchInfoCancelRequestTransport.Status == "N")
        //            {
        //                var resCancelStatus = CancelStatus(reqOrderCancelStatus);
        //                if (resCancelStatus.Code == "00")
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Hủy đơn hàng thành công";
        //                }
        //                else
        //                {
        //                    res.Code = resCancelStatus.Code;
        //                    res.Message = resCancelStatus.Message;
        //                    res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = resCancelStatus.Message });
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "01";
        //                res.Message = "Trạng thái đơn hàng không hợp lệ.";
        //                res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = string.Format("Đơn hàng đang ở trạng thái {0}.", XData.GetNameOrderStatus(itemSearchInfoCancelRequestTransport.Status)) });
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resSearchInfoCancelRequestTransport.Code;
        //            res.Message = resSearchInfoCancelRequestTransport.Message;
        //            res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = req.OrderCode, Description = "Không tồn tại dữ liệu" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //public static ZResponse CancelOrder(ZOrderCancelStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        List<ZOrderCancelStatusResponse> ListError = new List<ZOrderCancelStatusResponse>();
        //        var resSearchInfoCancelRequestTransport = SearchInfoCancelRequestTransport(new ZOrderSearchInfoCancelRequestTransportRequest()
        //        {
        //            OrderID = req.OrderID,
        //            ShopID = req.ShopID,
        //            CrmOrPaypostCode = req.CrmOrPaypostCode,
        //            OrderCode = req.OrderCode,
        //            ShippingCode = req.ShippingCode
        //        });
        //        if (resSearchInfoCancelRequestTransport.Code == "00")
        //        {
        //            var itemSearchInfoCancelRequestTransport = JsonConvert.DeserializeObject<ZOrderSearchInfoCancelRequestTransportResponse>(resSearchInfoCancelRequestTransport.Data);
        //            if (itemSearchInfoCancelRequestTransport.Status == "W" || itemSearchInfoCancelRequestTransport.Status == "P")
        //            {
        //                var resCancelRequestTransport = CancelRequestTransport(new ZOrderCancelRequestTransportRequest()
        //                {
        //                    OrderID = req.OrderID,
        //                    OrderCode = itemSearchInfoCancelRequestTransport.OrderCode,
        //                    CrmOrPaypostCode = itemSearchInfoCancelRequestTransport.CrmOrPaypostCode,
        //                    ShippingCode = itemSearchInfoCancelRequestTransport.ShippingCode,
        //                    ReasonCancel = req.ReasonCancel,
        //                    ShopID = req.ShopID,
        //                    UpdatedBy = req.UpdatedBy,
        //                });
        //                if (resCancelRequestTransport.Code == "00")
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Hủy đơn hàng thành công";
        //                }
        //                else
        //                {
        //                    res.Code = resCancelRequestTransport.Code;
        //                    res.Message = resCancelRequestTransport.Message;
        //                    res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = resCancelRequestTransport.Message });
        //                }
        //            }
        //            else if (itemSearchInfoCancelRequestTransport.Status == "D" || itemSearchInfoCancelRequestTransport.Status == "N")
        //            {
        //                var resCancelStatus = CancelStatus(req);
        //                if (resCancelStatus.Code == "00")
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Hủy đơn hàng thành công";
        //                }
        //                else
        //                {
        //                    res.Code = resCancelStatus.Code;
        //                    res.Message = resCancelStatus.Message;
        //                    res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = resCancelStatus.Message });
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "01";
        //                res.Message = "Trạng thái đơn hàng không hợp lệ.";
        //                res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = string.Format("Đơn hàng đang ở trạng thái {0}.", XData.GetNameOrderStatus(itemSearchInfoCancelRequestTransport.Status)) });
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resSearchInfoCancelRequestTransport.Code;
        //            res.Message = resSearchInfoCancelRequestTransport.Message;
        //            res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = "", Description = "Không tồn tại dữ liệu" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //public static ZResponse SearchInfoAcceptStatus(ZOrderSearchInfoAcceptStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        ZOrderSearchInfoAcceptStatusResponse item = null;
        //        List<ZOrderSearchInfoAcceptStatusOrderItemResponse> ListOrderItem = new List<ZOrderSearchInfoAcceptStatusOrderItemResponse>();
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DEV_PKG.SEARCH_INFO_ACCEPT_STATUS", new object[]
        //        {
        //            req.OrderID,            //P_ORDER_ID            NUMBER
        //            req.ShopID,             //P_SHOP_ID             NUMBER
        //            0,                      //P_RETURN_CODE         OUT NUMBER
        //            0,                      //P_OUT_ORDER_INFO      OUT T_CURSOR
        //            0                       //P_OUT_ORDER_ITEM_INFO OUT T_CURSOR
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
        //                if (result > 0)
        //                {
        //                    using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_ORDER_INFO"].Value)
        //                    {
        //                        if (dr.Read())
        //                        {
        //                            item = new ZOrderSearchInfoAcceptStatusResponse()
        //                            {
        //                                OrderID = Convert.ToInt32(dr["ORDER_ID"]),
        //                                EMSOneCode = dr["EMSONE_CODE"].ToString(),
        //                                OrderCode = dr["ORDER_CODE"].ToString(),

        //                                ReceiverName = dr["RECEIVER_NAME"].ToString(),
        //                                ReceiverPhone = dr["RECEIVER_PHONE"].ToString(),
        //                                ReceiverProvinceID = Convert.ToInt32(dr["RECEIVER_PROVINCE_ID"]),
        //                                ReceiverDistrictID = Convert.ToInt32(dr["RECEIVER_DISTRICT_ID"]),
        //                                ReceiverWardID = Convert.ToInt32(dr["RECEIVER_WARD_ID"]),
        //                                ReceiverStreet = dr["RECEIVER_STREET"].ToString(),

        //                                SenderName = dr["SENDER_NAME"].ToString(),
        //                                SenderPhone = dr["SENDER_PHONE"].ToString(),
        //                                SenderProvinceID = Convert.ToInt32(dr["SENDER_PROVINCE_ID"]),
        //                                SenderDistrictID = Convert.ToInt32(dr["SENDER_DISTRICT_ID"]),
        //                                SenderWardID = Convert.ToInt32(dr["SENDER_WARD_ID"]),
        //                                SenderStreet = dr["SENDER_STREET"].ToString(),

        //                                IsTransport = dr["IS_TRANSPORT"].ToString(),
        //                                TransportOrderValue = Convert.ToInt64(dr["TRANSPORT_ORDER_VALUE"]),
        //                                TransportMainService = Convert.ToInt32(dr["TRANSPORT_MAIN_SERVICE"]),
        //                                TransportExtraService = dr["TRANSPORT_EXTRA_SERVICE"].ToString(),
        //                                TransportPayer = Convert.ToInt32(dr["TRANSPORT_PAYER"]),
        //                                TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
        //                                TransportWeight = Convert.ToDouble(dr["TRANSPORT_WEIGHT"]),
        //                                TransportSizeLength = Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"]),
        //                                TransportSizeWidth = Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"]),
        //                                TransportSizeHeight = Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"]),
        //                                TransportQuantity = Convert.ToInt32(dr["TRANSPORT_QUANTITY"]),

        //                                TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
        //                                TotalPHU = Convert.ToInt64(dr["TOTAL_PHU"]),

        //                                Status = dr["STATUS"].ToString(),

        //                                CrmOrPaypostCode = dr["CRM_OR_PAYPOST_CODE"].ToString(),
        //                                TypeShop = Convert.ToInt32(dr["TYPE_SHOP"]),

        //                                WareHouseID = Convert.ToInt32(dr["WAREHOUSE_ID"]),
        //                                WareHouseIsLock = dr["WAREHOUSE_IS_LOCK"].ToString(),

        //                                CustomerID = Convert.ToInt32(dr["CUSTOMER_ID"]),

        //                                PickUpID = Convert.ToInt32(dr["PICK_UP_ID"]),
        //                                PickUpName = dr["PICK_UP_NAME"].ToString(),
        //                                PickUpPhone = dr["PICK_UP_PHONE"].ToString(),
        //                                PickUpProvinceID = Convert.ToInt32(dr["PICK_UP_PROVINCE_ID"]),
        //                                PickUpDistrictID = Convert.ToInt32(dr["PICK_UP_DISTRICT_ID"]),
        //                                PickUpWardID = Convert.ToInt32(dr["PICK_UP_WARD_ID"]),
        //                                PickUpStreet = dr["PICK_UP_STREET"].ToString(),
        //                            };
        //                        }
        //                    }

        //                    using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_ORDER_ITEM_INFO"].Value)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            ListOrderItem.Add(new ZOrderSearchInfoAcceptStatusOrderItemResponse()
        //                            {
        //                                ProvisionalProductID = Convert.ToInt32(dr["PROVISIONAL_PRO_ID"]),
        //                                ActualProductID = Convert.ToInt32(dr["ACTUAL_PRODUCT_ID"]),
        //                                ActualProductIsLock = dr["ACTUAL_PRODUCT_IS_LOCK"].ToString(),

        //                                ProvisionalProductVariantID = Convert.ToInt32(dr["PROVISIONAL_PRO_VAR_ID"]),
        //                                ActualProductVariantID = Convert.ToInt32(dr["ACTUAL_PRODUCT_VARIANT_ID"]),
        //                                ActualProductVariantIsLock = dr["ACTUAL_PRODUCT_VARIANT_IS_LOCK"].ToString(),

        //                                ProvisionalProductVariantPrice = Convert.ToInt64(dr["PROVISIONAL_PRO_VAR_PRICE"]),
        //                                ActualProductVariantPrice = Convert.ToInt64(dr["ACTUAL_PRODUCT_VARIANT_PRICE"]),

        //                                NumberOfProduct = Convert.ToInt32(dr["NUMBER_OF_PRODUCT"]),

        //                                ProductAndVariantName = dr["PRODUCT_AND_VARIANT_NAME"].ToString(),
        //                            });
        //                        }
        //                    }

        //                    if (item != null)
        //                    {
        //                        item.ListOrderItem = ListOrderItem;
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(item);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
        //                    }
        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Thất bại";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchInfoAcceptStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //public static ZResponse SendTransportStatus(ZOrderSendTransportStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHIPPING_MANAGEMENT_PKG.UPDATE_STATUS_TRANSPORT_ORDER", new object[]
        //        {
        //            0,
        //            req.EMSOneCode,
        //            req.UpdatedBy,
        //            req.Status
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Cập nhật trạng thái vận chuyển thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Dữ liệu không tồn tại.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Sai trạng thái đơn hàng.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Lưu lịch sử thất bại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SendTransportStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //public static ZResponse CancelTransportStatus(ZOrderCancelTransportStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CANCEL_REQUEST_TRANSPORT", new object[]
        //        {
        //            0,
        //            req.OrderID,
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
        //                    res.Message = "Thành công.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelTransportStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        public static ZResponse CancelOrderStatus(ZOrderCancelOrderStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CANCEL_ORDER_STATUS", new object[]
                {
                    0,
                    req.OrderID,
                    req.ReasonCancel,
                    req.ShopID,
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
                                    res.Message = "Trạng thái không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Thêm lịch sử phiếu xuất kho thất bại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Thêm lịch sử đơn hàng thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelOrderStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse CancelOrderInfo(ZOrderCancelOrderInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderCancelOrderInfoResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CANCEL_ORDER_INFO", new object[]
                {
                    req.OrderID,            //P_ORDER_ID            NUMBER
                    req.ShopID,             //P_SHOP_ID             NUMBER
                    0,                      //P_RETURN_CODE         OUT NUMBER
                    0                       //P_OUT_CURSOR          OUT T_CURSOR
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
                                    item = new ZOrderCancelOrderInfoResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        OrderStatus = Convert.ToString(dr["ORDER_STATUS"]),
                                        CrmOrPaypostCode = Convert.ToString(dr["CRM_OR_PAYPOST_CODE"]),
                                        ShippingCode = Convert.ToString(dr["SHIPPING_CODE"]),
                                        ShippingID = Convert.ToString(dr["SHIPPING_ID"]),
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelOrderInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion

        public static ZResponse UpdatePrintStatus(int ID, int UpdatedBy)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.UPDATE_PRINT_STATUS", new object[]
                {
                    0,
                    ID
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
                                    res.Message = "Trạng thái in không hợp lệ.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.UpdatePrintStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SearchHistory(ZOrderSearchHistoryRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZOrderSearchHistoryResponse> list = new List<ZOrderSearchHistoryResponse>();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_HISTORY_PKG.SEARCH_ORDER_HISTORY", new object[]
                {
                    req.OrderID,     //P_ORDER_ID    NUMBER
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
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
                                    list.Add(new ZOrderSearchHistoryResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        StatusName = XData.GetNameOrderStatus(dr["STATUS"].ToString()),
                                        StatusChar = dr["STATUS"].ToString(),
                                        Description = dr["DESCRIPTION"].ToString(),
                                        Reason = dr["REASON"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        StatusTimePartner = dr["STATUS_TIME_PARTNER"].ToString(),
                                        PosCode = dr["POS_CODE"].ToString(),
                                        PosName = dr["POS_NAME"].ToString(),
                                        PosAddress = dr["POS_ADDRESS"].ToString(),
                                        PosTel = dr["POS_TEL"].ToString(),
                                        StatusNote = dr["STATUS_NOTE"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchHistory: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SearchHistoryDelivery(ZOrderSearchHistoryDeliveryRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZOrderSearchHistoryDeliveryResponse> list = new List<ZOrderSearchHistoryDeliveryResponse>();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_HISTORY_PKG.SEARCH_ORDER_HISTORY_DELIVERY", new object[]
                {
                    req.ShippingCode,     //P_ORDER_ID    NUMBER
                    0,               //P_RETURN_CODE OUT NUMBER
                    0                //P_OUT_CURSOR  OUT T_CURSOR
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
                                    list.Add(new ZOrderSearchHistoryDeliveryResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        DeliveryIndex = Convert.ToInt32(dr["DELIVERYINDEX"]),
                                        DeliveryDate = dr["DELIVERY_DATE"].ToString(),
                                        InputDate = dr["INPUT_DATE"].ToString(),
                                        PostmanName = dr["POSTMAN_NAME"].ToString(),
                                        PostOffice = dr["POST_OFFICE"].ToString(),
                                        ReasonDeliveryFailure = dr["REASON_DELIVERY_FAILURE"].ToString(),
                                        SolutionName = dr["SOLUTION_NAME"].ToString(),
                                        DeliveryNote = dr["DELIVERY_NOTE"].ToString(),
                                        DeliveryReturn = Convert.ToInt32(dr["DELIVERY_RETURN"]),
                                        IsDeliverable = Convert.ToInt32(dr["IS_DELIVER_ABLE"]),
                                        RealReciverName = dr["REAL_RECIVER_NAME"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchHistory: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #region Gửi vận chuyển
        public static ZResponse SendTransport(ZOrderSendTransportRequest req)
        {
            //ZResponse res = new ZResponse();
            //try
            //{
            //    ZResponse resUpdateTransport = new ZResponse();
            //    var resSendRequestTransport = XCommon.SendTransportOrder(req.AmndState, req.OrderID, req.ShopID);
            //    if (resSendRequestTransport.Code == "00")
            //    {
            //        resUpdateTransport = SendTransportSuccess(new ZOrderSendTransportSuccessRequest() { AmndState = req.AmndState, OrderID = req.OrderID, ShopID = req.ShopID, UpdatedBy = req.CreatedBy });
            //    }
            //    else
            //    {
            //        resUpdateTransport = SendTransportError(new ZOrderSendTransportErrorRequest() { AmndState = req.AmndState, OrderID = req.OrderID, ShopID = req.ShopID });
            //    }

            //    if (resUpdateTransport.Code == "00")
            //    {
            //        res.Code = resSendRequestTransport.Code;
            //        res.Message = resSendRequestTransport.Message;
            //    }
            //    else
            //    {
            //        res.Code = resUpdateTransport.Code;
            //        res.Message = resUpdateTransport.Message;
            //        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("OrderRepository.SendTransport: request={0}||response={1}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(resUpdateTransport)));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SendTransport: " + ex.Message);
            //    res.Code = "99";
            //    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            //}
            //res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            //return res;
            ZResponse res = new ZResponse();

            try
            {
                var mongoRepo = new TransportMongoRepository();

                var queueItem = mongoRepo.InsertPending(req);

                res.Code = "00";
                res.Message = "Tạo đơn hàng thành công. Yêu cầu gửi vận chuyển đã được đưa vào hàng đợi.";
                res.Data = queueItem.Id.ToString();
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SendTransport.EnqueueMongo: " + ex.Message);

                res.Code = "99";
                res.Message = "Lỗi ghi hàng đợi gửi vận chuyển.: " + ex.Message;
            }

            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            return res;
        }

        public static ZResponse SendTransportInfo(ZOrderSendTransportInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderSendTransportInfoResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.SEND_TRANSPORT_INFO", new object[]
                {
                    req.OrderID,                                                //P_ORDER_ID N      NUMBER
                    req.ShopID,                                                 //P_SHOP_ID         NUMBER
                    string.IsNullOrEmpty(req.AmndState) ? "A" : req.AmndState,  //P_AMND_STATE      VARCHAR
                    0,                                                          //P_RETURN_CODE     OUT NUMBER
                    0                                                           //P_OUT_CURSOR      OUT T_CURSOR
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
                                    item = new ZOrderSendTransportInfoResponse()
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
                                    };
                                }
                            }
                            if (item != null)
                            {
                                if (item.OrderStatus != "N" && item.OrderStatus != "L")
                                {
                                    var notifyStatus = XData.GetNameOrderStatus(item.OrderStatus);
                                    res.Code = "02";
                                    res.Message = "Đơn hàng đang ở trạng thái " + notifyStatus + ".";
                                }
                                else if (item.Transport == "N")
                                {
                                    res.Code = "03";
                                    res.Message = "Đơn hàng không được chọn vận chuyển.";
                                }
                                else if (string.IsNullOrEmpty(item.ShippingCode))
                                {
                                    res.Code = "04";
                                    res.Message = "Mã bưu gửi bị trống.";
                                }
                                else
                                {
                                    res.Code = "00";
                                    res.Message = "Lấy dữ liệu thành công";
                                    res.Data = JsonConvert.SerializeObject(item);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SendTransportInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SendTransportData(ZOrderSendTransportDataRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.SEND_TRANSPORT_DATA", new object[]
                {
                    0,
                    req.EMSOneCode,
                    req.OrderID,
                    req.ShopID,
                    req.UpdatedBy,
                    0,
                    0
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
                            res.Data = dbCommand.Parameters["P_OUT_ORDER_ID"].Value.ToString();

                            var resOrderGetByID = OrderRepository.GetByID(new ZOrderGetByIDRequest() { ID = Convert.ToInt32(res.Data), ShopID = req.ShopID });

                            if(resOrderGetByID.Code == "00")
                            {
                                var itemById = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderGetByID.Data);
                                Task.Run(() =>
                                {
                                    try
                                    {
                                        ZSendReceiverInforRequest reqSendReceiverInfor = new ZSendReceiverInforRequest()
                                        {
                                            ItemCode = itemById.ShippingCode,
                                            OrderCode = itemById.EMSOneCode,
                                            CustomerCode = itemById.CrmOrPayPostCode,
                                            SenderName = itemById.ShipmentDetails.PickupFullName,
                                            SenderPhone = itemById.ShipmentDetails.PickupMobileNumber,
                                            SenderAddress = itemById.ShipmentDetails.PickupStreet,
                                            ReceiverName = itemById.ShipmentDetails.ReceiverName,
                                            ReceiverPhone = itemById.ShipmentDetails.MobileNumber,
                                            ReceiverAddress = itemById.ShipmentDetails.FullAddress,
                                            Channel = "EMSONE",
                                            Extend_1 = itemById.Message,

                                        };
                                        var resSendReceiverInfor = EMSRepository.SendReceiverInfor(reqSendReceiverInfor);
                                    }
                                    catch
                                    {
                                        // Bỏ qua luôn hoặc ghi log nếu muốn
                                    }
                                });
                            }
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
                                    res.Message = "Đơn hàng không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Trạng thái không hợp lệ.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Sản phẩm không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = string.Format("Sản phẩm {0} không tồn tại trong kho.", ProductVariantName);
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = string.Format("Sản phẩm {0} trong kho không đủ.", ProductVariantName);
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = string.Format("Sản phẩm {0} trong kho đã hết.", ProductVariantName);
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Tạo mã bưu gửi thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SendTransportData: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SendTransportSuccess(ZOrderSendTransportSuccessRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.SEND_TRANSPORT_SUCCESS", new object[]
                {
                    0,
                    req.OrderID,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SendTransportSuccess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SendTransportError(ZOrderSendTransportErrorRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.SEND_TRANSPORT_ERROR", new object[]
                {
                    0,
                    req.OrderID,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SendTransportError: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region Hủy vận chuyển
        public static ZResponse CancelTransport(ZOrderCancelTransportRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resCancelTransportData = CancelTransportData(new ZOrderCancelTransportDataRequest()
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
                        resCancelTransportUpdate = CancelTransportSuccess(new ZOrderCancelTransportSuccessRequest()
                        {
                            OrderID = req.OrderID,
                            ShopID = req.ShopID,
                            UpdatedBy = req.UpdatedBy
                        });
                    }
                    else
                    {
                        resCancelTransportUpdate = CancelTransportError(new ZOrderCancelTransportErrorRequest()
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
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("OrderRepository.CancelTransport: request={0}||response={1}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(resCancelTransportUpdate)));
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelTransport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //public static ZResponse CancelTransport(ZOrderCancelTransportRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        ZResponse resUpdateTransport = new ZResponse();
        //        var resSendRequestTransport = XCommon.CancelTransportOrder(req.OrderID, req.ShopID, "", "", "");
        //        if (resSendRequestTransport.Code == "00")
        //        {
        //            resUpdateTransport = CancelTransportSuccess(new ZOrderCancelTransportSuccessRequest() { OrderID = req.OrderID, ShopID = req.ShopID, UpdatedBy = req.UpdatedBy });
        //        }
        //        else
        //        {
        //            resUpdateTransport = CancelTransportError(new ZOrderCancelTransportErrorRequest() { OrderID = req.OrderID, ShopID = req.ShopID });
        //        }

        //        if (resUpdateTransport.Code == "00")
        //        {
        //            res.Code = resSendRequestTransport.Code;
        //            res.Message = resSendRequestTransport.Message;
        //        }
        //        else
        //        {
        //            res.Code = resUpdateTransport.Code;
        //            res.Message = resUpdateTransport.Message;
        //            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("OrderRepository.CancelTransport: request={0}||response={1}", JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(resUpdateTransport)));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelTransport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //public static ZResponse CancelTransport(ZOrderCancelRequestTransportRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var resSearchInfoCancelRequestTransport = CancelTransportInfo(new ZOrderCancelTransportInfoRequest()
        //        {
        //            OrderID = req.OrderID,
        //            ShopID = req.ShopID,
        //        });
        //        if (resSearchInfoCancelRequestTransport.Code == "00")
        //        {
        //            var itemSearchInfoCancelRequestTransport = JsonConvert.DeserializeObject<ZOrderCancelTransportInfoResponse>(resSearchInfoCancelRequestTransport.Data);
        //            if (itemSearchInfoCancelRequestTransport.Status == "W" || itemSearchInfoCancelRequestTransport.Status == "P")
        //            {
        //                ZTransportShippingCancelDataRequest reqCancelRequestTransport = new ZTransportShippingCancelDataRequest()
        //                {
        //                    CustomerCode = itemSearchInfoCancelRequestTransport.CrmOrPaypostCode,
        //                    TrackingCode = itemSearchInfoCancelRequestTransport.ShippingCode,
        //                    SignatureCode = Security.SHA256(itemSearchInfoCancelRequestTransport.ShippingCode + itemSearchInfoCancelRequestTransport.CrmOrPaypostCode + XHelper.EMSTransportKey).ToUpper()
        //                };
        //                var resCancelRequestTransport = XCommon.CancelRequestTransport(reqCancelRequestTransport);
        //                if (resCancelRequestTransport.Code == "00")
        //                {
        //                    ZOrderCancelTransportStatusRequest reqCancelTransportStatus = new ZOrderCancelTransportStatusRequest()
        //                    {
        //                        OrderID = req.OrderID,
        //                        ShopID = req.ShopID,
        //                        UpdatedBy = req.UpdatedBy
        //                    };
        //                    var resCancelTransportStatus = CancelTransportStatus(reqCancelTransportStatus);
        //                    if (resCancelTransportStatus.Code == "00")
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Hủy yêu cầu vận chuyển thành công";
        //                    }
        //                    else
        //                    {
        //                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("OrderRepository.CancelTransport: CancelTransportStatus||req={0}||res={1}", JsonConvert.SerializeObject(reqCancelTransportStatus), JsonConvert.SerializeObject(resCancelTransportStatus)));
        //                        res.Code = resCancelTransportStatus.Code;
        //                        res.Message = resCancelTransportStatus.Message;
        //                        res.Data = JsonConvert.SerializeObject(new ZOrderCancelRequestTransportResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = resCancelTransportStatus.Message });
        //                    }
        //                }
        //                else
        //                {
        //                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("OrderRepository.CancelTransport: CancelRequestTransport||req={0}||res={1}", JsonConvert.SerializeObject(reqCancelRequestTransport), JsonConvert.SerializeObject(resCancelRequestTransport)));
        //                    res.Code = resCancelRequestTransport.Code;
        //                    res.Message = resCancelRequestTransport.Message;
        //                    res.Data = JsonConvert.SerializeObject(new ZOrderCancelRequestTransportResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = resCancelRequestTransport.Message });
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "01";
        //                res.Message = "Trạng thái đơn hàng không hợp lệ.";
        //                res.Data = JsonConvert.SerializeObject(new ZOrderCancelRequestTransportResponse() { OrderCode = itemSearchInfoCancelRequestTransport.OrderCode, Description = string.Format("Đơn hàng đang ở trạng thái {0}.", XData.GetNameOrderStatus(itemSearchInfoCancelRequestTransport.Status)) });
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resSearchInfoCancelRequestTransport.Code;
        //            res.Message = resSearchInfoCancelRequestTransport.Message;
        //            res.Data = JsonConvert.SerializeObject(new ZOrderCancelRequestTransportResponse() { OrderCode = "", Description = "Không tồn tại dữ liệu" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelTransport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        public static ZResponse CancelTransportInfo(ZOrderCancelTransportInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderCancelTransportInfoResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CANCEL_TRANSPORT_INFO", new object[]
                {
                    req.OrderID,            //P_ORDER_ID            NUMBER
                    req.ShopID,             //P_SHOP_ID             NUMBER
                    req.CrmOrPaypostCode,   //P_CRM_OR_PAYPOST_CODE VARCHAR
                    req.OrderCode,          //P_ORDER_CODE          VARCHAR
                    req.ShippingCode,       //P_SHIPPING_CODE       VARCHAR
                    0,                      //P_RETURN_CODE         OUT NUMBER
                    0                       //P_OUT_CURSOR          OUT T_CURSOR
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
                                    item = new ZOrderCancelTransportInfoResponse()
                                    {
                                        OrderID = string.IsNullOrEmpty(dr["ORDER_ID"].ToString()) ? 0 : Convert.ToInt32(dr["ORDER_ID"]),
                                        Status = dr["STATUS"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        ShopID = string.IsNullOrEmpty(dr["SHOP_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SHOP_ID"]),
                                        CrmOrPaypostCode = dr["CRM_OR_PAYPOST_CODE"].ToString(),
                                        EmployeeID = string.IsNullOrEmpty(dr["EMPLOYEE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["EMPLOYEE_ID"]),
                                        ShippingID = dr["SHIPPING_ID"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelTransportInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse CancelTransportData(ZOrderCancelTransportDataRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CANCEL_TRANSPORT_DATA", new object[]
                {
                    0,
                    req.OrderID,
                    req.ShopID,
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
                                    res.Message = "Trạng thái đơn hàng không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Trạng thái phiếu xuất kho không hợp lệ.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Sản phẩm không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelTransportData: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse CancelTransportSuccess(ZOrderCancelTransportSuccessRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CANCEL_TRANSPORT_SUCCESS", new object[]
                {
                    0,
                    req.OrderID,
                    req.ShopID,
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
                                    res.Message = "Lưu lịch sử đơn hàng thất bại.";
                                    break;
                                case -3:
                                    res.Code = "03";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelTransportSuccess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse CancelTransportError(ZOrderCancelTransportErrorRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CANCEL_TRANSPORT_ERROR", new object[]
                {
                    0,
                    req.OrderID,
                    req.ShopID,
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
                                    res.Message = "Sản phẩm không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelTransportError: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        //public static ZResponse CheckOrderCode(ZOrderCheckCodeRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CHECK_ORDER_CODE", new object[]
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
        //                    res.Message = "Không tồn tại đơn hàng.";
        //                }
        //                else
        //                {
        //                    res.Code = "01";
        //                    res.Message = "Đã tồn tại đơn hàng.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CheckOrderCode: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        #region In đơn hàng
        public static ZResponse HandlePrint(ZOrderPrintRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var reqOrderPrint = OrderPrint(req);
                if (reqOrderPrint.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<ZOrderPrintResponse>(reqOrderPrint.Data);
                    if (req.PrintType == "A61" || req.PrintType == "A64")
                    {
                        req.PrintType = "A6";
                    }
                    if(req.PrintType == "E5")
                    {
                        req.PrintType = "B1";
                    }
                    var reqTemplatePrint = TemplatePrintRepository.SearchList(new ZTemplatePrintSearchListRequest()
                    {
                        PrintType = req.PrintType,
                        ShopID = req.ShopID,
                        TemplateType = "ORDER"
                    });
                    if (reqTemplatePrint.Code == "00")
                    {
                        var resTemplatePrint = JsonConvert.DeserializeObject<List<ZTemplatePrintSearchListResponse>>(reqTemplatePrint.Data);
                        var FilterResTemplatePrint = resTemplatePrint.Where(m => m.IsChoose == "N" && m.TemplatePrintParent > 0).ToList();
                        if (FilterResTemplatePrint.Count > 0)
                        {
                            if (req.PrintType == "A4")
                            {
                                foreach (var itemTemplatePrint in FilterResTemplatePrint)
                                {
                                    switch (itemTemplatePrint.TemplatePrintID)
                                    {
                                        case 4:
                                            item.SenderFullName = "";
                                            break;
                                        case 5:
                                            item.SenderFullAddress = "";
                                            break;
                                        case 6:
                                            item.SenderDistrictName = "";
                                            break;
                                        case 7:
                                            item.SenderProvinceName = "";
                                            break;
                                        case 8:
                                            item.SenderMobileNumber = "";
                                            break;

                                        case 11:
                                            item.ReceiverDistrictName = "";
                                            break;
                                        case 12:
                                            item.ReceiverProvinceName = "";
                                            break;
                                        case 13:
                                            item.ReceiverMobileNumber = "";
                                            break;

                                        case 14:
                                            item.OrderCode = "";
                                            item.OrderBarCode = null;
                                            break;

                                        case 17:
                                            item.TransportWeight = "0";
                                            break;
                                        case 18:
                                            item.TransportQuantity = "0";
                                            break;
                                    }
                                }
                            }
                            else if (req.PrintType == "A6")
                            {
                                foreach (var itemTemplatePrint in FilterResTemplatePrint)
                                {
                                    switch (itemTemplatePrint.TemplatePrintID)
                                    {
                                        case 25:
                                            item.ReceiverMobileNumber = "";
                                            break;
                                        //case 26:
                                        //    item.TransportFee = 0;
                                        //    break;
                                        case 27:
                                            item.Message = "";
                                            break;

                                        case 29:
                                            item.Note = "";
                                            break;

                                        //case 30:
                                        //    //item.PostOffice = item.PickUpProvinceName;
                                        //    item.SenderFullName = "";
                                        //    item.SenderMobileNumber = "";
                                        //    item.SenderFullAddress = "";
                                        //    break;
                                        case 101:
                                            item.SenderFullName = "";
                                            break;
                                        case 102:
                                            item.SenderMobileNumber = "";
                                            break;
                                        case 103:
                                            item.SenderFullAddress = "";
                                            break;

                                        case 104:
                                            item.ReceiverFullAddress = "";
                                            break;

                                        case 106:
                                            item.TransportMainServiceCode = "";
                                            break;
                                        case 107:
                                            item.TransportMainServiceName = "";
                                            break;

                                        case 108:
                                            item.TransportExtraServiceCode = "";
                                            break;
                                    }
                                }
                            }
                            else if (req.PrintType == "B1" || req.PrintType == "E5")
                            {
                                foreach (var itemTemplatePrint in FilterResTemplatePrint)
                                {
                                    switch (itemTemplatePrint.TemplatePrintID)
                                    {
                                        case 40:
                                            //item.PostOffice = item.PickUpProvinceName;
                                            item.SenderFullName = "";
                                            break;
                                        //case 42:
                                        //    item.PostOfficeCode = "";
                                        //    item.PostOfficeName = "";
                                        //    break;
                                        case 43:
                                            item.OrderCode = "";
                                            item.OrderBarCode = null;
                                            break;
                                        case 45:
                                            item.ReceiverMobileNumber = "";
                                            break;
                                            //case 47:
                                            //    TotalAmount
                                            //    break;
                                    }
                                }
                            }
                            else if (req.PrintType == "B2")
                            {
                                foreach (var itemTemplatePrint in FilterResTemplatePrint)
                                {
                                    switch (itemTemplatePrint.TemplatePrintID)
                                    {
                                        case 51:
                                            //item.PostOffice = item.PickUpProvinceName;
                                            item.SenderFullName = "";
                                            break;
                                        case 53:
                                            item.ReceiverMobileNumber = "";
                                            break;
                                            //case 54:
                                            //    item.TransportFee = 0;
                                            //    break;
                                    }
                                }
                            }
                            else if (req.PrintType == "Label")
                            {
                                foreach (var itemTemplatePrint in FilterResTemplatePrint)
                                {
                                    switch (itemTemplatePrint.TemplatePrintID)
                                    {
                                        //case 58:
                                        //    //item.PostOffice = item.PickUpProvinceName;
                                        //    item.SenderFullName = "";
                                        //    item.SenderMobileNumber = "";
                                        //    item.SenderFullAddress = "";
                                        //    break;
                                        case 60:
                                            item.ReceiverMobileNumber = "";
                                            break;
                                        //case 61:
                                        //    item.TransportFee = 0;
                                        //    break;

                                        case 62:
                                            item.Message = "";
                                            break;

                                        case 64:
                                            item.Note = "";
                                            break;

                                        case 109:
                                            item.SenderFullName = "";
                                            break;
                                        case 110:
                                            item.SenderMobileNumber = "";
                                            break;
                                        case 111:
                                            item.SenderFullAddress = "";
                                            break;

                                        case 112:
                                            item.ReceiverFullAddress = "";
                                            break;

                                        case 114:
                                            item.TransportMainServiceCode = "";
                                            break;
                                        case 115:
                                            item.TransportMainServiceName = "";
                                            break;

                                        case 116:
                                            item.TransportExtraServiceCode = "";
                                            break;
                                    }
                                }
                            }
                        }
                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = JsonConvert.SerializeObject(item);
                    }
                    else
                    {
                        res.Code = reqTemplatePrint.Code;
                        res.Message = reqTemplatePrint.Message;
                    }
                }
                else
                {
                    res.Code = reqOrderPrint.Code;
                    res.Message = reqOrderPrint.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.HandlePrint: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse OrderPrint(ZOrderPrintRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderPrintResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.ORDER_PRINT", new object[]
                {
                    req.EMSOneCode,         //P_EMSONE_CODE   VARCHAR
                    req.ShopID,             //P_SHOP_ID       NUMBER
                    0,                      //P_RETURN_CODE   OUT NUMBER
                    0                       //P_OUT_CURSOR    OUT T_CURSOR
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
                                    item = new ZOrderPrintResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        BatchCode = dr["BATCH_CODE"].ToString(),
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        PickUpProvinceName = dr["PICK_UP_PROVINCE_NAME"].ToString(),
                                        SenderFullName = dr["SENDER_NAME"].ToString(),
                                        SenderMobileNumber = dr["SENDER_PHONE"].ToString(),
                                        SenderProvinceName = dr["SENDER_PROVINCE_NAME"].ToString(),
                                        SenderDistrictName = dr["SENDER_DISTRICT_NAME"].ToString(),
                                        SenderFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                        SenderSentDate = dr["SENDER_SENT_DATE"].ToString(),
                                        ReceiverFullName = dr["RECEIVER_NAME"].ToString(),
                                        ReceiverMobileNumber = dr["RECEIVER_PHONE"].ToString(),
                                        ReceiverProvinceName = dr["RECEIVER_PROVINCE_NAME"].ToString(),
                                        ReceiverDistrictName = dr["RECEIVER_DISTRICT_NAME"].ToString(),
                                        ReceiverFullAddress = dr["RECEIVER_FULL_ADDRESS"].ToString(),
                                        Message = dr["MESSAGE"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        //Message = dr["MESSAGE"].ToString().Length > 160 ? dr["MESSAGE"].ToString().Substring(0, 160) + "..." : dr["MESSAGE"].ToString(),
                                        //Note = dr["NOTE"].ToString().Length > 160 ? dr["NOTE"].ToString().Substring(0, 160) + "..." : dr["NOTE"].ToString(),
                                        TransportWeight = dr["TRANSPORT_WEIGHT"].ToString(),
                                        TransportQuantity = dr["TRANSPORT_QUANTITY"].ToString(),
                                        TransportMainServiceCode = dr["TRANSPORT_MAIN_SERVICE_CODE"].ToString(),
                                        TransportMainServiceName = dr["TRANSPORT_MAIN_SERVICE_NAME"].ToString(),
                                        TransportExtraServiceCode = dr["TRANSPORT_EXTRA_SERVICE_CODE"].ToString(),
                                        TransportExtraServiceName = dr["TRANSPORT_EXTRA_SERVICE_NAME"].ToString(),
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                        TotalPHU = Convert.ToInt64(dr["TOTAL_PHU"]),
                                        PaymentFreightVat = Convert.ToInt64(dr["PAYMENT_FREIGHT_VAT"]),
                                        TransportPayer = Convert.ToInt32(dr["TRANSPORT_PAYER"]),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        QRCode = string.IsNullOrEmpty(dr["SHIPPING_CODE"].ToString()) ? null : XCommon.GenerateQRCode(dr["SHIPPING_CODE"].ToString()),
                                        ShippingBarCode = string.IsNullOrEmpty(dr["SHIPPING_CODE"].ToString()) ? null : XCommon.GenerateBarCodeWithoutText(dr["SHIPPING_CODE"].ToString()),
                                        OrderBarCode = XCommon.GenerateBarCodeWithoutText(dr["ORDER_CODE"].ToString()),
                                        PostOfficeCode = dr["COLLECTION_POST_OFFICE_CODE"].ToString(),
                                        PostOfficeName = dr["COLLECTION_POST_OFFICE_NAME"].ToString(),
                                    };

                                    if (!string.IsNullOrEmpty(item.PostOfficeCode) && !string.IsNullOrEmpty(item.PostOfficeName))
                                    {
                                        item.PostOffice = item.PostOfficeCode + " - " + item.PostOfficeName;
                                    }
                                    else
                                    {
                                        item.PostOffice = item.SenderProvinceName;
                                    }

                                    var isPPAOrPSH = item.TransportExtraServiceCode.Contains("PPA") || item.TransportExtraServiceCode.Contains("PSH");
                                    if (isPPAOrPSH == false)
                                    {
                                        item.PaymentFreightVat = 0;
                                    }
                                    else
                                    {
                                        item.TransportFee = 0;
                                    }

                                    var isTotalPHU = item.TransportExtraServiceCode.Contains("PHU");
                                    if (isTotalPHU == true)
                                    {
                                        item.Note = string.Format("Phát không thành công thu {0} đồng. ", item.TotalPHU.ToString("N0")) + item.Note;
                                    }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.OrderPrint: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        public static ZResponse SearchProduct(ZOrderSearchProductRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZOrderSearchProductResponse> listItem = new List<ZOrderSearchProductResponse>();
                List<ZOrderSearchProductResponse> listProduct = new List<ZOrderSearchProductResponse>();
                List<ZVariantResponse> listProductVariant = new List<ZVariantResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.SEARCH_PRODUCT", new object[]
                {
                    req.Name,       //P_NAME            VARCHAR
                    req.ShopID,     //P_SHOP_ID         NUMBER
                    0,              //P_RETURN_CODE     OUT NUMBER
                    0,              //P_PRODUCT_CURSOR  OUT T_CURSOR 
                    0,              //P_VARIANT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_PRODUCT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    listProduct.Add(new ZOrderSearchProductResponse()
                                    {
                                        ProductID = Convert.ToInt32(dr["ID"]),
                                        ProductName = dr["PRODUCT_NAME"].ToString()
                                    });
                                }
                            }
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_VARIANT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    if (Convert.ToInt32(dr["QUANTITY"]) > 0)
                                    {
                                        listProductVariant.Add(new ZVariantResponse()
                                        {
                                            ProductID = Convert.ToInt32(dr["ID"]),
                                            VariantID = string.IsNullOrEmpty(dr["VARIANT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["VARIANT_ID"]),
                                            VariantName = dr["VARIANT_NAME"].ToString(),
                                            Image = dr["AVATAR"].ToString(),
                                            Price = string.IsNullOrEmpty(dr["PRICE"].ToString()) ? 0 : Convert.ToInt64(dr["PRICE"]),
                                            SKU = dr["SKU"].ToString(),
                                            Barcode = dr["BARCODE"].ToString(),
                                            Weight = Convert.ToDouble(dr["WEIGHT"]),
                                            Quantity = Convert.ToInt32(dr["QUANTITY"]),
                                        });
                                    }
                                }
                            }
                            if (listProduct.Count > 0 && listProductVariant.Count > 0)
                            {
                                foreach (var item in listProduct)
                                {
                                    if (listProductVariant.Where(n => n.ProductID == item.ProductID).Count() > 0)
                                    {
                                        item.Variants = listProductVariant.Where(n => n.ProductID == item.ProductID).ToList();
                                        listItem.Add(item);
                                    }
                                }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchProduct: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse GetByID(ZOrderGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderGetByIDResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.GET_BY_ID", new object[]
                {
                    req.ID,     //P_ORDER_ID      NUMBER
                    req.ShopID, //P_SHOP_ID       NUMBER
                    0,          //P_RETURN_CODE   OUT NUMBER
                    0           //P_OUT_CURSOR    OUT T_CURSOR
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
                                    item = new ZOrderGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        BatchCode = dr["BATCH_CODE"].ToString(),
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        Status = dr["STATUS"].ToString(),
                                        StatusName = XData.GetNameOrderStatus(dr["STATUS"].ToString()),
                                        //DeliveryNoteID = Convert.ToInt32(!string.IsNullOrEmpty(dr["DELIVERY_NOTE_ID"].ToString()) ? dr["DELIVERY_NOTE_ID"].ToString() : "0"),
                                        //DeliveryNoteCode = dr["DELIVERY_NOTE_CODE"].ToString(),
                                        //DeliveryNoteStatus = dr["DELIVERY_NOTE_STATUS"].ToString(),
                                        WareHouseID = string.IsNullOrEmpty(dr["WAREHOUSE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["WAREHOUSE_ID"].ToString()),
                                        WareHouseName = dr["WAREHOUSE_NAME"].ToString(),
                                        Customer = new ZCustomerResponse()
                                        {
                                            ID = string.IsNullOrEmpty(dr["CUSTOMER_ID"].ToString()) ? 0 : Convert.ToInt32(dr["CUSTOMER_ID"].ToString()),
                                            FullName = dr["FULL_NAME"].ToString(),
                                            MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                            Address = dr["CUSTOMER_ADDRESS"].ToString(),
                                            Email = dr["EMAIL"].ToString(),
                                            BlackList = dr["BLACK_LIST"].ToString(),
                                            GroupCode = dr["GROUP_CODE"].ToString(),
                                            GroupName = dr["GROUP_NAME"].ToString(),
                                            SuccessOrder = Convert.ToInt32(dr["SUCCESS_ORDER"]),
                                            TotalOrder = Convert.ToInt32(dr["TOTAL_ORDER"]),
                                        },
                                        TotalAmount = Convert.ToInt64(!string.IsNullOrEmpty(dr["TOTAL_AMOUNT"].ToString()) ? dr["TOTAL_AMOUNT"].ToString() : "0"),
                                        TotalDiscount = Convert.ToInt32(!string.IsNullOrEmpty(dr["TOTAL_DISCOUNT"].ToString()) ? dr["TOTAL_DISCOUNT"].ToString() : "0"),
                                        TransportFee = string.IsNullOrEmpty(dr["PROVISIONAL_TRANSPORT_FEE"].ToString()) ? 0 : Convert.ToInt32(dr["PROVISIONAL_TRANSPORT_FEE"].ToString()),
                                        ActualTransportFee = string.IsNullOrEmpty(dr["ACTUAL_TRANSPORT_FEE"].ToString()) ? 0 : Convert.ToInt32(dr["ACTUAL_TRANSPORT_FEE"].ToString()),
                                        TotalProductValue = Convert.ToInt64(!string.IsNullOrEmpty(dr["TOTAL_PRODUCT_VALUE"].ToString()) ? dr["TOTAL_PRODUCT_VALUE"].ToString() : "0"),
                                        TotalOrderValue = Convert.ToInt64(!string.IsNullOrEmpty(dr["TOTAL_ORDER_VALUE"].ToString()) ? dr["TOTAL_ORDER_VALUE"].ToString() : "0"),
                                        TotalCOD = string.IsNullOrEmpty(dr["TOTAL_COD"].ToString()) ? 0 : Convert.ToInt64(dr["TOTAL_COD"].ToString()),
                                        ActualTotalCOD = string.IsNullOrEmpty(dr["ACTUAL_TOTAL_COD"].ToString()) ? 0 : Convert.ToInt64(dr["ACTUAL_TOTAL_COD"].ToString()),
                                        TotalPHU = string.IsNullOrEmpty(dr["TOTAL_PHU"].ToString()) ? 0 : Convert.ToInt64(dr["TOTAL_PHU"].ToString()),
                                        //PaymentStatus = dr["PAYMENT_STATUS"].ToString(),
                                        //PaymentMethod = Convert.ToInt32(dr["PAYMENT_METHOD"]),
                                        Transport = dr["TRANSPORT"].ToString(),
                                        Channel = dr["CHANNEL"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        Message = dr["MESSAGE"].ToString(),
                                        ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                                        {
                                            ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                            MobileNumber = dr["RECEIVER_MOBILE_NUMBER"].ToString(),
                                            ProvinceID = Convert.ToInt32(!string.IsNullOrEmpty(dr["PROVINCE_ID"].ToString()) ? dr["PROVINCE_ID"].ToString() : "0"),
                                            ProvinceName = dr["PROVINCE_NAME"].ToString(),
                                            DistrictID = Convert.ToInt32(!string.IsNullOrEmpty(dr["DISTRICT_ID"].ToString()) ? dr["DISTRICT_ID"].ToString() : "0"),
                                            DistrictName = dr["DISTRICT_NAME"].ToString(),
                                            WardID = Convert.ToInt32(!string.IsNullOrEmpty(dr["WARD_ID"].ToString()) ? dr["WARD_ID"].ToString() : "0"),
                                            WardName = dr["WARD_NAME"].ToString(),
                                            Address = dr["ADDRESS"].ToString(),
                                            FullAddress = dr["FULL_ADDRESS"].ToString(),
                                            PickUpAddressID = string.IsNullOrEmpty(dr["PICK_UP_ADDRESS_ID"].ToString()) ? 0 : Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"].ToString()),
                                            PickupFullName = dr["SENDER_NAME"].ToString(),
                                            PickupMobileNumber = dr["SENDER_PHONE"].ToString(),
                                            PickupProvinceID = string.IsNullOrEmpty(dr["SENDER_PROVINCE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_PROVINCE_ID"].ToString()),
                                            PickupProvinceName = dr["SENDER_PROVINCE_NAME"].ToString(),
                                            PickupDistrictID = string.IsNullOrEmpty(dr["SENDER_DISTRICT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_DISTRICT_ID"].ToString()),
                                            PickupDistrictName = dr["SENDER_DISTRICT_NAME"].ToString(),
                                            PickupWardID = string.IsNullOrEmpty(dr["SENDER_WARD_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_WARD_ID"].ToString()),
                                            PickupWardName = dr["SENDER_WARD_NAME"].ToString(),
                                            PickupStreet = dr["SENDER_STREET"].ToString(),
                                            PickupFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                        },
                                        //IsDeliveryNote = dr["IS_DELIVERY_NOTE"].ToString(),
                                        ReasonCancel = dr["REASON_CANCEL"].ToString(),
                                        UpdatedDate = dr["UPDATED_DATE"].ToString(),
                                        UpdatedName = dr["UPDATED_NAME"].ToString(),
                                        TransportOrderName = dr["TRANSPORT_ORDER_NAME"].ToString(),
                                        TransportWeight = string.IsNullOrEmpty(dr["PROVISIONAL_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["PROVISIONAL_TRANSPORT_WEIGHT"].ToString()),
                                        ActualTransportWeight = string.IsNullOrEmpty(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()),
                                        ExchangeTransportWeight = string.IsNullOrEmpty(dr["EXCHANGE_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["EXCHANGE_TRANSPORT_WEIGHT"].ToString()),
                                        TransportSizeLength = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_LENGTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"].ToString()),
                                        TransportSizeWidth = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_WIDTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"].ToString()),
                                        TransportSizeHeight = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_HEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"].ToString()),
                                        TransportQuantity = string.IsNullOrEmpty(dr["TRANSPORT_QUANTITY"].ToString()) ? 0 : Convert.ToInt64(dr["TRANSPORT_QUANTITY"].ToString()),
                                        TransportOrderValue = string.IsNullOrEmpty(dr["TRANSPORT_ORDER_VALUE"].ToString()) ? 0 : Convert.ToInt64(dr["TRANSPORT_ORDER_VALUE"].ToString()),
                                        TransportMainService = string.IsNullOrEmpty(dr["TRANSPORT_MAIN_SERVICE"].ToString()) ? 0 : Convert.ToInt32(dr["TRANSPORT_MAIN_SERVICE"].ToString()),
                                        TransportMainServiceName = dr["TRANSPORT_MAIN_SERVICE_NAME"].ToString(),
                                        TransportExtraService = dr["TRANSPORT_EXTRA_SERVICE"].ToString(),
                                        TransportExtraServiceName = dr["TRANSPORT_EXTRA_SERVICE_NAMES"].ToString(),
                                        TransportPayer = string.IsNullOrEmpty(dr["TRANSPORT_PAYER"].ToString()) ? 0 : Convert.ToInt32(dr["TRANSPORT_PAYER"].ToString()),
                                        TransportPayerName = string.IsNullOrEmpty(dr["TRANSPORT_PAYER"].ToString()) ? "" : XData.GetNameTransportPayer(dr["TRANSPORT_PAYER"].ToString()),
                                        //RequestSupportUpdateInformation = dr["REQ_SUP_UPDATE_INFORMATION"].ToString(),
                                        CollectionType = new ZBaseResponse()
                                        {
                                            ID = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                            Code = dr["COLLECTION_TYPE"].ToString(),
                                            Name = XData.GetNameCollectionType(dr["COLLECTION_TYPE"].ToString())
                                        },
                                        TypeShop = string.IsNullOrEmpty(dr["TYPE_SHOP"].ToString()) ? 0 : Convert.ToInt32(dr["TYPE_SHOP"].ToString()),
                                        AcceptancePosCode = dr["ACCEPTANCE_POSCODE"].ToString(),
                                        DeviceName = dr["DEVICE_NAME"].ToString(),
                                        OrderItems = new List<ZOrderItemGetByOrderIDResponse>(),
                                        ListUpdateInformation = new List<ZRequestSupportHistorySearchListUpdateInformationResponse>(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        ExpectedDeliveryDate = dr["EXPECTED_DELIVERY_DATE"].ToString(),
                                        CollectionPostOfficeRecent = new ZCollectionPostOfficeSearchListResponse() 
                                        {
                                            ID = string.IsNullOrEmpty(dr["CO_PO_RECENT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["CO_PO_RECENT_ID"].ToString()),
                                            FullAddress = dr["CO_PO_RECENT_FULL_ADDRESS"].ToString(),
                                        },
                                        CrmOrPayPostCode = dr["CUSTOMER_CODE"].ToString()
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
                                res.Message = "Không tồn tại dữ liệu";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse GetByOrderCode(ZOrderGetByOrderCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderGetByIDResponse item = null;
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.GET_BY_ORDER_CODE", new object[]
                {
                    req.OrderCode,     //P_ORDER_CODE      VARCHAR2
                    req.ShopID, //P_SHOP_ID       NUMBER
                    0,          //P_RETURN_CODE   OUT NUMBER
                    0           //P_OUT_CURSOR    OUT T_CURSOR
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
                                    item = new ZOrderGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        BatchCode = dr["BATCH_CODE"].ToString(),
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        Status = dr["STATUS"].ToString(),
                                        StatusName = XData.GetNameOrderStatus(dr["STATUS"].ToString()),
                                        //DeliveryNoteID = Convert.ToInt32(!string.IsNullOrEmpty(dr["DELIVERY_NOTE_ID"].ToString()) ? dr["DELIVERY_NOTE_ID"].ToString() : "0"),
                                        //DeliveryNoteCode = dr["DELIVERY_NOTE_CODE"].ToString(),
                                        //DeliveryNoteStatus = dr["DELIVERY_NOTE_STATUS"].ToString(),
                                        WareHouseID = string.IsNullOrEmpty(dr["WAREHOUSE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["WAREHOUSE_ID"].ToString()),
                                        WareHouseName = dr["WAREHOUSE_NAME"].ToString(),
                                        Customer = new ZCustomerResponse()
                                        {
                                            ID = string.IsNullOrEmpty(dr["CUSTOMER_ID"].ToString()) ? 0 : Convert.ToInt32(dr["CUSTOMER_ID"].ToString()),
                                            FullName = dr["FULL_NAME"].ToString(),
                                            MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                            Address = dr["CUSTOMER_ADDRESS"].ToString(),
                                            Email = dr["EMAIL"].ToString(),
                                            BlackList = dr["BLACK_LIST"].ToString(),
                                            GroupCode = dr["GROUP_CODE"].ToString(),
                                            GroupName = dr["GROUP_NAME"].ToString(),
                                            SuccessOrder = Convert.ToInt32(dr["SUCCESS_ORDER"]),
                                            TotalOrder = Convert.ToInt32(dr["TOTAL_ORDER"]),
                                        },
                                        TotalAmount = Convert.ToInt64(!string.IsNullOrEmpty(dr["TOTAL_AMOUNT"].ToString()) ? dr["TOTAL_AMOUNT"].ToString() : "0"),
                                        TotalDiscount = Convert.ToInt32(!string.IsNullOrEmpty(dr["TOTAL_DISCOUNT"].ToString()) ? dr["TOTAL_DISCOUNT"].ToString() : "0"),
                                        TransportFee = string.IsNullOrEmpty(dr["PROVISIONAL_TRANSPORT_FEE"].ToString()) ? 0 : Convert.ToInt32(dr["PROVISIONAL_TRANSPORT_FEE"].ToString()),
                                        ActualTransportFee = string.IsNullOrEmpty(dr["ACTUAL_TRANSPORT_FEE"].ToString()) ? 0 : Convert.ToInt32(dr["ACTUAL_TRANSPORT_FEE"].ToString()),
                                        TotalProductValue = Convert.ToInt64(!string.IsNullOrEmpty(dr["TOTAL_PRODUCT_VALUE"].ToString()) ? dr["TOTAL_PRODUCT_VALUE"].ToString() : "0"),
                                        TotalOrderValue = Convert.ToInt64(!string.IsNullOrEmpty(dr["TOTAL_ORDER_VALUE"].ToString()) ? dr["TOTAL_ORDER_VALUE"].ToString() : "0"),
                                        TotalCOD = string.IsNullOrEmpty(dr["TOTAL_COD"].ToString()) ? 0 : Convert.ToInt64(dr["TOTAL_COD"].ToString()),
                                        ActualTotalCOD = string.IsNullOrEmpty(dr["ACTUAL_TOTAL_COD"].ToString()) ? 0 : Convert.ToInt64(dr["ACTUAL_TOTAL_COD"].ToString()),
                                        TotalPHU = string.IsNullOrEmpty(dr["TOTAL_PHU"].ToString()) ? 0 : Convert.ToInt64(dr["TOTAL_PHU"].ToString()),
                                        //PaymentStatus = dr["PAYMENT_STATUS"].ToString(),
                                        //PaymentMethod = Convert.ToInt32(dr["PAYMENT_METHOD"]),
                                        Transport = dr["TRANSPORT"].ToString(),
                                        Channel = dr["CHANNEL"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        Message = dr["MESSAGE"].ToString(),
                                        ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                                        {
                                            ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                            MobileNumber = dr["RECEIVER_MOBILE_NUMBER"].ToString(),
                                            ProvinceID = Convert.ToInt32(!string.IsNullOrEmpty(dr["PROVINCE_ID"].ToString()) ? dr["PROVINCE_ID"].ToString() : "0"),
                                            ProvinceName = dr["PROVINCE_NAME"].ToString(),
                                            DistrictID = Convert.ToInt32(!string.IsNullOrEmpty(dr["DISTRICT_ID"].ToString()) ? dr["DISTRICT_ID"].ToString() : "0"),
                                            DistrictName = dr["DISTRICT_NAME"].ToString(),
                                            WardID = Convert.ToInt32(!string.IsNullOrEmpty(dr["WARD_ID"].ToString()) ? dr["WARD_ID"].ToString() : "0"),
                                            WardName = dr["WARD_NAME"].ToString(),
                                            Address = dr["ADDRESS"].ToString(),
                                            FullAddress = dr["FULL_ADDRESS"].ToString(),
                                            PickUpAddressID = string.IsNullOrEmpty(dr["PICK_UP_ADDRESS_ID"].ToString()) ? 0 : Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"].ToString()),
                                            PickupFullName = dr["SENDER_NAME"].ToString(),
                                            PickupMobileNumber = dr["SENDER_PHONE"].ToString(),
                                            PickupProvinceID = string.IsNullOrEmpty(dr["SENDER_PROVINCE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_PROVINCE_ID"].ToString()),
                                            PickupProvinceName = dr["SENDER_PROVINCE_NAME"].ToString(),
                                            PickupDistrictID = string.IsNullOrEmpty(dr["SENDER_DISTRICT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_DISTRICT_ID"].ToString()),
                                            PickupDistrictName = dr["SENDER_DISTRICT_NAME"].ToString(),
                                            PickupWardID = string.IsNullOrEmpty(dr["SENDER_WARD_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_WARD_ID"].ToString()),
                                            PickupWardName = dr["SENDER_WARD_NAME"].ToString(),
                                            PickupStreet = dr["SENDER_STREET"].ToString(),
                                            PickupFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                        },
                                        //IsDeliveryNote = dr["IS_DELIVERY_NOTE"].ToString(),
                                        ReasonCancel = dr["REASON_CANCEL"].ToString(),
                                        UpdatedDate = dr["UPDATED_DATE"].ToString(),
                                        UpdatedName = dr["UPDATED_NAME"].ToString(),
                                        TransportOrderName = dr["TRANSPORT_ORDER_NAME"].ToString(),
                                        TransportWeight = string.IsNullOrEmpty(dr["PROVISIONAL_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["PROVISIONAL_TRANSPORT_WEIGHT"].ToString()),
                                        ActualTransportWeight = string.IsNullOrEmpty(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()),
                                        ExchangeTransportWeight = string.IsNullOrEmpty(dr["EXCHANGE_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["EXCHANGE_TRANSPORT_WEIGHT"].ToString()),
                                        TransportSizeLength = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_LENGTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"].ToString()),
                                        TransportSizeWidth = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_WIDTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"].ToString()),
                                        TransportSizeHeight = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_HEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"].ToString()),
                                        TransportQuantity = string.IsNullOrEmpty(dr["TRANSPORT_QUANTITY"].ToString()) ? 0 : Convert.ToInt64(dr["TRANSPORT_QUANTITY"].ToString()),
                                        TransportOrderValue = string.IsNullOrEmpty(dr["TRANSPORT_ORDER_VALUE"].ToString()) ? 0 : Convert.ToInt64(dr["TRANSPORT_ORDER_VALUE"].ToString()),
                                        TransportMainService = string.IsNullOrEmpty(dr["TRANSPORT_MAIN_SERVICE"].ToString()) ? 0 : Convert.ToInt32(dr["TRANSPORT_MAIN_SERVICE"].ToString()),
                                        TransportMainServiceName = dr["TRANSPORT_MAIN_SERVICE_NAME"].ToString(),
                                        TransportExtraService = dr["TRANSPORT_EXTRA_SERVICE"].ToString(),
                                        TransportExtraServiceName = dr["TRANSPORT_EXTRA_SERVICE_NAMES"].ToString(),
                                        TransportPayer = string.IsNullOrEmpty(dr["TRANSPORT_PAYER"].ToString()) ? 0 : Convert.ToInt32(dr["TRANSPORT_PAYER"].ToString()),
                                        TransportPayerName = string.IsNullOrEmpty(dr["TRANSPORT_PAYER"].ToString()) ? "" : XData.GetNameTransportPayer(dr["TRANSPORT_PAYER"].ToString()),
                                        //RequestSupportUpdateInformation = dr["REQ_SUP_UPDATE_INFORMATION"].ToString(),
                                        CollectionType = new ZBaseResponse()
                                        {
                                            ID = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                            Code = dr["COLLECTION_TYPE"].ToString(),
                                            Name = XData.GetNameCollectionType(dr["COLLECTION_TYPE"].ToString())
                                        },
                                        TypeShop = string.IsNullOrEmpty(dr["TYPE_SHOP"].ToString()) ? 0 : Convert.ToInt32(dr["TYPE_SHOP"].ToString()),
                                        AcceptancePosCode = dr["ACCEPTANCE_POSCODE"].ToString(),
                                        DeviceName = dr["DEVICE_NAME"].ToString(),
                                        OrderItems = new List<ZOrderItemGetByOrderIDResponse>(),
                                        ListUpdateInformation = new List<ZRequestSupportHistorySearchListUpdateInformationResponse>(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        ExpectedDeliveryDate = dr["EXPECTED_DELIVERY_DATE"].ToString(),
                                        CollectionPostOfficeRecent = new ZCollectionPostOfficeSearchListResponse()
                                        {
                                            ID = string.IsNullOrEmpty(dr["CO_PO_RECENT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["CO_PO_RECENT_ID"].ToString()),
                                            FullAddress = dr["CO_PO_RECENT_FULL_ADDRESS"].ToString(),
                                        }
                                    };
                                }
                            }
                            if(item != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu";
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse GetByOrderCodeHM(ZOrderGetByOrderCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderGetByIDResponse item = null;
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.GET_BY_ORDER_CODE_HM", new object[]
                {
                    req.OrderCode,     //P_ORDER_CODE      VARCHAR2
                    req.ShopID, //P_SHOP_ID       NUMBER
                    0,          //P_RETURN_CODE   OUT NUMBER
                    0           //P_OUT_CURSOR    OUT T_CURSOR
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
                                    item = new ZOrderGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        Channel = dr["CHANNEL"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        Message = dr["MESSAGE"].ToString(),
                                        ReturnCount = Convert.ToInt32(!string.IsNullOrEmpty(dr["RETURN_ORDER_COUNT"].ToString()) ? dr["RETURN_ORDER_COUNT"].ToString() : "0"),
                                        ReturnShippingCodes = dr["RETURN_ORDER_CODES"].ToString(),
                                        ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                                        {
                                            ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                            MobileNumber = dr["RECEIVER_MOBILE_NUMBER"].ToString(),
                                            ProvinceID = Convert.ToInt32(!string.IsNullOrEmpty(dr["PROVINCE_ID"].ToString()) ? dr["PROVINCE_ID"].ToString() : "0"),
                                            ProvinceName = dr["PROVINCE_NAME"].ToString(),
                                            DistrictID = Convert.ToInt32(!string.IsNullOrEmpty(dr["DISTRICT_ID"].ToString()) ? dr["DISTRICT_ID"].ToString() : "0"),
                                            DistrictName = dr["DISTRICT_NAME"].ToString(),
                                            WardID = Convert.ToInt32(!string.IsNullOrEmpty(dr["WARD_ID"].ToString()) ? dr["WARD_ID"].ToString() : "0"),
                                            WardName = dr["WARD_NAME"].ToString(),
                                            Address = dr["ADDRESS"].ToString(),
                                            FullAddress = dr["FULL_ADDRESS"].ToString(),
                                            PickUpAddressID = string.IsNullOrEmpty(dr["PICK_UP_ADDRESS_ID"].ToString()) ? 0 : Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"].ToString()),
                                            PickupFullName = dr["SENDER_NAME"].ToString(),
                                            PickupMobileNumber = dr["SENDER_PHONE"].ToString(),
                                            PickupProvinceID = string.IsNullOrEmpty(dr["SENDER_PROVINCE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_PROVINCE_ID"].ToString()),
                                            PickupProvinceName = dr["SENDER_PROVINCE_NAME"].ToString(),
                                            PickupDistrictID = string.IsNullOrEmpty(dr["SENDER_DISTRICT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_DISTRICT_ID"].ToString()),
                                            PickupDistrictName = dr["SENDER_DISTRICT_NAME"].ToString(),
                                            PickupWardID = string.IsNullOrEmpty(dr["SENDER_WARD_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_WARD_ID"].ToString()),
                                            PickupWardName = dr["SENDER_WARD_NAME"].ToString(),
                                            PickupStreet = dr["SENDER_STREET"].ToString(),
                                            PickupFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                        },
                                        TransportOrderName = dr["TRANSPORT_ORDER_NAME"].ToString(),
                                        TransportWeight = string.IsNullOrEmpty(dr["PROVISIONAL_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["PROVISIONAL_TRANSPORT_WEIGHT"].ToString()),
                                        ActualTransportWeight = string.IsNullOrEmpty(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()),
                                        ExchangeTransportWeight = string.IsNullOrEmpty(dr["EXCHANGE_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["EXCHANGE_TRANSPORT_WEIGHT"].ToString()),
                                        TransportSizeLength = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_LENGTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"].ToString()),
                                        TransportSizeWidth = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_WIDTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"].ToString()),
                                        TransportSizeHeight = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_HEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"].ToString()),
                                    };
                                }
                            }
                            if(item != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu";
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse GetByIDV2(ZOrderGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZOrderGetByIDResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.GET_BY_ID_V2", new object[]
                {
                    req.ID,     //P_ORDER_ID      NUMBER
                    0,          //P_RETURN_CODE   OUT NUMBER
                    0           //P_OUT_CURSOR    OUT T_CURSOR
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
                                    item = new ZOrderGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ORDER_ID"]),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        TransportFee = Convert.ToInt32(dr["TRANSPORT_FEE"]),
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
                                res.Message = "Không tồn tại dữ liệu";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.GetByIDV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse ConvertToOrderCode(ZOrderConvertToOrderCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ORDER_PKG.CONVERT_TO_ORDER_CODE", new object[]
                {
                    0,
                    req.OrderID,
                    req.ShopID,
                    0
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
                            res.Data = dbCommand.Parameters["P_OUT_ORDER_CODE"].Value.ToString();
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.ConvertToOrderCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}