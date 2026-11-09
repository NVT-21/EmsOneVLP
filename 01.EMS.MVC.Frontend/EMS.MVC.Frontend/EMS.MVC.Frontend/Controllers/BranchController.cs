using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Repositories;
namespace EMS.MVC.Frontend.Controllers
{
    public class BranchController : BaseController
    {
        public ActionResult BranchView()
        {
            BranchViewModel model = new BranchViewModel()
            {
                ParentID = XHelper.Encrypt(XHelper.Session.EmployeeProfile.ShopInformation.ID.ToString())
            };
            return PartialView(model);
        }
        public ActionResult BranchInformation(string ParentID)
        {
            BranchInformationViewModel model = null;
            var res = ShopRepository.GetByShopID(new ZShopGetByIDRequest() { ID = Convert.ToInt32(XHelper.Decrypt(ParentID)) });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(res.Data);
                model = new BranchInformationViewModel()
                {
                    ID = XHelper.Encrypt(item.ID.ToString()),
                    Code = item.Code,
                    StoreName = item.StoreName,
                    MobileNumber = item.MobileNumber,
                    Description = item.Description,
                    IsLock = item.IsLock,
                    ShopLevel = item.ShopLevel,
                    PickUpID = item.PickUpID,
                    PickUpFullAddress = item.PickUpFullAddress,
                    ListPickUpAddress = XCommon.SearchListPickUpAddress(new ZPickUpAddressSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID })
                };
            };
            return PartialView(model);
        }

        public JsonResult JEditBranch(ZShopLevelEditViewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.ShopLevelEdit(new ZShopLevelEditRequest() {
                    ID = Convert.ToInt32(XHelper.Decrypt(req.ID)),
                    Name = req.Name,
                    MobileNumber = req.MobileNumber,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Address = req.Address,
                    IsLock = req.IsLock,
                    Description = req.Description,
                    NameAddress = req.NameAddress,
                    PickupAddress = req.PickupAddress,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });

                if (res.Code == "00")
                {
                    if (XHelper.Session.EmployeeProfile.ShopInformation.ID == Convert.ToInt32(XHelper.Decrypt(req.ID)))
                    {
                        XHelper.Session.EmployeeProfile.ShopInformation.StoreName = req.Name;
                        var PickupInfomation = ShopRepository.SearchPickUpInformation(new ZShopGetPickupInformationRequest() { ID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
                        if (PickupInfomation.Code == "00")
                        {
                            var Item = JsonConvert.DeserializeObject<ZShopGetPickupInformationResponse>(PickupInfomation.Data);
                            XHelper.Session.EmployeeProfile.ShopInformation.PickupAddressID = Item.PickupAddressID;
                            XHelper.Session.EmployeeProfile.ShopInformation.PickupFullName = Item.PickupFullName;
                            XHelper.Session.EmployeeProfile.ShopInformation.PickupMobileNumber = Item.PickupMobileNumber;
                            XHelper.Session.EmployeeProfile.ShopInformation.PickupProvinceID = Item.PickupProvinceID;
                            XHelper.Session.EmployeeProfile.ShopInformation.PickupDistrictID = Item.PickupDistrictID;
                            XHelper.Session.EmployeeProfile.ShopInformation.PickupWardID = Item.PickupWardID;
                            XHelper.Session.EmployeeProfile.ShopInformation.PickupStreet = Item.PickupStreet;
                            XHelper.Session.EmployeeProfile.ShopInformation.PickupFullAddress = Item.PickupFullAddress;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "BranchController.JEditBranch: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}