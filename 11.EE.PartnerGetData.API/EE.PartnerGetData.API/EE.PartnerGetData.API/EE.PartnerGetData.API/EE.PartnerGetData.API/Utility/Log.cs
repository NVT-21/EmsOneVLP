using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Utility
{
    public class Log
    {
        private static string _logPath = string.Empty;
        public static string LogPath { get { if (string.IsNullOrEmpty(_logPath)) _logPath = ConfigurationManager.AppSettings["LogPath"]; return _logPath; } }

        public static void WriteLog(string title, string description, LogFileType logFileType)
        {
            description = DateTime.Now.ToString("HH:mm:ss") + " " + title + ": " + description;

            string logPath = string.Empty;
            logPath = LogPath + @"\" + DateTime.Now.ToString("yyyyMMdd");

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            switch (logFileType)
            {
                case LogFileType.Trace:
                    logPath = logPath + @"\Trace.log";
                    break;

                case LogFileType.Message:
                    logPath = logPath + @"\Message.log";
                    break;

                case LogFileType.Exception:
                    logPath = logPath + @"\Exception.log";
                    break;

                case LogFileType.Validate:
                    logPath = logPath + @"\Validate.log";
                    break;
            }

            using (StreamWriter w = File.AppendText(logPath))
            {
                w.WriteLine(description);
            }
        }
    }

    public enum LogFileType
    {
        Trace,
        Message,
        Exception,
        Validate,
    }
}