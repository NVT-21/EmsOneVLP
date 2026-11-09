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
    public class PromotionController : BaseController
    {
        #region Danh sách khuyến mãi
        [AuthorizeController(FunctionalTitle = "DANH_SACH_KHUYEN_MAI")]
        public ActionResult PromotionView()
        {
            ViewBag.ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID;
            return View();
        }

        public ActionResult SearchListPromotionView(int ShopID)
        {
            List<PromotionViewModel> model = new List<PromotionViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PROMOTION_LIST",
                Data = JsonConvert.SerializeObject(new ZPromotionListRequest()
                {
                    ShopID = ShopID
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZPromotionListResponse>>(res.Data);
                foreach (var item in list)
                {
                    //if (item.PromotionCode != null && item.PromotionCode != "")
                    //{
                    model.Add(new PromotionViewModel()
                    {
                        ID = item.ID,
                        PromotionName = item.PromotionName,
                        PromotionCode = item.PromotionCode,
                        PromotionCodeFirstName = item.PromotionCodeFirstName,
                        PromotionCodeEndName = item.PromotionCodeEndName,
                        AllowUse = item.AllowUse,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        IsLock = item.IsLock,
                        PromotionTypeOption = item.PromotionTypeOption,
                        Reduction = item.Reduction,
                        MaxReduction = item.MaxReduction,
                        ApplyForOption = item.ApplyForOption,
                        AfoChild = item.AfoChild,
                        NumberOfUse = item.NumberOfUse,
                        NumberOfUseName = item.NumberOfUseName,
                        NumberOfCustomerUse = item.NumberOfCustomerUse,
                        NumberOfCustomerUseName = item.NumberOfCustomerUseName,
                        SubjectsOfApplication = item.SubjectsOfApplication,
                        ScopeOfUse = item.ScopeOfUse,
                        CreatedDate = item.CreatedDate,
                        EmployeeName = item.EmployeeName,
                        ProductGroupName = item.ProductGroupName,
                        ProductName = item.ProductName,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerMobileNumber = item.CustomerMobileNumber,
                        SubjectsOfApplicationName = item.SubjectsOfApplicationName
                    });
                }
            }
            return PartialView(model);
        }

        public ActionResult PromotionVariations(string IDs)
        {
            List<PromotionProductVariantViewModel> model = new List<PromotionProductVariantViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PRODUCT_VARIANT_GET_BY_IDS",
                Data = JsonConvert.SerializeObject(new ZProductVariantGetByIDsRequest
                {
                    IDs = IDs
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZProductVariantGetByIDsResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new PromotionProductVariantViewModel()
                    {
                        VariantID = item.VariantID,
                        VariantName = item.VariantName,
                        VariantImage = item.VariantImage,
                        ProductName = item.ProductName
                    });
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Thêm khuyến mãi
        [AuthorizeController(FunctionalTitle = "THEM_KHUYEN_MAI")]
        public ActionResult AddPromotionView()
        {
            return PartialView();
        }

        public ActionResult AddPromotionCode()
        {
            AddPromotionCodeViewModel model = new AddPromotionCodeViewModel()
            {
                ListCustomer = XCommon.SearchListCustomer(new ZCustomerSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                ListCustomerGroup = XCommon.SearchListCustomerGroup(new ZCustomerSearchGroupRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                ListProductGroup = XCommon.SearchListProductGroup(new ZProductGroupListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
            };
            ViewBag.ListPromotionCodeType = XData.GetListPromotionCodeType();
            ViewBag.ListPromotionCodeApplyForOption = XData.GetListPromotionCodeApplyForOption();
            //ViewBag.ListCustomerGroup = XCommon.GetListCustomerGroupName();
            ViewBag.ListPromotionScopeOfUse = XData.GetListPromotionScopeOfUse();
            //ViewBag.ListProductGroup = XCommon.GetListProductGroupName();
            ViewBag.ListProduct = XCommon.GetListProductName();
            //ViewBag.ListCustomer = XCommon.GetListEmailCustomer();
            //ViewBag.ListProvinces = XCommon.SearchListProvince(new ZProvinceListRequest());
            ViewBag.ListProductVariant = XCommon.GetListProductVariant();
            ViewBag.ListNumberOfUse = XData.PromotionGetListNumberOfUse();
            return View(model);
        }

        public ActionResult PromotionProductVariant(List<string> list, string selectID = "")
        {
            List<PromotionProductVariantViewModel> model = new List<PromotionProductVariantViewModel>();
            //var list = data.Split(',');
            foreach (var ProductVariantID in list)
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PRODUCT_VARIANT_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(new ZProductVariantGetByIDRequest
                    {
                        ID = Convert.ToInt32(ProductVariantID),
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                    })
                });
                if (res.Code == "00")
                {
                    var item = JsonConvert.DeserializeObject<ZProductVariantGetByIDResponse>(res.Data);
                    model.Add(new PromotionProductVariantViewModel()
                    {
                        VariantID = item.ID,
                        VariantName = item.Options,
                        VariantImage = item.Avatar,
                        ProductName = item.ProductName
                    });
                }
            }
            ViewBag.selectID = selectID;
            return PartialView(model);
        }

        public JsonResult JAddPromotion()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PromotionController.JAddPromotion: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ngừng khuyến mãi
        [AuthorizeController(FunctionalTitle = "NGUNG_KHUYEN_MAI")]
        public JsonResult JChangeIsLock()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PromotionController.JChangeIsLock: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa khuyến mãi
        [AuthorizeController(FunctionalTitle = "XOA_KHUYEN_MAI")]
        public JsonResult JRemovePromotion()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PromotionController.JRemovePromotion: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}