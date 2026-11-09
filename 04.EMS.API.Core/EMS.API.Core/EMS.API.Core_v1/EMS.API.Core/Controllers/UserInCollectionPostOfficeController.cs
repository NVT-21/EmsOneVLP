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
    [RoutePrefix("UserInCollectionPostOffice")]
    public class UserInCollectionPostOfficeController : ApiController
    {
        //[Route("SearchList")]
        //public ZResponse SearchList(ZUserInCollectionPostOfficeSearchListRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        List<ZUserInCollectionPostOfficeSearchListResponse> list = new List<ZUserInCollectionPostOfficeSearchListResponse>();
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_IN_COLLECT_PO_PKG.SEARCH_LIST", new object[]
        //        {
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
        //                            list.Add(new ZUserInCollectionPostOfficeSearchListResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                CollectionPostOfficeCode = dr["COLLECTION_POST_OFFICE_CODE"].ToString(),
        //                                CollectionPostOfficeName = dr["COLLECTION_POST_OFFICE_NAME"].ToString(),
        //                                ParentID = Convert.ToInt32(dr["PARENT_ID"]),
        //                                IsChoose = dr["IS_CHOOSE"].ToString(),
        //                                NumberOfChild = Convert.ToInt32(dr["NUMBER_OF_CHILD"]),
        //                            });
        //                        }
        //                    }
        //                    if (list.Count > 0)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Thành công.";
        //                        res.Data = JsonConvert.SerializeObject(list);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
        //                    }
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "UserInCollectionPostOfficeController.SearchList" + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("SearchListByUserID")]
        public ZResponse SearchListByUserID(ZUserInCollectionPostOfficeSearchListByUserIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZUserInCollectionPostOfficeSearchListByUserIDResponse> list = new List<ZUserInCollectionPostOfficeSearchListByUserIDResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_USER_IN_COLLECT_PO_PKG.SEARCH_LIST_BY_USER_ID", new object[]
                {
                    req.UserID,
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
                                    list.Add(new ZUserInCollectionPostOfficeSearchListByUserIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        CollectionPostOfficeCode = dr["COLLECTION_POST_OFFICE_CODE"].ToString(),
                                        CollectionPostOfficeName = dr["COLLECTION_POST_OFFICE_NAME"].ToString(),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        IsChoose = dr["IS_CHOOSE"].ToString(),
                                        NumberOfChild = Convert.ToInt32(dr["NUMBER_OF_CHILD"]),
                                    });
                                }
                            }
                            if (list.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(list);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "UserInCollectionPostOfficeController.SearchListByUserID" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}