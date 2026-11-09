using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ReconciliationRepository
    {
        #region ListFee
        public static ZResponse ListTransportFee(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchListFeeRequest>(data);
            return XHelper.Invoke("POST", "Reconciliation/ListTransportFee", req);
        }
        public static ZResponse ListTransportFeePeriod(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchListFeeRequest>(data);
            return XHelper.Invoke("POST", "Reconciliation/ListTransportFeePeriod", req);
        }
        #endregion

        #region ListFeeTradeDiscounts
        public static ZResponse ListTransportFeeTradeDiscounts(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchListFeeRequest>(data);
            return XHelper.Invoke("POST", "Reconciliation/ListTransportFeeTradeDiscounts", req);
        }
        #endregion

        #region ListCOD
        public static ZResponse ListCOD(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchListCODRequest>(data);
            
            return XHelper.Invoke("POST", "Reconciliation/ListCOD", req);
        }
        public static ZResponse ListCODPeriod(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchListCODRequest>(data);
            return XHelper.Invoke("POST", "Reconciliation/ListCODPeriod", req);
        }
        #endregion

        #region UpdateAgree Fee - COD
        public static ZResponse UpdateAgreeTransportFee(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationUpdateAgreeFeeRequest>(data);
            return XHelper.Invoke("POST", "Reconciliation/UpdateAgreeTransportFee", req);
        }
        public static ZResponse UpdateAgreeCOD(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationUpdateAgreeCODRequest>(data);
            return XHelper.Invoke("POST", "Reconciliation/UpdateAgreeCOD", req);
        }
        #endregion

        #region ListControlPeriod
        public static ZResponse ListControlPeriod(string data)
        {
            var req = JsonConvert.DeserializeObject<ZControlPeriodRequest>(data);
            return XHelper.Invoke("POST", "Reconciliation/ListControlPeriod", req);
        }
        public static ZResponse ControlPeriodDateID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZControlPeriodDateRequest>(data);
            return XHelper.Invoke("POST", "Reconciliation/ControlPeriodDateID", req);
        }
        #endregion

        #region COD - FEE Follow
        public static ZResponse ListCODFollow(string data)
        {

            var req = JsonConvert.DeserializeObject<ZReconciliationSearchListCODFollowRequest>(data);
            if(XHelper.IsSqlInjection(req.CustomerCode) || XHelper.IsSqlInjection(req.CustomerPPCRM) || XHelper.IsSqlInjection(req.Status))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Reconciliation/ListCODFollow", req);
        }
        public static ZResponse ListFeeFollow(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchListFeeFollowRequest>(data);
            if(XHelper.IsSqlInjection(req.CustomerCode) || XHelper.IsSqlInjection(req.CustomerPPCRM) || XHelper.IsSqlInjection(req.Status))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Reconciliation/ListFeeFollow", req);
        }
        #endregion

        #region FEE REPORT
        public static ZResponse ListFeeReport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchFeeReportRequest>(data);
            if(XHelper.IsSqlInjection(req.TypeItem) || XHelper.IsSqlInjection(req.CustomerPPCRM) || XHelper.IsSqlInjection(req.PostOffice))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Reconciliation/ListFeeReport", req);
        }
        public static ZResponse ListFeePostOfficeReport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchFeePostOfficeReportRequest>(data);
            if(XHelper.IsSqlInjection(req.TypeItem) || XHelper.IsSqlInjection(req.CustomerPPCRM) || XHelper.IsSqlInjection(req.PostOffice))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Reconciliation/ListFeePostOfficeReport", req);
        }
        #endregion

        #region ListFeeAdjustingJournalEntries
        public static ZResponse ListFeeAdjustingJournalEntries(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationFeeAdjustingJournalEntriesRequest>(data);
            if(XHelper.IsSqlInjection(req.CustomerPPCRM) || XHelper.IsSqlInjection(req.AdjustingPostOffice))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Reconciliation/ListFeeAdjustingJournalEntries", req);
        }
        #endregion

        #region ListDetailEditedItemCode
        public static ZResponse ListDetailEditedItemCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationListDetailEditedItemCodeRequest>(data);
            if(XHelper.IsSqlInjection(req.CustomerPPCRM) || XHelper.IsSqlInjection(req.PostOffice))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Reconciliation/ListDetailEditedItemCode", req);
        }
        #endregion

        #region ListPostageRefund
        public static ZResponse ListPostageRefund(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchPostageRefundV1Request>(data);
            if(XHelper.IsSqlInjection(req.CustomerCode))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Reconciliation/ListPostageRefund", req);
        }
        #endregion

        #region ListDebtNotices
        public static ZResponse ListSummaryOfDebtNotices(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchDebtNoticesV1Request>(data);
            if(XHelper.IsSqlInjection(req.CustomerCode))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Reconciliation/ListSummaryOfDebtNotices", req);
        }
        public static ZResponse ListDebtNotices(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationSearchDebtNoticesV1Request>(data);
            if(XHelper.IsSqlInjection(req.CustomerCode))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Reconciliation/ListDebtNotices", req);
        }
        #endregion
    }
}