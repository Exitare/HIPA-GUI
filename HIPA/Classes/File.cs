using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA
{
    class File
    {

        private readonly int _id;
        private string _path;
        private string _filename;
        private double _limit;
        private bool _high_stimulus_output;
        private bool _normalized_data_output;
        private List<Cell> _cells;
        private int _cellCount;
        private int _rowCount;
        private double _minutes;
        private string[] _content;

        public File(int ID, string Path, string FileName, double Limit,bool High_Stimulus_Output, bool Normalized_Data_Ouput, List<Cell> Cells, int CellCount, int RowCount, double Minutes, string[] Content)
        {
            _id = ID;
            _filename = FileName;
            _path = Path;
            _limit = Limit;
            _high_stimulus_output = High_Stimulus_Output;
            _normalized_data_output = Normalized_Data_Ouput;
            _cells = Cells;
            _cellCount = CellCount;
            _rowCount = RowCount;
            _minutes = Minutes;
            _content = Content;
        }

        public string FileName { get => _filename; set => _filename = value; }
        public int ID { get => _id;}
        public string Path { get => _path; set => _path = value; }
        public double Limit { get => _limit; set => _limit = value; }
        public bool High_Stimulus_Output { get => _high_stimulus_output; set => _high_stimulus_output = value; }
        public bool Normalized_data_output { get => _normalized_data_output; set => _normalized_data_output = value; }
        public int CellCount { get => _cellCount; set => _cellCount = value; }
        public int RowCount { get => _rowCount; set => _rowCount = value; }
        public double Minutes { get => _minutes; set => _minutes = value; }
        public string[] Content { get => _content; set => _content = value; }
        internal List<Cell> Cells { get => _cells; set => _cells = value; }
    }
}
