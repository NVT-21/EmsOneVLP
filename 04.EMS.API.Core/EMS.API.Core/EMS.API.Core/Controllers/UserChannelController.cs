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
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;
using EMS.API.Core.Repositories;
namespace EMS.API.Core.Controllers
{
    [RoutePrefix("UserChannel")]
    public class UserChannelController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZUserChannelSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZUserChannelSearchListResponse> oList = new List<ZUserChannelSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_CHANNEL_PKG.SEARCH_LIST", new object[]
                {
                    req.PageID,
                    req.Status,
                    req.Channel,
                    req.IsFollow,
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
                                    oList.Add(new ZUserChannelSearchListResponse()
                                    {
                                        UserChannelID = Convert.ToInt32(dr["USER_CHANNEL_ID"]),
                                        ChannelUserID = Convert.ToString(dr["CHANNEL_USER_ID"]),
                                        ChannelUserIDByApp = Convert.ToString(dr["CHANNEL_USER_ID_BY_APP"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        UserGender = XData.GetNameGender(Convert.ToInt32(dr["USER_GENDER"]).ToString()),
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
                                        AuthorizationCode = Convert.ToString(dr["AUTHORIZATION_CODE"]),
                                        AccessToken = Convert.ToString(dr["ACCESS_TOKEN"]),
                                        RefreshToken = Convert.ToString(dr["REFRESH_TOKEN"]),
                                        UserAccessToken = Convert.ToString(dr["USER_ACCESS_TOKEN"]),
                                        PageAccessToken = Convert.ToString(dr["PAGE_ACCESS_TOKEN"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserChannelController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #region Zalo
        [Route("ServiceUpdateInformation")]
        public ZResponse ServiceUpdateInformation(ZUserChannelServiceUpdateInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_CHANNEL_PKG.SERVICE_UPDATE_INFORMATION", new object[]
                {
                    0,
                    req.UserChannelID,
                    req.DisplayName,
                    req.BirthDay,
                    req.Avatar,
                    req.Gender,
                    req.IsFollower,
                    req.Link
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserChannelController.ServiceUpdateInformation" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ZaloFollow")]
        public ZResponse ZaloFollow(ZZaloFollowOrUnFollowRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = UserChannelRepository.FollowOrUnFollow(req, "Y");
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserChannelController.ZaloFollow: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ZaloUnFollow")]
        public ZResponse ZaloUnFollow(ZZaloFollowOrUnFollowRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = UserChannelRepository.FollowOrUnFollow(req, "N");
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserChannelController.ZaloUnFollow: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchByChannelUserID")]
        public ZResponse SearchByChannelUserID(ZUserChannelGetByChannelUserIDRequest req)
        {
            int result = -1;
            ZUserChannelGetByChannelUserIDResponse Item = null;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_CHANNEL_PKG.SEARCH_BY_CHANNEL_USER_ID", new object[]
                {
                    req.PageID,
                    req.ChannelUserID,
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
                                if (dr.Read())
                                {
                                    Item = new ZUserChannelGetByChannelUserIDResponse()
                                    {
                                        UserChannelID = Convert.ToInt32(dr["USER_CHANNEL_ID"]),
                                        DisplayName = Convert.ToString(dr["DISPLAY_NAME"]) == "" ? "Ẩn danh" : Convert.ToString(dr["DISPLAY_NAME"]),
                                        //Avatar = Convert.ToString(dr["AVATAR"]) == "" ? "http://124.158.6.19:1204/Assets/Images/image-default/avatar_default.jpg" : Convert.ToString(dr["AVATAR"]),
                                        Avatar = Convert.ToString(dr["AVATAR"]) == "" ? "https://dev-front.emsone.com.vn:1200/Assets/images/avatar_default.jpg" : Convert.ToString(dr["AVATAR"]),
                                        ChannelUserID = Convert.ToString(dr["CHANNEL_USER_ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        Province = new ZProvinceModel() 
                                        {
                                            ID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        },
                                        District = new ZDistrictModel()
                                        {
                                            ID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        },
                                        Ward = new ZWardModel()
                                        {
                                            ID = Convert.ToInt32(dr["WARD_ID"]),
                                        },
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        IsBlackList = Convert.ToString(dr["IS_BLACK_LIST"]),
                                    };
                                }
                            }
                            if (Item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(Item);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserChannelController.SearchByChannelUserID" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("WebUpdateInformation")]
        public ZResponse WebUpdateInformation(ZUserChannelWebUpdateInformationRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_CHANNEL_PKG.WEB_UPDATE_INFORMATION", new object[]
                {
                    0,
                    req.UserChannelID,
                    req.FullName,
                    req.MobileNumber,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.Channel,
                    req.IsBlackList,
                    req.isUpdatedCustomer,
                    req.ShopID,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserChannelController.WebUpdateInformation" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("ZaloSyncFollow")]
        //public ZResponse ZaloSyncFollow(ZZaloAddFollowerTypeXMLRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        var ConvertXML = req.ListProfileFollower.ToXML();
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_CHANNEL_PKG.ZAL_SYNC_FOLLOWERS", new object[]
        //        {
        //            0,
        //            ConvertXML
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Cập nhật thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại";
        //                            break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "UserChannelController.ZaloSyncFollow" + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("ZaloSyncFollow")]
        public ZResponse ZaloSyncFollow(ZZaloAddFollowerTypeXMLRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                var ConvertXML = req.ListProfileFollower.ToXML();
                var _parameters = new OracleParameter[]
                {
                    new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32},

                    new OracleParameter() { ParameterName = "P_LIST_FOLLOWER", Direction = ParameterDirection.Input, Value = ConvertXML, OracleDbType = OracleDbType.Clob}
                };

                XHelper.ExecuteNonQueryV2(XHelper.Schema +
                    "EMS_USER_CHANNEL_PKG.ZAL_SYNC_FOLLOWERS", _parameters, CommandType.StoredProcedure);

                result = (int)((Oracle.ManagedDataAccess.Types.OracleDecimal)_parameters[00].Value).Value;

                if (result > 0)
                {
                    res.Code = "00";
                    res.Message = "Thành công.";
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserChannelController.ZaloSyncFollow: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        #endregion
    }
}