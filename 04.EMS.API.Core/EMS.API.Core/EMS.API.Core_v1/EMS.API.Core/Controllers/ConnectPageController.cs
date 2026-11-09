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
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("ConnectPage")]
    public class ConnectPageController: ApiController
    {
        [Route("AddConnectPage")]
        public ZResponse AddConnectPage(ZConnectPageAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.ADD_CONNECT_PAGE", new object[]
                {
                    0,
                    req.AccountID,
                    req.PageID,
                    req.PageName,
                    req.PageAvatar,
                    req.PageCover,
                    req.PageCategory,
                    req.PageDescription,
                    req.PageCipher,
                    req.IsVerified,
                    req.AuthorizationCode,
                    req.AccessToken,
                    req.RefreshToken,
                    req.UserAccessToken,
                    req.PageAccessToken,
                    req.Channel,
                    req.ShopID,
                    req.CreatedBy
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
                                    res.Message = "Trang đã được kết nối với tài khoản shop khác. Vui lòng hủy liên kết trước khi kết nối mới.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Không tồn tại kênh bán hàng.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lỗi thêm thời gian hoạt động.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.AddConnectPage" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchListConnectPage")]
        public ZResponse SearchListConnectPage(ZConnectPageSearchListRequest req)
        {
            int result = -1;
            List<ZConnectPageSearchListResponse> oList = new List<ZConnectPageSearchListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.SEARCH_LIST_CONNECT_PAGE", new object[]
                {
                    req.PageID,
                    req.ShopID,
                    req.Channel,
                    req.IsActive,
                    req.IsSyncFollower,
                    req.IsSyncConversion,
                    req.IsSyncMessage,
                    req.IsSyncProduct,
                    req.IsSyncProductPrice,
                    req.IsSyncProductStock,
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
                                    oList.Add(new ZConnectPageSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        PageName = Convert.ToString(dr["PAGE_NAME"]),
                                        PageCategory = Convert.ToString(dr["PAGE_CATEGORY"]),
                                        PageDescription = Convert.ToString(dr["PAGE_DESCRIPTION"]),
                                        PageCipher = Convert.ToString(dr["PAGE_CIPHER"]),
                                        PageAvatar = string.IsNullOrEmpty(Convert.ToString(dr["PAGE_AVATAR"])) ? XHelper.UrlAvatarDefault : Convert.ToString(dr["PAGE_AVATAR"]),
                                        PageCover = Convert.ToString(dr["PAGE_COVER"]),
                                        IsActive = Convert.ToString(dr["IS_ACTIVE"]) == "Y" ? "Đang kết nối" : "Ngừng kết nối",
                                        Channel = XData.GetNameChannel(Convert.ToString(dr["CHANNEL"])),
                                        AccessToken = Convert.ToString(dr["ACCESS_TOKEN"]),
                                        RefreshToken = Convert.ToString(dr["REFRESH_TOKEN"]),
                                        UserAccessToken = Convert.ToString(dr["USER_ACCESS_TOKEN"]),
                                        PageAccessToken = Convert.ToString(dr["PAGE_ACCESS_TOKEN"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        CreatedName = Convert.ToString(dr["CREATED_BY"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        IsActiveCheck = Convert.ToString(dr["IS_ACTIVE"]),
                                        AccountID = Convert.ToString(dr["ACCOUNT_ID"])
                                    });
                                }
                            }
                            if (oList.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(oList);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.SearchListConnectPage" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchListConnectPageTokenExpired")]
        public ZResponse SearchListConnectPageTokenExpired(ZConnectPageSearchListTokenExpiredRequest req)
        {
            int result = -1;
            List<ZConnectPageSearchListTokenExpiredResponse> oList = new List<ZConnectPageSearchListTokenExpiredResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.SEARCH_LIST_TOKEN_EXPIRED", new object[]
                {
                    req.Channel,
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
                                    oList.Add(new ZConnectPageSearchListTokenExpiredResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PageID = dr["PAGE_ID"].ToString(),
                                        RefreshToken = dr["REFRESH_TOKEN"].ToString(),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                    });
                                }
                            }
                            if (oList.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(oList);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.SearchListConnectPageTokenExpired" + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateConnectPage")]
        public ZResponse UpdateConnectPage(ZConnectPageUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {

                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.UPDATE_CONNECT_PAGE", new object[]
                {
                    0,
                    req.PageID,
                    req.PageName,
                    req.PageAvatar,
                    req.PageCover,
                    req.PageCategory,
                    req.PageDescription,
                    req.IsVerified,
                    req.AccessToken,
                    req.RefreshToken,
                    req.Channel,
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
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại dữ liệu.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.UpdateConnectPage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateToken")]
        public ZResponse UpdateToken(ZConnectPageUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {

                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.UPDATE_TOKEN", new object[]
                {
                    0,
                    req.PageID,
                    req.ShopID,
                    req.Channel,
                    req.AccessToken,
                    req.RefreshToken,
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
                                    res.Message = "Không tồn tại dữ liệu.";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.UpdateToken: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateActive")]
        public ZResponse UpdateActive(ZConnectPageUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.UPDATE_ACTIVE", new object[]
                {
                    0,
                    req.PageID,
                    req.ShopID,
                    req.Channel,
                    req.IsActive
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
                                    res.Message = "Không tồn tại dữ liệu.";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.UpdateActive: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateSyncFirst")]
        public ZResponse UpdateSyncFirst(ZZaloUpdateSyncFirstRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.UPDATE_SYNC_FIRST", new object[]
                {
                    0,
                    req.PageID,
                    req.ShopID,
                    req.Channel,
                    req.Type
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.UpdateSyncFirst: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ResetConnectPage")]
        public ZResponse ResetConnectPage(ZConnectPageResetRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.RESET_CONNECT_PAGE", new object[]
                {
                    0,
                    req.ShopID,
                    req.AccountID,
                    req.Channel,
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
                                    res.Message = "Không tồn tại kênh bán hàng.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Không tồn tại dữ liệu.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.ResetConnectPage" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("CountFollow")]
        public ZResponse CountFollow(ZConnectPageCountFollowRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.COUNT_FOLLOW", new object[]
                {
                    0,
                    req.PageID,
                    req.Channel,
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
                            res.Message = "Lấy dữ liệu thành công.";
                            res.Data = dbCommand.Parameters[3].Value.ToString();
                        }
                        else
                        {
                            switch (result)
                            {
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.CountFollow: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("DeleteConnectPage")]
        public ZResponse DeleteConnectPage(ZFacebookDeleteConnectRequest req)
            {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONNECT_PAGES_PKG.DELETE_CONNECT_PAGE", new object[]
                {
                    0,
                    req.ShopID,
                    req.PageID,
                    req.Channel,
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
                            res.Message = "Đã xoá liên kết thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại kênh bán hàng.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.DeleteConnectPage: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateConnectPageByService")]
        public ZResponse UpdateConnectPageByService(ZConnectPageUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {

                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_SERVICE_PKG.UPDATE_CONNECT_PAGE", new object[]
                {
                    0,
                    req.ShopID,
                    req.PageID,
                    req.Channel,
                    1
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
                                    res.Message = "Không tồn tại dữ liệu.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.UpdateConnectPageByService: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("UpdateTokenByService")]
        public ZResponse UpdateTokenByService(ZConnectPageUpdateRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {

                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_SERVICE_PKG.UPDATE_PAGE_ACCESS_TOKEN", new object[]
                {
                    0,
                    req.AccessToken,
                    req.PageID
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
                                    res.Message = "Không tồn tại dữ liệu.";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.UpdateTokenByService: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("UpdateConnectPageInfoService")]
        public ZResponse UpdateConnectPageInfoService(ZConnectPageUpdateInfoRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_SERVICE_PKG.UPDATE_CONNECT_PAGE_INFO", new object[]
                {
                    0,
                    req.ShopID,
                    req.PageID,
                    req.Channel,
                    req.PageName,
                    req.PageAvatar,
                    req.PageCover,
                    1
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
                                    res.Message = "Không tồn tại dữ liệu.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConnectPageController.UpdateConnectPageInfoService: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}