using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Controllers
{
    public class PeriodController : BaseController
    {
        public ActionResult Index(PeriodIndexModel model)
        {
            return PartialView(model);
        }
        public ActionResult List(PeriodListModel data)
        {
            List<PeriodListViewModel> model = new List<PeriodListViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "PERIOD_LIST",
                Data = JsonConvert.SerializeObject(new ZPeriodListRequest()
                {
                    Type = data.Type,
                    ShopID = data.ShopID
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZPeriodListResponse>>(res.Data);
                if (data.PeriodType == 1)
                {
                    foreach (var item in list)
                    {
                        model.Add(new PeriodListViewModel()
                        {
                            ID = item.ID,
                            Value = item.Value,
                            Text = XData.WeekByValue(item.Value.ToString())
                        });
                    }
                }
                else if (data.PeriodType == 2)
                {
                    foreach (var item in list)
                    {
                        model.Add(new PeriodListViewModel()
                        {
                            ID = item.ID,
                            Value = item.Value,
                            Text = XData.AllDaysByValue(item.Value.ToString())
                        });
                    }
                }
                else
                {
                    XHelper.LogToFile(LogFileType.TRACE, "PeriodController.List: PeriodType không xác định.");
                }

            }
            ViewBag.Type = data.Type;
            ViewBag.PeriodType = data.PeriodType;
            ViewBag.ShopID = data.ShopID;
            ViewBag.AllDays = XData.GetAllDays();
            ViewBag.Week = XData.GetWeek();
            return PartialView(model);
        }
        public JsonResult JAdd(PeriodAddModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PERIOD_ADD",
                    Data = JsonConvert.SerializeObject(new ZPeriodAddRequest()
                    {
                        Value = data.Value,
                        Type = data.Type,
                        ShopID = data.ShopID,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PeriodController.JAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult JRemove(PeriodRemoveModel data)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PERIOD_REMOVE",
                    Data = JsonConvert.SerializeObject(new ZPeriodRemoveRequest()
                    {
                        ID = data.ID,
                        ShopID = data.ShopID
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PeriodController.JRemove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(PeriodDetailModel data)
        {
            PeriodDetailViewModel model = new PeriodDetailViewModel();
            //COD
            var resCOD = XHelper.Invoke(new ZRequest()
            {
                Code = "PERIOD_LIST",
                Data = JsonConvert.SerializeObject(new ZPeriodListRequest()
                {
                    Type = "COD",
                    ShopID = data.ShopID
                })
            });
            if (resCOD.Code == "00")
            {
                List<string> CODList = new List<string>();
                var list = JsonConvert.DeserializeObject<List<ZPeriodListResponse>>(resCOD.Data);
                if (data.CODPeriodType == 1)
                {
                    foreach (var item in list)
                    {
                        string Name = XData.WeekByValue(item.Value.ToString());
                        CODList.Add(Name);
                    }
                }
                else if (data.CODPeriodType == 2)
                {
                    foreach (var item in list)
                    {
                        string Name = XData.WeekByValue(item.Value.ToString());
                        CODList.Add(Name);
                    }
                }
                else
                {
                    XHelper.LogToFile(LogFileType.TRACE, "PeriodController.Detail: CODPeriodType không xác định.");
                }
                model.COD = CODList;
            }

            //Fee
            var resFee = XHelper.Invoke(new ZRequest()
            {
                Code = "PERIOD_LIST",
                Data = JsonConvert.SerializeObject(new ZPeriodListRequest()
                {
                    Type = "COD",
                    ShopID = data.ShopID
                })
            });
            if (resFee.Code == "00")
            {
                List<string> FeeList = new List<string>();
                var list = JsonConvert.DeserializeObject<List<ZPeriodListResponse>>(resFee.Data);
                if (data.FeePeriodType == 1)
                {
                    foreach (var item in list)
                    {
                        string Name = XData.WeekByValue(item.Value.ToString());
                        FeeList.Add(Name);
                    }
                }
                else if (data.FeePeriodType == 2)
                {
                    foreach (var item in list)
                    {
                        string Name = XData.AllDaysByValue(item.Value.ToString());
                        FeeList.Add(Name);
                    }
                }
                else
                {
                    XHelper.LogToFile(LogFileType.TRACE, "PeriodController.Detail: FeePeriodType không xác định.");
                }
                model.Fee = FeeList;
            }
            return View(model);
        }
    }
}