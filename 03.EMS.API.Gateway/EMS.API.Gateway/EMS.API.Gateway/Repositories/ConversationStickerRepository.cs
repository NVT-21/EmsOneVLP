using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class ConversationStickerRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConversationStickerSearchListRequest>(data);
            return XHelper.Invoke("POST", "ConversationSticker/SearchList", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConversationStickerByIDRequest>(data);
            return XHelper.Invoke("POST", "ConversationSticker/GetByID", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConversationStickerAddRequest>(data);
            return XHelper.Invoke("POST", "ConversationSticker/Add", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConversationStickerEditRequest>(data);
            return XHelper.Invoke("POST", "ConversationSticker/Edit", req);
        }
        public static ZResponse Delete(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConversationStickerRemoveRequest>(data);
            return XHelper.Invoke("POST", "ConversationSticker/Delete", req);
        }
        public static ZResponse SearchListCStickerOfC(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConversationStickerSearchListRequest>(data);
            return XHelper.Invoke("POST", "ConversationSticker/SearchListCStickerOfC", req);
        }
        public static ZResponse UpdateCStickerOfC(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCStickerOfCUpdateRequest>(data);
            return XHelper.Invoke("POST", "ConversationSticker/UpdateCStickerOfC", req);
        }
    }
}