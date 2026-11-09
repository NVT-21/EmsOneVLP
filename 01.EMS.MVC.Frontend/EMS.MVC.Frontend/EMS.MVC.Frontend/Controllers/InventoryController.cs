using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using EMS.MVC.Frontend.Models;
using EMS.Entity;
using EMS.MVC.Frontend.Repositories;
namespace EMS.MVC.Frontend.Controllers
{
    public class InventoryController : BaseController
    {
        [AuthorizeController(FunctionalTitle = "DANH_SACH_TON_KHO")]
        public ActionResult InventoryView()
        {
            InventoryViewModel model = new InventoryViewModel() 
            {
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
            };
            return View(model);
        }

        public ActionResult SearchListInventoryView(InventoryViewModel req)
        {
            List<SearchListInventoryViewModel> model = new List<SearchListInventoryViewModel>();
            var res = WareHouseRepository.DetailProductByWareHouseID(new ZWarehouseGetProductsByIDRequest() 
            {
                WarehouseID = req.WareHouseID,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                SearchCode = req.SearchKey
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZWarehouseGetProductsByIDResponse>>(res.Data);
                int stt = 0;
                foreach (var item in list)
                {
                    stt++;
                    model.Add(new SearchListInventoryViewModel()
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
                }
            }
            ViewBag.WareHouseID = req.WareHouseID;
            ViewBag.WareHouseName = req.WareHouseName;
            return PartialView(model);
        }

        public ActionResult ExportExcelDataProductsByID(InventoryViewModel req)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            var res = WareHouseRepository.DetailProductByWareHouseID(new ZWarehouseGetProductsByIDRequest()
            {
                WarehouseID = req.WareHouseID,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                SearchCode = req.SearchKey
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
    }
}