using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA {
    class Cell {

        private string _name;
        private List<TimeFrame> _time_frames;
        private double _baseline_mean;
        private double _all_mean;
        private double _normalized_data;
        private double _maximum;
        private double _over_under_limit;
        private int _high_stimulus_per_minute;



        public Cell(string Name, List<TimeFrame> Timeframes, double BaseLine_Mean, double All_Mean, double Normalized_Data, double Maximum, double Over_Under_Limit, int High_Stimulus_Per_Minute) {
            _name = Name;
            _time_frames = Timeframes;
            _baseline_mean = BaseLine_Mean;
            _all_mean = All_Mean;
            _normalized_data = Normalized_Data;
            _maximum = Maximum;
            _over_under_limit = Over_Under_Limit;
            _high_stimulus_per_minute = High_Stimulus_Per_Minute;
        }

        public string Name { get => _name; set => _name = value; }
        public List<TimeFrame> Timeframes { get => _time_frames; set => _time_frames = value; }
        public double Baseline_mean { get => _baseline_mean; set => _baseline_mean = value; }
        public double All_mean { get => _all_mean; set => _all_mean = value; }
        public double Normalized_data { get => _normalized_data; set => _normalized_data = value; }
        public double Maximum { get => _maximum; set => _maximum = value; }
        public double Over_under_limit { get => _over_under_limit; set => _over_under_limit = value; }
        public int High_stimulus_per_minute { get => _high_stimulus_per_minute; set => _high_stimulus_per_minute = value; }
    }
}
