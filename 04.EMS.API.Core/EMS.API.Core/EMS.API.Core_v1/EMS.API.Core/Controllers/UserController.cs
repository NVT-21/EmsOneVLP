using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        [Route("Login")]
        public ZResponse Login(ZUserLoginRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                var Password = "";
                ZUserLoginResponse model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.DATA_BY_USER_NAME", new object[]
                {
                    req.UserName,    //P_USER_NAME     VARCHAR,
                    0,               //P_RETURN_CODE   OUT NUMBER,
                    0                //P_OUT_CURSOR    OUT T_CURSOR
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
                                    Password = dr["PASSWORD"].ToString();
                                    model = new ZUserLoginResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        GroupName = dr["GROUP_NAME"].ToString(),
                                        UserName = dr["USER_NAME"].ToString(),
                                        FullName = dr["FULL_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Email = dr["EMAIL"].ToString(),
                                        GroupCode = dr["GROUP_CODE"].ToString(),
                                        IsLock = dr["IS_LOCK"].ToString(),
                                    };

                                }
                            }
                            if (model != null)
                            {
                                var EncryptPassword = Security.SHA256(req.UserName + req.Password + Security.PASSWORD_SALT);
                                if (Password == EncryptPassword)
                                {
                                    res.Code = "00";
                                    res.Message = "Đăng nhập thành công";
                                    res.Data = JsonConvert.SerializeObject(model);
                                }
                                else
                                {
                                    res.Code = "01";
                                    res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                                }
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                            }
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
                                    res.Message = "Tài khoản đã bị khoá. Vui lòng liên hệ quản trị viên.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.Login: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZUserAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.UserName,
                    Security.SHA256(req.UserName + Security.DEFAULT_PASSWORD_USER + Security.PASSWORD_SALT),
                    req.FullName,
                    req.MobileNumber,
                    req.Email,
                    req.GroupCode,
                    req.UserCMS,
                    req.ServesCustomer,
                    req.CreatedBy,
                    req.ListCollectionPostOfficeID.ToXML(),
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
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Số điện thoại đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tài khoản đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchList")]
        public ZResponse SearchList(ZUserSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if ( XCommon.ContainsSpecialChars(req.Email) || XCommon.ContainsSpecialChars(req.MobileNumber) || XCommon.ContainsSpecialChars(req.UserGroupCode) )
                {
                    res.Code = "99";
                    res.Message = "Chứa ký tự không hợp lệ";
                }
                else
                {
                    int result = -1;
                    List<ZUserSearchListResponse> lst = new List<ZUserSearchListResponse>();

                    using ( DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.SEARCH_LIST", new object[]
                    {
                    req.UserID,
                    req.UserGroupCode,
                    req.Email,
                    req.MobileNumber,
                    0,
                    0
                    }) )
                    {
                        result = XHelper.ExecuteNonQuery(dbCommand);
                        if ( result >= 0 )
                        {
                            result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                            if ( result > 0 )
                            {

                                using ( IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value )
                                {
                                    while ( dr.Read() )
                                    {
                                        lst.Add(new ZUserSearchListResponse()
                                        {
                                            ID = Convert.ToInt32(dr["ID"]),
                                            GroupName = Convert.ToString(dr["GROUP_NAME"]),
                                            UserName = Convert.ToString(dr["USER_NAME"]),
                                            FullName = Convert.ToString(dr["FULL_NAME"]),
                                            MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                            Email = Convert.ToString(dr["EMAIL"]),
                                            IsLock = Convert.ToString(dr["IS_LOCK"]),
                                            CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
                                            CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                            LastLogin = Convert.ToString(dr["LAST_LOGIN"]),
                                        });
                                    }
                                }
                                if ( lst.Count > 0 )
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
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.SearchList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZUserEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                var a = req.ListCollectionPostOfficeID.ToXML();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.EDIT", new object[]
                {
                    0,
                    req.ID,
                    req.FullName,
                    req.MobileNumber,
                    req.Email,
                    req.GroupCode,
                    req.UserCMS,
                    req.ServesCustomer,
                    req.UpdatedBy,
                    req.ListCollectionPostOfficeID.ToXML(),
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
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Số điện thoại đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int UserID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZUserGetByIDResponse model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.DATA_BY_ID", new object[]
                {
                    UserID,
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
                                    model = new ZUserGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        GroupCode = Convert.ToString(dr["GROUP_CODE"]),
                                        GroupName = Convert.ToString(dr["GROUP_NAME"]),
                                        UserName = Convert.ToString(dr["USER_NAME"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        ServesCustommer = Convert.ToString(dr["SERVES_CUSTOMERS"]),
                                        UserCMS = Convert.ToString(dr["USER_CMS"]),
                                        CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                    };
                                }
                            }
                            if (model != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại";
                            }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeToLocked")]
        public ZResponse ChangeToLocked(ZUserChangeToLockedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.CHANGE_TO_LOCKED", new object[]
                {
                    0,
                    req.ID,
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
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tài khoản này đã bị Khoá. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.ChangeToLocked: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeToUnlocked")]
        public ZResponse ChangeToUnlocked(ZUserChangeToUnlockedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.CHANGE_TO_UNLOCKED", new object[]
                {
                    0,
                    req.ID,
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
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tài khoản này đã Mở Khoá. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.ChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ResetPassword")]
        public ZResponse ResetPassword(ZUserResetPasswordRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.RESET_PASSWORD", new object[]
                {
                    0,
                    req.ID,
                    Security.SHA256(req.UserName + Security.DEFAULT_PASSWORD_USER + Security.PASSWORD_SALT),
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
                                    res.Message = "Dữ liệu không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.ResetPassword: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangePassword")]
        public ZResponse ChangePassword(ZUserChangePasswordRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                var EncrypteOldPassword = Security.SHA256(req.UserName + req.OldPassword + Security.PASSWORD_SALT);
                var EncrypteNewPassword = Security.SHA256(req.UserName + req.NewPassword + Security.PASSWORD_SALT);
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.CHANGE_PASSWORD", new object[]
                {
                    0,
                    req.ID,
                    EncrypteOldPassword,
                    EncrypteNewPassword
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
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mật khẩu cũ không đúng.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mật khẩu mới và mật khẩu cũ không được trùng.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.ChangePassword: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("UserGroupList")]
        //public ZResponse UserGroupList(ZUserListRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        List<ZUserListResponse> lst = new List<ZUserListResponse>();
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.LIST", new object[]
        //        {
        //            req.ID,
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
        //                            lst.Add(new ZUserListResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                GroupCode = Convert.ToString(dr["GROUP_CODE"]),
        //                                GroupName = Convert.ToString(dr["GROUP_NAME"]),
        //                                UserName = Convert.ToString(dr["USER_NAME"]),
        //                                FullName = Convert.ToString(dr["FULL_NAME"]),
        //                                MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
        //                                Email = Convert.ToString(dr["EMAIL"]),
        //                                IsLock = Convert.ToString(dr["IS_LOCK"]),
        //                                CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
        //                                CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
        //                            });
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
        //                        res.Data = null;
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.List " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("CheckCondition")]
        public ZResponse CheckCondition(ZUserCheckConditionRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.CHECK_CONDITION", new object[]
                {
                    0,
                    req.UserID
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
                                    res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.CheckCondition " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByUserName")]
        public ZResponse GetByUserName(string UserName)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZUserGetByUserNameResponse model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_PKG.DATA_BY_USER_NAME", new object[]
                {
                    UserName,    //P_USER_NAME     VARCHAR,
                    0,           //P_RETURN_CODE   OUT NUMBER,
                    0            //P_OUT_CURSOR    OUT T_CURSOR
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
                                    model = new ZUserGetByUserNameResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        GroupName = dr["GROUP_NAME"].ToString(),
                                        UserName = dr["USER_NAME"].ToString(),
                                        FullName = dr["FULL_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Email = dr["EMAIL"].ToString(),
                                        GroupCode = dr["GROUP_CODE"].ToString(),
                                        IsLock = dr["IS_LOCK"].ToString(),
                                    };

                                }
                            }
                            if (model != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserController.GetByUserName: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
