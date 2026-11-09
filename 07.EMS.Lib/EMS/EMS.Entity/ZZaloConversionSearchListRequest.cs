using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloConversionSearchListRequest
    {
        public string OfficialAccountID { get; set; }
        public int LimitRow { get; set; }
        public string UpdatedDate { get; set; }
    }
    public class ZZaloConversionSearchListResponse
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string SenderID { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string MessageText { get; set; }
        public int NumberOfMessage { get; set; }
        public string UpdatedDate { get; set; }
    }
}
