using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Areas.Tiktok.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Tiktok.Controllers
{
    public class TIK_ProductController : TIK_BaseController
    {
        // GET: Tiktok/TIK_Product
        public ActionResult TIK_ProductView()
        {
            return View();
        }
        public ActionResult TIK_SearchListProductView(TIK_ProductViewModel req)
        {
            List<TIK_SearchListProductViewModel> listItem = new List<TIK_SearchListProductViewModel>();
            var res = TiktokRepository.SearchListProduct(new ZTikProductListRequest()
            {
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTikProductListResponse>>(res.Data);
                foreach (var item in list)
                {
                    listItem.Add(new TIK_SearchListProductViewModel()
                    {
                        ProductID = item.ProductID,
                        ProductCode = item.ProductCode,
                        ProductAvatar = item.ProductAvatar,
                        ProductName = item.ProductName,
                        SKU = item.SKU,
                        Price = item.Price,
                        Options = item.Options,
                        Quantity = item.Quantity,
                        UpdatedDate = item.UpdatedDate,
                        TiktokSKUID = item.TiktokSKUID,
                        TiktokProductID = item.TiktokProductID,
                        Channel = item.Channel,
                        TiktokStatusUpdatePrice = item.TiktokStatusUpdatePrice,
                        TiktokStatusUpdateStock = item.TiktokStatusUpdateStock
                    });
                }
            }
            return PartialView(listItem);
        }
        public ActionResult TIK_SearchListProductProcessView(TIK_ProductViewModel req)
        {
            List<TIK_SearchListProductProcessViewModel> listItem = new List<TIK_SearchListProductProcessViewModel>();
            var res = TiktokRepository.SearchListProductProcess(new ZTikProductProcessListRequest()
            {
                SearchKey = req.SearchKey,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTikProductProcessListResponse>>(res.Data);
                foreach (var item in list)
                {
                    listItem.Add(new TIK_SearchListProductProcessViewModel()
                    {
                        ID = item.ID,
                        ShopID = item.ShopID,
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        ProductAvatar = item.ProductAvatar,
                        SKU = item.SKU,
                        Options = item.Options,
                        TiktokProductID = item.TiktokProductID,
                        TiktokProductAvatar = item.TiktokProductAvatar,
                        TiktokProductName = item.TiktokProductName,
                        TiktokSellerSKU = item.TiktokSellerSKU,
                        TiktokSKUID = item.TiktokSKUID,
                        TiktokOptions = item.TiktokOptions,
                        Status = item.Status,
                        StatusName = item.StatusName
                    });
                }
            }
            ViewBag.ListProduct = TiktokRepository.SearchListProductNotLinkedYet(
                new ZTikProductNotLinkedYetListRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                }
            );
            return PartialView(listItem);
        }
        public JsonResult TIK_JProductUpdateConnection(TIK_ProductUpdateConnectionModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TiktokRepository.ProductUpdateConnection(new ZTikProductUpdateConnectionRequest()
                {
                    TypeUpdate = req.TypeUpdate,
                    ShopID = req.ShopID,
                    UpdatedBy = req.UpdatedBy,
                    ListItem = req.ListItem
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TIK_ProductController.TIK_JProductUpdateConnection: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}