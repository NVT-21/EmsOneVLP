using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class InventoryImportRepository
    {
        #region Danh sách phiếu nhập kho
        public static ZResponse SearchListInventoryImport(ZInventoryImportSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_IMPORT_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.SearchListInventoryImport: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Thêm phiếu nhập kho
        public static ZResponse AddInventoryImport(ZInventoryImportAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_IMPORT_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.AddInventoryImport: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lấy thông tin phiếu nhập kho bằng ID 
        public static ZResponse SearchByInventoryID(ZInventoryImportGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_IMPORT_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.SearchByInventoryID: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Sửa phiếu nhập kho
        public static ZResponse EditInventoryImport(ZInventoryImportEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_IMPORT_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.EditInventoryImport: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Xác nhận phiếu nhập kho
        public static ZResponse AcceptInventoryImport(ZInventoryImportAcceptRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_IMPORT_ACCEPT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.AcceptInventoryImport: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Từ chối phiếu nhập kho
        public static ZResponse RejectInventoryImport(ZInventoryImportRejectRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_IMPORT_REJECT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.RejectInventoryImport: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Xóa phiếu nhập kho
        public static ZResponse RemoveInventoryImport(ZInventoryImportRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_IMPORT_REMOVE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.RemoveInventoryImport: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}