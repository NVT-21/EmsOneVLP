using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPromotionGetByCodeRequest
    {
        public int ShopID { get; set; }
        public string Code { get; set; }
    }
    public class ZPromotionGetByCodeResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int PromotionTypeOption { get; set; }
        public string AllowUse { get; set; }
        public string Reduction { get; set; }
        public string MaxReduction { get; set; }
        public int ApplyForOption { get; set; }
        public int AfoChild { get; set; }
        public int AfoCoditionsApply { get; set; }
        public int AfoCaValue { get; set; }
        public int NumberOfUse { get; set; }
        public int SubjectsOfApplication { get; set; }
        public int ScopeOfUse { get; set; }
    }
}
