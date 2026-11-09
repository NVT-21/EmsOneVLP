using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using EMS.API.Core.Repositories;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("EmployeeFunctional")]
    public class EmployeeFunctionalController : ApiController
    {
        [Route("ListMenu")]
        public ZResponse ListMenu(ZFunctionalListMenuRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZFunctionalListMenuResponse> ListMenu = new List<ZFunctionalListMenuResponse>();
            List<ZFunctionalListMenuResponse> ListAllFunctional = new List<ZFunctionalListMenuResponse>();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_FUNCTIONAL_PKG.LIST_MENU", new object[]
                {
                    req.EmployeeID,
                    req.ShopID,
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
                                    ListAllFunctional.Add(new ZFunctionalListMenuResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FunctionalCode = Convert.ToString(dr["FUNCTIONAL_CODE"]),
                                        FunctionalName = Convert.ToString(dr["FUNCTIONAL_NAME"]),
                                        FunctionalLevel = Convert.ToInt32(dr["FUNCTIONAL_LEVEL"]),
                                        FunctionalParentID = Convert.ToInt32(dr["FUNCTIONAL_PARENT_ID"]),
                                        MenuIcon = Convert.ToString(dr["MENU_ICON"]),
                                        IsType = Convert.ToInt32(dr["IS_TYPE"]),
                                        IsValue = Convert.ToInt32(dr["IS_VALUE"]),
                                        FunctionalTitle = Convert.ToString(dr["FUNCTIONAL_TITLE"]),
                                    });
                                }
                            }
                            if (ListAllFunctional.Count > 0)
                            {
                                var FilterLevel0 = ListAllFunctional.Where(m => m.FunctionalLevel == 0).ToList();
                                for (var i = 0; i < FilterLevel0.Count; i++)
                                {
                                    var FilterLevel1 = ListAllFunctional.Where(m => m.FunctionalParentID == FilterLevel0[i].ID).ToList();
                                    for (var j = 0; j < FilterLevel1.Count; j++)
                                    {
                                        var IsCheck = ListAllFunctional.Where(m => m.FunctionalParentID == FilterLevel1[j].ID && m.IsValue == 1).ToList().Count;
                                        if (IsCheck > 0)
                                        {
                                            FilterLevel1[j].IsValue = 1;
                                        }
                                    }
                                    FilterLevel0[i].IsValue = FilterLevel1.Where(m => m.IsValue == 1).ToList().Count > 0 ? 1 : 0;
                                    FilterLevel0[i].ListFunctional = FilterLevel1.Where(m => m.IsValue == 1).ToList();
                                }
                                ListMenu = FilterLevel0.Where(m => m.IsValue == 1).ToList();
                                if (ListMenu.Count > 0)
                                {
                                    res.Code = "00";
                                    res.Message = "Thành công.";
                                    res.Data = JsonConvert.SerializeObject(ListMenu);
                                }
                                else
                                {
                                    res.Code = "02";
                                    res.Message = "Lấy dữ liệu thất bại.";
                                }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeFunctionalController.ListMenu " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListFunctional")]
        public ZResponse ListFunctional(ZFunctionalListRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZFunctionalListResponse> ListItem = new List<ZFunctionalListResponse>();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_FUNCTIONAL_PKG.LIST_FUNCTIONAL", new object[]
                {
                    req.ShopID,
                    req.GroupPermissionID,
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
                                    ListItem.Add(new ZFunctionalListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FunctionalCode = Convert.ToString(dr["FUNCTIONAL_CODE"]),
                                        FunctionalName = Convert.ToString(dr["FUNCTIONAL_NAME"]),
                                        FunctionalLevel = Convert.ToInt32(dr["FUNCTIONAL_LEVEL"]),
                                        FunctionalParentID = Convert.ToInt32(dr["FUNCTIONAL_PARENT_ID"]),
                                        IsValue = Convert.ToInt32(dr["IS_VALUE"]),
                                    });
                                }
                            }
                            if (ListItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công";
                                res.Data = JsonConvert.SerializeObject(ListItem);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeFunctionalController.ListFunctional " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeFunctional")]
        public ZResponse ChangeFunctional(ZFunctionalChangeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_FUNCTIONAL_PKG.CHANGE_ROLE_FUNCTION", new object[]
                {
                    0,
                    req.ShopID,
                    req.GroupPermissionID,
                    req.strFunctionalID,
                    req.UpdatedBy
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
                                    res.Message = "Cập nhật thất bại.";
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
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }

            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeFunctionalController.ChangeFunctional: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("CheckFunctional")]
        public ZResponse CheckFunctional(ZFunctionalCheckRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_FUNCTIONAL_PKG.CHECK_FUNCTIONAL", new object[]
                {
                    0,
                    req.EmployeeID,
                    req.ShopID,
                    req.FunctionalTitle,
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
                                    res.Message = "Nhân viên chưa thuộc nhóm quyền nào";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Nhóm quyền chưa có chức năng";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Không có quyền truy cập.";
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
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeFunctionalController.CheckFunctional: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}