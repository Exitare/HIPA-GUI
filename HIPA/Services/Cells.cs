using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA {
    class CellService {
        public static void CreateCells(File file) {
            List<Cell> Cells = new List<Cell>();
            for (int i = 0; i < file.CellCount; ++i) {
                Cells.Add(new Cell("Line" + i, new List<TimeFrame>(), 0, 0, 0, 0, 0, 0));
            }

            file.Cells = Cells;
        }

        public static void PopulateCells(File file) {
            string[] content = file.Content;
            for (int line = 0; line < content.Length; ++line) {
                content[line].Trim(' ');

                if (content.Length != 0) {
                    string[] values = content[line].Split('\t');
                    for (int cell = 0; cell < file.CellCount; ++cell) {
                        if (line == 0) {
                            file.Cells[cell].Name = values[cell];
                        }
                        else {
                            file.Cells[cell].Timeframes.Add(TimeFrameService.CreateTimeFrame(line - 1, cell, file));
                        }
                    }
                }

            }
           
        }

        public static void CalculateMinutes(File file) {
            file.Minutes = file.Cells[0].Timeframes.Count * 3.9 / 60;
        }

        public static int CalculateRows(string[] lines) {
            return lines.Length;
        }

        public static int CalculateCellCount(string[] lines) {
            int count = 0;
            foreach (string line in lines) {
                line.Trim(' ');
                if (line.Length != 0) {
                    string[] value = line.Split('\t');
                    return value.Length;
                }

            }

            return count;
        }
    }
}
