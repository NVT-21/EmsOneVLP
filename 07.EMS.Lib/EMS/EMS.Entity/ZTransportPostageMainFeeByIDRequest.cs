using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    //Bỏ
    public class ZTransportPostageMainFeeByIDRequest
    {
        public int PostageID { get; set; }
    }
    public class ZTransportPostageMainFeeByIDResponse
    {
        public int PostageID { get; set; }
        public string PostageCode { get; set; }
        public string PostageName { get; set; }
        public int MainFeeRange { get; set; }
        public string MainFeeVAT { get; set; }
        public string Status { get; set; }
        public int UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
