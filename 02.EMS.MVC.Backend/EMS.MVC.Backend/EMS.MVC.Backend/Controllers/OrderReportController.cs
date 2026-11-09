using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Controllers
{
    public class OrderReportController : BaseController
    {
        // GET: OrderReport
        public ActionResult OrderReportView()
        {
            OrderReportViewModel model = new OrderReportViewModel()
            {
                Time = DateTime.Now.ToString("MM/yyyy"),
                ListPlaceOfCreation = new List<SelectListItem>()
                {
                    new SelectListItem(){ Value = "EMSONE", Text = "EMSOne" },
                    new SelectListItem(){ Value = "EMSENTERPRISE", Text = "EMS Enterprise"},
                    new SelectListItem(){ Value = "PARTNER", Text = "API Đối tác"},
                },
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListPartner = XCommon.SearchListPartner(new ZPartnerSearchListRequest() { IsLock = "N" })
            };
            return View(model);
        }

        public ActionResult SearchListOrderReportView(OrderReportViewModel req)
        {
            SearchOrderReportViewModel model = null;
            List<SearchOrderReportDataViewModel> ListItem = new List<SearchOrderReportDataViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "ORDER_REPORT_LIST",
                Data = JsonConvert.SerializeObject(new ZOrderReportRequest
                {
                    Time = req.Time,
                    SearchKey = req.SearchKey,
                    PlaceOfCreation = req.PlaceOfCreation,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    PartnerID = req.PartnerID
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZOrderReportResponse>>(res.Data);
                foreach (var item in list)
                {
                    ListItem.Add(new SearchOrderReportDataViewModel()
                    {
                        StoreName = item.StoreName,
                        CustomerCode = item.CustomerCode,
                        MobileNumber = item.MobileNumber,
                        ConfirmDate = item.ConfirmDate,
                        AreaName = item.AreaName,
                        OrderMonthQuantity = item.OrderMonthQuantity,
                        OrderMonthTurnover = item.OrderMonthTurnover,
                        PartnerID = item.PartnerID,
                        PartnerName = item.PartnerName
                    });
                }
                model = new SearchOrderReportViewModel()
                {
                    SumOrderMonthQuantity = ListItem.Sum(m => m.OrderMonthQuantity),
                    SumOrderMonthTurnover = ListItem.Sum(m => m.OrderMonthTurnover),
                    Data = ListItem
                };
            }
            return PartialView(model);
        }

        public ActionResult ExportExcelOrderReport(OrderReportViewModel req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            ISheet sheet = wb.CreateSheet("BÁO CÁO SẢN LƯỢNG");

            #region SetColumnWidth
            sheet.SetColumnWidth(0, 6000);
            sheet.SetColumnWidth(1, 12000);
            sheet.SetColumnWidth(2, 6000);
            sheet.SetColumnWidth(3, 6000);
            sheet.SetColumnWidth(4, 6000);
            sheet.SetColumnWidth(5, 10000);
            sheet.SetColumnWidth(6, 10000);
            sheet.SetColumnWidth(7, 6000);
            sheet.SetColumnWidth(8, 6000);
            #endregion

            #region Style
            HSSFFont _fontHeader = (HSSFFont)wb.CreateFont();
            _fontHeader.FontName = "Times New Roman";
            _fontHeader.FontHeightInPoints = 15; //Font size
            _fontHeader.IsBold = true;
            HSSFCellStyle _styleFontHeader = (HSSFCellStyle)wb.CreateCellStyle();
            _styleFontHeader.Alignment = HorizontalAlignment.Center;
            _styleFontHeader.SetFont(_fontHeader);

            HSSFFont _fontDescribe = (HSSFFont)wb.CreateFont();
            _fontDescribe.FontName = "Times New Roman";
            _fontDescribe.FontHeightInPoints = 13; //Font size
            HSSFCellStyle _styleFontDescribe = (HSSFCellStyle)wb.CreateCellStyle();
            _styleFontDescribe.SetFont(_fontDescribe);

            HSSFFont _font = (HSSFFont)wb.CreateFont();
            _font.FontHeightInPoints = 13;
            _font.FontName = "Times New Roman";
            HSSFCellStyle borderedCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
            borderedCellStyle.SetFont(_font);
            borderedCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            borderedCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            borderedCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            borderedCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            #endregion

            #region Header
            var row0 = sheet.CreateRow(0);
            row0.CreateCell(0);
            CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 8);
            sheet.AddMergedRegion(cellMerge);
            row0.GetCell(0).SetCellValue("BÁO CÁO SẢN LƯỢNG");
            row0.GetCell(0).CellStyle = _styleFontHeader;

            var row2 = sheet.CreateRow(2);
            row2.CreateCell(0);
            row2.GetCell(0).SetCellValue("THỜI GIAN: ");
            row2.GetCell(0).CellStyle = _styleFontDescribe;
            string title = "";
            if (req.Time != null)
            {
                title = "THÁNG " + req.Time;
            }
            else
            {
                title = req.FromDate + " - " + req.ToDate;
            }
            row2.CreateCell(1);
            row2.GetCell(1).SetCellValue(title);
            row2.GetCell(1).CellStyle = _styleFontDescribe;

            var row3 = sheet.CreateRow(3);
            row3.CreateCell(0);
            row3.GetCell(0).SetCellValue("MÃ PP/CRM - SĐT: ");
            row3.GetCell(0).CellStyle = _styleFontDescribe;
            row3.CreateCell(1);
            row3.GetCell(1).SetCellValue(req.SearchKey);
            row3.GetCell(1).CellStyle = _styleFontDescribe;

            var row4 = sheet.CreateRow(4);
            row4.CreateCell(0);
            row4.GetCell(0).SetCellValue("NƠI TẠO: ");
            row4.GetCell(0).CellStyle = _styleFontDescribe;
            string titlePlaceOfCreationName = string.IsNullOrEmpty(req.PlaceOfCreationName) ? "TẤT CẢ" : req.PlaceOfCreationName.ToUpper();
            row4.CreateCell(1);
            row4.GetCell(1).SetCellValue(titlePlaceOfCreationName);
            row4.GetCell(1).CellStyle = _styleFontDescribe;

            var row5 = sheet.CreateRow(5);
            row5.CreateCell(0);
            row5.GetCell(0).SetCellValue("ĐỐI TÁC: ");
            row5.GetCell(0).CellStyle = _styleFontDescribe;
            string titlePartnerName = string.IsNullOrEmpty(req.PartnerName) ? "TẤT CẢ" : req.PartnerName.ToUpper();
            row5.CreateCell(1);
            row5.GetCell(1).SetCellValue(titlePartnerName);
            row5.GetCell(1).CellStyle = _styleFontDescribe;
            #endregion

            #region Title Table
            var row8 = sheet.CreateRow(8);
            var cell_0_Row8 = row8.CreateCell(0);
            cell_0_Row8.SetCellValue("STT");
            cell_0_Row8.CellStyle = borderedCellStyle;
            var cell_1_Row8 = row8.CreateCell(1);
            cell_1_Row8.SetCellValue("Tên cửa hàng");
            cell_1_Row8.CellStyle = borderedCellStyle;
            var cell_2_Row8 = row8.CreateCell(2);
            cell_2_Row8.SetCellValue("Mã PP/CRM");
            cell_2_Row8.CellStyle = borderedCellStyle;
            var cell_3_Row8 = row8.CreateCell(3);
            cell_3_Row8.SetCellValue("Số điện thoại");
            cell_3_Row8.CellStyle = borderedCellStyle;
            var cell_4_Row8 = row8.CreateCell(4);
            cell_4_Row8.SetCellValue("Ngày tạo tài khoản");
            cell_4_Row8.CellStyle = borderedCellStyle;
            var cell_5_Row8 = row8.CreateCell(5);
            cell_5_Row8.SetCellValue("Đơn vị");
            cell_5_Row8.CellStyle = borderedCellStyle;
            var cell_6_Row8 = row8.CreateCell(6);
            cell_6_Row8.SetCellValue("Đối tác");
            cell_6_Row8.CellStyle = borderedCellStyle;
            var cell_7_Row8 = row8.CreateCell(7);
            cell_7_Row8.SetCellValue("Sản lượng phát sinh");
            cell_7_Row8.CellStyle = borderedCellStyle;
            var cell_8_Row8 = row8.CreateCell(8);
            cell_8_Row8.SetCellValue("Doanh thu phát sinh");
            cell_8_Row8.CellStyle = borderedCellStyle;
            #endregion

            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "ORDER_REPORT_LIST",
                Data = JsonConvert.SerializeObject(new ZOrderReportRequest
                {
                    Time = req.Time,
                    SearchKey = req.SearchKey,
                    PlaceOfCreation = req.PlaceOfCreation,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    PartnerID = req.PartnerID
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZOrderReportResponse>>(res.Data);
                int stt = 1;

                var row7 = sheet.CreateRow(7);
                var cell_0_Row7 = row7.CreateCell(0);
                cell_0_Row7.SetCellValue("Tổng dữ liệu");
                cell_0_Row7.CellStyle = borderedCellStyle;
                var cell_1_Row7 = row7.CreateCell(1);
                cell_1_Row7.CellStyle = borderedCellStyle;
                var cell_2_Row7 = row7.CreateCell(2);
                cell_2_Row7.CellStyle = borderedCellStyle;
                var cell_3_Row7 = row7.CreateCell(3);
                cell_3_Row7.CellStyle = borderedCellStyle;
                var cell_4_Row7 = row7.CreateCell(4);
                cell_4_Row7.CellStyle = borderedCellStyle;
                var cell_5_Row7 = row7.CreateCell(5);
                cell_5_Row7.CellStyle = borderedCellStyle;
                var cell_6_Row7 = row7.CreateCell(6);
                cell_6_Row7.CellStyle = borderedCellStyle;
                var cell_7_Row7 = row7.CreateCell(7);
                cell_7_Row7.SetCellValue(list.Sum(m => m.OrderMonthQuantity) == 0 ? "0" : double.Parse(list.Sum(m => m.OrderMonthQuantity).ToString()).ToString("#,###", cul.NumberFormat));
                cell_7_Row7.CellStyle = borderedCellStyle;
                var cell_8_Row7 = row7.CreateCell(8);
                cell_8_Row7.SetCellValue(list.Sum(m => m.OrderMonthTurnover) == 0 ? "0" : double.Parse(list.Sum(m => m.OrderMonthTurnover).ToString()).ToString("#,###", cul.NumberFormat));
                cell_8_Row7.CellStyle = borderedCellStyle;

                int rowIndex = 9;
                foreach (var item in list)
                {
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    var cell_0_newRow = newRow.CreateCell(0);
                    cell_0_newRow.SetCellValue(stt++);
                    cell_0_newRow.CellStyle = borderedCellStyle;

                    var cell_1_newRow = newRow.CreateCell(1);
                    cell_1_newRow.SetCellValue(item.StoreName);
                    cell_1_newRow.CellStyle = borderedCellStyle;

                    var cell_2_newRow = newRow.CreateCell(2);
                    cell_2_newRow.SetCellValue(item.CustomerCode);
                    cell_2_newRow.CellStyle = borderedCellStyle;


                    var cell_3_newRow = newRow.CreateCell(3);
                    cell_3_newRow.SetCellValue(item.MobileNumber);
                    cell_3_newRow.CellStyle = borderedCellStyle;

                    var cell_4_newRow = newRow.CreateCell(4);
                    cell_4_newRow.SetCellValue(item.ConfirmDate);
                    cell_4_newRow.CellStyle = borderedCellStyle;

                    var cell_5_newRow = newRow.CreateCell(5);
                    cell_5_newRow.SetCellValue(item.AreaName);
                    cell_5_newRow.CellStyle = borderedCellStyle;

                    var cell_6_newRow = newRow.CreateCell(6);
                    cell_6_newRow.SetCellValue(item.PartnerName);
                    cell_6_newRow.CellStyle = borderedCellStyle;

                    var cell_7_newRow = newRow.CreateCell(7);
                    cell_7_newRow.SetCellValue(item.OrderMonthQuantity == 0 ? "0" : double.Parse(item.OrderMonthQuantity.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_7_newRow.CellStyle = borderedCellStyle;

                    var cell_8_newRow = newRow.CreateCell(8);
                    cell_8_newRow.SetCellValue(item.OrderMonthTurnover == 0 ? "0" : double.Parse(item.OrderMonthTurnover.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_8_newRow.CellStyle = borderedCellStyle;
                    rowIndex++;
                }
            }
            else
            {
                var row7 = sheet.CreateRow(7);
                var cell_0_Row7 = row7.CreateCell(0);
                cell_0_Row7.SetCellValue("Tổng dữ liệu");
                cell_0_Row7.CellStyle = borderedCellStyle;
                var cell_1_Row7 = row7.CreateCell(1);
                cell_1_Row7.CellStyle = borderedCellStyle;
                var cell_2_Row7 = row7.CreateCell(2);
                cell_2_Row7.CellStyle = borderedCellStyle;
                var cell_3_Row7 = row7.CreateCell(3);
                cell_3_Row7.CellStyle = borderedCellStyle;
                var cell_4_Row7 = row7.CreateCell(4);
                cell_4_Row7.CellStyle = borderedCellStyle;
                var cell_5_Row7 = row7.CreateCell(5);
                cell_5_Row7.CellStyle = borderedCellStyle;
                var cell_6_Row7 = row7.CreateCell(6);
                cell_6_Row7.CellStyle = borderedCellStyle;
                var cell_7_Row7 = row7.CreateCell(7);
                cell_7_Row7.SetCellValue("0");
                cell_7_Row7.CellStyle = borderedCellStyle;
                var cell_8_Row7 = row7.CreateCell(8);
                cell_8_Row7.SetCellValue("0");
                cell_8_Row7.CellStyle = borderedCellStyle;
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachSanLuong_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls");
        }

        public ActionResult OrderDetailReportView(string SearchDetail, string PlaceOfCreationName)
        {
            OrderDetailReportViewModel model = new OrderDetailReportViewModel();
            string str = Security.DecodeBase64(SearchDetail);
            string[] strArray = str.Split('&');
            for(var i = 0; i < strArray.Length; i++)
            {
                switch (strArray[i].Split('=')[0])
                {
                    case "Time":
                        model.Time = strArray[i].Split('=')[1];
                        break;
                    case "FromDate":
                        model.FromDate = strArray[i].Split('=')[1];
                        break;
                    case "ToDate":
                        model.ToDate = strArray[i].Split('=')[1];
                        break;
                    case "PlaceOfCreation":
                        model.PlaceOfCreation = strArray[i].Split('=')[1];
                        break;
                    case "SearchKey":
                        model.SearchKey = strArray[i].Split('=')[1];
                        break;
                    default:
                        break;
                }
            }
            model.PlaceOfCreationName = string.IsNullOrEmpty(PlaceOfCreationName) ? "Tất cả" : PlaceOfCreationName;
            return View(model);
        }

        public ActionResult SearchListOrderDetailReportView(OrderDetailReportViewModel req)
        {
            SearchOrderDetailReportViewModel model = null;
            List<SearchOrderDetailReportDataViewModel> ListItem = new List<SearchOrderDetailReportDataViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "ORDER_REPORT_DETAIL_LIST",
                Data = JsonConvert.SerializeObject(new ZOrderDetailReportRequest
                {
                    Time = req.Time,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    PlaceOfCreation = req.PlaceOfCreation,
                    SearchKey = req.SearchKey
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZOrderDetailReportResponse>>(res.Data);
                foreach (var item in list)
                {
                    ListItem.Add(new SearchOrderDetailReportDataViewModel()
                    {
                        ItemCode = item.ItemCode,
                        OrderCreatedDate = item.OrderCreatedDate,
                        TempTransportFee = item.TempTransportFee,
                        TransportFee = item.TransportFee,
                        CODMoney = item.CODMoney,
                        PlaceOfOrderCreation = item.PlaceOfOrderCreation,
                        StatusName = item.StatusName
                    });
                }
                model = new SearchOrderDetailReportViewModel()
                {
                    SumTempTransportFee = ListItem.Sum(m => m.TempTransportFee),
                    SumTransportFee = ListItem.Sum(m => m.TransportFee),
                    SumCODMoney = ListItem.Sum(m => m.CODMoney),
                    Data = ListItem
                };
            }
            return PartialView(model);
        }

        public ActionResult ExportExcelOrderDetailReport(OrderDetailReportViewModel req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            ISheet sheet = wb.CreateSheet("BÁO CÁO CHI TIẾT SẢN LƯỢNG");

            #region SetColumnWidth
            sheet.SetColumnWidth(0, 6000);
            sheet.SetColumnWidth(1, 8500);
            sheet.SetColumnWidth(2, 6000);
            sheet.SetColumnWidth(3, 6000);
            sheet.SetColumnWidth(4, 6000);
            sheet.SetColumnWidth(5, 6000);
            sheet.SetColumnWidth(6, 6000);
            sheet.SetColumnWidth(7, 6000);
            #endregion

            #region Style
            HSSFFont _fontHeader = (HSSFFont)wb.CreateFont();
            _fontHeader.FontName = "Times New Roman";
            _fontHeader.FontHeightInPoints = 15; //Font size
            _fontHeader.IsBold = true;
            HSSFCellStyle _styleFontHeader = (HSSFCellStyle)wb.CreateCellStyle();
            _styleFontHeader.Alignment = HorizontalAlignment.Center;
            _styleFontHeader.SetFont(_fontHeader);

            HSSFFont _fontDescribe = (HSSFFont)wb.CreateFont();
            _fontDescribe.FontName = "Times New Roman";
            _fontDescribe.FontHeightInPoints = 13; //Font size
            HSSFCellStyle _styleFontDescribe = (HSSFCellStyle)wb.CreateCellStyle();
            _styleFontDescribe.SetFont(_fontDescribe);

            HSSFFont _font = (HSSFFont)wb.CreateFont();
            _font.FontHeightInPoints = 13;
            _font.FontName = "Times New Roman";
            HSSFCellStyle borderedCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
            borderedCellStyle.SetFont(_font);
            borderedCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            borderedCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            borderedCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            borderedCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            #endregion

            #region Header
            var row0 = sheet.CreateRow(0);
            row0.CreateCell(0);
            CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 7);
            sheet.AddMergedRegion(cellMerge);
            row0.GetCell(0).SetCellValue("BÁO CÁO CHI TIẾT SẢN LƯỢNG");
            row0.GetCell(0).CellStyle = _styleFontHeader;

            var row2 = sheet.CreateRow(2);
            row2.CreateCell(0);
            row2.GetCell(0).SetCellValue("THỜI GIAN: ");
            row2.GetCell(0).CellStyle = _styleFontDescribe;
            string title = "";
            if (req.Time != null)
            {
                title = "THÁNG " + req.Time;
            }
            else
            {
                title = req.FromDate + " - " + req.ToDate;
            }
            row2.CreateCell(1);
            row2.GetCell(1).SetCellValue(title);
            row2.GetCell(1).CellStyle = _styleFontDescribe;

            var row3 = sheet.CreateRow(3);
            row3.CreateCell(0);
            row3.GetCell(0).SetCellValue("MÃ PP/CRM - SĐT: ");
            row3.GetCell(0).CellStyle = _styleFontDescribe;
            row3.CreateCell(1);
            row3.GetCell(1).SetCellValue(req.SearchKey.ToUpper());
            row3.GetCell(1).CellStyle = _styleFontDescribe;

            var row4 = sheet.CreateRow(4);
            row4.CreateCell(0);
            row4.GetCell(0).SetCellValue("NƠI TẠO: ");
            row4.GetCell(0).CellStyle = _styleFontDescribe;
            string titlePlaceOfCreationName = string.IsNullOrEmpty(req.PlaceOfCreationName) ? "TẤT CẢ" : req.PlaceOfCreationName.ToUpper();
            row4.CreateCell(1);
            row4.GetCell(1).SetCellValue(titlePlaceOfCreationName);
            row4.GetCell(1).CellStyle = _styleFontDescribe;
            #endregion

            #region Title Table
            var row7 = sheet.CreateRow(7);
            var cell_0_Row7 = row7.CreateCell(0);
            cell_0_Row7.SetCellValue("STT");
            cell_0_Row7.CellStyle = borderedCellStyle;
            var cell_1_Row7 = row7.CreateCell(1);
            cell_1_Row7.SetCellValue("Mã bưu gửi");
            cell_1_Row7.CellStyle = borderedCellStyle;
            var cell_2_Row7 = row7.CreateCell(2);
            cell_2_Row7.SetCellValue("Ngày tạo đơn");
            cell_2_Row7.CellStyle = borderedCellStyle;
            var cell_3_Row7 = row7.CreateCell(3);
            cell_3_Row7.SetCellValue("Cước tạm tính");
            cell_3_Row7.CellStyle = borderedCellStyle;
            var cell_4_Row7 = row7.CreateCell(4);
            cell_4_Row7.SetCellValue("Cước thực tế");
            cell_4_Row7.CellStyle = borderedCellStyle;
            var cell_5_Row7 = row7.CreateCell(5);
            cell_5_Row7.SetCellValue("Tiền COD");
            cell_5_Row7.CellStyle = borderedCellStyle;
            var cell_6_Row7 = row7.CreateCell(6);
            cell_6_Row7.SetCellValue("Nơi tạo đơn");
            cell_6_Row7.CellStyle = borderedCellStyle;
            var cell_7_Row7 = row7.CreateCell(7);
            cell_7_Row7.SetCellValue("Trạng thái");
            cell_7_Row7.CellStyle = borderedCellStyle;
            #endregion

            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "ORDER_REPORT_DETAIL_LIST",
                Data = JsonConvert.SerializeObject(new ZOrderDetailReportRequest
                {
                    Time = req.Time,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    PlaceOfCreation = req.PlaceOfCreation,
                    SearchKey = req.SearchKey
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZOrderDetailReportResponse>>(res.Data);
                int stt = 1;

                var row6 = sheet.CreateRow(6);
                var cell_0_Row6 = row6.CreateCell(0);
                cell_0_Row6.SetCellValue("Tổng dữ liệu");
                cell_0_Row6.CellStyle = borderedCellStyle;
                var cell_1_Row6 = row6.CreateCell(1);
                cell_1_Row6.CellStyle = borderedCellStyle;
                var cell_2_Row6 = row6.CreateCell(2);
                cell_2_Row6.CellStyle = borderedCellStyle;
                var cell_3_Row6 = row6.CreateCell(3);
                cell_3_Row6.SetCellValue(list.Sum(m => m.TempTransportFee) == 0 ? "0" : double.Parse(list.Sum(m => m.TempTransportFee).ToString()).ToString("#,###", cul.NumberFormat));
                cell_3_Row6.CellStyle = borderedCellStyle;
                var cell_4_Row6 = row6.CreateCell(4);
                cell_4_Row6.SetCellValue(list.Sum(m => m.TransportFee) == 0 ? "0" : double.Parse(list.Sum(m => m.TransportFee).ToString()).ToString("#,###", cul.NumberFormat));
                cell_4_Row6.CellStyle = borderedCellStyle;
                var cell_5_Row6 = row6.CreateCell(5);
                cell_5_Row6.SetCellValue(list.Sum(m => m.CODMoney) == 0 ? "0" : double.Parse(list.Sum(m => m.CODMoney).ToString()).ToString("#,###", cul.NumberFormat));
                cell_5_Row6.CellStyle = borderedCellStyle;
                var cell_6_Row6 = row6.CreateCell(6);
                cell_6_Row6.CellStyle = borderedCellStyle;
                var cell_7_Row6 = row6.CreateCell(7);
                cell_7_Row6.CellStyle = borderedCellStyle;

                int rowIndex = 8;
                foreach (var item in list)
                {
                    var newRow = sheet.CreateRow(rowIndex);

                    var cell_0_newRow = newRow.CreateCell(0);
                    cell_0_newRow.SetCellValue(stt++);
                    cell_0_newRow.CellStyle = borderedCellStyle;

                    var cell_1_newRow = newRow.CreateCell(1);
                    cell_1_newRow.SetCellValue(item.ItemCode);
                    cell_1_newRow.CellStyle = borderedCellStyle;

                    var cell_2_newRow = newRow.CreateCell(2);
                    cell_2_newRow.SetCellValue(item.OrderCreatedDate);
                    cell_2_newRow.CellStyle = borderedCellStyle;


                    var cell_3_newRow = newRow.CreateCell(3);
                    cell_3_newRow.SetCellValue(item.TempTransportFee == 0 ? "0" : double.Parse(item.TempTransportFee.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_3_newRow.CellStyle = borderedCellStyle;

                    var cell_4_newRow = newRow.CreateCell(4);
                    cell_4_newRow.SetCellValue(item.TransportFee == 0 ? "0" : double.Parse(item.TransportFee.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_4_newRow.CellStyle = borderedCellStyle;

                    var cell_5_newRow = newRow.CreateCell(5);
                    cell_5_newRow.SetCellValue(item.CODMoney == 0 ? "0" : double.Parse(item.CODMoney.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_5_newRow.CellStyle = borderedCellStyle;

                    var cell_6_newRow = newRow.CreateCell(6);
                    cell_6_newRow.SetCellValue(item.PlaceOfOrderCreation);
                    cell_6_newRow.CellStyle = borderedCellStyle;

                    var cell_7_newRow = newRow.CreateCell(7);
                    cell_7_newRow.SetCellValue(item.StatusName);
                    cell_7_newRow.CellStyle = borderedCellStyle;
                    rowIndex++;
                }
            }
            else
            {
                var row6 = sheet.CreateRow(6);
                var cell_0_Row6 = row6.CreateCell(0);
                cell_0_Row6.SetCellValue("Tổng dữ liệu");
                cell_0_Row6.CellStyle = borderedCellStyle;
                var cell_1_Row6 = row6.CreateCell(1);
                cell_1_Row6.CellStyle = borderedCellStyle;
                var cell_2_Row6 = row6.CreateCell(2);
                cell_2_Row6.CellStyle = borderedCellStyle;
                var cell_3_Row6 = row6.CreateCell(3);
                cell_3_Row6.SetCellValue("0");
                cell_3_Row6.CellStyle = borderedCellStyle;
                var cell_4_Row6 = row6.CreateCell(4);
                cell_4_Row6.SetCellValue("0");
                cell_4_Row6.CellStyle = borderedCellStyle;
                var cell_5_Row6 = row6.CreateCell(5);
                cell_5_Row6.SetCellValue("0");
                cell_5_Row6.CellStyle = borderedCellStyle;
                var cell_6_Row6 = row6.CreateCell(6);
                cell_6_Row6.CellStyle = borderedCellStyle;
                var cell_7_Row6 = row6.CreateCell(7);
                cell_7_Row6.CellStyle = borderedCellStyle;

            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachChiTietSanLuong_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls");
        }
    }
}