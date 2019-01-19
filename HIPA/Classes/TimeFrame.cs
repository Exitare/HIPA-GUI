using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
namespace HIPA {
    class TimeFrame {

        private readonly int _id;
        private readonly decimal _value;
        private readonly double _including_minute;
        private bool _above_below_cell_threshold;

        public TimeFrame(int ID, decimal Value, double Including_Minute, bool Above_Below_Threshold)
        {
            _id = ID;
            _value = Value;
            _including_minute = Including_Minute;
            _above_below_cell_threshold = Above_Below_Threshold;
        }

        public int ID { get => _id; }
        public decimal Value { get => _value; }
        public double Including_Minute { get => _including_minute; }
        public bool Above_Below_Threshold { get => _above_below_cell_threshold; set => _above_below_cell_threshold = value; }

    

        public static TimeFrame CreateTimeFrame(int line, decimal timeFrameValue, InputFile file)
        {
            //Debug.Print("Minute: " + Math.Floor(Convert.ToDouble(Convert.ToDouble(line) * 3.9 / 60)) + " for Line: " +  line.ToString());
            //Debug.Print("LineNumber: " + line);
            return new TimeFrame(line, timeFrameValue, Math.Floor(Convert.ToDouble(Convert.ToDouble(line) * 3.9 / 60)), false);

        }
    }
}


