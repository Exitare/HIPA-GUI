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
using HIPA.Services.SettingsHandler;
using HIPA.Services.Misc;


namespace HIPA.Classes.InputFile
{
    partial class InputFile {

        private readonly int _id;
        private string _path;
        private string _folder;
        private string _name;
        private decimal _percentageLimit;
        private List<Cell> _cells;
        private int _cellCount;
        private int _rowCount;
        private int _timeframeCount;
        private double _totalDetectedMinutes;
        private string[] _content;
        private int _stimulationTimeframe;
        private string _selectedNormalizationMethod;
        private readonly NormalizeBaseLineDelegate _normalizeBaseLine;
        private readonly NormalizeToOneDelegate _normalizeToOne;


        public InputFile(int ID, string Folder, string Path, string Name, decimal PercentageLimit, List<Cell> Cells, int CellCount, int RowCount, double TotalDetectedMinutes, string[] Content, int StimulationTimeframe, string SelectedNormalizationMethod, int TimeFrameCount)
        {
            _id = ID;
            _name = Name;
            _path = Path;
            _folder = Folder;
            _percentageLimit = PercentageLimit;
            _cells = Cells;
            _cellCount = CellCount;
            _rowCount = RowCount;
            _totalDetectedMinutes = TotalDetectedMinutes;
            _content = Content;
            _stimulationTimeframe = StimulationTimeframe;
            _selectedNormalizationMethod = SelectedNormalizationMethod;
            _timeframeCount = TimeFrameCount;

           
            _normalizeBaseLine = new NormalizeBaseLineDelegate(NormalizeWithBaselineMean);
            _normalizeToOne = new NormalizeToOneDelegate(NormalizeWithToOne);
        }

        public string Name { get => _name; set => _name = value; }
        public int ID { get => _id; }
        public string FolderPath { get => _path; set => _path = value; }
        public decimal PercentageLimit { get => _percentageLimit; set => _percentageLimit = value; }
        public int CellCount { get => _cellCount; set => _cellCount = value; }
        public int RowCount { get => _rowCount; set => _rowCount = value; }
        public double TotalDetectedMinutes { get => _totalDetectedMinutes; set => _totalDetectedMinutes = value; }
        public string[] Content { get => _content; set => _content = value; }
        public int StimulationTimeframe { get => _stimulationTimeframe; set => _stimulationTimeframe = value; }
        public string SelectedNormalizationMethod { get => _selectedNormalizationMethod; set => _selectedNormalizationMethod = value; }
        internal List<Cell> Cells { get => _cells; set => _cells = value; }
        public string Folder { get => _folder; set => _folder = value; }
        public int TimeframeCount { get => _timeframeCount; set => _timeframeCount = value; }

     






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

            foreach (InputFile file in Globals.GetFiles())
            {
                if (file.ID > ID)
                    ID = file.ID;
            }

            if (ID != 0)
                ID++;


            if (Globals.GetFiles().Count == 1)
                ID = 1;

            foreach (String filePath in openFileDialog.FileNames)
            {
                Globals.GetFiles().Add(new InputFile(ID, GetFolder(filePath), filePath, GetName(filePath), (decimal)0.6, new List<Cell>(), 0, 0, 0, new string[0], 372, SettingsHandler.LoadStoredNormalizationMethod().Item2, 0));
                ID++;
            }
        }



     


    


        /// <summary>
        /// Handles the correct Execution of the Chosen Normalization
        /// </summary>
        public void ExecuteChosenNormalization()
        {
            switch (SettingsHandler.GetNormalizationMethodEnumValue(SelectedNormalizationMethod))
            {
                case NormalizationMethods.BASELINE:
                    NormalizeWithBaselineMean();
                    break;

                case NormalizationMethods.TO_ONE:
                    NormalizeWithToOne();
                    break;
                default:
                    break;
            }
        }


       
        private bool DataOK()
        {

            double lastDetectedMinutes = 0.0;

            if (Cells.Count() != CellCount)
                return false;

            for(int i = 0; i < Cells.Count; ++i)
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
