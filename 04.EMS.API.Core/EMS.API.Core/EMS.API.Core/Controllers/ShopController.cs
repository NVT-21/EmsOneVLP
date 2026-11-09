using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using EMS.API.Core.Repositories;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Shop")]
    public class ShopController : ApiController
    {
        [Route("Add")]
        public ZResponse Add(ZShopAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resAddShop = ShopRepository.AddShop(req);
                if (resAddShop.Code == "00")
                {
                    res = ShopRepository.SendEmailRegisterSuccess(new ZShopSearchInfoSendEmailRequest()
                    {
                        ShopID = Convert.ToInt32(resAddShop.Data),
                        FullName = req.FullName,
                        ToEmail = req.Email,
                        Password = req.Password,
                        MobileNumber = req.MobileNumber
                    });
                }
                else
                {
                    res.Code = resAddShop.Code;
                    res.Message = resAddShop.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
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
                ZShopGetByIDResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GET_BY_ID", new object[]
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
                                if (dr.Read())
                                {
                                    item = new ZShopGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = Convert.ToString(dr["CODE"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        StoreName = Convert.ToString(dr["STORE_NAME"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        ProductBusiness = Convert.ToString(dr["PRODUCT_BUSINESS"]),
                                        TypeBusiness = Convert.ToString(dr["TYPE_BUSINESS"]),
                                        TypeShop = Convert.ToInt32(dr["TYPE_SHOP"]),
                                        Status = Convert.ToString(dr["STATUS"]),
                                        AccountType = Convert.ToInt32(!string.IsNullOrEmpty(dr["ACCOUNT_TYPE"].ToString()) ? dr["ACCOUNT_TYPE"].ToString() : "0"),
                                        NumberOfEmployees = Convert.ToInt32(!string.IsNullOrEmpty(dr["NUMBER_OF_EMPLOYEES"].ToString()) ? dr["NUMBER_OF_EMPLOYEES"].ToString() : "0"),
                                        StartDate = Convert.ToString(dr["START_DATE"]),
                                        EndDate = Convert.ToString(dr["END_DATE"]),
                                        NumberOfEmployeesCreated = Convert.ToInt32(dr["NUMBER_OF_EMPLOYEE_CREATED"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        ShopLevel = Convert.ToInt32(dr["SHOP_LEVEL"]),
                                        PickUpID = Convert.ToInt32(dr["PICK_UP_ID"]),
                                        PickUpFullName = Convert.ToString(dr["PICKUP_FULL_NAME"]),
                                        PickUpMobileNumber = Convert.ToString(dr["PICKUP_MOBILE_NUMBER"]),
                                        PickUpProvinceID = string.IsNullOrEmpty(dr["PICKUP_PROVINCE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["PICKUP_PROVINCE_ID"]),
                                        PickUpDistrictID = string.IsNullOrEmpty(dr["PICKUP_DISTRICT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["PICKUP_DISTRICT_ID"]),
                                        PickUpWardID = string.IsNullOrEmpty(dr["PICKUP_WARD_ID"].ToString()) ? 0 : Convert.ToInt32(dr["PICKUP_WARD_ID"]),
                                        PickUpStreet = Convert.ToString(dr["PICKUP_STREET"]),
                                        PickUpFullAddress = Convert.ToString(dr["PICK_UP_FULL_ADDRESS"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        ContractNumber = Convert.ToString(dr["CONTRACT_NUMBER"]),
                                        CustomerCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                        CustomerToken = Convert.ToString(dr["CUSTOMER_TOKEN"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        Street = Convert.ToString(dr["STREET"]),
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        MainTransportServiceID = Convert.ToString(dr["MAIN_TRANSPORT_SERVICE_ID"]),
                                        ExtraTransportServiceID = Convert.ToString(dr["EXTRA_TRANSPORT_SERVICE_ID"]),
                                        IsExistExtraTransportServiceID = Convert.ToInt32(dr["IS_EXIST_EXTRA_TRANS_SERVICE"]),
                                        PaymentMethod = Convert.ToInt32(dr["PAYMENT_METHOD"]),
                                        PaymentType = Convert.ToInt32(dr["PAYMENT_TYPE"]),
                                        FullNameOfPayment = Convert.ToString(dr["FULL_NAME_OF_PAYMENT"]),
                                        MobileNumberOfPayment = Convert.ToString(dr["MOBILE_NUMBER_OF_PAYMENT"]),
                                        NameOfPayment = Convert.ToString(dr["NAME_OF_PAYMENT"]),
                                        BranchOfPayment = Convert.ToString(dr["BRANCH_OF_PAYMENT"]),
                                        NumberOfPayment = Convert.ToString(dr["NUMBER_OF_PAYMENT"]),
                                        AutoDivision = Convert.ToString(dr["AUTO_DIVISION"]),
                                        EmployeeReadyDivision = Convert.ToString(dr["EMPLOYEE_READY_DIVISION"]),
                                        EmployeeGroupPermissionID = Convert.ToString(dr["EMPLOYEE_GROUP_PERMISSION_ID"]),
                                        DateProcessRequestOrder = Convert.ToInt32(dr["DATE_PROCESS_REQUEST_ORDER"]),
                                        AutoCriteria = Convert.ToInt32(dr["AUTO_CRITERIA"]),
                                        AutoTimeDivision = Convert.ToString(dr["TIME_AUTO_DIVISION"]),
                                        CODPeriodType = Convert.ToInt32(dr["COD_PERIOD_TYPE"]),
                                        FeePeriodType = Convert.ToInt32(dr["FEE_PERIOD_TYPE"]),
                                        CollectionType = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                        CollectionTypeName = XData.GetNameCollectionType(Convert.ToString(dr["COLLECTION_TYPE"])),
                                        CollectionPostcode = Convert.ToString(dr["COLLECTION_POSTCODE"]),
                                        CollectionPostOfficeID = string.IsNullOrEmpty(dr["COLLECTION_POST_OFFICE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["COLLECTION_POST_OFFICE_ID"]),
                                        ChildCustomerCode = Convert.ToString(dr["CHILD_CUSTOMER_CODE"]),
                                        EmailBCG = Convert.ToString(dr["EMAIL_BCG"]),
                                        EmailCSKH = Convert.ToString(dr["EMAIL_CSKH"]),
                                        TaxCode = Convert.ToString(dr["TAX_CODE"]),
                                        PartnerID = Convert.ToInt32(dr["PARTNER_ID"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GetByID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByToken")]
        public ZResponse GetByToken(string Token)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZShopGetByIDResponse item = null;
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GET_BY_TOKEN", new object[]
                {
                    Token,
                    0,
                    0
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
                                    item = new ZShopGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = Convert.ToString(dr["CODE"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        StoreName = Convert.ToString(dr["STORE_NAME"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        ProductBusiness = Convert.ToString(dr["PRODUCT_BUSINESS"]),
                                        TypeBusiness = Convert.ToString(dr["TYPE_BUSINESS"]),
                                        TypeShop = Convert.ToInt32(dr["TYPE_SHOP"]),
                                        Status = Convert.ToString(dr["STATUS"]),
                                        AccountType = Convert.ToInt32(!string.IsNullOrEmpty(dr["ACCOUNT_TYPE"].ToString()) ? dr["ACCOUNT_TYPE"].ToString() : "0"),
                                        NumberOfEmployees = Convert.ToInt32(!string.IsNullOrEmpty(dr["NUMBER_OF_EMPLOYEES"].ToString()) ? dr["NUMBER_OF_EMPLOYEES"].ToString() : "0"),
                                        StartDate = Convert.ToString(dr["START_DATE"]),
                                        EndDate = Convert.ToString(dr["END_DATE"]),
                                        CustomerCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                        CustomerToken = Convert.ToString(dr["CUSTOMER_TOKEN"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        Street = Convert.ToString(dr["STREET"]),
                                        Address = Convert.ToString(dr["ADDRESS"])                                       
                                    };
                                }
                            }
                            if(item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GetByID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByAccessToken")]
        public ZResponse GetByAccessToken(string Token)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZShopGetByIDResponse item = null;
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GET_BY_ACCESS_TOKEN", new object[]
                {
                    Token,
                    0,
                    0
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
                                    item = new ZShopGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = Convert.ToString(dr["CODE"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        StoreName = Convert.ToString(dr["STORE_NAME"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        ProductBusiness = Convert.ToString(dr["PRODUCT_BUSINESS"]),
                                        TypeBusiness = Convert.ToString(dr["TYPE_BUSINESS"]),
                                        TypeShop = Convert.ToInt32(dr["TYPE_SHOP"]),
                                        Status = Convert.ToString(dr["STATUS"]),
                                        AccountType = Convert.ToInt32(!string.IsNullOrEmpty(dr["ACCOUNT_TYPE"].ToString()) ? dr["ACCOUNT_TYPE"].ToString() : "0"),
                                        NumberOfEmployees = Convert.ToInt32(!string.IsNullOrEmpty(dr["NUMBER_OF_EMPLOYEES"].ToString()) ? dr["NUMBER_OF_EMPLOYEES"].ToString() : "0"),
                                        StartDate = Convert.ToString(dr["START_DATE"]),
                                        EndDate = Convert.ToString(dr["END_DATE"]),
                                        CustomerCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                        CustomerToken = Convert.ToString(dr["CUSTOMER_TOKEN"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        Street = Convert.ToString(dr["STREET"]),
                                        Address = Convert.ToString(dr["ADDRESS"])
                                    };
                                }
                            }
                            if(item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
                            }
                        }
                        else if (result == -2)
                        {
                            res.Code = "98";
                            res.Message = "Token không tồn tại hoặc đã hết hạn";
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GetByID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpGet]
        [Route("GetListCrmOrPaypostCode")]
        public ZResponse GetListCrmOrPaypostCode(int ShopID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZShopGetListCrmOrPaypostCodeResponse> ListItem = new List<ZShopGetListCrmOrPaypostCodeResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GET_LIST_CRM_OR_PP_CODE", new object[]
                {
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
                                    ListItem.Add(new ZShopGetListCrmOrPaypostCodeResponse()
                                    {
                                        StoreName = Convert.ToString(dr["STORE_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        CrmOrPaypostCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                        MobileAndCrmOrPaypostCode = Convert.ToString(dr["MOBILE_NUMBER"]) + " / " + Convert.ToString(dr["CUSTOMER_CODE"]) + " / " + Convert.ToString(dr["STORE_NAME"])
                                    });
                                }
                            }
                            if (ListItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(ListItem);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GetListCrmOrPaypostCode " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("GetHistoryByID")]
        public ZResponse GetHistoryByID(ZShopGetHistoryByIDRequest req)
        {
            int result = -1;
            List<ZShopGetHistoryByIDResponse> Model = new List<ZShopGetHistoryByIDResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GET_HISTORY_BY_ID", new object[]
                {
                    req.ID,
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
                                    Model.Add(new ZShopGetHistoryByIDResponse()
                                    {
                                        ActionCode = Convert.ToString(dr["ACTION_CODE"]),
                                        ActionName = Convert.ToString(dr["ACTION_NAME"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        CreatedName = Convert.ToString(dr["CREATED_NAME"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"])
                                    });
                                }
                            }
                            if (Model.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(Model);
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
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GetHistoryByID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchList")]
        public ZResponse SearchList(ZShopSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZShopSearchListResponse> lst = new List<ZShopSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.SEARCH_SHOP", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.Status,
                    req.MobileNumber,
                    req.CustomerCode,
                    req.CrmOrPaypostCode,
                    req.IsLock,
                    req.DateType,
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
                                    lst.Add(new ZShopSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        StoreName = Convert.ToString(dr["STORE_NAME"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        ProductBusiness = Convert.ToString(dr["PRODUCT_BUSINESS"]),
                                        TypeBusiness = Convert.ToString(dr["TYPE_BUSINESS"]),
                                        Code = Convert.ToString(dr["CODE"]),
                                        CustomerCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                        Status = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["STATUS"]),
                                            Name = XData.GetNameAccountStatus(Convert.ToString(dr["STATUS"]))
                                        },
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        AccountType = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["ACCOUNT_TYPE"]),
                                            Name = XData.GetNameAccountType(Convert.ToString(dr["ACCOUNT_TYPE"]))
                                        },
                                        NumberOfEmployees = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["NUMBER_OF_EMPLOYEES"]),
                                            Name = XData.GetNameNumberOfEmployeeByValue(Convert.ToString(dr["NUMBER_OF_EMPLOYEES"]))
                                        },
                                        ReasonReject = Convert.ToString(dr["REASON_REJECT"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        CollectionPostOfficeCode = Convert.ToString(dr["COLLECTION_POSTCODE"]),
                                        TypeShop = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["TYPE_SHOP"]),
                                            Name = XData.GetNameTypeCustomer(dr["TYPE_SHOP"].ToString()),
                                        },
                                        ChildCustomerCode = Convert.ToString(dr["CHILD_CUSTOMER_CODE"]),
                                        EmailBCG = Convert.ToString(dr["EMAIL_BCG"]),
                                        EmailCSKH = Convert.ToString(dr["EMAIL_CSKH"]),
                                        CreatedName = Convert.ToString(dr["CREATED_NAME"]),
                                        CreatedPhone = Convert.ToString(dr["CREATED_PHONE"]),
                                        CreatedEmail = Convert.ToString(dr["CREATED_EMAIL"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        ConfirmedName = Convert.ToString(dr["CONFIRMED_NAME"]),
                                        ConfirmedPhone = Convert.ToString(dr["CONFIRMED_PHONE"]),
                                        ConfirmedEmail = Convert.ToString(dr["CONFIRMED_EMAIL"]),
                                        ConfirmedDate = Convert.ToString(dr["CONFIRMED_DATE"]),
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
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -99:
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.SearchList " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Register")]
        public ZResponse Register(ZShopRegisterRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.REGISTER", new object[]
                {
                    0,
                    req.FullName,               //P_FULL_NAME
                    req.StoreName,              //P_STORE_NAME
                    req.Email,                  //P_EMAIL
                    req.MobileNumber,           //P_MOBILE_NUMBER
                    req.ProductBusiness,        //P_PRODUCT_BUSINESS
                    req.TypeBusiness,           //P_TYPE_BUSINESS
                    req.TypeShop,               //P_TYPE_SHOP
                    req.ProvinceID,             //P_PROVINCE_ID
                    req.DistrictID,             //P_DISTRICT_ID
                    req.WardID,                 //P_WARD_ID
                    req.Address,                //P_ADDRESS
                    req.CRMOrPaypostCode,       //P_CRM_OR_PAYPOST_CODE
                    req.CollectionPostcode,
                    req.CollectionPostOfficeID  //P_COLLECTION_POST_OFFICE_ID
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
                                    res.Message = "Email đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Số điện thoại đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Bưu cục thu gom không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.Register: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("EditShop")]
        public ZResponse EditShop(ZShopEditShopRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.EDIT_SHOP", new object[]
                {
                    0,
                    req.ID,
                    req.ContractNumber,
                    req.CustomerCode,
                    req.CustomerToken,
                    req.StoreName,
                    req.FullName,
                    req.Email,
                    req.ProductBusiness,
                    req.TypeBusiness,
                    req.TypeShop,
                    req.AccountType,
                    req.NumberOfEmployees,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Street,
                    req.MainTransportServiceID,
                    req.ExtraTransportServiceID,
                    req.CollectionType,
                    req.CollectionPostOfficeID,
                    req.ChildCustomerCode,
                    req.CODPeriodType,
                    req.FeePeriodType,
                    req.PaymentMethod,
                    req.FullNameOfPayment,
                    req.NameOfPayment,
                    req.BranchOfPayment,
                    req.NumberOfPayment,
                    req.EmailBCG,
                    req.EmailCSKH,
                    req.TaxCode,
                    req.PartnerID,
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
                            res.Message = "Chỉnh sửa thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tìm thấy thông tin chỉnh sửa.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái thông tin không thể chỉnh sửa.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Số hợp đồng đã tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Mã CMR/ mã Paypost đã tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Email đã tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Bưu cục thu gom không tồn tại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Token đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ApprovedShop")]
        public ZResponse ApprovedShop(ZShopApprovedShopRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resApprovedShop = ShopRepository.ApprovedShop(req);
                if (resApprovedShop.Code == "00")
                {
                    res = ShopRepository.SendEmailRegisterSuccess(new ZShopSearchInfoSendEmailRequest()
                    {
                        ShopID = req.ID,
                        FullName = req.FullName,
                        ToEmail = req.Email,
                        Password = req.Password,
                        MobileNumber = req.MobileNumber
                    });
                }
                else
                {
                    res.Code = resApprovedShop.Code;
                    res.Message = resApprovedShop.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ApprovedShop: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("RejectShop")]
        public ZResponse RejectShop(ZShopRejectShopRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.REJECT_SHOP", new object[]
                {
                    0,
                    req.ID,
                    req.ConfirmedBy,
                    req.ReasonReject
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Từ chối thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Thông tin đăng ký không tồn tại";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Thất bại. Thông tin đăng ký đã thay đổi trạng thái";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.RejectShop: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[HttpGet]
        //[Route("UpdateConfirmationEmail")]
        //public ZResponse UpdateConfirmationEmail(ZShopUpdateConfirmationEmailRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = ShopRepository.UpdateStatusSendMail(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.UpdateConfirmationEmail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[Route("AddImage")]
        //public ZResponse AddImage(ZShopAddImageRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.ADD_IMAGE", new object[]
        //        {
        //            0,
        //            //req.ImageCode,
        //            //req.FileName,
        //            req.ShopID,
        //            req.CreatedBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Không tìm thấy thông tin cửa hàng.";
        //                            break;
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.AddImage: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //[Route("DataImageByID")]
        //public ZResponse DataImageByID(ZShopImageDataByImageIDRequest req)
        //{
        //    int result = -1;
        //    ZShopImageDataByImageIDResponse item = new ZShopImageDataByImageIDResponse();
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.IMAGE_DATA_BY_IMAGE_ID", new object[]
        //        {
        //            req.ImageID,
        //            req.ShopID,
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
        //                            item = new ZShopImageDataByImageIDResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                Code = Convert.ToString(dr["CODE"]),
        //                                Name = Convert.ToString(dr["NAME"]),
        //                            };
        //                        }
        //                    }
        //                    if (item != null)
        //                    {

        //                        res.Code = "00";
        //                        res.Message = "Thành công.";
        //                        res.Data = JsonConvert.SerializeObject(item);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
        //                        res.Data = null;
        //                    }
        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Thất bại.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GetByID " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //[Route("LockShop")]
        //public ZResponse LockShop(ZShopLockShopRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.LOCK_SHOP", new object[]
        //        {
        //            0,
        //            req.ID,
        //            req.UpdatedBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Khoá thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Không tìm thấy thông tin cập nhật";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Thất bại, tài khoản đang bị khoá";
        //                            break;
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.LockShop: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //[Route("UnlockShop")]
        //public ZResponse UnlockShop(ZShopLockShopRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.UNLOCK_SHOP", new object[]
        //        {
        //            0,
        //            req.ID,
        //            req.UpdatedBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Mở khoá thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Không tìm thấy thông tin cập nhật";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Thất bại, tài khoản đang mở khoá";
        //                            break;
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.UnlockShop: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("ChangeIsLock")]
        public ZResponse ChangeIsLock(ZShopChangeIsLockRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.CHANGE_IS_LOCK", new object[]
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
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Bạn không thể thay đổi trạng thái do cơ sở cấp trên đang bị khóa.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ResetPasswordShop")]
        public ZResponse ResetPasswordShop(ZShopResetPasswordShopRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                string oPassword = string.Empty;
                oPassword = Security.SHA256(req.MobileNumber + Security.DEFAULT_PASSWORD + Security.PASSWORD_SALT);
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.RESET_PASSWORD_SHOP", new object[]
                {
                    0,
                    req.ID,
                    req.Email,
                    oPassword,
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
                            res.Message = "Khôi phục mật khẩu thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Không tìm thấy thông tin để khôi phục mật khẩu.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Thông tin không trùng khớp.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ResetPasswordShop: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SalesChannelList")]
        public ZResponse SalesChannelList(ZShopSalesChannelListRequest req)
        {
            int result = -1;
            List<ZShopSalesChannelListResponse> lst = new List<ZShopSalesChannelListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.SALES_CHANNEL_SEARCH", new object[]
                {
                    req.ID,
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
                                    lst.Add(new ZShopSalesChannelListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Channel = Convert.ToString(dr["CHANNEL"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        EmployeeName = Convert.ToString(dr["EMPLOYEE_NAME"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.SalesChannelList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SalesChannelAdd")]
        public ZResponse SalesChannelAdd(ZShopSalesChannelAddRequest req)
        {
            int result = -1;
            string cPassword = Security.SHA256(req.UserName + req.Password + Security.PASSWORD_SALT);
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.SALES_CHANNEL_ADD", new object[]
                {
                    0,
                    req.ShopID,
                    req.Channel,
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
                                    res.Message = "Kênh liên kết đã tồn tại, Vui lòng kiểm tra lại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.SalesChannelAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SalesChannelRemove")]
        public ZResponse SalesChannelRemove(ZEmployeeRemoveRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.SALES_CHANNEL_REMOVE", new object[]
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
                                    res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.SalesChannelRemove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangePickUpAddress")]
        public ZResponse ChangePickUpAddress(ZShopChangePickUpAddressRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.CHANGE_PICK_UP_ADDRESS", new object[]
                {
                    0,
                    req.PickUpAddressID,
                    req.PickUpName,
                    req.CollectionPostOfficeID,
                    req.FullName,
                    req.MobileNumber,
                    req.ProvinceID,
                    //req.DistrictID,
                    req.WardID,
                    req.Street,
                    req.IsDefault,
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
                            res.Message = "Đổi thông tin lấy hàng thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Địa chỉ lấy hàng không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên địa chỉ lấy hàng đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Không có mẫu in với địa chỉ lấy hàng này.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Luôn phải có địa chỉ lấy hàng mặc định.";
                                    break;
                                case -5:
                                    res.Code = "04";
                                    res.Message = "Cửa hàng không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ChangePickUpAddress: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GroupPermissionList")]
        public ZResponse GroupPermissionList(ZShopGroupPermissionListRequest req)
        {
            int result = -1;
            List<ZShopGroupPermissionListResponse> lst = new List<ZShopGroupPermissionListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GROUP_PERMISSION_LIST", new object[]
                {
                    req.ShopLevel,
                    req.IsLock,
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
                                    lst.Add(new ZShopGroupPermissionListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        GroupPermissionName = Convert.ToString(dr["GROUP_PERMISSION_NAME"]),
                                        GroupPermissionCode = Convert.ToString(dr["GROUP_PERMISSION_CODE"]),
                                        ShopLevel = Convert.ToInt32(dr["SHOP_LEVEL"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        CreatedBy = Convert.ToString(dr["CREATED_BY"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        NumberOfEmployeesInGroup = Convert.ToInt32(dr["NUMBER_OF_EMPLOYEES_IN_GROUP"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GroupPermissionList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GroupPermissionChangeIsLock")]
        public ZResponse GroupPermissionChangeIsLock(ZShopGroupPermissionChangeIsLockRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GRO_PER_CHANGE_IS_LOCK", new object[]
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
                                    res.Message = "Không tồn tại dữ liệu.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GroupPermissionChangeIsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GroupPermissionAdd")]
        public ZResponse GroupPermissionAdd(ZShopGroupPermissionAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GROUP_PERMISSION_ADD", new object[]
                {
                    0,
                    req.GroupPermissionCode,
                    req.GroupPermissionName,
                    req.ShopLevel,
                    req.IsLock,
                    req.Description,
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
                                    res.Message = "Mã nhóm quyền đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên nhóm quyền đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GroupPermissionAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GroupPermissionGetByID")]
        public ZResponse GroupPermissionGetByID(ZShopGroupPermissionGetByIDRequest req)
        {
            int result = -1;
            ZShopGroupPermissionGetByIDResponse item = null;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GRO_PER_GET_BY_ID", new object[]
                {
                    req.ID,
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
                                    item = new ZShopGroupPermissionGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        GroupPermissionCode = Convert.ToString(dr["GROUP_PERMISSION_CODE"]),
                                        GroupPermissionName = Convert.ToString(dr["GROUP_PERMISSION_NAME"]),
                                        ShopLevel = Convert.ToInt32(dr["SHOP_LEVEL"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GroupPermissionGetByID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GroupPermissionEdit")]
        public ZResponse GroupPermissionEdit(ZShopGroupPermissionEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GROUP_PERMISSION_EDIT", new object[]
                {
                    0,
                    req.ID,
                    req.GroupPermissionName,
                    req.ShopLevel,
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
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên nhóm quyền đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GroupPermissionEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("LevelGetByParentID")]
        public ZResponse LevelGetByParentID(ZShopLevelGetByParentIDRequest req)
        {
            int result = -1;
            List<ZShopLevelGetByParentIDResponse> lst = new List<ZShopLevelGetByParentIDResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GET_BY_PARENT_ID", new object[]
                {
                    req.ParentID,
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
                                    lst.Add(new ZShopLevelGetByParentIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = Convert.ToString(dr["CODE"]),
                                        Name = Convert.ToString(dr["STORE_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        ShopLevel = Convert.ToInt32(dr["SHOP_LEVEL"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"])
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
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.LevelGetByParentID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("LevelEdit")]
        public ZResponse LevelEdit(ZShopLevelEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.SHOP_LEVEL_EDIT", new object[]
                {
                    0,
                    req.ID,
                    req.Name,
                    req.MobileNumber,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.Description,
                    req.IsLock,
                    req.NameAddress,
                    req.PickupAddress,
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
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên cơ sở đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Cơ sở trực thuộc đã bị khoá, vui lòng liên hệ quản trị.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Không được phép sửa số điện thoại trung tâm.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Địa chỉ lấy hàng không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.LevelEdit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("LevelAdd")]
        public ZResponse LevelAdd(ZShopLevelAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.SHOP_LEVEL_ADD", new object[]
                {
                    0,
                    req.ID,
                    req.Name,
                    req.MobileNumber,
                    req.Description,
                    req.IsLock,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.NameAddress,
                    req.PickupAddress,
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
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên cơ sở đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.LevelAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdatePaymentInformation")]
        public ZResponse UpdatePaymentInformation(ZShopUpdatePaymentInformationRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.UPDATE_PAYMENT_INFORMATION", new object[]
                {
                    0,
                    req.ID,
                    req.PaymentMethod,
                    req.PaymentType,
                    req.FullNameOfPayment,
                    req.MobileNumberOfPayment,
                    req.NameOfPayment,
                    req.BranchOfPayment,
                    req.NumberOfPayment,
                    req.CollectionType,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.UpdatePaymentInformation: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateAutoRequestOrder")]
        public ZResponse UpdateAutoRequestOrder(ZShopUpdateAutoRequestOrderRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.UPDATE_AUTO_REQUEST_ORDER", new object[]
                {
                    0,
                    req.ID,
                    req.AutoDivision,
                    req.EmployeeReadyDivision,
                    req.EmployeeGroupPermissionID,
                    req.DateProcessRequestOrder,
                    req.Criteria,
                    req.TimeDivision,
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
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lỗi cập nhật thời gian.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.UpdateAutoRequestOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetPickupInformation")]
        public ZResponse GetPickupInformation(int ID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZShopGetPickupInformationResponse item = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GET_PICKUP_INFORMATION", new object[]
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
                                if (dr.Read())
                                {
                                    item = new ZShopGetPickupInformationResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PickupAddressID = Convert.ToInt32(dr["PICKUP_ADDRESS_ID"]),
                                        PickupName = Convert.ToString(dr["PICKUP_NAME"]),
                                        PickupFullName = Convert.ToString(dr["PICKUP_FULL_NAME"]),
                                        PickupMobileNumber = Convert.ToString(dr["PICKUP_MOBILE_NUMBER"]),
                                        PickupProvinceID = Convert.ToInt32(dr["PICKUP_PROVINCE_ID"]),
                                        PickupProvinceName = dr["PICKUP_PROVINCE_NAME"].ToString(),
                                        PickupDistrictID = Convert.ToInt32(dr["PICKUP_DISTRICT_ID"]),
                                        PickupDistrictName = dr["PICKUP_DISTRICT_NAME"].ToString(),
                                        PickupWardID = Convert.ToInt32(dr["PICKUP_WARD_ID"]),
                                        PickupWardName = dr["PICKUP_WARD_NAME"].ToString(),
                                        PickupStreet = Convert.ToString(dr["PICKUP_STREET"]),
                                        PickupFullAddress = Convert.ToString(dr["PICKUP_FULL_ADDRESS"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GetPickupInformation " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChangeLabelPrint")]
        public ZResponse ChangeLabelPrint(ZShopChangeLabelPrintRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.CHANGE_LABEL_PRINT", new object[]
                {
                    0,
                    req.LabelPrintID,
                    req.PrintType,
                    req.PrintTypeDefault,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ChangeLabelPrint: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //#region Transport Service
        //[Route("TransportServiceByShopID")]
        //public ZResponse TransportServiceByShopID(ZShopTransportServiceByShopIDRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1; 
        //        List<ZShopTransportServiceByShopIDResponse> model = new List<ZShopTransportServiceByShopIDResponse>();
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_TRANSPORT_SERVICE_PKG.GET_BY_SHOP_ID", new object[]
        //        {
        //            req.ShopID,
        //            req.ParentID,
        //            req.IsLock,
        //            0,                   //P_RETURN_CODE OUT NUMBER
        //            0                    //P_OUT_CURSOR  OUT T_CURSOR
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
        //                            model.Add(new ZShopTransportServiceByShopIDResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                Code = dr["CODE"].ToString(),
        //                                Name = dr["NAME"].ToString(),
        //                                ParentID = Convert.ToInt32(dr["PARENT_ID"]),
        //                                SpecialService = dr["SPECIAL_SERVICE"].ToString(),
        //                                IsLock = dr["IS_LOCK"].ToString(),
        //                                IsDefault = dr["IS_DEFAULT"].ToString(),
        //                                Sort = Convert.ToInt32(dr["SORT"]),
        //                                CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
        //                                CreatedDate = dr["CREATED_DATE"].ToString()
        //                            });
        //                        }
        //                    }

        //                    if (model.Count > 0)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(model);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
        //                    }
        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Thất bại.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.TransportServiceByShopID: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //#endregion

        [Route("RemoveShop")]
        public ZResponse RemoveShop(ZShopRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.REMOVE_SHOP", new object[]
                {
                    0,
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
                            res.Message = "Đã xóa tài khoản thành công.";
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
                                    res.Message = "Không có quyền xóa cửa hàng.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.RemoveShop: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("LockShop")]
        public ZResponse LockShop(ZShopLockRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.LOCK_SHOP", new object[]
                {
                    0,
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
                            res.Message = "Đã khóa tài khoản thành công.";
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
                                    res.Message = "Không có quyền khóa cửa hàng.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.LockShop: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByCrmOrPaypostCode")]
        public ZResponse GetByCrmOrPaypostCode(string CrmOrPaypostCode)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.GetByCrmOrPaypostCode(CrmOrPaypostCode);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.GetByCrmOrPaypostCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ForgetPasswordByEmail")]
        public ZResponse ForgetPasswordByEmail(ZShopForgetPasswordByEmailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resCheckEmail = ShopRepository.ForgetPasswordByEmail(new ZShopForgetPasswordByEmailRequest()
                {
                    Email = req.Email
                });
                if (resCheckEmail.Code == "00")
                {
                    var itemCheckEmail = JsonConvert.DeserializeObject<ZShopForgetPasswordByEmailResponse>(resCheckEmail.Data);
                    var resSendMail = ShopRepository.SendEmailForgetPassword(new ZShopSearchInfoSendEmailRequest()
                    {
                        StoreName = itemCheckEmail.StoreName,
                        FullName = itemCheckEmail.FullName,
                        ToEmail = itemCheckEmail.Email,
                        OtpCode = itemCheckEmail.OtpCode,
                    });

                    if (resSendMail.Code == "00")
                    {
                        res.Code = resSendMail.Code;
                        res.Message = resSendMail.Message;
                        res.Data = itemCheckEmail.OtpID.ToString();
                    }
                    else
                    {
                        res.Code = resSendMail.Code;
                        res.Message = resSendMail.Message;
                    }
                }
                else
                {
                    res.Code = resCheckEmail.Code;
                    res.Message = resCheckEmail.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ForgetPasswordByEmail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ForgetPasswordUpdate")]
        public ZResponse ForgetPasswordUpdate(ZShopForgetPasswordUpdateRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                var encryptPassword = Security.SHA256(req.MobileNumber + req.Password + Security.PASSWORD_SALT);
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.FORGET_PASSWORD_UPDATE", new object[]
                {
                    0,
                    req.EmployeeID,
                    encryptPassword,
                    req.OtpCode
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
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã OTP không đúng.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.ForgetPasswordUpdate: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("TestEmail")]
        //public  ZResponse SendEmailRegisterSuccess(ZShopSearchInfoSendEmailRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var link = Security.MD5Encrypt(JsonConvert.SerializeObject(new
        //        {
        //            MobileNumber = req.MobileNumber,
        //            Password = req.Password
        //        }), Security.PASSWORD_SALT);

        //        var stringBody = "";
        //        stringBody = string.Format("<div>Chào {0},", req.FullName);
        //        stringBody += "<p>Bạn đã được cấp tài khoản trên hệ thống EMSOne. Thông tin đăng nhập như sau:</p>";
        //        stringBody += string.Format("<p>Số điện thoại đăng nhập: {0}</p>", req.MobileNumber);
        //        stringBody += string.Format("<p>Truy cập vào đây để tạo mật khẩu và sử dụng: <a href={0}Account/UpdatePasswordView?data={1}>tại đây</a> !</p>", XHelper.FrontEndURL, link);
        //        stringBody += "<p>Trân trọng cảm ơn!</p></div>";

        //        var to = new List<ZZohoSendMailToRequest>();
        //        to.Add(new ZZohoSendMailToRequest()
        //        {
        //            email_address = new ZZohoSendMailFromRequest()
        //            {
        //                address = req.ToEmail,
        //                name = req.FullName
        //            }
        //        });

        //        ZZohoSendMailRequest reqSendMail = new ZZohoSendMailRequest()
        //        {
        //            to = to,
        //            subject = "EMSONE - KÍCH HOẠT TÀI KHOẢN",
        //            htmlbody = stringBody
        //        };

        //        var resSendMail = ZohoRepository.SendMail(reqSendMail);

        //        if (resSendMail.Code == "00")
        //        {
        //            ZShopUpdateConfirmationEmailRequest reqUpdateStatusSendMail = new ZShopUpdateConfirmationEmailRequest()
        //            {
        //                ID = req.ShopID,
        //            };

        //            //var resUpdateStatusSendMail = UpdateStatusSendMail(reqUpdateStatusSendMail);

        //            //if (resUpdateStatusSendMail.Code == "00")
        //            //{
        //            //    res.Code = "00";
        //            //    res.Message = "Duyệt và gửi email xác nhận thành công.";
        //            //}
        //            //else
        //            //{
        //            //    res.Code = "02";
        //            //    res.Message = "Duyệt và đã gửi email xác nhận.";
        //            //    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("ShopRepository.UpdateStatusSendMail: Cập nhật trạng thái gửi email thất bại.||request={0}||response={1}", JsonConvert.SerializeObject(reqUpdateStatusSendMail), JsonConvert.SerializeObject(resUpdateStatusSendMail)));
        //            //}
        //        }
        //        else
        //        {
        //            res.Code = "01";
        //            res.Message = "Duyệt thành công nhưng gửi email xác nhận thất bại. Vui lòng liên hệ quản trị.";
        //            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("ShopRepository.SendEmailRegisterSuccess: Gửi email thất bại.||request={0}||response={1}", JsonConvert.SerializeObject(reqSendMail), JsonConvert.SerializeObject(resSendMail)));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.SendEmailRegisterSuccess: " + ex.Message);
        //    }
        //    return res;
        //}

        /// <summary>
        /// Gửi lại email reset mật khẩu
        /// </summary>
        /// <returns></returns>
        [Route("SendEmailRegisterSuccess")]
        public ZResponse SendEmailRegisterSuccess(ZShopSearchInfoSendEmailRequest request)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ShopRepository.SendEmailRegisterSuccess(request);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.SendEmailRegisterSuccess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}