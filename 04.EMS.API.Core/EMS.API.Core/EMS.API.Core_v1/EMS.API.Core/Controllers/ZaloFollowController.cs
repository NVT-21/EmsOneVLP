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
    [RoutePrefix("ZaloFollow")]
    public class ZaloFollowController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZZaloFollowSearchListRequest req)
        {
            int result = -1;
            List<ZZaloFollowSearchListResponse> oList = new List<ZZaloFollowSearchListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_FOLLOWER_PKG.SEARCH_LIST_FOLLOWER", new object[]
                {
                    req.PageID,
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
                                    oList.Add(new ZZaloFollowSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        ZaloUserID = Convert.ToString(dr["ZALO_USER_ID"]),
                                        ZaloUserIDByApp = Convert.ToString(dr["ZALO_USER_ID_BY_APP"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        UserGender = Convert.ToInt32(dr["USER_GENDER"]) == 1 ? "Nam" : "Nữ",
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Avatar = Convert.ToString(dr["AVATAR"]),
                                        BirthDay = Convert.ToString(dr["BIRTH_DAY"]),
                                        DisplayName = Convert.ToString(dr["DISPLAY_NAME"]),
                                        ProvinceName = Convert.ToString(dr["PROVINCE_NAME"]),
                                        DistrictName = Convert.ToString(dr["DISTRICT_NAME"]),
                                        WardName = Convert.ToString(dr["WARD_NAME"]),
                                        Street = Convert.ToString(dr["STREET"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloFollowController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddFollowerTypeXML")]
        public ZResponse AddFollowerTypeXML(ZZaloAddFollowerTypeXMLRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                var ConvertXML = req.ListProfileFollower.ToXML();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_SYNC_FOLLOWER_PKG.ADD_SYNC_DATA_FOLLOWERS", new object[]
                {
                    0,
                    ConvertXML
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloFollowController.AddFollowerTypeXML" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Follow")]
        public ZResponse Follow(ZZaloFollowOrUnFollowRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_FOLLOWER_PKG.FOLLOW", new object[]
                {
                    0,
                    req.oa_id,
                    req.follower.id,
                    req.user_id_by_app,
                    req.source,
                    XData.ConvertLongTypeDateTime(long.Parse(req.timestamp)),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloFollowController.Follow: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UnFollow")]
        public ZResponse UnFollow(ZZaloFollowOrUnFollowRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_FOLLOWER_PKG.UNFOLLOW", new object[]
                {
                    0,
                    req.oa_id,
                    req.follower.id,
                    req.user_id_by_app,
                    req.source,
                    XData.ConvertLongTypeDateTime(long.Parse(req.timestamp)),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloFollowController.UnFollow: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UserSubmitInfo")]
        public ZResponse UserSubmitInfo(ZZaloUserSubmitInfoRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_FOLLOWER_PKG.USER_SUBMIT_INFO", new object[]
                {
                    0,
                    req.sender.id,
                    req.recipient.id,
                    req.user_id_by_app,
                    req.info.name,
                    req.info.phone,
                    req.info.address,
                    req.info.ward,
                    req.info.district,
                    req.info.city,
                    XData.ConvertLongTypeDateTime(long.Parse(req.timestamp))
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloFollowController.UserSubmitInfo: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UserAuthentication")]
        public ZResponse UserAuthentication(ZZaloUserAuthenticationRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZAL_FOLLOWER_PKG.USER_AUTHENTICATION", new object[]
                {
                    0,
                    req.host.oa_id,
                    req.host.ip,
                    req.user.id,
                    req.user.phone,
                    XData.ConvertLongTypeDateTime(long.Parse(req.timestamp))
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloFollowController.UserAuthentication: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}