using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using HIPA.Classes.InputFile;

namespace HIPA.Statics
{
    partial class Globals
    {
        private static List<InputFile> files = new List<InputFile>();
        
        private static RoutedCommand myCommand = new RoutedCommand();
        private static bool connectionSuccessful = false;

        public static bool ConnectionSuccessful { get => connectionSuccessful; set => connectionSuccessful = value; }
        public static RoutedCommand MyCommand { get => myCommand; set => myCommand = value; }
      
        internal static List<InputFile> GetFiles()
        {
            return files;
        }

        internal static void SetFiles(List<InputFile> value)
        {
            files = value;
        }

        public delegate void NormalizationDelegate(InputFile file);

        public static string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string HIPAFolder = Path.Combine(appDataFolder, "HIPA");
        public static string LogsFolder = Path.Combine(HIPAFolder, LogDirectory);
        public static string LogTextFile = Path.Combine(LogsFolder, LogFileName);
        public static string ErrorLogTextFile = Path.Combine(LogsFolder, ErrorLogFileName);
        public static string HIPATempFolder = Path.Combine(Path.GetTempPath(), "HIPA");
        public static string HIPATempFolderSetupEXEFileName = Path.Combine(HIPATempFolder,"setup.exe");
        public static string HIPATempFolderSetupMSIFileName = Path.Combine(HIPATempFolder,"setup.msi");

        private static string LogDirectory => "Logs";
        private static string ErrorLogFileName => "ErrorLog.txt";
        private static string LogFileName => "Log.txt";

        public static Queue<string> LogQueue = new Queue<string>();

        public static bool UpdateAvailable = false;

        public static Dictionary<NormalizationMethods, string> NormalizationMethods = new Dictionary<NormalizationMethods, string>();

    }
}
