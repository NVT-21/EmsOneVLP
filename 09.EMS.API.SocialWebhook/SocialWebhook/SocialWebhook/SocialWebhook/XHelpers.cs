using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using System.Text;

namespace SocialWebhook
{
    public static class XHelpers
    {
        static string _coreURL = string.Empty;
        static string _logPath = string.Empty;
        static string _logType = string.Empty;
        static string _telegramBotToken = string.Empty;
        static string _telegramGroupChatID = string.Empty;
        //static HttpClient _clientCore = null;

        static string CoreUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_coreURL))
                    _coreURL = ConfigurationManager.AppSettings["CORE_URL"];
                return _coreURL;
            }

            set
            {
                _coreURL = value;
            }
        }
        static string LogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logPath))
                    _logPath = ConfigurationManager.AppSettings["LOG_PATH"];
                return _logPath;
            }

            set
            {
                _logPath = value;
            }
        }

        static string LogType
        {
            get
            {
                if (string.IsNullOrEmpty(_logType))
                    _logType = ConfigurationManager.AppSettings["LOG_TYPE"];
                return _logType;
            }

            set
            {
                _logType = value;
            }
        }

        static string TelegramBotToken
        {
            get
            {
                if (string.IsNullOrEmpty(_telegramBotToken))
                    _telegramBotToken = ConfigurationManager.AppSettings["TELEGRAM_BOT_TOKEN"];
                return _telegramBotToken;
            }

            set
            {
                _telegramBotToken = value;
            }
        }
        static string TelegramGroupChatID
        {
            get
            {
                if (string.IsNullOrEmpty(_telegramGroupChatID))
                    _telegramGroupChatID = ConfigurationManager.AppSettings["TELEGRAM_GROUP_CHAT_ID"];
                return _telegramGroupChatID;
            }

            set
            {
                _telegramGroupChatID = value;
            }
        }
        public static void LogToFile(LogFileType logFileType, string message)
        {
            try
            {
                LoggerGeneral logger = new LoggerGeneral();
                message = DateTime.Now.ToString("HH:mm:ss") + " " + message;
                FormatterGeneral formatter = new FormatterGeneral(message);
                string logPath = string.Empty;
                logPath = LogPath + @"\" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HH");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                switch (logFileType)
                {
                    case LogFileType.TRACE:
                        logPath = logPath + @"\TRACE.log";
                        break;
                    case LogFileType.MESSAGE:
                        logPath = logPath + @"\MESSAGE.log";
                        break;
                    default:
                        logPath = logPath + @"\EXCEPTION.log";
                        break;
                }
                logger.Write(formatter, logPath);
            }
            catch
            {
                //WriteLog(LogFileType.EXCEPTION, string.Format("XHelper.LogToFile: {0}", ex.Message));
            }
        }

        public static void WriteLog(LogFileType logFileType, string message)
        {
            if (LogType == "LOG_PATH")
            {
                LogToFile(logFileType, message);
            }
            else if (LogType == "TELEGRAM")
            {
                var description = "";
                switch (logFileType)
                {
                    case LogFileType.TRACE:
                        description = string.Format("{0} SocialWebhook.TRACE {1}", DateTime.Now.ToString("HH:mm:ss"), message);
                        break;
                    case LogFileType.MESSAGE:
                        description = string.Format("{0} SocialWebhook.MESSAGE {1}", DateTime.Now.ToString("HH:mm:ss"), message);
                        break;
                    case LogFileType.EXCEPTION:
                        description = string.Format("{0} SocialWebhook.EXCEPTION {1}", DateTime.Now.ToString("HH:mm:ss"), message);
                        break;
                }

                SendNotifyTelegram(description);
            }
        }

        public static ZResponse Invoke(string method, string requestUri, object value)
        {
            ZResponse res = new ZResponse();
            try
            {
                using (HttpClient _clientCore = new HttpClient())
                {
                    _clientCore.BaseAddress = new Uri(CoreUrl);
                    _clientCore.DefaultRequestHeaders.Accept.Clear();
                    _clientCore.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    _clientCore.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    HttpResponseMessage response = null;

                    switch (method)
                    {
                        case "GET":
                            response = _clientCore.GetAsync(requestUri).Result;
                            break;
                        case "POST":
                            response = _clientCore.PostAsJsonAsync(requestUri, value).Result;
                            break;
                        default:
                            break;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        res = response.Content.ReadAsAsync<ZResponse>().Result;
                    }
                    else
                    {
                        WriteLog(LogFileType.MESSAGE, string.Format("XHelper.Invoke: req={0}||res={1}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(response.IsSuccessStatusCode)));
                        switch (response.StatusCode)
                        {
                            default:
                                res.Code = "99";
                                res.Message = "Lỗi " + response.StatusCode + ", vui lòng liên hệ quản trị.";
                                break;
                        }
                    }
                }
                //if (_clientCore == null)
                //{
                //    _clientCore = new HttpClient();
                //    _clientCore.BaseAddress = new Uri(CoreUrl);
                //    _clientCore.DefaultRequestHeaders.Accept.Clear();
                //    _clientCore.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //    //client.DefaultRequestHeaders.Add("APIKey", APIKey);
                //    //client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", Security.EncodeBase64("lms:lms")));
                //}

                //HttpResponseMessage rs = null;
                //switch (method)
                //{
                //    case "GET":
                //        rs = _clientCore.GetAsync(requestUri).Result;
                //        break;
                //    case "POST":
                //        rs = _clientCore.PostAsJsonAsync(requestUri, value).Result;
                //        break;
                //    default:
                //        break;
                //}

                //if (rs.IsSuccessStatusCode)
                //{
                //    res = rs.Content.ReadAsAsync<ZResponse>().Result;
                //}
                //else
                //{
                //    WriteLog(LogFileType.MESSAGE, string.Format("XHelper.Invoke: req={0}||res={1}", JsonConvert.SerializeObject(value), JsonConvert.SerializeObject(rs.IsSuccessStatusCode)));
                //    switch (rs.StatusCode)
                //    {
                //        default:
                //            res.Code = "99";
                //            res.Message = "Lỗi " + rs.StatusCode + ", vui lòng liên hệ quản trị.";
                //            break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                WriteLog(LogFileType.EXCEPTION, string.Format("XHelper.Invoke: req={0}||exception={1}", JsonConvert.SerializeObject(value), ex.Message));
            }
            return res;
        }

        public static void SendNotifyTelegram(string Message)
        {
            try
            {
                //https://api.telegram.org/bot5213895575:AAEBXt25DaqURolM9JxABaH1GYczhp1dpF8/getUpdates //get id nhóm chat
                string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
                urlString = string.Format(urlString, TelegramBotToken, TelegramGroupChatID, Message);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebRequest request = WebRequest.Create(urlString);
                Stream rs = request.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(rs);
                string line = "";
                StringBuilder sb = new StringBuilder();
                while (line != null)
                {
                    line = reader.ReadLine();
                    if (line != null)
                        sb.Append(line);
                }
            }
            catch (Exception ex)
            {
                LogToFile(LogFileType.EXCEPTION, string.Format("XHelper.SendNotifyTelegram: {0}", ex.Message));
            }
        }

        //public static string SendTelegram(string text)
        //{
        //    string API = "5213895575:AAEBXt25DaqURolM9JxABaH1GYczhp1dpF8";
        //    try
        //    {
        //        ServicePointManager.Expect100Continue = true;
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        var botClient = new Telegram.Bot.TelegramBotClient(API);
        //        var s = botClient.SendTextMessageAsync("#5213895575", text, ParseMode.Html);
        //        return "00|Thành công";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "99|" + ex.Message;
        //    }
        //}
    }
}