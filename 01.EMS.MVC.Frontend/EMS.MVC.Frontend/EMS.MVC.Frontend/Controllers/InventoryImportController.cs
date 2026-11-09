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

namespace EMS.MVC.Frontend.Controllers
{
    public class InventoryImportController : BaseController
    {
        #region Danh sách phiếu nhập kho
        [AuthorizeController(FunctionalTitle = "DANH_SACH_PHIEU_NHAP_KHO")]
        public ActionResult InventoryImportView()
        {
            InventoryImportViewModel model = new InventoryImportViewModel()
            {
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListProvider = XCommon.SearchListProvider(new ZProviderSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListInventoryImportStatus = XData.GetListInventoryImportStatus().Where(m => m.Value != "C").ToList(),
                ListDateType = new List<SelectListItem>()
                {
                    new SelectListItem(){ Value = "IMPLEMENTATION_DATE", Text = "Thời gian nhập kho" },
                    new SelectListItem(){ Value = "CREATED_DATE", Text = "Thời gian tạo", Selected = true },
                    new SelectListItem(){ Value = "UPDATED_DATE", Text = "Thời gian cập nhật" }
                },
            };
            return View(model);
        }

        public ActionResult SearchListInventoryImportView(InventoryImportViewModel data)
        {
            List<SearchListInventoryImportViewModel> model = new List<SearchListInventoryImportViewModel>();
            var res = InventoryImportRepository.SearchListInventoryImport(new ZInventoryImportSearchListRequest()
            {
                FromDate = data.FromDate,
                ToDate = data.ToDate,
                DateType = data.DateType,
                InventoryImportCode = data.InventoryImportCode,
                WarehouseID = data.WareHouseID,
                ProviderID = data.ProviderID,
                Status = data.Status,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZInventoryImportSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new SearchListInventoryImportViewModel()
                    {
                        InventoryImportID = item.InventoryImportID,
                        InventoryImportCode = item.InventoryImportCode,
                        WareHouse = new WareHouseModel()
                        {
                            ID = item.WareHouse.WareHouseID,
                            WareHouseCode = item.WareHouse.WareHouseCode,
                            WareHouseName = item.WareHouse.WareHouseName
                        },
                        Provider = new ProviderModel()
                        {
                            ID = item.Provider.ProviderID,
                            ProviderCode = item.Provider.ProviderCode,
                            ProviderName = item.Provider.ProviderName
                        },
                        Status = new BaseModel()
                        {
                            Code = item.Status.Code,
                            Name = item.Status.Name
                        },
                        TotalAmount = item.TotalAmount,
                        DateOfInventoryImport = item.DateOfInventoryImport,
                        CreaterName = item.CreatedName,
                        CreatedDate = item.CreatedDate,
                    });
                }
            }
            return PartialView(model);
        }

        #endregion

        #region Thêm phiếu nhập kho
        [AuthorizeController(FunctionalTitle = "THEM_PHIEU_NHAP_KHO")]
        public ActionResult AddInventoryImportView()//--- Thêm 
        {
            AddInventoryImportViewModel model = new AddInventoryImportViewModel()
            {
                ListProvider = XCommon.SearchListProvider(new ZProviderSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                DateOfInventoryImport = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };
            model.WareHouseID = model.ListWareHouse.Where(m => m.IsDefault == "Y").Count() > 0 ? model.ListWareHouse.Where(m => m.IsDefault == "Y").First().ID : 0;
            return View(model);
        }

        public ActionResult AddDetailInventoryImportView(AddInventoryImportViewModel req)
        {
            AddDetailInventoryImportViewModel model = new AddDetailInventoryImportViewModel();
            model.NumberOfDetail = req.NumberOfDetail;
            model.ListProduct = XCommon.SearchListProductVariantByShopID(new ZProductVariantGetByShopIDRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            return PartialView(model);
        }

        public JsonResult JAddInventoryImport(AddInventoryImportViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZInventoryImportDetailAddRequest> ListInventoryImportDetail = new List<ZInventoryImportDetailAddRequest>();
                foreach (var item in req.ListInventoryImportDetail)
                {
                    ListInventoryImportDetail.Add(new ZInventoryImportDetailAddRequest()
                    {
                        ProductVariantID = item.ProductVariantID,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        PalletCode = item.PalletCode
                    });
                }

                res = InventoryImportRepository.AddInventoryImport(new ZInventoryImportAddRequest()
                {
                    ProviderID = req.ProviderID,
                    WareHouseID = req.WareHouseID,
                    DateOfInventoryImport = req.DateOfInventoryImport,
                    ImporterName = req.ImporterName,
                    MobileNumber = req.MobileNumber,
                    Note = req.Note,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    ListInventoryImportDetail = ListInventoryImportDetail
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JAddInventoryImport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa phiếu nhập kho
        [AuthorizeController(FunctionalTitle = "SUA_THONG_TIN_PHIEU_NHAP_KHO")]
        public ActionResult EditInventoryImportView(SearchListInventoryImportViewModel req)
        {
            EditInventoryImportViewModel model = null;
            var InventoryID = Security.DecodeBase64(req.SearchKey);
            var res = InventoryImportRepository.SearchByInventoryID(new ZInventoryImportGetByIDRequest()
            {
                ID = Convert.ToInt32(InventoryID),
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var itemData = JsonConvert.DeserializeObject<ZInventoryImportGetByIDResponse>(res.Data);
                List<InventoryDetailModel> ListInventoryImportDetail = new List<InventoryDetailModel>();
                foreach (var item in itemData.ListInventoryImportDetail)
                {
                    ListInventoryImportDetail.Add(new InventoryDetailModel()
                    {
                        ID = item.ID,
                        Product = new ProductModel() 
                        {
                            ProductName = item.Product.ProductName,
                            ProductVariant = new ProductVariantModel() 
                            {
                                ID = item.Product.ProductVariant.ProductVariantID,
                                Options = item.Product.ProductVariant.ProductVariantOptions,
                                Pallet = new PalletModel()
                                {
                                    PalletCode = item.Product.ProductVariant.Pallet.PalletCode
                                },
                            },
                        },
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalAmount = item.TotalAmount,
                    });
                };

                model = new EditInventoryImportViewModel()
                {
                    InventoryID = itemData.InventoryID,
                    ProviderID = itemData.Provider.ProviderID,
                    ListProvider = XCommon.SearchListProvider(new ZProviderSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                    DateOfInventoryImport = itemData.DateOfInventoryImport,
                    WareHouseID = itemData.WareHouse.WareHouseID,
                    ListWareHouse = new List<WareHouseModel>()
                    {
                        new WareHouseModel(){ ID = itemData.WareHouse.WareHouseID, WareHouseName = itemData.WareHouse.WareHouseName },
                    },
                    //ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseListRequest() { IsLock ="N", IsWareHouse = "Y", ShopID= XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                    ImporterName = itemData.ImporterName,
                    MobileNumber = itemData.MobileNumber,
                    Note = itemData.Note,
                    NumberOfDetail = ListInventoryImportDetail.Count(),
                    ListInventoryImportDetail = ListInventoryImportDetail
                };
            }
            return View(model);
        }

        public ActionResult EditDetailInventoryImportView(EditInventoryImportViewModel req)
        {
            EditDetailInventoryImportViewModel model = new EditDetailInventoryImportViewModel()
            {
                NumberOfDetail = req.NumberOfDetail,
                ListInventoryImportDetail = req.ListInventoryImportDetail == null ? new List<InventoryDetailModel>() : req.ListInventoryImportDetail,
                ListProduct = XCommon.SearchListProductVariantByShopID(new ZProductVariantGetByShopIDRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
            };
            return PartialView(model);
        }

        public JsonResult JEditInventoryImport(EditInventoryImportViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZInventoryImportDetailEditRequest> ListInventoryImportDetail = new List<ZInventoryImportDetailEditRequest>();
                foreach (var item in req.ListInventoryImportDetail)
                {
                    ListInventoryImportDetail.Add(new ZInventoryImportDetailEditRequest()
                    {
                        ProductVariantID = item.ProductVariantID,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        PalletCode = item.PalletCode
                    });
                }

                res = InventoryImportRepository.EditInventoryImport(new ZInventoryImportEditRequest()
                {
                    InventoryImportID = req.InventoryID,
                    ProviderID = req.ProviderID,
                    DateOfInventoryImport = req.DateOfInventoryImport,
                    ImporterName = req.ImporterName,
                    MobileNumber = req.MobileNumber,
                    Note = req.Note,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    ListInventoryImportDetail = ListInventoryImportDetail
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JEditInventoryImport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chi tiết phiếu nhập kho
        [AuthorizeController(FunctionalTitle = "CHI_TIET_PHIEU_NHAP_KHO")]
        public ActionResult DetailInventoryImportView(SearchListInventoryImportViewModel req)
        {
            DetailInventoryImportViewModel model = null;
            var InventoryImportID = Convert.ToInt32(Security.DecodeBase64(req.SearchKey));
            var res = InventoryImportRepository.SearchByInventoryID(new ZInventoryImportGetByIDRequest()
            {
                ID = InventoryImportID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var itemData = JsonConvert.DeserializeObject<ZInventoryImportGetByIDResponse>(res.Data);
                List<InventoryDetailModel> ListInventoryImportDetail = new List<InventoryDetailModel>();
                foreach (var item in itemData.ListInventoryImportDetail)
                {
                    ListInventoryImportDetail.Add(new InventoryDetailModel()
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
                                },
                            },
                        },
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalAmount = item.TotalAmount,
                    });
                };

                model = new DetailInventoryImportViewModel()
                {
                    Inventory = new InventoryModel()
                    {
                        ID = itemData.InventoryID,
                        InventoryCode = itemData.InventoryCode,
                        DateOfInventoryImport = itemData.DateOfInventoryImport,
                        ImporterName = itemData.ImporterName,
                        MobileNumber = itemData.MobileNumber,
                        Note = itemData.Note,
                        Reason = itemData.Reason,
                        WareHouse = new WareHouseModel()
                        {
                            WareHouseName = itemData.WareHouse.WareHouseName,
                        },
                        Provider = new ProviderModel()
                        {
                            ProviderName = itemData.Provider.ProviderName,
                        },
                        Status = new BaseModel()
                        {
                            Code = itemData.Status.Code,
                            Name = itemData.Status.Name
                        },
                        TotalAmount = itemData.TotalAmount,
                    },
                    ListInventoryImportDetail = ListInventoryImportDetail
                };
            }
            return View(model);
        }
        #endregion

        #region Xác nhận phiếu nhập kho
        [AuthorizeController(FunctionalTitle = "XAC_NHAN_PHIEU_NHAP_KHO")]
        public JsonResult JAcceptInventoryImport(SearchListInventoryImportViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryImportRepository.AcceptInventoryImport(new ZInventoryImportAcceptRequest()
                {
                    InventoryImportID = req.InventoryImportID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JAcceptInventoryImport: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Từ chối phiếu nhập kho
        [AuthorizeController(FunctionalTitle = "TU_CHOI_PHIEU_NHAP_KHO")]
        public ActionResult RejectInventoryImportView(int ID)
        {
            RejectInventoryImportViewModel model = new RejectInventoryImportViewModel()
            {
                InventoryImportID = ID
            };
            return PartialView(model);
        }

        public JsonResult JRejectInventoryImport(RejectInventoryImportViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryImportRepository.RejectInventoryImport(new ZInventoryImportRejectRequest()
                {
                    InventoryImportID = req.InventoryImportID,
                    Reason = req.Reason,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JRejectInventoryImport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa phiếu nhập kho
        public JsonResult JRemoveInventoryImport(SearchListInventoryImportViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = InventoryImportRepository.RemoveInventoryImport(new ZInventoryImportRemoveRequest()
                {
                    InventoryImportID = req.InventoryImportID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JRemoveInventoryImport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TEMP
        //[AuthorizeController(FunctionalTitle = "SUA_THONG_TIN_PHIEU_NHAP_KHO")]
        //public ActionResult Edit(int ID)//--- Chỉnh sửa thông tin
        //{
        //    InventoryImportEditViewModel model = new InventoryImportEditViewModel();
        //    try
        //    {
        //        var res = InventoryRepository.InventoryGetByID(new ZInventoryGetByIDRequest()
        //        {
        //            ID = ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //        if (res.Code == "00")
        //        {
        //            var Item = JsonConvert.DeserializeObject<ZInventoryGetByIDResponse>(res.Data);
        //            model = new InventoryImportEditViewModel()
        //            {
        //                ID = Item.ID,
        //                Code = Item.Code,
        //                Provider = Item.Provider,
        //                ListProvider = XCommon.SearchListProvider(new ZProviderSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
        //                Warehouse = Item.Warehouse,
        //                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseListRequest() { IsWareHouse = "Y", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
        //                ImplementationDate = Item.ImplementationDate,
        //                Note = Item.Note,
        //                ReceiverName = Item.ReceiverName,
        //                MobileNumber = Item.MobileNumber
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.Edit: " + ex.Message);
        //    }
        //    return PartialView(model);
        //}
        //public JsonResult JEdit(InventoryEditModel data)//--- J_Chỉnh sửa thông tin
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XCommon.InventoryEdit(new ZInventoryEditRequest()
        //        {
        //            ID = data.ID,
        //            WarehouseID = data.WarehouseID,
        //            ProviderID = data.ProviderID,
        //            ImplementationDate = data.ImplementationDate,
        //            Note = data.Note,
        //            Type = "I",
        //            UpdatedBy = XHelper.Session.EmployeeProfile.ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
        //            ReceiverName = data.ReceiverName,
        //            MobileNumber = data.MobileNumber
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JEdit: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        //[AuthorizeController(FunctionalTitle = "SUA_CHI_TIET_PHIEU_XUAT_KHO")]
        //public ActionResult EditDetail(int InventoryID)//--- Chỉnh sửa chi tiết
        //{
        //    InventoryImportEditDetailViewModel model = new InventoryImportEditDetailViewModel();
        //    try
        //    {
        //        var res = InventoryRepository.InventoryGetByID(new ZInventoryGetByIDRequest()
        //        {
        //            ID = InventoryID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //        if (res.Code == "00")
        //        {
        //            var Item = JsonConvert.DeserializeObject<ZInventoryGetByIDResponse>(res.Data);
        //            model = new InventoryImportEditDetailViewModel()
        //            {
        //                InventoryID = Item.ID,
        //                InventoryCode = Item.Code,
        //                Provider = Item.Provider,
        //                Warehouse = Item.Warehouse,
        //                Status = Item.Status,
        //                ImplementationDate = Item.ImplementationDate,
        //                Note = Item.Note,
        //                CreaterName = Item.CreaterName,
        //                CreatedDate = Item.CreatedDate,
        //                ProductVariantList = XCommon.SearchListProductVariant(),
        //                ListProvider = XCommon.SearchListProvider(new ZProviderSearchListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
        //                ListWareHouse = XCommon.SearchListWareHouse(new ZWareHouseListRequest() { IsWareHouse = "Y", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
        //                ReceiverName = Item.ReceiverName,
        //                MobileNumber = Item.MobileNumber
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.EditDetail: " + ex.Message);
        //    }
        //    return View(model);
        //}
        //public ActionResult DetailByInventoryID(int InventoryID)//--- Lấy các biến thể trong phiếu
        //{
        //    List<InventoryImportDetailByInventoryIDViewModel> model = new List<InventoryImportDetailByInventoryIDViewModel>();
        //    try
        //    {
        //        var list = InventoryRepository.InventoryDetailGetDataByInventoryID(new ZInventoryDetailGetByInventoryIDRequest()
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
        //            model.Add(new InventoryImportDetailByInventoryIDViewModel()
        //            {
        //                ID = item.ID,
        //                ProductVariant = item.ProductVariant,
        //                Quantity = item.Quantity,
        //                Price = item.Price,
        //                IntoMoney = IntoMoney
        //            });
        //        }
        //        ViewBag.TotalAmount = TotalAmount;
        //        ViewBag.ProductVariantList = XCommon.SearchListProductVariant();
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.DetailByInventoryID: " + ex.Message);
        //    }
        //    return PartialView(model);
        //}
        //public JsonResult JEditDetail(InventoryEditDetailModel data)//--- J_Chỉnh sửa số lượng, giá trong chi tiết
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = InventoryRepository.InventoryDetailEdit(new ZInventoryDetailEditRequest()
        //        {
        //            ID = data.DetailID,
        //            InventoryID = data.InventoryID,
        //            Quantity = data.Quantity,
        //            Price = data.Price,
        //            UpdatedBy = XHelper.Session.EmployeeProfile.ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JEditDetail: " + ex.Message);
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
        //        res = InventoryRepository.InventoryDetailRemove(new ZInventoryDetailRemoveRequest()
        //        {
        //            ID = data.DetailID,
        //            InventoryID = data.InventoryID,
        //            UpdatedBy = XHelper.Session.EmployeeProfile.ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JRemoveDetail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult JWaitConfirm(int ID)//--- J_Trạng thái --> chờ xác nhận
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = InventoryRepository.InventoryUpdateStatus(new ZInventoryUpdateStatusRequest()
        //        {
        //            ID = ID,
        //            Status = "W",
        //            Discription = "",
        //            UpdatedBy = XHelper.Session.EmployeeProfile.ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JWaitConfirm: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult Cancel(int ID)
        //{
        //    CancelViewModel model = new CancelViewModel()
        //    {
        //        ID = ID
        //    };
        //    return View(model);
        //}

        //public JsonResult JCancel(InventoryCancelModel data)//--- J_Huỷ phiếu nhập
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = InventoryRepository.InventoryCancel(new ZInventoryCancelRequest()
        //        {
        //            ID = data.ID,
        //            Reason = data.Reason,
        //            UpdatedBy = XHelper.Session.EmployeeProfile.ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportController.JCancel: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        #endregion
    }
}