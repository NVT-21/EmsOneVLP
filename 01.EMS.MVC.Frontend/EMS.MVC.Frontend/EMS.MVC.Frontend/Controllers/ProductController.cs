using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
namespace EMS.MVC.Frontend.Controllers
{
    public class ProductController : BaseController
    {
        #region Danh sách sản phẩm
        [AuthorizeController(FunctionalTitle = "DANH_SACH_SAN_PHAM")]
        public ActionResult ProductView()
        {
            return View();
        }
        public ActionResult SearchListProductView(ProductViewModel req)
        {
            List<SearchListProductViewModel> listItem = new List<SearchListProductViewModel>();
            var res = ProductRepository.SearchListProduct(new ZProductListRequest()
            {
                Code = req.Code,
                Name = req.Name,
                ProviderID = req.ProviderID,
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZProductListResponse>>(res.Data);
                foreach (var item in list)
                {
                    listItem.Add(new SearchListProductViewModel()
                    {
                        ProductID = item.ID,
                        ProductGroupName = item.ProductGroupName,
                        ProductName = item.ProductName,
                        ProductCode = item.Code,
                        Avatar = XHelper.pathImages + item.Avatar,
                        CreatedBy = item.CreatedByFullName,
                        CreatedDate = item.CreatedDate,
                        NumberOfVariant = item.VariantNumber,
                        NumberOfProduct = item.Quantity
                    });
                }
            }
            return PartialView(listItem);
        }
        #endregion

        #region Thêm sản phẩm
        [AuthorizeController(FunctionalTitle = "THEM_SAN_PHAM")]
        public ActionResult AddProductView()
        {
            AddProductViewModel model = new AddProductViewModel()
            {
                ListProductUnit = XCommon.SearchListProductUnit(new ZProductUnitSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID, IsLock = "N" }),
                ListProductGroup = XCommon.SearchListProductGroup(new ZProductGroupListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
            };
            return View(model);
        }
        public JsonResult JAddProduct(AddProductViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZProductVariantAddRequest> ListVariant = new List<ZProductVariantAddRequest>();
                if (req.ListVariant != null)
                {
                    foreach (var item in req.ListVariant)
                    {
                        ListVariant.Add(new ZProductVariantAddRequest()
                        {
                            Options = item.Options,
                            OptionName1 = item.OptionName1,
                            OptionValue1 = item.OptionValue1,
                            OptionName2 = item.OptionName2,
                            OptionValue2 = item.OptionValue2,
                            OptionName3 = item.OptionName3,
                            OptionValue3 = item.OptionValue3,
                        });
                    }
                }

                res = ProductRepository.AddProduct(new ZProductAddRequest()
                {
                    ProductGroupID = req.ProductGroupID,
                    Code = req.Code,
                    Name = req.Name,
                    Price = req.Price,
                    Size = req.Size,
                    Weight = req.Weight,
                    Description = req.Description,
                    Images = req.Images,
                    Avatar = req.Avatar,
                    Variants = ListVariant,
                    Unit = req.Unit,
                    BatchNumber = req.BatchNumber,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JAddProduct: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm sản phẩm vào nhóm sản phẩm
        public ActionResult AddProductToGroupView(string ListProductID)
        {
            AddProductToGroupViewModel model = new AddProductToGroupViewModel()
            {
                ListProductID = ListProductID,
                ListProductGroup = XCommon.SearchListProductGroup(new ZProductGroupListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
            };
            return PartialView(model);
        }

        public JsonResult JAddProductToGroup(AddProductToGroupViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ProductRepository.AddProductToProductGroup(new ZProductUpdateProductGroupRequest()
                {
                    IDs = req.ListProductID,
                    ProductGroupID = req.ProductGroupID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JAddProductToGroup: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Sửa sản phẩm
        [AuthorizeController(FunctionalTitle = "CHI_TIET_SAN_PHAM")]
        public ActionResult EditProductView(ProductGetByIDModel req)
        {
            EditProductViewModel model = new EditProductViewModel();
            var res = ProductRepository.GetByProductID(new ZProductGetByIDRequest()
            {
                ProductID = req.ProductID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
            });

            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZProductGetByIDResponse>(res.Data);
                double Length = 0;
                double Width = 0;
                double Height = 0;
                if (item.Size != "")
                {
                    var arr = item.Size.Split('D');
                    //Lấy chiều dài
                    if (arr.Length > 1)
                    {
                        Length = Convert.ToDouble(arr[0]);
                        arr = arr[1].Split('R');
                    }
                    else
                    {
                        arr = arr[0].Split('R');
                    }
                    //Lấy chiều rộng
                    if (arr.Length > 1)
                    {
                        Width = Convert.ToDouble(arr[0]);
                        arr = arr[1].Split('C');
                    }
                    else
                    {
                        arr = arr[0].Split('C');
                    }
                    //Lấy chiều cao
                    if (arr.Length > 1)
                    {
                        Height = Convert.ToDouble(arr[0]);
                    }
                }

                model = new EditProductViewModel()
                {
                    ID = item.ID,
                    Code = item.Code,
                    Name = item.Name,
                    Avatar = item.Avatar,
                    ProductGroupID = item.ProductGroupID,
                    ListProductGroup = XCommon.SearchListProductGroup(new ZProductGroupListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                    PricesSell = item.PricesSell,
                    Weight = item.Weight,
                    Length = Length,
                    Width = Width,
                    Height = Height,
                    Description = item.Description,
                    CreatedDate = item.CreatedDate,
                    CreatedByFullName = item.CreatedByFullName,
                    Unit = item.Unit,
                    ListProductUnit = XCommon.SearchListProductUnit(new ZProductUnitSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID, IsLock = "N" }),
                };
            }
            return View(model);
        }

        [AuthorizeController(FunctionalTitle = "SUA_SAN_PHAM")]
        public JsonResult JEditProduct(EditProductViewModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                string Size = "";
                if (data.Length != 0)
                {
                    Size += data.Length + "D";
                }
                if (data.Width != 0)
                {
                    Size += data.Width + "R";
                }
                if (data.Height != 0)
                {
                    Size += data.Height + "C";
                }

                res = ProductRepository.EditProduct(new ZProductEditRequest()
                {
                    ID = data.ID,
                    Name = data.Name,
                    Code = data.Code,
                    ProductGroupID = data.ProductGroupID,
                    PricesSell = data.PricesSell,
                    Size = Size,
                    Weight = data.Weight,
                    Unit = data.Unit,
                    Description = data.Description,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JEditProduct: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm sản phẩm bằng Excel
        public ActionResult ImportExcelView()
        {
            ImportExcelViewModel model = new ImportExcelViewModel();
            model.License = XHelper.JExcelLicense;

            var ListProductGroup = XCommon.SearchListProductGroup(new ZProductGroupListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            foreach (var item in ListProductGroup)
            {
                model.ListProductGroup.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.Code
                });
            }

            var ListProductUnit = XCommon.SearchListProductUnit(new ZProductUnitSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID, IsLock = "N" });
            foreach (var item in ListProductUnit)
            {
                model.ListProductUnit.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.Name
                });
            }

            var ListWareHosue = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            foreach (var item in ListWareHosue)
            {
                model.ListWareHouse.Add(new JSpreadSheetModel()
                {
                    id = item.ID.ToString(),
                    name = item.WareHouseCode
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
                var ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });

                //Loop the records upto filled row
                for (int row = 1; row <= total; row++)
                {
                    var messageError = "";
                    if (sheet.GetRow(row) != null)
                    {
                        var ProductGroup = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(1));
                        var ProductCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(2));
                        var ProductName = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(3));
                        var Description = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(4));
                        var Weight = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(5));
                        var Price = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(6));
                        var AttributeName1 = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(7));
                        var AttributeValue1 = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(8));
                        var AttributeName2 = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(9));
                        var AttributeValue2 = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(10));
                        var AttributeName3 = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(11));
                        var AttributeValue3 = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(12));
                        var ImageLink = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(13));
                        var Long = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(14));
                        var Width = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(15));
                        var Height = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(16));
                        var Unit = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(17));
                        var BatchNumber = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(18));
                        var WareHouse = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(19));
                        var NumberOfProduct = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(20));
                        var PalletCode = XCommon.CheckStyleColumnExcel(sheet.GetRow(row).GetCell(21));

                        if (!string.IsNullOrEmpty(ProductCode) && !string.IsNullOrEmpty(ProductName))
                        {
                            if (!string.IsNullOrEmpty(WareHouse))
                            {
                                var checkWareHouse = ListWareHouse.Where(m => m.WareHouseCode.ToUpper() == WareHouse).ToList();
                                if (checkWareHouse.Count == 0)
                                {
                                    WareHouse = "";
                                    messageError = "Mã kho không đúng, vui lòng kiểm tra lại.";
                                }
                                else
                                {
                                    if (NumberOfProduct == "0" || NumberOfProduct == "")
                                    {
                                        messageError = "Vui lòng nhập số lượng nhập kho";
                                    }
                                }
                            }
                            else
                            {
                                if (NumberOfProduct != "0" && NumberOfProduct != "")
                                {
                                    messageError = "Vui lòng chọn kho cần nhập";
                                }
                            }

                            ArrayList data = new ArrayList();
                            data.Add("");
                            data.Add(ProductGroup);
                            data.Add(ProductCode);
                            data.Add(ProductName);
                            data.Add(Description);
                            data.Add(Weight);
                            data.Add(Price);
                            data.Add(AttributeName1);
                            data.Add(AttributeValue1);
                            data.Add(AttributeName2);
                            data.Add(AttributeValue2);
                            data.Add(AttributeName3);
                            data.Add(AttributeValue3);
                            data.Add(ImageLink);
                            data.Add(Long);
                            data.Add(Width);
                            data.Add(Height);
                            data.Add(Unit);
                            data.Add(BatchNumber);
                            data.Add(WareHouse);
                            data.Add(NumberOfProduct);
                            data.Add(PalletCode);
                            data.Add(messageError);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JImportExcelData: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JImportExcel(List<ImportExcelProductModel> data, int countKHL, int countLoi)
        {
            ZResponse res = new ZResponse();
            try
            {
                var totalData = data.Count;
                if (totalData > 1000)
                {
                    res.Code = "02";
                    res.Message = "Hệ thống chỉ hỗ trợ import tối đa 1000 dòng";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                var countSuccess = 0;
                var countError = countLoi;

                List<ArrayList> lstError = new List<ArrayList>();

                foreach (var item in data)
                {
                    List<ZProductImportExcelProductRequest> ListProduct = new List<ZProductImportExcelProductRequest>();
                    foreach (var itemListProduct in item.ListProduct)
                    {
                        //Ảnh
                        string FileName = "image-default/image.png";
                        Task ImageHandle = new Task(() =>
                        {
                            if (!string.IsNullOrEmpty(itemListProduct.Images))
                            {
                                var resImg = XCommon.ExcelUploadImage(itemListProduct.Images);
                                if (resImg.Code == "00")
                                {
                                    var resImgData = JsonConvert.DeserializeObject<List<FileModel>>(resImg.Data);
                                    FileName = resImgData[0].FileName;
                                }
                            }
                        });

                        ImageHandle.Start();

                        List<ZProductVariantAddRequest> ListProductVariant = new List<ZProductVariantAddRequest>();
                        foreach (var itemListProductVariant in itemListProduct.ListProductVariant)
                        {
                            ListProductVariant.Add(new ZProductVariantAddRequest()
                            {
                                Options = itemListProductVariant.Options,
                                OptionName1 = itemListProductVariant.OptionName1,
                                OptionValue1 = itemListProductVariant.OptionValue1,
                                OptionName2 = itemListProductVariant.OptionName2,
                                OptionValue2 = itemListProductVariant.OptionValue2,
                                OptionName3 = itemListProductVariant.OptionName3,
                                OptionValue3 = itemListProductVariant.OptionValue3,
                                BatchNumber = itemListProductVariant.BatchNumber,
                            });
                        }

                        Task.WaitAll(ImageHandle);

                        ListProduct.Add(new ZProductImportExcelProductRequest()
                        {
                            ProductGroupID = itemListProduct.ProductGroup.ID,
                            Code = itemListProduct.Code,
                            Name = itemListProduct.Name,
                            Description = itemListProduct.Description,
                            Weight = itemListProduct.Weight,
                            Price = itemListProduct.Price,
                            Size = itemListProduct.Size,
                            Images = FileName,
                            ListProductVariant = ListProductVariant,
                            ProductUnitID = itemListProduct.ProductUnit.ID,
                            NumberOfProduct = itemListProduct.NumberOfProduct,
                            PalletCode = itemListProduct.PalletCode
                        });
                    }

                    var resAddProduct = ProductRepository.ImportExcel(new ZProductImportExcelRequest()
                    {
                        WareHouseID = item.WareHouse.ID,
                        ListProduct = ListProduct,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        CreatedBy = XHelper.Session.EmployeeProfile.ID
                    });

                    if (resAddProduct.Code == "00")
                    {
                        countSuccess++;
                    }
                    else
                    {
                        foreach (var itemProductError in item.ListProduct)
                        {
                            ArrayList errorItem = new ArrayList();
                            errorItem.Add("Lỗi");
                            errorItem.Add(itemProductError.ProductGroup.Name);
                            errorItem.Add(itemProductError.Code);
                            errorItem.Add(itemProductError.Name);
                            errorItem.Add(itemProductError.Description);
                            errorItem.Add(itemProductError.Weight);
                            errorItem.Add(itemProductError.Price);
                            errorItem.Add(itemProductError.Option1.Code);
                            errorItem.Add(itemProductError.Option1.Name);
                            errorItem.Add(itemProductError.Option2.Code);
                            errorItem.Add(itemProductError.Option2.Name);
                            errorItem.Add(itemProductError.Option3.Code);
                            errorItem.Add(itemProductError.Option3.Name);
                            errorItem.Add(itemProductError.Images);
                            errorItem.Add(itemProductError.Long);
                            errorItem.Add(itemProductError.Width);
                            errorItem.Add(itemProductError.Height);
                            errorItem.Add(itemProductError.ProductUnit.Name);
                            errorItem.Add(itemProductError.BatchNumber);
                            errorItem.Add(item.WareHouse.WareHouseName);
                            errorItem.Add(itemProductError.NumberOfProduct);
                            errorItem.Add(itemProductError.PalletCode);
                            errorItem.Add(resAddProduct.Message);
                            lstError.Add(errorItem);
                            countError++;
                        }
                    }
                }

                if (totalData == countSuccess)
                {
                    res.Code = "00";
                    res.Message = string.Format("<div class='text-green mb-2'>Sản phẩm tạo thành công: {0}</div><div class='text-warning mb-2'>Sản phẩm không hợp lệ: {1}</div><div class='text-danger mb-3'>Sản phẩm lỗi: {2}</div>", countSuccess, countKHL, countError);
                }
                else
                {
                    res.Code = "10";
                    res.Message = string.Format("<div class='text-green mb-2'>Sản phẩm tạo thành công: {0}</div><div class='text-warning mb-2'>Sản phẩm không hợp lệ: {1}</div><div class='text-danger mb-3'>Sản phẩm lỗi: {2}</div>", countSuccess, countKHL, countError);
                    res.Data = JsonConvert.SerializeObject(lstError);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JImportExcelData: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //public ArrayList JImportExcelError(ImportExcelProductModel data, string Message)
        //{
        //    ArrayList model = new ArrayList();
        //    model.Add("Lỗi");
        //    model.Add(data.ProductGroup.Name);
        //    model.Add(data.Code);
        //    model.Add(data.Name);
        //    model.Add(data.Description);
        //    model.Add(data.Weight);
        //    model.Add(data.Price);
        //    model.Add(data.Option1.Code);
        //    model.Add(data.Option1.Name);
        //    model.Add(data.Option2.Code);
        //    model.Add(data.Option2.Name);
        //    model.Add(data.Option3.Code);
        //    model.Add(data.Option3.Name);
        //    model.Add(data.Images);
        //    model.Add(data.Long);
        //    model.Add(data.Width);
        //    model.Add(data.Height);
        //    model.Add(data.ProductUnit.Name);
        //    model.Add(data.BatchNumber);
        //    model.Add(data.WareHouse.WareHouseName);
        //    model.Add(data.NumberOfProduct);
        //    model.Add(Message);
        //    return model;
        //}

        public ActionResult JExportExcel(List<ExportExcelProductModel> ListData)
        {
            var FullPath = System.Web.HttpContext.Current.Server.MapPath("~/Assets/temp/MauImportSanPham.xlsm");
            var fileNameNew = Guid.NewGuid().ToString() + ".xlsm";
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

                    sheet.GetRow(0).CreateCell(22).SetCellValue("Nguyên nhân lỗi");
                    sheet.GetRow(0).GetCell(22).CellStyle = borderedCellStyle;

                    foreach (var item in ListData)
                    {
                        // tao row mới
                        var newRow = sheet.CreateRow(rowIndex);
                        // set giá trị
                        newRow.CreateCell(0).SetCellValue(stt++);
                        newRow.CreateCell(1).SetCellValue(item.ProductGroupName);
                        newRow.CreateCell(2).SetCellValue(item.Code);
                        newRow.CreateCell(3).SetCellValue(item.Name);
                        newRow.CreateCell(4).SetCellValue(item.Description);
                        newRow.CreateCell(5).SetCellValue(item.Weight);
                        newRow.CreateCell(6).SetCellValue(item.Price);
                        newRow.CreateCell(7).SetCellValue(item.OptionName1);
                        newRow.CreateCell(8).SetCellValue(item.OptionValue1);
                        newRow.CreateCell(9).SetCellValue(item.OptionName2);
                        newRow.CreateCell(10).SetCellValue(item.OptionValue2);
                        newRow.CreateCell(11).SetCellValue(item.OptionName3);
                        newRow.CreateCell(12).SetCellValue(item.OptionValue3);
                        newRow.CreateCell(13).SetCellValue(item.Images);
                        newRow.CreateCell(14).SetCellValue(item.Long);
                        newRow.CreateCell(15).SetCellValue(item.Width);
                        newRow.CreateCell(16).SetCellValue(item.Height);
                        newRow.CreateCell(17).SetCellValue(item.ProductUnitName);
                        newRow.CreateCell(18).SetCellValue(item.BatchNumber);
                        newRow.CreateCell(19).SetCellValue(item.WareHouseName);
                        newRow.CreateCell(20).SetCellValue(item.NumberOfProduct);
                        newRow.CreateCell(21).SetCellValue(item.PalletCode);
                        newRow.CreateCell(22).SetCellValue(item.MessageError);

                        for (int i = 0; i <= 21; i++)
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JExportExcel: " + ex.Message);
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
             "MauImportSanPham");
        }

        #endregion

        #region Product

        public ActionResult ImagesView(ProductDataImagesByIDModel req, string Page)
        {
            List<ProductImageViewModel> model = new List<ProductImageViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PRODUCT_DATA_IMAGES_BY_ID",
                Data = JsonConvert.SerializeObject(new ZProductDataImagesByIDRequest()
                {
                    ProductID = req.ProductID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                })
            });
            if (res.Code == "00")
            {
                var data = JsonConvert.DeserializeObject<ZProductDataImagesByIDResponse>(res.Data);
                if (data.Images != null)
                {
                    var list = data.Images.Split(';');
                    foreach (var item in list)
                    {
                        if (item != "")
                        {
                            model.Add(new ProductImageViewModel()
                            {
                                Image = item
                            });
                        }
                    }
                }
            }
            ViewBag.Page = Page;
            ViewBag.List = XData.GetListAccountType();
            return View(model);
        }

        public JsonResult JEditImages(ProductEditImagesModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_EDIT_IMAGES",
                    Data = JsonConvert.SerializeObject(new ZProductEditImagesRequest()
                    {
                        ProductID = data.ProductID,
                        Images = data.Images,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        CreatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JEditImages: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductOptionDataByProductID(int ProductID)
        {
            ProductOptionDataByProductIDViewModel model = new ProductOptionDataByProductIDViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PRODUCT_OPTION_GET_BY_PRODUCT_ID",
                Data = JsonConvert.SerializeObject(new ZProductOptionGetByProductIDRequest()
                {
                    ProductID = ProductID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                })
            });
            if (res.Code == "00")
            {
                var data = JsonConvert.DeserializeObject<ZProductOptionGetByProductIDResponse>(res.Data);
                model = new ProductOptionDataByProductIDViewModel()
                {
                    Option01 = data.Option01,
                    Option02 = data.Option02,
                    Option03 = data.Option03
                };
            }
            return View(model);
        }
        public JsonResult JProductOptionAdd(ProductOptionAddModel data)
        {
            ZResponse res = new ZResponse();
            res = XCommon.CheckPermission("Product", "EditProduct");
            if (res.Code == "00")
            {
                try
                {
                    res = XHelper.Invoke(new ZRequest()
                    {
                        Code = "PRODUCT_OPTION_ADD",
                        Data = JsonConvert.SerializeObject(new ZProductOptionAddRequest()
                        {
                            ProductID = data.ProductID,
                            Name = data.Name,
                            Value = data.Value,
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                            CreatedBy = XHelper.Session.EmployeeProfile.ID
                        })
                    });
                }
                catch (Exception ex)
                {
                    res.Code = "99";
                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JProductOptionAdd: " + ex.Message);
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JProductOptionRemoveByName(ProductOptionRemoveByNameModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_OPTION_REMOVE_BY_NAME",
                    Data = JsonConvert.SerializeObject(new ZProductOptionRemoveByNameRequest()
                    {
                        Name = data.Name,
                        ProductID = data.ProductID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JProductOptionRemoveByName: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductVariantDataByProductID(int ProductID, int VariantID)
        {
            ProductVariantDataByProductIDViewModel model = new ProductVariantDataByProductIDViewModel();
            List<ProductVariantModel> Variants = new List<ProductVariantModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PRODUCT_VARIANT_GET_BY_PRODUCT_ID",
                Data = JsonConvert.SerializeObject(new ZProductVariantGetByProductIDRequest()
                {
                    ProductID = ProductID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByProductIDResponse>>(res.Data);
                foreach (var item in list)
                {
                    Variants.Add(new ProductVariantModel()
                    {
                        ID = item.ID,
                        Options = item.Options,
                        Avatar = item.Avatar,
                        Price = item.Price
                    });
                }
            }
            model = new ProductVariantDataByProductIDViewModel()
            {
                ProductID = ProductID,
                Variants = Variants
            };
            return View(model);
        }
        #endregion

        #region Variant
        public ActionResult ProductVariantIndex(ProductVariantIndexViewModel data)
        {
            ProductVariantIndexViewModel model = null;
            var res = ProductRepository.GetByProductID(new ZProductGetByIDRequest()
            {
                ProductID = data.ProductID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZProductGetByIDResponse>(res.Data);
                model = new ProductVariantIndexViewModel()
                {
                    ProductID = item.ID,
                    ProductName = item.Name,
                    VariantID = data.VariantID,
                    Variants = XCommon.SearchListProductVariantByProductID(new ZProductVariantGetByProductIDRequest() { ProductID = data.ProductID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
                };
            }
            return View(model);
        }
        public ActionResult ProductVariantDataByID(int VariantID)
        {
            ProductVariantDataByIDViewModel model = new ProductVariantDataByIDViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PRODUCT_VARIANT_GET_BY_ID",
                Data = JsonConvert.SerializeObject(new ZProductVariantDataByIDRequest()
                {
                    ID = VariantID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZProductVariantDataByIDResponse>(res.Data);
                double Length = 0;
                double Width = 0;
                double Height = 0;
                if (item.Size != "")
                {
                    var arr = item.Size.Split('D');
                    //Lấy chiều dài
                    if (arr.Length > 1)
                    {
                        Length = Convert.ToDouble(arr[0]);
                        arr = arr[1].Split('R');
                    }
                    else
                    {
                        arr = arr[0].Split('R');
                    }
                    //Lấy chiều rộng
                    if (arr.Length > 1)
                    {
                        Width = Convert.ToDouble(arr[0]);
                        arr = arr[1].Split('C');
                    }
                    else
                    {
                        arr = arr[0].Split('C');
                    }
                    //Lấy chiều cao
                    if (arr.Length > 1)
                    {
                        Height = Convert.ToDouble(arr[0]);
                    }
                }

                model = new ProductVariantDataByIDViewModel()
                {
                    ID = item.ID,
                    ProductID = item.ProductID,
                    ProductName = item.ProductName,
                    Options = item.Options,
                    OptionName1 = item.OptionName1,
                    OptionValue1 = item.OptionValue1,
                    OptionName2 = item.OptionName2,
                    OptionValue2 = item.OptionValue2,
                    OptionName3 = item.OptionName3,
                    OptionValue3 = item.OptionValue3,
                    Avatar = item.Avatar,
                    Price = item.Price,
                    SKU = item.SKU,
                    Barcode = item.Barcode,
                    Weight = item.Weight,
                    Length = Length,
                    Width = Width,
                    Height = Height
                };
            }
            return View(model);
        }
        public JsonResult JRemoveVariant(int VariantID)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_VARIANT_REMOVE",
                    Data = JsonConvert.SerializeObject(new ZProductVariantRemoveRequest()
                    {
                        ID = VariantID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JRemoveVariant: " + ex.Message);
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            return jResult;
        }
        public ActionResult ProductVariantAdd(int ProductID)
        {
            ProductVariantAddViewModel model = new ProductVariantAddViewModel();
            List<ProductOptionNameModel> ProductOptions = new List<ProductOptionNameModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PRODUCT_OPTION_GET_NAME_BY_PRODUCT_ID",
                Data = JsonConvert.SerializeObject(new ZProductOptionGetNameByProductIDRequest()
                {
                    ProductID = ProductID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZProductOptionGetNameByProductIDResponse>>(res.Data);
                foreach (var item in list)
                {
                    ProductOptions.Add(new ProductOptionNameModel()
                    {
                        Name = item.Name
                    });
                }
                model.ProductID = ProductID;
                model.ProductOptions = ProductOptions;
            }
            return View(model);
        }
        public JsonResult JProductVariantAdd(ProductVariantAddModel data)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                var Size = ProductRepository.ConvertSize(data.Length, data.Width, data.Height);
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_VARIANT_ADD",
                    Data = JsonConvert.SerializeObject(new ZProductVariantAddRequest()
                    {
                        ProductID = data.ProductID,
                        Price = data.Price,
                        Size = Size,
                        Weight = data.Weight,
                        Avatar = data.Avatar,
                        SKU = data.SKU,
                        Barcode = data.BarCode,
                        Options = data.Options,
                        OptionName1 = data.OptionName1,
                        OptionValue1 = data.OptionValue1,
                        OptionName2 = data.OptionName2,
                        OptionValue2 = data.OptionValue2,
                        OptionName3 = data.OptionName3,
                        OptionValue3 = data.OptionValue3,
                        CreatedBy = XHelper.Session.EmployeeProfile.ID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.EditVariant: " + ex.Message);
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            return jResult;
        }
        public JsonResult JProductVariantEdit(ProductVariantEditModel data)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                string Size = "";
                if (data.Length != 0)
                {
                    Size += data.Length + "D";
                }
                if (data.Width != 0)
                {
                    Size += data.Width + "R";
                }
                if (data.Height != 0)
                {
                    Size += data.Height + "C";
                }
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_VARIANT_EDIT",
                    Data = JsonConvert.SerializeObject(new ZProductVariantEditRequest()
                    {
                        ID = data.ID,
                        Options = data.Options,
                        OptionName1 = data.OptionName1,
                        OptionValue1 = data.OptionValue1,
                        OptionName2 = data.OptionName2,
                        OptionValue2 = data.OptionValue2,
                        OptionName3 = data.OptionName3,
                        OptionValue3 = data.OptionValue3,
                        SKU = data.SKU,
                        Barcode = data.Barcode,
                        Avatar = data.Avatar,
                        Price = data.Price,
                        Weight = data.Weight,
                        Size = Size,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JProductVariantEdit: " + ex.Message);
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            return jResult;
        }
        #endregion

        #region Thêm đơn vị tính
        public JsonResult JAddProductUnit(ProductUnitAddModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ProductUnitRepository.AddProductUnit(new ZProductUnitAddRequest()
                {
                    UnitName = req.UnitName,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductController.JAddProductUnit: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}