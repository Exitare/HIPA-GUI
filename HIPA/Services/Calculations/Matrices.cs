using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA.Calculations {
    class Matrices
    {
        public static string[,] CreateNormalizedTimeFrameMatrix(InputFile file)
        {
            try
            {
                string[,] data_matrix = new string[file.RowCount, file.CellCount];

                for (int j = 0; j < file.CellCount; j++)
                {
                    data_matrix[0, j] = file.Cells[j].Name;
                }




                for (int i = 1; i < file.RowCount; i++)
                {

                    for (int j = 0; j < file.CellCount; j++)
                    {

                        data_matrix[i, j] = file.Cells[j].Normalized_Timeframes[i - 1].Value.ToString();
                    }

                }
                return data_matrix;
            }
            catch (Exception ex)
            {
                Log.Logging.WriteLog(ex.Message, Log.LogLevel.Error);

                return new string[0,0];
            }
           

           
        }

        public static string[,] CreateHighIntensityCountsMatrix(InputFile file)
        {
            string[,] data_matrix = new string[Convert.ToInt32(file.Total_Detected_Minutes) + 2, file.CellCount];

            for (int j = 0; j < file.CellCount; j++)
            {
                data_matrix[0, j] = file.Cells[j].Name;
            }



            Debug.Print(Convert.ToString(Convert.ToInt32(file.Total_Detected_Minutes)));
            for (int i = 1; i <= Math.Floor(file.Total_Detected_Minutes) + 1; i++)
            {
                for (int j = 0; j < file.CellCount; j++)
                {

                    data_matrix[i, j] = file.Cells[j].High_Intensity_Counts[i - 1].ToString();
                }

            }


            return data_matrix;
        } 
    }
}
