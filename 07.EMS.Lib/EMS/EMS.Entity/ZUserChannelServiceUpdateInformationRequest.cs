using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZUserChannelServiceUpdateInformationRequest
    {
        public int UserChannelID { get; set; }
        public string DisplayName { get; set; }
        public string BirthDay { get; set; }
        public string Avatar { get; set; }
        public int Gender { get; set; }
        public string Link { get; set; }
        public string IsFollower { get; set; }
    }
}
