using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Library
{
    public static class DetailUrlHelper
    {
        // Case 1
        private const string PnsOldPrefix = "https://pns.vnpost.vn/";
        //private const string PnsNewPrefix = "https://localhost:7093/DataFile/VNPOST/";
        private const string PnsNewPrefix = "https://gatewayapi.ems.com.vn/DataFile/VNPOST/";

        // Case 2
        private const string EmsOldPrefix = "https://api-dingdong-v3.ems.com.vn";
        //private const string EmsNewPrefix = "https://localhost:7093/DataFile/EMS/";
        private const string EmsNewPrefix = "https://gatewayapi.ems.com.vn/DataFile/EMS/";

        // Case 3
        private const string VNEmsOldPrefix = "https://vnems.vn";
        //private const string VNEmsNewPrefix = "https://localhost:7093/DataFile/VNEMS/";
        private const string VNEmsNewPrefix = "https://gatewayapi.ems.com.vn/DataFile/VNEMS/";

        /// <summary>
        /// Convert URL theo 2 rule:
        /// - pns.vnpost.vn  => /DataFile/DetailImg/{b64url(path)}
        /// - api-dingdong   => /DataFile/EMS/{b64url(path)}
        /// Strict: chỉ xử lý đúng prefix, không fallback đoán thư mục.
        /// </summary>
        public static string ConvertOne(string inputUrl,string User)
        {
            if(string.IsNullOrWhiteSpace(inputUrl))
                return inputUrl;

            var s = inputUrl.Trim();

            // Normalize case pns has double slash after domain
            if(s.StartsWith("https://pns.vnpost.vn//", StringComparison.OrdinalIgnoreCase))
                s = "https://pns.vnpost.vn/" + s.Substring("https://pns.vnpost.vn//".Length);

            // Case 1: PNS
            if(s.StartsWith(PnsOldPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var tail = s.Substring(PnsOldPrefix.Length);          // e.g. "Assets_backup/..."
                var path = "/" + tail.TrimStart('/');                 // must start with "/"
                var b64 = Base64UrlEncode(path);
                return PnsNewPrefix + b64 + "/" + User;
            }

            // Case 2: EMS (old prefix does NOT include trailing slash by your requirement)
            if(s.StartsWith(EmsOldPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var tail = s.Substring(EmsOldPrefix.Length);          // e.g. "/IMAGE//captures/....jpg"
                var path = "/" + tail.TrimStart('/');                 // normalize to single leading "/"
                var b64 = Base64UrlEncode(path);
                return EmsNewPrefix + b64 + "/" + User;
            }

            // Case 3: VNEMS (old prefix does NOT include trailing slash by your requirement)
            if(s.StartsWith(VNEmsOldPrefix, StringComparison.OrdinalIgnoreCase))
            {

                var tail = s.Substring(VNEmsOldPrefix.Length);          // e.g. "/IMAGE//captures/....jpg"
                var path = "/" + tail.TrimStart('/');                 // normalize to single leading "/"
                var b64 = Base64UrlEncode(path);
                return VNEmsNewPrefix + b64 + "/" + User;
            }

            throw new ArgumentException("URL does not match any supported prefix.", nameof(inputUrl) + "|" + inputUrl);
        }

        public static List<string> ConvertAll(List<string> urls, string User)
        {
            if(urls == null) return null;

            var result = new List<string>(urls.Count);
            for(int i = 0; i < urls.Count; i++)
                result.Add(ConvertOne(urls[i],User));

            return result;
        }

        private static string Base64UrlEncode(string plain)
        {
            var bytes = Encoding.UTF8.GetBytes(plain);
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
