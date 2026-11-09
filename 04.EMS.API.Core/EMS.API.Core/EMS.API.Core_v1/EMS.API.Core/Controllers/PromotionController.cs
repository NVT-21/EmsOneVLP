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
    [RoutePrefix("Promotion")]
    public class PromotionController : ApiController
    {
        [Route("AddNewPromotionCode")]
        public ZResponse AddNewPromotionCode(ZPromotionAddNewCodeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            string PromotionName = "";
            try
            {
                int ProductGroupID = 0;
                int ProductID = 0;
                int CustomerGroupID = 0;
                int CustomerID = 0;
                
                if(req.ApplyForOption == 3) //Nhóm sản phẩm
                {
                    ProductGroupID = Convert.ToInt32(req.AfoChild);
                }
                else if (req.ApplyForOption == 4) //Sản phẩm
                {
                    ProductID = Convert.ToInt32(req.AfoChild);
                }
                else if (req.ApplyForOption == 5) //Nhóm khách hàng
                {
                    CustomerGroupID = Convert.ToInt32(req.AfoChild);
                }
                else if (req.ApplyForOption == 6) //Khách hàng
                {
                    CustomerID = Convert.ToInt32(req.AfoChild);
                }
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROMOTION_PKG.ADD_NEW_PROMOTION_CODE", new object[]
                {
                    0,
                    req.PromotionCode,
                    PromotionName,
                    req.AllowUse,
                    req.PromotionTypeOption,
                    req.Reduction,
                    req.MaxReduction,
                    req.ApplyForOption,
                    req.AfoChild,
                    req.AfoCoditionsApply,
                    req.AfoCaValue,
                    req.NumberOfUse,
                    req.NumberOfCustomerUse,
                    req.StartDate,
                    req.EndDate,
                    req.SubjectsOfApplication,
                    req.ScopeOfUse,
                    req.ShopID,
                    req.CreatedBy,
                    ProductGroupID,
                    ProductID,
                    CustomerGroupID,
                    CustomerID,
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
                                    res.Message = "Mã khuyến mãi đã tồn tại!";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PromotionController.AddNewPromotionCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("AddNewPromotion")]
        public ZResponse AddNewPromotion(ZPromotionAddNewRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                int ProductGroupID = 0;
                int ProductID = 0;
                int CustomerGroupID = 0;
                int CustomerID = 0;

                if (req.ApplyForOption == 3) //Nhóm sản phẩm
                {
                    ProductGroupID = Convert.ToInt32(req.AfoChild);
                }
                else if (req.ApplyForOption == 4) //Sản phẩm
                {
                    ProductID = Convert.ToInt32(req.AfoChild);
                }
                else if (req.ApplyForOption == 5) //Nhóm khách hàng
                {
                    CustomerGroupID = Convert.ToInt32(req.AfoChild);
                }
                else if (req.ApplyForOption == 6) //Khách hàng
                {
                    CustomerID = Convert.ToInt32(req.AfoChild);
                }
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROMOTION_PKG.ADD_NEW_PROMOTION", new object[]
                {
                    0,
                    req.PromotionName,
                    req.PromotionTypeOption,
                    req.Reduction,
                    req.ApplyForOption,
                    req.AfoChild,
                    req.AfoCoditionsApply,
                    req.AfoCaValue,
                    req.StartDate,
                    req.EndDate,
                    req.ShopID,
                    req.CreatedBy,
                    ProductGroupID,
                    ProductID,
                    CustomerGroupID,
                    CustomerID,
                    req.NumberOfUse,
                    req.NumberOfCustomerUse
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
                                    res.Message = "Tên chương trình khuyến mãi đã tồn tại!";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PromotionController.AddNewPromotion: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GetByCode")]
        [HttpPost]
        public ZResponse GetByCode(ZPromotionGetByCodeRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZPromotionGetByCodeResponse item = new ZPromotionGetByCodeResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROMOTION_PKG.GET_BY_CODE", new object[]
                {
                    req.Code,       //P_CODE        VARCHAR
                    req.ShopID,     //P_SHOP_ID     NUMBER
                    0,              //P_RETURN_CODE OUT NUMBER
                    0               //P_OUT_CURSOR  OUT T_CURSOR
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
                                    item = new ZPromotionGetByCodeResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["PROMOTION_CODE"].ToString(),
                                        PromotionTypeOption = Convert.ToInt32(dr["PROMOTION_TYPE_OPTION"]),
                                        AllowUse = dr["ALLOW_USE"].ToString(),
                                        Reduction = dr["PTO_REDUCTION"].ToString(),
                                        MaxReduction = dr["PTO_MAX_REDUCTION"].ToString(),
                                        ApplyForOption = Convert.ToInt32(dr["APPLY_FOR_OPTION"]),
                                        AfoChild = Convert.ToInt32(dr["AFO_CHILD"]),
                                        AfoCoditionsApply = Convert.ToInt32(dr["AFO_CONDITIONS_APPLY"]),
                                        AfoCaValue = Convert.ToInt32(dr["AFO_CA_VALUE"]),
                                        NumberOfUse = Convert.ToInt32(dr["NUMBER_OF_USE"]),
                                        SubjectsOfApplication = Convert.ToInt32(dr["SUBJECTS_OF_APPLICATION"]),
                                        ScopeOfUse = Convert.ToInt32(dr["SCOPE_OF_USE"])
                                    };
                                }
                            }
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(item);
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã khuyến mãi không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã khuyến mãi đã hết hạn sử dụng.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Kiểm tra mã khuyến mãi thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PromotionController.GetByCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("List")]
        public ZResponse List(ZPromotionListRequest req) //ZShopGetAllRequest req
        {
            int result = -1;
            List<ZPromotionListResponse> lst = new List<ZPromotionListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROMOTION_PKG.LIST", new object[]
                {
                    req.ShopID,
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
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    int PromotionTypeOption = Convert.ToInt32(dr["PROMOTION_TYPE_OPTION"]);
                                   
                                    lst.Add(new ZPromotionListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        PromotionCode = Convert.ToString(dr["PROMOTION_CODE"]),
                                        PromotionName = Convert.ToString(dr["PROMOTION_NAME"]),
                                        AllowUse = Convert.ToString(dr["ALLOW_USE"]),
                                        PromotionTypeOption = PromotionTypeOption,
                                        Reduction = Convert.ToInt32(!string.IsNullOrEmpty(dr["PTO_REDUCTION"].ToString()) ? dr["PTO_REDUCTION"].ToString() : "0"),
                                        MaxReduction = Convert.ToInt32(!string.IsNullOrEmpty(dr["PTO_MAX_REDUCTION"].ToString()) ? dr["PTO_MAX_REDUCTION"].ToString() : "0"),
                                        ApplyForOption = Convert.ToInt32(dr["APPLY_FOR_OPTION"]),
                                        AfoChild = Convert.ToString(dr["AFO_CHILD"]),
                                        AfoCoditionsApply = Convert.ToInt32(!string.IsNullOrEmpty(dr["AFO_CONDITIONS_APPLY"].ToString()) ? dr["AFO_CONDITIONS_APPLY"].ToString() : "0"),
                                        AfoCaValue = Convert.ToInt32(!string.IsNullOrEmpty(dr["AFO_CA_VALUE"].ToString()) ? dr["AFO_CA_VALUE"].ToString() : "0"),
                                        NumberOfUse = Convert.ToInt32(!string.IsNullOrEmpty(dr["NUMBER_OF_USE"].ToString()) ? dr["NUMBER_OF_USE"].ToString() : "0"),
                                        NumberOfCustomerUse = Convert.ToInt32(!string.IsNullOrEmpty(dr["NUMBER_OF_CUSTOMER_USE"].ToString()) ? dr["NUMBER_OF_CUSTOMER_USE"].ToString() : "0"),
                                        StartDate = Convert.ToString(dr["START_DATE"]),
                                        EndDate = Convert.ToString(dr["END_DATE"]),
                                        SubjectsOfApplication = Convert.ToInt32(!string.IsNullOrEmpty(dr["SUBJECTS_OF_APPLICATION"].ToString()) ? dr["SUBJECTS_OF_APPLICATION"].ToString() : "0"),
                                        ScopeOfUse = Convert.ToInt32(!string.IsNullOrEmpty(dr["SCOPE_OF_USE"].ToString()) ? dr["SCOPE_OF_USE"].ToString() : "0"),
                                        IsLock = Convert.ToString(dr["IS_LOCK"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        EmployeeName = Convert.ToString(dr["EMPLOYEE_NAME"]),
                                        ProductGroupName = Convert.ToString(dr["PRODUCT_GROUP_NAME"]),
                                        ProductName = Convert.ToString(dr["PRODUCT_NAME"]),
                                        CustomerGroupName = Convert.ToString(dr["CUSTOMER_GROUP_NAME"]),
                                        CustomerMobileNumber = Convert.ToString(dr["CUSTOMER_MOBILE_NUMBER"]),
                                        SubjectsOfApplicationName = Convert.ToString(dr["SUBJECTS_OF_APPLICATION_NAME"])
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
                                res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PromotionController.List " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("IsLock")]
        public ZResponse IsLock(ZPromotionIsLockRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROMOTION_PKG.IS_LOCK", new object[]
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PromotionController.IsLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("Remove")]
        public ZResponse Remove(ZPromotionDeleteRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROMOTION_PKG.DELETE_PROMOTION", new object[]
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
                                    res.Message = "Tên chương trình hoặc mã khuyến mãi đã xóa!";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PromotionController.Remove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
