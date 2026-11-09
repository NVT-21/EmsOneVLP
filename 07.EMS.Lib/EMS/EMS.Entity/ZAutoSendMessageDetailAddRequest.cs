using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZAutoSendMessageDetailAddRequest
    {
        public string MessageTitle { get; set; }
        public string MessageSubtitle { get; set; }
        public string ImageURL { get; set; }
        public string LinkURL { get; set; }
        public string FileToken { get; set; }
    }
}
