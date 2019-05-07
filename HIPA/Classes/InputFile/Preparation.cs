using HIPA.Services.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


enum Seperator
{
    TAB_SEPERATOR,
    COMMA_SEPERATOR,
    INVALID_SEPERATOR,
}

namespace HIPA.Classes.InputFile
{
   partial class InputFile
    {

        /// <summary>
        /// Prepares the given file. Generates the cells and timeframes
        /// </summary>
        public bool PrepareFile()
        {
            bool allOK = false;
            Thread prepareData = new Thread(() =>
            {
                try
                {
                    if (!ReadContent())
                        allOK = false;

                    DetectDataSizes();
                    Console.WriteLine("Sep is : {0}",DetectSeperator());
                   

                    if (CellBuilder() && DataOK())
                        allOK = true;

                    else
                        allOK = false;

                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex.Message, LogLevel.Error);
                    allOK = false;
                }
            });


            prepareData.Start();
            prepareData.Join();
            return allOK;
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

        public Seperator DetectSeperator()
        {
            foreach(string line in Content)
            {
                if (line.Contains('\t'))
                    return Seperator.TAB_SEPERATOR;

                else if (line.Contains(','))
                    return Seperator.COMMA_SEPERATOR;
            }
            return Seperator.INVALID_SEPERATOR;
        }

        /// <summary>
        /// Detect all needed data sizes
        /// </summary>
        public void DetectDataSizes()
        {
            CalculateCellCount();
            RowCount = Content.Length;
            TimeframeCount = Content.Length - 1;
        }


        /// <summary>
        /// Calculates the cell count 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private void CalculateCellCount()
        {
            int count = 0;
            foreach (string line in Content)
            {
                line.Trim(' ');
                if (line.Length != 0)
                {
                    string[] value = line.Split('\t');
                    count = value.Length;
                }

            }

            CellCount = count;
        }

        /// <summary>
        /// Calculates the minutes per Cell
        /// </summary>
        public void CalculateMinutes()
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
        }

        /// <summary>
        /// Create Cells
        /// </summary>
        public  void CreateCells()
        {
            for (int i = 0; i < CellCount; ++i)
            {
                Cells.Add(new Cell("Line" + i, new List<TimeFrame>(), 0, 0, new List<TimeFrame>(), 0, new Dictionary<double, int>()));
            }
#if DEBUG
            Debug.Print("Cell size is " + Cells.Count);
#endif
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool PopulateCells()
        {
            if (Cells.Count != 0)
            {
                for (int line = 0; line < Content.Length; ++line)
                {

                    if (!Content[line].ToLower().Contains('\t'))
                    {
                        Logger.WriteLog("Could not find char \\t (Tabs) in file " + Name, LogLevel.Error);
                        return false;
                    }


                    Content[line].Trim(' ');
                    Regex.Replace(Content[line], @"\s+", "");

                    const string reduceMultiSpace = @"[ ]{2,}";
                    Content[line] = Regex.Replace(Content[line].Replace("\t", "|"), reduceMultiSpace, "");

                    string previousValue = "";
                    if (Content.Length != 0)
                    {

                        string[] cellValues = Content[line].Split('|');
                        List<string> cellValueList = new List<string>(cellValues);
                        for (int i = 0; i < cellValueList.Count(); ++i)
                        {
                            if (cellValueList[i] == "")
                                cellValueList.RemoveAt(i);
                        }


                        for (int cell = 0; cell < CellCount; cell++)
                        {

                            if (line == 0)
                                Cells[cell].Name = cellValueList[cell];

                            else
                            {
                                if (decimal.TryParse(cellValueList[cell].Replace('.', ','), out decimal doublevalue))
                                    Cells[cell].Timeframes.Add(new TimeFrame(line, Math.Round(doublevalue, 1), Math.Floor(Convert.ToDouble(Convert.ToDouble(line) * 3.9 / 60)), false));

                                else
                                    Logger.WriteLog("Could not convert to Decimal because value is " + cellValueList[cell] + " for cell  " + Cells[cell].Name + " and line " + line, LogLevel.Error);

                            }

                            previousValue = cellValueList[cell];
                        }
                    }
                }
            }
            return true;
        }

    }
}
