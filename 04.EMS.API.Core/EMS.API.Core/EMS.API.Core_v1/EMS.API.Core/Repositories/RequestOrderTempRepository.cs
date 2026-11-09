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
using EMS.API.Core.Controllers;

namespace EMS.API.Core.Repositories
{
    public class RequestOrderTempRepository
    {
        public static ZResponse AddCommit(ZRequestOrderImportAddCommitRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_TEMP_PKG.ADD_COMMIT", new object[]
                {
                    0,
                    req.ImportCode,
                    req.ShopID,
                    req.CreatedBy,
                    XHelper.DeviceName
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
                            res.Code = "99";
                            res.Message = "Thất bại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderTempRepository.AddCommit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse ExcelDivision(ZRequestOrderImportAddCommitRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZRequestOrderImportAddCommitRequest> listEmployee = new List<ZRequestOrderImportAddCommitRequest>();
            var splitEmployeeID = req.strEmployeeID.Split(',');
            var RequestOrderUpdated = 0;
            if (req.Criteria == "DivisionEqually")
            {
                int RecieverRowInt = req.Quantity / (splitEmployeeID.Length); //phần nguyên
                int RecieverRowRemainder = req.Quantity % (splitEmployeeID.Length);//phần dư
                for (var i = 0; i < splitEmployeeID.Length; i++)
                {
                    if (RecieverRowRemainder > 0)
                    {
                        listEmployee.Add(new ZRequestOrderImportAddCommitRequest()
                        {
                            EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
                            NumberOfEmployeeOrder = RecieverRowInt + 1
                        });
                        RecieverRowRemainder--;
                    }
                    else
                    {
                        listEmployee.Add(new ZRequestOrderImportAddCommitRequest()
                        {
                            EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
                            NumberOfEmployeeOrder = RecieverRowInt
                        });
                    }
                }

                foreach (var item in listEmployee)
                {
                    res = ExcelDivisionEqually(req, item.EmployeeID, item.NumberOfEmployeeOrder);
                }
            }
            else
            {
                for (var i = 0; i < splitEmployeeID.Length; i++)
                {
                    res = ExcelDivisionEmployee(req, Convert.ToInt32(splitEmployeeID[i]));
                    if (res.Code == "00")
                    {
                        RequestOrderUpdated += Convert.ToInt32(res.Data);
                    }
                }
                var TotalUpdated = req.Quantity - RequestOrderUpdated;
                if (TotalUpdated > 0)
                {
                    int RecieverRowInt = TotalUpdated / (splitEmployeeID.Length); //phần nguyên
                    int RecieverRowRemainder = TotalUpdated % (splitEmployeeID.Length);//phần dư
                    for (var i = 0; i < splitEmployeeID.Length; i++)
                    {
                        if (RecieverRowRemainder > 0)
                        {
                            listEmployee.Add(new ZRequestOrderImportAddCommitRequest()
                            {
                                EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
                                NumberOfEmployeeOrder = RecieverRowInt + 1
                            });
                            RecieverRowRemainder--;
                        }
                        else
                        {
                            listEmployee.Add(new ZRequestOrderImportAddCommitRequest()
                            {
                                EmployeeID = Convert.ToInt32(splitEmployeeID[i]),
                                NumberOfEmployeeOrder = RecieverRowInt
                            });
                        }
                    }
                    foreach (var item in listEmployee)
                    {
                        res = ExcelDivisionEqually(req, item.EmployeeID, item.NumberOfEmployeeOrder);
                    }
                }
            }
            if (req.Quantity < req.TotalRowExcel)
            {
                res = AddCommit(req);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse ExcelDivisionEqually(ZRequestOrderImportAddCommitRequest req, int EmployeeID, int NumberOfEmployeeOrder)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_TEMP_PKG.EXCEL_DIVISION_EQUALLY", new object[]
                {
                    0,
                    req.ShopID,
                    EmployeeID,
                    NumberOfEmployeeOrder,
                    req.ImportCode,
                    req.CreatedBy,
                    XHelper.DeviceName
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Chia đơn thành công.";
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderTempRepository.ExcelDivisionEqually: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse ExcelDivisionEmployee(ZRequestOrderImportAddCommitRequest req, int EmployeeID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_ORDER_TEMP_PKG.EXCEL_DIVISION_CHECK_EMPLOYEE", new object[]
                {
                    0,
                    req.ShopID,
                    EmployeeID,
                    req.Quantity,
                    req.ImportCode,
                    req.CreatedBy,
                    XHelper.DeviceName,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Chia đơn thành công.";
                            res.Data = Convert.ToString(dbCommand.Parameters["P_OUT_COUNT_UPDATED"].Value);
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestOrderTempRepository.ExcelDivisionEmployee: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}