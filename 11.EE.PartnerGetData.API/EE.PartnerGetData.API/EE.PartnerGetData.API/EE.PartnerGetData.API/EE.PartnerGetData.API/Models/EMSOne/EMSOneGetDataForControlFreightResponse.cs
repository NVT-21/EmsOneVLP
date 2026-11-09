using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOne
{
    public class EMSOneGetDataForControlFreightResponse
    {
        public string CustomerCode { get; set; }
        public string Code { get; set; }
        public string LadingCode { get; set; }
        public string ReleaseTime { get; set; }
        public long Freight { get; set; }
        public string AdditionService { get; set; }
        public string SpecialService { get; set; }
    }
}