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

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Zalo")]
    public class ZaloController : ApiController
    {
        [Route("Connect")]
        public ZResponse Connect(ZZaloConnectRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZALO_PKG.CONNECT_ZALO", new object[]
                {
                    0,
                    req.Code,
                    req.OfficialAccountID,
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
                                    res.Message = "Zalo OA đã được kết nối với tài khoản shop khác. Vui lòng hủy liên kết trước khi kết nối mới.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloController.Connect" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchListOA")]
        public ZResponse SearchListOA(ZZaloSearchListOARequest req)
        {
            int result = -1;
            List<ZZaloSearchListOAResponse> oList = new List<ZZaloSearchListOAResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZALO_PKG.SEARCH_LIST_OA", new object[]
                {
                    req.ShopID,
                    req.IsSyncFirst,
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
                                    oList.Add(new ZZaloSearchListOAResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        AuthorizationCode = Convert.ToString(dr["AUTHORIZATION_CODE"]),
                                        RefreshToken = Convert.ToString(dr["REFRESH_TOKEN"]),
                                        OfficialAccountID = Convert.ToString(dr["OFFICIAL_ACCOUNT_ID"]),
                                        OfficialAccountName = Convert.ToString(dr["OFFICIAL_ACCOUNT_NAME"]),
                                        AccessToken = Convert.ToString(dr["ACCESS_TOKEN"]),
                                        Avatar = Convert.ToString(dr["IMG_AVATAR"]),
                                        Cover = Convert.ToString(dr["IMG_COVER"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        CreatedBy = Convert.ToString(dr["CREATED_BY"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloController.SearchListOA" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByOAID")]
        public ZResponse GetByOAID(string OfficialAccountID, int ShopID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZZaloGetByOfficialAccountIDResponse model = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "ZALO_PKG.GET_BY_OA_ID", new object[]
                {
                    OfficialAccountID,
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
                                if (dr.Read())
                                {
                                    model = new ZZaloGetByOfficialAccountIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        OfficialAccountID = Convert.ToString(dr["OFFICIAL_ACCOUNT_ID"]),
                                        OfficialAccountName = Convert.ToString(dr["OFFICIAL_ACCOUNT_NAME"]),
                                        OfficialAccountAvatar = Convert.ToString(dr["OFFICIAL_ACCOUNT_AVATAR"]),
                                        OfficialAccountCover = Convert.ToString(dr["OFFICIAL_ACCOUNT_COVER"]),
                                        OfficialAccountDescription = Convert.ToString(dr["OFFICIAL_ACCOUNT_DESCRIPTION"]),
                                        AccessToken = Convert.ToString(dr["ACCESS_TOKEN"]),
                                        RefreshToken = Convert.ToString(dr["REFRESH_TOKEN"]),
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
                                res.Message = "Không tồn tại";
                                res.Data = null;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloController.GetByOAID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        
    }
}