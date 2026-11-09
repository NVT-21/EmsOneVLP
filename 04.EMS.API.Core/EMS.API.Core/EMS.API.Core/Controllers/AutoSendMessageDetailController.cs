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
using EMS.API.Core.Repositories;
namespace EMS.API.Core.Controllers
{
    [RoutePrefix("AutoSendMessageDetail")]
    public class AutoSendMessageDetailController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZAutoSendMessageDetailSearchListRequest req)
        {
            int result = -1;
            List<ZAutoSendMessageDetailSearchListResponse> oList = new List<ZAutoSendMessageDetailSearchListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_AUTO_SM_DETAIL_PKG.SEARCH_LIST", new object[]
                {
                    req.AutoSendMessageID,
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
                                    oList.Add(new ZAutoSendMessageDetailSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        MessageTitle = Convert.ToString(dr["MESSAGE_TITLE"]),
                                        MessageSubTitle = Convert.ToString(dr["MESSAGE_SUBTITLE"]),
                                        ImageURL = Convert.ToString(dr["IMAGE_URL"]),
                                        LinkURL = Convert.ToString(dr["LINK_URL"]),
                                        FileToken = Convert.ToString(dr["FILE_TOKEN"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "AutoSendMessageDetail.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}