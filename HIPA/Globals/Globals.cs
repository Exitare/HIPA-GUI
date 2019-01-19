using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HIPA
{
    class Globals
    {
        public static List<InputFile> Files = new List<InputFile>();
        public static Dictionary<string, Delegate> NormalizationMethods = new Dictionary<string, Delegate>();
        public static RoutedCommand MyCommand = new RoutedCommand();



        public delegate void NormilzationDelegate(InputFile file);

        public static void InitializeNormalization()
        {
            NormalizationMethods.Add("Kaya", new NormilzationDelegate(TimeFrameNormalization.NormalizeTimeFrames));
            NormalizationMethods.Add("Other", new NormilzationDelegate(TimeFrameNormalization.AurielNormalization));
           
        }

    }
}
