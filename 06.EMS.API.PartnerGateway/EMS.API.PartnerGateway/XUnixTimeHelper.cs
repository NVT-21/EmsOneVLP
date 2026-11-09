using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.PartnerGateway
{
    public static class XUnixTimeHelper
    {
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetCurrentUnixTimeSeconds()
        {
            return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
        }

        public static long ToUnixTimeSeconds(DateTime dateTimeUtc)
        {
            return (long)(dateTimeUtc - UnixEpoch).TotalSeconds;
        }
    }
}