using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Repositories;
namespace EMS.MVC.Frontend.Controllers
{
    public class ShopController : BaseController
    {
        #region Thông tin tài khoản
        public ActionResult ShopView(string Message)
        {
            List<FunctionalModel> ListTab = new List<FunctionalModel>();
            if (XHelper.Session.EmployeeProfile.StoreOwner == "N")
            {
                var res = PermissionRepository.SearchListMenu(new ZFunctionalListMenuRequest() { EmployeeID = XHelper.Session.EmployeeProfile.ID, ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                if (res.Code == "00")
                {
                    var ListItem = JsonConvert.DeserializeObject<List<ZFunctionalListMenuResponse>>(res.Data).Where(m => m.FunctionalCode == "Shop_Index" && m.FunctionalLevel == 0).ToList();
                    if (ListItem.Count > 0)
                    {
                        foreach (var Item in ListItem.First().ListFunctional)
                        {
                            ListTab.Add(new FunctionalModel()
                            {
                                FunctionalCode = Item.FunctionalCode
                            });
                        }
                    }
                }
            }

            ShopViewModel model = new ShopViewModel()
            {
                ListTab = ListTab,
                Message = Message
            };
            return View(model);
        }

        public ActionResult ShopProfileView()
        {
            ShopProfileViewModel model = null;
            var res = ShopRepository.GetByShopID(new ZShopGetByIDRequest() { ID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(res.Data);
                model = new ShopProfileViewModel()
                {
                    StoreName = item.StoreName,
                    Email = item.Email,
                    MobileNumber = item.MobileNumber,
                    Code = item.Code,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    AccountType = XData.GetNameAccountType(item.AccountType.ToString()),
                    NumberOfEmployees = item.NumberOfEmployees,
                    NumberOfEmployeesCreated = item.NumberOfEmployeesCreated,
                    PickUpFullAddress = item.PickUpFullAddress,
                    PaymentMethod = item.PaymentMethod,
                    PaymentType = item.PaymentType,
                    FullNameOfPayment = item.FullNameOfPayment,
                    MobileNumberOfPayment = item.MobileNumberOfPayment,
                    NameOfPayment = item.NameOfPayment,
                    BranchOfPayment = item.BranchOfPayment,
                    NumberOfPayment = item.NumberOfPayment,
                    CollectionType = item.CollectionType,
                    PickUpID = item.PickUpID,
                    //CustomerCode = item.CustomerCode,
                    ChildCustomerCode = item.ChildCustomerCode,
                    ChildCustomerCodeQuantity = item.ChildCustomerCode.Split(',').Count(),
                    CrmOrPaypostCode = item.CustomerCode
                };
            }
            return PartialView(model);
        }

        #endregion

        #region Đổi địa chỉ lấy hàng
        [AuthorizeController(FunctionalTitle = "DOI_DIA_CHI_LAY_HANG_CUA_CUA_HANG")]
        public ActionResult ChangePickUpAddressView()
        {
            ChangePickUpAddressViewModel model = new ChangePickUpAddressViewModel();
            model.ListPickUpAddress = XCommon.SearchListPickUpAddress(new ZPickUpAddressSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            var PickUpAddress = model.ListPickUpAddress.Where(m => m.IsDefault == "Y").First();
            model.PickUpAddressID = PickUpAddress.PickUpAddressID;
            model.FullName = PickUpAddress.FullName;
            model.MobileNumber = PickUpAddress.MobileNumber;
            model.PickUpName = PickUpAddress.PickUpName;
            model.FullAddress = PickUpAddress.FullAddress;
            model.ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID;
            var res = ShopRepository.GetByShopID(new ZShopGetByIDRequest() { ID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(res.Data);
                if (item.TypeShop == 2)
                {
                    model.TypeShop = item.TypeShop;
                    model.ProvinceID = PickUpAddress.Province.ID;
                    model.ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest());
                    //model.DistrictID = PickUpAddress.District.ID;
                    //model.ListDistrict = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = PickUpAddress.Province.ID });
                    model.WardID = PickUpAddress.Ward.ID;
                    model.ListWard = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = PickUpAddress.Province.ID });
                    model.Street = PickUpAddress.Street;
                    model.CollectionPostOfficeID = PickUpAddress.CollectionPostOfficeID;
                    model.ListCollectionPostOffice = XCommon.SearchListCollectionPostOffice(new ZCollectionPostOfficeSearchListRequest() { IsLock = "N" });
                    model.isDefault = true;
                }
                else
                {
                    model.ListProvince = new List<ProvinceModel>();
                    //model.ListDistrict = new List<DistrictModel>();
                    model.ListWard = new List<WardModel>();
                    model.ListCollectionPostOffice = new List<CollectionPostOfficeModel>();
                }
            }
            return PartialView(model);
        }

        public JsonResult JChangePickUpAddress(ChangePickUpAddressViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.ChangePickUpAddress(new ZShopChangePickUpAddressRequest() 
                {
                    PickUpAddressID = req.PickUpAddressID,
                    PickUpName = req.PickUpName,
                    CollectionPostOfficeID = req.CollectionPostOfficeID,
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    Street = req.Street,
                    ProvinceID = req.ProvinceID,
                    //DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    IsDefault = req.isDefault == true ? "Y" : "N",
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
                if (res.Code == "00" && req.isDefault == true)
                {
                    var resPickupInfomation = ShopRepository.SearchPickUpInformation(new ZShopGetPickupInformationRequest() { ID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                    if (resPickupInfomation.Code == "00")
                    {
                        var item = JsonConvert.DeserializeObject<ZShopGetPickupInformationResponse>(resPickupInfomation.Data);
                        XHelper.Session.EmployeeProfile.ShopInformation.PickupAddressID = item.PickupAddressID;
                        XHelper.Session.EmployeeProfile.ShopInformation.PickupName = item.PickupName;
                        XHelper.Session.EmployeeProfile.ShopInformation.PickupFullName = item.PickupFullName;
                        XHelper.Session.EmployeeProfile.ShopInformation.PickupMobileNumber = item.PickupMobileNumber;
                        XHelper.Session.EmployeeProfile.ShopInformation.PickupProvinceID = item.PickupProvinceID;
                        //XHelper.Session.EmployeeProfile.ShopInformation.PickupDistrictID = item.PickupDistrictID;
                        XHelper.Session.EmployeeProfile.ShopInformation.PickupWardID = item.PickupWardID;
                        XHelper.Session.EmployeeProfile.ShopInformation.PickupStreet = item.PickupStreet;
                        XHelper.Session.EmployeeProfile.ShopInformation.PickupFullAddress = item.PickupFullAddress;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JChangePickUpAddress: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cập nhật thông tin thanh toán
        [AuthorizeController(FunctionalTitle = "CAP_NHAT_THONG_TIN_THANH_TOAN_CUA_CUA_HANG")]
        public JsonResult JUpdatePaymentInformation(ZShopUpdatePaymentInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.UpdatePaymentInformation(new ZShopUpdatePaymentInformationRequest()
                {
                    ID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    PaymentMethod = req.PaymentMethod,
                    PaymentType = req.PaymentType,
                    FullNameOfPayment = req.FullNameOfPayment,
                    MobileNumberOfPayment = req.MobileNumberOfPayment,
                    NameOfPayment = req.NameOfPayment,
                    BranchOfPayment = req.BranchOfPayment,
                    NumberOfPayment = req.NumberOfPayment,
                    CollectionType = req.CollectionType,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
                if (res.Code == "00")
                {
                    XHelper.Session.EmployeeProfile.ShopInformation.CollectionType = req.CollectionType;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.JUpdatePaymentInformation: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult TreeView()
        {
            //Tạm đóng cây phân cấp vì chưa triển khai

            //List<ShopLevelTreeViewModel> oListItem = new List<ShopLevelTreeViewModel>();
            //var res = XHelper.Invoke(new ZRequest()
            //{
            //    Code = "SHOP_LEVEL_GET_BY_PARENT_ID",
            //    Data = JsonConvert.SerializeObject(new ZShopLevelGetByParentIDRequest()
            //    {
            //        ParentID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            //    })
            //});
            //if (res.Code == "00")
            //{
            //    var list = JsonConvert.DeserializeObject<List<ZShopLevelGetByParentIDResponse>>(res.Data);
            //    foreach (var item in list)
            //    {
            //        oListItem.Add(new ShopLevelTreeViewModel()
            //        {
            //            ParentID = item.ID,
            //            Name = item.Name,
            //            ShopLevel = item.ShopLevel
            //        });
            //    }
            //}
            ViewBag.ParentID = XHelper.Encrypt(XHelper.Session.EmployeeProfile.ShopInformation.ID.ToString());
            ViewBag.ShopLevel = XHelper.Session.EmployeeProfile.ShopInformation.Level;
            ViewBag.Name = XHelper.Session.EmployeeProfile.ShopInformation.StoreName;
            return PartialView();//oListItem
        }
    }
}