using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Newtonsoft.Json;
using EMS.MVC.Frontend.Models;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Repositories;

namespace EMS.MVC.Frontend.Controllers
{
    public class CustomerController : BaseController
    {
        #region Danh sách khách hàng
        [AuthorizeController(FunctionalTitle = "DANH_SACH_KHACH_HANG")]
        public ActionResult CustomerView(CustomerViewModel req)
        {
            if (TempData.ContainsKey("GroupName"))
            {
                req.CustomerGroup = TempData["GroupName"].ToString();
            }
            CustomerViewModel model = new CustomerViewModel()
            {
                FromDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                CustomerGroup = req.CustomerGroup,
                ListCustomerGroup = XCommon.SearchListCustomerGroup(new ZCustomerSearchGroupRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListCustomerSticker = XCommon.SearchListCustomerSticker(new ZCustomerStickerListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
            };
            return View(model);
        }

        public ActionResult SearchListCustomerView(CustomerViewModel req)
        {
            List<SearchListCustomerViewModel> model = new List<SearchListCustomerViewModel>();
            var res = CustomerRepository.SearchList(new ZCustomerSearchListRequest()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                SearchKey = req.SearchKey,
                ProvinceID = req.ProvinceID,
                CustomerGroupName = req.CustomerGroup,
                CustomerStickerID = req.CustomerSticker,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var ListItem = JsonConvert.DeserializeObject<List<ZCustomerSearchListResponse>>(res.Data);
                foreach (var item in ListItem)
                {
                    model.Add(new SearchListCustomerViewModel()
                    {
                        ID = item.ID,
                        Code = item.CustomerCode,
                        FullName = item.FullName,
                        MobileNumber = item.MobileNumber,
                        Email = item.Email,
                        BirthDay = item.BirthDay,
                        TaxCode = item.TaxCode,
                        Channel = item.Channel,
                        Note = item.Note,
                        CustomerGroup = new CustomerGroupModel() { Name = item.GroupName },
                        CustomerSticker = new CustomerStickerModel() { Name = item.Sticker },
                        BlackList = item.BlackList,
                        Province = new ProvinceModel() { Name = item.ProvinceName },
                        District = new DistrictModel() { Name = item.DistrictName },
                        Ward = new WardModel() { Name = item.WardName },
                        Street = item.Address,
                        FullAddress = item.FullAddress,
                        SuccessOrder = item.CountOrderSuccess.ToString("N0"),
                        TotalAmount = item.TotalAmountSuccess.ToString("N0")
                    });
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Thêm khách hàng
        [AuthorizeController(FunctionalTitle = "THEM_KHACH_HANG")]
        public ActionResult AddCustomerView(AddCustomerViewModel req)
        {
            AddCustomerViewModel model = new AddCustomerViewModel() 
            {
                ListChannel = XData.GetListChannel(),
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                ListWard = new List<WardModel>(),
                ListCustomerSticker = XCommon.SearchListCustomerSticker(new ZCustomerStickerListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                IsReload = req.IsReload
            };
            return PartialView(model);
        }

        public JsonResult JAddCustomer(AddCustomerViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CustomerRepository.AddCustomer(new ZCustomerAddNewRequest()
                {
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    Email = req.Email,
                    ProvinceID = req.ProvinceID,
                    WardID = req.WardID,
                    Address = req.Street,
                    BirthDay = req.BirthDay,
                    TaxCode = req.TaxCode,
                    Channel = req.Channel,
                    Note = req.Note,
                    strStickerID = req.CustomerStickerID,
                    strStickerNew = req.CustomerStickerNew,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.JAddCustomer: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm khách hàng bằng file Excel
        [AuthorizeController(FunctionalTitle = "THEM_KHACH_HANG_BANG_EXCEL")]
        public ActionResult ImportExcelView()
        {
            ImportExcelViewModel model = new ImportExcelViewModel();
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

            var ListChannel = XData.GetListChannel();
            foreach (var item in ListChannel)
            {
                model.ListChannel.Add(new JSpreadSheetModel()
                {
                    id = item.Value,
                    name = item.Text
                });
            }

            var ListCustomerSticker = XCommon.SearchListCustomerSticker(new ZCustomerStickerListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            foreach (var item in ListCustomerSticker)
            {
                model.ListCustomerSticker.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.Name
                });
            }

            return View(model);
        }

        //public JsonResult TestRemoteSearch(string q)
        //{
        //    var ListArray = new List<JSpreadSheetModel>();
        //    ListArray.Add(new JSpreadSheetModel() { id = "1", name = "100 nguyễn văn cừ" });
        //    ListArray.Add(new JSpreadSheetModel() { id = "2", name = "200 mậu thân" });
        //    ListArray.Add(new JSpreadSheetModel() { id = "3", name = "300 nguyễn văn linh" });
        //    ListArray.Add(new JSpreadSheetModel() { id = "4", name = "400 nguyễn trãi" });
        //    ListArray.Add(new JSpreadSheetModel() { id = "5", name = "500 hai bà trưng" });

        //    var checkListArray = ListArray.Where(m => m.name == q).ToList();

        //    if (checkListArray.Count > 0)
        //    {
        //        ListArray = new List<JSpreadSheetModel>();
        //        ListArray = checkListArray;
        //    }

        //    return Json(ListArray, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult JImportExcelData()
        {
            ZResponse res = new ZResponse();
            try
            {
                HttpPostedFileBase files = Request.Files[0]; //Read the Posted Excel File  
                //var filesize = files.ContentLength;
                ISheet sheet; //Create the ISheet object to read the sheet cell values  
                string filename = files.FileName; //get the uploaded file name  
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
                List<CustomerStickerModel> ListCustomerSticker = new List<CustomerStickerModel>();
                for (int row = 1; row <= total; row++)
                {
                    var ProvinceID = 0;
                    var DistrictID = 0;
                    var WardID = 0;
                    if (sheet.GetRow(row) != null)
                    {
                        var FullName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(1));
                        var Email = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(2));
                        var MobileNumber = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(3));
                        var Street = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(4));
                        var ProvinceName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(5));
                        var DistrictName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(6));
                        var WardName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(7));
                        var BirthDay = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(8));
                        var TaxCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(9));
                        var Channel = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(10));
                        var Note = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(11));
                        var CustomerSticker = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(12));

                        if (string.IsNullOrEmpty(FullName) && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(MobileNumber) &&
                            string.IsNullOrEmpty(Street) && string.IsNullOrEmpty(ProvinceName) && string.IsNullOrEmpty(DistrictName) &&
                            string.IsNullOrEmpty(WardName) && string.IsNullOrEmpty(BirthDay) && string.IsNullOrEmpty(TaxCode) &&
                            string.IsNullOrEmpty(Channel) && string.IsNullOrEmpty(Note) && string.IsNullOrEmpty(CustomerSticker))
                        {
                            continue;
                        }

                        Task AddressHandle = new Task(() => {
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
                                if (!string.IsNullOrEmpty(Street))
                                {
                                    var resSearchPWDByMapsSearch = XCommon.SearchPWDByVNPMapsSearch(Street);
                                    if (resSearchPWDByMapsSearch.Code == "00")
                                    {
                                        var dataVnpMapsSearch = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeResponse>(resSearchPWDByMapsSearch.Data);
                                        ProvinceName = dataVnpMapsSearch.ProvinceName;
                                        DistrictName = dataVnpMapsSearch.DistrictName;
                                        WardName = dataVnpMapsSearch.WardName;
                                        ProvinceID = dataVnpMapsSearch.ProvinceID;
                                        DistrictID = dataVnpMapsSearch.DistrictID;
                                        WardID = dataVnpMapsSearch.WardID;
                                        Street = "(Vmapcode) " + Street;
                                    }
                                }
                            }
                        });

                        AddressHandle.Start();

                        if (!string.IsNullOrEmpty(BirthDay))
                        {
                            var CheckRegex = XCommon.CheckRegex(BirthDay, "Date");
                            if (CheckRegex == true)
                            {
                                BirthDay = DateTime.ParseExact(BirthDay, "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                BirthDay = "";
                            }
                        }

                        if (string.IsNullOrEmpty(Channel))
                        {
                            //Mặc định là trực tiếp
                            Channel = "TT";
                        }
                        else
                        {
                            Channel = XData.GetValueChannel(Channel);
                        }

                        if (!string.IsNullOrEmpty(CustomerSticker))
                        {
                            if(ListCustomerSticker.Count == 0)
                            {
                                ListCustomerSticker = XCommon.SearchListCustomerSticker(new ZCustomerStickerListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                            }

                            var CustomerStickerID = "";
                            var splitCustomerSticker = CustomerSticker.Split(',');
                            foreach(var itemCustomerSticker in splitCustomerSticker)
                            {
                                var checkCustomerSticker = ListCustomerSticker.Where(m => m.Name.ToUpper() == itemCustomerSticker.ToUpper().Trim()).ToList();
                                if (checkCustomerSticker.Count > 0)
                                {
                                    var checkCustomerStickerID = checkCustomerSticker.Select(m => m.ID).First();
                                    if (!CustomerStickerID.Contains(checkCustomerStickerID.ToString()))
                                    {
                                        CustomerStickerID += checkCustomerSticker.Select(m => m.ID).First() + ",";
                                    }
                                }
                            }
                            CustomerSticker = CustomerStickerID.TrimEnd(',');
                        }

                        Task.WaitAll(AddressHandle);

                        ArrayList data = new ArrayList();
                        data.Add("");
                        data.Add(FullName);
                        data.Add(Email);
                        data.Add(MobileNumber);
                        data.Add(Street);
                        data.Add(ProvinceName);
                        data.Add(DistrictName);
                        data.Add(WardName);
                        data.Add(BirthDay);
                        data.Add(TaxCode);
                        data.Add(Channel);
                        data.Add(Note);
                        data.Add(CustomerSticker);
                        data.Add(ProvinceID);
                        data.Add(DistrictID);
                        data.Add(WardID);
                        model.Add(data);
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.JImportExcel: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JImportExcel(List<ImportExcelCustomerModel> data, int countKHL, int countLoi)
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
                List<ArrayList> lstError = new List<ArrayList>();

                foreach(var itemExcel in data)
                {
                    rowAction++;

                    if (!string.IsNullOrEmpty(itemExcel.BirthDay))
                    {
                        itemExcel.BirthDay = DateTime.ParseExact(itemExcel.BirthDay, "yyyy-MM-dd", null).ToString("dd/MM/yyyy");
                    }

                    var resAddCustomer = CustomerRepository.ImportExcel(new ZCustomerImportExcelRequest()
                    {
                        FullName = itemExcel.FullName,
                        MobileNumber = itemExcel.MobileNumber,
                        Email = itemExcel.Email,
                        ProvinceID = itemExcel.ProvinceID,
                        DistrictID = itemExcel.DistrictID,
                        WardID = itemExcel.WardID,
                        Street = itemExcel.Street,
                        BirthDay = itemExcel.BirthDay,
                        TaxCode = itemExcel.TaxCode,
                        Channel = itemExcel.Channel,
                        Note = itemExcel.Note,
                        CustomerStickerID = itemExcel.CustomerStickerID,
                        IsUpdate = itemExcel.IsUpdate,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        CreatedBy = XHelper.Session.EmployeeProfile.ID
                    });

                    if(resAddCustomer.Code == "00")
                    {
                        count++;
                    }
                    else
                    {
                        if(resAddCustomer.Code == "01")
                        {
                            lstError.Add(JImportExcelError(itemExcel, "Số điện thoại - trùng với khách hàng đã tồn tại."));
                            countError++;
                        }
                        else
                        {
                            lstError.Add(JImportExcelError(itemExcel, "Khách hàng - " + resAddCustomer.Message));
                            countError++;
                        }
                    }
                }

                if (total == count)
                {
                    res.Code = "00";
                    res.Message = string.Format("<div class='text-green mb-2'>Khách hàng tạo thành công: {0}</div><div class='text-warning mb-2'>Khách hàng không hợp lệ: {1}</div><div class='text-danger mb-3'>Khách hàng lỗi: {2}</div>", count, countKHL, countError);
                }
                else
                {
                    res.Code = "10";
                    res.Message = string.Format("<div class='text-green mb-2'>Khách hàng tạo thành công: {0}</div><div class='text-warning mb-2'>Khách hàng không hợp lệ: {1}</div><div class='text-danger mb-3'>Khách hàng lỗi: {2}</div>", count, countKHL, countError);
                    res.Data = JsonConvert.SerializeObject(lstError);
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.JImportExcel: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ArrayList JImportExcelError(ImportExcelCustomerModel data, string Message)
        {
            ArrayList model = new ArrayList();
            model.Add("Lỗi");
            model.Add(data.FullName);
            model.Add(data.Email);
            model.Add(data.MobileNumber);
            model.Add(data.Street);
            model.Add(data.ProvinceID);
            model.Add(data.DistrictID);
            model.Add(data.WardID);
            
            if (string.IsNullOrEmpty(data.BirthDay))
            {
                model.Add("");
            }
            else
            {
                model.Add(DateTime.ParseExact(data.BirthDay, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"));
            }

            model.Add(data.TaxCode);
            model.Add(data.Channel);
            model.Add(data.Note);
            model.Add(data.CustomerStickerID);
            model.Add(data.ProvinceName);
            model.Add(data.DistrictName);
            model.Add(data.WardName);
            model.Add(Message);
            return model;
        }

        public ActionResult JExportExcel(List<ImportExcelCustomerModel> ListData)
        {
            var FullPath = System.Web.HttpContext.Current.Server.MapPath("~/Assets/temp/MauImportKhachHang.xls");
            var fileNameNew = Guid.NewGuid().ToString() + ".xls";
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
                    HSSFCellStyle borderedCellStyle = (HSSFCellStyle)xls.CreateCellStyle();
                    borderedCellStyle.BorderLeft = BorderStyle.Thin;
                    borderedCellStyle.BorderTop = BorderStyle.Thin;
                    borderedCellStyle.BorderRight = BorderStyle.Thin;
                    borderedCellStyle.BorderBottom = BorderStyle.Thin;

                    sheet.GetRow(0).CreateCell(13).SetCellValue("Nguyên nhân lỗi");
                    sheet.GetRow(0).GetCell(13).CellStyle = borderedCellStyle;

                    foreach (var item in ListData)
                    {
                        // tao row mới
                        var newRow = sheet.CreateRow(rowIndex);
                        // set giá trị
                        newRow.CreateCell(0).SetCellValue(stt++);
                        newRow.CreateCell(1).SetCellValue(item.FullName);
                        newRow.CreateCell(2).SetCellValue(item.Email);
                        newRow.CreateCell(3).SetCellValue(item.MobileNumber);
                        newRow.CreateCell(4).SetCellValue(item.Street);
                        newRow.CreateCell(5).SetCellValue(item.ProvinceName);
                        newRow.CreateCell(6).SetCellValue(item.DistrictName);
                        newRow.CreateCell(7).SetCellValue(item.WardName);

                        if (string.IsNullOrEmpty(item.BirthDay))
                        {
                            newRow.CreateCell(8).SetCellValue("");
                        }
                        else
                        {
                            newRow.CreateCell(8).SetCellValue(DateTime.ParseExact(item.BirthDay, "yyyy-MM-dd", null).ToString("dd/MM/yyyy"));
                        }

                        newRow.CreateCell(9).SetCellValue(item.TaxCode);
                        newRow.CreateCell(10).SetCellValue(item.Channel);
                        newRow.CreateCell(11).SetCellValue(item.Note);
                        newRow.CreateCell(12).SetCellValue(item.CustomerStickerName);
                        newRow.CreateCell(13).SetCellValue(item.MessageError);

                        for (int i = 0; i <= 13; i++)
                        {
                            newRow.GetCell(i).CellStyle = borderedCellStyle;
                        }

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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.JExportExcel: " + ex.Message);
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
             "MauImportKhachHang");
        }
        #endregion

        #region Chi tiết khách hàng
        [AuthorizeController(FunctionalTitle = "CHI_TIET_KHACH_HANG")]
        public ActionResult DetailCustomerView(SearchListCustomerViewModel req)
        {
            DetailCustomerViewModel model = null;
            var CustomerID = Convert.ToInt32(Security.DecodeBase64(req.SearchKey));
            var res = CustomerRepository.SearchCustomerByID(new ZCustomerGetByIDRequest() { ID = CustomerID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZCustomerGetByIDResponse>(res.Data);
                model = new DetailCustomerViewModel()
                {
                    ID = item.ID,
                    CustomerCode = item.CustomerCode,
                    FullName = item.FullName,
                    MobileNumber = item.MobileNumber,
                    Email = item.Email,
                    Province = new ProvinceModel() { ID = item.ProvinceID, Name = item.ProvinceName },
                    //ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    District = new DistrictModel() { ID = item.DistrictID, Name = item.DistrictName },
                    //ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictListRequest() { ProvinceID = item.ProvinceID }),
                    Ward = new WardModel() { ID = item.WardID, Name = item.WardName },
                    //ListWard = XCommon.SearchListWardByDistrictID(new ZWardListRequest() { DistrictID = item.DistrictID }),
                    Street = item.Address,
                    FullAddress = item.FullAddress,
                    BirthDay = item.BirthDay,
                    TaxCode = item.TaxCode,
                    Channel = XData.GetNameChannel(item.Channel),
                    //ListChannel = XData.GetListChannel(),
                    Note = item.Note,
                    CustomerSticker = item.Sticker,
                    //ListCustomerSticker = XCommon.SearchListCustomerSticker(new ZCustomerStickerListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                    GroupName = item.GroupName,
                };
                TempData["Customer_MobileNumber"] = model.MobileNumber;
                TempData["Customer_FullName"] = model.FullName;
            }
            return View(model);
        }

        [AuthorizeController(FunctionalTitle = "LICH_SU_DAT_HANG")]
        public ActionResult OrderHistoryCustomerView(DetailCustomerViewModel req)
        {
            List<OrderHistoryViewModel> oListItem = new List<OrderHistoryViewModel>();
            var res = OrderRepository.SearchOrderSuccessHistory(new ZOrderSuccessHistoryRequest()
            {
                MobileNumber = req.MobileNumber,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var ListItem = JsonConvert.DeserializeObject<List<ZOrderSuccessHistoryResponse>>(res.Data);
                foreach (var item in ListItem)
                {
                    oListItem.Add(new OrderHistoryViewModel()
                    {
                        ID = item.OrderID,
                        OrderCode = item.OrderCode,
                        CreatedDate = item.CreatedDate,
                        TotalProductValue = item.TotalProductValue
                    });
                }
            }
            return PartialView(oListItem);
        }
        #endregion

        #region Sửa khách hàng
        [AuthorizeController(FunctionalTitle = "SUA_KHACH_HANG")]
        public ActionResult EditCustomerView(DetailCustomerViewModel req)
        {
            EditCustomerViewModel model = null;
            var CustomerID = Convert.ToInt32(Security.DecodeBase64(req.SearchKey));
            var res = CustomerRepository.SearchCustomerByID(new ZCustomerGetByIDRequest() { ID = CustomerID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZCustomerGetByIDResponse>(res.Data);
                model = new EditCustomerViewModel()
                {
                    ID = item.ID,
                    FullName = item.FullName,
                    MobileNumber = item.MobileNumber,
                    Email = item.Email,
                    ProvinceID = item.ProvinceID,
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    DistrictID = item.DistrictID,
                    ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = item.ProvinceID }),
                    WardID = item.WardID,
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = item.DistrictID }),
                    Street = item.Address,
                    BirthDay = item.BirthDay,
                    TaxCode = item.TaxCode,
                    Channel = item.Channel,
                    ListChannel = XData.GetListChannel(),
                    Note = item.Note,
                    CustomerStickerID = item.Sticker,
                    ListCustomerSticker = XCommon.SearchListCustomerSticker(new ZCustomerStickerListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                };
            }
            return View(model);
        }

        public JsonResult JEditCustomer(EditCustomerViewModel model)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CustomerRepository.EditCustomer(new ZCustomerEditRequest()
                {
                    ID = model.ID,
                    FullName = model.FullName,
                    MobileNumber = model.MobileNumber,
                    Email = model.Email,
                    ProvinceID = model.ProvinceID,
                    DistrictID = model.DistrictID,
                    WardID = model.WardID,
                    Address = model.Street,
                    BirthDay = model.BirthDay,
                    TaxCode = model.TaxCode,
                    Note = model.Note,
                    Channel = model.Channel,
                    //strStickerID = model.strStickerID,
                    //strStickerNew = model.strStickerNew,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.JEditCustomer: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm hoặc xóa trong danh sách đen
        [AuthorizeController(FunctionalTitle = "THEM_KHACH_HANG_VAO_DANH_SACH_DEN")]
        public JsonResult JBlackList(SearchListCustomerViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CustomerRepository.BlackList(new ZCustomerCheckBlackListRequest()
                {
                    ID = req.ID,
                    IsChecked = req.BlackList,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.JBlackList: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa nhãn khách hàng
        [AuthorizeController(FunctionalTitle = "XOA_NHAN_CUA_KHACH_HANG")]
        public JsonResult JRemoveCustomerSticker()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.JRemoveCustomerSticker: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}