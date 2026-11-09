using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class PromotionViewModel
    {
        public int ID { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public string AllowUse { get; set; }
        public int PromotionTypeOption { get; set; }
        public string PromotionTypeOptionName { get; set; }
        public int Reduction { get; set; }
        public int MaxReduction { get; set; }
        public int ApplyForOption { get; set; }
        public string ApplyForOptionName { get; set; }
        public string AfoChild { get; set; }
        public string AfoChildName { get; set; }
        public int AfoCoditionsApply { get; set; } //Apply For Option Coditions Apply
        public string AfoCoditionsApplyName { get; set; } //Apply For Option Coditions Apply
        public int AfoCaValue { get; set; } //Apply For Option coditions Apply
        public int NumberOfUse { get; set; }
        public string NumberOfUseName { get; set; }
        public int NumberOfCustomerUse { get; set; }
        public string NumberOfCustomerUseName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int SubjectsOfApplication { get; set; }
        public string SubjectsOfApplicationName { get; set; }
        public int ScopeOfUse { get; set; }
        public string ScopeOfUseName { get; set; }
        public string IsLock { get; set; }
        public string CreatedDate { get; set; }
        public string EmployeeName { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductName { get; set; }
        public string CustomerGroupName { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string PromotionCodeFirstName { get; set; }
        public string PromotionCodeEndName { get; set; }
    }
}