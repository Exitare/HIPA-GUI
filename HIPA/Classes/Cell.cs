using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using HIPA.Statics;
using System.Text.RegularExpressions;
using System.Threading;
using HIPA.Services.Log;
using HIPA.Classes.InputFile;

namespace HIPA {


   

    partial class Cell {


        private string _name;
        private List<TimeFrame> _timeFrames;
        private decimal _threshold;
        private decimal _baseline_mean;
        private List<TimeFrame> _normalizedTimeFrames;
        private decimal _timeframMaximum;
        private Dictionary<double,int> _highIntesityCounts;

      

        public Cell(string Name, List<TimeFrame> Timeframes, decimal Threshold, decimal BaselineMean,  List<TimeFrame> NormalizedTimeframes, decimal TimeFrameMaximum, Dictionary<double, int> HighIntensityCounts) {
            _name = Name;
            _timeFrames = Timeframes;
            _baseline_mean = BaselineMean;
            _normalizedTimeFrames = NormalizedTimeframes;
            _timeframMaximum = TimeFrameMaximum;
            _highIntesityCounts = HighIntensityCounts;
            _threshold = Threshold;
        }

        public string Name { get => _name; set => _name = value; }
        public List<TimeFrame> Timeframes { get => _timeFrames; set => _timeFrames = value; }
        public decimal BaselineMean { get => _baseline_mean; set => _baseline_mean = value; }     
        public List<TimeFrame> NormalizedTimeframes { get => _normalizedTimeFrames; set => _normalizedTimeFrames = value; }
        public decimal TimeFrameMaximum { get => _timeframMaximum; set => _timeframMaximum = value; }
        public Dictionary<double, int> HighIntensityCounts { get => _highIntesityCounts; set => _highIntesityCounts = value; }
        public decimal Threshold { get => _threshold; set => _threshold = value; }

      


       
    }



}
