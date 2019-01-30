using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPA;
using System.Diagnostics;
using HIPA.Calculations;

namespace HIPA.Files {
    class Write {

        public static bool Export_Normalized_Timesframes(InputFile file)
        {
            string filename = file.Folder + file.Name + "-Normalized Timeframes" + ".txt";
            try
            {
                File.Delete(filename);
            } catch (Exception ex)
            {

            }

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
            } catch (Exception ex)
            {
                return false;
            }
          
        }


        public static bool Export_High_Intensity_Counts(InputFile file)
        {
            string filename = file.Folder + file.Name + "-High Intensity Counts" + ".txt";
            try
            {
                File.Delete(filename);
            }
            catch (Exception ex)
            {

            }
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
                return false;
            }


            //string[,] data_matrix = new string[Convert.ToInt32(file.Total_Detected_Minutes) + 2, file.CellCount];

            //for (int j = 0; j < file.CellCount; j++)
            //{
            //    data_matrix[0, j] = file.Cells[j].Name;
            //}




            //for (int i = 1; i <= Convert.ToInt32(file.Total_Detected_Minutes) + 1; i++)
            //{
            //    for (int j = 0; j < file.CellCount; j++)
            //    {

            //        data_matrix[i, j] = file.Cells[j].High_Intensity_Counts[i - 1].ToString();
            //    }

            //}



        }


    }
}
