using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPA;
using System.Diagnostics;
namespace FileService {
    class Write {

        public static void Export_Normalized_Timesframes(InputFile file)
        {

        }


        public static void Export_High_Stimulus_Counts(InputFile file)
        {
            string filename = file.Folder + file.Name + " results " + ".txt";
            File.Delete(filename);
            StreamWriter sw = new StreamWriter(filename);


            string[,] data_matrix = new string[Convert.ToInt32(file.Total_Detected_Minutes) + 2, file.CellCount];

            for (int j = 0; j < file.CellCount; j++)
            {
                data_matrix[0, j] = file.Cells[j].Name;
            }




            for (int i = 1; i <= Convert.ToInt32(file.Total_Detected_Minutes) + 1; i++)
            {
                for (int j = 0; j < file.CellCount; j++)
                {

                    data_matrix[i, j] = file.Cells[j].High_Intensity_Counts[i - 1].ToString();
                }

            }


            for (int i = 0; i < data_matrix.GetLength(0); ++i)
            {

                string[] row = new string[file.CellCount];


                for (int j = 0; j < data_matrix.GetLength(1); j++)
                    row[j] = data_matrix[i, j];


                sw.WriteLine(String.Join("\t", row));
            }


            sw.Close();
        }


    }
}
