using EMS.API.Image.Models;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace EMS.API.Image.Controllers
{
    [RoutePrefix("Handle")]
    public class HandleController : ApiController
    {
        [Route("UploadImage")]
        public async Task<HttpResponseMessage> UploadImage()
        {
            ZResponse res = new ZResponse();

            try
            {
                int check = 1;
                List<UploadImageReponse> Files = new List<UploadImageReponse>();
                var httpRequest = HttpContext.Current.Request;
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            res.Code = "01";
                            res.Message = "Please upload image of type: .jpg,.gif,.png.";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, res);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            res.Code = "02";
                            res.Message = "Please upload a file upto 5 MB.";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, res);
                        }
                        else
                        {
                            var date = DateTime.Now.ToString("yyMMdd");
                            var folderPath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + date);
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                            var fileName = Guid.NewGuid().ToString() + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + date + @"/" + fileName);


                            postedFile.SaveAs(filePath);
                            Files.Add(new UploadImageReponse()
                            {
                                FileName = date + @"/" + fileName
                            });
                        }
                    }
                    else if (check == httpRequest.Files.Count)
                    {
                        res.Code = "03";
                        res.Message = "Please select a image.";
                        return Request.CreateResponse(HttpStatusCode.NotFound, res);
                    }
                    else
                    {
                        check++;
                    }
                }
                res.Code = "00";
                res.Message = "Image updated successfully.";
                res.Data = JsonConvert.SerializeObject(Files);
                return Request.CreateResponse(HttpStatusCode.OK, res);

            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, res);
            }
        }

        [Route("UploadFile")]
        public async Task<HttpResponseMessage> UploadFile()
        {
            ZResponse res = new ZResponse();
            HttpStatusCode StatusCode = new HttpStatusCode();
            try
            {
                List<UploadFileResponse> Files = new List<UploadFileResponse>();
                var httpRequest = HttpContext.Current.Request;
                var countFile = httpRequest.Files.Count;
                var check = 0;
                var ext = "";
                if (countFile > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  
                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf" };
                            try
                            {
                                ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            }
                            catch
                            {
                                res.Code = "04";
                                res.Message = "Vui lòng nhập tên file không dấu.";
                                StatusCode = HttpStatusCode.ExpectationFailed;
                                return Request.CreateResponse(StatusCode, res);
                            }

                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                res.Code = "01";
                                res.Message = "Hệ thống chỉ hỗ trợ tải tập tin dạng ảnh, Word, Excel, PDF.";
                                StatusCode = HttpStatusCode.BadRequest;
                                return Request.CreateResponse(StatusCode, res);
                            }
                            else if (postedFile.ContentLength > MaxContentLength)
                            {
                                res.Code = "02";
                                res.Message = "Vui lòng chọn tập tin có kích thước thấp hơn hoặc bằng 5MB.";
                                StatusCode = HttpStatusCode.BadRequest;
                                return Request.CreateResponse(StatusCode, res);
                            }
                            else
                            {
                                var date = DateTime.Now.ToString("yyMMdd");
                                var folderPath = HttpContext.Current.Server.MapPath("~/Assets/RequestSupport/" + date);
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }
                                var fileName = Guid.NewGuid().ToString() + extension;
                                var filePath = HttpContext.Current.Server.MapPath("~/Assets/RequestSupport/" + date + @"/" + fileName);
                                postedFile.SaveAs(filePath);
                                Files.Add(new UploadFileResponse()
                                {
                                    FileName = date + @"/" + fileName
                                });
                                check++;
                            }
                        }
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Hệ thống chỉ hỗ trợ tải tập tin dạng Ảnh, Word, Excel, PDF.";
                    StatusCode = HttpStatusCode.NotFound;
                    return Request.CreateResponse(StatusCode, res);
                }

                if (check == countFile)
                {
                    res.Code = "00";
                    res.Message = "Tải tập tin thành công.";
                    res.Data = JsonConvert.SerializeObject(Files);
                    StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    res.Code = "03";
                    res.Message = "Tải tập tin thất bại.";
                    StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
                StatusCode = HttpStatusCode.ExpectationFailed;
            }
            return Request.CreateResponse(StatusCode, res);
        }

        [Route("UploadExcel")]
        public async Task<HttpResponseMessage> UploadExcel()
        {
            ZResponse res = new ZResponse();
            HttpStatusCode StatusCode = new HttpStatusCode();
            try
            {
                List<UploadExcelResponse> Files = new List<UploadExcelResponse>();
                var httpRequest = HttpContext.Current.Request;
                var countFile = httpRequest.Files.Count;
                var check = 0;
                var ext = "";
                if (countFile > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            int MaxContentLength = 1024 * 1024 * 10; //Size = 10 MB  
                            IList<string> AllowedFileExtensions = new List<string> { ".xls", ".xlsx", ".xlsm" };
                            try
                            {
                                ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            }
                            catch
                            {
                                res.Code = "04";
                                res.Message = "Vui lòng nhập tên file không dấu.";
                                StatusCode = HttpStatusCode.ExpectationFailed;
                                return Request.CreateResponse(StatusCode, res);
                            }

                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                res.Code = "01";
                                res.Message = "Hệ thống chỉ hỗ trợ tải tập tin Excel định dạng xls, xlsx hoặc xlsm.";
                                StatusCode = HttpStatusCode.BadRequest;
                                return Request.CreateResponse(StatusCode, res);
                            }
                            else if (postedFile.ContentLength > MaxContentLength)
                            {
                                res.Code = "02";
                                res.Message = "Vui lòng chọn tập tin có kích thước thấp hơn hoặc bằng 5MB.";
                                StatusCode = HttpStatusCode.BadRequest;
                                return Request.CreateResponse(StatusCode, res);
                            }
                            else
                            {
                                var date = DateTime.Now.ToString("yyMMdd");
                                var folderPath = HttpContext.Current.Server.MapPath("~/Assets/Excels/" + date);
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                    DirectorySecurity sec = Directory.GetAccessControl(folderPath);
                                    // Using this instead of the "Everyone" string means we work on non-English systems.
                                    SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                                    sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                                    Directory.SetAccessControl(folderPath, sec);
                                }
                                var fileName = Guid.NewGuid().ToString() + extension;
                                var filePath = HttpContext.Current.Server.MapPath("~/Assets/Excels/" + date + @"/" + fileName);
                                postedFile.SaveAs(filePath);
                                Files.Add(new UploadExcelResponse()
                                {
                                    FileName = date + @"/" + fileName,
                                    FullPath = Security.EncodeBase64(filePath)
                                });
                                check++;
                            }
                        }
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Hệ thống chỉ hỗ trợ tải tập tin Excel định dạng xls, xlsx hoặc xlsm.";
                    StatusCode = HttpStatusCode.NotFound;
                    return Request.CreateResponse(StatusCode, res);
                }

                if (check == countFile)
                {
                    res.Code = "00";
                    res.Message = "Tải tập tin thành công.";
                    res.Data = JsonConvert.SerializeObject(Files);
                    StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    res.Code = "03";
                    res.Message = "Tải tập tin thất bại.";
                    StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
                StatusCode = HttpStatusCode.ExpectationFailed;
            }
            return Request.CreateResponse(StatusCode, res);
        }

        [Route("UploadFacebookFile")]
        public async Task<HttpResponseMessage> UploadFacebookFile()
        {
            ZResponse res = new ZResponse();
            HttpStatusCode StatusCode = new HttpStatusCode();
            try
            {
                List<UploadFileResponse> Files = new List<UploadFileResponse>();
                var httpRequest = HttpContext.Current.Request;
                var countFile = httpRequest.Files.Count;
                var check = 0;
                var ext = "";
                if (countFile > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            int MaxContentLength = 1024 * 1024 * 100; //Size = 100 MB  
                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".gif", ".mov", ".mp4", ".avi", ".pptx", ".xml", ".mp3", ".m4a" };
                            try
                            {
                                ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            }
                            catch
                            {
                                res.Code = "04";
                                res.Message = "Vui lòng nhập tên file không dấu.";
                                StatusCode = HttpStatusCode.ExpectationFailed;
                                return Request.CreateResponse(StatusCode, res);
                            }

                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                res.Code = "01";
                                res.Message = "Hệ thống chỉ hỗ trợ tải tập tin dạng ảnh, Word, Excel, Powerpoint, PDF, Video, Audio.";
                                StatusCode = HttpStatusCode.BadRequest;
                                return Request.CreateResponse(StatusCode, res);
                            }
                            else if (postedFile.ContentLength > MaxContentLength)
                            {
                                res.Code = "02";
                                res.Message = "Vui lòng chọn tập tin có kích thước thấp hơn hoặc bằng 100MB.";
                                StatusCode = HttpStatusCode.BadRequest;
                                return Request.CreateResponse(StatusCode, res);
                            }
                            else
                            {
                                var date = DateTime.Now.ToString("yyMMdd");
                                var folderPath = HttpContext.Current.Server.MapPath("~/Assets/Facebook/File/" + date);
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }
                                var fileName = Guid.NewGuid().ToString() + extension;
                                var filePath = HttpContext.Current.Server.MapPath("~/Assets/Facebook/File/" + date + @"/" + fileName);
                                postedFile.SaveAs(filePath);
                                Files.Add(new UploadFileResponse()
                                {
                                    FileName = date + @"/" + fileName
                                });
                                check++;
                            }
                        }
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Hệ thống chỉ hỗ trợ tải tập tin dạng ảnh, Word, Excel, Powerpoint, PDF, Video, Audio.";
                    StatusCode = HttpStatusCode.NotFound;
                    return Request.CreateResponse(StatusCode, res);
                }

                if (check == countFile)
                {
                    res.Code = "00";
                    res.Message = "Tải tập tin thành công.";
                    res.Data = JsonConvert.SerializeObject(Files);
                    StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    res.Code = "03";
                    res.Message = "Tải tập tin thất bại.";
                    StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = ex.Message;
                StatusCode = HttpStatusCode.ExpectationFailed;
            }
            return Request.CreateResponse(StatusCode, res);
        }

        //[Route("UploadImage")]
        //public async Task<HttpResponseMessage> UploadImage()
        //{
        //    Dictionary<string, object> dict = new Dictionary<string, object>();

        //    try
        //    {
        //        var httpRequest = HttpContext.Current.Request;
        //        foreach (string file in httpRequest.Files)
        //        {
        //            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
        //            var postedFile = httpRequest.Files[file];
        //            if (postedFile != null && postedFile.ContentLength > 0)
        //            {
        //                int MaxContentLength = 1024 * 1024 * 10; //Size = 2 MB  
        //                IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".tiff" };
        //                var iIndex = postedFile.FileName.LastIndexOf('.');
        //                var ext = postedFile.FileName.Substring(iIndex);
        //                var extension = ext.ToLower();
        //                if (iIndex < 0)
        //                {
        //                    dict.Add("Code", "01");
        //                    //dict.Add("errMsg", "Please upload image of type: .jpg,.gif,.png.");
        //                    dict.Add("Message", "Chỉ được tải ảnh với định dạng: .jpg,.gif,.png.");
        //                    dict.Add("StatusCode", HttpStatusCode.BadRequest);
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
        //                }

        //                if (!AllowedFileExtensions.Contains(extension))
        //                {
        //                    if (extension == ".pdf")
        //                    {
        //                        var fileName = Guid.NewGuid().ToString() + ".pdf";// extension;
        //                        var filePath = HttpContext.Current.Server.MapPath("~/Assets/Pdfs/" + fileName);
        //                        postedFile.SaveAs(filePath);
        //                        dict.Add("Code", "00");
        //                        dict.Add("Message", "File updated successfully.");
        //                        dict.Add("fileName", fileName);
        //                        return Request.CreateResponse(HttpStatusCode.OK, dict);
        //                    }
        //                    dict.Add("Code", "01");
        //                    dict.Add("Message", "Chỉ được tải ảnh với định dạng: .jpg, .gif, .png, .pdf.");
        //                    dict.Add("StatusCode", HttpStatusCode.BadRequest);
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
        //                }
        //                else if (postedFile.ContentLength > MaxContentLength)
        //                {
        //                    dict.Add("Code", "02");
        //                    //dict.Add("errMsg", "Please upload a file upto 1 MB.");
        //                    dict.Add("Message", "Chỉ được tải ảnh với dung lượng nhỏ hơn 10 MB.");
        //                    dict.Add("StatusCode", HttpStatusCode.BadRequest);
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
        //                }
        //                else
        //                {
        //                    var fileName = Guid.NewGuid().ToString() + extension;
        //                    var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + fileName);
        //                    postedFile.SaveAs(filePath);
        //                    dict.Add("Code", "00");
        //                    dict.Add("Message", "Image updated successfully.");
        //                    dict.Add("fileName", fileName);
        //                    dict.Add("StatusCode", HttpStatusCode.OK);
        //                    return Request.CreateResponse(HttpStatusCode.OK, dict);
        //                }
        //            }
        //        }

        //        dict.Add("Code", "03");
        //        dict.Add("Message", "Please select a image.");
        //        dict.Add("StatusCode", HttpStatusCode.NotFound);
        //        return Request.CreateResponse(HttpStatusCode.NotFound, dict);
        //    }
        //    catch (Exception ex)
        //    {
        //        dict.Add("Code", "99");
        //        dict.Add("Message", ex.Message);
        //        dict.Add("StatusCode", HttpStatusCode.ExpectationFailed);
        //        return Request.CreateResponse(HttpStatusCode.ExpectationFailed, dict);
        //    }
        //}
    }
}
