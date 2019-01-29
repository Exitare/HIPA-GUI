using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HIPA;
using HIPA.Services;

namespace HIPA.Services {

    public class Update {
        public static bool CheckForUpdates(Version actualVersion)
        {
            Console.WriteLine("Check for Updates");
            Download.DownloadConfig();
            Version newVersion = XML.LoadXML();
            return newVersion > actualVersion ? true : false;
        }
    

        public static void StartUpdates()
        {
            Process.Start("Updater.exe");
        }

    }
}
