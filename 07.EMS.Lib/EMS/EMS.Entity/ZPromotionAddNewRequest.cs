using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPromotionAddNewRequest
    {
        public int ID { get; set; }
        public string PromotionName { get; set; }
        public int PromotionTypeOption { get; set; }
        public int Reduction { get; set; } //Promotion Type Option Reduction
        public int ApplyForOption { get; set; } // AFO
        public int AfoChild { get; set; } //Apply For Option Child
        public int AfoCoditionsApply { get; set; } //Apply For Option Coditions Apply
        public int AfoCaValue { get; set; } //Apply For Option coditions Apply
        public int NumberOfUse { get; set; }
        public int NumberOfCustomerUse { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int CreatedBy { get; set; }
        public int ShopID { get; set; }
    }
}
