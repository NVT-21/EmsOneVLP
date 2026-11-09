using EMS.API.SyncGateWay.Repositories;
using EMS.Entity;
using EMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.API.SyncGateWay.Controllers
{
    [RoutePrefix("api/Helper")]
    public class HelperController : ApiController
    {
        #region REQUEST_SUPPORT
        [Route(CommandCodeConst.CMS_ADD_REQUEST_SUPPORT)]
        public ZCMSAddRequestSupportResponse CMS_REQUEST_SUPPORT(ZCMSAddRequestSupportRequest req)
        {
            return new ZCMSAddRequestSupportResponse();
        }
        #endregion

        #region ZOHO
        [Route(CommandCodeConst.ZOHO_ACCESS_TOKEN_GET)]
        public ZApiZohoTokenRefreshResponse ZOHO_ACCESS_TOKEN_GET()
        {
            return new ZApiZohoTokenRefreshResponse();
        }

        [Route(CommandCodeConst.ZOHO_LEADS_PHONE_GET)]
        public ZApiZohoGetLeadsResponse ZOHO_LEADS_PHONE_GET(ZApiZohoCRMLeadsRequest req)
        {
            return new ZApiZohoGetLeadsResponse();
        }

        [Route(CommandCodeConst.ZOHO_LEADS_EMAIL_GET)]
        public ZApiZohoGetLeadsResponse ZOHO_LEADS_EMAIL_GET(ZApiZohoCRMLeadsRequest req)
        {
            return new ZApiZohoGetLeadsResponse();
        }

        [Route(CommandCodeConst.ZOHO_LEADS_ADD)]
        public ZApiZohoCreatResponse ZOHO_LEADS_ADD(ZZohoCRMCreateLeadsRequest req)
        {
            return new ZApiZohoCreatResponse();
        }

        #endregion

        #region PNS
        [Route(CommandCodeConst.PNS_EMS_PICKUP_CREATE)]
        public ZApiResponse PNS_EMS_PICKUP_CREATE(ZTransportInfoShippingApiRequest req)
        {
            return new ZApiResponse();
        }
        #endregion
    }
}