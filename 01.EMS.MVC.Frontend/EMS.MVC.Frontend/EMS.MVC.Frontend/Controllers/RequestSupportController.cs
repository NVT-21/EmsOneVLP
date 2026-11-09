using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Newtonsoft.Json;
using EMS.Library;
using EMS.Entity;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
namespace EMS.MVC.Frontend.Controllers
{
    public class RequestSupportController : BaseController
    {
        #region Danh sách yêu cầu hỗ trợ
        [AuthorizeController(FunctionalTitle = "DANH_SACH_YEU_CAU_HO_TRO")]
        public ActionResult RequestSupportView()
        {
            RequestSupportViewModel model = new RequestSupportViewModel()
            {
                TypeDate = 1,
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListRequestSupportGroup = XCommon.SearchListRequestSupportGroup(new ZRequestSupportGroupListRequest() { ParentID = 0 }),
                ListRequestSupportType = new List<ZRequestSupportGroupListResponse>(),
                ListRequestSupportStatus = XData.GetListRequestSupportStatus(),
            };
            return View(model);
        }

        public ActionResult SearchListRequestSupportView(RequestSupportViewModel req)
        {
            List<SearchListRequestSupportViewModel> model = new List<SearchListRequestSupportViewModel>();
            var res = RequestSupportRepository.SearchListRequestSupport(new ZRequestSupportFESearchListRequest()
            {
                TypeDate = 1,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                RequestGroup = req.RequestSupportGroup,
                RequestType = req.RequestSupportType,
                Status = req.RequestSupportStatus,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var ListItem = JsonConvert.DeserializeObject<List<ZRequestSupportFESearchListResponse>>(res.Data);
                foreach (var item in ListItem)
                {
                    model.Add(new SearchListRequestSupportViewModel()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        EShippingCode = item.EShippingCode,
                        RequestGroup = item.RequestGroup,
                        RequestType = item.RequestType,
                        Status = new BaseModel() { Code = item.Status.Code, Name = item.Status.Name },
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate
                    });
                }
            }
            return PartialView(model);
        }

        public ActionResult ExportListRequestSupport(RequestSupportViewModel req)
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

            //Create title 
            var TitleRow = sheet.CreateRow(0);
            TitleRow.CreateCell(0).SetCellValue("DANH SÁCH YÊU CẦU HỖ TRỢ");
            TitleRow.GetCell(0).CellStyle = TitleStyle;

            //sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 12));
            //sheet.CreateRow(0).GetCell(0).CellStyle = center;
            var res = RequestSupportRepository.SearchListRequestSupport(new ZRequestSupportFESearchListRequest()
            {
                TypeDate = 1,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                RequestGroup = req.RequestSupportGroup,
                RequestType = req.RequestSupportType,
                Status = req.RequestSupportStatus,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });

            List<ZRequestSupportFESearchListResponse> ListItem = new List<ZRequestSupportFESearchListResponse>();

            if (res.Code == "00")
            {
                ListItem = JsonConvert.DeserializeObject<List<ZRequestSupportFESearchListResponse>>(res.Data);
            }

            //Create a header row
            var headerRow = sheet.CreateRow(2);
            int ColumnIndex = 0;
            var TotalColumn = 7;

            headerRow.CreateCell(ColumnIndex++).SetCellValue("STT");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã yêu cầu");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã bưu gửi");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Nhóm yêu cầu");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Loại yêu cầu");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Trạng thái");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Ngày tạo");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Ngày cập nhật");

            for (var i = 0; i <= TotalColumn; i++)
            {
                headerRow.GetCell(i).CellStyle = HeaderStyle;
            }

            int rowNumber = 3;
            int rowIndex = 0;
            var stt = 1;
            foreach (var item in ListItem)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(rowIndex++).SetCellValue(stt.ToString());
                row.CreateCell(rowIndex++).SetCellValue(item.Code);
                row.CreateCell(rowIndex++).SetCellValue(item.EShippingCode);
                row.CreateCell(rowIndex++).SetCellValue(item.RequestGroup);
                row.CreateCell(rowIndex++).SetCellValue(item.RequestType);
                row.CreateCell(rowIndex++).SetCellValue(item.Status.Name);
                row.CreateCell(rowIndex++).SetCellValue(item.CreatedDate);
                row.CreateCell(rowIndex++).SetCellValue(item.UpdatedDate);

                for (var i = 0; i <= TotalColumn; i++)
                {
                    row.GetCell(i).CellStyle = BodyTextStyle;
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

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            //application/vnd.ms-excel xls
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet xlsx
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachYeuCauHoTro.xls");
        }
        #endregion

        #region Thêm yêu cầu hỗ trợ
        [AuthorizeController(FunctionalTitle = "THEM_YEU_CAU_HO_TRO")]
        public ActionResult AddRequestSupportView(AddRequestSupportViewModel req)
        {
            AddRequestSupportViewModel model = new AddRequestSupportViewModel()
            {
                EShippingCode = req.EShippingCode,
                ListRequestSupportGroup = XCommon.SearchListRequestSupportGroup(new ZRequestSupportGroupListRequest() { ParentID = 0 }),
                ListRequestSupportType = new List<ZRequestSupportGroupListResponse>(),
                ListRequestSupportServiceGroup = XData.GetListRequestSupportServiceGroup(),
                ListRequestSupportScope = XData.GetListRequestSupportScope(),
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            };
            return PartialView(model);
        }

        public ActionResult SearchInfoByRequestSupportTypeView(AddRequestSupportViewModel req)
        {
            SearchInfoByRequestSupportTypeViewModel model = new SearchInfoByRequestSupportTypeViewModel();
            if (req.RequestSupportType == "6")
            {
                //Sửa thông tin lấy hàng
                model.Sender = new CustomerModel()
                {
                    Province = new ProvinceModel() { ID = req.Sender.Province.ID },
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    District = new DistrictModel() { ID = req.Sender.District.ID },
                    ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = req.Sender.Province.ID }),
                    Ward = new WardModel() { ID = req.Sender.Ward.ID },
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = req.Sender.District.ID }),
                    Street = req.Sender.Street
                };
            }
            else if (req.RequestSupportType == "7")
            {
                //Sửa tiền đơn hàng (tiền COD)
                model.TotalCOD = req.TotalCOD;
            }
            else if (req.RequestSupportType == "8")
            {
                //Sửa số điện thoại giao hàng
                model.Receiver = new CustomerModel()
                {
                    MobileNumber = req.Receiver.MobileNumber
                };
            }else if(req.RequestSupportType == "9")
            {
                //Sửa địa chỉ giao hàng (có thể thu phí hoặc không)
                model.Receiver = new CustomerModel()
                {
                    Province = new ProvinceModel() { ID = req.Receiver.Province.ID },
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    District = new DistrictModel() { ID = req.Receiver.District.ID },
                    ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = req.Receiver.Province.ID }),
                    Ward = new WardModel() { ID = req.Receiver.Ward.ID },
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = req.Receiver.District.ID }),
                    Street = req.Receiver.Street
                }; 
            }
            model.RequestSupportType = req.RequestSupportType;
            return PartialView(model);
        }

        public JsonResult JAddRequestSupport(AddRequestSupportViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = RequestSupportRepository.AddRequestSupport(new ZRequestSupportAddRequest() 
                {
                    EShippingCode = req.EShippingCode,
                    RequestGroup = req.RequestSupportGroup,
                    RequestType = req.RequestSupportType,
                    Description = req.Description,
                    Note = req.Note,
                    UpdateInformation = req.UpdateInformation,
                    Document = req.Document,
                    Priority = req.Priority,
                    Origin = req.Origin,
                    ServiceGroup = req.ServiceGroup,
                    Scope = req.Scope,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.JAddRequestSupport: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chi tiết yêu cầu hỗ trợ
        [AuthorizeController(FunctionalTitle = "CHI_TIET_YEU_CAU_HO_TRO")]
        public ActionResult DetailRequestSupportView(RequestSupportViewModel req)
        {
            var RequestSupportID = Convert.ToInt32(Security.DecodeBase64(req.SearchKey));
            DetailRequestSupportViewModel model = new DetailRequestSupportViewModel();

            var res = RequestSupportRepository.DetailRequestSupport(new ZRequestSupportDetailRequest() 
            {
                RequestSupportID = RequestSupportID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZRequestSupportDetailResponse>(res.Data);
                List<RequestSupportSearchHistoryModel> ListSearchHistory = new List<RequestSupportSearchHistoryModel>();
                foreach (var itemSearchHistory in item.ListSearchHistory)
                {
                    ListSearchHistory.Add(new RequestSupportSearchHistoryModel() 
                    {
                        Description = itemSearchHistory.Description,
                        ResultDocument = itemSearchHistory.ResultDocument,
                        Reason = itemSearchHistory.Reason,
                        Status = new BaseModel()
                        {
                            Code = itemSearchHistory.Status.Code,
                            Name = itemSearchHistory.Status.Name
                        },
                        CreatedName = itemSearchHistory.CreatedName,
                        CreatedDate = itemSearchHistory.CreatedDate
                    });
                }

                model = new DetailRequestSupportViewModel()
                {
                    RequestSupportID = item.RequestSupportID,
                    RequestSupportCode = item.RequestSupportCode,
                    RequestGroup = item.RequestGroup,
                    RequestType = item.RequestType,
                    RequestSupportDescription = item.RequestSupportDescription,
                    RequestSupportDocument = item.RequestSupportDocument,
                    EmployeeName = item.EmployeeName,
                    CreatedDate = item.CreatedDate,
                    RequestSupportStatus = new BaseModel() 
                    {
                       Code = item.RequestSupportStatus.Code,
                       Name = item.RequestSupportStatus.Name
                    },
                    NewUpdateInformation = item.UpdateInformation,
                    OldUpdateInformation = item.OldUpdateInformation,
                    PosC = item.PosC,

                    EShippingCode = item.EShippingCode,
                    OrderID = item.OrderID,
                    OrderCode = item.OrderCode,
                    OrderMessage = item.OrderMessage,
                    WeightBefore = item.WeightBefore,
                    WeightAfter = item.WeightAfter,
                    SenderName = item.SenderName,
                    SenderPhone = item.SenderPhone,
                    SenderFullAddress = item.SenderFullAddress,
                    ReceiverName = item.ReceiverName,
                    ReceiverPhone = item.ReceiverPhone,
                    ReceiverFullAddress = item.ReceiverFullAddress,
                    OrderStatus = new BaseModel() 
                    {
                        Code = item.OrderStatus.Code,
                        Name = item.OrderStatus.Name
                    },
                    TransportOrderValue = item.TransportOrderValue,
                    TotalCOD = item.TotalCOD,
                    TransportFee = item.TransportFee,
                    TransportMainService = item.TransportMainService,
                    TransportExtraService = item.TransportExtraService,

                    ShopID = item.ShopID,
                    ShopCode = item.ShopCode,
                    CrmOrPaypostCode = item.CrmOrPaypostCode,
                    ShopPhone = item.ShopPhone,
                    ShopFullAddress = item.ShopFullAddress,

                    ListSearchHistory = ListSearchHistory,
                };
            }
            return View(model);
        }
        #endregion
    }
}