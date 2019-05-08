using HIPA.Services.Log;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using HIPA.Statics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


enum Seperator
{
    NOT_YET_DETECTED,
    TAB_SEPERATOR = '\t',
    COMMA_SEPERATOR = ',',
    INVALID_SEPERATOR,
}

namespace HIPA.Classes.InputFile
{
    partial class InputFile
    {


        public static List<string> PrepareFiles(OpenFileDialog openFileDialog)
        {
            List<string> errorList = new List<string>();
            try
            {
                AddFilesToList(openFileDialog);
                foreach (InputFile file in Globals.GetFiles())
                {

                    if (!file.PrepareFile())
                        errorList.Add(file.Name);


                }
                return errorList;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                Debug.Print("Error occured @ prepareFiles");
                return errorList;
            }
        }


        /// <summary>
        /// Prepares the given file. Generates the cells and timeframes
        /// </summary>
        public bool PrepareFile()
        {
            try
            {
                if (!ReadContent() || !DetectSeperator() || !DetectDataSizes() || !CellBuilder() || !DataOK())
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// Reads the content of the given file and stores it
        /// </summary>
        public bool ReadContent()
        {
            try
            {
                Content = File.ReadAllLines(FullPath);
                Console.WriteLine(Content);
            }
            catch (Exception ex)
            {
                Content = new string[0];
                Logger.WriteLog("Could not read file " + Name, LogLevel.Error);
                Logger.WriteLog(ex.Message, LogLevel.Error);
                return false;
            }

            return true;

        }

        public bool DetectSeperator()
        {

            if (Content[0].Contains('\t'))
                DetectedSeperator = Seperator.TAB_SEPERATOR;

            else if (Content[0].Contains(','))
                DetectedSeperator = Seperator.COMMA_SEPERATOR;

            else
                DetectedSeperator = Seperator.INVALID_SEPERATOR;

            Seperator _tempSeperator = Seperator.NOT_YET_DETECTED;

            // Test all lines
            foreach (string line in Content)
            {
                if (line.Contains('\t'))
                    _tempSeperator = Seperator.TAB_SEPERATOR;

                else if (line.Contains(','))
                    _tempSeperator = Seperator.COMMA_SEPERATOR;


                if (_tempSeperator != DetectedSeperator)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Detect all needed data sizes
        /// </summary>
        public bool DetectDataSizes()
        {

            RowCount = Content.Length;
            TimeframeCount = Content.Length - 1;
            return CalculateCellCount();
        }


        /// <summary>
        /// Calculates the cell count 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private bool CalculateCellCount()
        {
            try
            {
                int count = 0;
                foreach (string line in Content)
                {
                    line.Trim(' ');
                    if (line.Length != 0)
                    {
                        string[] value = line.Split((char)DetectedSeperator);
                        count = value.Length;
                    }

                }

                CellCount = count;
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                Debug.Print("Error occured while calculating Cell Count");
                return false;
            }

        }

        /// <summary>
        /// Calculates the minutes per Cell
        /// </summary>
        public bool CalculateMinutes()
        {
            try
            {
                double lastDetectedMinutes = 0;
                for (int i = 0; i < Cells.Count; ++i)
                {
                    if (i == 0)
                        lastDetectedMinutes = Cells[i].Timeframes.Count * 3.9 / 60;

                    else
                    {
                        if (lastDetectedMinutes != Cells[i].Timeframes.Count * 3.9 / 60)
                            TotalDetectedMinutes = 0;
                    }
                }

                TotalDetectedMinutes = Cells[0].Timeframes.Count * 3.9 / 60;
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                Debug.Print("Error occured while calculating total minutes");
                return false;
            }

        }


        private bool DataOK()
        {

            double lastDetectedMinutes = 0.0;

            if (Cells.Count() != CellCount)
                return false;

            for (int i = 0; i < Cells.Count; ++i)
            {
                Cell cell = Cells[i];

                if (cell.Timeframes.Count() < StimulationTimeframe)
                    StimulationTimeframe = cell.Timeframes.Count() / 2;

                double detectedMinutes = cell.Timeframes.Count * 3.9 / 60;

                if (i != 0)
                    if (detectedMinutes != lastDetectedMinutes)
                        return false;


                lastDetectedMinutes = detectedMinutes;

            }
            return true;
        }


    }
}
