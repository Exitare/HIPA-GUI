using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA.Services
{
    class Matricescs
    {
        public static string[,] CreateStringMatrix(InputFile file)
        {
            string[,] data_matrix = new string[file.RowCount, file.CellCount];

            for (int j = 0; j < file.CellCount; j++)
            {
                data_matrix[0, j] = file.Cells[j].Name;
            }




            for (int i = 1; i < file.RowCount; i++)
            {

                for (int j = 0; j < file.CellCount; j++)
                {

                    data_matrix[i, j] = file.Cells[j].Normalized_Timeframes[i - 1].Value.ToString();
                }

            }

            return data_matrix;
        }
    }
}
