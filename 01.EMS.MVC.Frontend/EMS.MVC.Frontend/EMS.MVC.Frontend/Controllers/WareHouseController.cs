using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Repositories;
namespace EMS.MVC.Frontend.Controllers
{
    public class WareHouseController : BaseController
    {
        #region Danh sách kho
        [AuthorizeController(FunctionalTitle = "DANH_SACH_KHO")]
        public ActionResult WareHouseView()
        {
            return View();
        }

        public ActionResult SearchListWareHouseView(WareHouseViewModel req)
        {
            List<SearchListWareHouseViewModel> lstWareHouse = new List<SearchListWareHouseViewModel>();
            var res = WareHouseRepository.SearchListWareHouse(new ZWareHouseSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID, SearchKey = req.SearchKey });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZWareHouseSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    lstWareHouse.Add(new SearchListWareHouseViewModel()
                    {
                        ID = item.ID,
                        WareHouseCode = item.WareHouseCode,
                        WareHouseName = item.WareHouseName,
                        FullName = item.FullName,
                        MobileNumber = item.MobileNumber,
                        ProvinceName = item.ProvinceName,
                        DistrictName = item.DistrictName,
                        WardName = item.WardName,
                        Address = item.Address,
                        EmployeeName = item.EmployeeName,
                        IsDefault = item.IsDefault,
                        FullAddress = item.FullAddress,
                        IsLock = item.IsLock
                    });
                }
            }
            return PartialView(lstWareHouse);
        }
        #endregion

        #region Thêm kho
        [AuthorizeController(FunctionalTitle = "THEM_KHO")]
        public ActionResult AddWareHouseView()
        {
            AddWareHouseViewModel model = new AddWareHouseViewModel() 
            {
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                //ListDistrict = new List<DistrictModel>(),
                ListWard = new List<WardModel>()
            };
            return PartialView(model);
        }

        public ActionResult JAddWareHouse(AddWareHouseViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = WareHouseRepository.AddWareHouse(new ZWareHouseAddNewRequest()
                {
                    WareHouseCode = req.WareHouseCode,
                    WareHouseName = req.WareHouseName,
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Address = req.Street,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    IsDefault = req.IsDefault
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.JAddWareHouse: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa kho
        [AuthorizeController(FunctionalTitle = "SUA_KHO")]
        public ActionResult EditWareHouseView(SearchListWareHouseViewModel req)
        {
            EditWareHouseViewModel model = null;
            var res = WareHouseRepository.DetailWareHouseByWareHouseID(new ZWareHouseGetByIDRequest()
            {
                ID = req.ID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZWareHouseGetByIDResponse>(res.Data);
                model = new EditWareHouseViewModel()
                {
                    WareHouseID = item.WareHouseID,
                    WareHouseCode = item.WareHouseCode,
                    WareHouseName = item.WareHouseName,
                    FullName = item.FullName,
                    MobileNumber = item.MobileNumber,
                    ProvinceID = item.ProvinceID,
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    //DistrictID = item.DistrictID,
                    //ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = item.ProvinceID }),
                    WardID = item.WardID,
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = item.ProvinceID }),
                    Street = item.Street,
                    IsDefault = item.IsDefault,
                    IsLock = item.IsLock
                };
            }
            return PartialView(model);
        }

        public JsonResult JEditWareHouse(EditWareHouseViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = WareHouseRepository.EditWareHouse(new ZWareHouseEditRequest()
                {
                    WareHouseID = req.WareHouseID,
                    WareHouseName = req.WareHouseName,
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    ProvinceID = req.ProvinceID,
                    //DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    IsDefault = req.IsDefault
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.JEditWareHouse: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mở/ Khóa kho
        [AuthorizeController(FunctionalTitle = "MO_HOAC_KHOA_KHO")]
        public JsonResult JChangeIsLock(SearchListWareHouseViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = WareHouseRepository.ChangeIsLock(new ZWareHouseChangeIsLockRequest()
                {
                    ID = req.ID,
                    IsLock = req.IsLock,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseController.JChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tồn kho
        [AuthorizeController(FunctionalTitle = "DANH_SACH_TON_KHO")]
        public ActionResult GetProductsByID()
        {
            List<SearchListWareHouseViewModel> model = new List<SearchListWareHouseViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "WAREHOUSE_LIST",
                Data = JsonConvert.SerializeObject(new ZWareHouseSearchListRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZWareHouseSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new SearchListWareHouseViewModel()
                    {
                        ID = item.ID,
                        WareHouseName = item.WareHouseName,
                        ProvinceName = item.ProvinceName,
                        DistrictName = item.DistrictName,
                        WardName = item.WardName,
                        Address = item.Address,
                        FullAddress = item.FullAddress,
                        IsDefault = item.IsDefault
                    });
                }
            }
            return View(model);
        }
        public ActionResult DataProductsByID(WarehouseGetProductsByIDModel data)
        {
            List<WarehouseDataProductByIDViewModel> model = new List<WarehouseDataProductByIDViewModel>();
            if (string.IsNullOrEmpty(data.FromDate))
            {
                data.FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            }
            if (string.IsNullOrEmpty(data.ToDate))
            {
                data.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "WAREHOUSE_GET_PRODUCTS_BY_ID",
                Data = JsonConvert.SerializeObject(new ZWarehouseGetProductsByIDRequest()
                {
                    WarehouseID = data.WarehouseID,
                    FromDate = data.FromDate,
                    ToDate = data.ToDate,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    SearchCode = data.SearchCode
                })
            });
            long TotalQuantity = 0;
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZWarehouseGetProductsByIDResponse>>(res.Data);
                int stt = 0;
                foreach (var item in list)
                {
                    stt++;
                    model.Add(new WarehouseDataProductByIDViewModel()
                    {
                        No = stt,
                        WarehouseProductID = item.WarehouseProductID,
                        FirstQuantity = item.FirstQuantity,
                        QuantityIn = item.QuantityIn,
                        QuantityOut = item.QuantityOut,
                        QuantityAdjust = item.QuantityAdjust,
                        FinalQuantity = item.FinalQuantity,
                        ProductID = item.ProductID,
                        ProductName = item.ProductName,
                        ProductCode = item.ProductCode,
                        VariantID = item.VariantID,
                        VariantName = item.VariantName,
                        VariantImage = item.VariantImage,
                        SKU = item.SKU,
                        Barcode = item.Barcode,
                        TotalInventory = item.TotalInventory,
                        PalletCode = item.PalletCode,
                        ListWareHouse = item.ListWareHouse
                    });
                    TotalQuantity += data.WarehouseID == 0 ? item.TotalInventory : item.FinalQuantity;
                }
            }
            else
            {
                ViewBag.Message = res.Message;
            }
            ViewBag.WareHouseID = data.WarehouseID;
            ViewBag.TotalQuantity = TotalQuantity;
            return PartialView(model);
        }
        public ActionResult ExportExcelDataProductsByID(WarehouseGetProductsByIDModel data)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            if (string.IsNullOrEmpty(data.FromDate))
            {
                data.FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy");
            }
            if (string.IsNullOrEmpty(data.FromDate))
            {
                data.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "WAREHOUSE_GET_PRODUCTS_BY_ID",
                Data = JsonConvert.SerializeObject(new ZWarehouseGetProductsByIDRequest()
                {
                    WarehouseID = data.WarehouseID,
                    FromDate = data.FromDate,
                    ToDate = data.ToDate,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    SearchCode = data.SearchCode
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZWarehouseGetProductsByIDResponse>>(res.Data);
                int stt = 1;
                ISheet sheet = wb.CreateSheet();
                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0);
                CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 2);
                sheet.AddMergedRegion(cellMerge);
                row0.GetCell(0).SetCellValue("Danh sách sản phẩm tồn kho");
                var row1 = sheet.CreateRow(1);
                row1.CreateCell(0).SetCellValue("STT");
                row1.CreateCell(1).SetCellValue("Mã sản phẩm");
                row1.CreateCell(2).SetCellValue("Sản phẩm");
                row1.CreateCell(3).SetCellValue("Biến thể");
                row1.CreateCell(4).SetCellValue("SKU");
                row1.CreateCell(5).SetCellValue("Tên kho");
                row1.CreateCell(6).SetCellValue("Tồn đầu kỳ");
                row1.CreateCell(7).SetCellValue("Nhập trong kỳ");
                row1.CreateCell(8).SetCellValue("Xuất trong kỳ");
                row1.CreateCell(9).SetCellValue("Chỉnh sửa trong kỳ");
                row1.CreateCell(10).SetCellValue("Tồn cuối kỳ");
                int rowIndex = 2;
                foreach (var item in list)
                {
                    var newRow = sheet.CreateRow(rowIndex);
                    newRow.CreateCell(0).SetCellValue(stt++);
                    newRow.CreateCell(1).SetCellValue(item.ProductCode);
                    newRow.CreateCell(2).SetCellValue(item.ProductName);
                    newRow.CreateCell(3).SetCellValue(item.VariantName);
                    newRow.CreateCell(4).SetCellValue(item.SKU);
                    newRow.CreateCell(5).SetCellValue(item.ListWareHouse[0].WarehouseName);
                    newRow.CreateCell(6).SetCellValue(item.ListWareHouse[0].FirstQuantity);
                    newRow.CreateCell(7).SetCellValue(item.ListWareHouse[0].QuantityIn);
                    newRow.CreateCell(8).SetCellValue(item.ListWareHouse[0].QuantityOut);
                    newRow.CreateCell(9).SetCellValue(item.ListWareHouse[0].QuantityAdjust);
                    newRow.CreateCell(10).SetCellValue(item.ListWareHouse[0].FinalQuantity);
                    rowIndex++;
                    if (item.ListWareHouse.Count > 1)
                    {
                        for (var i = 1; i < item.ListWareHouse.Count; i++)
                        {
                            var newRow_02 = sheet.CreateRow(rowIndex);
                            newRow_02.CreateCell(0).SetCellValue("");
                            newRow_02.CreateCell(1).SetCellValue(item.ProductCode);
                            newRow_02.CreateCell(2).SetCellValue(item.ProductName);
                            newRow_02.CreateCell(3).SetCellValue(item.VariantName);
                            newRow_02.CreateCell(4).SetCellValue(item.SKU);
                            newRow_02.CreateCell(5).SetCellValue(item.ListWareHouse[i].WarehouseName);
                            newRow_02.CreateCell(6).SetCellValue(item.ListWareHouse[i].FirstQuantity);
                            newRow_02.CreateCell(7).SetCellValue(item.ListWareHouse[i].QuantityIn);
                            newRow_02.CreateCell(8).SetCellValue(item.ListWareHouse[i].QuantityOut);
                            newRow_02.CreateCell(9).SetCellValue(item.ListWareHouse[i].QuantityAdjust);
                            newRow_02.CreateCell(10).SetCellValue(item.ListWareHouse[i].FinalQuantity);
                            rowIndex++;
                        }
                    }
                }
                sheet.AutoSizeColumn(0, true);
                sheet.AutoSizeColumn(1, true);
                sheet.AutoSizeColumn(2, true);
                sheet.AutoSizeColumn(3, true);
                sheet.AutoSizeColumn(4, true);
                sheet.AutoSizeColumn(5, true);
                sheet.AutoSizeColumn(6, true);
                sheet.AutoSizeColumn(7, true);
                sheet.AutoSizeColumn(8, true);
                sheet.AutoSizeColumn(9, true);
                sheet.AutoSizeColumn(10, true);
            }
            MemoryStream output = new MemoryStream();
            wb.Write(output);
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "DanhSachSanPhamTonKho.xls");
        }
        #endregion
    }
}
