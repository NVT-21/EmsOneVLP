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
    [RoutePrefix("Permission")]
    public class PermissionController : ApiController
    {
        [Route("List")]
        public ZResponse List(ZPermissionListRequest req)
        {
            int result = -1;
            List<ZPermissionListResponse> lst = new List<ZPermissionListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PERMISSION_PKG.PERMISSION_LIST", new object[]
                {
                    req.ShopLevel,
                    req.IsLock,
                    req.IsShop,
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
                                    lst.Add(new ZPermissionListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PermissionName = Convert.ToString(dr["PERMISSION_NAME"]),
                                        PermissionCode = Convert.ToString(dr["PERMISSION_CODE"]),
                                        ShopLevel = Convert.ToInt32(dr["SHOP_LEVEL"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionController.List " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("ListFunctionalGroup")]
        //public ZResponse ListFunctionalGroup(ZFunctionalGroupListRequest req)
        //{
        //    int result = -1;
        //    List<ZFunctional> lstFunctional = new List<ZFunctional>();
        //    List<ZFunctional> lstSubFunctional = new List<ZFunctional>();
        //    List<ZFunctional> lstSubFunctionalTemp = new List<ZFunctional>();
        //    List<ZFunctionalGroupListResponse> lst = new List<ZFunctionalGroupListResponse>();
        //    List<ZFunctionalGroupListResponse> lstTemp = new List<ZFunctionalGroupListResponse>();
        //    List<int> lstFunctionalGroupID = new List<int>();
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PERMISSION_PKG.LIST_FUNCTIONAL_GROUP", new object[]
        //        {
        //            req.ShopID,
        //            req.GroupPermissionID,
        //            req.IsShop,
        //            req.IsLock,
        //            0,
        //            0
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
        //                if (result > 0)
        //                {
        //                    using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            lstFunctional.Add(new ZFunctional()
        //                            {
        //                                FunctionalGroupID = Convert.ToInt32(dr["FUNCTIONAL_GROUP_ID"]),
        //                                FunctionalID = Convert.ToInt32(dr["FUNCTIONAL_ID"]),
        //                                FunctionalCode = Convert.ToString(dr["FUNCTIONAL_CODE"]),
        //                                FunctionalName = Convert.ToString(dr["FUNCTIONAL_NAME"]),
        //                                Value = Convert.ToInt32(dr["VALUE"]),
        //                                IsSubFunctional = Convert.ToString(dr["IS_SUB_FUNCTIONAL"])
        //                            });
        //                            if (!lstFunctionalGroupID.Exists(m => m == Convert.ToInt32(dr["FUNCTIONAL_GROUP_ID"])))
        //                            {
        //                                lstTemp.Add(new ZFunctionalGroupListResponse()
        //                                {
        //                                    FunctionalGroupID = Convert.ToInt32(dr["FUNCTIONAL_GROUP_ID"]),
        //                                    FunctionalGroupCode = Convert.ToString(dr["FUNCTIONAL_GROUP_CODE"]),
        //                                    FunctionalGroupName = Convert.ToString(dr["FUNCTIONAL_GROUP_NAME"]),
        //                                    Value = Convert.ToInt32(dr["VALUE"]),
        //                                });
        //                                lstFunctionalGroupID.Add(Convert.ToInt32(dr["FUNCTIONAL_GROUP_ID"]));
        //                            }
        //                        }
        //                        foreach (var item in lstTemp)
        //                        {
        //                            var lst1 = lstFunctional.Where(m => m.FunctionalGroupID == item.FunctionalGroupID);
        //                            foreach (var item1 in lst1)
        //                            {
        //                                if (item1.IsSubFunctional == "Y")
        //                                {
        //                                    lstSubFunctional = PermissionRepository.ListSubFunctional(item1.FunctionalID, req.ShopID, req.GroupPermissionID);
        //                                    lstSubFunctionalTemp.Add(new ZFunctional()
        //                                    {
        //                                        FunctionalID = item1.FunctionalID,
        //                                        FunctionalCode = item1.FunctionalCode,
        //                                        FunctionalName = item1.FunctionalName,
        //                                        Value = item1.Value,
        //                                        ListFunctional = lstSubFunctional
        //                                    });
        //                                }
        //                                else
        //                                {
        //                                    lstSubFunctionalTemp.Add(new ZFunctional()
        //                                    {
        //                                        FunctionalID = item1.FunctionalID,
        //                                        FunctionalCode = item1.FunctionalCode,
        //                                        FunctionalName = item1.FunctionalName,
        //                                        Value = item1.Value
        //                                    });
        //                                }
        //                            }
        //                            lst.Add(new ZFunctionalGroupListResponse()
        //                            {
        //                                FunctionalGroupID = item.FunctionalGroupID,
        //                                FunctionalGroupCode = item.FunctionalGroupCode,
        //                                FunctionalGroupName = item.FunctionalGroupName,
        //                                Value = item.Value,
        //                                listFunctional = lstSubFunctionalTemp
        //                            });
        //                            lstSubFunctionalTemp = new List<ZFunctional>();
        //                        }
        //                    }
        //                    if (lst.Count > 0)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Thành công.";
        //                        res.Data = JsonConvert.SerializeObject(lst);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
        //                    }

        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Thất bại.";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionController.ListFunctionalGroup " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("ChangePermission")]
        public ZResponse ChangePermission(ZPermissionChangeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionController.ChangePermission: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListFunctional")]
        public ZResponse ListFunctional(ZFunctionalListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZFunctionalListResponse> ListAllFunctional = new List<ZFunctionalListResponse>();
            List<ZFunctionalListResponse> ListFunctional = new List<ZFunctionalListResponse>();
            try
            {
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
                                    ListAllFunctional.Add(new ZFunctionalListResponse()
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
                            if (ListAllFunctional.Count > 0)
                            {
                                var FilterListLevel0 = ListAllFunctional.Where(m => m.FunctionalLevel == 0);
                                foreach (var ItemLevel0 in FilterListLevel0)
                                {
                                    List<ZFunctionalListResponse> ListFunctionalTemp = new List<ZFunctionalListResponse>();
                                    var FilterListLevel1 = ListAllFunctional.Where(m => m.FunctionalParentID == ItemLevel0.ID);
                                    foreach (var ItemLevel1 in FilterListLevel1)
                                    {
                                        var FilterListLevel2 = ListAllFunctional.Where(m => m.FunctionalParentID == ItemLevel1.ID);
                                        ListFunctionalTemp.Add(new ZFunctionalListResponse()
                                        {
                                            ID = ItemLevel1.ID,
                                            FunctionalCode = ItemLevel1.FunctionalCode,
                                            FunctionalName = ItemLevel1.FunctionalName,
                                            FunctionalLevel = ItemLevel1.FunctionalLevel,
                                            FunctionalParentID = ItemLevel1.FunctionalParentID,
                                            //IsValue = FilterListLevel2.Where(m => m.IsValue == 1).ToList().Count > 0 ? 1 : 0,
                                            IsValue = ItemLevel1.IsValue,
                                            ListFunctional = FilterListLevel2.ToList()
                                        });
                                    }
                                    ListFunctional.Add(new ZFunctionalListResponse()
                                    {
                                        ID = ItemLevel0.ID,
                                        FunctionalCode = ItemLevel0.FunctionalCode,
                                        FunctionalName = ItemLevel0.FunctionalName,
                                        FunctionalLevel = ItemLevel0.FunctionalLevel,
                                        FunctionalParentID = ItemLevel0.FunctionalParentID,
                                        //IsValue = ListFunctionalTemp.Where(m => m.IsValue == 1).ToList().Count > 0 ? 1 : 0,
                                        IsValue = ItemLevel0.IsValue,
                                        ListFunctional = ListFunctionalTemp
                                    });
                                }
                                if (ListFunctional.Count > 0)
                                {
                                    res.Code = "00";
                                    res.Message = "Thành công.";
                                    res.Data = JsonConvert.SerializeObject(ListFunctional);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionController.ListFunctional " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListMenu")]
        public ZResponse ListMenu(ZPermissionMenuListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZFunctionalListResponse> ListAllFunctional = new List<ZFunctionalListResponse>();
            List<ZPermissionMenuListResponse> ListFunctional = new List<ZPermissionMenuListResponse>();
            try
            {
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
                                    ListAllFunctional.Add(new ZPermissionMenuListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FunctionalCode = Convert.ToString(dr["FUNCTIONAL_CODE"]),
                                        FunctionalName = Convert.ToString(dr["FUNCTIONAL_NAME"]),
                                        FunctionalLevel = Convert.ToInt32(dr["FUNCTIONAL_LEVEL"]),
                                        FunctionalParentID = Convert.ToInt32(dr["FUNCTIONAL_PARENT_ID"]),
                                        MenuIcon = Convert.ToString(dr["MENU_ICON"]),
                                        IsType = Convert.ToInt32(dr["IS_TYPE"]),
                                        IsValue = Convert.ToInt32(dr["IS_VALUE"]),
                                    });
                                }
                            }
                            if (ListAllFunctional.Count > 0)
                            {
                                var FilterListLevel0 = ListAllFunctional.Where(m => m.FunctionalLevel == 0);
                                foreach (var ItemLevel0 in FilterListLevel0)
                                {
                                    List<ZFunctionalListResponse> ListFunctionalTemp = new List<ZFunctionalListResponse>();
                                    var FilterListLevel1 = ListAllFunctional.Where(m => m.FunctionalParentID == ItemLevel0.ID);
                                    foreach (var ItemLevel1 in FilterListLevel1)
                                    {
                                        var FilterListLevel2 = ListAllFunctional.Where(m => m.FunctionalParentID == ItemLevel1.ID);
                                        ListFunctionalTemp.Add(new ZPermissionMenuListResponse()
                                        {
                                            ID = ItemLevel1.ID,
                                            FunctionalCode = ItemLevel1.FunctionalCode,
                                            FunctionalName = ItemLevel1.FunctionalName,
                                            FunctionalLevel = ItemLevel1.FunctionalLevel,
                                            FunctionalParentID = ItemLevel1.FunctionalParentID,
                                            IsValue = FilterListLevel2.ToList().Count > 0 ? FilterListLevel2.Where(m => m.IsValue == 1).ToList().Count > 0 ? 1 : 0 : ItemLevel1.IsValue,
                                            ListFunctional = FilterListLevel2.ToList()
                                        });
                                    }
                                    ListFunctional.Add(new ZPermissionMenuListResponse()
                                    {
                                        ID = ItemLevel0.ID,
                                        FunctionalCode = ItemLevel0.FunctionalCode,
                                        FunctionalName = ItemLevel0.FunctionalName,
                                        FunctionalLevel = ItemLevel0.FunctionalLevel,
                                        FunctionalParentID = ItemLevel0.FunctionalParentID,
                                        MenuIcon = ItemLevel0.MenuIcon,
                                        IsType = ItemLevel0.IsType,
                                        IsValue = ListFunctionalTemp.Where(m => m.IsValue == 1).ToList().Count > 0 ? 1 : 0,
                                        ListFunctional = ListFunctionalTemp
                                    });
                                }
                                if (ListFunctional.Count > 0)
                                {
                                    res.Code = "00";
                                    res.Message = "Thành công.";
                                    res.Data = JsonConvert.SerializeObject(ListFunctional.Where(m => m.IsValue == 1).ToList());
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionController.ListMenu " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListAllow")]
        public ZResponse ListAllow(ZPermissionAllowListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZPermissionAllowListResponse> ListData= new List<ZPermissionAllowListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_FUNCTIONAL_PKG.LIST_FUNCTIONAL_ALLOW", new object[]
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
                                    ListData.Add(new ZPermissionAllowListResponse()
                                    {
                                        FunctionalCode = Convert.ToString(dr["FUNCTIONAL_CODE"]),
                                    });
                                }
                            }
                            if(ListData.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công";
                                res.Data = JsonConvert.SerializeObject(ListData);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionController.ListAllow " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}