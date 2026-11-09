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
    [RoutePrefix("AutoHidden")]
    public class AutoHiddenController : ApiController
    {
        [Route("ListPageAutoHidden")]
        public ZResponse ListPageAutoHidden()
        {
            int result = -1;
            List<ZFacebookListPageAutoHiddenResponse> oList = new List<ZFacebookListPageAutoHiddenResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_SERVICE_PKG.LIST_PAGE_AUTO_HIDDEN", new object[]
                {
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
                                    oList.Add(new ZFacebookListPageAutoHiddenResponse()
                                    {
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        AccountToken = Convert.ToString(dr["ACCESS_TOKEN"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        TextInComment = Convert.ToString(dr["TEXT_IN_COMMENT"]),
                                        IsEmail = Convert.ToInt32(dr["IS_EMAIL"]),
                                        IsPhonenumber = Convert.ToInt32(dr["IS_PHONENUMBER"]),
                                        IsText = Convert.ToInt32(dr["IS_TEXT"]),
                                        IsAll = Convert.ToInt32(dr["IS_ALL"]),
                                        AvailableDate = Convert.ToInt32(dr["AVAILABLE_DATE"]),
                                        PageAccessToken = Convert.ToString(dr["PAGE_ACCESS_TOKEN"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "AutoHiddenController.ListPageAutoHidden" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListAutoHidden")]
        public ZResponse ListAutoLike(ZFacebookListAutoHiddenRequest req)
        {
            int result = -1;
            List<ZFacebookListAutoHiddenResponse> oList = new List<ZFacebookListAutoHiddenResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_SERVICE_PKG.LIST_AUTO_HIDDEN", new object[]
                {
                    req.PageID,
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
                                    oList.Add(new ZFacebookListAutoHiddenResponse()
                                    {
                                        ChannelCommentID = Convert.ToString(dr["CHANNEL_COMMENT_ID"]),
                                        Message = Convert.ToString(dr["COMMENT_CONTENT"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "AutoHiddenController.ListAutoHidden" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateAutoHidden")]
        public ZResponse UpdateAutoHidden(ZFacebookUpdateAutoHiddenRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_SERVICE_PKG.UPDATE_AUTO_HIDDEN", new object[]
                {
                    0,
                    req.ChannelCommentID
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "AutoHiddenController.UpdateAutoHidden: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}