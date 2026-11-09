using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    //Bỏ
    public class ZTransportPostageSurchargeByIDRequest
    {
        public int PostageID { get; set; }
    }
    public class ZTransportPostageSurchargeByIDResponse
    {
        public int PostageID { get; set; }
        public string PostageCode { get; set; }
        public string PostageName { get; set; }
        public string SurchargeVAT { get; set; }
        public string Status { get; set; }
        public XValueModel Fuel { get; set; } //Xăng dầu
        public XValueModel RemoteArea { get; set; } //Vùng xa
        public XValueModel IslandsArea { get; set; } //Vùng hải đảo
        public int UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
