using EMS.Entity;
using EMS.Library;
using EMS.API.Gateway.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Controllers
{
    //[RoutePrefix("Handle")]
    public class HandleController : ApiController
    {
        //[HttpPost]
        //[Route("UploadImage")]
        //public async Task<HttpResponseMessage> UploadImage()
        //{
        //    ZResponse res = new ZResponse();
        //    List<string> lst = new List<string>();
        //    try
        //    {
        //        var httpRequest = HttpContext.Current.Request;
        //        foreach (string file in httpRequest.Files)
        //        {
        //            var postedFile = httpRequest.Files[file];
        //            if (postedFile != null && postedFile.ContentLength > 0)
        //            {
        //                var clientUrlImage = new HttpClient();
        //                clientUrlImage.BaseAddress = new Uri(XHelper.ImageUrl);
        //                clientUrlImage.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                for (var i = 0; i < httpRequest.Files.Count; i++)
        //                {
        //                    MultipartFormDataContent form = new MultipartFormDataContent();
        //                    HttpContent File = new StreamContent(httpRequest.Files[i].InputStream);
        //                    File.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        //                    {
        //                        FileName = httpRequest.Files[i].FileName
        //                    };
        //                    form.Add(File);
        //                    HttpResponseMessage response = clientUrlImage.PostAsync("Handle/UploadImage", form).Result;
        //                    //res = response.Content.ReadAsAsync<ResponseFile>().Result;
        //                    var result = response.Content.ReadAsStringAsync().Result;
        //                    res = JsonConvert.DeserializeObject<ZResponse>(result);

        //                    if (res.Code != "00")
        //                    {
        //                        break;
        //                    }
        //                    lst.Add(res.Data);
        //                }
        //                res.Data = JsonConvert.SerializeObject(lst);
        //                return Request.CreateResponse(res);
        //            }
        //        }
        //        res.Code = "03";
        //        res.Message = "Please select a image.";
        //        return Request.CreateResponse(HttpStatusCode.NotFound, res);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = ex.Message;
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "GatewayController.UploadImage: " + ex.Message);
        //        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, res);
        //    }
        //}
    }
}