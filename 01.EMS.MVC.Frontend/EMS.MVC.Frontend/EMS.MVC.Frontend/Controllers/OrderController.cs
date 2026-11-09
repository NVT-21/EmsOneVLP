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
using Org.BouncyCastle.Ocsp;
using static EMS.Library.Security;

namespace EMS.MVC.Frontend.Controllers
{
    public class OrderController : BaseController
    {
        #region Danh sách đơn hàng
        [AuthorizeController(FunctionalTitle = "DANH_SACH_DON_HANG")]
        public ActionResult OrderView()
        {
            OrderViewModel model = new OrderViewModel()
            {
                FromDate = DateTime.Now.AddDays(-0).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                CrmOrPaypostCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                ListCrmOrPaypostCode = XCommon.SearchListCrmOrPaypostCode(new ZShopGetListCrmOrPaypostCodeRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListOrderStatus = XData.GetListOrderStatus(),
                ListPrintStatus = XData.GetListPrintStatus(),
                ListPostalType = XData.GetListPostalType().OrderByDescending(m => m.Value).ToList(),
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>()
            };

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
            }
            return View(model);
        }

        public ActionResult OrderPaginationView()
        {
            OrderViewModel model = new OrderViewModel()
            {
                FromDate = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                CrmOrPaypostCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                ListCrmOrPaypostCode = XCommon.SearchListCrmOrPaypostCode(new ZShopGetListCrmOrPaypostCodeRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListOrderStatus = XData.GetListOrderStatus(),
                ListPrintStatus = XData.GetListPrintStatus(),
                ListPostalType = XData.GetListPostalType().OrderByDescending(m => m.Value).ToList(),
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>()
            };

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if(ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
            }
            return View(model);
        }

        public ActionResult OrderPaginationExpandView()
        {
            OrderViewModel model = new OrderViewModel()
            {
                FromDate = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                CrmOrPaypostCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                ListCrmOrPaypostCode = XCommon.SearchListCrmOrPaypostCode(new ZShopGetListCrmOrPaypostCodeRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListOrderStatus = XData.GetListOrderStatus(),
                ListPrintStatus = XData.GetListPrintStatus(),
                ListPostalType = XData.GetListPostalType().OrderByDescending(m => m.Value).ToList(),
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>()
            };

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if(ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
            }
            return View(model);
        }

        public ActionResult SearchListOrderView(OrderViewModel req)
        {
            List<SearchListOrderViewModel> ListItem = new List<SearchListOrderViewModel>();
            var res = OrderRepository.SearchListOrder(new ZOrderFESearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                OrderStatus = req.OrderStatus,
                PrintStatus = req.PrintStatus,
                PostalType = req.PostalType,
                MainTransportService = req.MainTransportService,
                ExtraTransportService = req.ExtraTransportService,
                ListCustomerCode = req.CrmOrPaypostCode,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                Channel = "EMSONE"
            });
            List<ZOrderFESearchListResponse> ListData = new List<ZOrderFESearchListResponse>();
            if (res.Code == "00")
            {
                ListData = JsonConvert.DeserializeObject<List<ZOrderFESearchListResponse>>(res.Data);
                List<ZOrderFESearchListResponse> ListFilter = new List<ZOrderFESearchListResponse>();
                if (string.IsNullOrEmpty(req.OrderStatus))
                {
                    ListFilter = ListData.Where(m => m.OrderStatus != "C" && m.OrderStatus != "D" && m.OrderStatus != "L").ToList();
                }
                else
                {
                    ListFilter = ListData.Where(m => m.OrderStatus == req.OrderStatus).ToList();
                }
                foreach (var item in ListFilter)
                {
                    ListItem.Add(new SearchListOrderViewModel()
                    {
                        OrderID = item.OrderID,
                        EMSOneCode = item.EMSOneCode,
                        OrderCode = item.OrderCode,
                        BatchCode = item.BatchCode,
                        CustomerName = item.CustomerName,
                        CustomerPhone = item.CustomerPhone,
                        CustomerFullAddress = item.CustomerFullAddress,
                        SenderName = item.SenderName,
                        SenderPhone = item.SenderPhone,
                        //SenderProvinceID = item.SenderProvinceID,
                        //SenderDistrictID = item.SenderDistrictID,
                        //SenderWardID = item.SenderWardID,
                        //SenderAddress = item.SenderAddress,
                        SenderFullAddress = item.SenderFullAddress,
                        ReceiverName = item.ReceiverName,
                        ReceiverPhone = item.ReceiverPhone,
                        ReceiverProvinceID = item.ReceiverProvinceID,
                        //ReceiverProvinceName = item.ReceiverProvinceName,
                        ReceiverDistrictID = item.ReceiverDistrictID,
                        //ReceiverDistrictName = item.ReceiverDistrictName,
                        ReceiverWardID = item.ReceiverWardID,
                        //ReceiverWardName = item.ReceiverWardName,
                        ReceiverStreet = item.ReceiverAddress,
                        ReceiverFullAddress = item.ReceiverFullAddress,
                        Message = item.Message,
                        TotalCOD = item.TotalCOD,
                        //TotalOrderValue = item.TotalOrderValue,
                        //TotalProductValue = item.TotalProductValue,
                        OrderStatus = item.OrderStatus,
                        OrderStatusName = item.OrderStatusName,
                        CreatedDate = item.CreatedDate,
                        UpdatedName = item.UpdatedName,
                        UpdatedDate = item.UpdatedDate,
                        ReasonCancel = item.ReasonCancel,
                        ShippingCode = item.ShippingCode,
                        //InventoryID = item.InventoryID,
                        //Transport = item.Transport,
                        TransportOrderName = item.TransportOrderName,
                        TransportFee = item.TransportFee,
                        //TrackingItemInfoCreatedDate = item.TrackingItemInfoCreatedDate,
                        //OrderHistoryDescription = item.OrderHistoryDescription,
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
                var ListOrderStatus = XData.GetListOrderStatus();
                List<object> OrderStatusNumber = new List<object>();
                foreach (var item in ListOrderStatus)
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


        public ActionResult SearchListOrderPaginationView(OrderViewModel req)
        {
            List<SearchListOrderViewModel> ListItem = new List<SearchListOrderViewModel>();
            var res = OrderRepository.SearchListOrderPagination(new ZOrderFEPaginationSearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                OrderStatus = req.OrderStatus,
                PrintStatus = req.PrintStatus,
                PostalType = req.PostalType,
                PageNumber = req.PageNumber,
                PageSize = req.PageLimit,
                MainTransportService = req.MainTransportService,
                ExtraTransportService = req.ExtraTransportService,
                ListCustomerCode = req.CrmOrPaypostCode,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                Channel = "EMSONE"
            });
            List<ZOrderFESearchListResponse> ListData = new List<ZOrderFESearchListResponse>();
            ZOrderFEPaginationSearchListResponse ListDataPagination = new ZOrderFEPaginationSearchListResponse();
            if(res.Code == "00")
            {
                //ListData = JsonConvert.DeserializeObject<List<ZOrderFESearchListResponse>>(res.Data);
                ListDataPagination = JsonConvert.DeserializeObject<ZOrderFEPaginationSearchListResponse>(res.Data);
                List<ZOrderFESearchListResponse> ListFilter = new List<ZOrderFESearchListResponse>();
                //if(string.IsNullOrEmpty(req.OrderStatus))
                //{
                //    ListFilter = ListData.Where(m => m.OrderStatus != "C" && m.OrderStatus != "D" && m.OrderStatus != "L").ToList();
                //}
                //else
                //{
                //    ListFilter = ListData.Where(m => m.OrderStatus == req.OrderStatus).ToList();
                //}
                foreach(var item in ListDataPagination.ListItem)
                {
                    ListItem.Add(new SearchListOrderViewModel()
                    {
                        OrderID = item.OrderID,
                        EMSOneCode = item.EMSOneCode,
                        OrderCode = item.OrderCode,
                        BatchCode = item.BatchCode,
                        CustomerName = item.CustomerName,
                        CustomerPhone = item.CustomerPhone,
                        CustomerFullAddress = item.CustomerFullAddress,
                        SenderName = item.SenderName,
                        SenderPhone = item.SenderPhone,
                        //SenderProvinceID = item.SenderProvinceID,
                        //SenderDistrictID = item.SenderDistrictID,
                        //SenderWardID = item.SenderWardID,
                        //SenderAddress = item.SenderAddress,
                        SenderFullAddress = item.SenderFullAddress,
                        ReceiverName = item.ReceiverName,
                        ReceiverPhone = item.ReceiverPhone,
                        ReceiverProvinceID = item.ReceiverProvinceID,
                        //ReceiverProvinceName = item.ReceiverProvinceName,
                        ReceiverDistrictID = item.ReceiverDistrictID,
                        //ReceiverDistrictName = item.ReceiverDistrictName,
                        ReceiverWardID = item.ReceiverWardID,
                        //ReceiverWardName = item.ReceiverWardName,
                        ReceiverStreet = item.ReceiverAddress,
                        ReceiverFullAddress = item.ReceiverFullAddress,
                        Message = item.Message,
                        TotalCOD = item.TotalCOD,
                        //TotalOrderValue = item.TotalOrderValue,
                        //TotalProductValue = item.TotalProductValue,
                        OrderStatus = item.OrderStatus,
                        OrderStatusName = item.OrderStatusName,
                        CreatedDate = item.CreatedDate,
                        UpdatedName = item.UpdatedName,
                        UpdatedDate = item.UpdatedDate,
                        ReasonCancel = item.ReasonCancel,
                        ShippingCode = item.ShippingCode,
                        //InventoryID = item.InventoryID,
                        //Transport = item.Transport,
                        TransportOrderName = item.TransportOrderName,
                        TransportFee = item.TransportFee,
                        //TrackingItemInfoCreatedDate = item.TrackingItemInfoCreatedDate,
                        //OrderHistoryDescription = item.OrderHistoryDescription,
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
            if(req.ReloadStatusNumber == "Y")
            {
                var ListOrderStatus = XData.GetListOrderStatus();
                List<object> OrderStatusNumber = new List<object>();
                
                foreach(var item in ListOrderStatus)
                {
                    var Count = ListDataPagination.ListStatusData?
                                                   .FirstOrDefault(m => m.StatusCode == item.Value)?
                                                   .OrderCount ?? 0;
                    OrderStatusNumber.Add(new
                    {
                        Status = item.Value,
                        Count = Count
                    });
                }
                ViewBag.OrderStatusNumber = OrderStatusNumber;
            }
            // Tổng đếm để hiển thị pager
            ViewBag.TotalRows = ListDataPagination.Total;
            ViewBag.PageIndex = req.PageNumber <= 0 ? 1 : req.PageNumber;
            ViewBag.PageSize = req.PageLimit <= 0 ? 20 : req.PageLimit;

            ViewBag.ReloadStatusNumber = req.ReloadStatusNumber;
            ViewBag.SearchOrderStatus = req.OrderStatus;
            ViewBag.CheckCustomerCode = req.CrmOrPaypostCode == XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode ? 1 : 2;
            ViewBag.ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID;
            return PartialView(ListItem);
        }

        public ActionResult SearchListOrderPaginationExpandView(OrderViewModel req)
        {
            List<SearchListOrderViewModel> ListItem = new List<SearchListOrderViewModel>();
            var res = OrderRepository.SearchListOrderExpandPagination(new ZOrderFEPaginationSearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                OrderStatus = req.OrderStatus,
                PrintStatus = req.PrintStatus,
                PostalType = req.PostalType,
                PageNumber = req.PageNumber,
                PageSize = req.PageLimit,
                MainTransportService = req.MainTransportService,
                ExtraTransportService = req.ExtraTransportService,
                ListCustomerCode = req.CrmOrPaypostCode,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                EmployeeID = XHelper.Session.EmployeeProfile.ID,
                Channel = "EMSONE"
            });
            List<ZOrderFESearchListResponse> ListData = new List<ZOrderFESearchListResponse>();
            ZOrderFEPaginationSearchListResponse ListDataPagination = new ZOrderFEPaginationSearchListResponse();
            if(res.Code == "00")
            {
                
                ListDataPagination = JsonConvert.DeserializeObject<ZOrderFEPaginationSearchListResponse>(res.Data);
                List<ZOrderFESearchListResponse> ListFilter = new List<ZOrderFESearchListResponse>();
                
                foreach(var item in ListDataPagination.ListItem)
                {
                    ListItem.Add(new SearchListOrderViewModel()
                    {
                        OrderID = item.OrderID,
                        EMSOneCode = item.EMSOneCode,
                        OrderCode = item.OrderCode,
                        BatchCode = item.BatchCode,
                        CustomerName = item.CustomerName,
                        CustomerPhone = item.CustomerPhone,
                        CustomerFullAddress = item.CustomerFullAddress,
                        SenderName = item.SenderName,
                        SenderPhone = item.SenderPhone,
                        //SenderProvinceID = item.SenderProvinceID,
                        //SenderDistrictID = item.SenderDistrictID,
                        //SenderWardID = item.SenderWardID,
                        //SenderAddress = item.SenderAddress,
                        SenderFullAddress = item.SenderFullAddress,
                        ReceiverName = item.ReceiverName,
                        ReceiverPhone = item.ReceiverPhone,
                        ReceiverProvinceID = item.ReceiverProvinceID,
                        //ReceiverProvinceName = item.ReceiverProvinceName,
                        ReceiverDistrictID = item.ReceiverDistrictID,
                        //ReceiverDistrictName = item.ReceiverDistrictName,
                        ReceiverWardID = item.ReceiverWardID,
                        //ReceiverWardName = item.ReceiverWardName,
                        ReceiverStreet = item.ReceiverAddress,
                        ReceiverFullAddress = item.ReceiverFullAddress,
                        Message = item.Message,
                        TotalCOD = item.TotalCOD,
                        //TotalOrderValue = item.TotalOrderValue,
                        //TotalProductValue = item.TotalProductValue,
                        OrderStatus = item.OrderStatus,
                        OrderStatusName = item.OrderStatusName,
                        CreatedDate = item.CreatedDate,
                        UpdatedName = item.UpdatedName,
                        UpdatedDate = item.UpdatedDate,
                        ReasonCancel = item.ReasonCancel,
                        ShippingCode = item.ShippingCode,
                        //InventoryID = item.InventoryID,
                        //Transport = item.Transport,
                        TransportOrderName = item.TransportOrderName,
                        TransportFee = item.TransportFee,
                        //TrackingItemInfoCreatedDate = item.TrackingItemInfoCreatedDate,
                        //OrderHistoryDescription = item.OrderHistoryDescription,
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
            if(req.ReloadStatusNumber == "Y")
            {
                var ListOrderStatus = XData.GetListOrderStatus();
                List<object> OrderStatusNumber = new List<object>();

                foreach(var item in ListOrderStatus)
                {
                    var Count = ListDataPagination.ListStatusData?
                                                   .FirstOrDefault(m => m.StatusCode == item.Value)?
                                                   .OrderCount ?? 0;
                    OrderStatusNumber.Add(new
                    {
                        Status = item.Value,
                        Count = Count
                    });
                }
                ViewBag.OrderStatusNumber = OrderStatusNumber;
            }
            // Tổng đếm để hiển thị pager
            ViewBag.TotalRows = ListDataPagination.Total;
            ViewBag.PageIndex = req.PageNumber <= 0 ? 1 : req.PageNumber;
            ViewBag.PageSize = req.PageLimit <= 0 ? 20 : req.PageLimit;

            ViewBag.ReloadStatusNumber = req.ReloadStatusNumber;
            ViewBag.SearchOrderStatus = req.OrderStatus;
            ViewBag.CheckCustomerCode = req.CrmOrPaypostCode == XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode ? 1 : 2;
            ViewBag.ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID;
            return PartialView(ListItem);
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

            //var center = workbook.CreateCellStyle();
            //center.Alignment = HorizontalAlignment.Center;
            //center.SetFont(fontChar);

            //Create title 
            var TitleRow = sheet.CreateRow(0);
            TitleRow.CreateCell(0).SetCellValue("DANH SÁCH ĐƠN HÀNG");
            TitleRow.GetCell(0).CellStyle = TitleStyle;

            //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 12));
            //sheet.CreateRow(0).GetCell(0).CellStyle = center;
            var res = OrderRepository.SearchListOrder(new ZOrderFESearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                OrderStatus = req.OrderStatus,
                PrintStatus = req.PrintStatus,
                PostalType = req.PostalType,
                MainTransportService = req.MainTransportService,
                ExtraTransportService = req.ExtraTransportService,
                ListCustomerCode = req.CrmOrPaypostCode,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                EmployeeID = XHelper.Session.EmployeeProfile.ID,
                Channel = "EMSONE"
            });
            List<ZOrderFESearchListResponse> ListItem = new List<ZOrderFESearchListResponse>();

            if (res.Code == "00")
            {
                var ListData = JsonConvert.DeserializeObject<List<ZOrderFESearchListResponse>>(res.Data);
                if (string.IsNullOrEmpty(req.OrderStatus))
                {
                    ListItem = ListData.Where(m => m.OrderStatus != "C" && m.OrderStatus != "D" && m.OrderStatus != "L").ToList();
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
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Thời gian chấp nhận");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Tên đơn hàng");

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
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Giá trị đơn hàng");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Tiền COD");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Cước tạm tính");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Cước thực tế");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Dịch vụ chính");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Dịch vụ cộng thêm");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Người xử lý");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Trạng thái");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Thời gian trạng thái");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Thông tin tạo đơn/ hủy đơn");


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

                row.CreateCell(rowIndex++).SetCellValue(item.CreatedOrderDate);
                row.CreateCell(rowIndex++).SetCellValue(item.AcceptOrderDate);
                row.CreateCell(rowIndex++).SetCellValue(item.TransportOrderName);

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
                row.CreateCell(rowIndex++).SetCellValue(item.TransportOrderValue);
                row.CreateCell(rowIndex).CellStyle.DataFormat = DataFormat.GetFormat("#,##0");
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
                row.CreateCell(rowIndex++).SetCellValue(item.UpdatedDate);
                if (item.OrderStatus == "C")
                {
                    row.CreateCell(rowIndex++).SetCellValue("Người hủy: " + item.UpdatedName + Environment.NewLine + "Thời gian hủy: " + item.UpdatedDate + Environment.NewLine + "Lý do hủy: " + item.ReasonCancel);
                }
                else
                {
                    //row.CreateCell(rowIndex++).SetCellValue("");
                    row.CreateCell(rowIndex++).SetCellValue("Người tạo: " + item.UpdatedName + Environment.NewLine + "Thời gian tạo: " + item.UpdatedDate);
                }

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
            sheet.AutoSizeColumn(21);
            sheet.AutoSizeColumn(22);
            sheet.AutoSizeColumn(23);
            sheet.AutoSizeColumn(24);

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

        #region API trả trạng thái phát kèm thông tin chữ ký , hình ảnh báo phát EMS hoặc VNP
        public ActionResult InformationSignaruteAndImageOrderView(SearchListOrderViewModel req)
        {
            List<EMSSignaruteAndImageInformationModel> ListEMS = new List<EMSSignaruteAndImageInformationModel>();
            List<VNPSignaruteAndImageInformationModel> ListVNP = new List<VNPSignaruteAndImageInformationModel>();
            var resEMS = EMSRepository.EMSSignaruteAndImageInformation(new ZEMSSignaruteAndImageInformationRequest() { LadingCode = req.ShippingCode });
            if (resEMS.Code == "00")
            {
                var listEMS = JsonConvert.DeserializeObject<List<ZEMSSignaruteAndImageInformationResponse>>(resEMS.Data);
                foreach (var item in listEMS)
                {
                    ListEMS.Add(new EMSSignaruteAndImageInformationModel()
                    {
                        Signature = item.Signature,
                        FileNames = item.FileNames,
                        FileNamesAuth = item.FileNamesAuth
                    });
                }
            }
            else
            {
                var resVNP = VNPRepository.VNPSignaruteAndImageInformation(new ZVNPSignaruteAndImageInformationRequest() { itemCode = req.ShippingCode });
                if (resVNP.Code == "00")
                {
                    var listVNP = JsonConvert.DeserializeObject<List<ZVNPSignaruteAndImageInformationResponse>>(resVNP.Data);
                    foreach (var item in listVNP)
                    {
                        ListVNP.Add(new VNPSignaruteAndImageInformationModel()
                        {
                            ImagesSignature = item.ImagesSignature,
                            ImagesDelivery = item.ImagesDelivery,
                            Images = item.Images,
                            ImagesPlus = item.ImagesPlus
                        });
                    }
                }
            }

            InformationSignaruteAndImageOrderViewModel model = new InformationSignaruteAndImageOrderViewModel()
            {
                Title = req.SearchKey == "NORMAL" ? "Hình ảnh báo phát" : "Hình ảnh xác thực",
                ShippingCode = req.ShippingCode,
                SearchKey = req.SearchKey,
                ListEMSSignaruteAndImageInformation = ListEMS,
                ListVNPSignaruteAndImageInformation = ListVNP
            };
            return PartialView(model);
        }
        #endregion

        #region Sửa địa chỉ người nhận
        public ActionResult EditOrderReceiverAddressView(SearchListOrderViewModel req)
        {
            EditOrderReceiverAddressViewModel model = new EditOrderReceiverAddressViewModel()
            {
                OrderID = req.OrderID,
                OrderCode = req.OrderCode,
                BatchCode = req.BatchCode,
                ProvinceID = req.ReceiverProvinceID,
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                DistrictID = req.ReceiverDistrictID,
                ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = req.ReceiverProvinceID }),
                WardID = req.ReceiverWardID,
                ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = req.ReceiverDistrictID }),
                Street = req.ReceiverStreet
            };
            return PartialView(model);
        }

        public JsonResult JEditOrderReceiverAddress(EditOrderReceiverAddressViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = OrderRepository.EditOrderReceiverAddress(new ZOrderEditReceiverAddressRequest()
                {
                    OrderID = req.OrderID,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JAddOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm đơn hàng khách hàng lẻ
        public ActionResult WalkInOrderView()
        {
            var ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());

            WalkInOrderViewModel model = new WalkInOrderViewModel()
            {
                Buyer = new CustomerModel(),
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>(),
                Weight = 100,
                Quantity = 1,
                Receiver = new CustomerModel()
                {
                    ListProvince = ListProvince,
                    ListDistrict = new List<DistrictModel>(),
                    ListWard = new List<WardModel>()
                },
                Sender = new CustomerModel() 
                {
                    ListProvince = ListProvince,
                    ListDistrict = new List<DistrictModel>(),
                    ListWard = new List<WardModel>()
                },
            };

            // 13084 là Shop ID - Khách hàng lẻ lấy trong DB -- Customer Code = ONE-000000000
            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = 13084 });
            if(ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                if(model.ListMainTransportService.Count > 0)
                {
                    model.MainTransportServiceID = model.ListMainTransportService.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListMainTransportService.Where(m => m.IsDefault == "Y").First().ID : 0;
                }
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
                if(model.ListExtraTransportService.Count > 0)
                {
                    model.ExtraTransportServiceID = model.ListExtraTransportService.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListExtraTransportService.Where(m => m.IsDefault == "Y").First().ID.ToString() : "0";
                }
            }
            return PartialView(model);
        }
        public JsonResult JAddWalkInOrder(AddOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var OrderAddRequest = JAddWalkInOrderDataConvertRequest(req);
                res = OrderRepository.AddReturnOrderAndSendTransport(OrderAddRequest);
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JAddWalkInOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private ZPartnerOrderAddRequest JAddWalkInOrderDataConvertRequest(AddOrderViewModel req)
        {
            ZPartnerOrderAddRequest OrderAddDataRequest = null;
            if(req.CollectionType.ID == 1)
            {
                req.Note = "Thu tiền người gửi - " + req.Note;
            }
            

            OrderAddDataRequest = new ZPartnerOrderAddRequest
            {
                CrmOrPaypostCode = "ONE-000000000",
                OrderCode = "",
                OrderName = req.OrderName,
                OrderValue = req.OrderValue,
                OrderQuantity = req.Quantity,
                IsTransport = "Y",
                IsSendTransport = "Y",
                Note = req.Note,
                Message = req.Message,
                BuyerInfo = new ZPartnerOrderAddBuyerInfoRequest
                {
                    FullName = req.Receiver.FullName,
                    MobileNumber = req.Receiver.MobileNumber,
                    ProvinceID = req.Receiver.Province.ID,
                    WardID = req.Receiver.Ward.ID,
                    Street = req.Receiver.Street
                },
                SenderInfo = new ZPartnerOrderAddSenderInfoRequest
                {
                    FullName = req.Sender.FullName,
                    MobileNumber = req.Sender.MobileNumber,
                    ProvinceID = req.Sender.Province.ID,
                    WardID = req.Sender.Ward.ID,
                    Street = req.Sender.Street
                },
                ReceiverInfo = new ZPartnerOrderAddReceiverInfoRequest
                {
                    FullName = req.Receiver.FullName,
                    MobileNumber = req.Receiver.MobileNumber,
                    ProvinceID = req.Receiver.Province.ID,
                    WardID = req.Receiver.Ward.ID,
                    Street = req.Receiver.Street,
                },
                TransportInfo = new ZPartnerOrderAddTransportInfoRequest
                {
                    TransportMainServiceID = req.MainTransportServiceID,
                    TransportExtraServiceID = req.ExtraTransportServiceID,
                    TransportWeight = req.Weight,
                    TransportSizeHeight = req.Height,
                    TransportSizeWidth = req.Width,
                    TransportSizeLength = req.Length,
                    CollectionType = req.CollectionType.ID,
                    TotalCOD = req.TotalCOD,
                    TotalPHU = req.TotalPHU,
                    TransportPayer = 1
                }
            };
            return OrderAddDataRequest;
        }

        #endregion
        public ActionResult ReturnOrderHMView(string OrderCodeEncrypt)
        {
            try
            {
                WalkInOrderViewModel model = new WalkInOrderViewModel();
                var OrderDetail = new ZOrderGetByIDResponse();
                if(OrderCodeEncrypt == null)
                {
                    return RedirectToAction("ErrorView", "Handle");
                }

                // OrderCode lúc này đang là encryptedToken từ H&M
                string encryptedToken = OrderCodeEncrypt;

                string sharedSecretKey = XHelper.SecretReturnHM;

                XHmReturnPayload payload = Security.DecryptTokenHM(
                    encryptedToken,
                    sharedSecretKey
                );

                if(payload == null || string.IsNullOrWhiteSpace(payload.OrderId))
                {
                    return RedirectToAction("ErrorView", "Handle");
                }

                string orderCode = payload.OrderId;

                var ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());
                var resOrderDetail = OrderRepository.GetByOrderHM(new ZOrderGetByOrderCodeRequest
                {
                    OrderCode = orderCode,
                    ShopID = 13331
                });
                if(resOrderDetail.Code == "00")
                {
                    OrderDetail = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderDetail.Data);
                    model = new WalkInOrderViewModel()
                    {
                        OrderCode = orderCode,
                        Buyer = new CustomerModel(),
                        Weight = 100,
                        Quantity = 1,
                        Sender = new CustomerModel()
                        {
                            ListProvince = ListProvince,
                            ListDistrict = new List<DistrictModel>(),
                            ListWard = XCommon.SearchListWardByProvinceID(new ZWardGetByDistrictIDRequest() { DistrictID = OrderDetail.ShipmentDetails.ProvinceID }),
                            Province = new ProvinceModel
                            {
                                ID = OrderDetail.ShipmentDetails.ProvinceID
                            },
                            District = new DistrictModel
                            {
                                ID = OrderDetail.ShipmentDetails.DistrictID
                            },
                            Ward = new WardModel
                            {
                                ID = OrderDetail.ShipmentDetails.WardID
                            },
                            Street = OrderDetail.ShipmentDetails.Address,
                            FullName = OrderDetail.ShipmentDetails.ReceiverName,
                            MobileNumber = OrderDetail.ShipmentDetails.MobileNumber

                        },
                        ReturnCount = OrderDetail.ReturnCount,
                        ReturnShippingCodes = OrderDetail.ReturnShippingCodes,
                        CollectionType = new BaseModel { ID = 1 }
                    };
                }
                else
                {
                    model = null;
                }

                return PartialView(model);
            }
            catch(UnauthorizedAccessException ex)
            {
                /*
                 * Các case sẽ vào đây:
                 * - Token hết hạn
                 * - Token bị sửa
                 * - Sai secret key
                 * - AuthTag không đúng
                 */
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorView", "Handle");
            }
            catch(FormatException)
            {
                TempData["ErrorMessage"] = "Token không đúng định dạng Base64Url.";
                return RedirectToAction("ErrorView", "Handle");
            }
            catch(ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ErrorView", "Handle");
            }
            catch(Exception)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình xử lý.";
                return RedirectToAction("ErrorView", "Handle");
            }

        }

        public JsonResult JAddReturnOrderHM(AddOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var OrderAddRequest = JAddReturnOrderHMDataConvertRequest(req);
                res = OrderRepository.AddReturnOrderAndSendTransport(OrderAddRequest);
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JAddWalkInOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private ZPartnerOrderAddRequest JAddReturnOrderHMDataConvertRequest(AddOrderViewModel req)
        {
            ZPartnerOrderAddRequest OrderAddDataRequest = null;
           
            OrderAddDataRequest = new ZPartnerOrderAddRequest
            {
                CrmOrPaypostCode = "T006882706",
                OrderCode = req.OrderCode,
                OrderName = "",
                OrderValue = req.OrderValue,
                OrderQuantity = 1,
                IsTransport = "Y",
                IsSendTransport = "Y",
                Note = req.Note,
                Message = req.Message,
                BuyerInfo = new ZPartnerOrderAddBuyerInfoRequest
                {
                    FullName = "HM Return Services Point",
                    MobileNumber = "0908369481",
                    ProvinceID = 70,
                    WardID = 76538,
                    Street = "20 Nguyễn Đức Thuận Phường Tân Bình,Thành Phố Hồ Chí Minh",
                },
                SenderInfo = new ZPartnerOrderAddSenderInfoRequest
                {
                    FullName = req.Sender.FullName,
                    MobileNumber = req.Sender.MobileNumber,
                    ProvinceID = req.Sender.Province.ID,
                    WardID = req.Sender.Ward.ID,
                    Street = req.Sender.Street
                },
                ReceiverInfo = new ZPartnerOrderAddReceiverInfoRequest
                {
                    FullName = "HM Return Services Point",
                    MobileNumber = "0908369481",
                    ProvinceID = 70,
                    WardID = 76538,
                    Street = "20 Nguyễn Đức Thuận Phường Tân Bình,Thành Phố Hồ Chí Minh",
                },
                TransportInfo = new ZPartnerOrderAddTransportInfoRequest
                {
                    TransportMainServiceID = 141,
                    TransportExtraServiceID = "110",
                    TransportWeight = 100,
                    TransportSizeHeight = 0,
                    TransportSizeWidth = 0,
                    TransportSizeLength = 0,
                    CollectionType = req.CollectionType.ID,
                    TotalCOD = 0,
                    TotalPHU = 0,
                    TransportPayer = 1
                }
            };
            return OrderAddDataRequest;
        }
        #region

        #endregion

        #region Thêm đơn hàng
        [AuthorizeController(FunctionalTitle = "THEM_DON_HANG")]
        public ActionResult AddOrderView()
        {
            AddOrderViewModel model = new AddOrderViewModel()
            {
                Buyer = new CustomerModel(),
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>()
            };

            if (TempData.ContainsKey("Customer_MobileNumber"))
            {
                model.Buyer.MobileNumber = TempData["Customer_MobileNumber"].ToString();
                model.Buyer.FullName = TempData["Customer_FullName"].ToString();
            }

            model.ListChannel = XData.GetListChannel();
            model.ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            model.WareHouseID = model.ListWareHouse.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListWareHouse.Where(m => m.IsDefault == "Y").First().ID : 0;
            //model.ListMainTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 1, IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            //model.MainTransportServiceID = model.ListMainTransportService.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListMainTransportService.Where(m => m.IsDefault == "Y").First().ID : 0;
            //model.ListExtraTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 2, IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            //model.ExtraTransportServiceID = model.ListExtraTransportService.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListExtraTransportService.Where(m => m.IsDefault == "Y").First().ID.ToString() : "0";
            model.CollectionType = new XBaseModel() { ID = XHelper.Session.EmployeeProfile.ShopInformation.CollectionType };
            model.Weight = 100;
            model.Quantity = 1;
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
            return View(model);
        }

        public JsonResult JAddOrder(AddOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var OrderAddRequest = JAddOrderDataConvertRequest(req);
                if (req.AddType == 1 || req.AddType == 3)
                {
                    res = OrderRepository.AddOrder(OrderAddRequest);
                }
                else if (req.AddType == 2)
                {
                    res = OrderRepository.AddOrderAndSendTransport(OrderAddRequest);
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Loại đơn hàng không tồn tại.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JAddOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ZOrderAddRequest JAddOrderDataConvertRequest(AddOrderViewModel req)
        {
            ZOrderAddRequest OrderAddDataRequest = null;
            if (req.IsTransport == "Y")
            {
                OrderAddDataRequest = new ZOrderAddRequest()
                {
                    CustomerID = req.Receiver.ID,
                    ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                    {
                        ReceiverName = req.Receiver.FullName,
                        MobileNumber = req.Receiver.MobileNumber,
                        ProvinceID = req.Receiver.Province.ID,
                        ProvinceName = req.Receiver.Province.Name,
                        WardID = req.Receiver.Ward.ID,
                        WardName = req.Receiver.Ward.Name,
                        Address = req.Receiver.Street,
                        FullAddress = req.Receiver.FullAddress,
                        IsReceiverNewAddress = req.Receiver.IsNewAddress,
                        PickUpAddressID = req.Sender.PickUpAddressID,
                        PickupFullName = req.Sender.FullName,
                        PickupMobileNumber = req.Sender.MobileNumber,
                        PickupProvinceID = req.Sender.Province.ID,
                        PickupWardID = req.Sender.Ward.ID,
                        PickupStreet = req.Sender.Street,
                        PickupFullAddress = req.Sender.FullAddress,
                    },
                    isUpdatedCustomer = req.IsUpdateReceiver,
                    WareHouseID = req.WareHouseID,
                    OrderedProducts = req.ListProduct,
                    //ListInventoryDetail = req.ListInventoryOutput,
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
                    ExpectedDeliveryDate = req.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentID = req.CollectionPostOfficeRecentID,
                };
            }
            else
            {
                OrderAddDataRequest = new ZOrderAddRequest()
                {
                    CustomerID = req.Receiver.ID,
                    WareHouseID = req.WareHouseID,
                    OrderedProducts = req.ListProduct,
                    //ListInventoryDetail = req.ListInventoryOutput,
                    Transport = req.IsTransport,
                    OrderCode = req.OrderCode,
                    TransportOrderName = req.OrderName,
                    TransportOrderValue = req.OrderValue,
                    TransportQuantity = req.Quantity,
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
                    ExpectedDeliveryDate = req.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentID = req.CollectionPostOfficeRecentID,
                };
            }
            return OrderAddDataRequest;
        }
        #endregion

        #region Thêm đơn hàng nhanh
        [AuthorizeController(FunctionalTitle = "THEM_DON_HANG_NHANH")]
        public ActionResult FastOrderView()
        {
            var ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());

            FastOrderViewModel model = new FastOrderViewModel()
            {
                Buyer = new CustomerModel(),
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>(),
                ListPickUpAddress = new List<PickUpAddressModel>(),
                //ListChannel = XData.GetListChannel(),
                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                TypeShop = XHelper.Session.EmployeeProfile.ShopInformation.TypeShop,
                CollectionType = new BaseModel() { ID = XHelper.Session.EmployeeProfile.ShopInformation.CollectionType },
                Weight = 100,
                Quantity = 1,
                Receiver = new CustomerModel()
                {
                    ListProvince = ListProvince,
                    ListDistrict = new List<DistrictModel>(),
                    ListWard = new List<WardModel>()
                },
                Sender = new CustomerModel(),
            };

            model.WareHouseID = model.ListWareHouse.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListWareHouse.Where(m => m.IsDefault == "Y").First().ID : 0;
            model.ListPickUpAddress = XCommon.SearchListPickUpAddress(new ZPickUpAddressSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            var PickUpAddressInfo = model.ListPickUpAddress.Where(m => m.IsDefault == "Y").First();

            if (model.TypeShop == 1)
            {
                model.PickUpAddressID = PickUpAddressInfo.PickUpAddressID;
            }
            else
            {
                model.PickUpAddressID = PickUpAddressInfo.PickUpAddressID;
            }

            model.Sender = new CustomerModel()
            {
                FullName = PickUpAddressInfo.FullName,
                MobileNumber = PickUpAddressInfo.MobileNumber,
                Province = new ProvinceModel() { ID = PickUpAddressInfo.Province.ID },
                Ward = new WardModel() { ID = PickUpAddressInfo.Ward.ID },
                Street = PickUpAddressInfo.Street,
                FullAddress = PickUpAddressInfo.FullAddress,
                ListProvince = ListProvince,
                ListWard = XCommon.SearchListWardByProvinceID(new ZWardGetByDistrictIDRequest() { DistrictID = PickUpAddressInfo.Province.ID, Type = 1 })
            };

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

            return View(model);
        }
        #endregion

        #region Sửa đơn hàng
        [AuthorizeController(FunctionalTitle = "SUA_DON_HANG")]
        public ActionResult EditOrderView(SearchListOrderViewModel req)
        {
            EditOrderViewModel model = null;
            var OrderID = Security.DecodeBase64(req.SearchKey);
            var res = OrderRepository.GetByID(new ZOrderGetByIDRequest() { ID = Convert.ToInt32(OrderID), ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                List<SearchListProductViewModel> ListProduct = new List<SearchListProductViewModel>();
                var item = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(res.Data);
                if (item.OrderItems.Count > 0)
                {
                    foreach (var itemOrder in item.OrderItems)
                    {
                        ListProduct.Add(new SearchListProductViewModel()
                        {
                            ProductID = itemOrder.ProductID,
                            ProductName = itemOrder.ProductName,
                            VariantID = itemOrder.VariantID,
                            VariantName = itemOrder.VariantName,
                            WareHouseQuantity = itemOrder.WareHouseQuantity,
                            NumberOfProduct = itemOrder.NumberOfProduct,
                            PriceProduct = itemOrder.Price,
                            TotalPrice = itemOrder.NumberOfProduct * itemOrder.Price,
                            Weight = itemOrder.Weight
                        });
                    }
                }

                model = new EditOrderViewModel()
                {
                    Buyer = new CustomerModel()
                    {
                        MobileNumber = item.Customer.MobileNumber,
                        FullName = item.Customer.FullName,
                    },
                    Sender = new CustomerModel()
                    {
                        PickUpAddressID = item.ShipmentDetails.PickUpAddressID,
                        FullName = item.ShipmentDetails.PickupFullName,
                        MobileNumber = item.ShipmentDetails.PickupMobileNumber,
                        Province = new ProvinceModel() { ID = item.ShipmentDetails.PickupProvinceID },
                        Ward = new WardModel() { ID = item.ShipmentDetails.PickupWardID },
                        Street = item.ShipmentDetails.PickupStreet
                    },
                    Receiver = new CustomerModel()
                    {
                        FullName = item.ShipmentDetails.ReceiverName,
                        MobileNumber = item.ShipmentDetails.MobileNumber,
                        Province = new ProvinceModel() { ID = item.ShipmentDetails.ProvinceID },
                        Ward = new WardModel() { ID = item.ShipmentDetails.WardID },
                        Street = item.ShipmentDetails.Address
                    },
                    WareHouseID = item.WareHouseID,
                    ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                    ListProduct = ListProduct,
                    IsTransport = item.Transport,
                    OrderCode = item.OrderCode,
                    OrderName = item.TransportOrderName,
                    OrderValue = item.TransportOrderValue,
                    Quantity = item.TransportQuantity,
                    Length = item.TransportSizeLength,
                    Width = item.TransportSizeWidth,
                    Height = item.TransportSizeHeight,
                    Weight = item.TransportWeight,
                    CollectionType = new BaseModel()
                    {
                        ID = item.CollectionType.ID,
                        Code = item.CollectionType.Code,
                        Name = item.CollectionType.Name
                    },
                    TransportPayer = item.TransportPayer,
                    MainTransportServiceID = item.TransportMainService,
                    ListMainTransportService = new List<TransportServiceModel>(),
                    ExtraTransportServiceID = item.TransportExtraService,
                    ListExtraTransportService = new List<TransportServiceModel>(),
                    TotalCOD = item.TotalCOD,
                    TotalPHU = item.TotalPHU,
                    Note = item.Note,
                    Message = item.Message,
                    Channel = item.Channel,
                    ListChannel = XData.GetListChannel(),
                    TotalMoney = item.TotalAmount,
                    TotalDiscount = item.TotalDiscount,
                    TotalOrderValue = item.TotalOrderValue,
                    OrderStatus = item.Status,
                    EMSOneCode = item.EMSOneCode,
                    CreatedDate = item.CreatedDate,
                    ExpectedDeliveryDate = item.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentID = item.CollectionPostOfficeRecent.ID,
                    //DeliveryNoteID = item.DeliveryNoteID,
                    //DeliveryNoteCode = item.DeliveryNoteCode
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

        public JsonResult JEditOrder(EditOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var OrderAddRequest = JEditOrderDataConvertRequest(req);
                if (req.AddType == 1 || req.AddType == 3)
                {
                    res = OrderRepository.EditOrder(OrderAddRequest);
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Loại đơn hàng không tồn tại.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JAddOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ZOrderEditRequest JEditOrderDataConvertRequest(EditOrderViewModel req)
        {
            List<ZOrderEditItemRequest> ListProductConvert = new List<ZOrderEditItemRequest>();
            if (req.ListProduct != null && req.ListProduct.Count > 0)
            {
                foreach (var item in req.ListProduct)
                {
                    ListProductConvert.Add(new ZOrderEditItemRequest()
                    {
                        ProductID = item.ProductID,
                        VariantID = item.VariantID,
                        NumberOfProduct = item.NumberOfProduct,
                        Price = item.PriceProduct,
                        Discount = 0
                    });
                }
            }

            ZOrderEditRequest OrderAddDataRequest = null;
            if (req.IsTransport == "Y")
            {
                OrderAddDataRequest = new ZOrderEditRequest()
                {
                    EMSOneCode = req.EMSOneCode,
                    CustomerID = req.Receiver.ID,
                    ShipmentDetails = new ZOrderEditShipmentInfoRequest()
                    {
                        ReceiverName = req.Receiver.FullName,
                        MobileNumber = req.Receiver.MobileNumber,
                        ProvinceID = req.Receiver.Province.ID,
                        WardID = req.Receiver.Ward.ID,
                        Address = req.Receiver.Street,
                        FullAddress = req.Receiver.FullAddress,
                        PickUpAddressID = req.Sender.PickUpAddressID,
                        PickupFullName = req.Sender.FullName,
                        PickupMobileNumber = req.Sender.MobileNumber,
                        PickupProvinceID = req.Sender.Province.ID,
                        PickupWardID = req.Sender.Ward.ID,
                        PickupStreet = req.Sender.Street,
                        PickupFullAddress = req.Sender.FullAddress,
                    },
                    isUpdatedCustomer = req.IsUpdateReceiver,
                    WareHouseID = req.WareHouseID,
                    OrderedProducts = ListProductConvert,
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
                    Status = req.OrderStatus,
                    DeviceName = "WEB",
                    ExpectedDeliveryDate = req.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentID = req.CollectionPostOfficeRecentID
                };
            }
            else
            {
                OrderAddDataRequest = new ZOrderEditRequest()
                {
                    EMSOneCode = req.EMSOneCode,
                    CustomerID = req.Receiver.ID,
                    WareHouseID = req.WareHouseID,
                    OrderedProducts = ListProductConvert,
                    Transport = req.IsTransport,
                    OrderCode = req.OrderCode,
                    TransportOrderName = req.OrderName,
                    TransportOrderValue = req.OrderValue,
                    TransportQuantity = req.Quantity,
                    Note = req.Note,
                    Message = req.Message,
                    Channel = req.Channel,
                    TotalAmount = req.TotalMoney,
                    TotalDiscount = req.TotalDiscount,
                    TotalOrderValue = req.TotalOrderValue,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    Status = req.OrderStatus,
                    DeviceName = "WEB",
                    ExpectedDeliveryDate = req.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentID = req.CollectionPostOfficeRecentID
                };
            }
            return OrderAddDataRequest;
        }
        #endregion

        #region Copy đơn hàng
        [AuthorizeController(FunctionalTitle = "SAO_CHEP_DON_HANG")]
        public ActionResult CoppyOrderView(SearchListOrderViewModel req)
        {
            CoppyOrderViewModel model = null;
            var OrderID = Security.DecodeBase64(req.SearchKey);
            var res = OrderRepository.GetByID(new ZOrderGetByIDRequest() { ID = Convert.ToInt32(OrderID), ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                List<SearchListProductViewModel> ListProduct = new List<SearchListProductViewModel>();
                var item = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(res.Data);
                if (item.OrderItems.Count > 0)
                {
                    foreach (var itemOrder in item.OrderItems)
                    {
                        ListProduct.Add(new SearchListProductViewModel()
                        {
                            ProductID = itemOrder.ProductID,
                            ProductName = itemOrder.ProductName,
                            VariantID = itemOrder.VariantID,
                            VariantName = itemOrder.VariantName,
                            WareHouseQuantity = itemOrder.WareHouseQuantity,
                            NumberOfProduct = itemOrder.NumberOfProduct,
                            PriceProduct = itemOrder.Price,
                            TotalPrice = itemOrder.NumberOfProduct * itemOrder.Price,
                            Weight = itemOrder.Weight
                        });
                    }
                }
                model = new CoppyOrderViewModel()
                {
                    Buyer = new CustomerModel()
                    {
                        MobileNumber = item.Customer.MobileNumber,
                        FullName = item.Customer.FullName,
                    },
                    Sender = new CustomerModel()
                    {
                        PickUpAddressID = item.ShipmentDetails.PickUpAddressID,
                        FullName = item.ShipmentDetails.PickupFullName,
                        MobileNumber = item.ShipmentDetails.PickupMobileNumber,
                        Province = new ProvinceModel() { ID = item.ShipmentDetails.PickupProvinceID },
                        District = new DistrictModel() { ID = item.ShipmentDetails.PickupDistrictID },
                        Ward = new WardModel() { ID = item.ShipmentDetails.PickupWardID },
                        Street = item.ShipmentDetails.PickupStreet
                    },
                    Receiver = new CustomerModel()
                    {
                        FullName = item.ShipmentDetails.ReceiverName,
                        MobileNumber = item.ShipmentDetails.MobileNumber,
                        Province = new ProvinceModel() { ID = item.ShipmentDetails.ProvinceID },
                        District = new DistrictModel() { ID = item.ShipmentDetails.DistrictID },
                        Ward = new WardModel() { ID = item.ShipmentDetails.WardID },
                        Street = item.ShipmentDetails.Address
                    },
                    WareHouseID = item.WareHouseID,
                    ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                    ListProduct = ListProduct,
                    IsTransport = item.Transport,
                    OrderCode = item.OrderCode,
                    OrderName = item.TransportOrderName,
                    OrderValue = item.TransportOrderValue,
                    Quantity = item.TransportQuantity,
                    Length = item.TransportSizeLength,
                    Width = item.TransportSizeWidth,
                    Height = item.TransportSizeHeight,
                    Weight = item.TransportWeight,
                    CollectionType = new BaseModel()
                    {
                        ID = item.CollectionType.ID,
                        Code = item.CollectionType.Code,
                        Name = item.CollectionType.Name
                    },
                    TransportPayer = item.TransportPayer,
                    MainTransportServiceID = item.TransportMainService,
                    ListMainTransportService = new List<TransportServiceModel>(),
                    ExtraTransportServiceID = item.TransportExtraService,
                    ListExtraTransportService = new List<TransportServiceModel>(),
                    TotalCOD = item.TotalCOD,
                    TotalPHU = item.TotalPHU,
                    Note = item.Note,
                    Message = item.Message,
                    Channel = item.Channel,
                    ListChannel = XData.GetListChannel(),
                    TotalMoney = item.TotalAmount,
                    TotalDiscount = item.TotalDiscount,
                    TotalOrderValue = item.TotalOrderValue,
                    OrderStatus = item.Status,
                    CreatedDate = item.CreatedDate,
                    ExpectedDeliveryDate = item.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentID = item.CollectionPostOfficeRecent.ID
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

        public JsonResult JCoppyOrder(CoppyOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var OrderAddRequest = JCoppyOrderDataConvertRequest(req);
                if (req.AddType == 1 || req.AddType == 3)
                {
                    res = OrderRepository.AddOrder(OrderAddRequest);
                }
                else if (req.AddType == 2)
                {
                    res = OrderRepository.AddOrderAndSendTransport(OrderAddRequest);
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Loại đơn hàng không tồn tại.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JAddOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ZOrderAddRequest JCoppyOrderDataConvertRequest(CoppyOrderViewModel req)
        {
            List<ZOrderAddItemRequest> ListProductConvert = new List<ZOrderAddItemRequest>();
            if (req.ListProduct != null && req.ListProduct.Count > 0)
            {
                foreach (var item in req.ListProduct)
                {
                    ListProductConvert.Add(new ZOrderAddItemRequest()
                    {
                        ProductID = item.ProductID,
                        VariantID = item.VariantID,
                        NumberOfProduct = item.NumberOfProduct,
                        Price = item.PriceProduct,
                        Discount = 0
                    });
                }
            }

            ZOrderAddRequest OrderAddDataRequest = null;
            if (req.IsTransport == "Y")
            {
                OrderAddDataRequest = new ZOrderAddRequest()
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
                    OrderedProducts = ListProductConvert,
                    //ListInventoryDetail = req.ListInventoryOutput,
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
                    Status = req.OrderStatus,
                    DeviceName = "WEB",
                    ExpectedDeliveryDate = req.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentID = req.CollectionPostOfficeRecentID
                };
            }
            else
            {
                OrderAddDataRequest = new ZOrderAddRequest()
                {
                    CustomerID = req.Receiver.ID,
                    WareHouseID = req.WareHouseID,
                    OrderedProducts = ListProductConvert,
                    //ListInventoryDetail = req.ListInventoryOutput,
                    Transport = req.IsTransport,
                    OrderCode = req.OrderCode,
                    TransportOrderName = req.OrderName,
                    TransportOrderValue = req.OrderValue,
                    TransportQuantity = req.Quantity,
                    Note = req.Note,
                    Message = req.Message,
                    Channel = req.Channel,
                    TotalAmount = req.TotalMoney,
                    TotalDiscount = req.TotalDiscount,
                    TotalOrderValue = req.TotalOrderValue,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    Status = req.OrderStatus,
                    DeviceName = "WEB",
                    ExpectedDeliveryDate = req.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentID = req.CollectionPostOfficeRecentID
                };
            }
            return OrderAddDataRequest;
        }
        #endregion

        #region Tìm kiếm khách hàng
        public ActionResult SearchCustomerView(AddOrderViewModel req)
        {
            SearchCustomerViewModel model = null;
            var res = XCommon.SearchCustomerByMobileNumber(new ZCustomerGetByMobileNumberRequest() { MobileNumber = req.MobileNumber, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                var data = JsonConvert.DeserializeObject<ZCustomerGetByMobileNumberResponse>(res.Data);
                var ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());
                if (req.Receiver != null)
                {
                    model = new SearchCustomerViewModel()
                    {
                        Receiver = new CustomerModel()
                        {
                            FullName = req.Receiver.FullName,
                            MobileNumber = req.Receiver.MobileNumber,
                            Province = new ProvinceModel()
                            {
                                ID = req.Receiver.Province.ID,
                            },
                            Ward = new WardModel()
                            {
                                ID = req.Receiver.Ward.ID,
                            },
                            Street = req.Receiver.Street,
                            ListWard = XCommon.SearchListWardByProvinceID(new ZWardGetByDistrictIDRequest() { DistrictID = data.Province.ID }),
                        }
                    };

                    if (req.Receiver.FullName != data.FullName || req.Receiver.MobileNumber != data.MobileNumber ||
                        req.Receiver.Province.ID != data.Province.ID ||
                        req.Receiver.Ward.ID != data.Ward.ID || req.Receiver.Street != data.Address)
                    {
                        model.Receiver.IsChange = "Y";
                    }
                    else
                    {
                        model.Receiver.IsChange = "N";
                    }
                }
                else
                {
                    model = new SearchCustomerViewModel()
                    {
                        Receiver = new CustomerModel()
                        {
                            FullName = data.FullName,
                            MobileNumber = data.MobileNumber,
                            Province = new ProvinceModel()
                            {
                                ID = data.Province.ID,
                            },
                            Ward = new WardModel()
                            {
                                ID = data.Ward.ID,
                            },
                            Street = data.Address,
                            ListWard = XCommon.SearchListWardByProvinceID(new ZWardGetByDistrictIDRequest() { DistrictID = data.Province.ID }),
                            IsChange = "N"
                        }
                    };
                }

                model.Receiver.ID = data.ID;
                //model.Receiver.CustomerGroup = new CustomerGroupModel() { ID = data.Group.ID, Code = data.Group.Code, Name = data.Group.Name };
                model.Receiver.Email = data.Email;
                //model.Receiver.BlackList = data.BlackList;
                //model.Receiver.SuccessOrder = data.SuccessOrder.ToString("N0");
                //model.Receiver.TotalOrder = data.TotalOrder.ToString("N0");
                model.Receiver.ListProvince = ListProvince;
                model.TypeShop = data.TypeShop;

                PickUpAddressModel PickupInformation = null;
                model.ListPickUpAddress = XCommon.SearchListPickUpAddress(new ZPickUpAddressSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (model.ListPickUpAddress.Count > 0)
                {
                    if (req.PickUpAddressID > 0)
                    {
                        PickupInformation = model.ListPickUpAddress.Where(m => m.PickUpAddressID == req.PickUpAddressID).First();
                    }
                    else
                    {
                        PickupInformation = model.ListPickUpAddress.Where(m => m.IsDefault == "Y").First();
                    }
                }

                if (model.TypeShop == 1)
                {
                    if (model.ListPickUpAddress.Count > 0)
                    {
                        model.PickUpAddressID = PickupInformation.PickUpAddressID;

                        model.Sender = new CustomerModel()
                        {
                            FullName = PickupInformation.FullName,
                            MobileNumber = PickupInformation.MobileNumber,
                            Province = new ProvinceModel() { ID = PickupInformation.Province.ID },
                            Ward = new WardModel() { ID = PickupInformation.Ward.ID },
                            Street = PickupInformation.Street,
                            FullAddress = PickupInformation.FullAddress,
                            ListProvince = ListProvince,              
                            ListWard = XCommon.SearchListWardByProvinceID(new ZWardGetByDistrictIDRequest() { DistrictID = PickupInformation.Province.ID })
                        };
                    }
                }
                else
                {
                    if (req.Sender != null)
                    {
                        model.Sender = new CustomerModel()
                        {
                            FullName = req.Sender.FullName,
                            MobileNumber = req.Sender.MobileNumber,
                            Province = new ProvinceModel() { ID = req.Sender.Province.ID },
                            Ward = new WardModel() { ID = req.Sender.Ward.ID },
                            Street = req.Sender.Street,
                            FullAddress = req.Sender.FullAddress,
                            ListProvince = ListProvince,
                            ListWard = XCommon.SearchListWardByProvinceID(new ZWardGetByDistrictIDRequest() { DistrictID = PickupInformation.Province.ID })
                        };
                    }
                    else
                    {
                        model.Sender = new CustomerModel()
                        {
                            FullName = PickupInformation.FullName,
                            MobileNumber = PickupInformation.MobileNumber,
                            Province = new ProvinceModel() { ID = PickupInformation.Province.ID },
                            Ward = new WardModel() { ID = PickupInformation.Ward.ID },
                            Street = PickupInformation.Street,
                            FullAddress = PickupInformation.FullAddress,
                            ListProvince = ListProvince,
                            ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = PickupInformation.Province.ID }),
                            ListWard = XCommon.SearchListWardByProvinceID(new ZWardGetByDistrictIDRequest() { DistrictID = PickupInformation.Province.ID })
                        };
                    }
                }
                model.Buyer = new CustomerModel()
                {
                    FullName = data.FullName,
                    MobileNumber = data.MobileNumber,
                    SuccessOrder = data.SuccessOrder.ToString("N0"),
                    TotalOrder = data.TotalOrder.ToString("N0"),
                    BlackList = data.BlackList,
                    CustomerGroup = new CustomerGroupModel()
                    {
                        ID = data.Group.ID,
                        Code = data.Group.Code,
                        Name = data.Group.Name
                    },
                };
                model.IsTransport = req.IsTransport;
                model.CollectionPostOfficeRecentID = req.CollectionPostOfficeRecentID;
            }
            return PartialView(model);
        }
        #endregion

        #region Tìm kiếm sản phẩm
        public ActionResult SearchProductView(List<AddOrderViewModel> req)
        {
            List<SearchProductViewModel> model = new List<SearchProductViewModel>();
            var res = OrderRepository.SearchListProduct(new ZOrderSearchProductRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                var ListItem = JsonConvert.DeserializeObject<List<ZOrderSearchProductResponse>>(res.Data);
                foreach (var item in ListItem)
                {
                    if (req != null && req.Count > 0)
                    {
                        foreach (var Choose in req)
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
            return PartialView(model);
        }

        public ActionResult SearchListProductView(List<SearchListProductViewModel> model)
        {
            return PartialView(model);
        }
        #endregion

        #region Tính cước
        public ActionResult TemporaryTransportServiceFeeView(AddOrderViewModel req)
        {
            TemporaryTransportServiceFeeViewModel model = new TemporaryTransportServiceFeeViewModel()
            {
                ShowView = req.ShowView
            };
            if (req.IsLoad == "Y")
            {
                var res = XCommon.TransportPostageCharged(new ZTransportPostageChargedRequest()
                {
                    MainServiceID = req.MainTransportServiceID,
                    ExtraServiceIDs = req.ExtraTransportServiceID,
                    FromProvinceID = req.Sender.Province.ID,
                    FromWardID = req.Sender.Ward.ID,
                    ToProvinceID = req.Receiver.Province.ID,
                    ToWardID = req.Receiver.Ward.ID,
                    Weight = req.Weight,
                    TransportSizeLength = req.Length,
                    TransportSizeWidth = req.Width,
                    TransportSizeHeight = req.Height,
                    Quantity = req.Quantity,
                    Amount = req.OrderValue,
                    COD = req.TotalCOD,
                    CustomerCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                });
                if (res.Code == "00")
                {
                    var Item = JsonConvert.DeserializeObject<ZTransportPostageChargedResponse>(res.Data);
                    model.ExtraServiceTotalAmount = Item.ExtraFee;
                    model.MainServiceTotalAmount = Item.MainFee + Item.Surcharge;
                    model.TotalAmount = Item.TotalAmount;
                }
                else
                {
                    model.Message = res.Message;
                }
            }
            return PartialView(model);
        }

        public ActionResult WalkInTemporaryTransportServiceFeeView(AddOrderViewModel req)
        {
            TemporaryTransportServiceFeeViewModel model = new TemporaryTransportServiceFeeViewModel()
            {
                ShowView = req.ShowView
            };
            if(req.IsLoad == "Y")
            {
                var res = XCommon.TransportPostageCharged(new ZTransportPostageChargedRequest()
                {
                    MainServiceID = req.MainTransportServiceID,
                    ExtraServiceIDs = req.ExtraTransportServiceID,
                    FromProvinceID = req.Sender.Province.ID,
                    FromDistrictID = 0,
                    FromWardID = req.Sender.Ward.ID,
                    ToProvinceID = req.Receiver.Province.ID,
                    ToDistrictID = 0,
                    ToWardID = req.Receiver.Ward.ID,
                    Weight = req.Weight,
                    TransportSizeLength = req.Length,
                    TransportSizeWidth = req.Width,
                    TransportSizeHeight = req.Height,
                    Quantity = req.Quantity,
                    Amount = req.OrderValue,
                    COD = req.TotalCOD,
                    CustomerCode = "ONE-000000000",
                });
                if(res.Code == "00")
                {
                    var Item = JsonConvert.DeserializeObject<ZTransportPostageChargedResponse>(res.Data);
                    model.ExtraServiceTotalAmount = Item.ExtraFee;
                    model.MainServiceTotalAmount = Item.MainFee + Item.Surcharge;
                    model.TotalAmount = Item.TotalAmount;
                }
                else
                {
                    model.Message = res.Message;
                }
            }
            return PartialView(model);
        }

        #endregion

        #region Xem chi tiết đơn hàng bằng OrderID 
        [AuthorizeController(FunctionalTitle = "CHI_TIET_DON_HANG")]
        public ActionResult DetailOrderView(SearchListOrderViewModel req)
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
                    TotalAmount = item.TotalAmount.ToString("N0"),//Item.OrderItems.Sum(m => m.Price),
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
                    //Length = item.TransportSizeLength.ToString("N", CultureInfo.InvariantCulture),
                    //Width = item.TransportSizeWidth.ToString("N", CultureInfo.CreateSpecificCulture("en-US")),
                    //Height = item.TransportSizeHeight.ToString("N", CultureInfo.CreateSpecificCulture("en-US")),
                    Buyer = new CustomerModel()
                    {
                        FullName = item.Customer.FullName,
                        //MobileNumber = item.Customer.MobileNumber,
                        MobileNumber = "*****",
                        FullAddress = item.Customer.Address,
                    },
                    Receiver = new CustomerModel()
                    {
                        FullName = item.ShipmentDetails.ReceiverName,
                        //MobileNumber = item.ShipmentDetails.MobileNumber,
                        MobileNumber = "*****",
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
                    ListVNPSignaruteAndImageInfo = ListVNPSignaruteAndImageInformation,
                    PlaceOfCreation = item.PlaceOfCreation,
                    PostalType = item.PostalType,
                    ExpectedDeliveryDate = item.ExpectedDeliveryDate,
                    CollectionPostOfficeRecentFullAddress = item.CollectionPostOfficeRecent.FullAddress
                };
            }
            return PartialView(model);
        }
        #endregion

        #region Thêm đơn hàng bằng Excel
        [AuthorizeController(FunctionalTitle = "THEM_DON_HANG_BANG_EXCEL")]
        public ActionResult ImportExcelView(string Version)
        {
            ImportExcelViewModel model = new ImportExcelViewModel();
            model.Version = Version;
            model.License = XHelper.JExcelLicense;

            var ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());
            foreach (var item in ListProvince)
            {
                model.ListProvince.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.Name
                });
            }

            var WareHouseDefault = XCommon.DetailWareHouseDefault(new ZWarehouseGetDefaultWarehouseRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (WareHouseDefault != null)
            {
                var ListProduct = XCommon.SearchListProductVariantByWareHouseID(new ZProductVariantGetByWareHouseIDRequest() { WareHouseID = WareHouseDefault.ID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                foreach (var item in ListProduct)
                {
                    model.ListProduct.Add(new JSpreadSheetModel() { id = item.ProductVariantSKU, name = item.ProductName + " (" + item.ProductVariantName + ")" });
                }
            }

            var ListChannel = XData.GetListChannel();
            foreach (var item in ListChannel)
            {
                model.ListChannel.Add(new JSpreadSheetModel()
                {
                    id = item.Value,
                    name = item.Text
                });
            }

            var ListCollectionType = XData.GetListCollectionType();
            foreach (var item in ListCollectionType)
            {
                model.ListCollectionType.Add(new JSpreadSheetModel()
                {
                    id = item.Value,
                    name = item.Text
                });
            }

            var ListMainTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 1, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            foreach (var item in ListMainTransportService)
            {
                model.ListMainTransportService.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.Name
                });
            }

            var ListExtraTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 2, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            foreach (var item in ListExtraTransportService)
            {
                model.ListExtraTransportService.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.Name
                });
            }

            var ListTransportPayer = XData.GetListTransportPayer();
            foreach (var item in ListTransportPayer)
            {
                model.ListTransportPayer.Add(new JSpreadSheetModel()
                {
                    id = item.Value,
                    name = item.Text
                });
            }

            var res = ShopRepository.GetByShopID(new ZShopGetByIDRequest() { ID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(res.Data);
                model.TypeShop = item.TypeShop;
            }

            return View(model);
        }
        public ActionResult ImportExcelViewNew(string Version)
        {
            ImportExcelViewModel model = new ImportExcelViewModel();
            model.Version = Version;
            model.License = XHelper.JExcelLicense;

            var ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());
            foreach(var item in ListProvince)
            {
                model.ListProvince.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.Name
                });
            }

            var WareHouseDefault = XCommon.DetailWareHouseDefault(new ZWarehouseGetDefaultWarehouseRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if(WareHouseDefault != null)
            {
                var ListProduct = XCommon.SearchListProductVariantByWareHouseID(new ZProductVariantGetByWareHouseIDRequest() { WareHouseID = WareHouseDefault.ID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                foreach(var item in ListProduct)
                {
                    model.ListProduct.Add(new JSpreadSheetModel() { id = item.ProductVariantSKU, name = item.ProductName + " (" + item.ProductVariantName + ")" });
                }
            }

            var ListChannel = XData.GetListChannel();
            foreach(var item in ListChannel)
            {
                model.ListChannel.Add(new JSpreadSheetModel()
                {
                    id = item.Value,
                    name = item.Text
                });
            }

            var ListCollectionType = XData.GetListCollectionType();
            foreach(var item in ListCollectionType)
            {
                model.ListCollectionType.Add(new JSpreadSheetModel()
                {
                    id = item.Value,
                    name = item.Text
                });
            }

            var ListMainTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 1, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            foreach(var item in ListMainTransportService)
            {
                model.ListMainTransportService.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.Name
                });
            }

            var ListExtraTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 2, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            foreach(var item in ListExtraTransportService)
            {
                model.ListExtraTransportService.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.Name
                });
            }

            var ListTransportPayer = XData.GetListTransportPayer();
            foreach(var item in ListTransportPayer)
            {
                model.ListTransportPayer.Add(new JSpreadSheetModel()
                {
                    id = item.Value,
                    name = item.Text
                });
            }

            var res = ShopRepository.GetByShopID(new ZShopGetByIDRequest() { ID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if(res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(res.Data);
                model.TypeShop = item.TypeShop;
            }

            return View(model);
        }
        public ActionResult SearchListExcelImportView()
        {
            List<SearchListExcelImportViewModel> model = new List<SearchListExcelImportViewModel>();
            var res = OrderRepository.SearchListImportExcel(new ZOrderGetByIDRequest {
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if(res.Code == "00")
            {
                var data = JsonConvert.DeserializeObject<List<XCustomeImportOrderJobInfo>>(res.Data);
                foreach(var item in data)
                {
                    model.Add(new SearchListExcelImportViewModel (){
                        JobId = item.JobId,
                        FileName = item.FileName,
                        Status = item.Status,
                        TotalRows = item.TotalRows,
                        SuccessRows = item.SuccessRows,
                        FailedRows = item.FailedRows,
                        CreatedAt = item.CreatedAt
                    });
                }
            }
            return PartialView(model);
        }
        public JsonResult JImportExcelData(ImportExcelViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                HttpPostedFileBase files = Request.Files[0]; //Read the Posted Excel File  
                var filesize = files.ContentLength;
                ISheet sheet; //Create the ISheet object to read the sheet cell values  
                string filename = Request["FileName"]; //get the uploaded file name  
                var fileExt = Path.GetExtension(filename); //get the extension of uploaded excel file  
                if (fileExt == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(files.InputStream); //HSSWorkBook object will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(files.InputStream); //XSSFWorkBook will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
                }
                var total = sheet.LastRowNum;
                if (total > 1000)
                {
                    res.Code = "02";
                    res.Message = "Hệ thống chỉ hỗ trợ import tối đa 1000 dòng";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                List<ArrayList> model = new List<ArrayList>();
                ZShopGetByIDResponse ShopInformation = new ZShopGetByIDResponse();
                var resShopInformation = ShopRepository.GetByShopID(new ZShopGetByIDRequest() { ID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (resShopInformation.Code == "00")
                {
                    ShopInformation = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(resShopInformation.Data);
                }

                var isService108 = false;
                var isService421 = false;
                List<TransportServiceModel> ListMainTransportService = new List<TransportServiceModel>();
                List<TransportServiceModel> ListExtraTransportService = new List<TransportServiceModel>();
                var ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (ListTransportService.Count > 0)
                {
                    ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                    ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();

                    if (ListExtraTransportService.Where(m => m.ID == 108).ToList().Count > 0)
                    {
                        isService108 = true;
                    }

                    if (ListExtraTransportService.Where(m => m.ID == 421).ToList().Count > 0)
                    {
                        isService421 = true;
                    }
                }

                List<ZProductVariantSearchListResponse> ListProductVariant = new List<ZProductVariantSearchListResponse>();
                var resProductVariant = ProductVariantRepository.SearchList(new ZProductVariantSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (resProductVariant.Code == "00")
                {
                    ListProductVariant = JsonConvert.DeserializeObject<List<ZProductVariantSearchListResponse>>(resProductVariant.Data);
                }

                //Loop the records upto filled row
                for (int row = 4; row <= total; row++)
                {
                    var ReceiverProvinceID = 0;
                    //var ReceiverDistrictID = 0;
                    var ReceiverWardID = 0;

                    var SenderProvinceID = 0;
                    //var SenderDistrictID = 0;
                    var SenderWardID = 0;

                    long TotalAmount = 0;
                    if (sheet.GetRow(row) != null)
                    {
                        var ReceiverName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(3));
                        if (!string.IsNullOrEmpty(ReceiverName)) //CHECK HANG NULL
                        {
                            var Product = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(1));
                            var OrderCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(2));

                            var ReceiverMobileNumber = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(4));
                            var ReceiverStreet = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(5));
                            var ReceiverProvinceName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(6));
                            //var ReceiverDistrictName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(7));
                            var ReceiverWardName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(7));
                            var Message = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(8));
                            var NumberOfProduct = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(9));
                            var Weight = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(10));
                            var TotalCOD = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(11));
                            var OrderValue = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(12));
                            var Note = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(13));
                            var TransportMainService = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(14));
                            var TransportExtraService = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(15));
                            var SenderName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(16));
                            var SenderMobileNumber = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(17));
                            var SenderStreet = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(18));
                            var SenderProvinceName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(19));
                            //var SenderDistrictName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(21));
                            var SenderWardName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(20));
                            var CollectionType = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(21));
                            var TransportPayer = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(22));
                            var Long = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(23));
                            var Width = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(24));
                            var Height = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(25));
                            var Channel = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(26));
                            var TotalPHU = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(27));

                            //Xử lý dữ liệu người nhận
                            Task ReceiverHandle = new Task(() =>
                            {
                                if (!string.IsNullOrEmpty(ReceiverProvinceName) && !string.IsNullOrEmpty(ReceiverWardName))
                                {
                                    var resPWD = PWDRepository.SearchPDWIDByPDWName(new ZMapsGetByPWDNameRequest()
                                    {
                                        ProvinceName = ReceiverProvinceName.Trim(),
                                        //DistrictName = ReceiverDistrictName.Trim(),
                                        WardName = ReceiverWardName.Trim()
                                    });
                                    if (resPWD.Code == "00")
                                    {
                                        var dataPWD = JsonConvert.DeserializeObject<ZMapsGetByPWDNameResponse>(resPWD.Data);
                                        ReceiverProvinceID = dataPWD.ProvinceID;
                                        //ReceiverDistrictID = dataPWD.DistrictID;
                                        ReceiverWardID = dataPWD.WardID;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(ReceiverStreet))
                                    {
                                        ZResponse resSearchPWDByMapsSearch = new ZResponse();
                                        if (req.Version == "EMS")
                                        {
                                            resSearchPWDByMapsSearch = XCommon.SearchPWDByEMSMapsSearch(ReceiverStreet);
                                        }
                                        else if (req.Version == "EMSOne")
                                        {
                                            resSearchPWDByMapsSearch = XCommon.SearchPWDByVNPMapsSearch(ReceiverStreet);
                                        }
                                        else if(req.Version == "EMSV2")
                                        {
                                            resSearchPWDByMapsSearch = XCommon.SearchPWDByEMSMapsSearchV2(ReceiverStreet);
                                        }


                                        if (resSearchPWDByMapsSearch.Code == "00")
                                        {
                                            var dataVnpMapsSearch = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeResponse>(resSearchPWDByMapsSearch.Data);
                                            ReceiverProvinceName = dataVnpMapsSearch.ProvinceName;
                                            //ReceiverDistrictName = dataVnpMapsSearch.DistrictName;
                                            ReceiverWardName = dataVnpMapsSearch.WardName;
                                            ReceiverProvinceID = dataVnpMapsSearch.ProvinceID;
                                            //ReceiverDistrictID = dataVnpMapsSearch.DistrictID;
                                            ReceiverWardID = dataVnpMapsSearch.WardID;
                                            switch (req.Version)
                                            {
                                                case "EMS":
                                                    ReceiverStreet = "(EMSmap) " + ReceiverStreet;
                                                    break;
                                                case "EMSOne":
                                                    ReceiverStreet = "(Vmapcode) " + ReceiverStreet;
                                                    break;
                                            }
                                            //ReceiverStreet = dataVnpMapsSearch.Street;
                                        }
                                    }
                                }
                            });

                            //Xử lý dữ liệu người gửi
                            Task SenderHandle = new Task(() =>
                            {
                                if (ShopInformation.TypeShop == 1)
                                {
                                    SenderName = "";
                                    SenderMobileNumber = "";
                                    SenderProvinceID = 0;
                                    SenderProvinceName = "";
                                    //SenderDistrictID = 0;
                                    //SenderDistrictName = "";
                                    SenderWardID = 0;
                                    SenderWardName = "";
                                    SenderStreet = "";
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(SenderProvinceName) && !string.IsNullOrEmpty(SenderWardName))
                                    {
                                        var resPWD = PWDRepository.SearchPDWIDByPDWName(new ZMapsGetByPWDNameRequest()
                                        {
                                            ProvinceName = SenderProvinceName.Trim(),
                                            //DistrictName = SenderDistrictName.Trim(),
                                            WardName = SenderWardName.Trim()
                                        });
                                        if (resPWD.Code == "00")
                                        {
                                            var dataPWD = JsonConvert.DeserializeObject<ZMapsGetByPWDNameResponse>(resPWD.Data);
                                            SenderProvinceID = dataPWD.ProvinceID;
                                            //SenderDistrictID = dataPWD.DistrictID;
                                            SenderWardID = dataPWD.WardID;
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(SenderStreet))
                                        {
                                            ZResponse resSearchPWDByMapsSearch = new ZResponse();
                                            if (req.Version == "EMS")
                                            {
                                                resSearchPWDByMapsSearch = XCommon.SearchPWDByEMSMapsSearch(SenderStreet);
                                            }
                                            else if (req.Version == "EMSOne")
                                            {
                                                resSearchPWDByMapsSearch = XCommon.SearchPWDByVNPMapsSearch(SenderStreet);
                                            }
                                            else if(req.Version == "EMSV2")
                                            {
                                                resSearchPWDByMapsSearch = XCommon.SearchPWDByEMSMapsSearchV2(SenderStreet);
                                            }

                                            if (resSearchPWDByMapsSearch.Code == "00")
                                            {
                                                var dataVnpMapsSearch = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeResponse>(resSearchPWDByMapsSearch.Data);
                                                SenderProvinceName = dataVnpMapsSearch.ProvinceName;
                                                //SenderDistrictName = dataVnpMapsSearch.DistrictName;
                                                SenderWardName = dataVnpMapsSearch.WardName;
                                                SenderProvinceID = dataVnpMapsSearch.ProvinceID;
                                                //SenderDistrictID = dataVnpMapsSearch.DistrictID;
                                                SenderWardID = dataVnpMapsSearch.WardID;
                                                switch (req.Version)
                                                {
                                                    case "EMS":
                                                        SenderStreet = "(EMSmap) " + SenderStreet;
                                                        break;
                                                    case "EMSOne":
                                                        SenderStreet = "(Vmapcode) " + SenderStreet;
                                                        break;
                                                }
                                                //SenderStreet = dataVnpMapsSearch.Street;
                                            }
                                        }
                                    }
                                }
                            });

                            ReceiverHandle.Start();
                            SenderHandle.Start();

                            //Số lượng sản phẩm
                            if (string.IsNullOrEmpty(NumberOfProduct))
                            {
                                NumberOfProduct = "1";
                            }

                            //Sản phẩm
                            if (!string.IsNullOrEmpty(Product) && ListProductVariant.Count > 0)
                            {
                                var checkProductVariant = ListProductVariant.Where(m => m.Product.ProductVariant.ProductVariantSKU == Product).ToList();
                                if (checkProductVariant.Count > 0)
                                {
                                    TotalAmount = checkProductVariant.Select(m => m.Product.ProductVariant.ProductVariantPrice).First() * Convert.ToInt32(NumberOfProduct);

                                    //Trọng lượng sản phẩm
                                    if (string.IsNullOrEmpty(Weight))
                                    {
                                        Weight = (checkProductVariant.Select(m => m.Product.ProductVariant.ProductVariantWeight).First() * Convert.ToDouble(NumberOfProduct)).ToString();
                                    }
                                }
                            }

                            //Số điện thoại người nhận
                            if (!string.IsNullOrEmpty(ReceiverMobileNumber))
                            {
                                var ReplaceSpecialChar = Regex.Replace(ReceiverMobileNumber, @"[^\d]", String.Empty);
                                if (ReplaceSpecialChar.Substring(0, 3).Contains("840"))
                                {
                                    ReceiverMobileNumber = ReplaceSpecialChar.Substring(0, 3).Replace("840", "0") + ReplaceSpecialChar.Substring(3, ReplaceSpecialChar.Length - 3);
                                }
                                else if (ReplaceSpecialChar.Substring(0, 2).Contains("84"))
                                {
                                    ReceiverMobileNumber = ReplaceSpecialChar.Substring(0, 2).Replace("84", "0") + ReplaceSpecialChar.Substring(2, ReplaceSpecialChar.Length - 2);
                                }
                                else if (ReplaceSpecialChar.Substring(0, 1) != "0")
                                {
                                    ReceiverMobileNumber = "0" + ReplaceSpecialChar;
                                }
                                else
                                {
                                    ReceiverMobileNumber = ReplaceSpecialChar;
                                }
                            }

                            //Số điện thoại người gửi
                            if (!string.IsNullOrEmpty(SenderMobileNumber))
                            {
                                var ReplaceSpecialChar = Regex.Replace(SenderMobileNumber, @"[^\d]", String.Empty);
                                if (ReplaceSpecialChar.Substring(0, 3).Contains("840"))
                                {
                                    SenderMobileNumber = ReplaceSpecialChar.Substring(0, 3).Replace("840", "0") + ReplaceSpecialChar.Substring(3, ReplaceSpecialChar.Length - 3);
                                }
                                else if (ReplaceSpecialChar.Substring(0, 2).Contains("84"))
                                {
                                    SenderMobileNumber = ReplaceSpecialChar.Substring(0, 2).Replace("84", "0") + ReplaceSpecialChar.Substring(2, ReplaceSpecialChar.Length - 2);
                                }
                                else if (ReplaceSpecialChar.Substring(0, 1) != "0")
                                {
                                    SenderMobileNumber = "0" + ReplaceSpecialChar;
                                }
                                else
                                {
                                    SenderMobileNumber = ReplaceSpecialChar;
                                }
                            }

                            //Kênh bán hàng
                            Channel = string.IsNullOrEmpty(Channel) ? "TT" : XData.GetValueChannel(Channel);

                            //Người trả tiền cước
                            TransportPayer = string.IsNullOrEmpty(TransportPayer) ? "1" : XData.GetValueTransportPayer(TransportPayer);

                            //Dịch vụ chính
                            if (!string.IsNullOrEmpty(TransportMainService))
                            {
                                if (ListMainTransportService.Count > 0)
                                {
                                    var checkMainTransportService = ListMainTransportService.Where(m => m.Name.ToLower() == TransportMainService.ToLower()).ToList();
                                    if (checkMainTransportService.Count > 0)
                                    {
                                        TransportMainService = checkMainTransportService.Select(m => m.ID).First().ToString();
                                    }
                                    else
                                    {
                                        TransportMainService = "";
                                    }
                                }
                                else
                                {
                                    TransportMainService = "";
                                }
                            }

                            //Dịch vụ cộng thêm
                            if (!string.IsNullOrEmpty(TransportExtraService))
                            {
                                if (ListExtraTransportService.Count > 0)
                                {
                                    var tempTransportExtraService = "";
                                    foreach (var item in TransportExtraService.Split(';'))
                                    {
                                        var checkExtraTransportService = ListExtraTransportService.Where(m => m.Name.ToLower() == item.Trim().ToLower()).ToList();
                                        if (checkExtraTransportService.Count > 0)
                                        {
                                            var ExtraTransportServiceID = checkExtraTransportService.Select(m => m.ID).First().ToString();
                                            tempTransportExtraService += ExtraTransportServiceID + ";";
                                            if (ExtraTransportServiceID == "108")
                                            {
                                                if (string.IsNullOrEmpty(TotalCOD))
                                                {
                                                    TotalCOD = TotalAmount.ToString();
                                                }
                                            }
                                        }
                                    }
                                    TransportExtraService = tempTransportExtraService.TrimEnd(';');
                                }
                                else
                                {
                                    TransportExtraService = "";
                                }
                            }

                            if (isService108 == true)
                            {
                                if (!string.IsNullOrEmpty(TotalCOD) && TransportExtraService.Contains("108") == false)
                                {
                                    TransportExtraService += string.IsNullOrEmpty(TransportExtraService) ? "108" : ";108";
                                }
                            }
                            else
                            {
                                TotalCOD = "";
                            }

                            if (isService421 == true)
                            {
                                if (!string.IsNullOrEmpty(TotalPHU) && TransportExtraService.Contains("421") == false)
                                {
                                    TransportExtraService += string.IsNullOrEmpty(TransportExtraService) ? "421" : ";421";
                                }
                            }
                            else
                            {
                                TotalPHU = "";
                            }


                            if (TransportPayer == "2")
                            {
                                switch (TransportMainService)
                                {
                                    case "141":
                                    case "360":
                                    case "481":
                                        if (TransportExtraService.Contains("300") == false && TransportExtraService.Contains("221") == false)
                                        {
                                            TransportExtraService += string.IsNullOrEmpty(TransportExtraService) ? "108" : ";108";
                                        }
                                        break;
                                    default:
                                        if (TransportExtraService.Contains("300") == false && TransportExtraService.Contains("221") == false)
                                        {
                                            TransportPayer = "1";
                                        }
                                        break;
                                }
                            }

                            //Giá trị đơn hàng
                            if (string.IsNullOrEmpty(OrderValue))
                            {
                                OrderValue = TotalAmount.ToString();
                            }

                            CollectionType = string.IsNullOrEmpty(CollectionType) ? ShopInformation.CollectionType.ToString() : XData.GetValueCollectionType(CollectionType);

                            Task.WaitAll(ReceiverHandle, SenderHandle);

                            ArrayList data = new ArrayList()
                            {
                                "",
                                Product,
                                OrderCode,
                                ReceiverName,
                                ReceiverMobileNumber,
                                ReceiverStreet,
                                ReceiverProvinceName,
                                ReceiverWardName,
                                Message,
                                NumberOfProduct,
                                Weight,
                                TotalCOD,
                                OrderValue,
                                Note,
                                TransportMainService,
                                TransportExtraService,
                                SenderName,
                                SenderMobileNumber,
                                SenderStreet,
                                SenderProvinceName,
                                SenderWardName,
                                CollectionType,
                                TransportPayer,
                                Long,
                                Width,
                                Height,
                                Channel,
                                TotalPHU,
                                ReceiverProvinceID,
                                ReceiverWardID,
                                SenderProvinceID,
                                SenderWardID
                            };

                            model.Add(data);
                        }
                    }
                }

                if (model.Count > 0)
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                    res.Data = JsonConvert.SerializeObject(model);
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Không có dữ liệu";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JImportExcelData: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JImportExcelDataNew(ImportExcelViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if(Request.Files.Count == 0)
                {
                    res.Code = "01";
                    res.Message = "Không có file upload";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                HttpPostedFileBase file = Request.Files[0];
                if(file == null || file.ContentLength <= 0)
                {
                    res.Code = "01";
                    res.Message = "File không hợp lệ";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                string fileName = file.FileName;
                if(string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = Request["FileName"];
                }

                var fileExt = Path.GetExtension(fileName)?.ToLowerInvariant();
                if(fileExt != ".xls" && fileExt != ".xlsx" && fileExt != ".xlsm")
                {
                    res.Code = "01";
                    res.Message = "Hệ thống chỉ hỗ trợ file Excel .xls, .xlsx, .xlsm";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                // Reset stream trước khi đẩy sang Gateway/Core
                if(file.InputStream.CanSeek)
                {
                    file.InputStream.Position = 0;
                }

                // Mapping từ ViewModel sang request gửi xuống gateway/core
                var request = new ZShopGetByIDRequest
                {
                    ID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                };

                res = OrderRepository.ImportExcelOrder(file, request);
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JImportExcelDataNew: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JSearchListOrderImport(ZOrderImportSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ArrayList> model = new List<ArrayList>();
                var resOrderImport = OrderRepository.SearchListOrderImport(new ZOrderImportSearchListRequest
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    JobID = req.JobID
                });
                if(resOrderImport.Code == "00")
                {
                    res.Code = "00";
                    res.Message = "Thành công";
                    res.Data = resOrderImport.Data;
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Không có dữ liệu";
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JSearchListOrderImport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JImportExcel(List<ImportExcelOrderModel> data, int AddType, int countKHL, int countLoi)
        {
            ZResponse res = new ZResponse();
            var count = 0;
            var rowAction = 0;
            try
            {
                var total = data.Count;
                if (total > 1000)
                {
                    res.Code = "02";
                    res.Message = "Hệ thống chỉ hỗ trợ import tối đa 1000 dòng";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                var countError = countLoi;
                var ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID;
                var DefaultWarehouseID = 0;
                var DetailWareHouseDefault = XCommon.DetailWareHouseDefault(new ZWarehouseGetDefaultWarehouseRequest() { ShopID = ShopID });
                if (DetailWareHouseDefault != null)
                {
                    DefaultWarehouseID = DetailWareHouseDefault.ID;
                }

                string Status = null;
                if (AddType == 3)//Đơn nháp
                {
                    Status = "D";
                }

                List<ArrayList> lstError = new List<ArrayList>();
                ZShopGetByIDResponse ShopInformation = new ZShopGetByIDResponse();
                var resShopInformation = ShopRepository.GetByShopID(new ZShopGetByIDRequest() { ID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (resShopInformation.Code == "00")
                {
                    ShopInformation = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(resShopInformation.Data);
                }

                foreach (var itemExcel in data)
                {
                    rowAction++;

                    if (string.IsNullOrEmpty(itemExcel.CollectionType))
                    {
                        itemExcel.CollectionType = ShopInformation.CollectionType.ToString();
                    }

                    if (string.IsNullOrEmpty(itemExcel.OrderValue))
                    {
                        itemExcel.OrderValue = "0";
                    }

                    string TransportExtraService = "";
                    if (!string.IsNullOrEmpty(itemExcel.TransportExtraService))
                    {
                        TransportExtraService = itemExcel.TransportExtraService.TrimEnd(';').Replace(';', ',');
                    };

                    //Sản phẩm
                    long TotalAmount = 0;
                    List<ZOrderAddItemRequest> OrderedProducts = new List<ZOrderAddItemRequest>();

                    //List<ZInventoryDetailAddRequest> ListInventoryDetail = new List<ZInventoryDetailAddRequest>();
                    if (!string.IsNullOrEmpty(itemExcel.Product))
                    {
                        var resDetailProductVariantBySKU = ProductVariantRepository.GetBySKU(new ZProductVariantGetBySKURequest() { SKU = itemExcel.Product, ShopID = ShopID });
                        if (resDetailProductVariantBySKU.Code == "00")
                        {
                            var ProductVariant = JsonConvert.DeserializeObject<ZProductVariantGetBySKUResponse>(resDetailProductVariantBySKU.Data);
                            OrderedProducts.Add(new ZOrderAddItemRequest()
                            {
                                ProductID = ProductVariant.ProductID,
                                VariantID = ProductVariant.VariantID,
                                Price = ProductVariant.Price,
                                NumberOfProduct = Convert.ToInt32(itemExcel.NumberOfProduct)
                            });

                            TotalAmount = ProductVariant.Price * Convert.ToInt32(itemExcel.NumberOfProduct);

                            //Giá trị đơn hàng
                            if (itemExcel.OrderValue == "0")
                            {
                                itemExcel.OrderValue = TotalAmount.ToString();
                            }

                            //Trọng lượng sản phẩm
                            if (string.IsNullOrEmpty(itemExcel.Weight))
                            {
                                itemExcel.Weight = ProductVariant.Weight.ToString();
                            }
                        }
                        else if (AddType != 3)
                        {
                            lstError.Add(JImportExcelError(itemExcel, "Sản phẩm - " + resDetailProductVariantBySKU.Message));
                            countError++;
                            continue;
                        }
                    }

                    if (!string.IsNullOrEmpty(TransportExtraService))
                    {
                        var checkKG = TransportExtraService.Contains("103");
                        if (checkKG == true)
                        {
                            if (itemExcel.OrderValue == "0" && AddType != 3)
                            {
                                lstError.Add(JImportExcelError(itemExcel, "Vui lòng nhập giá trị đơn hàng"));
                                countError++;
                                continue;
                            }
                        }

                        var checkCOD = TransportExtraService.Contains("108");
                        if (checkCOD == true)
                        {
                            if (itemExcel.TotalCOD == "0" && TotalAmount > 0)
                            {
                                itemExcel.TotalCOD = TotalAmount.ToString();
                            }
                        }

                        var checkPHU = TransportExtraService.Contains("421");
                        if (checkPHU == true)
                        {
                            if (itemExcel.TotalPHU == "0" && AddType != 3)
                            {
                                lstError.Add(JImportExcelError(itemExcel, "Vui lòng nhập thu phí hủy đơn"));
                                countError++;
                                continue;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(itemExcel.TotalPHU) && itemExcel.TotalPHU != "0" && TransportExtraService.Contains("421") == false)
                    {
                        TransportExtraService += string.IsNullOrEmpty(TransportExtraService) == true ? "421" : ",421";
                    }

                    if (Convert.ToInt64(itemExcel.OrderValue) > 100000000 && AddType != 3)
                    {
                        lstError.Add(JImportExcelError(itemExcel, "Giá trị đơn hàng không vượt quá 100 triệu đồng."));
                        countError++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(itemExcel.Channel))
                    {
                        itemExcel.Channel = "TT";
                    }

                    //-------------- Khách hàng
                    var CustomerID = 0;
                    if (!string.IsNullOrEmpty(itemExcel.MobileNumber))
                    {
                        var resIsExist = CustomerRepository.IsExist(new ZCustomerIsExistRequest()
                        {
                            MobileNumber = itemExcel.MobileNumber,
                            FullName = itemExcel.FullName,
                            ProvinceID = itemExcel.ProvinceID,
                            DistrictID = itemExcel.DistrictID,
                            WardID = itemExcel.WardID,
                            Street = itemExcel.Street,
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                            CreatedBy = XHelper.Session.EmployeeProfile.ID,
                            Channel = "TT",
                            IsUpdated = "N"
                        });

                        if (resIsExist.Code == "00")
                        {
                            CustomerID = Convert.ToInt32(resIsExist.Data);
                        }
                        else if (AddType != 3)
                        {
                            lstError.Add(JImportExcelError(itemExcel, "Khách hàng - " + resIsExist.Message));
                            countError++;
                            continue;
                        }
                    }

                    ZOrderAddShipmentInfoRequest ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                    {
                        PickUpAddressID = itemExcel.PickUpAddressID,
                        PickupFullName = itemExcel.SenderName,
                        PickupMobileNumber = itemExcel.SenderMobileNumber,
                        PickupProvinceID = itemExcel.SenderProvinceID,
                        PickupDistrictID = itemExcel.SenderDistrictID,
                        PickupWardID = itemExcel.SenderWardID,
                        PickupStreet = itemExcel.SenderStreet,
                        ReceiverName = itemExcel.FullName,
                        MobileNumber = itemExcel.MobileNumber,
                        ProvinceID = itemExcel.ProvinceID,
                        DistrictID = itemExcel.DistrictID,
                        WardID = itemExcel.WardID,
                        Address = itemExcel.Street,
                    };

                    if (ShopInformation.TypeShop == 1)
                    {
                        ShipmentDetails.PickUpAddressID = ShopInformation.PickUpID;
                        ShipmentDetails.PickupFullName = ShopInformation.PickUpFullName;
                        ShipmentDetails.PickupMobileNumber = ShopInformation.MobileNumber;
                        ShipmentDetails.PickupProvinceID = ShopInformation.PickUpProvinceID;
                        ShipmentDetails.PickupDistrictID = ShopInformation.PickUpDistrictID;
                        ShipmentDetails.PickupWardID = ShopInformation.PickUpWardID;
                        ShipmentDetails.PickupStreet = ShopInformation.PickUpStreet;
                    }
                    else
                    {
                        ShipmentDetails.PickUpAddressID = 0;
                        if (itemExcel.SenderProvinceID == 0 && itemExcel.SenderDistrictID == 0 && itemExcel.SenderWardID == 0)
                        {
                            ShipmentDetails.PickupProvinceID = ShopInformation.PickUpProvinceID;
                            ShipmentDetails.PickupDistrictID = ShopInformation.PickUpDistrictID;
                            ShipmentDetails.PickupWardID = ShopInformation.PickUpWardID;
                            ShipmentDetails.PickupStreet = ShopInformation.PickUpStreet;
                        }

                        if (string.IsNullOrEmpty(itemExcel.SenderName))
                        {
                            ShipmentDetails.PickupFullName = ShopInformation.PickUpFullName;
                        }

                        if (string.IsNullOrEmpty(itemExcel.SenderMobileNumber))
                        {
                            ShipmentDetails.PickupMobileNumber = ShopInformation.MobileNumber;
                        }
                    }

                    //-------------- thông tin giao nhận
                    //ZOrderAddShipmentInfoRequest ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                    //{
                    //    PickUpAddressID = itemExcel.PickUpAddressID,
                    //    PickupFullName = itemExcel.SenderName,
                    //    PickupMobileNumber = itemExcel.SenderMobileNumber,
                    //    PickupProvinceID = itemExcel.SenderProvinceID,
                    //    PickupDistrictID = itemExcel.SenderDistrictID,
                    //    PickupWardID = itemExcel.SenderWardID,
                    //    PickupStreet = itemExcel.SenderStreet,
                    //    //PickupFullAddress = itemExcel.SenderFullAddress,
                    //    ReceiverName = itemExcel.FullName,
                    //    MobileNumber = itemExcel.MobileNumber,
                    //    ProvinceID = itemExcel.ProvinceID,
                    //    DistrictID = itemExcel.DistrictID,
                    //    WardID = itemExcel.WardID,
                    //    Address = itemExcel.Street,
                    //    //FullAddress = XCommon.ConvertAddress("", itemExcel.Street, itemExcel.WardName, itemExcel.DistrictName, itemExcel.ProvinceName)
                    //};

                    //---------------- Tính cước
                    long Charged = 0;
                    var resCharged = XCommon.TransportPostageCharged(new ZTransportPostageChargedRequest()
                    {
                        MainServiceID = Convert.ToInt32(itemExcel.TransportMainService),
                        ExtraServiceIDs = TransportExtraService,
                        FromProvinceID = ShipmentDetails.PickupProvinceID,
                        FromDistrictID = ShipmentDetails.PickupDistrictID,
                        FromWardID = ShipmentDetails.PickupWardID,
                        ToProvinceID = itemExcel.ProvinceID,
                        ToDistrictID = itemExcel.DistrictID,
                        ToWardID = itemExcel.WardID,
                        Weight = Convert.ToDouble(itemExcel.Weight),
                        TransportSizeLength = Convert.ToDouble(itemExcel.Long),
                        TransportSizeWidth = Convert.ToDouble(itemExcel.Width),
                        TransportSizeHeight = Convert.ToDouble(itemExcel.Height),
                        Quantity = Convert.ToInt64(itemExcel.NumberOfProduct),
                        Amount = Convert.ToInt64(itemExcel.OrderValue),
                        COD = Convert.ToInt64(itemExcel.TotalCOD),
                        CustomerCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                    });
                    if (resCharged.Code == "00")
                    {
                        var modelCharged = JsonConvert.DeserializeObject<ZTransportPostageChargedResponse>(resCharged.Data);
                        Charged = modelCharged.TotalAmount;
                    }
                    else if (AddType != 3)
                    {
                        lstError.Add(JImportExcelError(itemExcel, "Tính cước - " + resCharged.Message));
                        countError++;
                        continue;
                    };

                    //Tạo đơn hàng
                    ZResponse resItem = new ZResponse();
                    if (AddType == 1 || AddType == 3)
                    {
                        resItem = OrderRepository.AddOrder(new ZOrderAddRequest()
                        {
                            OrderCode = itemExcel.OrderCode,
                            OrderedProducts = OrderedProducts,
                            WareHouseID = DefaultWarehouseID,
                            CustomerID = CustomerID,
                            ShipmentDetails = ShipmentDetails,
                            TotalAmount = TotalAmount,
                            TotalDiscount = 0,
                            TotalOrderValue = Convert.ToInt64(itemExcel.OrderValue),
                            TotalCOD = Convert.ToInt64(itemExcel.TotalCOD),
                            TotalPHU = Convert.ToInt64(itemExcel.TotalPHU),
                            ShopID = ShopID,
                            CreatedBy = XHelper.Session.EmployeeProfile.ID,
                            Transport = itemExcel.Transport,
                            Channel = itemExcel.Channel,
                            isRequestOrder = "N",
                            isUpdatedCustomer = "N",
                            RequestOrderID = 0,
                            Status = Status,
                            Note = itemExcel.Note,
                            Message = itemExcel.Message,
                            ProductSKU = "",
                            TransportOrderName = "",
                            TransportWeight = Convert.ToDouble(itemExcel.Weight),
                            TransportSizeLength = Convert.ToDouble(itemExcel.Long),
                            TransportSizeWidth = Convert.ToDouble(itemExcel.Width),
                            TransportSizeHeight = Convert.ToDouble(itemExcel.Height),
                            TransportQuantity = Convert.ToInt64(itemExcel.NumberOfProduct),
                            TransportOrderValue = Convert.ToInt64(itemExcel.OrderValue),
                            TransportMainService = Convert.ToInt32(itemExcel.TransportMainService),
                            TransportExtraService = TransportExtraService,
                            TransportFee = Charged,
                            TransportPayer = Convert.ToInt32(itemExcel.TransportPayer),
                            CollectionType = Convert.ToInt32(itemExcel.CollectionType),
                            DeviceName = "WEB"
                        });
                    }
                    else if (AddType == 2)
                    {
                        resItem = OrderRepository.AddOrderAndSendTransport(new ZOrderAddRequest()
                        {
                            OrderCode = itemExcel.OrderCode,
                            OrderedProducts = OrderedProducts,
                            WareHouseID = DefaultWarehouseID,
                            CustomerID = CustomerID,
                            ShipmentDetails = ShipmentDetails,
                            TotalAmount = TotalAmount,
                            TotalDiscount = 0,
                            TotalOrderValue = Convert.ToInt64(itemExcel.OrderValue),
                            TotalCOD = Convert.ToInt64(itemExcel.TotalCOD),
                            TotalPHU = Convert.ToInt64(itemExcel.TotalPHU),
                            ShopID = ShopID,
                            CreatedBy = XHelper.Session.EmployeeProfile.ID,
                            Transport = itemExcel.Transport,
                            Channel = itemExcel.Channel,
                            isRequestOrder = "N",
                            isUpdatedCustomer = "N",
                            RequestOrderID = 0,
                            Status = Status,
                            Note = itemExcel.Note,
                            Message = itemExcel.Message,
                            ProductSKU = "",
                            TransportOrderName = "",
                            TransportWeight = Convert.ToDouble(itemExcel.Weight),
                            TransportSizeLength = Convert.ToDouble(itemExcel.Long),
                            TransportSizeWidth = Convert.ToDouble(itemExcel.Width),
                            TransportSizeHeight = Convert.ToDouble(itemExcel.Height),
                            TransportQuantity = Convert.ToInt64(itemExcel.NumberOfProduct),
                            TransportOrderValue = Convert.ToInt64(itemExcel.OrderValue),
                            TransportMainService = Convert.ToInt32(itemExcel.TransportMainService),
                            TransportExtraService = TransportExtraService,
                            TransportFee = Charged,
                            TransportPayer = Convert.ToInt32(itemExcel.TransportPayer),
                            CollectionType = Convert.ToInt32(itemExcel.CollectionType),
                            DeviceName = "WEB"
                        });
                    }
                    else
                    {
                        lstError.Add(JImportExcelError(itemExcel, "Không xác định được loại Import"));
                        countError++;
                        continue;
                    }

                    if (resItem.Code == "00")
                    {
                        count++;
                    }
                    else
                    {
                        lstError.Add(JImportExcelError(itemExcel, "Đơn hàng - " + resItem.Message));
                        countError++;
                        continue;
                    }
                }

                if (total == count)
                {
                    res.Code = "00";
                    res.Message = string.Format("<div class='text-green mb-2'>Đơn tạo thành công: {0}</div><div class='text-warning mb-2'>Đơn không hợp lệ: {1}</div><div class='text-danger mb-3'>Đơn lỗi: {2}</div>", count, countKHL, countError);
                }
                else
                {
                    res.Code = "10";
                    res.Message = string.Format("<div class='text-green mb-2'>Đơn tạo thành công: {0}</div><div class='text-warning mb-2'>Đơn không hợp lệ: {1}</div><div class='text-danger mb-3'>Đơn lỗi: {2}</div>", count, countKHL, countError);
                    res.Data = JsonConvert.SerializeObject(lstError);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("OrderController.JImportExcel: rowAction={0} || exception={1}", rowAction, ex.Message));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị. Đơn tạo thành công: " + count;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ArrayList JImportExcelError(ImportExcelOrderModel data, string Message)
        {
            ArrayList model = new ArrayList();
            model.Add("Lỗi");
            model.Add(data.Product);
            model.Add(data.OrderCode);
            model.Add(data.FullName);
            model.Add(data.MobileNumber);
            model.Add(data.Street);
            model.Add(data.ProvinceName);
            //model.Add(data.DistrictName);
            model.Add(data.WardName);
            model.Add(data.Message);
            model.Add(data.NumberOfProduct);
            model.Add(data.Weight);
            model.Add(data.TotalCOD);
            model.Add(data.OrderValue);
            model.Add(data.Note);
            model.Add(data.TransportMainService);
            model.Add(data.TransportExtraService);
            model.Add(data.SenderName);
            model.Add(data.SenderMobileNumber);
            model.Add(data.SenderStreet);
            model.Add(data.SenderProvinceName);
            //model.Add(data.SenderDistrictName);
            model.Add(data.SenderWardName);
            model.Add(data.CollectionType);
            model.Add(data.TransportPayer);
            model.Add(data.Long);
            model.Add(data.Width);
            model.Add(data.Height);
            model.Add(data.Channel);
            model.Add(data.TotalPHU);
            model.Add(data.ProvinceID);
            //model.Add(data.DistrictID);
            model.Add(data.WardID);
            model.Add(data.SenderProvinceID);
            //model.Add(data.SenderDistrictID);
            model.Add(data.SenderWardID);
            model.Add(Message);
            return model;
        }

        public ActionResult JExportExcel(List<ExportExcelOrderModel> ListData)
        {
            var FullPath = System.Web.HttpContext.Current.Server.MapPath("~/Assets/temp/MauImportDonHang.xlsm");
            var fileNameNew = Guid.NewGuid().ToString() + ".xlsm";
            var FullPathNew = System.Web.HttpContext.Current.Server.MapPath("~/Assets/temp/" + fileNameNew);

            var fileExt = Path.GetExtension(FullPath);
            HSSFWorkbook xls = new HSSFWorkbook();
            XSSFWorkbook xlsx = new XSSFWorkbook();
            ISheet sheet;
            try
            {
                using (var fs = new FileStream(FullPath, FileMode.Open, FileAccess.ReadWrite))
                {
                    if (fileExt == ".xls")
                    {
                        xls = new HSSFWorkbook(fs);
                        sheet = xls.GetSheetAt(0);
                    }
                    else
                    {
                        xlsx = new XSSFWorkbook(fs);
                        sheet = xlsx.GetSheetAt(0);
                    }

                    var totalRow = sheet.LastRowNum;
                    //var totalList = 0;
                    int rowIndex = 4;
                    int stt = 1;
                    // create bordered cell style
                    XSSFCellStyle borderedCellStyle = (XSSFCellStyle)xlsx.CreateCellStyle();
                    borderedCellStyle.BorderLeft = BorderStyle.Thin;
                    borderedCellStyle.BorderTop = BorderStyle.Thin;
                    borderedCellStyle.BorderRight = BorderStyle.Thin;
                    borderedCellStyle.BorderBottom = BorderStyle.Thin;

                    sheet.GetRow(2).CreateCell(28).SetCellValue("Nguyên nhân lỗi");
                    sheet.GetRow(2).GetCell(28).CellStyle = borderedCellStyle;

                    foreach (var item in ListData)
                    {
                        // tao row mới
                        var newRow = sheet.CreateRow(rowIndex);
                        // set giá trị
                        newRow.CreateCell(0).SetCellValue(stt++);
                        newRow.CreateCell(1).SetCellValue(item.Product);
                        newRow.CreateCell(2).SetCellValue(item.OrderCode);
                        newRow.CreateCell(3).SetCellValue(item.FullName);
                        newRow.CreateCell(4).SetCellValue(item.MobileNumber);
                        newRow.CreateCell(5).SetCellValue(item.Street);
                        newRow.CreateCell(6).SetCellValue(item.ProvinceName);
                        //newRow.CreateCell(7).SetCellValue(item.DistrictName);
                        newRow.CreateCell(7).SetCellValue(item.WardName);
                        newRow.CreateCell(8).SetCellValue(item.Message);
                        newRow.CreateCell(9).SetCellValue(item.NumberOfProduct);
                        newRow.CreateCell(10).SetCellValue(item.Weight);
                        newRow.CreateCell(11).SetCellValue(item.TotalCOD);
                        newRow.CreateCell(12).SetCellValue(item.OrderValue);
                        newRow.CreateCell(13).SetCellValue(item.Note);
                        newRow.CreateCell(14).SetCellValue(item.TransportMainService);
                        newRow.CreateCell(15).SetCellValue(item.TransportExtraService);
                        newRow.CreateCell(16).SetCellValue(item.SenderName);
                        newRow.CreateCell(17).SetCellValue(item.SenderMobileNumber);
                        newRow.CreateCell(18).SetCellValue(item.SenderStreet);
                        newRow.CreateCell(19).SetCellValue(item.SenderProvinceName);
                        //newRow.CreateCell(21).SetCellValue(item.SenderDistrictName);
                        newRow.CreateCell(20).SetCellValue(item.SenderWardName);
                        newRow.CreateCell(21).SetCellValue(item.CollectionType);
                        newRow.CreateCell(22).SetCellValue(item.TransportPayer);
                        newRow.CreateCell(23).SetCellValue(item.Long);
                        newRow.CreateCell(24).SetCellValue(item.Width);
                        newRow.CreateCell(25).SetCellValue(item.Height);
                        newRow.CreateCell(26).SetCellValue(item.Channel);
                        newRow.CreateCell(27).SetCellValue(item.TotalPHU);
                        newRow.CreateCell(28).SetCellValue(item.MessageError);

                        for (int i = 0; i <= 28; i++)
                        {
                            newRow.GetCell(i).CellStyle = borderedCellStyle;
                        }
                        // tăng index
                        rowIndex++;
                    }
                }

                using (var fs = new FileStream(FullPathNew, FileMode.Create, FileAccess.ReadWrite))
                {
                    if (fileExt == ".xls")
                    {
                        xls.Write(fs);
                    }
                    else
                    {
                        xlsx.Write(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JExportExcel: " + ex.Message);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            if (fileExt == ".xls")
            {
                xls.Write(output);
            }
            else
            {
                xlsx.Write(output);
            }

            //Delete file
            XCommon.DeleteFile(fileNameNew);

            //Return the result to the end user
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "MauImportDonHang");
        }
        #endregion

        #region Duyệt đơn hàng
        [AuthorizeController(FunctionalTitle = "DUYET_DON_HANG")]
        public JsonResult JNewOrder(List<SearchListOrderViewModel> req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (req.Count > 0)
                {
                    List<ZOrderMultipleNewStatusOrderInfoRequest> ListOrderInfo = new List<ZOrderMultipleNewStatusOrderInfoRequest>();
                    foreach (var item in req)
                    {
                        ListOrderInfo.Add(new ZOrderMultipleNewStatusOrderInfoRequest()
                        {
                            OrderID = item.OrderID,
                            OrderCode = item.OrderCode,
                            BatchCode = item.BatchCode,
                            OrderStatus = item.OrderStatus
                        });
                    }

                    res = OrderRepository.NewStatus(new ZOrderMultipleNewStatusRequest()
                    {
                        ListOrderInfo = ListOrderInfo,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    });
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn đơn hàng";
                }

                //res = OrderRepository.NewStatus(new ZOrderMultipleNewStatusRequest()
                //{
                //    strID = string.Join(",", req.Select(m => m.OrderID)),
                //    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                //    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                //});
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JNewOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Hủy đơn hàng
        [AuthorizeController(FunctionalTitle = "HUY_DON_HANG")]
        public ActionResult RejectOrderView(OrderViewModel req)
        {
            RejectOrderViewModel model = new RejectOrderViewModel()
            {
                ListOrderStringfy = req.SearchKey
            };
            return PartialView(model);
        }

        public JsonResult JRejectOrder(RejectOrderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (string.IsNullOrEmpty(req.ListOrderStringfy) == false)
                {
                    var ListOrder = JsonConvert.DeserializeObject<List<SearchListOrderViewModel>>(req.ListOrderStringfy);

                    if (ListOrder.Count > 0)
                    {
                        List<ZOrderMultipleCancelStatusOrderInfoRequest> ListOrderInfo = new List<ZOrderMultipleCancelStatusOrderInfoRequest>();
                        foreach (var item in ListOrder)
                        {
                            ListOrderInfo.Add(new ZOrderMultipleCancelStatusOrderInfoRequest()
                            {
                                OrderID = item.OrderID,
                                OrderCode = item.OrderCode,
                                BatchCode = item.BatchCode,
                                OrderStatus = item.OrderStatus
                            });
                        }

                        res = OrderRepository.CancelStatus(new ZOrderMultipleCancelStatusRequest()
                        {
                            ListOrderInfo = ListOrderInfo,
                            ReasonCancel = req.Reason,
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                            UpdatedBy = XHelper.Session.EmployeeProfile.ID
                        });
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Vui lòng chọn đơn hàng";
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn đơn hàng";
                }

                //if (string.IsNullOrEmpty(req.OrderID))
                //{
                //    res.Code = "01";
                //    res.Message = "Vui lòng chọn đơn hàng";
                //}
                //else
                //{
                //    List<ZErrorDataListResponse> ListError = new List<ZErrorDataListResponse>();
                //    var splitOrderID = req.OrderID.Split(',');
                //    for (var i = 0; i < splitOrderID.Length; i++)
                //    {
                //        res = OrderRepository.CancelStatus(new ZOrderCancelStatusRequest()
                //        {
                //            OrderID = Convert.ToInt32(splitOrderID[i]),
                //            ReasonCancel = req.Reason,
                //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                //            UpdatedBy = XHelper.Session.EmployeeProfile.ID
                //        });

                //        if (res.Code != "00")
                //        {
                //            var data = JsonConvert.DeserializeObject<ZOrderCancelStatusResponse>(res.Data);
                //            ListError.Add(new ZErrorDataListResponse()
                //            {
                //                OrderCode = data.OrderCode,
                //                Description = data.Description
                //            });
                //        }
                //    }

                //    if (ListError.Count > 0)
                //    {
                //        var TotalSuccess = splitOrderID.Length - ListError.Count;
                //        res.Code = "10";
                //        res.Message = string.Format("Hủy đơn hàng thành công {0}/{1} đơn hàng, vui lòng nhấn vào Xem chi tiết để kiểm tra.", TotalSuccess, splitOrderID.Length);
                //        res.Data = JsonConvert.SerializeObject(ListError);
                //    }
                //    else
                //    {
                //        res.Code = "00";
                //        res.Message = "Thành công";
                //    }
                //}
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JRejectOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region In đơn hàng
        [AuthorizeController(FunctionalTitle = "IN_DON_HANG")]
        public ActionResult PrintOrderView()
        {
            PrintOrderViewModel model = new PrintOrderViewModel()
            {
                PrintTypeDefault = XHelper.Session.EmployeeProfile.ShopInformation.PrintTypeDefault
            };
            return PartialView(model);
        }

        public ActionResult PrintSingleOrderView(string ShippingCodes, string CrmOrPayPostCode, string PrintType)
        {
            ViewBag.ShippingCodes = ShippingCodes;
            ViewBag.CrmOrPayPostCode = CrmOrPayPostCode;
            ViewBag.PrintType = PrintType;
            return PartialView();
        }

        public ActionResult PrintOrderTypeView(PrintOrderViewModel req)
        {
            List<PrintOrderTypeViewModel> model = new List<PrintOrderTypeViewModel>();
            var ListOrderPrint = XCommon.ListPrintOrder(req);
            if (ListOrderPrint.Count > 0)
            {
                ViewBag.OrderIDs = string.Join(",", ListOrderPrint.Select(m => m.OrderID));
                if (req.PrintType == "A4" || req.PrintType == "A61" || req.PrintType == "B1" || req.PrintType == "B2" || req.PrintType == "Label" || req.PrintType == "E5")
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
                                BatchCode = ListOrderPrint[j].BatchCode,
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

        public ActionResult PrintSingleOrderTypeView(PrintSingleOrderViewModel req)
        {
            var listShippingCode = req.ShippingCodes.Split(',')
                .Select(x => x.Trim())
                .ToList();
            List<PrintOrderTypeDataModel> ListOrderPrint = new List<PrintOrderTypeDataModel>();

            foreach(var shippingcode in listShippingCode)
            {
                ListOrderPrint.Add(XCommon.SinglePrintOrder(new PrintSingleOrderViewModel 
                {
                    ShippingCodes = shippingcode,
                    CrmOrPayPostCode = req.CrmOrPayPostCode,
                    PrintType = req.PrintType
                }));
            }
            

            List<PrintOrderTypeViewModel> model = new List<PrintOrderTypeViewModel>();

            if(ListOrderPrint.Count > 0)
            {
                ViewBag.OrderIDs = string.Join(",", ListOrderPrint.Select(m => m.OrderID));
                if(req.PrintType == "A4" || req.PrintType == "A61" || req.PrintType == "B1" || req.PrintType == "B2" || req.PrintType == "Label")
                {
                    model.Add(new PrintOrderTypeViewModel()
                    {
                        ListOrderPrint = ListOrderPrint
                    });
                }
                else if(req.PrintType == "A64")
                {
                    var TotalPage = (ListOrderPrint.Count / 4) + 1;
                    var count = 0;
                    var index = 0;
                    for(var i = 1; i <= TotalPage; i++)
                    {
                        List<PrintOrderTypeDataModel> ListTemp = new List<PrintOrderTypeDataModel>();
                        for(var j = index; j < ListOrderPrint.Count; j++)
                        {
                            count++;
                            if(count > 4)
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
                        if(ListTemp.Count > 0)
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
            return PartialView(model);
        }

        public JsonResult JUpdatePrintStatus(string IDs)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = OrderRepository.UpdatePrintStatus(new ZOrderUpdatePrintStatusRequest() { IDs = IDs, UpdatedBy = XHelper.Session.EmployeeProfile.ID });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JUpdatePrintStatus: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gửi yêu cầu vận chuyển
        [AuthorizeController(FunctionalTitle = "GUI_YEU_CAU_VAN_CHUYEN")]
        public JsonResult JSendTransport(List<SearchListOrderViewModel> req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (req.Count > 0)
                {
                    List<ZOrderMultipleSendTransportOrderInfoRequest> ListOrderInfo = new List<ZOrderMultipleSendTransportOrderInfoRequest>();
                    foreach (var item in req)
                    {
                        ListOrderInfo.Add(new ZOrderMultipleSendTransportOrderInfoRequest()
                        {
                            OrderID = item.OrderID,
                            OrderCode = item.OrderCode,
                            BatchCode = item.BatchCode,
                            OrderStatus = item.OrderStatus
                        });
                    }

                    res = OrderRepository.SendTransport(new ZOrderMultipleSendTransportRequest()
                    {
                        ListOrderInfo = ListOrderInfo,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    });
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn đơn hàng";
                }

                //List<ZErrorDataListResponse> ListItem = new List<ZErrorDataListResponse>();
                //foreach (var item in req)
                //{
                //    if (item.OrderStatus != "N" && item.OrderStatus != "L")
                //    {
                //        ListItem.Add(new ZErrorDataListResponse()
                //        {
                //            OrderCode = item.EMSOneCode,
                //            Description = string.Format("Đơn hàng đang ở trạng thái {0}.", XData.GetNameOrderStatus(item.OrderStatus))
                //        });
                //    }
                //    else
                //    {
                //        res = OrderRepository.SendTransport(new ZOrderSendRequestTransportRequest()
                //        {
                //            EMSOneCode = item.EMSOneCode,
                //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                //            UpdatedBy = XHelper.Session.EmployeeProfile.ID
                //        });
                //        if (res.Code != "00")
                //        {
                //            ListItem.Add(new ZErrorDataListResponse()
                //            {
                //                OrderCode = item.EMSOneCode,
                //                Description = res.Message
                //            });
                //        }
                //    }
                //}
                //if (ListItem.Count > 0)
                //{
                //    var TotalSuccess = req.Count - ListItem.Count;
                //    res.Code = "10";
                //    res.Message = string.Format("Gửi đơn vị vận chuyển thành công {0}/{1} đơn hàng, vui lòng nhấn vào Xem chi tiết để kiểm tra.", TotalSuccess, req.Count);
                //    res.Data = JsonConvert.SerializeObject(ListItem);
                //}
                //else
                //{
                //    res.Code = "00";
                //    res.Message = "Gửi đơn vị vận chuyển thành công.";
                //}
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JSendTransport: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Hủy yêu cầu vận chuyển
        [AuthorizeController(FunctionalTitle = "HUY_YEU_CAU_VAN_CHUYEN")]
        public JsonResult JCancelTransport(List<SearchListOrderViewModel> req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (req.Count > 0)
                {
                    List<ZOrderMultipleCancelTransportOrderInfoRequest> ListOrderInfo = new List<ZOrderMultipleCancelTransportOrderInfoRequest>();
                    foreach (var item in req)
                    {
                        ListOrderInfo.Add(new ZOrderMultipleCancelTransportOrderInfoRequest()
                        {
                            OrderID = item.OrderID,
                            OrderCode = item.OrderCode,
                            BatchCode = item.BatchCode,
                            OrderStatus = item.OrderStatus
                        });
                    }

                    res = OrderRepository.CancelTransport(new ZOrderMultipleCancelTransportRequest()
                    {
                        ListOrderInfo = ListOrderInfo,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    });
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn đơn hàng";
                }

                //if (string.IsNullOrEmpty(req.SearchKey))
                //{
                //    res.Code = "01";
                //    res.Message = "Vui lòng chọn đơn hàng";
                //}
                //else
                //{
                //    List<ZErrorDataListResponse> ListError = new List<ZErrorDataListResponse>();
                //    var splitOrderID = req.SearchKey.Split(',');
                //    for (var i = 0; i < splitOrderID.Length; i++)
                //    {
                //        res = OrderRepository.CancelTransport(new ZOrderCancelRequestTransportRequest()
                //        {
                //            OrderID = Convert.ToInt32(splitOrderID[i]),
                //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                //            UpdatedBy = XHelper.Session.EmployeeProfile.ID
                //        });

                //        if (res.Code != "00")
                //        {
                //            var data = JsonConvert.DeserializeObject<ZOrderCancelStatusResponse>(res.Data);
                //            ListError.Add(new ZErrorDataListResponse()
                //            {
                //                OrderCode = data.OrderCode,
                //                Description = data.Description
                //            });
                //        }
                //    }

                //    if (ListError.Count > 0)
                //    {
                //        var TotalSuccess = splitOrderID.Length - ListError.Count;
                //        res.Code = "10";
                //        res.Message = string.Format("Huỷ yêu cầu vận chuyển thành công {0}/{1} đơn hàng, vui lòng nhấn vào Xem chi tiết để kiểm tra.", TotalSuccess, splitOrderID.Length);
                //        res.Data = JsonConvert.SerializeObject(ListError);
                //    }
                //    else
                //    {
                //        res.Code = "00";
                //        res.Message = "Huỷ yêu cầu vận chuyển thành công.";
                //    }
                //}
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderController.JCancelTransport: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet); ;
        }
        #endregion
    }
}