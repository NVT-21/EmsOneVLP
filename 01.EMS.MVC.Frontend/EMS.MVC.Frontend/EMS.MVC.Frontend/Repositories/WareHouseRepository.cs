using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class WareHouseRepository
    {
        public static ZResponse SearchListWareHouse(ZWareHouseSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WAREHOUSE_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.SearchListWareHouse: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddWareHouse(ZWareHouseAddNewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WAREHOUSE_ADDNEW",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.AddWareHouse: " + ex.Message);
            }
            return res;
        }

        public static ZResponse DetailWareHouseByWareHouseID(ZWareHouseGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WAREHOUSE_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.DetailWareHouseByWareHouseID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditWareHouse(ZWareHouseEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WAREHOUSE_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.EditWareHouse: " + ex.Message);
            }
            return res;
        }

        public static ZResponse DetailWareHouseDefault(ZWarehouseGetDefaultWarehouseRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WAREHOUSE_GET_DEFAULT_WAREHOUSE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.DetailWareHouseDefault: " + ex.Message);
            }
            return res;
        }

        public static ZResponse ChangeIsLock(ZWareHouseChangeIsLockRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WAREHOUSE_CHANGE_IS_LOCK",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.ChangeIsLock: " + ex.Message);
            }
            return res;
        }

        public static ZResponse DetailProductByWareHouseID(ZWarehouseGetProductsByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "WAREHOUSE_GET_PRODUCTS_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.DetailProductByWareHouseID: " + ex.Message);
            }
            return res;
        }
    }
}