using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Provider")]
    public class ProviderController : ApiController
    {
        [Route("List")]
        public ZResponse List(ZProviderSearchListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZProviderSearchListResponse> oItem = new List<ZProviderSearchListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROVIDER_PKG.LIST", new object[]
                {
                    req.Code,           //P_ID          VARCHAR
                    req.Name,           //P_NAME        VARCHAR
                    req.ShortName,      //P_SHORT_NAME  VARCHAR
                    req.MobileNumber,   //P_MOBILE_NUMBER VARCHAR
                    req.IsLock,         //P_IS_LOCK     VARCHAR
                    req.ShopID,         //P_SHOP_ID     NUMBER
                    0,                  //P_RETURN_CODE OUT NUMBER
                    0                   //P_OUT_CURSOR  OUT T_CURSOR
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
                                    oItem.Add(new ZProviderSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = Convert.ToString(dr["CODE"]),
                                        Name = Convert.ToString(dr["NAME"]),
                                        ShortName = Convert.ToString(dr["SHORT_NAME"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        TaxCode = Convert.ToString(dr["TAX_CODE"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                    });
                                }
                            }
                            if (oItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oItem);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.List: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZProviderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROVIDER_PKG.ADD_PROVIDER", new object[]
                {
                    0,
                    req.Code,
                    req.Name,
                    req.ShortName,
                    req.MobileNumber,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.TaxCode,
                    req.Note,
                    req.IsLock,
                    req.ShopID,
                    req.CreatedBy,
                    XHelper.DeviceName
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
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
                                    res.Message = "Mã nhà cung cấp đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên nhà cung cấp đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Số điện thoại đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int ID, int ShopID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZProviderGetByIDResponse model = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROVIDER_PKG.GET_BY_ID", new object[]
                {
                    ID,
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
                                    model = new ZProviderGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = Convert.ToString(dr["CODE"]),
                                        Name = Convert.ToString(dr["NAME"]),
                                        ShortName = Convert.ToString(dr["SHORT_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        TaxCode = Convert.ToString(dr["TAX_CODE"]),
                                        Note = Convert.ToString(dr["NOTE"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"])
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
                                res.Message = "Dữ liệu không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZProviderEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROVIDER_PKG.EDIT_PROVIDER", new object[]
                {
                    0,
                    req.ID,
                    req.Name,
                    req.ShortName,
                    req.MobileNumber,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.TaxCode,
                    req.Note,
                    req.IsLock,
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
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên nhà cung cấp đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Số điện thoại nhà cung cấp đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeIsLock")]
        public ZResponse ChangeIsLock(ZProviderChangeIsLockRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROVIDER_PKG.CHANGE_IS_LOCK", new object[]
                {
                    0,
                    req.ID,
                    req.IsLock,
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
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Sai trạng thái.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderController.ChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}