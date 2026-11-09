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

namespace EMS.API.Core.Repositories
{
    public class LabelPrintRepository
    {
        public static ZResponse SearchList(ZLabelPrintSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZLabelPrintSearchListResponse> lst = new List<ZLabelPrintSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_LABEL_PRINT_PKG.SEARCH_LIST", new object[]
                {
                    req.PickUpAddressID,
                    req.ShopID,
                    req.Type,
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
                                    lst.Add(new ZLabelPrintSearchListResponse()
                                    {
                                        LabelPrintID = Convert.ToInt32(dr["LABEL_PRINT_ID"]),
                                        PickUpAddressID = Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"]),
                                        LabelName = Convert.ToString(dr["LABEL_NAME"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        MobileNumber = Convert.ToString(dr["MOBILE_NUMBER"]),
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        ProvinceName = Convert.ToString(dr["PROVINCE_NAME"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        DistrictName = Convert.ToString(dr["DISTRICT_NAME"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        WardName = Convert.ToString(dr["WARD_NAME"]),
                                        Street = Convert.ToString(dr["STREET"]),
                                        FullAddress = Convert.ToString(dr["FULL_ADDRESS"]),
                                        IsDefault = Convert.ToString(dr["IS_DEFAULT"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "LabelPrintRepository.SearchList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}