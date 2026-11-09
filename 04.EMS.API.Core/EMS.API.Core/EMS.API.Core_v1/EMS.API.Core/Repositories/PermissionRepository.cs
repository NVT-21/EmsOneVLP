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
    public class PermissionRepository
    {
        public static ZResponse ResetValue(int ShopID, int GroupPermission)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ROLE_FUNCTIONAL_PKG.RESET_VALUE", new object[]
                {
                        0,
                        ShopID,
                        GroupPermission
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.ResetValue: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static ZResponse ListMenu(int EmployeeID, int ShopID)
        {
            int result = -1;
            List<ZPermissionListMenuResponse> lst = new List<ZPermissionListMenuResponse>();
            ZPermissionListMenuResponse lstParentMenu = new ZPermissionListMenuResponse();
            List<ZListSubMenu> lstSubMenu = new List<ZListSubMenu>();
            List<ZFunctional> lstFunctional = new List<ZFunctional>();
            List<int> lstMenuParentID = new List<int>();
            List<int> lstMenuParentID2 = new List<int>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PERMISSION_PKG.LIST_MENU", new object[]
                {
                    EmployeeID,
                    ShopID,
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
                                    if (Convert.ToInt32(dr["PARENT_MENU_ID"]) > 0)
                                    {
                                        if (!lstMenuParentID.Exists(m => m == Convert.ToInt32(dr["PARENT_MENU_ID"])))
                                        {
                                            lst.Add(new ZPermissionListMenuResponse()
                                            {
                                                ParentMenuID = Convert.ToInt32(dr["PARENT_MENU_ID"]),
                                                ParentMenuCode = Convert.ToString(dr["PARENT_MENU_CODE"]),
                                                ParentMenuName = Convert.ToString(dr["PARENT_MENU_NAME"]),
                                                ParentMenuIcon = Convert.ToString(dr["PARENT_MENU_ICON"]),
                                                ParentMenuIsSpecial = Convert.ToString(dr["PARENT_MENU_IS_SPECIAL"])
                                            });
                                            lstMenuParentID.Add(Convert.ToInt32(dr["PARENT_MENU_ID"]));
                                        }
                                    }
                                    else
                                    {
                                        lstSubMenu.Add(new ZListSubMenu()
                                        {
                                            ParentMenuID = Convert.ToInt32(dr["SUB_MENU_GROUP_FUNCTION_ID"]),
                                            SubMenuID = Convert.ToInt32(dr["SUB_MENU_ID"]),
                                            SubMenuName = Convert.ToString(dr["SUB_MENU_NAME"]),
                                            SubMenuCode = Convert.ToString(dr["SUB_MENU_CODE"]),
                                            SubMenuIcon = Convert.ToString(dr["SUB_MENU_ICON"]),
                                        });

                                        if (!lstMenuParentID2.Exists(m => m == Convert.ToInt32(dr["SUB_MENU_GROUP_FUNCTION_ID"])))
                                        {
                                            lstMenuParentID2.Add(Convert.ToInt32(dr["SUB_MENU_GROUP_FUNCTION_ID"]));
                                        }
                                    }
                                }
                            }
                            foreach (var item1 in lstMenuParentID2)
                            {
                                var lst1 = lstSubMenu.Where(m => m.ParentMenuID == item1);
                                lstParentMenu = GetFunctionalGroupByID(Convert.ToInt32(item1));
                                if(lstParentMenu != null)
                                {
                                    lst.Add(new ZPermissionListMenuResponse()
                                    {
                                        ParentMenuID = lstParentMenu.ParentMenuID,
                                        ParentMenuCode = lstParentMenu.ParentMenuCode,
                                        ParentMenuName = lstParentMenu.ParentMenuName,
                                        ParentMenuIcon = lstParentMenu.ParentMenuIcon,
                                        ParentMenuIsSpecial = lstParentMenu.ParentMenuIsSpecial,
                                        listSubMenu = lst1.ToList()
                                    });
                                }
                            }
                            lstFunctional = GetFunctionalAllowByEmployeeID(EmployeeID, ShopID);
                            lst.Add(new ZPermissionListMenuResponse()
                            {
                                listFunctional = lstFunctional
                            });
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.ListMenu " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        public static List<ZListSubMenu> ListSubMenu(int ParentID)
        {
            int result = -1;
            List<ZListSubMenu> lst = new List<ZListSubMenu>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PERMISSION_PKG.GET_FUNC_GROUP_BY_ID", new object[]
                {
                    ParentID,
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
                                    lst.Add(new ZListSubMenu()
                                    {
                                        SubMenuID = Convert.ToInt32(dr["SUB_MENU_ID"]),
                                        SubMenuCode = Convert.ToString(dr["SUB_MENU_CODE"]),
                                        SubMenuName = Convert.ToString(dr["SUB_MENU_NAME"]),
                                        SubMenuIcon = Convert.ToString(dr["SUB_MENU_ICON"]),
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                return lst;
                            }
                            else
                            {
                                lst = null;
                            }

                        }
                        else
                        {
                            lst = null;
                        }
                    }
                    else
                    {
                        lst = null;
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.ListSubMenu " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            return lst;
        }
        public static List<ZFunctional> ListSubFunctional(int ID, int ShopID, int GroupPermission)
        {
            int result = -1;
            List<ZFunctional> lst = new List<ZFunctional>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PERMISSION_PKG.GET_SUB_FUNC_BY_ID", new object[]
                {
                    ID,
                    ShopID,
                    GroupPermission,
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
                                    lst.Add(new ZFunctional()
                                    {
                                        FunctionalID = Convert.ToInt32(dr["SUB_FUNCTIONAL_ID"]),
                                        FunctionalName = Convert.ToString(dr["SUB_FUNCTIONAL_NAME"]),
                                        Value = Convert.ToInt32(dr["VALUE"]),
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                return lst;
                            }
                            else
                            {
                                lst = null;
                            }

                        }
                        else
                        {
                            lst = null;
                        }
                    }
                    else
                    {
                        lst = null;
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.ListSubFunctional " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            return lst;
        }
        public static List<ZFunctional> ListSubFunctional_V2(int ID, int ShopID)
        {
            int result = -1;
            List<ZFunctional> lst = new List<ZFunctional>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PERMISSION_PKG.GET_SUB_FUNC_BY_ID_V2", new object[]
                {
                    ID,
                    ShopID,
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
                                    lst.Add(new ZFunctional()
                                    {
                                        FunctionalID = Convert.ToInt32(dr["SUB_FUNCTIONAL_ID"]),
                                        FunctionalName = Convert.ToString(dr["SUB_FUNCTIONAL_NAME"]),
                                        Value = Convert.ToInt32(dr["VALUE"]),
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                return lst;
                            }
                            else
                            {
                                lst = null;
                            }

                        }
                        else
                        {
                            lst = null;
                        }
                    }
                    else
                    {
                        lst = null;
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.ListSubFunctional " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            return lst;
        }
        public static ZPermissionListMenuResponse GetFunctionalGroupByID(int ID)
        {
            int result = -1;
            ZPermissionListMenuResponse item = null;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PERMISSION_PKG.GET_FUNC_GROUP_BY_ID", new object[]
                {
                    ID,
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
                                if (dr.Read())
                                {
                                    item = new ZPermissionListMenuResponse()
                                    {
                                        ParentMenuID = Convert.ToInt32(dr["ID"]),
                                        ParentMenuCode = Convert.ToString(dr["CODE"]),
                                        ParentMenuName = Convert.ToString(dr["NAME"]),
                                        ParentMenuIcon = Convert.ToString(dr["ICON"]),
                                        ParentMenuIsSpecial = Convert.ToString(dr["IS_SPECIAL"])
                                    };
                                }
                            }
                            if (item != null)
                            {
                                return item;
                            }
                            else
                            {
                                return item;
                            }

                        }
                        else
                        {
                            return item;
                        }
                    }
                    else
                    {
                        return item;
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.GetFunctionalGroupByID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            return item;
        }
        public static List<ZFunctional> GetFunctionalAllowByEmployeeID(int EmployeeID, int ShopID)
        {
            int result = -1;
            List<ZFunctional> lst = new List<ZFunctional>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PERMISSION_PKG.GET_FUNC_ALLOW_BY_EMP_ID", new object[]
                {
                    EmployeeID,
                    ShopID,
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
                                    lst.Add(new ZFunctional()
                                    {
                                        FunctionalCode = Convert.ToString(dr["FUNCTIONAL_CODE"]),
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                return lst;
                            }
                            else
                            {
                                lst = null;
                            }

                        }
                        else
                        {
                            lst = null;
                        }
                    }
                    else
                    {
                        lst = null;
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.GetFunctionalAllowByEmployeeID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            return lst;
        }

    }
}