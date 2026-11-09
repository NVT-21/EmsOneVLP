using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class PaymentRepository
    {
        public static ZResponse ListPaymentCOD(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPaymentListCODRequest>(data);
            if(XHelper.IsSqlInjection(req.CustomerCode) || XHelper.IsSqlInjection(req.CustomerPPCRM) || XHelper.IsSqlInjection(req.Status) || XHelper.IsSqlInjection(req.PaymentStatus) || XHelper.IsSqlInjection(req.ApprovalDate) || XHelper.IsSqlInjection(req.OrderCode) || XHelper.IsSqlInjection(req.EmsBarCode))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Payment/ListPaymentCOD", req);
        }
        public static ZResponse UpdatePaymentCOD(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPaymentUpdateCODRequest>(data);
            return XHelper.Invoke("POST", "Payment/UpdatePaymentCOD", req);
        }
    }
}