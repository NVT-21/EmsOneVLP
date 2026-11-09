using EMS.API.Core.Repositories;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Country")]
    public class CountryController : ApiController
    {
        [Route("List")]
        public ZResponse List()
        {
            ZResponse res = new ZResponse();
            try
            {
                res = CountryRepository.List();
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProvinceController.List: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
