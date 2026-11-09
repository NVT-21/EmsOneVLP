using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using System.IO;

namespace EMS.MVC.Frontend.Controllers
{
    public class TemplatePrintController : BaseController
    {
        #region Mẫu in và kiểu in
        [AuthorizeController(FunctionalTitle = "MAU_IN")]
        public ActionResult TemplatePrintView()
        {
            TemplatePrintViewModel model = new TemplatePrintViewModel() 
            {
                PrintTypeDefault = XHelper.Session.EmployeeProfile.ShopInformation.PrintTypeDefault,
                ListPrintType = XData.GetListPrintType(),
                ListLabelPrint = XCommon.SearchListLabelPrint(new ZLabelPrintSearchListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID, Type = 1 })
            };
            return View(model);
        }

        public ActionResult LabelPrintData(LabelPrintDataViewModel data)
        {
            return PartialView(data);
        }

        public ActionResult TemplatePrintDetailView()
        {
            ViewBag.PrintTypeDefault = XHelper.Session.EmployeeProfile.ShopInformation.PrintTypeDefault;
            ViewBag.ListPrintType = XData.GetListPrintType();
            return View();
        }
        public ActionResult ChooseInfoTemplatePrint(string PrintType)
        {
            ViewBag.PrintType = PrintType;
            return PartialView();
        }

        public ActionResult TemplatePrintData(TemplatePrintDataViewModel model)
        {
            return PartialView(model);
        }
        #endregion

        #region Đổi kiểu in, mẫu in
        [AuthorizeController(FunctionalTitle = "DOI_MAU_IN")]
        public JsonResult JChangeLabelPrint(ZShopChangeLabelPrintRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_CHANGE_LABEL_PRINT",
                    Data = JsonConvert.SerializeObject(new ZShopChangeLabelPrintRequest()
                    {
                        LabelPrintID = req.LabelPrintID,
                        PrintType = req.PrintType,
                        PrintTypeDefault = req.PrintTypeDefault,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    })
                });
                if (res.Code == "00")
                {
                    if (req.PrintTypeDefault == "Y")
                    {
                        XHelper.Session.EmployeeProfile.ShopInformation.PrintTypeDefault = req.PrintType;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConfigurationController.JChangeLabelPrint: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JUpdateRole(ZTemplatePrintUpdateRoleRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TEMPLATE_PRINT_UPDATE_ROLE",
                    Data = JsonConvert.SerializeObject(new ZTemplatePrintUpdateRoleRequest()
                    {
                        strTemplatePrintID = req.strTemplatePrintID,
                        PrintType = req.PrintType,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConfigurationController.JUpdateRole: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}