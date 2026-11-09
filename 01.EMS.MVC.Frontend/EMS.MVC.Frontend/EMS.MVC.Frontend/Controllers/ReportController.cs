using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
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

namespace EMS.MVC.Frontend.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report

        #region Report COD
        public ActionResult CODMoneyReportView()
        {
            CODMoneyReportViewModel model = new CODMoneyReportViewModel();
            model = new CODMoneyReportViewModel()
            {
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListCODTransType = XData.GetListCODTransType()
            };
            return View(model);
        }
        public ActionResult DataCODMoneyReportView()
        {
            return PartialView();
        }
        public JsonResult JDataCODMoneyReport(CODMoneyReportModel model)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REPORT_COD_MONEY",
                    Data = JsonConvert.SerializeObject(new ZReportCODMoneyRequest
                    {
                        FromDate = model.FromDate,
                        ToDate = model.ToDate,
                        CustomerCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                        CODTransType = model.CODTransType
                        //FromDate = "01/01/2022",
                        //ToDate = "23/05/2023",
                        //CustomerCode = "12500000016427",
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ReportController.JDataCODMoneyReport: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DataTableCODMoneyReportView(List<CODMoneyReportDataViewModel> model)
        {
            return PartialView(model);
        }
        public ActionResult DataChartCODMoneyReportView(List<CODMoneyReportDataViewModel> model)
        {
            return PartialView(model);
        }
        public ActionResult ExcelCODMoneyReport(CODMoneyReportModel model)
        {
            ZResponse res = new ZResponse();
            HSSFWorkbook wb = new HSSFWorkbook();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"

            ISheet sheet = wb.CreateSheet("GIÁM SÁT ĐƠN HÀNG COD");

            #region SetColumnWidth
            sheet.SetColumnWidth(0, 6000);
            sheet.SetColumnWidth(1, 6000);
            sheet.SetColumnWidth(2, 6000);
            sheet.SetColumnWidth(3, 6000);
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
            CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 3);
            sheet.AddMergedRegion(cellMerge);
            row0.GetCell(0).SetCellValue("GIÁM SÁT SẢN LƯỢNG THEO TIỀN COD");
            row0.GetCell(0).CellStyle = _styleFontHeader;
            #endregion

            #region Title Table
            var row3 = sheet.CreateRow(3);
            row3.CreateCell(0).SetCellValue("STT");
            row3.GetCell(0).CellStyle = borderedCellStyle;
            row3.CreateCell(1).SetCellValue("Ngày phát hành");
            row3.GetCell(1).CellStyle = borderedCellStyle;
            row3.CreateCell(2).SetCellValue("Sản lượng (Bưu gửi)");
            row3.GetCell(2).CellStyle = borderedCellStyle;
            row3.CreateCell(3).SetCellValue("Số tiền (VNĐ)");
            row3.GetCell(3).CellStyle = borderedCellStyle;
            #endregion

            res = XHelper.Invoke(new ZRequest()
            {
                Code = "REPORT_COD_MONEY",
                Data = JsonConvert.SerializeObject(new ZReportCODMoneyRequest
                {
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    CustomerCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                    CODTransType = model.CODTransType
                    //FromDate = "01/01/2022",
                    //ToDate = "23/05/2023",
                    //CustomerCode = "12500000016427",
                })
            });
            if (res.Code == "00")
            {
                int stt = 1;
                var list = JsonConvert.DeserializeObject<List<ZReportCODMoneyResponse>>(res.Data);

                #region Header Table
                var row2 = sheet.CreateRow(2);
                row2.CreateCell(0).SetCellValue("Tổng dữ liệu");
                row2.GetCell(0).CellStyle = borderedCellStyle;
                row2.CreateCell(1).SetCellValue("");
                row2.GetCell(1).CellStyle = borderedCellStyle;
                row2.CreateCell(2).SetCellValue(list.Sum(m => m.Quantity) == 0 ? "0" : double.Parse(list.Sum(m => m.Quantity).ToString()).ToString("#,###", cul.NumberFormat));
                row2.GetCell(2).CellStyle = borderedCellStyle;
                row2.CreateCell(3).SetCellValue(list.Sum(m => m.AmountOfMoney) == 0 ? "0" : double.Parse(list.Sum(m => m.AmountOfMoney).ToString()).ToString("#,###", cul.NumberFormat));
                row2.GetCell(3).CellStyle = borderedCellStyle;
                #endregion

                #region Table
                int rowIndex = 4;
                foreach (var item in list)
                {
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRow.CreateCell(0).SetCellValue(stt++);
                    newRow.GetCell(0).CellStyle = borderedCellStyle;
                    newRow.CreateCell(1).SetCellValue(item.TimeRelease);
                    newRow.GetCell(1).CellStyle = borderedCellStyle;
                    newRow.CreateCell(2).SetCellValue(item.Quantity == 0 ? "0" : double.Parse(item.Quantity.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(2).CellStyle = borderedCellStyle;
                    newRow.CreateCell(3).SetCellValue(item.AmountOfMoney == 0 ? "0" : double.Parse(item.AmountOfMoney.ToString()).ToString("#,###", cul.NumberFormat));
                    newRow.GetCell(3).CellStyle = borderedCellStyle;
                    rowIndex++;
                }
                #endregion
            }
            else
            {
                #region Header Table
                var row2 = sheet.CreateRow(2);
                row2.CreateCell(0).SetCellValue("Tổng dữ liệu");
                row2.GetCell(0).CellStyle = borderedCellStyle;
                row2.CreateCell(1).SetCellValue("");
                row2.GetCell(1).CellStyle = borderedCellStyle;
                row2.CreateCell(2).SetCellValue("0");
                row2.GetCell(2).CellStyle = borderedCellStyle;
                row2.CreateCell(3).SetCellValue("0");
                row2.GetCell(3).CellStyle = borderedCellStyle;
                #endregion
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "giam-sat-san-luong-tien-cod-" + DateTime.Now.ToString("ddMMyyHHmmss") + ".xls");
        }
        #endregion
    }
}