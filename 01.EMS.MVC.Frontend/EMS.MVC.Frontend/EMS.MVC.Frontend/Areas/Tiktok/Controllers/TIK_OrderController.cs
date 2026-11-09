using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Areas.Tiktok.Models;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Tiktok.Controllers
{
    public class TIK_OrderController : TIK_BaseController
    {
        // GET: Tiktok/TIK_Order
        public ActionResult TIK_OrderView()
        {
            TIK_OrderViewModel model = new TIK_OrderViewModel()
            {
                FromDate = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListOrderStatus = XData.GetListOrderStatusTiktokShop(),
                ListPaymentMethod = XData.GetListPaymentMethod()
            };
            return View(model);
        }
        public ActionResult TIK_SearchListOrderView(TIK_OrderViewModel req)
        {
            List<TIK_SearchListOrderViewModel> ListItem = new List<TIK_SearchListOrderViewModel>();
            var res = OrderRepository.SearchListOrder(new ZOrderFESearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                OrderStatus = req.OrderStatus,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                Channel = "TIK"
            });
            List<ZOrderFESearchListResponse> ListData = new List<ZOrderFESearchListResponse>();
            if (res.Code == "00")
            {
                ListData = JsonConvert.DeserializeObject<List<ZOrderFESearchListResponse>>(res.Data);
                List<ZOrderFESearchListResponse> ListFilter = new List<ZOrderFESearchListResponse>();
                if (string.IsNullOrEmpty(req.OrderStatus))
                {
                    ListFilter = ListData.Where(m => m.OrderStatus != "C" 
                                                  && m.OrderStatus != "U" 
                                                  && m.OrderStatus != "V" 
                                                  && m.OrderStatus != "Z").ToList();
                }
                else
                {
                    ListFilter = ListData.Where(m => m.OrderStatus == req.OrderStatus).ToList();
                }
                foreach (var item in ListFilter)
                {
                    ListItem.Add(new TIK_SearchListOrderViewModel()
                    {
                        OrderID = item.OrderID,
                        EMSOneCode = item.EMSOneCode,
                        OrderCode = item.OrderCode,
                        CustomerName = item.CustomerName,
                        CustomerPhone = item.CustomerPhone,
                        CustomerFullAddress = item.CustomerFullAddress,
                        SenderName = item.SenderName,
                        SenderPhone = item.SenderPhone,
                        SenderFullAddress = item.SenderFullAddress,
                        ReceiverName = item.ReceiverName,
                        ReceiverPhone = item.ReceiverPhone,
                        ReceiverFullAddress = item.ReceiverFullAddress,
                        Message = item.Message,
                        TotalOrderValue = item.TotalOrderValue,
                        TotalCOD = item.TotalCOD,
                        OrderStatus = item.OrderStatus,
                        OrderStatusName = item.OrderStatusName,
                        CreatedDate = item.CreatedDate,
                        UpdatedName = item.UpdatedName,
                        ShippingCode = item.ShippingCode,
                        TransportOrderName = item.TransportOrderName,
                        TransportFee = item.TransportFee,
                        ShopID = item.ShopID,
                        CrmOrPaypostCode = item.CrmOrPaypostCode,
                        PaymentFreightVat = item.PaymentFreightVat,
                        TiktokReverseOrderCode = item.TiktokReverseOrderCode,
                        TiktokOrderInventoryStatus = item.TiktokOrderInventoryStatus
                    });
                }
            }
            if (req.ReloadStatusNumber == "Y")
            {
                var listOrderStatus = XData.GetListOrderStatusTiktokShop();
                List<object> OrderStatusNumber = new List<object>();
                foreach (var item in listOrderStatus)
                {
                    var Count = ListData.Where(m => m.OrderStatus == item.Value).Count();
                    OrderStatusNumber.Add(new
                    {
                        Status = item.Value,
                        Count = Count
                    });
                }
                ViewBag.OrderStatusNumber = OrderStatusNumber;
            }
            ViewBag.ReloadStatusNumber = req.ReloadStatusNumber;
            ViewBag.SearchOrderStatus = req.OrderStatus;
            return PartialView(ListItem);
        }
        public ActionResult TIK_DetailOrderView(SearchListOrderViewModel req)
        {
            DetailOrderViewModel model = null;
            var res = OrderRepository.DetailByOrderID(new ZOrderDetailByOrderIDRequest() { ID = req.OrderID, ShopID = req.ShopID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZOrderDetailByOrderIDResponse>(res.Data);
                List<OrderItemModel> ListOrderItem = new List<OrderItemModel>();
                foreach (var orderItem in item.OrderItems)
                {
                    ListOrderItem.Add(new OrderItemModel()
                    {
                        ID = orderItem.ID,
                        ProductID = orderItem.ProductID,
                        ProductCode = orderItem.ProductCode,
                        ProductName = orderItem.ProductName,
                        VariantID = orderItem.VariantID,
                        VariantSKU = orderItem.VariantSKU,
                        VariantName = orderItem.VariantName,
                        NumberOfProduct = orderItem.NumberOfProduct,
                        Price = orderItem.Price,
                        TotalPrice = orderItem.Price * orderItem.NumberOfProduct,
                        Discount = orderItem.Discount,
                        Weight = orderItem.Weight,
                        ProductVariantImage = orderItem.ProductVariantImage,
                        WareHouseQuantity = orderItem.WareHouseQuantity
                    });
                }

                List<RequestSupportModel> ListUpdateInformation = new List<RequestSupportModel>();
                foreach (var updateInformationItem in item.ListUpdateInformation)
                {
                    ListUpdateInformation.Add(new RequestSupportModel()
                    {
                        RequestSupportType = new BaseModel()
                        {
                            ID = updateInformationItem.RequestSupportType.ID,
                            Name = updateInformationItem.RequestSupportType.Name,
                        },
                        OldUpdateInformation = updateInformationItem.OldUpdateInformation,
                        NewUpdateInformation = updateInformationItem.NewUpdateInformation,
                        CreatedDate = updateInformationItem.CreatedDate
                    });
                }

                List<EMSSignaruteAndImageInformationModel> ListEMSSignaruteAndImageInformation = new List<EMSSignaruteAndImageInformationModel>();
                if (item.ListEMSSignaruteAndImageInfo != null)
                {
                    foreach (var informationSignaruteAndImageEMS in item.ListEMSSignaruteAndImageInfo)
                    {
                        ListEMSSignaruteAndImageInformation.Add(new EMSSignaruteAndImageInformationModel()
                        {
                            Signature = informationSignaruteAndImageEMS.Signature,
                            FileNames = informationSignaruteAndImageEMS.FileNames,
                            FileNamesAuth = informationSignaruteAndImageEMS.FileNamesAuth
                        });
                    }
                }

                List<VNPSignaruteAndImageInformationModel> ListVNPSignaruteAndImageInformation = new List<VNPSignaruteAndImageInformationModel>();
                if (item.ListVNPSignaruteAndImageInfo != null)
                {
                    foreach (var informationSignaruteAndImageVNP in item.ListVNPSignaruteAndImageInfo)
                    {
                        ListVNPSignaruteAndImageInformation.Add(new VNPSignaruteAndImageInformationModel()
                        {
                            ImagesSignature = informationSignaruteAndImageVNP.ImagesSignature,
                            ImagesDelivery = informationSignaruteAndImageVNP.ImagesDelivery,
                            Images = informationSignaruteAndImageVNP.Images,
                            ImagesPlus = informationSignaruteAndImageVNP.ImagesPlus
                        });
                    }
                }

                model = new DetailOrderViewModel()
                {
                    OrderID = item.ID,
                    OrderCode = item.OrderCode,
                    ShippingCode = item.ShippingCode,
                    OrderItems = ListOrderItem,
                    WareHouseName = item.WareHouseName,
                    Note = item.Note,
                    Message = item.Message,
                    Channel = item.Channel,
                    CollectionType = new BaseModel()
                    {
                        Code = item.CollectionType.Code,
                        Name = item.CollectionType.Name
                    },
                    Transport = new BaseModel()
                    {
                        Code = item.Transport,
                        Name = item.Transport == "Y" ? "Có vận chuyển" : "Không vận chuyển"
                    },
                    OrderName = item.TransportOrderName,
                    MainTransportService = new BaseModel()
                    {
                        Code = item.TransportMainService.ToString(),
                        Name = item.TransportMainServiceName
                    },
                    ExtraTransportService = new BaseModel()
                    {
                        Code = item.TransportExtraService,
                        Name = item.TransportExtraServiceName
                    },
                    TransportPayer = new BaseModel()
                    {
                        ID = item.TransportPayer,
                        Name = item.TransportPayerName
                    },
                    TotalProductQuantity = item.OrderItems.Sum(m => m.NumberOfProduct).ToString("N0"),
                    TotalAmount = item.TotalAmount.ToString("N0"),
                    TotalDiscount = item.TotalDiscount.ToString("N0"),
                    ProvisionalTransportFee = item.TransportFee.ToString("N0"),
                    ActualTransportFee = item.ActualTransportFee.ToString("N0"),
                    TotalOrderValue = item.TotalOrderValue.ToString("N0"),
                    TotalCOD = item.TotalCOD.ToString("N0"),
                    ActualTotalCOD = item.ActualTotalCOD.ToString("N0"),
                    TotalPHU = item.TotalPHU.ToString("N0"),
                    ProvisionalTransportWeight = item.TransportWeight.ToString("N0"),
                    ActualTransportWeight = item.ActualTransportWeight.ToString("N0"),
                    ExchangeTransportWeight = item.ExchangeTransportWeight.ToString("N0"),
                    OrderQuantity = item.TransportQuantity.ToString("N0"),
                    OrderValue = item.TransportOrderValue.ToString("N0"),
                    Length = item.TransportSizeLength.ToString(),
                    Width = item.TransportSizeWidth.ToString(),
                    Height = item.TransportSizeHeight.ToString(),
                    Buyer = new CustomerModel()
                    {
                        FullName = item.Customer.FullName,
                        MobileNumber = item.Customer.MobileNumber,
                        FullAddress = item.Customer.Address,
                    },
                    Receiver = new CustomerModel()
                    {
                        FullName = item.ShipmentDetails.ReceiverName,
                        MobileNumber = item.ShipmentDetails.MobileNumber,
                        FullAddress = item.ShipmentDetails.FullAddress,
                    },
                    OrderStatus = new BaseModel()
                    {
                        Code = item.Status,
                        Name = item.StatusName
                    },
                    ReasonCancel = item.ReasonCancel,
                    UpdatedName = item.UpdatedName,
                    UpdatedDate = item.UpdatedDate,
                    AcceptancePosCode = item.AcceptancePosCode,
                    ListRequestSupportInfo = ListUpdateInformation,
                    ListEMSSignaruteAndImageInfo = ListEMSSignaruteAndImageInformation,
                    ListVNPSignaruteAndImageInfo = ListVNPSignaruteAndImageInformation
                };
            }
            return PartialView(model);
        }
    }
}