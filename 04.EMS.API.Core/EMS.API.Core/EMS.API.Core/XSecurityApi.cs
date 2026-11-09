using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Claims;
using EMS.Core;
using EMS.Library;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Core
{
    public static class XSecurityApi
    {
        public static ZResponse GenerateToken(string MobileNumber, string CrmOrPayPostCode, int ShopID, int EmployeeId, string PublicKey)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim("MobileNumber", MobileNumber),
                    new Claim("Sub", MobileNumber),
                    new Claim("Jti", Guid.NewGuid().ToString()),
                    new Claim("ShopID", ShopID.ToString()),
                    new Claim("EmployeeId", EmployeeId.ToString()),
                    new Claim("CrmOrPayPostCode", CrmOrPayPostCode.ToString()),

                };

                var keyBytes = Encoding.UTF8.GetBytes(XHelper.EMSOneSecretKey);
                var securityKey = new InMemorySymmetricSecurityKey(keyBytes);

                var credentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256Signature,
                    SecurityAlgorithms.Sha256Digest
                );

                var issuedAtUtc = DateTime.UtcNow;


                var expiresAtUtc = issuedAtUtc.AddHours(Convert.ToInt32(XHelper.EMSOneExpiryHours));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    TokenIssuerName = XHelper.EMSOneIssuer,
                    Lifetime = new Lifetime(issuedAtUtc, expiresAtUtc),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                var res = new XTokenModel
                {
                    AccessToken = tokenString,
                    ExpiresAt = expiresAtUtc // Giờ Việt Nam
                };
                return new ZResponse
                {
                    Code = "00",
                    Message = "Thành công",
                    Data = JsonConvert.SerializeObject(res)
                };
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, $"SecurityApi.GenerateToken: {ex.Message}");
                return new ZResponse
                {
                    Code = "99",
                    Message = "Tạo token thất bại"
                };
            }
        }
    }
}