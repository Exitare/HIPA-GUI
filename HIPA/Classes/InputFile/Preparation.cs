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




        /// <summary>
        /// Prepares the given file. Generates the cells and timeframes
        /// </summary>
        public void PrepareFile()
        {
            ReadContent();
            FileValid();
            DetectSeperator();
            DetectDataSizes();
            CalculateCellCount();
            CellBuilder();
            DataOK();
            Prepared = true;
            return;
        }

        /// <summary>
        /// Reads the content of the given file and stores it
        /// </summary>
        public void ReadContent()
        {
            try
            {
                Content = File.ReadAllLines(FullPath);
            }
            catch (Exception ex)
            {
                Logger.logger.Fatal(ex.Message);
                Logger.logger.Fatal(ex.StackTrace);
                throw;
            }
            return;
        }

        public void FileValid()
        {
           
            try
            {
                foreach (string line in Content)
                {
                    if (line.Contains("Average") || line.Contains("Err"))
                        continue;

                    if (Regex.Matches(line, @"[a-zA-Z]").Count != 0)
                    {
                        StackTrace stackTrace = new StackTrace();
                        Logger.logger.Fatal("Found invalid character");
                        Logger.logger.Fatal(stackTrace.ToString());
                        throw new Exceptions.InvalidCharacterFound("Found invalid character");
                    }
                      
                }
                return;
            } 
            catch (Exception ex)
            {
                Logger.logger.Fatal(ex.Message);
                Logger.logger.Fatal(ex.StackTrace);
                throw;
            } 
        }

        public void DetectSeperator()
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

                try
                {
                    if (_tempSeperator != DetectedSeperator)
                        throw new Exceptions.InvalidSeperator("There are different seperators used in the file");
                }
                 catch (Exception ex)
                {
                    Logger.logger.Fatal(ex.Message);
                    Logger.logger.Fatal(ex.StackTrace);
                    throw;
                }
            }

            return;
        }

        /// <summary>
        /// Detect all needed data sizes
        /// </summary>
        public void DetectDataSizes()
        {
            try
            {
                RowCount = Content.Length;
                TimeframeCount = Content.Length - 1;
            } catch (Exception ex)
            {
                Logger.logger.Fatal(ex.Message);
                Logger.logger.Fatal(ex.StackTrace);
                throw;
            }
           
        }


        /// <summary>
        /// Calculates the cell count 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private void CalculateCellCount()
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
                return;
            }
            catch (Exception ex)
            {
                Logger.logger.Fatal(ex.Message);
                Logger.logger.Fatal(ex.StackTrace);
                throw;
            }

        }

        /// <summary>
        /// Calculates the minutes per Cell
        /// </summary>
        public void CalculateMinutes()
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
                return;
            }
            catch (Exception ex)
            {
                Logger.logger.Fatal(ex.Message);
                Logger.logger.Fatal(ex.StackTrace);
                throw;
            }

        }

        //TODO Fix bug which causes false positives
        private void DataOK()
        {

            double lastDetectedMinutes = 0.0;

            if (Cells.Count() != CellCount)
                throw new Exceptions.DataCheckNotPassed("Cell count not matching");

            for (int i = 0; i < Cells.Count; ++i)
            {
                Cell cell = Cells[i];

                if (cell.Timeframes.Count() < StimulationTimeframe)
                    StimulationTimeframe = cell.Timeframes.Count() / 2;

                double detectedMinutes = cell.Timeframes.Count * 3.9 / 60;

                if (i != 0)
                    if (detectedMinutes != lastDetectedMinutes)
                        throw new Exceptions.DataCheckNotPassed("Detected Minutes not matching");


                lastDetectedMinutes = detectedMinutes;

            }
            return;
        }


    }
}
