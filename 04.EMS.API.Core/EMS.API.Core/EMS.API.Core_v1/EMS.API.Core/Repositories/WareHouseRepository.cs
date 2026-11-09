using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using System.Data.Common;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;

namespace EMS.API.Core.Repositories
{
    public class WareHouseRepository
    {
        public static ZResponse SearchList(ZWareHouseSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZWareHouseSearchListResponse> lst = new List<ZWareHouseSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.SEARCH_LIST", new object[]
                {
                    req.CrmOrPaypostCode,
                    req.IsLock,
                    req.ShopID,
                    req.SearchKey,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    lst.Add(new ZWareHouseSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        WareHouseCode = Convert.ToString(dr["WAREHOUSE_CODE"]),
                                        WareHouseName = Convert.ToString(dr["WAREHOUSE_NAME"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        EmployeeName = Convert.ToString(dr["EMPLOYEE_NAME"]),
                                        IsDefault = Convert.ToString(dr["IS_DEFAULT"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse AddNew(ZWareHouseAddNewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WAREHOUSE_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.WareHouseCode,
                    req.WareHouseName,
                    req.FullName,
                    req.MobileNumber,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.CreatedBy,
                    req.ShopID,
                    XHelper.DeviceName,
                    req.IsDefault
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            //var WareHouseID = dbCommand.Parameters["P_OUT_WAREHOUSE_ID"].Value.ToString();
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã kho đã tồn tại.";
                                    //res.Data = WareHouseID;
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên kho đã tồn tại.";
                                    //res.Data = WareHouseID;
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.AddNew " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}