using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPromotionAddNewCodeRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public string PromotionCode { get; set; }
        public string AllowUse { get; set; }
        public int PromotionTypeOption { get; set; }
        public int Reduction { get; set; } //Promotion Type Option Reduction
        public int MaxReduction { get; set; } //Promotion Type Option Max Reduction
        public int ApplyForOption { get; set; } // AFO
        public string ApplyForOptionName { get; set; } // AFO Name
        public string AfoChild { get; set; } //Apply For Option Child
        public string AfoChildName { get; set; } //Apply For Option Child Name
        public int AfoCoditionsApply { get; set; } //Apply For Option Coditions Apply
        public string AfoCoditionsApplyName { get; set; } //Apply For Option Coditions Apply Name
        public int AfoCaValue { get; set; } //Apply For Option coditions Apply
        public int NumberOfUse { get; set; }
        public int NumberOfCustomerUse { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int SubjectsOfApplication { get; set; }
        public int ScopeOfUse { get; set; }
        public int CreatedBy { get; set; }
    }
}
