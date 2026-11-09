using EMS.API.GateWay.Repositories;
using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.API.GateWay;
using EMS.Library;

namespace EMS.API.SyncGateWay.Repositories
{
    public class PNSRepository
    {
        //#region PNS
        //public static ZResponse CreatePickupEMS(string data)
        //{
        //    ZResponse _response = new ZResponse();
        //    var _dataResponse = APILibraryRepository.CallApiCreatePickup(data);
        //    if (_dataResponse.Code == "00")
        //    {
        //        var _dataPartner = JsonConvert.DeserializeObject<ZApiResponse>(_dataResponse.Data);
        //        if (_dataResponse.Code == "00")
        //        {
        //            _response.Code = _dataPartner.Code;
        //            _response.Message = _dataPartner.Message;
        //            _response.Data = _dataPartner.Value;
        //        }
        //        else
        //        {
        //            XHelper.LogToFile(LogFileType.MESSAGE, "PNSRepository.CreatePickupEMS: request= " + data + "|| response= " + JsonConvert.SerializeObject(_dataResponse.Data));
        //            _response.Code = _dataPartner.Code;
        //            _response.Message = _dataPartner.Message;
        //        }
        //    }
        //    else
        //    {
        //        _response.Code = "99";
        //        _response.Message = "Lỗi kết nối với API của đối tác, Vui lòng liên hệ quản trị.";
        //    }
        //    return _response;
        //}
        //public static ZResponse CancelPickupEMS(string data)
        //{
        //    ZResponse _response = new ZResponse();
        //    var _dataResponse = APILibraryRepository.CallApiShipmentCancel(data);
        //    if (_dataResponse.Code == "00")
        //    {
        //        var _dataPartner = JsonConvert.DeserializeObject<ZApiResponse>(_dataResponse.Data);
        //        if(_dataPartner.Code == "00")
        //        {
        //            _response.Code = "00";
        //            _response.Message = "Thành công";
        //        }
        //        else
        //        {
        //            XHelper.LogToFile(LogFileType.MESSAGE, "PNSRepository.CancelPickupEMS: request= " + data + "|| response= " + JsonConvert.SerializeObject(_dataResponse.Data));
        //            switch (_dataPartner.Code)
        //            {
        //                case "01":
        //                    _response.Code = "01";
        //                    _response.Message = "Không tìm thấy thông tin đơn hàng, vui lòng kiểm tra lại";
        //                    break;
        //                case "02":
        //                    _response.Code = "02";
        //                    _response.Message = "Đơn hàng đã phát hành không thể hủy, vui lòng kiểm tra lại";
        //                    break;
        //                case "102":
        //                    _response.Code = "102";
        //                    _response.Message = "Chữ ký xác thực không chính xác, vui lòng kiểm tra lại";
        //                    break;
        //                default:
        //                    _response.Code = "99";
        //                    _response.Message = "Mã lỗi không trùng khớp, Vui lòng liên hệ quản trị.";
        //                    break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        _response.Code = "99";
        //        _response.Message = "Lỗi kết nối với API của đối tác, Vui lòng liên hệ quản trị.";
        //    }
        //    return _response;
        //}
        //#endregion
    }
}