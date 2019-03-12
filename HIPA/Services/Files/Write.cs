using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPA;
using System.Diagnostics;
using HIPA.Calculations;
using HIPA.Log;
namespace HIPA.Files {
    class Write {

        public static bool Export_Normalized_Timesframes(InputFile file)
        {
          
            string filename = file.Folder + file.Name + "-Normalized Timeframes-" + DateTime.Today.ToShortDateString() + ".txt";
          
            try
            {

                StreamWriter sw = new StreamWriter(filename);


                string[,] data_matrix = Matrices.CreateNormalizedTimeFrameMatrix(file);


                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[file.CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }


                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, LogLevel.Error);
                Logging.WriteLog("Could not create file in source folder. Used own execution folder!", LogLevel.Error);
                filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)  + "-Normalized Timeframes-" + DateTime.Today.ToShortDateString() + ".txt";
                StreamWriter sw = new StreamWriter(new Uri(filename).LocalPath);


                string[,] data_matrix = Matrices.CreateNormalizedTimeFrameMatrix(file);


                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[file.CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }


                sw.Close();
                return false;
            }

        }


        public static bool Export_High_Intensity_Counts(InputFile file)
        {
            string filename = file.Folder + file.Name + "-High Intensity Counts-" + DateTime.Today.ToShortDateString() + ".txt";
         
            try
            {
                StreamWriter sw = new StreamWriter(filename);

                string[,] data_matrix = Matrices.CreateHighIntensityCountsMatrix(file);
                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[file.CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }


                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, LogLevel.Error);
                Logging.WriteLog("Could not create file in source folder. Used own execution folder!", LogLevel.Error);
                filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "-High Intensity Counts-" + DateTime.Today.ToShortDateString() + ".txt";
                StreamWriter sw = new StreamWriter(new Uri(filename).LocalPath);
                string[,] data_matrix = Matrices.CreateHighIntensityCountsMatrix(file);
                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[file.CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }


                sw.Close();
                return false;
            }
        }


    }
}
