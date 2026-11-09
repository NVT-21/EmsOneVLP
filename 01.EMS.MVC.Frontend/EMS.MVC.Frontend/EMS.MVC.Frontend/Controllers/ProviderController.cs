using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using EMS.Entity;
using EMS.MVC.Frontend.Repositories;
using EMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using NPOI.SS.Util;
using System.Web.Script.Serialization;


namespace EMS.MVC.Frontend.Controllers
{
    public class ProviderController : BaseController
    {
        #region Danh sách nhà cung cấp
        [AuthorizeController(FunctionalTitle = "DANH_SACH_NHA_CUNG_CAP")]
        public ActionResult ProviderView()
        {
            return View();
        }

        public ActionResult SearchListProviderView()
        {
            List<SearchListProviderViewModel> listitem = new List<SearchListProviderViewModel>();
            var res = ProviderRepotiory.SearchListProvider(new ZProviderSearchListRequest()
            {
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });

            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZProviderSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    listitem.Add(new SearchListProviderViewModel()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                        ShortName = item.ShortName,
                        Email = item.Email,
                        MobileNumber = item.MobileNumber,
                        TaxCode = item.TaxCode,
                        FullAddress = item.FullAddress,
                        UpdatedDate = item.UpdatedDate,
                        IsLock = item.IsLock
                    });
                }
            }
            return PartialView(listitem);
        }

        public ActionResult ExportListProvider()
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
            TitleRow.CreateCell(0).SetCellValue("DANH SÁCH NHÀ CUNG CẤP");
            TitleRow.GetCell(0).CellStyle = TitleStyle;

            //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 12));
            //sheet.CreateRow(0).GetCell(0).CellStyle = center;
            var res = ProviderRepotiory.SearchListProvider(new ZProviderSearchListRequest()
            {
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });

            List<ProviderModel> ListItem = new List<ProviderModel>();

            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZProviderSearchListResponse>>(res.Data);
                foreach(var item in list)
                {
                    ListItem.Add(new ProviderModel() 
                    {
                        ProviderCode = item.Code,
                        ProviderName = item.Name,
                        MobileNumber = item.MobileNumber,
                        FullAddress = item.FullAddress,
                        Email = item.Email,
                        TaxCode = item.TaxCode,
                        IsLock = item.IsLock
                    });
                }
            }

            //Create a header row
            var headerRow = sheet.CreateRow(1);
            int ColumnIndex = 0;

            headerRow.CreateCell(ColumnIndex++).SetCellValue("STT");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã nhà cung cấp");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Tên nhà cung cấp");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Số điện thoại");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Địa chỉ");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Email");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã số thuế");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Trạng thái hoạt động");


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
                row.CreateCell(rowIndex++).SetCellValue(item.ProviderCode);
                row.CreateCell(rowIndex++).SetCellValue(item.ProviderName);
                row.CreateCell(rowIndex++).SetCellValue(item.MobileNumber);
                row.CreateCell(rowIndex++).SetCellValue(item.FullAddress);
                row.CreateCell(rowIndex++).SetCellValue(item.Email);
                row.CreateCell(rowIndex++).SetCellValue(item.TaxCode);
                row.CreateCell(rowIndex++).SetCellValue(item.IsLock == "N" ? "Hoạt động" : "Không hoạt động");

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

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            //application/vnd.ms-excel xls
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet xlsx
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachNhaCungCap.xls");
        }
        #endregion

        #region Thêm nhà cung cấp
        [AuthorizeController(FunctionalTitle = "THEM_NHA_CUNG_CAP")]
        public ActionResult AddProviderView(AddProviderViewModel req)
        {
            AddProviderViewModel model = new AddProviderViewModel() 
            {
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                ListDistrict = new List<DistrictModel>(),
                ListWard = new List<WardModel>(),
                ListIsLock = XData.GetListIsActive()
            };
            return PartialView(model);
        }

        public JsonResult JAddProvider(AddProviderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ProviderRepotiory.AddProvider(new ZProviderAddRequest()
                {
                    Code = req.Code,
                    Name = req.Name,
                    ShortName = req.ShortName,
                    MobileNumber = req.MobileNumber,
                    Email = req.Email,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Address = req.Address,
                    TaxCode = req.TaxCode,
                    Note = req.Note,
                    IsLock = req.IsLock,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.JAddProvider: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa nhà cung cấp
        [AuthorizeController(FunctionalTitle = "SUA_NHA_CUNG_CAP")]
        public ActionResult EditProviderView(SearchListProviderViewModel req)
        {
            EditProviderViewModel model = null;
            var res = ProviderRepotiory.DetailProviderByWareHouseID(new ZProviderGetByIDRequest()
            {
                ID = req.ID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZProviderGetByIDResponse>(res.Data);
                model = new EditProviderViewModel()
                {
                    ID = item.ID,
                    Code = item.Code,
                    Name = item.Name,
                    ShortName = item.ShortName,
                    Email = item.Email,
                    MobileNumber = item.MobileNumber,
                    TaxCode = item.TaxCode,
                    IsLock = item.IsLock,
                    Address = item.Address,
                    Note = item.Note,
                    ProvinceID = item.ProvinceID,
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    DistrictID = item.DistrictID,
                    ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = item.ProvinceID }),
                    WardID = item.WardID,
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = item.DistrictID }),
                };
            }
            return PartialView(model);
        }

        public JsonResult JEditProvider(EditProviderViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ProviderRepotiory.EditProvider(new ZProviderEditRequest()
                {
                    ID = req.ID,
                    Name = req.Name,
                    ShortName = req.ShortName,
                    MobileNumber = req.MobileNumber,
                    Email = req.Email,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Address = req.Address,
                    TaxCode = req.TaxCode,
                    Note = req.Note,
                    IsLock = req.IsLock,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.JEditProvider: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mở/ Khóa nhà cung cấp
        [AuthorizeController(FunctionalTitle = "MO_HOAC_KHOA_NHA_CUNG_CAP")]
        public JsonResult JChangeIsLock(ProviderChangeIsLockRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ProviderRepotiory.ChangeIsLockProvider(new ZProviderChangeIsLockRequest()
                {
                    ID = req.ID,
                    IsLock = req.IsLock,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.JChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm nhà cung cấp bằng Excel
        [AuthorizeController(FunctionalTitle = "THEM_NHA_CUNG_CAP_BANG_EXCEL")]
        public ActionResult ImportExcelView(ImportExcelViewModel model)
        {
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

            var ListIsActive = XData.GetListIsActive();
            foreach (var item in ListIsActive)
            {
                model.ListActive.Add(new JSpreadSheetModel()
                {
                    id = item.Value,
                    name = item.Text
                });
            }
            return View(model);
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

                //Loop the records upto filled row
                for (int row = 1; row <= total; row++)
                {
                    var ProvinceID = 0;
                    var DistrictID = 0;
                    var WardID = 0;
                    if (sheet.GetRow(row) != null)
                    {
                        var Code = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(1));
                        var Name = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(2));
                        var ShortName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(3));
                        var ProvinceName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(4));
                        var DistrictName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(5));
                        var WardName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(6));
                        var Address = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(7));
                        var PhoneNumber = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(8));
                        var Email = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(9));
                        var TaxCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(10));
                        var Note = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(11));
                        var IsLock = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(12));

                        if (!string.IsNullOrEmpty(Code)) //CHECK HANG NULL
                        {
                            //Số điện thoại
                            if (!string.IsNullOrEmpty(PhoneNumber))
                            {
                                var ReplaceSpecialChar = Regex.Replace(PhoneNumber, @"[^\d]", String.Empty);
                                if (ReplaceSpecialChar.Substring(0, 3).Contains("840"))
                                {
                                    PhoneNumber = ReplaceSpecialChar.Substring(0, 3).Replace("840", "0") + ReplaceSpecialChar.Substring(3, ReplaceSpecialChar.Length - 3);
                                }
                                else if (ReplaceSpecialChar.Substring(0, 2).Contains("84"))
                                {
                                    PhoneNumber = ReplaceSpecialChar.Substring(0, 2).Replace("84", "0") + ReplaceSpecialChar.Substring(2, ReplaceSpecialChar.Length - 2);
                                }
                                else if (ReplaceSpecialChar.Substring(0, 1) != "0")
                                {
                                    PhoneNumber = "0" + ReplaceSpecialChar;
                                }
                                else
                                {
                                    PhoneNumber = ReplaceSpecialChar;
                                }
                            }


                            if (!string.IsNullOrEmpty(ProvinceName) && !string.IsNullOrEmpty(DistrictName) && !string.IsNullOrEmpty(WardName))
                            {
                                var resPWD = PWDRepository.SearchPDWIDByPDWName(new ZMapsGetByPWDNameRequest()
                                {
                                    ProvinceName = ProvinceName.Trim(),
                                    DistrictName = DistrictName.Trim(),
                                    WardName = WardName.Trim()
                                });
                                if (resPWD.Code == "00")
                                {
                                    var dataPWD = JsonConvert.DeserializeObject<ZMapsGetByPWDNameResponse>(resPWD.Data);
                                    ProvinceID = dataPWD.ProvinceID;
                                    DistrictID = dataPWD.DistrictID;
                                    WardID = dataPWD.WardID;
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Address))
                                {
                                    var resSearchPWDByVNPMapsSearch = XCommon.SearchPWDByVNPMapsSearch(Address);
                                    if (resSearchPWDByVNPMapsSearch.Code == "00")
                                    {
                                        var dataVnpMapsSearch = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeResponse>(resSearchPWDByVNPMapsSearch.Data);
                                        ProvinceName = dataVnpMapsSearch.ProvinceName;
                                        DistrictName = dataVnpMapsSearch.DistrictName;
                                        WardName = dataVnpMapsSearch.WardName;
                                        ProvinceID = dataVnpMapsSearch.ProvinceID;
                                        DistrictID = dataVnpMapsSearch.DistrictID;
                                        WardID = dataVnpMapsSearch.WardID;
                                        Address = dataVnpMapsSearch.Street;
                                    }
                                }
                            }

                            IsLock = IsLock == "Hoạt động" ? "N" : "Y";

                            ArrayList data = new ArrayList();
                            data.Add(""); //0
                            data.Add(Code);//01
                            data.Add(Name);//02
                            data.Add(ShortName);//03
                            data.Add(ProvinceName);//04
                            data.Add(DistrictName);//05                     
                            data.Add(WardName);//06
                            data.Add(Address);//07
                            data.Add(PhoneNumber);//08
                            data.Add(Email);//09
                            data.Add(TaxCode);//010
                            data.Add(Note);//011
                            data.Add(IsLock);//012
                            data.Add(ProvinceID);//013
                            data.Add(DistrictID);//014
                            data.Add(WardID);//015
                            model.Add(data);//016

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

        public JsonResult JImportExcel(List<ImportExcelProviderModel> data, int countKHL, int countLoi)
        {
            ZResponse res = new ZResponse();
            var rowAction = 0;
            var count = 0;
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
                //var typeAddress = "";
            
                List<ArrayList> lstError = new List<ArrayList>();
                foreach (var itemExcel in data)
                {
                    rowAction++;
                    //Tạo nhà cung cấp
                    ZResponse resItem = new ZResponse();
                    resItem = ProviderRepotiory.AddProviderEcxel(new ImportExcelProviderModel()
                    {
                        Code = itemExcel.Code,
                        Name = itemExcel.Name,
                        ShortName = itemExcel.ShortName,
                        MobileNumber = itemExcel.MobileNumber,
                        Email = itemExcel.Email,
                        ProvinceID = itemExcel.ProvinceID,
                        DistrictID = itemExcel.DistrictID,
                        WardID = itemExcel.WardID,
                        Address = itemExcel.Address,
                        TaxCode = itemExcel.TaxCode,
                        Note = itemExcel.Note,
                        IsLock = itemExcel.IsLock,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        CreatedBy = XHelper.Session.EmployeeProfile.ID
                    });
                    if (resItem.Code == "00")
                    {
                        count++;
                    }
                    else
                    {
                        lstError.Add(JImportExcelError(itemExcel, "Nhà cung cấp - " + resItem.Message));
                        countError++;
                        continue;
                    }
                }

                if (total == count)
                {
                    res.Code = "00";
                    res.Message = string.Format("<div class='text-green mb-2'>Nhà cung cấp tạo thành công: {0}</div><div class='text-warning mb-2'>Nhà cung cấp không hợp lệ: {1}</div><div class='text-danger mb-3'>Nhà cung cấp lỗi: {2}</div>", count, countKHL, countError);
                }
                else
                {
                    res.Code = "10";
                    res.Message = string.Format("<div class='text-green mb-2'>Nhà cung cấp tạo thành công: {0}</div><div class='text-warning mb-2'>Nhà cung cấp  không hợp lệ: {1}</div><div class='text-danger mb-3'>Nhà cung cấp lỗi: {2}</div>", count, countKHL, countError);
                    res.Data = JsonConvert.SerializeObject(lstError);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.JImportExcel: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ArrayList JImportExcelError(ImportExcelProviderModel data, string Message)
        {
            ArrayList model = new ArrayList();
            model.Add("Lỗi");
            model.Add(data.Code);
            model.Add(data.Name);
            model.Add(data.ShortName);
            model.Add(data.ProvinceName);
            model.Add(data.DistrictName);
            model.Add(data.WardName);
            model.Add(data.Address);
            model.Add(data.MobileNumber);
            model.Add(data.Email);
            model.Add(data.TaxCode);
            model.Add(data.Note);
            model.Add(data.IsLock);
            model.Add(data.ProvinceID);
            model.Add(data.DistrictID);
            model.Add(data.WardID);
            model.Add(Message);
            return model;
        }

        public ActionResult JExportExcel(List<ImportExcelProviderModel> Data)
        {
            var FullPath = System.Web.HttpContext.Current.Server.MapPath("~/Assets/temp/MauImportNhaCungCap.xlsx");
            var fileNameNew = Guid.NewGuid().ToString() + ".xlsx";
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
                    int rowIndex = 1;
                    int stt = 1;
                    // create bordered cell style
                    //XSSFCellStyle borderedCellStyle = (XSSFCellStyle)xlsx.CreateCellStyle();
                    //borderedCellStyle.BorderLeft = BorderStyle.Thin;
                    //borderedCellStyle.BorderTop = BorderStyle.Thin;
                    //borderedCellStyle.BorderRight = BorderStyle.Thin;
                    //borderedCellStyle.BorderBottom = BorderStyle.Thin;

                    sheet.GetRow(0).CreateCell(13).SetCellValue("Nguyên nhân lỗi");
                    //sheet.GetRow(0).GetCell(13).CellStyle = borderedCellStyle;

                    foreach (var item in Data)
                    {
                        // tao row mới
                        var newRow = sheet.CreateRow(rowIndex);
                        // set giá trị
                        newRow.CreateCell(0).SetCellValue(stt++);
                        newRow.CreateCell(1).SetCellValue(item.Code);
                        newRow.CreateCell(2).SetCellValue(item.Name);
                        newRow.CreateCell(3).SetCellValue(item.ShortName);
                        newRow.CreateCell(4).SetCellValue(item.ProvinceName);
                        newRow.CreateCell(5).SetCellValue(item.DistrictName);
                        newRow.CreateCell(6).SetCellValue(item.WardName);
                        newRow.CreateCell(7).SetCellValue(item.Address);
                        newRow.CreateCell(8).SetCellValue(item.MobileNumber);
                        newRow.CreateCell(9).SetCellValue(item.Email);
                        newRow.CreateCell(10).SetCellValue(item.TaxCode);
                        newRow.CreateCell(11).SetCellValue(item.Note);
                        newRow.CreateCell(12).SetCellValue(item.IsLock);
                        newRow.CreateCell(13).SetCellValue(item.msgError);

                        //for (int i = 0; i <= 13; i++)
                        //{
                        //    newRow.GetCell(i).CellStyle = borderedCellStyle;
                        //}
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.JExportExcel: " + ex.Message);
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
             "MauImportNhaCungCap");
        }
        #endregion

        #region Lịch sử nhập hàng của nhà cung cấp
        [AuthorizeController(FunctionalTitle = "LICH_SU_NHAP_HANG_CUA_NHA_CUNG_CAP")]
        public ActionResult HistoryView()
        {
            return View();
        }
        #endregion
    }
}