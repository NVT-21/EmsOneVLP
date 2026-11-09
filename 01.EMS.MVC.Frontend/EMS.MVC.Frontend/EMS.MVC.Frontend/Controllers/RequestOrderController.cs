using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace EMS.MVC.Frontend.Controllers
{
    public class RequestOrderController: BaseController
    {
        #region Thống kê yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "THONG_KE_YEU_CAU_DAT_HANG")] 
        public ActionResult DashboardView()
        {
            return View();
        }
        #endregion

        #region Danh sách yêu cầu đặt hàng được chia lại
        [AuthorizeController(FunctionalTitle = "DANH_SACH_CHIA_LAI_YEU_CAU_DAT_HANG")] 
        public ActionResult DivisionAgainView()
        {
            ViewBag.ShopID = XHelper.Encrypt(XHelper.Session.EmployeeProfile.ShopInformation.ID.ToString());
            return View();
        }

        public ActionResult SearchListDivisionAgainView(string ShopID)
        {
            List<RequestOrderListViewModel> oListItem = new List<RequestOrderListViewModel>();
            var res = RequestOrderRepository.SearchListRequestOrder(new ZRequestOrderListRequest()
            {
                ShopID = Convert.ToInt32(XHelper.Decrypt(ShopID)),
                RecieverID = 0,
                StoreOwner = XHelper.Session.EmployeeProfile.StoreOwner,
                Status = "0",
                ProcessDivision = "Y"
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZRequestOrderListResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new RequestOrderListViewModel()
                    {
                        ID = item.ID,
                        FullName = item.FullName,
                        MobileNumber = item.MobileNumber,
                        Email = item.Email,
                        Channel = XData.GetNameChannel(item.Channel),
                        Address = item.Address,
                        ProductName = item.ProductName,
                        Product = item.Product,
                        Message = item.Message,
                        Note = item.Note,
                        Transport = item.Transport,
                        PaymentMethod = item.PaymentMethod,
                        RecieverName = item.RecieverName,
                        ProcessDivisionRequest = item.ProcessDivisionRequest,
                        Status = item.Status,
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate,
                        Reason = item.Reason
                    });
                }
            }
            return PartialView(oListItem);
        }
        #endregion

        #region Chọn yêu cầu đặt hàng cần chia lại
        [AuthorizeController(FunctionalTitle = "CHIA_LAI_YEU_CAU_DAT_HANG")]
        public ActionResult ChooseDivisionAgainView(int[] RequestOrderID)
        {
            ViewBag.ListRequestOrderID = RequestOrderID;
            //ViewBag.ListEmployeeGroup = XCommon.GetListEmployeeGroupName("N");
            ViewBag.ListEmployeeGroup = XCommon.SearchListEmployeeGroup(new ZEmployeeGroupListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            return PartialView();
        }

        public JsonResult JChooseDivisionAgainView(int[] arrRequestOrderID, int[] arrEmployeeID)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_ORDER_LIST_DIVISION_AGAIN",
                    Data = JsonConvert.SerializeObject(new ZRequestOrderDivisionAgainRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                        arrRequestOrderID = arrRequestOrderID,
                        arrEmployeeID = arrEmployeeID
                    })
                });
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.JChooseDivisionAgainView: " + ex.Message);
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            return jResult;
        }
        #endregion

        #region Danh sách yêu cầu đặt hàng được chia
        [AuthorizeController(FunctionalTitle = "DANH_SACH_CHIA_YEU_CAU_DAT_HANG")]
        public ActionResult DivisionView()
        {
            ViewBag.ShopID = XHelper.Encrypt(XHelper.Session.EmployeeProfile.ShopInformation.ID.ToString());
            return View();
        }

        public ActionResult SearchListDivisionView(string ShopID)
        {
            List<RequestOrderListViewModel> oListItem = new List<RequestOrderListViewModel>();
            var res = RequestOrderRepository.SearchListRequestOrder(new ZRequestOrderListRequest()
            {
                ShopID = Convert.ToInt32(XHelper.Decrypt(ShopID)),
                RecieverID = 0,
                StoreOwner = XHelper.Session.EmployeeProfile.StoreOwner,
                Status = "0",
                ProcessDivision = "N"
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZRequestOrderListResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new RequestOrderListViewModel()
                    {
                        ID = item.ID,
                        FullName = item.FullName,
                        MobileNumber = item.MobileNumber,
                        Email = item.Email,
                        Channel = XData.GetNameChannel(item.Channel),
                        Address = item.Address,
                        ProductName = item.ProductName,
                        Product = item.Product,
                        Message = item.Message,
                        Note = item.Note,
                        Transport = item.Transport,
                        PaymentMethod = item.PaymentMethod,
                        RecieverName = item.RecieverName,
                        ProcessDivisionRequest = item.ProcessDivisionRequest,
                        Status = item.Status,
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate,
                        Reason = item.Reason
                    });
                }
            }
            return PartialView(oListItem);
        }

        
        #endregion

        #region Chọn yêu cầu đặt hàng cần chia
        [AuthorizeController(FunctionalTitle = "CHIA_YEU_CAU_DAT_HANG")]
        public ActionResult RequestOrderDivisionView(int RequestOrderCount)
        {
            //ViewBag.ListEmployeeGroup = XCommon.GetListEmployeeGroupName("N");
            ViewBag.ListEmployeeGroup = XCommon.SearchListEmployeeGroup(new ZEmployeeGroupListRequest() { IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            ViewBag.RequestOrderCount = RequestOrderCount;
            return PartialView();
        }

        public JsonResult JChooseDivisionView(ZRequestOrderDivisionViewRequest req)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "REQUEST_ORDER_LIST_DIVISION",
                    Data = JsonConvert.SerializeObject(new ZRequestOrderDivisionRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        Criteria = req.Criteria,
                        Quantity = req.Quantity,
                        strEmployeeID = req.strEmployeeID,
                        FromDate = req.FromDate,
                        ToDate = req.ToDate
                    })
                });
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.JChooseDivisionView: " + ex.Message);
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            return jResult;
        }
        #endregion

        #region Lưu yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "LUU_YEU_CAU_DAT_HANG")]
        public JsonResult JUpdateRequestOrder()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.JUpdateRequestOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "THEM_YEU_CAU_DAT_HANG")]
        public ActionResult AddRequestOrderView()
        {
            ViewBag.ListChannel = XData.GetListChannel();
            return View();
        }

        public JsonResult JAddRequestOrder(ZRequestOrderAddViewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = RequestOrderRepository.AddRequestOder(new ZRequestOrderAddRequest()
                {
                    FullName = req.FullName,
                    MobileNumber = req.MobileNumber,
                    ProductSKU = req.ProductSKU,
                    Email = req.Email,
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Address = req.Address,
                    Channel = req.Channel,
                    Note = req.Note,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.JAddRequestOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Thêm yêu cầu đặt hàng bằng Excel
        [AuthorizeController(FunctionalTitle = "THEM_YEU_CAU_DAT_HANG_BANG_EXCEL")]
        public ActionResult ImportExccelView()
        {
            return View();
        }
        public JsonResult JImportExccel()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderController.JImportExccel: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xử lý yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "DANH_SACH_YEU_CAU_DAT_HANG")]
        public ActionResult RequestOrderView()
        {
            RequestOrderIndexViewModel model = new RequestOrderIndexViewModel();
            model = new RequestOrderIndexViewModel()
            {
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy")
            };
            ViewBag.ListRequestOrderStatus = XData.GetListRequestOrderStatus();
            ViewBag.ShopID = XHelper.Encrypt(XHelper.Session.EmployeeProfile.ShopInformation.ID.ToString());
            return View(model);
        }

        public ActionResult SearchListRequestOrderView(string ShopID)
        {
            List<RequestOrderListViewModel> oListItem = new List<RequestOrderListViewModel>();
            var res = RequestOrderRepository.SearchListRequestOrder(new ZRequestOrderListRequest()
            {
                ShopID = Convert.ToInt32(XHelper.Decrypt(ShopID)),
                RecieverID = XHelper.Session.EmployeeProfile.ID,
                StoreOwner = XHelper.Session.EmployeeProfile.StoreOwner,
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZRequestOrderListResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new RequestOrderListViewModel()
                    {
                        ID = item.ID,
                        FullName = item.FullName,
                        MobileNumber = item.MobileNumber,
                        Email = item.Email,
                        Channel = XData.GetNameChannel(item.Channel),
                        Address = item.Address,
                        ProductName = item.ProductName,
                        Product = item.Product,
                        Message = item.Message,
                        Note = item.Note,
                        Transport = item.Transport,
                        PaymentMethod = item.PaymentMethod,
                        RecieverID = item.RecieverID,
                        RecieverName = item.RecieverName,
                        ProcessDivisionRequest = item.ProcessDivisionRequest,
                        Status = XData.GetNameRequestOrderStatus(item.Status),
                        OrderID = item.OrderID,
                        OrderCode = item.OrderCode,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate,
                        Reason = item.Reason
                    });
                }
            }
            ViewBag.UserID = XHelper.Encrypt(XHelper.Session.EmployeeProfile.ID.ToString());
            return PartialView(oListItem);
        }
        #endregion

        #region Từ chối yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "TU_CHOI_YEU_CAU_DAT_HANG")]
        public JsonResult JCancelRequestOrder()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerStickerController.JCancelRequestOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chi tiết yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "CHI_TIET_YEU_CAU_DAT_HANG")]
        public ActionResult DetailRequestOrderView()
        {
            return View();
        }
        #endregion

        #region Cập nhật trạng thái yêu cầu đặt hàng

        [AuthorizeController(FunctionalTitle = "CAP_NHAT_TRANG_THAI_YEU_CAU_DAT_HANG")]
        public ActionResult RejectRequestOrderView()
        {
            return PartialView();
        }

        public JsonResult JRejectRequestOrder()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerStickerController.JRejectRequestOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tự động chia yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "TU_DONG_CHIA_YEU_CAU_DAT_HANG")] 
        public ActionResult AutoRequestOrderView()
        {
            return View();
        }
        #endregion


        public ActionResult ProductSearchView(string textSearch, string isRequestOrder = "N")
        {
            ViewBag.textSearch = textSearch;
            ViewBag.IsRequestOrder = isRequestOrder;
            return PartialView();
        }

        public ActionResult SearchProductDataView(string textSearch, string strCheckedList)
        {
            //List<OrderSearchProductDataViewModel> ProductList = new List<OrderSearchProductDataViewModel>();
            List<ZOrderSearchProductResponse> ProductList = new List<ZOrderSearchProductResponse>();
            List<int> lstProductID = new List<int>();
            List<OrderCheckboxModel> CheckedList = new List<OrderCheckboxModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "ORDER_SEARCH_PRODUCT",
                Data = JsonConvert.SerializeObject(new ZProductListRequest
                {
                    Name = textSearch,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                })
            });
            if (res.Code == "00")
            {
                ProductList = JsonConvert.DeserializeObject<List<ZOrderSearchProductResponse>>(res.Data);
                //foreach (var item in list)
                //{
                //    ProductList.Add(new OrderSearchProductDataViewModel()
                //    {


                //    });
                //}
                if (strCheckedList != "[]" && strCheckedList != null)
                {
                    CheckedList = JsonConvert.DeserializeObject<List<OrderCheckboxModel>>(strCheckedList);
                }
                ViewBag.CheckedList = CheckedList;
            }
            return PartialView(ProductList);
        }

        public ActionResult GetListEmployeeByEmployeeGroupID(ZEmployeeListEmployeeByEmployeeGroupIDViewRequest req)
        {
            List<EmployeeListViewModel> oListItem = new List<EmployeeListViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "EMPLOYEE_LIST_EMPLOYEE_BY_EMPLOYEE_GROUP_ID",
                Data = JsonConvert.SerializeObject(new ZEmployeeListEmployeeByEmployeeGroupIDRequest()
                {
                    strEmployeeGroupID = req.strEmployeeGroupID,
                    //EmployeeGroupID = req.EmployeeGroupID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZEmployeeListEmployeeByEmployeeGroupIDResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new EmployeeListViewModel()
                    {
                        ID = XHelper.Encrypt(item.EmployeeID.ToString()),
                        FullName = item.EmployeeName,
                    });
                }
            }
            return PartialView(oListItem);
        }

        public ActionResult OrderedProduct_V2(string textSearch, string strCheckedList, string Product)
        {
            List<OrderedProductViewModel> model = new List<OrderedProductViewModel>();
            if (Product != null)
            {
                var splitProduct = Product.Split('|');
                for (var i = 0; i < splitProduct.Length; i++)
                {
                    var splitProductContinued = splitProduct[i].Split('-');

                    var res = XHelper.Invoke(new ZRequest()
                    {
                        Code = "PRODUCT_VARIANT_GET_BY_SKU",
                        Data = JsonConvert.SerializeObject(new ZProductVariantGetBySKURequest
                        {
                            SKU = splitProductContinued[0],
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                        })
                    });
                    if (res.Code == "00")
                    {
                        var data = JsonConvert.DeserializeObject<ZProductVariantGetBySKUResponse>(res.Data);
                        if (data.WareHouseQuantity > 0 && data.CurrentQuantity > 0)
                        {
                            model.Add(new OrderedProductViewModel()
                            {
                                ProductID = data.ProductID,
                                ProductName = data.ProductName,
                                VariantID = data.VariantID,
                                VariantName = data.VariantName,
                                VariantImage = data.VariantImage,
                                SKU = data.SKU,
                                Barcode = data.Barcode,
                                Weight = data.Weight,
                                NumberOfProduct = Convert.ToInt32(splitProductContinued[1]),
                                WareHouseQuantity = data.WareHouseQuantity
                            });
                        }
                    }
                }
            }
            else
            {
                List<OrderProductModel> lstOrderedProduct = new List<OrderProductModel>();
                if (strCheckedList != "[]" && strCheckedList != null)
                {
                    lstOrderedProduct = JsonConvert.DeserializeObject<List<OrderProductModel>>(strCheckedList);
                }
                foreach (var item in lstOrderedProduct)
                {
                    var res = XHelper.Invoke(new ZRequest()
                    {
                        Code = "PRODUCT_VARIANT_GET_BY_ID",
                        Data = JsonConvert.SerializeObject(new ZProductVariantGetByIDRequest
                        {
                            ID = item.VariantID,
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                        })
                    });
                    if (res.Code == "00")
                    {
                        var data = JsonConvert.DeserializeObject<ZProductVariantGetByIDResponse>(res.Data);
                        model.Add(new OrderedProductViewModel()
                        {
                            ProductID = data.ProductID,
                            ProductName = data.ProductName,
                            VariantID = data.ID,
                            VariantName = data.Options,
                            VariantImage = data.Avatar,
                            Price = data.Price,
                            SKU = data.SKU,
                            Barcode = data.Barcode,
                            Weight = data.Weight,
                            NumberOfProduct = item.NumberOfProduct,
                            WareHouseQuantity = data.WareHouseQuantity
                        });
                        ViewBag.textSearch = textSearch;
                    }
                }
            }
            return PartialView(model);
        }

        public ActionResult OrderedProduct(string textSearch, string strCheckedList, string Product, string PaymentReload = "Y")
        {
            List<OrderedProductViewModel> model = new List<OrderedProductViewModel>();
            if (Product != null)
            {
                var splitProduct = Product.Split('|');
                for (var i = 0; i < splitProduct.Length; i++)
                {
                    var splitProductContinued = splitProduct[i].Split('-');

                    var res = XHelper.Invoke(new ZRequest()
                    {
                        Code = "PRODUCT_VARIANT_GET_BY_SKU",
                        Data = JsonConvert.SerializeObject(new ZProductVariantGetBySKURequest
                        {
                            SKU = splitProductContinued[0],
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                        })
                    });
                    if (res.Code == "00")
                    {
                        var data = JsonConvert.DeserializeObject<ZProductVariantGetBySKUResponse>(res.Data);
                        if (data.WareHouseQuantity > 0 && data.CurrentQuantity > 0)
                        {
                            model.Add(new OrderedProductViewModel()
                            {
                                ProductID = data.ProductID,
                                ProductName = data.ProductName,
                                VariantID = data.VariantID,
                                VariantName = data.VariantName,
                                VariantImage = data.VariantImage,
                                Price = data.Price,
                                PurchasePrice = data.PurchasePrice,
                                SKU = data.SKU,
                                Barcode = data.Barcode,
                                Weight = data.Weight,
                                NumberOfProduct = Convert.ToInt32(splitProductContinued[1]),
                                WareHouseQuantity = data.WareHouseQuantity
                            });
                        }
                    }
                }
            }
            else
            {
                List<OrderProductModel> lstOrderedProduct = new List<OrderProductModel>();
                if (strCheckedList != "[]" && strCheckedList != null)
                {
                    lstOrderedProduct = JsonConvert.DeserializeObject<List<OrderProductModel>>(strCheckedList);
                }
                foreach (var item in lstOrderedProduct)
                {
                    var res = XHelper.Invoke(new ZRequest()
                    {
                        Code = "PRODUCT_VARIANT_GET_BY_ID",
                        Data = JsonConvert.SerializeObject(new ZProductVariantGetByIDRequest
                        {
                            ID = item.VariantID,
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                        })
                    });
                    if (res.Code == "00")
                    {
                        var data = JsonConvert.DeserializeObject<ZProductVariantGetByIDResponse>(res.Data);
                        model.Add(new OrderedProductViewModel()
                        {
                            ProductID = data.ProductID,
                            ProductName = data.ProductName,
                            VariantID = data.ID,
                            VariantName = data.Options,
                            VariantImage = data.Avatar,
                            Price = data.Price,
                            SKU = data.SKU,
                            Barcode = data.Barcode,
                            Weight = data.Weight,
                            NumberOfProduct = item.NumberOfProduct,
                            WareHouseQuantity = data.WareHouseQuantity
                        });
                        ViewBag.textSearch = textSearch;
                    }
                }
            }
            ViewBag.ListWareHouse = XCommon.GetListWarehouse("Y", "N", XHelper.Session.EmployeeProfile.ShopInformation.ID);
            ViewBag.PaymentReload = PaymentReload;
            return PartialView(model);
        }
    }
}