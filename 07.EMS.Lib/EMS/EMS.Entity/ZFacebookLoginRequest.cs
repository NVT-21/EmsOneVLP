using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookLoginRequest
    {
        public string FBID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }       
        public string BirthDay { get; set; }
        public string HomeTown { get; set; }
        public string Link { get; set; }
        public int ShopID { get; set; }
        public string Channel { get; set; }
        public int CreatedBy { get; set; }
    }
}
