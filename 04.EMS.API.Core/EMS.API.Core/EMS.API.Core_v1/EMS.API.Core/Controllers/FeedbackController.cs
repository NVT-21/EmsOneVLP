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
    [RoutePrefix("Feedback")]
    public class FeedbackController : ApiController
    {
        [Route("ListPageFeedback")]
        public ZResponse ListPageFeedback(ZFacebookPageFeedbackRequest req)
        {
            int result = -1;
            List<ZFacebookPageFeedbackResponse> oList = new List<ZFacebookPageFeedbackResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_SERVICE_PKG.LIST_PAGE_FEEDBACK", new object[]
                {
                    req.ScriptType,
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
                                    oList.Add(new ZFacebookPageFeedbackResponse()
                                    {
                                        PageID = Convert.ToString(dr["PAGE_ID"]),
                                        AccountToken = Convert.ToString(dr["ACCESS_TOKEN"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FeedbackController.ListPageFeedback" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListAutoFeedback")]
        public ZResponse ListAutoFeedback(ZFacebookListAutoFeedbackRequest req)
        {
            int result = -1;
            List<ZFacebookListAutoFeedbackRespone> oList = new List<ZFacebookListAutoFeedbackRespone>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_SERVICE_PKG.LIST_FEEDBACK", new object[]
                {
                    req.PageID,
                    req.ScriptType,
                    req.ExecutionTimePeriod,
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
                                    if (req.ScriptType == 1)
                                    {
                                        oList.Add(new ZFacebookListAutoFeedbackRespone()
                                        {
                                            ID = Convert.ToInt32(dr["ID"]),
                                            ChannelCommentID = Convert.ToString(dr["CHANNEL_COMMENT_ID"]),
                                            Message = Convert.ToString(dr["MESSAGE"]),
                                            FromTime = Convert.ToString(dr["FROM_TIME"]),
                                            EndTime = Convert.ToString(dr["END_TIME"])
                                        });
                                    }
                                    else
                                    {
                                        oList.Add(new ZFacebookListAutoFeedbackRespone()
                                        {
                                            ID = Convert.ToInt32(dr["ID"]),
                                            Message = Convert.ToString(dr["MESSAGE"]),
                                            FromTime = Convert.ToString(dr["FROM_TIME"]),
                                            EndTime = Convert.ToString(dr["END_TIME"]),
                                            SenderID = Convert.ToString(dr["SENDER_ID"])
                                        });
                                    }
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
                    else if (result == -1)
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FeedbackController.ListAutoFeedback" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateAutoFeedback")]
        public ZResponse UpdateAutoFeedback(ZFacebookUpdateAutoFeedbackRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_FACEBOOK_SERVICE_PKG.UPDATE_FEEDBACK", new object[]
                {
                    0,
                    req.ScriptType,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "FeedbackController.UpdateAutoFeedback: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}