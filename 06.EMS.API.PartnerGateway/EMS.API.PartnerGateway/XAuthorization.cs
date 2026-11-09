using EMS.API.PartnerGateway.Repositories;
using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EMS.API.PartnerGateway
{
    public class XAuthorization : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isValidAuthorization = false;
            var path = request.RequestUri.AbsolutePath.ToLower();
            // 👉 BỎ QUA ExecuteV2
            if(path.EndsWith("/executev2"))
            {
                var rawBody = await request.Content.ReadAsStringAsync();

                // Cho phép đọc lại body cho controller
                request.Content = new StringContent(
                    rawBody,
                    Encoding.UTF8,
                    "application/json");

                // 2. Parse request
                var req = JsonConvert.DeserializeObject<ZRequestV2>(rawBody);
                if(req == null)
                    return request.CreateResponse(HttpStatusCode.BadRequest, new ZResponse
                    {
                        Code = "400",
                        Message = "BadRequest"
                    });

                // 3. BỎ QUA LOGIN
                if(req.Code == "PARTNER_GET_ACCESS")
                    return await base.SendAsync(request, cancellationToken);

                // 4. Validate JWT
                var token = request.Headers.Authorization?.Parameter;
                if(string.IsNullOrEmpty(token))
                    return request.CreateResponse(HttpStatusCode.Unauthorized, new ZResponse
                    {
                        Code = "401",
                        Message = "Unauthorized"
                    });


                var resShopInfor = ShopRepository.SearchByAccessToken(token);
                if(resShopInfor.Code != "00")
                {
                    return request.CreateResponse(HttpStatusCode.Unauthorized, new ZResponse
                    {
                        Code = resShopInfor.Code,
                        Message = resShopInfor.Message
                    });
                }

                var shopDetail = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(resShopInfor.Data);

                if(shopDetail.CustomerToken != req.PublicKey)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, new ZResponse
                    {
                        Code = "400",
                        Message = "Token không trùng khớp với PublicKey"
                    });
                }

                // 7. Verify SIGNATURE (DÙNG THƯ VIỆN SECURITY)
                string error;
                bool isVerity = XSecurity.ApiVerify(
                    req.Code,
                    req.Data,
                    token,
                    req.TimeStamp,
                    req.Signature,
                    req.PublicKey,
                    out error
                );

                if(!isVerity)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, new ZResponse
                    {
                        Code = "99",
                        Message = "Chữ ký số không hợp lệ"
                    });
                }
                 

                // 8. OK → cho request đi tiếp
                return await base.SendAsync(request, cancellationToken);
            }

            var UserAuthorization = HttpContext.Current.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(UserAuthorization))
            {
                var SystemAuthorization = string.Format("Bearer {0}", XHelper.EMSOneAuthorizationKey);
                if (UserAuthorization == SystemAuthorization)
                {
                    isValidAuthorization = true;
                }
            }

            //If the key is not valid, return an http status code.
            if (!isValidAuthorization)
                return request.CreateResponse(HttpStatusCode.Unauthorized);

            //Allow the request to process further down the pipeline
            return await base.SendAsync(request, cancellationToken);
        }
    }
}