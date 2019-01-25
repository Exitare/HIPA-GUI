using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService {
    class Create {

        public static void CreateResultDir()
        {
            if (!File.Exists("Results"))
            {
                File.Create("Results");
            }
        }
      

    }
}
