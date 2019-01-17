using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HIPA {
    class TimeFrameService {
        public static TimeFrame CreateTimeFrame(int line, int value, File file) {
            Debug.Print("Value: " + value);
            Debug.Print("Line: " + line);
            return new TimeFrame(line, value, Convert.ToInt32(Convert.ToDouble(line) * 3.9 / 60));
        }

    }
}
