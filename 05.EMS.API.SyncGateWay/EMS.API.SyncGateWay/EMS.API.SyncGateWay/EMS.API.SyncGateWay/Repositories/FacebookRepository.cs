using EMS.API.GateWay;
using EMS.API.GateWay.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace EMS.API.SyncGateWay.Repositories
{
    public class FacebookRepository
    {
        public static ZResponse ConnectPage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookConnectRequest>(data);
            var dataFormat = string.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}", APILibraryRepository.FacebookAppID, ConfigurationManager.AppSettings["FB_REDIRECT_URL"] + "Channel/AddConnectPage", APILibraryRepository.FacebookAppSecret, req.Code);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = "oauth/access_token?" + dataFormat
            }, "GET");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.ConnectPage: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GetAccountProfile(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookGetProfileRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = "me?fields=id,name,picture,cover&access_token=" + req.AccessToken
            }, "GET");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.GetAccountProfile: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GetUserInfoByUserID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookGetUserInfoByUserIDRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.ChannelUserID + "?fields=id,birthday,name,picture,link&access_token=" + req.PageAccessToken
            }, "GET");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.GetUserInfoByUserID: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GetPageInfo(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookGetProfileRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = "me/accounts?access_token=" + req.AccessToken
            }, "GET");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.GetPageInfo: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse Subcribed(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSubcribedRequest>(data);
            object obj = new
            {
                access_token = req.AccessToken
            };
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.PageID + "/subscribed_apps?subscribed_fields=message_mention,messages,message_reactions,message_deliveries,message_echoes,message_reads",
                Data = JsonConvert.SerializeObject(obj)
            }, "POST");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.Subcribed: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GetFeed(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookGetFeedRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.PageID + "/feed?fields=id,created_time,from,message,full_picture&access_token=" + req.PageAccessToken
            }, "GET");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.GetFeed: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse DeleteFeed(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookDeleteFeedRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.FeedID + "?access_token=" + req.PageAccessToken
            }, "DELETE");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.DeleteFeed: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GetComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookGetCommentRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.FeedID + "/comments?fields=id,from,message,comment_count,created_time&limit=10&access_token=" + req.PageAccessToken
            }, "GET");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.GetComment: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GetConversation(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookGetConversationRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.PageID + "?fields=conversations{id,message,senders,snippet,updated_time}&limit=2000&access_token=" + req.PageAccessToken
            }, "GET");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.GetConversation: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse GetMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookGetMessageRequest>(data);
            string code = "";
            if (!string.IsNullOrEmpty(req.PageAfter))
            {
                code = req.ConversationID + "/messages?fields=id,message,created_time,from,to&limit=2000&after=" + req.PageAfter + "&access_token=" + req.PageAccessToken;
            }
            else
            {
                code = req.ConversationID + "?fields=messages{id,message,created_time,from,to}&access_token=" + req.PageAccessToken;
            }
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = code
            }, "GET");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.GetMessage: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse SendMessage(string data)
        {
            dynamic dyn = JsonConvert.DeserializeObject<object>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = "me/messages?access_token=" + dyn.PageAccessToken,
                Data = JsonConvert.SerializeObject(dyn.req)
            });
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                dynamic dataEror = JsonConvert.DeserializeObject<object>(res.Data);
                res.Code = "99";
                res.Message = dataEror.error.message;
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.SendMessage: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse SendComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookSendCommentRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.ID + "/comments",
                Data = JsonConvert.SerializeObject(req.RequestData)
            });
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.SendComment: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse DeleteComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookDeleteCommentRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.CommentID + "?access_token=" + req.PageAccessToken
            }, "DELETE");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.DeleteComment: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse LikeComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookLikeCommentRequest>(data);
            object obj = new
            {
                access_token = req.PageAccessToken
            };
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.CommentID + "/likes",
                Data = JsonConvert.SerializeObject(obj)
            });
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.LikeComment: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse DeleteLikeComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookDeleteLikeCommentRequest>(data);
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.CommentID + "/likes?access_token=" + req.PageAccessToken
            }, "DELETE");
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.DeleteLikeComment: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }

        public static ZResponse HiddenComment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookHiddenCommentRequest>(data);
            object obj = new
            {
                access_token = req.PageAccessToken
            };
            var res = APILibraryRepository.InvokeFacebook(new ZRequest()
            {
                Code = req.CommentID + "?is_hidden=" + req.Type,
                Data = JsonConvert.SerializeObject(obj)
            });
            if (res.Code == "00")
            {
                res.Code = res.Code;
                res.Message = res.Message;
                res.Data = res.Data;
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API Facebook, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("FacebookRepository.HiddenComment: res={0}", JsonConvert.SerializeObject(res)));
            }
            return res;
        }
    }
}