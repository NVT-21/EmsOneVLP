using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
namespace EMS.MVC.Backend.Controllers
{
    public class CollectionPostOfficeController : BaseController
    {
        public ActionResult CollectionPostOfficeView()
        {
            UserProfile session = new UserProfile();
            if(XHelper.SessionUser != null)
            {
                session = (UserProfile)Session[XHelper.SessionUser];
            }

            // Kiểm tra quyền, nếu không phải admin thì return null view
            if(session.GroupCode != ".0.0.")
            {
                return new HttpStatusCodeResult(403, "Bạn không có quyền truy cập."); ; // hoặc return new EmptyResult(); tùy theo yêu cầu
            }
            CollectionPostOfficeViewModel model = new CollectionPostOfficeViewModel();
            return View(model);
        }

        public JsonResult JSearchListCollectionPostOfficeTree(CollectionPostOfficeViewModel req)
        {
            List<TreeModel> tree = new List<TreeModel>();
            try
            {
                var res = CollectionPostOfficeRepository.SearchListByParentID(new ZCollectionPostOfficeSearchListByParentIDRequest()
                {
                    ParentID = req.ParentID
                });
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCollectionPostOfficeSearchListByParentIDResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        var CollectionPostOfficeName = item.CollectionPostOfficeName;
                        if (item.ParentID > 0)
                        {
                            CollectionPostOfficeName = item.CollectionPostOfficeCode + " - " + item.CollectionPostOfficeName;
                        }
                        tree.Add(new TreeModel()
                        {
                            id = item.ID.ToString(),
                            parent = item.ParentID == 0 ? "#" : item.ParentID.ToString(),
                            text = CollectionPostOfficeName,
                            children = item.NumberOfChild > 0 ? true : false,
                            state = new TreeStateModel() { opened = false },
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.JSearchListCollectionPostOfficeTree: " + ex.Message);
            }
            return Json(tree, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailCollectionPostOfficeView(CollectionPostOfficeViewModel req)
        {
            DetailCollectionPostOfficeViewModel model = null;
            var res = CollectionPostOfficeRepository.GetByID(new ZCollectionPostOfficeGetByIDRequest() { ID = req.CollectionPostOfficeID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZCollectionPostOfficeGetByIDResponse>(res.Data);
                model = new DetailCollectionPostOfficeViewModel()
                {
                    ID = item.ID,
                    CollectionPostOfficeCode = item.CollectionPostOfficeCode,
                    CollectionPostOfficeName = item.CollectionPostOfficeName,
                    CollectionPostOfficeType = new BaseModel()
                    {
                        Code = item.CollectionPostOfficeType.Code,
                        Name = item.CollectionPostOfficeType.Name,
                    },
                    ListCollectionPostOfficeType = XData.GetListCollectionPostOfficeType(),
                    ProvinceID = item.ProvinceID,
                    ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                    //DistrictID = item.DistrictID,
                    //ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = item.ProvinceID }),
                    WardID = item.WardID,
                    ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = item.ProvinceID }),
                    Street = item.Street,
                    ParentID = item.ParentID,
                    IsLock = new BaseModel() 
                    { 
                        Code = item.IsLock.Code, 
                        Name = item.IsLock.Name 
                    },
                    ListIsLock = XData.GetListIsLock(),
                    RegionName = item.ParentID == 0 ? "Khu vực" : "Bưu cục",
                    Email = item.Email,
                    UserCMS = item.UserCMS
                };
            }
            return PartialView(model);
        }

        public ActionResult AddCollectionPostOfficeView(DetailCollectionPostOfficeViewModel req)
        {
            AddCollectionPostOfficeViewModel model = new AddCollectionPostOfficeViewModel() 
            {
                ParentID = req.ParentID,
                RegionName = req.ParentID == 0 ? "khu vực" : "bưu cục",
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                //ListDistrict = new List<DistrictModel>(),
                ListWard = new List<WardModel>(),
                ListIsLock = XData.GetListIsLock(),
                ListCollectionPostOfficeType = XData.GetListCollectionPostOfficeType(),
            };
            return PartialView(model);
        }

        public JsonResult JAddCollectionPostOffice(AddCollectionPostOfficeViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CollectionPostOfficeRepository.AddCollectionPostOffice(new ZCollectionPostOfficeAddRequest() 
                {
                    CollectionPostOfficeCode = req.CollectionPostOfficeCode,
                    CollectionPostOfficeName = req.CollectionPostOfficeName,
                    CollectionPostOfficeType = req.CollectionPostOfficeType,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street,
                    IsLock = req.IsLock,
                    ParentID = req.ParentID,
                    Email = req.Email,
                    UserCMS = req.UserCMS,
                    CreatedBy = XHelper.Session.UserProfile.ID,
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.JAddCollectionPostOffice: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JEditCollectionPostOffice(DetailCollectionPostOfficeViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CollectionPostOfficeRepository.EditCollectionPostOffice(new ZCollectionPostOfficeEditRequest() 
                {
                    CollectionPostOfficeID = req.ID,
                    CollectionPostOfficeName = req.CollectionPostOfficeName,
                    CollectionPostOfficeType = Convert.ToInt32(req.CollectionPostOfficeType.Code),
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID= req.WardID,
                    Street = req.Street,
                    IsLock = req.IsLock.Code,
                    Email = req.Email,
                    UserCMS = req.UserCMS,
                    UpdatedBy = XHelper.Session.UserProfile.ID,
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.JEditCollectionPostOffice: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}