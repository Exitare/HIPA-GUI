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
            if (!File.Exists(Globals.LogFileName))
                File.Create(Globals.LogFileName).Close();


            if (!File.Exists(Globals.ErrorLogFileName))
                File.Create(Globals.ErrorLogFileName).Close();

        }  

    }
}
