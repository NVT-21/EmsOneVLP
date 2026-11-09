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
    public class NotifyController : BaseController
    {
        // GET: Notify
        public ActionResult NotifyView()
        {
            NotifyListModel model = new NotifyListModel()
            {
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
                ListNotifyType = XData.GetListNotifyType()
            };
            return View(model);
        }
        public ActionResult NotifySearchListView(NotifyListModel req)
        {
            ZResponse res = new ZResponse();
            List<NotifyListViewModel> model = new List<NotifyListViewModel>();
            res = NotifyRepository.NotifyList(new NotifyListModel()
            {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                NotifyType = req.NotifyType,
                IsCheck = req.IsCheck
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZNotifyListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new NotifyListViewModel()
                    {
                        ShopID = item.ShopID,
                        ID = item.ID,
                        NotifyType = item.NotifyType,
                        NotifyTypeName = item.NotifyTypeName,
                        NotifyName = item.NotifyName,
                        NotifyContent = item.NotifyContent,
                        NotifyRedirectUrl = item.NotifyRedirectUrl,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate,
                        IsCheck = item.IsCheck
                    });
                }
            }
            return PartialView(model);
        }
        public ActionResult NotifyListView()
        {
            ZResponse res = new ZResponse();
            List<NotifyListViewModel> model = new List<NotifyListViewModel>();
            res = NotifyRepository.NotifyList(new NotifyListModel()
            {
                RowLimit = 30
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZNotifyListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new NotifyListViewModel()
                    {
                        ShopID = item.ShopID,
                        ID = item.ID,
                        NotifyType = item.NotifyType,
                        NotifyName = item.NotifyName,
                        NotifyContent = item.NotifyContent,
                        NotifyRedirectUrl = item.NotifyRedirectUrl,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate,
                        IsCheck = item.IsCheck
                    });
                }
            }
            ViewBag.CheckPermission = "Y";
            ViewBag.IsCheckCount = model.Where(n => n.IsCheck == "N").Count();
            return PartialView(model);
        }
        public JsonResult JUpdateClickNotify(NotifyUpdateModel req)
        {
            ZResponse res = new ZResponse();
            res = NotifyRepository.NotifyUpdate(new NotifyUpdateModel() { StrID = req.StrID });
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JRemoveClickNotify(NotifyUpdateModel req)
        {
            ZResponse res = new ZResponse();
            res = NotifyRepository.NotifyRemove(new NotifyUpdateModel() { ID = req.ID });
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NotifyDetailView(NotifyGetByIDModel req)
        {
            NotifyListViewModel model = null;
            var res = NotifyRepository.NotifyGetByID(new NotifyGetByIDModel() { ID = req.ID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZNotifyListResponse>(res.Data);
                model = new NotifyListViewModel()
                {
                    ShopID = item.ShopID,
                    ID = item.ID,
                    NotifyType = item.NotifyType,
                    NotifyTypeName = item.NotifyTypeName,
                    NotifyName = item.NotifyName,
                    NotifyContent = item.NotifyContent,
                    NotifyRedirectUrl = item.NotifyRedirectUrl,
                    CreatedDate = item.CreatedDate,
                    UpdatedDate = item.UpdatedDate,
                    IsCheck = item.IsCheck
                };
            }
            return PartialView(model);
        }
        public JsonResult JCountNotify()
        {
            ZResponse res = new ZResponse();
            object CountNotify = new {};
            res = NotifyRepository.NotifyList(new NotifyListModel()
            {
                RowLimit = 30
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZNotifyListResponse>>(res.Data);
                CountNotify = new {
                    Quantity = list.Where(n => n.IsCheck == "N").Count()
                };
            }
            else
            {
                CountNotify = new {
                    Quantity = 0
                };
            }
            return Json(CountNotify, JsonRequestBehavior.AllowGet);
        }
    }
}