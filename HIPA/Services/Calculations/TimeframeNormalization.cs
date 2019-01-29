using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using HIPA.Statics;
namespace HIPA {

    class TimeFrameNormalization {
        /// <summary>
        /// Handles the correct Execution of the Chosen Normalization
        /// </summary>
        /// <param name="file"></param>
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

        /// <summary>
        /// Normalize each Timeframe with previous calculated Baseline Mean
        /// </summary>
        /// <param name="file"></param>
        public static void Baseline_Mean(InputFile file)
        {

            foreach (Cell cell in file.Cells)
            {
                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    cell.Normalized_Timeframes.Add(new TimeFrame(timeframe.ID, timeframe.Value / cell.Baseline_Mean, timeframe.Including_Minute, timeframe.Above_Below_Threshold));
                }
            }

        }


        /// <summary>
        /// Normalize each Timeframe for each Cell. Range is from 0 - 1 where 1 is the highest Timeframe ín the Cell.
        /// </summary>
        /// <param name="file"></param>
        public static void To_One(InputFile file)
        {
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
                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    cell.Normalized_Timeframes.Add(new TimeFrame(timeframe.ID, timeframe.Value / max, timeframe.Including_Minute, timeframe.Above_Below_Threshold));
                }
            }
        }
    }
}



