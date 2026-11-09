using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class SearchListPeriodViewModel
    {
        public string Type { get; set; }
        public int PeriodType { get; set; }
        public int ShopID { get; set; }
        public List<SelectListItem> ListAllDay { get; set; }
        public List<SelectListItem> ListAllWeek { get; set; }
        public List<SearchListPeriodDataViewModel> ListData { get; set; }
    }

    public class SearchListPeriodDataViewModel
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }
        public int ShopID { get; set; }
        public string Type { get; set; }
    }
}