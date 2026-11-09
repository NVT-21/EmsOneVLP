using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using NPOI.SS.Util;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using System.Globalization;

namespace EMS.MVC.Frontend.Controllers
{
    public class DevController : BaseController
    {
        // GET: Dev
        public ActionResult Index()
        {
            OrderViewModel model = new OrderViewModel()
            {
                FromDate = DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy"),
            };
            return View(model);
        }

        public JsonResult SearchData()
        {
            OrderPagination model = null;
            try
            {
                var request = Request.Form;
                var draw = request["draw"];
                var start = request["start"];
                var length = request["length"];
                var pageNumber = (Convert.ToInt32(start) / Convert.ToInt32(length)) + 1;
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = request["start"];
                var res = OrderRepository.SearchListOrderPagination(new ZOrderPaginationRequest()
                {
                    FromDate = "01/02/2023",
                    ToDate = "01/02/2024",
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    PageNumber = 1,
                    PageLimit = pageSize
                });
                if (res.Code == "00")
                {
                    List<ZOrderPaginationItemResponse> ListOrder = new List<ZOrderPaginationItemResponse>();
                    var item = JsonConvert.DeserializeObject<ZOrderPaginationResponse>(res.Data);
                    ListOrder = item.Order;
                    model = new OrderPagination()
                    {
                        draw = Convert.ToInt32(draw),
                        recordsFiltered = item.TotalRow,
                        recordsTotal = item.TotalRow,
                        data = ListOrder
                    };
                }
            }
            catch
            {
                model = new OrderPagination();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SearchListOrderView(OrderViewModel req)
        //{
        //    List<SearchListOrderViewModel> ListItem = new List<SearchListOrderViewModel>();
        //    var res = OrderRepository.SearchListOrderPagination(new ZOrderPaginationRequest()
        //    {
        //        FromDate = req.FromDate,
        //        ToDate = req.ToDate,
        //        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
        //        PageNumber = req.PageNumber == 0 ? 1: req.PageNumber,
        //        PageLimit = req.PageLimit == 0 ? 10 : req.PageLimit
        //    });
        //    if (res.Code == "00")
        //    {
        //        var listItem = JsonConvert.DeserializeObject<ZOrderPaginationResponse>(res.Data);
        //        foreach (var item in listItem.Order)
        //        {
        //            ListItem.Add(new SearchListOrderViewModel()
        //            {
        //                OrderID = item.OrderID,
        //                EMSOneCode = item.EMSOneCode,
        //                OrderCode = item.OrderCode,
        //                OrderStatus = item.OrderStatus,
        //                CreatedDate = item.CreatedDate,
        //            });
        //        }
        //    }
        //    return PartialView(ListItem);
        //}
    }

    public class OrderPagination
    {
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<ZOrderPaginationItemResponse> data { get; set; }
    }
}