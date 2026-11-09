using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class OtpRepository
    {
        public static ZResponse VerifyCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOtpVerifyCodeRequest>(data);
            return XHelper.Invoke("POST", "Otp/VerifyCode", req);
        }
    }
}