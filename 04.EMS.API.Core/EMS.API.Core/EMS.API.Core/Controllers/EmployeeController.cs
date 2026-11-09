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
using System.Globalization;
using System.Web;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Employee")]
    public class EmployeeController : ApiController
    {
        [Route("Login")]
        public ZResponse Login(ZEmployeeLoginRequest req)
        {
            ZResponse res = new ZResponse();
            ZEmployeeLoginResponse model = null;
            try
            {
                int result = -1;
                var Password = "";
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.DATA_BY_USER_NAME", new object[]
                {
                    req.MobileNumber,    //P_MOBILE_NUMBER     VARCHAR,
                    0,                   //P_RETURN_CODE   OUT NUMBER,
                    0                    //P_OUT_CURSOR    OUT T_CURSOR
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
                                    Password = dr["PASSWORD"].ToString();
                                    model = new ZEmployeeLoginResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        UserName = dr["USER_NAME"].ToString(),
                                        FullName = dr["FULL_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Email = dr["EMAIL"].ToString(),
                                        IsLock = dr["IS_LOCK"].ToString(),
                                        EmployeeLevel = dr["EMPLOYEE_LEVEL"].ToString(),
                                        GroupPermissionID = Convert.ToInt32(dr["GROUP_PERMISSION_ID"]),
                                        StoreOwner = dr["STORE_OWNER"].ToString(),
                                        ReadyReceiveRequestOrder = dr["READY_RECEIVE_REQUEST_ORDER"].ToString(),
                                        ShopInformation = new XShopInformationModel() 
                                        {
                                            ID = Convert.ToInt32(dr["SHOP_ID"]),
                                            FullName = dr["FULL_NAME"].ToString(),
                                            Email = dr["EMAIL_SHOP"].ToString(),
                                            MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                            CrmOrPayPostCode = dr["CRM_OR_PP_CODE"].ToString(),
                                            ContractNumber = dr["CONTRACT_NUMBER"].ToString(),
                                            StoreName = dr["STORE_NAME"].ToString(),
                                            TaxCode = dr["TAX_CODE"].ToString(),
                                            FullAddress = dr["FULL_ADDRESS"].ToString(),
                                            Level = Convert.ToInt32(dr["SHOP_LEVEL"]),
                                            IsLock = dr["SHOP_IS_LOCK"].ToString(),
                                            EndDate = dr["SHOP_END_DATE"].ToString(),
                                            PickupAddressID = Convert.ToInt32(dr["PICKUP_ADDRESS_ID"]),
                                            PickupFullName = dr["PICKUP_FULL_NAME"].ToString(),
                                            PickupMobileNumber = dr["PICKUP_MOBILE_NUMBER"].ToString(),
                                            PickupProvinceID = Convert.ToInt32(dr["PICKUP_PROVINCE_ID"]),
                                            PickupProvinceName = dr["PICKUP_PROVINCE_NAME"].ToString(),
                                            PickupDistrictID = Convert.ToInt32(dr["PICKUP_DISTRICT_ID"]),
                                            PickupDistrictName = dr["PICKUP_DISTRICT_NAME"].ToString(),
                                            PickupWardID = Convert.ToInt32(dr["PICKUP_WARD_ID"]),
                                            PickupWardName = dr["PICKUP_WARD_NAME"].ToString(),
                                            PickupStreet = dr["PICKUP_STREET"].ToString(),
                                            PickupFullAddress = dr["PICKUP_FULL_ADDRESS"].ToString(),
                                            CollectionType = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                            PrintTypeDefault = dr["PRINT_TYPE_DEFAULT"].ToString(),
                                            Status = dr["SHOP_STATUS"].ToString(),
                                        }
                                    };
                                }
                            }
                            if(model != null)
                            {
                                if (req.Password == Password)
                                {
                                    res.Code = "00";
                                    res.Message = "Đăng nhập thành công";
                                    res.Data = JsonConvert.SerializeObject(model);
                                }
                                else
                                {
                                    res.Code = "01";
                                    res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                                }
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tài khoản của quý khách không tồn tại trên hệ thống. Vui lòng thực hiện Đăng ký hoặc liên hệ quản trị viên để được giúp đỡ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Tài khoản của quý khách không tồn tại trên hệ thống. Vui lòng thực hiện Đăng ký hoặc liên hệ quản trị viên để được giúp đỡ.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Cửa hàng đã hết thời hạn sử dụng. Vui lòng liên hệ quản trị viên.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Nhân viên đã bị xóa. Vui lòng liên hệ quản trị viên.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Nhân viên đã bị khóa. Vui lòng liên hệ quản trị viên.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Hết thời gian hoạt động. Vui lòng liên hệ quản trị viên.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Số điện thoại đã tồn tại. Vui lòng liên hệ quản trị viên.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.Login: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("LoginV2")]
        public ZResponse LoginV2(ZEmployeeLoginRequest req)
        {
            ZResponse res = new ZResponse();
            ZEmployeeLoginResponse model = null;
            try
            {
                int result = -1;
                var password = "";
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.DATA_BY_USER_NAME", new object[]
                {
                    req.MobileNumber,    //P_MOBILE_NUMBER     VARCHAR,
                    0,                   //P_RETURN_CODE   OUT NUMBER,
                    0                    //P_OUT_CURSOR    OUT T_CURSOR
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
                                    password = dr["PASSWORD"].ToString();
                                    model = new ZEmployeeLoginResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        UserName = dr["USER_NAME"].ToString(),
                                        FullName = dr["FULL_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Email = dr["EMAIL"].ToString(),
                                        IsLock = dr["IS_LOCK"].ToString(),
                                        EmployeeLevel = dr["EMPLOYEE_LEVEL"].ToString(),
                                        GroupPermissionID = Convert.ToInt32(dr["GROUP_PERMISSION_ID"]),
                                        StoreOwner = dr["STORE_OWNER"].ToString(),
                                        ReadyReceiveRequestOrder = dr["READY_RECEIVE_REQUEST_ORDER"].ToString(),
                                        ShopInformation = new XShopInformationModel()
                                        {
                                            ID = Convert.ToInt32(dr["SHOP_ID"]),
                                            FullName = dr["FULL_NAME"].ToString(),
                                            Email = dr["EMAIL_SHOP"].ToString(),
                                            MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                            CrmOrPayPostCode = dr["CRM_OR_PP_CODE"].ToString(),
                                            ContractNumber = dr["CONTRACT_NUMBER"].ToString(),
                                            StoreName = dr["STORE_NAME"].ToString(),
                                            TaxCode = dr["TAX_CODE"].ToString(),
                                            FullAddress = dr["FULL_ADDRESS"].ToString(),
                                            Level = Convert.ToInt32(dr["SHOP_LEVEL"]),
                                            IsLock = dr["SHOP_IS_LOCK"].ToString(),
                                            EndDate = dr["SHOP_END_DATE"].ToString(),
                                            PickupAddressID = Convert.ToInt32(dr["PICKUP_ADDRESS_ID"]),
                                            PickupName = dr["PICKUP_NAME"].ToString(),
                                            PickupFullName = dr["PICKUP_FULL_NAME"].ToString(),
                                            PickupMobileNumber = dr["PICKUP_MOBILE_NUMBER"].ToString(),
                                            PickupProvinceID = Convert.ToInt32(dr["PICKUP_PROVINCE_ID"]),
                                            PickupProvinceName = dr["PICKUP_PROVINCE_NAME"].ToString(),
                                            PickupDistrictID = Convert.ToInt32(dr["PICKUP_DISTRICT_ID"]),
                                            PickupDistrictName = dr["PICKUP_DISTRICT_NAME"].ToString(),
                                            PickupWardID = Convert.ToInt32(dr["PICKUP_WARD_ID"]),
                                            PickupWardName = dr["PICKUP_WARD_NAME"].ToString(),
                                            PickupStreet = dr["PICKUP_STREET"].ToString(),
                                            PickupFullAddress = dr["PICKUP_FULL_ADDRESS"].ToString(),
                                            CollectionType = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                            PrintTypeDefault = dr["PRINT_TYPE_DEFAULT"].ToString(),
                                            Status = dr["SHOP_STATUS"].ToString(),
                                            TypeShop = Convert.ToInt32(dr["TYPE_SHOP"]),
                                        }
                                    };
                                }
                            }
                            if (model != null)
                            {
                                var encryptPassword = Security.SHA256(req.MobileNumber + req.Password + Security.PASSWORD_SALT);
                                if (encryptPassword == password)
                                {
                                    var resCheckCondition = EmployeeRepository.CheckCondition(new ZEmployeeCheckConditionRequest() { EmployeeID = model.ID, ShopID = model.ShopInformation.ID });
                                    if(resCheckCondition.Code == "00")
                                    {
                                        res.Code = "00";
                                        res.Message = "Đăng nhập thành công";
                                        res.Data = JsonConvert.SerializeObject(model);
                                    }
                                    else
                                    {
                                        res.Code = resCheckCondition.Code;
                                        res.Message = resCheckCondition.Message;
                                    }
                                }
                                else
                                {
                                    res.Code = "01";
                                    res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                                }
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                //case -1:
                                //    res.Code = "01";
                                //    res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                                //    break;
                                //case -2:
                                //    res.Code = "02";
                                //    res.Message = "Tài khoản của quý khách không tồn tại trên hệ thống. Vui lòng thực hiện Đăng ký hoặc liên hệ quản trị viên để được giúp đỡ.";
                                //    break;
                                //case -3:
                                //    res.Code = "03";
                                //    res.Message = "Tài khoản của quý khách không tồn tại trên hệ thống. Vui lòng thực hiện Đăng ký hoặc liên hệ quản trị viên để được giúp đỡ.";
                                //    break;
                                //case -4:
                                //    res.Code = "04";
                                //    res.Message = "Cửa hàng đã hết thời hạn sử dụng. Vui lòng liên hệ quản trị viên.";
                                //    break;
                                //case -5:
                                //    res.Code = "05";
                                //    res.Message = "Nhân viên đã bị xóa. Vui lòng liên hệ quản trị viên.";
                                //    break;
                                //case -6:
                                //    res.Code = "06";
                                //    res.Message = "Nhân viên đã bị khóa. Vui lòng liên hệ quản trị viên.";
                                //    break;
                                //case -7:
                                //    res.Code = "07";
                                //    res.Message = "Hết thời gian hoạt động. Vui lòng liên hệ quản trị viên.";
                                //    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Số điện thoại đã tồn tại. Vui lòng liên hệ quản trị viên.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.LoginV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListAccount")]
        public ZResponse ListAccount(ZEmployeeListAcountRequest req)
        {
            List<ZEmployeeListAcountResponse> lst = new List<ZEmployeeListAcountResponse>();
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.LIST_ACCOUNT_EMPLOYEE", new object[]
                {
                    req.MobileNumber,
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
                                    lst.Add(new ZEmployeeListAcountResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Password = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        StoreName = Convert.ToString(dr["STORE_NAME"])
                                    });
                                }
                            }
                            if (lst != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
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
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ListAccount " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("LoginAPI")]
        public ZResponse LoginAPI(ZEmployeeLoginRequest req)
        {
            ZResponse res = new ZResponse();
            ZEmployeeLoginResponse model = null;
            try
            {
                int result = -1;
                var password = "";
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.DATA_BY_USER_NAME", new object[]
                {
                    req.MobileNumber,    //P_MOBILE_NUMBER     VARCHAR,
                    0,                   //P_RETURN_CODE   OUT NUMBER,
                    0                    //P_OUT_CURSOR    OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if(result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if(result > 0)
                        {
                            using(IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                if(dr.Read())
                                {
                                    password = dr["PASSWORD"].ToString();
                                    model = new ZEmployeeLoginResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        UserName = dr["USER_NAME"].ToString(),
                                        FullName = dr["FULL_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Email = dr["EMAIL"].ToString(),
                                        IsLock = dr["IS_LOCK"].ToString(),
                                        EmployeeLevel = dr["EMPLOYEE_LEVEL"].ToString(),
                                        GroupPermissionID = Convert.ToInt32(dr["GROUP_PERMISSION_ID"]),
                                        StoreOwner = dr["STORE_OWNER"].ToString(),
                                        ReadyReceiveRequestOrder = dr["READY_RECEIVE_REQUEST_ORDER"].ToString(),
                                        ShopInformation = new XShopInformationModel()
                                        {
                                            ID = Convert.ToInt32(dr["SHOP_ID"]),
                                            FullName = dr["FULL_NAME"].ToString(),
                                            Email = dr["EMAIL_SHOP"].ToString(),
                                            MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                            CrmOrPayPostCode = dr["CRM_OR_PP_CODE"].ToString(),
                                            ContractNumber = dr["CONTRACT_NUMBER"].ToString(),
                                            StoreName = dr["STORE_NAME"].ToString(),
                                            TaxCode = dr["TAX_CODE"].ToString(),
                                            FullAddress = dr["FULL_ADDRESS"].ToString(),
                                            Level = Convert.ToInt32(dr["SHOP_LEVEL"]),
                                            IsLock = dr["SHOP_IS_LOCK"].ToString(),
                                            EndDate = dr["SHOP_END_DATE"].ToString(),
                                            PickupAddressID = Convert.ToInt32(dr["PICKUP_ADDRESS_ID"]),
                                            PickupName = dr["PICKUP_NAME"].ToString(),
                                            PickupFullName = dr["PICKUP_FULL_NAME"].ToString(),
                                            PickupMobileNumber = dr["PICKUP_MOBILE_NUMBER"].ToString(),
                                            PickupProvinceID = Convert.ToInt32(dr["PICKUP_PROVINCE_ID"]),
                                            PickupProvinceName = dr["PICKUP_PROVINCE_NAME"].ToString(),                                      
                                            PickupWardID = Convert.ToInt32(dr["PICKUP_WARD_ID"]),
                                            PickupWardName = dr["PICKUP_WARD_NAME"].ToString(),
                                            PickupStreet = dr["PICKUP_STREET"].ToString(),
                                            PickupFullAddress = dr["PICKUP_FULL_ADDRESS"].ToString(),
                                            CollectionType = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                            PrintTypeDefault = dr["PRINT_TYPE_DEFAULT"].ToString(),
                                            Status = dr["SHOP_STATUS"].ToString(),
                                            TypeShop = Convert.ToInt32(dr["TYPE_SHOP"]),
                                        }
                                    };
                                }
                            }
                            if(model != null)
                            {
                                var encryptPassword = Security.SHA256(req.MobileNumber + req.Password + Security.PASSWORD_SALT);
                                if(encryptPassword == password)
                                {
                                    var resCheckCondition = EmployeeRepository.CheckCondition(new ZEmployeeCheckConditionRequest() { EmployeeID = model.ID, ShopID = model.ShopInformation.ID });
                                    if(resCheckCondition.Code == "00")
                                    {
                                        var tokenResponse = XSecurityApi.GenerateToken(model.MobileNumber, model.ShopInformation.CrmOrPayPostCode, model.ShopInformation.ID, model.ID, XHelper.EMSOneSecretKey);
                                        if(tokenResponse.Code == "99")
                                        {
                                            res.Code = tokenResponse.Code;
                                            res.Message = tokenResponse.Message;
                                        }
                                        else
                                        {
                                            var responseAPI = new ZEmployeeLoginAPIResponse
                                            {
                                                ID = model.ID,
                                                CrmOrPayPostCode = model.ShopInformation.CrmOrPayPostCode,
                                                TokenDetail = JsonConvert.DeserializeObject<XTokenModel>(tokenResponse.Data)
                                            };

                                            var resUpdateShopTokenAcess = ShopRepository.UpdateTokenAccess(new ZShopTokenAccessUpdateRequest
                                            {
                                                ShopID = model.ShopInformation.ID,
                                                TokenAccess = responseAPI.TokenDetail.AccessToken,
                                                Expired = responseAPI.TokenDetail.ExpiresAt
                                            });
                                            
                                            res.Code = "00";
                                            res.Message = "Đăng nhập thành công";
                                            res.Data = JsonConvert.SerializeObject(responseAPI);
                                        }
                                        
                                    }
                                    else
                                    {
                                        res.Code = resCheckCondition.Code;
                                        res.Message = resCheckCondition.Message;
                                    }
                                }
                                else
                                {
                                    res.Code = "01";
                                    res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                                }
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                            }
                        }
                        else
                        {
                            switch(result)
                            {
                                //case -1:
                                //    res.Code = "01";
                                //    res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                                //    break;
                                //case -2:
                                //    res.Code = "02";
                                //    res.Message = "Tài khoản của quý khách không tồn tại trên hệ thống. Vui lòng thực hiện Đăng ký hoặc liên hệ quản trị viên để được giúp đỡ.";
                                //    break;
                                //case -3:
                                //    res.Code = "03";
                                //    res.Message = "Tài khoản của quý khách không tồn tại trên hệ thống. Vui lòng thực hiện Đăng ký hoặc liên hệ quản trị viên để được giúp đỡ.";
                                //    break;
                                //case -4:
                                //    res.Code = "04";
                                //    res.Message = "Cửa hàng đã hết thời hạn sử dụng. Vui lòng liên hệ quản trị viên.";
                                //    break;
                                //case -5:
                                //    res.Code = "05";
                                //    res.Message = "Nhân viên đã bị xóa. Vui lòng liên hệ quản trị viên.";
                                //    break;
                                //case -6:
                                //    res.Code = "06";
                                //    res.Message = "Nhân viên đã bị khóa. Vui lòng liên hệ quản trị viên.";
                                //    break;
                                //case -7:
                                //    res.Code = "07";
                                //    res.Message = "Hết thời gian hoạt động. Vui lòng liên hệ quản trị viên.";
                                //    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Số điện thoại đã tồn tại. Vui lòng liên hệ quản trị viên.";
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.LoginV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("CountAccount")]
        public ZResponse CountAccount(ZEmployeeCountAccountRequest req)
        {
            int result = -1;
            ZEmployeeCountAccountResponse item = null;
            //string cPassword = Security.SHA256(req.MobileNumber + req.Password + Security.PASSWORD_SALT);
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.COUNT_ACCOUNT", new object[]
                {
                    0,
                    req.MobileNumber,
                    req.Password,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            item = new ZEmployeeCountAccountResponse()
                            {
                                CountAccount = Convert.ToInt32(dbCommand.Parameters["P_OUT_COUNT_ACCOUNT"].Value),
                                ShopID = Convert.ToInt32(dbCommand.Parameters["P_OUT_SHOP_ID"].Value)
                            };
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                            }

                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tài khoản không tồn tại. Vui lòng liên hệ quản trị viên.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.CountAccount: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddNew")]
        public ZResponse AddNew(ZEmployeeAddNewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.UserName,
                    Security.SHA256(req.UserName + Security.DEFAULT_PASSWORD_EMPLOYEE + Security.PASSWORD_SALT),
                    req.MobileNumber,
                    req.Email,
                    req.FullName,
                    req.EmployeeLevel,
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
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Số điện thoại đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tài khoản  đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.AddNew: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ShopAdd")]
        public ZResponse ShopAdd(ZEmployeeShopAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.SHOP_ADD", new object[]
                {
                    0,
                    req.FullName,
                    req.MobileNumber,
                    req.strGroupPermissionID,
                    req.IsLock,
                    req.Description,
                    Security.SHA256(req.MobileNumber + Security.DEFAULT_PASSWORD_EMPLOYEE + Security.PASSWORD_SALT),
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
                                    res.Message = "Số điện thoại đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Vượt quá số lượng nhân viên được phép tạo.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Thêm nhóm quyền thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ShopAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("List")]
        public ZResponse List(ZEmployeeListRequest req)
        {
            int result = -1;
            List<ZEmployeeListResponse> lst = new List<ZEmployeeListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.LIST", new object[]
                {
                    req.ShopID,
                    req.EmployeeID,
                    req.IsShop,
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
                                    lst.Add(new ZEmployeeListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        EmployeeLevel = Convert.ToString(dr["EMPLOYEE_LEVEL"]),
                                        UserName = Convert.ToString(dr["USER_NAME"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        CreatedBy = Convert.ToString(dr["CREATED_BY"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        CountActiveTime = Convert.ToInt32(dr["COUNT_TIME_ACTIVE"]),
                                        GroupPermissionName = Convert.ToString(dr["GROUP_PERMISSION_NAME"])
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
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
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.List " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZEmployeeEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.EDIT", new object[]
                {
                    0,
                    req.ID,
                    req.FullName,
                    req.Email,
                    req.EmployeeLevel,
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
                            res.Message = "Thành công";
                            //if (req.IsShop == "Y")
                            //{
                            //    res = UpdateEmployeeCorporation(req.ID, 0, "RESET");
                            //    if (res.Code == "00")
                            //    {
                            //        var splitCorporationID = req.CorporationID.Split(',');
                            //        for (var i = 0; i < splitCorporationID.Length; i++)
                            //        {
                            //            res = UpdateEmployeeCorporation(req.ID, Convert.ToInt32(splitCorporationID[i]), "UPDATE");
                            //        }
                            //        if (res.Code == "00")
                            //        {
                            //            res.Code = "00";
                            //            res.Message = "Thành công";
                            //        }
                            //        else
                            //        {
                            //            res.Code = "99";
                            //            res.Message = "Thất bại";
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    res.Code = "00";
                            //    res.Message = "Thành công";
                            //}

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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ShopEdit")]
        public ZResponse ShopEdit(ZEmployeeShopEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.SHOP_EDIT", new object[]
                {
                    0,
                    req.ID,
                    req.ShopID,
                    req.FullName,
                    req.strGroupPermissionID,
                    req.IsLock,
                    req.Description,
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
                            res.Message = "Thành công";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Làm mới nhóm quyền thất bại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Chỉnh sửa nhóm quyền thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ShopEdit: " + ex.Message);
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
            ZEmployeeGetByIDResponse model = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.GET_BY_ID", new object[]
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
                                    model = new ZEmployeeGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        UserName = Convert.ToString(dr["USER_NAME"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        EmployeeLevel = Convert.ToString(dr["EMPLOYEE_LEVEL"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        CountActiveTime = Convert.ToInt32(dr["COUNT_TIME_ACTIVE"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        strGroupPermissionID = Convert.ToString(dr["GROUP_PERMISSION_ID"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByMobileNumber")]
        public ZResponse GetByMobileNumber(string MobileNumber)
        {
            ZResponse res = new ZResponse();
            ZEmployeeGetByMobileNumberResponse model = null;
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.GET_BY_MOBILE_NUMBER", new object[]
                {
                    MobileNumber,
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
                                    model = new ZEmployeeGetByMobileNumberResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        UserName = dr["USER_NAME"].ToString(),
                                        FullName = dr["FULL_NAME"].ToString(),
                                        MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                        Email = dr["EMAIL"].ToString(),
                                        IsLock = dr["IS_LOCK"].ToString(),
                                        EmployeeLevel = dr["EMPLOYEE_LEVEL"].ToString(),
                                        GroupPermissionID = Convert.ToInt32(dr["GROUP_PERMISSION_ID"]),
                                        StoreOwner = dr["STORE_OWNER"].ToString(),
                                        ReadyReceiveRequestOrder = dr["READY_RECEIVE_REQUEST_ORDER"].ToString(),
                                        ShopInformation = new XShopInformationModel()
                                        {
                                            ID = Convert.ToInt32(dr["SHOP_ID"]),
                                            FullName = dr["FULL_NAME"].ToString(),
                                            Email = dr["EMAIL_SHOP"].ToString(),
                                            MobileNumber = dr["MOBILE_NUMBER"].ToString(),
                                            CrmOrPayPostCode = dr["CRM_OR_PP_CODE"].ToString(),
                                            ContractNumber = dr["CONTRACT_NUMBER"].ToString(),
                                            StoreName = dr["STORE_NAME"].ToString(),
                                            TaxCode = dr["TAX_CODE"].ToString(),
                                            FullAddress = dr["FULL_ADDRESS"].ToString(),
                                            Level = Convert.ToInt32(dr["SHOP_LEVEL"]),
                                            IsLock = dr["SHOP_IS_LOCK"].ToString(),
                                            EndDate = dr["SHOP_END_DATE"].ToString(),
                                            PickupAddressID = Convert.ToInt32(dr["PICKUP_ADDRESS_ID"]),
                                            PickupFullName = dr["PICKUP_FULL_NAME"].ToString(),
                                            PickupMobileNumber = dr["PICKUP_MOBILE_NUMBER"].ToString(),
                                            PickupProvinceID = Convert.ToInt32(dr["PICKUP_PROVINCE_ID"]),
                                            PickupProvinceName = dr["PICKUP_PROVINCE_NAME"].ToString(),
                                            PickupDistrictID = Convert.ToInt32(dr["PICKUP_DISTRICT_ID"]),
                                            PickupDistrictName = dr["PICKUP_DISTRICT_NAME"].ToString(),
                                            PickupWardID = Convert.ToInt32(dr["PICKUP_WARD_ID"]),
                                            PickupWardName = dr["PICKUP_WARD_NAME"].ToString(),
                                            PickupStreet = dr["PICKUP_STREET"].ToString(),
                                            PickupFullAddress = dr["PICKUP_FULL_ADDRESS"].ToString(),
                                            CollectionType = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                            PrintTypeDefault = dr["PRINT_TYPE_DEFAULT"].ToString(),
                                            Status = dr["SHOP_STATUS"].ToString(),
                                        }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.GetByMobileNumber: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByGroupPermissionID")]
        public ZResponse GetByGroupPermissionID(int GroupPermissionID, int ShopID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZEmployeeGetByGroupPermissionIDResponse> listItem = new List<ZEmployeeGetByGroupPermissionIDResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.GET_BY_GROUP_PERMISSION_ID", new object[]
                {
                    GroupPermissionID,
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
                                while (dr.Read())
                                {
                                    listItem.Add(new ZEmployeeGetByGroupPermissionIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = Convert.ToString(dr["FULL_NAME"])
                                    });
                                }
                            }
                            if (listItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(listItem);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.GetByGroupPermissionID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeIsLock")]
        public ZResponse ChangeIsLock(ZEmployeeChangeIsLockRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.CHANGE_IS_LOCK", new object[]
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
                                    res.Message = "Không thể khoá vì vẫn còn yêu cầu đặt hàng chưa được xử lý.";
                                    break;
                                case -3:
                                    res.Code = "03";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ResetPassword")]
        public ZResponse ResetPassword(ZEmployeeResetPasswordRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                string pwdHash = Security.SHA256(req.UserName + Security.DEFAULT_PASSWORD_EMPLOYEE + Security.PASSWORD_SALT);
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.RESET_PASSWORD", new object[]
                {
                    0,
                    req.ID,
                    pwdHash,
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
                                    res.Message = "Không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ResetPassword: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangePassword")]
        public ZResponse ChangePassword(ZEmployeeChangePasswordRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.CHANGE_PASSWORD", new object[]
                {
                    0,
                    req.ID,
                    req.OldPassword,
                    req.NewPassword
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
                                    res.Message = "Mật khẩu cũ không đúng.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mật khẩu mới và mật khẩu cũ không được trùng.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ChangePassword: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangePasswordV2")]
        public ZResponse ChangePasswordV2(ZEmployeeChangePasswordRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                var EncrypteOldPassword = Security.SHA256(req.UserName + req.OldPassword + Security.PASSWORD_SALT);
                var EncrypteNewPassword = Security.SHA256(req.UserName + req.NewPassword + Security.PASSWORD_SALT);
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.CHANGE_PASSWORD", new object[]
                {
                    0,
                    req.ID,
                    EncrypteOldPassword,
                    EncrypteNewPassword
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
                                    res.Message = "Mật khẩu cũ không đúng.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mật khẩu mới và mật khẩu cũ không được trùng.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ChangePasswordV2: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Remove")]
        public ZResponse Remove(ZEmployeeRemoveRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.REMOVE", new object[]
                {
                    0,
                    req.ID,
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
                                    res.Message = "Không thể xoá vì vẫn còn yêu cầu đặt hàng chưa được xử lý.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.Remove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("TimeActiveAdd")]
        public ZResponse TimeActiveAdd(ZEmployeeAddTimeActiveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (req.ListMonday != null)
                {
                    for (var i = 0; i < req.ListMonday.Count; i++)
                    {
                        if (req.ListMonday[i].FromTime != null && req.ListMonday[i].ToTime != null)
                        {
                            res = EmployeeRepository.doTimeActiveAdd(req.EmployeeID, 2, req.ListMonday[i].FromTime, req.ListMonday[i].ToTime, "Y");
                        }
                    }
                }
                if (req.ListTuesday != null)
                {
                    for (var i = 0; i < req.ListTuesday.Count; i++)
                    {
                        if (req.ListTuesday[i].FromTime != null && req.ListTuesday[i].ToTime != null)
                        {
                            res = EmployeeRepository.doTimeActiveAdd(req.EmployeeID, 3, req.ListTuesday[i].FromTime, req.ListTuesday[i].ToTime, "Y");
                        }
                    }
                }
                if (req.ListWednesday != null)
                {
                    for (var i = 0; i < req.ListWednesday.Count; i++)
                    {
                        if (req.ListWednesday[i].FromTime != null && req.ListWednesday[i].ToTime != null)
                        {
                            res = EmployeeRepository.doTimeActiveAdd(req.EmployeeID, 4, req.ListWednesday[i].FromTime, req.ListWednesday[i].ToTime, "Y");
                        }
                    }
                }
                if (req.ListThursday != null)
                {
                    for (var i = 0; i < req.ListThursday.Count; i++)
                    {
                        if (req.ListThursday[i].FromTime != null && req.ListThursday[i].ToTime != null)
                        {
                            res = EmployeeRepository.doTimeActiveAdd(req.EmployeeID, 5, req.ListThursday[i].FromTime, req.ListThursday[i].ToTime, "Y");
                        }
                    }
                }
                if (req.ListFriday != null)
                {
                    for (var i = 0; i < req.ListFriday.Count; i++)
                    {
                        if (req.ListFriday[i].FromTime != null && req.ListFriday[i].ToTime != null)
                        {
                            res = EmployeeRepository.doTimeActiveAdd(req.EmployeeID, 6, req.ListFriday[i].FromTime, req.ListFriday[i].ToTime, "Y");
                        }
                    }
                }
                if (req.ListSaturday != null)
                {
                    for (var i = 0; i < req.ListSaturday.Count; i++)
                    {
                        if (req.ListSaturday[i].FromTime != null && req.ListSaturday[i].ToTime != null)
                        {
                            res = EmployeeRepository.doTimeActiveAdd(req.EmployeeID, 7, req.ListSaturday[i].FromTime, req.ListSaturday[i].ToTime, "Y");
                        }
                    }
                }
                if (req.ListSunday != null)
                {
                    for (var i = 0; i < req.ListSunday.Count; i++)
                    {
                        if (req.ListSunday[i].FromTime != null && req.ListSunday[i].ToTime != null)
                        {
                            res = EmployeeRepository.doTimeActiveAdd(req.EmployeeID, 8, req.ListSunday[i].FromTime, req.ListSunday[i].ToTime, "Y");
                        }
                    }
                }
                if (res.Code == "00")
                {
                    res.Code = "00";
                    res.Message = "Thành công.";
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Thất bại.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.TimeActiveAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("TimeActiveEdit")]
        public ZResponse TimeActiveEdit(ZEmployeeEditTimeActiveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                if (req.EmployeeID != 0)
                {
                    res = EmployeeRepository.ResetTimeActive(req.EmployeeID);
                    if (res.Code == "00")
                    {
                        if (req.ListMonday != null)
                        {
                            for (var i = 0; i < req.ListMonday.Count; i++)
                            {
                                if (req.ListMonday[i].FromTime != null && req.ListMonday[i].ToTime != null)
                                {
                                    res = EmployeeRepository.UpdateTimeActive(req.EmployeeID, 2, req.ListMonday[i].ID, req.ListMonday[i].FromTime, req.ListMonday[i].ToTime, "Y");
                                }
                            }
                        }
                        if (req.ListTuesday != null)
                        {
                            for (var i = 0; i < req.ListTuesday.Count; i++)
                            {
                                if (req.ListTuesday[i].FromTime != null && req.ListTuesday[i].ToTime != null)
                                {
                                    res = EmployeeRepository.UpdateTimeActive(req.EmployeeID, 3, req.ListTuesday[i].ID, req.ListTuesday[i].FromTime, req.ListTuesday[i].ToTime, "Y");
                                }
                            }
                        }
                        if (req.ListWednesday != null)
                        {
                            for (var i = 0; i < req.ListWednesday.Count; i++)
                            {
                                if (req.ListWednesday[i].FromTime != null && req.ListWednesday[i].ToTime != null)
                                {
                                    res = EmployeeRepository.UpdateTimeActive(req.EmployeeID, 4, req.ListWednesday[i].ID, req.ListWednesday[i].FromTime, req.ListWednesday[i].ToTime, "Y");
                                }
                            }
                        }
                        if (req.ListThursday != null)
                        {
                            for (var i = 0; i < req.ListThursday.Count; i++)
                            {
                                if (req.ListThursday[i].FromTime != null && req.ListThursday[i].ToTime != null)
                                {
                                    res = EmployeeRepository.UpdateTimeActive(req.EmployeeID, 5, req.ListThursday[i].ID, req.ListThursday[i].FromTime, req.ListThursday[i].ToTime, "Y");
                                }
                            }
                        }
                        if (req.ListFriday != null)
                        {
                            for (var i = 0; i < req.ListFriday.Count; i++)
                            {
                                if (req.ListFriday[i].FromTime != null && req.ListFriday[i].ToTime != null)
                                {
                                    res = EmployeeRepository.UpdateTimeActive(req.EmployeeID, 6, req.ListFriday[i].ID, req.ListFriday[i].FromTime, req.ListFriday[i].ToTime, "Y");
                                }
                            }
                        }
                        if (req.ListSaturday != null)
                        {
                            for (var i = 0; i < req.ListSaturday.Count; i++)
                            {
                                if (req.ListSaturday[i].FromTime != null && req.ListSaturday[i].ToTime != null)
                                {
                                    res = EmployeeRepository.UpdateTimeActive(req.EmployeeID, 7, req.ListSaturday[i].ID, req.ListSaturday[i].FromTime, req.ListSaturday[i].ToTime, "Y");
                                }
                            }
                        }
                        if (req.ListSunday != null)
                        {
                            for (var i = 0; i < req.ListSunday.Count; i++)
                            {
                                if (req.ListSunday[i].FromTime != null && req.ListSunday[i].ToTime != null)
                                {
                                    res = EmployeeRepository.UpdateTimeActive(req.EmployeeID, 8, req.ListSunday[i].ID, req.ListSunday[i].FromTime, req.ListSunday[i].ToTime, "Y");
                                }
                            }
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
                    res.Code = "01";
                    res.Message = "Dữ liệu không tồn tại.";
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.TimeActiveEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("TimeActiveGetByEmployeeID")]
        public ZResponse TimeActiveGetByEmployeeID(ZEmployeeGetTimeActiveRequest req)
        {
            int result = -1;
            List<DayTimeActive> lstTemp = new List<DayTimeActive>();
            List<ZEmployeeGetTimeActiveResponse> lst = new List<ZEmployeeGetTimeActiveResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TIME_ACTIVE_PKG.GET_BY_EMPLOYEE_ID", new object[]
                {
                    req.EmployeeID,
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
                                    lstTemp.Add(new DayTimeActive()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Day = Convert.ToInt32(dr["DAY"]),
                                        FromTime = Convert.ToString(dr["FROM_TIME"]),
                                        ToTime = Convert.ToString(dr["TO_TIME"]),
                                        IsChoose = Convert.ToString(dr["IS_CHOOSE"])
                                    });
                                }
                            }
                            if (lstTemp != null)
                            {
                                lst.Add(new ZEmployeeGetTimeActiveResponse()
                                {
                                    ListMonday = lstTemp.Where(n => n.Day == 2).ToList(),
                                    ListTuesday = lstTemp.Where(n => n.Day == 3).ToList(),
                                    ListWednesday = lstTemp.Where(n => n.Day == 4).ToList(),
                                    ListThursday = lstTemp.Where(n => n.Day == 5).ToList(),
                                    ListFriday = lstTemp.Where(n => n.Day == 6).ToList(),
                                    ListSaturday = lstTemp.Where(n => n.Day == 7).ToList(),
                                    ListSunday = lstTemp.Where(n => n.Day == 8).ToList(),
                                });
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
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
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.List " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("CheckActive")]
        public ZResponse CheckActive(ZEmployeeCheckTimeActiveRequest req)
        {
            int result = -1;
            List<DayTimeActive> lst = new List<DayTimeActive>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TIME_ACTIVE_PKG.CHECK_ACTIVE", new object[]
                {
                    req.EmployeeID,
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
                                    lst.Add(new DayTimeActive()
                                    {
                                        Day = Convert.ToInt32(dr["DAY"]),
                                        FromTime = Convert.ToString(dr["FROM_TIME"]),
                                        ToTime = Convert.ToString(dr["TO_TIME"])
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Tài khoản đã bị khoá. Vui lòng liên hệ quản trị viên.";
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
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.CheckActive " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListEmployeeInPermission")]
        public ZResponse ListEmployeeInPermission(ZEmployeeListEmployeeInPermissionRequest req)
        {
            int result = -1;
            List<ZEmployeeListEmployeeInPermissionResponse> lstResult = new List<ZEmployeeListEmployeeInPermissionResponse>();
            List<ZEmployeeListEmployeeInPermissionResponse> lstOne = new List<ZEmployeeListEmployeeInPermissionResponse>();
            List<ZEmployeeListEmployeeInPermissionResponse> lstTwo = new List<ZEmployeeListEmployeeInPermissionResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.LIST_EMPLOYEE_IN_PERMISSION", new object[]
                {
                    req.ShopLevel,
                    req.ShopID,
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
                                    if (!lstOne.Exists(m => m.EmployeeGroupPermissionID == Convert.ToInt32(dr["EMPLOYEE_GROUP_PERMISSION_ID"])))
                                    {
                                        lstOne.Add(new ZEmployeeListEmployeeInPermissionResponse()
                                        {
                                            EmployeeGroupPermissionID = Convert.ToInt32(dr["EMPLOYEE_GROUP_PERMISSION_ID"]),
                                            EmployeeGroupPermissionName = Convert.ToString(dr["EMPLOYEE_GROUP_PERMISSION_NAME"]),
                                        });
                                    }
                                    lstTwo.Add(new ZEmployeeListEmployeeInPermissionResponse()
                                    {
                                        EmployeeGroupPermissionID = Convert.ToInt32(dr["EMPLOYEE_GROUP_PERMISSION_ID"]),
                                        EmployeeID = Convert.ToInt32(dr["EMPLOYEE_ID"]),
                                        EmployeeName = Convert.ToString(dr["EMPLOYEE_NAME"]),
                                        EmployeeMobileNumber = Convert.ToString(dr["EMPLOYEE_MOBILE_NUMBER"])
                                    });
                                }
                                if(lstOne.Count > 0)
                                {
                                    foreach (var item in lstOne)
                                    {
                                        var ListFilter = lstTwo.Where(m => m.EmployeeGroupPermissionID == item.EmployeeGroupPermissionID).ToList();
                                        lstResult.Add(new ZEmployeeListEmployeeInPermissionResponse()
                                        {
                                            EmployeeGroupPermissionID = item.EmployeeGroupPermissionID,
                                            EmployeeGroupPermissionName = item.EmployeeGroupPermissionName,
                                            ListEmployee = ListFilter
                                        });
                                    }
                                }
                            }
                            if (lstResult.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lstResult);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
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
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ListEmployeeInPermission " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ListEmployeeByEmployeeGroupID")]
        public ZResponse ListEmployeeByEmployeeGroupID(ZEmployeeListEmployeeByEmployeeGroupIDRequest req)
        {
            int result = -1;
            List<ZEmployeeListEmployeeByEmployeeGroupIDResponse> lst = new List<ZEmployeeListEmployeeByEmployeeGroupIDResponse>();
            ZResponse res = new ZResponse();
            List<int> checkEmployeeID = new List<int>();
            var splitEmployeeGroupID = req.strEmployeeGroupID.Split(',');
            for(var i = 0; i < splitEmployeeGroupID.Length; i++)
            {
                try
                {
                    using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.LIST_BY_EMPLOYEE_GROUP_ID", new object[]
                    {
                        splitEmployeeGroupID[i],
                        req.ShopID,
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
                                        if(!checkEmployeeID.Exists(m => m == Convert.ToInt32(dr["EMPLOYEE_ID"])))
                                        {
                                            lst.Add(new ZEmployeeListEmployeeByEmployeeGroupIDResponse()
                                            {
                                                EmployeeID = Convert.ToInt32(dr["EMPLOYEE_ID"]),
                                                EmployeeName = Convert.ToString(dr["EMPLOYEE_NAME"])
                                            });
                                            checkEmployeeID.Add(Convert.ToInt32(dr["EMPLOYEE_ID"]));
                                        }
                                    }
                                }
                                if (lst.Count > 0)
                                {
                                    res.Code = "00";
                                    res.Message = "Thành công.";
                                    res.Data = JsonConvert.SerializeObject(lst);
                                }
                                else
                                {
                                    res.Code = "01";
                                    res.Message = "Không tồn tại dữ liệu.";
                                    res.Data = null;
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
                            res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ListEmployeeByEmployeeGroupID " + ex.Message);
                    res.Code = "99";
                    res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                }
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeReadyReceiveRequestOrder")]
        public ZResponse ChangeReadyReceiveRequestOrder(ZEmployeeChangeReadyReceiveRequestOrderRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.CHANGE_IS_READY", new object[]
                {
                    0,
                    req.EmployeeID,
                    req.IsReady,
                    req.ShopID
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ChangeReadyReceiveRequestOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("CheckCondition")]
        //public ZResponse CheckCondition(ZEmployeeCheckConditionRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    ZResponse resPermission = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_PKG.CHECK_CONDITION", new object[]
        //        {
        //            0,
        //            req.EmployeeID,
        //            req.IsShop
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công";
        //                    //if (req.StoreOwner == "N")
        //                    //{
        //                    //    resPermission = PermissionRepository.ListMenu(req.EmployeeID, req.ShopID);
        //                    //    if (resPermission.Code == "00")
        //                    //    {
        //                    //        res.Code = "00";
        //                    //        res.Message = "Thành công";
        //                    //        res.Data = resPermission.Data;
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        res.Code = "07";
        //                    //        res.Message = "Tải danh sách menu thất bại. Vui lòng kiểm tra lại nhóm quyền.";
        //                    //    }
        //                    //}
        //                    //else
        //                    //{
        //                    //    res.Code = "00";
        //                    //    res.Message = "Thành công";
        //                    //}
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Cửa hàng đã bị khoá. Vui lòng liên hệ quản trị viên.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Tài khoản không tồn tại. Vui lòng liên hệ quản trị viên.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Tài khoản đã bị khoá. Vui lòng liên hệ quản trị viên.";
        //                            break;
        //                        case -4:
        //                            res.Code = "04";
        //                            res.Message = "Chi nhánh đã bị khoá. Vui lòng liên hệ quản trị viên.";
        //                            break;
        //                        case -5:
        //                            res.Code = "05";
        //                            res.Message = "Hết thời gian hoạt động. Vui lòng liên hệ quản trị viên.";
        //                            break;
        //                        case -6:
        //                            res.Code = "06";
        //                            res.Message = "Cửa hàng đã hết thời hạn sử dụng. Vui lòng liên hệ quản trị viên.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại.";
        //                            break;
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.CheckCondition " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("CheckCondition")]
        public ZResponse CheckCondition(ZEmployeeCheckConditionRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = EmployeeRepository.CheckCondition(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.CheckCondition " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
