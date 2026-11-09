using CrystalDecisions.CrystalReports.Engine;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
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
    public class ReconciliationController : BaseController
    {
        // GET: Reconciliation 

        #region CODMoney
        [AuthorizeController]
        public ActionResult CODMoney()
        {
            ReconciliationCODMoneyViewModel model = new ReconciliationCODMoneyViewModel()
            {
                FromDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListStatus = XData.GetListReconciliationStatus(),
                ListTypeDate = XData.GetReconciliationTypeDate(),
                ListCODTransType = XData.GetListCODTransType()
            };
            return View(model);
        }
        [AuthorizeController]
        public ActionResult GetDataCODMoney(ZReconciliationSearchListCODFollowRequest req)
        {
            List<ReconciliationCODMoneyDataViewModel> model = new List<ReconciliationCODMoneyDataViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "RECONCILIATION_COD_FOLLOW_LIST",
                Data = JsonConvert.SerializeObject(new ZReconciliationSearchListCODFollowRequest
                {
                    CustomerCode = req.CustomerCode,
                    CustomerPPCRM = req.CustomerPPCRM,
                    TypeDate = req.TypeDate,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    CODTransType = req.CODTransType
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZReconciliationSearchListCODFollowResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new ReconciliationCODMoneyDataViewModel()
                    {
                        ID = item.ID,
                        ShopID = item.ShopID,
                        CustomerCode = item.CustomerCode,
                        CustomerPPCRM = item.CustomerPPCRM,
                        ListCode = item.ListCode,
                        EmsBarCode = item.EmsBarCode,
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        CreatedDateOrder = item.CreatedDateOrder,
                        TimeRelease = item.TimeRelease,
                        CODMoney = item.CODMoney,
                        CODTransTypeName = item.CODTransTypeName
                    });
                }
            }
            ViewBag.SumCOD = model.Sum(m => m.CODMoney);
            return View(model);
        }
        public ActionResult ExportExcelDataCODMoney(ZReconciliationSearchListCODFollowRequest req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            List<ReconciliationCODMoneyDataViewModel> model = new List<ReconciliationCODMoneyDataViewModel>();
            long SumCOD = 0;
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "RECONCILIATION_COD_FOLLOW_LIST",
                Data = JsonConvert.SerializeObject(new ZReconciliationSearchListCODFollowRequest
                {
                    CustomerCode = req.CustomerCode,
                    CustomerPPCRM = req.CustomerPPCRM,
                    TypeDate = req.TypeDate,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    CODTransType = req.CODTransType
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZReconciliationSearchListCODFollowResponse>>(res.Data);
                int stt = 1;
                SumCOD = list.Sum(m => m.CODMoney);
                ISheet sheet = wb.CreateSheet();
                sheet.SetColumnWidth(0, 2000);
                sheet.SetColumnWidth(1, 8000);
                sheet.SetColumnWidth(2, 8000);
                sheet.SetColumnWidth(3, 8000);
                sheet.SetColumnWidth(4, 8000);
                sheet.SetColumnWidth(5, 8000);
                sheet.SetColumnWidth(6, 8000);
                sheet.SetColumnWidth(7, 8000);
                sheet.SetColumnWidth(8, 7000);
                sheet.SetColumnWidth(9, 9000);
                HSSFFont _font = (HSSFFont)wb.CreateFont();
                _font.FontHeightInPoints = (short)11;
                _font.FontName = "Tahoma";

                HSSFCellStyle borderedCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                borderedCellStyle.SetFont(_font);
                borderedCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0);
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 9);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("Danh sách đối soát tiền COD");

                var row2 = sheet.CreateRow(2);
                var cell_0_Row2 = row2.CreateCell(0);
                cell_0_Row2.SetCellValue("Tổng dữ liệu");
                cell_0_Row2.CellStyle = borderedCellStyle;

                var cell_1_Row2 = row2.CreateCell(1);
                cell_1_Row2.CellStyle = borderedCellStyle;
                var cell_2_Row2 = row2.CreateCell(2);
                cell_2_Row2.CellStyle = borderedCellStyle;
                var cell_3_Row2 = row2.CreateCell(3);
                cell_3_Row2.CellStyle = borderedCellStyle;
                var cell_4_Row2 = row2.CreateCell(4);
                cell_4_Row2.CellStyle = borderedCellStyle;
                var cell_5_Row2 = row2.CreateCell(5);
                cell_5_Row2.CellStyle = borderedCellStyle;
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.CellStyle = borderedCellStyle;
                var cell_8_Row2 = row2.CreateCell(8);
                cell_8_Row2.CellStyle = borderedCellStyle;
                cell_8_Row2.SetCellValue(double.Parse(SumCOD.ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                var cell_9_Row2 = row2.CreateCell(9);
                cell_9_Row2.CellStyle = borderedCellStyle;



                var row3 = sheet.CreateRow(3);
                var cell_0_Row3 = row3.CreateCell(0);
                cell_0_Row3.SetCellValue("STT");
                cell_0_Row3.CellStyle = borderedCellStyle;

                var cell_1_Row3 = row3.CreateCell(1);
                cell_1_Row3.SetCellValue("Trạng thái COD");
                cell_1_Row3.CellStyle = borderedCellStyle;

                var cell_2_Row3 = row3.CreateCell(2);
                cell_2_Row3.SetCellValue("Mã khách hàng");
                cell_2_Row3.CellStyle = borderedCellStyle;

                var cell_3_Row3 = row3.CreateCell(3);
                cell_3_Row3.SetCellValue("Mã PP/CRM");
                cell_3_Row3.CellStyle = borderedCellStyle;

                var cell_4_Row3 = row3.CreateCell(4);
                cell_4_Row3.SetCellValue("Mã bưu gửi");
                cell_4_Row3.CellStyle = borderedCellStyle;

                var cell_5_Row3 = row3.CreateCell(5);
                cell_5_Row3.SetCellValue("Mã đơn hàng");
                cell_5_Row3.CellStyle = borderedCellStyle;

                var cell_6_Row3 = row3.CreateCell(6);
                cell_6_Row3.SetCellValue("Ngày tạo đơn");
                cell_6_Row3.CellStyle = borderedCellStyle;

                var cell_7_Row3 = row3.CreateCell(7);
                cell_7_Row3.SetCellValue("Ngày phát hành");
                cell_7_Row3.CellStyle = borderedCellStyle;

                var cell_8_Row3 = row3.CreateCell(8);
                cell_8_Row3.SetCellValue("Tiền COD");
                cell_8_Row3.CellStyle = borderedCellStyle;

                var cell_9_Row3 = row3.CreateCell(9);
                cell_9_Row3.SetCellValue("Mã bảng kê");
                cell_9_Row3.CellStyle = borderedCellStyle;

                int rowIndex = 4;
                foreach (var item in list)
                {
                    var newRow = sheet.CreateRow(rowIndex);

                    var cell_0_newRow = newRow.CreateCell(0);
                    cell_0_newRow.SetCellValue(stt++);
                    cell_0_newRow.CellStyle = borderedCellStyle;

                    var cell_1_newRow = newRow.CreateCell(1);
                    cell_1_newRow.SetCellValue(item.CODTransTypeName);
                    cell_1_newRow.CellStyle = borderedCellStyle;

                    var cell_2_newRow = newRow.CreateCell(2);
                    cell_2_newRow.SetCellValue(item.CustomerCode);
                    cell_2_newRow.CellStyle = borderedCellStyle;

                    var cell_3_newRow = newRow.CreateCell(3);
                    cell_3_newRow.SetCellValue(item.CustomerPPCRM);
                    cell_3_newRow.CellStyle = borderedCellStyle;

                    var cell_4_newRow = newRow.CreateCell(4);
                    cell_4_newRow.SetCellValue(item.EmsBarCode);
                    cell_4_newRow.CellStyle = borderedCellStyle;

                    var cell_5_newRow = newRow.CreateCell(5);
                    cell_5_newRow.SetCellValue(item.OrderCode);
                    cell_5_newRow.CellStyle = borderedCellStyle;

                    var cell_6_newRow = newRow.CreateCell(6);
                    cell_6_newRow.SetCellValue(item.CreatedDateOrder);
                    cell_6_newRow.CellStyle = borderedCellStyle;

                    var cell_7_newRow = newRow.CreateCell(7);
                    cell_7_newRow.SetCellValue(item.TimeRelease);
                    cell_7_newRow.CellStyle = borderedCellStyle;

                    var cell_8_newRow = newRow.CreateCell(8);
                    cell_8_newRow.SetCellValue(double.Parse(item.CODMoney.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_8_newRow.CellStyle = borderedCellStyle;

                    var cell_9_newRow = newRow.CreateCell(9);
                    cell_9_newRow.SetCellValue(item.ListCode);
                    cell_9_newRow.CellStyle = borderedCellStyle;

                    rowIndex++;
                }
            }
            else
            {
                ISheet sheet = wb.CreateSheet();
                sheet.SetColumnWidth(0, 2000);
                sheet.SetColumnWidth(1, 8000);
                sheet.SetColumnWidth(2, 8000);
                sheet.SetColumnWidth(3, 8000);
                sheet.SetColumnWidth(4, 8000);
                sheet.SetColumnWidth(5, 8000);
                sheet.SetColumnWidth(6, 8000);
                sheet.SetColumnWidth(7, 8000);
                sheet.SetColumnWidth(8, 7000);
                sheet.SetColumnWidth(9, 9000);
                HSSFFont _font = (HSSFFont)wb.CreateFont();
                _font.FontHeightInPoints = (short)11;
                _font.FontName = "Tahoma";

                HSSFCellStyle borderedCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                borderedCellStyle.SetFont(_font);
                borderedCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0);
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 9);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("Danh sách đối soát tiền COD");

                var row2 = sheet.CreateRow(2);
                var cell_0_Row2 = row2.CreateCell(0);
                cell_0_Row2.SetCellValue("Tổng dữ liệu");
                cell_0_Row2.CellStyle = borderedCellStyle;

                var cell_1_Row2 = row2.CreateCell(1);
                cell_1_Row2.CellStyle = borderedCellStyle;
                var cell_2_Row2 = row2.CreateCell(2);
                cell_2_Row2.CellStyle = borderedCellStyle;
                var cell_3_Row2 = row2.CreateCell(3);
                cell_3_Row2.CellStyle = borderedCellStyle;
                var cell_4_Row2 = row2.CreateCell(4);
                cell_4_Row2.CellStyle = borderedCellStyle;
                var cell_5_Row2 = row2.CreateCell(5);
                cell_5_Row2.CellStyle = borderedCellStyle;
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.CellStyle = borderedCellStyle;
                var cell_8_Row2 = row2.CreateCell(8);
                cell_8_Row2.CellStyle = borderedCellStyle;
                cell_8_Row2.SetCellValue(double.Parse(SumCOD.ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                var cell_9_Row2 = row2.CreateCell(9);
                cell_9_Row2.CellStyle = borderedCellStyle;



                var row3 = sheet.CreateRow(3);
                var cell_0_Row3 = row3.CreateCell(0);
                cell_0_Row3.SetCellValue("STT");
                cell_0_Row3.CellStyle = borderedCellStyle;

                var cell_1_Row3 = row3.CreateCell(1);
                cell_1_Row3.SetCellValue("Trạng thái COD");
                cell_1_Row3.CellStyle = borderedCellStyle;

                var cell_2_Row3 = row3.CreateCell(2);
                cell_2_Row3.SetCellValue("Mã khách hàng");
                cell_2_Row3.CellStyle = borderedCellStyle;

                var cell_3_Row3 = row3.CreateCell(3);
                cell_3_Row3.SetCellValue("Mã PP/CRM");
                cell_3_Row3.CellStyle = borderedCellStyle;

                var cell_4_Row3 = row3.CreateCell(4);
                cell_4_Row3.SetCellValue("Mã bưu gửi");
                cell_4_Row3.CellStyle = borderedCellStyle;

                var cell_5_Row3 = row3.CreateCell(5);
                cell_5_Row3.SetCellValue("Mã đơn hàng");
                cell_5_Row3.CellStyle = borderedCellStyle;

                var cell_6_Row3 = row3.CreateCell(6);
                cell_6_Row3.SetCellValue("Ngày tạo đơn");
                cell_6_Row3.CellStyle = borderedCellStyle;

                var cell_7_Row3 = row3.CreateCell(7);
                cell_7_Row3.SetCellValue("Ngày phát hành");
                cell_7_Row3.CellStyle = borderedCellStyle;

                var cell_8_Row3 = row3.CreateCell(8);
                cell_8_Row3.SetCellValue("Tiền COD");
                cell_8_Row3.CellStyle = borderedCellStyle;

                var cell_9_Row3 = row3.CreateCell(9);
                cell_9_Row3.SetCellValue("Mã bảng kê");
                cell_9_Row3.CellStyle = borderedCellStyle;
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachDoiSoatTienCOD_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".xls");
        }
        #endregion

        #region Freight
        [AuthorizeController]
        public ActionResult Freight()
        {
            ViewBag.ReconciliationStatus = XData.GetListReconciliationStatus();
            ViewBag.ReconciliationTypeDate = XData.GetReconciliationTypeDate();
            ReconciliationViewModel model = new ReconciliationViewModel()
            {
                FromDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy")
            };
            return View(model);
        }
        [AuthorizeController]
        public ActionResult GetDataFreight(ZReconciliationSearchListFeeFollowRequest req)
        {
            List<ReconciliationFeeViewModelV2> model = new List<ReconciliationFeeViewModelV2>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "RECONCILIATION_FEE_FOLLOW_LIST",
                Data = JsonConvert.SerializeObject(new ZReconciliationSearchListFeeFollowRequest
                {
                    CustomerCode = req.CustomerCode,
                    CustomerPPCRM = req.CustomerPPCRM,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZReconciliationSearchListFeeFollowResponseV2>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new ReconciliationFeeViewModelV2()
                    {
                        CustomerCode = item.CustomerCode,
                        CustomerPPCRM = item.CustomerPPCRM,
                        EmsBarCode = item.EmsBarCode,
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        TimeRelease = item.TimeRelease,
                        TempTransportFee = item.TempTransportFee,
                        ServiceFee = item.ServiceFee,
                        MainFee = item.MainFee,
                        ReducedRates = item.ReducedRates,
                        TransportFeePublic = item.TransportFeePublic,
                        TransportFee = item.TransportFee,
                        TypeItemName = XData.GetNameListTypeItemFreight(item.TypeItem)
                    });
                }
            }
            ViewBag.SumTempTransportFee = model.Sum(m => m.TempTransportFee);
            ViewBag.SumServiceFee = model.Sum(m => m.ServiceFee);
            ViewBag.SumMainFee = model.Sum(m => m.MainFee);
            ViewBag.SumReducedRates = model.Sum(m => m.ReducedRates);
            ViewBag.SumTransportFeePublic = model.Sum(m => m.TransportFeePublic);
            ViewBag.SumTransportFee = model.Sum(m => m.TransportFee);
            return View(model);
        }
        public ActionResult ExportExcelDataFreight(ZReconciliationSearchListFeeFollowRequest req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "RECONCILIATION_FEE_FOLLOW_LIST",
                Data = JsonConvert.SerializeObject(new ZReconciliationSearchListFeeFollowRequest
                {
                    CustomerCode = req.CustomerCode,
                    CustomerPPCRM = req.CustomerPPCRM,
                    FromDate = req.FromDate,
                    ToDate = req.ToDate
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZReconciliationSearchListFeeFollowResponseV2>>(res.Data);
                int stt = 1;
                ISheet sheet = wb.CreateSheet();
                sheet.SetColumnWidth(0, 2000);
                sheet.SetColumnWidth(1, 6000);
                sheet.SetColumnWidth(2, 6000);
                sheet.SetColumnWidth(3, 10000);
                sheet.SetColumnWidth(4, 6000);
                sheet.SetColumnWidth(5, 6000);
                sheet.SetColumnWidth(6, 6000);
                sheet.SetColumnWidth(7, 6000);
                sheet.SetColumnWidth(8, 6000);
                sheet.SetColumnWidth(9, 6000);
                sheet.SetColumnWidth(10, 6000);
                sheet.SetColumnWidth(11, 6000);
                HSSFFont _font = (HSSFFont)wb.CreateFont();
                _font.FontHeightInPoints = (short)11;
                _font.FontName = "Tahoma";

                HSSFCellStyle borderedCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                borderedCellStyle.SetFont(_font);
                borderedCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0);
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 11);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("Danh sách đối soát tiền Cước");

                var row2 = sheet.CreateRow(2);
                var cell_0_Row2 = row2.CreateCell(0);
                cell_0_Row2.SetCellValue("Tổng dữ liệu");
                cell_0_Row2.CellStyle = borderedCellStyle;
                var cell_1_Row2 = row2.CreateCell(1);
                cell_1_Row2.CellStyle = borderedCellStyle;
                var cell_2_Row2 = row2.CreateCell(2);
                cell_2_Row2.CellStyle = borderedCellStyle;
                var cell_3_Row2 = row2.CreateCell(3);
                cell_3_Row2.CellStyle = borderedCellStyle;
                var cell_4_Row2 = row2.CreateCell(4);
                cell_4_Row2.CellStyle = borderedCellStyle;

                var cell_5_Row2 = row2.CreateCell(5);
                cell_5_Row2.SetCellValue(double.Parse(list.Sum(m => m.TempTransportFee).ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_5_Row2.CellStyle = borderedCellStyle;
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.SetCellValue(double.Parse(list.Sum(m => m.ServiceFee).ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.SetCellValue(double.Parse(list.Sum(m => m.MainFee).ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_7_Row2.CellStyle = borderedCellStyle;
                var cell_8_Row2 = row2.CreateCell(8);
                cell_8_Row2.SetCellValue(double.Parse(list.Sum(m => m.ReducedRates).ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_8_Row2.CellStyle = borderedCellStyle;
                var cell_9_Row2 = row2.CreateCell(9);
                cell_9_Row2.SetCellValue(double.Parse(list.Sum(m => m.TransportFeePublic).ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_9_Row2.CellStyle = borderedCellStyle;
                var cell_10_Row2 = row2.CreateCell(10);
                cell_10_Row2.SetCellValue(double.Parse(list.Sum(m => m.TransportFee).ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_10_Row2.CellStyle = borderedCellStyle;
                var cell_11_Row2 = row2.CreateCell(11);
                cell_11_Row2.CellStyle = borderedCellStyle;


                var row3 = sheet.CreateRow(3);
                var cell_0_Row3 = row3.CreateCell(0);
                cell_0_Row3.SetCellValue("STT");
                cell_0_Row3.CellStyle = borderedCellStyle;

                var cell_1_Row3 = row3.CreateCell(1);
                cell_1_Row3.SetCellValue("Mã khách hàng");
                cell_1_Row3.CellStyle = borderedCellStyle;

                var cell_2_Row3 = row3.CreateCell(2);
                cell_2_Row3.SetCellValue("Mã PP/CRM");
                cell_2_Row3.CellStyle = borderedCellStyle;

                var cell_3_Row3 = row3.CreateCell(3);
                cell_3_Row3.SetCellValue("Mã bưu gửi / Mã đơn hàng");
                cell_3_Row3.CellStyle = borderedCellStyle;

                var cell_4_Row3 = row3.CreateCell(4);
                cell_4_Row3.SetCellValue("Ngày phát hành");
                cell_4_Row3.CellStyle = borderedCellStyle;

                var cell_5_Row3 = row3.CreateCell(5);
                cell_5_Row3.SetCellValue("Cước tạm tính");
                cell_5_Row3.CellStyle = borderedCellStyle;

                var cell_6_Row3 = row3.CreateCell(6);
                cell_6_Row3.SetCellValue("Cước DV");
                cell_6_Row3.CellStyle = borderedCellStyle;

                var cell_7_Row3 = row3.CreateCell(7);
                cell_7_Row3.SetCellValue("Cước chính");
                cell_7_Row3.CellStyle = borderedCellStyle;

                var cell_8_Row3 = row3.CreateCell(8);
                cell_8_Row3.SetCellValue("Cước giảm");
                cell_8_Row3.CellStyle = borderedCellStyle;

                var cell_9_Row3 = row3.CreateCell(9);
                cell_9_Row3.SetCellValue("Cước thu hộ");
                cell_9_Row3.CellStyle = borderedCellStyle;

                var cell_10_Row3 = row3.CreateCell(10);
                cell_10_Row3.SetCellValue("Cước thực tế");
                cell_10_Row3.CellStyle = borderedCellStyle;

                var cell_11_Row3 = row3.CreateCell(11);
                cell_11_Row3.SetCellValue("Loại bưu gửi");
                cell_11_Row3.CellStyle = borderedCellStyle;

                // bắt đầu duyệt mảng và ghi tiếp tục
                int rowIndex = 4;
                foreach (var item in list)
                {
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    var cell_0_newRow = newRow.CreateCell(0);
                    cell_0_newRow.SetCellValue(stt++);
                    cell_0_newRow.CellStyle = borderedCellStyle;

                    var cell_1_newRow = newRow.CreateCell(1);
                    cell_1_newRow.SetCellValue(item.CustomerCode);
                    cell_1_newRow.CellStyle = borderedCellStyle;

                    var cell_2_newRow = newRow.CreateCell(2);
                    cell_2_newRow.SetCellValue(item.CustomerPPCRM);
                    cell_2_newRow.CellStyle = borderedCellStyle;


                    var cell_3_newRow = newRow.CreateCell(3);
                    cell_3_newRow.SetCellValue(item.EmsBarCode + " / " + item.OrderCode);
                    cell_3_newRow.CellStyle = borderedCellStyle;

                    var cell_4_newRow = newRow.CreateCell(4);
                    cell_4_newRow.SetCellValue(item.TimeRelease);
                    cell_4_newRow.CellStyle = borderedCellStyle;


                    var cell_5_newRow = newRow.CreateCell(5);
                    cell_5_newRow.SetCellValue(item.TempTransportFee == 0 ? "0" : double.Parse(item.TempTransportFee.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_5_newRow.CellStyle = borderedCellStyle;

                    var cell_6_newRow = newRow.CreateCell(6);
                    cell_6_newRow.SetCellValue(item.ServiceFee == 0 ? "0" : double.Parse(item.ServiceFee.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_6_newRow.CellStyle = borderedCellStyle;

                    var cell_7_newRow = newRow.CreateCell(7);
                    cell_7_newRow.SetCellValue(item.MainFee == 0 ? "0" : double.Parse(item.MainFee.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_7_newRow.CellStyle = borderedCellStyle;

                    var cell_8_newRow = newRow.CreateCell(8);
                    cell_8_newRow.SetCellValue(item.ReducedRates == 0 ? "0" : double.Parse(item.ReducedRates.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_8_newRow.CellStyle = borderedCellStyle;

                    var cell_9_newRow = newRow.CreateCell(9);
                    cell_9_newRow.SetCellValue(item.TransportFeePublic == 0 ? "0" : double.Parse(item.TransportFeePublic.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_9_newRow.CellStyle = borderedCellStyle;

                    var cell_10_newRow = newRow.CreateCell(10);
                    cell_10_newRow.SetCellValue(item.TransportFee == 0 ? "0" : double.Parse(item.TransportFee.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_10_newRow.CellStyle = borderedCellStyle;

                    var cell_11_newRow = newRow.CreateCell(11);
                    cell_11_newRow.SetCellValue(XData.GetNameListTypeItemFreight(item.TypeItem));
                    cell_11_newRow.CellStyle = borderedCellStyle;
                    rowIndex++;
                }
            }
            else
            {
                ISheet sheet = wb.CreateSheet();
                sheet.SetColumnWidth(0, 2000);
                sheet.SetColumnWidth(1, 6000);
                sheet.SetColumnWidth(2, 6000);
                sheet.SetColumnWidth(3, 10000);
                sheet.SetColumnWidth(4, 6000);
                sheet.SetColumnWidth(5, 6000);
                sheet.SetColumnWidth(6, 6000);
                sheet.SetColumnWidth(7, 6000);
                sheet.SetColumnWidth(8, 6000);
                sheet.SetColumnWidth(9, 6000);
                sheet.SetColumnWidth(10, 6000);
                sheet.SetColumnWidth(11, 6000);
                HSSFFont _font = (HSSFFont)wb.CreateFont();
                _font.FontHeightInPoints = (short)11;
                _font.FontName = "Tahoma";

                HSSFCellStyle borderedCellStyle = (HSSFCellStyle)wb.CreateCellStyle();
                borderedCellStyle.SetFont(_font);
                borderedCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                borderedCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0);
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 11);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("Danh sách đối soát tiền Cước");

                var row2 = sheet.CreateRow(2);
                var cell_0_Row2 = row2.CreateCell(0);
                cell_0_Row2.SetCellValue("Tổng dữ liệu");
                cell_0_Row2.CellStyle = borderedCellStyle;
                var cell_1_Row2 = row2.CreateCell(1);
                cell_1_Row2.CellStyle = borderedCellStyle;
                var cell_2_Row2 = row2.CreateCell(2);
                cell_2_Row2.CellStyle = borderedCellStyle;
                var cell_3_Row2 = row2.CreateCell(3);
                cell_3_Row2.CellStyle = borderedCellStyle;
                var cell_4_Row2 = row2.CreateCell(4);
                cell_4_Row2.CellStyle = borderedCellStyle;

                var cell_5_Row2 = row2.CreateCell(5);
                cell_5_Row2.SetCellValue("0 ₫");
                cell_5_Row2.CellStyle = borderedCellStyle;
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.SetCellValue("0 ₫");
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.SetCellValue("0 ₫");
                cell_7_Row2.CellStyle = borderedCellStyle;
                var cell_8_Row2 = row2.CreateCell(8);
                cell_8_Row2.SetCellValue("0 ₫");
                cell_8_Row2.CellStyle = borderedCellStyle;
                var cell_9_Row2 = row2.CreateCell(9);
                cell_9_Row2.SetCellValue("0 ₫");
                cell_9_Row2.CellStyle = borderedCellStyle;
                var cell_10_Row2 = row2.CreateCell(10);
                cell_10_Row2.SetCellValue("0 ₫");
                cell_10_Row2.CellStyle = borderedCellStyle;
                var cell_11_Row2 = row2.CreateCell(11);
                cell_11_Row2.CellStyle = borderedCellStyle;


                var row3 = sheet.CreateRow(3);
                var cell_0_Row3 = row3.CreateCell(0);
                cell_0_Row3.SetCellValue("STT");
                cell_0_Row3.CellStyle = borderedCellStyle;

                var cell_1_Row3 = row3.CreateCell(1);
                cell_1_Row3.SetCellValue("Mã khách hàng");
                cell_1_Row3.CellStyle = borderedCellStyle;

                var cell_2_Row3 = row3.CreateCell(2);
                cell_2_Row3.SetCellValue("Mã PP/CRM");
                cell_2_Row3.CellStyle = borderedCellStyle;

                var cell_3_Row3 = row3.CreateCell(3);
                cell_3_Row3.SetCellValue("Mã bưu gửi / Mã đơn hàng");
                cell_3_Row3.CellStyle = borderedCellStyle;

                var cell_4_Row3 = row3.CreateCell(4);
                cell_4_Row3.SetCellValue("Ngày phát hành");
                cell_4_Row3.CellStyle = borderedCellStyle;

                var cell_5_Row3 = row3.CreateCell(5);
                cell_5_Row3.SetCellValue("Cước tạm tính");
                cell_5_Row3.CellStyle = borderedCellStyle;

                var cell_6_Row3 = row3.CreateCell(6);
                cell_6_Row3.SetCellValue("Cước DV");
                cell_6_Row3.CellStyle = borderedCellStyle;

                var cell_7_Row3 = row3.CreateCell(7);
                cell_7_Row3.SetCellValue("Cước chính");
                cell_7_Row3.CellStyle = borderedCellStyle;

                var cell_8_Row3 = row3.CreateCell(8);
                cell_8_Row3.SetCellValue("Cước giảm");
                cell_8_Row3.CellStyle = borderedCellStyle;

                var cell_9_Row3 = row3.CreateCell(9);
                cell_9_Row3.SetCellValue("Cước thu hộ");
                cell_9_Row3.CellStyle = borderedCellStyle;

                var cell_10_Row3 = row3.CreateCell(10);
                cell_10_Row3.SetCellValue("Cước thực tế");
                cell_10_Row3.CellStyle = borderedCellStyle;

                var cell_11_Row3 = row3.CreateCell(11);
                cell_11_Row3.SetCellValue("Loại bưu gửi");
                cell_11_Row3.CellStyle = borderedCellStyle;
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachDoiSoatTienCuoc.xls");
        }
        #endregion

        #region FeeReport
        [AuthorizeController]
        public ActionResult ReconciliationFeeReport()
        {
            ReconciliationViewModel model = new ReconciliationViewModel()
            {
                FromDate = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")
            };
            return View(model);
        }
        public ActionResult DataReconciliationFeeReport(ZReconciliationSearchFeeReportRequest req)
        {
            List<ReconciliationFeeReportViewModel> model = new List<ReconciliationFeeReportViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "RECONCILIATION_FEE_REPORT_LIST",
                Data = JsonConvert.SerializeObject(new ZReconciliationSearchFeeReportRequest
                {
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    CustomerPPCRM = req.CustomerPPCRM,
                    PostOffice = req.PostOffice,
                    TypeItem = req.TypeItem
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZReconciliationSearchFeeReportResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new ReconciliationFeeReportViewModel()
                    {
                        PostOffice = item.PostOffice,
                        TotalOrder = item.TotalOrder,
                        TotalWeight = item.TotalWeight,
                        TotalFee = item.TotalFee,
                        TypeItemName = XData.GetNameListTypeItemFreight(item.TypeItem)
                    });
                }
            }
            return PartialView(model);
        }
        [AuthorizeController]
        public ActionResult ReconciliationFeePostOfficeReport(string PostOffice, string FromDate, string ToDate)
        {
            ReconciliationViewModel model = new ReconciliationViewModel()
            {
                FromDate = FromDate,
                ToDate = ToDate
            };
            ViewBag.PostOffice = PostOffice;
            return View(model);
        }
        public ActionResult DataReconciliationFeePostOfficeReport(ZReconciliationSearchFeePostOfficeReportRequest req)
        {
            List<ReconciliationFeePostOfficeReportViewModel> model = new List<ReconciliationFeePostOfficeReportViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "RECONCILIATION_FEE_POSTOFFICE_REPORT_LIST",
                Data = JsonConvert.SerializeObject(new ZReconciliationSearchFeePostOfficeReportRequest
                {
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    CustomerPPCRM = req.CustomerPPCRM,
                    PostOffice = req.PostOffice,
                    TypeItem = req.TypeItem
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZReconciliationSearchFeePostOfficeReportResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new ReconciliationFeePostOfficeReportViewModel()
                    {
                        CustomerPPCRM = item.CustomerPPCRM,
                        TotalOrder = item.TotalOrder,
                        TotalWeight = item.TotalWeight,
                        TotalFee = item.TotalFee,
                        TypeItemName = XData.GetNameListTypeItemFreight(item.TypeItem)
                    });
                }
            }
            ViewBag.SumTotalOrder = model.Sum(m => m.TotalOrder);
            ViewBag.SumTotalWeight = model.Sum(m => m.TotalWeight);
            ViewBag.SumTotalFee = model.Sum(m => m.TotalFee);
            ViewBag.PostOffice = req.PostOffice;
            return PartialView(model);
        }
        #endregion

        #region FeeAdjustingJournalEntries
        [AuthorizeController]
        public ActionResult FeeAdjustingJournalEntries()
        {
            ReconciliationViewModel model = new ReconciliationViewModel()
            {
                FromDate = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")
            };
            return View(model);
        }
        public ActionResult DataFeeAdjustingJournalEntries(ZReconciliationFeeAdjustingJournalEntriesRequest req)
        {
            AdjustingJournalEntriesViewModel model = ReconciliationRepository.DataFeeEditedItemCode(req);
            return PartialView(model);
        }
        public ActionResult ExportExcelDataFeeAdjusting(ZReconciliationFeeAdjustingJournalEntriesRequest req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            ISheet sheet = wb.CreateSheet("DANH SÁCH BÚT TOÁN TIỀN CƯỚC");

            #region SetColumnWidth
            sheet.SetColumnWidth(0, 2000);
            sheet.SetColumnWidth(1, 6000);
            sheet.SetColumnWidth(2, 6000);
            sheet.SetColumnWidth(3, 6000);
            sheet.SetColumnWidth(4, 6000);
            sheet.SetColumnWidth(5, 6000);
            sheet.SetColumnWidth(6, 6000);
            sheet.SetColumnWidth(7, 6000);
            sheet.SetColumnWidth(8, 6000);
            sheet.SetColumnWidth(9, 6000);
            sheet.SetColumnWidth(10, 6000);
            sheet.SetColumnWidth(11, 6000);
            sheet.SetColumnWidth(12, 12000);
            #endregion

            #region Style
            HSSFFont _font = (HSSFFont)wb.CreateFont();
            _font.FontHeightInPoints = (short)11;
            _font.FontName = "Tahoma";

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
            CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 13);
            sheet.AddMergedRegion(cellMerge);
            row0.GetCell(0).SetCellValue("DANH SÁCH BÚT TOÁN TIỀN CƯỚC");
            #endregion

            #region Title Table
            var row3 = sheet.CreateRow(3);
            var cell_0_Row3 = row3.CreateCell(0);
            cell_0_Row3.SetCellValue("STT");
            cell_0_Row3.CellStyle = borderedCellStyle;
            var cell_1_Row3 = row3.CreateCell(1);
            cell_1_Row3.SetCellValue("ID Điều chỉnh");
            cell_1_Row3.CellStyle = borderedCellStyle;
            var cell_2_Row3 = row3.CreateCell(2);
            cell_2_Row3.SetCellValue("Ngày điều chỉnh");
            cell_2_Row3.CellStyle = borderedCellStyle;
            var cell_3_Row3 = row3.CreateCell(3);
            cell_3_Row3.SetCellValue("Bưu cục điều chỉnh");
            cell_3_Row3.CellStyle = borderedCellStyle;
            var cell_4_Row3 = row3.CreateCell(4);
            cell_4_Row3.SetCellValue("Mã bưu gửi");
            cell_4_Row3.CellStyle = borderedCellStyle;
            var cell_5_Row3 = row3.CreateCell(5);
            cell_5_Row3.SetCellValue("Ngày phát hành");
            cell_5_Row3.CellStyle = borderedCellStyle;
            var cell_6_Row3 = row3.CreateCell(6);
            cell_6_Row3.SetCellValue("KL");
            cell_6_Row3.CellStyle = borderedCellStyle;
            var cell_7_Row3 = row3.CreateCell(7);
            cell_7_Row3.SetCellValue("KL Quy đổi");
            cell_7_Row3.CellStyle = borderedCellStyle;
            var cell_8_Row3 = row3.CreateCell(8);
            cell_8_Row3.SetCellValue("Cước chính");
            cell_8_Row3.CellStyle = borderedCellStyle;
            var cell_9_Row3 = row3.CreateCell(9);
            cell_9_Row3.SetCellValue("Cước DV");
            cell_9_Row3.CellStyle = borderedCellStyle;
            var cell_10_Row3 = row3.CreateCell(10);
            cell_10_Row3.SetCellValue("Cước COD");
            cell_10_Row3.CellStyle = borderedCellStyle;
            var cell_11_Row3 = row3.CreateCell(11);
            cell_11_Row3.SetCellValue("Tiền thu hộ");
            cell_11_Row3.CellStyle = borderedCellStyle;
            var cell_12_Row3 = row3.CreateCell(12);
            cell_12_Row3.SetCellValue("Lý do");
            cell_12_Row3.CellStyle = borderedCellStyle;
            #endregion

            AdjustingJournalEntriesViewModel model = ReconciliationRepository.DataFeeEditedItemCode(req);

            if (model != null)
            {
                int stt = 1;

                #region Header Table
                var row2 = sheet.CreateRow(2);
                var cell_0_Row2 = row2.CreateCell(0);
                cell_0_Row2.SetCellValue("Tổng dữ liệu");
                cell_0_Row2.CellStyle = borderedCellStyle;
                var cell_1_Row2 = row2.CreateCell(1);
                cell_1_Row2.CellStyle = borderedCellStyle;
                var cell_2_Row2 = row2.CreateCell(2);
                cell_2_Row2.CellStyle = borderedCellStyle;
                var cell_3_Row2 = row2.CreateCell(3);
                cell_3_Row2.CellStyle = borderedCellStyle;
                var cell_4_Row2 = row2.CreateCell(4);
                cell_4_Row2.CellStyle = borderedCellStyle;
                var cell_5_Row2 = row2.CreateCell(5);
                cell_5_Row2.CellStyle = borderedCellStyle;
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.SetCellValue(model.SumProvisionalVolume == 0 ? 0 : model.SumProvisionalVolume);
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.CellStyle = borderedCellStyle;
                cell_7_Row2.SetCellValue(model.SumChargeableVolume == 0 ? 0 : model.SumChargeableVolume);
                var cell_8_Row2 = row2.CreateCell(8);
                cell_8_Row2.SetCellValue(model.SumMainFee == 0 ? "0" : double.Parse(model.SumMainFee.ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_8_Row2.CellStyle = borderedCellStyle;
                var cell_9_Row2 = row2.CreateCell(9);
                cell_9_Row2.SetCellValue(model.SumServiceFee == 0 ? "0" : double.Parse(model.SumServiceFee.ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_9_Row2.CellStyle = borderedCellStyle;
                var cell_10_Row2 = row2.CreateCell(10);
                cell_10_Row2.SetCellValue(model.SumTransportFeePublic == 0 ? "0" : double.Parse(model.SumTransportFeePublic.ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_10_Row2.CellStyle = borderedCellStyle;
                var cell_11_Row2 = row2.CreateCell(11);
                cell_11_Row2.SetCellValue(model.SumFeePublic == 0 ? "0" : double.Parse(model.SumFeePublic.ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                cell_11_Row2.CellStyle = borderedCellStyle;
                #endregion

                int rowIndex = 4;
                foreach (var item in model.ListItem)
                {
                    var newRow = sheet.CreateRow(rowIndex);

                    var cell_0_newRow = newRow.CreateCell(0);
                    cell_0_newRow.SetCellValue(stt++);
                    cell_0_newRow.CellStyle = borderedCellStyle;

                    var cell_1_newRow = newRow.CreateCell(1);
                    cell_1_newRow.SetCellValue(item.AdjustingID);
                    cell_1_newRow.CellStyle = borderedCellStyle;

                    var cell_2_newRow = newRow.CreateCell(2);
                    cell_2_newRow.SetCellValue(item.AdjustingDate);
                    cell_2_newRow.CellStyle = borderedCellStyle;


                    var cell_3_newRow = newRow.CreateCell(3);
                    cell_3_newRow.SetCellValue(item.AdjustingPostOffice);
                    cell_3_newRow.CellStyle = borderedCellStyle;

                    var cell_4_newRow = newRow.CreateCell(4);
                    cell_4_newRow.SetCellValue(item.ItemCode);
                    cell_4_newRow.CellStyle = borderedCellStyle;

                    var cell_5_newRow = newRow.CreateCell(5);
                    cell_5_newRow.SetCellValue(item.TimeRelease);
                    cell_5_newRow.CellStyle = borderedCellStyle;

                    var cell_6_newRow = newRow.CreateCell(6);
                    cell_6_newRow.SetCellValue(item.ProvisionalVolume == 0 ? 0 : item.ProvisionalVolume);
                    cell_6_newRow.CellStyle = borderedCellStyle;

                    var cell_7_newRow = newRow.CreateCell(7);
                    cell_7_newRow.SetCellValue(item.ChargeableVolume == 0 ? 0 : item.ChargeableVolume);
                    cell_7_newRow.CellStyle = borderedCellStyle;

                    var cell_8_newRow = newRow.CreateCell(8);
                    cell_8_newRow.SetCellValue(item.MainFee == 0 ? "0" : double.Parse(item.MainFee.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_8_newRow.CellStyle = borderedCellStyle;

                    var cell_9_newRow = newRow.CreateCell(9);
                    cell_9_newRow.SetCellValue(item.ServiceFee == 0 ? "0" : double.Parse(item.ServiceFee.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_9_newRow.CellStyle = borderedCellStyle;

                    var cell_10_newRow = newRow.CreateCell(10);
                    cell_10_newRow.SetCellValue(item.TransportFeePublic == 0 ? "0" : double.Parse(item.TransportFeePublic.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_10_newRow.CellStyle = borderedCellStyle;

                    var cell_11_newRow = newRow.CreateCell(11);
                    cell_11_newRow.SetCellValue(item.FeePublic == 0 ? "0" : double.Parse(item.FeePublic.ToString()).ToString("#,###", cul.NumberFormat));
                    cell_11_newRow.CellStyle = borderedCellStyle;

                    var cell_12_newRow = newRow.CreateCell(12);
                    cell_12_newRow.SetCellValue(item.Reason);
                    cell_12_newRow.CellStyle = borderedCellStyle;
                    rowIndex++;
                }
            }
            else
            {
                #region Header Table
                var row2 = sheet.CreateRow(2);
                var cell_0_Row2 = row2.CreateCell(0);
                cell_0_Row2.SetCellValue("Tổng dữ liệu");
                cell_0_Row2.CellStyle = borderedCellStyle;
                var cell_1_Row2 = row2.CreateCell(1);
                cell_1_Row2.CellStyle = borderedCellStyle;
                var cell_2_Row2 = row2.CreateCell(2);
                cell_2_Row2.CellStyle = borderedCellStyle;
                var cell_3_Row2 = row2.CreateCell(3);
                cell_3_Row2.CellStyle = borderedCellStyle;
                var cell_4_Row2 = row2.CreateCell(4);
                cell_4_Row2.CellStyle = borderedCellStyle;
                var cell_5_Row2 = row2.CreateCell(5);
                cell_5_Row2.CellStyle = borderedCellStyle;
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.SetCellValue(0);
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.CellStyle = borderedCellStyle;
                cell_7_Row2.SetCellValue(0);
                var cell_8_Row2 = row2.CreateCell(8);
                cell_8_Row2.SetCellValue("0 ₫");
                cell_8_Row2.CellStyle = borderedCellStyle;
                var cell_9_Row2 = row2.CreateCell(9);
                cell_9_Row2.SetCellValue("0 ₫");
                cell_9_Row2.CellStyle = borderedCellStyle;
                var cell_10_Row2 = row2.CreateCell(10);
                cell_10_Row2.SetCellValue("0 ₫");
                cell_10_Row2.CellStyle = borderedCellStyle;
                var cell_11_Row2 = row2.CreateCell(11);
                cell_11_Row2.SetCellValue("0 ₫");
                cell_11_Row2.CellStyle = borderedCellStyle;
                #endregion
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachButToanTienCuoc.xls");
        }
        public ActionResult ExportPDFDataFeeAdjusting(ZReconciliationFeeAdjustingJournalEntriesRequest req)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReport"), "FeeEditedAdjJournalEntriesCR.rpt"));
            rd.DataSourceConnections.Clear();

            AdjustingJournalEntriesViewModel model = ReconciliationRepository.DataFeeEditedItemCode(req);

            if (model != null)
            {
                int stt = 1;
                DataSet.FeeAdjustmentJournalEntries ds = new DataSet.FeeAdjustmentJournalEntries();
                for (int i = 0; i < model.ListItem.Count; i++)
                {
                    System.Data.DataRow dr = ds.Tables["FeeEdited"].NewRow();
                    dr["NumRow"] = stt++;
                    dr["AdjustingID"] = model.ListItem[i].AdjustingID;
                    dr["AdjustingDate"] = model.ListItem[i].AdjustingDate;
                    dr["AdjustingPostOffice"] = model.ListItem[i].AdjustingPostOffice;
                    dr["ItemCode"] = model.ListItem[i].ItemCode;
                    dr["TimeRelease"] = model.ListItem[i].TimeRelease;
                    dr["ProvisionalVolume"] = model.ListItem[i].ProvisionalVolume;
                    dr["ChargeableVolume"] = model.ListItem[i].ChargeableVolume;
                    dr["MainFee"] = model.ListItem[i].MainFee;
                    dr["ServiceFee"] = model.ListItem[i].ServiceFee;
                    dr["TransportFeePublic"] = model.ListItem[i].TransportFeePublic;
                    dr["FeePublic"] = model.ListItem[i].FeePublic;
                    dr["Reason"] = model.ListItem[i].Reason;
                    ds.Tables["FeeEdited"].Rows.Add(dr);
                }
                rd.Database.Tables[0].SetDataSource(ds.Tables[0]);
                rd.SetParameterValue("PrSumProvisionalVolume", model.SumProvisionalVolume);
                rd.SetParameterValue("PrSumChargeableVolume", model.SumChargeableVolume);
                rd.SetParameterValue("PrSumMainFee", model.SumMainFee);
                rd.SetParameterValue("PrSumServiceFee", model.SumServiceFee);
                rd.SetParameterValue("PrSumTransportFeePublic", model.SumTransportFeePublic);
                rd.SetParameterValue("PrSumFeePublic", model.SumFeePublic);
            }
            else
            {
                rd.Database.Tables[0].SetDataSource("");
                rd.SetParameterValue("PrSumProvisionalVolume", 0);
                rd.SetParameterValue("PrSumChargeableVolume", 0);
                rd.SetParameterValue("PrSumMainFee", 0);
                rd.SetParameterValue("PrSumServiceFee", 0);
                rd.SetParameterValue("PrSumTransportFeePublic", 0);
                rd.SetParameterValue("PrSumFeePublic", 0);
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "DanhSachButToanTienCuoc_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf");
        }
        #endregion

        #region FeeDetailEditedItemCode
        [AuthorizeController]
        public ActionResult FeeDetailEditedItemCode()
        {
            ReconciliationViewModel model = new ReconciliationViewModel()
            {
                FromDate = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy")
            };
            return View(model);
        }
        public ActionResult DataFeeDetailEditedItemCode(ZReconciliationListDetailEditedItemCodeRequest req)
        {
            List<DetailEditedItemCodeViewModel>  model = ReconciliationRepository.DataFeeDetailEditedItemCode(req);
            return PartialView(model);
        }
        public ActionResult ExportExcelDataEditedItemCode(ZReconciliationListDetailEditedItemCodeRequest req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            IRow newRow;
            ISheet sheet = wb.CreateSheet("DANH SÁCH BẢNG KÊ");

            #region SetColumnWidth
            sheet.SetColumnWidth(0, 2000);
            sheet.SetColumnWidth(1, 6000);
            sheet.SetColumnWidth(2, 6000);
            sheet.SetColumnWidth(3, 6000);
            sheet.SetColumnWidth(4, 6000);
            sheet.SetColumnWidth(5, 6000);
            sheet.SetColumnWidth(6, 6000);
            sheet.SetColumnWidth(7, 6000);
            sheet.SetColumnWidth(8, 6000);
            sheet.SetColumnWidth(9, 6000);
            sheet.SetColumnWidth(10, 6000);
            sheet.SetColumnWidth(11, 6000);
            sheet.SetColumnWidth(12, 12000);
            #endregion

            #region Style
            var boldFont = wb.CreateFont();
            boldFont.FontName = "Times New Roman";
            boldFont.FontHeightInPoints = 11; //Font size
            boldFont.IsBold = true;

            var fontChar = wb.CreateFont();
            fontChar.FontName = "Times New Roman";
            fontChar.FontHeightInPoints = 11;

            var center = wb.CreateCellStyle();
            center.Alignment = HorizontalAlignment.Center;
            center.SetFont(fontChar);

            var rightBold = wb.CreateCellStyle();
            rightBold.Alignment = HorizontalAlignment.Right;
            rightBold.SetFont(boldFont);

            var right = wb.CreateCellStyle();
            right.Alignment = HorizontalAlignment.Right;
            right.SetFont(fontChar);

            var ItalicFont = wb.CreateFont();
            ItalicFont.IsItalic = true;
            ItalicFont.FontName = "Times New Roman";
            ItalicFont.FontHeightInPoints = 11;

            var italic = wb.CreateCellStyle();
            italic.SetFont(ItalicFont);
            italic.Alignment = HorizontalAlignment.Center;
            italic.VerticalAlignment = VerticalAlignment.Center;

            var style = wb.CreateCellStyle();
            style.SetFont(boldFont);

            var def = wb.CreateCellStyle();
            def.SetFont(fontChar);

            var styleCenter = wb.CreateCellStyle();
            styleCenter.SetFont(boldFont);
            styleCenter.VerticalAlignment = VerticalAlignment.Center;
            styleCenter.Alignment = HorizontalAlignment.Center;
            #endregion

            #region Header
            newRow = sheet.CreateRow(0);
            newRow.CreateCell(0).SetCellValue("DANH SÁCH BẢNG KÊ CHI TIẾT ĐIỀU CHỈNH BƯU GỬI");
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
            newRow.GetCell(0).CellStyle = style;
            #endregion

            #region Title Table
            newRow = sheet.CreateRow(2);
            newRow.CreateCell(0).SetCellValue("STT");
            newRow.GetCell(0).CellStyle = style;

            newRow.CreateCell(1).SetCellValue("Ngày điều chỉnh");
            newRow.GetCell(1).CellStyle = style;

            newRow.CreateCell(2).SetCellValue("Mã bưu gửi");
            newRow.GetCell(2).CellStyle = style;

            newRow.CreateCell(3).SetCellValue("Ngày phát hành");
            newRow.GetCell(3).CellStyle = style;

            newRow.CreateCell(4).SetCellValue("Mã PP/CRM");
            newRow.GetCell(4).CellStyle = style;

            newRow.CreateCell(5).SetCellValue("Bưu cục điều chỉnh");
            newRow.GetCell(5).CellStyle = style;

            newRow.CreateCell(6).SetCellValue("KL");
            newRow.GetCell(6).CellStyle = style;

            newRow.CreateCell(7).SetCellValue("KL Quy đổi");
            newRow.GetCell(7).CellStyle = style;

            newRow.CreateCell(8).SetCellValue("Cước chính");
            newRow.GetCell(8).CellStyle = style;

            newRow.CreateCell(9).SetCellValue("Cước DV");
            newRow.GetCell(9).CellStyle = style;

            newRow.CreateCell(10).SetCellValue("Cước COD");
            newRow.GetCell(10).CellStyle = style;

            newRow.CreateCell(11).SetCellValue("Tiền thu hộ");
            newRow.GetCell(11).CellStyle = style;

            newRow.CreateCell(12).SetCellValue("Lý do");
            newRow.GetCell(12).CellStyle = style;
            #endregion

            List<DetailEditedItemCodeViewModel> model = ReconciliationRepository.DataFeeDetailEditedItemCode(req);

            if (model.Count > 0)
            {
                int rowIndex = 2;
                var stt = 1;
                foreach (var item in model)
                {
                    rowIndex = rowIndex + 1;
                    newRow = sheet.CreateRow(rowIndex);
                    newRow.CreateCell(0).SetCellValue(stt++);
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 2, 0, 0));
                    newRow.GetCell(0).CellStyle = style;

                    newRow.CreateCell(1).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].AdjustingDate);
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 2, 1, 1));
                    newRow.GetCell(1).CellStyle = style;

                    newRow.CreateCell(2).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].ItemCode);
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 2, 2, 2));
                    newRow.GetCell(2).CellStyle = style;

                    newRow.CreateCell(3).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].TimeRelease);
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 2, 3, 3));
                    newRow.GetCell(3).CellStyle = style;

                    newRow.CreateCell(4).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].CustomerCode);
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 2, 4, 4));
                    newRow.GetCell(4).CellStyle = style;

                    newRow.CreateCell(5).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].AdjustingPostOffice);
                    newRow.GetCell(5).CellStyle = style;

                    newRow.CreateCell(6).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].ProvisionalVolume == 0 ? 0 : item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].ProvisionalVolume);
                    newRow.GetCell(6).CellStyle = style;

                    newRow.CreateCell(7).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].ChargeableVolume == 0 ? 0 : item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].ChargeableVolume);
                    newRow.GetCell(7).CellStyle = style;

                    newRow.CreateCell(8).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].MainFee == 0 ? "0" : double.Parse(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].MainFee.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(8).CellStyle = style;

                    newRow.CreateCell(9).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].ServiceFee == 0 ? "0" : double.Parse(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].ServiceFee.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(9).CellStyle = style;

                    newRow.CreateCell(10).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].TransportFeePublic == 0 ? "0" : double.Parse(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].TransportFeePublic.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(10).CellStyle = style;

                    newRow.CreateCell(11).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].FeePublic == 0 ? "0" : double.Parse(item.ItemCodeDetail[item.ItemCodeDetail.Count - 2].FeePublic.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(11).CellStyle = style;

                    newRow.CreateCell(12).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].Reason);
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 2, 12, 12));
                    newRow.GetCell(12).CellStyle = style;

                    rowIndex = rowIndex + 1;
                    newRow = sheet.CreateRow(rowIndex);
                    newRow.CreateCell(5).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].AdjustingPostOffice);
                    newRow.GetCell(5).CellStyle = style;

                    newRow.CreateCell(6).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].ProvisionalVolume == 0 ? 0 : item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].ProvisionalVolume);
                    newRow.GetCell(6).CellStyle = style;

                    newRow.CreateCell(7).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].ChargeableVolume == 0 ? 0 : item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].ChargeableVolume);
                    newRow.GetCell(7).CellStyle = style;

                    newRow.CreateCell(8).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].MainFee == 0 ? "0" : double.Parse(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].MainFee.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(8).CellStyle = style;

                    newRow.CreateCell(9).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].ServiceFee == 0 ? "0" : double.Parse(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].ServiceFee.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(9).CellStyle = style;

                    newRow.CreateCell(10).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].TransportFeePublic == 0 ? "0" : double.Parse(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].TransportFeePublic.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(10).CellStyle = style;

                    newRow.CreateCell(11).SetCellValue(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].FeePublic == 0 ? "0" : double.Parse(item.ItemCodeDetail[item.ItemCodeDetail.Count - 1].FeePublic.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(11).CellStyle = style;

                    rowIndex = rowIndex + 1;
                    newRow = sheet.CreateRow(rowIndex);
                    newRow.CreateCell(5).SetCellValue("Số dư điều chỉnh");
                    newRow.GetCell(5).CellStyle = style;

                    newRow.CreateCell(6).SetCellValue(item.AdjBalanceProvisionalVolume == 0 ? 0 : item.AdjBalanceProvisionalVolume);
                    newRow.GetCell(6).CellStyle = style;

                    newRow.CreateCell(7).SetCellValue(item.AdjBalanceChargeableVolume == 0 ? 0 : item.AdjBalanceChargeableVolume);
                    newRow.GetCell(7).CellStyle = style;

                    newRow.CreateCell(8).SetCellValue(item.AdjBalanceMainFee == 0 ? "0" : double.Parse(item.AdjBalanceMainFee.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(8).CellStyle = style;

                    newRow.CreateCell(9).SetCellValue(item.AdjBalanceServiceFee == 0 ? "0" : double.Parse(item.AdjBalanceServiceFee.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(9).CellStyle = style;

                    newRow.CreateCell(10).SetCellValue(item.AdjBalanceTransportFeePublic == 0 ? "0" : double.Parse(item.AdjBalanceTransportFeePublic.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(10).CellStyle = style;

                    newRow.CreateCell(11).SetCellValue(item.AdjBalanceFeePublic == 0 ? "0" : double.Parse(item.AdjBalanceFeePublic.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(11).CellStyle = style;
                }
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachBangKeChiTiet_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls");
        }
        public ActionResult ExportPDFDataEditedItemCode(ZReconciliationListDetailEditedItemCodeRequest req)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReport"), "FeeDetailEditedAdjJournalEntriesCR.rpt"));
            rd.DataSourceConnections.Clear();

            List<DetailEditedItemCodeViewModel> model = ReconciliationRepository.DataFeeDetailEditedItemCode(req);

            if (model.Count > 0)
            {
                DataSet.FeeAdjustmentJournalEntries ds = new DataSet.FeeAdjustmentJournalEntries();
                for (int i = 0; i < model.Count; i++)
                {
                    System.Data.DataRow dr = ds.Tables["FeeDetailEdited"].NewRow();
                    dr["NumRow"] = model[i].NumRow;
                    dr["AdjustingDate"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].AdjustingDate;
                    dr["ItemCode"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].ItemCode;
                    dr["TimeRelease"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].TimeRelease;
                    dr["CustomerCode"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].CustomerCode;
                    dr["AdjustingPostOffice_01"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 2].AdjustingPostOffice;
                    dr["AdjustingPostOffice_02"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].AdjustingPostOffice;
                    dr["ProvisionalVolume_01"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 2].ProvisionalVolume;
                    dr["ProvisionalVolume_02"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].ProvisionalVolume;
                    dr["AdjBalanceProvisionalVolume"] = model[i].AdjBalanceProvisionalVolume;
                    dr["ChargeableVolume_01"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 2].ChargeableVolume;
                    dr["ChargeableVolume_02"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].ChargeableVolume;
                    dr["AdjBalanceChargeableVolume"] = model[i].AdjBalanceChargeableVolume;
                    dr["MainFee_01"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 2].MainFee;
                    dr["MainFee_02"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].MainFee;
                    dr["AdjBalanceMainFee"] = model[i].AdjBalanceMainFee;
                    dr["ServiceFee_01"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 2].ServiceFee;
                    dr["ServiceFee_02"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].ServiceFee;
                    dr["AdjBalanceServiceFee"] = model[i].AdjBalanceServiceFee;
                    dr["TransportFeePublic_01"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 2].TransportFeePublic;
                    dr["TransportFeePublic_02"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].TransportFeePublic;
                    dr["AdjBalanceTransportFeePublic"] = model[i].AdjBalanceTransportFeePublic;
                    dr["FeePublic_01"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 2].FeePublic;
                    dr["FeePublic_02"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].FeePublic;
                    dr["AdjBalanceFeePublic"] = model[i].AdjBalanceFeePublic;
                    dr["Reason"] = model[i].ItemCodeDetail[model[i].ItemCodeDetail.Count - 1].Reason; ;
                    ds.Tables["FeeDetailEdited"].Rows.Add(dr);
                }
                rd.Database.Tables[0].SetDataSource(ds.Tables[1]);
            }
            else
            {
                rd.SetDataSource("");
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "DanhSachBangKeChiTiet_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf");
        }
        #endregion

        #region PostageRefund
        [AuthorizeController]
        public ActionResult PostageRefund()
        {
            ReconciliationViewModel model = new ReconciliationViewModel()
            {
                Time = DateTime.Now.ToString("MM/yyyy"),
            };
            return View(model);
        }
        public ActionResult DataPostageRefund(ZReconciliationSearchPostageRefundV1Request req)
        {
            PostageRefundViewModel model = ReconciliationRepository.DataPostageRefund(req);
            return PartialView(model);
        }
        public ActionResult ExportExcelDataPostageRefund(ZReconciliationSearchPostageRefundV1Request req)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
            HSSFWorkbook wb = new HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("DANH SÁCH BƯU GỬI HOÀN CƯỚC");

            #region SetColumnWidth
            sheet.SetColumnWidth(0, 2000);
            sheet.SetColumnWidth(1, 6000);
            sheet.SetColumnWidth(2, 6000);
            sheet.SetColumnWidth(3, 6000);
            sheet.SetColumnWidth(4, 6000);
            sheet.SetColumnWidth(5, 6000);
            sheet.SetColumnWidth(6, 6000);
            sheet.SetColumnWidth(7, 6000);
            sheet.SetColumnWidth(8, 6000);
            sheet.SetColumnWidth(9, 6000);
            sheet.SetColumnWidth(10, 6000);
            sheet.SetColumnWidth(11, 6000);
            sheet.SetColumnWidth(12, 12000);
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
            _fontDescribe.IsBold = true;
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
            CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 12);
            sheet.AddMergedRegion(cellMerge);
            row0.GetCell(0).SetCellValue("DANH SÁCH BƯU GỬI HOÀN CƯỚC");
            row0.GetCell(0).CellStyle = _styleFontHeader;
            #endregion

            #region Title Table
            var row3 = sheet.CreateRow(3);
            var cell_0_Row3 = row3.CreateCell(0);
            cell_0_Row3.SetCellValue("STT");
            cell_0_Row3.CellStyle = borderedCellStyle;

            var cell_1_Row3 = row3.CreateCell(1);
            cell_1_Row3.SetCellValue("Mã PP/CRM");
            cell_1_Row3.CellStyle = borderedCellStyle;

            var cell_2_Row3 = row3.CreateCell(2);
            cell_2_Row3.SetCellValue("Tham chiếu");
            cell_2_Row3.CellStyle = borderedCellStyle;

            var cell_3_Row3 = row3.CreateCell(3);
            cell_3_Row3.SetCellValue("Mã bưu gửi");
            cell_3_Row3.CellStyle = borderedCellStyle;

            var cell_4_Row3 = row3.CreateCell(4);
            cell_4_Row3.SetCellValue("Ngày phát hành");
            cell_4_Row3.CellStyle = borderedCellStyle;

            var cell_5_Row3 = row3.CreateCell(5);
            cell_5_Row3.SetCellValue("Ngày chuyển hoàn");
            cell_5_Row3.CellStyle = borderedCellStyle;

            var cell_6_Row3 = row3.CreateCell(6);
            cell_6_Row3.SetCellValue("Tiền nhờ thu (VNĐ)");
            cell_6_Row3.CellStyle = borderedCellStyle;

            var cell_7_Row3 = row3.CreateCell(7);
            cell_7_Row3.SetCellValue("Cước COD (VNĐ)");
            cell_7_Row3.CellStyle = borderedCellStyle;

            var cell_8_Row3 = row3.CreateCell(8);
            cell_8_Row3.SetCellValue("Cước hoàn (VNĐ)");
            cell_8_Row3.CellStyle = borderedCellStyle;

            var cell_9_Row3 = row3.CreateCell(9);
            cell_9_Row3.SetCellValue("Cước E1 CH (VNĐ)");
            cell_9_Row3.CellStyle = borderedCellStyle;

            var cell_10_Row3 = row3.CreateCell(10);
            cell_10_Row3.SetCellValue("Tỷ lệ giảm cước (%)");
            cell_10_Row3.CellStyle = borderedCellStyle;

            var cell_11_Row3 = row3.CreateCell(11);
            cell_11_Row3.SetCellValue("Số tiền hoàn cước (VNĐ)");
            cell_11_Row3.CellStyle = borderedCellStyle;

            var cell_12_Row3 = row3.CreateCell(12);
            cell_12_Row3.SetCellValue("Ghi chú");
            cell_12_Row3.CellStyle = borderedCellStyle;
            #endregion

            PostageRefundViewModel model = ReconciliationRepository.DataPostageRefund(req);
            if (model != null)
            {
                int stt = 1;

                #region Header Table
                var row2 = sheet.CreateRow(2);
                var cell_0_Row2 = row2.CreateCell(0);
                cell_0_Row2.SetCellValue("Tổng dữ liệu");
                cell_0_Row2.CellStyle = borderedCellStyle;
                var cell_1_Row2 = row2.CreateCell(1);
                cell_1_Row2.CellStyle = borderedCellStyle;
                var cell_2_Row2 = row2.CreateCell(2);
                cell_2_Row2.CellStyle = borderedCellStyle;
                var cell_3_Row2 = row2.CreateCell(3);
                cell_3_Row2.CellStyle = borderedCellStyle;
                var cell_4_Row2 = row2.CreateCell(4);
                cell_4_Row2.CellStyle = borderedCellStyle;
                var cell_5_Row2 = row2.CreateCell(5);
                cell_5_Row2.CellStyle = borderedCellStyle;
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.SetCellValue(model.SumMoneyforCollection == 0 ? "0" : double.Parse(model.SumMoneyforCollection.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.SetCellValue(model.SumCODFee == 0 ? "0" : double.Parse(model.SumCODFee.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_7_Row2.CellStyle = borderedCellStyle;
                var cell_8_Row2 = row2.CreateCell(8);
                cell_8_Row2.SetCellValue(model.SumCODFeeRefund == 0 ? "0" : double.Parse(model.SumCODFeeRefund.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_8_Row2.CellStyle = borderedCellStyle;
                var cell_9_Row2 = row2.CreateCell(9);
                cell_9_Row2.SetCellValue(model.SumFeeE1TransferRefund == 0 ? "0" : double.Parse(model.SumFeeE1TransferRefund.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_9_Row2.CellStyle = borderedCellStyle;
                var cell_10_Row2 = row2.CreateCell(10);
                cell_10_Row2.CellStyle = borderedCellStyle;
                var cell_11_Row2 = row2.CreateCell(11);
                cell_11_Row2.SetCellValue(model.SumRefundAmount == 0 ? "0" : double.Parse(model.SumRefundAmount.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_11_Row2.CellStyle = borderedCellStyle;
                var cell_12_Row2 = row2.CreateCell(12);
                cell_12_Row2.CellStyle = borderedCellStyle;
                #endregion

                int rowIndex = 4;
                foreach (var item in model.DataDetails)
                {
                    var newRow = sheet.CreateRow(rowIndex);

                    var cell_0_newRow = newRow.CreateCell(0);
                    cell_0_newRow.SetCellValue(stt++);
                    cell_0_newRow.CellStyle = borderedCellStyle;

                    var cell_1_newRow = newRow.CreateCell(1);
                    cell_1_newRow.SetCellValue(item.CustomerCode);
                    cell_1_newRow.CellStyle = borderedCellStyle;

                    var cell_2_newRow = newRow.CreateCell(2);
                    cell_2_newRow.SetCellValue(item.Reference);
                    cell_2_newRow.CellStyle = borderedCellStyle;

                    var cell_3_newRow = newRow.CreateCell(3);
                    cell_3_newRow.SetCellValue(item.ItemCode);
                    cell_3_newRow.CellStyle = borderedCellStyle;

                    var cell_4_newRow = newRow.CreateCell(4);
                    cell_4_newRow.SetCellValue(item.ReleaseDate);
                    cell_4_newRow.CellStyle = borderedCellStyle;

                    var cell_5_newRow = newRow.CreateCell(5);
                    cell_5_newRow.SetCellValue(item.RefundDate);
                    cell_5_newRow.CellStyle = borderedCellStyle;

                    var cell_6_newRow = newRow.CreateCell(6);
                    cell_6_newRow.SetCellValue(item.MoneyforCollection == 0 ? "0" : double.Parse(item.MoneyforCollection.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_6_newRow.CellStyle = borderedCellStyle;

                    var cell_7_newRow = newRow.CreateCell(7);
                    cell_7_newRow.SetCellValue(item.CODFee == 0 ? "0" : double.Parse(item.CODFee.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_7_newRow.CellStyle = borderedCellStyle;

                    var cell_8_newRow = newRow.CreateCell(8);
                    cell_8_newRow.SetCellValue(item.CODFeeRefund == 0 ? "0" : double.Parse(item.CODFeeRefund.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_8_newRow.CellStyle = borderedCellStyle;

                    var cell_9_newRow = newRow.CreateCell(9);
                    cell_9_newRow.SetCellValue(item.FeeE1TransferRefund == 0 ? "0" : double.Parse(item.FeeE1TransferRefund.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_9_newRow.CellStyle = borderedCellStyle;

                    var cell_10_newRow = newRow.CreateCell(10);
                    cell_10_newRow.SetCellValue(item.DiscountRate == 0 ? "0" : double.Parse(item.DiscountRate.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_10_newRow.CellStyle = borderedCellStyle;

                    var cell_11_newRow = newRow.CreateCell(11);
                    cell_11_newRow.SetCellValue(item.RefundAmount == 0 ? "0" : double.Parse(item.RefundAmount.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_11_newRow.CellStyle = borderedCellStyle;

                    var cell_12_newRow = newRow.CreateCell(12);
                    cell_12_newRow.SetCellValue(item.Note);
                    cell_12_newRow.CellStyle = borderedCellStyle;
                    rowIndex++;
                }
            }
            else
            {
                #region Header Table
                var row2 = sheet.CreateRow(2);
                var cell_0_Row2 = row2.CreateCell(0);
                cell_0_Row2.SetCellValue("Tổng dữ liệu");
                cell_0_Row2.CellStyle = borderedCellStyle;
                var cell_1_Row2 = row2.CreateCell(1);
                cell_1_Row2.CellStyle = borderedCellStyle;
                var cell_2_Row2 = row2.CreateCell(2);
                cell_2_Row2.CellStyle = borderedCellStyle;
                var cell_3_Row2 = row2.CreateCell(3);
                cell_3_Row2.CellStyle = borderedCellStyle;
                var cell_4_Row2 = row2.CreateCell(4);
                cell_4_Row2.CellStyle = borderedCellStyle;
                var cell_5_Row2 = row2.CreateCell(5);
                cell_5_Row2.CellStyle = borderedCellStyle;
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.SetCellValue("0 ₫");
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.SetCellValue("0 ₫");
                cell_7_Row2.CellStyle = borderedCellStyle;
                var cell_8_Row2 = row2.CreateCell(8);
                cell_8_Row2.SetCellValue("0 ₫");
                cell_8_Row2.CellStyle = borderedCellStyle;
                var cell_9_Row2 = row2.CreateCell(9);
                cell_9_Row2.SetCellValue("0 ₫");
                cell_9_Row2.CellStyle = borderedCellStyle;
                var cell_10_Row2 = row2.CreateCell(10);
                cell_10_Row2.SetCellValue(0);
                cell_10_Row2.CellStyle = borderedCellStyle;
                var cell_11_Row2 = row2.CreateCell(11);
                cell_11_Row2.SetCellValue("0 ₫");
                cell_11_Row2.CellStyle = borderedCellStyle;
                var cell_12_Row2 = row2.CreateCell(12);
                cell_12_Row2.CellStyle = borderedCellStyle;
                #endregion
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachBuuGuiHoanCuoc_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls");
        }
        public ActionResult ExportPDFDataPostageRefund(ZReconciliationSearchPostageRefundV1Request req)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReport"), "PostageRefundCR.rpt"));
            rd.DataSourceConnections.Clear();

            PostageRefundViewModel model = ReconciliationRepository.DataPostageRefund(req);
            if (model != null)
            {
                DataSet.PostageRefundDS ds = new DataSet.PostageRefundDS();
                for (int i = 0; i < model.DataDetails.Count; i++)
                {
                    System.Data.DataRow dr = ds.Tables["PostageRefund"].NewRow();
                    dr["CustomerCode"] = model.CustomerCode;
                    dr["Reference"] = model.DataDetails[i].Reference;
                    dr["ItemCode"] = model.DataDetails[i].ItemCode;
                    dr["ReleaseDate"] = model.DataDetails[i].ReleaseDate;
                    dr["RefundDate"] = model.DataDetails[i].RefundDate;
                    dr["MoneyforCollection"] = Convert.ToInt64(model.DataDetails[i].MoneyforCollection);
                    dr["CODFee"] = model.DataDetails[i].CODFee;
                    dr["CODFeeRefund"] = model.DataDetails[i].CODFeeRefund;
                    dr["FeeE1TransferRefund"] = model.DataDetails[i].FeeE1TransferRefund;
                    dr["DiscountRate"] = Convert.ToInt32(model.DataDetails[i].DiscountRate);
                    dr["RefundAmount"] = model.DataDetails[i].RefundAmount;
                    dr["Note"] = model.DataDetails[i].Note;
                    ds.Tables["PostageRefund"].Rows.Add(dr);
                }

                rd.Database.Tables[0].SetDataSource(ds.Tables[0]);
                rd.SetParameterValue("PrSumMoneyforCollection", model.SumMoneyforCollection);
                rd.SetParameterValue("PrSumCODFee", model.SumCODFee);
                rd.SetParameterValue("PrSumCODFeeRefund", model.SumCODFeeRefund);
                rd.SetParameterValue("PrSumFeeE1TransferRefund", model.SumFeeE1TransferRefund);
                rd.SetParameterValue("PrSumRefundAmount", model.SumRefundAmount);
            }
            else
            {
                rd.Database.Tables[0].SetDataSource("");
                rd.SetParameterValue("PrSumMoneyforCollection", 0);
                rd.SetParameterValue("PrSumCODFee", 0);
                rd.SetParameterValue("PrSumCODFeeRefund", 0);
                rd.SetParameterValue("PrSumFeeE1TransferRefund", 0);
                rd.SetParameterValue("PrSumRefundAmount", 0);
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "DanhSachBuuGuiHoanCuoc_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf");
        }
        #endregion

        #region DebtNotices
        [AuthorizeController]
        public ActionResult DebtNotices()
        {
            ReconciliationViewModel model = new ReconciliationViewModel()
            {
                Time = DateTime.Now.ToString("MM/yyyy"),
                ListReportType = XData.GetListDebtNoticesReportType()
            };
            return View(model);
        }
        public ActionResult DataSummaryOfDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            SummaryOfDebtNoticesViewModel model = ReconciliationRepository.DataSummaryOfDebtNotices(req);
            return PartialView(model);
        }
        public ActionResult ExportExcelDataSummaryOfDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
            SummaryOfDebtNoticesViewModel model = ReconciliationRepository.DataSummaryOfDebtNotices(req);
            ISheet sheet = wb.CreateSheet("GIẤY BÁO CƯỚC PHÍ TỔNG HỢP");

            #region SetColumnWidth
            sheet.SetColumnWidth(0, 6000);
            sheet.SetColumnWidth(1, 8000);
            sheet.SetColumnWidth(2, 6000);
            sheet.SetColumnWidth(3, 6000);
            sheet.SetColumnWidth(4, 6000);
            sheet.SetColumnWidth(5, 6000);
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

            #region Title Table
            var row9 = sheet.CreateRow(9);
            var cell_0_Row9 = row9.CreateCell(0);
            CellRangeAddress cell_0_MergeRow9_10 = new CellRangeAddress(9, 10, 0, 0);
            sheet.AddMergedRegion(cell_0_MergeRow9_10);
            cell_0_Row9.SetCellValue("STT");
            cell_0_Row9.CellStyle = borderedCellStyle;

            var cell_1_Row9 = row9.CreateCell(1);
            CellRangeAddress cell_1_MergeRow9_10 = new CellRangeAddress(9, 10, 1, 1);
            sheet.AddMergedRegion(cell_1_MergeRow9_10);
            cell_1_Row9.SetCellValue("Diễn giải");
            cell_1_Row9.CellStyle = borderedCellStyle;

            var cell_2_Row9 = row9.CreateCell(2);
            CellRangeAddress cell_2_MergeRow9_10 = new CellRangeAddress(9, 10, 2, 2);
            sheet.AddMergedRegion(cell_2_MergeRow9_10);
            cell_2_Row9.SetCellValue("Số lượng bưu gửi");
            cell_2_Row9.CellStyle = borderedCellStyle;

            var cell_3_Row9 = row9.CreateCell(3);
            CellRangeAddress cell_3_MergeRow9 = new CellRangeAddress(9, 9, 3, 5);
            sheet.AddMergedRegion(cell_3_MergeRow9);
            cell_3_Row9.SetCellValue("Cước phát sinh (VNĐ)");
            cell_3_Row9.CellStyle = borderedCellStyle;

            var row10 = sheet.CreateRow(10);

            var cell_3_Row10 = row10.CreateCell(3);
            cell_3_Row10.SetCellValue("Cước trước VAT");
            cell_3_Row10.CellStyle = borderedCellStyle;

            var cell_4_Row10 = row10.CreateCell(4);
            cell_4_Row10.SetCellValue("VAT");
            cell_4_Row10.CellStyle = borderedCellStyle;

            var cell_5_Row10 = row10.CreateCell(5);
            cell_5_Row10.SetCellValue("Cước sau VAT");
            cell_5_Row10.CellStyle = borderedCellStyle;
            #endregion

            if (model != null)
            {
                #region Header
                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0);
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 5);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("TỔNG CÔNG TY CHUYỂN PHÁT NHANH BƯU ĐIỆN - CÔNG TY CỔ PHẦN");
                row0.GetCell(0).CellStyle = _styleFontHeader;

                var row1 = sheet.CreateRow(1);
                row1.CreateCell(0);
                CellRangeAddress cellMergeRow1 = new CellRangeAddress(1, 1, 0, 5);
                sheet.AddMergedRegion(cellMergeRow1);
                row1.GetCell(0).SetCellValue(model.DataInfo.BranchNameMenu);
                row1.GetCell(0).CellStyle = _styleFontHeader;

                var row2 = sheet.CreateRow(2);
                row2.CreateCell(0);
                CellRangeAddress cellMergeRow2 = new CellRangeAddress(2, 2, 0, 5);
                sheet.AddMergedRegion(cellMergeRow2);
                row2.GetCell(0).SetCellValue(model.DataInfo.CustomerServiceCenterNameMenu);
                row2.GetCell(0).CellStyle = _styleFontHeader;

                var row4 = sheet.CreateRow(4);
                row4.CreateCell(0);
                CellRangeAddress cellMergeRow4 = new CellRangeAddress(4, 4, 0, 5);
                sheet.AddMergedRegion(cellMergeRow4);
                row4.GetCell(0).SetCellValue("GIẤY BÁO NỢ TỔNG HỢP");
                row4.GetCell(0).CellStyle = _styleFontHeader;

                var row6 = sheet.CreateRow(6);
                row6.CreateCell(0);
                CellRangeAddress cellMergeRow6 = new CellRangeAddress(6, 6, 0, 5);
                sheet.AddMergedRegion(cellMergeRow6);
                row6.GetCell(0).SetCellValue("Khách hàng nợ: " + model.DataInfoDetails.CustomerName);
                row6.GetCell(0).CellStyle = _styleFontDescribe;

                var row7 = sheet.CreateRow(7);
                row7.CreateCell(0);
                CellRangeAddress cellMergeRow7 = new CellRangeAddress(7, 7, 0, 5);
                sheet.AddMergedRegion(cellMergeRow7);
                row7.GetCell(0).SetCellValue("Thanh toán với " + model.DataInfo.BranchNameMenu + " các khoản cước phí ghi nợ tháng: " + model.MonthYear.ToString().Substring(4, 2) + "/" + model.MonthYear.ToString().Substring(0, 4));
                row7.GetCell(0).CellStyle = _styleFontDescribe;
                #endregion

                #region Table
                int rowIndex = 11;
                foreach (var item in model.DataInfoDetails.ListItem)
                {
                    var newRow = sheet.CreateRow(rowIndex);

                    var cell_0_newRow = newRow.CreateCell(0);
                    cell_0_newRow.SetCellValue(item.NumRow);
                    cell_0_newRow.CellStyle = borderedCellStyle;

                    var cell_1_newRow = newRow.CreateCell(1);
                    cell_1_newRow.SetCellValue(item.Explain);
                    cell_1_newRow.CellStyle = borderedCellStyle;

                    var cell_2_newRow = newRow.CreateCell(2);
                    cell_2_newRow.SetCellValue(item.NumberOfParcels);
                    cell_2_newRow.CellStyle = borderedCellStyle;

                    var cell_3_newRow = newRow.CreateCell(3);
                    cell_3_newRow.SetCellValue(item.FeeBeforeVAT == 0 ? "0" : double.Parse(item.FeeBeforeVAT.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_3_newRow.CellStyle = borderedCellStyle;

                    var cell_4_newRow = newRow.CreateCell(4);
                    cell_4_newRow.SetCellValue(item.FeeVAT == 0 ? "0" : double.Parse(item.FeeVAT.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_4_newRow.CellStyle = borderedCellStyle;

                    var cell_5_newRow = newRow.CreateCell(5);
                    cell_5_newRow.SetCellValue(item.FeeAfterVAT == 0 ? "0" : double.Parse(item.FeeAfterVAT.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_5_newRow.CellStyle = borderedCellStyle;

                    rowIndex++;
                }
                #endregion

                #region Footer
                rowIndex = rowIndex + 1;
                var newRowFooter1 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter1 = newRowFooter1.CreateCell(0);
                CellRangeAddress cellMergeFooter1 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter1);
                cell_0_RowFooter1.SetCellValue("Số tiền bằng chữ: " + model.TotalRevenueToText);
                cell_0_RowFooter1.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter2 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter2 = newRowFooter2.CreateCell(0);
                CellRangeAddress cellMergeFooter2 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter2);
                cell_0_RowFooter2.SetCellValue("Kèm theo: " + model.DataInfoDetails.InvoiceNumber + " chứng từ gốc");
                cell_0_RowFooter2.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter3 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter3 = newRowFooter3.CreateCell(0);
                CellRangeAddress cellMergeFooter3 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter3);
                cell_0_RowFooter3.SetCellValue("Quý khách có thể thanh toán bằng tiền mặt, séc, chuyển khoản qua ngân hàng.");
                cell_0_RowFooter3.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter4 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter4 = newRowFooter4.CreateCell(0);
                CellRangeAddress cellMergeFooter4 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter4);
                cell_0_RowFooter4.SetCellValue(model.DataInfo.PaymentInformationMenu);
                cell_0_RowFooter4.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter5 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter5 = newRowFooter5.CreateCell(0);
                CellRangeAddress cellMergeFooter5 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter5);
                cell_0_RowFooter5.SetCellValue("Quý khách vui lòng thanh toán và đối soát cho Chi nhánh Tổng công ty Chuyển phát nhanh Bưu điện trong vòng 15 ngày kể từ ngày nhận được giấy báo này.");
                cell_0_RowFooter5.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter6 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter6 = newRowFooter6.CreateCell(0);
                CellRangeAddress cellMergeFooter6 = new CellRangeAddress(rowIndex, rowIndex, 0, 2);
                sheet.AddMergedRegion(cellMergeFooter6);
                cell_0_RowFooter6.SetCellValue("Kế toán bưu cục: " + model.DataInfoDetails.AccountantName);
                cell_0_RowFooter6.CellStyle = _styleFontDescribe;

                var cell_3_RowFooter6 = newRowFooter6.CreateCell(3);
                CellRangeAddress cellMergeFooter6cell1 = new CellRangeAddress(rowIndex, rowIndex, 3, 5);
                sheet.AddMergedRegion(cellMergeFooter6cell1);
                cell_3_RowFooter6.SetCellValue("Số máy liên hệ: " + model.DataInfoDetails.AccountantPhone);
                cell_3_RowFooter6.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter7 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter7 = newRowFooter7.CreateCell(0);
                CellRangeAddress cellMergeFooter7 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter7);
                cell_0_RowFooter7.SetCellValue("Địa chỉ thanh toán: " + model.DataInfoDetails.PaymentPostCode + " - " + model.DataInfoDetails.PaymentPostAddress);
                cell_0_RowFooter7.CellStyle = _styleFontDescribe;
                #endregion
            }
            else
            {

                #region Header
                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0);
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 5);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("TỔNG CÔNG TY CHUYỂN PHÁT NHANH BƯU ĐIỆN - CÔNG TY CỔ PHẦN");
                row0.GetCell(0).CellStyle = _styleFontHeader;

                var row1 = sheet.CreateRow(1);
                row1.CreateCell(0);
                CellRangeAddress cellMergeRow1 = new CellRangeAddress(1, 1, 0, 5);
                sheet.AddMergedRegion(cellMergeRow1);
                row1.GetCell(0).SetCellValue(" - ");
                row1.GetCell(0).CellStyle = _styleFontHeader;

                var row2 = sheet.CreateRow(2);
                row2.CreateCell(0);
                CellRangeAddress cellMergeRow2 = new CellRangeAddress(2, 2, 0, 5);
                sheet.AddMergedRegion(cellMergeRow2);
                row2.GetCell(0).SetCellValue(" - ");
                row2.GetCell(0).CellStyle = _styleFontHeader;

                var row4 = sheet.CreateRow(4);
                row4.CreateCell(0);
                CellRangeAddress cellMergeRow4 = new CellRangeAddress(4, 4, 0, 5);
                sheet.AddMergedRegion(cellMergeRow4);
                row4.GetCell(0).SetCellValue("GIẤY BÁO NỢ TỔNG HỢP");
                row4.GetCell(0).CellStyle = _styleFontHeader;

                var row6 = sheet.CreateRow(6);
                row6.CreateCell(0);
                CellRangeAddress cellMergeRow6 = new CellRangeAddress(6, 6, 0, 5);
                sheet.AddMergedRegion(cellMergeRow6);
                row6.GetCell(0).SetCellValue("Khách hàng nợ: - ");
                row6.GetCell(0).CellStyle = _styleFontDescribe;

                var row7 = sheet.CreateRow(7);
                row7.CreateCell(0);
                CellRangeAddress cellMergeRow7 = new CellRangeAddress(7, 7, 0, 5);
                sheet.AddMergedRegion(cellMergeRow7);
                row7.GetCell(0).SetCellValue("Thanh toán với - các khoản cước phí ghi nợ tháng: - ");
                row7.GetCell(0).CellStyle = _styleFontDescribe;
                #endregion

                #region Footer
                int rowIndex = 12;
                var newRowFooter1 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter1 = newRowFooter1.CreateCell(0);
                CellRangeAddress cellMergeFooter1 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter1);
                cell_0_RowFooter1.SetCellValue("Số tiền bằng chữ: - ");
                cell_0_RowFooter1.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter2 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter2 = newRowFooter2.CreateCell(0);
                CellRangeAddress cellMergeFooter2 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter2);
                cell_0_RowFooter2.SetCellValue("Kèm theo: - chứng từ gốc");
                cell_0_RowFooter2.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter3 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter3 = newRowFooter3.CreateCell(0);
                CellRangeAddress cellMergeFooter3 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter3);
                cell_0_RowFooter3.SetCellValue("Quý khách có thể thanh toán bằng tiền mặt, séc, chuyển khoản qua ngân hàng.");
                cell_0_RowFooter3.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter4 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter4 = newRowFooter4.CreateCell(0);
                CellRangeAddress cellMergeFooter4 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter4);
                cell_0_RowFooter4.SetCellValue(" - ");
                cell_0_RowFooter4.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter5 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter5 = newRowFooter5.CreateCell(0);
                CellRangeAddress cellMergeFooter5 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter5);
                cell_0_RowFooter5.SetCellValue("Quý khách vui lòng thanh toán và đối soát cho Chi nhánh Tổng công ty Chuyển phát nhanh Bưu điện trong vòng 15 ngày kể từ ngày nhận được giấy báo này.");
                cell_0_RowFooter5.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter6 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter6 = newRowFooter6.CreateCell(0);
                CellRangeAddress cellMergeFooter6 = new CellRangeAddress(rowIndex, rowIndex, 0, 2);
                sheet.AddMergedRegion(cellMergeFooter6);
                cell_0_RowFooter6.SetCellValue("Kế toán bưu cục: - ");
                cell_0_RowFooter6.CellStyle = _styleFontDescribe;

                var cell_3_RowFooter6 = newRowFooter6.CreateCell(3);
                CellRangeAddress cellMergeFooter6cell1 = new CellRangeAddress(rowIndex, rowIndex, 3, 5);
                sheet.AddMergedRegion(cellMergeFooter6cell1);
                cell_3_RowFooter6.SetCellValue("Số máy liên hệ: - ");
                cell_3_RowFooter6.CellStyle = _styleFontDescribe;

                rowIndex = rowIndex + 1;
                var newRowFooter7 = sheet.CreateRow(rowIndex);
                var cell_0_RowFooter7 = newRowFooter7.CreateCell(0);
                CellRangeAddress cellMergeFooter7 = new CellRangeAddress(rowIndex, rowIndex, 0, 5);
                sheet.AddMergedRegion(cellMergeFooter7);
                cell_0_RowFooter7.SetCellValue("Địa chỉ thanh toán: - ");
                cell_0_RowFooter7.CellStyle = _styleFontDescribe;
                #endregion
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "GiayBaoCuocPhiTongHop_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls");
        }
        public ActionResult ExportPDFDataSummaryOfDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReport"), "SummaryOfDebtNoticesCR.rpt"));
            rd.DataSourceConnections.Clear();
            SummaryOfDebtNoticesViewModel model = ReconciliationRepository.DataSummaryOfDebtNotices(req);
            if (model != null)
            {
                DataSet.DebtNoticesDS ds = new DataSet.DebtNoticesDS();
                for (int i = 0; i < model.DataInfoDetails.ListItem.Count; i++)
                {
                    System.Data.DataRow dr = ds.Tables["SummaryOfDebtNotices"].NewRow();
                    dr["NumRow"] = model.DataInfoDetails.ListItem[i].NumRow;
                    dr["Explain"] = model.DataInfoDetails.ListItem[i].Explain;
                    dr["NumberOfParcels"] = model.DataInfoDetails.ListItem[i].NumberOfParcels;
                    dr["FeeBeforeVAT"] = model.DataInfoDetails.ListItem[i].FeeBeforeVAT;
                    dr["FeeVAT"] = model.DataInfoDetails.ListItem[i].FeeVAT;
                    dr["FeeAfterVAT"] = model.DataInfoDetails.ListItem[i].FeeAfterVAT;
                    ds.Tables["SummaryOfDebtNotices"].Rows.Add(dr);
                }
                rd.Database.Tables[0].SetDataSource(ds.Tables[0]);
                rd.SetParameterValue("PrBranchNameMenu", model.DataInfo.BranchNameMenu == null ? "" : model.DataInfo.BranchNameMenu);
                rd.SetParameterValue("PrCustomerServiceCenterNameMenu", model.DataInfo.CustomerServiceCenterNameMenu == null ? "" : model.DataInfo.CustomerServiceCenterNameMenu);
                rd.SetParameterValue("PrCustomerName", model.DataInfoDetails.CustomerName == null ? "" : model.DataInfoDetails.CustomerName);
                rd.SetParameterValue("PrMonthYear", model.MonthYear.ToString().Substring(4, 2) + "/" + model.MonthYear.ToString().Substring(0, 4));
                rd.SetParameterValue("PrTotalRevenueToText", model.TotalRevenueToText == null ? "" : model.TotalRevenueToText);
                rd.SetParameterValue("PrInvoiceNumber", model.DataInfoDetails.InvoiceNumber == null ? "" : model.DataInfoDetails.InvoiceNumber);
                rd.SetParameterValue("PrPaymentInformationMenu", model.DataInfo.PaymentInformationMenu == null ? "" : model.DataInfo.PaymentInformationMenu);
                rd.SetParameterValue("PrAccountantName", model.DataInfoDetails.AccountantName == null ? "" : model.DataInfoDetails.AccountantName);
                rd.SetParameterValue("PrAccountantPhone", model.DataInfoDetails.AccountantPhone == null ? "" : model.DataInfoDetails.AccountantPhone);
                rd.SetParameterValue("PrPaymentPostCode", model.DataInfoDetails.PaymentPostCode == null ? "" : model.DataInfoDetails.PaymentPostCode);
                rd.SetParameterValue("PrPaymentPostAddress", model.DataInfoDetails.PaymentPostAddress == null ? "" : model.DataInfoDetails.PaymentPostAddress);
            }
            else
            {
                rd.Database.Tables[0].SetDataSource("");
                rd.SetParameterValue("PrBranchNameMenu", "");
                rd.SetParameterValue("PrCustomerServiceCenterNameMenu", "");
                rd.SetParameterValue("PrCustomerName", "");
                rd.SetParameterValue("PrMonthYear", "");
                rd.SetParameterValue("PrTotalRevenueToText", "");
                rd.SetParameterValue("PrInvoiceNumber", "");
                rd.SetParameterValue("PrPaymentInformationMenu", "");
                rd.SetParameterValue("PrAccountantName", "");
                rd.SetParameterValue("PrAccountantPhone", "");
                rd.SetParameterValue("PrPaymentPostCode", "");
                rd.SetParameterValue("PrPaymentPostAddress", "");
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "GiayBaoCuocPhiTongHop_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf");
        }
        public ActionResult DataDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            DebtNoticesViewModel model = ReconciliationRepository.DataDebtNotices(req);
            return PartialView(model);
        }
        public ActionResult ExportExcelDataDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
            HSSFWorkbook wb = new HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("GIẤY BÁO CƯỚC PHÍ");

            #region SetColumnWidth
            sheet.SetColumnWidth(0, 10000);
            sheet.SetColumnWidth(1, 10000);
            sheet.SetColumnWidth(2, 8000);
            sheet.SetColumnWidth(3, 6000);
            sheet.SetColumnWidth(4, 6000);
            sheet.SetColumnWidth(5, 6000);
            sheet.SetColumnWidth(6, 6000);
            sheet.SetColumnWidth(7, 6000);
            sheet.SetColumnWidth(8, 6000);
            sheet.SetColumnWidth(9, 6000);
            sheet.SetColumnWidth(10, 6000);
            sheet.SetColumnWidth(11, 15000);
            sheet.SetColumnWidth(12, 6000);
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
            _fontDescribe.IsBold = true;
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

            #region Title Table
            var row7 = sheet.CreateRow(7);
            var cell_0_Row7 = row7.CreateCell(0);
            cell_0_Row7.SetCellValue("STT");
            cell_0_Row7.CellStyle = borderedCellStyle;

            var cell_1_Row7 = row7.CreateCell(1);
            cell_1_Row7.SetCellValue("Số hiệu");
            cell_1_Row7.CellStyle = borderedCellStyle;

            var cell_2_Row7 = row7.CreateCell(2);
            cell_2_Row7.SetCellValue("Số tham chiếu");
            cell_2_Row7.CellStyle = borderedCellStyle;

            var cell_3_Row7 = row7.CreateCell(3);
            cell_3_Row7.SetCellValue("Ngày");
            cell_3_Row7.CellStyle = borderedCellStyle;

            var cell_4_Row7 = row7.CreateCell(4);
            cell_4_Row7.SetCellValue("Bưu cục khai thác");
            cell_4_Row7.CellStyle = borderedCellStyle;

            var cell_5_Row7 = row7.CreateCell(5);
            cell_5_Row7.SetCellValue("Bưu cục trả");
            cell_5_Row7.CellStyle = borderedCellStyle;

            var cell_6_Row7 = row7.CreateCell(6);
            cell_6_Row7.SetCellValue("Trọng lượng");
            cell_6_Row7.CellStyle = borderedCellStyle;

            var cell_7_Row7 = row7.CreateCell(7);
            cell_7_Row7.SetCellValue("Cước chính (VNĐ)");
            cell_7_Row7.CellStyle = borderedCellStyle;

            var cell_8_Row7 = row7.CreateCell(8);
            cell_8_Row7.SetCellValue("Cước COD (VNĐ)");
            cell_8_Row7.CellStyle = borderedCellStyle;

            var cell_9_Row7 = row7.CreateCell(9);
            cell_9_Row7.SetCellValue("Cước DV (VNĐ)");
            cell_9_Row7.CellStyle = borderedCellStyle;

            var cell_10_Row7 = row7.CreateCell(10);
            cell_10_Row7.SetCellValue("Cước điều chỉnh (VNĐ)");
            cell_10_Row7.CellStyle = borderedCellStyle;

            var cell_11_Row7 = row7.CreateCell(11);
            cell_11_Row7.SetCellValue("Nội dung");
            cell_11_Row7.CellStyle = borderedCellStyle;

            var cell_12_Row7 = row7.CreateCell(12);
            cell_12_Row7.SetCellValue("Tổng cước (VNĐ)");
            cell_12_Row7.CellStyle = borderedCellStyle;
            #endregion

            DebtNoticesViewModel model = ReconciliationRepository.DataDebtNotices(req);
            if (model != null)
            {
                int stt = 1;

                #region Header
                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0);
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 12);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("GIẤY BÁO CƯỚC PHÍ");
                row0.GetCell(0).CellStyle = _styleFontHeader;

                var row2 = sheet.CreateRow(2);
                row2.CreateCell(0);
                CellRangeAddress cellMergeRow2 = new CellRangeAddress(2, 2, 0, 2);
                sheet.AddMergedRegion(cellMergeRow2);
                row2.GetCell(0).SetCellValue("THÔNG TIN KHÁCH HÀNG");
                row2.GetCell(0).CellStyle = _styleFontDescribe;

                var row3 = sheet.CreateRow(3);
                var cell_0_Row3 = row3.CreateCell(0);
                cell_0_Row3.SetCellValue("Mã KH: " + model.DataInfo.CustomerCode);
                cell_0_Row3.CellStyle = borderedCellStyle;
                var cell_1_Row3 = row3.CreateCell(1);
                cell_1_Row3.SetCellValue("Tên KH: " + model.DataInfo.CustomerName);
                cell_1_Row3.CellStyle = borderedCellStyle;
                var cell_2_Row3 = row3.CreateCell(2);
                cell_2_Row3.CellStyle = borderedCellStyle;

                var row4 = sheet.CreateRow(4);
                var cell_0_Row4 = row4.CreateCell(0);
                cell_0_Row4.SetCellValue("Số điện thoại: " + model.DataInfo.MobileNumber);
                cell_0_Row4.CellStyle = borderedCellStyle;
                var cell_1_Row4 = row4.CreateCell(1);
                cell_1_Row4.SetCellValue("Địa chỉ: " + model.DataInfo.Address);
                cell_1_Row4.CellStyle = borderedCellStyle;
                var cell_2_Row4 = row4.CreateCell(2);
                cell_2_Row4.SetCellValue("TaxCode: " + model.DataInfo.TaxCode);
                cell_2_Row4.CellStyle = borderedCellStyle;
                #endregion

                #region Header Table
                var row6 = sheet.CreateRow(6);
                var cell_0_Row6 = row6.CreateCell(0);
                cell_0_Row6.SetCellValue("Tổng dữ liệu");
                cell_0_Row6.CellStyle = borderedCellStyle;
                var cell_1_Row6 = row6.CreateCell(1);
                cell_1_Row6.CellStyle = borderedCellStyle;
                var cell_2_Row6 = row6.CreateCell(2);
                cell_2_Row6.CellStyle = borderedCellStyle;
                var cell_3_Row6 = row6.CreateCell(3);
                cell_3_Row6.CellStyle = borderedCellStyle;
                var cell_4_Row6 = row6.CreateCell(4);
                cell_4_Row6.CellStyle = borderedCellStyle;
                var cell_5_Row6 = row6.CreateCell(5);
                cell_5_Row6.CellStyle = borderedCellStyle;
                var cell_6_Row6 = row6.CreateCell(6);
                cell_6_Row6.CellStyle = borderedCellStyle;
                var cell_7_Row6 = row6.CreateCell(7);
                cell_7_Row6.SetCellValue(model.SumMainFee == 0 ? "0" : double.Parse(model.SumMainFee.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_7_Row6.CellStyle = borderedCellStyle;
                var cell_8_Row6 = row6.CreateCell(8);
                cell_8_Row6.SetCellValue(model.SumCODFee == 0 ? "0" : double.Parse(model.SumCODFee.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_8_Row6.CellStyle = borderedCellStyle;
                var cell_9_Row6 = row6.CreateCell(9);
                cell_9_Row6.SetCellValue(model.SumServiceFee == 0 ? "0" : double.Parse(model.SumServiceFee.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_9_Row6.CellStyle = borderedCellStyle;
                var cell_10_Row6 = row6.CreateCell(10);
                cell_10_Row6.SetCellValue(model.SumAdjustmentFee == 0 ? "0" : double.Parse(model.SumAdjustmentFee.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_10_Row6.CellStyle = borderedCellStyle;
                var cell_11_Row6 = row6.CreateCell(11);
                cell_11_Row6.CellStyle = borderedCellStyle;
                var cell_12_Row6 = row6.CreateCell(12);
                cell_12_Row6.SetCellValue(model.SumTotalCharge == 0 ? "0" : double.Parse(model.SumTotalCharge.ToString()).ToString("#,###", culture.NumberFormat) + " ₫");
                cell_12_Row6.CellStyle = borderedCellStyle;
                #endregion

                #region Table
                int rowIndex = 8;
                foreach (var item in model.DataInfoDetails)
                {
                    var newRow = sheet.CreateRow(rowIndex);

                    var cell_0_newRow = newRow.CreateCell(0);
                    cell_0_newRow.SetCellValue(stt++);
                    cell_0_newRow.CellStyle = borderedCellStyle;

                    var cell_1_newRow = newRow.CreateCell(1);
                    cell_1_newRow.SetCellValue(item.ItemCode);
                    cell_1_newRow.CellStyle = borderedCellStyle;

                    var cell_2_newRow = newRow.CreateCell(2);
                    cell_2_newRow.SetCellValue(item.ReferenceNumber);
                    cell_2_newRow.CellStyle = borderedCellStyle;

                    var cell_3_newRow = newRow.CreateCell(3);
                    cell_3_newRow.SetCellValue(item.ReleaseDate);
                    cell_3_newRow.CellStyle = borderedCellStyle;

                    var cell_4_newRow = newRow.CreateCell(4);
                    cell_4_newRow.SetCellValue(item.PostOfficeExploit);
                    cell_4_newRow.CellStyle = borderedCellStyle;

                    var cell_5_newRow = newRow.CreateCell(5);
                    cell_5_newRow.SetCellValue(item.PostOfficeReturn);
                    cell_5_newRow.CellStyle = borderedCellStyle;

                    var cell_6_newRow = newRow.CreateCell(6);
                    cell_6_newRow.SetCellValue(item.Weight == 0 ? "0" : double.Parse(item.Weight.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_6_newRow.CellStyle = borderedCellStyle;

                    var cell_7_newRow = newRow.CreateCell(7);
                    cell_7_newRow.SetCellValue(item.MainFee == 0 ? "0" : double.Parse(item.MainFee.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_7_newRow.CellStyle = borderedCellStyle;

                    var cell_8_newRow = newRow.CreateCell(8);
                    cell_8_newRow.SetCellValue(item.CODFee == 0 ? "0" : double.Parse(item.CODFee.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_8_newRow.CellStyle = borderedCellStyle;

                    var cell_9_newRow = newRow.CreateCell(9);
                    cell_9_newRow.SetCellValue(item.ServiceFee == 0 ? "0" : double.Parse(item.ServiceFee.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_9_newRow.CellStyle = borderedCellStyle;

                    var cell_10_newRow = newRow.CreateCell(10);
                    cell_10_newRow.SetCellValue(item.AdjustmentFee == 0 ? "0" : double.Parse(item.AdjustmentFee.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_10_newRow.CellStyle = borderedCellStyle;

                    var cell_11_newRow = newRow.CreateCell(11);
                    cell_11_newRow.SetCellValue(item.Content);
                    cell_11_newRow.CellStyle = borderedCellStyle;

                    var cell_12_newRow = newRow.CreateCell(12);
                    var TotalCharge = item.MainFee + item.CODFee + item.ServiceFee;
                    cell_12_newRow.SetCellValue(TotalCharge == 0 ? "0" : double.Parse(TotalCharge.ToString()).ToString("#,###", culture.NumberFormat));
                    cell_12_newRow.CellStyle = borderedCellStyle;
                    rowIndex++;
                }
                #endregion

                #region Footer
                rowIndex = rowIndex + 2;
                var newRowSum1 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum1 = newRowSum1.CreateCell(0);
                cell_0_RowSum1.SetCellValue("Cước khác: ");
                cell_0_RowSum1.CellStyle = borderedCellStyle;
                var cell_1_RowSum1 = newRowSum1.CreateCell(1);
                cell_1_RowSum1.SetCellValue(model.DataInfo.OtherCharges == 0 ? "0" : double.Parse(model.DataInfo.OtherCharges.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum1.CellStyle = borderedCellStyle;
                var cell_2_RowSum1 = newRowSum1.CreateCell(2);
                cell_2_RowSum1.CellStyle = borderedCellStyle;
                var cell_3_RowSum1 = newRowSum1.CreateCell(3);
                cell_3_RowSum1.SetCellValue(model.DataInfo.OtherCharges == 0 ? "0" : double.Parse(model.DataInfo.OtherCharges.ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum1.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum2 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum2 = newRowSum2.CreateCell(0);
                cell_0_RowSum2.SetCellValue("Cước điều chỉnh: ");
                cell_0_RowSum2.CellStyle = borderedCellStyle;
                var cell_1_RowSum2 = newRowSum2.CreateCell(1);
                cell_1_RowSum2.SetCellValue((model.DataInfo.AdjustmentFeeRevenue - model.SumAdjustmentFee) == 0 ? "0" : double.Parse((model.DataInfo.AdjustmentFeeRevenue - model.SumAdjustmentFee).ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum2.CellStyle = borderedCellStyle;
                var cell_2_RowSum2 = newRowSum2.CreateCell(2);
                cell_2_RowSum2.CellStyle = borderedCellStyle;
                var cell_3_RowSum2 = newRowSum2.CreateCell(3);
                cell_3_RowSum2.SetCellValue((model.DataInfo.AdjustmentFeeRevenue - model.SumAdjustmentFee) == 0 ? "0" : double.Parse((model.DataInfo.AdjustmentFeeRevenue - model.SumAdjustmentFee).ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum2.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum3 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum3 = newRowSum3.CreateCell(0);
                cell_0_RowSum3.SetCellValue("Trong nước: ");
                cell_0_RowSum3.CellStyle = borderedCellStyle;
                var cell_1_RowSum3 = newRowSum3.CreateCell(1);
                cell_1_RowSum3.SetCellValue(model.DataInfo.DomesticRevenue == 0 ? "0" : double.Parse(model.DataInfo.DomesticRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum3.CellStyle = borderedCellStyle;
                var cell_2_RowSum3 = newRowSum3.CreateCell(2);
                cell_2_RowSum3.SetCellValue(model.DataInfo.DomesticRate == 0 ? "0 %" : double.Parse(model.DataInfo.DomesticRate.ToString()).ToString("#,###", culture.NumberFormat) + " %");
                cell_2_RowSum3.CellStyle = borderedCellStyle;
                var cell_3_RowSum3 = newRowSum3.CreateCell(3);
                cell_3_RowSum3.SetCellValue(Math.Round((double)(model.DataInfo.DomesticRevenue * model.DataInfo.DomesticRate) / 100) == 0 ? "0" : double.Parse(Math.Round((double)(model.DataInfo.DomesticRevenue * model.DataInfo.DomesticRate) / 100).ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum3.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum4 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum4 = newRowSum4.CreateCell(0);
                cell_0_RowSum4.SetCellValue("Thoả thuận: ");
                cell_0_RowSum4.CellStyle = borderedCellStyle;
                var cell_1_RowSum4 = newRowSum4.CreateCell(1);
                cell_1_RowSum4.SetCellValue(model.DataInfo.NegotiableRevenue == 0 ? "0" : double.Parse(model.DataInfo.NegotiableRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum4.CellStyle = borderedCellStyle;
                var cell_2_RowSum4 = newRowSum4.CreateCell(2);
                cell_2_RowSum4.SetCellValue(model.DataInfo.AgreementRate == 0 ? "0 %" : double.Parse(model.DataInfo.AgreementRate.ToString()).ToString("#,###", culture.NumberFormat) + " %");
                cell_2_RowSum4.CellStyle = borderedCellStyle;
                var cell_3_RowSum4 = newRowSum4.CreateCell(3);
                cell_3_RowSum4.SetCellValue(Math.Round((double)(model.DataInfo.NegotiableRevenue * model.DataInfo.AgreementRate) / 100) == 0 ? "0" : double.Parse(Math.Round((double)(model.DataInfo.NegotiableRevenue * model.DataInfo.AgreementRate) / 100).ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum4.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum5 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum5 = newRowSum5.CreateCell(0);
                cell_0_RowSum5.SetCellValue("Quốc tế: ");
                cell_0_RowSum5.CellStyle = borderedCellStyle;
                var cell_1_RowSum5 = newRowSum5.CreateCell(1);
                cell_1_RowSum5.SetCellValue(model.DataInfo.InternationalRevenue == 0 ? "0" : double.Parse(model.DataInfo.InternationalRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum5.CellStyle = borderedCellStyle;
                var cell_2_RowSum5 = newRowSum5.CreateCell(2);
                cell_2_RowSum5.SetCellValue(model.DataInfo.InternationalRate == 0 ? "0 %" : double.Parse(model.DataInfo.InternationalRate.ToString()).ToString("#,###", culture.NumberFormat) + " %");
                cell_2_RowSum5.CellStyle = borderedCellStyle;
                var cell_3_RowSum5 = newRowSum5.CreateCell(3);
                cell_3_RowSum5.SetCellValue(Math.Round((double)(model.DataInfo.InternationalRevenue * model.DataInfo.InternationalRate) / 100) == 0 ? "0" : double.Parse(Math.Round((double)(model.DataInfo.InternationalRevenue * model.DataInfo.InternationalRate) / 100).ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum5.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum6 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum6 = newRowSum6.CreateCell(0);
                cell_0_RowSum6.SetCellValue("Quốc tế Priority: ");
                cell_0_RowSum6.CellStyle = borderedCellStyle;
                var cell_1_RowSum6 = newRowSum6.CreateCell(1);
                cell_1_RowSum6.SetCellValue(model.DataInfo.InternationalPriorityRevenue == 0 ? "0" : double.Parse(model.DataInfo.InternationalPriorityRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum6.CellStyle = borderedCellStyle;
                var cell_2_RowSum6 = newRowSum6.CreateCell(2);
                cell_2_RowSum6.SetCellValue(model.DataInfo.PriorityRate == 0 ? "0 %" : double.Parse(model.DataInfo.PriorityRate.ToString()).ToString("#,###", culture.NumberFormat) + " %");
                cell_2_RowSum6.CellStyle = borderedCellStyle;
                var cell_3_RowSum6 = newRowSum6.CreateCell(3);
                cell_3_RowSum6.SetCellValue(Math.Round((double)(model.DataInfo.InternationalPriorityRevenue * model.DataInfo.PriorityRate) / 100) == 0 ? "0" : double.Parse(Math.Round((double)(model.DataInfo.InternationalPriorityRevenue * model.DataInfo.PriorityRate) / 100).ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum6.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum7 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum7 = newRowSum7.CreateCell(0);
                cell_0_RowSum7.SetCellValue("Thương mại điện tử: ");
                cell_0_RowSum7.CellStyle = borderedCellStyle;
                var cell_1_RowSum7 = newRowSum7.CreateCell(1);
                cell_1_RowSum7.SetCellValue(model.DataInfo.EcommerceRevenue == 0 ? "0" : double.Parse(model.DataInfo.EcommerceRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum7.CellStyle = borderedCellStyle;
                var cell_2_RowSum7 = newRowSum7.CreateCell(2);
                cell_2_RowSum7.SetCellValue(model.DataInfo.EcommerceRate == 0 ? "0 %" : double.Parse(model.DataInfo.EcommerceRate.ToString()).ToString("#,###", culture.NumberFormat) + " %");
                cell_2_RowSum7.CellStyle = borderedCellStyle;
                var cell_3_RowSum7 = newRowSum7.CreateCell(3);
                cell_3_RowSum7.SetCellValue(Math.Round((double)(model.DataInfo.EcommerceRevenue * model.DataInfo.EcommerceRate) / 100) == 0 ? "0" : double.Parse(Math.Round((double)(model.DataInfo.EcommerceRevenue * model.DataInfo.EcommerceRate) / 100).ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum7.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum8 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum8 = newRowSum8.CreateCell(0);
                cell_0_RowSum8.SetCellValue("Chuyển hoàn: ");
                cell_0_RowSum8.CellStyle = borderedCellStyle;
                var cell_1_RowSum8 = newRowSum8.CreateCell(1);
                cell_1_RowSum8.SetCellValue(model.DataInfo.PermuteRevenue == 0 ? "0" : double.Parse(model.DataInfo.PermuteRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum8.CellStyle = borderedCellStyle;
                var cell_2_RowSum8 = newRowSum8.CreateCell(2);
                cell_2_RowSum8.SetCellValue(model.DataInfo.ReturnRate == 0 ? "0 %" : double.Parse(model.DataInfo.ReturnRate.ToString()).ToString("#,###", culture.NumberFormat) + " %");
                cell_2_RowSum8.CellStyle = borderedCellStyle;
                var cell_3_RowSum8 = newRowSum8.CreateCell(3);
                cell_3_RowSum8.SetCellValue(Math.Round((double)(model.DataInfo.PermuteRevenue * model.DataInfo.ReturnRate) / 100) == 0 ? "0" : double.Parse(Math.Round((double)(model.DataInfo.PermuteRevenue * model.DataInfo.ReturnRate) / 100).ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum8.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum9 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum9 = newRowSum9.CreateCell(0);
                cell_0_RowSum9.SetCellValue("Hoàn cước: ");
                cell_0_RowSum9.CellStyle = borderedCellStyle;
                var cell_1_RowSum9 = newRowSum9.CreateCell(1);
                cell_1_RowSum9.SetCellValue(model.DataInfo.RefundRevenue == 0 ? "0" : double.Parse(model.DataInfo.RefundRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum9.CellStyle = borderedCellStyle;
                var cell_2_RowSum9 = newRowSum9.CreateCell(2);
                cell_2_RowSum9.CellStyle = borderedCellStyle;
                var cell_3_RowSum9 = newRowSum9.CreateCell(3);
                cell_3_RowSum9.SetCellValue(model.DataInfo.RefundRevenue == 0 ? "0" : double.Parse(model.DataInfo.RefundRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum9.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum10 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum10 = newRowSum10.CreateCell(0);
                cell_0_RowSum10.SetCellValue("Truy thu: ");
                cell_0_RowSum10.CellStyle = borderedCellStyle;
                var cell_1_RowSum10 = newRowSum10.CreateCell(1);
                cell_1_RowSum10.SetCellValue(model.DataInfo.ArrearsRevenue == 0 ? "0" : double.Parse(model.DataInfo.ArrearsRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum10.CellStyle = borderedCellStyle;
                var cell_2_RowSum10 = newRowSum10.CreateCell(2);
                cell_2_RowSum10.CellStyle = borderedCellStyle;
                var cell_3_RowSum10 = newRowSum10.CreateCell(3);
                cell_3_RowSum10.SetCellValue(model.DataInfo.ArrearsRevenue == 0 ? "0" : double.Parse(model.DataInfo.ArrearsRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum10.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum11 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum11 = newRowSum11.CreateCell(0);
                cell_0_RowSum11.SetCellValue("Khuyến mãi theo CV: ");
                cell_0_RowSum11.CellStyle = borderedCellStyle;
                var cell_1_RowSum11 = newRowSum11.CreateCell(1);
                cell_1_RowSum11.SetCellValue(model.DataInfo.PromotionCVRevenue == 0 ? "0" : double.Parse(model.DataInfo.PromotionCVRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum11.CellStyle = borderedCellStyle;
                var cell_2_RowSum11 = newRowSum11.CreateCell(2);
                cell_2_RowSum11.CellStyle = borderedCellStyle;
                var cell_3_RowSum11 = newRowSum11.CreateCell(3);
                cell_3_RowSum11.SetCellValue(model.DataInfo.PromotionCVRevenue == 0 ? "0" : double.Parse(model.DataInfo.PromotionCVRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum11.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum12 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum12 = newRowSum12.CreateCell(0);
                cell_0_RowSum12.SetCellValue("Cước điều chỉnh khác: ");
                cell_0_RowSum12.CellStyle = borderedCellStyle;
                var cell_1_RowSum12 = newRowSum12.CreateCell(1);
                cell_1_RowSum12.SetCellValue(model.DataInfo.OtherAdjustmentChargesRevenue == 0 ? "0" : double.Parse(model.DataInfo.OtherAdjustmentChargesRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_1_RowSum12.CellStyle = borderedCellStyle;
                var cell_2_RowSum12 = newRowSum12.CreateCell(2);
                cell_2_RowSum12.CellStyle = borderedCellStyle;
                var cell_3_RowSum12 = newRowSum12.CreateCell(3);
                cell_3_RowSum12.SetCellValue(model.DataInfo.OtherAdjustmentChargesRevenue == 0 ? "0" : double.Parse(model.DataInfo.OtherAdjustmentChargesRevenue.ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum12.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum13 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum13 = newRowSum13.CreateCell(0);
                cell_0_RowSum13.SetCellValue("Tổng tiền TT sau khi chiết khấu: ");
                cell_0_RowSum13.CellStyle = borderedCellStyle;
                var cell_1_RowSum13 = newRowSum13.CreateCell(1);
                cell_1_RowSum13.CellStyle = borderedCellStyle;
                var cell_2_RowSum13 = newRowSum13.CreateCell(2);
                cell_2_RowSum13.CellStyle = borderedCellStyle;
                var cell_3_RowSum13 = newRowSum13.CreateCell(3);
                double Sum = (((double)(model.DataInfo.DomesticRevenue * model.DataInfo.DomesticRate) / 100) +
                            ((double)(model.DataInfo.PermuteRevenue * model.DataInfo.ReturnRate) / 100) +
                            ((double)(model.DataInfo.InternationalRevenue * model.DataInfo.InternationalRate) / 100) +
                            ((double)(model.DataInfo.NegotiableRevenue * model.DataInfo.AgreementRate) / 100) +
                            ((double)(model.DataInfo.EcommerceRevenue * model.DataInfo.EcommerceRate) / 100) +
                            (model.DataInfo.ArrearsRevenue + model.DataInfo.RefundRevenue));
                cell_3_RowSum13.SetCellValue(Math.Round((double)((model.SumTotalCharge + (model.DataInfo.AdjustmentFeeRevenue - model.SumAdjustmentFee) + model.DataInfo.OtherCharges) - Sum)) == 0 ? "0" : double.Parse(Math.Round((double)((model.SumTotalCharge + (model.DataInfo.AdjustmentFeeRevenue - model.SumAdjustmentFee) + model.DataInfo.OtherCharges) - Sum)).ToString()).ToString("#,###", culture.NumberFormat));
                cell_3_RowSum13.CellStyle = borderedCellStyle;
                #endregion
            }
            else
            {

                #region Header
                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0);
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 12);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("GIẤY BÁO CƯỚC PHÍ");
                row0.GetCell(0).CellStyle = _styleFontHeader;

                var row2 = sheet.CreateRow(2);
                row2.CreateCell(0);
                CellRangeAddress cellMergeRow2 = new CellRangeAddress(2, 2, 0, 2);
                sheet.AddMergedRegion(cellMergeRow2);
                row2.GetCell(0).SetCellValue("THÔNG TIN KHÁCH HÀNG");
                row2.GetCell(0).CellStyle = _styleFontDescribe;

                var row3 = sheet.CreateRow(3);
                var cell_0_Row3 = row3.CreateCell(0);
                cell_0_Row3.SetCellValue("Mã KH: - ");
                cell_0_Row3.CellStyle = borderedCellStyle;
                var cell_1_Row3 = row3.CreateCell(1);
                cell_1_Row3.SetCellValue("Tên KH: - ");
                cell_1_Row3.CellStyle = borderedCellStyle;
                var cell_2_Row3 = row3.CreateCell(2);
                cell_2_Row3.CellStyle = borderedCellStyle;

                var row4 = sheet.CreateRow(4);
                var cell_0_Row4 = row4.CreateCell(0);
                cell_0_Row4.SetCellValue("Số điện thoại: - ");
                cell_0_Row4.CellStyle = borderedCellStyle;
                var cell_1_Row4 = row4.CreateCell(1);
                cell_1_Row4.SetCellValue("Địa chỉ: - ");
                cell_1_Row4.CellStyle = borderedCellStyle;
                var cell_2_Row4 = row4.CreateCell(2);
                cell_2_Row4.SetCellValue("TaxCode: - ");
                cell_2_Row4.CellStyle = borderedCellStyle;
                #endregion

                #region Header Table
                var row6 = sheet.CreateRow(6);
                var cell_0_Row6 = row6.CreateCell(0);
                cell_0_Row6.SetCellValue("Tổng dữ liệu");
                cell_0_Row6.CellStyle = borderedCellStyle;
                var cell_1_Row6 = row6.CreateCell(1);
                cell_1_Row6.CellStyle = borderedCellStyle;
                var cell_2_Row6 = row6.CreateCell(2);
                cell_2_Row6.CellStyle = borderedCellStyle;
                var cell_3_Row6 = row6.CreateCell(3);
                cell_3_Row6.CellStyle = borderedCellStyle;
                var cell_4_Row6 = row6.CreateCell(4);
                cell_4_Row6.CellStyle = borderedCellStyle;
                var cell_5_Row6 = row6.CreateCell(5);
                cell_5_Row6.CellStyle = borderedCellStyle;
                var cell_6_Row6 = row6.CreateCell(6);
                cell_6_Row6.CellStyle = borderedCellStyle;
                var cell_7_Row6 = row6.CreateCell(7);
                cell_7_Row6.SetCellValue("0 ₫");
                cell_7_Row6.CellStyle = borderedCellStyle;
                var cell_8_Row6 = row6.CreateCell(8);
                cell_8_Row6.SetCellValue("0 ₫");
                cell_8_Row6.CellStyle = borderedCellStyle;
                var cell_9_Row6 = row6.CreateCell(9);
                cell_9_Row6.SetCellValue("0 ₫");
                cell_9_Row6.CellStyle = borderedCellStyle;
                var cell_10_Row6 = row6.CreateCell(10);
                cell_10_Row6.SetCellValue("0 ₫");
                cell_10_Row6.CellStyle = borderedCellStyle;
                var cell_11_Row6 = row6.CreateCell(11);
                cell_11_Row6.CellStyle = borderedCellStyle;
                var cell_12_Row6 = row6.CreateCell(12);
                cell_12_Row6.SetCellValue("0 ₫");
                cell_12_Row6.CellStyle = borderedCellStyle;
                #endregion

                #region Footer
                var rowIndex = 10;
                var newRowSum1 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum1 = newRowSum1.CreateCell(0);
                cell_0_RowSum1.SetCellValue("Cước khác: ");
                cell_0_RowSum1.CellStyle = borderedCellStyle;
                var cell_1_RowSum1 = newRowSum1.CreateCell(1);
                cell_1_RowSum1.SetCellValue(" - ");
                cell_1_RowSum1.CellStyle = borderedCellStyle;
                var cell_2_RowSum1 = newRowSum1.CreateCell(2);
                cell_2_RowSum1.CellStyle = borderedCellStyle;
                var cell_3_RowSum1 = newRowSum1.CreateCell(3);
                cell_3_RowSum1.SetCellValue(" - ");
                cell_3_RowSum1.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum2 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum2 = newRowSum2.CreateCell(0);
                cell_0_RowSum2.SetCellValue("Cước điều chỉnh: ");
                cell_0_RowSum2.CellStyle = borderedCellStyle;
                var cell_1_RowSum2 = newRowSum2.CreateCell(1);
                cell_1_RowSum2.SetCellValue(" - ");
                cell_1_RowSum2.CellStyle = borderedCellStyle;
                var cell_2_RowSum2 = newRowSum2.CreateCell(2);
                cell_2_RowSum2.CellStyle = borderedCellStyle;
                var cell_3_RowSum2 = newRowSum2.CreateCell(3);
                cell_3_RowSum2.SetCellValue(" - ");
                cell_3_RowSum2.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum3 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum3 = newRowSum3.CreateCell(0);
                cell_0_RowSum3.SetCellValue("Trong nước: ");
                cell_0_RowSum3.CellStyle = borderedCellStyle;
                var cell_1_RowSum3 = newRowSum3.CreateCell(1);
                cell_1_RowSum3.SetCellValue(" - ");
                cell_1_RowSum3.CellStyle = borderedCellStyle;
                var cell_2_RowSum3 = newRowSum3.CreateCell(2);
                cell_2_RowSum3.SetCellValue(" - ");
                cell_2_RowSum3.CellStyle = borderedCellStyle;
                var cell_3_RowSum3 = newRowSum3.CreateCell(3);
                cell_3_RowSum3.SetCellValue(" - ");
                cell_3_RowSum3.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum4 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum4 = newRowSum4.CreateCell(0);
                cell_0_RowSum4.SetCellValue("Thoả thuận: ");
                cell_0_RowSum4.CellStyle = borderedCellStyle;
                var cell_1_RowSum4 = newRowSum4.CreateCell(1);
                cell_1_RowSum4.SetCellValue(" - ");
                cell_1_RowSum4.CellStyle = borderedCellStyle;
                var cell_2_RowSum4 = newRowSum4.CreateCell(2);
                cell_2_RowSum4.SetCellValue(" - ");
                cell_2_RowSum4.CellStyle = borderedCellStyle;
                var cell_3_RowSum4 = newRowSum4.CreateCell(3);
                cell_3_RowSum4.SetCellValue(" - ");
                cell_3_RowSum4.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum5 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum5 = newRowSum5.CreateCell(0);
                cell_0_RowSum5.SetCellValue("Quốc tế: ");
                cell_0_RowSum5.CellStyle = borderedCellStyle;
                var cell_1_RowSum5 = newRowSum5.CreateCell(1);
                cell_1_RowSum5.SetCellValue(" - ");
                cell_1_RowSum5.CellStyle = borderedCellStyle;
                var cell_2_RowSum5 = newRowSum5.CreateCell(2);
                cell_2_RowSum5.SetCellValue(" - ");
                cell_2_RowSum5.CellStyle = borderedCellStyle;
                var cell_3_RowSum5 = newRowSum5.CreateCell(3);
                cell_3_RowSum5.SetCellValue(" - ");
                cell_3_RowSum5.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum6 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum6 = newRowSum6.CreateCell(0);
                cell_0_RowSum6.SetCellValue("Quốc tế Priority: ");
                cell_0_RowSum6.CellStyle = borderedCellStyle;
                var cell_1_RowSum6 = newRowSum6.CreateCell(1);
                cell_1_RowSum6.SetCellValue(" - ");
                cell_1_RowSum6.CellStyle = borderedCellStyle;
                var cell_2_RowSum6 = newRowSum6.CreateCell(2);
                cell_2_RowSum6.SetCellValue(" - ");
                cell_2_RowSum6.CellStyle = borderedCellStyle;
                var cell_3_RowSum6 = newRowSum6.CreateCell(3);
                cell_3_RowSum6.SetCellValue(" - ");
                cell_3_RowSum6.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum7 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum7 = newRowSum7.CreateCell(0);
                cell_0_RowSum7.SetCellValue("Thương mại điện tử: ");
                cell_0_RowSum7.CellStyle = borderedCellStyle;
                var cell_1_RowSum7 = newRowSum7.CreateCell(1);
                cell_1_RowSum7.SetCellValue(" - ");
                cell_1_RowSum7.CellStyle = borderedCellStyle;
                var cell_2_RowSum7 = newRowSum7.CreateCell(2);
                cell_2_RowSum7.SetCellValue(" - ");
                cell_2_RowSum7.CellStyle = borderedCellStyle;
                var cell_3_RowSum7 = newRowSum7.CreateCell(3);
                cell_3_RowSum7.SetCellValue(" - ");
                cell_3_RowSum7.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum8 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum8 = newRowSum8.CreateCell(0);
                cell_0_RowSum8.SetCellValue("Chuyển hoàn: ");
                cell_0_RowSum8.CellStyle = borderedCellStyle;
                var cell_1_RowSum8 = newRowSum8.CreateCell(1);
                cell_1_RowSum8.SetCellValue(" - ");
                cell_1_RowSum8.CellStyle = borderedCellStyle;
                var cell_2_RowSum8 = newRowSum8.CreateCell(2);
                cell_2_RowSum8.SetCellValue(" - ");
                cell_2_RowSum8.CellStyle = borderedCellStyle;
                var cell_3_RowSum8 = newRowSum8.CreateCell(3);
                cell_3_RowSum8.SetCellValue(" - ");
                cell_3_RowSum8.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum9 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum9 = newRowSum9.CreateCell(0);
                cell_0_RowSum9.SetCellValue("Hoàn cước: ");
                cell_0_RowSum9.CellStyle = borderedCellStyle;
                var cell_1_RowSum9 = newRowSum9.CreateCell(1);
                cell_1_RowSum9.SetCellValue(" - ");
                cell_1_RowSum9.CellStyle = borderedCellStyle;
                var cell_2_RowSum9 = newRowSum9.CreateCell(2);
                cell_2_RowSum9.CellStyle = borderedCellStyle;
                var cell_3_RowSum9 = newRowSum9.CreateCell(3);
                cell_3_RowSum9.SetCellValue(" - ");
                cell_3_RowSum9.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum10 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum10 = newRowSum10.CreateCell(0);
                cell_0_RowSum10.SetCellValue("Truy thu: ");
                cell_0_RowSum10.CellStyle = borderedCellStyle;
                var cell_1_RowSum10 = newRowSum10.CreateCell(1);
                cell_1_RowSum10.SetCellValue(" - ");
                cell_1_RowSum10.CellStyle = borderedCellStyle;
                var cell_2_RowSum10 = newRowSum10.CreateCell(2);
                cell_2_RowSum10.CellStyle = borderedCellStyle;
                var cell_3_RowSum10 = newRowSum10.CreateCell(3);
                cell_3_RowSum10.SetCellValue(" - ");
                cell_3_RowSum10.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum11 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum11 = newRowSum11.CreateCell(0);
                cell_0_RowSum11.SetCellValue("Khuyến mãi theo CV: ");
                cell_0_RowSum11.CellStyle = borderedCellStyle;
                var cell_1_RowSum11 = newRowSum11.CreateCell(1);
                cell_1_RowSum11.SetCellValue(" - ");
                cell_1_RowSum11.CellStyle = borderedCellStyle;
                var cell_2_RowSum11 = newRowSum11.CreateCell(2);
                cell_2_RowSum11.CellStyle = borderedCellStyle;
                var cell_3_RowSum11 = newRowSum11.CreateCell(3);
                cell_3_RowSum11.SetCellValue(" - ");
                cell_3_RowSum11.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum12 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum12 = newRowSum12.CreateCell(0);
                cell_0_RowSum12.SetCellValue("Cước điều chỉnh khác: ");
                cell_0_RowSum12.CellStyle = borderedCellStyle;
                var cell_1_RowSum12 = newRowSum12.CreateCell(1);
                cell_1_RowSum12.SetCellValue(" - ");
                cell_1_RowSum12.CellStyle = borderedCellStyle;
                var cell_2_RowSum12 = newRowSum12.CreateCell(2);
                cell_2_RowSum12.CellStyle = borderedCellStyle;
                var cell_3_RowSum12 = newRowSum12.CreateCell(3);
                cell_3_RowSum12.SetCellValue(" - ");
                cell_3_RowSum12.CellStyle = borderedCellStyle;

                rowIndex = rowIndex + 1;
                var newRowSum13 = sheet.CreateRow(rowIndex);
                var cell_0_RowSum13 = newRowSum13.CreateCell(0);
                cell_0_RowSum13.SetCellValue("Tổng tiền TT sau khi chiết khấu: ");
                cell_0_RowSum13.CellStyle = borderedCellStyle;
                var cell_1_RowSum13 = newRowSum13.CreateCell(1);
                cell_1_RowSum13.CellStyle = borderedCellStyle;
                var cell_2_RowSum13 = newRowSum13.CreateCell(2);
                cell_2_RowSum13.CellStyle = borderedCellStyle;
                var cell_3_RowSum13 = newRowSum13.CreateCell(3);
                cell_3_RowSum13.SetCellValue(" - ");
                cell_3_RowSum13.CellStyle = borderedCellStyle;
                #endregion
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "GiayBaoCuocPhi_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls");
        }
        public ActionResult ExportPDFDataDebtNotices(ZReconciliationSearchDebtNoticesV1Request req)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReport"), "DebtNoticesCR.rpt"));
            rd.DataSourceConnections.Clear();

            DebtNoticesViewModel model = ReconciliationRepository.DataDebtNotices(req);
            if (model != null)
            {
                DataSet.DebtNoticesDS ds = new DataSet.DebtNoticesDS();
                for (int i = 0; i < model.DataInfoDetails.Count; i++)
                {
                    System.Data.DataRow dr = ds.Tables["DebtNoticesDataInfoDetails"].NewRow();
                    dr["ItemCode"] = model.DataInfoDetails[i].ItemCode;
                    dr["ReferenceNumber"] = model.DataInfoDetails[i].ReferenceNumber;
                    dr["ReleaseDate"] = model.DataInfoDetails[i].ReleaseDate;
                    dr["PostOfficeExploit"] = model.DataInfoDetails[i].PostOfficeExploit;
                    dr["PostOfficeReturn"] = model.DataInfoDetails[i].PostOfficeReturn;
                    dr["Weight"] = model.DataInfoDetails[i].Weight;
                    dr["MainFee"] = model.DataInfoDetails[i].MainFee;
                    dr["CODFee"] = model.DataInfoDetails[i].CODFee;
                    dr["ServiceFee"] = model.DataInfoDetails[i].ServiceFee;
                    dr["AdjustmentFee"] = model.DataInfoDetails[i].AdjustmentFee;
                    dr["TotalCharge"] = (model.DataInfoDetails[i].MainFee + model.DataInfoDetails[i].CODFee + model.DataInfoDetails[i].ServiceFee + model.DataInfoDetails[i].AdjustmentFee);
                    dr["Content"] = model.DataInfoDetails[i].Content;
                    ds.Tables["DebtNoticesDataInfoDetails"].Rows.Add(dr);
                }

                rd.Database.Tables[0].SetDataSource(ds.Tables[1]);
                rd.SetParameterValue("PrCustomerCode", model.DataInfo.CustomerCode == null ? "" : model.DataInfo.CustomerCode);
                rd.SetParameterValue("PrCustomerName", model.DataInfo.CustomerName == null ? "" : model.DataInfo.CustomerName);
                rd.SetParameterValue("PrMobileNumber", model.DataInfo.MobileNumber == null ? "" : model.DataInfo.MobileNumber);
                rd.SetParameterValue("PrTaxCode", model.DataInfo.TaxCode == null ? "" : model.DataInfo.TaxCode);
                rd.SetParameterValue("PrAddress", model.DataInfo.Address == null ? "" : model.DataInfo.Address);

                rd.SetParameterValue("PrSumMainFee", model.SumMainFee);
                rd.SetParameterValue("PrSumCODFee", model.SumCODFee);
                rd.SetParameterValue("PrSumServiceFee", model.SumServiceFee);
                rd.SetParameterValue("PrSumAdjustmentFee", model.SumAdjustmentFee);
                rd.SetParameterValue("PrSumTotalCharge", model.SumTotalCharge);

                rd.SetParameterValue("PrTotalRow1Col2", model.DataInfo.OtherCharges);
                rd.SetParameterValue("PrTotalRow1Col4", model.DataInfo.OtherCharges);

                rd.SetParameterValue("PrTotalRow2Col2", model.DataInfo.AdjustmentFeeRevenue - model.SumAdjustmentFee);
                rd.SetParameterValue("PrTotalRow2Col4", model.DataInfo.AdjustmentFeeRevenue - model.SumAdjustmentFee);

                rd.SetParameterValue("PrTotalRow3Col2", model.DataInfo.DomesticRevenue);
                rd.SetParameterValue("PrTotalRow3Col3", model.DataInfo.DomesticRate);
                rd.SetParameterValue("PrTotalRow3Col4", Math.Round((double)(model.DataInfo.DomesticRevenue * model.DataInfo.DomesticRate) / 100));

                rd.SetParameterValue("PrTotalRow4Col2", model.DataInfo.NegotiableRevenue);
                rd.SetParameterValue("PrTotalRow4Col3", model.DataInfo.AgreementRate);
                rd.SetParameterValue("PrTotalRow4Col4", Math.Round((double)(model.DataInfo.NegotiableRevenue * model.DataInfo.AgreementRate) / 100));

                rd.SetParameterValue("PrTotalRow5Col2", model.DataInfo.InternationalRevenue);
                rd.SetParameterValue("PrTotalRow5Col3", model.DataInfo.InternationalRate);
                rd.SetParameterValue("PrTotalRow5Col4", Math.Round((double)(model.DataInfo.InternationalRevenue * model.DataInfo.InternationalRate) / 100));

                rd.SetParameterValue("PrTotalRow6Col2", model.DataInfo.InternationalPriorityRevenue);
                rd.SetParameterValue("PrTotalRow6Col3", model.DataInfo.PriorityRate);
                rd.SetParameterValue("PrTotalRow6Col4", Math.Round((double)(model.DataInfo.InternationalPriorityRevenue * model.DataInfo.PriorityRate) / 100));

                rd.SetParameterValue("PrTotalRow7Col2", model.DataInfo.EcommerceRevenue);
                rd.SetParameterValue("PrTotalRow7Col3", model.DataInfo.EcommerceRate);
                rd.SetParameterValue("PrTotalRow7Col4", Math.Round((double)(model.DataInfo.EcommerceRevenue * model.DataInfo.EcommerceRate) / 100));

                rd.SetParameterValue("PrTotalRow8Col2", model.DataInfo.PermuteRevenue);
                rd.SetParameterValue("PrTotalRow8Col3", model.DataInfo.ReturnRate);
                rd.SetParameterValue("PrTotalRow8Col4", Math.Round((double)(model.DataInfo.PermuteRevenue * model.DataInfo.ReturnRate) / 100));

                rd.SetParameterValue("PrTotalRow9Col2", model.DataInfo.RefundRevenue);
                rd.SetParameterValue("PrTotalRow9Col4", model.DataInfo.RefundRevenue);

                rd.SetParameterValue("PrTotalRow10Col2", model.DataInfo.ArrearsRevenue);
                rd.SetParameterValue("PrTotalRow10Col4", model.DataInfo.ArrearsRevenue);

                rd.SetParameterValue("PrTotalRow11Col2", model.DataInfo.PromotionCVRevenue);
                rd.SetParameterValue("PrTotalRow11Col4", model.DataInfo.PromotionCVRevenue);

                rd.SetParameterValue("PrTotalRow12Col2", model.DataInfo.OtherAdjustmentChargesRevenue);
                rd.SetParameterValue("PrTotalRow12Col4", model.DataInfo.OtherAdjustmentChargesRevenue);

                double Sum = (((double)(model.DataInfo.DomesticRevenue * model.DataInfo.DomesticRate) / 100) +
                        ((double)(model.DataInfo.PermuteRevenue * model.DataInfo.ReturnRate) / 100) +
                        ((double)(model.DataInfo.InternationalRevenue * model.DataInfo.InternationalRate) / 100) +
                        ((double)(model.DataInfo.NegotiableRevenue * model.DataInfo.AgreementRate) / 100) +
                        ((double)(model.DataInfo.EcommerceRevenue * model.DataInfo.EcommerceRate) / 100) +
                        (model.DataInfo.ArrearsRevenue + model.DataInfo.RefundRevenue));
                rd.SetParameterValue("PrTotalRow13Col4", Math.Round((double)((model.SumTotalCharge + (model.DataInfo.AdjustmentFeeRevenue - model.SumAdjustmentFee) + model.DataInfo.OtherCharges) - Sum)));
            }
            else
            {
                rd.SetDataSource("");
                rd.SetParameterValue("PrCustomerCode", "");
                rd.SetParameterValue("PrCustomerName", "");
                rd.SetParameterValue("PrMobileNumber", "");
                rd.SetParameterValue("PrTaxCode", "");
                rd.SetParameterValue("PrAddress", "");

                rd.SetParameterValue("PrSumMainFee", 0);
                rd.SetParameterValue("PrSumCODFee", 0);
                rd.SetParameterValue("PrSumServiceFee", 0);
                rd.SetParameterValue("PrSumAdjustmentFee", 0);
                rd.SetParameterValue("PrSumTotalCharge", 0);

                rd.SetParameterValue("PrTotalRow1Col2", 0);
                rd.SetParameterValue("PrTotalRow1Col4", 0);

                rd.SetParameterValue("PrTotalRow2Col2", 0);
                rd.SetParameterValue("PrTotalRow2Col4", 0);

                rd.SetParameterValue("PrTotalRow3Col2", 0);
                rd.SetParameterValue("PrTotalRow3Col3", 0);
                rd.SetParameterValue("PrTotalRow3Col4", 0);

                rd.SetParameterValue("PrTotalRow4Col2", 0);
                rd.SetParameterValue("PrTotalRow4Col3", 0);
                rd.SetParameterValue("PrTotalRow4Col4", 0);

                rd.SetParameterValue("PrTotalRow5Col2", 0);
                rd.SetParameterValue("PrTotalRow5Col3", 0);
                rd.SetParameterValue("PrTotalRow5Col4", 0);

                rd.SetParameterValue("PrTotalRow6Col2", 0);
                rd.SetParameterValue("PrTotalRow6Col3", 0);
                rd.SetParameterValue("PrTotalRow6Col4", 0);

                rd.SetParameterValue("PrTotalRow7Col2", 0);
                rd.SetParameterValue("PrTotalRow7Col3", 0);
                rd.SetParameterValue("PrTotalRow7Col4", 0);

                rd.SetParameterValue("PrTotalRow8Col2", 0);
                rd.SetParameterValue("PrTotalRow8Col3", 0);
                rd.SetParameterValue("PrTotalRow8Col4", 0);

                rd.SetParameterValue("PrTotalRow9Col2", 0);
                rd.SetParameterValue("PrTotalRow9Col4", 0);

                rd.SetParameterValue("PrTotalRow10Col2", 0);
                rd.SetParameterValue("PrTotalRow10Col4", 0);

                rd.SetParameterValue("PrTotalRow11Col2", 0);
                rd.SetParameterValue("PrTotalRow11Col4", 0);

                rd.SetParameterValue("PrTotalRow12Col2", 0);
                rd.SetParameterValue("PrTotalRow12Col4", 0);

                rd.SetParameterValue("PrTotalRow13Col4", 0);
            }
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "GiayBaoCuocPhi_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf");
        }
        #endregion
    }
}