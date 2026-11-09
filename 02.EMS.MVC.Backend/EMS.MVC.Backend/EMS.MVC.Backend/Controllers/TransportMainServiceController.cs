using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.SS.Util;

namespace EMS.MVC.Backend.Controllers
{
    public class TransportMainServiceController : BaseController
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
                Type = 1
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new SearchListFreightChargesViewModel()
                    {
                        TransportPostageID = item.TransportPostageID,
                        TransportPostageCode = item.TransportPostageCode,
                        TransportPostageName = item.TransportPostageName,
                        TransportServiceCode = item.TransportServiceCode,
                        TransportServiceName = item.TransportServiceName,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        IsLock = item.IsLock,
                        Status = new BaseModel()
                        {
                            Code = item.Status.Code,
                            Name = item.Status.Name
                        }
                    });
                }
            }
            return PartialView(model);
        }

        #endregion

        #region Thêm bảng cước
        public ActionResult AddFreightChargesView()
        {
            AddFreightChargesViewModel model = new AddFreightChargesViewModel()
            {
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>(),
                ListSubjectOfApplication = XData.GetListSubjectsOfApplication(), //DS đối tượng áp dụng
                //ListShop = XCommon.SearchListShop(new ZShopSearchListRequest() { Status = "A" }), // DS Shop đã phê duyệt
                //ListShop = new List<ShopModel>(),
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),  //DS tỉnh
                ListProvinceApply = XData.GetListOptionProvinceApply(), //Tỉnh áp dụng
                ListAreaApply = XData.GetListOptionAreaApply(), // Vùng áp dụng
                //ListTransportPostageType = XData.GetListTransportPostageType(),
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
                res = TransportPostageRepository.MainServiceBasicInfoAdd(new ZTransportPostageMainServiceBasicAddRequest()
                {
                    TransportPostageCode = req.TransportPostageCode,
                    TransportPostageName = req.TransportPostageName,
                    //TransportPostageType = req.TransportPostageType,
                    //CrmOrPaypostCode = req.CrmOrPaypostCode,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    MainTransportServiceID = req.MainTransportServiceID,
                    ExtraTransportServiceID = req.ExtraTransportServiceID,
                    Area = req.Area,
                    ProvinceApply = req.ProvinceApply,
                    AreaApply = req.AreaApply,
                    SubjectOfApplication = req.SubjectOfApplication,
                    AppliesToShop = req.AppliesToShop,
                    AppliesToProvinceFrom = req.AppliesToProvinceFrom,
                    //ApplicationObjectValue = req.ApplicationObjectValue,
                    CreatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JAddFreightCharges: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa bảng cước
        public ActionResult EditFreightChargesView(AddFreightChargesViewModel req)
        {
            EditFreightChargesViewModel model = new EditFreightChargesViewModel()
            {
                TransportPostageID = req.TransportPostageID,
                TransportPostageCode = req.TransportPostageCode
            };
            return View(model);
        }

        #region Tab Thông tin cơ bản
        public ActionResult EditBasicInfoTabView(EditFreightChargesViewModel req)
        {
            EditBasicInfoTabViewModel model = null;
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest()
            {
                TransportPostageID = req.TransportPostageID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new EditBasicInfoTabViewModel()
                {
                    TransportPostageID = item.TransportPostageID,
                    TransportPostageCode = item.TransportPostageCode,
                    TransportPostageName = item.TransportPostageName,
                    //TransportPostageType = item.TransportPostageType,
                    //CrmOrPaypostCode = item.CrmOrPaypostCode,
                    //ListTransportPostageType = XData.GetListTransportPostageType(),
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    MainTransportServiceID = item.MainTransportServiceID,
                    ListMainTransportService = new List<TransportServiceModel>(),
                    ExtraTransportServiceID = item.ExtraTransportServiceID,
                    ListExtraTransportService = new List<TransportServiceModel>(),
                    Area = item.Area,
                    ProvinceApply = item.ProvinceApply,
                    ListProvinceApply = XData.GetListOptionProvinceApply(), //Tỉnh áp dụng
                    AreaApply = item.AreaApply,
                    ListAreaApply = XData.GetListOptionAreaApply(), // Vùng áp dụng
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),  //DS tỉnh
                    SubjectOfApplication = item.SubjectOfApplication,
                    ListSubjectOfApplication = XData.GetListSubjectsOfApplication(), //DS đối tượng áp dụng
                    //ApplicationObjectValue = item.ApplicationObjectValue,
                    AppliesToShop = item.AppliesToShop,
                    AppliesToProvinceFrom = item.AppliesToProvinceFrom,
                    Status = item.Status
                };

                List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { ParentID = 0, IsLock = "N", Channel = "EMSONE" });
                if (ListTransportService.Count > 0)
                {
                    model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                    model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
                }
            }
            return PartialView(model);
        }

        public JsonResult JEditBasicInfoTab(EditBasicInfoTabViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.MainServiceBasicInfoEdit(new ZTransportPostageMainServiceBasicEditRequest()
                {
                    TransportPostageID = req.TransportPostageID,
                    TransportPostageCode = req.TransportPostageCode,
                    TransportPostageName = req.TransportPostageName,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    MainTransportServiceID = req.MainTransportServiceID,
                    ExtraTransportServiceID = req.ExtraTransportServiceID,
                    Area = req.Area,
                    ProvinceApply = req.ProvinceApply,
                    AreaApply = req.AreaApply,
                    SubjectOfApplication = req.SubjectOfApplication,
                    AppliesToShop = req.AppliesToShop,
                    AppliesToProvinceFrom = req.AppliesToProvinceFrom,
                    UpdateBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JEditBasicInfoTab: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tab Khai báo cước chính
        public ActionResult EditMainFeeTabView(EditFreightChargesViewModel req)
        {
            EditMainFeeTabViewModel model = null;
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest()
            {
                TransportPostageID = req.TransportPostageID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new EditMainFeeTabViewModel()
                {
                    TransportPostageID = item.TransportPostageID,
                    MainFeeVAT = item.MainFeeVAT,
                    MainFeeRange = item.MainFeeRange,
                    ListRange = XData.GetListTransportRange(),
                    Status = item.Status
                };
            }
            return PartialView(model);
        }

        public JsonResult JEditMainFeeTab(EditMainFeeTabViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.MainServiceFeeEdit(new ZTransportPostageMainServiceFeeEditRequest()
                {
                    TransportPostageID = req.TransportPostageID,
                    MainFeeRange = req.MainFeeRange,
                    MainFeeVAT = req.MainFeeVAT,
                    UpdateBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JEditMainFeeTab: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #region Cấu hình cước dịch vụ chính
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
                    TransportPostageCode = item.TransportPostageCode,
                    TransportPostageName = item.TransportPostageName,
                    TransportPostageRange = item.MainFeeRange
                };
            }
            return View(model);
        }

        public ActionResult ConfigMainFeeDetailView(ConfigMainFeeViewModel req)
        {
            List<ConfigMainFeeDetailViewModel> model = new List<ConfigMainFeeDetailViewModel>();
            var res = TransportPostageDetailRepository.GetByTransportPostageID(new ZTransportPostageDetailGetByTransportPostageIDRequest() { TransportPostageID = req.TransportPostageID });
            if(res.Code == "00")
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
                        IntercityFee5 = item.IntercityFee5
                    });
                }
            }
            ViewBag.TransportPostageID = req.TransportPostageID;
            ViewBag.TransportPostageRange = req.TransportPostageRange;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JAddConfigMainFeeDetailView: " + ex.Message);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JEditConfigMainFeeDetailView: " + ex.Message);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JRemoveConfigMainFeeDetailView: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 
        #endregion

        #region Xuất excel cấu hình cước dịch vụ chính
        public ActionResult ExportExcelConfigMainService(ConfigMainFeeViewModel req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();

            // Tạo ra 1 sheet
            ISheet sheet = wb.CreateSheet();

            // In đậm
            HSSFFont myFontBold = (HSSFFont)wb.CreateFont();
            myFontBold.FontHeightInPoints = 11;
            myFontBold.FontName = "Times New Roman";
            myFontBold.IsBold = true;

            HSSFCellStyle borderFontBold = (HSSFCellStyle)wb.CreateCellStyle();
            borderFontBold.SetFont(myFontBold);
            borderFontBold.VerticalAlignment = VerticalAlignment.Center;
            borderFontBold.Alignment = HorizontalAlignment.Center;
            borderFontBold.BorderLeft = BorderStyle.Thin;
            borderFontBold.BorderTop = BorderStyle.Thin;
            borderFontBold.BorderRight = BorderStyle.Thin;
            borderFontBold.BorderBottom = BorderStyle.Thin;

            // Thường
            HSSFFont myFont = (HSSFFont)wb.CreateFont();
            myFont.FontHeightInPoints = (short)11;
            myFont.FontName = "Times New Roman";

            HSSFCellStyle borderFont = (HSSFCellStyle)wb.CreateCellStyle();
            borderFont.SetFont(myFont);
            borderFont.BorderLeft = BorderStyle.Thin;
            borderFont.BorderTop = BorderStyle.Thin;
            borderFont.BorderRight = BorderStyle.Thin;
            borderFont.BorderBottom = BorderStyle.Thin;


            var res = TransportPostageDetailRepository.GetByTransportPostageID(new ZTransportPostageDetailGetByTransportPostageIDRequest() { TransportPostageID = req.TransportPostageID });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageDetailGetByTransportPostageIDResponse>>(res.Data);

                // Bắt đầu ghi lên sheet
                // Tạo row
                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0); // tạo ra cell trc khi merge
                if (req.TransportPostageRange == 1)
                {
                    //border
                    row0.CreateCell(2).CellStyle = borderFontBold;

                    CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 2);
                    sheet.AddMergedRegion(cellMerge);
                    row0.GetCell(0).SetCellValue("Bảng cước " + req.TransportPostageName);
                    row0.GetCell(0).CellStyle = borderFontBold;

                    var row1 = sheet.CreateRow(1);
                    row1.CreateCell(0).SetCellValue(" Nấc khối lượng (gram) ");
                    row1.CreateCell(1).SetCellValue(" Cước nội tỉnh (VNĐ) ");
                    row1.CreateCell(2).SetCellValue(" Cước liên tỉnh (VNĐ) ");
                    row1.GetCell(0).CellStyle = borderFontBold;
                    row1.GetCell(1).CellStyle = borderFontBold;
                    row1.GetCell(2).CellStyle = borderFontBold;

                    // bắt đầu duyệt mảng và ghi tiếp tục
                    int rowIndex = 2;
                    for (var i = 0; i < list.Count - 1; i++)
                    {
                        var item = list[i];
                        // tao row mới
                        var newRow = sheet.CreateRow(rowIndex);

                        // set giá trị
                        newRow.CreateCell(0).SetCellValue(item.Name);
                        newRow.CreateCell(1).SetCellValue(item.LocalFee);
                        newRow.CreateCell(2).SetCellValue(item.IntercityFee1);

                        newRow.GetCell(0).CellStyle = borderFont;
                        newRow.GetCell(1).CellStyle = borderFont;
                        newRow.GetCell(2).CellStyle = borderFont;

                        // tăng index
                        rowIndex++;
                    }

                    //Nấc
                    var itemNac = list[list.Count - 1];
                    // tao row mới
                    var newRowNac = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRowNac.CreateCell(0).SetCellValue("Mỗi nấc " + itemNac.Weight.ToString("N0") + " tiếp theo");
                    newRowNac.CreateCell(1).SetCellValue(itemNac.LocalFee);
                    newRowNac.CreateCell(2).SetCellValue(itemNac.IntercityFee1);

                    newRowNac.GetCell(0).CellStyle = borderFont;
                    newRowNac.GetCell(1).CellStyle = borderFont;
                    newRowNac.GetCell(2).CellStyle = borderFont;

                    sheet.AutoSizeColumn(0, true);
                    sheet.AutoSizeColumn(1, true);
                    sheet.AutoSizeColumn(2, true);
                }
                else if (req.TransportPostageRange == 2)
                {
                    //border
                    row0.CreateCell(5).CellStyle = borderFontBold;

                    CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 5);
                    sheet.AddMergedRegion(cellMerge);
                    row0.GetCell(0).SetCellValue("Bảng cước " + req.TransportPostageName);
                    row0.GetCell(0).CellStyle = borderFontBold;

                    var row1 = sheet.CreateRow(1);
                    var row2 = sheet.CreateRow(2);
                    var row3 = sheet.CreateRow(3);

                    row1.CreateCell(0);
                    row2.CreateCell(0);
                    row3.CreateCell(0);
                    CellRangeAddress cellMerge1 = new CellRangeAddress(1, 3, 0, 0);
                    sheet.AddMergedRegion(cellMerge1);
                    row1.GetCell(0).SetCellValue(" Nấc khối lượng (gram) ");
                    row1.GetCell(0).CellStyle = borderFontBold;
                    row2.GetCell(0).CellStyle = borderFontBold;
                    row3.GetCell(0).CellStyle = borderFontBold;
                    //border
                    row3.CreateCell(1).CellStyle = borderFontBold;

                    row1.CreateCell(1);
                    CellRangeAddress cellMerge2 = new CellRangeAddress(1, 3, 1, 1);
                    sheet.AddMergedRegion(cellMerge2);
                    row1.GetCell(1).SetCellValue(" Cước nội tỉnh (VNĐ) ");
                    row1.GetCell(1).CellStyle = borderFontBold;

                    row1.CreateCell(2);
                    CellRangeAddress cellMerge3 = new CellRangeAddress(1, 1, 2, 5);
                    sheet.AddMergedRegion(cellMerge3);
                    row1.GetCell(2).SetCellValue(" Cước liên tỉnh (VNĐ) ");
                    row1.GetCell(2).CellStyle = borderFontBold;
                    //border
                    row1.CreateCell(3).CellStyle = borderFontBold;
                    row1.CreateCell(4).CellStyle = borderFontBold;
                    row1.CreateCell(5).CellStyle = borderFontBold;

                    row2.CreateCell(2);
                    CellRangeAddress cellMerge4 = new CellRangeAddress(2, 3, 2, 2);
                    sheet.AddMergedRegion(cellMerge4);
                    row2.GetCell(2).SetCellValue(" Vùng 1 ");
                    row2.GetCell(2).CellStyle = borderFontBold;

                    row2.CreateCell(3);
                    CellRangeAddress cellMerge5 = new CellRangeAddress(2, 2, 3, 4);
                    sheet.AddMergedRegion(cellMerge5);
                    row2.GetCell(3).SetCellValue(" Vùng 2 ");
                    row2.GetCell(3).CellStyle = borderFontBold;

                    row2.CreateCell(5);
                    CellRangeAddress cellMerge6 = new CellRangeAddress(2, 3, 5, 5);
                    sheet.AddMergedRegion(cellMerge6);
                    row2.GetCell(5).SetCellValue(" Vùng 3 ");
                    row2.GetCell(5).CellStyle = borderFontBold;

                    row3.CreateCell(3).SetCellValue(" Đà Nẵng <-> Hà Nội, HCM ");
                    row3.CreateCell(4).SetCellValue(" Hà Nội <-> Hồ Chí Minh ");
                    row3.GetCell(3).CellStyle = borderFontBold;
                    row3.GetCell(4).CellStyle = borderFontBold;
                    //border
                    row3.CreateCell(5).CellStyle = borderFontBold;

                    // bắt đầu duyệt mảng và ghi tiếp tục
                    int rowIndex = 4;
                    for (var i = 0; i < list.Count - 1; i++)
                    {
                        var item = list[i];
                        // tao row mới
                        var newRowNac = sheet.CreateRow(rowIndex);

                        // set giá trị
                        newRowNac.CreateCell(0).SetCellValue(item.Name);
                        newRowNac.CreateCell(1).SetCellValue(item.LocalFee);
                        newRowNac.CreateCell(2).SetCellValue(item.IntercityFee1);
                        newRowNac.CreateCell(3).SetCellValue(item.IntercityFee2);
                        newRowNac.CreateCell(4).SetCellValue(item.IntercityFee3);
                        newRowNac.CreateCell(5).SetCellValue(item.IntercityFee4);

                        newRowNac.GetCell(0).CellStyle = borderFont;
                        newRowNac.GetCell(1).CellStyle = borderFont;
                        newRowNac.GetCell(2).CellStyle = borderFont;
                        newRowNac.GetCell(3).CellStyle = borderFont;
                        newRowNac.GetCell(4).CellStyle = borderFont;
                        newRowNac.GetCell(5).CellStyle = borderFont;

                        // tăng index
                        rowIndex++;
                    }

                    //Nấc
                    var itemNac = list[list.Count - 1];
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRow.CreateCell(0).SetCellValue("Mỗi nấc " + itemNac.Weight.ToString("N0") + " tiếp theo");
                    newRow.CreateCell(1).SetCellValue(itemNac.LocalFee);
                    newRow.CreateCell(2).SetCellValue(itemNac.IntercityFee1);
                    newRow.CreateCell(3).SetCellValue(itemNac.IntercityFee2);
                    newRow.CreateCell(4).SetCellValue(itemNac.IntercityFee3);
                    newRow.CreateCell(5).SetCellValue(itemNac.IntercityFee4);

                    newRow.GetCell(0).CellStyle = borderFont;
                    newRow.GetCell(1).CellStyle = borderFont;
                    newRow.GetCell(2).CellStyle = borderFont;
                    newRow.GetCell(3).CellStyle = borderFont;
                    newRow.GetCell(4).CellStyle = borderFont;
                    newRow.GetCell(5).CellStyle = borderFont;

                    sheet.AutoSizeColumn(0, true);
                    sheet.AutoSizeColumn(1, true);
                    sheet.AutoSizeColumn(2, true);
                    sheet.AutoSizeColumn(3, true);
                    sheet.AutoSizeColumn(4, true);
                    sheet.AutoSizeColumn(5, true);
                }
                else if (req.TransportPostageRange == 3)
                {
                    //border
                    row0.CreateCell(3).CellStyle = borderFontBold;

                    CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 3);
                    sheet.AddMergedRegion(cellMerge);
                    row0.GetCell(0).SetCellValue("Bảng cước " + req.TransportPostageName);
                    row0.GetCell(0).CellStyle = borderFontBold;

                    var row1 = sheet.CreateRow(1);
                    var row2 = sheet.CreateRow(2);

                    row1.CreateCell(0);
                    CellRangeAddress cellMerge1 = new CellRangeAddress(1, 2, 0, 0);
                    sheet.AddMergedRegion(cellMerge1);
                    row1.GetCell(0).SetCellValue(" Nấc khối lượng (gram) ");
                    row1.GetCell(0).CellStyle = borderFontBold;

                    row1.CreateCell(1);
                    CellRangeAddress cellMerge2 = new CellRangeAddress(1, 2, 1, 1);
                    sheet.AddMergedRegion(cellMerge2);
                    row1.GetCell(1).SetCellValue(" Cước nội tỉnh (VNĐ) ");
                    row1.GetCell(1).CellStyle = borderFontBold;

                    row1.CreateCell(2);
                    CellRangeAddress cellMerge3 = new CellRangeAddress(1, 1, 2, 3);
                    sheet.AddMergedRegion(cellMerge3);
                    row1.GetCell(2).SetCellValue(" Cước liên tỉnh (VNĐ) ");
                    row1.GetCell(2).CellStyle = borderFontBold;
                    //border
                    row1.CreateCell(3).CellStyle = borderFontBold;

                    row2.CreateCell(0).CellStyle = borderFontBold;
                    row2.CreateCell(2).SetCellValue(" Đà Nẵng <-> Hà Nội, HCM ");
                    row2.CreateCell(3).SetCellValue(" Hà Nội <-> Hồ Chí Minh ");
                    row2.GetCell(2).CellStyle = borderFontBold;
                    row2.GetCell(3).CellStyle = borderFontBold;

                    // bắt đầu duyệt mảng và ghi tiếp tục
                    int rowIndex = 3;
                    for (var i = 0; i < list.Count - 1; i++)
                    {
                        var item = list[i];
                        // tao row mới
                        var newRowNac = sheet.CreateRow(rowIndex);

                        // set giá trị
                        newRowNac.CreateCell(0).SetCellValue(item.Name);
                        newRowNac.CreateCell(1).SetCellValue(item.LocalFee);
                        newRowNac.CreateCell(2).SetCellValue(item.IntercityFee1);
                        newRowNac.CreateCell(3).SetCellValue(item.IntercityFee2);

                        newRowNac.GetCell(0).CellStyle = borderFont;
                        newRowNac.GetCell(1).CellStyle = borderFont;
                        newRowNac.GetCell(2).CellStyle = borderFont;
                        newRowNac.GetCell(3).CellStyle = borderFont;

                        // tăng index
                        rowIndex++;
                    }

                    //Nấc
                    var itemNac = list[list.Count - 1];
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRow.CreateCell(0).SetCellValue("Mỗi nấc " + itemNac.Weight.ToString("N0") + " tiếp theo");
                    newRow.CreateCell(1).SetCellValue(itemNac.LocalFee);
                    newRow.CreateCell(2).SetCellValue(itemNac.IntercityFee1);
                    newRow.CreateCell(3).SetCellValue(itemNac.IntercityFee2);

                    newRow.GetCell(0).CellStyle = borderFont;
                    newRow.GetCell(1).CellStyle = borderFont;
                    newRow.GetCell(2).CellStyle = borderFont;
                    newRow.GetCell(3).CellStyle = borderFont;

                    sheet.AutoSizeColumn(0, true);
                    sheet.AutoSizeColumn(1, true);
                    sheet.AutoSizeColumn(2, true);
                    sheet.AutoSizeColumn(3, true);
                }
                else if (req.TransportPostageRange == 4)
                {
                    //border
                    row0.CreateCell(4).CellStyle = borderFontBold;

                    CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 4);
                    sheet.AddMergedRegion(cellMerge);
                    row0.GetCell(0).SetCellValue("Bảng cước " + req.TransportPostageName);
                    row0.GetCell(0).CellStyle = borderFontBold;

                    var row1 = sheet.CreateRow(1);
                    var row2 = sheet.CreateRow(2);

                    row1.CreateCell(0);
                    row2.CreateCell(0);
                    CellRangeAddress cellMerge1 = new CellRangeAddress(1, 2, 0, 0);
                    sheet.AddMergedRegion(cellMerge1);
                    row1.GetCell(0).SetCellValue(" Nấc khối lượng (gram) ");
                    row1.GetCell(0).CellStyle = borderFontBold;
                    row2.GetCell(0).CellStyle = borderFontBold;

                    row1.CreateCell(1);
                    CellRangeAddress cellMerge2 = new CellRangeAddress(1, 1, 1, 4);
                    sheet.AddMergedRegion(cellMerge2);
                    row1.GetCell(1).SetCellValue(" Mức cước liên tỉnh (VNĐ) ");
                    row1.GetCell(1).CellStyle = borderFontBold;
                    row2.GetCell(0).CellStyle = borderFontBold;

                    //border
                    row2.CreateCell(1).SetCellValue(" Nội vùng ");
                    row2.CreateCell(2).SetCellValue(" Đà Nẵng <-> Hà Nội, HCM ");
                    row2.CreateCell(3).SetCellValue(" Hà Nội <-> Hồ Chí Minh ");
                    row2.CreateCell(4).SetCellValue(" Liên vùng ");
                    row2.GetCell(1).CellStyle = borderFontBold;
                    row2.GetCell(2).CellStyle = borderFontBold;
                    row2.GetCell(3).CellStyle = borderFontBold;
                    row2.GetCell(4).CellStyle = borderFontBold;

                    // bắt đầu duyệt mảng và ghi tiếp tục
                    int rowIndex = 3;
                    for (var i = 0; i < list.Count - 1; i++)
                    {
                        var item = list[i];
                        // tao row mới
                        var newRowNac = sheet.CreateRow(rowIndex);

                        // set giá trị
                        newRowNac.CreateCell(0).SetCellValue(item.Name);
                        newRowNac.CreateCell(1).SetCellValue(item.IntercityFee1);
                        newRowNac.CreateCell(2).SetCellValue(item.IntercityFee2);
                        newRowNac.CreateCell(3).SetCellValue(item.IntercityFee3);
                        newRowNac.CreateCell(4).SetCellValue(item.IntercityFee4);

                        newRowNac.GetCell(0).CellStyle = borderFont;
                        newRowNac.GetCell(1).CellStyle = borderFont;
                        newRowNac.GetCell(2).CellStyle = borderFont;
                        newRowNac.GetCell(3).CellStyle = borderFont;
                        newRowNac.GetCell(4).CellStyle = borderFont;

                        // tăng index
                        rowIndex++;
                    }

                    //Nấc
                    var itemNac = list[list.Count - 1];
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRow.CreateCell(0).SetCellValue("Mỗi nấc " + itemNac.Weight.ToString("N0") + " tiếp theo");
                    newRow.CreateCell(1).SetCellValue(itemNac.IntercityFee1);
                    newRow.CreateCell(2).SetCellValue(itemNac.IntercityFee2);
                    newRow.CreateCell(3).SetCellValue(itemNac.IntercityFee3);
                    newRow.CreateCell(4).SetCellValue(itemNac.IntercityFee4);

                    newRow.GetCell(0).CellStyle = borderFont;
                    newRow.GetCell(1).CellStyle = borderFont;
                    newRow.GetCell(2).CellStyle = borderFont;
                    newRow.GetCell(3).CellStyle = borderFont;
                    newRow.GetCell(4).CellStyle = borderFont;

                    sheet.AutoSizeColumn(0, true);
                    sheet.AutoSizeColumn(1, true);
                    sheet.AutoSizeColumn(2, true);
                    sheet.AutoSizeColumn(3, true);
                    sheet.AutoSizeColumn(4, true);
                }
                else
                {
                    XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.ExportExcelConfigMainService: request=" + JsonConvert.SerializeObject(req));
                }
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            wb.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "CauHinhCuocDichVuChinh.xls");
        }
        #endregion

        #endregion

        #endregion

        #region Tab Phụ phí
        public ActionResult EditSurchargeTabView(EditFreightChargesViewModel req)
        {
            EditSurchargeTabViewModel model = null;
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest()
            {
                TransportPostageID = req.TransportPostageID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new EditSurchargeTabViewModel()
                {
                    TransportPostageID = item.TransportPostageID,
                    TransportPostageCode = item.TransportPostageCode,
                    TransportPostageName = item.TransportPostageName,
                    SurchargeVAT = item.SurchargeVAT,
                    Fuel = new EditSurchargeFuelModel()
                    {
                        Checked = item.Fuel.Checked,
                        Unit = item.Fuel.Unit,
                        Value = item.Fuel.Value,
                        MinValue = item.Fuel.MinValue,
                        MaxValue = item.Fuel.MaxValue,
                        InOfProvince = item.Fuel.InOfProvince,
                        OutOfProvince = item.Fuel.OutOfProvince
                    },
                    RemoteArea = new EditSurchargeRemoteAreaModel()
                    {
                        Checked = item.RemoteArea.Checked,
                        Unit = item.RemoteArea.Unit,
                        Value = item.RemoteArea.Value,
                        MinValue = item.RemoteArea.MinValue,
                        MaxValue = item.RemoteArea.MaxValue,
                        InOfProvince = item.RemoteArea.InOfProvince,
                        OutOfProvince = item.RemoteArea.OutOfProvince,
                        IsFrom = item.RemoteArea.IsFrom,
                    },
                    IslandsArea = new EditSurchargeIslandsAreaModel()
                    {
                        Checked = item.IslandsArea.Checked,
                        Unit = item.IslandsArea.Unit,
                        Value = item.IslandsArea.Value,
                        MinValue = item.IslandsArea.MinValue,
                        MaxValue = item.IslandsArea.MaxValue,
                        InOfProvince = item.IslandsArea.InOfProvince,
                        OutOfProvince = item.IslandsArea.OutOfProvince,
                        IsFrom = item.RemoteArea.IsFrom
                    },
                    Status = item.Status
                };
            }
            return PartialView(model);
        }

        public JsonResult JEditSurchargeTab(EditSurchargeTabViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.MainServiceSurchargeEdit(new ZTransportPostageSurchargeEditRequest() 
                {
                    TransportPostageID = req.TransportPostageID,
                    SurchargeVAT = req.SurchargeVAT,
                    Fuel = new ZTransportPostageSurchargeEditFuelRequest()
                    {
                        Checked = req.Fuel.Checked,
                        Unit = req.Fuel.Unit,
                        Value = req.Fuel.Value,
                        MinValue = req.Fuel.MinValue,
                        MaxValue = req.Fuel.MaxValue,
                        InOfProvince = req.Fuel.InOfProvince,
                        OutOfProvince = req.Fuel.OutOfProvince
                    },
                    RemoteArea = new ZTransportPostageSurchargeEditRemoteAreaRequest()
                    {
                        Checked = req.RemoteArea.Checked,
                        Unit = req.RemoteArea.Unit,
                        Value = req.RemoteArea.Value,
                        MinValue = req.RemoteArea.MinValue,
                        MaxValue = req.RemoteArea.MaxValue,
                        InOfProvince = req.RemoteArea.InOfProvince,
                        OutOfProvince = req.RemoteArea.OutOfProvince,
                        IsFrom = req.RemoteArea.IsFrom,
                    },
                    IslandsArea = new ZTransportPostageSurchargeEditIslandsAreaRequest()
                    {
                        Checked = req.IslandsArea.Checked,
                        Unit = req.IslandsArea.Unit,
                        Value = req.IslandsArea.Value,
                        MinValue = req.IslandsArea.MinValue,
                        MaxValue = req.IslandsArea.MaxValue,
                        InOfProvince = req.IslandsArea.InOfProvince,
                        OutOfProvince = req.IslandsArea.OutOfProvince,
                        IsFrom = req.IslandsArea.IsFrom,
                    },
                    UpdateBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JEditSurchargeTab: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tab Lịch sử
        public ActionResult EditHistoryTabView(EditFreightChargesViewModel req)
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xác nhận bảng cấu hình cước
        public JsonResult JConfirm(EditFreightChargesViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.ConfirmTransportPostage(new ZTransportPostageConfirmRequest()
                {
                    ID = req.TransportPostageID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JConfirm: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Từ chối bảng cấu hình cước
        public JsonResult JUnConfirm(SearchListFreightChargesViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.UnConfirmTransportPostage(new ZTransportPostageUnConfirmRequest()
                {
                    ID = req.TransportPostageID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportMainServiceController.JUnConfirm: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        
    }
}