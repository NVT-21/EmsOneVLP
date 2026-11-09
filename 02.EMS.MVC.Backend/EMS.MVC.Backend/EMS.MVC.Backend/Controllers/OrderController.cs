using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EMS.Library;
using EMS.Entity;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
using Newtonsoft.Json;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace EMS.MVC.Backend.Controllers
{
    public class OrderController : BaseController
    {
        #region Danh sách đơn hàng

        public ActionResult OrderView()
        {
            OrderViewModel model = new OrderViewModel()
            {
                FromDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>(),
                ListOrderStatus = XData.GetListOrderStatus(),
                ListPlaceOfCreateOrder = XData.GetListPlaceOfCreateOrder(),
                ListTypeOrder = XData.GetListTypeOrder(),
                ListPostalType = XData.GetListPostalType().OrderByDescending(m => m.Value).ToList(),
                ListPartner = XCommon.SearchListPartner(new ZPartnerSearchListRequest() { IsLock = "N" }),
            };

            model.ListPostalType[0].Text = "Loại bưu gửi";

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { IsLock = "N", ParentID = 0, Channel = "EMSONE" });
            if (ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
            }

            return View(model);
        }

        public ActionResult SearchListOrderView(OrderViewModel req)
        {
            List<SearchListOrderViewModel> model = new List<SearchListOrderViewModel>();
            var res = OrderRepository.SearchListOrder(new ZOrderBESearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                OrderStatus = req.OrderStatus,
                OrderCode = req.OrderCode,
                ShippingCode = req.ShippingCode,
                ShopCode = req.ShopCode,
                CrmOrPaypostCode = req.CrmOrPaypostCode,
                ReceiverProvinceID = req.ReceiverProvinceID,
                TransportMainService = req.TransportMainService,
                TransportExtraService = req.TransportExtraService,
                PlaceOfCreateOrder = req.PlaceOfCreateOrder,
                PostalType = req.PostalType,
                PartnerID = req.PartnerID,
                Channel = req.TypeOrder,
                UserID = XHelper.Session.UserProfile.ID,
            });
            List<ZOrderBESearchListResponse> ListData = new List<ZOrderBESearchListResponse>();
            if (res.Code == "00")
            {
                ListData = JsonConvert.DeserializeObject<List<ZOrderBESearchListResponse>>(res.Data);
                List<ZOrderBESearchListResponse> ListFilter = new List<ZOrderBESearchListResponse>();
                if (string.IsNullOrEmpty(req.OrderStatus))
                {
                    ListFilter = ListData.Where(m => m.OrderStatus.Code != "C" && m.OrderStatus.Code != "D" && m.OrderStatus.Code != "L").ToList();
                }
                else
                {
                    ListFilter = ListData.Where(m => m.OrderStatus.Code == req.OrderStatus).ToList();
                }
                foreach (var item in ListFilter)
                {
                    model.Add(new SearchListOrderViewModel()
                    {
                        OrderID = item.OrderID,
                        EMSOneCode = item.EMSOneCode,
                        OrderCode = item.OrderCode,
                        BatchCode = item.BatchCode,
                        SenderName = item.SenderName,
                        SenderPhone = item.SenderPhone,
                        SenderFullAddress = item.SenderFullAddress,
                        ReceiverName = item.ReceiverName,
                        ReceiverPhone = item.ReceiverPhone,
                        ReceiverFullAddress = item.ReceiverFullAddress,
                        Message = item.Message,
                        TotalCOD = item.TotalCOD,
                        Transport = item.Transport,
                        TransportFee = item.TransportFee,
                        ProvisionalTransportWeight = item.ProvisionalTransportWeight,
                        ActualTransportWeight = item.ActualTransportWeight,
                        OrderStatus = new BaseModel()
                        {
                            Code = item.OrderStatus.Code,
                            Name = item.OrderStatus.Name
                        },
                        TransportMainService = new BaseModel()
                        {
                            Code = item.TransportMainService.Code,
                            Name = item.TransportMainService.Name,
                        },
                        TransportExtraService = new BaseModel()
                        {
                            Code = item.TransportExtraService.Code,
                            Name = item.TransportExtraService.Name,
                        },
                        CreatedDate = item.CreatedDate,

                        UpdatedName = item.UpdatedName,
                        UpdatedDate = item.UpdatedDate,
                        ReasonCancel = item.ReasonCancel,

                        PostalType = item.PostalType,

                        ShippingID = item.ShippingID,
                        ShippingCode = item.ShippingCode,

                        PaymentFreightVat = item.PaymentFreightVat,
                        TrackingItemInfoCreatedDate = item.TrackingItemInfoCreatedDate,

                        ShopID = item.ShopID,
                        ShopCode = item.ShopCode,
                        CrmOrPaypostCode = item.CrmOrPaypostCode,
                        PrintTypeDefault = item.PrintTypeDefault,
                        StoreName = item.StoreName,
                        PartnerName = item.PartnerName,

                        CustomerName = item.CustomerName,
                        CustomerPhone = item.CustomerPhone,
                        CustomerFullAddress = item.CustomerFullAddress,
                    });
                }
            }
            if (req.ReloadStatusNumber == "Y")
            {
                ViewBag.ListOrderStatus = XData.GetListOrderStatus();
                List<object> CountOrderStatus = new List<object>();
                foreach (var item in ViewBag.ListOrderStatus)
                {
                    var Count = ListData.Where(m => m.OrderStatus.Code == item.Value).Count();
                    CountOrderStatus.Add(new
                    {
                        Status = item.Value,
                        Count = Count
                    });
                }
                ViewBag.CountOrderStatus = CountOrderStatus;
            }
            ViewBag.ReloadCountStatus = req.ReloadStatusNumber;
            ViewBag.SearchOrderStatus = req.OrderStatus;
            return PartialView(model);
        }

        public ActionResult ExportListOrder(OrderViewModel req)
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

            var HeaderStyle = workbook.CreateCellStyle();
            HeaderStyle.SetFont(boldFont);
            HeaderStyle.BorderTop = BorderStyle.Thin;
            HeaderStyle.BorderRight = BorderStyle.Thin;
            HeaderStyle.BorderBottom = BorderStyle.Thin;
            HeaderStyle.BorderLeft = BorderStyle.Thin;

            var BodyTextStyle = workbook.CreateCellStyle();
            BodyTextStyle.SetFont(fontChar);
            BodyTextStyle.BorderTop = BorderStyle.Thin;
            BodyTextStyle.BorderRight = BorderStyle.Thin;
            BodyTextStyle.BorderBottom = BorderStyle.Thin;
            BodyTextStyle.BorderLeft = BorderStyle.Thin;

            var BodyNumberStyle = workbook.CreateCellStyle();
            BodyNumberStyle.SetFont(fontChar);
            BodyNumberStyle.BorderTop = BorderStyle.Thin;
            BodyNumberStyle.BorderRight = BorderStyle.Thin;
            BodyNumberStyle.BorderBottom = BorderStyle.Thin;
            BodyNumberStyle.BorderLeft = BorderStyle.Thin;
            BodyNumberStyle.DataFormat = DataFormat.GetFormat("#,##0");

            //var center = workbook.CreateCellStyle();
            //center.Alignment = HorizontalAlignment.Center;
            //center.SetFont(fontChar);

            //Create title 
            var TitleRow = sheet.CreateRow(0);
            TitleRow.CreateCell(0).SetCellValue("DANH SÁCH ĐƠN HÀNG");
            TitleRow.GetCell(0).CellStyle = TitleStyle;

            //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 12));
            //sheet.CreateRow(0).GetCell(0).CellStyle = center;
            var res = OrderRepository.SearchListOrder(new ZOrderBESearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                OrderStatus = req.OrderStatus,
                OrderCode = req.OrderCode,
                ShippingCode = req.ShippingCode,
                ShopCode = req.ShopCode,
                CrmOrPaypostCode = req.CrmOrPaypostCode,
                ReceiverProvinceID = req.ReceiverProvinceID,
                TransportMainService = req.TransportMainService,
                TransportExtraService = req.TransportExtraService,
                PlaceOfCreateOrder = req.PlaceOfCreateOrder,
                PostalType = req.PostalType,
                PartnerID = req.PartnerID,
                Channel = req.TypeOrder,
                UserID = XHelper.Session.UserProfile.ID,
            });
            List<ZOrderBESearchListResponse> ListItem = new List<ZOrderBESearchListResponse>();

            if (res.Code == "00")
            {
                var ListData = JsonConvert.DeserializeObject<List<ZOrderBESearchListResponse>>(res.Data);
                if (string.IsNullOrEmpty(req.OrderStatus))
                {
                    ListItem = ListData.Where(m => m.OrderStatus.Code != "C" && m.OrderStatus.Code != "D" && m.OrderStatus.Code != "L").ToList();
                }
                else
                {
                    ListItem = ListData.Where(m => m.OrderStatus.Code == req.OrderStatus).ToList();
                }
            }

            //Create a header row
            var headerRow = sheet.CreateRow(2);
            int ColumnIndex = 0;
            var TotalColumn = req.OrderStatus == "L" ? 15 : 23;
            var DefaultColumn = req.OrderStatus == "L" ? "10,11,12" : "14,15,16,17";

            headerRow.CreateCell(ColumnIndex++).SetCellValue("STT");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã đơn hàng");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã bưu gửi");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Loại bưu gửi");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Thời gian tạo đơn");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã KH");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã PP/CRM");

            if (req.OrderStatus == "L")
            {
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Tên người mua");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("SĐT người mua");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Địa chỉ người mua");
            }
            else
            {
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Tên người gửi");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("SĐT người gửi");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Địa chỉ người gửi");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Tên người nhận");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("SĐT người nhận");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Địa chỉ người nhận");
            }

            headerRow.CreateCell(ColumnIndex++).SetCellValue("Nội dung");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Tiền COD");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Cước tạm tính");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Cước thực tế");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Khối lượng thực tế");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã bưu cục chấp nhận");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã bưu cục phát");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Ngày phát hành");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Dịch vụ chính");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Dịch vụ cộng thêm");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Trạng thái");

            for (var i = 0; i <= TotalColumn; i++)
            {
                headerRow.GetCell(i).CellStyle = HeaderStyle;
            }

            //(Optional) freeze the header row so it is not scrolled
            //sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 3;
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
                row.CreateCell(rowIndex++).SetCellValue(item.PostalType + " " + Environment.NewLine + item.BatchCode);
                row.CreateCell(rowIndex++).SetCellValue(item.CreatedDate);
                row.CreateCell(rowIndex++).SetCellValue(item.ShopCode);
                row.CreateCell(rowIndex++).SetCellValue(item.CrmOrPaypostCode);

                if (req.OrderStatus == "L")
                {
                    row.CreateCell(rowIndex++).SetCellValue(item.CustomerName);
                    row.CreateCell(rowIndex++).SetCellValue(item.CustomerPhone);
                    row.CreateCell(rowIndex++).SetCellValue(item.CustomerFullAddress);
                }
                else
                {
                    row.CreateCell(rowIndex++).SetCellValue(item.SenderName);
                    row.CreateCell(rowIndex++).SetCellValue(item.SenderPhone);
                    row.CreateCell(rowIndex++).SetCellValue(item.SenderFullAddress);
                    row.CreateCell(rowIndex++).SetCellValue(item.ReceiverName);
                    row.CreateCell(rowIndex++).SetCellValue(item.ReceiverPhone);
                    row.CreateCell(rowIndex++).SetCellValue(item.ReceiverFullAddress);
                }
                row.CreateCell(rowIndex++).SetCellValue(item.Message);
                row.CreateCell(rowIndex++).SetCellValue(item.TotalCOD);
                row.CreateCell(rowIndex++).SetCellValue(item.TransportFee);
                row.CreateCell(rowIndex++).SetCellValue(item.PaymentFreightVat);
                row.CreateCell(rowIndex++).SetCellValue(item.ActualTransportWeight);
                row.CreateCell(rowIndex++).SetCellValue(item.AcceptancePosCode);
                row.CreateCell(rowIndex++).SetCellValue(item.DeliveryPosCode);
                row.CreateCell(rowIndex++).SetCellValue(item.TrackingItemInfoCreatedDate);
                row.CreateCell(rowIndex++).SetCellValue(item.TransportMainService.Name);
                row.CreateCell(rowIndex++).SetCellValue(item.TransportExtraService.Name);
                row.CreateCell(rowIndex++).SetCellValue(item.OrderStatus.Name);

                //row.CreateCell(rowIndex).CellStyle.SetFont(fontChar);

                for (var i = 0; i <= TotalColumn; i++)
                {
                    if (DefaultColumn.Contains(i.ToString()))
                    {
                        row.GetCell(i).CellStyle = BodyNumberStyle;
                    }
                    else
                    {
                        row.GetCell(i).CellStyle = BodyTextStyle;
                    }
                }

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
            sheet.AutoSizeColumn(21);
            sheet.AutoSizeColumn(22);
            sheet.AutoSizeColumn(23);

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            //application/vnd.ms-excel xls
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet xlsx
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachDonHang.xls");
        }

        #endregion

        #region Chi tiết đơn hàng lẻ

        public ActionResult OrderDetailView(int OrderID, int ShopID)
        {
            OrderDetailViewModel model = new OrderDetailViewModel();
            var res = OrderRepository.GetByID(new ZOrderGetByIDRequest()
            {
                ID = OrderID,
                ShopID = ShopID
            });
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

                //List<EMSSignaruteAndImageInformationModel> ListEMSSignaruteAndImageInformation = new List<EMSSignaruteAndImageInformationModel>();
                //if (item.ListEMSSignaruteAndImageInfo != null)
                //{
                //    foreach (var informationSignaruteAndImageEMS in item.ListEMSSignaruteAndImageInfo)
                //    {
                //        ListEMSSignaruteAndImageInformation.Add(new EMSSignaruteAndImageInformationModel()
                //        {
                //            Signature = informationSignaruteAndImageEMS.Signature,
                //            FileNames = informationSignaruteAndImageEMS.FileNames,
                //            FileNamesAuth = informationSignaruteAndImageEMS.FileNamesAuth
                //        });
                //    }
                //}


                //List<VNPSignaruteAndImageInformationModel> ListVNPSignaruteAndImageInformation = new List<VNPSignaruteAndImageInformationModel>();
                //if (item.ListVNPSignaruteAndImageInfo != null)
                //{
                //    foreach (var informationSignaruteAndImageVNP in item.ListVNPSignaruteAndImageInfo)
                //    {
                //        ListVNPSignaruteAndImageInformation.Add(new VNPSignaruteAndImageInformationModel()
                //        {
                //            ImagesSignature = informationSignaruteAndImageVNP.ImagesSignature,
                //            ImagesDelivery = informationSignaruteAndImageVNP.ImagesDelivery,
                //            Images = informationSignaruteAndImageVNP.Images,
                //            ImagesPlus = informationSignaruteAndImageVNP.ImagesPlus
                //        });
                //    }
                //}

                model = new OrderDetailViewModel()
                {
                    OrderID = item.ID,
                    OrderCode = item.OrderCode,
                    ShippingCode = item.ShippingCode,
                    OrderItems = ListOrderItem,
                    WareHouseName = item.WareHouseName,
                    Note = item.Note,
                    Message = item.Message,
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
                    TotalProductValue = item.TotalAmount.ToString("N0"),//Item.OrderItems.Sum(m => m.Price),
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
                    ListUpdateInformation = ListUpdateInformation,
                    //ListEMSSignaruteAndImageInfo = ListEMSSignaruteAndImageInformation,
                    //ListVNPSignaruteAndImageInfo = ListVNPSignaruteAndImageInformation,
                    PlaceOfCreation = item.PlaceOfCreation,
                    PostalType = item.PostalType,
                    ExpectedDeliveryDate = item.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentFullAddress = item.CollectionPostOfficeRecent.FullAddress
                };
            }
            return PartialView(model);
        }

        #endregion

        #region In đơn hàng

        public ActionResult OrderPrintView()
        {
            return PartialView();
        }

        public ActionResult OrderPrintType(OrderPrintViewModel req)
        {
            List<OrderPrintTypeViewModel> model = new List<OrderPrintTypeViewModel>();
            var ListOrderPrint = XCommon.ListOrderPrint(req);
            if (ListOrderPrint != null)
            {
                if (req.PrintType == "A4" || req.PrintType == "A61" || req.PrintType == "B1" || req.PrintType == "B2" || req.PrintType == "Label")
                {
                    model.Add(new OrderPrintTypeViewModel()
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
                                BatchCode = ListOrderPrint[j].BatchCode,
                                ShippingCode = ListOrderPrint[j].ShippingCode,
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
                            model.Add(new OrderPrintTypeViewModel()
                            {
                                Page = i,
                                ListOrderPrint = ListTemp
                            });
                        }
                    }
                }
            }
            ViewBag.Type = req.PrintType;
            return PartialView(model);
        }
        #endregion
    }
}