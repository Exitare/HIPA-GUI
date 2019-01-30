using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA.Calculations {

    class MinimumMaximum {
        /// <summary>
        /// Find the TimeFrame Maximum for later Threshold Calculation
        /// </summary>
        /// <param name="file"></param>
        public static void FindTimeFrameMaximum(InputFile file)
        {
            foreach (Cell cell in file.Cells)
            {
                decimal maximum = 0;
                foreach (TimeFrame timeframe in cell.Normalized_Timeframes)
                {
                    if (timeframe.Value >= maximum)
                    {
                        maximum = timeframe.Value;
                    }
                }
                cell.TimeFrame_Maximum = maximum;
            }

        }

        /// <summary>
        /// 
        /// Calculates the Threshold (e.g. 60% of Maximum)
        /// </summary>
        /// <param name="file"></param>
        public static void CalculateThreshold(InputFile file) {
         
                foreach(Cell cell in file.Cells) {
                    cell.Threshold = cell.TimeFrame_Maximum * file.Percentage_Limit;
                }
            
        }

    }
}
