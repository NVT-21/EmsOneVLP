using System;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EMS.API.Core.Repositories
{
    public class PNSRepository
    {
        //#region Gửi yêu cầu vận chuyển qua PNS
        //public static ZResponse SendRequestTransport(ZTransportInfoShippingApiRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.InvokeSyncGateway(new ZRequest()
        //        {
        //            Code = "PNS_EMS_PICKUP_CREATE",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "PNSRepository.SendRequestTransport: " + ex.Message);
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //#endregion

        //#region Hủy yêu cầu vận chuyển qua PNS
        //public static ZResponse CancelRequestTransport(ZTransportShippingCancelDataRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.InvokeSyncGateway(new ZRequest()
        //        {
        //            Code = "PNS_EMS_PICKUP_CANCEL",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "PNSRepository.CancelRequestTransport: " + ex.Message);
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //#endregion
    }
}