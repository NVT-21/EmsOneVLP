using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace EMS.API.PartnerGateway
{
    public static class XSecurity
    {
        public static string HmacSha256(string data, string secretKey)
        {
            using(var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash)
                    .Replace("-", "")
                    .ToLowerInvariant();
            }
        }
        public static string BuildCanonicalLogin(
            string data,
            string publicKey,
            long timestamp)
        {
            return $"{data}|{publicKey}|{timestamp}";
        }

        // API: code | data | token | timestamp
        public static string BuildCanonicalApi(
            string code,
            string data,
            string token,
            string publicKey,
            long timestamp)
        {
            return $"{code}|{data}|{token}|{publicKey}|{timestamp}";
        }

        public static bool TimeStampIsValid(long timestamp, int allowedSeconds = 300)
        {
            var now = XUnixTimeHelper.GetCurrentUnixTimeSeconds();
            return Math.Abs(now - timestamp) <= allowedSeconds;
        }

        public static bool LoginVerify(
            string data,
            string publicKey,
            long timestamp,
            string clientSignature,
            string secretKey,
            out string error)
        {
            error = null;

            if(!TimeStampIsValid(timestamp))
            {
                error = "Timestamp expired";
                return false;
            }

            var canonical = BuildCanonicalLogin(
                data, publicKey, timestamp);

            var serverSignature = HmacSha256(
                canonical, secretKey);

            if(!serverSignature.Equals(
                    clientSignature,
                    StringComparison.OrdinalIgnoreCase))
            {
                error = "Invalid login signature";
                return false;
            }

            return true;
        }

        public static bool ApiVerify(
            string code,
            string data,
            string token,
            long timestamp,
            string clientSignature,
            string publicKey,
            out string error)
        {
            error = null;

            if(!TimeStampIsValid(timestamp))
            {
                error = "Timestamp expired";
                return false;
            }

            var canonical = BuildCanonicalApi(
                code, data, token,publicKey, timestamp);

            var serverSignature = HmacSha256(
                canonical, XHelper.EMSOneSecretKey);

            if(!serverSignature.Equals(
                    clientSignature,
                    StringComparison.OrdinalIgnoreCase))
            {
                error = "Invalid API signature";
                return false;
            }

            return true;
        }
    }
}
