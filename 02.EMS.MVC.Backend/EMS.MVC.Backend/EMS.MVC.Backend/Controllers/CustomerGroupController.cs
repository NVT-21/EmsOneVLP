using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Controllers
{
    public class CustomerGroupController : BaseController
    {
        public ActionResult CustomerGroupView()
        {
            return View();
        }

        public JsonResult JSearchListCustomerGroupTree()
        {
            List<TreeModel> tree = new List<TreeModel>();
            try
            {
                var res = CustomerGroupRepository.SearchList(new ZCustomerGroupSearchListRequest()
                {

                });

                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCustomerGroupSearchListResponse>>(res.Data);

                    foreach (var item in list)
                    {
                        tree.Add(new TreeModel()
                        {
                            id = item.Id.ToString(),
                            parent = "#",
                            text = item.Name,
                            children = false,
                            state = new TreeStateModel() { opened = false },
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupController.JSearchListCollectionPostOfficeTree: " + ex.Message);
            }
            return Json(tree, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCustomerGroupView()
        {
            AddCustomerGroupViewModel model = new AddCustomerGroupViewModel()
            {

            };
            return PartialView(model);
        }

        public JsonResult JAddCustomerGroup(AddCustomerGroupViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CustomerGroupRepository.AddCustomerGroup(new ZCustomerGroupAddRequest()
                {
                    CreatedBy = XHelper.Session.UserProfile.ID,
                    Name = req.Name,
                    CRMCode = req.CRMCode,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupController.JAddCustomerGroup: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailCustomerGroupView(long id)
        {
            DetailCustomerGroupViewModel model = null;

            var res = CustomerGroupRepository.GetByID(id);

            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZCustomerGroupGetByIDResponse>(res.Data);

                model = new DetailCustomerGroupViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CRMCode = item.CRMCode,
                };
            }
            return PartialView(model);
        }

        public JsonResult JEditCustomerGroup(DetailCustomerGroupViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CustomerGroupRepository.EditCustomerGroup(new ZCustomerGroupEditRequest()
                {
                    Id = req.Id,
                    Name = req.Name,
                    CRMCode = req.CRMCode,
                    UpdatedBy = XHelper.Session.UserProfile.ID,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupController.JEditCustomerGroup: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}