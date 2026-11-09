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
    public class ProductGroupController : BaseController
    {
        #region Danh sách nhóm sản phẩm
        [AuthorizeController(FunctionalTitle = "DANH_SACH_NHOM_SAN_PHAM")]
        public ActionResult ProductGroupView()
        {
            return View();
        }

        public ActionResult SearchListProductGroupView(ProductGroupViewModel req)
        {
            List<SearchListProductGroupViewModel> listItem = new List<SearchListProductGroupViewModel>();
            var res = ProductGroupRepository.SearchListProductGroup(new ZProductGroupListRequest() 
            {
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if(res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZProductGroupListResponse>>(res.Data);
                foreach (var item in list)
                {
                    listItem.Add(new SearchListProductGroupViewModel()
                    {
                        ProductGroupID = item.ID,
                        ProductGroupCode = item.Code,
                        ProductGroupName = item.Name,
                        Description = item.Description,
                        EmployeeName = item.EmployeeName,
                        CreatedDate = item.CreatedDate,
                        CountProduct = item.CountProduct,
                        TotalPriceProduct = item.TotalPriceProduct
                    });
                }
            }
            return PartialView(listItem);
        }
        #endregion

        #region Thêm nhóm sản phẩm
        [AuthorizeController(FunctionalTitle = "THEM_NHOM_SAN_PHAM")]
        public ActionResult AddProductGroupView()
        {
            return View();
        }

        public JsonResult JAddProductGroup(AddProductGroupViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ProductGroupRepository.AddProductGroup(new ZProductGroupAddRequest() 
                {
                    ProductGroupCode = req.ProductGroupCode,
                    ProductGroupName = req.ProductGroupName,
                    Description = req.Description,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupController.JAddProductGroup: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa nhóm sản phẩm
        [AuthorizeController(FunctionalTitle = "SUA_NHOM_SAN_PHAM")]
        public ActionResult EditProductGroupView(SearchListProductGroupViewModel req)
        {
            EditProductGroupViewModel model = new EditProductGroupViewModel();
            var ProductGroupID = Security.DecodeBase64(req.SearchKey);
            var res = ProductGroupRepository.SearchProductGroupByID(new ZProductGroupGetByIDRequest() 
            {
                ProductGroupID = Convert.ToInt32(ProductGroupID),
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if(res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZProductGroupGetByIDResponse>(res.Data);
                model = new EditProductGroupViewModel()
                {
                    ProductGroupID = item.ProductGroupID,
                    ProductGroupCode = item.ProductGroupCode,
                    ProductGroupName = item.ProductGroupName,
                    Description = item.Description
                };
            }
            return View(model);
        }

        public JsonResult JEditProductGroup(EditProductGroupViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ProductGroupRepository.EditProductGroup(new ZProductGroupEditRequest() 
                {
                    ProductGroupID = req.ProductGroupID,
                    ProductGroupName = req.ProductGroupName,
                    Description = req.Description,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProductGroupController.JEditProductGroup: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách sản phẩm nằm trong nhóm sản phẩm
        [AuthorizeController(FunctionalTitle = "CHI_TIET_NHOM_SAN_PHAM")]
        public ActionResult SearchListProductView(SearchListProductGroupViewModel req)
        {
            List<SearchListProductViewModel> listItem = new List<SearchListProductViewModel>();
            var ProductGroupID = Security.DecodeBase64(req.SearchKey);
            var res = ProductGroupRepository.SearchListProduct(new ZProductGroupSearchListProductRequest() 
            {
                ProductGroupID = Convert.ToInt32(ProductGroupID),
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if(res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZProductGroupSearchListProductResponse>>(res.Data);
                foreach(var item in list)
                {
                    listItem.Add(new SearchListProductViewModel() 
                    {
                        ProductID = item.ProductID,
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        PriceProduct = item.ProductPriceSell,
                        Description = item.Description
                    });
                }
            }
            return View(listItem);
        }
        #endregion

    }
}