using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
namespace EMS.MVC.Frontend.Controllers
{
    public class CustomerStickerController : BaseController
    {
        #region Danh sách nhãn khách hàng
        [AuthorizeController(FunctionalTitle = "DANH_SACH_NHAN_KHACH_HANG")]
        public ActionResult CustomerStickerView()
        {
            CustomerStickerViewModel model = new CustomerStickerViewModel()
            {
                ListCustomerSticker = XCommon.SearchListCustomerSticker(new ZCustomerStickerListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
            };
            return View(model);
        }

        public ActionResult SearchListCustomerStickerView(CustomerStickerViewModel req)
        {
            List<SearchListCustomerStickerViewModel> model = new List<SearchListCustomerStickerViewModel>();
            var res = CustomerStickerRepository.SearchListCustomerSticker(new ZCustomerStickerListRequest()
            {
                StickerCode = req.StickerCode,
                StickerName = req.StickerName,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var ListItem = JsonConvert.DeserializeObject<List<ZCustomerStickerListResponse>>(res.Data);
                foreach (var item in ListItem)
                {
                    model.Add(new SearchListCustomerStickerViewModel()
                    {
                        ID = item.ID,
                        CustomerSticker = new XBaseModel() 
                        {
                            Code = item.StickerCode,
                            Name = item.StickerName
                        },
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Thêm nhãn khách hàng
        [AuthorizeController(FunctionalTitle = "THEM_NHAN_KHACH_HANG")]
        public ActionResult AddCustomerStickerView()
        {
            AddCustomerStickerViewModel model = new AddCustomerStickerViewModel()
            {
                ListCustomer = XCommon.SearchListCustomer(new ZCustomerSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
            };
            return View(model);
        }

        public JsonResult JAddCustomerSticker(AddCustomerStickerViewModel model)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CustomerStickerRepository.AddCustomerSticker(new ZCustomerStickerAddRequest()
                {
                    StickerCode = model.StickerCode,
                    StickerName = model.StickerName,
                    strCustomerID = model.CustomerID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerStickerController.JAddCustomerSticker: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa nhãn khách hàng
        [AuthorizeController(FunctionalTitle = "XOA_NHAN_KHACH_HANG")]
        public JsonResult JRemoveCustomerSticker(SearchListCustomerStickerViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CustomerStickerRepository.RemoveCustomerSticker(new ZCustomerStickerRemoveRequest() 
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerStickerController.JRemoveCustomerSticker: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}