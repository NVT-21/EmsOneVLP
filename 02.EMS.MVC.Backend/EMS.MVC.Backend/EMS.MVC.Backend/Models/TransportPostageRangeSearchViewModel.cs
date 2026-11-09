using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageRangeSearchViewModel
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int PostageID { get; set; }
        public int FromRange { get; set; }
        public string FromRangeName { get; set; }
        public int FromRangeValue { get; set; }
        public string FromRangeValueName { get; set; }
        public int ToRange { get; set; }
        public string ToRangeName { get; set; }
        public int ToRangeValue { get; set; }
        public string ToRangeValueName { get; set; }
        public string TwoWay { get; set; }
        public int Unit { get; set; }
        public long Value { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
    }
}