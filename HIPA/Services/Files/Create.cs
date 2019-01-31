using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA.Files {
    class Create {


        public static void CreateFiles()
        {
            CreateLogFiles();
        }

        private static void CreateLogFiles()
        {
            if (!File.Exists("Log.txt"))
            {
                File.Create("Log.txt");
            }
        }
      

    }
}
