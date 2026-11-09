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
    [RoutePrefix("PickUpAddress")]
    public class PickUpAddressController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZPickUpAddressSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZPickUpAddressSearchListResponse> listItem = new List<ZPickUpAddressSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PICK_UP_ADDRESS_PKG.SEARCH_LIST", new object[]
                {
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
                                    listItem.Add(new ZPickUpAddressSearchListResponse()
                                    {
                                        PickUpAddressID = Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        PickUpName = Convert.ToString(dr["PICK_UP_NAME"]),
                                        CollectionPostOfficeID = Convert.ToInt32(dr["COLLECTION_POST_OFFICE_ID"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        ProvinceName = Convert.ToString(dr["PROVINCE_NAME"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        WardName = Convert.ToString(dr["WARD_NAME"]),
                                        Street = Convert.ToString(dr["STREET"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        IsDefault = Convert.ToString(dr["IS_DEFAULT"]),
                                        QuantityLabelPrint = Convert.ToInt32(dr["QUANTITY_LABEL_PRINT"]),
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
                                res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PickUpAddressRepository.SearchList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZPickUpAddressAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PICK_UP_ADDRESS_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.FullName,
                    req.MobileNumber,
                    req.PickUpName,
                    req.CollectionPostOfficeID,
                    req.ProvinceID,
                    //req.DistrictID,
                    req.WardID,
                    req.Street,
                    req.IsDefault,
                    req.ShopID,
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
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên địa chỉ lấy hàng đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PickUpAddressController.Add " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int ShopID, int PickUpAddressID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZPickUpAddressGetByIDResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PICK_UP_ADDRESS_PKG.GET_BY_ID", new object[]
                {
                    PickUpAddressID,
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
                                    item = new ZPickUpAddressGetByIDResponse()
                                    {
                                        PickUpAddressID = Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        PickUpName = Convert.ToString(dr["PICK_UP_NAME"]),
                                        CollectionPostOfficeID = Convert.ToInt32(dr["COLLECTION_POST_OFFICE_ID"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        //DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        Street = Convert.ToString(dr["STREET"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        IsDefault = Convert.ToString(dr["IS_DEFAULT"])
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PickUpAddressController.GetByID " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Update")]
        public ZResponse Update(ZPickUpAddressUpdateRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PICK_UP_ADDRESS_PKG.UPDATE_PICK_UP_ADDRESS", new object[]
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
                                    res.Message = "Không tồn tại dữ liệu.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên địa chỉ lấy hàng đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Không có mẫu in mặc định.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Luôn phải có địa chỉ lấy hàng mặc định.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PickUpAddressController.Update " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Remove")]
        public ZResponse Remove(ZPickUpAddressRemoveRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PICK_UP_ADDRESS_PKG.REMOVE_PICK_UP_ADDRESS", new object[]
                {
                    0,
                    req.PickUpAddressID,
                    req.ShopID,
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
                            res.Message = "Xóa thành công.";
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
                                    res.Message = "Luôn phải có ít nhất 01 địa chỉ lấy hàng.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Không được xóa địa chỉ lấy hàng mặc định";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "PickUpAddressController.Remove " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}