using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace HIPA {

    class Mean {
        public static void Calculate_Baseline_Mean(InputFile file)
        {

            foreach (Cell cell in file.Cells)
            {
                int count = 0;
                decimal total = 0;

                for(int i = 0; i < file.Stimulation_Timeframe; ++i)
                {
                    total = total + cell.Timeframes[i].Value;
                    count++;
                }
                cell.Baseline_Mean = total / count; 
            }

        }


        public static void Calculate_Normalized_Mean(InputFile file)
        {

            foreach (Cell cell in file.Cells)
            {
                int count = 0;
                decimal total = 0;
                foreach (TimeFrame timeframe in cell.Normalized_Timeframes)
                {
                    total = total + timeframe.Value;
                    count++;
                }
                cell.Baseline_Mean = total / cell.Timeframes.Count;
            }
        }
        
    }
}
