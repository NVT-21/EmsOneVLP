using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using SocialWebhook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Repositories.Grab
{
    public class GrabRepository
    {
        public static void ReceiveStatus(object request)
        {
            var data = JsonConvert.DeserializeObject<GrabRequest>(JsonConvert.SerializeObject(request));
            ZGrabReceiveStatusRequest dataRequest = new ZGrabReceiveStatusRequest()
            {
                TrackingCode = data.TrackingCode,
                OrderCode = data.OrderCode,
                StatusCode = data.StatusCode.ToString(),
                StatusName = data.StatusName,
                PartnerCode = data.PartnerCode,
                DateTime = data.DateTime,
                Note = data.Note,
                Locate = data.Locate,
                TotalFee = data.TotalFee,
                Distance = data.Distance,
                DriverName = data.DriverName,
                DriverPhone = data.DriverPhone,
                DriverLicensePlate = data.DriverLicensePlate
            };
            var res = XHelpers.Invoke("POST", "Grab/ReceiveStatus", dataRequest);
            if (res.Code == "00")
            {
                //XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("GrabRepository.ReceiveStatus: api={0}||req={1}||res={2}", "Grab/ReceiveStatus", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(res)));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.EXCEPTION, string.Format("GrabRepository.ReceiveStatus: api={0}||req={1}||res={2}", "Grab/ReceiveStatus", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(res)));
            }
        }
    }
}