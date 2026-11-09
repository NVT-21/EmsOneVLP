using EE.PartnerGetData.API.Data;
using EE.PartnerGetData.API.Models;
using EE.PartnerGetData.API.Models.EMSOne;
using EE.PartnerGetData.API.Models.EMSOneTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Service
{
    public class EMSOneService
    {
        #region GetDataForControlFreight
        public static ResponseEntity GetDataForControlFreight(EMSOneGetDataForControlFreightRequest request)
        {
            return EMSOneRepository.GetDataForControlFreight(request);
        }
        #endregion

        #region SyncData
        public static ResponseEntity SyncData(EESyncDataRequest request)
        {
            request.Data.TableName = request.Name;
            return EMSOneRepository.SyncData(request.Data);
        }

        public static ResponseEntity SyncDataV2(EESyncDataRequest request)
        {
            request.Data.TableName = request.Name;
            return EMSOneRepository.SyncDataV2(request.Data);
        }
        #endregion

        #region DataEMSOneCustomer
        public static ResponseEntity DataEMSOneCustomer(EMSOneTrackingDataEMSOneCustomerRequest request)
        {
            return EMSOneRepository.DataEMSOneCustomer(request);
        }
        #endregion

        #region TrackingSyncData
        public static ResponseEntity TrackingSyncData(EMSOneTrackingSyncDataRequest request)
        {
            return EMSOneRepository.TrackingSyncData(request);
        }
        #endregion

        #region GetFilesUpdate
        public static ResponseEntity GetFilesUpdate()
        {
            return EMSOneRepository.GetFilesUpdate();
        }
        #endregion
    }
}