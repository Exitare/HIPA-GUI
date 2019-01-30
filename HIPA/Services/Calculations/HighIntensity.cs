using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA.Calculations {
    class HighIntensity {

        public static void Detect_Above_Below_Threshold(InputFile file)
        {
            foreach (Cell cell in file.Cells)
            {
                foreach (TimeFrame timeframe in cell.Normalized_Timeframes)
                {
                    if (timeframe.Value >= cell.Threshold)
                    {
                        timeframe.Above_Below_Threshold = true;
                    }
                    else
                    {
                        timeframe.Above_Below_Threshold = false;
                    }
                }
            }
        }


        public static void Count_High_Intensity_Peaks_Per_Minute(InputFile file)
        {
            foreach (Cell cell in file.Cells)
            {
                foreach (TimeFrame timeframe in cell.Normalized_Timeframes)
                {

                    if (!cell.High_Intensity_Counts.ContainsKey(timeframe.Including_Minute))
                    {
                        if (timeframe.Above_Below_Threshold)
                        {
                            cell.High_Intensity_Counts.Add(timeframe.Including_Minute, 1);
                        }
                        else
                        {
                            cell.High_Intensity_Counts.Add(timeframe.Including_Minute, 0);
                        }
                    }
                    else
                    {
                        if (timeframe.Above_Below_Threshold)
                        {
                            cell.High_Intensity_Counts[timeframe.Including_Minute] = cell.High_Intensity_Counts[timeframe.Including_Minute] + 1;
                        }
                    }
                    
                    
                }
            }
        }
    }
}
