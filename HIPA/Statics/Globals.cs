using System;
using System.Collections.Generic;
using System.Windows.Input;
using HIPA.Calculations;

namespace HIPA.Statics
{
    partial class Globals
    {
        private static List<InputFile> files = new List<InputFile>();
        private static Dictionary<string, Delegate> normalizationMethods = new Dictionary<string, Delegate>();
        private static RoutedCommand myCommand = new RoutedCommand();
        private static bool connectionSuccessful = false;

        public static bool ConnectionSuccessful { get => connectionSuccessful; set => connectionSuccessful = value; }
        public static RoutedCommand MyCommand { get => myCommand; set => myCommand = value; }
        public static Dictionary<string, Delegate> NormalizationMethods { get => normalizationMethods; set => normalizationMethods = value; }
        internal static List<InputFile> Files { get => files; set => files = value; }

        public delegate void NormilzationDelegate(InputFile file);
        public static string ErrorLog => "Error-Log.txt";
        public static string Log => "Log.txt";
        public static Queue<string> LogQueue = new Queue<string>();

        public static void InitializeNormalization()
        {
            NormalizationMethods.Add("Baseline", new NormilzationDelegate(TimeFrameNormalization.Baseline_Mean));
            NormalizationMethods.Add("ToOne", new NormilzationDelegate(TimeFrameNormalization.To_One));
           
        }

    }
}
