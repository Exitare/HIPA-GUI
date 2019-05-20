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
        private string _fullpath;
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
        private Seperator _detectedSeperator;
        private bool _prepared;
        private bool _invalid;


        public InputFile(int ID, string Folder, string FullPath, string Name, decimal PercentageLimit, List<Cell> Cells, int CellCount, int RowCount, double TotalDetectedMinutes, string[] Content,
            int StimulationTimeframe, string SelectedNormalizationMethod, int TimeFrameCount, Seperator DetectedSeperator, bool Prepared, bool Invalid)
        {
            _id = ID;
            _name = Name;
            _fullpath = FullPath;
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
            _detectedSeperator = DetectedSeperator;
            _prepared = Prepared;
            _invalid = Invalid;
            
        }

        public int ID { get => _id; }
        public string Folder { get => _folder; }
        public string FullPath { get => _fullpath;}
        public string Name { get => _name;}
        public decimal PercentageLimit { get => _percentageLimit; set => _percentageLimit = value; }
        public int CellCount { get => _cellCount; set => _cellCount = value; }
        public int RowCount { get => _rowCount; set => _rowCount = value; }
        public double TotalDetectedMinutes { get => _totalDetectedMinutes; set => _totalDetectedMinutes = value; }
        public string[] Content { get => _content; set => _content = value; }
        public int StimulationTimeframe { get => _stimulationTimeframe; set => _stimulationTimeframe = value; }
        public string SelectedNormalizationMethod { get => _selectedNormalizationMethod; set => _selectedNormalizationMethod = value; }
        internal List<Cell> Cells { get => _cells; set => _cells = value; }
        public int TimeframeCount { get => _timeframeCount; set => _timeframeCount = value; }
        public Seperator DetectedSeperator { get => _detectedSeperator; set => _detectedSeperator = value; }
        public bool Prepared { get => _prepared; set => _prepared = value; }
        public bool Invalid { get => _invalid; set => _invalid = value; }
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

            foreach (string filePath in openFileDialog.FileNames)
            {
                Globals.GetFiles().Add(new InputFile(ID, Path.GetDirectoryName(filePath), filePath, Path.GetFileNameWithoutExtension(filePath), (decimal)0.6, new List<Cell>(), 0, 0, 0, new string[0], 372, SettingsHandler.LoadStoredNormalizationMethod().Item2, 0, Seperator.NOT_YET_DETECTED,false, false));
                ID++;
            }
        }

      


        

       

    }
}

