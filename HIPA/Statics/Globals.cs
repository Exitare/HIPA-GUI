using System;
using System.Collections.Generic;
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

        public static string ErrorLog => "ErrorLog.txt";
        public static string Log => "Log.txt";
        public static Queue<string> LogQueue = new Queue<string>();

        public static bool UpdateAvailable = false;

        public static Dictionary<NormalizationMethods, string> NormalizationMethods = new Dictionary<NormalizationMethods, string>();

       
    }
}
