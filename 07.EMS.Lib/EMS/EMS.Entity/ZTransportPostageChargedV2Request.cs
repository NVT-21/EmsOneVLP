using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportPostageChargedV2Request
    {
        public string MainServiceCode { get; set; }
        public string ExtraServiceCodes { get; set; }
        public int FromProvinceID { get; set; }
        public int ToProvinceID { get; set; }
        public int ToDistrictID { get; set; }
        public int ToWardID { get; set; }
        public int Weight { get; set; }
        public string CustomerCode { get; set; }
        public int Quantity { get; set; }
        public long Amount { get; set; }
        public long COD { get; set; }
    }
    public class ZTransportPostageChargedV2Response
    {
        public long MainFee { get; set; }
        public long ExtraFee { get; set; }
        public long Surcharge { get; set; }
        public long VATOfMainFee { get; set; }
        public long VATOfExtraFee { get; set; }
        public long VATOfSurcharge { get; set; }
        public long TotalAmount { get; set; }
    }
}
