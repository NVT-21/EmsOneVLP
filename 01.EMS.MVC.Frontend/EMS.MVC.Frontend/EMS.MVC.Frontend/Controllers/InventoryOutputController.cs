using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Repositories;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using NPOI.SS.Util;

namespace EMS.MVC.Frontend.Controllers
{
    public class InventoryOutputController : BaseController
    {
        #region Danh sách phiếu xuất kho
        [AuthorizeController(FunctionalTitle = "DANH_SACH_PHIEU_XUAT_KHO")]
        public ActionResult InventoryOutputView()
        {
            InventoryOutputViewModel model = new InventoryOutputViewModel()
            {
                FromDate = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy")
            };
            return View(model);
        }

        public ActionResult SearchListInventoryOutputView(InventoryOutputViewModel data)
        {
            List<SearchListInventoryOutputViewModel> ListItem = new List<SearchListInventoryOutputViewModel>();

            var res = InventoryOutputRepository.SearchListInventoryOutput(new ZInventoryOutputSearchListRequest()
            {
                FromDate = data.FromDate,
                ToDate = data.ToDate,
                SearchKey = data.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZInventoryOutputSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    ListItem.Add(new SearchListInventoryOutputViewModel()
                    {
                        ID = item.InventoryOutputID,
                        Code = item.InventoryOutputCode,
                        Order = new OrderModel()
                        {
                            OrderID = item.Order.OrderID,
                            EMSOneCode = item.Order.EMSOneCode,
                            OrderCode = item.Order.OrderCode,
                            BatchCode = item.Order.BatchCode,
                            IsTransport = item.Order.IsTransport,
                            ShippingCode = item.Order.ShippingCode,
                        },
                        Status = new BaseModel()
                        {
                            Code = item.Status.Code,
                            Name = item.Status.Name
                        },
                        ReceiverName = item.ReceiverName,
                        CreaterName = item.CreatedName,
                        CreatedDate = item.CreatedDate,
                        UpdaterName = item.UpdatedName,
                        UpdatedDate = item.UpdatedDate,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    });
                }
            }
            return PartialView(ListItem);
        }

        public ActionResult ExportListInventoryOutput(InventoryOutputViewModel req)
        {
            MemoryStream output = new MemoryStream();
            try
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

                //Create title 
                var TitleRow = sheet.CreateRow(0);
                TitleRow.CreateCell(0).SetCellValue("DANH SÁCH PHIẾU XUẤT KHO");
                TitleRow.GetCell(0).CellStyle = TitleStyle;

                //Create a header row
                var headerRow = sheet.CreateRow(1);
                int ColumnIndex = 0;

                headerRow.CreateCell(ColumnIndex++).SetCellValue("STT");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã phiếu xuất");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Mã đơn hàng");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Trạng thái");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Người tạo");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Thời gian tạo");
                headerRow.CreateCell(ColumnIndex++).SetCellValue("Thời gian xử lý");

                var res = InventoryRepository.SearchListInventory(new ZInventorySearchListRequest()
                {
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    SearchKey = req.SearchKey,
                    Type = "O",
                    DateType = "CREATED_DATE",
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                if (res.Code == "00")
                {
                    var listItem = JsonConvert.DeserializeObject<List<ZInventorySearchListResponse>>(res.Data);
                    int rowNumber = 2;
                    int rowIndex = 0;
                    var stt = 1;
                    foreach (var item in listItem)
                    {
                        //Create a new Row
                        var row = sheet.CreateRow(rowNumber++);

                        //Set the Values for Cells
                        row.CreateCell(rowIndex++).SetCellValue(stt.ToString());
                        row.CreateCell(rowIndex++).SetCellValue(item.Code);
                        row.CreateCell(rowIndex++).SetCellValue(item.Order.OrderCode);
                        row.CreateCell(rowIndex++).SetCellValue(item.Status.Name);
                        row.CreateCell(rowIndex++).SetCellValue(item.CreaterName);
                        row.CreateCell(rowIndex++).SetCellValue(item.CreatedDate);
                        row.CreateCell(rowIndex++).SetCellValue(item.UpdatedDate);

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
                }

                workbook.Write(output);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.ExportListInventoryOutput: " + ex.Message);
            }
            return File(output.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "DanhSachPhieuXuatKho.xls");
        }
        #endregion

        #region Thêm phiếu xuất kho
        public ActionResult AddInventoryOutputView()
        {
            AddInventoryOutputViewModel model = new AddInventoryOutputViewModel();
            model.ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            model.WareHouseID = model.ListWareHouse.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListWareHouse.Where(m => m.IsDefault == "Y").First().ID : 0;
            model.DateOfInventoryOutput = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            return View(model);
        }

        public ActionResult AddDetailInventoryOutputView(AddInventoryOutputViewModel req)
        {
            AddDetailInventoryOutputViewModel model = new AddDetailInventoryOutputViewModel();
            model.WareHouseID = req.WareHouseID;
            model.NumberOfDetail = req.NumberOfDetail;
            model.ListProductVariant = XCommon.SearchListProductVariantByWareHouseID(new ZProductVariantGetByWareHouseIDRequest() { WareHouseID = req.WareHouseID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            return PartialView(model);
        }

        public JsonResult JAddInventoryOutput(AddInventoryOutputViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZInventoryOutputDetailAddRequest> ListInventoryOutputDetail = new List<ZInventoryOutputDetailAddRequest>();
                foreach (var item in req.ListInventoryOutputDetail)
                {
                    ListInventoryOutputDetail.Add(new ZInventoryOutputDetailAddRequest()
                    {
                        ProductVariantID = item.ProductVariantID,
                        Quantity = item.Quantity,
                        PalletCode = item.PalletCode
                    });
                }
                res = InventoryOutputRepository.AddInventoryOutput(new ZInventoryOutputAddRequest()
                {
                    AmndState = "A",
                    WareHouseID = req.WareHouseID,
                    DateOfInventoryOutput = req.DateOfInventoryOutput,
                    ReceiverName = req.ReceiverName,
                    MobileNumber = req.MobileNumber,
                    Note = req.Note,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    ListInventoryOutputDetail = ListInventoryOutputDetail
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JAddInventoryOutput: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa phiếu xuất kho
        [AuthorizeController(FunctionalTitle = "SUA_THONG_TIN_PHIEU_XUAT_KHO")]
        public ActionResult EditInventoryOutputView(SearchListInventoryOutputViewModel req)//--- Chỉnh sửa thông tin
        {
            EditInventoryOutputViewModel model = null;
            var InventoryID = Security.DecodeBase64(req.SearchKey);
            var res = InventoryOutputRepository.SearchByInventoryID(new ZInventoryOutputGetByIDRequest()
            {
                ID = Convert.ToInt32(InventoryID),
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });

            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZInventoryOutputGetByIDResponse>(res.Data);
                List<InventoryDetailModel> ListInventoryDetail = new List<InventoryDetailModel>();
                foreach (var itemInventoryDetail in item.ListInventoryOutputDetail)
                {
                    ListInventoryDetail.Add(new InventoryDetailModel()
                    {
                        ID = itemInventoryDetail.ID,
                        Product = new ProductModel() 
                        {
                            ProductName = itemInventoryDetail.Product.ProductName,
                            ProductVariant = new ProductVariantModel() 
                            {
                                ID = itemInventoryDetail.Product.ProductVariant.ProductVariantID,
                                Options = itemInventoryDetail.Product.ProductVariant.ProductVariantOptions,
                                Pallet = new PalletModel()
                                {
                                    PalletCode = itemInventoryDetail.Product.ProductVariant.Pallet.PalletCode
                                },
                            },
                        },
                        Quantity = itemInventoryDetail.Quantity,
                        Price = itemInventoryDetail.Price,
                        TotalAmount = itemInventoryDetail.TotalAmount,
                        WareHouseQuantity = itemInventoryDetail.WarehouseQuantity,
                    });
                }

                model = new EditInventoryOutputViewModel()
                {
                    InventoryOutputID = item.InventoryOutputID,
                    InventoryOutputCode = item.InventoryOutputCode,
                    WareHouse = new WareHouseModel()
                    {
                        ID = item.WareHouse.WareHouseID,
                        WareHouseCode = item.WareHouse.WareHouseCode,
                        WareHouseName = item.WareHouse.WareHouseName,
                        FullAddress = item.WareHouse.FullAddress
                    },
                    ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                    DateOfInventoryOutput = item.DateOfInventoryOutput,
                    ReceiverName = item.ReceiverName,
                    MobileNumber = item.MobileNumber,
                    Note = item.Note,
                    NumberOfDetail = ListInventoryDetail.Count(),
                    ListInventoryOutputDetail = ListInventoryDetail
                };
            }
            return View(model);
        }

        public ActionResult EditDetailInventoryOutputView(EditInventoryOutputViewModel req)
        {
            EditDetailInventoryOutputViewModel model = new EditDetailInventoryOutputViewModel()
            {
                NumberOfDetail = req.NumberOfDetail,
                ListInventoryOutputDetail = req.ListInventoryOutputDetail == null ? new List<InventoryDetailModel>() : req.ListInventoryOutputDetail,
                ListProductVariant = XCommon.SearchListProductVariantByWareHouseID(new ZProductVariantGetByWareHouseIDRequest() { WareHouseID = req.WareHouseID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
            };
            return PartialView(model);
        }

        public JsonResult JEditInventoryOutput(EditInventoryOutputViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZInventoryOutputDetailEditRequest> ListInventoryOutputDetail = new List<ZInventoryOutputDetailEditRequest>();
                foreach (var item in req.ListInventoryOutputDetail)
                {
                    ListInventoryOutputDetail.Add(new ZInventoryOutputDetailEditRequest()
                    {
                        ProductVariantID = item.ProductVariantID,
                        Quantity = item.Quantity,
                        PalletCode = item.PalletCode
                    });
                }

                res = InventoryOutputRepository.EditInventoryOutput(new ZInventoryOutputEditRequest()
                {
                    InventoryID = req.InventoryOutputID,
                    DateOfInventoryOutput = req.DateOfInventoryOutput,
                    ReceiverName = req.ReceiverName,
                    MobileNumber = req.MobileNumber,
                    Note = req.Note,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    ListInventoryOutputDetail = ListInventoryOutputDetail
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JEditInventoryOutput: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chi tiết phiếu xuất kho
        //[AuthorizeController(FunctionalTitle = "CHI_TIET_PHIEU_XUAT_KHO")]
        //public ActionResult DetailInventoryOutputView(SearchListInventoryOutputViewModel req)
        //{
        //    DetailInventoryOutputViewModel model = null;
        //    var InventoryID = Convert.ToInt32(Security.DecodeBase64(req.SearchKey));
        //    var res = InventoryOutputRepository.SearchByInventoryID(new ZInventoryOutputGetByIDRequest()
        //    {
        //        ID = InventoryID,
        //        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //    });
        //    if (res.Code == "00")
        //    {
        //        var itemData = JsonConvert.DeserializeObject<ZInventoryOutputGetByIDResponse>(res.Data);
        //        List<InventoryDetailModel> ListInventoryOutputDetail = new List<InventoryDetailModel>();
        //        foreach (var item in itemData.ListInventoryOutputDetail)
        //        {
        //            ListInventoryOutputDetail.Add(new InventoryDetailModel()
        //            {
        //                ID = item.ID,
        //                ProductVariant = new ProductVariantModel()
        //                {
        //                    ID = item.ProductVariant.ProductVariantID,
        //                    SKU = item.ProductVariant.ProductVariantSKU,
        //                    Options = item.ProductVariant.ProductVariantOptions,
        //                    Product = new ProductModel()
        //                    {
        //                        ProductName = item.ProductVariant.Product.ProductName
        //                    },
        //                    Pallet = new PalletModel()
        //                    {
        //                        PalletCode = item.ProductVariant.Pallet.PalletCode
        //                    },
        //                },
        //                Quantity = item.Quantity,
        //                Price = item.Price,
        //                TotalAmount = item.TotalAmount,
        //            });
        //        }

        //        model = new DetailInventoryOutputViewModel()
        //        {
        //            Inventory = new InventoryModel()
        //            {
        //                ID = itemData.InventoryOutputID,
        //                InventoryCode = itemData.InventoryOutputCode,
        //                ReceiverName = itemData.ReceiverName,
        //                WareHouse = new WareHouseModel()
        //                {
        //                    WareHouseName = itemData.WareHouse.WareHouseName,
        //                    FullAddress = itemData.WareHouse.FullAddress
        //                },
        //                Shop = new ShopModel()
        //                {
        //                    ID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //                },
        //                Status = new BaseModel()
        //                {
        //                    Code = itemData.Status.Code,
        //                    Name = itemData.Status.Name
        //                },
        //                Order = new OrderModel()
        //                {
        //                    OrderID = itemData.Order.OrderID,
        //                    EMSOneCode = itemData.Order.EMSOneCode,
        //                    BatchCode = itemData.Order.BatchCode
        //                },
        //                TotalAmount = itemData.TotalAmount,
        //                TotalAmountByText = itemData.TotalAmountByText
        //            },
        //            ListInventoryOutputDetail = ListInventoryOutputDetail
        //        };
        //    }
        //    return View(model);
        //}

        [AuthorizeController(FunctionalTitle = "CHI_TIET_PHIEU_XUAT_KHO")]
        public ActionResult DetailInventoryOutputView(SearchListInventoryOutputViewModel req)
        {
            List<DetailInventoryOutputViewModel> model = new List<DetailInventoryOutputViewModel>();
            var InventoryID = Convert.ToInt32(Security.DecodeBase64(req.SearchKey));
            var res = InventoryOutputRepository.InventoryOutputInfo(new ZInventoryOutputInfoRequest()
            {
                InventoryOutputID = InventoryID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<List<ZInventoryOutputInfoResponse>>(res.Data);
                foreach (var ItemInventoryOutput in item)
                {
                    List<InventoryDetailModel> ListInventoryOutputDetail = new List<InventoryDetailModel>();
                    foreach (var ItemInventoryOutputDetail in ItemInventoryOutput.ListInventoryOutputDetail)
                    {
                        ListInventoryOutputDetail.Add(new InventoryDetailModel()
                        {
                            ID = ItemInventoryOutputDetail.ID,
                            Product = new ProductModel()
                            {
                                ProductName = ItemInventoryOutputDetail.Product.ProductName,
                                ProductVariant = new ProductVariantModel()
                                {
                                    ID = ItemInventoryOutputDetail.Product.ProductVariant.ProductVariantID,
                                    SKU = ItemInventoryOutputDetail.Product.ProductVariant.ProductVariantSKU,
                                    Options = ItemInventoryOutputDetail.Product.ProductVariant.ProductVariantOptions,
                                    Pallet = new PalletModel()
                                    {
                                        PalletCode = ItemInventoryOutputDetail.Product.ProductVariant.Pallet.PalletCode
                                    },
                                },
                            },
                            Quantity = ItemInventoryOutputDetail.Quantity,
                            Price = ItemInventoryOutputDetail.Price,
                            TotalAmount = ItemInventoryOutputDetail.TotalAmount,
                        });
                    }

                    model.Add(new DetailInventoryOutputViewModel()
                    {
                        Inventory = new InventoryModel()
                        {
                            ID = ItemInventoryOutput.InventoryOutputID,
                            InventoryCode = ItemInventoryOutput.InventoryOutputCode,
                            ReceiverName = ItemInventoryOutput.ReceiverName,
                            WareHouse = new WareHouseModel()
                            {
                                WareHouseName = ItemInventoryOutput.WareHouse.WareHouseName,
                                FullAddress = ItemInventoryOutput.WareHouse.FullAddress
                            },
                            Shop = new ShopModel()
                            {
                                ID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                            },
                            Status = new BaseModel()
                            {
                                Code = ItemInventoryOutput.Status.Code,
                                Name = ItemInventoryOutput.Status.Name
                            },
                            Order = new OrderModel()
                            {
                                OrderID = ItemInventoryOutput.Order.OrderID,
                                OrderCode = ItemInventoryOutput.Order.OrderCode,
                                EMSOneCode = ItemInventoryOutput.Order.EMSOneCode,
                                BatchCode = ItemInventoryOutput.Order.BatchCode,
                                OrderStatus = new BaseModel()
                                {
                                    Code = ItemInventoryOutput.Order.OrderStatus.Code,
                                    Name = ItemInventoryOutput.Order.OrderStatus.Name
                                },
                            },
                            TotalAmount = ItemInventoryOutput.TotalAmount,
                            TotalAmountByText = ItemInventoryOutput.TotalAmountByText
                        },
                        ListInventoryOutputDetail = ListInventoryOutputDetail
                    });
                }

                var isStatus = true;
                var countStatus = model.GroupBy(m => m.Inventory.Status).Count();
                if (countStatus != model.Count)
                {
                    isStatus = false;
                }

                ViewBag.CheckStatus = isStatus;
            }
            return View(model);
        }
        #endregion

        #region Thêm phiếu xuất kho bằng Excel
        public ActionResult ImportExcelView()
        {
            ImportExcelViewModel model = new ImportExcelViewModel();
            model.License = XHelper.JExcelLicense;

            var ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            foreach (var item in ListWareHouse)
            {
                model.ListWareHouse.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.WareHouseName,
                    code = item.WareHouseCode
                });
            }
            return View(model);
        }

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
                var ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                var WareHouseDefaultID = 0;
                if (ListWareHouse.Count > 0)
                {
                    var WareHouseDefault = ListWareHouse.Where(m => m.IsDefault == "Y").ToList();
                    if (WareHouseDefault.Count > 0)
                    {
                        WareHouseDefaultID = WareHouseDefault.First().ID;
                    }
                }
                for (int row = 1; row <= total; row++)
                {
                    var WareHouseID = 0;
                    var SKUID = 0;
                    var SKUName = "";
                    var SKUInventory = 0;
                    if (sheet.GetRow(row) != null)
                    {
                        var CustomerName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(1));
                        var WareHouseCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(2));
                        var SKUCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(3));
                        var NumberOfOutput = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(4));
                        var InventoryOutputCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(5));
                        var InventoryOutputDate = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(6));
                        var NumberOfDateInWareHouse = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(7));
                        var TermInWareHouse = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(8));
                        var PalletCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(9));

                        if (string.IsNullOrEmpty(CustomerName) && string.IsNullOrEmpty(WareHouseCode) && string.IsNullOrEmpty(SKUCode)
                            && string.IsNullOrEmpty(NumberOfOutput) && string.IsNullOrEmpty(InventoryOutputCode) && string.IsNullOrEmpty(InventoryOutputDate)
                            && string.IsNullOrEmpty(NumberOfDateInWareHouse) && string.IsNullOrEmpty(TermInWareHouse) && string.IsNullOrEmpty(PalletCode))
                        {
                            continue;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(WareHouseCode))
                            {
                                WareHouseID = WareHouseDefaultID;
                            }
                            else
                            {
                                if (ListWareHouse.Count > 0)
                                {
                                    var Filter = ListWareHouse.Where(m => m.WareHouseCode.ToUpper() == WareHouseCode.Trim().ToUpper()).ToList();
                                    if (Filter.Count > 0)
                                    {
                                        WareHouseID = Filter.First().ID;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(SKUCode) && WareHouseID > 0)
                            {
                                var ListProductVariant = XCommon.SearchListProductVariantByWareHouseID(new ZProductVariantGetByWareHouseIDRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID, WareHouseID = WareHouseID });
                                if (ListProductVariant.Count > 0)
                                {
                                    var Filter = ListProductVariant.Where(m => m.ProductVariantSKU.ToUpper() == SKUCode.ToUpper()).ToList();
                                    if (Filter.Count > 0)
                                    {
                                        SKUID = Filter.First().ProductVariantID;
                                        SKUName = Filter.First().ProductName + "(" + Filter.First().ProductVariantName + ")";
                                        SKUInventory = Filter.First().NumberOfInventory;
                                    }
                                }
                            }

                            if (WareHouseID == 0 && SKUID == 0)
                            {
                                NumberOfOutput = "";
                            }

                            if (!string.IsNullOrEmpty(InventoryOutputDate))
                            {
                                var CheckRegex = XCommon.CheckRegex(InventoryOutputDate, "Date");
                                if (CheckRegex == true)
                                {
                                    InventoryOutputDate = DateTime.ParseExact(InventoryOutputDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    InventoryOutputDate = "";
                                }
                            }

                            if (!string.IsNullOrEmpty(NumberOfDateInWareHouse))
                            {
                                var CheckRegex = XCommon.CheckRegex(NumberOfDateInWareHouse, "Number");
                                if (CheckRegex == false)
                                {
                                    NumberOfDateInWareHouse = "";
                                }
                            }

                            if (!string.IsNullOrEmpty(TermInWareHouse))
                            {
                                var CheckRegex = XCommon.CheckRegex(TermInWareHouse, "Date");
                                if (CheckRegex == true)
                                {
                                    TermInWareHouse = DateTime.ParseExact(TermInWareHouse, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    TermInWareHouse = "";
                                }
                            }

                            ArrayList data = new ArrayList();
                            data.Add("");                       //00
                            data.Add(CustomerName);             //01
                            data.Add(WareHouseID);              //02
                            data.Add(SKUID);                    //03
                            data.Add(SKUInventory);             //04
                            data.Add(NumberOfOutput);           //05
                            data.Add(InventoryOutputCode);      //06
                            data.Add(InventoryOutputDate);      //07
                            data.Add(NumberOfDateInWareHouse);  //08
                            data.Add(TermInWareHouse);          //09
                            data.Add(PalletCode);               //10
                            data.Add(SKUName);                  //11
                            model.Add(data);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JImportExcelData: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JImportExcel(List<ImportExcelInventoryOutputModel> ListData, int NumberOfReject, int NumberOfError)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (ListData == null)
                {
                    ListData = new List<ImportExcelInventoryOutputModel>();
                }
                var Total = ListData.Count;
                if (Total > 1000)
                {
                    res.Code = "02";
                    res.Message = "Hệ thống chỉ hỗ trợ import tối đa 1000 dòng";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                var CurrentError = NumberOfError;
                var CurrentSuccess = 0;
                List<ArrayList> ListDataError = new List<ArrayList>();
                var WareHouseDefaultID = 0;
                var WareHouseDefault = XCommon.DetailWareHouseDefault(new ZWarehouseGetDefaultWarehouseRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (WareHouseDefault != null)
                {
                    WareHouseDefaultID = WareHouseDefault.ID;
                }

                foreach (var Item in ListData)
                {
                    List<ZInventoryOutputDetailAddRequest> ListInventoryOutputDetail = new List<ZInventoryOutputDetailAddRequest>();

                    if (Item.WareHouseID == 0)
                    {
                        Item.WareHouseID = WareHouseDefaultID;
                    }

                    if (!string.IsNullOrEmpty(Item.InventoryOutputDate))
                    {
                        Item.InventoryOutputDate = DateTime.ParseExact(Item.InventoryOutputDate, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        Item.InventoryOutputDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    }

                    var IsCheckErrorDetail = false;
                    if (Item.ListInventoryOutputDetail.Count > 0)
                    {
                        foreach (var ItemDetail in Item.ListInventoryOutputDetail)
                        {
                            Item.SKUID = ItemDetail.SKUID;
                            Item.SKUInventory = ItemDetail.SKUInventory;
                            Item.NumberOfOutput = ItemDetail.NumberOfOutput;
                            Item.NumberOfDateInWareHouse = ItemDetail.NumberOfDateInWareHouse;
                            if (!string.IsNullOrEmpty(ItemDetail.TermInWareHouse))
                            {
                                ItemDetail.TermInWareHouse = DateTime.ParseExact(ItemDetail.TermInWareHouse, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy HH:mm:ss");
                                Item.TermInWareHouse = ItemDetail.TermInWareHouse;
                            }
                            Item.SKUName = ItemDetail.SKUName;
                            if (ItemDetail.SKUID > 0)
                            {
                                var ListProductVariant = XCommon.SearchListProductVariantByWareHouseID(new ZProductVariantGetByWareHouseIDRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID, WareHouseID = Item.WareHouseID });
                                if (ListProductVariant.Count > 0)
                                {
                                    var Filter = ListProductVariant.Where(m => m.ProductVariantID == ItemDetail.SKUID).ToList();
                                    if (Filter.Count > 0)
                                    {
                                        if (ItemDetail.NumberOfOutput > Filter.First().NumberOfInventory)
                                        {
                                            ListDataError.Add(JImportExcelError(Item, "Số lượng xuất không được vượt quá số lượng tồn."));
                                            NumberOfError++;
                                            IsCheckErrorDetail = true;
                                            continue;
                                        }
                                        else
                                        {
                                            ListInventoryOutputDetail.Add(new ZInventoryOutputDetailAddRequest()
                                            {
                                                ProductVariantID = ItemDetail.SKUID,
                                                Quantity = ItemDetail.NumberOfOutput,
                                                NumberOfDateInWareHouse = ItemDetail.NumberOfDateInWareHouse,
                                                TermInWareHouse = ItemDetail.TermInWareHouse,
                                                PalletCode = ItemDetail.PalletCode,
                                            });
                                        }
                                    }
                                    else
                                    {
                                        ListDataError.Add(JImportExcelError(Item, "Sản phẩm không tồn tại trong kho."));
                                        NumberOfError++;
                                        IsCheckErrorDetail = true;
                                        continue;
                                    }
                                }
                                else
                                {
                                    ListDataError.Add(JImportExcelError(Item, "Kho không có sản phẩm."));
                                    NumberOfError++;
                                    IsCheckErrorDetail = true;
                                    continue;
                                }
                            }
                            else
                            {
                                ListDataError.Add(JImportExcelError(Item, "Vui lòng chọn sản phẩm."));
                                NumberOfError++;
                                IsCheckErrorDetail = true;
                                continue;
                            }
                        }

                        if (IsCheckErrorDetail == false)
                        {
                            res = InventoryOutputRepository.AddInventoryOutput(new ZInventoryOutputAddRequest()
                            {
                                AmndState = "A",
                                InventoryCode = Item.InventoryOutputCode,
                                WareHouseID = Item.WareHouseID,
                                DateOfInventoryOutput = Item.InventoryOutputDate,
                                ReceiverName = Item.CustomerName,
                                MobileNumber = "",
                                Note = "",
                                CreatedBy = XHelper.Session.EmployeeProfile.ID,
                                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                                ListInventoryOutputDetail = ListInventoryOutputDetail
                            });

                            if (res.Code == "00")
                            {
                                CurrentSuccess++;
                            }
                            else
                            {
                                foreach (var ItemDetail in Item.ListInventoryOutputDetail)
                                {
                                    Item.SKUID = ItemDetail.SKUID;
                                    Item.SKUInventory = ItemDetail.SKUInventory;
                                    Item.NumberOfOutput = ItemDetail.NumberOfOutput;
                                    Item.NumberOfDateInWareHouse = ItemDetail.NumberOfDateInWareHouse;
                                    Item.TermInWareHouse = ItemDetail.TermInWareHouse;
                                    Item.PalletCode = ItemDetail.PalletCode;
                                    Item.SKUName = ItemDetail.SKUName;
                                    ListDataError.Add(JImportExcelError(Item, res.Message));
                                }
                                NumberOfError++;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        ListDataError.Add(JImportExcelError(Item, "Vui lòng chọn sản phẩm."));
                        NumberOfError++;
                        continue;
                    }
                }

                if (CurrentSuccess == ListData.Count)
                {
                    res.Code = "00";
                    res.Message = string.Format("<div class='text-green mb-2'>Phiếu xuất kho thành công: {0}</div><div class='text-warning mb-2'>Phiếu xuất kho không hợp lệ: {1}</div><div class='text-danger mb-3'>Phiếu xuất kho lỗi: {2}</div>", CurrentSuccess, NumberOfReject, NumberOfError);
                }
                else
                {
                    res.Code = "10";
                    res.Message = string.Format("<div class='text-green mb-2'>Phiếu xuất kho thành công: {0}</div><div class='text-warning mb-2'>Phiếu xuất kho không hợp lệ: {1}</div><div class='text-danger mb-3'>Phiếu xuất kho lỗi: {2}</div>", CurrentSuccess, NumberOfReject, NumberOfError);
                    res.Data = JsonConvert.SerializeObject(ListDataError);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JImportExcel: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ArrayList JImportExcelError(ImportExcelInventoryOutputModel data, string Message)
        {
            ArrayList model = new ArrayList();
            model.Add("Lỗi");
            model.Add(data.CustomerName);
            model.Add(data.WareHouseID);
            model.Add(data.SKUID);
            model.Add(data.SKUInventory);
            model.Add(data.NumberOfOutput);
            model.Add(data.InventoryOutputCode);

            if (string.IsNullOrEmpty(data.InventoryOutputDate))
            {
                model.Add("");
            }
            else
            {
                model.Add(DateTime.ParseExact(data.InventoryOutputDate, "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss"));
            }

            model.Add(data.NumberOfDateInWareHouse);

            if (string.IsNullOrEmpty(data.TermInWareHouse))
            {
                model.Add("");
            }
            else
            {
                model.Add(DateTime.ParseExact(data.TermInWareHouse, "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss"));
            }

            model.Add(data.PalletCode);
            model.Add(data.SKUName);
            model.Add(Message);
            model.Add(data.NumberOfRow);
            return model;
        }

        public ActionResult JExportExcel(List<ExportExcelInventoryOutputModel> ListData)
        {
            var FullPath = System.Web.HttpContext.Current.Server.MapPath("~/Assets/temp/MauImportPhieuXuatKho.xlsx");
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
                    XSSFCellStyle borderedCellStyle = (XSSFCellStyle)xlsx.CreateCellStyle();
                    borderedCellStyle.BorderLeft = BorderStyle.Thin;
                    borderedCellStyle.BorderTop = BorderStyle.Thin;
                    borderedCellStyle.BorderRight = BorderStyle.Thin;
                    borderedCellStyle.BorderBottom = BorderStyle.Thin;

                    sheet.GetRow(0).CreateCell(10).SetCellValue("Nguyên nhân lỗi");
                    sheet.GetRow(0).GetCell(10).CellStyle = borderedCellStyle;

                    foreach (var item in ListData)
                    {
                        // tao row mới
                        var newRow = sheet.CreateRow(rowIndex);
                        newRow.CreateCell(0).SetCellValue(stt++);
                        newRow.CreateCell(1).SetCellValue(item.CustomerName);
                        newRow.CreateCell(2).SetCellValue(item.WareHouseCode);
                        newRow.CreateCell(3).SetCellValue(item.SKUCode);
                        newRow.CreateCell(4).SetCellValue(item.NumberOfOutput);
                        newRow.CreateCell(5).SetCellValue(item.InventoryOutputCode);

                        if (string.IsNullOrEmpty(item.InventoryOutputDate))
                        {
                            newRow.CreateCell(6).SetCellValue("");
                        }
                        else
                        {
                            newRow.CreateCell(6).SetCellValue(DateTime.ParseExact(item.InventoryOutputDate, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy"));
                        }

                        newRow.CreateCell(7).SetCellValue(item.NumberOfDateInWareHouse);

                        if (string.IsNullOrEmpty(item.TermInWareHouse))
                        {
                            newRow.CreateCell(8).SetCellValue("");
                        }
                        else
                        {
                            newRow.CreateCell(8).SetCellValue(DateTime.ParseExact(item.TermInWareHouse, "yyyy-MM-dd HH:mm:ss", null).ToString("dd/MM/yyyy"));
                        }

                        newRow.CreateCell(9).SetCellValue(item.PalletCode);
                        newRow.CreateCell(10).SetCellValue(item.MessageError);

                        for (int i = 0; i <= 9; i++)
                        {
                            newRow.GetCell(i).CellStyle = borderedCellStyle;
                        }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JExportExcel: " + ex.Message);
            }

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
             "MauImportPhieuXuatKho");
        }
        #endregion

        #region Xử lý phiếu xuất kho
        [AuthorizeController(FunctionalTitle = "XU_LY_PHIEU_XUAT_KHO")]
        public JsonResult JProcessInventoryOutput(SearchListInventoryOutputViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryOutputRepository.ProcessInventoryOutput(new ZInventoryOutputProcessRequest()
                {
                    InventoryOutputID = req.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JProcessInventoryOutput: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Phê duyệt phiếu xuất kho
        [AuthorizeController(FunctionalTitle = "PHE_DUYET_PHIEU_XUAT_KHO")]
        public JsonResult JAcceptInventoryOutput(SearchListInventoryOutputViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (req.ID > 0)
                {
                    res = InventoryOutputRepository.AcceptInventoryOutput(new ZInventoryOutputAcceptRequest()
                    {
                        InventoryOutputID = req.ID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    });
                }
                else
                {
                    var ListOrder = JsonConvert.DeserializeObject<List<SearchListOrderViewModel>>(req.SearchKey);
                    List<ZOrderMultipleSendTransportOrderInfoRequest> ListOrderInfo = new List<ZOrderMultipleSendTransportOrderInfoRequest>();
                    foreach (var item in ListOrder)
                    {
                        ListOrderInfo.Add(new ZOrderMultipleSendTransportOrderInfoRequest()
                        {
                            OrderID = item.OrderID,
                            OrderCode = item.OrderCode,
                            BatchCode = item.BatchCode,
                            OrderStatus = item.OrderStatus
                        });
                    }

                    res = OrderRepository.SendTransport(new ZOrderMultipleSendTransportRequest()
                    {
                        ListOrderInfo = ListOrderInfo,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    });
                }

                //if (string.IsNullOrEmpty(req.Order.EMSOneCode))
                //{
                //    res = InventoryOutputRepository.AcceptInventoryOutput(new ZInventoryOutputAcceptRequest()
                //    {
                //        InventoryOutputID = req.ID,
                //        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                //        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                //    });
                //}
                //else
                //{
                //    res = OrderRepository.SendTransport(new ZOrderMultipleSendTransportRequest()
                //    {
                //        EMSOneCode = req.Order.EMSOneCode,
                //        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                //        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                //    });
                //}
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JAcceptInventoryOutput: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Từ chối phiếu xuất kho
        [AuthorizeController(FunctionalTitle = "TU_CHOI_PHIEU_XUAT_KHO")]
        public ActionResult RejectInventoryOutputView(SearchListInventoryOutputViewModel req)
        {
            RejectInventoryOutputViewModel model = new RejectInventoryOutputViewModel()
            {
                InventoryOutputID = req.ID
            };
            return PartialView(model);
        }

        public JsonResult JRejectInventoryOutput(RejectInventoryOutputViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryOutputRepository.RejectInventoryOutput(new ZInventoryOutputRejectRequest()
                {
                    InventoryOutputID = req.InventoryOutputID,
                    Reason = req.Reason,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JRejectInventoryOutput: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xuất file Word
        public ActionResult ExportWord(SearchListInventoryOutputViewModel req)
        {
            MemoryStream output = new MemoryStream();
            try
            {
                InventoryOutputExportModel model = null;
                var res = InventoryOutputRepository.SearchByInventoryID(new ZInventoryOutputGetByIDRequest()
                {
                    ID = req.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                if (res.Code == "00")
                {
                    var itemData = JsonConvert.DeserializeObject<ZInventoryOutputGetByIDResponse>(res.Data);
                    List<InventoryDetailModel> ListInventoryDetail = new List<InventoryDetailModel>();
                    foreach (var item in itemData.ListInventoryOutputDetail)
                    {
                        ListInventoryDetail.Add(new InventoryDetailModel()
                        {
                            ID = item.ID,
                            Product = new ProductModel() 
                            {
                                ProductName = item.Product.ProductName,
                                ProductVariant = new ProductVariantModel() 
                                {
                                    ID = item.Product.ProductVariant.ProductVariantID,
                                    SKU = item.Product.ProductVariant.ProductVariantSKU,
                                    Options = item.Product.ProductVariant.ProductVariantOptions,
                                    Pallet = new PalletModel()
                                    {
                                        PalletCode = item.Product.ProductVariant.Pallet.PalletCode
                                    }
                                },
                            },
                            Quantity = item.Quantity,
                            Price = item.Price,
                            TotalAmount = item.TotalAmount
                        });
                    }

                    model = new InventoryOutputExportModel()
                    {
                        Inventory = new InventoryModel()
                        {
                            ReceiverName = itemData.ReceiverName,
                            WareHouse = new WareHouseModel()
                            {
                                WareHouseName = itemData.WareHouse.WareHouseName,
                                FullAddress = itemData.WareHouse.FullAddress
                            },
                            TotalAmount = itemData.TotalAmount,
                            TotalAmountByText = itemData.TotalAmountByText,
                        },
                        ListInventoryDetail = ListInventoryDetail,
                    };

                    XWPFDocument document = new XWPFDocument();

                    document.SetParagraph(ParagraphInstanceSetting(document, "PHIẾU XUẤT KHO", true, 16, "Song Style", "#000000", UnderlinePatterns.None, ParagraphAlignment.CENTER), 0);
                    document.SetParagraph(ParagraphInstanceSetting(document, "Ngày.....tháng.....năm ......... ", false, 12, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.CENTER), 1);
                    document.SetParagraph(ParagraphInstanceSetting(document, "Số: ............................", false, 12, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.CENTER), 2);
                    document.SetParagraph(ParagraphInstanceSetting(document, "- Họ và tên người nhận hàng: " + model.Inventory.ReceiverName + "      Địa chỉ (bộ phận): ...............................", false, 12, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.LEFT), 3);
                    document.SetParagraph(ParagraphInstanceSetting(document, "- Lý do xuất kho: .......................................................................................", false, 12, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.LEFT), 4);
                    document.SetParagraph(ParagraphInstanceSetting(document, "- Xuất tại kho: " + model.Inventory.WareHouse.WareHouseName + "      Địa điểm: " + model.Inventory.WareHouse.FullAddress, false, 12, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.LEFT), 5);
                    //Tạo bảng
                    XWPFTable table = document.CreateTable(model.ListInventoryDetail.Count + 3, 8);

                    table.SetCellMargins(50, 50, 50, 50);
                    table.GetRow(0).GetCell(0).SetParagraph(SetCellText(document, table, "STT", ParagraphAlignment.CENTER, 10));
                    table.GetRow(0).GetCell(1).SetParagraph(SetCellText(document, table, "Tên, nhãn hiệu, quy cách, phẩm chất vật tư, dụng cụ, sp, hàng hoá", ParagraphAlignment.CENTER, 10));
                    table.GetRow(0).GetCell(2).SetParagraph(SetCellText(document, table, "Mã số", ParagraphAlignment.CENTER, 10));
                    table.GetRow(0).GetCell(3).SetParagraph(SetCellText(document, table, "ĐVT", ParagraphAlignment.CENTER, 10));
                    table.GetRow(0).MergeCells(4, 5);
                    table.GetRow(0).GetCell(4).SetParagraph(SetCellText(document, table, "Số lượng", ParagraphAlignment.CENTER, 10));
                    table.GetRow(0).GetCell(5).SetParagraph(SetCellText(document, table, "Đơn giá", ParagraphAlignment.CENTER, 10));
                    table.GetRow(0).GetCell(6).SetParagraph(SetCellText(document, table, "Thành tiền", ParagraphAlignment.CENTER, 10));
                    table.GetRow(1).GetCell(0).SetBorderTop(XWPFTable.XWPFBorderType.NONE, 0, 0, "");
                    table.GetRow(1).GetCell(1).SetBorderTop(XWPFTable.XWPFBorderType.NONE, 0, 0, "");
                    table.GetRow(1).GetCell(2).SetBorderTop(XWPFTable.XWPFBorderType.NONE, 0, 0, "");
                    table.GetRow(1).GetCell(3).SetBorderTop(XWPFTable.XWPFBorderType.NONE, 0, 0, "");
                    table.GetRow(1).GetCell(4).SetParagraph(SetCellText(document, table, "Theo chứng từ", ParagraphAlignment.CENTER, 10));
                    table.GetRow(1).GetCell(5).SetParagraph(SetCellText(document, table, "Thực xuất", ParagraphAlignment.CENTER, 10));
                    table.GetRow(1).GetCell(6).SetBorderTop(XWPFTable.XWPFBorderType.NONE, 0, 0, "");
                    table.GetRow(1).GetCell(7).SetBorderTop(XWPFTable.XWPFBorderType.NONE, 0, 0, "");

                    var row = 2;
                    var stt = 1;

                    foreach (var itemInventoryDetail in model.ListInventoryDetail)
                    {
                        table.GetRow(row).GetCell(0).SetParagraph(SetCellText(document, table, stt.ToString(), ParagraphAlignment.CENTER, 10));
                        table.GetRow(row).GetCell(1).SetParagraph(SetCellText(document, table, itemInventoryDetail.Product.ProductVariant.SKU + " - " + itemInventoryDetail.Product.ProductName + Environment.NewLine + ".Mã Pallet: " + itemInventoryDetail.Product.ProductVariant.Pallet.PalletCode + Environment.NewLine + " (" + itemInventoryDetail.Product.ProductVariant.Options + ")", ParagraphAlignment.LEFT, 10));
                        table.GetRow(row).GetCell(2).SetParagraph(SetCellText(document, table, "", ParagraphAlignment.LEFT, 10));
                        table.GetRow(row).GetCell(3).SetParagraph(SetCellText(document, table, "", ParagraphAlignment.LEFT, 10));
                        table.GetRow(row).GetCell(4).SetParagraph(SetCellText(document, table, itemInventoryDetail.Quantity.ToString("N0"), ParagraphAlignment.RIGHT, 10));
                        table.GetRow(row).GetCell(5).SetParagraph(SetCellText(document, table, itemInventoryDetail.Quantity.ToString("N0"), ParagraphAlignment.RIGHT, 10));
                        table.GetRow(row).GetCell(6).SetParagraph(SetCellText(document, table, itemInventoryDetail.Price.ToString("N0"), ParagraphAlignment.RIGHT, 10));
                        table.GetRow(row).GetCell(7).SetParagraph(SetCellText(document, table, itemInventoryDetail.TotalAmount.ToString("N0"), ParagraphAlignment.RIGHT, 10));
                        stt++;
                        row++;
                    }

                    table.GetRow(row).GetCell(1).SetParagraph(SetCellText(document, table, "Cộng", ParagraphAlignment.CENTER, 10, true));
                    table.GetRow(row).GetCell(2).SetParagraph(SetCellText(document, table, "x", ParagraphAlignment.CENTER, 10, true));
                    table.GetRow(row).GetCell(3).SetParagraph(SetCellText(document, table, "x", ParagraphAlignment.CENTER, 10, true));
                    table.GetRow(row).GetCell(4).SetParagraph(SetCellText(document, table, "x", ParagraphAlignment.CENTER, 10, true));
                    table.GetRow(row).GetCell(5).SetParagraph(SetCellText(document, table, "x", ParagraphAlignment.CENTER, 10, true));
                    table.GetRow(row).GetCell(6).SetParagraph(SetCellText(document, table, "x", ParagraphAlignment.CENTER, 10, true));
                    table.GetRow(row).GetCell(7).SetParagraph(SetCellText(document, table, model.Inventory.TotalAmount.ToString("N0"), ParagraphAlignment.RIGHT, 10, true));

                    document.SetParagraph(ParagraphInstanceSetting(document, "", false, 12, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.LEFT), 6);
                    document.SetParagraph(ParagraphInstanceSetting(document, "- Tổng số tiền (viết bằng chữ): " + model.Inventory.TotalAmountByText, false, 12, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.LEFT), 7);
                    document.SetParagraph(ParagraphInstanceSetting(document, "- Số chứng từ gốc kèm theo:..........................................................................................", false, 12, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.LEFT), 8);

                    document.SetParagraph(ParagraphInstanceSetting(document, "Người lập phiếu      Người nhận hàng      Thủ kho      Kế toán trưởng      Giám đốc", true, 12, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.CENTER, 0), 9);
                    document.SetParagraph(ParagraphInstanceSetting(document, "               (Ký, họ tên)                  (Ký, họ tên)            (Ký, họ tên)         (Ký, họ tên)           (Ký, họ tên)", false, 11, "Song Style", "", UnderlinePatterns.None, ParagraphAlignment.LEFT), 10);

                    //XWPFRun run = document.CreateParagraph().CreateRun();
                    //run.AddBreak(BreakType.PAGE);

                    document.Write(output);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.ExportWord: " + ex.Message);
            }
            //Return the result to the end user
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
             "PhieuXuatKho.docx");
        }

        private static XWPFParagraph ParagraphInstanceSetting(XWPFDocument document, string fillContent, bool isBold, int fontSize, string fontFamily, string rgbStr, UnderlinePatterns value, ParagraphAlignment paragraphAlign, int textPos = 8/*, bool isStatement = false, string secondFillContent = ""*/)
        {
            XWPFParagraph paragraph = document.CreateParagraph();//Create Paragraph Object
            paragraph.Alignment = paragraphAlign;//Text Display Position,Paragraph alignment (left, center, right)
            XWPFRun xwpfRun = paragraph.CreateRun();//Create Paragraph Text Object
            xwpfRun.IsBold = isBold;//font-weight
            xwpfRun.SetText(fillContent);//Fill in content
            xwpfRun.FontSize = fontSize;//Set text size
            xwpfRun.SetFontFamily(fontFamily, FontCharRange.None); //Set the title style such as: (Microsoft Yahei, Lishu, Kai Title) according to your needs
            xwpfRun.SetColor(rgbStr);//Set Font Color--Hexadecimal
            xwpfRun.SetUnderline(value);//Set Underline, Enumerate Types
            ////Style of the second text
            //if (isStatement)
            //{
            //    XWPFRun secondxwpfRun = paragraph.CreateRun();//Create Paragraph Text Object
            //    secondxwpfRun.IsBold = isBold;//font-weight
            //    secondxwpfRun.SetText(secondFillContent);//Fill in content
            //    secondxwpfRun.FontSize = fontSize;//Set text size
            //    secondxwpfRun.SetFontFamily(fontFamily, FontCharRange.None); //Set the title style such as: (Microsoft Yahei, Lishu, Kai Title) according to your needs
            //}
            return paragraph;
        }

        public static XWPFParagraph SetCellText(XWPFDocument doc, XWPFTable table, string setText, ParagraphAlignment align, int textPos, bool isBold = false, bool borderNone = false)
        {
            var para = new CT_P();
            var pCell = new XWPFParagraph(para, table.Body);
            //pCell.Alignment = ParagraphAlignment.LEFT;// typeface 
            pCell.Alignment = align;
            var r1c1 = pCell.CreateRun();
            r1c1.SetText(setText);
            r1c1.FontSize = 12;
            r1c1.IsBold = isBold;//font-weight
            r1c1.SetFontFamily("Song Style", FontCharRange.None); // Set elegant black font 
            //r1c1.SetTextPosition(textPos); // Set height 
            if (borderNone)
            {
                pCell.BorderBottom = Borders.None;
                pCell.BorderLeft = Borders.None;
                pCell.BorderRight = Borders.None;
                pCell.BorderTop = Borders.None;
            }
            return pCell;
        }
        #endregion

        #region Xuất file Excel
        public ActionResult ExportExcel(SearchListInventoryOutputViewModel req)
        {
            MemoryStream output = new MemoryStream();
            try
            {
                InventoryOutputExportModel model = null;
                var res = InventoryOutputRepository.SearchByInventoryID(new ZInventoryOutputGetByIDRequest()
                {
                    ID = req.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                if (res.Code == "00")
                {
                    #region Lấy data
                    var itemData = JsonConvert.DeserializeObject<ZInventoryOutputGetByIDResponse>(res.Data);
                    List<InventoryDetailModel> ListInventoryDetail = new List<InventoryDetailModel>();
                    foreach (var item in itemData.ListInventoryOutputDetail)
                    {
                        ListInventoryDetail.Add(new InventoryDetailModel()
                        {
                            ID = item.ID,
                            Product = new ProductModel()
                            {
                                ProductName = item.Product.ProductName,
                                ProductVariant = new ProductVariantModel()
                                {
                                    ID = item.Product.ProductVariant.ProductVariantID,
                                    SKU = item.Product.ProductVariant.ProductVariantSKU,
                                    Options = item.Product.ProductVariant.ProductVariantOptions,
                                    Pallet = new PalletModel()
                                    {
                                        PalletCode = item.Product.ProductVariant.Pallet.PalletCode
                                    }
                                },
                            },
                            Quantity = item.Quantity,
                            Price = item.Price,
                            TotalAmount = item.TotalAmount
                        });
                    }

                    model = new InventoryOutputExportModel()
                    {
                        Inventory = new InventoryModel()
                        {
                            ReceiverName = itemData.ReceiverName,
                            WareHouse = new WareHouseModel()
                            {
                                WareHouseName = itemData.WareHouse.WareHouseName,
                                FullAddress = itemData.WareHouse.FullAddress
                            },
                            TotalAmount = itemData.TotalAmount,
                            TotalAmountByText = itemData.TotalAmountByText
                        },
                        ListInventoryDetail = ListInventoryDetail,
                    };
                    #endregion

                    //Create new Excel Workbook
                    var workbook = new HSSFWorkbook();

                    //Create new Excel Sheet
                    var sheet = workbook.CreateSheet();

                    sheet.SetColumnWidth(1, 5000);
                    sheet.SetColumnWidth(2, 5000);
                    sheet.SetColumnWidth(3, 3000);
                    sheet.SetColumnWidth(4, 5000);
                    sheet.SetColumnWidth(5, 3000);

                    var boldFont = workbook.CreateFont();
                    boldFont.FontName = "Times New Roman";
                    boldFont.FontHeightInPoints = 13; //Font size
                    boldFont.IsBold = true;

                    var ItalicFont = workbook.CreateFont();
                    ItalicFont.IsItalic = true;
                    ItalicFont.FontName = "Times New Roman";
                    ItalicFont.FontHeightInPoints = 11;

                    var TitleStyle = workbook.CreateCellStyle();
                    TitleStyle.SetFont(boldFont);
                    TitleStyle.VerticalAlignment = VerticalAlignment.Center;
                    TitleStyle.Alignment = HorizontalAlignment.Center;

                    var CenterStyle = workbook.CreateCellStyle();
                    CenterStyle.VerticalAlignment = VerticalAlignment.Center;
                    CenterStyle.Alignment = HorizontalAlignment.Center;

                    var Center_Italic_Style = workbook.CreateCellStyle();
                    Center_Italic_Style.SetFont(ItalicFont);
                    Center_Italic_Style.VerticalAlignment = VerticalAlignment.Center;
                    Center_Italic_Style.Alignment = HorizontalAlignment.Center;

                    var borderAllStyle = workbook.CreateCellStyle();
                    borderAllStyle.BorderTop = BorderStyle.Thin;
                    borderAllStyle.BorderBottom = BorderStyle.Thin;
                    borderAllStyle.BorderLeft = BorderStyle.Thin;
                    borderAllStyle.BorderRight = BorderStyle.Thin;

                    var Center_BorderAll_Style = workbook.CreateCellStyle();
                    Center_BorderAll_Style.Alignment = HorizontalAlignment.Center;
                    Center_BorderAll_Style.BorderTop = BorderStyle.Thin;
                    Center_BorderAll_Style.BorderBottom = BorderStyle.Thin;
                    Center_BorderAll_Style.BorderLeft = BorderStyle.Thin;
                    Center_BorderAll_Style.BorderRight = BorderStyle.Thin;


                    //Create title 
                    var TitleRow = sheet.CreateRow(0);
                    TitleRow.CreateCell(1).SetCellValue("PHIẾU XUẤT KHO");
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 6));
                    TitleRow.GetCell(1).CellStyle = TitleStyle;

                    var DateMonthYear = sheet.CreateRow(1);
                    DateMonthYear.CreateCell(2).SetCellValue("Ngày.....tháng.....năm .........");
                    sheet.AddMergedRegion(new CellRangeAddress(1, 1, 2, 5));
                    DateMonthYear.GetCell(2).CellStyle = CenterStyle;

                    var RowTemp1 = sheet.CreateRow(2);
                    RowTemp1.CreateCell(2).SetCellValue("Số:.............................");
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 2, 5));
                    RowTemp1.GetCell(2).CellStyle = CenterStyle;

                    var FullName = sheet.CreateRow(4);
                    FullName.CreateCell(0).SetCellValue(string.Format("- Họ và tên người nhận hàng: {0}     Địa chỉ (bộ phận):.......................", model.Inventory.ReceiverName));

                    var Reason = sheet.CreateRow(5);
                    Reason.CreateCell(0).SetCellValue("- Lý do xuất kho:.........................................");

                    var OutputInWareHouse = sheet.CreateRow(6);
                    OutputInWareHouse.CreateCell(0).SetCellValue(string.Format("- Xuất tại kho: {0}     Địa điểm: {1}", model.Inventory.WareHouse.WareHouseName, model.Inventory.WareHouse.FullAddress));

                    var HeaderTable1 = sheet.CreateRow(8);
                    HeaderTable1.CreateCell(0).SetCellValue("STT");
                    HeaderTable1.GetCell(0).CellStyle = borderAllStyle;
                    HeaderTable1.CreateCell(1).SetCellValue("Tên, nhãn hiệu, quy cách, phẩm chất vật tư, dụng cụ, sp, hàng hoá");
                    HeaderTable1.GetCell(1).CellStyle = borderAllStyle;
                    HeaderTable1.CreateCell(2).SetCellValue("Mã số");
                    HeaderTable1.GetCell(2).CellStyle = borderAllStyle;
                    HeaderTable1.CreateCell(3).SetCellValue("ĐVT");
                    HeaderTable1.GetCell(3).CellStyle = borderAllStyle;
                    HeaderTable1.CreateCell(4).SetCellValue("Số lượng");
                    HeaderTable1.GetCell(4).CellStyle = Center_BorderAll_Style;
                    HeaderTable1.CreateCell(5).SetCellValue("");
                    HeaderTable1.GetCell(5).CellStyle = Center_BorderAll_Style;
                    sheet.AddMergedRegion(new CellRangeAddress(8, 8, 4, 5));
                    HeaderTable1.CreateCell(6).SetCellValue("Đơn giá");
                    HeaderTable1.GetCell(6).CellStyle = borderAllStyle;
                    HeaderTable1.CreateCell(7).SetCellValue("Thành tiền");
                    HeaderTable1.GetCell(7).CellStyle = borderAllStyle;

                    var HeaderTable2 = sheet.CreateRow(9);
                    HeaderTable2.CreateCell(0).SetCellValue("");
                    HeaderTable2.GetCell(0).CellStyle = borderAllStyle;
                    HeaderTable2.CreateCell(1).SetCellValue("");
                    HeaderTable2.GetCell(1).CellStyle = borderAllStyle;
                    HeaderTable2.CreateCell(2).SetCellValue("");
                    HeaderTable2.GetCell(2).CellStyle = borderAllStyle;
                    HeaderTable2.CreateCell(3).SetCellValue("");
                    HeaderTable2.GetCell(3).CellStyle = borderAllStyle;
                    HeaderTable2.CreateCell(4).SetCellValue("Theo chứng từ");
                    HeaderTable2.GetCell(4).CellStyle = borderAllStyle;
                    HeaderTable2.CreateCell(5).SetCellValue("Thực xuất");
                    HeaderTable2.GetCell(5).CellStyle = borderAllStyle;
                    HeaderTable2.CreateCell(6).SetCellValue("");
                    HeaderTable2.GetCell(6).CellStyle = borderAllStyle;
                    HeaderTable2.CreateCell(7).SetCellValue("");
                    HeaderTable2.GetCell(7).CellStyle = borderAllStyle;

                    var rowNumber = 10;
                    var rowIndex = 0;
                    var stt = 1;
                    foreach (var itemInventoryDetail in model.ListInventoryDetail)
                    {
                        var row = sheet.CreateRow(rowNumber++);
                        row.CreateCell(rowIndex++).SetCellValue(stt.ToString());
                        row.CreateCell(rowIndex++).SetCellValue(itemInventoryDetail.Product.ProductVariant.SKU + " - " + itemInventoryDetail.Product.ProductName + Environment.NewLine + "Mã Pallet: " + itemInventoryDetail.Product.ProductVariant.Pallet.PalletCode + Environment.NewLine + "(" + itemInventoryDetail.Product.ProductVariant.Options + ")");
                        row.CreateCell(rowIndex++).SetCellValue("");
                        row.CreateCell(rowIndex++).SetCellValue("");
                        row.CreateCell(rowIndex++).SetCellValue(itemInventoryDetail.Quantity.ToString("N0"));
                        row.CreateCell(rowIndex++).SetCellValue(itemInventoryDetail.Quantity.ToString("N0"));
                        row.CreateCell(rowIndex++).SetCellValue(itemInventoryDetail.Price.ToString("N0"));
                        row.CreateCell(rowIndex++).SetCellValue(itemInventoryDetail.TotalAmount.ToString("N0"));

                        for (var i = 0; i < rowIndex; i++)
                        {
                            row.GetCell(i).CellStyle = borderAllStyle;
                        }

                        rowIndex = 0;
                        stt++;
                    }

                    var FooterTable = sheet.CreateRow(rowNumber++);
                    FooterTable.CreateCell(0).SetCellValue("");
                    FooterTable.GetCell(0).CellStyle = borderAllStyle;
                    FooterTable.CreateCell(1).SetCellValue("Cộng");
                    FooterTable.GetCell(1).CellStyle = borderAllStyle;
                    FooterTable.CreateCell(2).SetCellValue("x");
                    FooterTable.GetCell(2).CellStyle = borderAllStyle;
                    FooterTable.CreateCell(3).SetCellValue("x");
                    FooterTable.GetCell(3).CellStyle = borderAllStyle;
                    FooterTable.CreateCell(4).SetCellValue("x");
                    FooterTable.GetCell(4).CellStyle = borderAllStyle;
                    FooterTable.CreateCell(5).SetCellValue("x");
                    FooterTable.GetCell(5).CellStyle = borderAllStyle;
                    FooterTable.CreateCell(6).SetCellValue("x");
                    FooterTable.GetCell(6).CellStyle = borderAllStyle;
                    FooterTable.CreateCell(7).SetCellValue(model.Inventory.TotalAmount.ToString("N0"));
                    FooterTable.GetCell(7).CellStyle = borderAllStyle;

                    rowNumber = rowNumber + 1;

                    var TotalAmountByText = sheet.CreateRow(rowNumber++);
                    TotalAmountByText.CreateCell(0).SetCellValue(string.Format("Tổng số tiền (viết bằng chữ): {0}", model.Inventory.TotalAmountByText));

                    var RowTemp2 = sheet.CreateRow(rowNumber++);
                    RowTemp2.CreateCell(0).SetCellValue("Số chứng từ gốc kèm theo:..........................................................................................");

                    rowNumber = rowNumber + 1;

                    var RowTemp3 = sheet.CreateRow(rowNumber++);
                    RowTemp3.CreateCell(1).SetCellValue("Người lập phiếu");
                    RowTemp3.GetCell(1).CellStyle = TitleStyle;
                    RowTemp3.CreateCell(2).SetCellValue("Người nhận hàng");
                    RowTemp3.GetCell(2).CellStyle = TitleStyle;
                    RowTemp3.CreateCell(3).SetCellValue("Thủ kho");
                    RowTemp3.GetCell(3).CellStyle = TitleStyle;
                    RowTemp3.CreateCell(4).SetCellValue("Kế toán trưởng");
                    RowTemp3.GetCell(4).CellStyle = TitleStyle;
                    RowTemp3.CreateCell(5).SetCellValue("Giám đốc");
                    RowTemp3.GetCell(5).CellStyle = TitleStyle;

                    var RowTemp4 = sheet.CreateRow(rowNumber++);
                    RowTemp4.CreateCell(1).SetCellValue("(Ký, họ tên)");
                    RowTemp4.GetCell(1).CellStyle = Center_Italic_Style;
                    RowTemp4.CreateCell(2).SetCellValue("(Ký, họ tên)");
                    RowTemp4.GetCell(2).CellStyle = Center_Italic_Style;
                    RowTemp4.CreateCell(3).SetCellValue("(Ký, họ tên)");
                    RowTemp4.GetCell(3).CellStyle = Center_Italic_Style;
                    RowTemp4.CreateCell(4).SetCellValue("(Ký, họ tên)");
                    RowTemp4.GetCell(4).CellStyle = Center_Italic_Style;
                    RowTemp4.CreateCell(5).SetCellValue("(Ký, họ tên)");
                    RowTemp4.GetCell(5).CellStyle = Center_Italic_Style;

                    //Write the Workbook to a memory stream
                    workbook.Write(output);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.ExportExcel: " + ex.Message);
            }
            return File(output.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "PhieuXuatKho.xls");
        }
        #endregion

        #region In phiếu xuất kho
        public ActionResult PrintInventoryOutputView()
        {
            return PartialView();
        }

        public ActionResult PrintInventoryOutputTypeView(SearchListInventoryOutputViewModel req)
        {
            List<PrintInventoryOutputTypeViewModel> model = new List<PrintInventoryOutputTypeViewModel>();
            var ListDataInventoryOutputPrint = JsonConvert.DeserializeObject<List<SearchListInventoryOutputViewModel>>(req.ListDataInventoryOutputPrint);
            foreach (var itemPrint in ListDataInventoryOutputPrint)
            {
                var res = InventoryOutputRepository.SearchByInventoryID(new ZInventoryOutputGetByIDRequest()
                {
                    ID = itemPrint.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                if (res.Code == "00")
                {
                    var itemData = JsonConvert.DeserializeObject<ZInventoryOutputGetByIDResponse>(res.Data);
                    List<InventoryDetailModel> ListInventoryDetail = new List<InventoryDetailModel>();
                    foreach (var item in itemData.ListInventoryOutputDetail)
                    {
                        ListInventoryDetail.Add(new InventoryDetailModel()
                        {
                            ID = item.ID,
                            Product = new ProductModel()
                            {
                                ProductName = item.Product.ProductName,
                                ProductVariant = new ProductVariantModel()
                                {
                                    ID = item.Product.ProductVariant.ProductVariantID,
                                    SKU = item.Product.ProductVariant.ProductVariantSKU,
                                    Options = item.Product.ProductVariant.ProductVariantOptions,
                                    Pallet = new PalletModel()
                                    {
                                        PalletCode = item.Product.ProductVariant.Pallet.PalletCode
                                    }
                                },
                            },
                            Quantity = item.Quantity,
                            Price = item.Price,
                            TotalAmount = item.TotalAmount,
                        });
                    }

                    model.Add(new PrintInventoryOutputTypeViewModel()
                    {
                        Inventory = new InventoryModel()
                        {
                            ID = itemData.InventoryOutputID,
                            InventoryCode = itemData.InventoryOutputCode,
                            ReceiverName = itemData.ReceiverName,
                            WareHouse = new WareHouseModel()
                            {
                                WareHouseName = itemData.WareHouse.WareHouseName,
                                FullAddress = itemData.WareHouse.FullAddress
                            },
                            Shop = new ShopModel()
                            {
                                ID = req.ShopID
                            },
                            TotalAmount = itemData.TotalAmount,
                            TotalAmountByText = itemData.TotalAmountByText
                        },
                        ListInventoryOutputDetail = ListInventoryDetail
                    });
                }
            }
            return PartialView(model);
        }
        #endregion

        #region TEMP
        //public JsonResult JAddDetail(InventoryImportDetailAddModel data)//--- J_Thêm chi tiết
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XCommon.InventoryDetailAdd(new ZInventoryDetailAddRequest()
        //        {
        //            InventoryID = data.InventoryID,
        //            ProductVariantID = data.ProductVariantID,
        //            Quantity = data.Quantity,
        //            Price = data.Price,
        //            CreatedBy = XHelper.Session.EmployeeProfile.ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JAddDetail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult DetailByInventoryID(int InventoryID)//--- Lấy các biến thể trong phiếu
        //{
        //    List<InventoryOutputDetailByInventoryIDViewModel> model = new List<InventoryOutputDetailByInventoryIDViewModel>();
        //    try
        //    {
        //        var list = XCommon.InventoryDetailGetDataByInventoryID(new ZInventoryDetailGetByInventoryIDRequest()
        //        {
        //            InventoryID = InventoryID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //        long TotalAmount = 0;
        //        long IntoMoney = 0;
        //        foreach (var item in list)
        //        {
        //            IntoMoney = item.Quantity * item.Price;
        //            TotalAmount += IntoMoney;
        //            model.Add(new InventoryOutputDetailByInventoryIDViewModel()
        //            {
        //                ID = item.ID,
        //                ProductVariant = item.ProductVariant,
        //                Quantity = item.Quantity,
        //                Price = item.Price,
        //                IntoMoney = IntoMoney,
        //                WarehouseQuantity = item.WarehouseQuantity
        //            });
        //        }
        //        ViewBag.TotalAmount = TotalAmount;
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.DetailByInventoryID: " + ex.Message);
        //    }
        //    return PartialView(model);
        //}

        //public JsonResult JEditDetail(InventoryEditDetailModel data)//--- J_Chỉnh sửa số lượng, giá trong chi tiết
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XCommon.InventoryDetailEdit(new ZInventoryDetailEditRequest()
        //        {
        //            ID = data.DetailID,
        //            InventoryID = data.InventoryID,
        //            Quantity = data.Quantity,
        //            UpdatedBy = XHelper.Session.EmployeeProfile.ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JEditDetail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult JRemoveDetail(InventoryRemoveDetailModel data)//--- J_Xoá biến thể trong chi tiết phiếu
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XCommon.InventoryDetailRemove(new ZInventoryDetailRemoveRequest()
        //        {
        //            ID = data.DetailID,
        //            InventoryID = data.InventoryID,
        //            UpdatedBy = XHelper.Session.EmployeeProfile.ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputController.JRemoveDetail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        #endregion
    }
}