using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
namespace EMS.MVC.Backend.Controllers
{
    public class OrderBatchController : BaseController
    {
        #region Chi tiết đơn hàng lô
        public ActionResult DetailOrderBatchView(SearchListOrderViewModel req)
        {
            DetailOrderBatchViewModel model = null;
            var res = OrderBatchRepository.DetailByBatchCode(new ZOrderBatchDetailByBatchCodeRequest() { BatchCode = req.BatchCode, ShopID = req.ShopID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZOrderBatchDetailByBatchCodeResponse>(res.Data);


                List<DetailOrderBatchOrderInfoModel> ListOrderInfo = new List<DetailOrderBatchOrderInfoModel>();
                foreach (var ItemOrderInfo in item.ListOrderInfo)
                {
                    List<DetailOrderBatchProductInfoModel> ListProductInfo = new List<DetailOrderBatchProductInfoModel>();
                    foreach (var ItemProductInfo in ItemOrderInfo.ListProductInfo)
                    {
                        ListProductInfo.Add(new DetailOrderBatchProductInfoModel()
                        {
                            ProductName = ItemProductInfo.ProductName,
                            VariantSKU = ItemProductInfo.VariantSKU,
                            VariantName = ItemProductInfo.VariantName,
                            NumberOfProduct = ItemProductInfo.NumberOfProduct,
                            Price = ItemProductInfo.Price,
                            Weight = ItemProductInfo.Weight,
                            Discount = ItemProductInfo.Discount,
                            TotalPrice = ItemProductInfo.TotalPrice,
                        });
                    }

                    List<RequestSupportModel> ListRequestSupportInfo = new List<RequestSupportModel>();
                    foreach (var ItemRequestSupportInfo in ItemOrderInfo.ListRequestSupportInfo)
                    {
                        ListRequestSupportInfo.Add(new RequestSupportModel()
                        {
                            RequestSupportType = new BaseModel()
                            {
                                ID = ItemRequestSupportInfo.RequestSupportType.ID,
                                Name = ItemRequestSupportInfo.RequestSupportType.Name,
                            },
                            OldUpdateInformation = ItemRequestSupportInfo.OldUpdateInformation,
                            NewUpdateInformation = ItemRequestSupportInfo.NewUpdateInformation,
                            CreatedDate = ItemRequestSupportInfo.CreatedDate
                        });
                    }

                    //List<EMSSignaruteAndImageInformationModel> ListEMSSignaruteAndImageInfo = new List<EMSSignaruteAndImageInformationModel>();
                    //foreach (var ItemEMSSignaruteAndImageInfo in ItemOrderInfo.ListEMSSignaruteAndImageInfo)
                    //{
                    //    ListEMSSignaruteAndImageInfo.Add(new EMSSignaruteAndImageInformationModel()
                    //    {
                    //        Signature = ItemEMSSignaruteAndImageInfo.Signature,
                    //        FileNames = ItemEMSSignaruteAndImageInfo.FileNames,
                    //        FileNamesAuth = ItemEMSSignaruteAndImageInfo.FileNamesAuth
                    //    });
                    //}

                    //List<VNPSignaruteAndImageInformationModel> ListVNPSignaruteAndImageInfo = new List<VNPSignaruteAndImageInformationModel>();
                    //foreach (var ItemVNPSignaruteAndImageInfo in ItemOrderInfo.ListVNPSignaruteAndImageInfo)
                    //{
                    //    ListVNPSignaruteAndImageInfo.Add(new VNPSignaruteAndImageInformationModel()
                    //    {
                    //        ImagesSignature = ItemVNPSignaruteAndImageInfo.ImagesSignature,
                    //        ImagesDelivery = ItemVNPSignaruteAndImageInfo.ImagesDelivery,
                    //        Images = ItemVNPSignaruteAndImageInfo.Images,
                    //        ImagesPlus = ItemVNPSignaruteAndImageInfo.ImagesPlus
                    //    });
                    //}

                    ListOrderInfo.Add(new DetailOrderBatchOrderInfoModel()
                    {
                        ShippingCode = ItemOrderInfo.ShippingCode,
                        EMSOneCode = ItemOrderInfo.EMSOneCode,
                        OrderCode = ItemOrderInfo.OrderCode,
                        OrderName = ItemOrderInfo.OrderName,
                        OrderStatus = new BaseModel()
                        {
                            Code = ItemOrderInfo.OrderStatus.Code,
                            Name = ItemOrderInfo.OrderStatus.Name
                        },
                        OrderQuantity = ItemOrderInfo.OrderQuantity,
                        OrderValue = ItemOrderInfo.OrderValue,
                        ProvisionalTransportFee = ItemOrderInfo.ProvisionalTransportFee,
                        ActualTransportFee = ItemOrderInfo.ActualTransportFee,
                        Length = ItemOrderInfo.Length,
                        Width = ItemOrderInfo.Width,
                        Height = ItemOrderInfo.Height,
                        ProvisionalTransportWeight = ItemOrderInfo.ProvisionalTransportWeight,
                        ActualTransportWeight = ItemOrderInfo.ActualTransportWeight,
                        ExchangeTransportWeight = ItemOrderInfo.ExchangeTransportWeight,
                        TotalPHU = ItemOrderInfo.TotalPHU,
                        Message = ItemOrderInfo.Message,
                        AcceptancePosCode = ItemOrderInfo.AcceptancePosCode,
                        ReasonCancel = ItemOrderInfo.ReasonCancel,
                        UpdatedName = ItemOrderInfo.UpdatedName,
                        UpdatedDate = ItemOrderInfo.UpdatedDate,

                        TotalAmount = ItemOrderInfo.TotalAmount,
                        TotalProductQuantity = ItemOrderInfo.TotalProductQuantity,
                        TotalProductValue = ItemOrderInfo.TotalProductValue,
                        TotalOrderValue = ItemOrderInfo.TotalOrderValue,
                        TotalDiscount = ItemOrderInfo.TotalDiscount,

                        ListProductInfo = ListProductInfo,
                        ListRequestSupportInfo = ListRequestSupportInfo,
                        //ListEMSSignaruteAndImageInfo = ListEMSSignaruteAndImageInfo,
                        //ListVNPSignaruteAndImageInfo = ListVNPSignaruteAndImageInfo
                    });
                }

                model = new DetailOrderBatchViewModel()
                {
                    BatchCode = req.BatchCode,
                    BuyerInfo = new CustomerModel()
                    {
                        FullName = item.BuyerInfo.FullName,
                        MobileNumber = item.BuyerInfo.MobileNumber,
                    },
                    ReceiverInfo = new CustomerModel()
                    {
                        FullName = item.ReceiverInfo.FullName,
                        MobileNumber = item.ReceiverInfo.MobileNumber,
                        FullAddress = item.ReceiverInfo.FullAddress
                    },
                    WareHouseName = item.WareHouseName,
                    CollectionType = new BaseModel()
                    {
                        Code = item.CollectionType.Code,
                        Name = item.CollectionType.Name
                    },
                    TransportPayer = new BaseModel()
                    {
                        Code = item.TransportPayer.Code,
                        Name = item.TransportPayer.Name
                    },
                    MainTransportServiceID = new BaseModel()
                    {
                        Code = item.MainTransportServiceID.Code,
                        Name = item.MainTransportServiceID.Name
                    },
                    ExtraTransportServiceID = new BaseModel()
                    {
                        Code = item.ExtraTransportServiceID.Code,
                        Name = item.ExtraTransportServiceID.Name
                    },
                    ProvisionalTotalCOD = item.ProvisionalTotalCOD,
                    ActualTotalCOD = item.ActualTotalCOD,
                    Note = item.Note,
                    Channel = new BaseModel()
                    {
                        Code = item.Channel.Code,
                        Name = item.Channel.Name
                    },
                    PlaceOfCreation = item.PlaceOfCreation,
                    ListOrderInfo = ListOrderInfo,
                };
            }
            return PartialView(model);
        }
        #endregion
    }
}