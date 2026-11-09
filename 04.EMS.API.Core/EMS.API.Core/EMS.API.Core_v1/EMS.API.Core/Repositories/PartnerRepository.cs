using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.API.Core.Repositories
{
    public class PartnerRepository
    {
        public static ZResponse OrderAdd(ZPartnerOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                #region Khai báo biến
                ZPartnerInfoResponse PartnerInfoData = null;
                ZOrderAddShipmentInfoRequest ShipmentDetails = new ZOrderAddShipmentInfoRequest();
                List<ZTransportServiceSearchListByShopIDResponse> ListTransportService = new List<ZTransportServiceSearchListByShopIDResponse>();
                List<ZOrderAddItemRequest> ListOrderItem = new List<ZOrderAddItemRequest>();
                List<object> messageError = new List<object>();
                long TotalAmount = 0;
                var isOrderInfoError = false;
                var isBuyerInfoError = false;
                var isSenderInfoError = false;
                var isReceiverInfoError = false;
                var isTransportInfoError = false;
                #endregion

                #region Kiếm tra Mã PP/CRM or Token
                if (string.IsNullOrEmpty(req.CrmOrPaypostCode) && string.IsNullOrEmpty(req.CustomerToken))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập CrmOrPaypostCode hoặc CustomerToken.";
                }
                else if (!string.IsNullOrEmpty(req.CrmOrPaypostCode))
                {
                    var resPartnerInfo = PartnerInfo(new ZPartnerInfoRequest() { CrmOrPaypostCode = req.CrmOrPaypostCode.Trim() });
                    if (resPartnerInfo.Code == "00")
                    {
                        PartnerInfoData = JsonConvert.DeserializeObject<ZPartnerInfoResponse>(resPartnerInfo.Data);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ";
                        res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không tồn tại." });
                    }
                }
                else if (!string.IsNullOrEmpty(req.CustomerToken))
                {
                    var resPartnerInfo = PartnerInfo(new ZPartnerInfoRequest() { CustomerToken = req.CustomerToken.Trim() });
                    if (resPartnerInfo.Code == "00")
                    {
                        PartnerInfoData = JsonConvert.DeserializeObject<ZPartnerInfoResponse>(resPartnerInfo.Data);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ";
                        res.Data = JsonConvert.SerializeObject(new { Parameter = "CustomerToken", Message = "Mã CustomerToken không tồn tại." });
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không được bỏ trống." });
                }
                #endregion

                if (PartnerInfoData == null)
                {
                    return res;
                }
                else
                {
                    //10: Hà Nội, 55: Đà Nẵng, 70: HCM
                    //DEV: KIOTVIET123
                    //PRO: 11527858277222
                    if (req.CrmOrPaypostCode == "11527858277222" && req.SenderInfo.ProvinceID != 10 && req.SenderInfo.ProvinceID != 55 && req.SenderInfo.ProvinceID != 70)
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ";
                        res.Data = JsonConvert.SerializeObject(new { Parameter = "SenderInfo.ProvinceID", Message = "Tạo đơn thất bại, Tỉnh/tp không thuộc địa bàn phục vụ." });
                        return res;
                    }
                }

                #region Thông tin chung
                if (!string.IsNullOrEmpty(req.OrderCode))
                {
                    if (req.OrderCode.Length > 70)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "OrderCode", Message = "Mã đơn hàng tối đa 70 ký tự." });
                    }
                    else if (XCommon.CheckRegex("OrderCode", req.OrderCode) == false)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "OrderCode", Message = "Mã đơn hàng không hợp lệ." });
                    }
                }

                if (!string.IsNullOrEmpty(req.OrderName))
                {
                    if (req.OrderName.Length > 150)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "OrderName", Message = "Tên đơn hàng tối đa 150 ký tự." });
                    }
                }

                if (!string.IsNullOrEmpty(req.Note))
                {
                    if (req.Note.Length > 250)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "Note", Message = "Ghi chú đơn hàng tối đa 250 ký tự." });
                    }
                }

                if (!string.IsNullOrEmpty(req.Message))
                {
                    if (req.Message.Length > 250)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "Message", Message = "Nội dung đơn hàng tối đa 250 ký tự." });
                    }
                }

                if (string.IsNullOrEmpty(req.Channel))
                {
                    req.Channel = "API";
                }
                else if (XData.GetNameChannel(req.Channel) == "")
                {
                    req.Channel = "API";
                }

                switch (req.IsTransport)
                {
                    case "":
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "IsTransport", Message = "Loại hình kinh doanh không được để trống." });
                        break;
                    case "Y":
                        break;
                    case "N":
                        break;
                    default:
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "IsTransport", Message = "Loại hình kinh doanh không hợp lệ." });
                        break;
                }

                switch (req.IsSendTransport)
                {
                    case "":
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "IsSendTransport", Message = "Yêu cầu gửi vận chuyển không được để trống." });
                        break;
                    case "Y":
                        break;
                    case "N":
                        break;
                    default:
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "IsSendTransport", Message = "Yêu cầu gửi vận chuyển không hợp lệ." });
                        break;
                }

                if (req.IsSendTransport == "Y" && req.IsTransport == "N")
                {
                    isOrderInfoError = true;
                    messageError.Add(new { Parameter = "IsTransport", Message = "Yêu cầu gửi vận chuyển thì loại hình kinh doanh bắt buộc là vận chuyển hàng." });
                }

                req.WareHouseID = req.WareHouseID == 0 ? PartnerInfoData.DefaultWareHouseID : req.WareHouseID;
                #endregion

                #region Thông tin sản phẩm

                if (req.ProductInfo == null)
                {
                    req.ProductInfo = new List<ZPartnerOrderAddProductInfoRequest>();
                    //if(req.TransportInfo != null)
                    //{
                    //    TotalAmount = req.TransportInfo.TotalCOD > 0 ? req.TransportInfo.TotalCOD : req.OrderValue;
                    //}
                }
                else
                {
                    if (req.ProductInfo.Count > 0)
                    {
                        foreach (var item in req.ProductInfo)
                        {
                            ListOrderItem.Add(new ZOrderAddItemRequest()
                            {
                                ProductID = item.ProductID,
                                VariantID = item.VariantID,
                                NumberOfProduct = item.NumberOfProduct,
                                Price = item.Price,
                            });
                            TotalAmount += item.NumberOfProduct * item.Price;
                            req.OrderQuantity += item.NumberOfProduct;
                        }

                        req.OrderValue = req.OrderValue == 0 ? TotalAmount : req.OrderValue;
                    }
                }

                if (req.OrderQuantity == 0)
                {
                    req.OrderQuantity = 1;
                }
                else if (req.OrderQuantity > 999)
                {
                    isOrderInfoError = true;
                    messageError.Add(new { Parameter = "OrderQuantity", Message = "Số lượng sản phẩm không vượt quá 999." });
                }

                if (req.OrderValue > 100000000)
                {
                    isOrderInfoError = true;
                    messageError.Add(new { Parameter = "OrderValue", Message = "Giá trị đơn hàng không vượt quá 100 triệu đồng." });
                }
                #endregion

                #region Thông tin người mua
                Task BuyerHandle = new Task(() =>
                {
                    if (req.BuyerInfo == null)
                    {
                        isBuyerInfoError = true;
                        messageError.Add(new { Parameter = "BuyerInfo", Message = "Thông tin người mua không được để trống." });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(req.BuyerInfo.FullName))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.FullName", Message = "Họ và tên người mua không được để trống." });
                        }

                        if (string.IsNullOrEmpty(req.BuyerInfo.MobileNumber))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.MobileNumber", Message = "Số điện thoại người mua không được để trống." });
                        }
                        else if (XCommon.CheckRegex("MobileNumber", req.BuyerInfo.MobileNumber) == false)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.MobileNumber", Message = "Số điện thoại người mua không hợp lệ." });
                        }

                        if (req.BuyerInfo.ProvinceID == 0)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.ProvinceID", Message = "Tỉnh/ thành phố người mua không được bỏ trống." });
                        }

                        if (req.BuyerInfo.DistrictID == 0)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.DistrictID", Message = "Quận/ huyện người mua không được bỏ trống." });

                        }

                        if (req.BuyerInfo.WardID == 0)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.WardID", Message = "Phường/ xã người mua không được bỏ trống." });
                        }

                        if (string.IsNullOrEmpty(req.BuyerInfo.Street))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.Street", Message = "Địa chỉ người mua không được bỏ trống." });
                        }

                        req.BuyerInfo.IsUpdate = string.IsNullOrEmpty(req.BuyerInfo.IsUpdate) ? "N" : req.BuyerInfo.IsUpdate;

                        if (isBuyerInfoError == false)
                        {
                            var resCustomerIsExist = CustomerRepository.IsExist(new ZCustomerIsExistRequest()
                            {
                                MobileNumber = req.BuyerInfo.MobileNumber,
                                FullName = req.BuyerInfo.FullName,
                                ProvinceID = req.BuyerInfo.ProvinceID,
                                DistrictID = req.BuyerInfo.DistrictID,
                                WardID = req.BuyerInfo.WardID,
                                Street = req.BuyerInfo.Street,
                                ShopID = PartnerInfoData.ShopID,
                                CreatedBy = PartnerInfoData.StoreOwnerID,
                                Channel = req.Channel,
                                DeviceName = "PARTNER",
                                IsUpdated = "N"
                            });

                            if (resCustomerIsExist.Code == "00")
                            {
                                req.BuyerInfo.CustomerID = Convert.ToInt32(resCustomerIsExist.Data);
                            }
                            else
                            {
                                isBuyerInfoError = true;
                                messageError.Add(new { Parameter = "BuyerInfo", Message = "Lỗi tìm kiếm thông tin người mua." });
                            }
                        }
                    }
                });
                #endregion

                #region Thông tin vận chuyển
                if (req.IsTransport == "Y" || req.IsSendTransport == "Y")
                {
                    #region Thông tin người gửi
                    Task SenderHandle = new Task(() =>
                    {
                        if (PartnerInfoData.TypeShop == 1)
                        {
                            ShipmentDetails.PickUpAddressID = PartnerInfoData.PickUpAddress.ID;
                            ShipmentDetails.PickupFullName = PartnerInfoData.PickUpAddress.FullName;
                            ShipmentDetails.PickupMobileNumber = PartnerInfoData.PickUpAddress.MobileNumber;
                            ShipmentDetails.PickupProvinceID = PartnerInfoData.PickUpAddress.ProvinceID;
                            ShipmentDetails.PickupDistrictID = PartnerInfoData.PickUpAddress.DistrictID;
                            ShipmentDetails.PickupWardID = PartnerInfoData.PickUpAddress.WardID;
                            ShipmentDetails.PickupStreet = PartnerInfoData.PickUpAddress.Street;
                        }
                        else
                        {
                            if (req.SenderInfo == null)
                            {
                                isSenderInfoError = true;
                                messageError.Add(new { Parameter = "SenderInfo", Message = "Thông tin người gửi không được để trống." });
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(req.SenderInfo.FullName))
                                {
                                    isSenderInfoError = true;
                                    messageError.Add(new { Parameter = "SenderInfo.FullName", Message = "Họ và tên người gửi không được để trống." });
                                }

                                if (string.IsNullOrEmpty(req.SenderInfo.MobileNumber))
                                {
                                    isSenderInfoError = true;
                                    messageError.Add(new { Parameter = "SenderInfo.MobileNumber", Message = "Số điện thoại người gửi không được để trống." });
                                }
                                else if (XCommon.CheckRegex("MobileNumber", req.SenderInfo.MobileNumber) == false)
                                {
                                    isSenderInfoError = true;
                                    messageError.Add(new { Parameter = "SenderInfo.MobileNumber", Message = "Số điện thoại người gửi không hợp lệ." });
                                }

                                if (req.SenderInfo.ProvinceID == 0)
                                {
                                    isSenderInfoError = true;
                                    messageError.Add(new { Parameter = "SenderInfo.ProvinceID", Message = "Tỉnh/ thành phố người gửi không hợp lệ." });
                                }

                                if (req.SenderInfo.DistrictID == 0)
                                {
                                    isSenderInfoError = true;
                                    messageError.Add(new { Parameter = "SenderInfo.DistrictID", Message = "Quận/ huyện người gửi không hợp lệ." });
                                }

                                if (req.SenderInfo.WardID == 0)
                                {
                                    isSenderInfoError = true;
                                    messageError.Add(new { Parameter = "SenderInfo.WardID", Message = "Phường/ xã người gửi không hợp lệ." });
                                }

                                if (string.IsNullOrEmpty(req.SenderInfo.Street))
                                {
                                    isSenderInfoError = true;
                                    messageError.Add(new { Parameter = "SenderInfo.Street", Message = "Địa chỉ người gửi không hợp lệ." });
                                }

                                if (isSenderInfoError == false)
                                {
                                    ShipmentDetails.PickUpAddressID = 0;
                                    ShipmentDetails.PickupFullName = req.SenderInfo.FullName;
                                    ShipmentDetails.PickupMobileNumber = req.SenderInfo.MobileNumber;
                                    ShipmentDetails.PickupProvinceID = req.SenderInfo.ProvinceID;
                                    ShipmentDetails.PickupDistrictID = req.SenderInfo.DistrictID;
                                    ShipmentDetails.PickupWardID = req.SenderInfo.WardID;
                                    ShipmentDetails.PickupStreet = req.SenderInfo.Street;
                                }
                            }
                        }
                    });

                    #endregion

                    #region Thông tin người nhận
                    Task ReceiverHandle = new Task(() =>
                    {
                        if (req.ReceiverInfo == null)
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo", Message = "Thông tin người nhận không được để trống." });
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(req.ReceiverInfo.FullName))
                            {
                                isReceiverInfoError = true;
                                messageError.Add(new { Parameter = "ReceiverInfo.FullName", Message = "Họ và tên người nhận không được để trống." });
                            }

                            if (string.IsNullOrEmpty(req.ReceiverInfo.MobileNumber))
                            {
                                isReceiverInfoError = true;
                                messageError.Add(new { Parameter = "ReceiverInfo.MobileNumber", Message = "Số điện thoại người nhận không được để trống." });
                            }
                            else if (XCommon.CheckRegex("MobileNumber", req.ReceiverInfo.MobileNumber) == false)
                            {
                                isReceiverInfoError = true;
                                messageError.Add(new { Parameter = "ReceiverInfo.MobileNumber", Message = "Số điện thoại người nhận không hợp lệ" });
                            }

                            if (req.ReceiverInfo.ProvinceID == 0)
                            {
                                isReceiverInfoError = true;
                                messageError.Add(new { Parameter = "ReceiverInfo.ProvinceID", Message = "Tỉnh/ thành phố người nhận không hợp lệ." });
                            }

                            if (req.ReceiverInfo.DistrictID == 0)
                            {
                                isReceiverInfoError = true;
                                messageError.Add(new { Parameter = "ReceiverInfo.DistrictID", Message = "Quận/ huyện người nhận không hợp lệ." });
                            }

                            if (req.ReceiverInfo.WardID == 0)
                            {
                                isReceiverInfoError = true;
                                messageError.Add(new { Parameter = "ReceiverInfo.WardID", Message = "Phường/ xã người nhận không hợp lệ." });
                            }

                            if (string.IsNullOrEmpty(req.ReceiverInfo.Street))
                            {
                                isReceiverInfoError = true;
                                messageError.Add(new { Parameter = "ReceiverInfo.Street", Message = "Địa chỉ người nhận không hợp lệ." });
                            }

                            if (isReceiverInfoError == false)
                            {
                                ShipmentDetails.ReceiverName = req.ReceiverInfo.FullName;
                                ShipmentDetails.MobileNumber = req.ReceiverInfo.MobileNumber;
                                ShipmentDetails.ProvinceID = req.ReceiverInfo.ProvinceID;
                                ShipmentDetails.DistrictID = req.ReceiverInfo.DistrictID;
                                ShipmentDetails.WardID = req.ReceiverInfo.WardID;
                                ShipmentDetails.Address = req.ReceiverInfo.Street;
                            }
                        }
                    });

                    #endregion

                    #region Thông tin vận chuyển
                    Task TransportHandle = new Task(() =>
                    {
                        if (req.TransportInfo == null)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo", Message = "Thông tin vận chuyển không được bỏ trống." });
                        }
                        else
                        {
                            if (req.TransportInfo.TransportWeight == 0)
                            {
                                req.TransportInfo.TransportWeight = 100;
                            }
                            else if (req.TransportInfo.TransportWeight > 9999999)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportWeight", Message = "Khối lượng không vượt quá 9999999." });
                            }

                            if (req.TransportInfo.TransportSizeLength > 999)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportSizeLength", Message = "Chiều dài không vượt quá 999." });
                            }

                            if (req.TransportInfo.TransportSizeWidth > 999)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportSizeWidth", Message = "Chiều rộng không vượt quá 999." });
                            }

                            if (req.TransportInfo.TransportSizeHeight > 999)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportSizeHeight", Message = "Chiều cao không vượt quá 999." });
                            }

                            switch (req.TransportInfo.CollectionType)
                            {
                                case 0:
                                    req.TransportInfo.CollectionType = PartnerInfoData.CollectionType;
                                    break;
                                case 1:
                                    break;
                                case 2:
                                    break;
                                default:
                                    isTransportInfoError = true;
                                    messageError.Add(new { Parameter = "TransportInfo.CollectionType", Message = "Hình thức thu gom không hợp lệ." });
                                    break;
                            }

                            if (string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                            {
                                req.TransportInfo.TransportExtraServiceID = "";
                            }

                            if (req.TransportInfo.TransportPayer == 2)
                            {
                                switch (req.TransportInfo.TransportMainServiceID)
                                {
                                    case 141:
                                    case 360:
                                    case 481:
                                        if (req.TransportInfo.TransportExtraServiceID.Contains("108") == false)
                                        {
                                            req.TransportInfo.TransportExtraServiceID += string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID) ? "108" : ",108";
                                        }
                                        break;
                                    default:
                                        if (req.TransportInfo.TransportExtraServiceID.Contains("108") == false)
                                        {
                                            req.TransportInfo.TransportPayer = 1;
                                            isTransportInfoError = true;
                                            messageError.Add(new { Parameter = "TransportInfo.TransportExtraServiceID", Message = "Vui lòng sử dụng dịch vụ Thu phí ship hàng (PSH)" });
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                req.TransportInfo.TransportPayer = 1;
                            }

                            //108 - COD
                            if (req.TransportInfo.TransportExtraServiceID.Contains("108") == true)
                            {
                                req.TransportInfo.TotalCOD = req.TransportInfo.TotalCOD == 0 ? TotalAmount : req.TransportInfo.TotalCOD;
                            }
                            else
                            {
                                if (req.TransportInfo.TotalCOD > 0)
                                {
                                    req.TransportInfo.TransportExtraServiceID += string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID) ? "108" : ",108";
                                }
                                else
                                {
                                    req.TransportInfo.TotalCOD = 0;
                                }
                            }

                            if (req.TransportInfo.TotalCOD > 50000000)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TotalCOD", Message = "Tiền thu hộ không vượt quá 50 triệu đồng." });
                            }

                            //421 - PHU
                            if (req.TransportInfo.TransportExtraServiceID.Contains("421") == true && req.TransportInfo.TotalPHU == 0)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TotalPHU", Message = "Tiền thu phí hủy đơn không được bỏ trống." });
                            }
                            else if (req.TransportInfo.TotalPHU > 0 && req.TransportInfo.TransportExtraServiceID.Contains("421") == false)
                            {
                                req.TransportInfo.TransportExtraServiceID += string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID) ? "421" : ",421";
                            }

                            //103 - KG
                            if (req.TransportInfo.TransportExtraServiceID.Contains("103") == true && req.OrderValue == 0)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.OrderValue", Message = "Giá trị đơn hàng không được bỏ trống." });
                            }

                            if (req.TransportInfo.TransportMainServiceID > 0 && string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID) == false)
                            {
                                if ((req.TransportInfo.TransportMainServiceID == 102 || req.TransportInfo.TransportMainServiceID == 141 || req.TransportInfo.TransportMainServiceID == 502 || req.TransportInfo.TransportMainServiceID == 541 || req.TransportInfo.TransportMainServiceID == 542 || req.TransportInfo.TransportMainServiceID == 543)
                                    && (req.TransportInfo.TransportExtraServiceID.Contains("104") == true || req.TransportInfo.TransportExtraServiceID.Contains("105") == true || req.TransportInfo.TransportExtraServiceID.Contains("106") == true))
                                {
                                    //Nếu DV chính là Phát hỏa tốc, CPN TMĐT đồng giá TMĐ, CPN Tài liệu, EMS TCNH Bạc, EMS TCNH Vàng, EMS TCNH Tiêu chuẩn
                                    //Thì không được chọn các DV cộng thêm là Hàng nhạy cảm EMS - VUN, Dịch vụ EMS cây cảnh, Dịch vụ EMS thực phẩm
                                    isTransportInfoError = true;
                                    messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. Vui lòng điều chỉnh." });
                                }
                                else if ((req.TransportInfo.TransportMainServiceID == 502 || req.TransportInfo.TransportMainServiceID == 541 || req.TransportInfo.TransportMainServiceID == 542 || req.TransportInfo.TransportMainServiceID == 543)
                                         && (req.TransportInfo.TransportExtraServiceID.Contains("108") == true || req.TransportInfo.TransportExtraServiceID.Contains("341") == true || req.TransportInfo.TransportExtraServiceID.Contains("342") == true || req.TransportInfo.TransportExtraServiceID.Contains("420") == true))
                                {
                                    //Nếu DV chính là CPN Tài liệu, EMS TCNH Bạc, EMS TCNH Vàng, EMS TCNH Tiêu chuẩn
                                    //Thì không được chọn các DV cộng thêm là Thu hộ - chi hộ, Bốc xếp khâu chấp nhận, Bốc xếp khâu phát hàng, Phát hàng siêu thị
                                    isTransportInfoError = true;
                                    messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. Vui lòng điều chỉnh." });
                                }
                                else if ((req.TransportInfo.TransportMainServiceID == 541 || req.TransportInfo.TransportMainServiceID == 542 || req.TransportInfo.TransportMainServiceID == 543) && (req.TransportInfo.TransportExtraServiceID.Contains("462") == true))
                                {
                                    //Nếu DV chính là EMS TCNH Bạc, EMS TCNH Vàng, EMS TCNH Tiêu chuẩn
                                    //Thì không được chọn các DV cộng thêm là Phát hàng một phần chiều đến
                                    isTransportInfoError = true;
                                    messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. Vui lòng điều chỉnh." });
                                }
                            }

                            var ParentID = 0;
                            if (req.TransportInfo.TransportMainServiceID > 0 && string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                            {
                                ParentID = 1;
                            }
                            else if (req.TransportInfo.TransportMainServiceID == 0 && !string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                            {
                                ParentID = 2;
                            }

                            var resTransportServiceSearchListByShopID = TransportServiceRepository.SearchListByShopID(new ZTransportServiceSearchListByShopIDRequest()
                            {
                                ShopID = PartnerInfoData.ShopID,
                                ParentID = ParentID,
                                IsLock = "N"
                            });

                            if (resTransportServiceSearchListByShopID.Code == "00")
                            {
                                ListTransportService = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(resTransportServiceSearchListByShopID.Data);
                            }

                            if (ListTransportService.Count > 0)
                            {
                                if (req.TransportInfo.TransportMainServiceID == 0)
                                {
                                    isTransportInfoError = true;
                                    messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ vận chuyển chính không được bỏ trống." });
                                }
                                else
                                {
                                    var isExistTransportMainService = ListTransportService.Where(m => m.ID == req.TransportInfo.TransportMainServiceID && m.ParentID == 1).ToList();
                                    if (isExistTransportMainService.Count == 0)
                                    {
                                        isTransportInfoError = true;
                                        messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ vận chuyển chính không tồn tại." });
                                    }
                                }

                                if (!string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                                {
                                    var splitTransportExtraService = req.TransportInfo.TransportExtraServiceID.Split(',');
                                    foreach (var item in splitTransportExtraService)
                                    {
                                        var isExistTransportExtraService = ListTransportService.Where(m => m.ID == Convert.ToInt32(item) && m.ParentID == 2).ToList();
                                        if (isExistTransportExtraService.Count == 0)
                                        {
                                            isTransportInfoError = true;
                                            messageError.Add(new { Parameter = "TransportInfo.TransportExtraService", Message = "Dịch vụ vận chuyển cộng thêm không tồn tại." });
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Không có dịch vụ vận chuyển chính." });
                                messageError.Add(new { Parameter = "TransportInfo.TransportExtraServiceID", Message = "Không có dịch vụ vận chuyển cộng thêm." });
                            }
                        }
                    });

                    #endregion

                    BuyerHandle.Start();
                    SenderHandle.Start();
                    ReceiverHandle.Start();
                    TransportHandle.Start();

                    Task.WaitAll(BuyerHandle, SenderHandle, ReceiverHandle, TransportHandle);

                    if (req.TransportInfo.TransportMainServiceID > 0 && ShipmentDetails.PickupProvinceID > 0
                        && ShipmentDetails.ProvinceID > 0 && ShipmentDetails.DistrictID > 0 && ShipmentDetails.WardID > 0
                        && req.TransportInfo.TransportWeight > 0 && req.OrderQuantity > 0)
                    {
                        var resTransportFee = TransportRepository.Charged(new ZTransportPostageChargedRequest()
                        {
                            MainServiceID = req.TransportInfo.TransportMainServiceID,
                            ExtraServiceIDs = req.TransportInfo.TransportExtraServiceID,
                            FromProvinceID = ShipmentDetails.PickupProvinceID,
                            FromDistrictID = ShipmentDetails.PickupDistrictID,
                            FromWardID = ShipmentDetails.PickupWardID,
                            ToProvinceID = ShipmentDetails.ProvinceID,
                            ToDistrictID = ShipmentDetails.DistrictID,
                            ToWardID = ShipmentDetails.WardID,
                            Weight = req.TransportInfo.TransportWeight,
                            TransportSizeLength = req.TransportInfo.TransportSizeLength,
                            TransportSizeWidth = req.TransportInfo.TransportSizeWidth,
                            TransportSizeHeight = req.TransportInfo.TransportSizeHeight,
                            Quantity = req.OrderQuantity,
                            Amount = req.OrderValue,
                            COD = req.TransportInfo.TotalCOD,
                            CustomerCode = req.CrmOrPaypostCode,
                        });

                        if (resTransportFee.Code == "00")
                        {
                            req.TransportInfo.TransportFee = JsonConvert.DeserializeObject<ZTransportPostageChargedResponse>(resTransportFee.Data).TotalAmount;
                        }
                        else
                        {
                            if (resTransportFee.Code == "18" && req.TransportInfo.TransportMainServiceID == 21)
                            {

                            }
                            else
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportFee", Message = resTransportFee.Message });
                            }
                        }
                    }
                    else
                    {
                        isTransportInfoError = true;
                        messageError.Add(new { Parameter = "TransportInfo.TransportFee", Message = "Chưa đủ thông tin để tính cước" });
                    }
                }
                else if (req.IsTransport == "N")
                {
                    BuyerHandle.Start();

                    req.IsSendTransport = "N";
                    req.TransportInfo = new ZPartnerOrderAddTransportInfoRequest()
                    {
                        CollectionType = PartnerInfoData.CollectionType,
                        TransportPayer = 1,
                        TransportWeight = 100,
                        TransportFee = 0,
                    };

                    Task.WaitAll(BuyerHandle);

                    ShipmentDetails.ReceiverName = req.BuyerInfo.FullName;
                    ShipmentDetails.MobileNumber = req.BuyerInfo.MobileNumber;
                }
                #endregion

                ZOrderAddRequest reqOrderAdd = new ZOrderAddRequest()
                {
                    OrderCode = req.OrderCode,
                    TotalAmount = TotalAmount,
                    TotalDiscount = 0,
                    TotalCOD = req.TransportInfo.TotalCOD,
                    TotalPHU = req.TransportInfo.TotalPHU,
                    Channel = req.Channel,
                    Note = req.Note,
                    Transport = req.IsTransport,
                    Message = req.Message,
                    Status = "",
                    TransportOrderName = req.OrderName,
                    TransportWeight = req.TransportInfo.TransportWeight,
                    TransportSizeLength = req.TransportInfo.TransportSizeLength,
                    TransportSizeWidth = req.TransportInfo.TransportSizeWidth,
                    TransportSizeHeight = req.TransportInfo.TransportSizeHeight,
                    TransportQuantity = req.OrderQuantity,
                    TransportOrderValue = req.OrderValue,
                    TransportMainService = req.TransportInfo.TransportMainServiceID,
                    TransportExtraService = req.TransportInfo.TransportExtraServiceID,
                    TransportFee = req.TransportInfo.TransportFee,
                    TransportPayer = req.TransportInfo.TransportPayer,
                    CollectionType = req.TransportInfo.CollectionType,
                    WareHouseID = req.WareHouseID,
                    OrderedProducts = ListOrderItem,
                    CustomerID = req.BuyerInfo.CustomerID,
                    ShipmentDetails = ShipmentDetails,
                    ShopID = PartnerInfoData.ShopID,
                    CreatedBy = PartnerInfoData.StoreOwnerID,
                    DeviceName = "PARTNER",
                    isUpdatedCustomer = req.BuyerInfo.IsUpdate,
                    isSendTransport = req.IsSendTransport,
                };

                if (isOrderInfoError == true || isTransportInfoError == true ||
                    isBuyerInfoError == true || isSenderInfoError == true || isReceiverInfoError == true)
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(messageError);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                    res.Data = JsonConvert.SerializeObject(reqOrderAdd);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerRepository.OrderAdd: " + ex.Message);
            }
            return res;
        }

        public static ZResponse VIBOrderAdd(ZPartnerVIBOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                ZOrderAddRequest reqOrderAdd = null;
                ZShopGetByCrmOrPaypostCodeResponse ShopInfo = null;
                List<object> messageError = new List<object>();
                List<ZTransportServiceSearchListByShopIDResponse> ListTransportService = new List<ZTransportServiceSearchListByShopIDResponse>();
                var Status = "";

                if (string.IsNullOrEmpty(req.CrmOrPaypostCode))
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không được bỏ trống." });
                }
                else
                {
                    var resShopInfo = ShopRepository.GetByCrmOrPaypostCode(req.CrmOrPaypostCode);
                    if (resShopInfo.Code == "00")
                    {
                        ShopInfo = JsonConvert.DeserializeObject<ZShopGetByCrmOrPaypostCodeResponse>(resShopInfo.Data);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ";
                        res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không tồn tại." });
                    }
                }

                if (ShopInfo == null)
                {
                    return res;
                }

                #region Thông tin chung
                var isOrderInfoError = false;
                if (!string.IsNullOrEmpty(req.OrderCode))
                {
                    if (req.OrderCode.Length > 70)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "OrderCode", Message = "Mã đơn hàng tối đa 70 ký tự." });
                    }
                    else if (XCommon.CheckRegex("OrderCode", req.OrderCode) == false)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "OrderCode", Message = "Mã đơn hàng không hợp lệ." });
                    }
                }

                if (!string.IsNullOrEmpty(req.OrderName))
                {
                    if (req.OrderName.Length > 150)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "OrderName", Message = "Tên đơn hàng tối đa 150 ký tự." });
                    }
                }

                if (req.OrderQuantity == 0)
                {
                    req.OrderQuantity = 1;
                }
                else if (req.OrderQuantity > 999)
                {
                    isOrderInfoError = true;
                    messageError.Add(new { Parameter = "OrderQuantity", Message = "Số lượng sản phẩm không vượt quá 999." });
                }

                if (req.OrderValue > 100000000)
                {
                    isOrderInfoError = true;
                    messageError.Add(new { Parameter = "OrderValue", Message = "Giá trị đơn hàng không vượt quá 100 triệu đồng." });
                }

                if (!string.IsNullOrEmpty(req.Note))
                {
                    if (req.Note.Length > 250)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "Note", Message = "Ghi chú đơn hàng tối đa 250 ký tự." });
                    }
                }

                if (!string.IsNullOrEmpty(req.Message))
                {
                    if (req.Message.Length > 250)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "Message", Message = "Nội dung đơn hàng tối đa 250 ký tự." });
                    }
                }
                #endregion

                #region Thông tin người mua
                var isBuyerInfoError = false;

                Task BuyerHandle = new Task(() =>
                {
                    if (req.BuyerInfo == null)
                    {
                        isBuyerInfoError = true;
                        messageError.Add(new { Parameter = "BuyerInfo", Message = "Thông tin người mua không được để trống." });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(req.BuyerInfo.FullName))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.FullName", Message = "Họ và tên người mua không được để trống." });
                        }

                        if (string.IsNullOrEmpty(req.BuyerInfo.MobileNumber))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.MobileNumber", Message = "Số điện thoại người mua không được để trống." });
                        }
                        else if (XCommon.CheckRegex("MobileNumber", req.BuyerInfo.MobileNumber) == false)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.MobileNumber", Message = "Số điện thoại người mua không hợp lệ." });
                        }

                        if (string.IsNullOrEmpty(req.BuyerInfo.Street))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.Street", Message = "Địa chỉ người mua không hợp lệ." });
                        }
                        else
                        {
                            var reqSearchPWDByEMSMapsSearch = XCommon.SearchPWDByEMSMapsSearch(req.BuyerInfo.Street);
                            if (reqSearchPWDByEMSMapsSearch.Code == "00")
                            {
                                var resSearchPWDByEMSMapsSearch = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeResponse>(reqSearchPWDByEMSMapsSearch.Data);
                                req.BuyerInfo.ProvinceID = resSearchPWDByEMSMapsSearch.ProvinceID;
                                req.BuyerInfo.DistrictID = resSearchPWDByEMSMapsSearch.DistrictID;
                                req.BuyerInfo.WardID = resSearchPWDByEMSMapsSearch.WardID;
                                req.BuyerInfo.Street = "(EMSmap) " + req.BuyerInfo.Street;
                            }
                            else
                            {
                                req.BuyerInfo.ProvinceID = 0;
                                req.BuyerInfo.DistrictID = 0;
                                req.BuyerInfo.WardID = 0;
                                req.BuyerInfo.Street = "";
                            }
                        }

                        if (isBuyerInfoError == false)
                        {
                            var resCustomerIsExist = CustomerRepository.IsExist(new ZCustomerIsExistRequest()
                            {
                                MobileNumber = req.BuyerInfo.MobileNumber,
                                FullName = req.BuyerInfo.FullName,
                                ProvinceID = req.BuyerInfo.ProvinceID,
                                DistrictID = req.BuyerInfo.DistrictID,
                                WardID = req.BuyerInfo.WardID,
                                Street = req.BuyerInfo.Street,
                                ShopID = ShopInfo.ShopID,
                                CreatedBy = ShopInfo.StoreOwnerID,
                                Channel = "VIB",
                                DeviceName = "PARTNER",
                                IsUpdated = "Y"
                            });
                            if (resCustomerIsExist.Code == "00")
                            {
                                req.BuyerInfo.CustomerID = Convert.ToInt32(resCustomerIsExist.Data);
                            }
                            else
                            {
                                isBuyerInfoError = true;
                                messageError.Add(new { Parameter = "BuyerInfo", Message = "Lỗi tìm kiếm thông tin người mua." });
                            }
                        }
                    }
                });
                #endregion

                #region Thông tin người nhận
                var isReceiverInfoError = false;
                Task ReceiverHandle = new Task(() =>
                {
                    if (req.ReceiverInfo == null)
                    {
                        isReceiverInfoError = true;
                        messageError.Add(new { Parameter = "ReceiverInfo", Message = "Thông tin người nhận không được để trống." });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(req.ReceiverInfo.FullName))
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.FullName", Message = "Họ và tên người nhận không được để trống." });
                        }

                        if (string.IsNullOrEmpty(req.ReceiverInfo.MobileNumber))
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.MobileNumber", Message = "Số điện thoại người nhận không được để trống." });
                        }
                        else if (XCommon.CheckRegex("MobileNumber", req.ReceiverInfo.MobileNumber) == false)
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.MobileNumber", Message = "Số điện thoại người nhận không hợp lệ" });
                        }

                        if (string.IsNullOrEmpty(req.ReceiverInfo.Street))
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.Street", Message = "Địa chỉ người nhận không hợp lệ." });
                        }
                        else
                        {
                            var reqSearchPWDByEMSMapsSearch = XCommon.SearchPWDByEMSMapsSearch(req.ReceiverInfo.Street);
                            if (reqSearchPWDByEMSMapsSearch.Code == "00")
                            {
                                var resSearchPWDByEMSMapsSearch = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeResponse>(reqSearchPWDByEMSMapsSearch.Data);
                                if (resSearchPWDByEMSMapsSearch.ProvinceID > 0 && resSearchPWDByEMSMapsSearch.DistrictID > 0 && resSearchPWDByEMSMapsSearch.WardID > 0)
                                {
                                    req.ReceiverInfo.ProvinceID = resSearchPWDByEMSMapsSearch.ProvinceID;
                                    req.ReceiverInfo.DistrictID = resSearchPWDByEMSMapsSearch.DistrictID;
                                    req.ReceiverInfo.WardID = resSearchPWDByEMSMapsSearch.WardID;
                                    req.ReceiverInfo.Street = "(EMSmap) " + req.ReceiverInfo.Street;
                                }
                                else
                                {
                                    Status = "D";
                                    req.ReceiverInfo.ProvinceID = resSearchPWDByEMSMapsSearch.ProvinceID;
                                    req.ReceiverInfo.DistrictID = resSearchPWDByEMSMapsSearch.DistrictID;
                                    req.ReceiverInfo.WardID = resSearchPWDByEMSMapsSearch.WardID;
                                    req.ReceiverInfo.Street = "(EMSmap) " + req.ReceiverInfo.Street;
                                }
                            }
                            else
                            {
                                Status = "D";
                                req.ReceiverInfo.ProvinceID = 0;
                                req.ReceiverInfo.DistrictID = 0;
                                req.ReceiverInfo.WardID = 0;
                                req.ReceiverInfo.Street = req.ReceiverInfo.Street;
                            }
                        }
                    }
                });
                #endregion

                #region Thông tin vận chuyển
                var isTransportInfoError = false;
                Task TransportHandle = new Task(() =>
                {
                    if (req.TransportInfo == null)
                    {
                        isTransportInfoError = true;
                        messageError.Add(new { Parameter = "TransportInfo", Message = "Thông tin vận chuyển không được bỏ trống." });
                    }
                    else
                    {
                        if (req.TransportInfo.TransportWeight > 9999999)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportWeight", Message = "Khối lượng không vượt quá 9999999." });
                        }

                        if (req.TransportInfo.TransportSizeLength > 999)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportSizeLength", Message = "Chiều dài không vượt quá 999." });
                        }

                        if (req.TransportInfo.TransportSizeWidth > 999)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportSizeWidth", Message = "Chiều rộng không vượt quá 999." });
                        }

                        if (req.TransportInfo.TransportSizeHeight > 999)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportSizeHeight", Message = "Chiều cao không vượt quá 999." });
                        }

                        req.TransportInfo.CollectionType = ShopInfo.CollectionType;

                        req.TransportInfo.TransportPayer = 1;

                        if (string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                        {
                            req.TransportInfo.TransportExtraServiceID = "";
                        }

                        if (req.TransportInfo.TransportMainServiceID > 0 && string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID) == false)
                        {
                            switch (req.TransportInfo.TransportMainServiceID)
                            {
                                case 102: //Phát hỏa tốc
                                case 141: //CPN TMĐT đồng giá TMĐ
                                case 502: //CPN Tài liệu
                                case 541: //EMS TCNH Bạc
                                case 542: //EMS TCNH Vàng
                                case 543: //EMS TCNH Tiêu chuẩn
                                    if (req.TransportInfo.TransportExtraServiceID.Contains("104") == true || req.TransportInfo.TransportExtraServiceID.Contains("105") == true || req.TransportInfo.TransportExtraServiceID.Contains("106") == true)
                                    {
                                        isTransportInfoError = true;
                                        messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. Vui lòng điều chỉnh." });
                                    }
                                    break;
                            }
                        }

                        var ParentID = 0;
                        if (req.TransportInfo.TransportMainServiceID > 0 && string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                        {
                            ParentID = 1;
                        }
                        else if (req.TransportInfo.TransportMainServiceID == 0 && !string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                        {
                            ParentID = 2;
                        }

                        var resTransportServiceSearchListByShopID = TransportServiceRepository.SearchListByShopID(new ZTransportServiceSearchListByShopIDRequest()
                        {
                            ShopID = ShopInfo.ShopID,
                            ParentID = ParentID,
                            IsLock = "N"
                        });

                        if (resTransportServiceSearchListByShopID.Code == "00")
                        {
                            ListTransportService = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(resTransportServiceSearchListByShopID.Data);
                        }

                        if (ListTransportService.Count > 0)
                        {
                            if (req.TransportInfo.TransportMainServiceID == 0)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ vận chuyển chính không được bỏ trống." });
                            }
                            else
                            {
                                var isExistTransportMainService = ListTransportService.Where(m => m.ID == req.TransportInfo.TransportMainServiceID && m.ParentID == 1).ToList();
                                if (isExistTransportMainService.Count == 0)
                                {
                                    isTransportInfoError = true;
                                    messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ vận chuyển chính không tồn tại." });
                                }
                            }

                            if (!string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                            {
                                var splitTransportExtraService = req.TransportInfo.TransportExtraServiceID.Split(',');
                                foreach (var item in splitTransportExtraService)
                                {
                                    var isExistTransportExtraService = ListTransportService.Where(m => m.ID == Convert.ToInt32(item) && m.ParentID == 2).ToList();
                                    if (isExistTransportExtraService.Count == 0)
                                    {
                                        isTransportInfoError = true;
                                        messageError.Add(new { Parameter = "TransportInfo.TransportExtraService", Message = "Dịch vụ vận chuyển cộng thêm không tồn tại." });
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Không có dịch vụ vận chuyển chính." });
                            messageError.Add(new { Parameter = "TransportInfo.TransportExtraServiceID", Message = "Không có dịch vụ vận chuyển cộng thêm." });
                        }
                    }
                });
                #endregion

                BuyerHandle.Start();
                ReceiverHandle.Start();
                TransportHandle.Start();

                Task.WaitAll(BuyerHandle, ReceiverHandle, TransportHandle);

                #region Tính cước
                if (!string.IsNullOrEmpty(Status))
                {
                    req.TransportInfo.TransportFee = 0;
                }
                else
                {
                    if (req.TransportInfo.TransportMainServiceID > 0 && ShopInfo.PickUpAddressProvinceID > 0 && req.ReceiverInfo.ProvinceID > 0
                        && req.TransportInfo.TransportWeight > 0 && req.OrderQuantity > 0)
                    {
                        var resTransportFee = TransportRepository.Charged(new ZTransportPostageChargedRequest()
                        {
                            MainServiceID = req.TransportInfo.TransportMainServiceID,
                            ExtraServiceIDs = req.TransportInfo.TransportExtraServiceID,
                            FromProvinceID = ShopInfo.PickUpAddressProvinceID,
                            FromDistrictID = ShopInfo.PickUpAddressDistrictID,
                            FromWardID = ShopInfo.PickUpAddressWardID,
                            ToProvinceID = req.ReceiverInfo.ProvinceID,
                            ToDistrictID = req.ReceiverInfo.DistrictID,
                            ToWardID = req.ReceiverInfo.WardID,
                            Weight = req.TransportInfo.TransportWeight,
                            TransportSizeLength = req.TransportInfo.TransportSizeLength,
                            TransportSizeWidth = req.TransportInfo.TransportSizeWidth,
                            TransportSizeHeight = req.TransportInfo.TransportSizeHeight,
                            Quantity = req.OrderQuantity,
                            Amount = req.OrderValue,
                            COD = 0,
                            CustomerCode = req.CrmOrPaypostCode,
                        });

                        if (resTransportFee.Code == "00")
                        {
                            req.TransportInfo.TransportFee = JsonConvert.DeserializeObject<ZTransportPostageChargedResponse>(resTransportFee.Data).TotalAmount;
                        }
                        else
                        {
                            if (resTransportFee.Code == "18" && req.TransportInfo.TransportMainServiceID == 21)
                            {

                            }
                            else
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportFee", Message = resTransportFee.Message });
                            }
                        }
                    }
                    else
                    {
                        isTransportInfoError = true;
                        messageError.Add(new { Parameter = "TransportInfo.TransportFee", Message = "Chưa đủ thông tin để tính cước" });
                    }
                }
                #endregion

                reqOrderAdd = new ZOrderAddRequest()
                {
                    OrderCode = req.OrderCode,
                    TotalAmount = req.OrderValue,
                    TotalDiscount = 0,
                    TotalCOD = 0,
                    TotalPHU = 0,
                    Channel = "API",
                    Note = req.Note,
                    Transport = "Y",
                    Message = req.Message,
                    Status = Status,
                    TransportOrderName = req.OrderName,
                    TransportWeight = req.TransportInfo.TransportWeight,
                    TransportSizeLength = req.TransportInfo.TransportSizeLength,
                    TransportSizeWidth = req.TransportInfo.TransportSizeWidth,
                    TransportSizeHeight = req.TransportInfo.TransportSizeHeight,
                    TransportQuantity = req.OrderQuantity,
                    TransportOrderValue = req.OrderValue,
                    TransportMainService = req.TransportInfo.TransportMainServiceID,
                    TransportExtraService = req.TransportInfo.TransportExtraServiceID,
                    TransportFee = req.TransportInfo.TransportFee,
                    TransportPayer = req.TransportInfo.TransportPayer,
                    CollectionType = req.TransportInfo.CollectionType,
                    WareHouseID = ShopInfo.DefaultWareHouseID,
                    OrderedProducts = new List<ZOrderAddItemRequest>(),
                    CustomerID = req.BuyerInfo.CustomerID,
                    ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                    {
                        ReceiverName = req.ReceiverInfo.FullName,
                        MobileNumber = req.ReceiverInfo.MobileNumber,
                        ProvinceID = req.ReceiverInfo.ProvinceID,
                        DistrictID = req.ReceiverInfo.DistrictID,
                        WardID = req.ReceiverInfo.WardID,
                        Address = req.ReceiverInfo.Street,
                        PickUpAddressID = ShopInfo.PickUpAddressID,
                        PickupFullName = "",
                        PickupMobileNumber = "",
                        PickupProvinceID = 0,
                        PickupDistrictID = 0,
                        PickupWardID = 0,
                        PickupStreet = "",
                    },
                    ShopID = ShopInfo.ShopID,
                    CreatedBy = ShopInfo.StoreOwnerID,
                    DeviceName = "PARTNER",
                    isUpdatedCustomer = "N",
                    isSendTransport = "N",
                };

                if (isOrderInfoError == true || isBuyerInfoError == true || isReceiverInfoError == true || isTransportInfoError == true)
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(messageError);
                }
                else
                {
                    if (string.IsNullOrEmpty(Status) == false)
                    {
                        res.Code = "02";
                        res.Message = "Tạo đơn nháp thành công";
                        res.Data = JsonConvert.SerializeObject(reqOrderAdd);
                    }
                    else
                    {
                        res.Code = "00";
                        res.Message = "Tạo đơn hàng thành công";
                        res.Data = JsonConvert.SerializeObject(reqOrderAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerRepository.VIBOrderAdd: " + ex.Message);
            }
            return res;
        }

        public static ZResponse TemuOrderAdd(ZPartnerTemuOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                ZOrderAddRequest reqOrderAdd = null;
                ZShopGetByCrmOrPaypostCodeResponse ShopInfo = null;
                List<object> messageError = new List<object>();
                List<ZTransportServiceSearchListByShopIDResponse> ListTransportService = new List<ZTransportServiceSearchListByShopIDResponse>();
                var Status = "";

                if(string.IsNullOrEmpty(req.CrmOrPaypostCode))
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không được bỏ trống." });
                }
                else
                {
                    var resShopInfo = ShopRepository.GetByCrmOrPaypostCode(req.CrmOrPaypostCode);
                    if(resShopInfo.Code == "00")
                    {
                        ShopInfo = JsonConvert.DeserializeObject<ZShopGetByCrmOrPaypostCodeResponse>(resShopInfo.Data);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ";
                        res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không tồn tại." });
                    }
                }

                if(ShopInfo == null)
                {
                    return res;
                }

                #region Thông tin chung
                var isOrderInfoError = false;
                if(!string.IsNullOrEmpty(req.OrderCode))
                {
                    if(req.OrderCode.Length > 70)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "OrderCode", Message = "Mã đơn hàng tối đa 70 ký tự." });
                    }
                    else if(XCommon.CheckRegex("OrderCode", req.OrderCode) == false)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "OrderCode", Message = "Mã đơn hàng không hợp lệ." });
                    }
                }

                if(!string.IsNullOrEmpty(req.OrderName))
                {
                    if(req.OrderName.Length > 150)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "OrderName", Message = "Tên đơn hàng tối đa 150 ký tự." });
                    }
                }

                if(req.OrderQuantity == 0)
                {
                    req.OrderQuantity = 1;
                }
                else if(req.OrderQuantity > 999)
                {
                    isOrderInfoError = true;
                    messageError.Add(new { Parameter = "OrderQuantity", Message = "Số lượng sản phẩm không vượt quá 999." });
                }

                if(req.OrderValue > 100000000)
                {
                    isOrderInfoError = true;
                    messageError.Add(new { Parameter = "OrderValue", Message = "Giá trị đơn hàng không vượt quá 100 triệu đồng." });
                }

                if(!string.IsNullOrEmpty(req.Note))
                {
                    if(req.Note.Length > 250)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "Note", Message = "Ghi chú đơn hàng tối đa 250 ký tự." });
                    }
                }

                if(!string.IsNullOrEmpty(req.Message))
                {
                    if(req.Message.Length > 250)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "Message", Message = "Nội dung đơn hàng tối đa 250 ký tự." });
                    }
                }
                #endregion

                #region Thông tin người mua
                var isBuyerInfoError = false;

                Task BuyerHandle = new Task(() =>
                {
                    if(req.BuyerInfo == null)
                    {
                        isBuyerInfoError = true;
                        messageError.Add(new { Parameter = "BuyerInfo", Message = "Thông tin người mua không được để trống." });
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(req.BuyerInfo.FullName))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.FullName", Message = "Họ và tên người mua không được để trống." });
                        }

                        if(string.IsNullOrEmpty(req.BuyerInfo.MobileNumber))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.MobileNumber", Message = "Số điện thoại người mua không được để trống." });
                        }
                        else if(XCommon.CheckRegex("MobileNumber", req.BuyerInfo.MobileNumber) == false)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.MobileNumber", Message = "Số điện thoại người mua không hợp lệ." });
                        }

                        if(string.IsNullOrEmpty(req.BuyerInfo.Street))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.Street", Message = "Địa chỉ người mua không hợp lệ." });
                        }
                        else
                        {
                            var reqSearchPWDByEMSMapsSearch = XCommon.SearchPWDByEMSMapsSearch(req.BuyerInfo.Street);
                            if(reqSearchPWDByEMSMapsSearch.Code == "00")
                            {
                                var resSearchPWDByEMSMapsSearch = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeResponse>(reqSearchPWDByEMSMapsSearch.Data);
                                req.BuyerInfo.ProvinceID = resSearchPWDByEMSMapsSearch.ProvinceID;
                                req.BuyerInfo.DistrictID = resSearchPWDByEMSMapsSearch.DistrictID;
                                req.BuyerInfo.WardID = resSearchPWDByEMSMapsSearch.WardID;
                                req.BuyerInfo.Street = "(EMSmap) " + req.BuyerInfo.Street;
                            }
                            else
                            {
                                req.BuyerInfo.ProvinceID = 0;
                                req.BuyerInfo.DistrictID = 0;
                                req.BuyerInfo.WardID = 0;
                                req.BuyerInfo.Street = "";
                            }
                        }

                        if(isBuyerInfoError == false)
                        {
                            var resCustomerIsExist = CustomerRepository.IsExist(new ZCustomerIsExistRequest()
                            {
                                MobileNumber = req.BuyerInfo.MobileNumber,
                                FullName = req.BuyerInfo.FullName,
                                ProvinceID = req.BuyerInfo.ProvinceID,
                                DistrictID = req.BuyerInfo.DistrictID,
                                WardID = req.BuyerInfo.WardID,
                                Street = req.BuyerInfo.Street,
                                ShopID = ShopInfo.ShopID,
                                CreatedBy = ShopInfo.StoreOwnerID,
                                Channel = "TEMU",
                                DeviceName = "PARTNER",
                                IsUpdated = "Y"
                            });
                            if(resCustomerIsExist.Code == "00")
                            {
                                req.BuyerInfo.CustomerID = Convert.ToInt32(resCustomerIsExist.Data);
                            }
                            else
                            {
                                isBuyerInfoError = true;
                                messageError.Add(new { Parameter = "BuyerInfo", Message = "Lỗi tìm kiếm thông tin người mua." });
                            }
                        }
                    }
                });
                #endregion

                #region Thông tin người nhận
                var isReceiverInfoError = false;
                Task ReceiverHandle = new Task(() =>
                {
                    if(req.ReceiverInfo == null)
                    {
                        isReceiverInfoError = true;
                        messageError.Add(new { Parameter = "ReceiverInfo", Message = "Thông tin người nhận không được để trống." });
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(req.ReceiverInfo.FullName))
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.FullName", Message = "Họ và tên người nhận không được để trống." });
                        }

                        if(string.IsNullOrEmpty(req.ReceiverInfo.MobileNumber))
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.MobileNumber", Message = "Số điện thoại người nhận không được để trống." });
                        }
                        else if(XCommon.CheckRegex("MobileNumber", req.ReceiverInfo.MobileNumber) == false)
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.MobileNumber", Message = "Số điện thoại người nhận không hợp lệ" });
                        }

                        if(string.IsNullOrEmpty(req.ReceiverInfo.Street))
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.Street", Message = "Địa chỉ người nhận không hợp lệ." });
                        }
                        else
                        {
                            var reqSearchPWDByEMSMapsSearch = XCommon.SearchPWDByEMSMapsSearch(req.ReceiverInfo.Street);
                            if(reqSearchPWDByEMSMapsSearch.Code == "00")
                            {
                                var resSearchPWDByEMSMapsSearch = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeResponse>(reqSearchPWDByEMSMapsSearch.Data);
                                if(resSearchPWDByEMSMapsSearch.ProvinceID > 0 && resSearchPWDByEMSMapsSearch.DistrictID > 0 && resSearchPWDByEMSMapsSearch.WardID > 0)
                                {
                                    req.ReceiverInfo.ProvinceID = resSearchPWDByEMSMapsSearch.ProvinceID;
                                    req.ReceiverInfo.DistrictID = resSearchPWDByEMSMapsSearch.DistrictID;
                                    req.ReceiverInfo.WardID = resSearchPWDByEMSMapsSearch.WardID;
                                    req.ReceiverInfo.Street = "(EMSmap) " + req.ReceiverInfo.Street;
                                }
                                else
                                {
                                    Status = "D";
                                    req.ReceiverInfo.ProvinceID = resSearchPWDByEMSMapsSearch.ProvinceID;
                                    req.ReceiverInfo.DistrictID = resSearchPWDByEMSMapsSearch.DistrictID;
                                    req.ReceiverInfo.WardID = resSearchPWDByEMSMapsSearch.WardID;
                                    req.ReceiverInfo.Street = "(EMSmap) " + req.ReceiverInfo.Street;
                                }
                            }
                            else
                            {
                                Status = "D";
                                req.ReceiverInfo.ProvinceID = 0;
                                req.ReceiverInfo.DistrictID = 0;
                                req.ReceiverInfo.WardID = 0;
                                req.ReceiverInfo.Street = req.ReceiverInfo.Street;
                            }
                        }
                    }
                });
                #endregion

                #region Thông tin vận chuyển
                var isTransportInfoError = false;
                Task TransportHandle = new Task(() =>
                {
                    if(req.TransportInfo == null)
                    {
                        isTransportInfoError = true;
                        messageError.Add(new { Parameter = "TransportInfo", Message = "Thông tin vận chuyển không được bỏ trống." });
                    }
                    else
                    {
                        if(req.TransportInfo.TransportWeight > 9999999)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportWeight", Message = "Khối lượng không vượt quá 9999999." });
                        }

                        if(req.TransportInfo.TransportSizeLength > 999)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportSizeLength", Message = "Chiều dài không vượt quá 999." });
                        }

                        if(req.TransportInfo.TransportSizeWidth > 999)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportSizeWidth", Message = "Chiều rộng không vượt quá 999." });
                        }

                        if(req.TransportInfo.TransportSizeHeight > 999)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportSizeHeight", Message = "Chiều cao không vượt quá 999." });
                        }

                        req.TransportInfo.CollectionType = ShopInfo.CollectionType;

                        req.TransportInfo.TransportPayer = 1;

                        if(string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                        {
                            req.TransportInfo.TransportExtraServiceID = "";
                        }

                        if(req.TransportInfo.TransportMainServiceID > 0 && string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID) == false)
                        {
                            switch(req.TransportInfo.TransportMainServiceID)
                            {
                                case 102: //Phát hỏa tốc
                                case 141: //CPN TMĐT đồng giá TMĐ
                                case 502: //CPN Tài liệu
                                case 541: //EMS TCNH Bạc
                                case 542: //EMS TCNH Vàng
                                case 543: //EMS TCNH Tiêu chuẩn
                                    if(req.TransportInfo.TransportExtraServiceID.Contains("104") == true || req.TransportInfo.TransportExtraServiceID.Contains("105") == true || req.TransportInfo.TransportExtraServiceID.Contains("106") == true)
                                    {
                                        isTransportInfoError = true;
                                        messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. Vui lòng điều chỉnh." });
                                    }
                                    break;
                            }
                        }

                        var ParentID = 0;
                        if(req.TransportInfo.TransportMainServiceID > 0 && string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                        {
                            ParentID = 1;
                        }
                        else if(req.TransportInfo.TransportMainServiceID == 0 && !string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                        {
                            ParentID = 2;
                        }

                        var resTransportServiceSearchListByShopID = TransportServiceRepository.SearchListByShopID(new ZTransportServiceSearchListByShopIDRequest()
                        {
                            ShopID = ShopInfo.ShopID,
                            ParentID = ParentID,
                            IsLock = "N"
                        });

                        if(resTransportServiceSearchListByShopID.Code == "00")
                        {
                            ListTransportService = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(resTransportServiceSearchListByShopID.Data);
                        }

                        if(ListTransportService.Count > 0)
                        {
                            if(req.TransportInfo.TransportMainServiceID == 0)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ vận chuyển chính không được bỏ trống." });
                            }
                            else
                            {
                                var isExistTransportMainService = ListTransportService.Where(m => m.ID == req.TransportInfo.TransportMainServiceID && m.ParentID == 1).ToList();
                                if(isExistTransportMainService.Count == 0)
                                {
                                    isTransportInfoError = true;
                                    messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ vận chuyển chính không tồn tại." });
                                }
                            }

                            if(!string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                            {
                                var splitTransportExtraService = req.TransportInfo.TransportExtraServiceID.Split(',');
                                foreach(var item in splitTransportExtraService)
                                {
                                    var isExistTransportExtraService = ListTransportService.Where(m => m.ID == Convert.ToInt32(item) && m.ParentID == 2).ToList();
                                    if(isExistTransportExtraService.Count == 0)
                                    {
                                        isTransportInfoError = true;
                                        messageError.Add(new { Parameter = "TransportInfo.TransportExtraService", Message = "Dịch vụ vận chuyển cộng thêm không tồn tại." });
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Không có dịch vụ vận chuyển chính." });
                            messageError.Add(new { Parameter = "TransportInfo.TransportExtraServiceID", Message = "Không có dịch vụ vận chuyển cộng thêm." });
                        }
                    }
                });
                #endregion

                BuyerHandle.Start();
                ReceiverHandle.Start();
                TransportHandle.Start();

                Task.WaitAll(BuyerHandle, ReceiverHandle, TransportHandle);

                #region Tính cước
                if(!string.IsNullOrEmpty(Status))
                {
                    req.TransportInfo.TransportFee = 0;
                }
                else
                {
                    if(req.TransportInfo.TransportMainServiceID > 0 && ShopInfo.PickUpAddressProvinceID > 0 && req.ReceiverInfo.ProvinceID > 0
                        && req.TransportInfo.TransportWeight > 0 && req.OrderQuantity > 0)
                    {
                        var resTransportFee = TransportRepository.Charged(new ZTransportPostageChargedRequest()
                        {
                            MainServiceID = req.TransportInfo.TransportMainServiceID,
                            ExtraServiceIDs = req.TransportInfo.TransportExtraServiceID,
                            FromProvinceID = ShopInfo.PickUpAddressProvinceID,
                            FromDistrictID = ShopInfo.PickUpAddressDistrictID,
                            FromWardID = ShopInfo.PickUpAddressWardID,
                            ToProvinceID = req.ReceiverInfo.ProvinceID,
                            ToDistrictID = req.ReceiverInfo.DistrictID,
                            ToWardID = req.ReceiverInfo.WardID,
                            Weight = req.TransportInfo.TransportWeight,
                            TransportSizeLength = req.TransportInfo.TransportSizeLength,
                            TransportSizeWidth = req.TransportInfo.TransportSizeWidth,
                            TransportSizeHeight = req.TransportInfo.TransportSizeHeight,
                            Quantity = req.OrderQuantity,
                            Amount = req.OrderValue,
                            COD = 0,
                            CustomerCode = req.CrmOrPaypostCode,
                        });

                        if(resTransportFee.Code == "00")
                        {
                            req.TransportInfo.TransportFee = JsonConvert.DeserializeObject<ZTransportPostageChargedResponse>(resTransportFee.Data).TotalAmount;
                        }
                        else
                        {
                            if(resTransportFee.Code == "18" && req.TransportInfo.TransportMainServiceID == 21)
                            {

                            }
                            else
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportFee", Message = resTransportFee.Message });
                            }
                        }
                    }
                    else
                    {
                        isTransportInfoError = true;
                        messageError.Add(new { Parameter = "TransportInfo.TransportFee", Message = "Chưa đủ thông tin để tính cước" });
                    }
                }
                #endregion

                reqOrderAdd = new ZOrderAddRequest()
                {
                    OrderCode = req.OrderCode,
                    TotalAmount = req.OrderValue,
                    TotalDiscount = 0,
                    TotalCOD = 0,
                    TotalPHU = 0,
                    Channel = "API",
                    Note = req.Note,
                    Transport = "Y",
                    Message = req.Message,
                    Status = Status,
                    TransportOrderName = req.OrderName,
                    TransportWeight = req.TransportInfo.TransportWeight,
                    TransportSizeLength = req.TransportInfo.TransportSizeLength,
                    TransportSizeWidth = req.TransportInfo.TransportSizeWidth,
                    TransportSizeHeight = req.TransportInfo.TransportSizeHeight,
                    TransportQuantity = req.OrderQuantity,
                    TransportOrderValue = req.OrderValue,
                    TransportMainService = req.TransportInfo.TransportMainServiceID,
                    TransportExtraService = req.TransportInfo.TransportExtraServiceID,
                    TransportFee = req.TransportInfo.TransportFee,
                    TransportPayer = req.TransportInfo.TransportPayer,
                    CollectionType = req.TransportInfo.CollectionType,
                    WareHouseID = ShopInfo.DefaultWareHouseID,
                    OrderedProducts = new List<ZOrderAddItemRequest>(),
                    CustomerID = req.BuyerInfo.CustomerID,
                    ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                    {
                        ReceiverName = req.ReceiverInfo.FullName,
                        MobileNumber = req.ReceiverInfo.MobileNumber,
                        ProvinceID = req.ReceiverInfo.ProvinceID,
                        DistrictID = req.ReceiverInfo.DistrictID,
                        WardID = req.ReceiverInfo.WardID,
                        Address = req.ReceiverInfo.Street,
                        PickUpAddressID = ShopInfo.PickUpAddressID,
                        PickupFullName = "",
                        PickupMobileNumber = "",
                        PickupProvinceID = 0,
                        PickupDistrictID = 0,
                        PickupWardID = 0,
                        PickupStreet = "",
                    },
                    ShopID = ShopInfo.ShopID,
                    CreatedBy = ShopInfo.StoreOwnerID,
                    DeviceName = "PARTNER",
                    isUpdatedCustomer = "N",
                    isSendTransport = "N",
                };

                if(isOrderInfoError == true || isBuyerInfoError == true || isReceiverInfoError == true || isTransportInfoError == true)
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(messageError);
                }
                else
                {
                    if(string.IsNullOrEmpty(Status) == false)
                    {
                        res.Code = "02";
                        res.Message = "Tạo đơn nháp thành công";
                        res.Data = JsonConvert.SerializeObject(reqOrderAdd);
                    }
                    else
                    {
                        res.Code = "00";
                        res.Message = "Tạo đơn hàng thành công";
                        res.Data = JsonConvert.SerializeObject(reqOrderAdd);
                    }
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerRepository.TemuOrderAdd: " + ex.Message);
            }
            return res;
        }
        public static ZResponse OrderInfo(ZPartnerOrderInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZPartnerOrderInfoResponse item = null;

                
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_PKG.ORDER_INFO", new object[]
                {
                    req.CrmOrPaypostCode, //P_CRM_OR_PAYPOST_CODE   VARCHAR
                    req.CustomerToken,    //P_CUSTOMER_TOKEN      VARCHAR,
                    req.OrderCode,        //P_ORDER_CODE           VARCHAR
                    req.ShippingCode,     //P_SHIPPING_CODE            VARCHAR
                    0,                    //P_RETURN_CODE           OUT NUMBER
                    0                     //P_OUT_CURSOR            OUT T_CURSOR
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
                                    item = new ZPartnerOrderInfoResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"].ToString()),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        BatchCode = dr["BATCH_CODE"].ToString(),
                                        OrderName = dr["ORDER_NAME"].ToString(),
                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                        OrderQuantity = string.IsNullOrEmpty(dr["ORDER_QUANTITY"].ToString()) ? 0 : Convert.ToInt64(dr["ORDER_QUANTITY"].ToString()),
                                        OrderValue = string.IsNullOrEmpty(dr["ORDER_VALUE"].ToString()) ? 0 : Convert.ToInt64(dr["ORDER_VALUE"].ToString()),
                                        Message = dr["MESSAGE"].ToString(),
                                        Note = dr["NOTE"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        UpdatedDate = dr["UPDATED_DATE"].ToString(),
                                        ShopInfo = new ZPartnerOrderInfoShopInfoResponse()
                                        {
                                            ID = Convert.ToInt32(dr["SHOP_ID"].ToString()),
                                        },
                                        EmployeeInfo = new ZPartnerOrderInfoEmployeeInfoResponse()
                                        {
                                            ID = Convert.ToInt32(dr["EMPLOYEE_ID"].ToString()),
                                        },
                                        TransportInfo = new ZPartnerOrderInfoTransportInfoResponse()
                                        {
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
                                            TransportFee = string.IsNullOrEmpty(dr["TRANSPORT_FEE"].ToString()) ? 0 : Convert.ToInt64(dr["TRANSPORT_FEE"].ToString()),
                                            ActualTransportFee = string.IsNullOrEmpty(dr["ACTUAL_TRANSPORT_FEE"].ToString()) ? 0 : Convert.ToInt64(dr["ACTUAL_TRANSPORT_FEE"].ToString()),
                                            TransportWeight = string.IsNullOrEmpty(dr["TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_WEIGHT"].ToString()),
                                            ActualTransportWeight = string.IsNullOrEmpty(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()),
                                            TransportSizeLength = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_LENGTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"].ToString()),
                                            TransportSizeWidth = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_WIDTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"].ToString()),
                                            TransportSizeHeight = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_HEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"].ToString()),
                                            TotalCOD = string.IsNullOrEmpty(dr["TOTAL_COD"].ToString()) ? 0 : Convert.ToInt64(dr["TOTAL_COD"].ToString()),
                                            ActualTotalCOD = string.IsNullOrEmpty(dr["ACTUAL_TOTAL_COD"].ToString()) ? 0 : Convert.ToInt64(dr["ACTUAL_TOTAL_COD"].ToString()),
                                            TotalPHU = string.IsNullOrEmpty(dr["TOTAL_PHU"].ToString()) ? 0 : Convert.ToInt64(dr["TOTAL_PHU"].ToString()),
                                        },
                                        SenderInfo = new ZPartnerOrderInfoSenderInfoResponse()
                                        {
                                            FullName = dr["SENDER_NAME"].ToString(),
                                            MobileNumber = dr["SENDER_PHONE"].ToString(),
                                            FullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                        },
                                        ReceiverInfo = new ZPartnerOrderInfoReceiverInfoResponse()
                                        {
                                            FullName = dr["RECEIVER_NAME"].ToString(),
                                            MobileNumber = dr["RECEIVER_PHONE"].ToString(),
                                            FullAddress = dr["RECEIVER_FULL_ADDRESS"].ToString(),
                                        },
                                        BuyerInfo = new ZPartnerOrderInfoBuyerInfoResponse()
                                        {
                                            FullName = dr["BUYER_NAME"].ToString(),
                                            MobileNumber = dr["BUYER_PHONE"].ToString(),
                                            FullAddress = dr["BUYER_FULL_ADDRESS"].ToString(),
                                        },
                                        CollectionType = new ZBaseResponse()
                                        {
                                            Code = dr["COLLECTION_TYPE"].ToString(),
                                            Name = XData.GetNameCollectionType(dr["COLLECTION_TYPE"].ToString()),
                                        },
                                        OrderStatus = new ZBaseResponse()
                                        {
                                            Code = dr["ORDER_STATUS"].ToString(),
                                            Name = XData.GetNameOrderStatus(dr["ORDER_STATUS"].ToString()),
                                        },
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerRepository.OrderInfo: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse WareHouseAdd(ZPartnerWareHouseAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var isError = false;
                List<object> messageError = new List<object>();
                ZShopGetByCrmOrPaypostCodeResponse ShopInfo = null;

                #region Kiếm tra Mã PP/CRM 
                if (string.IsNullOrEmpty(req.CrmOrPaypostCode))
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không được bỏ trống." });
                    return res;
                }
                else
                {
                    var resShopInfo = ShopRepository.GetByCrmOrPaypostCode(req.CrmOrPaypostCode);
                    if (resShopInfo.Code == "00")
                    {
                        ShopInfo = JsonConvert.DeserializeObject<ZShopGetByCrmOrPaypostCodeResponse>(resShopInfo.Data);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ";
                        res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không tồn tại." });
                        return res;
                    }
                }
                #endregion

                if (string.IsNullOrEmpty(req.WareHouseCode))
                {
                    isError = true;
                    messageError.Add(new { Parameter = "WareHouseCode", Message = "Mã kho không được để trống." });
                }
                else if (XCommon.CheckRegex("Code", req.WareHouseCode) == false)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "WareHouseCode", Message = "Mã kho không hợp lệ." });
                }
                else if (req.WareHouseCode.Length > 20)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "WareHouseCode", Message = "Mã kho tối đa 20 ký tự." });
                }

                if (string.IsNullOrEmpty(req.WareHouseName))
                {
                    isError = true;
                    messageError.Add(new { Parameter = "WareHouseName", Message = "Tên kho không được để trống." });
                }
                else if (XCommon.CheckRegex("Vietnamese", req.WareHouseName) == false)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "WareHouseName", Message = "Tên kho không hợp lệ." });
                }
                else if (req.WareHouseName.Length > 100)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "WareHouseName", Message = "Tên kho tối đa 100 ký tự." });
                }

                if (!string.IsNullOrEmpty(req.FullName) && XCommon.CheckRegex("Vietnamese", req.FullName) == false)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "MobileNumber", Message = "Họ và tên không hợp lệ." });
                }

                if (!string.IsNullOrEmpty(req.MobileNumber) && XCommon.CheckRegex("MobileNumber", req.MobileNumber) == false)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "MobileNumber", Message = "Số điện thoại không hợp lệ." });
                }

                if (string.IsNullOrEmpty(req.Street))
                {
                    isError = true;
                    messageError.Add(new { Parameter = "WareHouseName", Message = "Địa chỉ không được để trống." });
                }
                else if (req.Street.Length > 255)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "WareHouseCode", Message = "Địa chỉ tối đa 255 ký tự." });
                }

                if (req.ProvinceID == 0)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "ProvinceID", Message = "Tỉnh/ Thành phố không được để trống." });
                }

                if (req.DistrictID == 0)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "DistrictID", Message = "Quận/ Huyện không được để trống." });
                }

                if (req.WardID == 0)
                {
                    isError = true;
                    messageError.Add(new { Parameter = "WardID", Message = "Phường/ Xã không được để trống." });
                }

                ZWareHouseAddNewRequest reqWareHouseAdd = new ZWareHouseAddNewRequest()
                {
                    WareHouseCode = req.WareHouseCode,
                    WareHouseName = req.WareHouseName,
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Address = req.Street,
                    CreatedBy = ShopInfo.StoreOwnerID,
                    ShopID = ShopInfo.ShopID,
                };

                switch (req.IsDefault)
                {
                    case "":
                        reqWareHouseAdd.IsDefault = "N";
                        break;
                    case "Y":
                    case "N":
                        reqWareHouseAdd.IsDefault = req.IsDefault;
                        break;
                    default:
                        isError = true;
                        messageError.Add(new { Parameter = "IsDefault", Message = "Kho mặc định không hợp lệ." });
                        break;
                }

                if (isError == true)
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(messageError);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                    res.Data = JsonConvert.SerializeObject(reqWareHouseAdd);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerRepository.WareHouseAdd: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse PartnerInfo(ZPartnerInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZPartnerInfoResponse item = null;

                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PARTNER_PKG.PARTNER_INFO", new object[]
                {
                    req.CrmOrPaypostCode, //P_CRM_OR_PAYPOST_CODE   VARCHAR
                    req.CustomerToken,    //P_CUSTOMER_TOKEN        VARCHAR
                    0,                    //P_RETURN_CODE           OUT NUMBER
                    0                     //P_OUT_CURSOR            OUT T_CURSOR
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
                                    item = new ZPartnerInfoResponse()
                                    {
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        CrmOrPaypostCode = Convert.ToString(dr["CRM_OR_PAYPOST_CODE"]),
                                        CollectionType = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                        TypeShop = Convert.ToInt32(dr["TYPE_SHOP"]),
                                        DefaultWareHouseID = Convert.ToInt32(dr["DEFAULT_WAREHOUSE_ID"]),
                                        StoreOwnerID = Convert.ToInt32(dr["STORE_OWNER_ID"]),
                                        PickUpAddress = new ZPickUpAddressRequest()
                                        {
                                            ID = Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"]),
                                            FullName = Convert.ToString(dr["PICK_UP_ADDRESS_FULL_NAME"]),
                                            MobileNumber = Convert.ToString(dr["PICK_UP_ADDRESS_MOBILE_NUMBER"]),
                                            ProvinceID = Convert.ToInt32(dr["PICK_UP_ADDRESS_PROVINCE_ID"]),
                                            DistrictID = Convert.ToInt32(dr["PICK_UP_ADDRESS_DISTRICT_ID"]),
                                            WardID = Convert.ToInt32(dr["PICK_UP_ADDRESS_WARD_ID"]),
                                            Street = Convert.ToString(dr["PICK_UP_ADDRESS_STREET"])
                                        },
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerRepository.OrderInfo: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse OrderBatchAdd(ZPartnerOrderBatchAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                #region Khai báo biến
                ZPartnerInfoResponse PartnerInfoData = null;
                List<ZTransportServiceSearchListByShopIDResponse> ListTransportService = new List<ZTransportServiceSearchListByShopIDResponse>();

                List<object> messageError = new List<object>();
                long TotalAmount = 0;
                var isOrderInfoError = false;
                var isBuyerInfoError = false;
                var isSenderInfoError = false;
                var isReceiverInfoError = false;
                var isTransportInfoError = false;
                #endregion

                #region Kiếm tra Mã PP/CRM or Token
                if (string.IsNullOrEmpty(req.CrmOrPaypostCode) && string.IsNullOrEmpty(req.CustomerToken))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập CrmOrPaypostCode hoặc CustomerToken.";
                }
                else if (!string.IsNullOrEmpty(req.CrmOrPaypostCode))
                {
                    var resPartnerInfo = PartnerInfo(new ZPartnerInfoRequest() { CrmOrPaypostCode = req.CrmOrPaypostCode.Trim() });
                    if (resPartnerInfo.Code == "00")
                    {
                        PartnerInfoData = JsonConvert.DeserializeObject<ZPartnerInfoResponse>(resPartnerInfo.Data);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ";
                        res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không tồn tại." });
                    }
                }
                else if (!string.IsNullOrEmpty(req.CustomerToken))
                {
                    var resPartnerInfo = PartnerInfo(new ZPartnerInfoRequest() { CustomerToken = req.CustomerToken.Trim() });
                    if (resPartnerInfo.Code == "00")
                    {
                        PartnerInfoData = JsonConvert.DeserializeObject<ZPartnerInfoResponse>(resPartnerInfo.Data);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ";
                        res.Data = JsonConvert.SerializeObject(new { Parameter = "CustomerToken", Message = "Mã CustomerToken không tồn tại." });
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(new { Parameter = "CrmOrPaypostCode", Message = "Mã CrmOrPaypostCode không được bỏ trống." });
                }
                #endregion

                if (PartnerInfoData == null)
                {
                    return res;
                }
                else
                {
                    //10: Hà Nội, 55: Đà Nẵng, 70: HCM
                    //DEV: KIOTVIET123
                    //PRO: 11527858277222
                    if (req.CrmOrPaypostCode == "11527858277222" && req.SenderInfo.ProvinceID != 10 && req.SenderInfo.ProvinceID != 55 && req.SenderInfo.ProvinceID != 70)
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ";
                        res.Data = JsonConvert.SerializeObject(new { Parameter = "SenderInfo.ProvinceID", Message = "Tạo đơn thất bại, Tỉnh/tp không thuộc địa bàn phục vụ." });
                        return res;
                    }
                }

                #region Thông tin chung
                req.WareHouseID = req.WareHouseID == 0 ? PartnerInfoData.DefaultWareHouseID : req.WareHouseID;

                if (!string.IsNullOrEmpty(req.Note))
                {
                    if (req.Note.Length > 250)
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "Note", Message = "Ghi chú đơn hàng tối đa 250 ký tự." });
                    }
                }

                if (string.IsNullOrEmpty(req.Channel))
                {
                    req.Channel = "API";
                }
                else if (XData.GetNameChannel(req.Channel) == "")
                {
                    req.Channel = "API";
                }

                switch (req.ActionType)
                {
                    case "N":
                        break;
                    case "W":
                        break;
                    default:
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "ActionType", Message = "Thao tác tạo đơn hàng lô không hợp lệ." });
                        break;
                }


                #endregion

                #region Thông tin người mua
                Task BuyerHandle = new Task(() =>
                {
                    if (req.BuyerInfo == null)
                    {
                        isBuyerInfoError = true;
                        messageError.Add(new { Parameter = "BuyerInfo", Message = "Thông tin người mua không được để trống." });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(req.BuyerInfo.FullName))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.FullName", Message = "Họ và tên người mua không được để trống." });
                        }

                        if (string.IsNullOrEmpty(req.BuyerInfo.MobileNumber))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.MobileNumber", Message = "Số điện thoại người mua không được để trống." });
                        }
                        else if (XCommon.CheckRegex("MobileNumber", req.BuyerInfo.MobileNumber) == false)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.MobileNumber", Message = "Số điện thoại người mua không hợp lệ." });
                        }

                        if (req.BuyerInfo.ProvinceID == 0)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.ProvinceID", Message = "Tỉnh/ thành phố người mua không được bỏ trống." });
                        }

                        if (req.BuyerInfo.DistrictID == 0)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.DistrictID", Message = "Quận/ huyện người mua không được bỏ trống." });

                        }

                        if (req.BuyerInfo.WardID == 0)
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.WardID", Message = "Phường/ xã người mua không được bỏ trống." });
                        }

                        if (string.IsNullOrEmpty(req.BuyerInfo.Street))
                        {
                            isBuyerInfoError = true;
                            messageError.Add(new { Parameter = "BuyerInfo.Street", Message = "Địa chỉ người mua không được bỏ trống." });
                        }

                        req.BuyerInfo.IsUpdate = string.IsNullOrEmpty(req.BuyerInfo.IsUpdate) ? "N" : req.BuyerInfo.IsUpdate;

                        if (isBuyerInfoError == false)
                        {
                            var resCustomerIsExist = CustomerRepository.IsExist(new ZCustomerIsExistRequest()
                            {
                                MobileNumber = req.BuyerInfo.MobileNumber,
                                FullName = req.BuyerInfo.FullName,
                                ProvinceID = req.BuyerInfo.ProvinceID,
                                DistrictID = req.BuyerInfo.DistrictID,
                                WardID = req.BuyerInfo.WardID,
                                Street = req.BuyerInfo.Street,
                                ShopID = PartnerInfoData.ShopID,
                                CreatedBy = PartnerInfoData.StoreOwnerID,
                                Channel = req.Channel,
                                DeviceName = "PARTNER",
                                IsUpdated = "N"
                            });

                            if (resCustomerIsExist.Code == "00")
                            {
                                req.BuyerInfo.CustomerID = Convert.ToInt32(resCustomerIsExist.Data);
                            }
                            else
                            {
                                isBuyerInfoError = true;
                                messageError.Add(new { Parameter = "BuyerInfo", Message = "Lỗi tìm kiếm thông tin người mua." });
                            }
                        }
                    }
                });
                #endregion

                #region Thông tin người gửi
                ZOrderBatchAddSenderInfoRequest SenderInfo = null;
                Task SenderHandle = new Task(() =>
                {
                    if (PartnerInfoData.TypeShop == 1)
                    {
                        SenderInfo = new ZOrderBatchAddSenderInfoRequest()
                        {
                            PickUpAddressID = PartnerInfoData.PickUpAddress.ID,
                            FullName = PartnerInfoData.PickUpAddress.FullName,
                            MobileNumber = PartnerInfoData.PickUpAddress.MobileNumber,
                            ProvinceID = PartnerInfoData.PickUpAddress.ProvinceID,
                            DistrictID = PartnerInfoData.PickUpAddress.DistrictID,
                            WardID = PartnerInfoData.PickUpAddress.WardID,
                            Street = PartnerInfoData.PickUpAddress.Street,
                        };
                    }
                    else
                    {
                        if (req.SenderInfo == null)
                        {
                            isSenderInfoError = true;
                            messageError.Add(new { Parameter = "SenderInfo", Message = "Thông tin người gửi không được để trống." });
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(req.SenderInfo.FullName))
                            {
                                isSenderInfoError = true;
                                messageError.Add(new { Parameter = "SenderInfo.FullName", Message = "Họ và tên người gửi không được để trống." });
                            }

                            if (string.IsNullOrEmpty(req.SenderInfo.MobileNumber))
                            {
                                isSenderInfoError = true;
                                messageError.Add(new { Parameter = "SenderInfo.MobileNumber", Message = "Số điện thoại người gửi không được để trống." });
                            }
                            else if (XCommon.CheckRegex("MobileNumber", req.SenderInfo.MobileNumber) == false)
                            {
                                isSenderInfoError = true;
                                messageError.Add(new { Parameter = "SenderInfo.MobileNumber", Message = "Số điện thoại người gửi không hợp lệ." });
                            }

                            if (req.SenderInfo.ProvinceID == 0)
                            {
                                isSenderInfoError = true;
                                messageError.Add(new { Parameter = "SenderInfo.ProvinceID", Message = "Tỉnh/ thành phố người gửi không hợp lệ." });
                            }

                            if (req.SenderInfo.DistrictID == 0)
                            {
                                isSenderInfoError = true;
                                messageError.Add(new { Parameter = "SenderInfo.DistrictID", Message = "Quận/ huyện người gửi không hợp lệ." });
                            }

                            if (req.SenderInfo.WardID == 0)
                            {
                                isSenderInfoError = true;
                                messageError.Add(new { Parameter = "SenderInfo.WardID", Message = "Phường/ xã người gửi không hợp lệ." });
                            }

                            if (string.IsNullOrEmpty(req.SenderInfo.Street))
                            {
                                isSenderInfoError = true;
                                messageError.Add(new { Parameter = "SenderInfo.Street", Message = "Địa chỉ người gửi không hợp lệ." });
                            }

                            if (isSenderInfoError == false)
                            {
                                SenderInfo = new ZOrderBatchAddSenderInfoRequest()
                                {
                                    PickUpAddressID = 0,
                                    FullName = req.SenderInfo.FullName,
                                    MobileNumber = req.SenderInfo.MobileNumber,
                                    ProvinceID = req.SenderInfo.ProvinceID,
                                    DistrictID = req.SenderInfo.DistrictID,
                                    WardID = req.SenderInfo.WardID,
                                    Street = req.SenderInfo.Street,
                                };
                            }
                        }
                    }
                });

                #endregion

                #region Thông tin người nhận
                ZOrderBatchAddReceiverInfoRequest ReceiverInfo = null;
                Task ReceiverHandle = new Task(() =>
                {
                    if (req.ReceiverInfo == null)
                    {
                        isReceiverInfoError = true;
                        messageError.Add(new { Parameter = "ReceiverInfo", Message = "Thông tin người nhận không được để trống." });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(req.ReceiverInfo.FullName))
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.FullName", Message = "Họ và tên người nhận không được để trống." });
                        }

                        if (string.IsNullOrEmpty(req.ReceiverInfo.MobileNumber))
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.MobileNumber", Message = "Số điện thoại người nhận không được để trống." });
                        }
                        else if (XCommon.CheckRegex("MobileNumber", req.ReceiverInfo.MobileNumber) == false)
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.MobileNumber", Message = "Số điện thoại người nhận không hợp lệ" });
                        }

                        if (req.ReceiverInfo.ProvinceID == 0)
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.ProvinceID", Message = "Tỉnh/ thành phố người nhận không hợp lệ." });
                        }

                        if (req.ReceiverInfo.DistrictID == 0)
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.DistrictID", Message = "Quận/ huyện người nhận không hợp lệ." });
                        }

                        if (req.ReceiverInfo.WardID == 0)
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.WardID", Message = "Phường/ xã người nhận không hợp lệ." });
                        }

                        if (string.IsNullOrEmpty(req.ReceiverInfo.Street))
                        {
                            isReceiverInfoError = true;
                            messageError.Add(new { Parameter = "ReceiverInfo.Street", Message = "Địa chỉ người nhận không hợp lệ." });
                        }

                        if (isReceiverInfoError == false)
                        {
                            ReceiverInfo = new ZOrderBatchAddReceiverInfoRequest()
                            {
                                FullName = req.ReceiverInfo.FullName,
                                MobileNumber = req.ReceiverInfo.MobileNumber,
                                ProvinceID = req.ReceiverInfo.ProvinceID,
                                DistrictID = req.ReceiverInfo.DistrictID,
                                WardID = req.ReceiverInfo.WardID,
                                Street = req.ReceiverInfo.Street,
                            };
                        }
                    }
                });

                #endregion

                #region Thông tin vận chuyển
                Task TransportHandle = new Task(() =>
                {
                    if (req.TransportInfo == null)
                    {
                        isTransportInfoError = true;
                        messageError.Add(new { Parameter = "TransportInfo", Message = "Thông tin vận chuyển không được bỏ trống." });
                    }
                    else
                    {
                        switch (req.TransportInfo.CollectionType)
                        {
                            case 0:
                                req.TransportInfo.CollectionType = PartnerInfoData.CollectionType;
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            default:
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.CollectionType", Message = "Hình thức thu gom không hợp lệ." });
                                break;
                        }

                        if (string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                        {
                            req.TransportInfo.TransportExtraServiceID = "";
                        }

                        if (req.TransportInfo.TransportPayer == 2)
                        {
                            switch (req.TransportInfo.TransportMainServiceID)
                            {
                                case 141:
                                case 360:
                                case 481:
                                    if (req.TransportInfo.TransportExtraServiceID.Contains("108") == false)
                                    {
                                        req.TransportInfo.TransportExtraServiceID += string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID) ? "108" : ",108";
                                    }
                                    break;
                                default:
                                    if (req.TransportInfo.TransportExtraServiceID.Contains("108") == false)
                                    {
                                        req.TransportInfo.TransportPayer = 1;
                                        isTransportInfoError = true;
                                        messageError.Add(new { Parameter = "TransportInfo.TransportExtraServiceID", Message = "Vui lòng sử dụng dịch vụ Thu phí ship hàng (PSH)" });
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            req.TransportInfo.TransportPayer = 1;
                        }

                        //108 - COD
                        if (req.TransportInfo.TransportExtraServiceID.Contains("108") == true)
                        {
                            req.TransportInfo.TotalCOD = req.TransportInfo.TotalCOD == 0 ? TotalAmount : req.TransportInfo.TotalCOD;
                        }
                        else
                        {
                            if (req.TransportInfo.TotalCOD > 0)
                            {
                                req.TransportInfo.TransportExtraServiceID += string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID) ? "108" : ",108";
                            }
                            else
                            {
                                req.TransportInfo.TotalCOD = 0;
                            }
                        }

                        if (req.TransportInfo.TotalCOD > 50000000)
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TotalCOD", Message = "Tiền thu hộ không vượt quá 50 triệu đồng." });
                        }

                        if (req.TransportInfo.TransportMainServiceID > 0 && string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID) == false)
                        {
                            if ((req.TransportInfo.TransportMainServiceID == 102 || req.TransportInfo.TransportMainServiceID == 141 || req.TransportInfo.TransportMainServiceID == 502 || req.TransportInfo.TransportMainServiceID == 541 || req.TransportInfo.TransportMainServiceID == 542 || req.TransportInfo.TransportMainServiceID == 543)
                                && (req.TransportInfo.TransportExtraServiceID.Contains("104") == true || req.TransportInfo.TransportExtraServiceID.Contains("105") == true || req.TransportInfo.TransportExtraServiceID.Contains("106") == true))
                            {
                                //Nếu DV chính là Phát hỏa tốc, CPN TMĐT đồng giá TMĐ, CPN Tài liệu, EMS TCNH Bạc, EMS TCNH Vàng, EMS TCNH Tiêu chuẩn
                                //Thì không được chọn các DV cộng thêm là Hàng nhạy cảm EMS - VUN, Dịch vụ EMS cây cảnh, Dịch vụ EMS thực phẩm
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. Vui lòng điều chỉnh." });
                            }
                            else if ((req.TransportInfo.TransportMainServiceID == 502 || req.TransportInfo.TransportMainServiceID == 541 || req.TransportInfo.TransportMainServiceID == 542 || req.TransportInfo.TransportMainServiceID == 543)
                                     && (req.TransportInfo.TransportExtraServiceID.Contains("108") == true || req.TransportInfo.TransportExtraServiceID.Contains("341") == true || req.TransportInfo.TransportExtraServiceID.Contains("342") == true || req.TransportInfo.TransportExtraServiceID.Contains("420") == true))
                            {
                                //Nếu DV chính là CPN Tài liệu, EMS TCNH Bạc, EMS TCNH Vàng, EMS TCNH Tiêu chuẩn
                                //Thì không được chọn các DV cộng thêm là Thu hộ - chi hộ, Bốc xếp khâu chấp nhận, Bốc xếp khâu phát hàng, Phát hàng siêu thị
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. Vui lòng điều chỉnh." });
                            }
                            else if ((req.TransportInfo.TransportMainServiceID == 541 || req.TransportInfo.TransportMainServiceID == 542 || req.TransportInfo.TransportMainServiceID == 543) && (req.TransportInfo.TransportExtraServiceID.Contains("462") == true))
                            {
                                //Nếu DV chính là EMS TCNH Bạc, EMS TCNH Vàng, EMS TCNH Tiêu chuẩn
                                //Thì không được chọn các DV cộng thêm là Phát hàng một phần chiều đến
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ chính và Dịch vụ cộng thêm không phù hợp. Vui lòng điều chỉnh." });
                            }
                        }

                        var ParentID = 0;
                        if (req.TransportInfo.TransportMainServiceID > 0 && string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                        {
                            ParentID = 1;
                        }
                        else if (req.TransportInfo.TransportMainServiceID == 0 && !string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                        {
                            ParentID = 2;
                        }

                        var resTransportServiceSearchListByShopID = TransportServiceRepository.SearchListByShopID(new ZTransportServiceSearchListByShopIDRequest()
                        {
                            ShopID = PartnerInfoData.ShopID,
                            ParentID = ParentID,
                            IsLock = "N"
                        });

                        if (resTransportServiceSearchListByShopID.Code == "00")
                        {
                            ListTransportService = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(resTransportServiceSearchListByShopID.Data);
                        }

                        if (ListTransportService.Count > 0)
                        {
                            if (req.TransportInfo.TransportMainServiceID == 0)
                            {
                                isTransportInfoError = true;
                                messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ vận chuyển chính không được bỏ trống." });
                            }
                            else
                            {
                                var isExistTransportMainService = ListTransportService.Where(m => m.ID == req.TransportInfo.TransportMainServiceID && m.ParentID == 1).ToList();
                                if (isExistTransportMainService.Count == 0)
                                {
                                    isTransportInfoError = true;
                                    messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Dịch vụ vận chuyển chính không tồn tại." });
                                }
                            }

                            if (!string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                            {
                                var splitTransportExtraService = req.TransportInfo.TransportExtraServiceID.Split(',');
                                foreach (var item in splitTransportExtraService)
                                {
                                    var isExistTransportExtraService = ListTransportService.Where(m => m.ID == Convert.ToInt32(item) && m.ParentID == 2).ToList();
                                    if (isExistTransportExtraService.Count == 0)
                                    {
                                        isTransportInfoError = true;
                                        messageError.Add(new { Parameter = "TransportInfo.TransportExtraService", Message = "Dịch vụ vận chuyển cộng thêm không tồn tại." });
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isTransportInfoError = true;
                            messageError.Add(new { Parameter = "TransportInfo.TransportMainServiceID", Message = "Không có dịch vụ vận chuyển chính." });
                            messageError.Add(new { Parameter = "TransportInfo.TransportExtraServiceID", Message = "Không có dịch vụ vận chuyển cộng thêm." });
                        }
                    }
                });
                #endregion

                #region Thông tin đơn hàng
                if (req.ListOrderInfo == null)
                {
                    req.ListOrderInfo = new List<ZPartnerOrderBatchAddOrderInfoRequest>();
                }

                List<ZOrderBatchAddOrderInfoRequest> ListOrderInfo = new List<ZOrderBatchAddOrderInfoRequest>();

                Task OrderHandle = new Task(() =>
                {
                    if (req.ListOrderInfo.Count > 2)
                    {
                        foreach (var item in req.ListOrderInfo)
                        {
                            if (!string.IsNullOrEmpty(item.OrderCode))
                            {
                                var ExistOrderCode = req.ListOrderInfo.Where(m => m.OrderCode == item.OrderCode).ToList();
                                if (ExistOrderCode.Count > 0)
                                {
                                    isOrderInfoError = true;
                                    messageError.Add(new { Parameter = "ListOrderInfo.OrderCode", Message = "Mã đơn hàng đã tồn tại." });
                                }
                                else
                                {
                                    if (item.OrderCode.Length > 70)
                                    {
                                        isOrderInfoError = true;
                                        messageError.Add(new { Parameter = "ListOrderInfo.OrderCode", Message = "Mã đơn hàng tối đa 70 ký tự." });
                                    }
                                    else if (XCommon.CheckRegex("OrderCode", item.OrderCode) == false)
                                    {
                                        isOrderInfoError = true;
                                        messageError.Add(new { Parameter = "ListOrderInfo.OrderCode", Message = "Mã đơn hàng không hợp lệ." });
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(req.TransportInfo.TransportExtraServiceID))
                            {
                                if (req.TransportInfo.TransportExtraServiceID.Contains("103") && item.OrderValue == 0)
                                {
                                    isOrderInfoError = true;
                                    messageError.Add(new { Parameter = "ListOrderInfo.OrderCode", Message = "Vui lòng nhập giá trị đơn hàng." });
                                }

                                if (req.TransportInfo.TransportExtraServiceID.Contains("421") == false)
                                {
                                    item.TotalPHU = 0;
                                }
                                else if (req.TransportInfo.TransportExtraServiceID.Contains("421") && item.TotalPHU == 0)
                                {
                                    isOrderInfoError = true;
                                    messageError.Add(new { Parameter = "ListOrderInfo.TotalPHU", Message = "Vui lòng nhập tiền thu phí hủy đơn." });
                                }
                            }

                            if (item.OrderValue > 100000000)
                            {
                                isOrderInfoError = true;
                                messageError.Add(new { Parameter = "ListOrderInfo.OrderValue", Message = "Giá trị đơn hàng không vượt quá 100 triệu đồng." });
                            }


                        }
                    }
                    else
                    {
                        isOrderInfoError = true;
                        messageError.Add(new { Parameter = "ListOrderInfo", Message = "Đơn hàng tối thiếu 2 bưu gửi." });
                    }
                });
                #endregion

                BuyerHandle.Start();
                SenderHandle.Start();
                ReceiverHandle.Start();
                TransportHandle.Start();
                OrderHandle.Start();

                Task.WaitAll(BuyerHandle, SenderHandle, ReceiverHandle, TransportHandle, OrderHandle);



                ZOrderBatchAddRequest reqOrderBatchAdd = new ZOrderBatchAddRequest()
                {
                    CustomerID = req.BuyerInfo.CustomerID,
                    SenderInfo = SenderInfo,
                    ReceiverInfo = ReceiverInfo,
                    WareHouseID = req.WareHouseID,
                    CollectionType = req.TransportInfo.CollectionType,
                    TransportPayer = req.TransportInfo.TransportPayer,
                    TransportMainServiceID = req.TransportInfo.TransportMainServiceID,
                    TransportExtraServiceID = req.TransportInfo.TransportExtraServiceID,
                    TotalCOD = req.TransportInfo.TotalCOD,
                    Note = req.Note,
                    Channel = req.Channel,
                    ListOrderInfo = ListOrderInfo,
                    ActionType = req.ActionType,
                    CreatedBy = PartnerInfoData.StoreOwnerID,
                    DeviceName = "PARTNER",
                    ShopID = PartnerInfoData.ShopID
                };

                if (isOrderInfoError == true || isTransportInfoError == true ||
                   isBuyerInfoError == true || isSenderInfoError == true || isReceiverInfoError == true)
                {
                    res.Code = "01";
                    res.Message = "Dữ liệu không hợp lệ";
                    res.Data = JsonConvert.SerializeObject(messageError);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                    res.Data = JsonConvert.SerializeObject(reqOrderBatchAdd);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PartnerRepository.OrderBatchAdd: " + ex.Message);
            }
            return res;
        }
    }
}