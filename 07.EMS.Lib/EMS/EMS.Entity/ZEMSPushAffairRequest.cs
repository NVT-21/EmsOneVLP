using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEMSPushAffairRequest
    {
        public string id_e1 { get; set; }
        public int ma_bc_khai_thac { get; set; }
        public string ma_e1 { get; set; }
        public int tien_cod { get; set; }
        public string dv_cong_them { get; set; }
    }
    public class ZEMSPushAffairResponse
    {
        public int Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ZEMSPushAffairListAffairResponse> listResponseAFFAIR { get; set; }
    }
    public class ZEMSPushAffairListAffairResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string ID_E1 { get; set; }
    }
}
