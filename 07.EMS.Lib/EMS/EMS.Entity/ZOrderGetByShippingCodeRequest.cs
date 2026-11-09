using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOrderGetByShippingCodeRequest
    {
        public string ShippingCode { get; set; }
        public int ShopID { get; set; }
    }
    public class ZOrderGetByShippingCodeResponse
    {
        public string ReceiverPhone { get; set; }
        public int ReceiverProvinceID { get; set; }
        public string ReceiverProvinceName { get; set; }
        public int ReceiverDistrictID { get; set; }
        public string ReceiverDistrictName { get; set; }
        public int ReceiverWardID { get; set; }
        public string ReceiverWardName { get; set; }
        public string ReceiverAddress { get; set; }
        public int SenderProvinceID { get; set; }
        public string SenderProvinceName { get; set; }
        public int SenderDistrictID { get; set; }
        public string SenderDistrictName { get; set; }
        public int SenderWardID { get; set; }
        public string SenderWardName { get; set; }
        public string SenderAddress { get; set; }
        public string IsCheckCOD { get; set; }
        public long TotalProductValue { get; set; }
        public long TotalCOD { get; set; }
        
    }
}
