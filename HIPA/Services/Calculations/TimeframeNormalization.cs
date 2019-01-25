using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace HIPA {

    class TimeFrameNormalization {

        public static void Execute_Chosen_Normalization(InputFile file)
        {
            foreach (KeyValuePair<string, Delegate> Dic in Globals.NormalizationMethods)
            {
                if (file.Normalization_Method == Dic.Key)
                {
                    Dic.Value.DynamicInvoke(new object[] { file });
                }
            }
        }


        public static void Baseline_Mean(InputFile file)
        {
            Debug.Print("Kaya execute");
            foreach (Cell cell in file.Cells)
            {
                foreach(TimeFrame timeframe in cell.Timeframes)
                {
                    cell.Normalized_Timeframes.Add(new TimeFrame(timeframe.ID, timeframe.Value / cell.Baseline_Mean, timeframe.Including_Minute, timeframe.Above_Below_Threshold));
                }
            }

        }

        public static void To_One(InputFile file)
        {
            Debug.Print("Other execute");

            foreach (Cell cell in file.Cells)
            {
                decimal max = 0.0M;

                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    if (timeframe.Value >= max)
                    {
                        max = timeframe.Value;
                    }
                }

                Debug.Print("Maximum detected {0}", max);


                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    cell.Normalized_Timeframes.Add(new TimeFrame(timeframe.ID, timeframe.Value / max, timeframe.Including_Minute, timeframe.Above_Below_Threshold));
                }
            }
        }
    }
}



