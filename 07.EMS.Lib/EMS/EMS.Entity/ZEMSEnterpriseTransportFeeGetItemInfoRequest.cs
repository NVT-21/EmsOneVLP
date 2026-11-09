using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEMSEnterpriseTransportFeeGetItemInfoRequest
    {
        public string CustomerCode { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }
    }
    public class ZEMSEnterpriseTransportFeeGetItemInfoResponse
    {
        public string CustomerCode { get; set; }
        public string Code { get; set; }
        public string LadingCode { get; set; }
        public string ReleaseTime { get; set; }
        public long Freight { get; set; }
        public string AdditionService { get; set; }
        public string SpecialService { get; set; }
    }

    public class ZEMSEnterpriseTransportFeeReturnResultRequest
    {
        public string CustomerCode { get; set; }
        public string ListCode { get; set; }
        public string EmsBarCode { get; set; }
        public string TimeRelease { get; set; }
        public long TransportFee { get; set; }
        public long ServiceCode { get; set; }
        public string ReasonReject { get; set; }
    }

    public class ZEMSEnterpriseCancelParcelReleaseRequest
    {
        public string CustomerCode { get; set; }
        public string EmsBarCode { get; set; }
        public string ReasonReject { get; set; }
    }
    public class ZEMSEnterpriseInfoPaymentTransportFeeRequest
    {
        public string CustomerCode { get; set; }
    }
    public class ZEMSEnterpriseInfoPaymentTransportFeeResponse
    {
        public string CustomerCode { get; set; }
        public string EmsBarCode { get; set; }
        public string TimeRelease { get; set; }
        public long Mass { get; set; }
        public string ServiceCode { get; set; }
        public long TransportFee { get; set; }
    }
    public class ZEMSEnterpriseInfoPaymentTransportFeeReturnRequest
    {
        public string CustomerCode { get; set; }
        public string EmsBarCode { get; set; }
        public string PaymentStatus { get; set; }
    }
}
