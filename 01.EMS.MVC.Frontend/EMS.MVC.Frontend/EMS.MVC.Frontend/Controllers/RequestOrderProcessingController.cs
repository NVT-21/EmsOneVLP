using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Controllers
{
    public class RequestOrderProcessingController : BaseController
    {
        #region Danh sách tình trạng xử lý yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "DANH_SACH_TINH_TRANG_XU_LY")]
        public ActionResult RequestOrderProcessingProcessView()
        {
            return View();
        }

        public ActionResult SearchListRequestOrderProcessingView()
        {
            return PartialView();
        }
        #endregion

        #region Thêm tình trạng xử lý
        [AuthorizeController(FunctionalTitle = "THEM_TINH_TRANG_XU_LY")]
        public ActionResult AddRequestOrderProcessingProcessView()
        {
            return PartialView();
        }

        public JsonResult JAddRequestOrderProcessingProcess()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderProcessingController.JAddRequestOrderProcessingProcess: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mở/ khóa tình trạng xử lý yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "MO_HOAC_KHOA_TINH_TRANG_XU_LY")]
        public JsonResult JChangeIsLock()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderProcessingController.JChangeIsLock: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Xóa tình trạng xử lý yêu cầu đặt hàng
        [AuthorizeController(FunctionalTitle = "XOA_TINH_TRANG_XU_LY")]
        public JsonResult JRemoveRequestOrderProcessing()
        {
            ZResponse res = new ZResponse();
            try
            {

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderProcessingController.JRemoveRequestOrderProcessing: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}