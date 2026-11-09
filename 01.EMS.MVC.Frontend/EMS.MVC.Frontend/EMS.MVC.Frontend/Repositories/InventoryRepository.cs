using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Repositories
{
    public class InventoryRepository
    {
        public static ZResponse AddInventory(ZInventoryAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_ADD_V2",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.AddInventory: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SearchListInventory(ZInventorySearchListRequest req) // Tìm phiếu Xuất - Nhập - Điều chỉnh
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.SearchListInventory: " + ex.Message);
            }
            return res;
        }
        public static ZResponse InventoryGetByID(ZInventoryGetByIDRequest req) // Lấy thông tin phiếu Xuất - Nhập - Điều chỉnh
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryGetByID: " + ex.Message);
            }
            return res;
        }
        public static List<ZInventoryDetailGetByInventoryIDResponse> InventoryDetailGetDataByInventoryID(ZInventoryDetailGetByInventoryIDRequest req) // Lấy chi tiết phiếu Xuất - Nhập - Điều chỉnh
        {
            List<ZInventoryDetailGetByInventoryIDResponse> model = new List<ZInventoryDetailGetByInventoryIDResponse>();
            try
            {
                var res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_DETAIL_GET_BY_INVENTORY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
                if (res.Code == "00")
                {
                    model = JsonConvert.DeserializeObject<List<ZInventoryDetailGetByInventoryIDResponse>>(res.Data);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryDetailGetDataByInventoryID: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
            }
            return model;
        }

        public static ZResponse EditInventory(ZInventoryEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.EditInventory: " + ex.Message);
            }
            return res;
        }
        public static ZResponse InventoryDetailEdit(ZInventoryDetailEditRequest req) // Chỉnh sửa chi tiết phiếu Xuất - Nhập - Điều chỉnh
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_DETAIL_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryDetailEdit: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        public static ZResponse InventoryDetailAdd(ZInventoryDetailAddRequest req) // Thêm chi tiêt phiếu Xuất - Nhập - Điều chỉnh
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_DETAIL_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryAdd: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        public static ZResponse InventoryDetailRemove(ZInventoryDetailRemoveRequest req) // Xoá chi tiết phiếu Xuất - Nhập - Điều chỉnh theo ID chi tiết
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_DETAIL_REMOVE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryDetailRemove: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        public static ZResponse InventoryUpdateStatus(ZInventoryUpdateStatusRequest req) // Cập nhật trạng thái phiếu Xuất - Nhập - Điều chỉnh theo ID chi tiết
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_UPDATE_STATUS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryUpdateStatus: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        public static ZResponse InventoryCancel(ZInventoryCancelRequest req) // Huỷ phiếu Xuất - Nhập - Điều chỉnh theo ID chi tiết
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_CANCEL",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryCancel: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        public static ZResponse InventoryDelete(ZInventoryDeleteRequest req) // Xoá phiếu Nhập theo ID
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_DELETE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryDelete: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        public static ZResponse InventoryReject(ZInventoryRejectRequest req) // Từ chối phiếu Xuất - Nhập - Điều chỉnh theo ID chi tiết
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_REJECT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryReject: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        public static ZResponse InventoryConfirm(ZInventoryConfirmRequest req) // Xác nhận phiếu Xuất - Nhập - Điều chỉnh theo ID chi tiết
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "INVENTORY_CONFIRM",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "InventoryRepository.InventoryConfirm: " + ex.Message + ". " + JsonConvert.SerializeObject(req));
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
    }
}