using EMS.API.GateWay;
using EMS.API.GateWay.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.SyncGateWay.Repositories
{
    public class GrabRepository
    {
        #region GrabDelivery

        public static ZResponse GrabDeliveryService()
        {
            var res = APILibraryRepository.GrabDelivery(new ZRequest()
            {
                Code = "api/v1/grab/service?merchant_token=" + APILibraryRepository.GrabDeliveryToken + "&datatype=list"
            }, "GET");

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZGrabDeliveryServiceResponse>(res.Data);
                res.Code = ResponseData.code == "success" ? "00" : "99";
                res.Data = JsonConvert.SerializeObject(ResponseData.data);
                res.Message = ResponseData.message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryService: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GrabDeliveryStatus()
        {
            var res = APILibraryRepository.GrabDelivery(new ZRequest()
            {
                Code = "api/v1/grab/status?merchant_token=" + APILibraryRepository.GrabDeliveryToken + "&datatype=list"
            }, "GET");

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZGrabDeliveryStatusResponse>(res.Data);
                res.Code = ResponseData.code == "success" ? "00" : "99";
                res.Data = JsonConvert.SerializeObject(ResponseData.data);
                res.Message = ResponseData.message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryStatus: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GrabDeliveryProvince()
        {
            var res = APILibraryRepository.GrabDelivery(new ZRequest()
            {
                Code = "api/v1/address/province?merchant_token=" + APILibraryRepository.GrabDeliveryToken
            }, "GET");

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZGrabDeliveryProvinceResponse>(res.Data);
                res.Code = ResponseData.code == "success" ? "00" : "99";
                res.Data = JsonConvert.SerializeObject(ResponseData.data);
                res.Message = ResponseData.message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryProvince: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GrabDeliveryDistrict()
        {
            var res = APILibraryRepository.GrabDelivery(new ZRequest()
            {
                Code = "api/v1/address/district?merchant_token=" + APILibraryRepository.GrabDeliveryToken
            }, "GET");

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZGrabDeliveryDistrictResponse>(res.Data);
                res.Code = ResponseData.code == "success" ? "00" : "99";
                res.Data = JsonConvert.SerializeObject(ResponseData.data);
                res.Message = ResponseData.message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryDistrict: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GrabDeliveryWard()
        {
            var res = APILibraryRepository.GrabDelivery(new ZRequest()
            {
                Code = "api/v1/address/ward?merchant_token=" + APILibraryRepository.GrabDeliveryToken
            }, "GET");

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZGrabDeliveryWardResponse>(res.Data);
                res.Code = ResponseData.code == "success" ? "00" : "99";
                res.Data = JsonConvert.SerializeObject(ResponseData.data);
                res.Message = ResponseData.message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryWard: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GrabDeliveryQuotation(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZGrabTransportFeeRequest>(data);

                List<ZGrabDeliveryQuotationOriginRequest> QuotationOrigin = new List<ZGrabDeliveryQuotationOriginRequest>()
                {
                    new ZGrabDeliveryQuotationOriginRequest()
                    {
                        province = req.FromProvinceID.ToString(),
                        district = req.FromDistrictID.ToString(),
                        ward = req.FromWardID.ToString(),
                        address = req.FromFullAddress
                    }
                };

                List<ZGrabDeliveryQuotationDestinationRequest> QuotationDestination = new List<ZGrabDeliveryQuotationDestinationRequest>()
                {
                    new ZGrabDeliveryQuotationDestinationRequest()
                    {
                        product_name = req.ProductName,
                        money_collect = req.COD,
                        total_amount = req.Amount,
                        quantity = req.Quantity,
                        weight = req.Weight,
                        length = req.Length,
                        height = req.Height,
                        width = req.Width,
                        province = req.ToProvinceID.ToString(),
                        district = req.ToDistrictID.ToString(),
                        ward = req.ToWardID.ToString(),
                        address = req.ToFullAddress
                    }
                };

                var serviceType = "";
                switch (req.MainServiceID)
                {
                    case 523:
                        serviceType = "INSTANT";
                        break;
                    case 524:
                        serviceType = "SAME_DAY";
                        break;
                    default:
                        serviceType = "";
                        break;
                }

                ZResponse callAPI = new ZResponse();
                if (string.IsNullOrEmpty(req.PromoCode))
                {
                    callAPI = APILibraryRepository.InvokeGrab(new ZGrabRequest()
                    {
                        Method = "POST",
                        RequestURI = "api/v1/grab/quotations",
                        Value = new ZGrabDeliveryQuotationRequest()
                        {
                            merchant_token = APILibraryRepository.GrabDeliveryToken,
                            serviceType = serviceType,
                            origin = QuotationOrigin,
                            destination = QuotationDestination
                        }
                    });
                }
                else
                {
                    callAPI = APILibraryRepository.InvokeGrab(new ZGrabRequest()
                    {
                        Method = "POST",
                        RequestURI = "api/v1/grab/quotations",
                        Value = new ZGrabDeliveryQuotationV2Request()
                        {
                            merchant_token = APILibraryRepository.GrabDeliveryToken,
                            serviceType = serviceType,
                            origin = QuotationOrigin,
                            destination = QuotationDestination,
                            promo_code = req.PromoCode,
                            paymentMethod = "CASHLESS"
                        }
                    });
                }

                if (callAPI.Code == "00")
                {
                    var resGrab = JsonConvert.DeserializeObject<ZGrabDeliveryQuotationResponse>(callAPI.Data);
                    if (resGrab.code == "success")
                    {
                        var responseData = resGrab.data;
                        ZGrabTransportFeeResponse item = new ZGrabTransportFeeResponse()
                        {
                            Distance = Math.Round(((double)(responseData.distance) / 1000), 2),
                            TotalFreight = responseData.total_freight + 1500,
                            Message = ""
                        };

                        if (responseData.estimatedTimeLine.Count > 0)
                        {
                            item.EstimatedTimeLinePickup = string.IsNullOrEmpty(responseData.estimatedTimeLine[0].pickup) == true ? "" : DateTime.ParseExact(responseData.estimatedTimeLine[0].pickup, "yyyy-MM-dd HH:mm", null).ToString("dd/MM/yyyy HH:mm");
                            item.EstimatedTimeLineDropoff = string.IsNullOrEmpty(responseData.estimatedTimeLine[0].dropoff) == true ? "" : DateTime.ParseExact(responseData.estimatedTimeLine[0].dropoff, "yyyy-MM-dd HH:mm", null).ToString("dd/MM/yyyy HH:mm");
                            item.EstimatedTimeLineCompleted = string.IsNullOrEmpty(responseData.estimatedTimeLine[0].completed) == true ? "" : DateTime.ParseExact(responseData.estimatedTimeLine[0].completed, "yyyy-MM-dd HH:mm", null).ToString("dd/MM/yyyy HH:mm");
                        }

                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = JsonConvert.SerializeObject(item);
                    }
                    else
                    {
                        res.Code = "98";
                        res.Message = resGrab.message;
                    }
                }
                else
                {
                    res.Code = callAPI.Code;
                    res.Message = callAPI.Message;
                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryQuotation: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("GrabRepository.GrabDeliveryQuotation: request={0}||exception={1}", data, ex.Message));
            }
            return res;
        }

        //public static ZResponse GrabDeliveryQuotation(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<dynamic>(data);
        //    req.merchant_token = APILibraryRepository.GrabDeliveryToken;
        //    var res = APILibraryRepository.GrabDelivery(new ZRequest()
        //    {
        //        Code = "api/v1/grab/quotations",
        //        Data = JsonConvert.SerializeObject(req)
        //    }, "POST");

        //    if (res.Code == "00")
        //    {
        //        var ResponseData = JsonConvert.DeserializeObject<ZGrabDeliveryQuotationResponse>(res.Data);
        //        res.Code = ResponseData.code == "success" ? "00" : "99";
        //        res.Data = JsonConvert.SerializeObject(ResponseData.data);
        //        res.Message = ResponseData.message;
        //    }
        //    else
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryQuotation: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
        //    }
        //    return res;
        //}

        public static ZResponse GrabDeliveryOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<dynamic>(data);
            req.merchant_token = APILibraryRepository.GrabDeliveryToken;
            var res = APILibraryRepository.GrabDelivery(new ZRequest()
            {
                Code = "api/v1/grab/deliveries",
                Data = JsonConvert.SerializeObject(req)
            }, "POST");

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZGrabDeliveryOrderResponse>(res.Data);
                res.Code = ResponseData.code == "success" ? "00" : "99";
                res.Data = JsonConvert.SerializeObject(ResponseData.data);
                res.Message = ResponseData.message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryOrder: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GrabDeliveryDetail(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZGrabDetailByShippingIDRequest>(data);

                var callAPI = APILibraryRepository.InvokeGrab(new ZGrabRequest()
                {
                    Method = "GET",
                    RequestURI = string.Format("api/v1/grab/detail/{0}?merchant_token={1}", req.ShippingID, APILibraryRepository.GrabDeliveryToken),
                });

                if (callAPI.Code == "00")
                {
                    var resGrab = JsonConvert.DeserializeObject<ZGrabDeliveryDetailResponse>(callAPI.Data);
                    if (resGrab.code == "success")
                    {
                        var responseData = resGrab.data;

                        ZGrabDetailByShippingIDResponse convertData = new ZGrabDetailByShippingIDResponse()
                        {
                            CustomerCode = responseData.customer_code,
                            ShippingID = responseData.id,
                            ShippingCode = responseData.itemcode,
                            OrderCode = responseData.ordercode,
                            ServiceType = responseData.serviceType,
                            ServiceName = responseData.serviceName,
                            ShareLink = responseData.shareLink,
                            StatusCode = responseData.status,
                            StatusName = responseData.statusName,
                            Distance = Math.Round(((double)(responseData.distance) / 1000), 2),
                            TransportFee = responseData.total_fee,
                            SystemSurcharge = 1500,
                            TotalFee = responseData.total_fee + 1500,
                            CreatedDate = responseData.created_at == "0" ? "" : responseData.created_at,
                            SendedDate = responseData.sended_at == "0" ? "" : responseData.sended_at,
                            SucceedDate = responseData.succeed_at == "0" ? "" : responseData.succeed_at,
                            CanceledDate = responseData.canceled_at == "0" ? "" : responseData.canceled_at,
                            //CreatedDate = responseData.created_at == "0" ? "" : DateTime.ParseExact(responseData.created_at, "dd/MM/yy HH:mm", null).ToString("dd/MM/yyyy HH:mm"),
                            //SendedDate = responseData.sended_at == "0" ? "" : DateTime.ParseExact(responseData.sended_at, "dd/MM/yy HH:mm", null).ToString("dd/MM/yyyy HH:mm"),
                            //SucceedDate = responseData.succeed_at == "0" ? "" : DateTime.ParseExact(responseData.succeed_at, "dd/MM/yy HH:mm", null).ToString("dd/MM/yyyy HH:mm"),
                            //CanceledDate = responseData.canceled_at == "0" ? "" : DateTime.ParseExact(responseData.canceled_at, "dd/MM/yy HH:mm", null).ToString("dd/MM/yyyy HH:mm"),
                            SenderInfo = new ZGrabDetailByShippingIDSenderInfoResponse()
                            {
                                FullName = responseData.origin.name,
                                MobileNumber = responseData.origin.phone,
                                ProvinceName = responseData.origin.province,
                                DistrictName = responseData.origin.district,
                                WardName = responseData.origin.ward,
                                FullAddress = responseData.origin.address,
                                Description = responseData.origin.description,
                            },
                            ListReceiverInfo = new List<ZGrabDetailByShippingIDReceiverInfoResponse>(),
                            DriverInfo = new ZGrabDetailByShippingIDDriverInfoResponse()
                            {
                                FullName = responseData.driver.name,
                                MobileNumber = responseData.driver.phone,
                                LicensePlate = responseData.driver.licensePlate,
                                PhotoURL = responseData.driver.photoURL
                            },
                            ListOrderHistory = new List<ZGrabDetailByShippingIDHistoryResponse>(),
                        };

                        foreach (var item in responseData.destination)
                        {
                            convertData.ListReceiverInfo.Add(new ZGrabDetailByShippingIDReceiverInfoResponse()
                            {
                                FullName = item.name,
                                MobileNumber = item.phone,
                                ProvinceName = item.province,
                                DistrictName = item.district,
                                WardName = item.ward,
                                FullAddress = item.address,
                                Weight = item.weight,
                                Length = item.length,
                                Width = item.width,
                                Height = item.height,
                                TotalAmount = item.total_amount,
                                MoneyCollect = item.money_collect,
                                EstimatedInfo = new ZGrabDetailByShippingIDReceiverInfoEstimatedInfoResponse()
                                {
                                    PickUpDate = item.estimated.pickup,
                                    DropOffDate = item.estimated.dropoff,
                                    CompletedDate = item.estimated.completed,
                                    //PickUpDate = string.IsNullOrEmpty(item.estimated.pickup) == true ? "" : DateTime.ParseExact(item.estimated.pickup, "yyyy-MM-dd HH:mm", null).ToString("dd/MM/yyyy HH:mm"),
                                    //DropOffDate = string.IsNullOrEmpty(item.estimated.dropoff) == true ? "" : DateTime.ParseExact(item.estimated.dropoff, "yyyy-MM-dd HH:mm", null).ToString("dd/MM/yyyy HH:mm"),
                                    //CompletedDate = string.IsNullOrEmpty(item.estimated.completed) == true ? "" : DateTime.ParseExact(item.estimated.completed, "yyyy-MM-dd HH:mm", null).ToString("dd/MM/yyyy HH:mm")
                                },
                            });
                        }

                        //if (convertData.ListReceiverInfo.Count > 0)
                        //{
                        //    convertData.Weight = responseData.destination[0].weight;
                        //    convertData.Length = responseData.destination[0].length;
                        //    convertData.Width = responseData.destination[0].width;
                        //    convertData.Height = responseData.destination[0].height;
                        //    convertData.TotalAmount = responseData.destination[0].total_amount;
                        //    convertData.MoneyCollect = responseData.destination[0].money_collect;
                        //    var PickUpDate = string.IsNullOrEmpty(responseData.destination[0].estimated.pickup) == true ? "" : DateTime.ParseExact(responseData.destination[0].estimated.pickup, "yyyy-MM-dd HH:mm", null).ToString("dd/MM/yyyy HH:mm");
                        //    convertData.EstimatedInfo.PickUpDate = PickUpDate;
                        //    var DropOffDate = string.IsNullOrEmpty(responseData.destination[0].estimated.pickup) == true ? "" : DateTime.ParseExact(responseData.destination[0].estimated.pickup, "yyyy-MM-dd HH:mm", null).ToString("dd/MM/yyyy HH:mm");
                        //    convertData.EstimatedInfo.DropOffDate = DropOffDate;
                        //    var CompletedDate = string.IsNullOrEmpty(responseData.destination[0].estimated.pickup) == true ? "" : DateTime.ParseExact(responseData.destination[0].estimated.pickup, "yyyy-MM-dd HH:mm", null).ToString("dd/MM/yyyy HH:mm");
                        //    convertData.EstimatedInfo.CompletedDate = CompletedDate;
                        //}

                        foreach (var item in responseData.history)
                        {
                            convertData.ListOrderHistory.Add(new ZGrabDetailByShippingIDHistoryResponse()
                            {
                                StatusCode = item.status,
                                StatusName = item.statusName,
                                CreatedDate = item.created_at
                                //CreatedDate = string.IsNullOrEmpty(item.created_at) == true ? "" : DateTime.ParseExact(item.created_at, "dd/MM/yy HH:mm", null).ToString("dd/MM/yyyy HH:mm")
                            });
                        }

                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = JsonConvert.SerializeObject(convertData);
                    }
                    else
                    {
                        res.Code = "98";
                        res.Message = resGrab.message;
                    }
                }
                else
                {
                    res.Code = callAPI.Code;
                    res.Message = callAPI.Message;
                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryDetail: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("GrabRepository.GrabDeliveryDetail: request={0}||exception={1}", data, ex.Message));
            }
            return res;
        }

        //public static ZResponse GrabDeliveryDetail(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZGrabDeliveryDetailRequest>(data);
        //    var res = APILibraryRepository.GrabDelivery(new ZRequest()
        //    {
        //        Code = "api/v1/grab/detail/" + req.id + "?merchant_token=" + APILibraryRepository.GrabDeliveryToken
        //    }, "GET");

        //    if (res.Code == "00")
        //    {
        //        var ResponseData = JsonConvert.DeserializeObject<ZGrabDeliveryDetailResponse>(res.Data);
        //        res.Code = ResponseData.code == "success" ? "00" : "99";
        //        res.Message = ResponseData.code == "success" ? "Thành công" : "Thất bại";
        //        res.Data = JsonConvert.SerializeObject(ResponseData.data);
        //    }
        //    else
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryDetail: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
        //    }
        //    return res;
        //}

        public static ZResponse GrabDeliveryCancel(string data)
        {
            var req = JsonConvert.DeserializeObject<ZGrabDeliveryCancelRequest>(data);
            var res = APILibraryRepository.GrabDelivery(new ZRequest()
            {
                Code = "api/v1/grab/cancel/" + req.id + "?merchant_token=" + APILibraryRepository.GrabDeliveryToken
            }, "GET");

            if (res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZGrabDeliveryCancelResponse>(res.Data);
                res.Code = ResponseData.code == "success" ? "00" : "99";
                res.Message = ResponseData.code == "success" ? "Thành công" : ResponseData.message;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabRepository.GrabDeliveryCancel: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        #endregion
    }
}