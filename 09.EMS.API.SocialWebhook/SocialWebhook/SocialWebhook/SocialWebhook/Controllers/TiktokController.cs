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
using SocialWebhook.Repositories.Tiktok;

namespace SocialWebhook.Controllers
{
    [RoutePrefix("Tiktok")]
    public class TiktokController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post(object request)
        {
            TIK_EventRepository.Event(request);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        public HttpResponseMessage Get([FromUri] hub hub)
        {
            return Request.CreateResponse(HttpStatusCode.OK, hub.challenge);
        }
    }
}