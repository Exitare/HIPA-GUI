using System;
using System.IO;
using HIPA.Statics;

namespace HIPA.Services.Log {


    class Logger {

        private const int NumberOfRetries = 3;
        private const int DelayOnRetry = 2;

        public static void WriteLog(string message, LogLevel logLevel)
        {
            DateTime dateTime = DateTime.UtcNow; ;
          
            if(logLevel == LogLevel.Error)
            {
                using (StreamWriter sw = new StreamWriter(Globals.ErrorLog, true))
                {
                    sw.WriteLine(dateTime + ": " + message);
                }
                return;
            }
         
            using (StreamWriter sw = new StreamWriter(Globals.Log, true))
            {
                switch (logLevel)
                {
                    case LogLevel.Debug:
                        sw.WriteLine(dateTime + ": " + message);
                        break;
                    case LogLevel.Warning:
                        sw.WriteLine(dateTime + ": " + message);
                        break;
                    case LogLevel.Info:
                        sw.WriteLine(dateTime + ": " + message);
                        break;

                    default:
                        sw.WriteLine(dateTime + ": " + message);
                        break;
                }
            }
        }

    }


    enum LogLevel {
        Error,
        Warning,
        Debug,
        Info
    }



}
