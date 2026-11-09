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
    [RoutePrefix("CollectionPostOffice")]
    public class CollectionPostOfficeController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZCollectionPostOfficeSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZCollectionPostOfficeSearchListResponse> list = new List<ZCollectionPostOfficeSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CO_PO_PKG.SEARCH_LIST_CO_PO", new object[]
                {
                    req.ProvinceID,
                    req.DistrictID,
                    req.IsLock,
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
                                    list.Add(new ZCollectionPostOfficeSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        CollectionPostOfficeCode = dr["COLLECTION_POST_OFFICE_CODE"].ToString(),
                                        CollectionPostOfficeName = dr["COLLECTION_POST_OFFICE_NAME"].ToString(),
                                        FullAddress = dr["FULL_ADDRESS"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.SearchList" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZCollectionPostOfficeAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CO_PO_PKG.ADD_COLLECTION_POST_OFFICE", new object[]
                {
                    0,
                    req.CollectionPostOfficeCode,
                    req.CollectionPostOfficeName,
                    req.CollectionPostOfficeType,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Street,
                    req.IsLock,
                    req.ParentID,
                    req.Email,
                    req.UserCMS,
                    req.CreatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[00].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Trạng thái hoạt động không hợp lệ.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã bưu cục thu gom đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Tên khu vực đã tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Khu vực đang bị khóa, không thể thêm bưu cục với trạng thái hoạt động là mở.";
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
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.Add " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZCollectionPostOfficeEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CO_PO_PKG.EDIT_COLLECTION_POST_OFFICE", new object[]
                {
                    0,
                    req.CollectionPostOfficeID,
                    req.CollectionPostOfficeName,
                    req.CollectionPostOfficeType,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Street,
                    req.IsLock,
                    req.Email,
                    req.UserCMS,
                    req.UpdatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[00].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Trạng thái hoạt động không hợp lệ.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Tên khu vực đã tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Khu vực đang bị khóa, không thể thay đổi trạng thái hoạt động của bưu cục.";
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
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.Edit " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int ID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZCollectionPostOfficeGetByIDResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CO_PO_PKG.GET_BY_ID", new object[]
                {
                    ID,
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
                                    item = new ZCollectionPostOfficeGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        CollectionPostOfficeCode = dr["COLLECTION_POST_OFFICE_CODE"].ToString(),
                                        CollectionPostOfficeName = dr["COLLECTION_POST_OFFICE_Name"].ToString(),
                                        CollectionPostOfficeType = new ZBaseResponse() 
                                        {
                                            Code = dr["COLLECTION_POST_OFFICE_TYPE"].ToString(),
                                            Name = XData.GetNameCollectionPostOfficeType(dr["COLLECTION_POST_OFFICE_TYPE"].ToString()),
                                        },
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        Street = dr["STREET"].ToString(),
                                        FullAddress = dr["FULL_ADDRESS"].ToString(),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        IsLock = new ZBaseResponse() 
                                        {
                                            Code = dr["IS_LOCK"].ToString(),
                                            Name = XData.GetNameIsLock(dr["IS_LOCK"].ToString()),
                                        },
                                        Email = dr["EMAIL"].ToString(),
                                        UserCMS = dr["USER_CMS"].ToString(),
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.GetByID" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchListByParentID")]
        public ZResponse SearchListByParentID(ZCollectionPostOfficeSearchListByParentIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZCollectionPostOfficeSearchListByParentIDResponse> list = new List<ZCollectionPostOfficeSearchListByParentIDResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CO_PO_PKG.SEARCH_LIST_BY_PARENT_ID", new object[]
                {
                    req.ParentID,
                    req.IsLock,
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
                                    list.Add(new ZCollectionPostOfficeSearchListByParentIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        CollectionPostOfficeCode = dr["COLLECTION_POST_OFFICE_CODE"].ToString(),
                                        CollectionPostOfficeName = dr["COLLECTION_POST_OFFICE_NAME"].ToString(),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.SearchListByParentID" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchListRecent")]
        public ZResponse SearchListRecent(ZCollectionPostOfficeSearchListRecentRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZCollectionPostOfficeSearchListRecentResponse> list = new List<ZCollectionPostOfficeSearchListRecentResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CO_PO_PKG.SEARCH_LIST_RECENT", new object[]
                {
                    req.ProvinceID,
                    req.DistrictID,
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
                                    list.Add(new ZCollectionPostOfficeSearchListRecentResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        CollectionPostOfficeCode = dr["COLLECTION_POST_OFFICE_CODE"].ToString(),
                                        CollectionPostOfficeName = dr["COLLECTION_POST_OFFICE_NAME"].ToString(),
                                        FullAddress = dr["FULL_ADDRESS"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CollectionPostOfficeController.SearchListRecent" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}