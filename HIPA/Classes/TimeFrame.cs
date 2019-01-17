using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA {
    class TimeFrame {

        private readonly int _id;
        private readonly int _value;
        private readonly int _including_minute;

        public TimeFrame(int ID, int Value, int Including_Minute) {
            _id = ID;
            _value = Value;
            _including_minute = Including_Minute;
        }

        public int ID { get => _id; }
        public int Value { get => _value; }
        public int Minute { get => _including_minute; }
    }
}
