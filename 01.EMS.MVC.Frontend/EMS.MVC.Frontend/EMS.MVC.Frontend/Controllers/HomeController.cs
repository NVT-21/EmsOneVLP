using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using EMS.MVC.Frontend.Repositories;
namespace EMS.MVC.Frontend.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel()
            {
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                CrmOrPaypostCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                ListCrmOrPaypostCode = XCommon.SearchListCrmOrPaypostCode(new ZShopGetListCrmOrPaypostCodeRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID }),
                Fullname = XHelper.Session.EmployeeProfile.ShopInformation.FullName,
                Email = XHelper.Session.EmployeeProfile.ShopInformation.Email,
                MobileNumber = XHelper.Session.EmployeeProfile.ShopInformation.MobileNumber
            };
            return View(model);
        }

        public ActionResult OverView(long TotalTurnover, List<StatisticalOrderViewModel> model)
        {
            ViewBag.Turnover = TotalTurnover;
            return PartialView(model);
        }

        public ActionResult DeliveryTransportView(List<StatisticalDeliveryTransportViewModel> model)
        {
            return PartialView(model);
        }

        public ActionResult CashflowView(List<StatisticalCashflowViewModel> model)
        {
            ViewBag.SumTotalCOD = model.Sum(m => m.TotalCOD);
            ViewBag.SumTotalTransportFee = model.Sum(m => m.TotalTransportFee);
            return PartialView(model);
        }

        public ActionResult ProductView(StatisticalProductViewModel model)
        {
            return PartialView(model);
        }

        public ActionResult CustomerView(StatisticalCustomerViewModel model)
        {
            return PartialView(model);
        }

        public JsonResult JStatisticalData(StatisticalDataModel model)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STATISTICAL",
                    Data = JsonConvert.SerializeObject(new ZStatisticalDashboardV2Request
                    {
                        DeviceName = "WEB",
                        PPCRMCode = model.PPCRMCode,
                        FromDate = model.FromDate,
                        ToDate = model.ToDate
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HomeController.JStatisticalData: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}
