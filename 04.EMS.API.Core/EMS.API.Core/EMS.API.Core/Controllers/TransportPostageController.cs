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
    [RoutePrefix("TransportPostage")]
    public class TransportPostageController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZTransportPostageSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            List<ZTransportPostageSearchListResponse> list = new List<ZTransportPostageSearchListResponse>();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.SEARCH_LIST", new object[]
                {
                    req.Type,
                    0,                   //P_RETURN_CODE OUT NUMBER
                    0                    //P_OUT_CURSOR  OUT T_CURSOR
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
                                    list.Add(new ZTransportPostageSearchListResponse()
                                    {
                                        TransportPostageID = Convert.ToInt32(dr["TRANSPORT_POSTAGE_ID"]),
                                        TransportPostageCode = dr["TRANSPORT_POSTAGE_CODE"].ToString(),
                                        TransportPostageName = dr["TRANSPORT_POSTAGE_NAME"].ToString(),
                                        StartDate = dr["START_DATE"].ToString(),
                                        EndDate = dr["END_DATE"].ToString(),
                                        IsLock = dr["IS_LOCK"].ToString(),
                                        VAT = new ZBaseResponse()
                                        {
                                            Code = dr["VAT"].ToString(),
                                            Name = XData.GetNameYesNoStatus(dr["VAT"].ToString())
                                        },
                                        Status = new ZBaseResponse()
                                        {
                                            Code = dr["STATUS"].ToString(),
                                            Name = XData.GetNameTransportPostageStatus(dr["STATUS"].ToString()),
                                        },
                                        CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        TransportServiceCode = dr["TRANSPORT_SERVICE_CODE"].ToString(),
                                        TransportServiceName = dr["TRANSPORT_SERVICE_NAME"].ToString(),
                                        ListCrmOrPaypostCode = dr["LIST_CRM_OR_PAYPOST_CODE"].ToString(),
                                    });
                                }
                            }

                            if (list.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(list);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.SearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("MainServiceBasicAdd")]
        public ZResponse MainServiceBasicAdd(ZTransportPostageMainServiceBasicAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.MAIN_SERVICE_BASIC_ADD", new object[]
                {
                   0,
                   req.TransportPostageCode,    //P_POSTAGE_CODE                VARCHAR,
                   req.TransportPostageName,    //P_POSTAGE_NAME                VARCHAR,
                   //req.TransportPostageType,  //P_POSTAGE_TYPE                NUMBER,
                   //req.CrmOrPaypostCode,      //P_CRM_OR_PAYPOST_CODE         VARCHAR,
                   req.StartDate,               //P_START_DATE                  VARCHAR,
                   req.EndDate,                 //P_END_DATE                    VARCHAR,
                   req.MainTransportServiceID,  //P_MAIN_SERVICE_ID             NUMBER,
                   req.ExtraTransportServiceID, //P_MAIN_SERVICE_IDS            VARCHAR,
                   req.Area,                    //P_AREA                        NUMBER,
                   req.SubjectOfApplication,    //P_SUBJECTS_OF_APPLICATION     NUMBER,
                   req.AppliesToShop,           //P_APPLIES_TO_SHOP         VARCHAR,
                   req.AppliesToProvinceFrom,   //P_APPLIES_TO_PROVINCE_FROM    VARCHAR,
                   req.ProvinceApply,           //P_PROVINCE_APPLY              NUMBER,
                   req.AreaApply,               //P_AREA_APPLY                  NUMBER,
                   req.CreatedBy,               //P_CREATED_BY                  NUMBER,
                   0                            //P_OUT_POSTAGE_ID              NUMBER
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
                            res.Data = dbCommand.Parameters["P_OUT_TRANSPORT_POSTAGE_ID"].Value.ToString();
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã bảng cước đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên bảng cước đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Loại áp dụng của dịch vụ chính này đã tồn tại, vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Nơi xuất phát đã tồn tại, vui lòng kiểm tra lại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Khách hàng đã thuộc bảng cước dịch vụ chính, vui lòng kiểm tra lại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Thêm khách hàng thất bại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Thêm nơi xuất phát thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.MainServiceBasicAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("MainServiceBasicEdit")]
        public ZResponse MainServiceBasicEdit(ZTransportPostageMainServiceBasicEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.MAIN_SERVICE_BASIC_EDIT", new object[]
                {
                   0,
                   req.TransportPostageID,      //P_TRANSPORT_POSTAGE_ID        NUMBER,
                   req.TransportPostageCode,    //P_TRANSPORT_POSTAGE_CODE      VARCHAR,
                   req.TransportPostageName,    //P_TRANSPORT_POSTAGE_NAME      VARCHAR,
                   req.StartDate,               //P_START_DATE                  VARCHAR,
                   req.EndDate,                 //P_END_DATE                    VARCHAR,
                   req.MainTransportServiceID,  //P_MAIN_SERVICE_ID             NUMBER,
                   req.ExtraTransportServiceID, //P_EXTRA_SERVICE_IDS           VARCHAR
                   req.Area,                    //P_AREA                        NUMBER,
                   req.SubjectOfApplication,    //P_SUBJECTS_OF_APPLICATION     NUMBER,
                   req.AppliesToShop,           //P_APPLIES_TO_SHOP             VARCHAR,
                   req.AppliesToProvinceFrom,   //P_APPLIES_TO_PROVINCE_FROM    VARCHAR,
                   req.ProvinceApply,           //P_PROVINCE_APPLY              NUMBER
                   req.AreaApply,               //P_AREA_APPLY                  NUMBER
                   req.UpdateBy,                //P_UPDATED_BY                  NUMBER
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
                                    res.Message = "Không tìm thấy bảng cước. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không được phép chỉnh sửa. Vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mã bảng cước đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Tên bảng cước đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Loại áp dụng của dịch vụ chính này đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Nơi xuất phát đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Khách hàng đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Thêm khách hàng thất bại.";
                                    break;
                                case -9:
                                    res.Code = "09";
                                    res.Message = "Thêm nơi xuất phát thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.MainServiceBasicEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("MainServiceFeeEdit")]
        public ZResponse MainServiceFeeEdit(ZTransportPostageMainServiceFeeEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.MAIN_SERVICE_FEE_EDIT", new object[]
                {
                   0,
                   req.TransportPostageID,
                   req.MainFeeRange,
                   req.MainFeeVAT,
                   req.UpdateBy
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
                                    res.Message = "Không tìm thấy bảng cước";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không được phép chỉnh sửa. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.MainServiceFeeEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("MainServiceSurchargeEdit")]
        public ZResponse MainServiceSurchargeEdit(ZTransportPostageSurchargeEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.MAIN_SERVICE_SURCHARGE_EDIT", new object[]
                {
                   0,
                   req.TransportPostageID,
                   req.SurchargeVAT,
                   req.Fuel.Checked,
                   req.Fuel.Unit,
                   req.Fuel.Value,
                   req.Fuel.MinValue,
                   req.Fuel.MaxValue,
                   req.Fuel.InOfProvince,
                   req.Fuel.OutOfProvince,
                   req.RemoteArea.Checked,
                   req.RemoteArea.Unit,
                   req.RemoteArea.Value,
                   req.RemoteArea.MinValue,
                   req.RemoteArea.MaxValue,
                   req.RemoteArea.InOfProvince,
                   req.RemoteArea.OutOfProvince,
                   req.RemoteArea.IsFrom,
                   req.IslandsArea.Checked,
                   req.IslandsArea.Unit,
                   req.IslandsArea.Value,
                   req.IslandsArea.MinValue,
                   req.IslandsArea.MaxValue,
                   req.IslandsArea.InOfProvince,
                   req.IslandsArea.OutOfProvince,
                   req.IslandsArea.IsFrom,
                   req.UpdateBy
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
                                    res.Message = "Không tìm thấy bảng cước";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không được phép chỉnh sửa. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.MainServiceSurchargeEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ExtraServiceBasicAdd")]
        public ZResponse ExtraServiceBasicAdd(ZTransportPostageExtraServiceBasicAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.EXTRA_SERVICE_BASIC_ADD", new object[]
                {
                       0,
                       req.ExtraTransportServiceID,
                       req.AppliesToShop,
                       req.VAT,
                       req.StartDate,
                       req.EndDate,
                       req.ChargingConditionsID,
                       req.ChargingConditionsValue,
                       req.ChargingConditionsMinValue,
                       req.ServicesUsed,
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
                                    res.Message = "Dịch vụ cộng thêm không tồn tại";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Bảng cước dịch vụ cộng thêm đã tồn tại";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Khách hàng đã thuộc bảng cước dịch vụ cộng thêm, vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Thêm khách hàng thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.ExtraServiceBasicAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ExtraServiceBasicEdit")]
        public ZResponse ExtraServiceBasicEdit(ZTransportPostageExtraServiceEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.EXTRA_SERVICE_BASIC_EDIT", new object[]
                {
                   0,
                   req.TransportPostageID,
                   req.AppliesToShop,
                   req.VAT,
                   req.StartDate,
                   req.EndDate,
                   req.ChargingConditionsID,
                   req.ChargingConditionsValue,
                   req.ChargingConditionsMinValue,
                   req.ServicesUsed,
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
                                    res.Message = "Bảng cước không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Bảng cước dịch vụ cộng thêm đã tồn tại";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Khách hàng đã thuộc bảng cước dịch vụ cộng thêm, vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Thêm khách hàng thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.ExtraServiceBasicEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByTransportPostageID")]
        public ZResponse GetByTransportPostageID(int TransportPostageID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZTransportPostageGetByTransportPostageIDResponse model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.GET_BY_TRANSPORT_POSTAGE_ID", new object[]
                {
                    TransportPostageID,
                    0,                   //P_RETURN_CODE OUT NUMBER
                    0                    //P_OUT_CURSOR  OUT T_CURSOR
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
                                    model = new ZTransportPostageGetByTransportPostageIDResponse()
                                    {
                                        TransportPostageID = Convert.ToInt32(dr["TRANSPORT_POSTAGE_ID"]),
                                        TransportPostageCode = dr["TRANSPORT_POSTAGE_CODE"].ToString(),
                                        TransportPostageName = dr["TRANSPORT_POSTAGE_NAME"].ToString(),
                                        //CrmOrPaypostCode = dr["CRM_OR_PAYPOST_CODE"].ToString(),
                                        StartDate = dr["START_DATE"].ToString(),
                                        EndDate = dr["END_DATE"].ToString(),
                                        MainTransportServiceID = Convert.ToInt32(dr["SERVICE_ID"]),
                                        ExtraTransportServiceID = dr["EXTRA_SERVICE_IDS"].ToString(),
                                        Area = string.IsNullOrEmpty(dr["AREA"].ToString()) ? 0 : Convert.ToInt32(dr["AREA"]),
                                        SubjectOfApplication = string.IsNullOrEmpty(dr["SUBJECTS_OF_APPLICATION"].ToString()) ? 0 : Convert.ToInt32(dr["SUBJECTS_OF_APPLICATION"]),
                                        //ApplicationObjectValue = dr["APPLICATION_OBJECT_VALUE"].ToString(),
                                        AppliesToShop = dr["APPLIES_TO_SHOP"].ToString(),
                                        AppliesToProvinceFrom = dr["APPLIES_TO_PROVINCE_FROM"].ToString(),
                                        Status = dr["STATUS"].ToString(),
                                        ProvinceApply = Convert.ToInt32(dr["PROVINCE_APPLY"]),
                                        AreaApply = Convert.ToInt32(dr["AREA_APPLY"]),
                                        CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),

                                        MainFeeVAT = dr["MAIN_FEE_VAT"].ToString(),
                                        MainFeeRange = Convert.ToInt32(dr["MAIN_FEE_RANGE"]),
                                        UpdateBy = Convert.ToInt32(dr["UPDATED_BY"]),
                                        UpdateDate = dr["UPDATED_DATE"].ToString(),

                                        SurchargeVAT = dr["S_VAT"].ToString(),

                                        Fuel = new ZTransportPostageGetByTransportPostageIDFuelResponse()
                                        {
                                            Checked = dr["S_FUEL"].ToString(),
                                            Unit = Convert.ToInt32(dr["S_FUEL_UNIT"]),
                                            Value = Convert.ToDouble(dr["S_FUEL_VALUE"]),
                                            MinValue = Convert.ToInt64(dr["S_FUEL_MIN_VALUE"]),
                                            MaxValue = Convert.ToInt64(dr["S_FUEL_MAX_VALUE"]),
                                            InOfProvince = dr["S_FUEL_IN_OF_PROVINCE"].ToString(),
                                            OutOfProvince = dr["S_FUEL_OUT_OF_PROVINCE"].ToString()
                                        },
                                        RemoteArea = new ZTransportPostageGetByTransportPostageIDRemoteAreaResponse()
                                        {
                                            Checked = dr["S_REMOTE_AREA"].ToString(),
                                            Unit = Convert.ToInt32(dr["S_REMOTE_AREA_UNIT"]),
                                            Value = Convert.ToDouble(dr["S_REMOTE_AREA_VALUE"]),
                                            MinValue = Convert.ToInt64(dr["S_REMOTE_AREA_MIN_VALUE"]),
                                            MaxValue = Convert.ToInt64(dr["S_REMOTE_AREA_MAX_VALUE"]),
                                            InOfProvince = dr["S_REMOTE_AREA_IN_OF_PROVINCE"].ToString(),
                                            OutOfProvince = dr["S_REMOTE_AREA_OUT_OF_PROVINCE"].ToString(),
                                            IsFrom = dr["S_REMOTE_AREA_IS_FROM"].ToString(),

                                        },
                                        IslandsArea = new ZTransportPostageGetByTransportPostageIDIslandsAreaResponse()
                                        {
                                            Checked = dr["S_ISLANDS_AREA"].ToString(),
                                            Unit = Convert.ToInt32(dr["S_ISLANDS_AREA_UNIT"]),
                                            Value = Convert.ToDouble(dr["S_ISLANDS_AREA_VALUE"]),
                                            MinValue = Convert.ToInt64(dr["S_ISLANDS_AREA_MIN_VALUE"]),
                                            MaxValue = Convert.ToInt64(dr["S_ISLANDS_AREA_MAX_VALUE"]),
                                            InOfProvince = dr["S_ISLANDS_AREA_IN_OF_PROVINCE"].ToString(),
                                            OutOfProvince = dr["S_ISLANDS_AREA_OUT_OF_PROVINCE"].ToString(),
                                            IsFrom = dr["S_ISLANDS_AREA_IS_FROM"].ToString(),
                                        },

                                        TransportServiceName = dr["TRANSPORT_SERVICE_NAME"].ToString(),
                                        VAT = new ZBaseResponse() 
                                        {
                                            Code = dr["VAT"].ToString(),
                                            Name = XData.GetNameYesNoStatus(dr["VAT"].ToString()),
                                        },
                                        ChargingConditionsID = Convert.ToInt32(dr["CHARGING_CONDITIONS_ID"]),
                                        ChargingConditionsValue = Convert.ToDouble(dr["CHARGING_CONDITIONS_VALUE"]),
                                        ChargingConditionsMinValue = Convert.ToInt64(dr["CHARGING_CONDITIONS_MIN_VALUE"]),
                                        ServicesUsed = dr["SERVICES_USED"].ToString()
                                    };
                                }
                            }
                            if(model != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.GetByTransportPostageID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeIsLock")]
        public ZResponse ChangeIsLock(ZTransportPostageChangeIsLockRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.CHANGE_IS_LOCK", new object[]
                {
                    0,
                    req.ID,
                    req.IsLock,
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
                                    res.Message = "Trạng thái Mở/ Khóa không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Bảng cước đang bị khóa.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Bảng cước đang mở khóa.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Dịch vụ đang bị khóa";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.ChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Confirm")]
        public ZResponse Confirm(ZTransportPostageConfirmRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.CONFIRM", new object[]
                {
                    0,
                    req.ID,
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
                                    res.Message = "Không tìm thấy thông tin phù hợp.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không hợp lệ. Vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Khai báo cước chính - Chưa khai báo loại phạm vi. Vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Khai báo cước chính - Chưa cấu hình cước dịch vụ chính. Vui lòng kiểm tra lại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Thời gian kết thúc không hợp lệ. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.Confirm: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UnConfirm")]
        public ZResponse UnConfirm(ZTransportPostageUnConfirmRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.UNCONFIRM", new object[]
                {
                    0,
                    req.ID,
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
                                    res.Message = "Không tìm thấy thông tin phù hợp.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không hợp lệ. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.UnConfirm: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchHistory")]
        public ZResponse SearchHistory(ZTransportPostageSearchHistoryRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZTransportPostageSearchHistoryResponse> model = new List<ZTransportPostageSearchHistoryResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_POSTAGE_PKG.SEARCH_HISTORY", new object[]
                {
                    req.TransportPostageID,
                    0,                   //P_RETURN_CODE OUT NUMBER
                    0                    //P_OUT_CURSOR  OUT T_CURSOR
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
                                    model.Add(new ZTransportPostageSearchHistoryResponse()
                                    {
                                        Name = Convert.ToString(dr["NAME"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        CreatedName = Convert.ToString(dr["CREATED_NAME"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"])
                                    });
                                }
                            }

                            if (model.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageController.SearchHistory: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}