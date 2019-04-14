using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA.Classes.InputFile {
    partial class InputFile
    {



        /// <summary>
        /// Find the TimeFrame Maximum for later Threshold Calculation
        /// </summary>
        /// <param name="file"></param>
        public void FindTimeFrameMaximum()
        {
            foreach (Cell cell in Cells)
            {
                decimal maximum = 0;
                foreach (TimeFrame timeframe in cell.NormalizedTimeframes)
                {
                    if (timeframe.Value >= maximum)
                        maximum = timeframe.Value;

                }
                cell.TimeFrameMaximum = maximum;
            }

        }

        /// <summary>
        /// 
        /// Calculates the Threshold (e.g. 60% of Maximum)
        /// </summary>
        /// <param name="file"></param>
        public void CalculateThreshold()
        {
            foreach (Cell cell in Cells)
            {
                cell.Threshold = cell.TimeFrameMaximum * PercentageLimit;
            }

        }

        /// <summary>
        /// Calculates the BaseLine Mean foreach Cell. This is used for the Baseline Mean Normalization
        /// This Mean is
        /// </summary>
        /// <param name="file"></param>
        public void CalculateBaselineMean()
        {
            foreach (Cell cell in Cells)
            {
                int count = 0;
                decimal total = 0;

                for (int i = 0; i < StimulationTimeframe; ++i)
                {
                    total = total + cell.Timeframes[i].Value;
                    count++;
                }
                cell.BaselineMean = total / count;
            }
        }

        /// <summary>
        /// Detects if a given timeframe value is above or below the threshold
        /// </summary>
        public void DetectAboveBelowThreshold()
        {
            foreach (Cell cell in Cells)
            {
                foreach (TimeFrame timeframe in cell.NormalizedTimeframes)
                {
                    if (timeframe.Value >= cell.Threshold)
                        timeframe.AboveBelowThreshold = true;

                    else
                        timeframe.AboveBelowThreshold = false;
                }
            }
        }


        /// <summary>
        /// Counts the High intensity peaks per minute
        /// </summary>
        public void CountHighIntensityPeaksPerMinute()
        {
            foreach (Cell cell in Cells)
            {
                foreach (TimeFrame timeframe in cell.NormalizedTimeframes)
                {

                    if (!cell.HighIntensityCounts.ContainsKey(timeframe.IncludingMinute))
                    {
                        if (timeframe.AboveBelowThreshold)
                            cell.HighIntensityCounts.Add(timeframe.IncludingMinute, 1);

                        else
                            cell.HighIntensityCounts.Add(timeframe.IncludingMinute, 0);
                    }
                    else
                    {
                        if (timeframe.AboveBelowThreshold)
                            cell.HighIntensityCounts[timeframe.IncludingMinute] = cell.HighIntensityCounts[timeframe.IncludingMinute] + 1;
                    }


                }
            }
        }
    }
}
