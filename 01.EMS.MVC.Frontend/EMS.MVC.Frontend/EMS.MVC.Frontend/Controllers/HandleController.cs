using System.Collections.Generic;
using System.Web.Mvc;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using EMS.Entity;
using System;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using System.Linq;

namespace EMS.MVC.Frontend.Controllers
{
    public class HandleController : Controller
    {

        #region Danh sách Tỉnh/ Thành phố
        public JsonResult SearchListProvince(int Type = 0)
        {
            List<ProvinceModel> ListItem = new List<ProvinceModel>();
            try
            {
                ListItem = XCommon.SearchListProvince(new ZProvinceListRequest{
                    Type = Type
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListProvince: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách Quận/ Huyện
        public JsonResult SearchListDistrictByProvinceID(int ProvinceID = 0)
        {
            List<DistrictModel> ListItem = new List<DistrictModel>();
            try
            {
                ListItem = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = ProvinceID });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListDistrictByProvinceID: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy ID Quận/ Huyện bằng tên Quận/ Huyện
        public JsonResult SearchDistrictIDByDistrictName(int ProvinceID, string DistrictName)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PWDRepository.SearchDistrictIDByDistrictName(new ZPDWGetIDByPDWNameRequest() { ID = ProvinceID, Name = DistrictName });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchDistrictIDByDistrictName: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách Phường/ Xã
        public JsonResult SearchListWardByDistrictID(int DistrictID = 0, int Type = 0)
        {
            List<WardModel> ListItem = new List<WardModel>();
            try
            {
                ListItem = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = DistrictID, Type = Type });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListWardByDistrictID: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy ID Phường/ Xã bằng tên Phường/ Xã
        public JsonResult SearchWardIDByWardName(int DistrictID, string WardName)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PWDRepository.SearchWardIDByWardName(new ZPDWGetIDByPDWNameRequest() { ID = DistrictID, Name = WardName });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchWardIDByWardName: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gợi ý địa chỉ của VNP
        public JsonResult VNPMapsAutocomplete(ZVNPMapsAutocompleteRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = VNPRepository.VNPMapsAutocomplete(req);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.VNPMapsAutocomplete: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo VMapCode (từ gợi ý địa chỉ của VNP)
        public JsonResult SearchPWDByVMapCode(ZMapsGetByVMapCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PWDRepository.SearchPWDByVMapCode(new ZMapsGetByVMapCodeRequest()
                {
                    ProvinceVMapCode = req.ProvinceVMapCode == 0 ? 0 : req.ProvinceVMapCode - 100000000, //mã bưu chính = gid - 100000000
                    DistrictVMapCode = req.DistrictVMapCode == 0 ? 0 : req.DistrictVMapCode - 100000000, //mã bưu chính = gid - 100000000
                    WardVMapCode = req.WardVMapCode == 0 ? 0 : req.WardVMapCode - 100000000, //mã bưu chính = gid - 100000000
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchPWDByVMapCode: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo địa chỉ nhập vào (từ gợi ý địa chỉ của VNP)

        public JsonResult SearchPWDByVNPMapsAutocomplete(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XCommon.SearchPWDByVNPMapsAutocomplete(Address);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchPWDByVNPMapsAutocomplete: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchPWDByVNPMapsSearch(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XCommon.SearchPWDByVNPMapsSearch(Address);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchPWDByVNPMapsSearch: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo địa chỉ nhập vào (từ gợi ý địa chỉ của EMS)

        public JsonResult SearchPWDByEMSMapsSearch(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XCommon.SearchPWDByEMSMapsSearch(Address);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchPWDByEMSMapsSearch: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchPWDByEMSVMapsSearch(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XCommon.SearchPWDByEMSMapsSearchV2(Address);
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchPWDByEMSMapsSearch: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy chi tiết kỳ đối soát theo ID
        public JsonResult ControlPeriodDateByID(int ID)
        {
            ControlPeriodDateModel Item = new ControlPeriodDateModel();
            Item = ReconciliationRepository.ControlPeriodDateID(ID);
            return Json(Item, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy danh sách các biến thể sản phẩm (theo mã kho)

        public JsonResult GetListProductVariantByWareHouseID(int WareHouseID)
        {
            List<object> listItem = new List<object>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_VARIANT_GET_BY_WAREHOUSE_ID",
                    Data = JsonConvert.SerializeObject(new ZProductVariantGetByWareHouseIDRequest()
                    {
                        WareHouseID = WareHouseID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByWareHouseIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new
                        {
                            DetailID = item.ProductID + "_" + item.ProductVariantID,
                            DetailName = item.ProductName + " (" + item.ProductVariantName + ")",
                            ProductVariantSKU = item.ProductVariantSKU,
                            ProductVariantPrice = item.ProductVariantPrice,
                            NumberOfInventory = item.NumberOfInventory
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.GetListProductVariantByWareHouseID: " + ex.Message);
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListProductVariantByWarehouseID_V2(int WareHouseID)
        {
            List<object> listItem = new List<object>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_VARIANT_GET_BY_WAREHOUSE_ID",
                    Data = JsonConvert.SerializeObject(new ZProductVariantGetByWareHouseIDRequest()
                    {
                        WareHouseID = WareHouseID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByWareHouseIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new
                        {
                            ID = item.ProductVariantID,
                            Name = item.ProductName + " (" + item.ProductVariantName + ")",
                            SKU = item.ProductVariantSKU,
                            Price = item.ProductVariantPrice,
                            NumberOfInventory = item.NumberOfInventory
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.GetListProductVariantByWarehouseID_V2: " + ex.Message);
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chi tiết bưu gửi - Tracking bưu gửi
        public ActionResult DetailShippingView(string ShippingCode, string ShopID)
        {
            DetailShippingViewModel model = new DetailShippingViewModel()
            {
                ShippingCode = Security.DecodeBase64(ShippingCode),
                ShopID = ShopID
            };
            return PartialView(model);
        }

        public ActionResult InfoShippingView(DetailShippingViewModel req)
        {
            InfoShippingViewModel model = null;
            var res = OrderRepository.DetailByShippingCode(new ZOrderDetailByShippingCodeRequest()
            {
                ShippingCode = req.ShippingCode,
                ShopID = string.IsNullOrEmpty(req.ShopID) ? 0 : Convert.ToInt32(Security.DecodeBase64(req.ShopID)),
                AuthorizationCode = XHelper.Session == null ? "" : "NEKO",
                EmployeeID = XHelper.Session.EmployeeProfile.ID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZOrderDetailByShippingCodeResponse>(res.Data);

                List<OrderHistoryModel> ListOrderHistory = new List<OrderHistoryModel>();
                foreach (var ItemOrderHistory in item.ListSearchHistory)
                {
                    ListOrderHistory.Add(new OrderHistoryModel()
                    {
                        StatusTimePartner = ItemOrderHistory.StatusTimePartner,
                        CreatedDate = ItemOrderHistory.CreatedDate,
                        StatusName = ItemOrderHistory.StatusName,
                        PosCode = ItemOrderHistory.PosCode,
                        PosName = ItemOrderHistory.PosName,
                        PosTel = ItemOrderHistory.PosTel,
                        PosAddress = ItemOrderHistory.PosAddress,
                        StatusNote = ItemOrderHistory.StatusNote
                    });
                }

                List<RequestSupportModel> ListUpdateInformation = new List<RequestSupportModel>();
                //foreach (var ItemUpdateInformation in item.ListUpdateInformation)
                //{
                //    ListUpdateInformation.Add(new RequestSupportModel()
                //    {
                //        RequestSupportType = new BaseModel()
                //        {
                //            ID = ItemUpdateInformation.RequestSupportType.ID,
                //            Name = ItemUpdateInformation.RequestSupportType.Name,
                //        },
                //        OldUpdateInformation = ItemUpdateInformation.OldUpdateInformation,
                //        NewUpdateInformation = ItemUpdateInformation.NewUpdateInformation,
                //        CreatedDate = ItemUpdateInformation.CreatedDate
                //    });
                //}

                List<EMSSignaruteAndImageInformationModel> ListEMSSignaruteAndImageInfo = new List<EMSSignaruteAndImageInformationModel>();
                foreach (var ItemEMSSignaruteAndImageInfo in item.ListEMSSignaruteAndImageInfo)
                {
                    if(ItemEMSSignaruteAndImageInfo.StatusCode == "C14")
                    {
                        ListEMSSignaruteAndImageInfo.Add(new EMSSignaruteAndImageInformationModel()
                        {
                            Signature = ItemEMSSignaruteAndImageInfo.Signature,
                            FileNames = ItemEMSSignaruteAndImageInfo.FileNames,
                            FileNamesAuth = ItemEMSSignaruteAndImageInfo.FileNamesAuth,
                            ImagesPlus = ItemEMSSignaruteAndImageInfo.ImagePlus
                        });
                    }        
                }

                List<VNPSignaruteAndImageInformationModel> ListVNPSignaruteAndImageInfo = new List<VNPSignaruteAndImageInformationModel>();
                foreach (var ItemVNPSignaruteAndImageInfo in item.ListVNPSignaruteAndImageInfo)
                {
                    if(ItemVNPSignaruteAndImageInfo.StatusCode == "C14")
                    {
                        ListVNPSignaruteAndImageInfo.Add(new VNPSignaruteAndImageInformationModel()
                        {
                            ImagesSignature = ItemVNPSignaruteAndImageInfo.ImagesSignature,
                            ImagesDelivery = ItemVNPSignaruteAndImageInfo.ImagesDelivery,
                            Images = ItemVNPSignaruteAndImageInfo.Images,
                            ImagesPlus = ItemVNPSignaruteAndImageInfo.ImagesPlus
                        });
                    }
                }

                
                model = new InfoShippingViewModel()
                {
                    OrderID = item.OrderID,
                    ShippingCode = item.ShippingCode,
                    ListShippingCode = item.ShippingCode.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Security.EncodeBase64(x)).ToArray(),
                    OrderCode = item.OrderCode,
                    MainTransportService = new BaseModel()
                    {
                        Name = item.TransportMainServiceName
                    },
                    ExtraTransportService = new BaseModel()
                    {
                        Code = item.TransportExtraServiceCode,
                        Name = item.TransportExtraServiceName
                    },
                    CreatedDate = item.CreatedDate,
                    Length = item.TransportSizeLength.ToString(),
                    Width = item.TransportSizeWidth.ToString(),
                    Height = item.TransportSizeHeight.ToString(),
                    TransportWeight = item.TransportWeight.ToString("N0"),
                    ExchangeTransportWeight = item.ExchangeTransportWeight.ToString("N0"),
                    TotalCOD = item.TotalCOD.ToString("N0"),
                    TotalPHU = item.TotalPHU.ToString("N0"),
                    Message = item.Message,
                    PosCodeAccept = item.PosCodeAccept,
                    PosNameAccept = item.PosNameAccept,
                    PosTelAccept = item.PosTelAccept,
                    PosCodeDelivery = item.PosCodeDelivery,
                    PosNameDelivery = item.PosNameDelivery,
                    PosTelDelivery = item.PosTelDelivery,
                    Sender = new CustomerModel()
                    {
                        FullName = item.SenderName,
                        MobileNumber = item.SenderPhone,
                        FullAddress = item.SenderFullAddress
                    },
                    Receiver = new CustomerModel()
                    {
                        FullName = item.ReceiverName,
                        MobileNumber = item.ReceiverPhone,
                        FullAddress = item.ReceiverFullAddress
                    },
                    OrderStatus = new BaseModel()
                    {
                        Code = item.StatusChar,
                        Name = item.StatusName,
                    },
                    ListSearchHistory = ListOrderHistory,
                    ListUpdateInformation = ListUpdateInformation,
                    ListSearchHistoryDelivery = item.ListSearchHistoryDelivery,
                    ListEMSSignaruteAndImageInfo = ListEMSSignaruteAndImageInfo,
                    ListVNPSignaruteAndImageInfo = ListVNPSignaruteAndImageInfo
                };
                ViewBag.ShopID = req.ShopID;
            }
            return PartialView(model);
        }
        #endregion

        #region In đơn hàng
        public ActionResult PrintOrderView()
        {
            PrintOrderViewModel model = new PrintOrderViewModel();
            if (XHelper.Session.EmployeeProfile == null)
            {
                model.PrintTypeDefault = "A4";
            }
            else
            {
                model.PrintTypeDefault = XHelper.Session.EmployeeProfile.ShopInformation.PrintTypeDefault;
            }
            return PartialView(model);
        }

        public ActionResult PrintOrderTypeView(PrintOrderViewModel req)
        {
            List<PrintOrderTypeViewModel> model = new List<PrintOrderTypeViewModel>();
            var ListOrderPrint = XCommon.ListPrintOrder(req);
            if (ListOrderPrint.Count > 0)
            {
                ViewBag.OrderIDs = string.Join(",", ListOrderPrint.Select(m => m.OrderID));
                if (req.PrintType == "A4" || req.PrintType == "A61" || req.PrintType == "B1" || req.PrintType == "B2" || req.PrintType == "Label")
                {
                    model.Add(new PrintOrderTypeViewModel()
                    {
                        ListOrderPrint = ListOrderPrint
                    });
                }
                else if (req.PrintType == "A64")
                {
                    var TotalPage = (ListOrderPrint.Count / 4) + 1;
                    var count = 0;
                    var index = 0;
                    for (var i = 1; i <= TotalPage; i++)
                    {
                        List<PrintOrderTypeDataModel> ListTemp = new List<PrintOrderTypeDataModel>();
                        for (var j = index; j < ListOrderPrint.Count; j++)
                        {
                            count++;
                            if (count > 4)
                            {
                                count = 0;
                                break;
                            }
                            index++;
                            ListTemp.Add(new PrintOrderTypeDataModel()
                            {
                                OrderID = ListOrderPrint[j].OrderID,
                                OrderCode = ListOrderPrint[j].OrderCode,
                                ShippingCode = ListOrderPrint[j].ShippingCode,
                                PickUpProvinceName = ListOrderPrint[j].PickUpProvinceName,
                                SenderFullName = ListOrderPrint[j].SenderFullName,
                                SenderFullAddress = ListOrderPrint[j].SenderFullAddress,
                                SenderDistrictName = ListOrderPrint[j].SenderDistrictName,
                                SenderProvinceName = ListOrderPrint[j].SenderProvinceName,
                                SenderMobileNumber = ListOrderPrint[j].SenderMobileNumber,
                                SenderSentDate = ListOrderPrint[j].SenderSentDate,
                                RecieverFullName = ListOrderPrint[j].RecieverFullName,
                                RecieverFullAddress = ListOrderPrint[j].RecieverFullAddress,
                                RecieverDistrictName = ListOrderPrint[j].RecieverDistrictName,
                                RecieverProvinceName = ListOrderPrint[j].RecieverProvinceName,
                                RecieverMobileNumber = ListOrderPrint[j].RecieverMobileNumber,
                                Message = ListOrderPrint[j].Message,
                                Note = ListOrderPrint[j].Note,
                                TransportWeight = ListOrderPrint[j].TransportWeight,
                                TransportQuantity = ListOrderPrint[j].TransportQuantity,
                                TransportMainServiceCode = ListOrderPrint[j].TransportMainServiceCode,
                                TransportMainServiceName = ListOrderPrint[j].TransportMainServiceName,
                                TransportExtraServiceCode = ListOrderPrint[j].TransportExtraServiceCode,
                                TransportExtraServiceName = ListOrderPrint[j].TransportExtraServiceName,
                                TotalCOD = ListOrderPrint[j].TotalCOD,
                                TotalPHU = ListOrderPrint[j].TotalPHU,
                                QRCode = ListOrderPrint[j].QRCode,
                                ShippingBarCode = ListOrderPrint[j].ShippingBarCode,
                                OrderBarCode = ListOrderPrint[j].OrderBarCode,
                                PaymentFreightVat = ListOrderPrint[j].PaymentFreightVat,
                                TransportPayer = ListOrderPrint[j].TransportPayer,
                                TransportFee = ListOrderPrint[j].TransportFee,
                                PostOffice = ListOrderPrint[j].PostOffice,
                            });
                        }
                        if (ListTemp.Count > 0)
                        {
                            model.Add(new PrintOrderTypeViewModel()
                            {
                                Page = i,
                                ListOrderPrint = ListTemp
                            });
                        }
                    }
                }
            }
            ViewBag.PrintType = req.PrintType;
            //ViewBag.TemplatePrintData = req.TemplatePrintData;
            return PartialView(model);
        }
        #endregion

        #region Danh sách nhóm/ loại yêu cầu (RequestSupportGroup/Type)
        public JsonResult SearchListRequestSupportGroup(int ParentID = 0)
        {
            List<ZRequestSupportGroupListResponse> ListItem = new List<ZRequestSupportGroupListResponse>();
            try
            {
                ListItem = XCommon.SearchListRequestSupportGroup(new ZRequestSupportGroupListRequest() { ParentID = ParentID });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListRequestSupportGroup: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy chi tiết đơn hàng bằng ShippingCode
        public JsonResult DetailOrderByShippingCode(ZOrderGetByShippingCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XCommon.GetByShippingCode(new ZOrderGetByShippingCodeRequest() { ShippingCode = req.ShippingCode, ShopID = req.ShopID });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.DetailOrderByShippingCode: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Hiển thị danh sách chi tiết lỗi
        public ActionResult ShowListErrorMultipleSelect(string Error, string Controller)
        {
            List<ShowListErrorMultipleSelectViewModel> ListItem = new List<ShowListErrorMultipleSelectViewModel>();
            var data = JsonConvert.DeserializeObject<List<ZErrorDataListResponse>>(Error);
            if (Controller == "Order")
            {
                foreach (var item in data)
                {
                    ListItem.Add(new ShowListErrorMultipleSelectViewModel()
                    {
                        OrderCode = item.OrderCode,
                        BatchCode = item.BatchCode,
                        Description = item.Description
                    });
                }
            }
            else if (Controller == "RequestSupport")
            {
                foreach (var item in data)
                {
                    ListItem.Add(new ShowListErrorMultipleSelectViewModel()
                    {
                        RequestSupportCode = item.RequestSupportCode,
                        Description = item.Description
                    });
                }
            }
            else if (Controller == "IsAgree")
            {
                foreach (var item in data)
                {
                    ListItem.Add(new ShowListErrorMultipleSelectViewModel()
                    {
                        EmsBarCode = item.EmsBarCode,
                        Description = item.Description
                    });
                }
            }
            else if (Controller == "Product")
            {
                foreach (var item in data)
                {
                    ListItem.Add(new ShowListErrorMultipleSelectViewModel()
                    {
                        ProductName = item.ProductName,
                        Description = item.Description
                    });
                }
            }
            else if (Controller == "TIK_Product")
            {
                foreach (var item in data)
                {
                    ListItem.Add(new ShowListErrorMultipleSelectViewModel()
                    {
                        ProductName = item.ProductName,
                        ProductVariantSKU = item.ProductVariantSKU,
                        Description = item.Description
                    });
                }
            }
            ViewBag.Controller = Controller;
            return PartialView(ListItem);
        }
        #endregion

        #region Mẫu in (Template Print)

        #region Danh sách mẫu in
        public JsonResult SearchListTemplatePrint(ZTemplatePrintSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TemplatePrintRepository.SearchListTemplatePrint(new ZTemplatePrintSearchListRequest()
                {
                    PrintType = req.PrintType,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    TemplateType = "ORDER"
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListTemplatePrint: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Sản phẩm (Product)

        #region Danh sách sản phẩm 
        public JsonResult SearchListProductVariantByWareHouseID(ZProductVariantGetByWareHouseIDRequest req)
        {
            List<ZProductVariantGetByWareHouseIDResponse> ListItem = new List<ZProductVariantGetByWareHouseIDResponse>();
            try
            {
                ListItem = XCommon.SearchListProductVariantByWareHouseID(new ZProductVariantGetByWareHouseIDRequest() { WareHouseID = req.WareHouseID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListProductVariantByWareHouseID: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Danh sách nhóm quyền
        public JsonResult SearchListGroupPermission(ZShopGroupPermissionListRequest req)
        {
            List<ZShopGroupPermissionListResponse> ListItem = new List<ZShopGroupPermissionListResponse>();
            try
            {
                ListItem = XCommon.SearchListGroupPermission(new ZShopGroupPermissionListRequest() { ShopLevel = req.ShopLevel, IsLock = "N", IsShop = "Y" });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListGroupPermission: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách chức năng theo nhóm quyền
        public JsonResult SearchListFunctionalByGroupPermissionID(ZFunctionalListRequest req)
        {
            List<ZFunctionalListResponse> ListItem = new List<ZFunctionalListResponse>();
            try
            {
                var res = PermissionRepository.SearchListFunctional(new ZFunctionalListRequest() { GroupPermissionID = req.GroupPermissionID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (res.Code == "00")
                {
                    ListItem = JsonConvert.DeserializeObject<List<ZFunctionalListResponse>>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListFunctionalByGroupPermissionID: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ZALO
        public JsonResult SearchListConversion(string Channel, int LimitRow = 0, string UpdatedDate = "", string UserID = "")
        {
            ZResponse res = new ZResponse();
            try
            {
                if (Channel == "ZA" && XHelper.Session.ZaloProfile == null)
                {
                    res.Code = "408";
                    res.Message = "Thời gian đăng nhập đã hết, vui lòng đăng nhập lại.";
                }
                else if (Channel == "FB" && XHelper.Session.FacebookProfile == null)
                {
                    res.Code = "408";
                    res.Message = "Thời gian đăng nhập đã hết, vui lòng đăng nhập lại.";
                }
                else
                {
                    res = ConversionRepository.SearchListConversion(new ZConversionSearchListRequest()
                    {
                        PageID = Channel == "ZA" ? XHelper.Session.ZaloProfile.PageID : XHelper.Session.FacebookProfile.PageID,
                        UserID = UserID,
                        Channel = Channel,
                        LimitRow = LimitRow,
                        UpdatedDate = UpdatedDate
                    });
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListConversion: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MessageOfConversion(int ConversionID, string Channel, int LimitRow = 0, string CreatedDate = "")
        {
            ZResponse res = new ZResponse();
            try
            {
                if (Channel == "ZA" && XHelper.Session.ZaloProfile == null)
                {
                    res.Code = "408";
                    res.Message = "Thời gian đăng nhập đã hết, vui lòng đăng nhập lại.";
                }
                else if (Channel == "FB" && XHelper.Session.FacebookProfile == null)
                {
                    res.Code = "408";
                    res.Message = "Thời gian đăng nhập đã hết, vui lòng đăng nhập lại.";
                }
                else
                {
                    var resMessageOfConversion = MessageRepository.SearchListMessageOfConversion(new ZMessageSearchListRequest()
                    {
                        ConversionID = ConversionID,
                        Channel = Channel,
                        LimitRow = LimitRow,
                        CreatedDate = CreatedDate
                    });
                    if (resMessageOfConversion.Code == "00")
                    {
                        var list = JsonConvert.DeserializeObject<List<ZMessageSearchListResponse>>(resMessageOfConversion.Data);
                        res.Code = resMessageOfConversion.Code;
                        res.Message = resMessageOfConversion.Message;
                        if (LimitRow > 0)
                        {
                            res.Data = JsonConvert.SerializeObject(list);
                        }
                        else
                        {
                            res.Data = JsonConvert.SerializeObject(list.OrderBy(m => DateTime.ParseExact(m.CreatedDate, "dd/MM/yyyy HH:mm:ss", null)).ThenBy(m => DateTime.ParseExact(m.AmndDate, "dd/MM/yyyy HH:mm:ss", null)).ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.MessageOfConversion: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách kho
        public JsonResult SearchListWareHouse()
        {
            List<WareHouseModel> ListItem = new List<WareHouseModel>();
            try
            {
                ListItem = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListWareHouse: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Phóng to hình ảnh và hiển thị dạng Slide
        public ActionResult ImageZoomAndSliderView(List<string> ListImage, string ActiveImage)
        {
            List<ImageZoomAndSliderViewModel> model = new List<ImageZoomAndSliderViewModel>();

            for (var i = 0; i < ListImage.Count; i++)
            {
                model.Add(new ImageZoomAndSliderViewModel()
                {
                    //Title = ListImage[i].Title,
                    Url = ListImage[i],
                    IsActive = ActiveImage == ListImage[i] ? "active" : ""
                });
            }

            return PartialView(model);
        }
        #endregion

        #region Tính cước
        public JsonResult TransportPostageCharged(ZTransportPostageChargedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XCommon.TransportPostageCharged(req);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.TransportPostageCharged: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách khách hàng
        public JsonResult SearchListCustomer(ZCustomerSearchListRequest req)
        {
            List<CustomerModel> ListItem = new List<CustomerModel>();
            try
            {
                ListItem = XCommon.SearchListCustomer(new ZCustomerSearchListRequest() { SearchKey = req.SearchKey, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListCustomer: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách bưu cục thu gom gần theo quận/ huyện, tỉnh/ thành phố
        public JsonResult SearchListRecentCollectionPostOffice(ZCollectionPostOfficeSearchListRecentRequest req)
        {
            List<CollectionPostOfficeModel> ListItem = new List<CollectionPostOfficeModel>();
            try
            {
                ListItem = XCommon.SearchListRecentCollectionPostOffice(new ZCollectionPostOfficeSearchListRecentRequest() { ProvinceID = req.ProvinceID, DistrictID = req.DistrictID });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListRecentCollectionPostOffice: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ngày dự kiến giao hàng
        public JsonResult SearchDateExpectedDelivery(ZExpectedDeliverySearchDateRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XCommon.SearchDateExpectedDelivery(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchDateExpectedDelivery: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy thông tin khách hàng bằng số điện thoại
        public JsonResult SearchCustomerByMobileNumber(ZCustomerGetByMobileNumberRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XCommon.SearchCustomerByMobileNumber(new ZCustomerGetByMobileNumberRequest() { MobileNumber = req.MobileNumber, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchCustomerByMobileNumber: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region ErrorView
        public ActionResult ErrorView()
        {
            return PartialView();
        }
        #endregion
    }
}