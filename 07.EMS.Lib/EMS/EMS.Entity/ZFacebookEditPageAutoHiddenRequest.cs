using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookEditPageAutoHiddenRequest
    {
        public int ID { get; set; }
        public int IsAll { get; set; }
        public string Text { get; set; }
        public int IsEmail { get; set; }
        public int IsPhone { get; set; }
        public int IsText{ get; set; }
        public int AvailableDate{ get; set; }
        public int UpdatedBy { get; set; }
    }
}
