using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using HIPA.Statics;
using System.Text.RegularExpressions;
using System.Threading;

namespace HIPA {
    partial class Cell {

        private string _name;
        private List<TimeFrame> _time_frames;
        private decimal _threshold;
        private decimal _baseline_mean;
        private List<TimeFrame> _normalized_time_frames;
        private decimal _timeframe_maximum;
        private Dictionary<double,int> _high_intesity_counts;



        public Cell(string Name, List<TimeFrame> Timeframes, decimal Threshold, decimal Baseline_Mean,  List<TimeFrame> Normalized_Timeframes, decimal TimeFrame_Maximum, Dictionary<double, int> High_Intensity_Counts) {
            _name = Name;
            _time_frames = Timeframes;
            _baseline_mean = Baseline_Mean;
            _normalized_time_frames = Normalized_Timeframes;
            _timeframe_maximum = TimeFrame_Maximum;
            _high_intesity_counts = High_Intensity_Counts;
            _threshold = Threshold;
        }

        public string Name { get => _name; set => _name = value; }
        public List<TimeFrame> Timeframes { get => _time_frames; set => _time_frames = value; }
        public decimal Baseline_Mean { get => _baseline_mean; set => _baseline_mean = value; }     
        public List<TimeFrame> Normalized_Timeframes { get => _normalized_time_frames; set => _normalized_time_frames = value; }
        public decimal TimeFrame_Maximum { get => _timeframe_maximum; set => _timeframe_maximum = value; }
        public Dictionary<double, int> High_Intensity_Counts { get => _high_intesity_counts; set => _high_intesity_counts = value; }
        public decimal Threshold { get => _threshold; set => _threshold = value; }



        public static void CellBuilder()
        {
            foreach (InputFile file in Globals.Files)
            {
                file.Cells = new List<Cell>();
                CreateCells(file);
                PopulateCells(file);
                Calculate_Minutes_Per_Cell(file);
            }
        }



        public static void CreateCells(InputFile file)
        {
            List<Cell> Cells = new List<Cell>();
            for (int i = 0; i < file.CellCount; ++i)
            {
                Cells.Add(new Cell("Line" + i, new List<TimeFrame>(), 0, 0, new List<TimeFrame>(), 0, new Dictionary<double, int>()));
            }
            Debug.Print("Cell size is " + Cells.Count);
            file.Cells = Cells;
        }

        public static void PopulateCells(InputFile file)
        {
            string[] content = file.Content;
            for (int line = 0; line < content.Length; ++line)
            {
               
                content[line].Trim(' ');
                Regex.Replace(content[line], @"\s+", "");
              // content[line].Replace(' ', ' ');
              //  content[line].Replace('\t', '-');

                const string reduceMultiSpace = @"[ ]{2,}";
                content[line] = Regex.Replace(content[line].Replace("\t", "|"), reduceMultiSpace, "|");

               Debug.Print(content[line] + "Lenght is " + content[line].Length);
                if (content.Length != 0)
                {
                    string[] cellValues = content[line].Split('|');
                    for (int i = 0; i < cellValues.Length; ++i)
                    {
                        if(cellValues[i] == "")
                        {
                            Debug.Print(cellValues[i] + " i ist: " + i);
                        }
                     
                    }

                    for (int cell = 0; cell < file.CellCount; ++cell)
                    {
                       
                        if (line == 0)
                        {
                            file.Cells[cell].Name = cellValues[cell];
                        }
                        else
                        {
                           
                       
                          
                            if (decimal.TryParse(cellValues[cell].Replace('.', ','), out decimal doublevalue))
                            {
                                file.Cells[cell].Timeframes.Add(TimeFrame.CreateTimeFrame(line, doublevalue, file));
                            }
                            else
                            {
                                Log.Logging.WriteLog("Could not convert to Decimal because value is " + cellValues[cell] + " for cell  " + file.Cells[cell].Name + " and line " + line, Log.LogLevel.Error);
                            }
                               
                        }
                   
                    }
                }
             

            }

        }


    

        public static void Calculate_Minutes_Per_Cell(InputFile file)
        {
            file.Total_Detected_Minutes = file.Cells[0].Timeframes.Count * 3.9 / 60;
        }

        public static int Calculate_Rows_Per_Cell(string[] lines)
        {
            return lines.Length;
        }

        public static int Calculate_Cell_Count(string[] lines)
        {
            int count = 0;
            foreach (string line in lines)
            {
                line.Trim(' ');
                if (line.Length != 0)
                {
                    string[] value = line.Split('\t');
                    return value.Length;
                }

            }

            return count;
        }

    }



}
