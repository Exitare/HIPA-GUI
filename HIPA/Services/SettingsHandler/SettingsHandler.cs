using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPA;
using HIPA.Services.Misc;
using HIPA.Statics;
enum NormalizationMethods
{
    BASELINE = 0,
    TO_ONE = 1,
}

namespace HIPA.Services.SettingsHandler
{
    class SettingsHandler
    {
        public static void InitializeNormalizationMethods()
        {
            Globals.NormalizationMethods.Add(NormalizationMethods.BASELINE, "Baseline");
            Globals.NormalizationMethods.Add(NormalizationMethods.TO_ONE, "To One");
        }

        public static NormalizationMethods GetNormalizationMethodEnumValue(string method)
        {
            foreach(var item in Globals.NormalizationMethods)
            {
                if (item.Value == method)
                    return item.Key;
            }

            return NormalizationMethods.BASELINE;
        }
      
        public static List<string> GetStringNormalizationMethods()
        {
            List<string> options = new List<string>();
            foreach(var method in Globals.NormalizationMethods)
            {
                options.Add(method.Value);
            }
            return options;
        }

        public static Tuple<NormalizationMethods, string> LoadStoredNormalizationMethod()
        {

            switch (Settings.Default.DefaultNormalization)
            {
                case 0:
                    return Tuple.Create(NormalizationMethods.BASELINE, "Baseline");
                case 1:
                    return Tuple.Create(NormalizationMethods.TO_ONE, "To One");

            }

            return Tuple.Create(NormalizationMethods.BASELINE, "Baseline");
        }
    }
}
