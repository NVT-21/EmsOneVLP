using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;

namespace EMS.MVC.Frontend
{
    public static class CaptchaHelper
    {
        private const string VerifyUrl = "https://challenges.cloudflare.com/turnstile/v0/siteverify";

        public static string SiteKey
        {
            get { return (ConfigurationManager.AppSettings["TURNSTILE_SITE_KEY"] ?? "").Trim(); }
        }

        public static bool Verify(string token, string remoteIp, out string message)
        {
            message = "Xác thực captcha thất bại. Vui lòng thử lại.";
            var secret = (ConfigurationManager.AppSettings["TURNSTILE_SECRET_KEY"] ?? "").Trim();
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(SiteKey))
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CaptchaHelper.Verify: thiếu TURNSTILE_SITE_KEY hoặc TURNSTILE_SECRET_KEY");
                message = "Hệ thống chưa cấu hình captcha.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                message = "Vui lòng xác nhận captcha.";
                return false;
            }

            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                using (var client = new WebClient())
                {
                    var form = new NameValueCollection
                    {
                        { "secret", secret },
                        { "response", token.Trim() },
                        { "remoteip", remoteIp ?? "" }
                    };
                    var body = Encoding.UTF8.GetString(client.UploadValues(VerifyUrl, "POST", form));
                    var result = JsonConvert.DeserializeObject<TurnstileVerifyResponse>(body);
                    if (result != null && result.Success)
                    {
                        message = null;
                        return true;
                    }

                    var errors = result != null && result.ErrorCodes != null
                        ? string.Join(",", result.ErrorCodes)
                        : body;
                    XHelper.LogToFile(LogFileType.EXCEPTION, "CaptchaHelper.Verify: " + errors);
                    return false;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CaptchaHelper.Verify: " + ex.Message);
                message = "Không xác thực được captcha. Vui lòng thử lại.";
                return false;
            }
        }

        private class TurnstileVerifyResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public string[] ErrorCodes { get; set; }
        }
    }
}
