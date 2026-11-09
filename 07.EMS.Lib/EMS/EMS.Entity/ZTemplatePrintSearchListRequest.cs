using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTemplatePrintSearchListRequest
    {
        public string PrintType { get; set; }
        public string TemplateType { get; set; }
        public int ShopID { get; set; }
    }
    public class ZTemplatePrintSearchListResponse
    {
        public int TemplatePrintID { get; set; }
        public string Title { get; set; }
        public int TemplatePrintParent { get; set; }
        public string PrintType { get; set; }
        public int QuantityChild { get; set; }
        public string IsChoose { get; set; }
    }
}
