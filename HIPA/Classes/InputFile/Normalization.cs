using HIPA.Services.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace HIPA.Classes.InputFile
{
    partial class InputFile
    {
        /// <summary>
        /// Delegates for normalization Methods
        /// </summary>
        private delegate void NormalizeBaseLineDelegate();
        private delegate void NormalizeToOneDelegate();


     

        /// <summary>
        /// Normalize each Timeframe with previous calculated Baseline Mean
        /// </summary>
        /// <param name="file"></param>
        public void NormalizeWithBaselineMean()
        {
            foreach (Cell cell in Cells)
            {
                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    cell.NormalizedTimeframes.Add(new TimeFrame(timeframe.ID, timeframe.Value / cell.BaselineMean, timeframe.IncludingMinute, timeframe.AboveBelowThreshold));
                }
            }

        }


        /// <summary>
        /// Normalize each Timeframe for each Cell. Range is from 0 - 1 where 1 is the highest Timeframe ín the Cell.
        /// </summary>
        public void NormalizeWithToOne()
        {
            foreach (Cell cell in Cells)
            {
                decimal max = 0.0M;

                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    if (timeframe.Value >= max)
                        max = timeframe.Value;
                }

                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    cell.NormalizedTimeframes.Add(new TimeFrame(timeframe.ID, timeframe.Value / max, timeframe.IncludingMinute, timeframe.AboveBelowThreshold));
                }
            }
        }
    }
}
