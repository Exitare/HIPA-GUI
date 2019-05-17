using HIPA.Services.SettingsHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPA.Services.Log;

namespace HIPA.Classes.InputFile {
    partial class InputFile
    {

        /// <summary>
        /// Handles the correct Execution of the Chosen Normalization
        /// </summary>
        public void ExecuteChosenNormalization()
        {
            switch (SettingsHandler.GetNormalizationMethodEnumValue(SelectedNormalizationMethod))
            {
                case NormalizationMethods.BASELINE:
                    NormalizeWithBaselineMean();
                    break;

                case NormalizationMethods.TO_ONE:
                    NormalizeWithToOne();
                    break;
                default:
                    //TODO handle this error correct, or check on startup if value is valid and change it properly
                    //throw new Exceptions.NoNormalizationMethodFound();
                    NormalizeWithBaselineMean();
                    break;
            }
        }

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

                if (maximum == 0)
                    throw new Exceptions.NoTimeFrameMaximumDetected("Maximum is still 0.");

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
                try
                {
                    cell.Threshold = cell.TimeFrameMaximum * PercentageLimit;
                }
                catch (Exception ex)
                {
                    Logger.logger.Fatal(ex.Message);
                    Logger.logger.Fatal(ex.StackTrace);
                    throw new Exceptions.CouldNotCalculateThreshold("Could not calculate Treshold");

                }               
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
                    total += cell.Timeframes[i].Value;
                    count++;
                }

                try
                {
                    cell.BaselineMean = total / count;
                }
                catch (Exception ex)
                {
                    Logger.logger.Fatal(ex.Message);
                    Logger.logger.Fatal(ex.StackTrace);
                    throw new Exceptions.CouldNotCalculateBaselineMean("Could not calculate Baseline mean");
                    
                }
               
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
                    try
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
                    catch(Exception ex)
                    {
                        Logger.logger.Fatal(ex.Message);
                        Logger.logger.Fatal(ex.StackTrace);
                        throw new Exceptions.CouldNotCountHighIntensityPeaksPerMinute();
                    }
                }
            }
        }
    }
}
