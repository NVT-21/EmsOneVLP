using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SocialWebhook.Models;
using EMS.Entity;
using EMS.Library;
using SocialWebhook.Repositories.Grab;

namespace SocialWebhook.Controllers
{
    [RoutePrefix("Grab")]
    public class GrabController : ApiController
    {
        // GET: Grab
        [HttpPost]
        public HttpResponseMessage Post(object request)
        {
            ZResponse res = new ZResponse();
            try
            {
                GrabRepository.ReceiveStatus(request);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
                res.Data = JsonConvert.SerializeObject(request);
                XHelpers.WriteLog(LogFileType.EXCEPTION, string.Format("GrabController.Post: res={0}||exeception={1}", JsonConvert.SerializeObject(res), ex.Message));
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        public HttpResponseMessage Get([FromUri] hub hub)
        {
            return Request.CreateResponse(HttpStatusCode.OK, hub.challenge);
        }
    }
}
