using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using HIPA.Services;
using Microsoft.Win32;
using HIPA.Statics;
using System.IO;
using HIPA.Services.Log;

namespace HIPA
{
    partial class InputFile {

        private readonly int _id;
        private string _path;
        private string _folder;
        private string _name;
        private decimal _percentage_limit;
        private List<Cell> _cells;
        private int _cellCount;
        private int _rowCount;
        private int _timeframeCount;
        private double _totalDetectedMinutes;
        private string[] _content;
        private int _stimulationTimeframe;
        private string _normalizationMethod;

        public InputFile(int ID, string Folder, string Path, string Name, decimal Percentage_Limit, List<Cell> Cells, int CellCount, int RowCount, double Total_Detected_Minutes, string[] Content, int Stimulation_Timeframe, string Normalization_Method, int TimeFrameCount)
        {
            _id = ID;
            _name = Name;
            _path = Path;
            _folder = Folder;
            _percentage_limit = Percentage_Limit;
            _cells = Cells;
            _cellCount = CellCount;
            _rowCount = RowCount;
            _totalDetectedMinutes = Total_Detected_Minutes;
            _content = Content;
            _stimulationTimeframe = Stimulation_Timeframe;
            _normalizationMethod = Normalization_Method;
            _timeframeCount = TimeframeCount;
        }

        public string Name { get => _name; set => _name = value; }
        public int ID { get => _id; }
        public string SourcePath { get => _path; set => _path = value; }
        public decimal Percentage_Limit { get => _percentage_limit; set => _percentage_limit = value; }
        public int CellCount { get => _cellCount; set => _cellCount = value; }
        public int RowCount { get => _rowCount; set => _rowCount = value; }
        public double Total_Detected_Minutes { get => _totalDetectedMinutes; set => _totalDetectedMinutes = value; }
        public string[] Content { get => _content; set => _content = value; }
        public int Stimulation_Timeframe { get => _stimulationTimeframe; set => _stimulationTimeframe = value; }
        public string Normalization_Method { get => _normalizationMethod; set => _normalizationMethod = value; }
        internal List<Cell> Cells { get => _cells; set => _cells = value; }
        public string Folder { get => _folder; set => _folder = value; }
        public int TimeframeCount { get => _timeframeCount; set => _timeframeCount = value; }


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
                    ReadContent();
                    Cell.CellBuilder();
                    if (DataOK())
                        allOK = true;
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.Print(ex.Message);
#endif
                    Logger.WriteLog(ex.Message, LogLevel.Error);
                
                }
            });


            prepareData.Start();
            prepareData.Join();
            return allOK;
        
           
        }

        /// <summary>
        /// Resolve the file Path
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static string GetName(string Path)
        {
            string[] pathFragments = Path.Split('\\');

          


            string[] nameFragments = pathFragments[pathFragments.Length - 1].Split('.');
            string fileName = "";
            for (int i = 0; i < nameFragments.Length; i++)
            {
                if(i != nameFragments.Length -1)
                    fileName = fileName + nameFragments[i] + "_";

                if (i == nameFragments.Length - 2)
                    fileName = fileName + nameFragments[i];
            }
            fileName.Replace(' ', '_');
            return fileName;
        }

        public static string GetFolder(string Path)
        {
            string[] words = Path.Split('\\');
            string path = "";

            for (int i = 0; i < words.Length - 1; ++i)
            {
                if (i == 0)
                {
                    path += words[i] + "\\";
                }
                else
                {
                    path += words[i] + "\\";
                }
             ;
            }

            path = path.Replace(@"\\", @"\");
            return path;
        }

        /// <summary>
        /// Adds every selected file to the list 
        /// </summary>
        /// <param name="openFileDialog"></param>
        public static void AddFilesToList(OpenFileDialog openFileDialog)
        {
            int ID = 0;

            foreach (InputFile file in Globals.Files)
            {
                if (file.ID > ID)
                    ID = file.ID;
            }

            if (ID != 0)
                ID++;


            if (Globals.Files.Count == 1)
                ID = 1;

            foreach (String filePath in openFileDialog.FileNames)
            {
                Globals.Files.Add(new InputFile(ID, GetFolder(filePath), filePath, GetName(filePath), (decimal)0.6, new List<Cell>(), 0, 0, 0, new string[0], 372, Settings.Default.DefaultNormalization, 0));
                ID++;
            }
        }


        /// <summary>
        /// Reads the content of the given file and stores it
        /// </summary>
        public void ReadContent()
        {
            string[] lines = System.IO.File.ReadAllLines(SourcePath);
            CellCount = Cell.Calculate_Cell_Count(lines);
            RowCount = Cell.Calculate_Rows_Per_Cell(lines);
            TimeframeCount = lines.Length - 1;
            Content = lines;
        }


        /// <summary>
        /// Calculates the BaseLine Mean foreach Cell. This is used for the Baseline Mean Normalization
        /// This Mean is
        /// </summary>
        /// <param name="file"></param>
        public void Calculate_Baseline_Mean()
        {
            foreach (Cell cell in Cells)
            {
                int count = 0;
                decimal total = 0;

                for (int i = 0; i < Stimulation_Timeframe; ++i)
                {
                    total = total + cell.Timeframes[i].Value;
                    count++;
                }
                cell.Baseline_Mean = total / count;
            }
        }

        /// <summary>
        /// Detects if a given timeframe value is above or below the threshold
        /// </summary>
        public void Detect_Above_Below_Threshold()
        {
            foreach (Cell cell in Cells)
            {
                foreach (TimeFrame timeframe in cell.Normalized_Timeframes)
                {
                    if (timeframe.Value >= cell.Threshold)
                        timeframe.Above_Below_Threshold = true;

                    else
                        timeframe.Above_Below_Threshold = false;
                }
            }
        }


        /// <summary>
        /// Counts the High intensity peaks per minute
        /// </summary>
        public void Count_High_Intensity_Peaks_Per_Minute()
        {
            foreach (Cell cell in Cells)
            {
                foreach (TimeFrame timeframe in cell.Normalized_Timeframes)
                {

                    if (!cell.High_Intensity_Counts.ContainsKey(timeframe.Including_Minute))
                    {
                        if (timeframe.Above_Below_Threshold)
                            cell.High_Intensity_Counts.Add(timeframe.Including_Minute, 1);

                        else
                            cell.High_Intensity_Counts.Add(timeframe.Including_Minute, 0);
                    }
                    else
                    {
                        if (timeframe.Above_Below_Threshold)
                            cell.High_Intensity_Counts[timeframe.Including_Minute] = cell.High_Intensity_Counts[timeframe.Including_Minute] + 1;
                    }


                }
            }
        }

        /// <summary>
        /// Find the TimeFrame Maximum for later Threshold Calculation
        /// </summary>
        /// <param name="file"></param>
        public void FindTimeFrameMaximum()
        {
            foreach (Cell cell in Cells)
            {
                decimal maximum = 0;
                foreach (TimeFrame timeframe in cell.Normalized_Timeframes)
                {
                    if (timeframe.Value >= maximum)
                        maximum = timeframe.Value;

                }
                cell.TimeFrame_Maximum = maximum;
            }

        }

        /// <summary>
        /// 
        /// Calculates the Threshold (e.g. 60% of Maximum)
        /// </summary>
        /// <param name="file"></param>
        public void CalculateThreshold()
        {
            foreach (Cell cell in Cells)
            {
                cell.Threshold = cell.TimeFrame_Maximum * Percentage_Limit;
            }

        }

        /// <summary>
        /// Handles the correct Execution of the Chosen Normalization
        /// </summary>
        /// <param name="file"></param>
        public void Execute_Chosen_Normalization()
        {
            foreach (KeyValuePair<string, Delegate> Dic in Globals.NormalizationMethods)
            {
                if (Normalization_Method == Dic.Key)
                    Dic.Value.DynamicInvoke(new object[] { this });        
            }
        }

        /// <summary>
        /// Normalize each Timeframe with previous calculated Baseline Mean
        /// </summary>
        /// <param name="file"></param>
        public static void Baseline_Mean(InputFile file)
        {
            foreach (Cell cell in file.Cells)
            {
                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    cell.Normalized_Timeframes.Add(new TimeFrame(timeframe.ID, timeframe.Value / cell.Baseline_Mean, timeframe.Including_Minute, timeframe.Above_Below_Threshold));
                }
            }

        }


        /// <summary>
        /// Normalize each Timeframe for each Cell. Range is from 0 - 1 where 1 is the highest Timeframe ín the Cell.
        /// </summary>
        /// <param name="file"></param>
        public static void To_One(InputFile file)
        {
            foreach (Cell cell in file.Cells)
            {
                decimal max = 0.0M;

                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    if (timeframe.Value >= max)
                        max = timeframe.Value;
                }

                foreach (TimeFrame timeframe in cell.Timeframes)
                {
                    cell.Normalized_Timeframes.Add(new TimeFrame(timeframe.ID, timeframe.Value / max, timeframe.Including_Minute, timeframe.Above_Below_Threshold));
                }
            }
        }

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
                Debug.Print("TimeFrame COunt is is {0} and Cell Count is {1}", TimeframeCount, CellCount);
#endif
                string[,] data_matrix = new string[RowCount, CellCount];

                // Cell Names
                for (int j = 0; j < CellCount; j++)
                {
                    data_matrix[0, j] = Cells[j].Name;
                }

                for (int i = 1; i <= TimeframeCount ; i++)
                {
                    for (int j = 0; j < CellCount; j++)
                    {
                        data_matrix[i, j] = Cells[j].Normalized_Timeframes[i - 1].Value.ToString();
                      
                    }
                }
                return data_matrix;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, LogLevel.Error);
                return new string[0, 0];
            }



        }

        /// <summary>
        /// Creates the Matrix for the High Intensity Counts
        /// </summary>
        /// <returns></returns>
        public string[,] CreateHighIntensityCountsMatrix()
        {

#if DEBUG
            Debug.Print("Total detected Minutes {0}", Math.Floor(Total_Detected_Minutes));
#endif

            string[,] data_matrix = new string[Convert.ToInt32(Math.Floor(Total_Detected_Minutes)) + 2, CellCount];

            for (int j = 0; j < CellCount; j++)
            {
                data_matrix[0, j] = Cells[j].Name;
            }

            for (int i = 1; i <= Math.Floor(Total_Detected_Minutes) + 1; i++)
            {
                for (int j = 0; j < CellCount; j++)
                {
                    data_matrix[i, j] = Cells[j].High_Intensity_Counts[i - 1].ToString();
                }

            }


            return data_matrix;
        }


        /// <summary>
        /// Creates the txt file for the normalized Timesframes
        /// </summary>
        /// <returns></returns>
        public bool Export_Normalized_Timesframes()
        {

            string filename = Folder + Name + "-Normalized Timeframes-" + DateTime.Today.ToShortDateString() + ".txt";

            try
            {

                StreamWriter sw = new StreamWriter(filename);

                string[,] data_matrix = CreateNormalizedTimeFrameMatrix();

                Debug.Print("Row Count is {0} and Column Count is {1}", data_matrix.GetLength(0), data_matrix.GetLength(1));

                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }


                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, LogLevel.Error);
                Logger.WriteLog("Could not create file in source folder. Use own execution folder!", LogLevel.Error);
                filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "-Normalized Timeframes-" + DateTime.Today.ToShortDateString() + ".txt";
                StreamWriter sw = new StreamWriter(new Uri(filename).LocalPath);


                string[,] data_matrix = CreateNormalizedTimeFrameMatrix();


                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }


                sw.Close();
                return false;
            }

        }

        /// <summary>
        /// Creates the txt file for the high intensity counts
        /// </summary>
        /// <returns></returns>
        public bool Export_High_Intensity_Counts()
        {
            string filename = Folder + Name + "-High Intensity Counts-" + DateTime.Today.ToShortDateString() + ".txt";

            try
            {
                StreamWriter sw = new StreamWriter(filename);

                string[,] data_matrix = CreateHighIntensityCountsMatrix();
                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }


                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, LogLevel.Error);
                Logger.WriteLog("Could not create file in source folder. Used own execution folder!", LogLevel.Error);
                filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "-High Intensity Counts-" + DateTime.Today.ToShortDateString() + ".txt";
                StreamWriter sw = new StreamWriter(new Uri(filename).LocalPath);
                string[,] data_matrix = CreateHighIntensityCountsMatrix();
                for (int i = 0; i < data_matrix.GetLength(0); ++i)
                {

                    string[] row = new string[CellCount];


                    for (int j = 0; j < data_matrix.GetLength(1); j++)
                        row[j] = data_matrix[i, j];


                    sw.WriteLine(String.Join("\t", row));
                }


                sw.Close();
                return false;
            }
        }


        public bool DataOK()
        {
            if (Cells.Count() != CellCount)
                return false;

            foreach(Cell cell in this.Cells)
            {
                if (cell.Timeframes.Count() < Stimulation_Timeframe)
                    Stimulation_Timeframe = cell.Timeframes.Count() / 2;
            }
                        
            return true;
        }

    }
}
