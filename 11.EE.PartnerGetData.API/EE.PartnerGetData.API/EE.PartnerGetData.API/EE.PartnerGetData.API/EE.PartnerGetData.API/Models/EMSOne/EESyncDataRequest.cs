using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOne
{
    public class EESyncDataRequest
    {
        public string Name { get; set; }
        public DataTable Data { get; set; }
    }
}