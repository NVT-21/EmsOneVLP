using EMS.Entity;
using EMS.Library;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Controllers
{
    //[System.Web.Http.RoutePrefix("DataFile")]
    public class DataFileController : Controller
    {
        #region Tải file
        public static HttpClient _clientUploadFile = null;
        public JsonResult UploadFile()
        {
            ZResponse res = new ZResponse();
            try
            {
                HttpFileCollectionBase httpRequest = Request.Files;
                if (httpRequest.Count > 0)
                {
                    if (_clientUploadFile == null)
                    {
                        _clientUploadFile = new HttpClient();
                        _clientUploadFile.BaseAddress = new Uri(XHelper.URLImage);
                        _clientUploadFile.DefaultRequestHeaders.Accept.Clear();
                        _clientUploadFile.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    for (int i = 0; i < httpRequest.Count; i++)
                    {
                        HttpContent content = new StreamContent(httpRequest[i].InputStream);
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = httpRequest[i].FileName
                        };
                        form.Add(content);
                    }
                    HttpResponseMessage response = _clientUploadFile.PostAsync("Handle/UploadFile", form).Result;
                    res = response.Content.ReadAsAsync<ZResponse>().Result;
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn tập tin.";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
                XHelper.LogToFile(LogFileType.EXCEPTION, "DataFileController.UploadFile: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tải ảnh
        public static HttpClient _clientUploadImage = null;
        public JsonResult UploadImage()
        {
            ZResponse res = new ZResponse();
            try
            {
                HttpFileCollectionBase httpRequest = Request.Files;
                if (httpRequest.Count > 0)
                {
                    if (_clientUploadImage == null)
                    {
                        _clientUploadImage = new HttpClient();
                        _clientUploadImage.BaseAddress = new Uri(XHelper.URLImage);
                        _clientUploadImage.DefaultRequestHeaders.Accept.Clear();
                        _clientUploadImage.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    for (int i = 0; i < httpRequest.Count; i++)
                    {
                        if (httpRequest[i].ContentLength > MaxContentLength)
                        {
                            res.Code = "02";
                            res.Message = "Vui lòng chọn tập tin có kích thước thấp hơn hoặc bằng 5MB.";
                            return Json(res, JsonRequestBehavior.AllowGet);
                        }
                        HttpContent content = new StreamContent(httpRequest[i].InputStream);
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            FileName = httpRequest[i].FileName
                        };
                        form.Add(content);
                    }
                    HttpResponseMessage response = _clientUploadImage.PostAsync("Handle/UploadImage", form).Result;
                    res = response.Content.ReadAsAsync<ZResponse>().Result;
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Vui lòng chọn ảnh";
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
                XHelper.LogToFile(LogFileType.EXCEPTION, "DataFileController.UploadImage: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //HttpClient _clientDataFile = null;

        //[System.Web.Http.Route("UploadImage")]
        //public async Task<HttpResponseMessage> UploadImage()
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var httpRequest = HttpContext.Current.Request;
        //        if (httpRequest.Files.Count > 0)
        //        {
        //            if (_clientDataFile == null)
        //            {
        //                _clientDataFile = new HttpClient();
        //                _clientDataFile.BaseAddress = new Uri(XHelper.ImageUrl);
        //                _clientDataFile.DefaultRequestHeaders.Accept.Clear();
        //                _clientDataFile.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //            }

        //            MultipartFormDataContent form = new MultipartFormDataContent();
        //            for (int i = 0; i < httpRequest.Files.Count; i++)
        //            {
        //                HttpContent content = new StreamContent(httpRequest.Files[i].InputStream);
        //                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        //                {
        //                    FileName = httpRequest.Files[i].FileName
        //                };
        //                form.Add(content);
        //            }
        //            HttpResponseMessage response = _clientDataFile.PostAsync("Handle/UploadImage", form).Result;
        //            res = response.Content.ReadAsAsync<ZResponse>().Result;
        //            return Request.CreateResponse(HttpStatusCode.OK, res);
        //        }

        //        res.Code = "03";
        //        res.Message = "Please select a image.";
        //        return Request.CreateResponse(HttpStatusCode.NotFound, res);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = ex.Message;
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "DataFileController.UploadImage: " + ex.Message);
        //        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, res);
        //    }
        //}

        //[System.Web.Http.Route("UploadFile")]
        //public async Task<HttpResponseMessage> UploadFile()
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var httpRequest = HttpContext.Current.Request;
        //        var file = httpRequest.Files;
        //        if (httpRequest.Files.Count > 0)
        //        {
        //            if (_clientDataFile == null)
        //            {
        //                _clientDataFile = new HttpClient();
        //                _clientDataFile.BaseAddress = new Uri(XHelper.ImageUrl);
        //                _clientDataFile.DefaultRequestHeaders.Accept.Clear();
        //                _clientDataFile.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //            }
        //            MultipartFormDataContent form = new MultipartFormDataContent();
        //            for (int i = 0; i < httpRequest.Files.Count; i++)
        //            {
        //                HttpContent content = new StreamContent(httpRequest.Files[i].InputStream);
        //                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        //                {
        //                    FileName = httpRequest.Files[i].FileName
        //                };
        //                form.Add(content);
        //            }
        //            HttpResponseMessage response = _clientDataFile.PostAsync("Handle/UploadFile", form).Result;
        //            res = response.Content.ReadAsAsync<ZResponse>().Result;
        //            return Request.CreateResponse(HttpStatusCode.OK, res);
        //        }

        //        res.Code = "01";
        //        res.Message = "Vui lòng chọn tập tin.";
        //        return Request.CreateResponse(HttpStatusCode.NotFound, res);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = ex.Message;
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "DataFileController.UploadFile: " + ex.Message);
        //        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, res);
        //    }
        //}
    }
}
