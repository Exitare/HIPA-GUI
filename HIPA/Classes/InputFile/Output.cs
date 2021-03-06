﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPA.Services.Log;
using HIPA.Classes.InputFile;

namespace HIPA.Classes.InputFile {

    partial class InputFile
    {

        /// <summary>
        /// Creates the matrix which is used to create the textfile
        /// </summary>
        /// <returns></returns>
        public string[,] CreateNormalizedTimeFrameMatrix()
        {
            try
            {
#if DEBUG
                Debug.Print("RowCount is {0} and Cell Count is {1}", RowCount, CellCount);
                Debug.Print("TimeFrame Count is is {0} and Cell Count is {1}", TimeframeCount, CellCount);
#endif
                string[,] data_matrix = new string[RowCount, CellCount];

                // Cell Names
                for (int j = 0; j < CellCount; j++)
                {
                    data_matrix[0, j] = Cells[j].Name;
                }

                for (int i = 1; i <= TimeframeCount; i++)
                {
                    for (int j = 0; j < CellCount; j++)
                    {
                        data_matrix[i, j] = Cells[j].NormalizedTimeframes[i - 1].Value.ToString();
                    }
                }
                return data_matrix;
            }
            catch (Exception ex)
            {
                Logger.logger.Fatal(ex.Message);
                Logger.logger.Fatal(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Creates the Matrix for the High Intensity Counts
        /// </summary>
        /// <returns></returns>
        public string[,] CreateHighIntensityCountsMatrix()
        {

            try
            {
#if DEBUG
                Debug.Print("Total detected Minutes {0}", Math.Floor(TotalDetectedMinutes));
#endif

                string[,] data_matrix = new string[Convert.ToInt32(Math.Floor(TotalDetectedMinutes)) + 2, CellCount];

                for (int j = 0; j < CellCount; j++)
                {
                    data_matrix[0, j] = Cells[j].Name;
                }

                for (int i = 1; i <= Math.Floor(TotalDetectedMinutes) + 1; i++)
                {
                    for (int j = 0; j < CellCount; j++)
                    {
                        data_matrix[i, j] = Cells[j].HighIntensityCounts[i - 1].ToString();
                    }

                }

                return data_matrix;

            } catch (Exception ex)
            {
                Logger.logger.Fatal(ex.Message);
                Logger.logger.Fatal(ex.StackTrace);
                throw;
            }

        }


        /// <summary>
        ///  Creates the txt file for the normalized Timesframes
        /// </summary>
        /// <returns></returns>
        public void ExportNormalizedTimesframes()
        {
            string[,] data_matrix = CreateNormalizedTimeFrameMatrix();
            string filename;
            if (Settings.Default.CustomOutputPathActive)
                filename = Settings.Default.CustomOutputPath + "\\" + Name + "-NormalizedTimeframes-" + DateTime.Today.ToShortDateString() + ".txt";
            else
                filename = FullPath + Name + "-High Intensity Counts-" + DateTime.Today.ToShortDateString() + ".txt";

            try
            {

                StreamWriter sw = new StreamWriter(filename);
                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }
                sw.Close();
                return;
            }
            catch (Exception ex)
            {
                Logger.logger.Error(ex.Message);
                Logger.logger.Error(ex.StackTrace);
                

                filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + Name + "-Normalized Timeframes-" + DateTime.Today.ToShortDateString() + ".txt";
                StreamWriter sw = new StreamWriter(new Uri(filename).LocalPath);

                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }


                sw.Close();
                return;
            }

        }

        /// <summary>
        /// Creates the txt file for the high intensity counts
        /// </summary>
        /// <returns></returns>
        public void ExportHighIntensityCounts()
        {
            string[,] data_matrix = CreateHighIntensityCountsMatrix();
            string filename;
            if (Settings.Default.CustomOutputPathActive)
                filename = Settings.Default.CustomOutputPath + "\\" + Name + "-High Intensity Counts-" + DateTime.Today.ToShortDateString() + ".txt";
            else
                filename = FullPath + Name + "-High Intensity Counts-" + DateTime.Today.ToShortDateString() + ".txt";

            try
            {
                StreamWriter sw = new StreamWriter(filename);

                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];

                    sw.WriteLine(String.Join("\t", row));
                }

                sw.Close();
            }

            catch (Exception ex)
            {
                Logger.logger.Error(ex.Message);
                Logger.logger.Error(ex.StackTrace);

                filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + Name + "-High Intensity Counts-" + DateTime.Today.ToShortDateString() + ".txt";
                StreamWriter sw = new StreamWriter(new Uri(filename).LocalPath);

                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }

                sw.Close();
            }
        }
    }
}
