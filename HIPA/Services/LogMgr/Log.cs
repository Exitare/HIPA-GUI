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
                FileName = Globals.LogTextFile,
                Layout = "${longdate}\n${level}\n${message}\n${exception} \n"
            };

            var errorLog = new FileTarget("errorLog")
            {
                FileName = Globals.ErrorLogTextFile,
                Layout = "${longdate}\n${level}\n${message}\n${exception} \n"
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
    }


}
