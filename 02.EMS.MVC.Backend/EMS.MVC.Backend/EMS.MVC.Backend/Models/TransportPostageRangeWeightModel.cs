using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageRangeWeightModel
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int PostageID { get; set; }
        public int FromRange { get; set; }
        public int FromRangeValue { get; set; }
        public int ToRange { get; set; }
        public int ToRangeValue { get; set; }
        public string TwoWay { get; set; }
        public TransportPostageWeight TransportPostageWeight { get; set; }
    }
    public class TransportPostageWeight
    {
        public int FromWeight { get; set; }
        public int ToWeight { get; set; }
        public int Unit { get; set; }
        public long Value { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
    }

}