using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using EMS.Entity;
using EMS.Library;

namespace EMS.MVC.Frontend.Controllers
{
    public class OrderBatchController : BaseController
    {
        #region Thêm đơn hàng lô
        public ActionResult AddOrderBatchView()
        {
            AddOrderBatchViewModel model = new AddOrderBatchViewModel()
            {
                ListChannel = XData.GetListChannel(),
                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                CollectionType = XHelper.Session.EmployeeProfile.ShopInformation.CollectionType,
            };
            model.WareHouseID = model.ListWareHouse.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListWareHouse.Where(m => m.IsDefault == "Y").First().ID : 0;

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                if (model.ListMainTransportService.Count > 0)
                {
                    model.MainTransportServiceID = model.ListMainTransportService.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListMainTransportService.Where(m => m.IsDefault == "Y").First().ID : 0;
                }

                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
                if (model.ListExtraTransportService.Count > 0)
                {
                    model.ExtraTransportServiceID = model.ListExtraTransportService.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListExtraTransportService.Where(m => m.IsDefault == "Y").First().ID.ToString() : "0";
                }
            }
            else
            {
                model.ListMainTransportService = new List<TransportServiceModel>();
                model.ListExtraTransportService = new List<TransportServiceModel>();
            }
            return View(model);
        }

        public ActionResult AddOrderBatchOrderInfoView(AddOrderBatchViewModel req)
        {
            AddOrderBatchOrderInfoViewModel model = new AddOrderBatchOrderInfoViewModel()
            {
                NumberOfDetail = req.NumberOfDetail,
            };
            return PartialView(model);
        }

        public JsonResult JAddOrderBatch(AddOrderBatchViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZOrderBatchAddOrderInfoRequest> ListOrderInfo = new List<ZOrderBatchAddOrderInfoRequest>();
                foreach (var item in req.ListOrderInfo)
                {
                    List<ZOrderBatchAddProductInfoRequest> ListProductInfo = new List<ZOrderBatchAddProductInfoRequest>();
                    if (item.ListProductInfo != null)
                    {
                        foreach (var product in item.ListProductInfo)
                        {
                            ListProductInfo.Add(new ZOrderBatchAddProductInfoRequest()
                            {
                                ProductID = product.ProductID,
                                VariantID = product.VariantID,
                                NumberOfProduct = product.NumberOfProduct,
                                Price = product.Price,
                                Discount = product.Discount
                            });
                        }
                    }


                    ListOrderInfo.Add(new ZOrderBatchAddOrderInfoRequest()
                    {
                        OrderCode = item.OrderCode,
                        OrderName = item.OrderName,
                        OrderQuantity = item.OrderQuantity,
                        OrderValue = item.OrderValue,
                        TransportFee = item.TransportFee,
                        TotalAmount = item.TotalAmount,
                        TotalDiscount = item.TotalDiscount,
                        Length = item.Length,
                        Width = item.Width,
                        Height = item.Height,
                        Weight = item.Weight,
                        ExchangeWeight = item.ExchangeWeight,
                        TotalPHU = item.TotalPHU,
                        Message = item.Message,
                        ListProductInfo = ListProductInfo
                    });
                }

                res = OrderBatchRepository.AddOrderBatch(new ZOrderBatchAddRequest()
                {
                    CustomerID = req.ShipmentInfo.Buyer.ID,
                    SenderInfo = new ZOrderBatchAddSenderInfoRequest()
                    {
                        FullName = req.ShipmentInfo.Sender.FullName,
                        MobileNumber = req.ShipmentInfo.Sender.MobileNumber,
                        ProvinceID = req.ShipmentInfo.Sender.Province.ID,
                        WardID = req.ShipmentInfo.Sender.Ward.ID,
                        Street = req.ShipmentInfo.Sender.Street,
                        PickUpAddressID = req.ShipmentInfo.Sender.PickUpAddressID,
                    },
                    ReceiverInfo = new ZOrderBatchAddReceiverInfoRequest()
                    {
                        FullName = req.ShipmentInfo.Receiver.FullName,
                        MobileNumber = req.ShipmentInfo.Receiver.MobileNumber,
                        ProvinceID = req.ShipmentInfo.Receiver.Province.ID,
                        WardID = req.ShipmentInfo.Receiver.Ward.ID,
                        Street = req.ShipmentInfo.Receiver.Street,
                        IsUpdatedCustomer = req.ShipmentInfo.Receiver.IsChange,
                    },
                    WareHouseID = req.WareHouseID,
                    CollectionType = req.CollectionType,
                    TransportPayer = req.TransportPayer,
                    TransportMainServiceID = req.MainTransportServiceID,
                    TransportExtraServiceID = req.ExtraTransportServiceID,
                    TotalCOD = req.TotalCOD,
                    Note = req.Note,
                    Channel = req.Channel,
                    ListOrderInfo = ListOrderInfo,
                    ActionType = req.ActionType,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    DeviceName = "WEB",
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchController.JAddOrderBatch: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tìm kiếm và hiển thị sản phẩm
        public ActionResult SearchProductView(AddOrderBatchOrderInfoViewModel req)
        {
            List<SearchProductViewModel> model = new List<SearchProductViewModel>();
            var res = OrderRepository.SearchListProduct(new ZOrderSearchProductRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                var ListItem = JsonConvert.DeserializeObject<List<ZOrderSearchProductResponse>>(res.Data);
                foreach (var item in ListItem)
                {
                    if (req.ListProductInfo != null && req.ListProductInfo.Count > 0)
                    {
                        foreach (var Choose in req.ListProductInfo)
                        {
                            var index = item.Variants.FindIndex(m => m.VariantID == Choose.VariantID);
                            if (index >= 0)
                            {
                                item.Variants[index].Selected = true;
                                item.Variants[index].NumberOfProduct = Choose.NumberOfProduct;
                            }
                        }
                    }
                    model.Add(new SearchProductViewModel()
                    {
                        ProductID = item.ProductID,
                        ProductName = item.ProductName,
                        Variants = item.Variants,
                    });
                }
            }
            ViewBag.NumberOfDetail = req.NumberOfDetail;
            return PartialView(model);
        }

        public ActionResult ProductInfoView(AddOrderBatchOrderInfoViewModel req)
        {
            return PartialView(req);
        }
        #endregion

        #region Sửa đơn hàng lô

        public JsonResult CheckEditOrderBatch(SearchListOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = OrderBatchRepository.EditOrderBatchInfo(new ZOrderBatchEditInfoRequest()
                {
                    BatchCode = Security.DecodeBase64(req.SearchKey),
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });

                //if (res.Code == "00")
                //{
                //    TempData["EditOrderBatchView"] = res.Data;
                //}
                //else
                //{
                //    TempData.Remove("EditOrderBatchView");
                //}
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchController.CheckEditOrderBatch: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HandleEditOrderBatchView()
        {
            return View();
        }

        public ActionResult EditOrderBatchView(SearchListOrderViewModel req)
        {
            var item = JsonConvert.DeserializeObject<ZOrderBatchEditInfoResponse>(req.SearchKey);

            List<EditOrderBatchOrderInfoViewModel> ListOrderInfo = new List<EditOrderBatchOrderInfoViewModel>();
            foreach (var itemOrder in item.ListOrderInfo)
            {
                List<ProductInfoViewModel> ListProductInfo = new List<ProductInfoViewModel>();
                foreach (var itemProduct in itemOrder.ListProductInfo)
                {
                    ListProductInfo.Add(new ProductInfoViewModel()
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

                ListOrderInfo.Add(new EditOrderBatchOrderInfoViewModel()
                {
                    OrderID = itemOrder.OrderID,
                    OrderCode = itemOrder.OrderCode,
                    OrderName = itemOrder.OrderName,
                    OrderQuantity = itemOrder.OrderQuantity,
                    OrderValue = itemOrder.OrderValue,
                    TransportFee = itemOrder.TransportFee,
                    TotalAmount = itemOrder.TotalAmount,
                    Length = itemOrder.Length,
                    Width = itemOrder.Width,
                    Height = itemOrder.Height,
                    Weight = itemOrder.Weight,
                    TotalPHU = itemOrder.TotalPHU,
                    Message = itemOrder.Message,
                    ListProductInfo = ListProductInfo
                });
            }

            EditOrderBatchViewModel model = new EditOrderBatchViewModel()
            {
                BatchCode = item.BatchCode,
                ShipmentInfo = new SearchCustomerViewModel()
                {
                    Buyer = new CustomerModel()
                    {
                        MobileNumber = item.BuyerInfo.MobileNumber,
                        FullName = item.BuyerInfo.FullName
                    },
                    Sender = new CustomerModel()
                    {
                        PickUpAddressID = item.SenderInfo.PickUpAddressID,
                        FullName = item.SenderInfo.FullName,
                        MobileNumber = item.SenderInfo.MobileNumber,
                        Province = new ProvinceModel() { ID = item.SenderInfo.ProvinceID },
                        District = new DistrictModel() { ID = item.SenderInfo.DistrictID },
                        Ward = new WardModel() { ID = item.SenderInfo.WardID },
                        Street = item.SenderInfo.Street
                    },
                    Receiver = new CustomerModel()
                    {
                        FullName = item.ReceiverInfo.FullName,
                        MobileNumber = item.ReceiverInfo.MobileNumber,
                        Province = new ProvinceModel() { ID = item.ReceiverInfo.ProvinceID },
                        District = new DistrictModel() { ID = item.ReceiverInfo.DistrictID },
                        Ward = new WardModel() { ID = item.ReceiverInfo.WardID },
                        Street = item.ReceiverInfo.Street
                    },
                },
                WareHouseID = item.WareHouseID,
                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                CollectionType = item.CollectionType,
                TransportPayer = item.TransportPayer,
                MainTransportServiceID = item.MainTransportServiceID,
                ListMainTransportService = new List<TransportServiceModel>(),
                ExtraTransportServiceID = item.ExtraTransportServiceID,
                ListExtraTransportService = new List<TransportServiceModel>(),
                TotalCOD = item.TotalCOD,
                Note = item.Note,
                Channel = item.Channel,
                ListChannel = XData.GetListChannel(),
                Status = item.OrderStatus,
                //TotalAmount = ListOrderInfo.Sum(m => m.TotalAmount),
                ListOrderInfo = ListOrderInfo,
                NumberOfDetail = ListOrderInfo.Count + 1
            };

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
            }
            return PartialView(model);

            //if (TempData.ContainsKey("EditOrderBatchView"))
            //{
            //    var item = JsonConvert.DeserializeObject<ZOrderBatchEditInfoResponse>(TempData["EditOrderBatchView"].ToString());

            //    List<EditOrderBatchOrderInfoViewModel> ListOrderInfo = new List<EditOrderBatchOrderInfoViewModel>();
            //    foreach (var itemOrder in item.ListOrderInfo)
            //    {
            //        List<ProductInfoViewModel> ListProductInfo = new List<ProductInfoViewModel>();
            //        foreach (var itemProduct in itemOrder.ListProductInfo)
            //        {
            //            ListProductInfo.Add(new ProductInfoViewModel()
            //            {
            //                ProductID = itemProduct.ProductID,
            //                ProductName = itemProduct.ProductName,
            //                VariantID = itemProduct.VariantID,
            //                VariantName = itemProduct.VariantName,
            //                NumberOfProduct = itemProduct.NumberOfProduct,
            //                Price = itemProduct.Price,
            //                Weight = itemProduct.Weight,
            //                Discount = itemProduct.Discount
            //            });
            //        }

            //        ListOrderInfo.Add(new EditOrderBatchOrderInfoViewModel()
            //        {
            //            OrderID = itemOrder.OrderID,
            //            OrderCode = itemOrder.OrderCode,
            //            OrderName = itemOrder.OrderName,
            //            OrderQuantity = itemOrder.OrderQuantity,
            //            OrderValue = itemOrder.OrderValue,
            //            TransportFee = itemOrder.TransportFee,
            //            TotalAmount = itemOrder.TotalAmount,
            //            Length = itemOrder.Length,
            //            Width = itemOrder.Width,
            //            Height = itemOrder.Height,
            //            Weight = itemOrder.Weight,
            //            TotalPHU = itemOrder.TotalPHU,
            //            Message = itemOrder.Message,
            //            ListProductInfo = ListProductInfo
            //        });
            //    }

            //    EditOrderBatchViewModel model = new EditOrderBatchViewModel()
            //    {
            //        BatchCode = item.BatchCode,
            //        ShipmentInfo = new SearchCustomerViewModel()
            //        {
            //            Buyer = new CustomerModel()
            //            {
            //                MobileNumber = item.BuyerInfo.MobileNumber,
            //                FullName = item.BuyerInfo.FullName
            //            },
            //            Sender = new CustomerModel()
            //            {
            //                PickUpAddressID = item.SenderInfo.PickUpAddressID,
            //                FullName = item.SenderInfo.FullName,
            //                MobileNumber = item.SenderInfo.MobileNumber,
            //                Province = new ProvinceModel() { ID = item.SenderInfo.ProvinceID },
            //                District = new DistrictModel() { ID = item.SenderInfo.DistrictID },
            //                Ward = new WardModel() { ID = item.SenderInfo.WardID },
            //                Street = item.SenderInfo.Street
            //            },
            //            Receiver = new CustomerModel()
            //            {
            //                FullName = item.ReceiverInfo.FullName,
            //                MobileNumber = item.ReceiverInfo.MobileNumber,
            //                Province = new ProvinceModel() { ID = item.ReceiverInfo.ProvinceID },
            //                District = new DistrictModel() { ID = item.ReceiverInfo.DistrictID },
            //                Ward = new WardModel() { ID = item.ReceiverInfo.WardID },
            //                Street = item.ReceiverInfo.Street
            //            },
            //        },
            //        WareHouseID = item.WareHouseID,
            //        ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
            //        CollectionType = item.CollectionType,
            //        TransportPayer = item.TransportPayer,
            //        MainTransportServiceID = item.MainTransportServiceID,
            //        ListMainTransportService = new List<TransportServiceModel>(),
            //        ExtraTransportServiceID = item.ExtraTransportServiceID,
            //        ListExtraTransportService = new List<TransportServiceModel>(),
            //        TotalCOD = item.TotalCOD,
            //        Note = item.Note,
            //        Channel = item.Channel,
            //        ListChannel = XData.GetListChannel(),
            //        Status = item.OrderStatus,
            //        //TotalAmount = ListOrderInfo.Sum(m => m.TotalAmount),
            //        ListOrderInfo = ListOrderInfo,
            //        NumberOfDetail = ListOrderInfo.Count + 1
            //    };

            //    List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            //    if (ListTransportService.Count > 0)
            //    {
            //        model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
            //        model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
            //    }

            //    TempData.Keep("EditOrderBatchView");

            //    return View(model);
            //}
            //return PartialView("~/Views/Error/PageNotFound.cshtml");
        }

        public ActionResult EditOrderBatchOrderInfoView(EditOrderBatchViewModel req)
        {
            List<EditOrderBatchOrderInfoViewModel> model = new List<EditOrderBatchOrderInfoViewModel>();
            if (req.ListOrderInfo != null && req.ListOrderInfo.Count > 0)
            {
                var i = 1;
                foreach (var item in req.ListOrderInfo)
                {
                    model.Add(new EditOrderBatchOrderInfoViewModel()
                    {
                        NumberOfDetail = i,
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        OrderName = item.OrderName,
                        OrderQuantity = item.OrderQuantity,
                        Length = item.Length,
                        Width = item.Width,
                        Height = item.Height,
                        Weight = item.Weight,
                        OrderValue = item.OrderValue,
                        TotalPHU = item.TotalPHU,
                        Message = item.Message,
                        ListProductInfo = item.ListProductInfo
                    });
                    i++;
                }
            }
            return PartialView(model);
        }

        public JsonResult JEditOrderBatch(EditOrderBatchViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZOrderBatchEditOrderInfoRequest> ListOrderInfo = new List<ZOrderBatchEditOrderInfoRequest>();
                foreach (var item in req.ListOrderInfo)
                {
                    List<ZOrderBatchEditProductInfoRequest> ListProductInfo = new List<ZOrderBatchEditProductInfoRequest>();
                    if (item.ListProductInfo != null)
                    {
                        foreach (var product in item.ListProductInfo)
                        {
                            ListProductInfo.Add(new ZOrderBatchEditProductInfoRequest()
                            {
                                ProductID = product.ProductID,
                                VariantID = product.VariantID,
                                NumberOfProduct = product.NumberOfProduct,
                                Price = product.Price,
                                Discount = product.Discount
                            });
                        }
                    }


                    ListOrderInfo.Add(new ZOrderBatchEditOrderInfoRequest()
                    {
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        OrderName = item.OrderName,
                        OrderQuantity = item.OrderQuantity,
                        OrderValue = item.OrderValue,
                        TransportFee = item.TransportFee,
                        TotalAmount = item.TotalAmount,
                        TotalDiscount = item.TotalDiscount,
                        Length = item.Length,
                        Width = item.Width,
                        Height = item.Height,
                        Weight = item.Weight,
                        ExchangeWeight = item.ExchangeWeight,
                        TotalPHU = item.TotalPHU,
                        Message = item.Message,
                        ListProductInfo = ListProductInfo
                    });
                }

                res = OrderBatchRepository.EditOrderBatch(new ZOrderBatchEditRequest()
                {
                    BatchCode = req.BatchCode,
                    CustomerID = req.ShipmentInfo.Buyer.ID,
                    SenderInfo = new ZOrderBatchEditSenderInfoRequest()
                    {
                        FullName = req.ShipmentInfo.Sender.FullName,
                        MobileNumber = req.ShipmentInfo.Sender.MobileNumber,
                        ProvinceID = req.ShipmentInfo.Sender.Province.ID,
                        DistrictID = req.ShipmentInfo.Sender.District.ID,
                        WardID = req.ShipmentInfo.Sender.Ward.ID,
                        Street = req.ShipmentInfo.Sender.Street,
                        PickUpAddressID = req.ShipmentInfo.Sender.PickUpAddressID,
                    },
                    ReceiverInfo = new ZOrderBatchEditReceiverInfoRequest()
                    {
                        FullName = req.ShipmentInfo.Receiver.FullName,
                        MobileNumber = req.ShipmentInfo.Receiver.MobileNumber,
                        ProvinceID = req.ShipmentInfo.Receiver.Province.ID,
                        DistrictID = req.ShipmentInfo.Receiver.District.ID,
                        WardID = req.ShipmentInfo.Receiver.Ward.ID,
                        Street = req.ShipmentInfo.Receiver.Street,
                        IsUpdatedCustomer = req.ShipmentInfo.Receiver.IsChange,
                    },
                    WareHouseID = req.WareHouseID,
                    CollectionType = req.CollectionType,
                    TransportPayer = req.TransportPayer,
                    TransportMainServiceID = req.MainTransportServiceID,
                    TransportExtraServiceID = req.ExtraTransportServiceID,
                    TotalCOD = req.TotalCOD,
                    Note = req.Note,
                    Channel = req.Channel,
                    ListOrderInfo = ListOrderInfo,
                    ActionType = req.ActionType,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    DeviceName = "WEB",
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchController.JEditOrderBatch: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sao chép đơn hàng lô
        public ActionResult CoppyOrderBatchView(SearchListOrderViewModel req)
        {
            CoppyOrderBatchViewModel model = null;
            var res = OrderBatchRepository.CoppyOrderBatchInfo(new ZOrderBatchCoppyInfoRequest() { BatchCode = Security.DecodeBase64(req.SearchKey), ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZOrderBatchCoppyInfoResponse>(res.Data);

                List<EditOrderBatchOrderInfoViewModel> ListOrderInfo = new List<EditOrderBatchOrderInfoViewModel>();
                foreach (var itemOrder in item.ListOrderInfo)
                {
                    List<ProductInfoViewModel> ListProductInfo = new List<ProductInfoViewModel>();
                    foreach (var itemProduct in itemOrder.ListProductInfo)
                    {
                        ListProductInfo.Add(new ProductInfoViewModel()
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

                    ListOrderInfo.Add(new EditOrderBatchOrderInfoViewModel()
                    {
                        OrderName = itemOrder.OrderName,
                        OrderQuantity = itemOrder.OrderQuantity,
                        OrderValue = itemOrder.OrderValue,
                        TransportFee = itemOrder.TransportFee,
                        TotalAmount = itemOrder.TotalAmount,
                        Length = itemOrder.Length,
                        Width = itemOrder.Width,
                        Height = itemOrder.Height,
                        Weight = itemOrder.Weight,
                        TotalPHU = itemOrder.TotalPHU,
                        Message = itemOrder.Message,
                        ListProductInfo = ListProductInfo
                    });
                }

                model = new CoppyOrderBatchViewModel()
                {
                    ShipmentInfo = new SearchCustomerViewModel()
                    {
                        Buyer = new CustomerModel()
                        {
                            MobileNumber = item.BuyerInfo.MobileNumber,
                            FullName = item.BuyerInfo.FullName
                        },
                        Sender = new CustomerModel()
                        {
                            PickUpAddressID = item.SenderInfo.PickUpAddressID,
                            FullName = item.SenderInfo.FullName,
                            MobileNumber = item.SenderInfo.MobileNumber,
                            Province = new ProvinceModel() { ID = item.SenderInfo.ProvinceID },
                            District = new DistrictModel() { ID = item.SenderInfo.DistrictID },
                            Ward = new WardModel() { ID = item.SenderInfo.WardID },
                            Street = item.SenderInfo.Street
                        },
                        Receiver = new CustomerModel()
                        {
                            FullName = item.ReceiverInfo.FullName,
                            MobileNumber = item.ReceiverInfo.MobileNumber,
                            Province = new ProvinceModel() { ID = item.ReceiverInfo.ProvinceID },
                            District = new DistrictModel() { ID = item.ReceiverInfo.DistrictID },
                            Ward = new WardModel() { ID = item.ReceiverInfo.WardID },
                            Street = item.ReceiverInfo.Street
                        },
                    },
                    WareHouseID = item.WareHouseID,
                    ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                    CollectionType = item.CollectionType,
                    TransportPayer = item.TransportPayer,
                    MainTransportServiceID = item.MainTransportServiceID,
                    ListMainTransportService = new List<TransportServiceModel>(),
                    ExtraTransportServiceID = item.ExtraTransportServiceID,
                    ListExtraTransportService = new List<TransportServiceModel>(),
                    TotalCOD = item.TotalCOD,
                    Note = item.Note,
                    Channel = item.Channel,
                    ListChannel = XData.GetListChannel(),
                    //TotalAmount = ListOrderInfo.Sum(m => m.TotalAmount),
                    ListOrderInfo = ListOrderInfo,
                    NumberOfDetail = ListOrderInfo.Count + 1
                };

                List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (ListTransportService.Count > 0)
                {
                    model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                    model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
                }
            }
            return View(model);
        }

        public ActionResult CoppyOrderBatchOrderInfoView(CoppyOrderBatchViewModel req)
        {
            List<CoppyOrderBatchOrderInfoViewModel> model = new List<CoppyOrderBatchOrderInfoViewModel>();
            if (req.ListOrderInfo != null && req.ListOrderInfo.Count > 0)
            {
                var i = 1;
                foreach (var item in req.ListOrderInfo)
                {
                    model.Add(new CoppyOrderBatchOrderInfoViewModel()
                    {
                        NumberOfDetail = i,
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        OrderName = item.OrderName,
                        OrderQuantity = item.OrderQuantity,
                        Length = item.Length,
                        Width = item.Width,
                        Height = item.Height,
                        Weight = item.Weight,
                        OrderValue = item.OrderValue,
                        TotalPHU = item.TotalPHU,
                        Message = item.Message,
                        ListProductInfo = item.ListProductInfo
                    });
                    i++;
                }
            }
            return PartialView(model);
        }

        public JsonResult JCoppyOrderBatch(CoppyOrderBatchViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZOrderBatchAddOrderInfoRequest> ListOrderInfo = new List<ZOrderBatchAddOrderInfoRequest>();
                foreach (var item in req.ListOrderInfo)
                {
                    List<ZOrderBatchAddProductInfoRequest> ListProductInfo = new List<ZOrderBatchAddProductInfoRequest>();
                    if (item.ListProductInfo != null)
                    {
                        foreach (var product in item.ListProductInfo)
                        {
                            ListProductInfo.Add(new ZOrderBatchAddProductInfoRequest()
                            {
                                ProductID = product.ProductID,
                                VariantID = product.VariantID,
                                NumberOfProduct = product.NumberOfProduct,
                                Price = product.Price,
                                Discount = product.Discount
                            });
                        }
                    }

                    ListOrderInfo.Add(new ZOrderBatchAddOrderInfoRequest()
                    {
                        OrderCode = item.OrderCode,
                        OrderName = item.OrderName,
                        OrderQuantity = item.OrderQuantity,
                        OrderValue = item.OrderValue,
                        TransportFee = item.TransportFee,
                        TotalAmount = item.TotalAmount,
                        TotalDiscount = item.TotalDiscount,
                        Length = item.Length,
                        Width = item.Width,
                        Height = item.Height,
                        Weight = item.Weight,
                        ExchangeWeight = item.ExchangeWeight,
                        TotalPHU = item.TotalPHU,
                        Message = item.Message,
                        ListProductInfo = ListProductInfo
                    });
                }

                res = OrderBatchRepository.AddOrderBatch(new ZOrderBatchAddRequest()
                {
                    CustomerID = req.ShipmentInfo.Buyer.ID,
                    SenderInfo = new ZOrderBatchAddSenderInfoRequest()
                    {
                        FullName = req.ShipmentInfo.Sender.FullName,
                        MobileNumber = req.ShipmentInfo.Sender.MobileNumber,
                        ProvinceID = req.ShipmentInfo.Sender.Province.ID,
                        DistrictID = req.ShipmentInfo.Sender.District.ID,
                        WardID = req.ShipmentInfo.Sender.Ward.ID,
                        Street = req.ShipmentInfo.Sender.Street,
                        PickUpAddressID = req.ShipmentInfo.Sender.PickUpAddressID,
                    },
                    ReceiverInfo = new ZOrderBatchAddReceiverInfoRequest()
                    {
                        FullName = req.ShipmentInfo.Receiver.FullName,
                        MobileNumber = req.ShipmentInfo.Receiver.MobileNumber,
                        ProvinceID = req.ShipmentInfo.Receiver.Province.ID,
                        DistrictID = req.ShipmentInfo.Receiver.District.ID,
                        WardID = req.ShipmentInfo.Receiver.Ward.ID,
                        Street = req.ShipmentInfo.Receiver.Street,
                        IsUpdatedCustomer = req.ShipmentInfo.Receiver.IsChange,
                    },
                    WareHouseID = req.WareHouseID,
                    CollectionType = req.CollectionType,
                    TransportPayer = req.TransportPayer,
                    TransportMainServiceID = req.MainTransportServiceID,
                    TransportExtraServiceID = req.ExtraTransportServiceID,
                    TotalCOD = req.TotalCOD,
                    Note = req.Note,
                    Channel = req.Channel,
                    ListOrderInfo = ListOrderInfo,
                    ActionType = req.ActionType,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    DeviceName = "WEB",
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchController.JCoppyOrderBatch: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chi tiết đơn hàng lô
        public ActionResult DetailOrderBatchView(SearchListOrderViewModel req)
        {
            DetailOrderBatchViewModel model = null;
            var res = OrderBatchRepository.DetailByBatchCode(new ZOrderBatchDetailByBatchCodeRequest() { BatchCode = req.BatchCode, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
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

                    List<EMSSignaruteAndImageInformationModel> ListEMSSignaruteAndImageInfo = new List<EMSSignaruteAndImageInformationModel>();
                    foreach (var ItemEMSSignaruteAndImageInfo in ItemOrderInfo.ListEMSSignaruteAndImageInfo)
                    {
                        ListEMSSignaruteAndImageInfo.Add(new EMSSignaruteAndImageInformationModel()
                        {
                            Signature = ItemEMSSignaruteAndImageInfo.Signature,
                            FileNames = ItemEMSSignaruteAndImageInfo.FileNames,
                            FileNamesAuth = ItemEMSSignaruteAndImageInfo.FileNamesAuth
                        });
                    }

                    List<VNPSignaruteAndImageInformationModel> ListVNPSignaruteAndImageInfo = new List<VNPSignaruteAndImageInformationModel>();
                    foreach (var ItemVNPSignaruteAndImageInfo in ItemOrderInfo.ListVNPSignaruteAndImageInfo)
                    {
                        ListVNPSignaruteAndImageInfo.Add(new VNPSignaruteAndImageInformationModel()
                        {
                            ImagesSignature = ItemVNPSignaruteAndImageInfo.ImagesSignature,
                            ImagesDelivery = ItemVNPSignaruteAndImageInfo.ImagesDelivery,
                            Images = ItemVNPSignaruteAndImageInfo.Images,
                            ImagesPlus = ItemVNPSignaruteAndImageInfo.ImagesPlus
                        });
                    }

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
                        ListEMSSignaruteAndImageInfo = ListEMSSignaruteAndImageInfo,
                        ListVNPSignaruteAndImageInfo = ListVNPSignaruteAndImageInfo
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