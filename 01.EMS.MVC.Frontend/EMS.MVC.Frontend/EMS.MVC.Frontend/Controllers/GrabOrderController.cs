using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using NPOI.SS.Util;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Globalization;

namespace EMS.MVC.Frontend.Controllers
{
    public class GrabOrderController : BaseController
    {
        // GET: GrabOrder

        #region Tạo đơn hàng siêu tốc
        public ActionResult AddGrabOrderView()
        {
            AddGrabOrderViewModel model = new AddGrabOrderViewModel();
            if (TempData.ContainsKey("MobileNumber"))
            {
                model.MobileNumber = TempData["MobileNumber"].ToString();
            }
            model.ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            model.WareHouseID = model.ListWareHouse.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListWareHouse.Where(m => m.IsDefault == "Y").First().ID : 0;
            model.Weight = 100;
            model.Quantity = 1;
            model.ListMainTransportService = new List<TransportServiceModel>()
            {
                new TransportServiceModel(){ ID = 523, Name = "Giao hàng siêu tốc" },
                new TransportServiceModel(){ ID = 524, Name = "Giao hàng siêu rẻ - 4 giờ" },
            };

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 2, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (ListTransportService.Count > 0)
            {
                model.ListExtraTransportService = ListTransportService.Where(m => m.Code == "COD").ToList();
            }
            else
            {
                model.ListExtraTransportService = new List<TransportServiceModel>();
            }
            return View(model);
        }
        public JsonResult JAddGrabOrder(AddGrabOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var GrabOrderAddRequest = JAddGrabOrderDataConvertRequest(req);
                res = GrabOrderRepository.GrabDeliveryAddOrder(GrabOrderAddRequest);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderController.JAddGrabOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ZOrderAddRequest JAddGrabOrderDataConvertRequest(AddGrabOrderViewModel req)
        {
            ZOrderAddRequest GrabOrderAddRequest = null;
            GrabOrderAddRequest = new ZOrderAddRequest()
            {
                CustomerID = req.Receiver.ID,
                ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                {
                    ReceiverName = req.Receiver.FullName,
                    MobileNumber = req.Receiver.MobileNumber,
                    ProvinceID = req.Receiver.Province.ID,
                    DistrictID = req.Receiver.District.ID,
                    WardID = req.Receiver.Ward.ID,
                    Address = req.Receiver.Street,
                    FullAddress = req.Receiver.FullAddress,
                    PickUpAddressID = req.Sender.PickUpAddressID,
                    PickupFullName = req.Sender.FullName,
                    PickupMobileNumber = req.Sender.MobileNumber,
                    PickupProvinceID = req.Sender.Province.ID,
                    PickupDistrictID = req.Sender.District.ID,
                    PickupWardID = req.Sender.Ward.ID,
                    PickupStreet = req.Sender.Street,
                    PickupFullAddress = req.Sender.FullAddress,
                },
                isUpdatedCustomer = req.IsUpdateReceiver,
                WareHouseID = req.WareHouseID,
                OrderedProducts = req.ListProduct,
                Transport = req.IsTransport,
                OrderCode = req.OrderCode,
                TransportOrderName = req.OrderName,
                TransportOrderValue = req.OrderValue,
                TransportQuantity = req.Quantity,
                TransportSizeLength = req.Length,
                TransportSizeWidth = req.Width,
                TransportSizeHeight = req.Height,
                TransportWeight = req.Weight,
                TransportPayer = req.TransportPayer,
                CollectionType = req.CollectionType.ID,
                TransportMainService = req.MainTransportServiceID,
                TransportExtraService = req.ExtraTransportServiceID,
                TotalCOD = req.TotalCOD,
                TotalPHU = req.TotalPHU,
                TransportFee = req.TransportFee,
                Note = req.Note,
                Message = req.Message,
                Channel = req.Channel,
                TotalAmount = req.TotalMoney,
                TotalDiscount = req.TotalDiscount,
                TotalOrderValue = req.TotalOrderValue,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                CreatedBy = XHelper.Session.EmployeeProfile.ID,
                isRequestOrder = req.IsRequestOrder,
                RequestOrderID = req.RequestOrderID,
                Status = req.OrderStatus,
                DeviceName = "WEB",
                PromoCode = req.PromoCode
            };
            return GrabOrderAddRequest;
        }
        #endregion

        #region Danh sách đơn hàng Grab
        public ActionResult GrabOrderView()
        {
            var listGrabOrderStatus = XData.GetListOrderStatus()
                                       .Where(m => m.Value == "N"
                                                || m.Value == "W"
                                                || m.Value == "P"
                                                || m.Value == "T"
                                                || m.Value == "V"
                                                || m.Value == "G"
                                                || m.Value == "Z"
                                                || m.Value == "C").ToList();
            GrabOrderViewModel model = new GrabOrderViewModel()
            {
                FromDate = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                CrmOrPaypostCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                ListCrmOrPaypostCode = XCommon.SearchListCrmOrPaypostCode(new ZShopGetListCrmOrPaypostCodeRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListOrderStatus = listGrabOrderStatus
            };
            return View(model);
        }
        public ActionResult SearchListGrabOrderView(GrabOrderViewModel req)
        {
            List<SearchListGrabOrderViewModel> ListItem = new List<SearchListGrabOrderViewModel>();
            //var res = GrabOrderRepository.GrabDeliveryListOrder(new ZGrabOrderSearchListRequest()
            //{
            //    FromDate = req.FromDate,
            //    ToDate = req.ToDate,
            //    OrderStatus = req.OrderStatus,
            //    ListCustomerCode = req.CrmOrPaypostCode,
            //    SearchKey = req.SearchKey,
            //    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            //});
            //List<ZGrabOrderSearchListResponse> ListData = new List<ZGrabOrderSearchListResponse>();
            var res = OrderRepository.SearchListOrder(new ZOrderFESearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                OrderStatus = req.OrderStatus,
                ListCustomerCode = req.CrmOrPaypostCode,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                Channel = "GRA"
            });
            List<ZOrderFESearchListResponse> ListData = new List<ZOrderFESearchListResponse>();
            if (res.Code == "00")
            {
                //ListData = JsonConvert.DeserializeObject<List<ZGrabOrderSearchListResponse>>(res.Data);
                //List<ZGrabOrderSearchListResponse> ListFilter = new List<ZGrabOrderSearchListResponse>();
                ListData = JsonConvert.DeserializeObject<List<ZOrderFESearchListResponse>>(res.Data);
                List<ZOrderFESearchListResponse> ListFilter = new List<ZOrderFESearchListResponse>();
                if (string.IsNullOrEmpty(req.OrderStatus))
                {
                    ListFilter = ListData.Where(m => m.OrderStatus != "C").ToList();
                }
                else
                {
                    ListFilter = ListData.Where(m => m.OrderStatus == req.OrderStatus).ToList();
                }
                foreach (var item in ListFilter)
                {
                    ListItem.Add(new SearchListGrabOrderViewModel()
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
                        TotalCOD = item.TotalCOD,
                        OrderStatus = item.OrderStatus,
                        OrderStatusName = item.OrderStatusName,
                        CreatedDate = item.CreatedDate,
                        UpdatedName = item.UpdatedName,
                        ShippingCode = item.ShippingCode,
                        ShippingID = item.ShippingID,
                        TransportOrderName = item.TransportOrderName,
                        TransportFee = item.TransportFee,
                        ShopID = item.ShopID,
                        CrmOrPaypostCode = item.CrmOrPaypostCode,
                        PaymentFreightVat = item.PaymentFreightVat,
                        PrintStatus = item.PrintStatus,
                        TransportMainServiceCode = item.TransportMainServiceCode,
                        TransportMainServiceName = item.TransportMainServiceName,
                        TransportExtraServiceCode = item.TransportExtraServiceCode,
                        TransportExtraServiceName = item.TransportExtraServiceName
                    });
                }
            }
            if (req.ReloadStatusNumber == "Y")
            {
                var listGrabOrderStatus = XData.GetListOrderStatus()
                                       .Where(m => m.Value == "N"
                                                || m.Value == "W"
                                                || m.Value == "P"
                                                || m.Value == "T"
                                                || m.Value == "V"
                                                || m.Value == "G"
                                                || m.Value == "Z"
                                                || m.Value == "C").ToList();
                List<object> OrderStatusNumber = new List<object>();
                foreach (var item in listGrabOrderStatus)
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
            ViewBag.CheckCustomerCode = req.CrmOrPaypostCode == XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode ? 1 : 2;
            ViewBag.ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID;
            return PartialView(ListItem);
        }
        #endregion

        #region Tính cước Grab
        public ActionResult TempGrabServiceFeeView(AddGrabOrderViewModel req)
        {
            TempGrabServiceFeeViewModel model = new TempGrabServiceFeeViewModel();
            if (req != null)
            {
                var res = GrabRepository.GrabDeliveryQuotation(new ZGrabTransportFeeRequest() 
                {
                    MainServiceID = req.MainTransportServiceID,

                    FromProvinceID = req.Sender.Province.ID,
                    FromDistrictID = req.Sender.District.ID,
                    FromWardID = req.Sender.Ward.ID,
                    FromFullAddress = req.Sender.FullAddress,

                    ToProvinceID = req.Receiver.Province.ID,
                    ToDistrictID = req.Receiver.District.ID,
                    ToWardID = req.Receiver.Ward.ID,
                    ToFullAddress = req.Receiver.FullAddress,

                    Weight = Convert.ToInt32(req.Weight),
                    Length = Convert.ToInt32(req.Length),
                    Width = Convert.ToInt32(req.Width),
                    Height = Convert.ToInt32(req.Height),

                    COD = Convert.ToInt32(req.TotalCOD),
                    Amount = Convert.ToInt32(req.OrderValue),
                    Quantity = Convert.ToInt32(req.Quantity),

                    ProductName = req.ProductName,
                    PromoCode = req.PromoCode,

                });

                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<ZGrabTransportFeeResponse>(res.Data);

                    model = new TempGrabServiceFeeViewModel()
                    {
                        Distance = item.Distance,
                        TotalFreight = item.TotalFreight,
                        EstimatedTimeLinePickup = item.EstimatedTimeLinePickup,
                        EstimatedTimeLineDropoff = item.EstimatedTimeLineDropoff,
                        EstimatedTimeLineCompleted = item.EstimatedTimeLineCompleted,
                    };
                }
                else
                {
                    model.Message = res.Message;
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Huỷ đơn hàng Grab
        public JsonResult JRejectGrabOrder(RejectGrabOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (string.IsNullOrEmpty(req.OrderID))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn đơn hàng";
                }
                else
                {
                    List<ZErrorDataListResponse> ListError = new List<ZErrorDataListResponse>();
                    var splitOrderID = req.OrderID.Split(',');
                    for (var i = 0; i < splitOrderID.Length; i++)
                    {
                        res = GrabOrderRepository.GrabDeliveryCancelOrder(new ZOrderCancelStatusRequest()
                        {
                            OrderID = Convert.ToInt32(splitOrderID[i]),
                            ReasonCancel = req.Reason,
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                            UpdatedBy = XHelper.Session.EmployeeProfile.ID
                        });

                        if (res.Code != "00")
                        {
                            var data = JsonConvert.DeserializeObject<ZOrderCancelStatusResponse>(res.Data);
                            ListError.Add(new ZErrorDataListResponse()
                            {
                                OrderCode = data.OrderCode,
                                Description = data.Description
                            });
                        }
                    }

                    if (ListError.Count > 0)
                    {
                        var TotalSuccess = splitOrderID.Length - ListError.Count;
                        res.Code = "10";
                        res.Message = string.Format("Hủy đơn hàng thành công {0}/{1} đơn hàng, vui lòng nhấn vào Xem chi tiết để kiểm tra.", TotalSuccess, splitOrderID.Length);
                        res.Data = JsonConvert.SerializeObject(ListError);
                    }
                    else
                    {
                        res.Code = "00";
                        res.Message = "Thành công";
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderController.JRejectGrabOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Export Excel
        public ActionResult ExportListGrabOrder(GrabOrderViewModel req)
        {
            //Create new Excel Workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel Sheet
            var sheet = workbook.CreateSheet();

            var DataFormat = workbook.CreateDataFormat();

            var fontChar = workbook.CreateFont();
            fontChar.FontName = "Times New Roman";
            fontChar.FontHeightInPoints = 12;

            var boldFont = workbook.CreateFont();
            boldFont.FontName = "Times New Roman";
            boldFont.FontHeightInPoints = 13; //Font size
            boldFont.IsBold = true;

            var TitleStyle = workbook.CreateCellStyle();
            TitleStyle.SetFont(boldFont);

            var TitleRow = sheet.CreateRow(0);
            TitleRow.CreateCell(0).SetCellValue("DANH SÁCH ĐƠN HÀNG VẬN CHUYỂN GRAB");
            TitleRow.GetCell(0).CellStyle = TitleStyle;

            var res = GrabOrderRepository.GrabDeliveryListOrder(new ZGrabOrderSearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                OrderStatus = req.OrderStatus,
                ListCustomerCode = req.CrmOrPaypostCode,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            List<ZGrabOrderSearchListResponse> ListItem = new List<ZGrabOrderSearchListResponse>();

            if (res.Code == "00")
            {
                var ListData = JsonConvert.DeserializeObject<List<ZGrabOrderSearchListResponse>>(res.Data);
                if (string.IsNullOrEmpty(req.OrderStatus))
                {
                    ListItem = ListData.Where(m => m.OrderStatus != "C").ToList();
                }
                else
                {
                    ListItem = ListData.Where(m => m.OrderStatus == req.OrderStatus).ToList();
                }
            }

            //Create a header row
            var headerRow = sheet.CreateRow(1);
            int ColumnIndex = 0;

            headerRow.CreateCell(ColumnIndex++).SetCellValue("STT");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã đơn hàng");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã bưu gửi");
            var CheckCustomerCode = req.CrmOrPaypostCode == XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode ? 1 : 2;
            if (CheckCustomerCode == 2)
            {
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã PP/CRM");
            }
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Thời gian tạo đơn");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Tên đơn hàng");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Tên người gửi");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("SĐT người gửi");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Địa chỉ người gửi");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Tên người nhận");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("SĐT người nhận");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Địa chỉ người nhận");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Nội dung");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Tiền COD");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Cước tạm tính");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Cước thực tế");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Dịch vụ chính");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Dịch vụ cộng thêm");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Người xử lý");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Trạng thái");


            //(Optional) freeze the header row so it is not scrolled
            //sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 2;
            int rowIndex = 0;
            var stt = 1;
            foreach (var item in ListItem)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(rowIndex++).SetCellValue(stt.ToString());
                row.CreateCell(rowIndex++).SetCellValue(item.OrderCode);
                row.CreateCell(rowIndex++).SetCellValue(item.ShippingCode);
                if (CheckCustomerCode == 2)
                {
                    row.CreateCell(rowIndex++).SetCellValue(item.CrmOrPaypostCode);
                }

                row.CreateCell(rowIndex++).SetCellValue(item.CreatedDate);
                row.CreateCell(rowIndex++).SetCellValue(item.TransportOrderName);

                row.CreateCell(rowIndex++).SetCellValue(item.SenderName);
                row.CreateCell(rowIndex++).SetCellValue(item.SenderPhone);
                row.CreateCell(rowIndex++).SetCellValue(item.SenderFullAddress);
                row.CreateCell(rowIndex++).SetCellValue(item.ReceiverName);
                row.CreateCell(rowIndex++).SetCellValue(item.ReceiverPhone);
                row.CreateCell(rowIndex++).SetCellValue(item.ReceiverFullAddress);
                row.CreateCell(rowIndex++).SetCellValue(item.Message);
                row.CreateCell(rowIndex++).SetCellValue(item.TotalCOD);
                row.CreateCell(rowIndex).CellStyle.DataFormat = DataFormat.GetFormat("#,##0");
                row.CreateCell(rowIndex++).SetCellValue(item.TransportFee);
                row.CreateCell(rowIndex).CellStyle.DataFormat = DataFormat.GetFormat("#,##0");
                row.CreateCell(rowIndex++).SetCellValue(item.PaymentFreightVat);
                row.CreateCell(rowIndex).CellStyle.DataFormat = DataFormat.GetFormat("#,##0");
                row.CreateCell(rowIndex++).SetCellValue(item.TransportMainServiceName);
                row.CreateCell(rowIndex++).SetCellValue(item.TransportExtraServiceName);
                row.CreateCell(rowIndex++).SetCellValue(item.UpdatedName);
                row.CreateCell(rowIndex++).SetCellValue(item.OrderStatusName);

                row.CreateCell(rowIndex).CellStyle.SetFont(fontChar);

                stt++;
                rowIndex = 0;
            }

            sheet.AutoSizeColumn(1);
            sheet.AutoSizeColumn(2);
            sheet.AutoSizeColumn(3);
            sheet.AutoSizeColumn(4);
            sheet.AutoSizeColumn(5);
            sheet.AutoSizeColumn(6);
            sheet.AutoSizeColumn(7);
            sheet.AutoSizeColumn(8);
            sheet.AutoSizeColumn(9);
            sheet.AutoSizeColumn(10);
            sheet.AutoSizeColumn(11);
            sheet.AutoSizeColumn(12);
            sheet.AutoSizeColumn(13);
            sheet.AutoSizeColumn(14);
            sheet.AutoSizeColumn(15);
            sheet.AutoSizeColumn(16);
            sheet.AutoSizeColumn(17);
            sheet.AutoSizeColumn(18);
            sheet.AutoSizeColumn(19);
            sheet.AutoSizeColumn(20);

            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachDonHangVanChuyenGrab_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".xls");
        }
        #endregion

        #region Chi tiết bưu gửi Grab
        public ActionResult GrabDetailShippingView(string ShippingID)
        {
            GrabDetailShippingViewModel model = new GrabDetailShippingViewModel()
            {
                ShippingID = Security.DecodeBase64(ShippingID),
            };
            return PartialView(model);
        }

        public ActionResult GrabInfoShippingView(GrabDetailShippingViewModel req)
        {
            GrabInfoShippingViewModel model = null;
            var res = GrabRepository.GrabDeliveryDetail(new ZGrabDetailByShippingIDRequest()
            {
                ShippingID = req.ShippingID
            });
            if (res.Code == "00")
            {
                var responseData = JsonConvert.DeserializeObject<ZGrabDetailByShippingIDResponse>(res.Data);
                model = new GrabInfoShippingViewModel() 
                {
                    CustomerCode = responseData.CustomerCode,
                    ShippingID = responseData.ShippingID,
                    ShippingCode = responseData.ShippingCode,
                    OrderCode = responseData.OrderCode,
                    Service = new BaseModel() 
                    {
                        Name = responseData.ServiceName,
                        Type = responseData.ServiceType
                    },
                    ShareLink = responseData.ShareLink,
                    Status = new BaseModel() 
                    {
                        Code = responseData.StatusCode,
                        Name = responseData.StatusName
                    },
                    Distance = responseData.Distance,
                    TransportFee = responseData.TransportFee,
                    SystemSurcharge = responseData.SystemSurcharge,
                    TotalFee = responseData.TotalFee,
                    CreatedDate = responseData.CreatedDate,
                    SendedDate = responseData.SendedDate,
                    SucceedDate = responseData.SucceedDate,
                    CanceledDate = responseData.CanceledDate,
                    SenderInfo = new GrabInfoShippingSenderInfoModel() 
                    {
                        FullName = responseData.SenderInfo.FullName,
                        MobileNumber = responseData.SenderInfo.MobileNumber,
                        ProvinceName = responseData.SenderInfo.ProvinceName,
                        DistrictName = responseData.SenderInfo.DistrictName,
                        WardName = responseData.SenderInfo.WardName,
                        FullAddress = responseData.SenderInfo.FullAddress,
                        Description = responseData.SenderInfo.Description
                    },
                    ListReceiverInfo = new List<GrabInfoShippingReceiverInfoModel>(),
                    DriverInfo = new GrabInfoShippingDriverInfoModel() 
                    {
                        FullName = responseData.DriverInfo.FullName,
                        MobileNumber = responseData.DriverInfo.MobileNumber,
                        LicensePlate = responseData.DriverInfo.LicensePlate,
                        PhotoURL = responseData.DriverInfo.PhotoURL,
                    },
                    ListOrderHistory = new List<OrderHistoryModel>(),
                };

                foreach (var item in responseData.ListReceiverInfo)
                {
                    model.ListReceiverInfo.Add(new GrabInfoShippingReceiverInfoModel()
                    {
                        FullName = item.FullName,
                        MobileNumber = item.MobileNumber,
                        ProvinceName = item.ProvinceName,
                        DistrictName = item.DistrictName,
                        WardName = item.WardName,
                        FullAddress = item.FullAddress,
                        Weight = item.Weight,
                        Length = item.Length,
                        Width = item.Width,
                        Height = item.Height,
                        TotalAmount = item.TotalAmount,
                        MoneyCollect = item.MoneyCollect,
                        EstimatedInfo = new GrabInfoShippingEstimatedInfoModel()
                        {
                            PickUpDate = item.EstimatedInfo.PickUpDate,
                            DropOffDate = item.EstimatedInfo.DropOffDate,
                            CompletedDate = item.EstimatedInfo.CompletedDate,
                        },
                    });
                }

                foreach (var item in responseData.ListOrderHistory)
                {
                    model.ListOrderHistory.Add(new OrderHistoryModel() 
                    {
                        StatusChar = item.StatusCode,
                        StatusName = item.StatusName,
                        CreatedDate = item.CreatedDate,
                    });
                }
            }
            return PartialView(model);
        }
        #endregion
    }
}