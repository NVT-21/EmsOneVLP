using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class EditSurchargeTabViewModel
    {
        public int TransportPostageID { get; set; }
        public string TransportPostageCode { get; set; }
        public string TransportPostageName { get; set; }
        public string SurchargeVAT { get; set; }
        public string Status { get; set; }
        public EditSurchargeFuelModel Fuel { get; set; } //Xăng dầu
        public EditSurchargeRemoteAreaModel RemoteArea { get; set; } //Vùng xa
        public EditSurchargeIslandsAreaModel IslandsArea { get; set; } //Vùng hải đảo
    }
    public class EditSurchargeFuelModel
    {
        public string Checked { get; set; }   // Y - N
        public int Unit { get; set; }     // 1(VNĐ) - 2(%)
        public double Value { get; set; }
        public double Percent { get; set; }
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
        public string InOfProvince { get; set; }
        public string OutOfProvince { get; set; }
    }
    public class EditSurchargeRemoteAreaModel
    {
        public string Checked { get; set; }   // Y - N
        public int Unit { get; set; }     // 1(VNĐ) - 2(%)
        public double Value { get; set; }
        public double Percent { get; set; }
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
        public string InOfProvince { get; set; }
        public string OutOfProvince { get; set; }
        public string IsFrom { get; set; }
    }
    public class EditSurchargeIslandsAreaModel
    {
        public string Checked { get; set; }   // Y - N
        public int Unit { get; set; }     // 1(VNĐ) - 2(%)
        public double Value { get; set; }
        public double Percent { get; set; }
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
        public string InOfProvince { get; set; }
        public string OutOfProvince { get; set; }
        public string IsFrom { get; set; }
    }
}