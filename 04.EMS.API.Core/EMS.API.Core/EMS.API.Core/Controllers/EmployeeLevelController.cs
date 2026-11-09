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
    [RoutePrefix("EmployeeLevel")]
    public class EmployeeLevelController : ApiController
    {
        [Route("Gets")]
        public ZResponse Gets()
        {
            int result = -1;
            List<ZEmployeeLevelGetsResponse> lst = new List<ZEmployeeLevelGetsResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EMPLOYEE_LEVEL_PKG.GETS", new object[]
                {
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
                                    lst.Add(new ZEmployeeLevelGetsResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = Convert.ToString(dr["NAME"]),
                                        EmployeeLevelCode = Convert.ToString(dr["EMPLOYEE_LEVEL_CODE"]),
                                        IsLock = Convert.ToString(dr["IS_LOCK"])
                                    });
                                }
                            }
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = JsonConvert.SerializeObject(lst);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeLevelController.Gets " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
