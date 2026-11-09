using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequest
    {
        public string Channel { get; set; }
        public string Version { get; set; }
        public string Code { get; set; }
        public string Data { get; set; }
        public string Time { get; set; }
        public string Signature { get; set; }
    }

    public class ZRequestV2
    {
        public string Code { get; set; }
        public string Data { get; set; }
        public long TimeStamp { get; set; }
        public string Signature { get; set; }
        public string PublicKey { get; set; }
    }
}