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
using SocialWebhook.Repositories.Zalo;
namespace SocialWebhook.Controllers
{
    [RoutePrefix("Zalo")]
    public class ZaloController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post(dynamic request)
        {
            ZAL_EventRepository.Event(new ZaloRequest() 
            {
                event_name = request.event_name,
                data = request
            });
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        public HttpResponseMessage Get(object request)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}