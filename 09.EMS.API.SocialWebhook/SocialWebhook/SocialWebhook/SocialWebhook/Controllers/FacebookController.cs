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
using SocialWebhook.Repositories.Facebook;

namespace SocialWebhook.Controllers
{
    [RoutePrefix("Facebook")]
    public class FacebookController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post(object request)
        {
            //XHelpers.SendNotifyTelegram(JsonConvert.SerializeObject(request));
            ZResponse res = new ZResponse();
            var _request = JsonConvert.DeserializeObject<FacebookRequest>(JsonConvert.SerializeObject(request));
            try
            {
                if (_request.Entry[0].Messaging.Count > 0)
                {
                    if (_request.Entry[0].Messaging[0].Message != null)
                    {
                        //Gửi tin nhắn
                        if (_request.Entry[0].ID == _request.Entry[0].Messaging[0].Sender.ID)
                        {
                            FAB_EventRepository.Event("PageSendMessage", _request);
                        }
                        else
                        {
                            FAB_EventRepository.Event("UserSendMessage", _request);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else if (_request.Entry[0].Messaging[0].Delivery != null)
                    {
                        //Đã nhận
                        FAB_EventRepository.Event("RecievedMessage", _request);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else if (_request.Entry[0].Messaging[0].Read != null)
                    {
                        //Đã xem
                        FAB_EventRepository.Event("SeenMessage", _request);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        //XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("FacebookController.Post: 2 ||req={0}", JsonConvert.SerializeObject(request)));
                    }
                }
                else
                {
                    //XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("FacebookController.Post: 1 ||req={0}", JsonConvert.SerializeObject(request)));
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
                res.Data = JsonConvert.SerializeObject(request);
                //XHelpers.WriteLog(LogFileType.EXCEPTION, string.Format("FacebookController.Post: res={0}||exeception={1}", JsonConvert.SerializeObject(res), ex.Message));
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        public HttpResponseMessage Get([FromUri] hub hub)
        {
            //XHelpers.Invoke("POST", "Log/WriteLog", _request);
            return Request.CreateResponse(HttpStatusCode.OK, hub.challenge);
        }
    }
}