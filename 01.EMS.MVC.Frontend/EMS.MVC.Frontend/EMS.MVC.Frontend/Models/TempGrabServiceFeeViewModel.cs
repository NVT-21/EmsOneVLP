using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class TempGrabServiceFeeViewModel
    {
        public double Distance { get; set; }
        public int TotalFreight { get; set; }
        public string Message { get; set; }
        public string EstimatedTimeLinePickup { get; set; }
        public string EstimatedTimeLineDropoff { get; set; }
        public string EstimatedTimeLineCompleted { get; set; }
    }
}