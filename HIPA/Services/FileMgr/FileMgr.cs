using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPA.Statics;

namespace HIPA.Services.FileMgr {

    class FileMgr {

        public static void CreateFiles()
        {
            CreateLogFiles();
        }

        private static void CreateLogFiles()
        {
            if (!File.Exists(Globals.Log))
                File.Create(Globals.Log).Close();


            if (!File.Exists(Globals.ErrorLog))
                File.Create(Globals.ErrorLog).Close();

        }
      

    }
}
