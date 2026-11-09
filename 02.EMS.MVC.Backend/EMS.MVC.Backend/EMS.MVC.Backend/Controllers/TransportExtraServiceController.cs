using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Linq;

namespace EMS.MVC.Backend.Controllers
{
    public class TransportExtraServiceController : BaseController
    {
        #region Danh sách bảng cấu hình cước
        public ActionResult FreightChargesView()
        {
            return View();
        }

        public ActionResult SearchListFreightChargesView()
        {
            List<SearchListFreightChargesViewModel> model = new List<SearchListFreightChargesViewModel>();
            var res = TransportPostageRepository.SearchList(new ZTransportPostageSearchListRequest()
            {
                Type = 2
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new SearchListFreightChargesViewModel()
                    {
                        TransportPostageID = item.TransportPostageID,
                        TransportServiceCode = item.TransportServiceCode,
                        TransportServiceName = item.TransportServiceName,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        IsLock = item.IsLock,
                        VAT = new BaseModel()
                        {
                            Code = item.VAT.Code,
                            Name = item.VAT.Name
                        },
                        ListCrmOrPaypostCode = item.ListCrmOrPaypostCode
                    });
                }
            }
            return PartialView(model);
        }

        public ActionResult ExportListFreightCharges()
        {
            //Create new Excel Workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel Sheet
            var sheet = workbook.CreateSheet();

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
            TitleRow.CreateCell(0).SetCellValue("DANH SÁCH BẢNG CƯỚC DỊCH VỤ CỘNG THÊM");
            TitleRow.GetCell(0).CellStyle = TitleStyle;

            //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 12));
            //sheet.CreateRow(0).GetCell(0).CellStyle = center;

            List<ZTransportPostageSearchListResponse> ListItem = new List<ZTransportPostageSearchListResponse>();
            var res = TransportPostageRepository.SearchList(new ZTransportPostageSearchListRequest()
            {
                Type = 2
            });
            if (res.Code == "00")
            {
                ListItem = JsonConvert.DeserializeObject<List<ZTransportPostageSearchListResponse>>(res.Data);
            }

            //Create a header row
            var headerRow = sheet.CreateRow(1);
            int ColumnIndex = 0;

            headerRow.CreateCell(ColumnIndex++).SetCellValue("STT");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã dịch vụ");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Tên dịch vụ");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Đã bao gồm VAT");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Thời gian áp dụng");

            int rowNumber = 2;
            int rowIndex = 0;
            var stt = 1;

            foreach (var item in ListItem)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(rowIndex++).SetCellValue(stt.ToString());
                row.CreateCell(rowIndex++).SetCellValue(item.TransportServiceCode);
                row.CreateCell(rowIndex++).SetCellValue(item.TransportServiceName);
                row.CreateCell(rowIndex++).SetCellValue(item.VAT.Name);

                var Time = "Từ " + item.StartDate;
                if (!string.IsNullOrEmpty(item.EndDate))
                {
                    Time += " Đến " + item.EndDate;
                }

                row.CreateCell(rowIndex++).SetCellValue(Time);

                row.CreateCell(rowIndex).CellStyle.SetFont(fontChar);

                stt++;
                rowIndex = 0;
            }

            sheet.AutoSizeColumn(1);
            sheet.AutoSizeColumn(2);
            sheet.AutoSizeColumn(3);
            sheet.AutoSizeColumn(4);

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            //application/vnd.ms-excel xls
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet xlsx
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachBangCuocDichVuCongThem.xls");
        }
        #endregion

        #region Thêm bảng cấu hình cước
        public ActionResult AddFreightChargesView()
        {
            AddFreightChargesViewModel model = new AddFreightChargesViewModel()
            {
                ListChargingConditions = XData.GetListChargingConditions(), // DS điều kiện tính cước DV cộng thêm
            };

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { ParentID = 0, IsLock = "N", Channel = "EMSONE" });
            if (ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
            }
            return View(model);
        }

        public JsonResult JAddFreightCharges(AddFreightChargesViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.ExtraServiceBasicInfoAdd(new ZTransportPostageExtraServiceBasicAddRequest()
                {
                    ExtraTransportServiceID = Convert.ToInt32(req.ExtraTransportServiceID),
                    AppliesToShop = req.AppliesToShop,
                    SubjectOfApplication = req.SubjectOfApplication,
                    VAT = req.VAT,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    ChargingConditionsID = req.ChargingConditionsID,
                    ChargingConditionsValue = req.ChargingConditionsValue,
                    ChargingConditionsMinValue = req.ChargingConditionsMinValue,
                    ServicesUsed = req.ServicesUsed,
                    CreatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportExtraServiceController.JAddFreightCharges: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa bảng cấu hình cước
        public ActionResult EditFreightChargesView(SearchListFreightChargesViewModel req)
        {
            EditFreightChargesViewModel model = null;
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest() { TransportPostageID = req.TransportPostageID });
            if (res.Code == "00")
            {
                List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { ParentID = 0, IsLock = "N", Channel = "EMSONE" });
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new EditFreightChargesViewModel()
                {
                    TransportPostageID = item.TransportPostageID,
                    TransportServiceName = item.TransportServiceName,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    ChargingConditionsID = item.ChargingConditionsID,
                    ListChargingConditions = XData.GetListChargingConditions(), // DS điều kiện tính cước DV cộng thêm
                    ChargingConditionsValue = item.ChargingConditionsValue,
                    ChargingConditionsMinValue = item.ChargingConditionsMinValue,
                    ServicesUsed = item.ServicesUsed,
                    MainTransportServiceID = 0,
                    ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList(),
                    ExtraTransportServiceID = item.MainTransportServiceID.ToString(),
                    ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList(),
                    VAT = new BaseModel()
                    {
                        Code = item.VAT.Code,
                        Name = item.VAT.Name
                    },
                    AppliesToShop = item.AppliesToShop
                };
            }
            return View(model);
        }

        public JsonResult JEditFreightCharges(EditFreightChargesViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.ExtraServiceBasicInfoEdit(new ZTransportPostageExtraServiceEditRequest()
                {
                    TransportPostageID = req.TransportPostageID,
                    AppliesToShop = req.AppliesToShop,
                    SubjectOfApplication = req.SubjectOfApplication,
                    VAT = req.VAT.Code,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    ChargingConditionsID = req.ChargingConditionsID,
                    ChargingConditionsValue = req.ChargingConditionsValue,
                    ChargingConditionsMinValue = req.ChargingConditionsMinValue,
                    ServicesUsed = req.ServicesUsed,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportExtraServiceController.JEditFreightCharges: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #region Cấu hình cước dịch vụ cộng thêm
        public ActionResult ConfigMainFeeView(EditFreightChargesViewModel req)
        {
            ConfigMainFeeViewModel model = null;
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest()
            {
                TransportPostageID = req.TransportPostageID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new ConfigMainFeeViewModel()
                {
                    TransportPostageID = item.TransportPostageID,
                    TransportServiceName = item.TransportServiceName,
                    ChargingConditionsID = item.ChargingConditionsID,
                    ServicesUsed = item.ServicesUsed
                };
            }
            return View(model);
        }

        public ActionResult ConfigMainFeeDetailView(ConfigMainFeeViewModel req)
        {
            List<ConfigMainFeeDetailViewModel> model = new List<ConfigMainFeeDetailViewModel>();
            var res = TransportPostageDetailRepository.GetByTransportPostageID(new ZTransportPostageDetailGetByTransportPostageIDRequest() { TransportPostageID = req.TransportPostageID });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageDetailGetByTransportPostageIDResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new ConfigMainFeeDetailViewModel()
                    {
                        TransportPostageDetailID = item.ID,
                        TransportPostageDetailName = item.Name,
                        IsStep = item.IsStep,
                        Weight = item.Weight,
                        LocalFee = item.LocalFee,
                        IntercityFee1 = item.IntercityFee1,
                        IntercityFee2 = item.IntercityFee2,
                        IntercityFee3 = item.IntercityFee3,
                        IntercityFee4 = item.IntercityFee4,
                        IntercityFee5 = item.IntercityFee5,
                    });
                }
            }
            ViewBag.TransportPostageID = req.TransportPostageID;
            ViewBag.ChargingConditionsID = req.ChargingConditionsID;
            return PartialView(model);
        }

        #region Thêm cấu hình cước
        public JsonResult JAddConfigMainFeeDetailView(ConfigMainFeeDetailViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageDetailRepository.Add(new ZTransportPostageDetailAddRequest()
                {
                    PostageID = req.TransportPostageID,
                    IsStep = req.IsStep,
                    Weight = req.Weight,
                    LocalFee = req.LocalFee,
                    IntercityFee1 = req.IntercityFee1,
                    IntercityFee2 = req.IntercityFee2,
                    IntercityFee3 = req.IntercityFee3,
                    IntercityFee4 = req.IntercityFee4,
                    IntercityFee5 = req.IntercityFee5,
                    Key = req.Key,
                    CreatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportExtraServiceController.JAddConfigMainFeeDetailView: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa cấu hình cước
        public JsonResult JEditConfigMainFeeDetailView(ConfigMainFeeDetailViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageDetailRepository.Edit(new ZTransportPostageDetailEditRequest()
                {
                    ID = req.TransportPostageDetailID,
                    Weight = req.Weight,
                    LocalFee = req.LocalFee,
                    IntercityFee1 = req.IntercityFee1,
                    IntercityFee2 = req.IntercityFee2,
                    IntercityFee3 = req.IntercityFee3,
                    IntercityFee4 = req.IntercityFee4,
                    IntercityFee5 = req.IntercityFee5,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportExtraServiceController.JEditConfigMainFeeDetailView: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa cấu hình cước
        public JsonResult JRemoveConfigMainFeeDetailView(ConfigMainFeeDetailViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageDetailRepository.Remove(new ZTransportPostageDetailRemoveRequest()
                {
                    ID = req.TransportPostageDetailID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportExtraServiceController.JRemoveConfigMainFeeDetailView: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
        #endregion

        #region Chi tiết cấu hình cước
        public ActionResult DetailFreightChargesView(SearchListFreightChargesViewModel req)
        {
            DetailFreightChargesViewModel model = null;
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest() { TransportPostageID = req.TransportPostageID });
            if (res.Code == "00")
            {
                List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { ParentID = 0, IsLock = "N", Channel = "EMSONE" });
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new DetailFreightChargesViewModel()
                {
                    TransportPostageID = item.TransportPostageID,
                    TransportServiceName = item.TransportServiceName,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    ChargingConditionsID = item.ChargingConditionsID,
                    ListChargingConditions = XData.GetListChargingConditions(), // DS điều kiện tính cước DV cộng thêm
                    ChargingConditionsValue = item.ChargingConditionsValue,
                    ChargingConditionsMinValue = item.ChargingConditionsMinValue,
                    ServicesUsed = item.ServicesUsed,
                    MainTransportServiceID = 0,
                    ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList(),
                    ExtraTransportServiceID = item.MainTransportServiceID.ToString(),
                    ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList(),
                    VAT = new BaseModel()
                    {
                        Code = item.VAT.Code,
                        Name = item.VAT.Name
                    },
                    AppliesToShop = item.AppliesToShop
                };
            }
            return View(model);
        }
        #endregion

        #region Lịch sử cấu hình cước
        public ActionResult HistoryFreightChargesView(DetailFreightChargesViewModel req)
        {
            List<EditHistoryTabViewModel> model = new List<EditHistoryTabViewModel>();
            var res = TransportPostageRepository.SearchHistory(new ZTransportPostageSearchHistoryRequest()
            {
                TransportPostageID = req.TransportPostageID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageSearchHistoryResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new EditHistoryTabViewModel()
                    {
                        Name = item.Name,
                        Description = item.Description,
                        CreatedName = item.CreatedName,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Mở/ Khóa bảng cấu hình cước
        public JsonResult JChangeIsLock(SearchListFreightChargesViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.ChangeIsLock(new ZTransportPostageChangeIsLockRequest()
                {
                    ID = req.TransportPostageID,
                    IsLock = req.IsLock,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportExtraServiceController.JChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}