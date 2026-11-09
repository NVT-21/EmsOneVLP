using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ReconciliationAgreeModel
    {
        public int ID { get; set; }
        public string EmsBarCode { get; set; }
        public string Agree { get; set; }
    }
}