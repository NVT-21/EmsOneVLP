using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Linq;
using System.Web.Security;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;

namespace EMS.MVC.Backend.Controllers
{
    public class ShopController : BaseController
    {
        #region Danh sách cửa hàng
        [AuthorizeController]
        public ActionResult ShopView()
        {
            ShopViewModel model = new ShopViewModel()
            {
                FromDate = DateTime.Now.ToString("01/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListAccountStatus = XData.GetListAccountStatus(),
                ListDateType = new List<SelectListItem>()
                {
                    new SelectListItem(){ Value = "CREATED_DATE", Text = "Thời gian tạo", Selected = true },
                    new SelectListItem(){ Value = "CONFIRM_DATE", Text = "Thời gian duyệt" },
                },
            };
            return View(model);
        }

        [AuthorizeController]
        public ActionResult SearchListShopView(ShopViewModel req)
        {
            List<SearchListShopViewModel> model = new List<SearchListShopViewModel>();
            var res = ShopRepository.SearchListShop(new ZShopSearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                CustomerCode = req.CustomerCode,
                CrmOrPaypostCode = req.CrmOrPaypostCode,
                MobileNumber = req.MobileNumber,
                Status = req.AccountStatus,
                DateType = req.DateType,
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZShopSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new SearchListShopViewModel()
                    {
                        ID = item.ID,
                        FullName = item.FullName,
                        StoreName = item.StoreName,
                        Email = item.Email,
                        MobileNumber = item.MobileNumber,
                        ProductBusiness = item.ProductBusiness,
                        TypeBusiness = item.TypeBusiness,
                        Code = item.Code,
                        CustomerCode = item.CustomerCode,
                        Status = new BaseModel()
                        {
                            Code = item.Status.Code,
                            Name = item.Status.Name
                        },
                        IsLock = item.IsLock,
                        AccountType = new BaseModel()
                        {
                            Code = item.AccountType.Code,
                            Name = item.AccountType.Name
                        },
                        NumberOfEmployee = new BaseModel()
                        {
                            Code = item.NumberOfEmployees.Code,
                            Name = item.NumberOfEmployees.Name
                        },
                        CreatedName = item.CreatedName,
                        CreatedPhone = item.CreatedPhone,
                        CreatedEmail = item.CreatedEmail,
                        CreatedDate = item.CreatedDate,
                        ConfirmedName = item.ConfirmedName,
                        ConfirmedPhone = item.ConfirmedPhone,
                        ConfirmedEmail = item.ConfirmedEmail,
                        ConfirmedDate = item.ConfirmedDate,
                        ReasonReject = item.ReasonReject,
                    });
                }
            }
            return PartialView(model);
        }

        public ActionResult ExportListShop(ShopViewModel req)
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
            HeaderStyle.VerticalAlignment = VerticalAlignment.Center;
            HeaderStyle.Alignment = HorizontalAlignment.Center;

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

            var CenterStyle = workbook.CreateCellStyle();
            CenterStyle.VerticalAlignment = VerticalAlignment.Center;
            CenterStyle.Alignment = HorizontalAlignment.Center;

            //var center = workbook.CreateCellStyle();
            //center.Alignment = HorizontalAlignment.Center;
            //center.SetFont(fontChar);

            //Create title 
            var TitleRow = sheet.CreateRow(0);
            TitleRow.CreateCell(0).SetCellValue("DANH SÁCH TÀI KHOẢN KHÁCH HÀNG");
            TitleRow.GetCell(0).CellStyle = TitleStyle;

            //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 12));
            //sheet.CreateRow(0).GetCell(0).CellStyle = center;
            var res = ShopRepository.SearchListShop(new ZShopSearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                CustomerCode = req.CustomerCode,
                CrmOrPaypostCode = req.CrmOrPaypostCode,
                MobileNumber = req.MobileNumber,
                Status = req.AccountStatus,
                DateType = req.DateType,
            });

            List<ZShopSearchListResponse> listData = new List<ZShopSearchListResponse>();

            if (res.Code == "00")
            {
                listData = JsonConvert.DeserializeObject<List<ZShopSearchListResponse>>(res.Data);
            }

            //Create a header row
            var headerRow2 = sheet.CreateRow(2);
            int ColumnIndex = 0;

            headerRow2.CreateCell(ColumnIndex++).SetCellValue("STT");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 0, 0));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Mã KH");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 1, 1));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Mã CRM");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 2, 2));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Tên cửa hàng");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 3, 3));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("SĐT");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 4, 4));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Email");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 5, 5));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Tên người liên hệ");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 6, 6));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Địa chỉ");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 7, 7));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Mã bưu cục thu gom");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 8, 8));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Sản phẩm kinh doanh");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 9, 9));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Loại hình kinh doanh");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 10, 10));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Loại khách hàng");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 11, 11));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Trạng thái");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 12, 12));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Ngày tạo");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 13, 13));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Ngày duyệt");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 14, 14));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Mã KH con");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 15, 15));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Email bưu cục gửi");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 16, 16));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Email CSKH");
            sheet.AddMergedRegion(new CellRangeAddress(2, 3, 17, 17));
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("Người phê duyệt");
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("");
            headerRow2.CreateCell(ColumnIndex++).SetCellValue("");
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 18, 20));

            var headerRow3 = sheet.CreateRow(3);
            headerRow3.CreateCell(0).SetCellValue("");
            headerRow3.CreateCell(1).SetCellValue("");
            headerRow3.CreateCell(2).SetCellValue("");
            headerRow3.CreateCell(3).SetCellValue("");
            headerRow3.CreateCell(4).SetCellValue("");
            headerRow3.CreateCell(5).SetCellValue("");
            headerRow3.CreateCell(6).SetCellValue("");
            headerRow3.CreateCell(7).SetCellValue("");
            headerRow3.CreateCell(8).SetCellValue("");
            headerRow3.CreateCell(9).SetCellValue("");
            headerRow3.CreateCell(10).SetCellValue("");
            headerRow3.CreateCell(11).SetCellValue("");
            headerRow3.CreateCell(12).SetCellValue("");
            headerRow3.CreateCell(13).SetCellValue("");
            headerRow3.CreateCell(14).SetCellValue("");
            headerRow3.CreateCell(15).SetCellValue("");
            headerRow3.CreateCell(16).SetCellValue("");
            headerRow3.CreateCell(17).SetCellValue("");
            headerRow3.CreateCell(18).SetCellValue("Họ và tên");
            headerRow3.CreateCell(19).SetCellValue("SĐT");
            headerRow3.CreateCell(20).SetCellValue("Email");

            for (var i = 0; i <= 20; i++)
            {
                headerRow2.GetCell(i).CellStyle = HeaderStyle;
                headerRow3.GetCell(i).CellStyle = HeaderStyle;
            }

            //(Optional) freeze the header row so it is not scrolled
            //sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 4;
            int rowIndex = 0;
            var stt = 1;
            foreach (var item in listData)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(rowIndex++).SetCellValue(stt.ToString());
                row.CreateCell(rowIndex++).SetCellValue(item.Code);
                row.CreateCell(rowIndex++).SetCellValue(item.CustomerCode);
                row.CreateCell(rowIndex++).SetCellValue(item.StoreName);
                row.CreateCell(rowIndex++).SetCellValue(item.MobileNumber);
                row.CreateCell(rowIndex++).SetCellValue(item.Email);
                row.CreateCell(rowIndex++).SetCellValue(item.FullName);
                row.CreateCell(rowIndex++).SetCellValue(item.FullAddress);
                row.CreateCell(rowIndex++).SetCellValue(item.CollectionPostOfficeCode);
                row.CreateCell(rowIndex++).SetCellValue(item.ProductBusiness);
                row.CreateCell(rowIndex++).SetCellValue(item.TypeBusiness);
                row.CreateCell(rowIndex++).SetCellValue(item.TypeShop.Name);
                row.CreateCell(rowIndex++).SetCellValue(item.Status.Name);
                row.CreateCell(rowIndex++).SetCellValue(item.CreatedDate);
                row.CreateCell(rowIndex++).SetCellValue(item.ConfirmedDate);
                row.CreateCell(rowIndex++).SetCellValue(item.ChildCustomerCode);
                row.CreateCell(rowIndex++).SetCellValue(item.EmailBCG);
                row.CreateCell(rowIndex++).SetCellValue(item.EmailCSKH);
                row.CreateCell(rowIndex++).SetCellValue(item.ConfirmedName);
                row.CreateCell(rowIndex++).SetCellValue(item.ConfirmedPhone);
                row.CreateCell(rowIndex++).SetCellValue(item.ConfirmedEmail);

                for (var i = 0; i <= 20; i++)
                {
                    row.GetCell(i).CellStyle = BodyTextStyle;
                }

                stt++;
                rowIndex = 0;
            }

            //sheet.AutoSizeColumn(1);
            //sheet.AutoSizeColumn(2);
            //sheet.AutoSizeColumn(3);
            //sheet.AutoSizeColumn(4);
            //sheet.AutoSizeColumn(5);
            //sheet.AutoSizeColumn(6);
            //sheet.AutoSizeColumn(7);
            //sheet.AutoSizeColumn(8);
            //sheet.AutoSizeColumn(9);
            //sheet.AutoSizeColumn(10);
            //sheet.AutoSizeColumn(11);
            //sheet.AutoSizeColumn(12);
            //sheet.AutoSizeColumn(13);
            //sheet.AutoSizeColumn(14);
            //sheet.AutoSizeColumn(15);
            //sheet.AutoSizeColumn(16);
            //sheet.AutoSizeColumn(17);
            //sheet.AutoSizeColumn(18);
            //sheet.AutoSizeColumn(19);
            //sheet.AutoSizeColumn(20);

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            //application/vnd.ms-excel xls
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet xlsx
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachTaiKhoanKhachHang.xls");
        }
        #endregion

        #region Thêm cửa hàng
        [AuthorizeController]
        public ActionResult AddShopView()
        {
            AddShopViewModel model = new AddShopViewModel()
            {
                AccountType = 99,
                ListAccountType = XData.GetListAccountType(),
                ListNumberOfEmployee = XData.GetListNumberOfEmployee(),
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                //ListDistrict = new List<DistrictModel>(),
                ListWard = new List<WardModel>(),
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>(),
                PeriodType = XData.GetPeriodType(),
                ListPaymentMethod = XData.GetListPaymentMethod(),
                ListCollectionType = XData.GetListCollectionType(),
                ListCollectionPostOffice = XCommon.SearchListCollectionPostOffice(new ZCollectionPostOfficeSearchListRequest() { IsLock = "N" }),
                ListPartner = XCommon.SearchListPartner(new ZPartnerSearchListRequest() { IsLock = "N" })
            };

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { IsLock = "N", ParentID = 0, Channel = "EMSONE" });
            if (ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
            }

            return View(model);
        }

        public JsonResult JAddShop(AddShopViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.AddShop(new ZShopAddRequest()
                {
                    ContractNumber = req.ContractNumber,
                    CustomerCode = req.CustomerCode,
                    CustomerToken = req.CustomerToken,
                    FullName = req.FullName,
                    StoreName = req.StoreName,
                    Email = req.Email,
                    MobileNumber = req.MobileNumber,
                    Password = Membership.GeneratePassword(8, 3),
                    ProductBusiness = req.ProductBusiness,
                    TypeBusiness = req.TypeBusiness,
                    TypeShop = req.TypeShop,
                    AccountType = req.AccountType,
                    NumberOfEmployees = req.NumberOfEmployees,
                    ProvinceID = req.ProvinceID,
                    //DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street,
                    MainTransportServiceID = req.MainTransportServiceID,
                    ExtraTransportServiceID = req.ExtraTransportServiceID,
                    CollectionType = req.CollectionType,
                    CollectionPostOfficeID = req.CollectionPostOfficeID,
                    ChildCustomerCode = req.ChildCustomerCode,
                    CODPeriodType = req.CODPeriodType,
                    FeePeriodType = req.FeePeriodType,
                    PaymentMethod = req.PaymentMethod,
                    FullNameOfPayment = req.FullNameOfPayment,
                    NameOfPayment = req.NameOfPayment,
                    BranchOfPayment = req.BranchOfPayment,
                    NumberOfPayment = req.NumberOfPayment,
                    EmailBCG = req.EmailBCG,
                    EmailCSKH = req.EmailCSKH,
                    TaxCode = req.TaxCode,
                    PartnerID = req.PartnerID,
                    CreatedBy = XHelper.Session.UserProfile.ID,
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JAddShop: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa thông tin cửa hàng
        public ActionResult EditShopView(SearchListShopViewModel req)
        {
            EditShopViewModel model = new EditShopViewModel();
            var res = ShopRepository.GetByID(new ZShopGetByIDRequest() { ID = req.ID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(res.Data);
                model = new EditShopViewModel()
                {
                    ID = item.ID,
                    Code = item.Code,
                    FullName = item.FullName,
                    StoreName = item.StoreName,
                    Email = item.Email,
                    MobileNumber = item.MobileNumber,
                    ProductBusiness = item.ProductBusiness,
                    TypeBusiness = item.TypeBusiness,
                    TypeShop = item.TypeShop,
                    Status = item.Status,
                    AccountType = item.AccountType,
                    ListAccountType = XData.GetListAccountType(),
                    NumberOfEmployees = item.NumberOfEmployees,
                    ListNumberOfEmployee = XData.GetListNumberOfEmployee(),
                    ContractNumber = item.ContractNumber,
                    CustomerCode = item.CustomerCode,
                    CustomerToken = item.CustomerToken,
                    ProvinceID = item.ProvinceID,
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    //DistrictID = item.DistrictID,
                    //ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = item.ProvinceID }),
                    WardID = item.WardID,
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = item.ProvinceID }),
                    Street = item.Street,
                    MainTransportServiceID = item.MainTransportServiceID,
                    ListMainTransportService = new List<TransportServiceModel>(),
                    ExtraTransportServiceID = item.ExtraTransportServiceID,
                    IsExistExtraTransportServiceID = item.IsExistExtraTransportServiceID,
                    ListExtraTransportService = new List<TransportServiceModel>(),
                    CollectionType = item.CollectionType,
                    ListCollectionType = XData.GetListCollectionType(),
                    CollectionPostOfficeID = item.CollectionPostOfficeID,
                    ListCollectionPostOffice = XCommon.SearchListCollectionPostOffice(new ZCollectionPostOfficeSearchListRequest() { IsLock = "N" }),
                    ChildCustomerCode = item.ChildCustomerCode,
                    CODPeriodType = item.CODPeriodType,
                    FeePeriodType = item.FeePeriodType,
                    ListPeriodType = XData.GetPeriodType(),
                    PaymentMethod = item.PaymentMethod,
                    IsDisplay = item.PaymentMethod == 1 ? "block" : "none",
                    ListPaymentMethod = XData.GetListPaymentMethod(),
                    FullNameOfPayment = item.FullNameOfPayment,
                    NameOfPayment = item.NameOfPayment,
                    BranchOfPayment = item.BranchOfPayment,
                    NumberOfPayment = item.NumberOfPayment,
                    PickUpID = item.PickUpID,
                    PickUpFullName = item.PickUpFullName,
                    PickUpMobileNumber = item.PickUpMobileNumber,
                    EmailBCG = item.EmailBCG,
                    EmailCSKH = item.EmailCSKH,
                    TaxCode = item.TaxCode,
                    PartnerID = item.PartnerID,
                    IsSercurity = "N",
                    ListPartner = XCommon.SearchListPartner(new ZPartnerSearchListRequest() { IsLock = "N" }),
                    ListIsSercurity = XData.GetListIsSercurity()
                };

                List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { IsLock = "N", ParentID = 0, Channel = "EMSONE" });
                if (ListTransportService.Count > 0)
                {
                    model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                    model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
                }
            }
            return View(model);
        }

        #region Tab Thông tin
        public ActionResult InfoShopView(EditShopViewModel req)
        {
            InfoShopViewModel model = new InfoShopViewModel()
            {
                ID = req.ID,
                Code = req.Code,
                MobileNumber = req.MobileNumber,
                ContractNumber = req.ContractNumber,
                CustomerCode = req.CustomerCode,
                CustomerToken = req.CustomerToken,
                StoreName = req.StoreName,
                FullName = req.FullName,
                Email = req.Email,
                ProductBusiness = req.ProductBusiness,
                TypeBusiness = req.TypeBusiness,
                TypeShop = req.TypeShop,
                AccountType = req.AccountType,
                ListAccountType = req.ListAccountType,
                NumberOfEmployees = req.NumberOfEmployees,
                ListNumberOfEmployee = req.ListNumberOfEmployee,
                Street = req.Street,
                ProvinceID = req.ProvinceID,
                ListProvince = req.ListProvince,
                DistrictID = req.DistrictID,
                ListDistrict = req.ListDistrict,
                WardID = req.WardID,
                ListWard = req.ListWard,
                MainTransportServiceID = req.MainTransportServiceID,
                ListMainTransportService = req.ListMainTransportService,
                ExtraTransportServiceID = req.ExtraTransportServiceID,
                ListExtraTransportService = req.ListExtraTransportService,
                IsExistExtraTransportServiceID = req.IsExistExtraTransportServiceID,
                CollectionType = req.CollectionType,
                ListCollectionType = req.ListCollectionType,
                CollectionPostOfficeID = req.CollectionPostOfficeID,
                ListCollectionPostOffice = req.ListCollectionPostOffice,
                ChildCustomerCode = req.ChildCustomerCode,
                CODPeriodType = req.CODPeriodType,
                FeePeriodType = req.FeePeriodType,
                ListPeriodType = req.ListPeriodType,
                PaymentMethod = req.PaymentMethod,
                IsDisplay = req.IsDisplay,
                ListPaymentMethod = req.ListPaymentMethod,
                FullNameOfPayment = req.FullNameOfPayment,
                NumberOfPayment = req.NumberOfPayment,
                NameOfPayment = req.NameOfPayment,
                BranchOfPayment = req.BranchOfPayment,
                EmailBCG = req.EmailBCG,
                EmailCSKH = req.EmailCSKH,
                TaxCode = req.TaxCode,
                PartnerID = req.PartnerID,
                ListPartner = req.ListPartner,
                IsSercurity = req.IsSercurity,
                ListIsSercurity = req.ListIsSercurity
            };
            return PartialView(model);
        }

        public JsonResult JInfoShop(InfoShopViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.EditShop(new ZShopEditShopRequest()
                {
                    ID = req.ID,
                    ContractNumber = req.ContractNumber,
                    CustomerCode = req.CustomerCode,
                    CustomerToken = req.CustomerToken,
                    StoreName = req.StoreName,
                    FullName = req.FullName,
                    Email = req.Email,
                    ProductBusiness = req.ProductBusiness,
                    TypeBusiness = req.TypeBusiness,
                    TypeShop = req.TypeShop,
                    AccountType = req.AccountType,
                    NumberOfEmployees = req.NumberOfEmployees,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street,
                    MainTransportServiceID = req.MainTransportServiceID,
                    ExtraTransportServiceID = req.ExtraTransportServiceID,
                    CollectionType = req.CollectionType,
                    CollectionPostOfficeID = req.CollectionPostOfficeID,
                    ChildCustomerCode = req.ChildCustomerCode,
                    CODPeriodType = req.CODPeriodType,
                    FeePeriodType = req.FeePeriodType,
                    PaymentMethod = req.PaymentMethod,
                    FullNameOfPayment = req.FullNameOfPayment,
                    NameOfPayment = req.NameOfPayment,
                    BranchOfPayment = req.BranchOfPayment,
                    NumberOfPayment = req.NumberOfPayment,
                    EmailBCG = req.EmailBCG,
                    EmailCSKH = req.EmailCSKH,
                    TaxCode = req.TaxCode,
                    PartnerID = req.PartnerID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JInfoShop: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tab Tài liệu
        public ActionResult DocumentView()
        {
            return PartialView();
        }
        #endregion

        #region Tab Kỳ đối soát

        #region Danh sách kỳ đối soát
        public ActionResult PeriodView(EditShopViewModel req)
        {
            PeriodViewModel model = new PeriodViewModel()
            {
                CODPeriodType = req.CODPeriodType,
                FeePeriodType = req.FeePeriodType,
                ShopID = req.ID
            };
            return PartialView(model);
        }

        public ActionResult SearchListPeriodView(PeriodViewModel req)
        {
            List<SearchListPeriodDataViewModel> listData = new List<SearchListPeriodDataViewModel>();
            var res = PeriodRepository.SearchListPeriod(new ZPeriodListRequest()
            {
                Type = req.Type,
                ShopID = req.ShopID
            });
            if (res.Code == "00")
            {
                var listItem = JsonConvert.DeserializeObject<List<ZPeriodListResponse>>(res.Data);
                if (req.PeriodType == 1)
                {
                    foreach (var item in listItem)
                    {
                        listData.Add(new SearchListPeriodDataViewModel()
                        {
                            ID = item.ID,
                            Value = item.Value,
                            Text = XData.WeekByValue(item.Value.ToString())
                        });
                    }
                }
                else if (req.PeriodType == 2)
                {
                    foreach (var item in listItem)
                    {
                        listData.Add(new SearchListPeriodDataViewModel()
                        {
                            ID = item.ID,
                            Value = item.Value,
                            Text = XData.AllDaysByValue(item.Value.ToString())
                        });
                    }
                }
            }
            SearchListPeriodViewModel model = new SearchListPeriodViewModel()
            {
                Type = req.Type,
                PeriodType = req.PeriodType,
                ShopID = req.ShopID,
                ListAllDay = XData.GetAllDays(),
                ListAllWeek = XData.GetWeek(),
                ListData = listData
            };
            return PartialView(model);
        }
        #endregion

        #region Chi tiết kỳ đối soát
        public ActionResult DetailPeriodView(EditShopViewModel req)
        {
            List<string> ListCOD = new List<string>();
            List<string> ListFEE = new List<string>();
            var resCOD = PeriodRepository.SearchListPeriod(new ZPeriodListRequest()
            {
                Type = "COD",
                ShopID = req.ID
            });
            if (resCOD.Code == "00")
            {
                var listItem = JsonConvert.DeserializeObject<List<ZPeriodListResponse>>(resCOD.Data);
                if (req.CODPeriodType == 1)
                {
                    foreach (var item in listItem)
                    {
                        string Name = XData.WeekByValue(item.Value.ToString());
                        ListCOD.Add(Name);
                    }
                }
                else if (req.CODPeriodType == 2)
                {
                    foreach (var item in listItem)
                    {
                        string Name = XData.AllDaysByValue(item.Value.ToString());
                        ListCOD.Add(Name);
                    }
                }
            }

            var resFEE = PeriodRepository.SearchListPeriod(new ZPeriodListRequest()
            {
                Type = "FEE",
                ShopID = req.ID
            });
            if (resFEE.Code == "00")
            {
                var listItem = JsonConvert.DeserializeObject<List<ZPeriodListResponse>>(resFEE.Data);
                if (req.CODPeriodType == 1)
                {
                    foreach (var item in listItem)
                    {
                        string Name = XData.WeekByValue(item.Value.ToString());
                        ListFEE.Add(Name);
                    }
                }
                else if (req.CODPeriodType == 2)
                {
                    foreach (var item in listItem)
                    {
                        string Name = XData.AllDaysByValue(item.Value.ToString());
                        ListFEE.Add(Name);
                    }
                }
            }

            DetailPeriodViewModel model = new DetailPeriodViewModel()
            {
                ListCOD = ListCOD,
                ListFee = ListFEE,
            };
            return PartialView(model);
        }
        #endregion

        #region Thêm kỳ đối soát
        public JsonResult JAddPeriod(SearchListPeriodDataViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PeriodRepository.AddPeriod(new ZPeriodAddRequest()
                {
                    Value = req.Value,
                    Type = req.Type,
                    ShopID = req.ShopID,
                    CreatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JAddPeriod: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa kỳ đối soát
        public JsonResult JRemovePeriod(SearchListPeriodDataViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PeriodRepository.RemovePeriod(new ZPeriodRemoveRequest()
                {
                    ID = req.ID,
                    ShopID = req.ShopID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JRemovePeriod: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #endregion

        #region Tab Địa chỉ và mẫu in
        public ActionResult AddressAndLabelPrintView(EditShopViewModel req)
        {
            AddressAndLabelPrintViewModel model = new AddressAndLabelPrintViewModel()
            {
                StoreName = req.StoreName,
                ShopID = req.ID,
                CollectionPostOfficeID = req.CollectionPostOfficeID
            };
            return PartialView(model);
        }

        #region Hiển thị và sửa địa chỉ lấy hàng
        public ActionResult ShowInfoPickUpAddress(AddressAndLabelPrintViewModel req, string IsDisplay = "Y")
        {
            ShowInfoPickUpAddressViewModel model = new ShowInfoPickUpAddressViewModel();
            var res = PickUpAddressRepository.GetByID(new ZPickUpAddressGetByIDRequest() { PickUpAddressID = req.PickUpAddressID, ShopID = req.ShopID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZPickUpAddressGetByIDResponse>(res.Data);
                model = new ShowInfoPickUpAddressViewModel()
                {
                    PickUpAddressID = item.PickUpAddressID,
                    FullName = item.FullName,
                    MobileNumber = item.MobileNumber,
                    PickUpName = item.PickUpName,
                    CollectionPostOfficeID = item.CollectionPostOfficeID,
                    ListCollectionPostOffice = XCommon.SearchListCollectionPostOffice(new ZCollectionPostOfficeSearchListRequest() { IsLock = "N" }),
                    ProvinceID = item.ProvinceID,
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    //DistrictID = item.DistrictID,
                    //ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = item.ProvinceID }),
                    WardID = item.WardID,
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = item.ProvinceID }),
                    Street = item.Street,
                    FullAddress = item.FullAddress,
                    ShopID = item.ShopID,
                    IsDefault = item.IsDefault
                };
            }
            ViewBag.IsDisplay = IsDisplay;
            return PartialView(model);
        }

        public JsonResult JUpdatePickUpAddress(ShowInfoPickUpAddressViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PickUpAddressRepository.Update(new ZPickUpAddressUpdateRequest()
                {
                    PickUpAddressID = req.PickUpAddressID,
                    PickUpName = req.PickUpName,
                    CollectionPostOfficeID = req.CollectionPostOfficeID,
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    ProvinceID = req.ProvinceID,
                    //DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street,
                    IsDefault = req.IsDefault,
                    ShopID = req.ShopID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JUpdatePickUpAddress: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm địa chỉ lấy hàng
        public ActionResult AddPickUpAddressView(AddressAndLabelPrintViewModel req)
        {
            AddPickUpAddressViewModel model = new AddPickUpAddressViewModel()
            {
                ShopID = req.ShopID,
                CollectionPostOfficeID = req.CollectionPostOfficeID,
                ListCollectionPostOffice = XCommon.SearchListCollectionPostOffice(new ZCollectionPostOfficeSearchListRequest() { IsLock = "N" }),
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                //ListDistrict = new List<DistrictModel>(),
                ListWard = new List<WardModel>()
            };
            return PartialView(model);
        }

        public JsonResult JAddPickUpAddress(AddPickUpAddressViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PickUpAddressRepository.Add(new ZPickUpAddressAddRequest()
                {
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    PickUpName = req.PickUpName,
                    CollectionPostOfficeID = req.CollectionPostOfficeID,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street,
                    IsDefault = req.IsDefault,
                    ShopID = req.ShopID,
                    CreatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JAddPickUpAddress: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa địa chỉ lấy hàng
        public JsonResult JRemovePickUpAddress(ShowInfoPickUpAddressViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PickUpAddressRepository.Remove(new ZPickUpAddressRemoveRequest()
                {
                    PickUpAddressID = req.PickUpAddressID,
                    ShopID = req.ShopID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JRemovePickUpAddress: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Hiển thị và sửa Mẫu in
        public ActionResult ShowInfoLabelPrint(AddressAndLabelPrintViewModel req, string IsDisplay = "Y")
        {
            ShowInfoLabelPrintViewModel model = new ShowInfoLabelPrintViewModel();
            var res = LabelPrintRepository.GetByID(new ZLabelPrintGetByIDRequest()
            {
                LabelPrintID = req.LabelPrintID,
                ShopID = req.ShopID,
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZLabelPrintGetByIDResponse>(res.Data);
                model = new ShowInfoLabelPrintViewModel()
                {
                    LabelPrintID = item.LabelPrintID,
                    LabelName = item.LabelName,
                    FullName = item.FullName,
                    MobileNumber = item.MobileNumber,
                    ProvinceID = item.ProvinceID,
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    //DistrictID = item.DistrictID,
                    //ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = item.ProvinceID }),
                    WardID = item.WardID,
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = item.ProvinceID }),
                    Street = item.Street,
                    FullAddress = item.FullAddress,
                    PickUpAddressID = item.PickUpAddressID,
                    ShopID = item.ShopID,
                    IsDefault = item.IsDefault
                };
            }
            ViewBag.IsDisplay = IsDisplay;
            return PartialView(model);
        }

        public JsonResult JUpdateLabelPrint(ShowInfoLabelPrintViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = LabelPrintRepository.Update(new ZLabelPrintUpdateRequest()
                {
                    LabelPrintID = req.LabelPrintID,
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    ProvinceID = req.ProvinceID,
                    //DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street,
                    IsDefault = req.IsDefault,
                    ShopID = req.ShopID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JUpdateLabelPrint: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm mẫu in
        public ActionResult AddLabelPrintView(ShowInfoPickUpAddressViewModel req)
        {
            AddLabelPrintViewModel model = new AddLabelPrintViewModel()
            {
                ShopID = req.ShopID,
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                ListDistrict = new List<DistrictModel>(),
                ListWard = new List<WardModel>(),
                PickUpAddressID = req.PickUpAddressID,
            };
            return PartialView(model);
        }

        public JsonResult JAddLabelPrint(AddLabelPrintViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = LabelPrintRepository.Add(new ZLabelPrintAddRequest()
                {
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street,
                    IsDefault = req.IsDefault,
                    PickUpAddressID = req.PickUpAddressID,
                    ShopID = req.ShopID,
                    CreatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JAddLabelPrint: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa mẫu in
        public JsonResult JRemoveLabelPrint(ShowInfoLabelPrintViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = LabelPrintRepository.Remove(new ZLabelPrintRemoveRequest()
                {
                    LabelPrintID = req.LabelPrintID,
                    ShopID = req.ShopID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JRemoveLabelPrint: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Đặt lại giá trị mặc định cho mẫu in
        public JsonResult JDefaultLabelPrint(ShowInfoLabelPrintViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = LabelPrintRepository.DefaultAccordingToPickUpAddress(new ZLabelPrintDefaultAccordingToPickUpAddressRequest()
                {
                    LabelPrintID = req.LabelPrintID,
                    ShopID = req.ShopID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JDefaultLabelPrint: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #endregion

        #region Chi tiết thông tin cửa hàng
        public ActionResult DetailShopView(SearchListShopViewModel req)
        {
            DetailShopViewModel model = null;
            var res = ShopRepository.GetByID(new ZShopGetByIDRequest() { ID = req.ID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(res.Data);
                model = new DetailShopViewModel()
                {
                    ID = item.ID,
                    Code = item.Code,
                    FullName = item.FullName,
                    StoreName = item.StoreName,
                    Email = item.Email,
                    MobileNumber = item.MobileNumber,
                    ProductBusiness = item.ProductBusiness,
                    TypeBusiness = item.TypeBusiness,
                    TypeShop = item.TypeShop,
                    //Note = item.Note,
                    Status = item.Status,
                    AccountType = item.AccountType,
                    NumberOfEmployees = item.NumberOfEmployees,
                    ContractNumber = item.ContractNumber,
                    CustomerCode = item.CustomerCode,
                    ProvinceID = item.ProvinceID,
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    DistrictID = item.DistrictID,
                    ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = item.ProvinceID }),
                    WardID = item.WardID,
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = item.DistrictID }),
                    Street = item.Street,
                    MainTransportServiceID = item.MainTransportServiceID,
                    ListMainTransportService = new List<TransportServiceModel>(),
                    ExtraTransportServiceID = item.ExtraTransportServiceID,
                    ListExtraTransportService = new List<TransportServiceModel>(),
                    IsExistExtraTransportServiceID = item.IsExistExtraTransportServiceID,
                    //PickUpAddress = item.PickUpAddress,
                    CollectionType = item.CollectionType,
                    CollectionTypeName = item.CollectionTypeName,
                    //CollectionPostcode = item.CollectionPostcode,
                    CollectionPostOfficeID = item.CollectionPostOfficeID,
                    ListCollectionPostOffice = XCommon.SearchListCollectionPostOffice(new ZCollectionPostOfficeSearchListRequest() { IsLock = "N" }),
                    ChildCustomerCode = item.ChildCustomerCode,
                    CODPeriodType = item.CODPeriodType,
                    CODPeriodTypeName = XData.PeriodTypeByValue(item.CODPeriodType.ToString()),
                    FeePeriodType = item.FeePeriodType,
                    FeePeriodTypeName = XData.PeriodTypeByValue(item.FeePeriodType.ToString()),
                    PaymentMethod = item.PaymentMethod,
                    PaymentMethodName = XData.GetNamePaymentMethod(item.PaymentMethod.ToString()),
                    FullNameOfPayment = item.FullNameOfPayment,
                    NameOfPayment = item.NameOfPayment,
                    BranchOfPayment = item.BranchOfPayment,
                    NumberOfPayment = item.NumberOfPayment,
                    EmailBCG = item.EmailBCG,
                    EmailCSKH = item.EmailCSKH,
                    ListAccountType = XData.GetListAccountType(),
                    ListNumberOfEmployee = XData.GetListNumberOfEmployee(),
                    TaxCode = item.TaxCode,
                    CustomerToken = item.CustomerToken,
                    PartnerID = item.PartnerID,
                    ListPartner = XCommon.SearchListPartner(new ZPartnerSearchListRequest() { IsLock = "N" }),
                    ListIsSercurity = XData.GetListIsSercurity()
                };

                List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { IsLock = "N", ParentID = 0, Channel = "EMSONE" });
                if (ListTransportService.Count > 0)
                {
                    model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                    model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
                }
            }
            return View(model);
        }

        public ActionResult HistoryByShopID(int ShopID)
        {
            List<ShopHistoryViewModel> model = new List<ShopHistoryViewModel>();
            var res = ShopRepository.GetHistoryByID(new ZShopGetHistoryByIDRequest() { ID = ShopID });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZShopGetHistoryByIDResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new ShopHistoryViewModel()
                    {
                        ActionCode = item.ActionCode,
                        ActionName = item.ActionName,
                        Description = item.Description,
                        CreatedName = item.CreatedName,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Duyệt thông tin cửa hàng
        public ActionResult ApprovedShopView(int ShopID)
        {
            ApprovedShopViewModel model = null;
            var res = ShopRepository.GetByID(new ZShopGetByIDRequest() { ID = ShopID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(res.Data);
                model = new ApprovedShopViewModel()
                {
                    ID = item.ID,
                    Code = item.Code,
                    FullName = item.FullName,
                    StoreName = item.StoreName,
                    Email = item.Email,
                    MobileNumber = item.MobileNumber,
                    ProductBusiness = item.ProductBusiness,
                    TypeBusiness = item.TypeBusiness,
                    TypeShop = item.TypeShop,
                    Status = item.Status,
                    ContractNumber = item.ContractNumber,
                    CustomerCode = item.CustomerCode,
                    CustomerToken = item.CustomerToken,
                    CollectionPostOfficeID = item.CollectionPostOfficeID,
                    ListCollectionPostOffice = XCommon.SearchListCollectionPostOffice(new ZCollectionPostOfficeSearchListRequest() { IsLock = "N" }),
                    AccountType = 99,
                    //AccountType = item.AccountType,
                    ListAccountType = XData.GetListAccountType(),
                    NumberOfEmployees = item.NumberOfEmployees,
                    ListNumberOfEmployee = XData.GetListNumberOfEmployee(),
                    ProvinceID = item.ProvinceID,
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    //DistrictID = item.DistrictID,
                    //ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = item.ProvinceID }),
                    WardID = item.WardID,
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = item.ProvinceID }),
                    Street = item.Street,
                    MainTransportServiceID = item.MainTransportServiceID,
                    ListMainTransportService = new List<TransportServiceModel>(),
                    ExtraTransportServiceID = item.ExtraTransportServiceID,
                    ListExtraTransportService = new List<TransportServiceModel>(),
                    IsExistExtraTransportServiceID = item.IsExistExtraTransportServiceID,
                    ListPeriodType = XData.GetPeriodType(),
                    ListPaymentMethod = XData.GetListPaymentMethod(),
                    ListCollectionType = XData.GetListCollectionType(),
                    EmailBCG = item.EmailBCG,
                    EmailCSKH = item.EmailCSKH,
                    TaxCode = item.TaxCode,
                    PartnerID = item.PartnerID,
                    ListPartner = XCommon.SearchListPartner(new ZPartnerSearchListRequest() { IsLock = "N" }),
                    ListIsSercurity = XData.GetListIsSercurity()
                };

                List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { IsLock = "N", ParentID = 0, Channel = "EMSONE" });
                if (ListTransportService.Count > 0)
                {
                    model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                    model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
                }
            }
            return View(model);
        }

        public JsonResult JApprovedShop(ApprovedShopViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.ApprovedShop(new ZShopApprovedShopRequest()
                {
                    ID = req.ID,
                    MobileNumber = req.MobileNumber,
                    FullName = req.FullName,
                    Email = req.Email,
                    Password = Membership.GeneratePassword(8, 3),
                    ContractNumber = req.ContractNumber,
                    CustomerCode = req.CustomerCode,
                    CustomerToken = req.CustomerToken,
                    AccountType = req.AccountType,
                    NumberOfEmployees = req.NumberOfEmployees,
                    MainTransportServiceID = req.MainTransportServiceID,
                    ExtraTransportServiceID = req.ExtraTransportServiceID,
                    CollectionType = req.CollectionType,
                    CollectionPostOfficeID = req.CollectionPostOfficeID,
                    ChildCustomerCode = req.ChildCustomerCode,
                    CODPeriodType = req.CODPeriodType,
                    FeePeriodType = req.FeePeriodType,
                    PaymentMethod = req.PaymentMethod,
                    FullNameOfPayment = req.FullNameOfPayment,
                    NameOfPayment = req.NameOfPayment,
                    BranchOfPayment = req.BranchOfPayment,
                    NumberOfPayment = req.NumberOfPayment,
                    EmailBCG = req.EmailBCG,
                    EmailCSKH = req.EmailCSKH,
                    TaxCode = req.TaxCode,
                    PartnerID = req.PartnerID,
                    ConfirmedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JApprovedShop: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Từ chối thông tin cửa hàng
        public ActionResult RejectShopView(ApprovedShopViewModel req)
        {
            RejectShopViewModel model = new RejectShopViewModel()
            {
                ID = req.ID
            };
            return PartialView(model);
        }
        public JsonResult JRejectShop(RejectShopViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.RejectShop(new ZShopRejectShopRequest()
                {
                    ID = req.ID,
                    ReasonReject = req.Reason,
                    ConfirmedBy = XHelper.Session.UserProfile.ID,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JRejectShop: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mở/ Khóa cửa hàng
        public JsonResult ChangeIsLock(ZShopChangeIsLockRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.ChangeIsLock(new ZShopChangeIsLockRequest()
                {
                    ID = req.ID,
                    IsLock = req.IsLock,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ChangeIsLock: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet); ;
        }
        #endregion

        #region Reset mật khẩu cửa hàng
        public JsonResult ResetPasswordShop(ZShopResetPasswordShopRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.ResetPasswordShop(new ZShopResetPasswordShopRequest()
                {
                    ID = req.ID,
                    MobileNumber = req.MobileNumber,
                    Email = req.Email,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ResetPasswordShop: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TEMP
        //public ZResponse SendConfirmationEmail(SendMailModel reqSendMail)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //using (MailMessage mail = new MailMessage(from, to))
        //{
        //    mail.Subject = "EMS-ONE - KÍCH HOẠT TÀI KHOẢN";
        //    mail.BodyEncoding = System.Text.Encoding.UTF8;
        //    mail.Body = "Chào " + reqSendMail.FullName + ",";
        //    mail.Body += "<p>Bạn đã được cấp tài khoản trên hệ thống EMS One</p>";
        //    mail.Body += "<p><b>Số điện thoại đăng nhập: </b> " + reqSendMail.MobileNumber + "</p>";
        //    mail.Body += "<p><b>Mật khẩu: </b> " + reqSendMail.Password + "</p>";
        //    mail.Body += "<p>Đăng nhập <a href='" + XHelper.Ems247Url + "'>tại đây</a> !</p>";
        //    mail.IsBodyHtml = true;
        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = "smtp.gmail.com";
        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    smtp.EnableSsl = true;
        //    NetworkCredential networkCredential = new NetworkCredential(from, password);
        //    smtp.UseDefaultCredentials = true;
        //    smtp.Credentials = networkCredential;
        //    smtp.Port = 587;
        //    smtp.Send(mail);
        //    res = UpdateConfirmationEmail(reqSendMail.IDShop);
        //    if (res.Code != "00")
        //    {
        //        XHelper.LogToFile(LogFileType.MESSAGE, "ShopController.SendConfirmationEmail: Faild - " + reqSendMail.IDShop);
        //    }
        //    return res;
        //}
        //        //new TransMail
        //        var str = JsonConvert.SerializeObject(new
        //        {
        //            MobileNumber = reqSendMail.MobileNumber,
        //            Password = reqSendMail.Password
        //        });
        //        var link = Security.MD5Encrypt(str, Security.PASSWORD_SALT);
        //        var stringBody = "";
        //        stringBody = "<div>Chào " + reqSendMail.FullName + ",";
        //        stringBody += "<p>Bạn đã được cấp tài khoản trên hệ thống EMS One. Thông tin đăng nhập như sau:</p>";
        //        stringBody += "<p>Số điện thoại đăng nhập: " + reqSendMail.MobileNumber + "</p>";
        //        stringBody += "<p>Truy cập vào đây để tạo mật khẩu và sử dụng: <a href=" + XHelper.Ems247Url + "Account/UpdatePasswork?data=" + link + ">tại đây</a> !</p>";
        //        stringBody += "<p>Trân trọng cảm ơn!</p></div>";


        //        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        //        var baseAddress = "https://api.transmail.com/v1.1/email";

        //        var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
        //        http.Accept = "application/json";
        //        http.ContentType = "application/json; charset=utf-8";
        //        http.Method = "POST";
        //        http.PreAuthenticate = true;
        //        http.Headers.Add("Authorization", "Zoho-enczapikey wSsVR60k8h73Dal+nDCldLhtyFpUVV3+FUgujFuk6XL1TazBocdun0CcVgXzFPNLEGVhFjFBpLl9mx4H0zAGjtp7m15WWiiF9mqRe1U4J3x17qnvhDzPV21akBuKKokIwARsnGNgEM0k+g==");
        //        JObject parsedContent = JObject.Parse("{'bounce_address':'dev@ems.ems.com.vn','from': { 'address': 'transmail@ems.com.vn'},'to': [{'email_address': {'address': '" + reqSendMail.ToEmail + "','name': 'ktcntt'}}],'subject':'EMS-ONE - KÍCH HOẠT TÀI KHOẢN','htmlbody':'" + stringBody + "'}");
        //        Console.WriteLine(parsedContent.ToString());
        //        UTF8Encoding encoding = new UTF8Encoding();
        //        Byte[] bytes = encoding.GetBytes(parsedContent.ToString());

        //        Stream newStream = http.GetRequestStream();
        //        newStream.Write(bytes, 0, bytes.Length);
        //        newStream.Close();

        //        var response = http.GetResponse();

        //        var stream = response.GetResponseStream();
        //        var sr = new StreamReader(stream);
        //        var content = sr.ReadToEnd();
        //        Console.WriteLine(content);
        //        res = UpdateConfirmationEmail(reqSendMail.IDShop);
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.SendConfirmationEmail: - " + content);
        //    }
        //    catch (SmtpFailedRecipientsException ex)
        //    {
        //        ModelState.AddModelError("99", ex.Message);
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.SendConfirmationEmail: Faild EX - " + ex.ToString());
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}

        //public static void Main(string[] args)
        //{
        //    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        //    var baseAddress = "https://api.transmail.com/v1.1/email";

        //    var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
        //    http.Accept = "application/json";
        //    http.ContentType = "application/json";
        //    http.Method = "POST";
        //    http.PreAuthenticate = true;
        //    http.Headers.Add("Authorization", "Zoho-enczapikey wSsVR60k8h73Dal+nDCldLhtyFpUVV3+FUgujFuk6XL1TazBocdun0CcVgXzFPNLEGVhFjFBpLl9mx4H0zAGjtp7m15WWiiF9mqRe1U4J3x17qnvhDzPV21akBuKKokIwARsnGNgEM0k+g==");
        //    JObject parsedContent = JObject.Parse("{'bounce_address':'dev@ems.ems.com.vn','from': { 'address': 'transmail@ems.com.vn'},'to': [{'email_address': {'address': 'ktcntt@ems.com.vn','name': 'ktcntt'}}],'subject':'Test Email','htmlbody':'<div><b> Test email sent successfully.  </b></div>'}");
        //    Console.WriteLine(parsedContent.ToString());
        //    ASCIIEncoding encoding = new ASCIIEncoding();
        //    Byte[] bytes = encoding.GetBytes(parsedContent.ToString());

        //    Stream newStream = http.GetRequestStream();
        //    newStream.Write(bytes, 0, bytes.Length);
        //    newStream.Close();

        //    var response = http.GetResponse();

        //    var stream = response.GetResponseStream();
        //    var sr = new StreamReader(stream);
        //    var content = sr.ReadToEnd();
        //    Console.WriteLine(content);
        //}
        #endregion

        public JsonResult SendEmailRegiterSuccess(ZShopSearchInfoSendEmailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.ResetPasswordShop(new ZShopResetPasswordShopRequest()
                {
                    ID = req.ShopID,
                    MobileNumber = req.MobileNumber,
                    Email = req.ToEmail,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });

                if (res.Code == "00")
                {
                    req.Password = Security.DEFAULT_PASSWORD;

                    res = ShopRepository.SendEmailRegiterSuccess(req);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ResetPasswordShop: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}