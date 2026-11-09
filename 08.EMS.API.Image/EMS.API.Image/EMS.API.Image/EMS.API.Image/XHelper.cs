using EMS.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace EMS.API.Image
{
    public static class XHelper
    {
        static string _logPath = string.Empty;
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
        public static void LogToFile(LogFileType logFileType, string message)
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
    }
}