using System.Collections.Generic;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using EMS.Entity;
using EMS.MVC.Frontend.Repositories;
using EMS.Library;
using System;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Controllers
{
    public class InventoryAdjustController: BaseController
    {
        #region Danh sách phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "DANH_SACH_PHIEU_DIEU_CHINH_KHO")]
        public ActionResult InventoryAdjustView()
        {
            InventoryAdjustViewModel model = new InventoryAdjustViewModel()
            {
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListInventoryAjustStatus = XData.GetListInventoryStatus()
            };
            return View(model);
        }

        public ActionResult SearchListInventoryAdjustView(InventoryAdjustViewModel req)
        {
            List<InventoryAdjustListViewModel> model = new List<InventoryAdjustListViewModel>();
            var res = InventoryRepository.SearchListInventory(new ZInventorySearchListRequest()
            {
                WarehouseID = req.WareHouseID,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                DateType = req.DateType,
                Status = req.Status,
                SearchKey = req.SearchKey,
                Type = "A",
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZInventorySearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new InventoryAdjustListViewModel()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Warehouse = item.Warehouse,
                        StockTake = item.StockTake,
                        Status = item.Status,
                        ImplementationDate = item.ImplementationDate,
                        Note = item.Note,
                        Reason = item.Reason,
                        CreaterName = item.CreaterName,
                        CreatedDate = item.CreatedDate,
                        UpdaterName = item.UpdaterName,
                        UpdatedDate = item.UpdatedDate
                    });
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Thêm phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "THEM_PHIEU_DIEU_CHINH_KHO")]
        public ActionResult AddInventoryAdjustView()
        {
            InventerAdjustAddViewModel model = new InventerAdjustAddViewModel()
            {
                WarehouseList = XCommon.GetListWareHouseSetDefault("Y", "N", XHelper.Session.EmployeeProfile.ShopInformation.ID),
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy")
            };
            return View(model);
        }

        public ActionResult AddInventoryAdjustDetailView (int Number)
        {
            ViewBag.Number = Number;
            return PartialView();
        }

        public ActionResult SearchStockTake(InventoryAdjustSearchStockTakeModel req)
        {
            List<InventoryAdjustSearchStockTakeViewModel> model = new List<InventoryAdjustSearchStockTakeViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "STOCK_TAKE_LIST",
                Data = JsonConvert.SerializeObject(new ZStockTakeSearchListRequest()
                {
                    StockTakeCode = req.StockTakeCode,
                    DateType = "CREATED_DATE", //Thời gian kiểm kho
                    FromDate = req.FromDate,
                    ToDate = req.ToDate,
                    Status = "A",
                    WareHouseID = req.WarehouseID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZStockTakeSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new InventoryAdjustSearchStockTakeViewModel()
                    {
                        ID = item.StockTakeID,
                        Code = item.StockTakeCode,
                        CreatedName = item.CreatedName,
                        CreatedDate = item.CreatedDate,
                        UpdatedName = item.UpdatedName,
                        UpdatedDate = item.UpdatedDate,
                    });
                }
            }
            return PartialView(model);
        }

        public JsonResult JAddInventoryAdjust(AddInventoryAdjustModel data)//--- J_Thêm 
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.AddInventory(new ZInventoryAddRequest()
                {
                    WarehouseID = data.WarehouseID,
                    ImplementationDate = data.ImplementationDate,
                    Note = data.Note,
                    StockTakeID = data.StockTakeID,
                    Type = "A",
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                if (res.Code == "00")
                {
                    var resData = JsonConvert.DeserializeObject<ZInventoryAddResponse>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.JAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Sửa thông tin phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "SUA_THONG_TIN_PHIEU_DIEU_CHINH_KHO")]
        public ActionResult EditInventoryAdjustView(int ID)
        {
            EditInventoryAdjustViewModel model = new EditInventoryAdjustViewModel();
            var res = InventoryRepository.InventoryGetByID(new ZInventoryGetByIDRequest
            {
                ID = ID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if(res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZInventoryGetByIDResponse>(res.Data);
                model = new EditInventoryAdjustViewModel()
                {
                    Warehouse = item.Warehouse,
                    WarehouseList = XCommon.GetListWareHouseSetDefault("Y", "N", XHelper.Session.EmployeeProfile.ShopInformation.ID),
                    ImplementationDate = item.ImplementationDate,
                    Note = item.Note,
                    StockTake = item.StockTake
                };
            }
            return View(model);
        }

        public JsonResult JEditInventoryAdjust(EditInventoryAdjustModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.EditInventory(new ZInventoryEditRequest()
                {
                    ID = data.ID,
                    WarehouseID = data.WarehouseID,
                    ImplementationDate = data.ImplementationDate,
                    Note = data.Note,
                    StockTakeID = data.StockTakeID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.JEditInventoryAdjust: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa chi tiết phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "SUA_CHI_TIET_PHIEU_DIEU_CHINH_KHO")]
        public ActionResult EditDetailInventoryAdjustView(int ID)
        {
            EditDetailInventoryAdjustViewModel model = new EditDetailInventoryAdjustViewModel();
            try
            {
                var res = InventoryRepository.InventoryGetByID(new ZInventoryGetByIDRequest
                {
                    ID = ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<ZInventoryGetByIDResponse>(res.Data);
                    model = new EditDetailInventoryAdjustViewModel()
                    {
                        InventoryID = item.ID,
                        InventoryCode = item.Code,
                        Warehouse = item.Warehouse,
                        Status = item.Status,
                        ImplementationDate = item.ImplementationDate,
                        Note = item.Note,
                        StockTake = item.StockTake,
                        CreaterName = item.CreaterName,
                        CreatedDate = item.CreatedDate,
                        ProductVariantList = XCommon.GetListProductVariant()
                    };
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.EditDetailInventoryAdjustView: " + ex.Message);
            }
            return View(model);
        }

        public JsonResult JEditDetailInventoryAdjust(InventoryEditDetailModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.InventoryDetailEdit(new ZInventoryDetailEditRequest()
                {
                    ID = data.DetailID,
                    InventoryID = data.InventoryID,
                    Quantity = data.Quantity,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.JEditDetailInventoryAdjust: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chi tiết phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "CHI_TIET_PHIEU_DIEU_CHINH_KHO")]
        public ActionResult DetailInventoryAdjustView(int ID)
        {
            DetailInventoryAdjustViewModel model = new DetailInventoryAdjustViewModel();
            List<InventoryAdjustDetailByInventoryIDViewModel> modelDetail = new List<InventoryAdjustDetailByInventoryIDViewModel>();
            try
            {
                //Lấy thông tin phiếu
                var res = InventoryRepository.InventoryGetByID(new ZInventoryGetByIDRequest
                {
                    ID = ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<ZInventoryGetByIDResponse>(res.Data);
                    model = new DetailInventoryAdjustViewModel()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Warehouse = item.Warehouse,
                        StockTake = item.StockTake,
                        Status = item.Status,
                        ImplementationDate = item.ImplementationDate,
                        Note = item.Note,
                        Reason = item.Reason
                    };
                }
                
                //Lấy chi tiết phiếu
                var list = XCommon.InventoryDetailGetDataByInventoryID(new ZInventoryDetailGetByInventoryIDRequest()
                {
                    InventoryID = ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                long QuantityAfterAdjustment = 0;
                if (model.Status.Code == "F")
                {
                    foreach (var itemDetail in list)
                    {
                        QuantityAfterAdjustment = itemDetail.InventoryQuantity + itemDetail.Quantity;
                        modelDetail.Add(new InventoryAdjustDetailByInventoryIDViewModel()
                        {
                            ProductVariant = itemDetail.ProductVariant,
                            WarehouseQuantity = itemDetail.InventoryQuantity,
                            StockTakeQuantity = itemDetail.StockTakeQuantity,
                            QuantityAdjustment = itemDetail.Quantity,
                            QuantityAfterAdjustment = QuantityAfterAdjustment
                        });
                    }
                }
                else
                {
                    foreach (var itemDetail in list)
                    {
                        QuantityAfterAdjustment = itemDetail.WarehouseQuantity + itemDetail.Quantity;
                        modelDetail.Add(new InventoryAdjustDetailByInventoryIDViewModel()
                        {
                            ProductVariant = itemDetail.ProductVariant,
                            WarehouseQuantity = itemDetail.WarehouseQuantity,
                            StockTakeQuantity = itemDetail.StockTakeQuantity,
                            QuantityAdjustment = itemDetail.Quantity,
                            QuantityAfterAdjustment = QuantityAfterAdjustment
                        });
                    }
                }
                model.Detail = modelDetail;
                ViewBag.ID = ID;
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.Detail: " + ex.Message);
            }
            return View(model);
        }

        public List<EditDetailInventoryAdjustProductVariantModel> GetProductVariantByID (int ID)
        {
            List<EditDetailInventoryAdjustProductVariantModel> model = new List<EditDetailInventoryAdjustProductVariantModel>();
            try
            {
                //var res = InventoryRepository.GetProductVariantByID(new ZInventoryGetProductVariantByIDRequest()
                //{
                //    ID = ID,
                //    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                //});
                //if (res.Code == "00")
                //{
                //    var ListItem = JsonConvert.DeserializeObject<List<ZInventoryGetProductVariantByIDResponse>>(res.Data);
                //    foreach (var item in ListItem)
                //    {
                //        model.Add(new EditDetailInventoryAdjustProductVariantModel()
                //        {
                //            ID = item.ProductVariantID,
                //            Name = item.ProductName + " (" + item.ProductVariantOptions + ")",
                //            NumberOfInventory = item.NumberOfInventory,
                //            NumberOfDifferent = item.NumberOfDifferent
                //        });
                //    }
                //}
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.GetProductVariantByID: " + ex.Message);
            }
            return model;
        }

        public ActionResult DetailByInventoryID(int InventoryID, int StockTakeID = 0) /*--- Lấy các biến thể trong phiếu*/
        {
            List<EditDetailInventoryAdjustDetailModel> model = new List<EditDetailInventoryAdjustDetailModel>();
            List<EditDetailInventoryAdjustProductVariantModel> VariantModel = new List<EditDetailInventoryAdjustProductVariantModel>();
            try
            {
                var list = InventoryRepository.InventoryDetailGetDataByInventoryID(new ZInventoryDetailGetByInventoryIDRequest()
                {
                    InventoryID = InventoryID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                long QuantityAfterAdjustment = 0;
                foreach (var item in list)
                {
                    QuantityAfterAdjustment = item.Quantity + item.WarehouseQuantity;
                    model.Add(new EditDetailInventoryAdjustDetailModel()
                    {
                        ID = item.ID,
                        ProductVariant = item.ProductVariant,
                        WarehouseQuantity = item.WarehouseQuantity,
                        StockTakeQuantity = item.StockTakeQuantity,
                        QuantityAdjustment = item.Quantity,
                        QuantityAfterAdjustment = QuantityAfterAdjustment
                    });
                }

                ViewBag.InventoryID = InventoryID;
                ViewBag.ProductVariantList = GetProductVariantByID(InventoryID);
                ViewBag.StockTakeID = StockTakeID;
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.DetailByInventoryID: " + ex.Message);
            }
            return View(model);
        }
        #endregion

        #region Xác nhận phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "XAC_NHAN_PHIEU_DIEU_CHINH_KHO")]
        public JsonResult JConfirmInventoryAdjust(int ID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.InventoryUpdateStatus(new ZInventoryUpdateStatusRequest()
                {
                    ID = ID,
                    Status = "W",
                    Discription = "",
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.JConfirmInventoryAdjust: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Từ chối phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "TU_CHOI_PHIEU_DIEU_CHINH_KHO")]
        public ActionResult RejectInventoryAdjust(int ID)
        {
            ViewBag.ID = ID;
            return PartialView();
        }

        public JsonResult JRejectInventoryAdjust(RejectInventoryAdjustModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.InventoryReject(new ZInventoryRejectRequest()
                {
                    ID = data.ID,
                    Reason = data.Reason,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {                
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.JRejectInventoryAdjust: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Hủy phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "HUY_PHIEU_DIEU_CHINH_KHO")]
        public JsonResult JCancelInventoryAdjust(ZInventoryCancelRequest data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.InventoryCancel(new ZInventoryCancelRequest()
                {
                    ID = data.ID,
                    Reason = data.Reason,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.JCancelInventoryAdjust: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Yêu cầu xác nhận phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "YEU_CAU_XAC_NHAN_PHIEU_DIEU_CHINH_KHO")]
        public JsonResult JRequestConfirmInventoryAdjust(int ID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.InventoryConfirm(new ZInventoryConfirmRequest()
                {
                    ID = ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.JRequestConfirmInventoryAdjust: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm chi tiết phiếu điều chỉnh kho
        [AuthorizeController(FunctionalTitle = "THEM_CHI_TIET_PHIEU_DIEU_CHINH_KHO")]

        public JsonResult JAddDetail(ZInventoryDetailAddRequest data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryRepository.InventoryDetailAdd(new ZInventoryDetailAddRequest()
                {
                    InventoryID = data.InventoryID,
                    ProductVariantID = data.ProductVariantID,
                    Quantity = data.Quantity,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });                
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryAdjustController.JAddDetail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}