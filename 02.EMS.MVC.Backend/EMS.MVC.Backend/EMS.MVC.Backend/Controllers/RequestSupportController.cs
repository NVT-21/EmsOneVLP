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
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
namespace EMS.MVC.Backend.Controllers
{
    public class RequestSupportController : BaseController
    {
        #region Danh sách yêu cầu hỗ trợ
        public ActionResult RequestSupportView()
        {
            RequestSupportViewModel model = new RequestSupportViewModel()
            {
                TypeDate = 1,
                ListTypeDate = new List<SelectListItem>()
                {
                    new SelectListItem(){ Value= "1", Text = "Thời gian tạo", Selected = true},
                    new SelectListItem(){ Value= "2", Text = "Thời gian cập nhật" }
                },
                FromDate = DateTime.Now.AddDays(0).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListRequestSupportGroup = XCommon.SearchListRequestSupportGroup(0),
                ListRequestSupportStatus = XData.GetListRequestSupportStatus(),
            };
            return View(model);
        }

        public ActionResult SearchListRequestSupportView(RequestSupportViewModel req)
        {
            List<SearchListRequestSupportViewModel> oListItem = new List<SearchListRequestSupportViewModel>();
            var res = RequestSupportRepository.SearchListRequestSupport(new ZRequestSupportBESearchListRequest()
            {
                TypeDate = req.TypeDate,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                RequestSupportCode = req.RequestSupportCode,
                EShippingCode = req.EShippingCode,
                CrmOrPaypostCode = req.CrmOrPaypostCode,
                RequestSupportGroup = req.RequestSupportGroup,
                RequestSupportType = req.RequestSupportType,
                RequestSupportStatus = req.RequestSupportStatus,
                UserID = XHelper.Session.UserProfile.ID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZRequestSupportBESearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new SearchListRequestSupportViewModel()
                    {
                        RequestSupportID = item.RequestSupportID,
                        RequestSupportCode = item.RequestSupportCode,
                        EShippingCode = item.EShippingCode,
                        RequestSupportType = new BaseModel()
                        {
                            Name = item.RequestSupportType.Name
                        },
                        Description = item.Description,
                        Note = item.Note,
                        RequestSupportStatus = new BaseModel()
                        {
                            Code = item.RequestSupportStatus.Code,
                            Name = item.RequestSupportStatus.Name
                        },
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate,

                        ShopID = item.ShopID,
                        CrmOrPaypostCode = item.CrmOrPaypostCode,

                        RequestSupportGroup = new BaseModel()
                        {
                            ID = item.RequestSupportGroup.ID,
                            Name = item.RequestSupportGroup.Name
                        },
                    });
                }
            }
            List<object> CountRequestSupportStatus = new List<object>();
            ViewBag.ListRequestSupportStatus = XData.GetListRequestSupportStatus();
            foreach (var item in ViewBag.ListRequestSupportStatus)
            {
                var Count = oListItem.Where(m => m.RequestSupportStatus.Code == item.Value).Count();
                CountRequestSupportStatus.Add(new
                {
                    Status = item.Value,
                    Count = Count
                });
            }
            ViewBag.CountRequestSupportStatus = CountRequestSupportStatus;
            return PartialView(oListItem);
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
            var res = RequestSupportRepository.SearchListRequestSupport(new ZRequestSupportBESearchListRequest()
            {
                TypeDate = req.TypeDate,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                RequestSupportCode = req.RequestSupportCode,
                EShippingCode = req.EShippingCode,
                CrmOrPaypostCode = req.CrmOrPaypostCode,
                RequestSupportGroup = req.RequestSupportGroup,
                RequestSupportType = req.RequestSupportType,
                RequestSupportStatus = req.RequestSupportStatus,
                UserID = XHelper.Session.UserProfile.ID
            });

            List<ZRequestSupportBESearchListResponse> ListItem = new List<ZRequestSupportBESearchListResponse>();

            if (res.Code == "00")
            {
                ListItem = JsonConvert.DeserializeObject<List<ZRequestSupportBESearchListResponse>>(res.Data);
                
            }

            //Create a header row
            var headerRow = sheet.CreateRow(2);
            int ColumnIndex = 0;
            var TotalColumn = 9;

            headerRow.CreateCell(ColumnIndex++).SetCellValue("STT");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã khách hàng");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã yêu cầu");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã bưu gửi");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Nhóm yêu cầu");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Loại yêu cầu");
            headerRow.CreateCell(ColumnIndex++).SetCellValue("Nội dung yêu cầu");
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
                row.CreateCell(rowIndex++).SetCellValue(item.CrmOrPaypostCode);
                row.CreateCell(rowIndex++).SetCellValue(item.RequestSupportCode);
                row.CreateCell(rowIndex++).SetCellValue(item.EShippingCode);
                row.CreateCell(rowIndex++).SetCellValue(item.RequestSupportGroup.Name);
                row.CreateCell(rowIndex++).SetCellValue(item.RequestSupportType.Name);
                row.CreateCell(rowIndex++).SetCellValue(item.Description);
                row.CreateCell(rowIndex++).SetCellValue(item.RequestSupportStatus.Name);
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
            sheet.AutoSizeColumn(8);
            sheet.AutoSizeColumn(9);

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

        #region Chi tiết yêu cầu hỗ trợ
        public ActionResult DetailRequestSupportView(SearchListRequestSupportViewModel req)
        {
            //var SearchDecodeBase64 = Security.DecodeBase64(req.SearchKey).Split('&');
            var DecodeBase64 = Security.DecodeBase64(req.SearchKey);
            var SearchDecodeBase64 = DecodeBase64.Split('&');
            DetailRequestSupportViewModel model = new DetailRequestSupportViewModel();
            var res = RequestSupportRepository.DetailRequestSupport(new ZRequestSupportDetailRequest()
            {
                RequestSupportID = Convert.ToInt32(SearchDecodeBase64[0]),
                ShopID = Convert.ToInt32(SearchDecodeBase64[1])
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

                List<RequestSupportSearchProcessModel> ListSearchProcess = new List<RequestSupportSearchProcessModel>();
                foreach (var itemSearchProcess in item.ListSearchProcess)
                {
                    ListSearchProcess.Add(new RequestSupportSearchProcessModel()
                    {
                        Description = itemSearchProcess.Description,
                        ResultDocument = itemSearchProcess.ResultDocument,
                        Result = itemSearchProcess.Result,
                        Type = new BaseModel()
                        {
                            Code = itemSearchProcess.Type.Code,
                            Name = itemSearchProcess.Type.Name
                        },
                        CreatedName = itemSearchProcess.CreatedName,
                        CreatedDate = itemSearchProcess.CreatedDate
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
                       Name = item.RequestSupportStatus.Name,
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

                    ListSearchProcess = ListSearchProcess,
                };
            }
            return View(model);
        }
        #endregion

        #region Chấp nhận yêu cầu hỗ trợ
        public JsonResult JAcceptedStatus(List<SearchListRequestSupportViewModel> model)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ShowListErrorMultipleSelectModel> ListItem = new List<ShowListErrorMultipleSelectModel>();
                foreach (var item in model)
                {
                    var resAcceptedStatus = RequestSupportRepository.SendRequestSupport(new ZRequestSupportSendRequest() 
                    {
                        RequestSupportID = item.RequestSupportID,
                        ShopID = item.ShopID,
                        UserID = XHelper.Session.UserProfile.ID,
                    });
                    if (resAcceptedStatus.Code != "00")
                    {
                        ListItem.Add(new ShowListErrorMultipleSelectModel()
                        {
                            RequestSupportCode = item.RequestSupportCode,
                            Description = resAcceptedStatus.Message
                        });
                    }
                }
                if (ListItem.Count > 0)
                {
                    var TotalSuccess = model.Count - ListItem.Count;
                    res.Code = "10";
                    res.Message = "Duyệt yêu cầu thành công " + TotalSuccess + "/" + model.Count + ", vui lòng nhấn vào Xem chi tiết để kiểm tra.";
                    res.Data = JsonConvert.SerializeObject(ListItem);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Thành công.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.JAcceptedStatus: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Từ chối yêu cầu hỗ trợ
        public ActionResult UnAcceptedStatusView(List<SearchListRequestSupportViewModel> model)
        {
            return PartialView(model);
        }

        public JsonResult JUnAcceptedStatus(List<SearchListRequestSupportViewModel> model, string Reason)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ShowListErrorMultipleSelectModel> ListItem = new List<ShowListErrorMultipleSelectModel>();
                foreach (var item in model)
                {
                    var resUnAcceptedStatus = RequestSupportRepository.UnAcceptedStatus(new ZRequestSupportUnAcceptedStatusRequest()
                    {
                        RequestSupportID = item.RequestSupportID,
                        ShopID = item.ShopID,
                        Reason = Reason,
                        UpdatedBy = XHelper.Session.UserProfile.ID,
                    });
                    if (resUnAcceptedStatus.Code != "00")
                    {
                        ListItem.Add(new ShowListErrorMultipleSelectModel()
                        {
                            RequestSupportCode = item.RequestSupportCode,
                            Description = resUnAcceptedStatus.Message
                        });
                    }
                }
                if (ListItem.Count > 0)
                {
                    var TotalSuccess = model.Count - ListItem.Count;
                    res.Code = "10";
                    res.Message = "Không duyệt yêu cầu thành công " + TotalSuccess + "/" + model.Count + ", vui lòng nhấn vào Xem chi tiết để kiểm tra.";
                    res.Data = JsonConvert.SerializeObject(ListItem);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Không duyệt yêu cầu thành công.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.UnAcceptedStatus: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xử lý yêu cầu hỗ trợ
        public ActionResult ProcessRequestSupportView(ProcessRequestSupportViewModel model)
        {
            model.ListResultRequestSupport = XData.GetListRequestSupportResult();
            return PartialView(model);
        }

        public JsonResult JProcessStatus(ProcessRequestSupportViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = RequestSupportRepository.ProcessStatus(new ZRequestSupportProcessStatusRequest() 
                {
                    RequestSupportID = req.RequestSupportID,
                    ShopID = req.ShopID,
                    ResultDocument = req.ResultDocument,
                    ResultDescription = req.ResultDescription,
                    ResultIndemnify = req.ResultIndemnify,
                    ResultRequestSupport = req.ResultRequestSupport,
                    UpdatedBy = XHelper.Session.UserProfile.ID,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.JProcessStatus: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Đóng yêu cầu hỗ trợ
        public JsonResult JCloseStatus(List<SearchListRequestSupportViewModel> model)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ShowListErrorMultipleSelectModel> ListItem = new List<ShowListErrorMultipleSelectModel>();
                foreach (var item in model)
                {
                    var resCloseStatus = RequestSupportRepository.CloseStatus(new ZRequestSupportCloseStatusRequest()
                    {
                        RequestSupportID = item.RequestSupportID,
                        ShopID = item.ShopID,
                        UpdatedBy = XHelper.Session.UserProfile.ID,
                    });
                    if (resCloseStatus.Code != "00")
                    {
                        ListItem.Add(new ShowListErrorMultipleSelectModel()
                        {
                            RequestSupportCode = item.RequestSupportCode,
                            Description = resCloseStatus.Message
                        });
                    }
                }
                if (ListItem.Count > 0)
                {
                    var TotalSuccess = model.Count - ListItem.Count;
                    res.Code = "10";
                    res.Message = "Đã đóng yêu cầu thành công " + TotalSuccess + "/" + model.Count + ", vui lòng nhấn vào Xem chi tiết để kiểm tra.";
                    res.Data = JsonConvert.SerializeObject(ListItem);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Đã đóng yêu cầu thành công.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.JCloseStatus: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mở lại yêu cầu hỗ trợ
        public JsonResult JReOpenStatus(List<SearchListRequestSupportViewModel> model)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ShowListErrorMultipleSelectModel> ListItem = new List<ShowListErrorMultipleSelectModel>();
                foreach (var item in model)
                {
                    res = RequestSupportRepository.ReOpenStatus(new ZRequestSupportReOpenStatusRequest() 
                    {
                        RequestSupportID = item.RequestSupportID,
                        ShopID = item.ShopID,
                        UpdatedBy = XHelper.Session.UserProfile.ID,
                    });
                    if (res.Code != "00")
                    {
                        ListItem.Add(new ShowListErrorMultipleSelectModel()
                        {
                            RequestSupportCode = item.RequestSupportCode,
                            Description = res.Message
                        });
                    }
                }
                if (ListItem.Count > 0)
                {
                    var TotalSuccess = model.Count - ListItem.Count;
                    res.Code = "10";
                    res.Message = "Mở lại yêu cầu thành công " + TotalSuccess + "/" + model.Count + ", vui lòng nhấn vào Xem chi tiết để kiểm tra.";
                    res.Data = JsonConvert.SerializeObject(ListItem);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Mở lại yêu cầu thành công.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.JReOpenStatus: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cập nhật trạng thái đã xem ở thông báo yêu cầu hỗ trợ
        public JsonResult JUpdateClickNotify(ZRequestSupportUpdateClickNotifyRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = RequestSupportRepository.UpdateClickNotify(new ZRequestSupportUpdateClickNotifyRequest() 
                {
                    RequestSupportID = req.RequestSupportID,
                    ShopID = req.ShopID,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.JUpdateClickNotify: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy kết quả xử lý
        public JsonResult JAddResultRequestSupport(List<SearchListRequestSupportViewModel> model)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ShowListErrorMultipleSelectModel> ListItem = new List<ShowListErrorMultipleSelectModel>();
                foreach (var item in model)
                {
                    res = RequestSupportRepository.AddResultRequestSupport(new ZRequestSupportAddProcessResultRequest() 
                    {
                        RequestSupportCode = item.RequestSupportCode,
                        ShopID = item.ShopID,
                        UpdatedBy = XHelper.Session.UserProfile.ID
                    });
                    if (res.Code != "00")
                    {
                        ListItem.Add(new ShowListErrorMultipleSelectModel()
                        {
                            RequestSupportCode = item.RequestSupportCode,
                            Description = res.Message
                        });
                    }
                }
                if (ListItem.Count > 0)
                {
                    var TotalSuccess = model.Count - ListItem.Count;
                    res.Code = "10";
                    res.Message = "Lấy kết quả yêu cầu thành công " + TotalSuccess + "/" + model.Count + ", vui lòng nhấn vào Xem chi tiết để kiểm tra.";
                    res.Data = JsonConvert.SerializeObject(ListItem);
                }
                else
                {
                    res.Code = "00";
                    res.Message = "Lấy kết quả yêu cầu thành công";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.AddResultRequestSupport: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}