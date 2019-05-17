using System;
using System.IO;
using HIPA.Statics;
using NLog;
using NLog.Targets;
using NLog.Config;


namespace HIPA.Services.Log {


    class Logger
    {
        public static NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public static void ConfigureLogger()
        {

            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget("target1")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
          

            var infoLog = new FileTarget("infoLog")
            {
                FileName = "${basedir}/Logs/log.txt",
                Layout = "${longdate} ${level}\n ${message}  ${exception} \n"
            };

            var errorLog = new FileTarget("errorLog")
            {
                FileName = "${basedir}/Logs/errorLog.txt",
                Layout = "${longdate} ${level}\n ${message}  ${exception} \n"
            };

            config.AddTarget(infoLog);
            config.AddTarget(errorLog);
            config.AddTarget(consoleTarget);


            config.AddRuleForAllLevels(consoleTarget); // all to console
            config.AddRuleForAllLevels(infoLog);
            config.AddRuleForOneLevel(LogLevel.Error, errorLog);
            config.AddRuleForOneLevel(LogLevel.Fatal, errorLog);

            LogManager.Configuration = config;
        }


        //    private const int NumberOfRetries = 3;
        //    private const int DelayOnRetry = 2;

        //    public static void WriteLog(string message, LogLevel logLevel)
        //    {
        //        DateTime dateTime = DateTime.UtcNow; ;

        //        if(logLevel == LogLevel.Error)
        //        {
        //            using (StreamWriter sw = new StreamWriter(Globals.ErrorLogFileName, true))
        //            {
        //                sw.WriteLine(dateTime + ": " + message);
        //            }
        //            return;
        //        }

        //        using (StreamWriter sw = new StreamWriter(Globals.LogFileName, true))
        //        {
        //            switch (logLevel)
        //            {
        //                case LogLevel.Debug:
        //                    sw.WriteLine(dateTime + ": " + message);
        //                    break;
        //                case LogLevel.Warning:
        //                    sw.WriteLine(dateTime + ": " + message);
        //                    break;
        //                case LogLevel.Info:
        //                    sw.WriteLine(dateTime + ": " + message);
        //                    break;

        //                default:
        //                    sw.WriteLine(dateTime + ": " + message);
        //                    break;
        //            }
        //        }
        //    }

        //}


        //enum LogLevel {
        //    Error,
        //    Warning,
        //    Debug,
        //    Info
        //}

    }


}
