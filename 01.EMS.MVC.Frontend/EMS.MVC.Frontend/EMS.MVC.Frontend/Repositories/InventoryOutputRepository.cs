using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
namespace EMS.MVC.Frontend.Repositories
{
    public class InventoryOutputRepository
    {
        #region Danh sách phiếu xuất kho
        public static ZResponse SearchListInventoryOutput(ZInventoryOutputSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_OUTPUT_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputRepository.SearchListInventoryOutput: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Thêm phiếu xuất kho
        public static ZResponse AddInventoryOutput(ZInventoryOutputAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_OUTPUT_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputRepository.AddInventoryOutput: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Lấy thông tin phiếu xuất kho để sửa.
        public static ZResponse SearchByInventoryID(ZInventoryOutputGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_OUTPUT_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputRepository.SearchByInventoryID: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Chi tiết phiếu xuất kho
        public static ZResponse InventoryOutputInfo(ZInventoryOutputInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_OUTPUT_INFO",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputRepository.InventoryOutputInfo: " + ex.Message);
            }
            return res;
        }
        #endregion


        #region Sửa phiếu xuất kho
        public static ZResponse EditInventoryOutput(ZInventoryOutputEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_OUTPUT_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryOutputRepository.EditInventoryOutput: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Xử lý phiếu xuất kho
        public static ZResponse ProcessInventoryOutput(ZInventoryOutputProcessRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_OUTPUT_PROCESS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.ProcessInventoryOutput: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Phê duyệt phiếu xuất kho
        public static ZResponse AcceptInventoryOutput(ZInventoryOutputAcceptRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_OUTPUT_ACCEPT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.AcceptInventoryOutput: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region Từ chối phiếu xuất kho
        public static ZResponse RejectInventoryOutput(ZInventoryOutputRejectRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_OUTPUT_REJECT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryImportRepository.RejectInventoryOutput: " + ex.Message);
            }
            return res;
        }
        #endregion
    }
}