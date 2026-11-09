using EMS.Core;
using EMS.Entity;
using EMS.Library;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace EMS.API.Core.Repositories
{
    public class OptionRepositiry
    {
        //public static ZResponse SearchOption(ZOptionAddRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_OPTION_PKG.SEARCH", new object[]
        //        {
        //            0,
        //            req.ParentID,
        //            req.Name,
        //            req.ShopID,
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
        //                    res.Message = "Thành công.";
        //                    res.Data = Convert.ToString(dbCommand.Parameters["P_OUT_OPTION_ID"].Value);
        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Lỗi biến thể.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OptionRepositiry.SearchOption: " + ex.Message);
        //        result = -99;
        //    }
        //    return res;
        //}
        public static int Add(ZOptionAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_OPTION_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.ShopID,
                    req.ProductID,
                    req.Name,
                    req.Values,
                    req.CreatedBy,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                    }
                    else
                    {
                        result = -2;
                        XHelper.LogToFile(LogFileType.EXCEPTION, "OptionRepositiry.Add: Lỗi hệ thống, vui lòng liên hệ quản trị.");
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OptionRepositiry.Add: " + ex.Message);
                result = -99;
            }
            return result;
        }
    }
}