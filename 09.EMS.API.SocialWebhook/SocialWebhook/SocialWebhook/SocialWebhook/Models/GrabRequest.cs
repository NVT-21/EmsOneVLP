using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Models
{
    public class GrabRequest
    {
        [JsonProperty("tracking_code")]
        public string TrackingCode { get; set; }
        [JsonProperty("order_code")]
        public string OrderCode { get; set; }
        [JsonProperty("status_code")]
        public string StatusCode { get; set; }
        [JsonProperty("status_name")]
        public string StatusName { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("locate")]
        public string Locate { get; set; }
        [JsonProperty("datetime")]
        public string DateTime { get; set; }
        [JsonProperty("total_fee")]
        public long TotalFee { get; set; }
        [JsonProperty("partner_code")]
        public string PartnerCode { get; set; }
        [JsonProperty("distance")]
        public long Distance { get; set; }
        [JsonProperty("driver_name")]
        public string DriverName { get; set; }
        [JsonProperty("driver_phone")]
        public string DriverPhone { get; set; }
        [JsonProperty("driver_license_plate")]
        public string DriverLicensePlate { get; set; }
    }
}