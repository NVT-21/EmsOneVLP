using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Repositories;

namespace EMS.MVC.Frontend.Repositories
{
    public class StockTakeRepository
    {
        #region Tìm kiếm danh sách kiểm kho
        public static ZResponse SearchListStockTake(ZStockTakeSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STOCK_TAKE_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeRepository.SearchListStockTake: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Thêm phiếu kiểm kho
        public static ZResponse AddStockTake(ZStockTakeAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STOCK_TAKE_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeRepository.AddStockTake: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Sửa phiếu kiểm kho
        public static ZResponse EditStockTake(ZStockTakeEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STOCK_TAKE_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeRepository.EditStockTake: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lấy thông tin phiếu kiểm kho
        public static ZResponse SearchByStockTakeID(ZStockTakeGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STOCK_TAKE_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeRepository.SearchByStockTakeID: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Duyệt phiếu kiểm kho
        public static ZResponse AcceptStockTake(ZStockTakeAcceptRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STOCK_TAKE_ACCEPT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeRepository.AcceptStockTake: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Từ chối phiếu kiểm kho
        public static ZResponse RejectStockTake(ZStockTakeRejectRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STOCK_TAKE_REJECT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeRepository.RejectStockTake: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Xóa phiếu kiểm kho
        public static ZResponse RemoveStockTake(ZStockTakeRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STOCK_TAKE_REMOVE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "StockTakeRepository.RemoveStockTake: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}