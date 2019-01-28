using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;
using HIPA;

namespace FileService {

    class Read {

        public static void ReadFileContent() {
            foreach (InputFile file in Globals.Files) {
                string[] lines = System.IO.File.ReadAllLines(file.Path);
                file.CellCount = Cell.Calculate_Cell_Count(lines);
                file.RowCount = Cell.Calculate_Rows_Per_Cell(lines);
                file.TimeframeCount = lines.Length - 1;
                file.Content = lines;
            }
        }


      


      
        

    }
}
