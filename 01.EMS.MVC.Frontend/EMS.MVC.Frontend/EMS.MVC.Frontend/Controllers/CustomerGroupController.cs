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
    public class CustomerGroupController : BaseController
    {
        #region Danh sách nhóm khách hàng
        [AuthorizeController(FunctionalTitle = "DANH_SACH_NHOM_KHACH_HANG")]
        public ActionResult CustomerGroupView()
        {
            CustomerGroupViewModel model = new CustomerGroupViewModel() 
            {
                ListCustomerGroup = XCommon.SearchListCustomerGroup(new ZCustomerSearchGroupRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
            };           
            return View(model);
        }

        public ActionResult SearchListCustomerGroupView(CustomerGroupViewModel req)
        {
            List<SearchListCustomerGroupViewModel> model = new List<SearchListCustomerGroupViewModel>();
            var res = CustomerGroupRepository.SearchListCustomerGroup(new ZCustomerSearchGroupRequest()
            {        
                GroupCode = req.GroupCode,
                GroupName = req.GroupName,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if(res.Code == "00")
            {
                var ListItem = JsonConvert.DeserializeObject<List<ZCustomerSearchGroupResponse>>(res.Data);
                foreach (var item in ListItem)
                {
                    model.Add(new SearchListCustomerGroupViewModel()
                    {
                        CustomerGroup = new CustomerGroupModel() 
                        {
                            ID = item.ID,
                            Code = item.GroupCode,
                            Name = item.GroupName
                        },
                        OrderSuccess = item.OrderSuccess,
                        PurchaseMoney = item.PurchaseMoney
                    });
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Thêm nhóm khách hàng
        [AuthorizeController(FunctionalTitle = "THEM_NHOM_KHACH_HANG")]
        public ActionResult AddCustomerGroupView()
        {
            return View();
        }

        public JsonResult JAddNewCustomerGroup(AddCustomerGroupViewModel model)
        {
            ZResponse res = new ZResponse();            
            try
            {
                res = CustomerGroupRepository.AddNewCustomerGroup(new ZCustomerAddNewGroupRequest()
                {
                    GroupCode = model.GroupCode,
                    GroupName = model.GroupName,
                    OrderSuccess = model.OrderSuccess,
                    PurchaseMoney = model.PurchaseMoney,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupController.JAddCustomerGroup: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chi tiết nhóm khách hàng
        //[AuthorizeController(FunctionalTitle = "CHI_TIET_NHOM_KHACH_HANG")]
        //public ActionResult DetailCustomerGroupView(int ID) 
        //{
        //    CustomerDetailViewModel model = new CustomerDetailViewModel();
        //    var res = XHelper.Invoke(new ZRequest()
        //    {
        //        Code = "CUSTOMER_GET_BY_ID",
        //        Data = JsonConvert.SerializeObject(new ZCustomerGetByIdRequest()
        //        {
        //            ID = ID,
        //            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
        //        })
        //    });
        //    if(res.Code == "00")
        //    {
        //        var item = JsonConvert.DeserializeObject<ZCustomerGetByIdResponse>(res.Data);
        //        model = new CustomerDetailViewModel()
        //        {
        //            ID = item.ID,
        //            CustomerCode = item.CustomerCode,
        //            GroupName = item.GroupName,
        //            FullName = item.FullName,
        //            MobileNumber = item.MobileNumber,
        //            Email = item.Email,
        //            BirthDay = item.BirthDay,
        //            TaxCode = item.TaxCode,
        //            Channel = item.Channel,
        //            Note = item.Note,
        //            Sticker = item.Sticker,
        //            ProvinceID = item.ProvinceID,
        //            DistrictID = item.DistrictID,
        //            WardID = item.WardID,
        //            Address = item.Address
        //        };
        //    }
        //    ViewBag.ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());
        //    ViewBag.ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictListRequest() { ProvinceID = model.ProvinceID });
        //    ViewBag.ListWard = XCommon.SearchListWardByDistrictID(new ZWardListRequest() { DistrictID = model.DistrictID });
        //    ViewBag.ListChannel = XData.GetListChannel();
        //    ViewBag.ListCustomerSticker = XCommon.GetListCustomerStickerName();
        //    return View(model);
        //}

        public ActionResult ShowCustomerInGroup(string GroupName)
        {
            TempData["GroupName"] = GroupName;
            return RedirectToAction("CustomerView", "Customer");
            //List<SearchListCustomerGroupModel> model = new List<SearchListCustomerGroupModel>();
            //var res = CustomerRepository.SearchListCustomer(new ZCustomerListRequest()
            //{
            //    SearchKey = SearchKey,
            //    CustomerGroupName = GroupName,
            //    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            //});
            //if (res.Code == "00")
            //{
            //    var ListItem = JsonConvert.DeserializeObject<List<ZCustomerListResponse>>(res.Data);
            //    var stt = 1;
            //    foreach (var item in ListItem)
            //    {
            //        model.Add(new SearchListCustomerGroupModel()
            //        {
            //            Index = stt++,
            //            ID = item.ID,
            //            CustomerCode = item.CustomerCode,
            //            FullName = item.FullName,
            //            MobileNumber = item.MobileNumber,
            //            Email = item.Email,
            //            Address = item.Address,
            //            BirthDay = item.BirthDay,
            //            TaxCode = item.TaxCode,
            //            Channel = item.Channel,
            //            Note = item.Note,
            //            GroupName = item.GroupName,
            //            Sticker = item.Sticker,
            //            BlackList = item.BlackList,
            //            ProvinceName = item.ProvinceName,
            //            DistrictName = item.DistrictName,
            //            WardName = item.WardName,
            //            CountOrderSuccess = item.CountOrderSuccess,
            //            TotalAmountSuccess = item.TotalAmountSuccess
            //        });
            //    }
            //}
            //ViewBag.GroupName = GroupName;
            //return PartialView(model);
        }
        #endregion
    }
}