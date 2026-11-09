using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
namespace EMS.MVC.Backend.Controllers
{
    public class TransportController : BaseController
    {
        // GET: Transport
        public ActionResult Index()
        {
            return View();
        }

        #region Transport Service
        //public ActionResult TransportServiceIndex()
        //{
        //    return View();
        //}
        //public ActionResult TransportServiceAdd(string TransportServiceGroup)
        //{
        //    ViewBag.TransportServiceGroup = XData.TransportGetServiceGroup();
        //    return View();
        //}
        //public ActionResult TransportServiceEdit(int ServiceID)
        //{
        //    TransportServiceEditViewModel model = new TransportServiceEditViewModel();
        //    var res = TransportServiceRepository.GetByID(new ZTransportServiceGetByIDRequest() 
        //    {
        //        ID = ServiceID
        //    });
        //    if (res.Code == "00")
        //    {
        //        //var RangeList = XData.TransportGetRange(); // DS phạm vi
        //        var item = JsonConvert.DeserializeObject<ZTransportServiceGetByIDResponse>(res.Data);
        //        model = new TransportServiceEditViewModel()
        //        {
        //            ID = item.ID,
        //            ParentID = item.ParentID,
        //            Code = item.Code,
        //            Name = item.Name,
        //            SpecialService = item.SpecialService
        //        };
        //    }
        //    return View(model);
        //}
        //public JsonResult JTransportServiceAdd(TransportServiceModel data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "TRANSPORT_SERVICE_ADD",
        //            Data = JsonConvert.SerializeObject(new ZTransportServiceAddRequest()
        //            {
        //                Code = data.Code,
        //                Name = data.Name,
        //                ParentID = data.ParentID,
        //                SpecialService = data.SpecialService.Code,
        //                CreatedBy = XHelper.Session.UserProfile.ID
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportServiceAdd: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult JTransportServiceEdit(TransportServiceEditModel data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "TRANSPORT_SERVICE_EDIT",
        //            Data = JsonConvert.SerializeObject(new ZTransportServiceEditRequest()
        //            {
        //                ID = data.ID,
        //                Name = data.Name,
        //                SpecialService = data.SpecialService,
        //                UpdatedBy = XHelper.Session.UserProfile.ID
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportServiceEdit: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult TransportServiceList(int ServiceGroupID)
        //{
        //    List<TransportServiceListViewModel> model = new List<TransportServiceListViewModel>();
        //    var res = TransportServiceRepository.SearchList(new ZTransportServiceSearchListRequest() 
        //    {
        //        ParentID = ServiceGroupID
        //    });
        //    if (res.Code == "00")
        //    {
        //        var list = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListResponse>>(res.Data);
        //        foreach (var item in list)
        //        {
        //            model.Add(new TransportServiceListViewModel()
        //            {
        //                ID = item.ID,
        //                Code = item.Code,
        //                Name = item.Name,
        //                ParentID = item.ParentID,
        //                SpecialService = new BaseModel() 
        //                {
        //                    Code = item.SpecialService.Code,
        //                    Name = item.SpecialService.Name
        //                },
        //                IsLock = item.IsLock,
        //                CreatedBy = item.CreatedBy,
        //                CreatedDate = item.CreatedDate
        //            });
        //        }
        //    }
        //    return PartialView(model);
        //}
        //public JsonResult JTransportServiceLock(int ServiceID)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "TRANSPORT_SERVICE_LOCK",
        //            Data = JsonConvert.SerializeObject(new ZTransportServiceLockRequest()
        //            {
        //                ID = ServiceID,
        //                UpdatedBy = XHelper.Session.UserProfile.ID
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportServiceLock: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult JTransportServiceUnlock(int ServiceID)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "TRANSPORT_SERVICE_UNLOCK",
        //            Data = JsonConvert.SerializeObject(new ZTransportServiceUnlockRequest()
        //            {
        //                ID = ServiceID,
        //                UpdatedBy = XHelper.Session.UserProfile.ID
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportServiceUnlock: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region Transport Region Group
        public ActionResult TransportRegionGroupIndex()
        {
            List<TransportRegionGroupViewModel> model = new List<TransportRegionGroupViewModel>();
            var res = TransportRegionGroupRepository.SearchList(new ZTransportRegionGroupSearchListRequest());
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportRegionGroupSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new TransportRegionGroupViewModel()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                        Description = item.Description,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return View(model);
        }
        public ActionResult TransportRegionGroupAdd()
        {
            return View();
        }
        public JsonResult JTransportRegionGroupAdd(TransportRegionGroupModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_REGION_GROUP_ADD",
                    Data = JsonConvert.SerializeObject(new ZTransportRegionGroupAddRequest()
                    {
                        Code = data.Code,
                        Name = data.Name,
                        Description = data.Description,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportRegionGroupAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Transport Region
        public ActionResult TransportRegionIndex()
        {
            List<TransportRegionViewModel> model = new List<TransportRegionViewModel>();
            var res = TransportRegionRepository.SearchList(new ZTransportRegionSearchListRequest());
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportRegionSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new TransportRegionViewModel()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                        ParentID = item.ParentID,
                        ProvincesID = item.ProvincesID,
                        Description = item.Description,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return View(model);
        }
        public ActionResult TransportRegionAdd()
        {
            ViewBag.TransportRegionGroupList = XCommon.TransportRegionGroupList();
            ViewBag.ProvincesList = XCommon.ProvincesList();
            return View();
        }
        public JsonResult JTransportRegionAdd(TransportRegionModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_REGION_ADD",
                    Data = JsonConvert.SerializeObject(new ZTransportRegionAddRequest()
                    {
                        Code = data.Code,
                        Name = data.Name,
                        ParentID = data.ParentID,
                        ProvincesID = data.ProvincesID,
                        Description = data.Description,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportRegionAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Transport Postage
        //Add
        public ActionResult TransportPostageIndex()
        {
            return View();
        }
        public ActionResult TransportPostageAdd()
        {
            ViewBag.MainServiceList = XCommon.TransportServiceList(1); // DS dịch vụ chính
            ViewBag.ExtraServiceList = XCommon.TransportServiceList(2); // DS dịch vụ cộng thêm
            //ViewBag.TransportRegionGroupList = XCommon.TransportRegionGroupList(); // DS nhóm vùng tính cước
            ViewBag.ApprovedShopList = XCommon.ApprovedShopList(); // DS Shop đã phê duyệt
            ViewBag.TransportGetRange = XData.GetListTransportRange(); // DS phạm vi
            ViewBag.ProvincesList = XCommon.ProvincesList(); //DS tỉnh
            return View();
        }
        public ActionResult TransportPostageEdit(TransportPostageEditViewModel model)
        {
            return View(model);
        }
        public ActionResult TransportPostageBasicInfoAdd()
        {
            ViewBag.MainServiceList = XCommon.TransportServiceList(1); // DS dịch vụ chính
            ViewBag.ExtraServiceList = XCommon.TransportServiceList(2); // DS dịch vụ cộng thêm
            //ViewBag.TransportRegionGroupList = XCommon.TransportRegionGroupList(); // DS nhóm vùng tính cước
            ViewBag.SubjectsOfApplication = XData.GetListSubjectsOfApplication();
            ViewBag.ApprovedShopList = XCommon.ApprovedShopList(); // DS Shop đã phê duyệt
            ViewBag.ProvincesList = XCommon.ProvincesList(); //DS tỉnh
            ViewBag.ProvinceApplyList = XData.GetListOptionProvinceApply();//Tỉnh áp dụng
            ViewBag.AreaApplyList = XData.GetListOptionAreaApply();// Vùng áp dụng
            return View();
        }

        public ActionResult TransportPostageBasicInfo(int PostageID)
        {
            ViewBag.MainServiceList = XCommon.TransportServiceList(1); // DS dịch vụ chính
            ViewBag.ExtraServiceList = XCommon.TransportServiceList(2); // DS dịch vụ cộng thêm
            //ViewBag.TransportRegionGroupList = XCommon.TransportRegionGroupList(); // DS nhóm vùng tính cước
            ViewBag.ApprovedShopList = XCommon.ApprovedShopList(); // DS Shop đã phê duyệt
            ViewBag.TransportGetRange = XData.GetListTransportRange(); // DS phạm vi
            ViewBag.ProvincesList = XCommon.ProvincesList(); //DS tỉnh
            ViewBag.ProvinceApplyList = XData.GetListOptionProvinceApply();//Tỉnh áp dụng
            ViewBag.AreaApplyList = XData.GetListOptionAreaApply();// Vùng áp dụng

            TransportPostageBasicInfoModel model = new TransportPostageBasicInfoModel();
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest()
            {
                TransportPostageID = PostageID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new TransportPostageBasicInfoModel()
                {
                    PostageID = item.TransportPostageID,
                    PostageCode = item.TransportPostageCode,
                    PostageName = item.TransportPostageName,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    MainServiceID = item.MainTransportServiceID,
                    ExtraServiceIDs = item.ExtraTransportServiceID,
                    Area = item.Area,
                    ProvinceApply = item.ProvinceApply,
                    AreaApply = item.AreaApply,
                    SubjectsOfApplication = item.SubjectOfApplication,
                    //ApplicationObjectValue = item.ApplicationObjectValue,
                    Status = item.Status
                };
            }
            ViewBag.SubjectsOfApplication = XData.GetListSubjectsOfApplication();
            //if (model.PostageID == 0)
            //{
            //    return RedirectToAction("TransportPostageIndex");
            //}
            return View(model);
        }
        public ActionResult TransportPostageMainFee(int PostageID)
        {
            TransportPostageMainFeeModel model = new TransportPostageMainFeeModel();
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest()
            {
                TransportPostageID = PostageID
            });
            if (res.Code == "00")
            {
                var RangeList = XData.GetListTransportRange(); // DS phạm vi
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new TransportPostageMainFeeModel()
                {
                    PostageID = item.TransportPostageID,
                    MainFeeVAT = item.MainFeeVAT,
                    MainFeeRange = item.MainFeeRange,
                    RangeList = RangeList,
                    Status = item.Status
                };
            }
            //if (model.PostageID == 0)
            //{
            //    return RedirectToAction("TransportPostageIndex");
            //}
            return View(model);
        }
        public ActionResult TransportPostageSurcharge(int PostageID)
        {
            TransportPostageSurchargeModel model = new TransportPostageSurchargeModel();
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest() 
            {
                TransportPostageID = PostageID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new TransportPostageSurchargeModel()
                {
                    PostageID = item.TransportPostageID,
                    PostageCode = item.TransportPostageCode,
                    PostageName = item.TransportPostageName,
                    SurchargeVAT = item.SurchargeVAT,
                    Fuel = item.Fuel,
                    RemoteArea = item.RemoteArea,
                    IslandsArea = item.IslandsArea,
                    Status = item.Status
                };
            }
            if (model.PostageID == 0)
            {
                return RedirectToAction("TransportPostageIndex");
            }
            return View(model);
        }


        //Detail + Update
        public ActionResult TransportPostageList()
        {
            List<TransportPostageListViewModel> model = new List<TransportPostageListViewModel>();
            var res = TransportPostageRepository.SearchList(new ZTransportPostageSearchListRequest()
            {
                Type = 1
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageSearchListResponse>>(res.Data);
                int stt = 1;
                foreach (var item in list)
                {
                    model.Add(new TransportPostageListViewModel()
                    {
                        STT = stt,
                        ID = item.TransportPostageID,
                        Code = item.TransportPostageCode,
                        Name = item.TransportPostageName,
                        ServiceCode = item.TransportServiceCode,
                        ServiceName = item.TransportServiceName,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        IsLock = item.IsLock,
                        Status = new StatusModel()
                        {
                            Code = item.Status.Code,
                            Name = item.Status.Name
                        }
                    });
                    stt++;
                }
            }
            return View(model);
        }
        public ActionResult TransportPostageBasicInfoDetail(TransportPostageBasicInfoModel model)
        {
            if (model == null)
            {
                model = new TransportPostageBasicInfoModel();
            }
            ViewBag.MainServiceList = XCommon.TransportServiceList(1); // DS dịch vụ chính
            //ViewBag.TransportRegionGroupList = XCommon.TransportRegionGroupList(); // DS nhóm vùng tính cước
            ViewBag.ApprovedShopList = XCommon.ApprovedShopList(); // DS Shop đã phê duyệt
            ViewBag.ProvincesList = XCommon.ProvincesList(); //DS tỉnh
            return View(model);
        }
        public JsonResult JTransportPostageBasicInfoAdd(TransportPostageBasicInfoModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_MAIN_SERVICE_BASIC_ADD",
                    Data = JsonConvert.SerializeObject(new ZTransportPostageMainServiceBasicAddRequest()
                    {
                        TransportPostageCode = data.PostageCode,
                        TransportPostageName = data.PostageName,
                        StartDate = data.StartDate,
                        EndDate = data.EndDate,
                        MainTransportServiceID = data.MainServiceID,
                        ExtraTransportServiceID = data.ExtraServiceIDs,
                        Area = data.Area,
                        ProvinceApply = data.ProvinceApply,
                        AreaApply = data.AreaApply,
                        SubjectOfApplication = data.SubjectsOfApplication,
                        //ApplicationObjectValue = data.ApplicationObjectValue,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportServiceAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public JsonResult JTransportPostageBasicInfoEdit(TransportPostageBasicInfoModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_MAIN_SERVICE_BASIC_EDIT",
                    Data = JsonConvert.SerializeObject(new ZTransportPostageMainServiceBasicEditRequest()
                    {
                        TransportPostageID = data.PostageID,
                        TransportPostageCode = data.PostageCode,
                        TransportPostageName = data.PostageName,
                        StartDate = data.StartDate,
                        EndDate = data.EndDate,
                        MainTransportServiceID = data.MainServiceID,
                        ExtraTransportServiceID = data.ExtraServiceIDs,
                        Area = data.Area,
                        ProvinceApply = data.ProvinceApply,
                        AreaApply = data.AreaApply,
                        SubjectOfApplication = data.SubjectsOfApplication,
                        //ApplicationObjectValue = data.ApplicationObjectValue,
                        UpdateBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageBasicInfoEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JTransportPostageMainFeeEdit(TransportPostageMainFeeModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_MAIN_SERVICE_FEE_EDIT",
                    Data = JsonConvert.SerializeObject(new ZTransportPostageMainServiceFeeEditRequest()
                    {
                        TransportPostageID = data.PostageID,
                        MainFeeRange = data.MainFeeRange,
                        MainFeeVAT = data.MainFeeVAT,
                        UpdateBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageMainFeeEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JTransportPostageSurchargeEdit(TransportPostageSurchargeModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.MainServiceSurchargeEdit(new ZTransportPostageSurchargeEditRequest() 
                {
                    TransportPostageID = data.PostageID,
                    SurchargeVAT = data.SurchargeVAT,
                    Fuel = new ZTransportPostageSurchargeEditFuelRequest()
                    {
                        Checked = data.Fuel.Checked,
                        Unit = data.Fuel.Unit,
                        Value = data.Fuel.Value,
                        MinValue = data.Fuel.MinValue,
                        MaxValue = data.Fuel.MaxValue,
                        InOfProvince = data.Fuel.InOfProvince,
                        OutOfProvince = data.Fuel.OutOfProvince
                    },
                    RemoteArea = new ZTransportPostageSurchargeEditRemoteAreaRequest()
                    {
                        Checked = data.RemoteArea.Checked,
                        Unit = data.RemoteArea.Unit,
                        Value = data.RemoteArea.Value,
                        MinValue = data.RemoteArea.MinValue,
                        MaxValue = data.RemoteArea.MaxValue,
                        InOfProvince = data.RemoteArea.InOfProvince,
                        OutOfProvince = data.RemoteArea.OutOfProvince
                    },
                    IslandsArea = new ZTransportPostageSurchargeEditIslandsAreaRequest()
                    {
                        Checked = data.IslandsArea.Checked,
                        Unit = data.IslandsArea.Unit,
                        Value = data.IslandsArea.Value,
                        MinValue = data.IslandsArea.MinValue,
                        MaxValue = data.IslandsArea.MaxValue,
                        InOfProvince = data.IslandsArea.InOfProvince,
                        OutOfProvince = data.IslandsArea.OutOfProvince
                    },
                    UpdateBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageSurcharge: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TransportPostageByID(int PostageID)
        {
            TransportPostageBasicInfoModel model = new TransportPostageBasicInfoModel();
            var res = TransportPostageRepository.GetByTransportPostageID(new ZTransportPostageGetByTransportPostageIDRequest() 
            {
                TransportPostageID = PostageID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageGetByTransportPostageIDResponse>(res.Data);
                model = new TransportPostageBasicInfoModel()
                {
                    PostageID = item.TransportPostageID,
                    PostageCode = item.TransportPostageCode,
                    PostageName = item.TransportPostageName,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    MainServiceID = item.MainTransportServiceID,
                    Area = item.Area,
                    SubjectsOfApplication = item.SubjectOfApplication,
                    //ApplicationObjectValue = item.ApplicationObjectValue
                };
            }
            return View(model);
        }
        public ActionResult TransportPostageRangeSearch(TransportPostageCommonModel data)
        {
            List<TransportPostageRangeSearchViewModel> model = new List<TransportPostageRangeSearchViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "TRANSPORT_POSTAGE_RANGE_SEARCH",
                Data = JsonConvert.SerializeObject(new ZTransportPostageRangeSearchRequest()
                {
                    PostageID = data.PostageID,
                    GroupID = data.GroupID
                })
            });
            if (res.Code == "00" && res.Data != "[]")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageRangeSearchResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new TransportPostageRangeSearchViewModel()
                    {
                        ID = item.ID,
                        GroupID = item.GroupID,
                        PostageID = item.PostageID,
                        FromRangeName = item.FromRangeName,
                        FromRangeValueName = item.FromRangeValueName,
                        ToRange = item.ID,
                        ToRangeName = item.ToRangeName,
                        ToRangeValueName = item.ToRangeValueName,
                        TwoWay = item.TwoWay,
                        Unit = item.Unit,
                        Value = item.Value,
                        MaxValue = item.MaxValue,
                        MinValue = item.MinValue,
                    });
                }
            }
            ViewBag.Common = data;
            return View(model);
        }
        public ActionResult TransportPostageWeightSearch(TransportPostageCommonModel data)
        {
            List<TransportPostageWeightSearchModel> model = new List<TransportPostageWeightSearchModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "TRANSPORT_POSTAGE_WEIGHT_SEARCH",
                Data = JsonConvert.SerializeObject(new ZTransportPostageWeightSearchRequest()
                {
                    PostageID = data.PostageID,
                    GroupID = data.GroupID,
                    ParentID = data.ParentID
                })
            });
            if (res.Code == "00" && res.Data != "[]")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageWeightSearchResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new TransportPostageWeightSearchModel()
                    {
                        ID = item.ID,
                        GroupID = item.GroupID,
                        PostageID = item.PostageID,
                        ParentID = item.ParentID,
                        FromWeight = item.FromWeight,
                        ToWeight = item.ToWeight,
                        Unit = item.Unit,
                        Value = item.Value,
                        MaxValue = item.MaxValue,
                        MinValue = item.MinValue
                    });
                }
            }
            ViewBag.Common = data;
            return View(model);
        }

        public ActionResult TransportPostageWeight()
        {
            return View();
        }
        public ActionResult TransportPostageRange(int RegionGroupID)
        {
            ViewBag.ProvincesList = XCommon.ProvincesList(); //DS tỉnh
            ViewBag.RegionList = XCommon.TransportRegionByParentID(RegionGroupID); //DS vùng
            return View();
        }
        public ActionResult TransportPostageRangeWeight(int RegionGroupID)
        {
            ViewBag.ProvincesList = XCommon.ProvincesList(); //DS tỉnh
            ViewBag.RegionList = XCommon.TransportRegionByParentID(RegionGroupID); //DS vùng
            return View();
        }
        public ActionResult TransportPostageExtraServiceWeight()
        {
            return View();
        }
        public ActionResult TransportPostageExtraServiceRange(int RegionGroupID = 0)
        {
            ViewBag.ProvincesList = XCommon.ProvincesList();
            ViewBag.RegionList = XCommon.TransportRegionByParentID(RegionGroupID);
            return View();
        }
        public ActionResult TransportPostageExtraServiceRangeWeight(int RegionGroupID = 0)
        {
            ViewBag.ProvincesList = XCommon.ProvincesList();
            ViewBag.RegionList = XCommon.TransportRegionByParentID(RegionGroupID);
            return View();
        }
        public ActionResult TransportPostageRegionOptionByParentID(int ParentID)
        {
            List<SelectListItem> model = new List<SelectListItem>();
            model = XCommon.TransportRegionByParentID(ParentID);
            return View(model);
        }
        public ActionResult TransportPostageRangeAdd(TransportPostageCommonModel data)
        {
            data.ProvincesList = XCommon.ProvincesList(); //DS tỉnh
            data.RegionList = XCommon.TransportRegionByParentID(data.RegionGroupID); //DS vùng
            return View(data);
        }
        public ActionResult TransportPostageWeightAdd(TransportPostageCommonModel data)
        {
            return View(data);
        }
        public ActionResult TransportPostageWeightEdit(int PostageID, int GroupID)
        {
            List<TransportPostageWeightSearchModel> model = new List<TransportPostageWeightSearchModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "TRANSPORT_POSTAGE_WEIGHT_SEARCH",
                Data = JsonConvert.SerializeObject(new ZTransportPostageWeightSearchRequest()
                {
                    PostageID = PostageID,
                    GroupID = GroupID,
                    ParentID = 0
                })
            });
            if (res.Code == "00" && res.Data != "[]")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageWeightSearchResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new TransportPostageWeightSearchModel()
                    {
                        ID = item.ID,
                        GroupID = item.GroupID,
                        PostageID = item.PostageID,
                        ParentID = item.ParentID,
                        FromWeight = item.FromWeight,
                        ToWeight = item.ToWeight,
                        Unit = item.Unit,
                        Value = item.Value,
                        MaxValue = item.MaxValue,
                        MinValue = item.MinValue
                    });
                }
            }
            return View(model);
        }
        public JsonResult JTransportPostageRangeAdd(TransportPostageRangeModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_RANGE_ADD",
                    Data = JsonConvert.SerializeObject(new ZTransportPostageRangeAddRequest()
                    {
                        GroupID = data.GroupID,
                        PostageID = data.PostageID,
                        FromRange = data.FromRange,
                        FromRangeValue = data.FromRangeValue,
                        ToRange = data.ToRange,
                        ToRangeValue = data.ToRangeValue,
                        TwoWay = data.TwoWay,
                        Unit = data.Unit,
                        Value = data.Value,
                        MaxValue = data.MaxValue,
                        MinValue = data.MinValue,
                        Weight = data.Weight,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageRangeAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JTransportPostageWeightAdd(List<TransportPostageWeightModel> data)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZTransportPostageWeightAddRequest> req = new List<ZTransportPostageWeightAddRequest>();
                foreach (var item in data)
                {
                    req.Add(new ZTransportPostageWeightAddRequest()
                    {
                        GroupID = item.GroupID,
                        PostageID = item.PostageID,
                        FromWeight = item.FromWeight,
                        ToWeight = item.ToWeight,
                        Unit = item.Unit,
                        Value = item.Value,
                        MaxValue = item.MaxValue,
                        MinValue = item.MinValue,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    });
                }

                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_WEIGHT_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageWeightAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JTransportPostageConfirm(int PostageID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_CONFIRM",
                    Data = JsonConvert.SerializeObject(new ZTransportPostageConfirmRequest()
                    {
                        ID = PostageID,
                        UpdatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageConfirm: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JTransportPostageUnconfirm(int PostageID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.UnConfirmTransportPostage(new ZTransportPostageUnConfirmRequest() 
                {
                    ID = PostageID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageUnconfirm: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JTransportPostageLock(int PostageID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.ChangeIsLock(new ZTransportPostageChangeIsLockRequest() 
                {
                    ID = PostageID,
                    IsLock = "Y",
                    UpdatedBy = XHelper.Session.UserProfile.ID,
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JTransportPostageUnlock(int PostageID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportPostageRepository.ChangeIsLock(new ZTransportPostageChangeIsLockRequest()
                {
                    ID = PostageID,
                    IsLock = "N",
                    UpdatedBy = XHelper.Session.UserProfile.ID,
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageUnlock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        //Cấu hình phí dịch vụ chính
        public ActionResult TransportPostageDetail(int PostageID)
        {
            TransportPostageDetailViewModel model = new TransportPostageDetailViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "TRANSPORT_POSTAGE_MAIN_FEE_BY_ID",
                Data = JsonConvert.SerializeObject(new ZTransportPostageMainFeeByIDRequest()
                {
                    PostageID = PostageID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageMainFeeByIDResponse>(res.Data);
                model = new TransportPostageDetailViewModel()
                {
                    PostageID = item.PostageID,
                    PostageCode = item.PostageCode,
                    PostageName = item.PostageName,
                    PostageRange = item.MainFeeRange
                };
            }
            //if (model.PostageID == 0)
            //{
            //    return RedirectToAction("TransportPostageIndex");
            //}
            return View(model);
        }
        public ActionResult TransportPostageDetailData(int PostageID, int PostageRange)
        {
            List<TransportPostageDetailDataModel> model = new List<TransportPostageDetailDataModel>();
            var res = TransportPostageDetailRepository.GetByTransportPostageID(new ZTransportPostageDetailGetByTransportPostageIDRequest() 
            {
                TransportPostageID = PostageID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageDetailGetByTransportPostageIDResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new TransportPostageDetailDataModel()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        IsStep = item.IsStep,
                        Weight = item.Weight,
                        LocalFee = item.LocalFee,
                        IntercityFee1 = item.IntercityFee1,
                        IntercityFee2 = item.IntercityFee2,
                        IntercityFee3 = item.IntercityFee3,
                        IntercityFee4 = item.IntercityFee4,
                        IntercityFee5 = item.IntercityFee5
                    });
                }
            }
            ViewBag.PostageRange = PostageRange;
            return View(model);
        }
        public JsonResult JTransportPostageDetailAdd(TransportPostageDetailAddModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_DETAIL_ADD",
                    Data = JsonConvert.SerializeObject(new ZTransportPostageDetailAddRequest()
                    {
                        PostageID = data.PostageID,
                        IsStep = data.IsStep,
                        Weight = data.Weight,
                        LocalFee = data.LocalFee,
                        IntercityFee1 = data.IntercityFee1,
                        IntercityFee2 = data.IntercityFee2,
                        IntercityFee3 = data.IntercityFee3,
                        IntercityFee4 = data.IntercityFee4,
                        IntercityFee5 = data.IntercityFee5,
                        Key = data.Key,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageDetailAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JTransportPostageDetailEdit(TransportPostageDetailEditModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_DETAIL_EDIT",
                    Data = JsonConvert.SerializeObject(new ZTransportPostageDetailEditRequest()
                    {
                        ID = data.ID,
                        Weight = data.Weight,
                        LocalFee = data.LocalFee,
                        IntercityFee1 = data.IntercityFee1,
                        IntercityFee2 = data.IntercityFee2,
                        IntercityFee3 = data.IntercityFee3,
                        IntercityFee4 = data.IntercityFee4,
                        IntercityFee5 = data.IntercityFee5,
                        UpdatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageDetailEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JTransportPostageDetailRemove(TransportPostageDetailRemoveModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_DETAIL_REMOVE",
                    Data = JsonConvert.SerializeObject(new ZTransportPostageDetailRemoveRequest()
                    {
                        ID = data.ID,
                        UpdatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageDetailRemove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //Xuất Excel
        public ActionResult ExportExcelMainServiceConfig(ExportExcelMainServiceConfigModel data)
        {
            HSSFWorkbook wb = new HSSFWorkbook();
            var res = TransportPostageDetailRepository.GetByTransportPostageID(new ZTransportPostageDetailGetByTransportPostageIDRequest()
            {
                TransportPostageID = data.PostageID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageDetailGetByTransportPostageIDResponse>>(res.Data);
                
                // Tạo ra 1 sheet
                ISheet sheet = wb.CreateSheet();

                // In đậm
                HSSFFont myFontBold = (HSSFFont)wb.CreateFont();
                myFontBold.FontHeightInPoints = 11;
                myFontBold.FontName = "Times New Roman";
                myFontBold.IsBold = true;

                HSSFCellStyle borderFontBold = (HSSFCellStyle)wb.CreateCellStyle();
                borderFontBold.SetFont(myFontBold);
                borderFontBold.VerticalAlignment = VerticalAlignment.Center;
                borderFontBold.Alignment = HorizontalAlignment.Center;
                borderFontBold.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                borderFontBold.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                borderFontBold.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                borderFontBold.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

                // Thường
                HSSFFont myFont = (HSSFFont)wb.CreateFont();
                myFont.FontHeightInPoints = (short)11;
                myFont.FontName = "Times New Roman";

                HSSFCellStyle borderFont = (HSSFCellStyle)wb.CreateCellStyle();
                borderFont.SetFont(myFont);
                borderFont.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                borderFont.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                borderFont.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                borderFont.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;



                // Bắt đầu ghi lên sheet

                // Tạo row
                var row0 = sheet.CreateRow(0);
                row0.CreateCell(0); // tạo ra cell trc khi merge
                if (data.PostageRange == 1)
                {
                    //border
                    row0.CreateCell(2).CellStyle = borderFontBold;

                    CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 2);
                    sheet.AddMergedRegion(cellMerge);
                    row0.GetCell(0).SetCellValue("Bảng cước " + data.PostageName);
                    row0.GetCell(0).CellStyle = borderFontBold;

                    var row1 = sheet.CreateRow(1);
                    row1.CreateCell(0).SetCellValue(" Nấc khối lượng (gram) ");
                    row1.CreateCell(1).SetCellValue(" Cước nội tỉnh (VNĐ) ");
                    row1.CreateCell(2).SetCellValue(" Cước liên tỉnh (VNĐ) ");
                    row1.GetCell(0).CellStyle = borderFontBold;
                    row1.GetCell(1).CellStyle = borderFontBold;
                    row1.GetCell(2).CellStyle = borderFontBold;

                    // bắt đầu duyệt mảng và ghi tiếp tục
                    int rowIndex = 2;
                    for (var i = 0; i < list.Count - 1; i++)
                    {
                        var item = list[i];
                        // tao row mới
                        var newRow = sheet.CreateRow(rowIndex);

                        // set giá trị
                        newRow.CreateCell(0).SetCellValue(item.Name);
                        newRow.CreateCell(1).SetCellValue(item.LocalFee);
                        newRow.CreateCell(2).SetCellValue(item.IntercityFee1);

                        newRow.GetCell(0).CellStyle = borderFont;
                        newRow.GetCell(1).CellStyle = borderFont;
                        newRow.GetCell(2).CellStyle = borderFont;

                        // tăng index
                        rowIndex++;
                    }

                    //Nấc
                    var itemNac = list[list.Count - 1];
                    // tao row mới
                    var newRowNac = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRowNac.CreateCell(0).SetCellValue("Mỗi nấc " + itemNac.Weight.ToString("N0") + " tiếp theo");
                    newRowNac.CreateCell(1).SetCellValue(itemNac.LocalFee);
                    newRowNac.CreateCell(2).SetCellValue(itemNac.IntercityFee1);

                    newRowNac.GetCell(0).CellStyle = borderFont;
                    newRowNac.GetCell(1).CellStyle = borderFont;
                    newRowNac.GetCell(2).CellStyle = borderFont;

                    sheet.AutoSizeColumn(0, true);
                    sheet.AutoSizeColumn(1, true);
                    sheet.AutoSizeColumn(2, true);
                }
                else if (data.PostageRange == 2)
                {
                    //border
                    row0.CreateCell(5).CellStyle = borderFontBold; 

                    CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 5);
                    sheet.AddMergedRegion(cellMerge);
                    row0.GetCell(0).SetCellValue("Bảng cước " + data.PostageName);
                    row0.GetCell(0).CellStyle = borderFontBold;

                    var row1 = sheet.CreateRow(1);
                    var row2 = sheet.CreateRow(2);
                    var row3 = sheet.CreateRow(3);

                    row1.CreateCell(0);
                    row2.CreateCell(0);
                    row3.CreateCell(0);
                    CellRangeAddress cellMerge1 = new CellRangeAddress(1, 3, 0, 0);
                    sheet.AddMergedRegion(cellMerge1);
                    row1.GetCell(0).SetCellValue(" Nấc khối lượng (gram) ");
                    row1.GetCell(0).CellStyle = borderFontBold;
                    row2.GetCell(0).CellStyle = borderFontBold;
                    row3.GetCell(0).CellStyle = borderFontBold;
                    //border
                    row3.CreateCell(1).CellStyle = borderFontBold;

                    row1.CreateCell(1);
                    CellRangeAddress cellMerge2 = new CellRangeAddress(1, 3, 1, 1);
                    sheet.AddMergedRegion(cellMerge2);
                    row1.GetCell(1).SetCellValue(" Cước nội tỉnh (VNĐ) ");
                    row1.GetCell(1).CellStyle = borderFontBold;

                    row1.CreateCell(2);
                    CellRangeAddress cellMerge3 = new CellRangeAddress(1, 1, 2, 5);
                    sheet.AddMergedRegion(cellMerge3);
                    row1.GetCell(2).SetCellValue(" Cước liên tỉnh (VNĐ) ");
                    row1.GetCell(2).CellStyle = borderFontBold;
                    //border
                    row1.CreateCell(3).CellStyle = borderFontBold;
                    row1.CreateCell(4).CellStyle = borderFontBold;
                    row1.CreateCell(5).CellStyle = borderFontBold;

                    row2.CreateCell(2);
                    CellRangeAddress cellMerge4 = new CellRangeAddress(2, 3, 2, 2);
                    sheet.AddMergedRegion(cellMerge4);
                    row2.GetCell(2).SetCellValue(" Vùng 1 ");
                    row2.GetCell(2).CellStyle = borderFontBold;

                    row2.CreateCell(3);
                    CellRangeAddress cellMerge5 = new CellRangeAddress(2, 2, 3, 4);
                    sheet.AddMergedRegion(cellMerge5);
                    row2.GetCell(3).SetCellValue(" Vùng 2 ");
                    row2.GetCell(3).CellStyle = borderFontBold;

                    row2.CreateCell(5);
                    CellRangeAddress cellMerge6 = new CellRangeAddress(2, 3, 5, 5);
                    sheet.AddMergedRegion(cellMerge6);
                    row2.GetCell(5).SetCellValue(" Vùng 3 ");
                    row2.GetCell(5).CellStyle = borderFontBold;

                    row3.CreateCell(3).SetCellValue(" Đà Nẵng <-> Hà Nội, HCM ");
                    row3.CreateCell(4).SetCellValue(" Hà Nội <-> Hồ Chí Minh ");
                    row3.GetCell(3).CellStyle = borderFontBold;
                    row3.GetCell(4).CellStyle = borderFontBold;
                    //border
                    row3.CreateCell(5).CellStyle = borderFontBold;

                    // bắt đầu duyệt mảng và ghi tiếp tục
                    int rowIndex = 4;
                    for (var i = 0; i < list.Count - 1; i++)
                    {
                        var item = list[i];
                        // tao row mới
                        var newRowNac = sheet.CreateRow(rowIndex);

                        // set giá trị
                        newRowNac.CreateCell(0).SetCellValue(item.Name);
                        newRowNac.CreateCell(1).SetCellValue(item.LocalFee);
                        newRowNac.CreateCell(2).SetCellValue(item.IntercityFee1);
                        newRowNac.CreateCell(3).SetCellValue(item.IntercityFee2);
                        newRowNac.CreateCell(4).SetCellValue(item.IntercityFee3);
                        newRowNac.CreateCell(5).SetCellValue(item.IntercityFee4);

                        newRowNac.GetCell(0).CellStyle = borderFont;
                        newRowNac.GetCell(1).CellStyle = borderFont;
                        newRowNac.GetCell(2).CellStyle = borderFont;
                        newRowNac.GetCell(3).CellStyle = borderFont;
                        newRowNac.GetCell(4).CellStyle = borderFont;
                        newRowNac.GetCell(5).CellStyle = borderFont;

                        // tăng index
                        rowIndex++;
                    }

                    //Nấc
                    var itemNac = list[list.Count - 1];
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRow.CreateCell(0).SetCellValue("Mỗi nấc " + itemNac.Weight.ToString("N0") + " tiếp theo");
                    newRow.CreateCell(1).SetCellValue(itemNac.LocalFee);
                    newRow.CreateCell(2).SetCellValue(itemNac.IntercityFee1);
                    newRow.CreateCell(3).SetCellValue(itemNac.IntercityFee2);
                    newRow.CreateCell(4).SetCellValue(itemNac.IntercityFee3);
                    newRow.CreateCell(5).SetCellValue(itemNac.IntercityFee4);

                    newRow.GetCell(0).CellStyle = borderFont;
                    newRow.GetCell(1).CellStyle = borderFont;
                    newRow.GetCell(2).CellStyle = borderFont;
                    newRow.GetCell(3).CellStyle = borderFont;
                    newRow.GetCell(4).CellStyle = borderFont;
                    newRow.GetCell(5).CellStyle = borderFont;
                    
                    sheet.AutoSizeColumn(0, true);
                    sheet.AutoSizeColumn(1, true);
                    sheet.AutoSizeColumn(2, true);
                    sheet.AutoSizeColumn(3, true);
                    sheet.AutoSizeColumn(4, true);
                    sheet.AutoSizeColumn(5, true);
                }
                else if (data.PostageRange == 3)
                {
                    //border
                    row0.CreateCell(3).CellStyle = borderFontBold;

                    CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 3);
                    sheet.AddMergedRegion(cellMerge);
                    row0.GetCell(0).SetCellValue("Bảng cước " + data.PostageName);
                    row0.GetCell(0).CellStyle = borderFontBold;

                    var row1 = sheet.CreateRow(1);
                    var row2 = sheet.CreateRow(2);

                    row1.CreateCell(0);
                    CellRangeAddress cellMerge1 = new CellRangeAddress(1, 2, 0, 0);
                    sheet.AddMergedRegion(cellMerge1);
                    row1.GetCell(0).SetCellValue(" Nấc khối lượng (gram) ");
                    row1.GetCell(0).CellStyle = borderFontBold;

                    row1.CreateCell(1);
                    CellRangeAddress cellMerge2 = new CellRangeAddress(1, 2, 1, 1);
                    sheet.AddMergedRegion(cellMerge2);
                    row1.GetCell(1).SetCellValue(" Cước nội tỉnh (VNĐ) ");
                    row1.GetCell(1).CellStyle = borderFontBold;
                    
                    row1.CreateCell(2);
                    CellRangeAddress cellMerge3 = new CellRangeAddress(1, 1, 2, 3);
                    sheet.AddMergedRegion(cellMerge3);
                    row1.GetCell(2).SetCellValue(" Cước liên tỉnh (VNĐ) ");
                    row1.GetCell(2).CellStyle = borderFontBold;
                    //border
                    row1.CreateCell(3).CellStyle = borderFontBold;

                    row2.CreateCell(0).CellStyle = borderFontBold;
                    row2.CreateCell(2).SetCellValue(" Đà Nẵng <-> Hà Nội, HCM ");
                    row2.CreateCell(3).SetCellValue(" Hà Nội <-> Hồ Chí Minh ");
                    row2.GetCell(2).CellStyle = borderFontBold;
                    row2.GetCell(3).CellStyle = borderFontBold;

                    // bắt đầu duyệt mảng và ghi tiếp tục
                    int rowIndex = 3;
                    for (var i = 0; i < list.Count - 1; i++)
                    {
                        var item = list[i];
                        // tao row mới
                        var newRowNac = sheet.CreateRow(rowIndex);

                        // set giá trị
                        newRowNac.CreateCell(0).SetCellValue(item.Name);
                        newRowNac.CreateCell(1).SetCellValue(item.LocalFee);
                        newRowNac.CreateCell(2).SetCellValue(item.IntercityFee1);
                        newRowNac.CreateCell(3).SetCellValue(item.IntercityFee2);

                        newRowNac.GetCell(0).CellStyle = borderFont;
                        newRowNac.GetCell(1).CellStyle = borderFont;
                        newRowNac.GetCell(2).CellStyle = borderFont;
                        newRowNac.GetCell(3).CellStyle = borderFont;

                        // tăng index
                        rowIndex++;
                    }

                    //Nấc
                    var itemNac = list[list.Count - 1];
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRow.CreateCell(0).SetCellValue("Mỗi nấc " + itemNac.Weight.ToString("N0") + " tiếp theo");
                    newRow.CreateCell(1).SetCellValue(itemNac.LocalFee);
                    newRow.CreateCell(2).SetCellValue(itemNac.IntercityFee1);
                    newRow.CreateCell(3).SetCellValue(itemNac.IntercityFee2);

                    newRow.GetCell(0).CellStyle = borderFont;
                    newRow.GetCell(1).CellStyle = borderFont;
                    newRow.GetCell(2).CellStyle = borderFont;
                    newRow.GetCell(3).CellStyle = borderFont;

                    sheet.AutoSizeColumn(0, true);
                    sheet.AutoSizeColumn(1, true);
                    sheet.AutoSizeColumn(2, true);
                    sheet.AutoSizeColumn(3, true);
                }
                else if (data.PostageRange == 4)
                {
                    //border
                    row0.CreateCell(4).CellStyle = borderFontBold;

                    CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 4);
                    sheet.AddMergedRegion(cellMerge);
                    row0.GetCell(0).SetCellValue("Bảng cước " + data.PostageName);
                    row0.GetCell(0).CellStyle = borderFontBold;

                    var row1 = sheet.CreateRow(1);
                    var row2 = sheet.CreateRow(2);

                    row1.CreateCell(0);
                    row2.CreateCell(0);
                    CellRangeAddress cellMerge1 = new CellRangeAddress(1, 2, 0, 0);
                    sheet.AddMergedRegion(cellMerge1);
                    row1.GetCell(0).SetCellValue(" Nấc khối lượng (gram) ");
                    row1.GetCell(0).CellStyle = borderFontBold;
                    row2.GetCell(0).CellStyle = borderFontBold;

                    row1.CreateCell(1);
                    CellRangeAddress cellMerge2 = new CellRangeAddress(1, 1, 1, 4);
                    sheet.AddMergedRegion(cellMerge2);
                    row1.GetCell(1).SetCellValue(" Mức cước liên tỉnh (VNĐ) ");
                    row1.GetCell(1).CellStyle = borderFontBold;
                    row2.GetCell(0).CellStyle = borderFontBold;

                    //border
                    row2.CreateCell(1).SetCellValue(" Nội vùng ");
                    row2.CreateCell(2).SetCellValue(" Đà Nẵng <-> Hà Nội, HCM ");
                    row2.CreateCell(3).SetCellValue(" Hà Nội <-> Hồ Chí Minh ");
                    row2.CreateCell(4).SetCellValue(" Liên vùng ");
                    row2.GetCell(1).CellStyle = borderFontBold;
                    row2.GetCell(2).CellStyle = borderFontBold;
                    row2.GetCell(3).CellStyle = borderFontBold;
                    row2.GetCell(4).CellStyle = borderFontBold;

                    // bắt đầu duyệt mảng và ghi tiếp tục
                    int rowIndex = 3;
                    for (var i = 0; i < list.Count - 1; i++)
                    {
                        var item = list[i];
                        // tao row mới
                        var newRowNac = sheet.CreateRow(rowIndex);

                        // set giá trị
                        newRowNac.CreateCell(0).SetCellValue(item.Name);
                        newRowNac.CreateCell(1).SetCellValue(item.IntercityFee1);
                        newRowNac.CreateCell(2).SetCellValue(item.IntercityFee2);
                        newRowNac.CreateCell(3).SetCellValue(item.IntercityFee3);
                        newRowNac.CreateCell(4).SetCellValue(item.IntercityFee4);

                        newRowNac.GetCell(0).CellStyle = borderFont;
                        newRowNac.GetCell(1).CellStyle = borderFont;
                        newRowNac.GetCell(2).CellStyle = borderFont;
                        newRowNac.GetCell(3).CellStyle = borderFont;
                        newRowNac.GetCell(4).CellStyle = borderFont;

                        // tăng index
                        rowIndex++;
                    }

                    //Nấc
                    var itemNac = list[list.Count - 1];
                    // tao row mới
                    var newRow = sheet.CreateRow(rowIndex);

                    // set giá trị
                    newRow.CreateCell(0).SetCellValue("Mỗi nấc " + itemNac.Weight.ToString("N0") + " tiếp theo");
                    newRow.CreateCell(1).SetCellValue(itemNac.IntercityFee1);
                    newRow.CreateCell(2).SetCellValue(itemNac.IntercityFee2);
                    newRow.CreateCell(3).SetCellValue(itemNac.IntercityFee3);
                    newRow.CreateCell(4).SetCellValue(itemNac.IntercityFee4);

                    newRow.GetCell(0).CellStyle = borderFont;
                    newRow.GetCell(1).CellStyle = borderFont;
                    newRow.GetCell(2).CellStyle = borderFont;
                    newRow.GetCell(3).CellStyle = borderFont;
                    newRow.GetCell(4).CellStyle = borderFont;

                    sheet.AutoSizeColumn(0, true);
                    sheet.AutoSizeColumn(1, true);
                    sheet.AutoSizeColumn(2, true);
                    sheet.AutoSizeColumn(3, true);
                    sheet.AutoSizeColumn(4, true);
                }
                else
                {
                    XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.ExportExcelMainServiceConfig: data = " + JsonConvert.SerializeObject(data));
                }
            }
            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            wb.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "CauHinhCuocDichVuChinh.xls");
        }
        //Cấu hình phí dịch vụ cộng thêm
        public ActionResult TransportPostageExtraServiceIndex()
        {
            return View();
        }
        public ActionResult TransportPostageExtraServiceList()
        {
            List<TransportPostageExtraServiceListViewModel> model = new List<TransportPostageExtraServiceListViewModel>();
            var res = TransportPostageRepository.SearchList(new ZTransportPostageSearchListRequest() 
            {
                Type = 2
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new TransportPostageExtraServiceListViewModel()
                    {
                        ID = item.TransportPostageID,
                        ServiceCode = item.TransportServiceCode,
                        ServiceName = item.TransportServiceName,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        IsLock = item.IsLock,
                        VAT = item.VAT.Name
                    });
                }
            }
            return View(model);
        }
        public ActionResult TransportPostageExtraServiceAdd()
        {
            TransportPostageExtraServiceAddViewModel model = new TransportPostageExtraServiceAddViewModel()
            {
                ListChargingConditions = XData.GetListChargingConditions(), // DS điều kiện tính cước DV cộng thêm
                MainServiceList = XCommon.TransportServiceList(1), // DS dịch vụ chính
                ExtraServiceList = XCommon.TransportServiceList(2) // DS dịch vụ cộng thêm
            };
            return View(model);
        }
        public ActionResult TransportPostageExtraService(int PostageID)
        {
            TransportPostageExtraServiceModel model = new TransportPostageExtraServiceModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "TRANSPORT_POSTAGE_EXTRA_SERVICE_BY_ID",
                Data = JsonConvert.SerializeObject(new ZTransportPostageExtraServiceByIDRequest()
                {
                    PostageID = PostageID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageExtraServiceByIDResponse>(res.Data);
                model = new TransportPostageExtraServiceModel()
                {
                    PostageID = item.PostageID,
                    VAT = item.VAT,
                    ServiceID = item.ServiceID,
                    ServiceName = item.ServiceName,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    ChargingConditionsID = item.ChargingConditionsID,
                    ChargingConditionsValue = item.ChargingConditionsValue,
                    ChargingConditionsMinValue = item.ChargingConditionsMinValue,
                    ServicesUsed = item.ServicesUsed
                };
            }
            ViewBag.ListChargingConditions = XData.GetListChargingConditions(); // DS điều kiện tính cước DV cộng thêm
            ViewBag.MainServiceList = XCommon.TransportServiceList(1); // DS dịch vụ chính
            ViewBag.ExtraServiceList = XCommon.TransportServiceList(2); // DS dịch vụ cộng thêm
            if (model.PostageID == 0)
            {
                return RedirectToAction("TransportPostageExtraServiceList");
            }
            return View(model);
        }
        public ActionResult TransportPostageExtraServiceDetail(int PostageID)
        {
            TransportPostageExtraServiceModel model = new TransportPostageExtraServiceModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "TRANSPORT_POSTAGE_EXTRA_SERVICE_BY_ID",
                Data = JsonConvert.SerializeObject(new ZTransportPostageExtraServiceByIDRequest()
                {
                    PostageID = PostageID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageExtraServiceByIDResponse>(res.Data);
                model = new TransportPostageExtraServiceModel()
                {
                    PostageID = item.PostageID,
                    VAT = item.VAT,
                    ServiceID = item.ServiceID,
                    ServiceName = item.ServiceName,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    ChargingConditionsID = item.ChargingConditionsID,
                    ChargingConditionsValue = item.ChargingConditionsValue,
                    ChargingConditionsMinValue = item.ChargingConditionsMinValue,
                    ServicesUsed = item.ServicesUsed
                };
            }
            ViewBag.ListChargingConditions = XData.GetListChargingConditions(); // DS điều kiện tính cước DV cộng thêm
            ViewBag.MainServiceList = XCommon.TransportServiceList(1); // DS dịch vụ chính
            ViewBag.ExtraServiceList = XCommon.TransportServiceList(2); // DS dịch vụ cộng thêm
            if (model.PostageID == 0)
            {
                return RedirectToAction("TransportPostageExtraServiceList");
            }
            return View(model);
        }

        public ActionResult ExtraServiceFeeConfigEdit(int PostageID)
        {
            TransportPostageExtraDetailViewModel model = new TransportPostageExtraDetailViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "TRANSPORT_POSTAGE_EXTRA_SERVICE_BY_ID",
                Data = JsonConvert.SerializeObject(new ZTransportPostageExtraServiceByIDRequest()
                {
                    PostageID = PostageID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageExtraServiceByIDResponse>(res.Data);
                model = new TransportPostageExtraDetailViewModel()
                {
                    PostageID = item.PostageID,
                    ServiceName = item.ServiceName,
                    ChargingConditionsID = item.ChargingConditionsID,
                    ServicesUsed = item.ServicesUsed
                };
            }
            //if (model.PostageID == 0)
            //{
            //    return RedirectToAction("TransportPostageIndex");
            //}
            return View(model);
        }
        public ActionResult ExtraServiceFeeConfigEditData(int PostageID)
        {
            List<TransportPostageExtraDetailDataViewModel> model = new List<TransportPostageExtraDetailDataViewModel>();
            var res = TransportPostageDetailRepository.GetByTransportPostageID(new ZTransportPostageDetailGetByTransportPostageIDRequest() 
            {
                TransportPostageID = PostageID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageDetailGetByTransportPostageIDResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new TransportPostageExtraDetailDataViewModel()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        IsStep = item.IsStep,
                        Weight = item.Weight,
                        LocalFee = item.LocalFee,
                        IntercityFee1 = item.IntercityFee1,
                        IntercityFee2 = item.IntercityFee2,
                        IntercityFee3 = item.IntercityFee3,
                        IntercityFee4 = item.IntercityFee4,
                        IntercityFee5 = item.IntercityFee5,
                        ChargingConditionsID = item.ChargingConditionsID
                    });
                }
            }
            return View(model);
        }

        public ActionResult ExtraServiceFeeConfigDetail(int PostageID)
        {
            ExtraServiceFeeConfigViewModel model = new ExtraServiceFeeConfigViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "TRANSPORT_POSTAGE_EXTRA_SERVICE_BY_ID",
                Data = JsonConvert.SerializeObject(new ZTransportPostageExtraServiceByIDRequest()
                {
                    PostageID = PostageID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportPostageExtraServiceByIDResponse>(res.Data);
                model = new ExtraServiceFeeConfigViewModel()
                {
                    PostageID = item.PostageID,
                    ServiceName = item.ServiceName,
                    ChargingConditionsID = item.ChargingConditionsID,
                    ServicesUsed = item.ServicesUsed
                };
            }
            return View(model);
        }
        public ActionResult ExtraServiceFeeConfigDetailData(int PostageID)
        {
            List<ExtraServiceFeeConfigDetailDataViewModel> model = new List<ExtraServiceFeeConfigDetailDataViewModel>();
            var res = TransportPostageDetailRepository.GetByTransportPostageID(new ZTransportPostageDetailGetByTransportPostageIDRequest()
            {
                TransportPostageID = PostageID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageDetailGetByTransportPostageIDResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new ExtraServiceFeeConfigDetailDataViewModel()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        IsStep = item.IsStep,
                        Weight = item.Weight,
                        LocalFee = item.LocalFee,
                        IntercityFee1 = item.IntercityFee1,
                        IntercityFee2 = item.IntercityFee2,
                        IntercityFee3 = item.IntercityFee3,
                        IntercityFee4 = item.IntercityFee4,
                        IntercityFee5 = item.IntercityFee5,
                        ChargingConditionsID = item.ChargingConditionsID
                    });
                }
            }
            return View(model);
        }

        //public JsonResult JTransportPostageExtraServiceAdd(TransportPostageExtraServiceAddModel data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "TRANSPORT_POSTAGE_EXTRA_SERVICE_BASIC_ADD",
        //            Data = JsonConvert.SerializeObject(new ZTransportPostageExtraServiceBasicAddRequest()
        //            {
        //                //ExtraServiceIDs = data.ExtraServiceIDs,
        //                //ExtraTransportServiceID = string.Join(",", data.ExtraServiceIDs),
        //                VAT = data.VAT,
        //                StartDate = data.StartDate,
        //                EndDate = data.EndDate,
        //                ChargingConditionsID = data.ChargingConditionsID,
        //                ChargingConditionsValue = data.ChargingConditionsValue,
        //                ChargingConditionsMinValue = data.ChargingConditionsMinValue,
        //                ServicesUsed = data.ServicesUsed,
        //                CreatedBy = XHelper.Session.UserProfile.ID
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageDetailAdd: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult JTransportPostageExtraServiceEdit(TransportPostageExtraServiceModel data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "TRANSPORT_POSTAGE_EXTRA_SERVICE_BASIC_EDIT",
        //            Data = JsonConvert.SerializeObject(new ZTransportPostageExtraServiceEditRequest()
        //            {
        //                TransportPostageID = data.PostageID,
        //                VAT = data.VAT,
        //                StartDate = data.StartDate,
        //                EndDate = data.EndDate,
        //                ChargingConditionsID = data.ChargingConditionsID,
        //                ChargingConditionsValue = data.ChargingConditionsValue,
        //                ChargingConditionsMinValue = data.ChargingConditionsMinValue,
        //                ServicesUsed = data.ServicesUsed,
        //                UpdatedBy = XHelper.Session.UserProfile.ID
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JTransportPostageExtraService: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult TransportPostageHistory(int PostageID)
        {
            List<TransportPostageHistoryViewModel> model = new List<TransportPostageHistoryViewModel>();
            var res = TransportPostageRepository.SearchHistory(new ZTransportPostageSearchHistoryRequest() 
            {
                TransportPostageID = PostageID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportPostageSearchHistoryResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new TransportPostageHistoryViewModel()
                    {
                        Name = item.Name,
                        Description = item.Description,
                        CreatedName = item.CreatedName,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return View(model);
        }
        #endregion

        //#region Charged
        //public ActionResult ChargedIndex()
        //{
        //    TransportFeeViewModel Model = new TransportFeeViewModel() 
        //    {
        //        //ListMainTransportService = XCommon.TransportServiceList(1),
        //        //ListExtraTransportService = XCommon.TransportServiceList(2),
        //        ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
        //        ListDistrict = new List<DistrictModel>(),
        //        ListWard = new List<WardModel>()
        //    };
        //    return View(Model);
        //}

        //public JsonResult JCharged(ChargedModel data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "TRANSPORT_POSTAGE_CHARGED",
        //            Data = JsonConvert.SerializeObject(new ZTransportPostageChargedRequest()
        //            {
        //                MainServiceID = data.MainServiceID,
        //                ExtraServiceIDs = data.ExtraServiceIDs,
        //                FromProvinceID = data.FromProvinceID,
        //                ToProvinceID = data.ToProvinceID,
        //                ToDistrictID = data.ToDistrictID,
        //                ToWardID = data.ToWardID,
        //                Weight = data.Weight,
        //                CustomerCode = data.CustomerCode,
        //                Quantity = data.Quantity,
        //                Amount = data.Amount,
        //                COD = data.COD
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.JCharged: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region Transport A
        #endregion

        #region Transport B
        #endregion
    }
}