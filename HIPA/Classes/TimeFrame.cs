using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using HIPA.Classes.InputFile;

namespace HIPA {
    partial class TimeFrame {

        private readonly int _id;
        private readonly decimal _value;
        private readonly double _includingMinute;
        private bool _aboveBelowCellThreshold;

        public TimeFrame(int ID, decimal Value, double IncludingMinute, bool AboveBelowThreshold)
        {
            _id = ID;
            _value = Value;
            _includingMinute = IncludingMinute;
            _aboveBelowCellThreshold = AboveBelowThreshold;
        }

        public int ID { get => _id; }
        public decimal Value { get => _value; }
        public double IncludingMinute { get => _includingMinute; }
        public bool AboveBelowThreshold { get => _aboveBelowCellThreshold; set => _aboveBelowCellThreshold = value; }

    

        public static TimeFrame CreateTimeFrame(int line, decimal timeFrameValue, InputFile file)
        {
            return new TimeFrame(line, timeFrameValue, Math.Floor(Convert.ToDouble(Convert.ToDouble(line) * 3.9 / 60)), false);
        }
    }
}


