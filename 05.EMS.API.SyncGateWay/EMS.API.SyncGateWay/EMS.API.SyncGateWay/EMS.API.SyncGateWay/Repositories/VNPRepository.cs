using EMS.API.GateWay;
using EMS.API.GateWay.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.SyncGateWay.Repositories
{
    public class VNPRepository
    {
        public static ZResponse Autocomplete(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZVNPMapsAutocompleteRequest>(data);
                var callAPI = APILibraryRepository.InvokeVNP(new ZVNPRequest()
                {
                    Method = "GET",
                    RequestURL = APILibraryRepository.VNPMapsUrl,
                    RequestURI = string.Format("autocomplete?api-version={0}&apikey={1}&text={2}&size={3}", APILibraryRepository.VNPMapsAutocompleteVersion, APILibraryRepository.VNPMapsAutocompleteKey, req.Text, req.Size),
                    AuthorizationName = "Basic",
                    AuthorizationValue = APILibraryRepository.VNPMapsKey
                });
                if (callAPI.Code == "00")
                {
                    var resVNP = JsonConvert.DeserializeObject<ZVNPMapsAutocompleteResponse>(callAPI.Data);
                    if (resVNP.code.ToUpper() == "OK")
                    {
                        if (resVNP.data.features.Count > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = JsonConvert.SerializeObject(resVNP.data);
                        }
                        else
                        {
                            res.Code = "01";
                            res.Message = "Không có dữ liệu.";
                        }
                    }
                    else
                    {
                        res.Code = "98";
                        res.Message = "Phân tích địa chỉ thất bại.";
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("VNPRepository.Autocomplete: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("VNPRepository.Autocomplete: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }

        public static ZResponse Search(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZVNPMapsSearchRequest>(data);
                var callAPI = APILibraryRepository.InvokeVNP(new ZVNPRequest()
                {
                    Method = "GET",
                    RequestURL = APILibraryRepository.VNPMapsUrl,
                    RequestURI = string.Format("search?api-version={0}&points_encoded={1}&apikey={2}&text={3}&size={4}", APILibraryRepository.VNPMapsAutocompleteVersion, false, APILibraryRepository.VNPMapsAutocompleteKey, req.Text, req.Size),
                    AuthorizationName = "Basic",
                    AuthorizationValue = APILibraryRepository.VNPMapsKey
                });
                if (callAPI.Code == "00")
                {
                    var resVNP = JsonConvert.DeserializeObject<ZVNPMapsSearchResponse>(callAPI.Data);
                    if (resVNP.code.ToUpper() == "OK")
                    {
                        if (resVNP.data.features.Count > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = JsonConvert.SerializeObject(resVNP.data);
                        }
                        else
                        {
                            res.Code = "01";
                            res.Message = "Không có dữ liệu.";
                        }
                    }
                    else
                    {
                        res.Code = "98";
                        res.Message = "Có lỗi phân tích địa chỉ, vui lòng nhập thủ công các trường thông tin địa chỉ.";
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("VNPRepository.Search: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("VNPRepository.Search: request={0}||response={1}", data, ex.Message));
            }
            return res;
        }

        //public static ZResponse Autocomplete(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZVNPMapsAutocompleteRequest>(data);
        //    var requestUri = string.Format("autocomplete?api-version={0}&apikey={1}&text={2}&size={3}", APILibraryRepository.VNPMapsAutocompleteVersion, APILibraryRepository.VNPMapsAutocompleteKey, req.Text, req.Size);
        //    var res = APILibraryRepository.CallApiVNPMaps("GET", requestUri, "");
        //    if (res.Code == "00")
        //    {
        //        var ResponseData = JsonConvert.DeserializeObject<ZVNPMapsAutocompleteResponse>(res.Data);
        //        if (ResponseData.code.ToUpper() == "OK")
        //        {
        //            if (ResponseData.data.features.Count > 0)
        //            {
        //                res.Code = "00";
        //                res.Message = "Thành công.";
        //            }
        //            else
        //            {
        //                res.Code = "01";
        //                res.Message = "Không có dữ liệu.";
        //            }
        //        }
        //        else
        //        {
        //            res.Code = "98";
        //            res.Message = "Phân tích địa chỉ thất bại.";
        //            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("VNPRepository.Autocomplete: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
        //        }
        //    }
        //    else
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("VNPRepository.Autocomplete: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
        //    }
        //    return res;
        //}

        //public static ZResponse Search(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZVNPMapsSearchRequest>(data);
        //    var requestUri = string.Format("search?api-version={0}&points_encoded={1}&apikey={2}&text={3}&size={4}", APILibraryRepository.VNPMapsAutocompleteVersion, false, APILibraryRepository.VNPMapsAutocompleteKey, req.Text, req.Size);
        //    var res = APILibraryRepository.CallApiVNPMaps("GET", requestUri, "");
        //    if (res.Code == "00")
        //    {
        //        var ResponseData = JsonConvert.DeserializeObject<ZVNPMapsAutocompleteResponse>(res.Data);
        //        if (ResponseData.code.ToUpper() == "OK")
        //        {
        //            if (ResponseData.data.features.Count > 0)
        //            {
        //                res.Code = "00";
        //                res.Message = "Thành công.";
        //            }
        //            else
        //            {
        //                res.Code = "01";
        //                res.Message = "Không có dữ liệu.";
        //            }
        //        }
        //        else
        //        {
        //            res.Code = "98";
        //            res.Message = "Có lỗi phân tích địa chỉ, vui lòng nhập thủ công các trường thông tin địa chỉ.";
        //            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("VNPRepository.Search: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
        //        }
        //    }
        //    else
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("VNPRepository.Search: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
        //    }
        //    return res;
        //}

        public static ZResponse VNPSignaruteAndImageInformation(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZVNPSignaruteAndImageInformationRequest>(data);
                var callAPI = APILibraryRepository.InvokeVNP(new ZVNPRequest()
                {
                    Method = "GET",
                    RequestURL = APILibraryRepository.VNPImageUrl,
                    RequestURI = string.Format("Partner/VIB/GetImageAuthen?itemCode={0}", req.itemCode),
                    APIKeyName = "API_KEY",
                    APIKeyValue = APILibraryRepository.VNPImageKey
                });
                if (callAPI.Code == "00")
                {
                    var resVNP = JsonConvert.DeserializeObject<ZVNPResponse>(callAPI.Data);
                    if (resVNP.Code == "00")
                    {
                        res.Code = resVNP.Code;
                        res.Message = resVNP.Message;
                        res.Data = JsonConvert.SerializeObject(resVNP.ListValue);
                    }
                    else
                    {
                        res.Code = resVNP.Code;
                        res.Message = resVNP.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("VNPRepository.VNPSignaruteAndImageInformation: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("VNPRepository.VNPSignaruteAndImageInformation: request={0}||exception={1}", data, ex.Message));
            }
            return res;
        }

        public static ZResponse VNPPickupImageInformation(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZVNPSignaruteAndImageInformationRequest>(data);
                var callAPI = APILibraryRepository.InvokeVNP(new ZVNPRequest()
                {
                    Method = "GET",
                    RequestURL = APILibraryRepository.VNPImageUrl,
                    RequestURI = string.Format("Partner/VNP/GetPickupInformation?itemCode={0}", req.itemCode),
                    APIKeyName = "API_KEY",
                    APIKeyValue = APILibraryRepository.VNPImageKey
                });
                if(callAPI.Code == "00")
                {
                    var resVNP = JsonConvert.DeserializeObject<ZVNPResponse>(callAPI.Data);
                    if(resVNP.Code == "00")
                    {
                        res.Code = resVNP.Code;
                        res.Message = resVNP.Message;
                        res.Data = resVNP.Data;
                    }
                    else
                    {
                        res.Code = resVNP.Code;
                        res.Message = resVNP.Message;
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("VNPRepository.VNPPickupImageInformation: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                    }
                }
                else
                {
                    res = callAPI;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("VNPRepository.VNPSignaruteAndImageInformation: request={0}||exception={1}", data, ex.Message));
            }
            return res;
        }

    }
}