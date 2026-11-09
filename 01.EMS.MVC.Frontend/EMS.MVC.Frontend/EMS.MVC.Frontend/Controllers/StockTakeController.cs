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
    public class StockTakeController : BaseController
    {
        #region Danh sách phiếu kiểm kho
        [AuthorizeController(FunctionalTitle = "DANH_SACH_PHIEU_KIEM_KHO")]
        public ActionResult StockTakeView()
        {
            StockTakeViewModel model = new StockTakeViewModel()
            {                
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListStockTakeStatus = XData.GetListStockTakeStatus(),
                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock ="N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListDateType = new List<SelectListItem>()
                {
                    new SelectListItem(){ Value = "CREATED_DATE", Text = "Thời gian tạo", Selected = true },
                    new SelectListItem(){ Value = "UPDATED_DATE", Text = "Thời gian cập nhật" }
                },
            };
            return View(model);
        }

        public ActionResult SearchListStockTakeView(StockTakeViewModel req)
        {
            List<SearchListStockTakeViewModel> List = new List<SearchListStockTakeViewModel>();
            var res = StockTakeRepository.SearchListStockTake(new ZStockTakeSearchListRequest()
            {
                StockTakeCode = req.StockTakeCode,
                DateType = req.DateType,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                Status = req.Status,
                WareHouseID = req.WareHouseID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZStockTakeSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    List.Add(new SearchListStockTakeViewModel()
                    {
                        StockTakeID = item.StockTakeID,
                        StockTakeCode = item.StockTakeCode,
                        WareHouseName = item.WareHouseName,
                        TotalProduct = item.TotalProduct,
                        TotalInventory = item.TotalInventory,
                        TotalActual = item.TotalActual,
                        TotalDifferent = item.TotalDifferent,
                        TotalMoneyDifferent = item.TotalMoneyDifferent,
                        Status = new BaseModel() 
                        {
                            Code = item.Status.Code,
                            Name = item.Status.Name
                        },
                        CreatedName = item.CreatedName,
                        CreatedDate = item.CreatedDate,
                        UpdatedName = item.UpdatedName,
                        UpdatedDate = item.UpdatedDate,
                    });
                }    
            }    
            return PartialView(List);
        }
        #endregion

        #region Thêm phiếu kiểm kho
        [AuthorizeController(FunctionalTitle = "THEM_PHIEU_KIEM_KHO")]
        public ActionResult AddStockTakeView()
        {
            AddStockTakeViewModel model = new AddStockTakeViewModel();
            model.ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            model.WareHouseID = model.ListWareHouse.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListWareHouse.Where(m => m.IsDefault == "Y").First().ID : 0;
            return View(model);
        }

        public ActionResult AddDetailStockTakeView(AddStockTakeViewModel req)
        {
            AddDetailStockTakeViewModel model = new AddDetailStockTakeViewModel() 
            {
               NumberOfDetail = req.NumberOfDetail,
               ListProductVariant = XCommon.SearchListProductVariantByWareHouseID(new ZProductVariantGetByWareHouseIDRequest() { WareHouseID = req.WareHouseID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
            };
            return PartialView(model);
        }

        public JsonResult JAddStockTake(AddStockTakeViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZStockTakeDetailAddRequest> ListDetailStockTake = new List<ZStockTakeDetailAddRequest>();
                foreach (var item in req.ListDetailStockTake)
                {
                    ListDetailStockTake.Add(new ZStockTakeDetailAddRequest() 
                    {
                        ProductVariantID = item.ProductVariantID,
                        Quantity = item.Quantity,
                    });
                }

                res = StockTakeRepository.AddStockTake(new ZStockTakeAddRequest() 
                {
                    WareHouseID = req.WareHouseID,
                    Note = req.Note,
                    Status = req.CreateType,
                    ListDetailStockTake = ListDetailStockTake,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.JAddStockTake: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa phiếu kiểm kho
        [AuthorizeController(FunctionalTitle = "SUA_PHIEU_KIEM_KHO")]
        public ActionResult EditStockTakeView(SearchListStockTakeViewModel req)
        {
            EditStockTakeViewModel model = new EditStockTakeViewModel();
            var StockTakeID = Convert.ToInt32(Security.DecodeBase64(req.SearchKey));
            var res = StockTakeRepository.SearchByStockTakeID(new ZStockTakeGetByIDRequest()
            {
                ID = StockTakeID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZStockTakeGetByIDResponse>(res.Data);
                List<StockTakeDetailModel> ListDetailStockTake = new List<StockTakeDetailModel>();
                foreach (var itemStockTakeDetail in item.ListStockTakeDetail)
                {
                    ListDetailStockTake.Add(new StockTakeDetailModel()
                    {
                        Product = new ProductModel() { ProductName = itemStockTakeDetail.Product.ProductName },
                        ProductVariant = new ProductVariantModel()
                        {
                            ID = itemStockTakeDetail.ProductVariant.ProductVariantID,
                            Options = itemStockTakeDetail.ProductVariant.ProductVariantOptions,
                            SKU = itemStockTakeDetail.ProductVariant.ProductVariantSKU,
                            Price = itemStockTakeDetail.ProductVariant.ProductVariantPrice
                        },
                        NumberOfInventory = itemStockTakeDetail.NumberOfInventory,
                        NumberOfActual = itemStockTakeDetail.NumberOfActual,
                        NumberOfDifferent = itemStockTakeDetail.NumberOfDifferent,
                        MoneyOfDifferent = itemStockTakeDetail.MoneyOfDifferent,
                    });
                }
                model = new EditStockTakeViewModel()
                {
                    StockTakeID = item.StockTakeID,
                    StockTakeCode = item.StockTakeCode,
                    WareHouseID = item.WareHouse.WareHouseID,
                    ListWareHouse = new List<WareHouseModel>() 
                    {
                        new WareHouseModel(){ ID = item.WareHouse.WareHouseID, WareHouseName = item.WareHouse.WareHouseName }
                    },
                    Note = item.Note,
                    Status = new BaseModel()
                    {
                        Code = item.Status.Code,
                        Name = item.Status.Name
                    },
                    NumberOfDetail = ListDetailStockTake.Count(),
                    ListDetailStockTake = ListDetailStockTake,
                    TotalInventory = ListDetailStockTake.Sum(m => m.NumberOfInventory),
                    TotalActual = ListDetailStockTake.Sum(m => m.NumberOfActual),
                    TotalDifferent = ListDetailStockTake.Sum(m => m.NumberOfDifferent),
                    TotalMoney = ListDetailStockTake.Sum(m => m.MoneyOfDifferent)
                };
            }
            return View(model);
        }

        public ActionResult EditDetailStockTakeView(EditStockTakeViewModel req)
        {
            EditDetailStockTakeViewModel model = new EditDetailStockTakeViewModel() 
            {
                NumberOfDetail = req.NumberOfDetail,
                ListDetailStockTake = req.ListDetailStockTake == null ? new List<StockTakeDetailModel>() : req.ListDetailStockTake,
                ListProductVariant = XCommon.SearchListProductVariantByWareHouseID(new ZProductVariantGetByWareHouseIDRequest() { WareHouseID = req.WareHouseID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
            };
            return PartialView(model);
        }


        public JsonResult JEditStockTake(EditStockTakeViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZStockTakeDetailEditRequest> ListDetailStockTake = new List<ZStockTakeDetailEditRequest>();
                foreach(var item in req.ListDetailStockTake)
                {
                    ListDetailStockTake.Add(new ZStockTakeDetailEditRequest() 
                    {
                        ProductVariantID = item.ProductVariantID,
                        Quantity = item.Quantity
                    });
                }

                res = StockTakeRepository.EditStockTake(new ZStockTakeEditRequest() 
                {
                    StockTakeID = req.StockTakeID,
                    Note = req.Note,
                    Status = req.EditType,
                    ListDetailStockTake = ListDetailStockTake,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.JEditStockTake: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Chi tiết phiếu kiểm kho
        [AuthorizeController(FunctionalTitle = "CHI_TIET_PHIEU_KIEM_KHO")]
        public ActionResult DetailStockTakeView(SearchListStockTakeViewModel req)
        {
            DetailStockTakeViewModel model = null;
            var StockTakeID = Convert.ToInt32(Security.DecodeBase64(req.SearchKey));
            var res = StockTakeRepository.SearchByStockTakeID(new ZStockTakeGetByIDRequest()
            {
                ID = StockTakeID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZStockTakeGetByIDResponse>(res.Data);
                List<StockTakeDetailModel> ListStockTakeDetail = new List<StockTakeDetailModel>();
                foreach(var itemStockTakeDetail in item.ListStockTakeDetail)
                {
                    ListStockTakeDetail.Add(new StockTakeDetailModel() 
                    {
                        Product = new ProductModel() { ProductName = itemStockTakeDetail.Product.ProductName },
                        ProductVariant = new ProductVariantModel() 
                        {
                            SKU = itemStockTakeDetail.ProductVariant.ProductVariantSKU,
                            Options = itemStockTakeDetail.ProductVariant.ProductVariantOptions
                        },
                        NumberOfInventory = itemStockTakeDetail.NumberOfInventory,
                        NumberOfActual = itemStockTakeDetail.NumberOfActual,
                        NumberOfDifferent = itemStockTakeDetail.NumberOfDifferent,
                        MoneyOfDifferent = itemStockTakeDetail.MoneyOfDifferent,
                    });
                }

                model = new DetailStockTakeViewModel()
                {
                    StockTakeID = item.StockTakeID,
                    StockTakeCode = item.StockTakeCode,
                    WareHouse = new WareHouseModel() 
                    {
                        WareHouseName = item.WareHouse.WareHouseName
                    },
                    Note = item.Note,
                    Status = new BaseModel() 
                    {
                        Code = item.Status.Code,
                        Name = item.Status.Name,
                    },
                    CreatedName = item.CreatedName,
                    CreatedDate = item.CreatedDate,
                    UpdatedName = item.UpdatedName,
                    UpdatedDate = item.UpdatedDate,
                    ListStockTakeDetail = ListStockTakeDetail,
                    TotalProduct = ListStockTakeDetail.Count(),
                    TotalInventory = ListStockTakeDetail.Sum(m => m.NumberOfInventory),
                    TotalActual = ListStockTakeDetail.Sum(m => m.NumberOfActual),
                    TotalDifferent = ListStockTakeDetail.Sum(m => m.NumberOfDifferent),
                    TotalMoney = ListStockTakeDetail.Sum(m => m.MoneyOfDifferent)
                };
            }
            return View(model);
        }
        #endregion

        #region Duyệt phiếu kiểm kho
        [AuthorizeController(FunctionalTitle = "DUYET_PHIEU_KIEM_KHO")]
        public JsonResult JAcceptStockTake(ZStockTakeAcceptRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = StockTakeRepository.AcceptStockTake(new ZStockTakeAcceptRequest() 
                {
                    ID = req.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.JAcceptStockTake: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Từ chối phiếu kiểm kho
        [AuthorizeController(FunctionalTitle = "TU_CHOI_PHIEU_KIEM_KHO")]
        public JsonResult JRejectStockTake(ZStockTakeRejectRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = StockTakeRepository.RejectStockTake(new ZStockTakeRejectRequest() 
                {
                    ID = req.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.JRejectStockTake: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa phiếu kiểm kho
        [AuthorizeController(FunctionalTitle = "XOA_PHIEU_KIEM_KHO")]
        public JsonResult JRemoveStockTake(ZStockTakeRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = StockTakeRepository.RemoveStockTake(new ZStockTakeRemoveRequest() 
                {
                    ID = req.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeController.JRemoveStockTake: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}