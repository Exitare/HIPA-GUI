using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HIPA;
using HIPA.Statics;
using HIPA.Services.XML;


namespace HIPA.Services.Updater {

    public class UpdateHandler  {
        public static bool CheckForUpdate()
        {
#if DEBUG
            Debug.WriteLine("Check for Updates");
#endif

            DownloadHandler.DownloadFiles();

            if (Globals.ConnectionSuccessful)
                return XMLHandler.LoadConfigVersion() > Version.Parse(FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion) ? true : false;

            return false;

        }
    

        public static void StartUpdates()
        {
            Process.Start("Updater.exe");
        }

    }
}
