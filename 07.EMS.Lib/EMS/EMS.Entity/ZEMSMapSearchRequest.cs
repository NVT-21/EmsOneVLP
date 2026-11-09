using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEMSMapSearchRequest
    {
        public string AddressFull { get; set; }
    }
    public class ZVNPOSTMapSearchRequest
    {
        public int direction { get; set; } = 2;
        public string provinceCode { get; set; } 
        public string wardCode { get; set; }
        public int territory { get; set; } = 2;
        public string inputText { get; set; }
    }
    public class ZEMSMapSearchResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public ZEMSMapSearchDataResponse data { get; set; }
    }
    public class ZEMSMapSearchDataResponse
    {
        public string Province_Code { get; set; }
        public string Province_Code_2017 { get; set; }
        public string Province_Name { get; set; }
        public string District_Code { get; set; }
        public string District_Code_2017 { get; set; }
        public string District_Name { get; set; }
        public string Ward_Code { get; set; }
        public string Ward_Code_2017 { get; set; }
        public string Ward_Name { get; set; }
        public string Label { get; set; }
    }
    public class ZVNPOSTMapSearchResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public List<ZVNPOSTMapSearchDataResponse> data { get; set; } = new List<ZVNPOSTMapSearchDataResponse>();
    }
    public class ZVNPOSTMapSearchDataResponse
    {
        public string provinceCodeOld { get; set; }
        public string provinceNameOld { get; set; }
        public string districtCodeOld { get; set; }
        public string districtNameOld { get; set; }
        public string wardCodeOld { get; set; }
        public string wardNameOld { get; set; }
        public string subAddressName { get; set; }
    }
}
