using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HIPA.Services.Log;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HIPA.Classes.InputFile {
    partial class InputFile
    {

        /// <summary>
        /// Cellbuilder which handles the cellcreation
        /// </summary>
        public bool CellBuilder()
        {
              
            if (!CreateCells() || !PopulateCells() || !CalculateMinutes())
                return false;

            return true;
        }



        /// <summary>
        /// Create Cells
        /// </summary>
        public bool CreateCells()
        {
            try
            {
                for (int i = 0; i < CellCount; ++i)
                {
                    Cells.Add(new Cell("Line" + i, new List<TimeFrame>(), 0, 0, new List<TimeFrame>(), 0, new Dictionary<double, int>()));
                }
#if DEBUG
                Debug.Print("Cell size is " + Cells.Count);
#endif
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }

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
                    Content[line].Trim(' ');
                    Regex.Replace(Content[line], @"\s+", "");

                    const string reduceMultiSpace = @"[ ]{2,}";
                    Content[line] = Regex.Replace(Content[line].Replace((char)DetectedSeperator, '|'), reduceMultiSpace, "");

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
                                    Logger.WriteLog("Could not convert to Decimal because value is " + cellValueList[cell] + "for file " + Name + " for cell  " + Cells[cell].Name + " and line " + line, LogLevel.Error);
                                   
                            }


                        }
                    }
                }
            }
            return true;
        }
    }
}
