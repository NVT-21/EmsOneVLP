using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZohoSendMailRequest
    {
        public string bounce_address { get; set; }
        public ZZohoSendMailFromRequest from { get; set; }
        public List<ZZohoSendMailToRequest> to { get; set; }
        public string subject { get; set; }
        public string htmlbody { get; set; }
    }
    public class ZZohoSendMailFromRequest
    {
        public string address { get; set; }
        public string name { get; set; }
    }
    public class ZZohoSendMailToRequest
    {
        public ZZohoSendMailFromRequest email_address {get;set;}
    }

    public class ZZohoSendMailResponse
    {
        public List<ZZohoSendMailDataResponse> data { get; set; }
        public string message { get; set; }
        public string request_id { get; set; }
        public ZZohoSendMailErrorResponse error { get; set; }
    }
    public class ZZohoSendMailDataResponse
    {
        public string code { get; set; }
        public string message { get; set; }
    }
    public class ZZohoSendMailErrorResponse
    {
        public string code { get; set; }
        public List<ZZohoSendMailDetailResponse> details { get; set; }
    }
    public class ZZohoSendMailDetailResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public string target { get; set; }
    }
}
