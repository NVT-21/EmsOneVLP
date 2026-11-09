using EE.PartnerGetData.API.Models;
using EE.PartnerGetData.API.Models.EMSOne;
using EE.PartnerGetData.API.Models.EMSOneTracking;
using EE.PartnerGetData.API.Service;
using EE.PartnerGetData.API.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EE.PartnerGetData.API.Controllers
{
    //[RoutePrefix("api/EMSOne")]
    public class EMSOneController : ApiController
    {
        /// <summary>
        /// Đối soát cước vận chuyển
        /// </summary>
        #region GetDataForControlFreight
        public HttpResponseMessage GetDataForControlFreight(EMSOneGetDataForControlFreightRequest request)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, EMSOneService.GetDataForControlFreight(request));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        #endregion

        #region EESyncData
        public HttpResponseMessage EESyncData(EESyncDataRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, EMSOneService.SyncData(request));
        }
        #endregion

        #region EESyncDataV2
        public HttpResponseMessage EESyncDataV2(EESyncDataRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, EMSOneService.SyncDataV2(request));
        }
        #endregion

        #region DataEMSOneCustomer
        public HttpResponseMessage DataEMSOneCustomer(EMSOneTrackingDataEMSOneCustomerRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, EMSOneService.DataEMSOneCustomer(request));
        }
        #endregion

        #region TrackingSyncData
        public HttpResponseMessage TrackingSyncData(EMSOneTrackingSyncDataRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, EMSOneService.TrackingSyncData(request));
        }
        #endregion

        #region GetFilesUpdate
        public HttpResponseMessage GetFilesUpdate()
        {
            return Request.CreateResponse(HttpStatusCode.OK, EMSOneService.GetFilesUpdate());
        }
        #endregion
    }
}
