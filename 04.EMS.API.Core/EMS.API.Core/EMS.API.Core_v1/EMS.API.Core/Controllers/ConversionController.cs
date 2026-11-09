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
    [RoutePrefix("Conversion")]
    public class ConversionController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZConversionSearchListRequest req)
        {
            int result = -1;
            List<ZConversionSearchListResponse> oList = new List<ZConversionSearchListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONVERSIONS_PKG.SEARCH_LIST_CONVERSION", new object[]
                {
                    req.PageID,
                    req.UserID,
                    req.Channel,
                    req.LimitRow,
                    req.UpdatedDate,
                    req.SearchKey,
                    req.StickerID,
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
                                    List<ZConversationStickerSearchListResponse> listSticker = new List<ZConversationStickerSearchListResponse>();
                                    var resListSticker = ConversationStickerRepository.ListCStickerOfC(new ZConversationStickerSearchListRequest()
                                    {
                                        PageID = req.PageID,
                                        Channel = req.Channel,
                                        ConversationID = Convert.ToInt32(dr["CONVERSION_ID"])
                                    });
                                    if (resListSticker.Code == "00")
                                    {
                                        listSticker = JsonConvert.DeserializeObject<List<ZConversationStickerSearchListResponse>>(resListSticker.Data);
                                    }
                                    oList.Add(new ZConversionSearchListResponse()
                                    {
                                        ConversionID = Convert.ToInt32(dr["CONVERSION_ID"]),
                                        UserID = Convert.ToString(dr["USER_ID"]),
                                        SenderID = Convert.ToString(dr["SENDER_ID"]),
                                        DisplayName = string.IsNullOrEmpty(Convert.ToString(dr["DISPLAY_NAME"])) ? XHelper.DisplayNameDefault : Convert.ToString(dr["DISPLAY_NAME"]),
                                        Avatar = string.IsNullOrEmpty(Convert.ToString(dr["AVATAR"])) ? XHelper.UrlAvatarDefault : Convert.ToString(dr["AVATAR"]),
                                        ProfileLink = Convert.ToString(dr["USER_LINK"]),
                                        MessageType = Convert.ToString(dr["MESSAGE_TYPE"]),
                                        MessageText = Convert.ToString(dr["MESSAGE_TEXT"]),
                                        NumberOfMessage = Convert.ToInt32(dr["NUMBER_OF_MESSAGE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        ListSticker = listSticker
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConversionController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("ZaloSyncConversion")]
        //public ZResponse ZaloSyncConversion(List<ZZaloConversionAddRequest> req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        var ConvertXML = req.ToXML();
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CONVERSIONS_PKG.ZAL_SYNC_CONVERSION", new object[]
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ConversionController.ZaloSyncConversion: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("ZaloSyncConversion")]
        public ZResponse ZaloSyncConversion(List<ZZaloConversionAddRequest> req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                var ConvertXML = req.ToXML();
                var _parameters = new OracleParameter[]
                {
                    new OracleParameter() { ParameterName = "P_VALUE", Direction = ParameterDirection.ReturnValue, OracleDbType = OracleDbType.Int32},

                    new OracleParameter() { ParameterName = "P_LIST_CONVERSION", Direction = ParameterDirection.Input, Value = ConvertXML, OracleDbType = OracleDbType.Clob}
                };

                XHelper.ExecuteNonQueryV2(XHelper.Schema +
                    "EMS_CONVERSIONS_PKG.ZAL_SYNC_CONVERSION", _parameters, CommandType.StoredProcedure);

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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConversionController.ZaloSyncConversion: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
    }
}