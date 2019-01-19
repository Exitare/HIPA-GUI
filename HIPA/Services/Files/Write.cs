using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPA;
using System.Diagnostics;
namespace FileService {
    class Write {

        static T[,] PivotArray<T>(T[][] source)
        {
            var numRows = source.Max(a => a.Length);
            var numCols = source.Length;

            var target = new T[numRows, numCols];
            for (int row = 0; row < source.Length; ++row)
            {
                for (int col = 0; col < source[row].Length; ++col)
                    target[col, row] = source[row][col];
            };

            return target;
        }


        public static void ExportData(InputFile file)
        {
            string filename = Path.Combine(file.Folder, file.Name + "-Results-" + DateTime.Now.ToLocalTime().ToString() + ".txt");

            StreamWriter sw = new StreamWriter(filename);

      


            foreach(Cell cell in file.Cells)
            {
              
            }


            for(int i = 0; i < file.Cells.Count; ++i)
            {
                if(i == 0)
                {
                    sw.WriteLine(file.Cells[i].Name + "\t");
                    for (int j = 0; j < file.Cells[i].Timeframes.Count; ++j)
                    {
                        sw.WriteLine(file.Cells[i].Timeframes[j].Value + "\t");
                    }
                } else
                {
                    sw.WriteLine(file.Cells[i].Name + "\t");
                    for (int j = 0; j < file.Cells[i].Timeframes.Count; ++j)
                    {
                        sw.WriteLine(file.Cells[i].Timeframes[j].Value + "\t");
                    }
                }
               
             
            }
        }


        public static void ExportTestData(InputFile file) {

            
            Debug.Print(file.Folder);

            string name = file.Folder + file.Name + " Results " + ".txt";

            File.Delete(name);
            StreamWriter sw = new StreamWriter(name);
            foreach(Cell cell in file.Cells)
            {
                sw.WriteLine(cell.Name);
                foreach (KeyValuePair<double, int> Dic in cell.High_Intensity_Counts)
                {
                    sw.WriteLine("Minute: " + Dic.Key + " Count: " + Dic.Value + " Baseline_Mean: " + cell.Baseline_Mean + " Treshold: " + cell.Threshold + " Maximum: "+ cell.TimeFrame_Maximum );

                }
                    
            }

            sw.Close();
        }


        public static void ExportTimeFrames(InputFile file)
        {
            string filename = file.Folder + "TimeFrames.txt";
         
            File.Delete(filename);
            StreamWriter sw = new StreamWriter(filename);
            foreach(Cell cell in file.Cells)
            {
                foreach(TimeFrame timeframe in cell.Normalized_Timeframes)
                {
                    sw.WriteLine("Cell: " + cell.Name + " ID: " + timeframe.ID + " Value: " + timeframe.Value + " Minute: " + timeframe.Including_Minute + " Above_Below_Treshold:  " + timeframe.Above_Below_Threshold  + " Threshold: " + cell.Threshold);
                   
                }
            }

            sw.Close();
           
        }

    }
}
