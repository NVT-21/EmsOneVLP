using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using System.Data.Common;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;
using EMS.API.Core.Repositories;
using System.Net.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Customer")]
    public class CustomerController : ApiController
    {
        [Route("AddNew")]
        public ZResponse AddNew(ZCustomerAddNewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.ShopID,
                    req.FullName,
                    req.MobileNumber,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.BirthDay,
                    req.TaxCode,
                    req.Channel,
                    req.Note,
                    req.strStickerID,
                    req.strStickerNew,
                    req.CreatedBy,
                    req.IDAccountChannel,
                    req.PageID,
                    XHelper.DeviceName,
                    0
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
                            res.Data = JsonConvert.SerializeObject(new ZCustomerAddNewResponse()
                            {
                                ID = Convert.ToInt32(dbCommand.Parameters["P_OUT_CUSTOMER_ID"].Value)
                            });
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
                                    res.Message = "Tên nhãn đã tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Gán nhãn thất bại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Tạo mã nhãn thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.AddNew: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("EditCustomer")]
        public ZResponse EditCustomer(ZCustomerEditRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.EDIT", new object[]
                {
                    0,
                    req.ID,
                    req.FullName,
                    req.MobileNumber,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.BirthDay,
                    req.TaxCode,
                    req.Note,
                    req.Channel,
                    req.strStickerID,
                    req.strStickerNew,
                    req.UpdatedBy,
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
                            res.Message = "Chỉnh sửa thành công.";
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
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Tên nhãn đã tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Gán nhãn thất bại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Tạo mã nhãn thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.EditCustomer: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchList")]
        public ZResponse SearchList(ZCustomerSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZCustomerSearchListResponse> listItem = new List<ZCustomerSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.SEARCH_LIST", new object[]
                {
                    req.FromDate,
                    req.ToDate,
                    req.MobileNumber,
                    req.CustomerGroupName,
                    req.Channel,
                    req.CustomerStickerID,
                    req.ProvinceID,
                    req.PageID,
                    req.ShopID,
                    req.SearchKey,
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
                                    listItem.Add(new ZCustomerSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["CUSTOMER_ID"]),
                                        CustomerCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        BirthDay = Convert.ToString(dr["BIRTH_DAY"]),
                                        TaxCode = Convert.ToString(dr["TAX_CODE"]),
                                        Channel = Convert.ToString(dr["CHANNEL"]),
                                        Note = Convert.ToString(dr["NOTE"]),
                                        GroupName = Convert.ToString(dr["GROUP_NAME"]),
                                        BlackList = Convert.ToString(dr["BLACK_LIST"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        IDAccountChannel = Convert.ToString(dr["ID_ACCOUNT_CHANNEL"]),
                                        ProvinceName = Convert.ToString(dr["PROVINCE_NAME"]),
                                        DistrictName = Convert.ToString(dr["DISTRICT_NAME"]),
                                        WardName = Convert.ToString(dr["WARD_NAME"]),
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        Sticker = Convert.ToString(dr["STICKER_NAME"]),
                                        CountOrderSuccess = Convert.ToInt32(dr["COUNT_ORDER_SUCCESS"]),
                                        TotalAmountSuccess = Convert.ToInt64(dr["TOTAL_AMOUNT_SUCCESS"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.SearchList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchListV2")]
        public ZResponse SearchListV2(ZCustomerSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZCustomerSearchListResponse> listItem = new List<ZCustomerSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.SEARCH_LIST_V2", new object[]
                {
                    req.SearchKey,
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
                                    listItem.Add(new ZCustomerSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["CUSTOMER_ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.SearchListV2 " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByCustomerCode")]
        public ZResponse GetByCustomerCode(string CustomerCode) //ZShopGetAllRequest req
        {
            int result = -1;
            ZCustomerGetByCodeResponse model = null;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.GET_BY_CUSTOMER_CODE", new object[]
                {
                    CustomerCode,
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
                                    model = new ZCustomerGetByCodeResponse()
                                    {
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        BirthDay = Convert.ToString(dr["BIRTH_DAY"]),
                                        TaxCode = Convert.ToString(dr["TAX_CODE"]),
                                        Channel = Convert.ToString(dr["CHANNEL"]),
                                        Note = Convert.ToString(dr["NOTE"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.GetByCustomerCode" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddNewGroup")]
        public ZResponse AddNewGroup(ZCustomerAddNewGroupRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.ADD_NEW_GROUP", new object[]
                {
                    0,
                    req.ShopID,
                    req.GroupCode,
                    req.GroupName,
                    req.OrderSuccess,
                    req.PurchaseMoney,
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
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã nhóm đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Đã có nhóm khách hàng cùng điều kiện, vui lòng chọn lại";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Tên nhóm đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.AddNewGroup " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchGroup")]
        public ZResponse SearchGroup(ZCustomerSearchGroupRequest req) //ZShopGetAllRequest req
        {
            int result = -1;
            List<ZCustomerSearchGroupResponse> lst = new List<ZCustomerSearchGroupResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.SEARCH_GROUP", new object[]
                {
                    req.GroupCode,
                    req.GroupName,
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
                                    lst.Add(new ZCustomerSearchGroupResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        GroupCode = Convert.ToString(dr["GROUP_CODE"]),
                                        GroupName = Convert.ToString(dr["GROUP_NAME"]),
                                        OrderSuccess = Convert.ToInt32(dr["ORDER_SUCCESS"]),
                                        PurchaseMoney = Convert.ToInt32(dr["PURCHASE_MONEY"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.SearchGroup" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int ID, int ShopID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZCustomerGetByIDResponse model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.GET_BY_ID", new object[]
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
                                    model = new ZCustomerGetByIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        BirthDay = Convert.ToString(dr["BIRTH_DAY"]),
                                        TaxCode = Convert.ToString(dr["TAX_CODE"]),
                                        Channel = Convert.ToString(dr["CHANNEL"]),
                                        Note = Convert.ToString(dr["NOTE"]),
                                        Sticker = Convert.ToString(dr["CUSTOMER_STICKER"]),
                                        CustomerCode = Convert.ToString(dr["CUSTOMER_CODE"]),
                                        BlackList = Convert.ToString(dr["BLACK_LIST"]),
                                        GroupCode = Convert.ToString(dr["GROUP_CODE"]),
                                        GroupName = Convert.ToString(dr["GROUP_NAME"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        ProvinceName = Convert.ToString(dr["PROVINCE_NAME"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        DistrictName = Convert.ToString(dr["DISTRICT_NAME"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        WardName = Convert.ToString(dr["WARD_NAME"]),
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
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
                                res.Message = "Không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("CheckBlackList")]
        public ZResponse CheckBlackList(ZCustomerCheckBlackListRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.CHECK_BLACK_LIST", new object[]
                {
                    0,
                    req.ID,
                    req.IsChecked,
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
                                    res.Message = "Sai trạng thái.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.CheckBlackList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByMobileNumber")]
        [HttpPost]
        public ZResponse GetByMobileNumber(ZCustomerGetByMobileNumberRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZCustomerGetByMobileNumberResponse model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.GET_BY_MOBILE_NUMBER", new object[]
                {
                    req.MobileNumber,
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
                                if (dr.Read())
                                {
                                    model = new ZCustomerGetByMobileNumberResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        Channel = Convert.ToString(dr["CHANNEL"]),
                                        Province = new ZProvinceModel()
                                        {
                                            ID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                            Name = Convert.ToString(dr["PROVINCE_NAME"]),
                                            Title = Convert.ToString(dr["PROVINCE_TITLE"])
                                        },
                                        District = new ZDistrictModel()
                                        {
                                            ID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                            Name = Convert.ToString(dr["DISTRICT_NAME"]),
                                            Title = Convert.ToString(dr["DISTRICT_TITLE"])
                                        },
                                        Ward = new ZWardModel()
                                        {
                                            ID = Convert.ToInt32(dr["WARD_ID"]),
                                            Name = Convert.ToString(dr["WARD_NAME"]),
                                            Title = Convert.ToString(dr["WARD_TITLE"])
                                        },
                                        Address = Convert.ToString(dr["ADDRESS"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        BlackList = Convert.ToString(dr["BLACK_LIST"]),
                                        Group = new ZCustomerGroupModel()
                                        {
                                            ID = Convert.ToInt32(dr["GROUP_ID"]),
                                            Code = Convert.ToString(dr["GROUP_CODE"]),
                                            Name = Convert.ToString(dr["GROUP_NAME"])
                                        },
                                        SuccessOrder = Convert.ToInt32(dr["SUCCESS_ORDER"]),
                                        TotalOrder = Convert.ToInt32(dr["TOTAL_ORDER"]),
                                        TypeShop = Convert.ToInt32(dr["TYPE_SHOP"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.GetByMobileNumber: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("CheckIDByMobileNumber")]
        public ZResponse CheckIDByMobileNumber(ZCustomerCheckIDByMobileNumberRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.CHECK_ID_BY_MOBILE_NUMBER", new object[]
                {
                    0,
                    req.MobileNumber,
                    req.ShopID,
                    0
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
                            res.Data = dbCommand.Parameters["P_OUT_CUSTOMER_ID"].Value.ToString();
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.CheckIDByMobileNumber: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("GetCustomerIDByOrderCode")]
        //public ZResponse GetCustomerIDByOrderCode(string OrderCode, int ShopID)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.GET_CUSTOMER_ID_BY_ORDER_CODE", new object[]
        //        {
        //            0,
        //            OrderCode,
        //            ShopID,
        //            0
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
        //                    res.Data = dbCommand.Parameters["P_OUT_CUSTOMER_ID"].Value.ToString();
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Dữ liệu không tồn tại.";
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
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.GetCustomerIDByOrderCode: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("RemoveSticker")]
        public ZResponse RemoveSticker(ZCustomerRemoveStickerRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.REMOVE_STICKER", new object[]
                {
                    0,
                    req.ID,
                    req.StickerName,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.RemoveSticker " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("IsExist")]
        public ZResponse IsExist(ZCustomerIsExistRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CustomerRepository.IsExist(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.IsExist " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ImportExcel")]
        public ZResponse ImportExcel(ZCustomerImportExcelRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_PKG.IMPORT_EXCEL", new object[]
                {
                    0,
                    req.FullName,
                    req.MobileNumber,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Street,
                    req.BirthDay,
                    req.TaxCode,
                    req.Channel,
                    req.Note,
                    req.CustomerStickerID,
                    req.IsUpdate,
                    req.ShopID,
                    req.CreatedBy,
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
                                    res.Message = "Khách hàng đã tồn tại";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Gán nhãn thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerController.ImportExcel " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}