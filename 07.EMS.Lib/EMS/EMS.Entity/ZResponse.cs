using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        //public int TotalData { get; set; }
        public string Time { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        public string Signature { get; set; }
    }
}
