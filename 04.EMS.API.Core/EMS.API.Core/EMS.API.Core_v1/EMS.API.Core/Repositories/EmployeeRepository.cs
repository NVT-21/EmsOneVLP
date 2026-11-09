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

namespace EMS.API.Core.Repositories
{
    public class EmployeeRepository
    {
        public static ZResponse doTimeActiveAdd(int EmployeeID, int Day, string FromTime, string ToTime, string IsChoose)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TIME_ACTIVE_PKG.ADD_NEW", new object[]
                {
                    0,
                    EmployeeID,
                    Day,
                    FromTime,
                    ToTime,
                    IsChoose
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
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
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeRepository.doAddTimeActive: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse ResetTimeActive(int EmployeeID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TIME_ACTIVE_PKG.DEFAULT_IS_CHOOSE", new object[]
                {
                    0,
                    EmployeeID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
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
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeRepository.ResetTimeActive: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse UpdateTimeActive(int EmployeeID, int Day, int ID, string FromTime, string ToTime, string IsChoose)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TIME_ACTIVE_PKG.UPDATE_TIME_ACTIVE", new object[]
                {
                    0,
                    EmployeeID,
                    Day,
                    ID,
                    FromTime,
                    ToTime,
                    IsChoose
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại";
                                    break;
                                case -2:
                                    res.Code = "01";
                                    res.Message = "Mật khẩu mới và mật khẩu cũ không được trùng!";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeRepository.UpdateTimeActive: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse CheckCondition(ZEmployeeCheckConditionRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.CHECK_CONDITION", new object[]
                {
                    0,
                    req.EmployeeID,
                    req.ShopID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Cửa hàng đã bị xóa. Vui lòng liên hệ quản trị viên.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Cửa hàng đã bị khoá. Vui lòng liên hệ quản trị viên.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Cửa hàng đã hết thời hạn sử dụng. Vui lòng liên hệ quản trị viên.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Nhân viên đã bị xóa. Vui lòng liên hệ quản trị viên.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Nhân viên đã bị khóa. Vui lòng liên hệ quản trị viên.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Hết thời gian hoạt động. Vui lòng liên hệ quản trị viên.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
                                    break;
                            }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeRepository.CheckCondition " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

    }
}