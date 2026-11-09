using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class CustomerRepository
    {
        public static ZResponse AddNew(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerAddNewRequest>(data);
            return XHelper.Invoke("POST", "Customer/AddNew", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerEditRequest>(data);
            return XHelper.Invoke("POST", "Customer/EditCustomer", req);
        }
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerSearchListRequest>(data);
            return XHelper.Invoke("POST", "Customer/SearchList", req);
        }
        public static ZResponse SearchListV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerSearchListRequest>(data);
            return XHelper.Invoke("POST", "Customer/SearchListV2", req);
        }
        public static ZResponse GetByCustomerCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerGetByCodeRequest>(data);
            return XHelper.Invoke("GET", "Customer/GetByCustomerCode?CustomerCode=" + req.CustomerCode, null);
        }
        public static ZResponse GetByMobileNumber(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerGetByMobileNumberRequest>(data);
            return XHelper.Invoke("POST", "Customer/GetByMobileNumber", req);
            //return XHelper.Invoke("GET", "Customer/GetByMobileNumber?MobileNumber=" + req.MobileNumber + "&ShopID=" + req.ShopID, null);
        }
        
        public static ZResponse AddNewGroup(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerAddNewGroupRequest>(data);
            return XHelper.Invoke("POST", "Customer/AddNewGroup", req);
        }
        public static ZResponse SearchGroup(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerSearchGroupRequest>(data);
            return XHelper.Invoke("POST", "Customer/SearchGroup", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("Customer/GetByID?ID={0}&ShopID={1}", req.ID, req.ShopID), null);
        }
        public static ZResponse CheckBlackList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerCheckBlackListRequest>(data);
            return XHelper.Invoke("POST", "Customer/CheckBlackList", req);
        }
        public static ZResponse CheckIDByMobileNumber(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerCheckIDByMobileNumberRequest>(data);
            return XHelper.Invoke("POST", "Customer/CheckIDByMobileNumber", req);
        }
        //public static ZResponse GetCustomerIDByOrderCode(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZCustomerGetByOrderCodeRequest>(data);
        //    return XHelper.Invoke("GET", string.Format("Customer/GetCustomerIDByOrderCode?OrderCode={0}&ShopID={1}", req.OrderCode, req.ShopID), null);
        //}
        public static ZResponse RemoveSticker(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerRemoveStickerRequest>(data);
            return XHelper.Invoke("POST", "Customer/RemoveSticker", req);
        }
        public static ZResponse IsExist(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerIsExistRequest>(data);
            return XHelper.Invoke("POST", "Customer/IsExist", req);
        }
        public static ZResponse ImportExcel(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerImportExcelRequest>(data);
            return XHelper.Invoke("POST", "Customer/ImportExcel", req);
        }
    }
}