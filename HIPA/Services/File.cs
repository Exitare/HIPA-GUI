using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HIPA {

    class FileService {

        public static string GetFileName(string Pathname) {

            string[] words = Pathname.Split('\\');
            return words[words.Length - 1];
        }


        public static void ReadFiles() {
            foreach (File file in Globals.Files) {
                string[] lines = System.IO.File.ReadAllLines(file.Path);
                file.CellCount = CellService.CalculateCellCount(lines);
                file.RowCount = CellService.CalculateRows(lines);
                file.Content = lines;
            }
        }

        public static IEnumerable<T[]> Zip<T>(params IEnumerable<T>[] iterables) {
            IEnumerator<T>[] enumerators = Array.ConvertAll(iterables, (iterable) => iterable.GetEnumerator());

            while (true) {
                int index = 0;
                T[] values = new T[enumerators.Length];

                foreach (IEnumerator<T> enumerator in enumerators) {
                    if (!enumerator.MoveNext())
                        yield break;

                    values[index++] = enumerator.Current;
                }

                yield return values;
            }
        }

    }
}
