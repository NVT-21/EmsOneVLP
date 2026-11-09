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
    public class PaymentController : BaseController
    {
        // GET: Payment
        [AuthorizeController]
        public ActionResult PaymentCOD()
        {
            ViewBag.ReconciliationStatus = XData.GetListReconciliationStatus();
            ViewBag.PaymentStatus = XData.GetListPaymentStatus();
            return View();
        }
        [AuthorizeController]
        public ActionResult GetDataPaymentCOD(ZPaymentListCODRequest req)
        {
            List<PaymentListCODViewModel> model = new List<PaymentListCODViewModel>();
            long SumCOD = 0;
            string _paymentstatus;
            if (req.PaymentStatus == null)
            {
                _paymentstatus = "";
            }
            else
            {
                _paymentstatus = req.PaymentStatus == "0" ? "N" : "Y";
            }
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PAYMENT_COD_LIST",
                Data = JsonConvert.SerializeObject(new ZPaymentListCODRequest
                {
                    CustomerCode = req.CustomerCode,
                    CustomerPPCRM = req.CustomerPPCRM,
                    Status = req.Status,
                    PaymentStatus = _paymentstatus,
                    ApprovalDate = req.ApprovalDate,
                    OrderCode = req.OrderCode,
                    EmsBarCode = req.EmsBarCode
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZPaymentListCODResponse>>(res.Data);
                foreach (var item in list)
                {
                    SumCOD += item.CODMoney;
                    model.Add(new PaymentListCODViewModel()
                    {
                        ID = item.ID,
                        CustomerCode = item.CustomerCode,
                        CustomerPPCRM = item.CustomerPPCRM,
                        ShopID = item.ShopID,
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        EmsBarCode = item.EmsBarCode,
                        CODMoney = item.CODMoney,
                        IsAgree = item.IsAgree,
                        IsPayment = item.IsPayment,
                        StatusIsAgree = XData.GetNameFreightReconciliationStatus(item.IsAgree)
                    });
                }
            }
            ViewBag.SumCOD = SumCOD;
            return View(model);
        }
        public JsonResult JIsPaymentCOD(int ID)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                var oUpdatedBy = XHelper.Session.UserProfile.ID;
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PAYMENT_COD_UPDATE",
                    Data = JsonConvert.SerializeObject(new ZPaymentUpdateCODRequest()
                    {
                        ID = ID,
                        UpdatedBy = oUpdatedBy
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "Payment.JIsPaymentCOD: " + ex.Message);
            }
            jResult = Json(res, JsonRequestBehavior.AllowGet);
            return jResult;
        }
        public JsonResult JIsPaymentCODMultiple(List<PaymentCODMultipleModel> arr)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            List<XShowListErrorMultipleSelectModel> ListItem = new List<XShowListErrorMultipleSelectModel>();
            try
            {
                var oUpdatedBy = XHelper.Session.UserProfile.ID;
                foreach (var req in arr)
                {
                    res = XHelper.Invoke(new ZRequest()
                    {
                        Code = "PAYMENT_COD_UPDATE",
                        Data = JsonConvert.SerializeObject(new ZPaymentUpdateCODRequest()
                        {
                            ID = req.ID,
                            UpdatedBy = oUpdatedBy
                        })
                    });
                    if (res.Code != "00")
                    {
                        ListItem.Add(new XShowListErrorMultipleSelectModel()
                        {
                            EmsBarCode = req.EmsBarCode,
                            Description = res.Message
                        });
                    }
                }
                if (ListItem.Count > 0)
                {
                    var TotalSuccess = arr.Count - ListItem.Count;
                    res.Code = "01";
                    res.Message = "Xác nhận thanh toán thành công " + TotalSuccess + "/" + arr.Count + " đơn hàng, vui lòng nhấn vào Xem chi tiết để kiểm tra.";
                    res.Data = JsonConvert.SerializeObject(ListItem);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Xác nhận thanh toán thành công.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "Payment.JIsPaymentCODMultiple: " + ex.Message);
            }
            jResult = Json(res, JsonRequestBehavior.AllowGet);
            return jResult;
        }
        public ActionResult ExportExcelDataPaymentCOD(ZPaymentListCODRequest req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            List<PaymentListCODViewModel> model = new List<PaymentListCODViewModel>();
            long SumCOD = 0;
            string _paymentstatus;
            if (req.PaymentStatus == null)
            {
                _paymentstatus = "";
            }
            else
            {
                _paymentstatus = req.PaymentStatus == "0" ? "N" : "Y";
            }
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PAYMENT_COD_LIST",
                Data = JsonConvert.SerializeObject(new ZPaymentListCODRequest
                {
                    CustomerCode = req.CustomerCode,
                    CustomerPPCRM = req.CustomerPPCRM,
                    Status = req.Status,
                    PaymentStatus = _paymentstatus,
                    ApprovalDate = req.ApprovalDate,
                    OrderCode = req.OrderCode,
                    EmsBarCode = req.EmsBarCode
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZPaymentListCODResponse>>(res.Data);
                int stt = 1;
                foreach (var item in list)
                {
                    SumCOD += item.CODMoney;
                }
                ISheet sheet = wb.CreateSheet();
                sheet.SetColumnWidth(0,2000);
                sheet.SetColumnWidth(1,6000);
                sheet.SetColumnWidth(2,6000);
                sheet.SetColumnWidth(3,6000);
                sheet.SetColumnWidth(4,6000);
                sheet.SetColumnWidth(5,6000);
                sheet.SetColumnWidth(6,6000);
                sheet.SetColumnWidth(7,6000);
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
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 7);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("Danh sách quản lý thanh toán tiền COD");

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
                cell_5_Row2.SetCellValue(double.Parse(SumCOD.ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.CellStyle = borderedCellStyle;

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
                cell_3_Row3.SetCellValue("Mã bưu gửi");
                cell_3_Row3.CellStyle = borderedCellStyle;

                var cell_4_Row3 = row3.CreateCell(4);
                cell_4_Row3.SetCellValue("Mã đơn hàng");
                cell_4_Row3.CellStyle = borderedCellStyle;

                var cell_5_Row3 = row3.CreateCell(5);
                cell_5_Row3.SetCellValue("Tiền COD");
                cell_5_Row3.CellStyle = borderedCellStyle;

                var cell_6_Row3 = row3.CreateCell(6);
                cell_6_Row3.SetCellValue("Trạng thái đối soát");
                cell_6_Row3.CellStyle = borderedCellStyle;

                var cell_7_Row3 = row3.CreateCell(7);
                cell_7_Row3.SetCellValue("Trạng thái thanh toán");
                cell_7_Row3.CellStyle = borderedCellStyle;

                // bắt đầu duyệt mảng và ghi tiếp tục
                int rowIndex = 4;
                foreach (var item in list)
                {
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    // set giá trị
                    var cell_0_newRow = newRow.CreateCell(0);
                    cell_0_newRow.SetCellValue(stt++);
                    cell_0_newRow.CellStyle = borderedCellStyle;

                    var cell_1_newRow = newRow.CreateCell(1);
                    cell_1_newRow.CellStyle = borderedCellStyle;
                    cell_1_newRow.SetCellValue(item.CustomerCode);

                    var cell_2_newRow = newRow.CreateCell(2);
                    cell_2_newRow.CellStyle = borderedCellStyle;
                    cell_2_newRow.SetCellValue(item.CustomerPPCRM);

                    var cell_3_newRow = newRow.CreateCell(3);
                    cell_3_newRow.CellStyle = borderedCellStyle;
                    cell_3_newRow.SetCellValue(item.EmsBarCode);

                    var cell_4_newRow = newRow.CreateCell(4);
                    cell_4_newRow.CellStyle = borderedCellStyle;
                    cell_4_newRow.SetCellValue(item.OrderCode);

                    var cell_5_newRow = newRow.CreateCell(5);
                    cell_5_newRow.CellStyle = borderedCellStyle;
                    cell_5_newRow.SetCellValue(double.Parse(item.CODMoney.ToString()).ToString("#,###", cul.NumberFormat));

                    var cell_6_newRow = newRow.CreateCell(6);
                    cell_6_newRow.CellStyle = borderedCellStyle;
                    cell_6_newRow.SetCellValue(XData.GetNameFreightReconciliationStatus(item.IsAgree));

                    var cell_7_newRow = newRow.CreateCell(7);
                    cell_7_newRow.CellStyle = borderedCellStyle;
                    cell_7_newRow.SetCellValue(XData.GetNamePaymentStatus(item.IsPayment == "Y" ? "1" : "0"));
                    rowIndex++;
                }
            }
            else
            {
                ISheet sheet = wb.CreateSheet();
                sheet.SetColumnWidth(0, 2000);
                sheet.SetColumnWidth(1, 6000);
                sheet.SetColumnWidth(2, 6000);
                sheet.SetColumnWidth(3, 6000);
                sheet.SetColumnWidth(4, 6000);
                sheet.SetColumnWidth(5, 6000);
                sheet.SetColumnWidth(6, 6000);
                sheet.SetColumnWidth(7, 6000);
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
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 7);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("Danh sách quản lý thanh toán tiền COD");

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
                cell_5_Row2.SetCellValue(double.Parse(SumCOD.ToString()).ToString("#,###", cul.NumberFormat) + " ₫");
                var cell_6_Row2 = row2.CreateCell(6);
                cell_6_Row2.CellStyle = borderedCellStyle;
                var cell_7_Row2 = row2.CreateCell(7);
                cell_7_Row2.CellStyle = borderedCellStyle;

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
                cell_3_Row3.SetCellValue("Mã bưu gửi");
                cell_3_Row3.CellStyle = borderedCellStyle;

                var cell_4_Row3 = row3.CreateCell(4);
                cell_4_Row3.SetCellValue("Mã đơn hàng");
                cell_4_Row3.CellStyle = borderedCellStyle;

                var cell_5_Row3 = row3.CreateCell(5);
                cell_5_Row3.SetCellValue("Tiền COD");
                cell_5_Row3.CellStyle = borderedCellStyle;

                var cell_6_Row3 = row3.CreateCell(6);
                cell_6_Row3.SetCellValue("Trạng thái đối soát");
                cell_6_Row3.CellStyle = borderedCellStyle;

                var cell_7_Row3 = row3.CreateCell(7);
                cell_7_Row3.SetCellValue("Trạng thái thanh toán");
                cell_7_Row3.CellStyle = borderedCellStyle;
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachQuanLyThanhToanTienCOD.xls");
        }
    }
}