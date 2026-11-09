using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageDetailEditModel
    {
        public int ID { get; set; }
        public double Weight { get; set; }
        public int LocalFee { get; set; }
        public int IntercityFee1 { get; set; }
        public int IntercityFee2 { get; set; }
        public int IntercityFee3 { get; set; }
        public int IntercityFee4 { get; set; }
        public int IntercityFee5 { get; set; }
    }
}