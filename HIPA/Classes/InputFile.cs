﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using FileService;
using Microsoft.Win32;

namespace HIPA
{
    class InputFile
    {

        private readonly int _id;
        private string _path;
        private string _folder;
        private string _name;
        private decimal _percentage_limit;
        private List<Cell> _cells;
        private int _cellCount;
        private int _rowCount;
        private double _total_detected_minutes;
        private string[] _content;
        private int _stimulation_timeframe;
        private string _normalization_method;

        public InputFile(int ID, string Folder, string Path, string Name, decimal Percentage_Limit, List<Cell> Cells, int CellCount, int RowCount, double Total_Detected_Minutes, string[] Content, int Stimulation_Timeframe, string Normalization_Method)
        {
            _id = ID;
            _name = Name;
            _path = Path;
            _folder = Folder;
            _percentage_limit = Percentage_Limit;
            _cells = Cells;
            _cellCount = CellCount;
            _rowCount = RowCount;
            _total_detected_minutes = Total_Detected_Minutes;
            _content = Content;
            _stimulation_timeframe = Stimulation_Timeframe;
            _normalization_method = Normalization_Method;
        }

        public string Name { get => _name; set => _name = value; }
        public int ID { get => _id;}
        public string Path { get => _path; set => _path = value; }
        public decimal Percentage_Limit { get => _percentage_limit; set => _percentage_limit = value; } 
        public int CellCount { get => _cellCount; set => _cellCount = value; }
        public int RowCount { get => _rowCount; set => _rowCount = value; }
        public double Total_Detected_Minutes { get => _total_detected_minutes; set => _total_detected_minutes = value; }
        public string[] Content { get => _content; set => _content = value; }
        public int Stimulation_Timeframe { get => _stimulation_timeframe; set => _stimulation_timeframe = value; }
        public string Normalization_Method { get => _normalization_method; set => _normalization_method = value; }
        internal List<Cell> Cells { get => _cells; set => _cells = value; }
        public string Folder { get => _folder; set => _folder = value; }

        public static void PrepareFiles()
        {

            Thread prepareData = new Thread(() =>
            {
                try
                {
                    Debug.Print("Prepare Files");
                    Read.ReadFileContent();
                    Cell.CellBuilder();

                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message);

                }

            });

 
            prepareData.Start();
            prepareData.Join();
        }


        public static string GetFileName(string Path)
        {

            string[] words = Path.Split('\\');
            return words[words.Length - 1].Split('.')[0];
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
            Debug.Print("Path: " + path);
            return path;
        }

        public static void AddFilesToList(OpenFileDialog openFileDialog)
        {
            int id = 0;

            foreach (InputFile file in Globals.Files)
            {
                if (file.ID >= id)
                {
                    id = file.ID;
                }
            }

            if (id != 0)
            {
                id = id + 1;
            }


            foreach (String file in openFileDialog.FileNames)
            {
                Debug.Print(file);
                Globals.Files.Add(new InputFile(id, GetFolder(file), file, GetFileName(file), (decimal)0.6, new List<Cell>(), 0,0, 0, new string[0],372, Settings.Default.DefaultNormalization));
                id++;
            }
        }
    }


}
