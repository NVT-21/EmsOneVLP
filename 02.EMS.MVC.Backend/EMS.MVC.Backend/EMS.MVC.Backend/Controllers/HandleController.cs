using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
namespace EMS.MVC.Backend.Controllers
{
    public class HandleController : Controller
    {
        #region Danh sách Tỉnh/ Thành phố
        //public JsonResult SearchListProvince()
        //{
        //    List<ProvinceModel> ListItem = new List<ProvinceModel>();
        //    try
        //    {
        //        ListItem = XCommon.SearchListProvince(new ZProvinceListRequest());
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListProvince: " + ex.Message);
        //    }
        //    return Json(ListItem, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult SearchListProvince(int Type = 0)
        {
            List<ProvinceModel> ListItem = new List<ProvinceModel>();
            try
            {
                ListItem = XCommon.SearchListProvince(new ZProvinceListRequest
                {
                    Type = Type
                });
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListProvince: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách Quận/ Huyện
        public JsonResult SearchListDistrictByProvinceID(int ProvinceID = 0)
        {
            List<DistrictModel> ListItem = new List<DistrictModel>();
            try
            {
                ListItem = XCommon.SearchListDistrictByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = ProvinceID });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListDistrictByProvinceID: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy ID Quận/ Huyện bằng tên Quận/ Huyện
        public JsonResult SearchDistrictIDByDistrictName(int ProvinceID, string DistrictName)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PWDRepository.SearchDistrictIDByDistrictName(new ZPDWGetIDByPDWNameRequest() { ID = ProvinceID, Name = DistrictName });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchDistrictIDByDistrictName: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh sách Phường/ Xã
        //public JsonResult SearchListWardByDistrictID(int DistrictID = 0)
        //{
        //    List<WardModel> ListItem = new List<WardModel>();
        //    try
        //    {
        //        ListItem = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = DistrictID });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListWardByDistrictID: " + ex.Message);
        //    }
        //    return Json(ListItem, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult SearchListWardByDistrictID(int DistrictID = 0, int Type = 0)
        {
            List<WardModel> ListItem = new List<WardModel>();
            try
            {
                ListItem = XCommon.SearchListWardByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = DistrictID, Type = Type });
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchListWardByDistrictID: " + ex.Message);
            }
            return Json(ListItem, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Lấy ID Phường/ Xã bằng tên Phường/ Xã
        public JsonResult SearchWardIDByWardName(int DistrictID, string WardName)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PWDRepository.SearchWardIDByWardName(new ZPDWGetIDByPDWNameRequest() { ID = DistrictID, Name = WardName });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchWardIDByWardName: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Phân tích địa chỉ
        public JsonResult VNPMapsAutocomplete(ZVNPMapsAutocompleteRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "VNP_MAPS_AUTOCOMPLETE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.VNPMapsAutocomplete: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MapsGetByVMapCode(ZMapsGetByVMapCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "MAPS_GET_BY_V_MAP_CODE",
                    Data = JsonConvert.SerializeObject(new ZMapsGetByVMapCodeRequest()
                    {
                        ProvinceVMapCode = req.ProvinceVMapCode == 0 ? 0 : req.ProvinceVMapCode - 100000000, //mã bưu chính = gid - 100000000
                        DistrictVMapCode = req.DistrictVMapCode == 0 ? 0 : req.DistrictVMapCode - 100000000, //mã bưu chính = gid - 100000000
                        WardVMapCode = req.WardVMapCode == 0 ? 0 : req.WardVMapCode - 100000000, //mã bưu chính = gid - 100000000
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.MapsGetByVMapCode: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult SearchListRequestSupportGroup(int ParentID)
        {
            List<RequestSupportGroupModel> listItem = XCommon.SearchListRequestSupportGroup(ParentID);
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowListErrorMultipleSelect(string Error, string Controller)
        {
            List<ShowListErrorMultipleSelectViewModel> ListItem = new List<ShowListErrorMultipleSelectViewModel>();
            var data = JsonConvert.DeserializeObject<List<ShowListErrorMultipleSelectModel>>(Error);
            if (Controller == "Order")
            {
                foreach (var item in data)
                {
                    ListItem.Add(new ShowListErrorMultipleSelectViewModel()
                    {
                        OrderCode = item.OrderCode,
                        Description = item.Description
                    });
                }
            }
            else if (Controller == "RequestSupport")
            {
                foreach (var item in data)
                {
                    ListItem.Add(new ShowListErrorMultipleSelectViewModel()
                    {
                        RequestSupportCode = item.RequestSupportCode,
                        Description = item.Description
                    });
                }
            }
            else if (Controller == "PaymentCOD")
            {
                foreach (var item in data)
                {
                    ListItem.Add(new ShowListErrorMultipleSelectViewModel()
                    {
                        EmsBarCode = item.EmsBarCode,
                        Description = item.Description
                    });
                }
            }
            ViewBag.Controller = Controller;
            return PartialView(ListItem);
        }

        public ActionResult NotifyListView(string IsLoad = "N")
        {
            NotifyListViewModel model = null;
            var res = XCommon.NotifyListRequestSupport();
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZRequestSupportNotifyListResponse>(res.Data);
                model = new NotifyListViewModel()
                {
                    MaxRequestSupportID = item.MaxRequestSupportID,
                    CountRequestSupport = item.CountRequestSupport,
                    ListRequestSupportNotify = item.ListRequestSupportNotify
                };
            }
            ViewBag.IsLoad = IsLoad;
            return PartialView(model);
        }

        public JsonResult NotifyList(int MaxRequestSupportID)
        {
            ZResponse res = new ZResponse();
            //ZRequestSupportNotifyListResponse oListRequestSupport = null;
            try
            {
                res = XCommon.NotifyListRequestSupport(MaxRequestSupportID);
                //if (res.Code == "00")
                //{
                //    var item = JsonConvert.DeserializeObject<ZRequestSupportNotifyListResponse>(res.Data);
                //    oListRequestSupport = new ZRequestSupportNotifyListResponse()
                //    {
                //        MaxRequestSupportID = item.MaxRequestSupportID,
                //        CountRequestSupport = item.CountRequestSupport,
                //        ListRequestSupportNotify = item.ListRequestSupportNotify
                //    };
                //}
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.NotifyList: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetProvincesList()
        //{
        //    List<SelectListItem> listItem = new List<SelectListItem>();
        //    listItem = XCommon.ProvincesList();
        //    return Json(listItem, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetDistrictByProvinceID(ZDistrictListRequest req)
        //{
        //    List<SelectListItem> listItem = new List<SelectListItem>();
        //    listItem = XCommon.GetDistrictByProvinceID(req.ProvinceID);
        //    return Json(listItem, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetWardByDistrictID(ZWardListRequest req)
        //{
        //    List<SelectListItem> listItem = new List<SelectListItem>();
        //    listItem = XCommon.GetWardByDistrictID(req.DistrictID);
        //    return Json(listItem, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult PickUpAddressTreeView(int ShopID)
        {
            List<TreeModel> tree = new List<TreeModel>();
            try
            {
                var res = PickUpAddressRepository.SearchList(new ZPickUpAddressSearchListRequest() { ShopID = ShopID });
                if (res.Code == "00")
                {
                    var ListPickUpAddress = JsonConvert.DeserializeObject<List<ZPickUpAddressSearchListResponse>>(res.Data);
                    foreach (var item in ListPickUpAddress)
                    {
                        tree.Add(new TreeModel()
                        {
                            id = item.PickUpAddressID.ToString(),
                            parent = "#",
                            text = item.PickUpName,
                            children = item.QuantityLabelPrint > 0 ? true : false,
                            state = new TreeStateModel()
                            {
                                opened = false
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.PickUpAddressTreeView: " + ex.Message);
            }
            return Json(tree, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LabelPrintTreeView(int PickUpAddressID)
        {
            List<TreeModel> tree = new List<TreeModel>();
            try
            {
                var res = LabelPrintRepository.SearchList(new ZLabelPrintSearchListRequest() { PickUpAddressID = PickUpAddressID });
                if (res.Code == "00")
                {
                    var ListTemplatePrint = JsonConvert.DeserializeObject<List<ZLabelPrintSearchListResponse>>(res.Data);
                    foreach (var item in ListTemplatePrint)
                    {
                        tree.Add(new TreeModel()
                        {
                            id = Convert.ToString(PickUpAddressID + "_" + item.LabelPrintID),
                            parent = PickUpAddressID.ToString(),
                            text = item.LabelName,
                            children = false,
                            state = new TreeStateModel()
                            {
                                opened = false
                            },
                            li_attr = new TreeLiAttrModel()
                            {
                                id = item.LabelPrintID.ToString()
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.LabelPrintTreeView: " + ex.Message);
            }
            return Json(tree, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetPickUpAddressAndTemplatePrintList(int ShopID)
        //{
        //    ZResponse res = new ZResponse();
        //    List<TreeViewModel> node = new List<TreeViewModel>();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "PICK_UP_ADDRESS_SEARCH_LIST_TREE",
        //            Data = JsonConvert.SerializeObject(new { ShopID = ShopID })
        //        });
        //        if (res.Code == "00")
        //        {
        //            node = JsonConvert.DeserializeObject<List<TreeViewModel>>(res.Data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.GetPickUpAddressAndTemplatePrintList: " + ex.Message);
        //    }
        //    return Json(node, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult SearchTemplatePrint(ZTemplatePrintSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TEMPLATE_PRINT_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(new ZTemplatePrintSearchListRequest()
                    {
                        PrintType = req.PrintType,
                        ShopID = req.ShopID,
                        TemplateType = "ORDER"
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "HandleController.SearchTemplatePrint: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RedirectUsingAjax(string URL)
        {
            return Json(new { RedirectURL = URL }, JsonRequestBehavior.AllowGet);
        }

        #region Phóng to hình ảnh và hiển thị dạng Slide
        public ActionResult ImageZoomAndSliderView(List<string> ListImage, string ActiveImage)
        {
            List<ImageZoomAndSliderViewModel> model = new List<ImageZoomAndSliderViewModel>();

            for (var i = 0; i < ListImage.Count; i++)
            {
                model.Add(new ImageZoomAndSliderViewModel()
                {
                    //Title = ListImage[i].Title,
                    Url = ListImage[i],
                    IsActive = ActiveImage == ListImage[i] ? "active" : ""
                });
            }

            return PartialView(model);
        }
        #endregion
    }
}